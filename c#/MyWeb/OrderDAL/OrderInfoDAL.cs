using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using OrderModel;
using Tool;
namespace OrderDAL
{
    public class OrderInfoDAL
    {
        //OleDbHelper.OleDbHelper db=new OleDbHelper.OleDbHelper();
        public OrderInfoDAL() { }

        public DataTable SelectAll()
        {
            String str = "select * from [order]";
            return OleDbHelper.GetDataTable(str);
        }
        public int SelectAllNum()
        {
            String str = "select count(*) from [order]";
            return OleDbHelper.GetScalar(str);
        }

        public DataTable Search(String searchStr)
        {
            String str = "select * from [order]" + searchStr;
            return OleDbHelper.GetDataTable(str);
        }
        public int SearchNum(String searchStr)
        {
            String str = "select count(*)  from [order]" + searchStr;
            return OleDbHelper.GetScalar(str);
        }

        public int DeleteOrder(String id)
        {
            String str =
                "delete from [order] where 单号 = " + id;
            return OleDbHelper.ExecuteCommand(str);
        }

        public int InsertOrder(Order tmp)
        {
            //在上下3h内无其他订单
            DateTime dt_h = Convert.ToDateTime(tmp.ScheTime);
            DateTime dt_l = Convert.ToDateTime(tmp.ScheTime);

            String time_h = dt_h.AddHours(+3d).ToString("yyyy-MM-dd HH:mm:ss");
            String time_l = dt_l.AddHours(-3d).ToString("yyyy-MM-dd HH:mm:ss");
            //   Response.Write("<script>window.confirm('确实要删除吗?');</script>");
            String str =
                "select 桌号,可容人数 from [table] " +
                "where 可容人数>=" + tmp.PeopleNum +
                " AND 桌号<> ALL" +
                "(select 桌号 from [order] where " +
                " 预定时间 < #" + time_h +
                "# AND 预定时间 > #" + time_l +
                "# ) order by 可容人数";
            OleDbDataReader reader = OleDbHelper.GetReader(str);
            if (reader.Read())
                tmp.TableNum = reader["桌号"].ToString();

            str =
                "insert into [order](单号,下单时间,预定时间,人数,下单人,联系电话,桌号) " +
                "values ('" + tmp.ID + "','"
                + tmp.OrderTime + "','"
                + tmp.ScheTime + "','"
                + tmp.PeopleNum + "','"
                + tmp.People + "','"
                + tmp.phone + "','"
                + tmp.TableNum + "')";

            OleDbHelper.ExecuteCommand(str);
            return int.Parse(tmp.TableNum);
        }
        public bool UpdateOrder(Order tmp)
        {
            //在上下3h内无其他订单
            DateTime dt_h = Convert.ToDateTime(tmp.ScheTime);
            DateTime dt_l = Convert.ToDateTime(tmp.ScheTime);

            String time_h = dt_h.AddHours(+3d).ToString("yyyy-MM-dd HH:mm:ss");
            String time_l = dt_l.AddHours(-3d).ToString("yyyy-MM-dd HH:mm:ss");
           
            //查看更新后日期是否冲突
            String str =
                "select count(*) from [order] where" +
                " 预定时间 < #" + time_h +
                "# AND 预定时间 > #" + time_l +
                "#  AND 桌号 = " + tmp.TableNum +
                " AND 单号 <> " + tmp.ID;
            int n = OleDbHelper.GetScalar(str);
            if (n > 0)
            {                
                return false;
            }

            //更新
            str = "UPDATE [order] SET " +
                "下单时间='" + tmp.OrderTime +
                "',预定时间='" + tmp.ScheTime +
                "',人数='" + tmp.PeopleNum +
                "',订单状态='" + tmp.State +
                "',下单人='" + tmp.People +
                "',联系电话='" + tmp.phone +
                "',桌号='" + tmp.TableNum +
                "' where 单号=" + tmp.ID;
            OleDbHelper.ExecuteCommand(str);
            return true;
        }

        public OleDbDataReader BindDropList()
        {
            String strSQL = "select 桌号 from [table]";
            return OleDbHelper.GetReader(strSQL);
        }
    }
}
