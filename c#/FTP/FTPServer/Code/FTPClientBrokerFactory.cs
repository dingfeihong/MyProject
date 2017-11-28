using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FTPServer.Code
{
    public static class FTPClientBrokerFactory
    {
        private static Dictionary<string, FTPClientBroker> Brokers = new Dictionary<string, FTPClientBroker>();

        public static FTPClientBroker GetOrCreateClientBrokerByCommandClient(TcpClient commandClient)
        {
            string remoteIp = Convert2IP(commandClient.Client.RemoteEndPoint.ToString());
            if (!Brokers.ContainsKey(remoteIp))
            {
                FTPClientBroker broker = new FTPClientBroker();
                broker.CommandTcpClient = commandClient;
                Brokers[remoteIp] = broker;
            }

            Brokers[remoteIp].CommandTcpClient = commandClient;
            return Brokers[remoteIp];
        }
        public static FTPClientBroker GetOrCreateClientBrokerByDataClient(TcpClient dataClient)
        {
            string remoteIp = Convert2IP(dataClient.Client.RemoteEndPoint.ToString());
            if (!Brokers.ContainsKey(remoteIp))
            {
                FTPClientBroker broker = new FTPClientBroker();
                broker.DataTcpClient = dataClient;
                Brokers[remoteIp] = broker;
            }
            Brokers[remoteIp].DataTcpClient = dataClient;
            return Brokers[remoteIp];
        }

        private static string Convert2IP(string remoteEndpoint)
        {
            string[] str = remoteEndpoint.Split(':');
            return str[0].Trim();
        }
    }
}
