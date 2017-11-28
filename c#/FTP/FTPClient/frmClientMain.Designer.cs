namespace FTPClient
{
    partial class frmClientMain
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
            this.ftpServerIP = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.cmdPort = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.msg = new System.Windows.Forms.TextBox();
            this.btnBye = new System.Windows.Forms.Button();
            this.serverPanel = new System.Windows.Forms.GroupBox();
            this.dataPort = new System.Windows.Forms.NumericUpDown();
            this.download = new System.Windows.Forms.Button();
            this.fileName = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.cmdPort)).BeginInit();
            this.serverPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataPort)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ftpServerIP
            // 
            this.ftpServerIP.Location = new System.Drawing.Point(85, 19);
            this.ftpServerIP.Name = "ftpServerIP";
            this.ftpServerIP.Size = new System.Drawing.Size(101, 21);
            this.ftpServerIP.TabIndex = 0;
            this.ftpServerIP.Text = "127.0.0.1";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(250, 20);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // cmdPort
            // 
            this.cmdPort.Location = new System.Drawing.Point(192, 20);
            this.cmdPort.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.cmdPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cmdPort.Name = "cmdPort";
            this.cmdPort.Size = new System.Drawing.Size(52, 21);
            this.cmdPort.TabIndex = 2;
            this.cmdPort.Value = new decimal(new int[] {
            9000,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "FTP Server:";
            // 
            // progress
            // 
            this.progress.Location = new System.Drawing.Point(10, 58);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(489, 23);
            this.progress.TabIndex = 4;
            // 
            // msg
            // 
            this.msg.Location = new System.Drawing.Point(14, 105);
            this.msg.Multiline = true;
            this.msg.Name = "msg";
            this.msg.ReadOnly = true;
            this.msg.Size = new System.Drawing.Size(512, 148);
            this.msg.TabIndex = 5;
            // 
            // btnBye
            // 
            this.btnBye.Enabled = false;
            this.btnBye.Location = new System.Drawing.Point(357, 34);
            this.btnBye.Name = "btnBye";
            this.btnBye.Size = new System.Drawing.Size(75, 23);
            this.btnBye.TabIndex = 6;
            this.btnBye.Text = "Bye";
            this.btnBye.UseVisualStyleBackColor = true;
            this.btnBye.Click += new System.EventHandler(this.btnBye_Click);
            // 
            // serverPanel
            // 
            this.serverPanel.Controls.Add(this.dataPort);
            this.serverPanel.Controls.Add(this.ftpServerIP);
            this.serverPanel.Controls.Add(this.btnConnect);
            this.serverPanel.Controls.Add(this.cmdPort);
            this.serverPanel.Controls.Add(this.label1);
            this.serverPanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.serverPanel.Location = new System.Drawing.Point(14, 12);
            this.serverPanel.Name = "serverPanel";
            this.serverPanel.Size = new System.Drawing.Size(337, 87);
            this.serverPanel.TabIndex = 7;
            this.serverPanel.TabStop = false;
            // 
            // dataPort
            // 
            this.dataPort.Location = new System.Drawing.Point(192, 47);
            this.dataPort.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.dataPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.dataPort.Name = "dataPort";
            this.dataPort.Size = new System.Drawing.Size(52, 21);
            this.dataPort.TabIndex = 4;
            this.dataPort.Value = new decimal(new int[] {
            9001,
            0,
            0,
            0});
            // 
            // download
            // 
            this.download.Location = new System.Drawing.Point(424, 29);
            this.download.Name = "download";
            this.download.Size = new System.Drawing.Size(75, 23);
            this.download.TabIndex = 8;
            this.download.Text = "Download";
            this.download.UseVisualStyleBackColor = true;
            this.download.Click += new System.EventHandler(this.download_Click);
            // 
            // fileName
            // 
            this.fileName.Location = new System.Drawing.Point(10, 29);
            this.fileName.Name = "fileName";
            this.fileName.Size = new System.Drawing.Size(408, 21);
            this.fileName.TabIndex = 9;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.progress);
            this.groupBox2.Controls.Add(this.fileName);
            this.groupBox2.Controls.Add(this.download);
            this.groupBox2.Location = new System.Drawing.Point(14, 259);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(512, 96);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            // 
            // frmClientMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 364);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnBye);
            this.Controls.Add(this.serverPanel);
            this.Controls.Add(this.msg);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmClientMain";
            this.Text = "frmClientMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmClientMain_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.cmdPort)).EndInit();
            this.serverPanel.ResumeLayout(false);
            this.serverPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataPort)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ftpServerIP;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.NumericUpDown cmdPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.TextBox msg;
        private System.Windows.Forms.Button btnBye;
        private System.Windows.Forms.GroupBox serverPanel;
        private System.Windows.Forms.Button download;
        private System.Windows.Forms.TextBox fileName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown dataPort;
    }
}