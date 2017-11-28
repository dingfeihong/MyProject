using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FTPClient.Code
{
    public static class GlobalRuntime
    {
        public static TcpClient CommandTcpClient;
        public static TcpClient DataTcpClient;
        public static List<Thread> Threads = new List<Thread>();
        public static FTPBroker Broker;
        public static string RootDirectory = @"D:\TDDownload\new";
    }
}
