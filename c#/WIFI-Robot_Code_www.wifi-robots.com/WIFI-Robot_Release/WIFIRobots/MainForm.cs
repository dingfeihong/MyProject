

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using VideoSource;
using Tiger.Video.VFW;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.IO.Ports;
using Blaney;
using WIFIRobotCMDEngineV2;
using System.Threading;
using System.Diagnostics;
using System.IO;
using ChangeVGA;
using WIFIRobot;
using AviFile;
using WIFIRobot.Properties;

namespace motion
{
    /// <summary>
    /// Summary description for MainForm
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        // statistics
        private const int statLength = 15;
        private int statIndex = 0, statReady = 0;
        private int[] statCount = new int[statLength];
        private SerialPort comm = new SerialPort();
        private StringBuilder builder = new StringBuilder();
        private bool Listening = false;//是否没有执行完invoke相关操作

        private IMotionDetector detector = null;
        private int detectorType = 0;
        private int signalType = 0;
        private int intervalsToSave = 0;
        static string RootPath = Application.StartupPath;
        static string FileName = RootPath + "\\Config.ini";
        static string ImagePath = Application.StartupPath+"\\Snapshot\\";
        static string VideoPath = Application.StartupPath + "\\Video\\";
        private AVIWriter writer = null;
        private bool saveOnMotion = false;
        private int x_data = 0, y_data = 0;
        bool MjpgFlag = false;

        string CameraIp = "";
        string ControlIp = "192.168.1.1";
        string Port = "2001";
        string CMD_Forward = "", CMD_Backward = "", CMD_TurnLeft = "", CMD_TurnRight = "", CMD_Stop = "";
        string CMD_TurnOnLight = "", CMD_TurnOffLight = "", CMD_Beep = "";
        string CMD_LeftForward = "", CMD_RightForward = "",CMD_LeftBackward="",CMD_RightBackward="";
        string AutoSetScreen;
        private int controlType = 3;
        private string btCom;
        private string btBaudrate;
        public WifiRobotCMDEngine RobotEngine ;//实例化引擎
        public WifiRobotCMDEngineV2 RobotEngine2;//实例化引擎
        static IPAddress ips;
        static IPEndPoint ipe;
        static Socket socket = null;

        private long received_count = 0;//接收计数   
        private long send_count = 0;//发送计数

        Radar MyRadar;
        RadarItem item1;
        VGA vga  = new VGA();

        public bool Send_status = true;
        int VideoRecordCount = 0;
        public double videoRecRate;
        AviManager aviManager;
        FrameRateForm dlg = new FrameRateForm();
        AviFile.VideoStream aviStream;

        #region one
        private System.Windows.Forms.MenuItem fileItem;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem exitFileItem;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.MainMenu mainMenu;
        private System.Timers.Timer timer;
        private System.Windows.Forms.StatusBar statusBar;
        private System.Windows.Forms.StatusBarPanel fpsPanel;
        private System.Windows.Forms.Panel panel;
        private motion.CameraWindow cameraWindow;
        private System.Windows.Forms.MenuItem motionItem;
        private System.Windows.Forms.MenuItem noneMotionItem;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem detector1MotionItem;
        private System.Windows.Forms.MenuItem detector2MotionItem;
        private System.Windows.Forms.MenuItem detector3MotionItem;
        private System.Windows.Forms.MenuItem detector3OptimizedMotionItem;
        private System.Windows.Forms.MenuItem SYSConfig;
        private System.Windows.Forms.MenuItem detector4MotionItem;
        private System.Windows.Forms.MenuItem helpItem;
        private System.Windows.Forms.MenuItem aboutHelpItem;
        private System.Windows.Forms.MenuItem menuItem3;
        private GroupBox groupBox1;
        private Button btnLightTurnOff;
        private Button buttonStop;
        private Button btnRadar;
        private Button btnLightTurnOn;
        private Button buttonBackward;
        private Button buttonRight;
        private Button buttonLeft;
        private Button buttonForward;
        private Button btnOpenVideo;
        private MenuItem menuItem4;
        private MenuItem menuItem5;
        private MenuItem menuItem6;
        private GroupBox groupBox2;
        private VScrollBar Gear1;
        private Label label1;
        private Label label5;
        private VScrollBar Gear5;
        private Label label4;
        private VScrollBar Gear4;
        private Label label3;
        private VScrollBar Gear3;
        private Label label2;
        private VScrollBar Gear2;
        private GroupBox groupBox3;
        private PictureBox RadarPanel;
        private Label label_radar1;
        private GroupBox groupBox4;
        private HScrollBar Speedleft;
        private Label label_SpeedL;
        private Label label7;
        private Label label_SpeedR;
        private HScrollBar Speedright;
        private Label label8;
        private Button btnSpeaker;
        private GroupBox groupBox5;
        private GroupBox GroupBoxControl;
        private TextBox txtCommandPanel;
        private Button btnSend;
        private Label label6;
        private Label labelGetCount;
        private Label label_radar2;
        private Button btnClear;
        private Button button1;
        private Button btnGearsRegual;
        private StatusBarPanel GroupINFO;
        private StatusBarPanel Systemstatus;
        private VScrollBar Gear8;
        private VScrollBar Gear7;
        private VScrollBar Gear6;
        private Label label11;
        private Label label10;
        private Label label9;
        private MenuItem menuItem7;
        private MenuItem nonesignal;
        private MenuItem signal1;
        private MenuItem signal2;
        private Button btnCustom1;
        private Button btnCustom12;
        private Button btnCustom10;
        private Button btnCustom8;
        private Button btnCustom11;
        private Button btnCustom9;
        private Button btnCustom7;
        private Button btnCustom6;
        private Button btnCustom4;
        private Button btnCustom2;
        private Button btnCustom5;
        private Button btnCustom3;
        private Button btnGearStandby;
        private Button btnCaptureVideo;
        private Button btnRightForward;
        private Button btnLeftForward;
        private Button btnRightBack;
        private Button btnLeftBack;
        private Button btnTakePhotos;
        private IContainer components;

        #endregion

