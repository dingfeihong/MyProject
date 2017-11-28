using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WIFIRobot
{
    public partial class FrameRateForm : Form
    {
        public FrameRateForm()
        {
            InitializeComponent();
        }

        private double rate = 0;

        public double Rate
        {
            get { return rate; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            rate = (double)numRate.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
