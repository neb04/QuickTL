using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuickTL
{
    // Main form for QuickTL. Handles UI, hotkeys, and main translation functionality
    public partial class MainForm : Form
    {
        private const string deepLApiKey = "APIKEYHERE";
        private const string googleApiKey = "APIKEYHERE";
        private const int wmHotkey = 0x0312;
        private const int imageHotkeyId = 1;
        private const int clipboardHotkeyId = 3;

        private Point mouseDownPosition = Point.Empty;
        private Keys keyBind;
        private bool isSettingKey = false;
        private string previousText;

        private static readonly HttpClient deepLClient = new HttpClient();
        private static readonly HttpClient googleClient = new HttpClient();

        private readonly SelectionForm selectionForm;
        private readonly TranslationDisplay translationDisplay;
        private readonly Dictionary<string, string> languageNameToCode = new Dictionary<string, string>();

        private bool isMoving = false;
        private Point newPoint = Point.Empty;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public MainForm()
        {
            InitializeComponent();
            LoadLanguagesAsync();
            KeyPreview = true;

            // Register event handlers
            MouseDown += HandleMouseDown;
            MouseMove += HandleMouseMove;
            MouseUp += HandleMouseUp;

            // Register default hotkeys
            RegisterHotKey(Handle, imageHotkeyId, 0x0000, (uint)Keys.Insert);
            RegisterHotKey(Handle, clipboardHotkeyId, 0x0000, (uint)Keys.Home);

            googleClient.Timeout = TimeSpan.FromSeconds(5);

            selectionForm = new SelectionForm();
            translationDisplay = new TranslationDisplay();
        }

        // Override keypress event to handle keybinds
        protected override void WndProc(ref Message m)
        {
            if (!isSettingKey && m.Msg == wmHotkey)
            {
                switch (m.WParam.ToInt32())
                {
                    case imageHotkeyId:
                        _ = CaptureAsync();
                        break;
                    case clipboardHotkeyId:
                        _ = HandleClipboardHotkeyAsync();
                        break;
                }
            }
            base.WndProc(ref m);
        }

        // Unregister hotkeys before form closes
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            UnregisterHotKey(Handle, imageHotkeyId);
            UnregisterHotKey(Handle, clipboardHotkeyId);
            base.OnFormClosing(e);
        }

        // Display translation once complete
        private void ShowTranslation(string translatedText)
        {
            translationDisplay.Show();
            translationDisplay.TopMost = true;
            translationDisplay.TopMost = false;
        }


        // Capture screen, extract text, trasnate, then display
        private async Task CaptureAsync()
        {
            try
            {
                selectionForm.Show();
                selectionForm.TopMost = true;
                selectionForm.TopMost = false;
                var bitmap = await GetBitmapAsync();

                if (bitmap != null)
                {
                    var extractedText = await ExtractTextFromBitmapAsync(bitmap, googleApiKey);
                    var translatedText = await TranslateTextAsync(extractedText);
                    translationDisplay.SetSource(extractedText);
                    translationDisplay.SetTranslated(translatedText);
                    ShowTranslation(translatedText);
                }
            }
            catch (OperationCanceledException)
            {
                // Operation was canceled, handle if necessary
            }
        }

        // Handle clipboard-only translation keybind press
        private async Task HandleClipboardHotkeyAsync()
        {
            var clipboardText = Clipboard.GetText();
            if (!string.IsNullOrEmpty(clipboardText))
            {
                translationDisplay.SetSource(clipboardText);
                var translatedText = await TranslateTextAsync(clipboardText);
                translationDisplay.SetTranslated(translatedText);
                ShowTranslation(translatedText);
            }
        }

        // Get screenshot portion as bitmap from selectionForm
        private Task<Bitmap> GetBitmapAsync()
        {
            var tcs = new TaskCompletionSource<Bitmap>();
            var cts = new CancellationTokenSource();

            void OnBitmapReady(object sender, EventArgs e)
            {
                if (!cts.Token.IsCancellationRequested)
                {
                    tcs.SetResult(selectionForm.getBitmap());
                }

                selectionForm.bitmapReady -= OnBitmapReady;
                selectionForm.operationCancelled -= OnOperationCancelled;
            }

            void OnOperationCancelled(object sender, EventArgs e)
            {
                if (!tcs.Task.IsCompleted)
                {
                    cts.Cancel();
                    tcs.SetResult(null);
                }

                selectionForm.bitmapReady -= OnBitmapReady;
                selectionForm.operationCancelled -= OnOperationCancelled;
            }

            selectionForm.bitmapReady += OnBitmapReady;
            selectionForm.operationCancelled += OnOperationCancelled;

            return tcs.Task;
        }

        // Convert bitmap to base64 string
        private static string ConvertBitmapToBase64String(Bitmap bitmap)
        {
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        // Use Google Vision API to get text from bitmap
        private static async Task<string> ExtractTextFromBitmapAsync(Bitmap bitmap, string apiKey)
        {
            string base64Image = ConvertBitmapToBase64String(bitmap);
            string requestUrl = $"https://vision.googleapis.com/v1/images:annotate?key={apiKey}";

            var requestBody = new
            {
                requests = new[]
                {
                    new
                    {
                        image = new { content = base64Image },
                        features = new[] { new { type = "TEXT_DETECTION" } }
                    }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            try
            {
                var response = await googleClient.PostAsync(requestUrl, content);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();

                var jsonDoc = JsonDocument.Parse(responseBody);
                var textAnnotations = jsonDoc.RootElement
                    .GetProperty("responses")[0]
                    .GetProperty("textAnnotations");

                foreach (var annotation in textAnnotations.EnumerateArray())
                {
                    return annotation.GetProperty("description").GetString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting text: {ex.Message}");
            }

            return string.Empty;
        }

        // Translate text using DeepL API
        private async Task<string> TranslateTextAsync(string text)
        {
            var targetLang = languageNameToCode[outLang.Text];
            var sourceLang = languageNameToCode[inLang.Text];

            var requestBody = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("auth_key", deepLApiKey),
                new KeyValuePair<string, string>("text", text),
                new KeyValuePair<string, string>("target_lang", targetLang),
                new KeyValuePair<string, string>("source_lang", sourceLang)
            });

            try
            {
                var response = await deepLClient.PostAsync("https://api-free.deepl.com/v2/translate", requestBody);
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JObject.Parse(jsonResponse)["translations"]?[0]?["text"]?.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error translating text: {ex.Message}");
                return string.Empty;
            }
        }

        // Load available languages from DeepL API and populate comboboxes
        private async void LoadLanguagesAsync()
        {
            try
            {
                deepLClient.DefaultRequestHeaders.Add("Authorization", $"DeepL-Auth-Key {deepLApiKey}");

                var sourceResponse = await deepLClient.GetAsync("https://api-free.deepl.com/v2/languages?type=source");
                sourceResponse.EnsureSuccessStatusCode();
                var sourceBody = await sourceResponse.Content.ReadAsStringAsync();
                var sourceLanguages = JArray.Parse(sourceBody);

                var targetResponse = await deepLClient.GetAsync("https://api-free.deepl.com/v2/languages?type=target");
                targetResponse.EnsureSuccessStatusCode();
                var targetBody = await targetResponse.Content.ReadAsStringAsync();
                var targetLanguages = JArray.Parse(targetBody);

                Invoke(new Action(() =>
                {
                    inLang.Items.Clear();
                    outLang.Items.Clear();
                    inLang.Items.Add("Auto-detect");
                    foreach (var lang in sourceLanguages)
                    {
                        var name = lang["name"].ToString();
                        var code = lang["language"].ToString();
                        inLang.Items.Add(name);
                        languageNameToCode[name] = code;
                    }
                    languageNameToCode["Auto-detect"] = null;

                    foreach (var lang in targetLanguages)
                    {
                        var name = lang["name"].ToString();
                        var code = lang["language"].ToString();
                        outLang.Items.Add(name);
                        languageNameToCode[name] = code;
                    }

                    inLang.Text = "Auto-detect";
                    outLang.Text = "English (American)";
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load languages: {ex.Message}");
            }
        }

        // Handle mouseDown event for moving the form with mouse
        private void HandleMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMoving = true;
                mouseDownPosition = e.Location;
            }
        }

        // Handle mouseMove event for moving form once mouseDown is pressed
        private void HandleMouseMove(object sender, MouseEventArgs e)
        {
            if (isMoving && e.Button == MouseButtons.Left)
            {
                newPoint = new Point(Location.X + e.X - mouseDownPosition.X, Location.Y + e.Y - mouseDownPosition.Y);
                Location = newPoint;
            }
        }

        // Handles mouseup to stop moving form
        private void HandleMouseUp(object sender, MouseEventArgs e)
        {
            isMoving = false;
        }


        // Handles clicking the keybind change button
        private void HandleKeybindButtonClick(object sender, EventArgs e)
        {
            var button = (Button)sender;

            if (button.Name == "clipboardButton" && button.Text == "Keybind: ...")
            {
                button.Text = previousText;
                isSettingKey = false;
            }
            else if (button.Name != "clipboardButton" && button.Text == "Keybind: ...")
            {
                button.Text = previousText;
                isSettingKey = false;
            }

            if (!isSettingKey)
            {
                previousText = button.Text;
                button.Text = "Keybind: ...";
                isSettingKey = true;
            }
        }

        // Handles keyup for setting new keybinds
        private void HandleKeybindButtonKeyUp(object sender, KeyEventArgs e)
        {
            if (!isSettingKey) return;

            if (e.KeyCode == Keys.Escape)
            {
                ((Button)sender).Text = previousText;
                isSettingKey = false;
                return;
            }

            uint modifiers = 0x0000;
            var modifiersList = new List<string>();

            if (e.Control)
            {
                modifiersList.Add("Ctrl+");
                modifiers |= 0x0002;
            }
            if (e.Shift)
            {
                modifiersList.Add("Shift+");
                modifiers |= 0x0004;
            }
            if (e.Alt)
            {
                modifiersList.Add("Alt+");
                modifiers |= 0x0001;
            }

            modifiers |= 0x4000; // MOD_NOREPEAT
            keyBind = e.KeyCode;

            string keyText = ParseKey(e.KeyCode);
            string modifierText = string.Join("", modifiersList);
            ((Button)sender).Text = $"Keybind: {modifierText}{keyText}";

            isSettingKey = false;

            if (((Button)sender).Name == "keybindButton")
            {
                UnregisterHotKey(Handle, imageHotkeyId);
                RegisterHotKey(Handle, imageHotkeyId, modifiers, (uint)keyBind);
            }
            else if (((Button)sender).Name == "clipboardButton")
            {
                UnregisterHotKey(Handle, clipboardHotkeyId);
                RegisterHotKey(Handle, clipboardHotkeyId, modifiers, (uint)keyBind);
            }
        }

        // Originally planned to handle OEMkeys but this works too for now
        private string ParseKey(Keys key)
        {
            return key.ToString();
        }
    }
}