        //声明读写INI文件的API函数
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            RobotEngine2 = new WifiRobotCMDEngineV2((Object)this.statusBar);
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.fileItem = new System.Windows.Forms.MenuItem();
            this.SYSConfig = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.exitFileItem = new System.Windows.Forms.MenuItem();
            this.motionItem = new System.Windows.Forms.MenuItem();
            this.noneMotionItem = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.detector1MotionItem = new System.Windows.Forms.MenuItem();
            this.detector2MotionItem = new System.Windows.Forms.MenuItem();
            this.detector3MotionItem = new System.Windows.Forms.MenuItem();
            this.detector3OptimizedMotionItem = new System.Windows.Forms.MenuItem();
            this.detector4MotionItem = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.nonesignal = new System.Windows.Forms.MenuItem();
            this.signal1 = new System.Windows.Forms.MenuItem();
            this.signal2 = new System.Windows.Forms.MenuItem();
            this.helpItem = new System.Windows.Forms.MenuItem();
            this.aboutHelpItem = new System.Windows.Forms.MenuItem();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.timer = new System.Timers.Timer();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.fpsPanel = new System.Windows.Forms.StatusBarPanel();
            this.Systemstatus = new System.Windows.Forms.StatusBarPanel();
            this.GroupINFO = new System.Windows.Forms.StatusBarPanel();
            this.panel = new System.Windows.Forms.Panel();
            this.GroupBoxControl = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.labelGetCount = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtCommandPanel = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnTakePhotos = new System.Windows.Forms.Button();
            this.btnCaptureVideo = new System.Windows.Forms.Button();
            this.btnGearStandby = new System.Windows.Forms.Button();
            this.btnCustom12 = new System.Windows.Forms.Button();
            this.btnCustom10 = new System.Windows.Forms.Button();
            this.btnCustom8 = new System.Windows.Forms.Button();
            this.btnCustom11 = new System.Windows.Forms.Button();
            this.btnCustom9 = new System.Windows.Forms.Button();
            this.btnCustom7 = new System.Windows.Forms.Button();
            this.btnCustom6 = new System.Windows.Forms.Button();
            this.btnCustom4 = new System.Windows.Forms.Button();
            this.btnCustom2 = new System.Windows.Forms.Button();
            this.btnCustom5 = new System.Windows.Forms.Button();
            this.btnCustom3 = new System.Windows.Forms.Button();
            this.btnCustom1 = new System.Windows.Forms.Button();
            this.btnGearsRegual = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label_SpeedR = new System.Windows.Forms.Label();
            this.Speedright = new System.Windows.Forms.HScrollBar();
            this.label_SpeedL = new System.Windows.Forms.Label();
            this.Speedleft = new System.Windows.Forms.HScrollBar();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label_radar2 = new System.Windows.Forms.Label();
            this.label_radar1 = new System.Windows.Forms.Label();
            this.RadarPanel = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.Gear8 = new System.Windows.Forms.VScrollBar();
            this.Gear7 = new System.Windows.Forms.VScrollBar();
            this.Gear6 = new System.Windows.Forms.VScrollBar();
            this.label5 = new System.Windows.Forms.Label();
            this.Gear5 = new System.Windows.Forms.VScrollBar();
            this.label4 = new System.Windows.Forms.Label();
            this.Gear4 = new System.Windows.Forms.VScrollBar();
            this.label3 = new System.Windows.Forms.Label();
            this.Gear3 = new System.Windows.Forms.VScrollBar();
            this.label2 = new System.Windows.Forms.Label();
            this.Gear2 = new System.Windows.Forms.VScrollBar();
            this.label1 = new System.Windows.Forms.Label();
            this.Gear1 = new System.Windows.Forms.VScrollBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRightBack = new System.Windows.Forms.Button();
            this.btnLeftBack = new System.Windows.Forms.Button();
            this.btnLeftForward = new System.Windows.Forms.Button();
            this.btnRightForward = new System.Windows.Forms.Button();
            this.btnSpeaker = new System.Windows.Forms.Button();
            this.btnLightTurnOff = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.btnRadar = new System.Windows.Forms.Button();
            this.btnLightTurnOn = new System.Windows.Forms.Button();
            this.buttonBackward = new System.Windows.Forms.Button();
            this.buttonRight = new System.Windows.Forms.Button();
            this.buttonLeft = new System.Windows.Forms.Button();
            this.buttonForward = new System.Windows.Forms.Button();
            this.btnOpenVideo = new System.Windows.Forms.Button();
            this.cameraWindow = new motion.CameraWindow();
            ((System.ComponentModel.ISupportInitialize)(this.timer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Systemstatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GroupINFO)).BeginInit();
            this.panel.SuspendLayout();
            this.GroupBoxControl.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RadarPanel)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.fileItem,
            this.motionItem,
            this.menuItem4,
            this.menuItem7,
            this.helpItem});
            // 
            // fileItem
            // 
            this.fileItem.Index = 0;
            this.fileItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.SYSConfig,
            this.menuItem1,
            this.exitFileItem});
            this.fileItem.Text = "平台操作";
            // 
            // SYSConfig
            // 
            this.SYSConfig.Index = 0;
            this.SYSConfig.Text = "系统设置";
            this.SYSConfig.Click += new System.EventHandler(this.SYSConfig_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 1;
            this.menuItem1.Text = "-";
            // 
            // exitFileItem
            // 
            this.exitFileItem.Index = 2;
            this.exitFileItem.Text = "退出";
            this.exitFileItem.Click += new System.EventHandler(this.exitFileItem_Click);
            // 
            // motionItem
            // 
            this.motionItem.Index = 1;
            this.motionItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.noneMotionItem,
            this.menuItem2,
            this.detector1MotionItem,
            this.detector2MotionItem,
            this.detector3MotionItem,
            this.detector3OptimizedMotionItem,
            this.detector4MotionItem,
            this.menuItem3});
            this.motionItem.Text = "视频效果";
            this.motionItem.Popup += new System.EventHandler(this.motionItem_Popup);
            // 
            // noneMotionItem
            // 
            this.noneMotionItem.Index = 0;
            this.noneMotionItem.Text = "标准（默认）";
            this.noneMotionItem.Click += new System.EventHandler(this.noneMotionItem_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.Text = "-";
            // 
            // detector1MotionItem
            // 
            this.detector1MotionItem.Index = 2;
            this.detector1MotionItem.Text = "探测模式1";
            this.detector1MotionItem.Click += new System.EventHandler(this.detector1MotionItem_Click);
            // 
            // detector2MotionItem
            // 
            this.detector2MotionItem.Index = 3;
            this.detector2MotionItem.Text = "探测模式2";
            this.detector2MotionItem.Click += new System.EventHandler(this.detector2MotionItem_Click);
            // 
            // detector3MotionItem
            // 
            this.detector3MotionItem.Index = 4;
            this.detector3MotionItem.Text = "探测模式3";
            this.detector3MotionItem.Click += new System.EventHandler(this.detector3MotionItem_Click);
            // 
            // detector3OptimizedMotionItem
            // 
            this.detector3OptimizedMotionItem.Index = 5;
            this.detector3OptimizedMotionItem.Text = "探测模式4";
            this.detector3OptimizedMotionItem.Click += new System.EventHandler(this.detector3OptimizedMotionItem_Click);
            // 
            // detector4MotionItem
            // 
            this.detector4MotionItem.Index = 6;
            this.detector4MotionItem.Text = "探测模式5";
            this.detector4MotionItem.Click += new System.EventHandler(this.detector4MotionItem_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 7;
            this.menuItem3.Text = "-";
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem5,
            this.menuItem6});
            this.menuItem4.Text = "控制模式";
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 0;
            this.menuItem5.Text = "WIFI模式";
            this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 1;
            this.menuItem6.Text = "蓝牙模式";
            this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 3;
            this.menuItem7.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.nonesignal,
            this.signal1,
            this.signal2});
            this.menuItem7.Text = "辅助功能";
            this.menuItem7.Popup += new System.EventHandler(this.menuItem7_Popup);
            // 
            // nonesignal
            // 
            this.nonesignal.Checked = true;
            this.nonesignal.Index = 0;
            this.nonesignal.Text = "无准星";
            this.nonesignal.Click += new System.EventHandler(this.nonesignal_Click);
            // 
            // signal1
            // 
            this.signal1.Index = 1;
            this.signal1.Text = "十字准星";
            this.signal1.Click += new System.EventHandler(this.signal1_Click);
            // 
            // signal2
            // 
            this.signal2.Index = 2;
            this.signal2.Text = "圆形准星";
            this.signal2.Click += new System.EventHandler(this.signal2_Click);
            // 
            // helpItem
            // 
            this.helpItem.Index = 4;
            this.helpItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.aboutHelpItem});
            this.helpItem.Text = "帮助";
            // 
            // aboutHelpItem
            // 
            this.aboutHelpItem.Index = 0;
            this.aboutHelpItem.Text = "关于";
            this.aboutHelpItem.Click += new System.EventHandler(this.aboutHelpItem_Click);
            // 
            // ofd
            // 
            this.ofd.Filter = "AVI files (*.avi)|*.avi";
            this.ofd.Title = "Open movie";
            // 
            // timer
            // 
            this.timer.Interval = 1000D;
            this.timer.SynchronizingObject = this;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 586);
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.fpsPanel,
            this.Systemstatus,
            this.GroupINFO});
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = new System.Drawing.Size(1064, 24);
            this.statusBar.TabIndex = 1;
            // 
            // fpsPanel
            // 
            this.fpsPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.fpsPanel.MinWidth = 300;
            this.fpsPanel.Name = "fpsPanel";
            this.fpsPanel.Width = 347;
            // 
            // Systemstatus
            // 
            this.Systemstatus.MinWidth = 40;
            this.Systemstatus.Name = "Systemstatus";
            this.Systemstatus.Width = 200;
            // 
            // GroupINFO
            // 
            this.GroupINFO.Name = "GroupINFO";
            this.GroupINFO.Text = "www.wifi-robots.com WIFI机器人网·机器人创意工作室  QQ群： 145181710/196564839 ";
            this.GroupINFO.Width = 500;
            // 
            // panel
            // 
            this.panel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel.Controls.Add(this.GroupBoxControl);
            this.panel.Controls.Add(this.groupBox5);
            this.panel.Controls.Add(this.groupBox4);
            this.panel.Controls.Add(this.groupBox3);
            this.panel.Controls.Add(this.groupBox2);
            this.panel.Controls.Add(this.groupBox1);
            this.panel.Controls.Add(this.cameraWindow);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(1064, 586);
            this.panel.TabIndex = 2;
            this.panel.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_Paint);
            // 
            // GroupBoxControl
            // 
            this.GroupBoxControl.Controls.Add(this.btnClear);
            this.GroupBoxControl.Controls.Add(this.labelGetCount);
            this.GroupBoxControl.Controls.Add(this.label6);
            this.GroupBoxControl.Controls.Add(this.btnSend);
            this.GroupBoxControl.Controls.Add(this.txtCommandPanel);
            this.GroupBoxControl.Location = new System.Drawing.Point(273, 378);
            this.GroupBoxControl.Name = "GroupBoxControl";
            this.GroupBoxControl.Size = new System.Drawing.Size(452, 182);
            this.GroupBoxControl.TabIndex = 17;
            this.GroupBoxControl.TabStop = false;
            this.GroupBoxControl.Text = "命令控制台";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(365, 74);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 37);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "清除缓冲区";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // labelGetCount
            // 
            this.labelGetCount.AutoSize = true;
            this.labelGetCount.Location = new System.Drawing.Point(369, 45);
            this.labelGetCount.Name = "labelGetCount";
            this.labelGetCount.Size = new System.Drawing.Size(23, 12);
            this.labelGetCount.TabIndex = 3;
            this.labelGetCount.Text = "0/0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(369, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "接收/发送：";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(365, 117);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 37);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "发送指令";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtCommandPanel
            // 
            this.txtCommandPanel.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.txtCommandPanel.ForeColor = System.Drawing.SystemColors.Info;
            this.txtCommandPanel.Location = new System.Drawing.Point(8, 22);
            this.txtCommandPanel.Multiline = true;
            this.txtCommandPanel.Name = "txtCommandPanel";
            this.txtCommandPanel.Size = new System.Drawing.Size(344, 151);
            this.txtCommandPanel.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnTakePhotos);
            this.groupBox5.Controls.Add(this.btnCaptureVideo);
            this.groupBox5.Controls.Add(this.btnGearStandby);
            this.groupBox5.Controls.Add(this.btnCustom12);
            this.groupBox5.Controls.Add(this.btnCustom10);
            this.groupBox5.Controls.Add(this.btnCustom8);
            this.groupBox5.Controls.Add(this.btnCustom11);
            this.groupBox5.Controls.Add(this.btnCustom9);
            this.groupBox5.Controls.Add(this.btnCustom7);
            this.groupBox5.Controls.Add(this.btnCustom6);
            this.groupBox5.Controls.Add(this.btnCustom4);
            this.groupBox5.Controls.Add(this.btnCustom2);
            this.groupBox5.Controls.Add(this.btnCustom5);
            this.groupBox5.Controls.Add(this.btnCustom3);
            this.groupBox5.Controls.Add(this.btnCustom1);
            this.groupBox5.Controls.Add(this.btnGearsRegual);
            this.groupBox5.Location = new System.Drawing.Point(739, 325);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(311, 235);
            this.groupBox5.TabIndex = 16;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "操作系统";
            // 
            // btnTakePhotos
            // 
            this.btnTakePhotos.Enabled = false;
            this.btnTakePhotos.Location = new System.Drawing.Point(18, 197);
            this.btnTakePhotos.Name = "btnTakePhotos";
            this.btnTakePhotos.Size = new System.Drawing.Size(83, 33);
            this.btnTakePhotos.TabIndex = 77;
            this.btnTakePhotos.Text = "拍照";
            this.btnTakePhotos.UseVisualStyleBackColor = false;
            this.btnTakePhotos.Click += new System.EventHandler(this.btnTakePhotos_Click);
            // 
            // btnCaptureVideo
            // 
            this.btnCaptureVideo.Enabled = false;
            this.btnCaptureVideo.Location = new System.Drawing.Point(210, 162);
            this.btnCaptureVideo.Name = "btnCaptureVideo";
            this.btnCaptureVideo.Size = new System.Drawing.Size(83, 33);
            this.btnCaptureVideo.TabIndex = 75;
            this.btnCaptureVideo.Text = "开始录像";
            this.btnCaptureVideo.UseVisualStyleBackColor = false;
            this.btnCaptureVideo.Click += new System.EventHandler(this.btnCaptureVideo_Click);
            // 
            // btnGearStandby
            // 
            this.btnGearStandby.Location = new System.Drawing.Point(115, 162);
            this.btnGearStandby.Name = "btnGearStandby";
            this.btnGearStandby.Size = new System.Drawing.Size(83, 33);
            this.btnGearStandby.TabIndex = 74;
            this.btnGearStandby.Text = "机械手归位";
            this.btnGearStandby.UseVisualStyleBackColor = false;
            this.btnGearStandby.Click += new System.EventHandler(this.btnGearStandby_Click);
            // 
            // btnCustom12
            // 
            this.btnCustom12.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCustom12.Location = new System.Drawing.Point(210, 127);
            this.btnCustom12.Name = "btnCustom12";
            this.btnCustom12.Size = new System.Drawing.Size(83, 32);
            this.btnCustom12.TabIndex = 73;
            this.btnCustom12.Text = "自定义12";
            this.btnCustom12.UseVisualStyleBackColor = true;
            this.btnCustom12.Click += new System.EventHandler(this.btnCustom12_Click);
            // 
            // btnCustom10
            // 
            this.btnCustom10.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCustom10.Location = new System.Drawing.Point(115, 127);
            this.btnCustom10.Name = "btnCustom10";
            this.btnCustom10.Size = new System.Drawing.Size(83, 32);
            this.btnCustom10.TabIndex = 72;
            this.btnCustom10.Text = "自定义10";
            this.btnCustom10.UseVisualStyleBackColor = true;
            this.btnCustom10.Click += new System.EventHandler(this.btnCustom10_Click);
            // 
            // btnCustom8
            // 
            this.btnCustom8.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCustom8.Location = new System.Drawing.Point(18, 126);
            this.btnCustom8.Name = "btnCustom8";
            this.btnCustom8.Size = new System.Drawing.Size(83, 32);
            this.btnCustom8.TabIndex = 71;
            this.btnCustom8.Text = "自定义8";
            this.btnCustom8.UseVisualStyleBackColor = true;
            this.btnCustom8.Click += new System.EventHandler(this.btnCustom8_Click);
            // 
            // btnCustom11
            // 
            this.btnCustom11.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCustom11.Location = new System.Drawing.Point(210, 91);
            this.btnCustom11.Name = "btnCustom11";
            this.btnCustom11.Size = new System.Drawing.Size(83, 32);
            this.btnCustom11.TabIndex = 70;
            this.btnCustom11.Text = "自定义11";
            this.btnCustom11.UseVisualStyleBackColor = true;
            this.btnCustom11.Click += new System.EventHandler(this.btnCustom11_Click);
            // 
            // btnCustom9
            // 
            this.btnCustom9.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCustom9.Location = new System.Drawing.Point(115, 91);
            this.btnCustom9.Name = "btnCustom9";
            this.btnCustom9.Size = new System.Drawing.Size(83, 32);
            this.btnCustom9.TabIndex = 69;
            this.btnCustom9.Text = "自定义9";
            this.btnCustom9.UseVisualStyleBackColor = true;
            this.btnCustom9.Click += new System.EventHandler(this.btnCustom9_Click);
            // 
            // btnCustom7
            // 
            this.btnCustom7.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCustom7.Location = new System.Drawing.Point(18, 90);
            this.btnCustom7.Name = "btnCustom7";
            this.btnCustom7.Size = new System.Drawing.Size(83, 32);
            this.btnCustom7.TabIndex = 68;
            this.btnCustom7.Text = "自定义7";
            this.btnCustom7.UseVisualStyleBackColor = true;
            this.btnCustom7.Click += new System.EventHandler(this.btnCustom7_Click);
            // 
            // btnCustom6
            // 
            this.btnCustom6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCustom6.Location = new System.Drawing.Point(210, 54);
            this.btnCustom6.Name = "btnCustom6";
            this.btnCustom6.Size = new System.Drawing.Size(83, 32);
            this.btnCustom6.TabIndex = 67;
            this.btnCustom6.Text = "自定义6";
            this.btnCustom6.UseVisualStyleBackColor = true;
            this.btnCustom6.Click += new System.EventHandler(this.btnCustom6_Click);
            // 
            // btnCustom4
            // 
            this.btnCustom4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCustom4.Location = new System.Drawing.Point(115, 54);
            this.btnCustom4.Name = "btnCustom4";
            this.btnCustom4.Size = new System.Drawing.Size(83, 32);
            this.btnCustom4.TabIndex = 66;
            this.btnCustom4.Text = "自定义4";
            this.btnCustom4.UseVisualStyleBackColor = true;
            this.btnCustom4.Click += new System.EventHandler(this.btnCustom4_Click);
            // 
            // btnCustom2
            // 
            this.btnCustom2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCustom2.Location = new System.Drawing.Point(18, 53);
            this.btnCustom2.Name = "btnCustom2";
            this.btnCustom2.Size = new System.Drawing.Size(83, 32);
            this.btnCustom2.TabIndex = 65;
            this.btnCustom2.Text = "自定义2";
            this.btnCustom2.UseVisualStyleBackColor = true;
            this.btnCustom2.Click += new System.EventHandler(this.btnCustom2_Click);
            // 
            // btnCustom5
            // 
            this.btnCustom5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCustom5.Location = new System.Drawing.Point(210, 17);
            this.btnCustom5.Name = "btnCustom5";
            this.btnCustom5.Size = new System.Drawing.Size(83, 32);
            this.btnCustom5.TabIndex = 64;
            this.btnCustom5.Text = "自定义5";
            this.btnCustom5.UseVisualStyleBackColor = true;
            this.btnCustom5.Click += new System.EventHandler(this.btnCustom5_Click);
            // 
            // btnCustom3
            // 
            this.btnCustom3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCustom3.Location = new System.Drawing.Point(115, 17);
            this.btnCustom3.Name = "btnCustom3";
            this.btnCustom3.Size = new System.Drawing.Size(83, 32);
            this.btnCustom3.TabIndex = 63;
            this.btnCustom3.Text = "自定义3";
            this.btnCustom3.UseVisualStyleBackColor = true;
            this.btnCustom3.Click += new System.EventHandler(this.btnCustom3_Click);
            // 
            // btnCustom1
            // 
            this.btnCustom1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCustom1.Location = new System.Drawing.Point(18, 16);
            this.btnCustom1.Name = "btnCustom1";
            this.btnCustom1.Size = new System.Drawing.Size(83, 32);
            this.btnCustom1.TabIndex = 62;
            this.btnCustom1.Text = "自定义1";
            this.btnCustom1.UseVisualStyleBackColor = true;
            this.btnCustom1.Click += new System.EventHandler(this.btnCustom1_Click);
            // 
            // btnGearsRegual
            // 
            this.btnGearsRegual.Location = new System.Drawing.Point(18, 162);
            this.btnGearsRegual.Name = "btnGearsRegual";
            this.btnGearsRegual.Size = new System.Drawing.Size(83, 33);
            this.btnGearsRegual.TabIndex = 61;
            this.btnGearsRegual.Text = "机械手校准";
            this.btnGearsRegual.UseVisualStyleBackColor = false;
            this.btnGearsRegual.Click += new System.EventHandler(this.btnGearsRegual_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label_SpeedR);
            this.groupBox4.Controls.Add(this.Speedright);
            this.groupBox4.Controls.Add(this.label_SpeedL);
            this.groupBox4.Controls.Add(this.Speedleft);
            this.groupBox4.Location = new System.Drawing.Point(738, 234);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(313, 87);
            this.groupBox4.TabIndex = 15;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "速度调整";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 5;
            this.label8.Text = "右侧速度";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 4;
            this.label7.Text = "左侧速度";
            // 
            // label_SpeedR
            // 
            this.label_SpeedR.AutoSize = true;
            this.label_SpeedR.Location = new System.Drawing.Point(290, 59);
            this.label_SpeedR.Name = "label_SpeedR";
            this.label_SpeedR.Size = new System.Drawing.Size(11, 12);
            this.label_SpeedR.TabIndex = 3;
            this.label_SpeedR.Text = "0";
            // 
            // Speedright
            // 
            this.Speedright.LargeChange = 2;
            this.Speedright.Location = new System.Drawing.Point(74, 53);
            this.Speedright.Maximum = 10;
            this.Speedright.Name = "Speedright";
            this.Speedright.Size = new System.Drawing.Size(203, 24);
            this.Speedright.SmallChange = 2;
            this.Speedright.TabIndex = 2;
            this.Speedright.Scroll += new System.Windows.Forms.ScrollEventHandler(this.Speedright_Scroll);
            // 
            // label_SpeedL
            // 
            this.label_SpeedL.AutoSize = true;
            this.label_SpeedL.Location = new System.Drawing.Point(290, 21);
            this.label_SpeedL.Name = "label_SpeedL";
            this.label_SpeedL.Size = new System.Drawing.Size(11, 12);
            this.label_SpeedL.TabIndex = 1;
            this.label_SpeedL.Text = "0";
            // 
            // Speedleft
            // 
            this.Speedleft.LargeChange = 2;
            this.Speedleft.Location = new System.Drawing.Point(74, 17);
            this.Speedleft.Maximum = 10;
            this.Speedleft.Name = "Speedleft";
            this.Speedleft.Size = new System.Drawing.Size(203, 24);
            this.Speedleft.SmallChange = 2;
            this.Speedleft.TabIndex = 0;
            this.Speedleft.Scroll += new System.Windows.Forms.ScrollEventHandler(this.Speedleft_Scroll);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label_radar2);
            this.groupBox3.Controls.Add(this.label_radar1);
            this.groupBox3.Controls.Add(this.RadarPanel);
            this.groupBox3.Location = new System.Drawing.Point(6, 10);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(260, 283);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "雷达";
            // 
            // label_radar2
            // 
            this.label_radar2.AutoSize = true;
            this.label_radar2.Font = new System.Drawing.Font("宋体", 8F);
            this.label_radar2.Location = new System.Drawing.Point(193, 19);
            this.label_radar2.Name = "label_radar2";
            this.label_radar2.Size = new System.Drawing.Size(60, 11);
            this.label_radar2.TabIndex = 15;
            this.label_radar2.Text = "雷达未启动";
            // 
            // label_radar1
            // 
            this.label_radar1.AutoSize = true;
            this.label_radar1.Font = new System.Drawing.Font("宋体", 8F);
            this.label_radar1.Location = new System.Drawing.Point(6, 19);
            this.label_radar1.Name = "label_radar1";
            this.label_radar1.Size = new System.Drawing.Size(60, 11);
            this.label_radar1.TabIndex = 14;
            this.label_radar1.Text = "雷达未启动";
            // 
            // RadarPanel
            // 
            this.RadarPanel.ErrorImage = null;
            this.RadarPanel.InitialImage = ((System.Drawing.Image)(resources.GetObject("RadarPanel.InitialImage")));
            this.RadarPanel.Location = new System.Drawing.Point(6, 17);
            this.RadarPanel.Name = "RadarPanel";
            this.RadarPanel.Size = new System.Drawing.Size(248, 259);
            this.RadarPanel.TabIndex = 13;
            this.RadarPanel.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.Gear8);
            this.groupBox2.Controls.Add(this.Gear7);
            this.groupBox2.Controls.Add(this.Gear6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.Gear5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.Gear4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.Gear3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.Gear2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.Gear1);
            this.groupBox2.Location = new System.Drawing.Point(737, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(314, 218);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "机械手操控";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(273, 193);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 12);
            this.label11.TabIndex = 15;
            this.label11.Text = "舵机8";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(236, 193);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 12);
            this.label10.TabIndex = 14;
            this.label10.Text = "舵机7";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(200, 193);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 12);
            this.label9.TabIndex = 13;
            this.label9.Text = "舵机6";
            // 
            // Gear8
            // 
            this.Gear8.LargeChange = 5;
            this.Gear8.Location = new System.Drawing.Point(275, 17);
            this.Gear8.Name = "Gear8";
            this.Gear8.Size = new System.Drawing.Size(27, 165);
            this.Gear8.TabIndex = 12;
            this.Gear8.Value = 50;
            this.Gear8.Scroll += new System.Windows.Forms.ScrollEventHandler(this.Gear8_Scroll);
            // 
            // Gear7
            // 
            this.Gear7.LargeChange = 5;
            this.Gear7.Location = new System.Drawing.Point(237, 17);
            this.Gear7.Name = "Gear7";
            this.Gear7.Size = new System.Drawing.Size(27, 165);
            this.Gear7.TabIndex = 11;
            this.Gear7.Value = 50;
            this.Gear7.Scroll += new System.Windows.Forms.ScrollEventHandler(this.Gear7_Scroll);
            // 
            // Gear6
            // 
            this.Gear6.LargeChange = 5;
            this.Gear6.Location = new System.Drawing.Point(201, 17);
            this.Gear6.Name = "Gear6";
            this.Gear6.Size = new System.Drawing.Size(27, 165);
            this.Gear6.TabIndex = 10;
            this.Gear6.Value = 50;
            this.Gear6.Scroll += new System.Windows.Forms.ScrollEventHandler(this.Gear6_Scroll);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(163, 193);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "舵机5";
            // 
            // Gear5
            // 
            this.Gear5.LargeChange = 5;
            this.Gear5.Location = new System.Drawing.Point(164, 17);
            this.Gear5.Name = "Gear5";
            this.Gear5.Size = new System.Drawing.Size(27, 165);
            this.Gear5.TabIndex = 8;
            this.Gear5.Value = 50;
            this.Gear5.Scroll += new System.Windows.Forms.ScrollEventHandler(this.Gear5_Scroll);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(125, 193);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "舵机4";
            // 
            // Gear4
            // 
            this.Gear4.LargeChange = 5;
            this.Gear4.Location = new System.Drawing.Point(127, 17);
            this.Gear4.Name = "Gear4";
            this.Gear4.Size = new System.Drawing.Size(27, 165);
            this.Gear4.TabIndex = 6;
            this.Gear4.Value = 50;
            this.Gear4.Scroll += new System.Windows.Forms.ScrollEventHandler(this.Gear4_Scroll);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(87, 193);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "舵机3";
            // 
            // Gear3
            // 
            this.Gear3.LargeChange = 5;
            this.Gear3.Location = new System.Drawing.Point(88, 17);
            this.Gear3.Name = "Gear3";
            this.Gear3.Size = new System.Drawing.Size(27, 165);
            this.Gear3.TabIndex = 4;
            this.Gear3.Value = 50;
            this.Gear3.Scroll += new System.Windows.Forms.ScrollEventHandler(this.Gear3_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 193);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "舵机2";
            // 
            // Gear2
            // 
            this.Gear2.LargeChange = 5;
            this.Gear2.Location = new System.Drawing.Point(47, 17);
            this.Gear2.Name = "Gear2";
            this.Gear2.Size = new System.Drawing.Size(27, 165);
            this.Gear2.TabIndex = 2;
            this.Gear2.Value = 50;
            this.Gear2.Scroll += new System.Windows.Forms.ScrollEventHandler(this.Gear2_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 193);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "舵机1";
            // 
            // Gear1
            // 
            this.Gear1.LargeChange = 5;
            this.Gear1.Location = new System.Drawing.Point(10, 17);
            this.Gear1.Name = "Gear1";
            this.Gear1.Size = new System.Drawing.Size(27, 165);
            this.Gear1.TabIndex = 0;
            this.Gear1.Value = 50;
            this.Gear1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.Gear1_Scroll);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnRightBack);
            this.groupBox1.Controls.Add(this.btnLeftBack);
            this.groupBox1.Controls.Add(this.btnLeftForward);
            this.groupBox1.Controls.Add(this.btnRightForward);
            this.groupBox1.Controls.Add(this.btnSpeaker);
            this.groupBox1.Controls.Add(this.btnLightTurnOff);
            this.groupBox1.Controls.Add(this.buttonStop);
            this.groupBox1.Controls.Add(this.btnRadar);
            this.groupBox1.Controls.Add(this.btnLightTurnOn);
            this.groupBox1.Controls.Add(this.buttonBackward);
            this.groupBox1.Controls.Add(this.buttonRight);
            this.groupBox1.Controls.Add(this.buttonLeft);
            this.groupBox1.Controls.Add(this.buttonForward);
            this.groupBox1.Controls.Add(this.btnOpenVideo);
            this.groupBox1.Location = new System.Drawing.Point(6, 295);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 265);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "操作指令";
            // 
            // btnRightBack
            // 
            this.btnRightBack.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRightBack.BackgroundImage")));
            this.btnRightBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRightBack.Location = new System.Drawing.Point(155, 154);
            this.btnRightBack.Name = "btnRightBack";
            this.btnRightBack.Size = new System.Drawing.Size(55, 55);
            this.btnRightBack.TabIndex = 23;
            this.btnRightBack.UseVisualStyleBackColor = false;
            this.btnRightBack.Click += new System.EventHandler(this.btnRightBack_Click);
            // 
            // btnLeftBack
            // 
            this.btnLeftBack.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLeftBack.BackgroundImage")));
            this.btnLeftBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnLeftBack.Location = new System.Drawing.Point(47, 154);
            this.btnLeftBack.Name = "btnLeftBack";
            this.btnLeftBack.Size = new System.Drawing.Size(55, 55);
            this.btnLeftBack.TabIndex = 22;
            this.btnLeftBack.UseVisualStyleBackColor = false;
            this.btnLeftBack.Click += new System.EventHandler(this.btnLeftBack_Click);
            // 
            // btnLeftForward
            // 
            this.btnLeftForward.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLeftForward.BackgroundImage")));
            this.btnLeftForward.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnLeftForward.Location = new System.Drawing.Point(47, 46);
            this.btnLeftForward.Name = "btnLeftForward";
            this.btnLeftForward.Size = new System.Drawing.Size(55, 55);
            this.btnLeftForward.TabIndex = 21;
            this.btnLeftForward.UseVisualStyleBackColor = false;
            this.btnLeftForward.Click += new System.EventHandler(this.btnLeftForward_Click);
            // 
            // btnRightForward
            // 
            this.btnRightForward.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRightForward.BackgroundImage")));
            this.btnRightForward.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRightForward.Location = new System.Drawing.Point(155, 46);
            this.btnRightForward.Name = "btnRightForward";
            this.btnRightForward.Size = new System.Drawing.Size(55, 55);
            this.btnRightForward.TabIndex = 20;
            this.btnRightForward.UseVisualStyleBackColor = false;
            this.btnRightForward.Click += new System.EventHandler(this.btnRightForward_Click);
            // 
            // btnSpeaker
            // 
            this.btnSpeaker.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSpeaker.BackgroundImage")));
            this.btnSpeaker.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSpeaker.Location = new System.Drawing.Point(89, 214);
            this.btnSpeaker.Name = "btnSpeaker";
            this.btnSpeaker.Size = new System.Drawing.Size(75, 33);
            this.btnSpeaker.TabIndex = 19;
            this.btnSpeaker.UseVisualStyleBackColor = false;
            this.btnSpeaker.Click += new System.EventHandler(this.btnSpeaker_Click);
            // 
            // btnLightTurnOff
            // 
            this.btnLightTurnOff.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLightTurnOff.BackgroundImage")));
            this.btnLightTurnOff.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnLightTurnOff.Location = new System.Drawing.Point(168, 214);
            this.btnLightTurnOff.Name = "btnLightTurnOff";
            this.btnLightTurnOff.Size = new System.Drawing.Size(75, 33);
            this.btnLightTurnOff.TabIndex = 18;
            this.btnLightTurnOff.UseVisualStyleBackColor = false;
            this.btnLightTurnOff.Click += new System.EventHandler(this.btnLightTurnOff_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonStop.BackgroundImage")));
            this.buttonStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonStop.Location = new System.Drawing.Point(101, 100);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(55, 55);
            this.buttonStop.TabIndex = 16;
            this.buttonStop.UseVisualStyleBackColor = false;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // btnRadar
            // 
            this.btnRadar.Location = new System.Drawing.Point(169, 14);
            this.btnRadar.Name = "btnRadar";
            this.btnRadar.Size = new System.Drawing.Size(75, 28);
            this.btnRadar.TabIndex = 15;
            this.btnRadar.Text = "开启雷达";
            this.btnRadar.UseVisualStyleBackColor = false;
            this.btnRadar.Click += new System.EventHandler(this.btnRadar_Click);
            // 
            // btnLightTurnOn
            // 
            this.btnLightTurnOn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLightTurnOn.BackgroundImage")));
            this.btnLightTurnOn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnLightTurnOn.Location = new System.Drawing.Point(10, 214);
            this.btnLightTurnOn.Name = "btnLightTurnOn";
            this.btnLightTurnOn.Size = new System.Drawing.Size(75, 33);
            this.btnLightTurnOn.TabIndex = 17;
            this.btnLightTurnOn.UseVisualStyleBackColor = false;
            this.btnLightTurnOn.Click += new System.EventHandler(this.btnLightTurnOn_Click);
            // 
            // buttonBackward
            // 
            this.buttonBackward.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonBackward.BackgroundImage")));
            this.buttonBackward.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonBackward.Location = new System.Drawing.Point(101, 154);
            this.buttonBackward.Name = "buttonBackward";
            this.buttonBackward.Size = new System.Drawing.Size(55, 55);
            this.buttonBackward.TabIndex = 14;
            this.buttonBackward.UseVisualStyleBackColor = false;
            this.buttonBackward.Click += new System.EventHandler(this.buttonBackward_Click);
            // 
            // buttonRight
            // 
            this.buttonRight.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonRight.BackgroundImage")));
            this.buttonRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonRight.Location = new System.Drawing.Point(155, 100);
            this.buttonRight.Name = "buttonRight";
            this.buttonRight.Size = new System.Drawing.Size(55, 55);
            this.buttonRight.TabIndex = 13;
            this.buttonRight.UseVisualStyleBackColor = false;
            this.buttonRight.Click += new System.EventHandler(this.buttonRight_Click);
            // 
            // buttonLeft
            // 
            this.buttonLeft.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonLeft.BackgroundImage")));
            this.buttonLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonLeft.Location = new System.Drawing.Point(47, 100);
            this.buttonLeft.Name = "buttonLeft";
            this.buttonLeft.Size = new System.Drawing.Size(55, 55);
            this.buttonLeft.TabIndex = 12;
            this.buttonLeft.UseVisualStyleBackColor = false;
            this.buttonLeft.Click += new System.EventHandler(this.buttonLeft_Click);
            // 
            // buttonForward
            // 
            this.buttonForward.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonForward.BackgroundImage")));
            this.buttonForward.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonForward.Location = new System.Drawing.Point(101, 46);
            this.buttonForward.Name = "buttonForward";
            this.buttonForward.Size = new System.Drawing.Size(55, 55);
            this.buttonForward.TabIndex = 11;
            this.buttonForward.UseVisualStyleBackColor = false;
            this.buttonForward.Click += new System.EventHandler(this.buttonForward_Click);
            // 
            // btnOpenVideo
            // 
            this.btnOpenVideo.Location = new System.Drawing.Point(13, 14);
            this.btnOpenVideo.Name = "btnOpenVideo";
            this.btnOpenVideo.Size = new System.Drawing.Size(75, 28);
            this.btnOpenVideo.TabIndex = 10;
            this.btnOpenVideo.Text = "开启视频";
            this.btnOpenVideo.UseVisualStyleBackColor = false;
            this.btnOpenVideo.Click += new System.EventHandler(this.btnOpenVideo_Click);
            // 
            // cameraWindow
            // 
            this.cameraWindow.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.cameraWindow.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cameraWindow.BackgroundImage")));
            this.cameraWindow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cameraWindow.Camera = null;
            this.cameraWindow.Location = new System.Drawing.Point(276, 17);
            this.cameraWindow.Name = "cameraWindow";
            this.cameraWindow.Size = new System.Drawing.Size(449, 352);
            this.cameraWindow.TabIndex = 1;
            this.cameraWindow.Paint += new System.Windows.Forms.PaintEventHandler(this.cameraWindow_Paint);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(1064, 610);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.statusBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Menu = this.mainMenu;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WIFI/蓝牙智能小车操纵平台 正式版V1.1 By  Liuviking      机器人创意工作室       www.wifi-robots.com";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.timer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Systemstatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GroupINFO)).EndInit();
            this.panel.ResumeLayout(false);
            this.GroupBoxControl.ResumeLayout(false);
            this.GroupBoxControl.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RadarPanel)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }


        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CloseFile();
            DestorySocket();
        }


        private void exitFileItem_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }


        private void aboutHelpItem_Click(object sender, System.EventArgs e)
        {
            AboutForm form = new AboutForm();

            form.ShowDialog();
        }

        //初始化Socket连接
        bool ret = false;
        private bool InitWIFISocket(String controlIp,String port)
        {
            this.Systemstatus.Text = "正在尝试连接WIFI板···";

            try
            {

                ips = IPAddress.Parse(controlIp.ToString());
                ipe = new IPEndPoint(ips, Convert.ToInt32(port.ToString()));
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                RobotEngine2.SOCKET = socket;
                RobotEngine2.IPE = ipe;
                ret = RobotEngine2.SocketConnect();

            }
            catch (Exception e)
            {
              
                MessageBox.Show("WIFI初始化失败：" + e.Message, "WIFI初始化失败提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                return ret;
        }


        //销毁socket
        private void DestorySocket()
        {
            try
            {
                socket.Close();
            }
            catch (Exception e1)
            {
            
            }
        }

        private void OpenMJPEG()
        {


            MJPEGStream mjpegSource = new MJPEGStream();
            mjpegSource.VideoSource = CameraIp;
            OpenVideoSource(mjpegSource);


        }


        // 打开视频源
        private void OpenVideoSource(IVideoSource source)
        {

            this.Cursor = Cursors.WaitCursor;
            CloseFile();
            if (detector != null)
            {
                detector.MotionLevelCalculation = true;
            }

            Camera camera = new Camera(source, detector);
            camera.Start();
            cameraWindow.Camera = camera;
            statIndex = statReady = 0;
            timer.Start();

            this.Cursor = Cursors.Default;
        }


        private void CloseFile()
        {
            this.Invoke((EventHandler)(delegate
            {

                this.Systemstatus.Text = "正在关闭视频···";
                this.btnOpenVideo.Enabled = false;

            }));
            
            Camera camera = cameraWindow.Camera;

            if (camera != null)
            {

                cameraWindow.Camera = null;

                camera.SignalToStop();

                camera.WaitForStop();

                camera = null;

               
                if (detector != null)
                    detector.Reset();
            }

            if (writer != null)
            {
                writer.Dispose();
                writer = null;
            }
           
            this.Invoke((EventHandler)(delegate
            {

                this.Systemstatus.Text = "";
                this.btnOpenVideo.Enabled = true;

            }));
        }


        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Camera camera = cameraWindow.Camera;

            if (camera != null)
            {

                statCount[statIndex] = camera.FramesReceived;

                if (++statIndex >= statLength)
                    statIndex = 0;
                if (statReady < statLength)
                    statReady++;

                float fps = 0;


                for (int i = 0; i < statReady; i++)
                {
                    fps += statCount[i];
                }
                fps /= statReady;

                statCount[statIndex] = 0;

                fpsPanel.Text = fps.ToString("F2") + " fps";
                videoRecRate = (double)fps;
            }

           
           
        }


        private void noneMotionItem_Click(object sender, System.EventArgs e)
        {
            detector = null;
            detectorType = 0;
            SetMotionDetector();
        }

        private void detector1MotionItem_Click(object sender, System.EventArgs e)
        {
            detector = new MotionDetector1();
            detectorType = 1;
            SetMotionDetector();
        }


        private void detector2MotionItem_Click(object sender, System.EventArgs e)
        {
            detector = new MotionDetector2();
            detectorType = 2;
            SetMotionDetector();
        }

        private void detector3MotionItem_Click(object sender, System.EventArgs e)
        {
            detector = new MotionDetector3();
            detectorType = 3;
            SetMotionDetector();
        }


        private void detector3OptimizedMotionItem_Click(object sender, System.EventArgs e)
        {
            detector = new MotionDetector3Optimized();
            detectorType = 4;
            SetMotionDetector();
        }


        private void detector4MotionItem_Click(object sender, System.EventArgs e)
        {
            detector = new MotionDetector4();
            detectorType = 5;
            SetMotionDetector();
        }


        private void SetMotionDetector()
        {
            Camera camera = cameraWindow.Camera;


            if (detector != null)
            {
                detector.MotionLevelCalculation = true;
            }


            if (camera != null)
            {
                camera.Lock();
                camera.MotionDetector = detector;


                statIndex = statReady = 0;
                camera.Unlock();
            }
        }


        private void motionItem_Popup(object sender, System.EventArgs e)
        {
            MenuItem[] items = new MenuItem[]
			{
				noneMotionItem, detector1MotionItem,
				detector2MotionItem, detector3MotionItem, detector3OptimizedMotionItem,
				detector4MotionItem
			};

            for (int i = 0; i < items.Length; i++)
            {
                items[i].Checked = (i == detectorType);
            }

        }

    
        private void camera_Alarm(object sender, System.EventArgs e)
        {
           
            intervalsToSave = (int)(5 * (1000 / timer.Interval));
        }

    
        private void camera_NewFrame(object sender, System.EventArgs e)
      
        {
            if (saveOnMotion == true)
            {
                Camera camera = cameraWindow.Camera;
                camera.Lock();
                writer.AddFrame(camera.LastFrame);
                camera.Unlock();
            }
            else
            {
                if (writer != null)
                {
                    writer.Dispose();
                    writer = null;
                }
            
            }
        }
        private string CreateVideoFile()
        {
                DateTime date = DateTime.Now;
                String fileName = String.Format("{0}-{1}-{2} {3}-{4}-{5}.avi",
                    date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
                return fileName;
        }

        private string CreatePictureFile()
        {
            DateTime date = DateTime.Now;
            String fileName = String.Format("{0}-{1}-{2} {3}-{4}-{5}.bmp",
                date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
            return fileName;
        }
       
        private void btnOpenVideo_Click(object sender, EventArgs e)
        {
            if (!MjpgFlag)
            {
                OpenMJPEG();
                MjpgFlag = true;
               
            }
            else
            {

                Thread t;
                t = new Thread(delegate()
                {
                    CloseFile();
                });
                t.Start();
               
                this.cameraWindow.BackgroundImage = Resources.backimage;
                this.btnOpenVideo.Enabled = false;
                MjpgFlag = false;
                saveOnMotion = false;
            }
            this.btnTakePhotos.Enabled= this.btnCaptureVideo.Enabled = this.btnOpenVideo.Enabled;
            this.btnOpenVideo.Text = MjpgFlag ? "关闭视频" : "开启视频";
            this.btnCaptureVideo.Text = saveOnMotion ? "停止录像" : "开始录像";
        }






        //读取INI
        public string ReadIni(string Section, string Ident, string Default)
        {
            Byte[] Buffer = new Byte[65535];
            int bufLen = GetPrivateProfileString(Section, Ident, Default, Buffer, Buffer.GetUpperBound(0), FileName);
            string s = Encoding.GetEncoding(0).GetString(Buffer);
            s = s.Substring(0, bufLen);
            return s.Trim();
        }
        private void GetIni()
        {
            CameraIp = ReadIni("VideoUrl", "videoUrl", "");
            ControlIp = ReadIni("ControlUrl", "controlUrl", "");
            Port = ReadIni("Port", "port", "");


            CMD_Forward = ReadIni("Forward", "forward", "");
            CMD_Backward = ReadIni("Backward", "backward", "");
            CMD_TurnLeft = ReadIni("Left", "left", "");
            CMD_TurnRight = ReadIni("Right", "right", "");
            CMD_Stop = ReadIni("Stop", "stop", "");

            CMD_LeftForward = ReadIni("LeftForward", "leftForward", "");
            CMD_RightForward = ReadIni("RightForward", "rightForward", "");
            CMD_LeftBackward = ReadIni("LeftBackward", "leftBackward", "");
            CMD_RightBackward = ReadIni("RightBackward", "rightBackward", "");


            CMD_TurnOnLight = ReadIni("TurnOnLight", "turnOnLight", "");
            CMD_TurnOffLight = ReadIni("TurnOffLight", "turnOffLight", "");
            CMD_Beep = ReadIni("Speaker", "speaker", "");

            btCom = ReadIni("BTCOM", "btcom", "");
            btBaudrate = ReadIni("BTBaudrate", "btbaudrate", "");

            AutoSetScreen = ReadIni("AutoSetScreen", "autoSetScreen", "");

        }
        

        private void SYSConfig_Click(object sender, EventArgs e)
        {
            Config config = new Config();
            config.ShowDialog();
        }

   

        private void panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

         //   ButtonColorInit();
            GetIni();
          //  InitRadar();
          //  InitDataCallBack();
          //  GearsInit();
          //  CustomButtonInit();
          //  ScreenInit();
 

        }
        private void InitDataCallBack()
        {

          //  RobotEngine2.Setcallbackvalue += new WifiRobotCMDEngineV2.SetCallBackDataValue(DataCallBack);//注册数据回调函数
        }

        private void ScreenInit()
        {
            if (AutoSetScreen == "1")
            {
                vga.ChangeRes(1280, 1024, 60, 32);
            }
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {
            if (0 == controlType) return;
            //开启WIFI模式
            if (comm.IsOpen)
            {
                while (Listening) Application.DoEvents();
                //打开时点击，则关闭蓝牙串口
                comm.Close();
            }
            controlType= InitWIFISocket(ControlIp, Port)? 0:2;
            if (0 == controlType)
            {
                //this.menuItem5.Checked = true;
                //this.menuItem6.Checked = false;
                //this.btnRadar.Enabled = this.menuItem5.Checked;
                //InitHeartPackage();
                //this.Systemstatus.Text = "WIFI模式";
            }
        }

        private void menuItem6_Click(object sender, EventArgs e)
        {
            if (1 == controlType) return;
            //开启蓝牙模式
           
            if (comm.IsOpen)
            {
                return;
            }
            else
            {
                if (btCom == null || btCom=="")
                {
                    MessageBox.Show("请先在系统设置中设置蓝牙端口号！","蓝牙端口号设置提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                   
                    return;
                }
  
                
                comm.PortName = btCom;
                comm.BaudRate = int.Parse(btBaudrate);
                comm.DataReceived += comm_DataReceived;
               
                try
                {
                    comm.Open();
                }
                catch (Exception ex)
                {
                    //捕获到异常信息，创建一个新的comm对象，之前的不能用了。
                    comm = new SerialPort();
                    MessageBox.Show("蓝牙模式出错！"+ex.Message,"蓝牙模式设置提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }

                controlType = 1;
               
                this.menuItem5.Checked = false;
                this.menuItem6.Checked = true;

                this.Systemstatus.Text = "蓝牙模式";
            }


        }

        //串口监听事件
        void comm_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //int n = comm.BytesToRead; 
            //byte[] buf = new byte[n]; 
            //received_count += n;
            //comm.Read(buf, 0, n);  
            //builder.Clear();    
            //this.Invoke((EventHandler)(delegate
            //{
                  
            //    foreach (byte b in buf)
            //    {
            //        builder.Append(b.ToString("X2") + " ");
            //    }

            //    this.txtCommandPanel.AppendText(builder.ToString()+"\r\n");  
            //    labelGetCount.Text = send_count.ToString() + "/" + received_count.ToString();
            //}));
        }



        //初始化雷达
        private void InitRadar()
        {
          //this.btnRadar.Enabled = this.menuItem5.Checked;
          //MyRadar = new Radar(this.RadarPanel.Width);
          //RadarPanel.Image = MyRadar.Image;
          //MyRadar.ImageUpdate += new ImageUpdateHandler(MyRadar_ImageUpdate);
          //MyRadar.DrawScanInterval = 10;
          //MyRadar.DrawScanLine = true;
        }
       


        void MyRadar_ImageUpdate(object sender, ImageUpdateEventArgs e)
        {
           
            RadarPanel.Image = e.Image;
 
        }
        
        private void GetTarget(int rad, int el)
        {
            item1 = new SquareRadarItem(1, 8, rad, el);
            this.label_radar1.Text = "弧度：" + rad;
            this.label_radar2.Text = "高度：" + el;
            MyRadar.AddItem(item1);
        }




        private void Speedright_Scroll(object sender, ScrollEventArgs e)
        {
            this.label_SpeedR.Text = this.Speedright.Value.ToString();
            Application.DoEvents();
            byte[] Speedright_data = RobotEngine2.CreateData(0X02, 0X01, Convert.ToByte(this.Speedright.Value));//舵机数据打包第一个参数代表舵机，第二个代表哪个舵机，第三个代表转动角度值
            RobotEngine2.SendCMD(controlType, Speedright_data, comm);
        }

        private void Speedleft_Scroll(object sender, ScrollEventArgs e)
        {
            this.label_SpeedL.Text = this.Speedleft.Value.ToString();
            Application.DoEvents();
            byte[] Speedleft_data = RobotEngine2.CreateData(0X02, 0X02, Convert.ToByte(this.Speedleft.Value));//舵机数据打包第一个参数代表舵机，第二个代表哪个舵机，第三个代表转动角度值
            RobotEngine2.SendCMD(controlType, Speedleft_data,comm);
        }

        private void buttonForward_Click(object sender, EventArgs e)
        {
          
           
            if (Send_status)
            {
                RobotEngine2.SendCMD(controlType, CMD_Forward, comm);
                Send_status = false;
            }
            
        }

        private void buttonLeft_Click(object sender, EventArgs e)
        {
            if (Send_status)
            {
                RobotEngine2.SendCMD(controlType, CMD_TurnLeft, comm);
                Send_status = false;
            }
        }

        private void buttonRight_Click(object sender, EventArgs e)
        {
            if (Send_status)
            {
                RobotEngine2.SendCMD(controlType, CMD_TurnRight, comm);
                Send_status = false;
            }
        }

        private void buttonBackward_Click(object sender, EventArgs e)
        {
            if (Send_status)
            {
                RobotEngine2.SendCMD(controlType, CMD_Backward, comm);
                Send_status = false;
            }
        }


        private void buttonStop_Click(object sender, EventArgs e)
        {
            Send_status = true;
            RobotEngine2.SendCMD(controlType, CMD_Stop, comm);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            //send_count++;
            //RobotEngine2.SendCMD(controlType, this.txtCommandPanel.Text.Trim(), comm);
            //labelGetCount.Text = send_count.ToString() + "/" + received_count.ToString();
        }
      
        private void Gear1_Scroll(object sender, ScrollEventArgs e)
        {
            Application.DoEvents();
           
            byte[] Gear1_data = RobotEngine2.CreateData(0X01, 0X01, Convert.ToByte(this.Gear1.Value));//舵机数据打包第一个参数代表舵机，第二个代表哪个舵机，第三个代表转动角度值
            this.txtCommandPanel.AppendText("舵机1: "+Gear1_data[3].ToString() + "\r\n");
            RobotEngine2.SendCMD(controlType,Gear1_data,comm);
        }

        private void Gear2_Scroll(object sender, ScrollEventArgs e)
        {
            Application.DoEvents();
           byte[] Gear2_data = RobotEngine2.CreateData(0X01, 0X02, Convert.ToByte(this.Gear2.Value));//舵机数据打包
           this.txtCommandPanel.AppendText("舵机2: " + Gear2_data[3].ToString() + "\r\n");
           RobotEngine2.SendCMD(controlType, Gear2_data,comm);
           
        }

        private void Gear3_Scroll(object sender, ScrollEventArgs e)
        {
            Application.DoEvents();
            byte[] Gear3_data = RobotEngine2.CreateData(0X01, 0X03, Convert.ToByte(this.Gear3.Value));//舵机数据打包
            this.txtCommandPanel.AppendText("舵机3: " + Gear3_data[3].ToString() + "\r\n");
            RobotEngine2.SendCMD(controlType, Gear3_data, comm);
           
        }

        private void Gear4_Scroll(object sender, ScrollEventArgs e)
        {
            Application.DoEvents();
            byte[] Gear4_data = RobotEngine2.CreateData(0X01, 0X04, Convert.ToByte(this.Gear4.Value));//舵机数据打包
            this.txtCommandPanel.AppendText("舵机4: " + Gear4_data[3].ToString() + "\r\n");
            RobotEngine2.SendCMD(controlType, Gear4_data, comm);
          

        }

        private void Gear5_Scroll(object sender, ScrollEventArgs e)
        {
            Application.DoEvents();
            byte[] Gear5_data = RobotEngine2.CreateData(0X01, 0X05, Convert.ToByte(this.Gear5.Value));//舵机数据打包
            this.txtCommandPanel.AppendText("舵机5: " + Gear5_data[3].ToString() + "\r\n");
            RobotEngine2.SendCMD(controlType, Gear5_data,comm);

           
        }



        private void Gear6_Scroll(object sender, ScrollEventArgs e)
        {
            Application.DoEvents();
            byte[] Gear6_data = RobotEngine2.CreateData(0X01, 0X06, Convert.ToByte(this.Gear6.Value));//舵机数据打包
            this.txtCommandPanel.AppendText("舵机6: " + Gear6_data[3].ToString() + "\r\n");
            RobotEngine2.SendCMD(controlType, Gear6_data,comm);
 
            
        }

        private void Gear7_Scroll(object sender, ScrollEventArgs e)
        {
            Application.DoEvents();
            byte[] Gear7_data = RobotEngine2.CreateData(0X01, 0X07, Convert.ToByte(this.Gear7.Value));//舵机数据打包
            this.txtCommandPanel.AppendText("舵机7: " + Gear7_data[3].ToString() + "\r\n");
            RobotEngine2.SendCMD(controlType, Gear7_data, comm);

           
        }

        private void Gear8_Scroll(object sender, ScrollEventArgs e)
        {
            Application.DoEvents();
            byte[] Gear8_data = RobotEngine2.CreateData(0X01, 0X08, Convert.ToByte(this.Gear8.Value));//舵机数据打包
            this.txtCommandPanel.AppendText("舵机8: " + Gear8_data[3].ToString() + "\r\n");
            RobotEngine2.SendCMD(controlType, Gear8_data,comm);
        }

       
        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtCommandPanel.Text = "";
            builder.Clear();
        }

            private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
            {
                vga.FuYuan();
                Process.GetCurrentProcess().Kill();
                Application.Exit();
               
            }

            private void MainForm_KeyDown(object sender, KeyEventArgs e)
            {
                if (controlType == 3)
                    return;
                if (e.KeyCode == Keys.W)
                {
                   
                    buttonForward.PerformClick();
                    buttonForward.BackColor = Color.Blue;

                }
                else if (e.KeyCode == Keys.S)
                {
                  
                    buttonBackward.PerformClick();
                    buttonBackward.BackColor = Color.Blue;
                }
                else if (e.KeyCode == Keys.A)
                {
                    buttonLeft.PerformClick();
                    buttonLeft.BackColor = Color.Blue;
                }
                else if (e.KeyCode == Keys.D)
                {
                   
                    buttonRight.PerformClick();
                    buttonRight.BackColor = Color.Blue;
                }
                else if (e.KeyCode == Keys.U)
                {

                    btnCustom1.PerformClick();
                    btnCustom1.BackColor = Color.Green;
                   
                }
                else if (e.KeyCode == Keys.I)
                {
                    btnCustom3.PerformClick();
                    btnCustom3.BackColor = Color.Green;
                }
                else if (e.KeyCode == Keys.O)
                {
                    btnCustom5.PerformClick();
                    btnCustom5.BackColor = Color.Green;
                }
                else if (e.KeyCode == Keys.J)
                {
                    btnCustom2.PerformClick();
                    btnCustom2.BackColor = Color.Green;
                }
                else if (e.KeyCode == Keys.K)
                {
                    btnCustom4.PerformClick();
                    btnCustom4.BackColor = Color.Green;
                }
                else if (e.KeyCode == Keys.L)
                {
                    btnCustom6.PerformClick();
                    btnCustom6.BackColor = Color.Green;
                }
                else if (e.KeyCode == Keys.M)
                {
                    btnCustom7.PerformClick();
                    btnCustom7.BackColor = Color.Green;
                }
                else if (e.KeyCode == Keys.Oemcomma)
                {
                    btnCustom9.PerformClick();
                    btnCustom9.BackColor = Color.Green;
                }
                else if (e.KeyCode == Keys.OemPeriod)
                {
                    btnCustom11.PerformClick();
                    btnCustom11.BackColor = Color.Green;
                }
                else if (e.KeyCode == Keys.P)
                {
                    btnCustom8.PerformClick();
                    btnCustom8.BackColor = Color.Green;
                }
                else if (e.KeyCode == Keys.OemOpenBrackets)
                {
                    btnCustom10.PerformClick();
                    btnCustom10.BackColor = Color.Green;

                }
                else if (e.KeyCode == Keys.Oem6)
                {
                    btnCustom12.PerformClick();
                    btnCustom12.BackColor = Color.Green;
                }
                else if (e.KeyCode == Keys.Q)
                {
                    btnLeftForward.PerformClick();
                    btnLeftForward.BackColor = Color.Blue;
                }
                else if (e.KeyCode == Keys.E)
                {
                    btnRightForward.PerformClick();
                    btnRightForward.BackColor = Color.Blue;
                }
                else if (e.KeyCode == Keys.Z)
                {
                    btnLeftBack.PerformClick();
                    btnLeftBack.BackColor = Color.Blue;
                }
                else if (e.KeyCode == Keys.C)
                {
                    btnRightBack.PerformClick();
                    btnRightBack.BackColor = Color.Blue;
                }
                else if (e.KeyCode == Keys.Space)
                {
                    buttonStop.PerformClick();
                    buttonStop.BackColor = Color.Blue;
                }
                else if (e.KeyCode == Keys.F)
                {
                    btnLightTurnOn.PerformClick();
                    btnLightTurnOn.BackColor = Color.Blue;
                }
                else if (e.KeyCode == Keys.R)
                {
                    btnSpeaker.PerformClick();
                    btnSpeaker.BackColor = Color.Blue;
                }
                else if (e.KeyCode == Keys.T)
                {
                    btnLightTurnOff.PerformClick();
                    btnLightTurnOff.BackColor = Color.Blue;
                }

              
               
       
               
            }

            private void MainForm_KeyUp(object sender, KeyEventArgs e)
            {
                if (this.txtCommandPanel.Focused)
                { 
                    return; 
                }
                buttonStop.PerformClick();
                buttonStop.BackColor= buttonForward.BackColor = buttonBackward.BackColor = buttonLeft.BackColor = buttonRight.BackColor = btnLeftForward.BackColor = btnRightForward.BackColor = btnLeftBack.BackColor = btnRightBack.BackColor = Color.LightSkyBlue;
                btnCustom1.BackColor = btnCustom2.BackColor = btnCustom3.BackColor = btnCustom4.BackColor = btnCustom5.BackColor = btnCustom6.BackColor = btnCustom7.BackColor = btnCustom8.BackColor = btnCustom9.BackColor = btnCustom10.BackColor = btnCustom11.BackColor = btnCustom12.BackColor = Color.LightGreen;
                btnLightTurnOn.BackColor = btnLightTurnOff.BackColor = btnSpeaker.BackColor = SystemColors.Control;
            }


 
            //雷达接收
            bool RadarStatus = false;
            private void btnRadar_Click(object sender, EventArgs e)
            {
               
              
                if (!RadarStatus)
                {
                    RadarStatus = true;
                    GetTarget(0, 0);
                }
                else
                {
                    RadarStatus = false;
                  
                }
                
                this.btnRadar.Text=(RadarStatus==true) ? "关闭雷达" : "开启雷达";
                this.label_radar2.Text = this.label_radar1.Text = (RadarStatus == true) ? "0" : "雷达未启动";
            }

            void DataCallBack(byte[] CallbackDataValue)
            {

                ///*数据回传回调函数
                // * 当有数据从下位机到达上位机后，将执行到此函数
                // * CallbackDataValue[0]为类型位
                // * CallbackDataValue[1]为命令位
                // * CallbackDataValue[2]为数据位
                // * 包头包尾已经去掉
                // * 
                // * 协议说明
                // * 0x03   雷达
                // * 0x89   拍照
                // * 0x60   电量
                // * 0x61   湿度
                // * 0x62   辐射
                // * 0x63   温度
                // * 
                // * 数据位不允许超过10,步长1
                // * 
                //*/
          
                //    foreach (byte b in CallbackDataValue)
                //    {
                //        builder.Append(b.ToString("X2") + " ");

                //    }
                //    DelegateUI(builder.ToString());
                //    builder.Clear();
                

                
                //if (CallbackDataValue[0] == 0x03)
                //{
                //    this.Invoke((EventHandler)(delegate
                //    {

                //        try
                //        {

                //            if (Int32.Parse(CallbackDataValue[1].ToString()) == 0)//雷达数据：弧度值
                //            {
                //                x_data = 0;
                //            }
                //            else
                //            {
                //                x_data = Int32.Parse(CallbackDataValue[1].ToString());
                //            }

                //            if (Int32.Parse(CallbackDataValue[2].ToString()) == 0)//雷达数据：距离值
                //            {
                //                y_data = 0;
                //            }
                //            else
                //            {
                //                y_data = Int32.Parse(CallbackDataValue[2].ToString());
                //            }

                //         //   RobotEngine2.WR_DEBUG("RADER", "x_data is:" + x_data.ToString() + ";++y_data is :" + y_data.ToString());

                //        }
                //        catch
                //        {
                //            x_data = 0;
                //            y_data = 0;
                //        }
                //        if (RadarStatus)
                //        {
                //            GetTarget(x_data, y_data);
                //        }

                //    }));
                //}
                
                //else if (CallbackDataValue[0] == 0x99)
                //{
                //    //标志位为0x99
                //    //CallbackDataValue[1]即为数据内容。
                //   //请自行拓展
                //}
               
                //else
                //{

                //    RobotEngine2.WR_DEBUG("DATACALLBACK", "+++++++DATACALLBACK NOTHING+++++++++");
                //}

            }  
       

     

            private void btnGearsRegual_Click(object sender, EventArgs e)
            {
                GearRegul gr = new GearRegul();
                gr.ChangeGearSetting += new motion.GearRegul.ChangeSetting(ChangeGearCallBack);  
                gr.ShowDialog();
                
            }
            void ChangeGearCallBack(bool topmost)
            {
                GearsInit();
            }

 

            private void ButtonColorInit()
            {
               // buttonForward.BackColor = buttonBackward.BackColor = buttonLeft.BackColor = buttonRight.BackColor = btnLeftForward.BackColor = btnRightForward.BackColor = btnLeftBack.BackColor = btnRightBack.BackColor = Color.LightSkyBlue; ;
               // buttonStop.BackColor = Color.LightSkyBlue;
               // btnCustom1.BackColor = btnCustom2.BackColor = btnCustom3.BackColor = btnCustom4.BackColor = btnCustom5.BackColor = btnCustom6.BackColor = btnCustom7.BackColor = btnCustom8.BackColor = btnCustom9.BackColor = btnCustom10.BackColor = btnCustom11.BackColor = btnCustom12.BackColor = Color.LightGreen;
               //btnTakePhotos.BackColor= btnGearsRegual.BackColor = btnGearStandby.BackColor = btnCaptureVideo.BackColor = Color.LightGreen;
            }


         private void DelegateUI(string s)
         {
          //  this.Invoke((EventHandler)(delegate
            //{
             
                  //  this.txtCommandPanel.AppendText(s.ToString()+"\r\n");
           // }
         //   ));
//
         }

            private void btnDrill_Click(object sender, EventArgs e)
            {

            }

             //机械手控制台初始化
            public void GearsInit()
            {
                //读取上限
                //this.Gear1.Minimum =int.Parse( ReadIni("Gear1UP", "gear1up", ""));
                //this.Gear2.Minimum =int.Parse( ReadIni("Gear2UP", "gear2up", ""));
                //this.Gear3.Minimum = int.Parse( ReadIni("Gear3UP", "gear3up", ""));
                //this.Gear4.Minimum = int.Parse( ReadIni("Gear4UP", "gear4up", ""));
                //this.Gear5.Minimum = int.Parse( ReadIni("Gear5UP", "gear5up", ""));
                //this.Gear6.Minimum =int.Parse(  ReadIni("Gear6UP", "gear6up", ""));
                //this.Gear7.Minimum = int.Parse( ReadIni("Gear7UP", "gear7up", ""));
                //this.Gear8.Minimum = int.Parse( ReadIni("Gear8UP", "gear8up", ""));


                ////读取默认
                //this.Gear1.Value = int.Parse(ReadIni("Gear1Default", "gear1default", ""));
                //this.Gear2.Value =  int.Parse(ReadIni("Gear2Default", "gear2default", ""));
                //this.Gear3.Value =  int.Parse(ReadIni("Gear3Default", "gear3default", ""));
                //this.Gear4.Value = int.Parse( ReadIni("Gear4Default", "gear4default", ""));
                //this.Gear5.Value = int.Parse( ReadIni("Gear5Default", "gear5default", ""));
                //this.Gear6.Value =  int.Parse(ReadIni("Gear6Default", "gear6default", ""));
                //this.Gear7.Value = int.Parse( ReadIni("Gear7Default", "gear7default", ""));
                //this.Gear8.Value =  int.Parse(ReadIni("Gear8Default", "gear8default", ""));


                ////读取下限
                //this.Gear1.Maximum = int.Parse(ReadIni("Gear1Down", "gear1down", "")) ;
                //this.Gear2.Maximum = int.Parse(ReadIni("Gear2Down", "gear2down", "")) ;
                //this.Gear3.Maximum = int.Parse(ReadIni("Gear3Down", "gear3down", "")) ;
                //this.Gear4.Maximum = int.Parse(ReadIni("Gear4Down", "gear4down", "")) ;
                //this.Gear5.Maximum = int.Parse(ReadIni("Gear5Down", "gear5down", "")) ;
                //this.Gear6.Maximum = int.Parse(ReadIni("Gear6Down", "gear6down", "")) ;
                //this.Gear7.Maximum = int.Parse(ReadIni("Gear7Down", "gear7down", "")) ;
                //this.Gear8.Maximum = int.Parse(ReadIni("Gear8Down", "gear8down", "")) ;

            }

            private void button1_Click(object sender, EventArgs e)
            {


            }


            private void cameraWindow_Paint(object sender, PaintEventArgs e)
            {
                RobotEngine2.DrawSignal(signalType,e.Graphics,this.cameraWindow.Width,this.cameraWindow.Height,2);
            }

            private void menuItem7_Popup(object sender, EventArgs e)
            {
                MenuItem[] items = new MenuItem[]
			    {
				   nonesignal,signal1,signal2
			    };

                for (int i = 0; i < items.Length; i++)
                {
                    items[i].Checked = (i == signalType);
                }
            }

            private void nonesignal_Click(object sender, EventArgs e)
            {
                signalType = 0;
            }

            private void signal1_Click(object sender, EventArgs e)
            {
                signalType = 1;
            }

            private void signal2_Click(object sender, EventArgs e)
            {
                signalType = 2;
            }
              
            //自定义按钮初始化
            private string CMD_Custom1;
            private string CMD_Custom2;
            private string CMD_Custom3;
            private string CMD_Custom4;
            private string CMD_Custom5;
            private string CMD_Custom6;
            private string CMD_Custom7;
            private string CMD_Custom8;
            private string CMD_Custom9;
            private string CMD_Custom10;
            private string CMD_Custom11;
            private string CMD_Custom12;

            private void CustomButtonInit()
            {
                this.btnCustom1.Text = ReadIni("LabelCustomCMD", "lbCustom1", "自定义1");
                CMD_Custom1 = ReadIni("CustomCMD", "Custombox1", "");

                this.btnCustom2.Text = ReadIni("LabelCustomCMD", "lbCustom2", "自定义2");
                CMD_Custom2 = ReadIni("CustomCMD", "Custombox2", "");


                this.btnCustom3.Text = ReadIni("LabelCustomCMD", "lbCustom3", "自定义3");
                CMD_Custom3 = ReadIni("CustomCMD", "Custombox3", "");


                this.btnCustom4.Text = ReadIni("LabelCustomCMD", "lbCustom4", "自定义4");
                CMD_Custom4 = ReadIni("CustomCMD", "Custombox4", "");


                this.btnCustom5.Text = ReadIni("LabelCustomCMD", "lbCustom5", "自定义5");
                CMD_Custom5 = ReadIni("CustomCMD", "Custombox5", "");


                this.btnCustom6.Text = ReadIni("LabelCustomCMD", "lbCustom6", "自定义6");
                CMD_Custom6 = ReadIni("CustomCMD", "Custombox6", "");


                this.btnCustom7.Text = ReadIni("LabelCustomCMD", "lbCustom7", "自定义7");
                CMD_Custom7 = ReadIni("CustomCMD", "Custombox7", "");


                this.btnCustom8.Text = ReadIni("LabelCustomCMD", "lbCustom8", "自定义8");
                CMD_Custom8 = ReadIni("CustomCMD", "Custombox8", "");

                this.btnCustom9.Text = ReadIni("LabelCustomCMD", "lbCustom9", "自定义9");
                CMD_Custom9 = ReadIni("CustomCMD", "Custombox9", "");


                this.btnCustom10.Text = ReadIni("LabelCustomCMD", "lbCustom10", "自定义10");
                CMD_Custom10 = ReadIni("CustomCMD", "Custombox10", "");


                this.btnCustom11.Text = ReadIni("LabelCustomCMD", "lbCustom11", "自定义11");
                CMD_Custom11 = ReadIni("CustomCMD", "Custombox11", "");

                this.btnCustom12.Text = ReadIni("LabelCustomCMD", "lbCustom12", "自定义12");
                CMD_Custom12 = ReadIni("CustomCMD", "Custombox12", "");
            
            
            
            }

            private void btnCustom1_Click(object sender, EventArgs e)
            {

                RobotEngine2.SendCMD(controlType,CMD_Custom1,comm);

            }

            private void btnCustom2_Click(object sender, EventArgs e)
            {
                RobotEngine2.SendCMD(controlType, CMD_Custom2, comm);
            }

            private void btnCustom3_Click(object sender, EventArgs e)
            {
                RobotEngine2.SendCMD(controlType, CMD_Custom3, comm);
            }

            private void btnCustom4_Click(object sender, EventArgs e)
            {
                RobotEngine2.SendCMD(controlType, CMD_Custom4, comm);
            }

            private void btnCustom5_Click(object sender, EventArgs e)
            {
                RobotEngine2.SendCMD(controlType, CMD_Custom5, comm);
            }

            private void btnCustom6_Click(object sender, EventArgs e)
            {
                RobotEngine2.SendCMD(controlType, CMD_Custom6, comm);
            }

            private void btnCustom7_Click(object sender, EventArgs e)
            {
                RobotEngine2.SendCMD(controlType, CMD_Custom7, comm);
            }

            private void btnCustom8_Click(object sender, EventArgs e)
            {
                RobotEngine2.SendCMD(controlType, CMD_Custom8, comm);
            }

            private void btnCustom9_Click(object sender, EventArgs e)
            {
                RobotEngine2.SendCMD(controlType, CMD_Custom9, comm);
            }

            private void btnCustom10_Click(object sender, EventArgs e)
            {
                RobotEngine2.SendCMD(controlType, CMD_Custom10, comm);
            }

            private void btnCustom11_Click(object sender, EventArgs e)
            {
                RobotEngine2.SendCMD(controlType, CMD_Custom11, comm);
            }

            private void btnCustom12_Click(object sender, EventArgs e)
            {
                RobotEngine2.SendCMD(controlType, CMD_Custom12, comm);
            }

            private void btnGearStandby_Click(object sender, EventArgs e)
            {

                Thread t;
                t = new Thread(delegate()
                {
                    for (int i = 0; i < 8; i++)
                    {
                        GearStandby(i);
                        Thread.Sleep(500);
                    }
                   
                });
                t.Start();
               
            }

            private void btnLightTurnOn_Click(object sender, EventArgs e)
            {
                RobotEngine2.SendCMD(controlType, CMD_TurnOnLight, comm);
            }

            private void btnLightTurnOff_Click(object sender, EventArgs e)
            {
                RobotEngine2.SendCMD(controlType, CMD_TurnOffLight, comm);
            }

            private void btnSpeaker_Click(object sender, EventArgs e)
            {
                RobotEngine2.SendCMD(controlType, CMD_Beep, comm);
            }

            private void btnCaptureVideo_Click(object sender, EventArgs e)
            {
              
                //if (!saveOnMotion)
                //{
                   
                //    if (dlg.ShowDialog() == DialogResult.OK)
                //    {
                //        Camera camera = cameraWindow.Camera;
                //        aviManager = new AviManager(VideoPath+CreateVideoFile(), false);
                //        double Rate = dlg.Rate == 0.0 ? videoRecRate : dlg.Rate;
                //        camera.Lock();
                //        aviStream = aviManager.AddVideoStream(false, Rate, camera.LastFrame);
                //        camera.Unlock();

                //        StartVideoRecorder();
                //        saveOnMotion = true;

                //    }
                   
                   
                //}
                //else
                //{
                //    saveOnMotion = false;
                //}

                //btnCaptureVideo.Text = saveOnMotion ? "停止录像" : "开始录像";
            }

            private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
            {
                if (AutoSetScreen == "1")
                {
                    vga.FuYuan();
                }
            }

            
            private void  GearStandby(int i)
            {
                i++;
                DelegateUI("正在归位舵机"+i);
                string Parm1="Gear" + i.ToString() + "Default";
                string Parm2="gear" + i.ToString() + "default";
                RobotEngine2.SendCMD(controlType, RobotEngine2.CreateData(0x01, Byte.Parse(i.ToString()), Byte.Parse(ReadIni(Parm1, Parm2, ""))), comm);
                if (i == 8)
                {
                    i = 0;
                    DelegateUI("所有舵机归位完毕！");
                }
            }

                

                //心跳包
            private void InitHeartPackage()
            {
                Thread HThread = new Thread(HeartPackage);
                HThread.IsBackground = true;
                HThread.Start();
            }

            private void HeartPackage()
            {
                while (true)
                {
                  
                    //RobotEngine2.SendHeartCMD(controlType,comm);
                    //Thread.Sleep(10000);
                    
                }
            }

            Thread VideoRecordThread;
            private void StartVideoRecorder()
            {
                VideoRecordThread = new Thread(SetVideoRecorder);
                VideoRecordThread.IsBackground = true;
                VideoRecordThread.Start();
            }
            private void SetVideoRecorder()
            {
                Camera camera = cameraWindow.Camera;
                while (saveOnMotion)
                {
                    try
                    {
                        
                        camera.Lock();
                        aviStream.AddFrame(cameraWindow.Camera.LastFrame);
                        camera.Unlock();
                        Thread.Sleep(500);
                 
                    }
                    catch
                    {

                    }
                }
                if (aviManager != null)
                {
                   
                    aviManager.Close();
                }

                VideoRecordThread.Abort();
            }

            private void btnLeftForward_Click(object sender, EventArgs e)
            {
                if (Send_status)
                {
                    RobotEngine2.SendCMD(controlType, CMD_LeftForward, comm);
                    Send_status = false;
                }
            }

            private void btnRightForward_Click(object sender, EventArgs e)
            {
                if (Send_status)
                {
                    RobotEngine2.SendCMD(controlType, CMD_RightForward, comm);
                    Send_status = false;
                }
            }

            private void btnLeftBack_Click(object sender, EventArgs e)
            {
                if (Send_status)
                {
                    RobotEngine2.SendCMD(controlType, CMD_LeftBackward, comm);
                    Send_status = false;
                }
            }

            private void btnRightBack_Click(object sender, EventArgs e)
            {
                if (Send_status)
                {
                    RobotEngine2.SendCMD(controlType, CMD_RightBackward, comm);
                    Send_status = false;
                }
            }

            private void btnTakePhotos_Click(object sender, EventArgs e)
            {

                RobotEngine2.TakePhoto(cameraWindow.Camera.LastFrame, ImagePath, CreatePictureFile());
               
            }


          
    }

    
}
