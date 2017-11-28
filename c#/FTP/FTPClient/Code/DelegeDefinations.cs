using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient.Code
{
    public delegate void NewTextMessageArrival(string msg);
    public delegate void FileListMessageArrival(List<string> files);
    public delegate void FileTransferRequestConfirmed(long fileSize);
    public delegate void FileTransferNotify(long transferedLength);
}
