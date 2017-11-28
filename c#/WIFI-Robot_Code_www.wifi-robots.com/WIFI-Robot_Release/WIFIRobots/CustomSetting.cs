using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace motion
{
    public partial class CustomSetting : Form
    {
        public CustomSetting()
        {
            InitializeComponent();
        }
        public delegate void Setlabelname(string labelname);
        public event Setlabelname  Setcustomlabelname;

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.txtCustomLabelName.Text != "")
            {
                Setcustomlabelname(this.txtCustomLabelName.Text);
            }
            this.Close();
        }
    }
}
