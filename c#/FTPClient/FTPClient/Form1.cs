using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.IO;
using Microsoft.Win32;
using System.Collections;
using Microsoft.VisualBasic;

namespace FTPClient
{
    public partial class Form1 : Form
    {
        #region Constructor
        public Form1()
        {
            InitializeComponent();
            fileExplorer1.Controls.Find("TreeView", true)[0].MouseDoubleClick += new MouseEventHandler(FExp_Tree_MouseClick);
            fileExplorer1.Controls.Find("TreeView", true)[0].MouseClick += new MouseEventHandler(FExp_Tree_MouseClick);
            fileExplorer1.Controls.Find("TreeView", true)[0].ContextMenuStrip = ContextForLocalTreeView;
            txtPassword.TextBox.PasswordChar = '*';
        }
        #endregion
        #region Destructor
        ~Form1()
        {
        LogOut();
        }
        #endregion
        #region Variables
        private Socket FTPSocket=null,DataSock=null;
        private FileInfo fi,fi2;
        private ImageList lstServerImages = new ImageList();
        private RegistryKey yni;
        private string RnmOldName="",StatusMessage = "",Result="",Server="",UserName="",Password="",Path="/",ServerInfoRegedit="";
        private Byte[] Buffer=new Byte[512];
        private int StatusCode,Bytes,Port;
        private string[] Msg;
        private bool Logged = false,Changed = false,Binary = false,Deleted=false;
        public bool IsBinary
        {
            get { return Binary; }
            set
            {
                if (Binary == value) return;
                if (value)
                    SendCommand("TYPE I");
                else
                    SendCommand("TYPE A");
                if (StatusCode != 200)
                {
                    AppendText(rchLog,"Status : "+Result.Substring(4)+"\n",Color.Red);
                    return;
                }
            }
        }
        #endregion
        #region FTP!
        private void UploadDirectory(string LocalPath,string FTPPath,string RootDirName,TreeNode ParentNode)
        {
            if (!Logged)
            {
                AppendText(rchLog, "Status : Login First Please\n", Color.Red);
                return;
            }
            NewFoldertoTreeView(RootDirName, ParentNode);
            ChangeWorkingDirectory(FTPPath + "/" + RootDirName);
            if (!Changed)
                return;
            foreach (string item in Directory.GetFiles(LocalPath))
            {
                UploadFile(item);
            }
            if (Directory.GetDirectories(LocalPath).Length==0)
                return;
            string NewFtpPath=FTPPath+"/"+RootDirName;
            TreeNode NewParent = new TreeNode();
            NewParent = ParentNode.Nodes[RootDirName+"_"];
            foreach (string directory in Directory.GetDirectories(LocalPath))
            {
                string temp = directory.Substring(directory.LastIndexOf('\\') + 1);
                UploadDirectory(directory, NewFtpPath, temp,NewParent);
            }
            AppendText(rchLog, "Status : All Directory And Included Files Uploaded Sucessfully\n", Color.Red);
        }
        private void NewFoldertoTreeView(string Text,TreeNode ParentNode)
        {
            if (TreeView.SelectedNode != null && Text=="")
            {
                TreeNode temp = new TreeNode();
                temp.Name = "New Folder" + "_";
                temp.Text = "New Folder";
                if (TreeView.SelectedNode.Text != @"/")
                    temp.Tag = TreeView.SelectedNode.Tag.ToString().Trim(' ') + @"/" + "New Folder";
                else
                    temp.Tag = @"/" + "New Folder";
                temp.ImageIndex = 1;
                TreeView.SelectedNode.Nodes.Add(temp);
                TreeView.SelectedNode.Expand();
                TreeView.LabelEdit = true;
                temp.BeginEdit();
            }
            else if (Text != "" && ParentNode!=null)
            {
                TreeNode temp = new TreeNode();
                temp.Name = Text + "_";
                temp.Text = Text;
                if (Text != @"/")
                    temp.Tag = ParentNode.Tag.ToString().Trim(' ') + @"/" + Text;
                else
                    temp.Tag = @"/" + Text;
                temp.ImageIndex = 1;
                ParentNode.Nodes.Add(temp);
                ParentNode.Expand();
                CreateDirectory(temp.Tag.ToString(), temp.Index);
            }
        }
        private void RemoveDirectory(string DirPath)
        {
            if (!Logged)
            {
                AppendText(rchLog, "Status : Login First Please\n", Color.Red);
                return;
            }
            if (DirPath == null || DirPath.Equals(".") || DirPath.Length == 0)
            {
                AppendText(rchLog, "Status : A directory name wasn't provided. Please provide one and try your request again.\n", Color.Red);
                return;
            }
            SendCommand("RMD " + DirPath);
            if (StatusCode != 250)
            {
                AppendText(rchLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
                return;
            }
            AppendText(rchLog,"Statu : Removed directory " + DirPath+"\n", Color.Red);
            TreeView.SelectedNode.Remove();
        }
        private void CreateDirectory(string DirPath,int DirIndex)
        {
            if (!Logged)
            {
                AppendText(rchLog, "Status : Login First Please\n", Color.Red);
                TreeView.SelectedNode.Nodes[DirIndex].Remove();
                return;
            }
            if (DirPath == null || DirPath.Equals(".") || DirPath.Length == 0)
            {
                AppendText(rchLog,"Status : A directory name wasn't provided. Please provide one and try your request again.\n",Color.Red);
                TreeView.SelectedNode.Nodes[DirIndex].Remove();
                return;
            }
            SendCommand("MKD " + DirPath);
            if (StatusCode != 250 && StatusCode != 257)
            {
                AppendText(rchLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
                TreeView.SelectedNode.Nodes[DirIndex].Remove();
                return;
            }
            AppendText(rchLog,"Status : Created directory to" + DirPath+"\n",Color.Red);
        }
        private void RenameFile(string oldName, string newName)
        {
            if (!Logged)
            {
                AppendText(rchLog, "Status : Login First Please\n", Color.Red);
                return;
            }
            SendCommand("RNFR " + oldName);
            /*if (StatusCode==550)
            {
                AppendText(rchLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
                lstServerFiles.SelectedItems[0].Text = RnmOldName.Substring(RnmOldName.LastIndexOf('/') + 1);
                return;
            }*/
            if (StatusCode != 350)
            {
                AppendText(rchLog,"Status : "+Result.Substring(4)+"\n",Color.Red);
                return;
            }
            SendCommand("RNTO " + newName);
            if (StatusCode != 250)
            {
                AppendText(rchLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
                return;
            }
            AppendText(rchLog,"Status Renamed file " + oldName + " to " + newName+"\n",Color.Red);
        }
        private void DeleteFile(string file)
        {
            Deleted = false;
            if (!Logged)
            {
                AppendText(rchLog, "Status : Login First Please\n", Color.Red);
                return;
            }
            SendCommand("DELE " + file);
            if (StatusCode != 250)
            {
                AppendText(rchLog, "Status : "+Result.Substring(4)+"\n", Color.Red);
                return;
            }
            AppendText(rchLog,"Status Deleted file " + file+"\n",Color.Red);
            Deleted = true;
        }
        private void UploadFile(string LocalPath)
        {
            if (!Logged)
            {
                AppendText(rchLog, "Status : Login First Please\n", Color.Red);
                return;
            }
            Socket dataSocket = null;
            dataSocket = OpenSocketForTransfer();
            SendCommand("STOR " + System.IO.Path.GetFileName(LocalPath));
            if (StatusCode != 125 && StatusCode != 150)
            {
                AppendText(rchLog, "Status : "+Result.Substring(4)+"\n", Color.Red);
                return;
            }
            AppendText(rchLog, "Status : Uploading File from " + LocalPath + "\n", Color.Red);
            FileStream input = new FileStream(LocalPath,FileMode.Open);
            while ((Bytes = input.Read(Buffer, 0, Buffer.Length)) > 0)
            {
                dataSocket.Send(Buffer, Bytes, 0);
            }
            input.Close();
            if (dataSocket.Connected)
            {
                dataSocket.Close();
            }
            ReadResponse();
            if (StatusCode != 226 && StatusCode != 250)
            {
                AppendText(rchLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
                return;
            }
        }
        private void DownloadFile(string FtpPath,string LocalPath)
        {
            if (!Logged)
            {
                AppendText(rchLog, "Status : Login First Please\n", Color.Red);
                return ;
            }
            IsBinary = true;
            AppendText(rchLog,"Status : Downloading File From "+FtpPath+" To "+LocalPath+"\n",Color.Red);
            if (LocalPath.Equals(""))
                LocalPath = fileExplorer1.SelectedDirectoryPath();
            FileStream output = null;
            if (!File.Exists(LocalPath))
                output = File.Create(LocalPath);
            else
            {
                output = new FileStream(LocalPath, FileMode.Open);
                AppendText(rchLog, "Status : OverWriting...To "+LocalPath+"\n", Color.Red);
            }
            Socket dataSocket = OpenSocketForTransfer();
            SendCommand("RETR " + FtpPath);
            if (StatusCode != 150 && StatusCode != 125)
            {
                AppendText(rchLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
                return;
            }
            DateTime timeout = DateTime.Now.AddSeconds(30);
            while (timeout > DateTime.Now)
            {
                Bytes = dataSocket.Receive(Buffer, Buffer.Length, 0);
                output.Write(Buffer, 0, Bytes);
                if (Bytes <= 0)
                    break;
            }
            output.Close();
            if (dataSocket.Connected)
                dataSocket.Close();
            ReadResponse();
            if (StatusCode != 226 && StatusCode != 250)
            {
                AppendText(rchLog,"Status : "+Result.Substring(4)+"\n",Color.Red);
                return;
            }
            AppendText(rchLog, "Status : Download Completed Sucessfully\n", Color.Red);
        }
        private long FileSize(string FileName)
        {
            if (!Logged)
            {
                AppendText(rchLog,"Status : Login First Please\n",Color.Red);
                return 0;
            }
            SendCommand("SIZE "+FileName);
            long Filesize;
            if (StatusCode == 213)
                Filesize = long.Parse(Result.Substring(4));
            else
            {
                AppendText(rchLog,"Status : "+Result.Substring(4)+"\n",Color.Red);
                return 0;
            }
            return Filesize;
        }
        private void SendCommand(string msg)
        {
            AppendText(rchLog, "Command : " + msg + "\n", Color.Blue);
            Byte[] CommandBytes = Encoding.ASCII.GetBytes((msg + "\r\n").ToCharArray());
            FTPSocket.Send(CommandBytes, CommandBytes.Length, 0);
            //read Response
            ReadResponse();
        }

        private void ReadResponse()
        {
            StatusMessage = "";
            Result = SplitResponse();
            StatusCode = int.Parse(Result.Substring(0, 3));
        }

        private string SplitResponse()
        {
            try
            {
                while (true)
                {
                    Bytes = FTPSocket.Receive(Buffer, Buffer.Length, 0); //Number Of Bytes (Count)
                    StatusMessage += Encoding.ASCII.GetString(Buffer, 0, Bytes); //Convert to String
                    if (Bytes < Buffer.Length)  //End Of Response
                        break;
                }
                string[] msg = StatusMessage.Split('\n');
                if (StatusMessage.Length > 2)
                    StatusMessage = msg[msg.Length - 2];  //Remove Last \n
                else
                    StatusMessage = msg[0];
                if (!StatusMessage.Substring(3, 1).Equals(" "))
                    return SplitResponse();
                for (int i = 0; i < msg.Length - 1; i++)
                    AppendText(rchLog, "Response : " + msg[i] + "\n", Color.Green);
                return StatusMessage;
            }
            catch(Exception ex)
            {
                AppendText(rchLog, "Status : ERROR. " +ex.Message+ "\n", Color.Red);
                FTPSocket.Close();
                return "";
            }
        }

        private void FtpLogin()
        {
            if (Logged)
                CloseConnection();
            IPAddress remoteAddress = null;
            IPEndPoint addrEndPoint = null;
            AppendText(rchLog,"Status : Opening Connection to : " + Server + "\n",Color.Red);
            try
            {
                FTPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                AppendText(rchLog,"Status : Resolving IP Address\n",Color.Red);
                remoteAddress = Dns.GetHostEntry(Server).AddressList[0];
                AppendText(rchLog, "Status : IP Address Found ->" + remoteAddress.ToString() + "\n", Color.Red);
                addrEndPoint = new IPEndPoint(remoteAddress, Port);
                AppendText(rchLog,"Status : EndPoint Found ->" + addrEndPoint.ToString() + "\n", Color.Red);
                FTPSocket.Connect(addrEndPoint);
            }
            catch (Exception ex)
            {
                if (FTPSocket != null && FTPSocket.Connected)
                {
                    FTPSocket.Close();
                }
                AppendText(rchLog, "Status : Couldn't connect to remote server. " + ex.Message + "\n", Color.Red);
                return;
            }
            ReadResponse();
            if (StatusCode != 220) //220->Server Ready for New User
            {
                CloseConnection();
                AppendText(rchLog, "Status : " + Result.Substring(4) + "\n", Color.Red); //Error
                return;
            }
            SendCommand("USER " + UserName);
            if (!(StatusCode == 331 || StatusCode == 230) || StatusCode == 530) //230->Logged in , 331->UserName Okey,Need Password , 530->Login Fail
            {
                //Something Wrong!
                LogOut();
                AppendText(rchLog,"Status : " + Result.Substring(4) + "\n", Color.Red);
                return;
            }
            if (StatusCode != 230) //If Not Logged in!
            {
                SendCommand("PASS " + Password);
                if (!(StatusCode == 230 || StatusCode == 202)) //202 ->Command Not implemented(Password Not Required)
                {
                    //Something Wrong
                    LogOut();
                    AppendText(rchLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
                    return;
                }
            }
            Logged = true;
            AppendText(rchLog, "Status : Connected to " + Server + "\n", Color.Red);
            ChangeWorkingDirectory(Path);
        }

        private void ChangeWorkingDirectory(string Path)
        {
            if (Path == null || Path.Length == 0)
            {
                AppendText(rchLog, "Status : Directory Was Not Found\n", Color.Red);
                Changed = false;
                return;
            }
            if (!Logged)
            {
                AppendText(rchLog, "Status : Login First Please\n", Color.Red);
                Changed = false;
                return;
            }
            SendCommand("CWD "+Path);
            if (StatusCode != 250)  //250->Requested file action okay
            {
                AppendText(rchLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
                Changed = false;
                return;
            }
            SendCommand("PWD");// PWD -> Print Working Directory
            if (StatusCode != 257) //257->"PATHNAME" created.
            {
                AppendText(rchLog, "Status :" + Result.Substring(4) + "\n", Color.Red);
                Changed = false;
                return;
            }
            Path=StatusMessage.Split('"')[1]; //Get Response Path
            AppendText(rchLog, "Status : Current Working Directory is " + Path + "\n", Color.Red);
            Changed = true;
        }

        private void CloseConnection()
        {
            AppendText(rchLog, "Status : Closing Connection to " + Server + "\n", Color.Red);
            if (FTPSocket != null)
            {
                SendCommand("QUIT");
            }
            LogOut();
        }

        private void LogOut()
        {
            if (FTPSocket != null)
            {
                FTPSocket.Close();
                FTPSocket = null;
            }
            Logged = false;
        }

        public string[] GetListFiles()
        {
            if (!Logged)
                AppendText(rchLog, "Status : You Need To Log In First\n", Color.Red);
                DataSock = OpenSocketForTransfer();
                if (DataSock==null)
                {
                    AppendText(rchLog, "Status : Socket Error\n", Color.Red);
                    return Msg;
                }
            SendCommand("MLSD");
            if (!(StatusCode == 150 || StatusCode == 125)) //150->File status okay , 125->Data connection already open; transfer starting
            {
                AppendText(rchLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
                return Msg;
            }
            StatusMessage = "";
            DateTime timeout = DateTime.Now.AddSeconds(30);
            while (timeout > DateTime.Now)
            {
                int Bytes = DataSock.Receive(Buffer, Buffer.Length, 0);
                StatusMessage += Encoding.ASCII.GetString(Buffer, 0, Bytes);
                if (Bytes < Buffer.Length) break;
            }
            Msg = StatusMessage.Replace("\r", "").Split('\n');
            DataSock.Close();
            if (StatusMessage.Contains("No files found"))
                Msg = new string[] { };
            ReadResponse();
            if (StatusCode != 226) //226->Closing data connection. Requested file action successful
                Msg = new string[] { };
            return Msg;
        }
        private Socket OpenSocketForTransfer()
        {
            SendCommand("PASV");
            if (StatusCode != 227) //227->Succed
                AppendText(rchLog, "Status : " + Result.Substring(4) + "\n", Color.Red);
            //response from server is the IP and port number for the client in "(" & ")"
            Socket tranferSocket = null;
            IPEndPoint ipEndPoint = null;
            int indx1 = Result.IndexOf('(');
            int indx2 = Result.IndexOf(')');
            string IpPort = Result.Substring((indx1 + 1), (indx2 - indx1) - 1);
            int[] Parts = new int[6];
            int PartCount = 0;
            string Buffer = "";
            for (int i = 0; i < IpPort.Length && PartCount <= 6; i++)
            {
                char chr = char.Parse(IpPort.Substring(i, 1)); //Convert To Char
                if (char.IsDigit(chr)) //Are Chars Numeric? 
                    Buffer += chr;
                else if (chr != ',') //Pasv Result should come only numeric and ',' Chars
                {
                    AppendText(rchLog, "Status : Wrong PASV result->" + Result, Color.Red);
                    return null;
                }
                else
                {
                    if (chr == ',' || i + 1 == IpPort.Length)
                    {
                        try
                        {
                            Parts[PartCount++] = int.Parse(Buffer);
                            Buffer = "";
                        }
                        catch (Exception)
                        {
                            AppendText(rchLog, "Status : Wrong PASV result (not supported?): " + Result + "\n", Color.Red);
                            return null;
                        }
                    }
                }
            }
            Parts[PartCount] = int.Parse(Buffer);
            string ipAddress = Parts[0] + "." + Parts[1] + "." + Parts[2] + "." + Parts[3];
            int port = (Parts[4] << 8) + Parts[5];  //Parts[4] <<8 = Parts[4]*256
            try
            {
                tranferSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //ipEndPoint = new IPEndPoint(Dns.GetHostEntry(ipAddress).AddressList[0], port);
                ipEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
                tranferSocket.Connect(ipEndPoint);
            }
            catch (Exception ex)
            {
                if (tranferSocket != null && tranferSocket.Connected) tranferSocket.Close();
                AppendText(rchLog, "Status : Can't connect to remote server ->" + ex.Message + " \n", Color.Red);
                return null;
            }
            return tranferSocket;
        }

        #endregion
        #region OtherFunctions
        #region Regedit_OldCon
        private bool RemoveRegistry()
        {
            RegistryKey root = Registry.LocalMachine;
            RegistryKey soft = root.OpenSubKey("SOFTWARE",true);
            RegistryKey FtpClient = soft.OpenSubKey("FTPClient", true);
            if (FtpClient == null)
            {
                MessageBox.Show("Error! Can't Remove Old Connections");
                return false;
            }
            else
            {
                soft.DeleteSubKeyTree("FTPClient");
                return true;
            }
        }
        private void WriteRegistry(string Number,string FTPServer,string Username,string Password,string Port)
        {
            RegistryKey root = Registry.LocalMachine;
            RegistryKey soft = root.OpenSubKey("SOFTWARE",true);
            RegistryKey FtpClient = soft.OpenSubKey("FTPClient", true);
            if (FtpClient == null)
            {
                yni = soft.CreateSubKey("FTPClient");
                FtpClient = yni;
            }
            RegistryKey Con = FtpClient.CreateSubKey(Number);
            Con.SetValue("Server", FTPServer);
            Con.SetValue("Username",Username);
            Con.SetValue("Password",Password);
            Con.SetValue("Port",Port);
        }
        private int OldConCount()
        {
            RegistryKey root = Registry.LocalMachine;
            RegistryKey soft = root.OpenSubKey("SOFTWARE", true);
            RegistryKey FtpClient = soft.OpenSubKey("FTPClient", true);
            if (FtpClient == null)
                return 0;
            return FtpClient.SubKeyCount;
        }
        private bool ControlForMore(string FTPServer)
        {
            RegistryKey root = Registry.LocalMachine;
            RegistryKey soft = root.OpenSubKey("SOFTWARE", true);
            RegistryKey FtpClient = soft.OpenSubKey("FTPClient", true);
            if (FtpClient == null)
                return true;
            for (int i = 0; i < FtpClient.SubKeyCount; i++)
            {
                RegistryKey Info=FtpClient.OpenSubKey(i.ToString(),true);
                if (Server==Info.GetValue("Server").ToString())
                    return false;
            }
            return true;
        }
        private string ReadServerNamesRegistry(string Number)
        {
            RegistryKey root = Registry.LocalMachine;
            RegistryKey soft = root.OpenSubKey("SOFTWARE", true);
            RegistryKey FtpClient = soft.OpenSubKey("FTPClient", true);
            if (FtpClient == null)
                return "";
            RegistryKey Info = FtpClient.OpenSubKey(Number, true);
            return Info.GetValue("Server").ToString();
        }
        private string ReadServerInfo(string FTPServer)
        {
            ServerInfoRegedit = "";
            RegistryKey root = Registry.LocalMachine;
            RegistryKey soft = root.OpenSubKey("SOFTWARE", true);
            RegistryKey FtpClient = soft.OpenSubKey("FTPClient", true);
            if (FtpClient == null)
                return ServerInfoRegedit;
            for (int i = 0; i < FtpClient.SubKeyCount; i++)
            {
                RegistryKey Info = FtpClient.OpenSubKey(i.ToString(), true);
                if (Info.GetValue("Server").ToString()==FTPServer)
                {
                    ServerInfoRegedit += Info.GetValue("Server").ToString() + ",";
                    ServerInfoRegedit += Info.GetValue("Username").ToString() + ",";
                    ServerInfoRegedit += Info.GetValue("Password").ToString() + ",";
                    ServerInfoRegedit += Convert.ToInt32(Info.GetValue("Port"));
                }
            }
            return ServerInfoRegedit;
        }
        #endregion
        private void AppendText(RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;
            if (text.Contains("PASS"))
            {
                int len=text.Length-16;
                text = "Command : PASS ";
                for (int i = 0; i < len; i++)
                    text += "*";
                text += "\n";
            }
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
        private void ParseDirNames(TreeNode ParentNode)
        {
            GetListFiles();
            lstServerFiles.Items.Clear();
            int Temp,Temp2,Temp3;
            string Temp_Type = "", Temp_Name = "" ;
            if (Msg.Length != 0)
            {
                for (int i = 0; i < Msg.Length - 1; i++)
                {
                    Temp_Type = "";
                    Temp = Msg[i].ToString().IndexOf("type"); 
                    Temp2 = Msg[i].ToString().IndexOf('=', Temp);
                    Temp3 = Msg[i].ToString().IndexOf(';', Temp2);
                    Temp_Type = Msg[i].Substring(Temp2 + 1, Temp3 - Temp2 - 1);
                    if (Temp_Type == "dir")
                    {
                        Temp = Msg[i].ToString().LastIndexOf(';');
                        Temp_Name = Msg[i].Substring(Temp + 1, Msg[i].Length-Temp-1);
                        AddTreeNode(ParentNode, "dir", Temp_Name);
                    }
                    else if (Temp_Type=="file")
                    {
                        Temp = Msg[i].ToString().LastIndexOf(';');
                        Temp_Name = Msg[i].Substring(Temp + 1, Msg[i].Length - Temp - 1);
                        AddListServerFilesItem(TreeView.SelectedNode.Tag+"/"+Temp_Name.Trim(' '),Temp_Name.Trim(' '));
                    }
                }
                TreeView.Nodes[0].Expand();
            }
            else return;
        }
        private void AddTreeNode(TreeNode ParentNode,string NodeType,string Text)
        {
            TreeNode temp = new TreeNode();
            temp.Name = Text + "_";
            temp.Text = Text.Trim(' ');
            if (ParentNode.Text != @"/")
                temp.Tag = ParentNode.Tag.ToString().Trim(' ') + @"/" + Text.Trim(' ');
            else
                temp.Tag = @"/"+Text.Trim(' ');
            if (NodeType=="dir")
                temp.ImageIndex = 1;
            ParentNode.Nodes.Add(temp);
        }
        private void AddListServerFilesItem(string Path,string Name)
        {            
            Icon tempico = FileIconLoader.GetFileIcon(Name, false);
            lstServerImages.Images.Add(tempico); 
            lstServerFiles.SmallImageList = lstServerImages;
            lstServerFiles.Items.Add(Name);
            lstServerFiles.Items[lstServerFiles.Items.Count-1].SubItems.Add(FileSize(Path).ToString()+" Bytes");
            lstServerFiles.Items[lstServerFiles.Items.Count - 1].ImageIndex = lstServerImages.Images.Count - 1;
            try
            {
                fi = new FileInfo(Name); 
            }
            catch
            {
                lstServerFiles.Items[lstServerFiles.Items.Count - 1].SubItems.Add("");   
                return;
            }
            lstServerFiles.Items[lstServerFiles.Items.Count - 1].SubItems.Add(GetFileType(fi.Extension));   
        }
        private string GetFileType(string ext)
        {
            RegistryKey rKey = null;
            RegistryKey sKey = null;
            string FileType = "";
            try
            {
                rKey = Registry.ClassesRoot;
                sKey = rKey.OpenSubKey(ext);
                if (sKey != null && (string)sKey.GetValue("", ext) != ext)
                {
                    sKey = rKey.OpenSubKey((string)sKey.GetValue("", ext));
                    FileType = (string)sKey.GetValue("");
                }
                else
                    FileType = ext.Substring(ext.LastIndexOf('.') + 1).ToUpper() + " File";
                return FileType;
            }
            finally
            {
                if (sKey != null) sKey.Close();
                if (rKey != null) rKey.Close();
            }
        }
        #endregion
        #region FormFunctions
        private void btnDc_Click(object sender, EventArgs e)
        {
            if (Logged)
            {
                LogOut();
                btnDc.Enabled = false;
                TreeView.Nodes.Clear();
                TreeView.Nodes.Add("Waiting..");
            }
        }
        private void cleanOldConnectionInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!RemoveRegistry())
                return;
            btnOldCon.DropDown.Items.Clear();
        }
        private void sendCommandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string send=Interaction.InputBox("String to Send : ", "Send String", "", this.Location.X, this.Location.Y);
            if (send != "" || send != null)
                SendCommand(send);
            else
                MessageBox.Show("ERROR", "Bad Characters in Send String");
        }
        private void removeFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveDirectory(TreeView.SelectedNode.Tag.ToString());
        }
        private void TreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label!="" && e.Label!=null)
            {
                e.Node.Tag=e.Node.Tag.ToString().Replace("New Folder", e.Label);
                CreateDirectory(e.Node.Tag.ToString(),e.Node.Index);
            }
            if (e.Label == null)
            {
                AppendText(rchLog,"Status : You Should Give a Name to Your Directory\n",Color.Red);
                e.Node.Remove();
            }
        }
        private void newFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFoldertoTreeView("",null);
        }
        private void lstServerFiles_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
            RnmOldName = TreeView.SelectedNode.Tag + "/" + lstServerFiles.SelectedItems[0].Text;
        }
        private void lstServerFiles_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label != "" && e.Label != null)
                RenameFile(RnmOldName, e.Label);
        }
        private void lstClientFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstClientFiles.SelectedItems.Count!=0)
            {
                for (int i = 0; i < lstClientFiles.Items.Count; i++)
                {
                    if (lstClientFiles.Items[i].Selected)
                    {
                        UploadFile(fileExplorer1.SelectedDirectoryPath() + "\\" + lstClientFiles.Items[i].Text);
                        TreeViewEventArgs e2 = new TreeViewEventArgs(TreeView.SelectedNode);
                        TreeView_AfterSelect(sender, e2);
                    }
                }
            }
        }
        private void lstServerFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstServerFiles.SelectedItems.Count!=0)
            {
                for (int i = 0; i < lstServerFiles.Items.Count; i++)
                {
                    if (lstServerFiles.Items[i].Selected)
                        DownloadFile(TreeView.SelectedNode.Tag + "/" + lstServerFiles.Items[i].Text,fileExplorer1.SelectedDirectoryPath()+"\\"+lstServerFiles.Items[i].Text);
                }
            }
            FExp_Tree_MouseClick(sender, e);
        }
        private void FExp_Tree_MouseClick(object sender, MouseEventArgs e)
        {
            lstClientFiles.Items.Clear();
            if (fileExplorer1.SelectedDirectoryName()!="")
            {
                ImageList temp = new ImageList();
                for (int i = 0; i < fileExplorer1.SelectedDirectoryFiles().Length; i++)
                {
                    temp.Images.Add(Icon.ExtractAssociatedIcon(fileExplorer1.SelectedDirectoryFiles()[i]));
                    FileInfo fi = new FileInfo(fileExplorer1.SelectedDirectoryFiles()[i]);
                    lstClientFiles.SmallImageList = temp;
                    lstClientFiles.Items.Add(fi.Name);
                    lstClientFiles.Items[i].SubItems.Add(fi.Length.ToString()+" Bytes");
                    lstClientFiles.Items[i].SubItems.Add(GetFileType(fi.Extension));
                    lstClientFiles.Items[i].ImageIndex = i;
                }
            }
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            TreeView.Nodes[0].Nodes.Clear();
            if (TreeView.Nodes[0].Name != "main")
            {
                TreeView.Nodes[0].Remove();
                TreeNode main = new TreeNode();
                main.Name = "main";
                main.Text = @"/";
                TreeView.Nodes.Add(main);
            }
            Server = txtServer.Text;
            UserName = txtusername.Text;
            Password = txtPassword.Text;
            Port = int.Parse(txtPort.Text);
            FtpLogin();
            if (Logged)
            {
                if (ControlForMore(Server))
                {
                    WriteRegistry(OldConCount().ToString(), txtServer.Text, txtusername.Text, txtPassword.Text, txtPort.Text);
                    btnOldCon.DropDown.Items.Add(Server);
                }
                ParseDirNames(TreeView.Nodes[0]);
                btnDc.Enabled = true;
            }
            else
                TreeView.Nodes[0].Nodes.Clear();
        }

        private void rchLog_TextChanged(object sender, EventArgs e)
        {
            rchLog.ScrollToCaret();
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (TreeView.SelectedNode.Index != -1)
            {
                TreeView.SelectedNode.Nodes.Clear();
                ChangeWorkingDirectory(TreeView.SelectedNode.Tag.ToString());
                if (!Changed)
                    return;
                ParseDirNames(TreeView.SelectedNode);
                for (int i = 0; i < lstServerFiles.Items.Count; i++)
                {
                    if (lstServerFiles.Items[i].SubItems[2].Text == "")
                    {
                        try
                        {
                            fi2 = new FileInfo(lstServerFiles.Items[i].Text);
                        }
                        catch
                        {
                            lstServerFiles.Items[i].SubItems.Add("");
                            return;
                        }
                        lstServerFiles.Items[i].SubItems[2].Text = fi2.Extension.ToUpper().Trim('.') + " File";
                    }
                }
            }
        }
        private void DrpDown_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem a=(ToolStripMenuItem)sender;
            string[] parts = ReadServerInfo(a.Text).Split(new char[] {','});
            txtServer.Text = parts[0];
            txtusername.Text = parts[1];
            txtPassword.Text = parts[2];
            txtPort.Text = parts[3];
            if (!Logged)
                btnConnect_Click(sender, e);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < OldConCount(); i++)
            {
                btnOldCon.DropDown.Items.Add(ReadServerNamesRegistry(i.ToString()));
                btnOldCon.DropDown.Items[i].Click += new EventHandler(DrpDown_Click);
            }
        }
        private void deleteFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstServerFiles.Items.Count; i++)
            {
                if (lstServerFiles.Items[i].Selected == true)
                    DeleteFile(lstServerFiles.Items[i].Text);
            }
            if (Deleted)
                lstServerFiles.SelectedItems[0].Remove();
        }
        private void renameFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstServerFiles.SelectedItems.Count > 0)
                lstServerFiles.SelectedItems[0].BeginEdit();
        }
        private void uploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UploadDirectory(fileExplorer1.SelectedDirectoryPath(), TreeView.SelectedNode.Tag.ToString(), fileExplorer1.SelectedDirectoryName(),TreeView.SelectedNode);
        }
        private void lstServerFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        private void lstServerFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] File = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (System.IO.Path.GetExtension(File[0]) != "" && System.IO.Path.GetExtension(File[0]) != null)
            {
                UploadFile(File[0]);
                TreeViewEventArgs e2=new TreeViewEventArgs(TreeView.SelectedNode);
                TreeView_AfterSelect(sender, e2);
            }
            else return;
        }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
