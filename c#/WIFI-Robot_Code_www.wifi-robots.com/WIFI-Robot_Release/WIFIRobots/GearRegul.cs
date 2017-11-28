using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using WIFIRobotCMDEngineV2;
using System.Threading;
using System.Net.Sockets;

namespace motion
{
    public partial class GearRegul : Form
    {
        public GearRegul()
        {
            InitializeComponent();
            RobotEngine = new WifiRobotCMDEngine(CheckStr.str);
        }
        public delegate void ChangeSetting(bool topmost);  
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);

        public event ChangeSetting ChangeGearSetting;
        private string FileName;
        public WifiRobotCMDEngine RobotEngine;//实例化引擎 

        private void btnOpti_Click(object sender, EventArgs e)
        {
            this.Gear1UPValue.Text = this.Gear2UPValue.Text = this.Gear3UPValue.Text = this.Gear4UPValue.Text = this.Gear5UPValue.Text = this.Gear6UPValue.Text = this.Gear7UPValue.Text = this.Gear8UPValue.Text = "0";
            this.Gear1DownValue.Text = this.Gear2DownValue.Text = this.Gear3DownValue.Text = this.Gear4DownValue.Text = this.Gear5DownValue.Text = this.Gear6DownValue.Text = this.Gear7DownValue.Text = this.Gear8DownValue.Text = "180";
            this.Gear1Default.Text = this.Gear2Default.Text = this.Gear3Default.Text = this.Gear4Default.Text = this.Gear5Default.Text = this.Gear6Default.Text = this.Gear7Default.Text = this.Gear8Default.Text = "90";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveSetting();
          
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

        private void GearRegul_Load(object sender, EventArgs e)
        {
            FileName = Application.StartupPath + "\\Config.ini";
            GetIni();
        }

        private void GetIni()
        {
            //读取上限
            this.Gear1UPValue.Text = ReadIni("Gear1UP", "gear1up", "");
            this.Gear2UPValue.Text = ReadIni("Gear2UP", "gear2up", "");
            this.Gear3UPValue.Text = ReadIni("Gear3UP", "gear3up", "");
            this.Gear4UPValue.Text = ReadIni("Gear4UP", "gear4up", "");
            this.Gear5UPValue.Text = ReadIni("Gear5UP", "gear5up", "");
            this.Gear6UPValue.Text = ReadIni("Gear6UP", "gear6up", "");
            this.Gear7UPValue.Text = ReadIni("Gear7UP", "gear7up", "");
            this.Gear8UPValue.Text = ReadIni("Gear8UP", "gear8up", "");


            //读取默认
            this.Gear1Default.Text = ReadIni("Gear1Default", "gear1default", "");
            this.Gear2Default.Text = ReadIni("Gear2Default", "gear2default", "");
            this.Gear3Default.Text = ReadIni("Gear3Default", "gear3default", "");
            this.Gear4Default.Text = ReadIni("Gear4Default", "gear4default", "");
            this.Gear5Default.Text = ReadIni("Gear5Default", "gear5default", "");
            this.Gear6Default.Text = ReadIni("Gear6Default", "gear6default", "");
            this.Gear7Default.Text = ReadIni("Gear7Default", "gear7default", "");
            this.Gear8Default.Text = ReadIni("Gear8Default", "gear8default", "");


            //读取下限
            this.Gear1DownValue.Text = ReadIni("Gear1Down", "gear1down", "");
            this.Gear2DownValue.Text = ReadIni("Gear2Down", "gear2down", "");
            this.Gear3DownValue.Text = ReadIni("Gear3Down", "gear3down", "");
            this.Gear4DownValue.Text = ReadIni("Gear4Down", "gear4down", "");
            this.Gear5DownValue.Text = ReadIni("Gear5Down", "gear5down", "");
            this.Gear6DownValue.Text = ReadIni("Gear6Down", "gear6down", "");
            this.Gear7DownValue.Text = ReadIni("Gear7Down", "gear7down", "");
            this.Gear8DownValue.Text = ReadIni("Gear8Down", "gear8down", "");

        }

        private void SaveSetting()
        {

            if (
                checkvalue(this.Gear1UPValue.Text, this.Gear1Default.Text, this.Gear1DownValue.Text) && checkvalue(this.Gear2UPValue.Text, this.Gear2Default.Text, this.Gear2DownValue.Text) && checkvalue(this.Gear3UPValue.Text, this.Gear3Default.Text, this.Gear3DownValue.Text) && checkvalue(this.Gear4UPValue.Text, this.Gear4Default.Text, this.Gear4DownValue.Text) && checkvalue(this.Gear5UPValue.Text, this.Gear5Default.Text, this.Gear5DownValue.Text) && checkvalue(this.Gear6UPValue.Text, this.Gear6Default.Text, this.Gear6DownValue.Text) && checkvalue(this.Gear7UPValue.Text, this.Gear7Default.Text, this.Gear7DownValue.Text) && checkvalue(this.Gear8UPValue.Text, this.Gear8Default.Text, this.Gear8DownValue.Text)
                )
            {
                //上限
                WriteIni("Gear1UP", "gear1up", this.Gear1UPValue.Text);
                WriteIni("Gear2UP", "gear2up", this.Gear2UPValue.Text);
                WriteIni("Gear3UP", "gear3up", this.Gear3UPValue.Text);
                WriteIni("Gear4UP", "gear4up", this.Gear4UPValue.Text);
                WriteIni("Gear5UP", "gear5up", this.Gear5UPValue.Text);
                WriteIni("Gear6UP", "gear6up", this.Gear6UPValue.Text);
                WriteIni("Gear7UP", "gear7up", this.Gear7UPValue.Text);
                WriteIni("Gear8UP", "gear8up", this.Gear8UPValue.Text);

                //默认
                WriteIni("Gear1Default", "gear1default", this.Gear1Default.Text);
                WriteIni("Gear2Default", "gear2default", this.Gear2Default.Text);
                WriteIni("Gear3Default", "gear3default", this.Gear3Default.Text);
                WriteIni("Gear4Default", "gear4default", this.Gear4Default.Text);
                WriteIni("Gear5Default", "gear5default", this.Gear5Default.Text);
                WriteIni("Gear6Default", "gear6default", this.Gear6Default.Text);
                WriteIni("Gear7Default", "gear7default", this.Gear7Default.Text);
                WriteIni("Gear8Default", "gear8default", this.Gear8Default.Text);



                //下限
                WriteIni("Gear1Down", "gear1down", this.Gear1DownValue.Text);
                WriteIni("Gear2Down", "gear2down", this.Gear2DownValue.Text);
                WriteIni("Gear3Down", "gear3down", this.Gear3DownValue.Text);
                WriteIni("Gear4Down", "gear4down", this.Gear4DownValue.Text);
                WriteIni("Gear5Down", "gear5down", this.Gear5DownValue.Text);
                WriteIni("Gear6Down", "gear6down", this.Gear6DownValue.Text);
                WriteIni("Gear7Down", "gear7down", this.Gear7DownValue.Text);
                WriteIni("Gear8Down", "gear8down", this.Gear8DownValue.Text);
                MessageBox.Show("舵机角度设置保存成功！","舵机角度设置提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                ChangeGearSetting(true);

                this.Close();
            }
           
        }
        private bool checkvalue(string up,string defaultvalue,string down)
        {
            if (up == "" || defaultvalue == "" || down == "")
            {
                MessageBox.Show("上限值、下限值、默认值不能为空！", "舵机角度设置提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (int.Parse(up) > int.Parse(down))
            {
                MessageBox.Show("上限值必须小于下限值", "舵机角度设置提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (int.Parse(up)<0 || int.Parse(down)>180)
            {
                MessageBox.Show("上限值不能小于0，下限值不能大于180","舵机角度设置提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }


            if (!(int.Parse(defaultvalue) >= int.Parse(up) && int.Parse(defaultvalue) <= int.Parse(down)))
            {
                MessageBox.Show("默认值必须在上限值与下限值的范围内", "舵机角度设置提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      

    }
}
