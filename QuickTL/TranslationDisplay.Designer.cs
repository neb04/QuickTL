﻿namespace QuickTL
{
    partial class TranslationDisplay
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.leftLabel = new System.Windows.Forms.Label();
            this.sourceCopyButton = new System.Windows.Forms.Button();
            this.rightLabel = new System.Windows.Forms.Label();
            this.translatedCopyButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.translatedTextbox = new System.Windows.Forms.RichTextBox();
            this.sourceTextbox = new System.Windows.Forms.RichTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.leftLabel);
            this.panel1.Controls.Add(this.sourceCopyButton);
            this.panel1.Controls.Add(this.rightLabel);
            this.panel1.Controls.Add(this.translatedCopyButton);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.translatedTextbox);
            this.panel1.Controls.Add(this.sourceTextbox);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(605, 319);
            this.panel1.TabIndex = 0;
            // 
            // leftLabel
            // 
            this.leftLabel.AutoSize = true;
            this.leftLabel.ForeColor = System.Drawing.Color.White;
            this.leftLabel.Location = new System.Drawing.Point(99, 277);
            this.leftLabel.Name = "leftLabel";
            this.leftLabel.Size = new System.Drawing.Size(99, 13);
            this.leftLabel.TabIndex = 7;
            this.leftLabel.Text = "Saved to clipboard!";
            // 
            // sourceCopyButton
            // 
            this.sourceCopyButton.Location = new System.Drawing.Point(98, 293);
            this.sourceCopyButton.Name = "sourceCopyButton";
            this.sourceCopyButton.Size = new System.Drawing.Size(101, 23);
            this.sourceCopyButton.TabIndex = 6;
            this.sourceCopyButton.Text = "Copy to Clipboard";
            this.sourceCopyButton.UseVisualStyleBackColor = true;
            this.sourceCopyButton.Click += new System.EventHandler(this.SourceCopyButton_Click);
            // 
            // rightLabel
            // 
            this.rightLabel.AutoSize = true;
            this.rightLabel.ForeColor = System.Drawing.Color.White;
            this.rightLabel.Location = new System.Drawing.Point(410, 277);
            this.rightLabel.Name = "rightLabel";
            this.rightLabel.Size = new System.Drawing.Size(99, 13);
            this.rightLabel.TabIndex = 5;
            this.rightLabel.Text = "Saved to clipboard!";
            // 
            // translatedCopyButton
            // 
            this.translatedCopyButton.Location = new System.Drawing.Point(409, 293);
            this.translatedCopyButton.Name = "translatedCopyButton";
            this.translatedCopyButton.Size = new System.Drawing.Size(101, 23);
            this.translatedCopyButton.TabIndex = 4;
            this.translatedCopyButton.Text = "Copy to Clipboard";
            this.translatedCopyButton.UseVisualStyleBackColor = true;
            this.translatedCopyButton.Click += new System.EventHandler(this.TranslationCopyButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(417, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Translated Text";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(107, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Source Text";
            // 
            // translatedTextbox
            // 
            this.translatedTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.translatedTextbox.ForeColor = System.Drawing.Color.White;
            this.translatedTextbox.Location = new System.Drawing.Point(310, 42);
            this.translatedTextbox.Name = "translatedTextbox";
            this.translatedTextbox.ReadOnly = true;
            this.translatedTextbox.Size = new System.Drawing.Size(295, 226);
            this.translatedTextbox.TabIndex = 1;
            this.translatedTextbox.Text = "";
            // 
            // sourceTextbox
            // 
            this.sourceTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.sourceTextbox.ForeColor = System.Drawing.Color.White;
            this.sourceTextbox.Location = new System.Drawing.Point(0, 42);
            this.sourceTextbox.Name = "sourceTextbox";
            this.sourceTextbox.ReadOnly = true;
            this.sourceTextbox.Size = new System.Drawing.Size(295, 226);
            this.sourceTextbox.TabIndex = 0;
            this.sourceTextbox.Text = "";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.Timer2_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(275, 283);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // TranslationScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(629, 343);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Name = "TranslationScreen";
            this.Text = "Translation";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox translatedTextbox;
        private System.Windows.Forms.RichTextBox sourceTextbox;
        private System.Windows.Forms.Label rightLabel;
        private System.Windows.Forms.Button translatedCopyButton;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label leftLabel;
        private System.Windows.Forms.Button sourceCopyButton;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Button button1;
    }
}