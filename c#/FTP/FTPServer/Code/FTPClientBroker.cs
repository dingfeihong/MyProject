using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FTPServer.Code
{
    public class FTPClientBroker
    {
        public TcpClient CommandTcpClient { get; set; }
        public TcpClient DataTcpClient { get; set; }

        private StreamWriter sw_command;
        private StreamReader sr_command;

        private EventWaitHandle receiveSignal_Command = new EventWaitHandle(false, EventResetMode.ManualReset);
        private EventWaitHandle receiveSignal_Data = new EventWaitHandle(false, EventResetMode.ManualReset);

        public event NewTextMessageArrival NewTextMessageArrival;
        public event FileTransferNotify FileTransferNotify;

        private string transfer_filePath;

        private void StartTransferFile()
        {
            ThreadStart ths=new ThreadStart(DoTransferFile);
            Thread thd = new Thread(ths);
            thd.Start();

            GlobalRuntime.Threads.Add(thd);
            
        }
        private void DoTransferFile()
        {
            byte[] data = new byte[DataTcpClient.Client.ReceiveBufferSize];
            long sentLength = 0;

            using (FileStream fs = new FileStream(transfer_filePath, FileMode.Open))
            {
                int length = 0;
                while((length = fs.Read(data, 0, DataTcpClient.Client.ReceiveBufferSize))>0)
                {
                    receiveSignal_Data.Reset();
                    DataTcpClient.Client.BeginSend(data, 0, length, SocketFlags.None, asyncResult =>
                    {
                        int sentLengthCur = DataTcpClient.Client.EndSend(asyncResult);
                        sentLength += sentLengthCur;

                        if (this.FileTransferNotify != null)
                            this.FileTransferNotify.Invoke(sentLength);

                        receiveSignal_Data.Set();

                    }, null);
                    receiveSignal_Data.WaitOne();
                }
                SendTextMessage("Transfer-Done", fs.Length.ToString());
            }
        }

        public void Start()
        {
            Init();

            ReadyReceive();
        }

        private void ReadyReceive()
        {
            while (true)
            {
                receiveSignal_Command.Reset();

                byte[] data = new byte[CommandTcpClient.Client.ReceiveBufferSize];
                CommandTcpClient.Client.BeginReceive(data, 0, data.Length, SocketFlags.None, asyncResult =>
                                                                                            {
                                                                                                int length = 0;
                                                                                                try
                                                                                                {
                                                                                                    length = CommandTcpClient.Client.EndReceive(asyncResult);
                                                                                                    if (length <= 0)    //invalid command
                                                                                                        return;
                                                                                                }
                                                                                                catch
                                                                                                {
                                                                                                    return;
                                                                                                }

                                                                                                string[] commands=ParseAllCommands(System.Text.Encoding.Default.GetString(data, 0, length).TrimEnd("\r\n".ToCharArray()));
                                                                                                foreach (string command in commands)
                                                                                                {
                                                                                                    if (command.Trim().Length == 0)
                                                                                                        continue;

                                                                                                    if (NewTextMessageArrival != null)
                                                                                                        NewTextMessageArrival.Invoke(CommandTcpClient.Client.RemoteEndPoint.ToString(), command);

                                                                                                    receiveSignal_Command.Set();


                                                                                                    string[] parsedCommand = command.Split(',');
                                                                                                    string cmd = parsedCommand[0].ToLower().Trim();
                                                                                                    switch (cmd)
                                                                                                    {
                                                                                                        case "hello":
                                                                                                            SendTextMessage("OK");
                                                                                                            break;
                                                                                                        case "list":
                                                                                                            string filelist = "";
                                                                                                            string[] files = System.IO.Directory.GetFiles(GlobalRuntime.BaseDirectory);
                                                                                                            foreach (string file in files)
                                                                                                            {
                                                                                                                filelist = file + "|" + filelist;
                                                                                                            }
                                                                                                            filelist = filelist.TrimEnd(':');
                                                                                                            SendTextMessage("FILELIST", filelist);
                                                                                                            break;
                                                                                                        case "transfer-request":
                                                                                                            if (!File.Exists(parsedCommand[1].Trim()))
                                                                                                            {
                                                                                                                SendTextMessage("ERROR", "File not found");
                                                                                                                break;
                                                                                                            }
                                                                                                            transfer_filePath = parsedCommand[1].Trim();
                                                                                                            FileInfo fi = new FileInfo(parsedCommand[1].Trim());
                                                                                                            SendTextMessage("TRANSFER-CONFIRMED", fi.Length.ToString());
                                                                                                            break;
                                                                                                        case "transfer-launch":
                                                                                                            StartTransferFile();
                                                                                                            break;
                                                                                                        default:
                                                                                                            SendTextMessage("ERROR", "Unknow command");
                                                                                                            break;
                                                                                                    }
                                                                                                }
                                                                                            }, null);
                receiveSignal_Command.WaitOne();
            }
        }

        private void SendTextMessage(string cmd)
        {
            sw_command.WriteLine(cmd);
            sw_command.Flush();
        }
        private void SendTextMessage(string cmd, string parameter)
        {
            string msg = string.Format("{0}, {1}", cmd, parameter);
            SendTextMessage(msg);
        }

        private void Init()
        {
            sw_command = new StreamWriter(CommandTcpClient.GetStream());
            sr_command = new StreamReader(CommandTcpClient.GetStream());
        }

        private string[] ParseAllCommands(string rawData)
        {
            if (rawData == null || rawData.Trim().Length == 0)
                return new string[] { };

            return rawData.Split("\r\n".ToCharArray());
        }
    }
}
