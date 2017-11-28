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
using LogLayer;
using Model;
namespace MyStore
{
    public partial class booklist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindData();
        }
        public void BindData()
        {
            DataTable dt = BookInfoBLL.GetAllTable(BookGrid.PageSize);
            BookGrid.DataSource = dt;
            BookGrid.DataBind();
        }
        protected void Buy(Object sender, DataGridCommandEventArgs E)
        {
            int n = int.Parse(E.Item.Cells[4].Text);
            if (n <= 0)
            {
                Response.Write("<script>window.alert('购买失败，图书不足');</script>");
                return;
            }
            Model.Book tmp1=new Book();
            tmp1.Id = int.Parse(E.Item.Cells[1].Text);
            tmp1.Name = E.Item.Cells[2].Text;
            tmp1.Writer = E.Item.Cells[3].Text;
            tmp1.Count = n - 1;
            BookInfoBLL.Update(tmp1);

            Model.Order tmp2 = new Order();
            Random ran = new Random();
            int id = ran.Next(10, 99);
            tmp2.ID = id;
            tmp2.OrderTime = DateTime.Now.ToString();
            tmp2.Customer= Request.QueryString["user"];
            tmp2.BookNum= int.Parse(E.Item.Cells[1].Text);
            OrderInfoBLL.AddOrder(tmp2);
            BindData();
        }
        protected void Search(object sender, EventArgs e)
        {
            String tmp = CInfo.Text;
            BookGrid.DataSource= BookInfoBLL.Search(tmp,BookGrid.PageSize);
            BookGrid.DataBind();
        }
        protected void Reset(object sender, EventArgs e)
        {
            BindData();
        }
        protected void BookGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            BookGrid.CurrentPageIndex = e.NewPageIndex;
        }

        protected void btn_return(object sender, EventArgs e)
        {
            Response.Redirect("index.aspx");
        }
    }
}