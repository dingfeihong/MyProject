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
    public partial class register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Register(object sender, EventArgs e)
        {
            Account tmp = new Account();
            tmp.User = Ruser.Text;
            tmp.Password = int.Parse(Rpsd.Text);
            tmp.Phone = Rphone.Text;
            tmp.AType = "用户";
            Random ran = new Random();
            AccountInfoBLL.Register(tmp);
            Response.Redirect("index.aspx");
        }

        protected void btn_return(object sender, EventArgs e)
        {
            Response.Redirect("index.aspx");
        }
    }
}