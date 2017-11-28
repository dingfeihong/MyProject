namespace FTPServer
{
    partial class frmServerMain
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
            this.messages = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // messages
            // 
            this.messages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messages.Location = new System.Drawing.Point(0, 0);
            this.messages.Multiline = true;
            this.messages.Name = "messages";
            this.messages.Size = new System.Drawing.Size(426, 827);
            this.messages.TabIndex = 0;
            // 
            // frmServerMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 827);
            this.Controls.Add(this.messages);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmServerMain";
            this.Text = "frmServerMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmServerMain_FormClosing);
            this.Load += new System.EventHandler(this.frmServerMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox messages;
    }
}