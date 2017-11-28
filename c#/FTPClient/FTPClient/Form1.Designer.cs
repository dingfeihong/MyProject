namespace FTPClient
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Waiting..");
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendCommandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cleanOldConnectionInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtServer = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.txtusername = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.txtPassword = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.txtPort = new System.Windows.Forms.ToolStripTextBox();
            this.btnConnect = new System.Windows.Forms.ToolStripButton();
            this.btnOldCon = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnDc = new System.Windows.Forms.ToolStripButton();
            this.rchLog = new System.Windows.Forms.RichTextBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.TreeView = new System.Windows.Forms.TreeView();
            this.ContextForTreeViewServer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lstClientFiles = new System.Windows.Forms.ListView();
            this.clmName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lstServerFiles = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ContextForListViewServer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextForLocalTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.uploadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileExplorer1 = new FileExplorerComponent.FileExplorer();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.ContextForTreeViewServer.SuspendLayout();
            this.ContextForListViewServer.SuspendLayout();
            this.ContextForLocalTreeView.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(983, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendCommandToolStripMenuItem,
            this.cleanOldConnectionInfoToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.editToolStripMenuItem.Text = "Interface";
            // 
            // sendCommandToolStripMenuItem
            // 
            this.sendCommandToolStripMenuItem.Name = "sendCommandToolStripMenuItem";
            this.sendCommandToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.sendCommandToolStripMenuItem.Text = "Send Command";
            this.sendCommandToolStripMenuItem.Click += new System.EventHandler(this.sendCommandToolStripMenuItem_Click);
            // 
            // cleanOldConnectionInfoToolStripMenuItem
            // 
            this.cleanOldConnectionInfoToolStripMenuItem.Name = "cleanOldConnectionInfoToolStripMenuItem";
            this.cleanOldConnectionInfoToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.cleanOldConnectionInfoToolStripMenuItem.Text = "Clean Old Connection Info";
            this.cleanOldConnectionInfoToolStripMenuItem.Click += new System.EventHandler(this.cleanOldConnectionInfoToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.txtServer,
            this.toolStripLabel2,
            this.txtusername,
            this.toolStripLabel3,
            this.txtPassword,
            this.toolStripLabel4,
            this.txtPort,
            this.btnConnect,
            this.btnOldCon,
            this.btnDc});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(983, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(48, 22);
            this.toolStripLabel1.Text = "Server : ";
            // 
            // txtServer
            // 
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(125, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(71, 22);
            this.toolStripLabel2.Text = "UserName : ";
            // 
            // txtusername
            // 
            this.txtusername.Name = "txtusername";
            this.txtusername.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(66, 22);
            this.toolStripLabel3.Text = "Password : ";
            // 
            // txtPassword
            // 
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(38, 22);
            this.toolStripLabel4.Text = "Port : ";
            // 
            // txtPort
            // 
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(100, 25);
            this.txtPort.Text = "21";
            // 
            // btnConnect
            // 
            this.btnConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnConnect.Image = global::FTPClient.Properties.Resources._1320669255_knetworkconf;
            this.btnConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(23, 22);
            this.btnConnect.Text = "Connect";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnOldCon
            // 
            this.btnOldCon.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOldCon.Image = ((System.Drawing.Image)(resources.GetObject("btnOldCon.Image")));
            this.btnOldCon.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOldCon.Name = "btnOldCon";
            this.btnOldCon.Size = new System.Drawing.Size(29, 22);
            this.btnOldCon.Text = "Old Connections";
            // 
            // btnDc
            // 
            this.btnDc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDc.Enabled = false;
            this.btnDc.Image = ((System.Drawing.Image)(resources.GetObject("btnDc.Image")));
            this.btnDc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDc.Name = "btnDc";
            this.btnDc.Size = new System.Drawing.Size(23, 22);
            this.btnDc.Text = "Disconnect";
            this.btnDc.Click += new System.EventHandler(this.btnDc_Click);
            // 
            // rchLog
            // 
            this.rchLog.Dock = System.Windows.Forms.DockStyle.Top;
            this.rchLog.Location = new System.Drawing.Point(0, 49);
            this.rchLog.Name = "rchLog";
            this.rchLog.Size = new System.Drawing.Size(983, 96);
            this.rchLog.TabIndex = 2;
            this.rchLog.Text = "";
            this.rchLog.TextChanged += new System.EventHandler(this.rchLog_TextChanged);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "");
            this.imageList1.Images.SetKeyName(4, "");
            this.imageList1.Images.SetKeyName(5, "");
            this.imageList1.Images.SetKeyName(6, "");
            this.imageList1.Images.SetKeyName(7, "");
            this.imageList1.Images.SetKeyName(8, "");
            this.imageList1.Images.SetKeyName(9, "");
            this.imageList1.Images.SetKeyName(10, "");
            this.imageList1.Images.SetKeyName(11, "");
            this.imageList1.Images.SetKeyName(12, "");
            this.imageList1.Images.SetKeyName(13, "");
            this.imageList1.Images.SetKeyName(14, "");
            this.imageList1.Images.SetKeyName(15, "");
            this.imageList1.Images.SetKeyName(16, "");
            this.imageList1.Images.SetKeyName(17, "");
            this.imageList1.Images.SetKeyName(18, "");
            this.imageList1.Images.SetKeyName(19, "");
            this.imageList1.Images.SetKeyName(20, "");
            this.imageList1.Images.SetKeyName(21, "");
            this.imageList1.Images.SetKeyName(22, "");
            this.imageList1.Images.SetKeyName(23, "");
            this.imageList1.Images.SetKeyName(24, "");
            this.imageList1.Images.SetKeyName(25, "");
            this.imageList1.Images.SetKeyName(26, "");
            this.imageList1.Images.SetKeyName(27, "");
            this.imageList1.Images.SetKeyName(28, "");
            this.imageList1.Images.SetKeyName(29, "unknown.png");
            // 
            // TreeView
            // 
            this.TreeView.ContextMenuStrip = this.ContextForTreeViewServer;
            this.TreeView.ImageIndex = 1;
            this.TreeView.ImageList = this.imageList1;
            this.TreeView.Location = new System.Drawing.Point(495, 145);
            this.TreeView.Name = "TreeView";
            treeNode2.ImageIndex = 1;
            treeNode2.Name = "Wait";
            treeNode2.Text = "Waiting..";
            this.TreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.TreeView.SelectedImageIndex = 2;
            this.TreeView.Size = new System.Drawing.Size(488, 167);
            this.TreeView.TabIndex = 6;
            this.TreeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.TreeView_AfterLabelEdit);
            this.TreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_AfterSelect);
            // 
            // ContextForTreeViewServer
            // 
            this.ContextForTreeViewServer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFolderToolStripMenuItem,
            this.removeFolderToolStripMenuItem});
            this.ContextForTreeViewServer.Name = "ContextForTreeViewServer";
            this.ContextForTreeViewServer.Size = new System.Drawing.Size(154, 48);
            // 
            // newFolderToolStripMenuItem
            // 
            this.newFolderToolStripMenuItem.Name = "newFolderToolStripMenuItem";
            this.newFolderToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.newFolderToolStripMenuItem.Text = "New Folder";
            this.newFolderToolStripMenuItem.Click += new System.EventHandler(this.newFolderToolStripMenuItem_Click);
            // 
            // removeFolderToolStripMenuItem
            // 
            this.removeFolderToolStripMenuItem.Name = "removeFolderToolStripMenuItem";
            this.removeFolderToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.removeFolderToolStripMenuItem.Text = "Remove Folder";
            this.removeFolderToolStripMenuItem.Click += new System.EventHandler(this.removeFolderToolStripMenuItem_Click);
            // 
            // lstClientFiles
            // 
            this.lstClientFiles.AllowDrop = true;
            this.lstClientFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmName,
            this.clmSize,
            this.clmType});
            this.lstClientFiles.Location = new System.Drawing.Point(0, 318);
            this.lstClientFiles.Name = "lstClientFiles";
            this.lstClientFiles.Size = new System.Drawing.Size(489, 170);
            this.lstClientFiles.TabIndex = 8;
            this.lstClientFiles.UseCompatibleStateImageBehavior = false;
            this.lstClientFiles.View = System.Windows.Forms.View.Details;
            this.lstClientFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstClientFiles_MouseDoubleClick);
            // 
            // clmName
            // 
            this.clmName.Text = "File Name";
            this.clmName.Width = 169;
            // 
            // clmSize
            // 
            this.clmSize.Text = "Size";
            this.clmSize.Width = 113;
            // 
            // clmType
            // 
            this.clmType.Text = "File Type";
            this.clmType.Width = 203;
            // 
            // lstServerFiles
            // 
            this.lstServerFiles.AllowDrop = true;
            this.lstServerFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lstServerFiles.ContextMenuStrip = this.ContextForListViewServer;
            this.lstServerFiles.LabelEdit = true;
            this.lstServerFiles.Location = new System.Drawing.Point(495, 318);
            this.lstServerFiles.Name = "lstServerFiles";
            this.lstServerFiles.Size = new System.Drawing.Size(489, 170);
            this.lstServerFiles.TabIndex = 9;
            this.lstServerFiles.UseCompatibleStateImageBehavior = false;
            this.lstServerFiles.View = System.Windows.Forms.View.Details;
            this.lstServerFiles.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lstServerFiles_AfterLabelEdit);
            this.lstServerFiles.BeforeLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lstServerFiles_BeforeLabelEdit);
            this.lstServerFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstServerFiles_DragDrop);
            this.lstServerFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstServerFiles_DragEnter);
            this.lstServerFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstServerFiles_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "File Name";
            this.columnHeader1.Width = 169;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Size";
            this.columnHeader2.Width = 113;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "File Type";
            this.columnHeader3.Width = 203;
            // 
            // ContextForListViewServer
            // 
            this.ContextForListViewServer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteFileToolStripMenuItem,
            this.renameFileToolStripMenuItem});
            this.ContextForListViewServer.Name = "contextMenuStrip1";
            this.ContextForListViewServer.Size = new System.Drawing.Size(139, 48);
            // 
            // deleteFileToolStripMenuItem
            // 
            this.deleteFileToolStripMenuItem.Name = "deleteFileToolStripMenuItem";
            this.deleteFileToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.deleteFileToolStripMenuItem.Text = "Delete File";
            this.deleteFileToolStripMenuItem.Click += new System.EventHandler(this.deleteFileToolStripMenuItem_Click);
            // 
            // renameFileToolStripMenuItem
            // 
            this.renameFileToolStripMenuItem.Name = "renameFileToolStripMenuItem";
            this.renameFileToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.renameFileToolStripMenuItem.Text = "Rename File";
            this.renameFileToolStripMenuItem.Click += new System.EventHandler(this.renameFileToolStripMenuItem_Click);
            // 
            // ContextForLocalTreeView
            // 
            this.ContextForLocalTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uploadToolStripMenuItem});
            this.ContextForLocalTreeView.Name = "ContextForLocalTreeView";
            this.ContextForLocalTreeView.Size = new System.Drawing.Size(116, 26);
            // 
            // uploadToolStripMenuItem
            // 
            this.uploadToolStripMenuItem.Name = "uploadToolStripMenuItem";
            this.uploadToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.uploadToolStripMenuItem.Text = "Upload!";
            this.uploadToolStripMenuItem.Click += new System.EventHandler(this.uploadToolStripMenuItem_Click);
            // 
            // fileExplorer1
            // 
            this.fileExplorer1.BackColor = System.Drawing.Color.White;
            this.fileExplorer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fileExplorer1.Location = new System.Drawing.Point(0, 145);
            this.fileExplorer1.Name = "fileExplorer1";
            this.fileExplorer1.Size = new System.Drawing.Size(489, 167);
            this.fileExplorer1.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(983, 494);
            this.Controls.Add(this.fileExplorer1);
            this.Controls.Add(this.lstServerFiles);
            this.Controls.Add(this.lstClientFiles);
            this.Controls.Add(this.TreeView);
            this.Controls.Add(this.rchLog);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "FTPClient";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ContextForTreeViewServer.ResumeLayout(false);
            this.ContextForListViewServer.ResumeLayout(false);
            this.ContextForLocalTreeView.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox txtServer;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox txtusername;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox @txtPassword;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripTextBox txtPort;
        private System.Windows.Forms.ToolStripButton btnConnect;
        private System.Windows.Forms.RichTextBox rchLog;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TreeView TreeView;
        private System.Windows.Forms.ListView lstClientFiles;
        private System.Windows.Forms.ColumnHeader clmName;
        private System.Windows.Forms.ColumnHeader clmSize;
        private System.Windows.Forms.ColumnHeader clmType;
        private System.Windows.Forms.ListView lstServerFiles;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ToolStripDropDownButton btnOldCon;
        private System.Windows.Forms.ContextMenuStrip ContextForListViewServer;
        private System.Windows.Forms.ToolStripMenuItem deleteFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameFileToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip ContextForTreeViewServer;
        private System.Windows.Forms.ToolStripMenuItem newFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeFolderToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip ContextForLocalTreeView;
        private System.Windows.Forms.ToolStripMenuItem uploadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendCommandToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cleanOldConnectionInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnDc;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private FileExplorerComponent.FileExplorer fileExplorer1;
    }
}

