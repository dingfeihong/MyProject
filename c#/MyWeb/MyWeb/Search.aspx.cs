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
    public partial class Search : System.Web.UI.Page
    {
        OrderInfoBLL OInfo = new OrderInfoBLL();
        TableInfoBLL TInfo = new TableInfoBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            SOInfo.Attributes.Add("OnFocus", "this.focus();this.select();");
            STInfo.Attributes.Add("OnFocus", "this.focus();this.select();");
            if (!IsPostBack)
                BindData();
        }

        public void BindData()
        {
            DataTable dt = OInfo.GetAllOrder(odg.PageSize);
            odg.DataSource = dt;
            odg.DataBind();            

            dt = TInfo.GetAllTable(tdg.PageSize);
            tdg.DataSource = dt;
            tdg.DataBind();
            if (!IsPostBack)
                BindControl();
        }
        protected void BindControl()
        {
            DataTable dt = TInfo.GetAllTable();
            ListItem temp = new ListItem("桌号", "桌号");
            SOTableNum.DataSource = dt;
            SOTableNum.DataValueField = "桌号";
            SOTableNum.DataBind();
            SOTableNum.Items.Add(temp);
            SOTableNum.Items.FindByValue("桌号").Selected = true;

            ATableNum.DataSource = dt;
            ATableNum.DataValueField = "桌号";
            ATableNum.DataBind();
        }
      
        protected void Cancel(Object sender, DataGridCommandEventArgs E)
        {
            odg.EditItemIndex = -1;
            tdg.EditItemIndex = -1;
            BindData();
        }        

        protected void DeleteOrder(Object sender, DataGridCommandEventArgs E)
        {
            String str = odg.DataKeys[(int)E.Item.ItemIndex].ToString();
            OInfo.DelectOrder(str);
            BindData();
        }
        protected void EditOrder(Object sender, DataGridCommandEventArgs E)
        {
            String Old_Tnum = String.Empty,
               Old_Sctime = String.Empty,
               Old_Otime = String.Empty,
               Old_NoP = String.Empty,
               TableNum = String.Empty;

            Old_Tnum =
                ((Label)odg.Items[E.Item.ItemIndex].FindControl("_OTableNum")).Text;
            Old_Sctime =
                ((Label)odg.Items[E.Item.ItemIndex].FindControl("_ScheTime")).Text;
            Old_Otime =
                ((Label)odg.Items[E.Item.ItemIndex].FindControl("_OrderTime")).Text;
            Old_NoP =
                ((Label)odg.Items[E.Item.ItemIndex].FindControl("_PeopleNum")).Text;

            odg.EditItemIndex = (int)E.Item.ItemIndex;//改为可编辑状态
            BindData();

            DropDownList MyDropDownList =
               (DropDownList)odg.Items[E.Item.ItemIndex].FindControl("OTableNum");
            MyDropDownList.Items.Add(new ListItem(Old_Tnum, Old_Tnum));

            OleDbDataReader reader = OInfo.BindDropList();
            while (reader.Read())
            {
                TableNum = reader["桌号"].ToString();
                MyDropDownList.Items.Add(new ListItem(TableNum, TableNum));
            }

            TextBox MyTextBox =
                (TextBox)odg.Items[E.Item.ItemIndex].FindControl("ScheTime");
            MyTextBox.Text = Old_Sctime;

            MyTextBox =
               (TextBox)odg.Items[E.Item.ItemIndex].FindControl("OrderTime");
            MyTextBox.Text = Old_Otime;

            MyTextBox =
               (TextBox)odg.Items[E.Item.ItemIndex].FindControl("PeopleNum");
            MyTextBox.Text = Old_NoP;
        }
        protected void SearchOrder(object sender, EventArgs e)
        {
            odg.DataSource = OInfo.SearchOrder(SOInfo.Text, SOTableNum.Text, SOOrderTime.Text, SOScheTime.Text, OrderState.Text, odg.PageSize);
            odg.DataBind();
        }
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
            BindData();
        }
        protected void UpdateOrder(Object sender, DataGridCommandEventArgs E)
        {
            //编辑桌号下拉菜单
            Order order = new Order();
            DropDownList TableNum =
                (DropDownList)odg.Items[E.Item.ItemIndex].FindControl("OTableNum");
            DropDownList OrderState =
               (DropDownList)odg.Items[E.Item.ItemIndex].FindControl("OrderState");
            TextBox PeopleNum =
               (TextBox)odg.Items[E.Item.ItemIndex].FindControl("PeopleNum");
            TextBox ScheTime =
               (TextBox)odg.Items[E.Item.ItemIndex].FindControl("ScheTime");
            TextBox OrderTime =
              (TextBox)odg.Items[E.Item.ItemIndex].FindControl("OrderTime");

            order.TableNum = TableNum.Text;
            order.State = OrderState.Text;
            order.PeopleNum = PeopleNum.Text;
            order.OrderTime = OrderTime.Text;
            order.ScheTime = ScheTime.Text;
            order.phone = ((TextBox)E.Item.Cells[8].Controls[0]).Text;
            order.People = ((TextBox)E.Item.Cells[7].Controls[0]).Text;
            order.ID = odg.DataKeys[(int)E.Item.ItemIndex].ToString();
            //编辑订单状态下拉菜单

            if (!OInfo.UpdateOrder(order))
            {
                Response.Write("<script>window.alert('更新失败,该日期已被占用');</script>");
                Cancel(sender, E);
                return;
            }
            BindData();
        }

        protected void DeleteTable(Object sender, DataGridCommandEventArgs E)
        {
            String id = tdg.DataKeys[(int)E.Item.ItemIndex].ToString();
            TInfo.DelectTable(id);
            BindData();
        }      
        protected void EditTable(Object sender, DataGridCommandEventArgs E)
        {
            tdg.EditItemIndex = (int)E.Item.ItemIndex;//改为可编辑状态
            BindData();
        }
        protected void SearchTable(object sender, EventArgs e)
        {
            tdg.DataSource = TInfo.SearchTable(STInfo.Text, tdg.PageSize);
            tdg.DataBind();
        }
        protected void AddTable(object sender, EventArgs e)
        {
            Random ran = new Random();
            int id = ran.Next(10, 99);

            OrderModel.Table table = new OrderModel.Table();
            table.TableNum = id.ToString();
            table.Name = tableName.ToString();
            table.MaxNoP = tableVolume.ToString();

            if (TInfo.InsertTable(table))
            {
                Response.Write("<script>window.alert('" + "添加成功,您添加桌号为" + id + "');</script>");
            }
            else
            {
                Response.Write("<script>window.alert('添加失败');</script>");

            }
            BindData();
        }
        protected void UpdateTable(Object sender, DataGridCommandEventArgs E)
        {
            OrderModel.Table table = new OrderModel.Table();

            table.TableNum = tdg.DataKeys[(int)E.Item.ItemIndex].ToString();
            table.MaxNoP = ((TextBox)E.Item.Cells[5].Controls[0]).Text;
            table.Name = ((TextBox)E.Item.Cells[3].Controls[0]).Text;
            //编辑订单状态下拉菜单

            if (!TInfo.UpdateTable(table))
            {
                Response.Write("<script>window.alert('更新失败');</script>");
                Cancel(sender, E);
                return;
            }
            BindData();
        }

        protected void odg_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            odg.CurrentPageIndex = e.NewPageIndex;
            BindData(); ;
        }
        protected void tdg_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            tdg.CurrentPageIndex = e.NewPageIndex;
            BindData(); ;
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


        protected void Reset(object sender, EventArgs e)
        {
            BindData();
        }
        protected void btnIntroduction_Click(object sender, EventArgs e)
        {

            //获取被触发的Button对象  
            Button b = (Button)sender;
            if (b.ID == "btnIntroduction")
            {
                //激活View1  
                //sign.Text = "1";
                MultiView1.SetActiveView(View1);
            }
            else
            {
                //sign.Text = "0";
                //激活View2  
                MultiView1.SetActiveView(View2);
            }
            BindData();
        }
    }

}