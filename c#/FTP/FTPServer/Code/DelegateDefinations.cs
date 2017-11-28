using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPServer.Code
{
    public delegate void NewCommandClientConnected(string remoteIp);
    public delegate void NewDataClientConnected(string remoteIp);

    public delegate void NewTextMessageArrival(string remoteIp, string msg);
    public delegate void FileTransferNotify(long transferedLength);
}
