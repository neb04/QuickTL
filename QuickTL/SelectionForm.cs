using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuickTL
{
    public partial class SelectionForm : Form
    {
        private Bitmap bmp;
        public Bitmap getBitmap()
        {
            return bmp;
        }
        Point startPos;
        Point currentPos;
        bool isDrawing = false;
        public event EventHandler operationCancelled;
        public event EventHandler bitmapReady;
        Rectangle selection;

        //[DllImport("user32.dll")]
        //private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        //protected override void WndProc(ref Message m)
        //{
        //    if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HOTKEY_ID && this.Visible)
        //    {
        //        this.Hide();
        //        OperationCancelled?.Invoke(this, EventArgs.Empty);
        //    }
        //    base.WndProc(ref m);
        //}

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Check if keypress is escape key
            if (keyData == Keys.Escape && this.Visible)
            {
                // Hide form since we don't need to see it anymore
                this.Hide();

                // Invoke operationCancelled event
                operationCancelled?.Invoke(this, EventArgs.Empty);

                // Return true since successfully handled
                return true;
            }

            // Call base process otherwise
            return base.ProcessCmdKey(ref msg, keyData);
        }



        public SelectionForm()
        {
            this.WindowState = FormWindowState.Maximized;
            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);
            //this.KeyPreview = true;
            this.BackColor = Color.Transparent;
            this.Opacity = 0.1;
            this.Cursor = Cursors.Cross;
            InitializeComponent();
            //TopMost = true;
            this.DoubleBuffered = true;
            //RegisterHotKey(this.Handle, HOTKEY_ID, 0x0000, (uint)Keys.Escape);
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle(
                Math.Min(startPos.X, currentPos.X),
                Math.Min(startPos.Y, currentPos.Y),
                Math.Abs(startPos.X - currentPos.X),
                Math.Abs(startPos.Y - currentPos.Y));
        }

        private void SelectionForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startPos = e.Location;
                isDrawing = true;
            }
        }

        private void SelectionForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                // Calculate width and height of the rectangle
                int width = Math.Abs(e.X - startPos.X);
                int height = Math.Abs(e.Y - startPos.Y);

                // Create selection rectangle
                selection = new Rectangle(
                    Math.Min(e.X, startPos.X),
                    Math.Min(e.Y, startPos.Y),
                    width,
                    height);

                // Invalidate to trigger paint event
                Invalidate();
            }
        }

        private void SelectionForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                isDrawing = false;
                // Reset selection rectangle after creating bmp
                this.Hide();
                if (selection.Width > 0 && selection.Height > 0)
                {
                    this.bmp = new Bitmap(selection.Width, selection.Height);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.CopyFromScreen(selection.Location, Point.Empty, selection.Size);
                    }
                    bitmapReady?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    operationCancelled?.Invoke(this, EventArgs.Empty);
                }
                //bmp.Save("selectedArea.png", System.Drawing.Imaging.ImageFormat.Png);

                selection = Rectangle.Empty;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            // Draw selection rectangle with outline
            if (isDrawing)
            {
                // "Cut out" transparent area for selection
                e.Graphics.SetClip(selection);
                e.Graphics.Clear(this.BackColor);

                e.Graphics.ResetClip();
                using (SolidBrush transparentBrush = new SolidBrush(Color.FromArgb(0, Color.Transparent)))
                {
                    e.Graphics.FillRectangle(transparentBrush, selection);
                }

                // Draw border
                using (Pen redPen = new Pen(Color.Red, 3))
                {
                    e.Graphics.DrawRectangle(redPen, selection);
                }
            }
        }

    }
}
