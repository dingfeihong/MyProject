using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;
using LogLayer;
namespace MyStore
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void Clog_in_Click(object sender, EventArgs e)
        {           
            int n=AccountInfoBLL.login(Cuser.Text, Cpassword.Text);
            String urlAddress;
            if (n == 0)
            {
                urlAddress = "booklist.aspx?user=" + Cuser.Text;
               
            }
            else if (n == 1)
            {
                urlAddress = "Manage.aspx";
            }
            else
            {
                Csign.Text = "找不到该用户";
                return;
            }
            Response.Redirect(urlAddress);
        }
        protected void CRegister(object sender, EventArgs e)
        {
            Response.Redirect("register.aspx");
        }
    }
}