using FTPServer.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTPServer
{
    public partial class frmServerMain : Form
    {
        public delegate void AppendMessage(string msg);


        public frmServerMain()
        {
            InitializeComponent();
        }


        private Thread thread_listen = null;
        private frmServerMain.AppendMessage AppendMessageDelegate;

        private void frmServerMain_Load(object sender, EventArgs e)
        {
            this.AppendMessageDelegate = new frmServerMain.AppendMessage(AppendMessage2TextBox);

            ClientListener lsner = new ClientListener();
            lsner.NewCommandClientConnected += lsner_NewCommandClientConnected;
            lsner.NewDataClientConnected += lsner_NewDataClientConnected;
            lsner.TextMessageArrival += lsner_TextMessageArrival;
            lsner.FileTransferNotify += lsner_FileTransferNotify;

            ThreadStart ths = new ThreadStart(lsner.StartListen);
            thread_listen= new Thread(ths);
            thread_listen.Start();

            GlobalRuntime.Threads.Add(thread_listen);
        }

        void lsner_FileTransferNotify(long transferedLength)
        {
            messages.Invoke(this.AppendMessageDelegate, string.Format("File transfered {0}.", transferedLength));
        }

        void lsner_TextMessageArrival(string remoteIp, string msg)
        {
            messages.Invoke(this.AppendMessageDelegate, string.Format("New text message arrival [from:{0}, msg: {1}].", remoteIp, msg));
        }

        void lsner_NewDataClientConnected(string remoteIp)
        {
            messages.Invoke(this.AppendMessageDelegate, string.Format("New data connection connected [{0}].", remoteIp));
        }

        void lsner_NewCommandClientConnected(string remoteIp)
        {
            messages.Invoke(this.AppendMessageDelegate, string.Format("New command connection connected [{0}].", remoteIp));
        }

        private void AppendMessage2TextBox(string msg)
        {
            this.messages.Text = msg+"\r\n" +this.messages.Text;
        }

        private void frmServerMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                GlobalRuntime.Threads.ForEach(t => t.Abort());
                GlobalRuntime.Threads.ForEach(t => t.Join(500));
            }
            catch
            { 
            }
        }
    }
}
