using FTPClient.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTPClient
{
    public delegate void AppendMessage2TextBox(string msg);
    public delegate void InitProgressBar(long max);
    public delegate void SetProgressBarValue(long curValue);

    public partial class frmClientMain : Form
    {
        private AppendMessage2TextBox AppendMessage2TextBox;
        private InitProgressBar InitProgressBar;
        private SetProgressBarValue SetProgressBarValue;

        public frmClientMain()
        {
            InitializeComponent();
            AppendMessage2TextBox = new AppendMessage2TextBox(Append2TextBox);
            InitProgressBar = new InitProgressBar(ConfigProgressSetting);
            SetProgressBarValue = new SetProgressBarValue(SetProgressBarSetting);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            serverPanel.Enabled = false;
            btnBye.Enabled = true;

            GlobalRuntime.CommandTcpClient = new TcpClient();
            GlobalRuntime.DataTcpClient = new TcpClient();

            try
            {
                GlobalRuntime.CommandTcpClient.Connect(ftpServerIP.Text, int.Parse(cmdPort.Value.ToString()));
                GlobalRuntime.DataTcpClient.Connect(ftpServerIP.Text, int.Parse(dataPort.Value.ToString()));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connect. reason: "+ex.Message);
                ResetServerConnectionPanel();
                return;
            }



            GlobalRuntime.Broker = new FTPBroker();

            GlobalRuntime.Broker.NewTextMessageArrival += Broker_NewTextMessageArrival;
            GlobalRuntime.Broker.FileListMessageArrival += Broker_FileListMessageArrival;
            GlobalRuntime.Broker.FileTransferRequestConfirmed += Broker_FileTransferRequestConfirmed;
            GlobalRuntime.Broker.FileTransferNotify += Broker_FileTransferNotify;

            ThreadStart ths = new ThreadStart(GlobalRuntime.Broker.Start);
            Thread thd = new Thread(ths);
            thd.Start();

            GlobalRuntime.Threads.Add(thd);
        }

        void Broker_FileTransferNotify(long transferedLength)
        {
            progress.Invoke(SetProgressBarValue, transferedLength);
        }

        private void SetProgressBarSetting(long transferedLength)
        {
            progress.Value = (int)transferedLength / (1024 * 1024);
        }

        void Broker_FileTransferRequestConfirmed(long fileSize)
        {
            progress.Invoke(InitProgressBar, fileSize);
        }
        private void ConfigProgressSetting(long max)
        { 
            progress.Minimum = 0;
            progress.Maximum = (int)max/(1024*1024); //M
            progress.Value = 0;
        }

        void Broker_FileListMessageArrival(List<string> files)
        {
            string msg = "";
            files.ForEach(t=>msg+=t+"\r\n");
            this.msg.Invoke(this.AppendMessage2TextBox, msg);
        }
        
        void Broker_NewTextMessageArrival(string msg)
        {
            this.msg.Invoke(this.AppendMessage2TextBox, msg);
        }
        private void Append2TextBox(string msg)
        {
            this.msg.Text = msg + "\r\n" + this.msg.Text;
        }

        private void btnBye_Click(object sender, EventArgs e)
        {
            CleanConnections();

            ResetServerConnectionPanel();
        }

        private static void CleanConnections()
        {
            GlobalRuntime.Threads.ForEach(t => t.Abort());
            GlobalRuntime.Threads.ForEach(t => t.Join(500));

            if (GlobalRuntime.CommandTcpClient != null)
            {
                GlobalRuntime.CommandTcpClient.Close();
                GlobalRuntime.CommandTcpClient = null;
            }
            if (GlobalRuntime.DataTcpClient != null)
            {
                GlobalRuntime.DataTcpClient.Close();
                GlobalRuntime.DataTcpClient = null;
            }
        }

        private void ResetServerConnectionPanel()
        {
            serverPanel.Enabled = true;
            btnBye.Enabled = false;
        }

        private void download_Click(object sender, EventArgs e)
        {
            GlobalRuntime.Broker.RequestTransferFile(fileName.Text.Trim());
        }

        private void frmClientMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                CleanConnections();
            }
            catch
            { 
            }
        }
    }
}
