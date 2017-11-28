using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace FileExplorerComponent
{
    [ToolboxBitmap("Resouces.mypng.png")]
    public partial class FileExplorer : UserControl
    {
        public FileExplorer()
        {
            InitializeComponent();
            
        }
        #region Variables
        TreeNode TreeNodeMyComputer,node;
        string selectedPath = "home";
        #endregion
        #region Public Functions
        public string SelectedDirectoryPath()
        {
            return txtPath.Text;
        }
        public string SelectedDirectoryName()
        {
            return TreeView.SelectedNode.Text;
        }
        public string[] SelectedDirectoryFiles()
        {
            return Directory.GetFiles(txtPath.Text);
        }
        #endregion
        #region Form Functions
        private void TreeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button==MouseButtons.Right || e.Button==MouseButtons.Left)
            {
                TreeView.SelectedNode = TreeView.GetNodeAt(e.X, e.Y);
            }
        }
        private void btnUp_Click(object sender, EventArgs e)
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(txtPath.Text);
                if (info.Parent.Exists)
                    txtPath.Text = info.Parent.FullName;
                btnGo_Click(sender, e);
            }
            catch (Exception)
            {
            }
        }
        private void btnGo_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                ExploreMyComputer();
                string myString = "";
                int h = 1;
                myString = txtPath.Text.ToLower();
                TreeNode tn = TreeNodeMyComputer;
            StartAgain:
                do
                {
                    foreach (TreeNode t in tn.Nodes)
                    {
                        string mypath = t.Tag.ToString();
                        mypath = mypath.ToLower();
                        string mypathf = mypath;
                        if (!mypath.EndsWith(@"\"))
                        {
                            if (myString.Length > mypathf.Length) mypathf = mypath + @"\";
                        }

                        if (myString.StartsWith(mypathf))
                        {
                            t.TreeView.Focus();
                            t.TreeView.SelectedNode = t;
                            t.EnsureVisible();
                            t.Expand();
                            if (t.Nodes.Count >= 1)
                            {
                                t.Expand();
                                tn = t;
                            }
                            else
                            {
                                if (String.Compare(myString, mypath) == 0)
                                {
                                    h = -1;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            if (mypathf.StartsWith(myString))
                            {
                                h = -1;
                                break;
                            }
                            else
                            {
                                goto StartAgain;
                                //return;
                            }
                        }
                    }

                    try
                    {
                        tn = tn.NextNode;
                    }
                    catch (Exception)
                    { }

                } while (h >= 0);

            }
            catch (Exception e1)
            {
                MessageBox.Show("Error: " + e1.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                refreshFolders();
            }
            catch (Exception e1)
            {
                MessageBox.Show("Error: " + e1.Message);
            }
            finally
            {
                setCurrentPath("home");
                Cursor.Current = Cursors.Default;
                ExploreMyComputer();
            }
        }

        private void TreeView_DoubleClick(object sender, EventArgs e)
        {
            TreeNode n;
            n = TreeView.SelectedNode;
            if (!TreeView.SelectedNode.IsExpanded)
                TreeView.SelectedNode.Collapse();
            else
            {
                ExploreTreeNode(n);
            }
        }
        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode n;
            n = e.Node;
            try
            {
                if ((String.Compare(n.Text, "My Computer") == 0))
                {
                }
                else
                {
                    txtPath.Text = n.Tag.ToString();
                }
            }
            catch { }
        }
        private void UserControl1_Load(object sender, EventArgs e)
        {
            GetTreeViewDirectory();
            if (Directory.Exists(selectedPath))
            {
                setCurrentPath(selectedPath);
            }
            else
            {
                setCurrentPath("home");
            }
            btnGo_Click(this, e);
        }
        private void TreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            string[] drives = Environment.GetLogicalDrives();
            string dir2 = "";
            Cursor.Current = Cursors.WaitCursor;
            TreeNode n;
            TreeNode nodemyC;
            TreeNode nodeDrive;
            nodemyC = e.Node;
            n = e.Node;
            if (n.Text.IndexOf(":", 1) > 0)
            {
                ExploreTreeNode(n);
            }
            else
            {//(String.Compare(n.Text ,"My Documents")==0) || (String.Compare(n.Text,"Desktop")==0) || 

                if ((String.Compare(n.Text, "Desktop") == 0) || String.Compare(n.Text,"My Computer")==0)
                {
                    if ((String.Compare(n.Text, "My Computer") == 0) && (nodemyC.GetNodeCount(true) < 2))
                    ///////////
                    //add each drive and files and dirs
                    {
                        nodemyC.FirstNode.Remove();

                        foreach (string drive in drives)
                        {
                            nodeDrive = new TreeNode();
                            nodeDrive.Tag = drive;
                            nodeDrive.Text = drive;
                            //Determine icon to display by drive
                            switch (Win32.GetDriveType(drive))
                            {
                                case 2:
                                    nodeDrive.ImageIndex = 17;
                                    nodeDrive.SelectedImageIndex = 17;
                                    break;
                                case 3:
                                    nodeDrive.ImageIndex = 0;
                                    nodeDrive.SelectedImageIndex = 0;
                                    break;
                                case 4:
                                    nodeDrive.ImageIndex = 8;
                                    nodeDrive.SelectedImageIndex = 8;
                                    break;
                                case 5:
                                    nodeDrive.ImageIndex = 7;
                                    nodeDrive.SelectedImageIndex = 7;
                                    break;
                                default:
                                    nodeDrive.ImageIndex = 0;
                                    nodeDrive.SelectedImageIndex = 0;
                                    break;
                            }

                            nodemyC.Nodes.Add(nodeDrive);
                            nodeDrive.EnsureVisible();
                            TreeView.Refresh();
                            try
                            {
                                //add dirs under drive
                                if (Directory.Exists(drive))
                                {
                                    foreach (string dir in Directory.GetDirectories(drive))
                                    {
                                        dir2 = dir;
                                        node = new TreeNode();
                                        node.Tag = dir;
                                        node.Text = dir.Substring(dir.LastIndexOf(@"\") + 1);
                                        node.ImageIndex = 1;
                                        nodeDrive.Nodes.Add(node);
                                    }
                                }
                            }
                            catch (Exception)	//error just add blank dir
                            {

                            }
                            nodemyC.Expand();
                        }
                    }
                    else
                    {
                        ExploreTreeNode(n);
                    }
                }
                else
                {
                    ExploreTreeNode(n);
                }
            }
            Cursor.Current = Cursors.Default;
        }
        #endregion
        #region Functions
        public void setCurrentPath(string strPath)
        {
            selectedPath = strPath;
            if (String.Compare(strPath, "home") == 0)
            {
                txtPath.Text = Application.StartupPath;
            }
            else
            {
                DirectoryInfo inf = new DirectoryInfo(strPath);
                if (inf.Exists)
                {
                    txtPath.Text = strPath;
                }
                else
                    txtPath.Text = Application.StartupPath;
            }
        }
        private void GetTreeViewDirectory()
        {
            TreeView.Nodes.Clear();
            TreeNode nodeD = new TreeNode();
            nodeD.Tag = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            nodeD.Text = "Desktop";
            nodeD.ImageIndex = 10;
            nodeD.SelectedImageIndex = 10;
            TreeView.Nodes.Add(nodeD);
            //Add My Documents and Desktop folder outside
            TreeNode nodemydoc = new TreeNode();
            nodemydoc.Tag = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            nodemydoc.Text = "My Documents";
            nodemydoc.ImageIndex = 9;
            nodemydoc.SelectedImageIndex = 9;
            nodeD.Nodes.Add(nodemydoc);
            GetFilesAndDir(nodemydoc);
            //MyComputer
            TreeNode nodemyComp = new TreeNode();
            nodemyComp.Tag = "My Computer";
            nodemyComp.Text = "My Computer";
            nodemyComp.ImageIndex = 12;
            nodemyComp.SelectedImageIndex = 12;
            nodeD.Nodes.Add(nodemyComp);
            nodemyComp.EnsureVisible();
            TreeNodeMyComputer = nodemyComp;
            TreeNode nodemNc = new TreeNode(); 
            nodemNc.Tag = "my Node";
            nodemNc.Text = "my Node";
            nodemNc.ImageIndex = 12;
            nodemNc.SelectedImageIndex = 12;
            nodemyComp.Nodes.Add(nodemNc);
            //MyFavorites
            TreeNode nodemf = new TreeNode();
            nodemf.Tag = Environment.GetFolderPath(Environment.SpecialFolder.Favorites);
            nodemf.Text = "My Favorites";
            nodemf.ImageIndex = 26;
            nodemf.SelectedImageIndex = 26;
            nodeD.Nodes.Add(nodemf);
            GetFilesAndDir(nodemf);

            ExploreTreeNode(nodeD);
        }
        public void refreshFolders()
        {
            TreeView.Nodes.Clear();
            setCurrentPath(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            GetTreeViewDirectory();
        }
        private void ExploreTreeNode(TreeNode n)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                GetFilesAndDir(n);
                foreach (TreeNode node in n.Nodes)
                {
                    if (String.Compare(n.Text, "Desktop") == 0)
                    {
                        if ((String.Compare(node.Text, "My Documents") == 0) || (String.Compare(node.Text, "My Computer") == 0))
                        { }
                        else
                        {
                            GetFilesAndDir(node);
                        }
                    }
                    else
                    {
                        GetFilesAndDir(node);
                    }
                }
            }
            catch
            { }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void GetSubDirectories(TreeNode parentNode)
        {
            string[] dirList;
            dirList = Directory.GetDirectories(parentNode.Tag.ToString());
            Array.Sort(dirList);
            if (dirList.Length == parentNode.Nodes.Count)
                return;
            for (int i = 0; i < dirList.Length; i++)
            {
                node = new TreeNode();
                node.Tag = dirList[i]; 
                node.Text = dirList[i].Substring(dirList[i].LastIndexOf(@"\") + 1);
                node.ImageIndex = 1;
                parentNode.Nodes.Add(node);
            }
        }
        private void GetFilesAndDir(TreeNode Node)
        {
            try
            {
                GetSubDirectories(Node);
            }
            catch (Exception)
            {
                return;
            }
        }
        private void ExploreMyComputer()
        {
            string[] drives = Environment.GetLogicalDrives();
            string dir2 = "";
            Cursor.Current = Cursors.WaitCursor;
            TreeNode nodeDrive;
            if (TreeNodeMyComputer.GetNodeCount(true) < 2)
            {
                TreeNodeMyComputer.FirstNode.Remove();
                foreach (string drive in drives)
                {
                    nodeDrive = new TreeNode();
                    nodeDrive.Tag = drive;

                    nodeDrive.Text = drive;

                    switch (Win32.GetDriveType(drive))
                    {
                        case 2:
                            nodeDrive.ImageIndex = 17;
                            nodeDrive.SelectedImageIndex = 17;
                            break;
                        case 3:
                            nodeDrive.ImageIndex = 0;
                            nodeDrive.SelectedImageIndex = 0;
                            break;
                        case 4:
                            nodeDrive.ImageIndex = 8;
                            nodeDrive.SelectedImageIndex = 8;
                            break;
                        case 5:
                            nodeDrive.ImageIndex = 7;
                            nodeDrive.SelectedImageIndex = 7;
                            break;
                        default:
                            nodeDrive.ImageIndex = 0;
                            nodeDrive.SelectedImageIndex = 0;
                            break;
                    }

                    TreeNodeMyComputer.Nodes.Add(nodeDrive);
                    try
                    {
                        //add dirs under drive
                        if (Directory.Exists(drive))
                        {
                            foreach (string dir in Directory.GetDirectories(drive))
                            {
                                dir2 = dir;
                                node = new TreeNode();
                                node.Tag = dir;
                                node.Text = dir.Substring(dir.LastIndexOf(@"\") + 1);
                                node.ImageIndex = 1;
                                nodeDrive.Nodes.Add(node);
                            }
                        }
                    }
                    catch (Exception ex)	//error just add blank dir
                    {
                        MessageBox.Show("Error while Filling the Explorer:" + ex.Message);
                    }
                }
            }
            TreeNodeMyComputer.Expand();
        }
        #endregion
        #region Struct,Class
        public class Win32
        {
            public const uint SHGFI_ICON = 0x100;
            //public const uint SHGFI_LARGEICON = 0x0;    // 'Large icon
            public const uint SHGFI_SMALLICON = 0x1;    // 'Small icon

            [DllImport("shell32.dll")]
            public static extern IntPtr SHGetFileInfo(
                string pszPath,
                uint dwFileAttributes,
                ref SHFILEINFO psfi,
                uint cbSizeFileInfo,
                uint uFlags);

            [DllImport("kernel32")]
            public static extern uint GetDriveType(
                string lpRootPathName);

            [DllImport("shell32.dll")]
            public static extern bool SHGetDiskFreeSpaceEx(
                string pszVolume,
                ref ulong pqwFreeCaller,
                ref ulong pqwTot,
                ref ulong pqwFree);

            [DllImport("shell32.Dll")]
            public static extern int SHQueryRecycleBin(
                string pszRootPath,
                ref SHQUERYRBINFO pSHQueryRBInfo);

            [StructLayout(LayoutKind.Sequential)]
            public struct SHFILEINFO
            {
                public IntPtr hIcon;
                public IntPtr iIcon;
                public uint dwAttributes;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
                public string szDisplayName;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
                public string szTypeName;
            };
            [StructLayout(LayoutKind.Sequential)]
            public class BITMAPINFO
            {
                public Int32 biSize;
                public Int32 biWidth;
                public Int32 biHeight;
                public Int16 biPlanes;
                public Int16 biBitCount;
                public Int32 biCompression;
                public Int32 biSizeImage;
                public Int32 biXPelsPerMeter;
                public Int32 biYPelsPerMeter;
                public Int32 biClrUsed;
                public Int32 biClrImportant;
                public Int32 colors;
            };
            [DllImport("comctl32.dll")]
            public static extern bool ImageList_Add(IntPtr hImageList, IntPtr hBitmap, IntPtr hMask);
            [DllImport("kernel32.dll")]
            private static extern bool RtlMoveMemory(IntPtr dest, IntPtr source, int dwcount);
            [DllImport("shell32.dll")]
            public static extern IntPtr DestroyIcon(IntPtr hIcon);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateDIBSection(IntPtr hdc, [In, MarshalAs(UnmanagedType.LPStruct)]BITMAPINFO pbmi, uint iUsage, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);


        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SHQUERYRBINFO
        {
            public uint cbSize;
            public ulong i64Size;
            public ulong i64NumItems;
        };
        #endregion
    }
}
