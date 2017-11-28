using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using System.IO.Ports;

namespace motion
{
	/// <summary>
	/// Summary description for URLForm.
	/// </summary>

    public class Config : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;

        private string url;
        private string FileName;
        private GroupBox groupBox1;
        private Label label15;
        private TextBox txtTurnOffLight;
        private Label label10;
        private TextBox txtTurnOnLight;
        private Label label7;
        private TextBox txtStop;
        private TextBox txtBackward;
        private TextBox txtRight;
        private TextBox txtLeft;
        private TextBox txtForward;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private GroupBox groupBox2;
        private Label label11;
        private TextBox txtPort;
        private Label label2;
        private Label label1;
        private GroupBox groupBox3;
        private ComboBox comboPortName;
        private Label label13;
        private ComboBox comboBaudrate;
        private Label label14;
        private TextBox ControlURL;
        private TextBox VideoURL;
        private Label label12;
        private TextBox txtSpeaker;
        private GroupBox groupBox4;
        private Label Custom1;
        private TextBox CustomBox1;
        private Label Custom11;
        private TextBox CustomBox11;
        private Label Custom10;
        private TextBox CustomBox10;
        private Label Custom9;
        private TextBox CustomBox9;
        private Label Custom8;
        private TextBox CustomBox8;
        private Label Custom7;
        private TextBox CustomBox7;
        private Label Custom6;
        private TextBox CustomBox6;
        private Label Custom5;
        private TextBox CustomBox5;
        private Label Custom4;
        private TextBox CustomBox4;
        private Label Custom3;
        private TextBox CustomBox3;
        private Label Custom2;
        private TextBox CustomBox2;
        private Label Custom12;
        private TextBox CustomBox12;
        private GroupBox groupBox5;
        private CheckBox checkBox1;
        private TextBox txtRForward;
        private Label label9;
        private TextBox txtLForward;
        private Label label8;
        private TextBox txtRBackward;
        private Label label17;
        private TextBox txtLBackward;
        private Label label16;
        private IContainer components;

        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);
        private SerialPort comm = new SerialPort();
        int AutoSetScreen;
        public Config()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }


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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Config));
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtRBackward = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtLBackward = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtRForward = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtLForward = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtSpeaker = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtTurnOffLight = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtTurnOnLight = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtStop = new System.Windows.Forms.TextBox();
            this.txtBackward = new System.Windows.Forms.TextBox();
            this.txtRight = new System.Windows.Forms.TextBox();
            this.txtLeft = new System.Windows.Forms.TextBox();
            this.txtForward = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ControlURL = new System.Windows.Forms.TextBox();
            this.VideoURL = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboBaudrate = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.comboPortName = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.Custom12 = new System.Windows.Forms.Label();
            this.CustomBox12 = new System.Windows.Forms.TextBox();
            this.Custom11 = new System.Windows.Forms.Label();
            this.CustomBox11 = new System.Windows.Forms.TextBox();
            this.Custom10 = new System.Windows.Forms.Label();
            this.CustomBox10 = new System.Windows.Forms.TextBox();
            this.Custom9 = new System.Windows.Forms.Label();
            this.CustomBox9 = new System.Windows.Forms.TextBox();
            this.Custom8 = new System.Windows.Forms.Label();
            this.CustomBox8 = new System.Windows.Forms.TextBox();
            this.Custom7 = new System.Windows.Forms.Label();
            this.CustomBox7 = new System.Windows.Forms.TextBox();
            this.Custom6 = new System.Windows.Forms.Label();
            this.CustomBox6 = new System.Windows.Forms.TextBox();
            this.Custom5 = new System.Windows.Forms.Label();
            this.CustomBox5 = new System.Windows.Forms.TextBox();
            this.Custom4 = new System.Windows.Forms.Label();
            this.CustomBox4 = new System.Windows.Forms.TextBox();
            this.Custom3 = new System.Windows.Forms.Label();
            this.CustomBox3 = new System.Windows.Forms.TextBox();
            this.Custom2 = new System.Windows.Forms.Label();
            this.CustomBox2 = new System.Windows.Forms.TextBox();
            this.Custom1 = new System.Windows.Forms.Label();
            this.CustomBox1 = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(210, 445);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(90, 25);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "保存";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(332, 445);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(90, 25);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "取消";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtRBackward);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.txtLBackward);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.txtRForward);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtLForward);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtSpeaker);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.txtTurnOffLight);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtTurnOnLight);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtStop);
            this.groupBox1.Controls.Add(this.txtBackward);
            this.groupBox1.Controls.Add(this.txtRight);
            this.groupBox1.Controls.Add(this.txtLeft);
            this.groupBox1.Controls.Add(this.txtForward);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(13, 173);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(607, 129);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "指令设置";
            // 
            // txtRBackward
            // 
            this.txtRBackward.Location = new System.Drawing.Point(176, 99);
            this.txtRBackward.Name = "txtRBackward";
            this.txtRBackward.Size = new System.Drawing.Size(93, 21);
            this.txtRBackward.TabIndex = 55;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(136, 102);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(29, 12);
            this.label17.TabIndex = 54;
            this.label17.Text = "右后";
            // 
            // txtLBackward
            // 
            this.txtLBackward.Location = new System.Drawing.Point(34, 99);
            this.txtLBackward.Name = "txtLBackward";
            this.txtLBackward.Size = new System.Drawing.Size(93, 21);
            this.txtLBackward.TabIndex = 53;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(5, 102);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(29, 12);
            this.label16.TabIndex = 52;
            this.label16.Text = "左后";
            // 
            // txtRForward
            // 
            this.txtRForward.Location = new System.Drawing.Point(176, 72);
            this.txtRForward.Name = "txtRForward";
            this.txtRForward.Size = new System.Drawing.Size(93, 21);
            this.txtRForward.TabIndex = 51;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(136, 75);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 50;
            this.label9.Text = "右前";
            // 
            // txtLForward
            // 
            this.txtLForward.Location = new System.Drawing.Point(34, 72);
            this.txtLForward.Name = "txtLForward";
            this.txtLForward.Size = new System.Drawing.Size(93, 21);
            this.txtLForward.TabIndex = 49;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 48;
            this.label8.Text = "左前";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(462, 23);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(29, 12);
            this.label12.TabIndex = 47;
            this.label12.Text = "鸣笛";
            // 
            // txtSpeaker
            // 
            this.txtSpeaker.Location = new System.Drawing.Point(507, 18);
            this.txtSpeaker.Name = "txtSpeaker";
            this.txtSpeaker.Size = new System.Drawing.Size(93, 21);
            this.txtSpeaker.TabIndex = 46;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(458, 53);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(41, 12);
            this.label15.TabIndex = 43;
            this.label15.Text = "车灯关";
            // 
            // txtTurnOffLight
            // 
            this.txtTurnOffLight.Location = new System.Drawing.Point(507, 48);
            this.txtTurnOffLight.Name = "txtTurnOffLight";
            this.txtTurnOffLight.Size = new System.Drawing.Size(93, 21);
            this.txtTurnOffLight.TabIndex = 42;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(317, 53);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 41;
            this.label10.Text = "车灯开";
            // 
            // txtTurnOnLight
            // 
            this.txtTurnOnLight.Location = new System.Drawing.Point(363, 48);
            this.txtTurnOnLight.Name = "txtTurnOnLight";
            this.txtTurnOnLight.Size = new System.Drawing.Size(93, 21);
            this.txtTurnOnLight.TabIndex = 40;
            this.txtTurnOnLight.Text = " ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(340, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 12);
            this.label7.TabIndex = 35;
            this.label7.Text = "停";
            // 
            // txtStop
            // 
            this.txtStop.Location = new System.Drawing.Point(363, 20);
            this.txtStop.Name = "txtStop";
            this.txtStop.Size = new System.Drawing.Size(93, 21);
            this.txtStop.TabIndex = 34;
            // 
            // txtBackward
            // 
            this.txtBackward.Location = new System.Drawing.Point(176, 18);
            this.txtBackward.Name = "txtBackward";
            this.txtBackward.Size = new System.Drawing.Size(93, 21);
            this.txtBackward.TabIndex = 33;
            // 
            // txtRight
            // 
            this.txtRight.Location = new System.Drawing.Point(176, 45);
            this.txtRight.Name = "txtRight";
            this.txtRight.Size = new System.Drawing.Size(93, 21);
            this.txtRight.TabIndex = 32;
            // 
            // txtLeft
            // 
            this.txtLeft.Location = new System.Drawing.Point(34, 45);
            this.txtLeft.Name = "txtLeft";
            this.txtLeft.Size = new System.Drawing.Size(93, 21);
            this.txtLeft.TabIndex = 31;
            // 
            // txtForward
            // 
            this.txtForward.Location = new System.Drawing.Point(34, 20);
            this.txtForward.Name = "txtForward";
            this.txtForward.Size = new System.Drawing.Size(93, 21);
            this.txtForward.TabIndex = 30;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(142, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 29;
            this.label6.Text = "后";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(142, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 28;
            this.label5.Text = "右";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 27;
            this.label4.Text = "左";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 26;
            this.label3.Text = "前";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ControlURL);
            this.groupBox2.Controls.Add(this.VideoURL);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.txtPort);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(13, 81);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(607, 86);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "WIFI参数设置";
            // 
            // ControlURL
            // 
            this.ControlURL.Location = new System.Drawing.Point(70, 55);
            this.ControlURL.Name = "ControlURL";
            this.ControlURL.Size = new System.Drawing.Size(267, 21);
            this.ControlURL.TabIndex = 39;
            // 
            // VideoURL
            // 
            this.VideoURL.Location = new System.Drawing.Point(70, 24);
            this.VideoURL.Name = "VideoURL";
            this.VideoURL.Size = new System.Drawing.Size(267, 21);
            this.VideoURL.TabIndex = 38;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(366, 30);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 12);
            this.label11.TabIndex = 37;
            this.label11.Text = "端口";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(406, 24);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(93, 21);
            this.txtPort.TabIndex = 36;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(17, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "控制地址";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(17, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(398, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "视频地址";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBaudrate);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.comboPortName);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Location = new System.Drawing.Point(13, 25);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(337, 50);
            this.groupBox3.TabIndex = 28;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "蓝牙参数设置";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // comboBaudrate
            // 
            this.comboBaudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBaudrate.Items.AddRange(new object[] {
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.comboBaudrate.Location = new System.Drawing.Point(232, 20);
            this.comboBaudrate.Name = "comboBaudrate";
            this.comboBaudrate.Size = new System.Drawing.Size(92, 20);
            this.comboBaudrate.TabIndex = 33;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(181, 23);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(45, 20);
            this.label14.TabIndex = 32;
            this.label14.Text = "波特率";
            // 
            // comboPortName
            // 
            this.comboPortName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPortName.Location = new System.Drawing.Point(79, 20);
            this.comboPortName.Name = "comboPortName";
            this.comboPortName.Size = new System.Drawing.Size(83, 20);
            this.comboPortName.TabIndex = 31;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(17, 24);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(271, 20);
            this.label13.TabIndex = 30;
            this.label13.Text = "蓝牙串口";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.Custom12);
            this.groupBox4.Controls.Add(this.CustomBox12);
            this.groupBox4.Controls.Add(this.Custom11);
            this.groupBox4.Controls.Add(this.CustomBox11);
            this.groupBox4.Controls.Add(this.Custom10);
            this.groupBox4.Controls.Add(this.CustomBox10);
            this.groupBox4.Controls.Add(this.Custom9);
            this.groupBox4.Controls.Add(this.CustomBox9);
            this.groupBox4.Controls.Add(this.Custom8);
            this.groupBox4.Controls.Add(this.CustomBox8);
            this.groupBox4.Controls.Add(this.Custom7);
            this.groupBox4.Controls.Add(this.CustomBox7);
            this.groupBox4.Controls.Add(this.Custom6);
            this.groupBox4.Controls.Add(this.CustomBox6);
            this.groupBox4.Controls.Add(this.Custom5);
            this.groupBox4.Controls.Add(this.CustomBox5);
            this.groupBox4.Controls.Add(this.Custom4);
            this.groupBox4.Controls.Add(this.CustomBox4);
            this.groupBox4.Controls.Add(this.Custom3);
            this.groupBox4.Controls.Add(this.CustomBox3);
            this.groupBox4.Controls.Add(this.Custom2);
            this.groupBox4.Controls.Add(this.CustomBox2);
            this.groupBox4.Controls.Add(this.Custom1);
            this.groupBox4.Controls.Add(this.CustomBox1);
            this.groupBox4.Location = new System.Drawing.Point(12, 308);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(608, 127);
            this.groupBox4.TabIndex = 29;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "自定义动作";
            // 
            // Custom12
            // 
            this.Custom12.AutoSize = true;
            this.Custom12.Location = new System.Drawing.Point(455, 98);
            this.Custom12.Name = "Custom12";
            this.Custom12.Size = new System.Drawing.Size(53, 12);
            this.Custom12.TabIndex = 61;
            this.Custom12.Text = "自定义12";
            this.Custom12.Click += new System.EventHandler(this.Custom12_Click);
            // 
            // CustomBox12
            // 
            this.CustomBox12.Location = new System.Drawing.Point(508, 89);
            this.CustomBox12.Name = "CustomBox12";
            this.CustomBox12.Size = new System.Drawing.Size(93, 21);
            this.CustomBox12.TabIndex = 60;
            // 
            // Custom11
            // 
            this.Custom11.AutoSize = true;
            this.Custom11.Location = new System.Drawing.Point(455, 65);
            this.Custom11.Name = "Custom11";
            this.Custom11.Size = new System.Drawing.Size(53, 12);
            this.Custom11.TabIndex = 59;
            this.Custom11.Text = "自定义11";
            this.Custom11.Click += new System.EventHandler(this.Custom11_Click);
            // 
            // CustomBox11
            // 
            this.CustomBox11.Location = new System.Drawing.Point(508, 56);
            this.CustomBox11.Name = "CustomBox11";
            this.CustomBox11.Size = new System.Drawing.Size(93, 21);
            this.CustomBox11.TabIndex = 58;
            // 
            // Custom10
            // 
            this.Custom10.AutoSize = true;
            this.Custom10.Location = new System.Drawing.Point(455, 29);
            this.Custom10.Name = "Custom10";
            this.Custom10.Size = new System.Drawing.Size(53, 12);
            this.Custom10.TabIndex = 57;
            this.Custom10.Text = "自定义10";
            this.Custom10.Click += new System.EventHandler(this.Custom10_Click);
            // 
            // CustomBox10
            // 
            this.CustomBox10.Location = new System.Drawing.Point(508, 23);
            this.CustomBox10.Name = "CustomBox10";
            this.CustomBox10.Size = new System.Drawing.Size(93, 21);
            this.CustomBox10.TabIndex = 56;
            // 
            // Custom9
            // 
            this.Custom9.AutoSize = true;
            this.Custom9.Location = new System.Drawing.Point(304, 98);
            this.Custom9.Name = "Custom9";
            this.Custom9.Size = new System.Drawing.Size(47, 12);
            this.Custom9.TabIndex = 55;
            this.Custom9.Text = "自定义9";
            this.Custom9.Click += new System.EventHandler(this.Custom9_Click);
            // 
            // CustomBox9
            // 
            this.CustomBox9.Location = new System.Drawing.Point(357, 89);
            this.CustomBox9.Name = "CustomBox9";
            this.CustomBox9.Size = new System.Drawing.Size(93, 21);
            this.CustomBox9.TabIndex = 54;
            // 
            // Custom8
            // 
            this.Custom8.AutoSize = true;
            this.Custom8.Location = new System.Drawing.Point(304, 65);
            this.Custom8.Name = "Custom8";
            this.Custom8.Size = new System.Drawing.Size(47, 12);
            this.Custom8.TabIndex = 53;
            this.Custom8.Text = "自定义8";
            this.Custom8.Click += new System.EventHandler(this.Custom8_Click);
            // 
            // CustomBox8
            // 
            this.CustomBox8.Location = new System.Drawing.Point(357, 58);
            this.CustomBox8.Name = "CustomBox8";
            this.CustomBox8.Size = new System.Drawing.Size(93, 21);
            this.CustomBox8.TabIndex = 52;
            // 
            // Custom7
            // 
            this.Custom7.AutoSize = true;
            this.Custom7.Location = new System.Drawing.Point(304, 29);
            this.Custom7.Name = "Custom7";
            this.Custom7.Size = new System.Drawing.Size(47, 12);
            this.Custom7.TabIndex = 51;
            this.Custom7.Text = "自定义7";
            this.Custom7.Click += new System.EventHandler(this.Custom7_Click);
            // 
            // CustomBox7
            // 
            this.CustomBox7.Location = new System.Drawing.Point(357, 23);
            this.CustomBox7.Name = "CustomBox7";
            this.CustomBox7.Size = new System.Drawing.Size(93, 21);
            this.CustomBox7.TabIndex = 50;
            // 
            // Custom6
            // 
            this.Custom6.AutoSize = true;
            this.Custom6.Location = new System.Drawing.Point(155, 98);
            this.Custom6.Name = "Custom6";
            this.Custom6.Size = new System.Drawing.Size(47, 12);
            this.Custom6.TabIndex = 49;
            this.Custom6.Text = "自定义6";
            this.Custom6.Click += new System.EventHandler(this.Custom6_Click);
            // 
            // CustomBox6
            // 
            this.CustomBox6.Location = new System.Drawing.Point(208, 90);
            this.CustomBox6.Name = "CustomBox6";
            this.CustomBox6.Size = new System.Drawing.Size(93, 21);
            this.CustomBox6.TabIndex = 48;
            // 
            // Custom5
            // 
            this.Custom5.AutoSize = true;
            this.Custom5.Location = new System.Drawing.Point(155, 65);
            this.Custom5.Name = "Custom5";
            this.Custom5.Size = new System.Drawing.Size(47, 12);
            this.Custom5.TabIndex = 47;
            this.Custom5.Text = "自定义5";
            this.Custom5.Click += new System.EventHandler(this.Custom5_Click);
            // 
            // CustomBox5
            // 
            this.CustomBox5.Location = new System.Drawing.Point(208, 58);
            this.CustomBox5.Name = "CustomBox5";
            this.CustomBox5.Size = new System.Drawing.Size(93, 21);
            this.CustomBox5.TabIndex = 46;
            // 
            // Custom4
            // 
            this.Custom4.AutoSize = true;
            this.Custom4.Location = new System.Drawing.Point(155, 31);
            this.Custom4.Name = "Custom4";
            this.Custom4.Size = new System.Drawing.Size(47, 12);
            this.Custom4.TabIndex = 45;
            this.Custom4.Text = "自定义4";
            this.Custom4.Click += new System.EventHandler(this.Custom4_Click);
            // 
            // CustomBox4
            // 
            this.CustomBox4.Location = new System.Drawing.Point(208, 26);
            this.CustomBox4.Name = "CustomBox4";
            this.CustomBox4.Size = new System.Drawing.Size(93, 21);
            this.CustomBox4.TabIndex = 44;
            // 
            // Custom3
            // 
            this.Custom3.AutoSize = true;
            this.Custom3.Location = new System.Drawing.Point(5, 98);
            this.Custom3.Name = "Custom3";
            this.Custom3.Size = new System.Drawing.Size(47, 12);
            this.Custom3.TabIndex = 43;
            this.Custom3.Text = "自定义3";
            this.Custom3.Click += new System.EventHandler(this.Custom3_Click);
            // 
            // CustomBox3
            // 
            this.CustomBox3.Location = new System.Drawing.Point(58, 91);
            this.CustomBox3.Name = "CustomBox3";
            this.CustomBox3.Size = new System.Drawing.Size(93, 21);
            this.CustomBox3.TabIndex = 42;
            // 
            // Custom2
            // 
            this.Custom2.AutoSize = true;
            this.Custom2.Location = new System.Drawing.Point(5, 65);
            this.Custom2.Name = "Custom2";
            this.Custom2.Size = new System.Drawing.Size(47, 12);
            this.Custom2.TabIndex = 41;
            this.Custom2.Text = "自定义2";
            this.Custom2.Click += new System.EventHandler(this.Custom2_Click);
            // 
            // CustomBox2
            // 
            this.CustomBox2.Location = new System.Drawing.Point(58, 60);
            this.CustomBox2.Name = "CustomBox2";
            this.CustomBox2.Size = new System.Drawing.Size(93, 21);
            this.CustomBox2.TabIndex = 40;
            // 
            // Custom1
            // 
            this.Custom1.AutoSize = true;
            this.Custom1.Location = new System.Drawing.Point(5, 34);
            this.Custom1.Name = "Custom1";
            this.Custom1.Size = new System.Drawing.Size(47, 12);
            this.Custom1.TabIndex = 39;
            this.Custom1.Text = "自定义1";
            this.Custom1.Click += new System.EventHandler(this.Custom1_Click);
            // 
            // CustomBox1
            // 
            this.CustomBox1.Location = new System.Drawing.Point(58, 27);
            this.CustomBox1.Name = "CustomBox1";
            this.CustomBox1.Size = new System.Drawing.Size(93, 21);
            this.CustomBox1.TabIndex = 38;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.checkBox1);
            this.groupBox5.Location = new System.Drawing.Point(357, 25);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(263, 50);
            this.groupBox5.TabIndex = 30;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "高级";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(19, 24);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(84, 16);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "屏幕自适应";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Config
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(637, 482);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Config";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "系统设置";
            this.Load += new System.EventHandler(this.URLForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion


        private void okButton_Click(object sender, System.EventArgs e)
        {
            SaveSetting();
            MessageBox.Show("参数修改成功！请重启程序以使设置生效。","保存提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
        private void SaveSetting()
        {
            //WIFI
            WriteIni("VideoUrl", "videourl", this.VideoURL.Text);
            WriteIni("ControlUrl", "controlurl", this.ControlURL.Text);
            WriteIni("Port", "port", this.txtPort.Text);
            //BT
            WriteIni("BTCOM", "btcom", this.comboPortName.Text);
            WriteIni("BTBaudrate", "btbaudrate", this.comboBaudrate.Text);

            //Command
            WriteIni("Forward", "forward", this.txtForward.Text);
            WriteIni("Left", "left", this.txtLeft.Text);
            WriteIni("Right", "right", this.txtRight.Text);
            WriteIni("Backward", "backward", this.txtBackward.Text);
            WriteIni("Stop", "stop", this.txtStop.Text);

            WriteIni("LeftForward", "leftForward", this.txtLForward.Text);
            WriteIni("RightForward", "rightForward", this.txtRForward.Text);
            WriteIni("LeftBackward", "leftBackward", this.txtLBackward.Text);
            WriteIni("RightBackward", "rightBackward", this.txtRBackward.Text);

            WriteIni("Speaker", "speaker", this.txtSpeaker.Text);
            WriteIni("TurnOnLight", "turnOnLight", this.txtTurnOnLight.Text);
            WriteIni("TurnOffLight", "turnOffLight", this.txtTurnOffLight.Text);

            //Custom Command
            WriteIni("LabelCustomCMD", "lbCustom1", this.Custom1.Text);
            WriteIni("CustomCMD", "Custombox1", this.CustomBox1.Text);

            WriteIni("LabelCustomCMD", "lbCustom2", this.Custom2.Text);
            WriteIni("CustomCMD", "Custombox2", this.CustomBox2.Text);

            WriteIni("LabelCustomCMD", "lbCustom3", this.Custom3.Text);
            WriteIni("CustomCMD", "Custombox3", this.CustomBox3.Text);

            WriteIni("LabelCustomCMD", "lbCustom4", this.Custom4.Text);
            WriteIni("CustomCMD", "Custombox4", this.CustomBox4.Text);


            WriteIni("LabelCustomCMD", "lbCustom5", this.Custom5.Text);
            WriteIni("CustomCMD", "Custombox5", this.CustomBox5.Text);


            WriteIni("LabelCustomCMD", "lbCustom6", this.Custom6.Text);
            WriteIni("CustomCMD", "Custombox6", this.CustomBox6.Text);

            WriteIni("LabelCustomCMD", "lbCustom7", this.Custom7.Text);
            WriteIni("CustomCMD", "Custombox7", this.CustomBox7.Text);

            WriteIni("LabelCustomCMD", "lbCustom8", this.Custom8.Text);
            WriteIni("CustomCMD", "Custombox8", this.CustomBox8.Text);

            WriteIni("LabelCustomCMD", "lbCustom9", this.Custom9.Text);
            WriteIni("CustomCMD", "Custombox9", this.CustomBox9.Text);

            WriteIni("LabelCustomCMD", "lbCustom10", this.Custom10.Text);
            WriteIni("CustomCMD", "Custombox10", this.CustomBox10.Text);

            WriteIni("LabelCustomCMD", "lbCustom11", this.Custom11.Text);
            WriteIni("CustomCMD", "Custombox11", this.CustomBox11.Text);

            WriteIni("LabelCustomCMD", "lbCustom12", this.Custom12.Text);
            WriteIni("CustomCMD", "Custombox12", this.CustomBox12.Text);



            //高级选项设置
            WriteIni("AutoSetScreen", "autoSetScreen", AutoSetScreen.ToString());

        }
        //写INI文件
        public void WriteIni(string Section, string Ident, string Value)
        {
            if (!WritePrivateProfileString(Section, Ident, Value, FileName))
            {

                throw (new ApplicationException("写入配置文件出错"));
            }

        }
        //读取INI文件指定
        public string ReadIni(string Section, string Ident, string Default)
        {
            Byte[] Buffer = new Byte[65535];
            int bufLen = GetPrivateProfileString(Section, Ident, Default, Buffer, Buffer.GetUpperBound(0), FileName);
            //必须设定0（系统默认的代码页）的编码方式，否则无法支持中文
            string s = Encoding.GetEncoding(0).GetString(Buffer);
            s = s.Substring(0, bufLen);
            return s.Trim();
        }

        private void URLForm_Load(object sender, EventArgs e)
        {
            FileName = Application.StartupPath + "\\Config.ini";
            InitCOM();
        }
        private void InitCOM()
        {
            //初始化下拉串口名称列表框
            GetIni();
            string[] ports = SerialPort.GetPortNames();
            Array.Sort(ports);
            comboPortName.Items.AddRange(ports);
            comboPortName.SelectedIndex = comboPortName.Items.IndexOf(ReadIni("BTCOM", "btcom", ""));
            comboBaudrate.SelectedIndex = comboBaudrate.Items.IndexOf(ReadIni("BTBaudrate", "btbaudrate", ""));
          
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GetIni()
        {
            this.VideoURL.Text = ReadIni("VideoUrl", "videourl", "");
            this.txtPort.Text = ReadIni("Port", "port", "");
            this.ControlURL.Text = ReadIni("ControlUrl", "controlurl", "");

            this.txtForward.Text = ReadIni("Forward", "forward", "");
            this.txtBackward.Text = ReadIni("Backward", "backward", "");
            this.txtLeft.Text = ReadIni("Left", "left", "");
            this.txtRight.Text = ReadIni("Right", "right", "");
            this.txtStop.Text = ReadIni("Stop", "stop", "");


           this.txtLForward.Text=  ReadIni("LeftForward", "leftForward", "");
           this.txtRForward.Text= ReadIni("RightForward", "rightForward","" );
           this.txtLBackward.Text=ReadIni("LeftBackward", "leftBackward", "");
           this.txtRBackward.Text= ReadIni("RightBackward", "rightBackward","" );


            this.comboPortName.Text = ReadIni("BTCOM", "btcom", "");
            this.comboBaudrate.Text = ReadIni("BTBaudrate", "baudrate", "");
            this.txtSpeaker.Text = ReadIni("Speaker", "speaker", "");
            this.txtTurnOnLight.Text = ReadIni("TurnOnLight", "turnOnLight", "");
            this.txtTurnOffLight.Text = ReadIni("TurnOffLight", "turnOffLight", "");

            //显示自定义

            this.Custom1.Text = ReadIni("LabelCustomCMD", "lbCustom1", "自定义1");
            this.CustomBox1.Text = ReadIni("CustomCMD", "Custombox1", "");

            this.Custom2.Text = ReadIni("LabelCustomCMD", "lbCustom2", "自定义2");
            this.CustomBox2.Text = ReadIni("CustomCMD", "Custombox2", "");


            this.Custom3.Text = ReadIni("LabelCustomCMD", "lbCustom3", "自定义3");
            this.CustomBox3.Text = ReadIni("CustomCMD", "Custombox3", "");


            this.Custom4.Text = ReadIni("LabelCustomCMD", "lbCustom4", "自定义4");
            this.CustomBox4.Text = ReadIni("CustomCMD", "Custombox4", "");


            this.Custom5.Text = ReadIni("LabelCustomCMD", "lbCustom5", "自定义5");
            this.CustomBox5.Text = ReadIni("CustomCMD", "Custombox5", "");


            this.Custom6.Text = ReadIni("LabelCustomCMD", "lbCustom6", "自定义6");
            this.CustomBox6.Text = ReadIni("CustomCMD", "Custombox6", "");


            this.Custom7.Text = ReadIni("LabelCustomCMD", "lbCustom7", "自定义7");
            this.CustomBox7.Text = ReadIni("CustomCMD", "Custombox7", "");


            this.Custom8.Text = ReadIni("LabelCustomCMD", "lbCustom8", "自定义8");
            this.CustomBox8.Text = ReadIni("CustomCMD", "Custombox8", "");

            this.Custom9.Text = ReadIni("LabelCustomCMD", "lbCustom9", "自定义9");
            this.CustomBox9.Text = ReadIni("CustomCMD", "Custombox9", "");


            this.Custom10.Text = ReadIni("LabelCustomCMD", "lbCustom10", "自定义10");
            this.CustomBox10.Text = ReadIni("CustomCMD", "Custombox10", "");


            this.Custom11.Text = ReadIni("LabelCustomCMD", "lbCustom11", "自定义11");
            this.CustomBox11.Text = ReadIni("CustomCMD", "Custombox11", "");

            this.Custom12.Text = ReadIni("LabelCustomCMD", "lbCustom12", "自定义12");
            this.CustomBox12.Text = ReadIni("CustomCMD", "Custombox12", "");

            this.checkBox1.Checked = ReadIni("AutoSetScreen", "autoSetScreen", "") == "0" ? false : true;
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void Custom1_Click(object sender, EventArgs e)
        {
            CustomSetting cs = new CustomSetting();
            cs.Setcustomlabelname += new motion.CustomSetting.Setlabelname(CustomLabel1SettingCallBack);
            cs.ShowDialog();
        }

        void CustomLabel1SettingCallBack(string str)
        {
            this.Custom1.Text = str.Length < 4 ? str : str.Substring(0, 3)+"``"; 
        }

        private void Custom2_Click(object sender, EventArgs e)
        {
            CustomSetting cs = new CustomSetting();
            cs.Setcustomlabelname += new motion.CustomSetting.Setlabelname(CustomLabel2SettingCallBack);
            cs.ShowDialog();
        }

        void CustomLabel2SettingCallBack(string str)
        {
            this.Custom2.Text = str.Length < 4 ? str : str.Substring(0, 3) + "``";
        }

        private void Custom3_Click(object sender, EventArgs e)
        {
            CustomSetting cs = new CustomSetting();
            cs.Setcustomlabelname += new motion.CustomSetting.Setlabelname(CustomLabel3SettingCallBack);
            cs.ShowDialog();
        }

        void CustomLabel3SettingCallBack(string str)
        {
            this.Custom3.Text = str.Length < 4 ? str : str.Substring(0, 3) + "``";
        }


        private void Custom4_Click(object sender, EventArgs e)
        {
            CustomSetting cs = new CustomSetting();
            cs.Setcustomlabelname += new motion.CustomSetting.Setlabelname(CustomLabel4SettingCallBack);
            cs.ShowDialog();
        }

        void CustomLabel4SettingCallBack(string str)
        {
            this.Custom4.Text = str.Length < 4 ? str : str.Substring(0, 3) + "``";
        }


        private void Custom5_Click(object sender, EventArgs e)
        {
            CustomSetting cs = new CustomSetting();
            cs.Setcustomlabelname += new motion.CustomSetting.Setlabelname(CustomLabel5SettingCallBack);
            cs.ShowDialog();
        }

        void CustomLabel5SettingCallBack(string str)
        {
            this.Custom5.Text = str.Length < 4 ? str : str.Substring(0, 3) + "``";
        }


        private void Custom6_Click(object sender, EventArgs e)
        {
            CustomSetting cs = new CustomSetting();
            cs.Setcustomlabelname += new motion.CustomSetting.Setlabelname(CustomLabel6SettingCallBack);
            cs.ShowDialog();
        }

        void CustomLabel6SettingCallBack(string str)
        {
            this.Custom6.Text = str.Length < 4 ? str : str.Substring(0, 3) + "``";
        }


        private void Custom7_Click(object sender, EventArgs e)
        {
            CustomSetting cs = new CustomSetting();
            cs.Setcustomlabelname += new motion.CustomSetting.Setlabelname(CustomLabel7SettingCallBack);
            cs.ShowDialog();
        }

        void CustomLabel7SettingCallBack(string str)
        {
            this.Custom7.Text = str.Length < 4 ? str : str.Substring(0, 3) + "``";
        }


        private void Custom8_Click(object sender, EventArgs e)
        {
            CustomSetting cs = new CustomSetting();
            cs.Setcustomlabelname += new motion.CustomSetting.Setlabelname(CustomLabel8SettingCallBack);
            cs.ShowDialog();
        }

        void CustomLabel8SettingCallBack(string str)
        {
            this.Custom8.Text = str.Length < 4 ? str : str.Substring(0, 3) + "``";
        }


        private void Custom9_Click(object sender, EventArgs e)
        {
            CustomSetting cs = new CustomSetting();
            cs.Setcustomlabelname += new motion.CustomSetting.Setlabelname(CustomLabel9SettingCallBack);
            cs.ShowDialog();
        }

        void CustomLabel9SettingCallBack(string str)
        {
            this.Custom9.Text = str.Length < 4 ? str : str.Substring(0, 3) + "``";
        }


        private void Custom10_Click(object sender, EventArgs e)
        {
            CustomSetting cs = new CustomSetting();
            cs.Setcustomlabelname += new motion.CustomSetting.Setlabelname(CustomLabel10SettingCallBack);
            cs.ShowDialog();
        }

        void CustomLabel10SettingCallBack(string str)
        {
            this.Custom10.Text = str.Length < 4 ? str : str.Substring(0, 3) + "``";
        }

        private void Custom11_Click(object sender, EventArgs e)
        {
            CustomSetting cs = new CustomSetting();
            cs.Setcustomlabelname += new motion.CustomSetting.Setlabelname(CustomLabel11SettingCallBack);
            cs.ShowDialog();
        }

        void CustomLabel11SettingCallBack(string str)
        {
            this.Custom11.Text = str.Length < 4 ? str : str.Substring(0, 3) + "``";
        }

        private void Custom12_Click(object sender, EventArgs e)
        {
            CustomSetting cs = new CustomSetting();
            cs.Setcustomlabelname += new motion.CustomSetting.Setlabelname(CustomLabel12SettingCallBack);
            cs.ShowDialog();
        }

        void CustomLabel12SettingCallBack(string str)
        {
            this.Custom12.Text = str.Length < 4 ? str : str.Substring(0, 3) + "``";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            AutoSetScreen = this.checkBox1.Checked ? 1 : 0;
        }  
    }
}
