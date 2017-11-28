using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTPClient.Code
{
    public class FTPBroker
    {
        private StreamReader sr;
        private StreamWriter sw;
        public event NewTextMessageArrival NewTextMessageArrival;
        public event FileListMessageArrival FileListMessageArrival;
        public event FileTransferRequestConfirmed FileTransferRequestConfirmed;
        public event FileTransferNotify FileTransferNotify;

        private EventWaitHandle receiveSignal_Command = new EventWaitHandle(false, EventResetMode.ManualReset);
        private EventWaitHandle receiveSignal_Data = new EventWaitHandle(false, EventResetMode.ManualReset);

        private string TransferingServerFilePath;

        public void SayHello2Server()
        {
            SendTextMessage("Hello");
        }

        public void DoListCommand()
        {
            SendTextMessage("LIST");
        }

        public void Start()
        {
            Init();

            StartReceive_Command();
            StartReceive_Data();

            SayHello2Server();

            DoListCommand();
        }

        private void StartReceive_Command()
        {
            ThreadStart ths = new ThreadStart(DoReceive_Command);
            Thread thd = new Thread(ths);
            thd.Start();

            GlobalRuntime.Threads.Add(thd);
        }
        private void StartReceive_Data()
        {
            ThreadStart ths = new ThreadStart(DoReceive_Data);
            Thread thd = new Thread(ths);
            thd.Start();

            GlobalRuntime.Threads.Add(thd);
        }

        private void DoReceive_Command()
        {
            byte[] data = new byte[GlobalRuntime.CommandTcpClient.Client.ReceiveBufferSize];

            while (true)
            {
                receiveSignal_Command.Reset();

                GlobalRuntime.CommandTcpClient.Client.BeginReceive(data, 0, data.Length, SocketFlags.None, asyncResult =>
                {
                    int length = 0;
                    try
                    {
                        length = GlobalRuntime.CommandTcpClient.Client.EndReceive(asyncResult);
                    }
                    catch
                    {
                        return;
                    }

                    string command = System.Text.Encoding.Default.GetString(data, 0, length).TrimEnd("\r\n".ToCharArray());

                    if (NewTextMessageArrival != null)
                        NewTextMessageArrival.Invoke(command);

                    receiveSignal_Command.Set();


                    string[] parsedCommand = command.Split(',');
                    string cmd = parsedCommand[0].ToLower().Trim();
                    switch (cmd)
                    {
                        case "ok":
                            //nothing to do
                            break;
                        case "filelist":
                            string[] files = parsedCommand[1].Trim().Split('|');
                            if (this.FileListMessageArrival != null)
                                this.FileListMessageArrival.Invoke(files.ToList());
                            break;
                        case "transfer-confirmed":
                            long fileSize = long.Parse(parsedCommand[1]);
                            if (this.FileTransferRequestConfirmed != null)
                                this.FileTransferRequestConfirmed.Invoke(fileSize);
                            confirmedLength = fileSize;
                            SendTextMessage("Transfer-launch");
                            break;
                        default:
                            break;
                    }
                }, null);
                receiveSignal_Command.WaitOne();
            }
        }

        private long transferedLength = 0;
        private long confirmedLength = 0;
        private void DoReceive_Data()
        {
            FileStream fs=null;

            byte[] data = new byte[GlobalRuntime.DataTcpClient.Client.ReceiveBufferSize];

            bool isExit = false;

            while (true && !isExit)
            {
                receiveSignal_Data.Reset();

                GlobalRuntime.DataTcpClient.Client.BeginReceive(data, 0, data.Length, SocketFlags.None, asyncResult =>
                {
                    int length = 0;
                    try
                    {
                        length = GlobalRuntime.DataTcpClient.Client.EndReceive(asyncResult);
                    }
                    catch
                    {
                        isExit = true;
                        receiveSignal_Data.Set();
                        return;
                    }
                    if(fs==null)
                        fs = new FileStream(System.IO.Path.Combine(GlobalRuntime.RootDirectory, System.IO.Path.GetFileName(TransferingServerFilePath)), FileMode.Append);

                    fs.Write(data, 0, length);
                    fs.Flush();

                    transferedLength += length;

                    if (FileTransferNotify != null)
                        FileTransferNotify.Invoke(transferedLength);

                    if (transferedLength >= confirmedLength)
                        isExit = true;

                    receiveSignal_Data.Set();
                }, null);
                receiveSignal_Data.WaitOne();
            }

            if (fs != null)
            {
                fs.Close();
                fs.Dispose();
            }
            MessageBox.Show("Transfer done.");
        }

        private void SendTextMessage(string cmd)
        {
            sw.WriteLine(cmd);
            sw.Flush();
        }
        private void SendTextMessage(string cmd, string parameter)
        {
            string msg = string.Format("{0}, {1}", cmd, parameter);
            SendTextMessage(msg);
        }

        private void Init()
        {
            sr = new StreamReader(GlobalRuntime.CommandTcpClient.GetStream());
            sw = new StreamWriter(GlobalRuntime.CommandTcpClient.GetStream());
        }

        public void RequestTransferFile(string serverFilePath)
        {
            SendTextMessage("TRANSFER-REQUEST", serverFilePath);
            TransferingServerFilePath = serverFilePath;
        }
    }
}
