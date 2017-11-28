using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FTPServer.Code
{
    public class ClientListener
    {
        public event NewCommandClientConnected NewCommandClientConnected;
        public event NewDataClientConnected NewDataClientConnected;
        public event NewTextMessageArrival TextMessageArrival;
        public event FileTransferNotify FileTransferNotify;

        private const int port_command = 9000;
        private const int port_data = 9001;

        TcpListener lsner_command;
        TcpListener lsner_data;

        public void StartListen()
        {
            lsner_command = new TcpListener(port_command);
            lsner_command.Start();

            lsner_data = new TcpListener(port_data);
            lsner_data.Start();

            lsner_command.BeginAcceptTcpClient(new AsyncCallback(ProcessCommandClientArrival), lsner_command);
            lsner_data.BeginAcceptTcpClient(new AsyncCallback(ProcessDataClientArrival), lsner_data);

        }

        public void ProcessCommandClientArrival(IAsyncResult iar)
        {
            TcpListener listener = (TcpListener)iar.AsyncState;
            TcpClient client = listener.EndAcceptTcpClient(iar);

            Monitor.Enter(this);
            FTPClientBroker broker = FTPClientBrokerFactory.GetOrCreateClientBrokerByCommandClient(client);
            if (IsLaunchable(broker))
            {
                broker.NewTextMessageArrival += broker_NewTextMessageArrival;
                broker.FileTransferNotify += broker_FileTransferNotify;
                LaunchClientBroker(broker);
            }
            Monitor.Exit(this);


            if (this.NewCommandClientConnected != null)
                this.NewCommandClientConnected.Invoke(client.Client.RemoteEndPoint.ToString());
        }

        void broker_FileTransferNotify(long transferedLength)
        {
            if (this.FileTransferNotify != null)
                this.FileTransferNotify.Invoke(transferedLength);
        }

        public void ProcessDataClientArrival(IAsyncResult iar)
        {
            TcpListener listener = (TcpListener)iar.AsyncState;
            TcpClient client = listener.EndAcceptTcpClient(iar);

            Monitor.Enter(this);
            FTPClientBroker broker = FTPClientBrokerFactory.GetOrCreateClientBrokerByDataClient(client);
            if (IsLaunchable(broker))
            {
                broker.NewTextMessageArrival += broker_NewTextMessageArrival;
                broker.FileTransferNotify += broker_FileTransferNotify;
                LaunchClientBroker(broker);
            }
            Monitor.Exit(this);

            if (this.NewDataClientConnected != null)
                this.NewDataClientConnected.Invoke(client.Client.RemoteEndPoint.ToString());
        }

        private bool IsLaunchable(FTPClientBroker broker)
        {
            return broker.CommandTcpClient != null && broker.DataTcpClient != null;
        }

        //private void BindBrokerEvent(FTPClientBroker broker, ClientListener lsner)
        //{
        //    broker.NewTextMessageArrival += broker_NewTextMessageArrival;
        //}

        private void broker_NewTextMessageArrival(string remoteIp, string msg)
        {
            if (this.TextMessageArrival != null)
                this.TextMessageArrival.Invoke(remoteIp, msg);
        }
        private void LaunchClientBroker(FTPClientBroker broker)
        {
            ThreadStart ths = new ThreadStart(broker.Start);
            Thread thd = new Thread(ths);
            thd.Start();

            GlobalRuntime.Threads.Add(thd);
        }
    }
}