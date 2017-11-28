using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using OrderModel;
using OrderBLL;
namespace MyWeb
{
    public partial class Index : System.Web.UI.Page
    {
        OrderInfoBLL OInfo = new OrderInfoBLL();

        protected void Order(object sender, EventArgs e)
        {
            Random ran = new Random();
            int id = ran.Next(1000, 9999);
            String time = DateTime.Now.ToString();

            Order order = new Order();

            order.ID = id.ToString();
            order.OrderTime = time;
            order.ScheTime = AScheTime.Text;
            order.People = APeople.Text;
            order.PeopleNum = APeopleNum.Text;
            order.State = "未完成";
            order.phone = Aphone.Text;
            order.TableNum = "0";
            int num = OInfo.InsertOrder(order);
            if (num != 0)
            {
                Response.Write("<script>window.alert('" + "预订成功,您预订桌号为" + num + "');</script>");
            }
            else
            {
                Response.Write("<script>window.alert('预订失败');</script>");

            }
        }

      
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Table_Click(object sender, EventArgs e)
        {
            Server.Transfer("Search.aspx"); 
        }

        public static bool IsNum(string text)
        {

            for (int i = 0; i < text.Length; i++)
            {

                if (!Char.IsNumber(text, i))
                {

                    return true; //输入的不是数字  

                }

            }

            return false; //否则是数字

        }
    }
}