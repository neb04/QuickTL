using System;
using System.Windows.Forms;

namespace QuickTL
{
    // Form to display results of translation. Only shows up after successful translation
    public partial class TranslationDisplay : Form
    {
        public TranslationDisplay()
        {
            InitializeComponent();
            this.FormClosing += SubForm_FormClosing;
            rightLabel.Hide();
            leftLabel.Hide();
        }
        private void SubForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Check if the close was initiated by the user
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Cancel the close operation
                e.Cancel = true;

                // Hide the form instead of closing it
                this.Hide();
            }
            else
            {
                base.OnFormClosing(e);
            }
        }
        public void SetSource(string s)
        {
            sourceTextbox.Text = s;
        }
        public void SetTranslated(string s)
        {
            translatedTextbox.Text = s;
        }
        private void TranslationCopyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(translatedTextbox.Text);
            rightLabel.Show();
            timer1.Start();
        }

        private void SourceCopyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(sourceTextbox.Text);
            leftLabel.Show();
            timer2.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            rightLabel.Hide();
            timer1.Stop();
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            leftLabel.Hide();
            timer2.Stop();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
