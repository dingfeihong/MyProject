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
    public partial class Manage : System.Web.UI.Page
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

            dt = OrderInfoBLL.GetAllTable(OrderGrid.PageSize);
            OrderGrid.DataSource = dt;
            OrderGrid.DataBind();

            dt = AccountInfoBLL.GetAllTable(AccountGrid.PageSize);
            AccountGrid.DataSource = dt;
            AccountGrid.DataBind();
        }

        #region Book
        protected void EditBook(Object sender, DataGridCommandEventArgs E)
        {            
            BookGrid.EditItemIndex = (int)E.Item.ItemIndex;//改为可编辑状态
            BindData();
        }

        protected void DeleteBook(Object sender, DataGridCommandEventArgs E)
        {
            String id = BookGrid.DataKeys[(int)E.Item.ItemIndex].ToString();
            BookInfoBLL.Delete(id);
            BindData();
        }
        protected void SearchBook(object sender, EventArgs e)
        {
            String tmp = CInfo.Text;
            BookGrid.DataSource = BookInfoBLL.Search(tmp, BookGrid.PageSize);
            BookGrid.DataBind();
        }
        protected void AddBook(object sender, EventArgs e)
        {
            Book tmp = new Book();
            tmp.Name = RBookName.Text;
            tmp.Writer = RBookWriter.Text;
            tmp.Count = int.Parse(RBookNum.Text);
            Random ran = new Random();
            tmp.Id= ran.Next(10, 99);
            BookInfoBLL.AddBook(tmp);
            BindData();
        }
        protected void UpdateBook(object sender, DataGridCommandEventArgs E)
        {
            Book tmp = new Book();
            tmp.Id = int.Parse(BookGrid.DataKeys[(int)E.Item.ItemIndex].ToString());
            tmp.Name = ((TextBox)E.Item.Cells[3].Controls[0]).Text;
            tmp.Writer = ((TextBox)E.Item.Cells[4].Controls[0]).Text;
            tmp.Count = int.Parse(((TextBox)E.Item.Cells[5].Controls[0]).Text);

            if (!BookInfoBLL.Update(tmp))            
                Response.Write("<script>window.alert('更新失败');</script>");            
            else            
                BindData();
            
            Cancel(sender, E);
        }

        protected void BookGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            BookGrid.CurrentPageIndex = e.NewPageIndex;
            BindData(); ;
        }
        #endregion

        #region Order
        protected void EditOrder(Object sender, DataGridCommandEventArgs E)
        {
            String Old_ID = String.Empty,
              Old_Time = String.Empty,
             // Old_Customer = String.Empty,
              Old_BID = String.Empty;

            Old_ID =
               ((Label)OrderGrid.Items[E.Item.ItemIndex].FindControl("OrderID")).Text;
            Old_Time =
                ((Label)OrderGrid.Items[E.Item.ItemIndex].FindControl("_OrderTime")).Text;
            Old_BID =
                ((Label)OrderGrid.Items[E.Item.ItemIndex].FindControl("_OBookNum")).Text;


            OrderGrid.EditItemIndex = (int)E.Item.ItemIndex;//改为可编辑状态
            BindData();
   
            DropDownList DDList = (DropDownList)OrderGrid.Items[E.Item.ItemIndex].FindControl("OBookNum"); ;
            DDList.Items.Add(new ListItem(Old_BID, Old_BID));
            OleDbDataReader reader = OrderInfoBLL.BindDropList_Tnum();
            while (reader.Read())
            {
                Old_BID = reader["图书编号"].ToString();
                DDList.Items.Add(new ListItem(Old_BID, Old_BID));
            }

            TextBox MyTextBox =               
               (TextBox)OrderGrid.Items[E.Item.ItemIndex].FindControl("OrderTime");
            MyTextBox.Text = Old_Time;

        }
        protected void DeleteOrder(Object sender, DataGridCommandEventArgs E)
        {
            String id = OrderGrid.DataKeys[(int)E.Item.ItemIndex].ToString();
            OrderInfoBLL.Delete(id);
            BindData();
        }
        protected void SearchOrder(object sender, EventArgs e)
        {
            OrderGrid.DataSource = OrderInfoBLL.Search(SOrderInfo.Text,SOrderTime.Text,OrderGrid.PageSize);
            OrderGrid.DataBind();
        }
        protected void UpdateOrder(object sender, DataGridCommandEventArgs E)
        {
            Order tmp = new Order();

           
            tmp.ID =
                int.Parse(((Label)OrderGrid.Items[E.Item.ItemIndex].FindControl("OrderID")).Text);
            tmp.OrderTime =
                ((TextBox)OrderGrid.Items[E.Item.ItemIndex].FindControl("OrderTime")).Text;
            tmp.BookNum =
                int.Parse(((DropDownList)OrderGrid.Items[E.Item.ItemIndex].FindControl("OBookNum")).Text);
            tmp.Customer = ((Label)OrderGrid.Items[E.Item.ItemIndex].FindControl("OCustomer")).Text;

            if (!OrderInfoBLL.Update(tmp))
                Response.Write("<script>window.alert('更新失败');</script>");
            else
                BindData();

            Cancel(sender, E);
        }
        protected void OrderGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            OrderGrid.CurrentPageIndex = e.NewPageIndex;
            BindData(); ;
        }
        #endregion

        #region Account
        protected void EditAccount(Object sender, DataGridCommandEventArgs E)
        {
            AccountGrid.EditItemIndex = (int)E.Item.ItemIndex;//改为可编辑状态
            BindData();
            DropDownList MyDropDownList =
              (DropDownList)AccountGrid.Items[E.Item.ItemIndex].FindControl("AccountType");
            MyDropDownList.Items.Add(new ListItem("管理员", "管理员"));
            MyDropDownList.Items.Add(new ListItem("用户", "用户"));
        }
        protected void DeleteAccount(Object sender, DataGridCommandEventArgs E)
        {
            String id = AccountGrid.DataKeys[(int)E.Item.ItemIndex].ToString();
            AccountInfoBLL.Delete(id);
            BindData();
        }
        protected void SearchAccount(object sender, EventArgs e)
        {
            AccountGrid.DataSource = AccountInfoBLL.Search(SAInfo.Text, AccountType.Text, AccountGrid.PageSize);
            AccountGrid.DataBind();
        }
        protected void RegisterAccount(object sender, EventArgs e)
        {
            Account tmp = new Account();
            tmp.User = Ruser.Text;
            tmp.Password = int.Parse(Rpsd.Text);
            tmp.Phone = Rphone.Text;
            tmp.AType = Rtype.Text;
            Random ran = new Random();
            AccountInfoBLL.Register(tmp);
            BindData();
        }
        
        protected void UpdateAccount(object sender, DataGridCommandEventArgs E)
        {
            Account tmp = new Account();

            tmp.User = AccountGrid.DataKeys[(int)E.Item.ItemIndex].ToString();
            tmp.Password= int.Parse(((TextBox)E.Item.Cells[3].Controls[0]).Text);
            tmp.Phone = ((TextBox)E.Item.Cells[4].Controls[0]).Text;
            tmp.AType= ((DropDownList)AccountGrid.Items[E.Item.ItemIndex].FindControl("AccountType")).Text;
                   
            if (!AccountInfoBLL.Update(tmp))
                Response.Write("<script>window.alert('更新失败');</script>");
            else
                BindData();
            Cancel(sender, E);
        }
        protected void AccountGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            OrderGrid.CurrentPageIndex = e.NewPageIndex;
            BindData(); ;
        }
        #endregion


        #region 公用
        protected void Cancel(Object sender, DataGridCommandEventArgs E)
        {
            BookGrid.EditItemIndex = -1;
            OrderGrid.EditItemIndex = -1;
            AccountGrid.EditItemIndex = -1;
            BindData();
        }
        protected void Reset(object sender, EventArgs e)
        {
            BindData();
        }
        protected void btn_return(object sender, EventArgs e)
        {
            Response.Redirect("index.aspx");
        }
        protected void btn_Click(object sender, EventArgs e)
        {

            //获取被触发的Button对象  
            Button b = (Button)sender;
            if (b.ID == "btnBook")
            {
                //激活View1  
                //sign.Text = "1";
                MultiView1.SetActiveView(Vbook);
            }
            else if(b.ID=="btnOrder")
            {
                //sign.Text = "0";
                //激活View2  
                MultiView1.SetActiveView(Vorder);
            }
            else if (b.ID == "btnAccount")
            {
                //sign.Text = "0";
                //激活View2  
                MultiView1.SetActiveView(Vaccount);
            }
            BindData();
        }
        #endregion
    }
}