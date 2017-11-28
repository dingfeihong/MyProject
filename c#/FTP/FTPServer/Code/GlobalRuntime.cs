using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FTPServer.Code
{
    public static class GlobalRuntime
    {
        public static string BaseDirectory = @"D:\TDDownload";
        public static List<Thread> Threads = new List<Thread>();
    }
}
