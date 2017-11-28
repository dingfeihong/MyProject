using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Model;
using Tool;
using System.Data.OleDb;
namespace LogLayer
{
    public static class OrderInfoBLL
    {
        public static DataTable GetAllTable(int size)
        {

            String str = "select count(*) from [order]";
            int n = OleDbHelper.GetScalar(str);

            str = "select * from [order]";
            DataTable dt = OleDbHelper.GetDataTable(str);

            int count = size - n % size;
            int i = 0;
            for (; i < count && (n == 0 || count != size); i++)//插入空
            {
                DataRow dr = null;
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public static bool AddOrder(Order tmp)
        {
            String str = "INSERT INTO [order](单号,购书时间,购书人,图书编号)"+
                "values ('" + tmp.ID + "','"
                +tmp.OrderTime+"','"
                + tmp.Customer + "','"
                + tmp.BookNum +"')";
            try
            {
                OleDbHelper.ExecuteCommand(str);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public static bool Update(Order tmp)
        {
            String str = "UPDATE [order] SET 购书时间 = '" + tmp.OrderTime +
                "',购书人 = '" + tmp.Customer +
                "',图书编号 = " + tmp.BookNum+
                " where 单号 = " + tmp.ID;
            //  OleDbHelper.ExecuteCommand(str);
            try
            {
                OleDbHelper.ExecuteCommand(str);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public static bool Delete(String id)
        {
            String str = "delete from [order] where 单号 = " + id;
            try
            {
                OleDbHelper.ExecuteCommand(str);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public static OleDbDataReader BindDropList_Tnum()
        {
            String strSQL = "select 图书编号 from [book]";
            return OleDbHelper.GetReader(strSQL);
        }
        public static DataTable Search(String info,String time, int size)
        {
            int num = -1;
            if (info != ""&&OleDbHelper.IsNum(info))
                num = int.Parse(info);
            String condition = " 单号 <> -1"; 
            if (info!= "单号/图书编号/购书人"&&info!="")
            {
                condition += " AND (" +
                    " 单号 = " + num +
                    " OR 图书编号 = " + num +
                    " OR 购书人 = '" + info + "')";
            }
            if (time != "购书日期"&&time!="")
            {
                condition += " AND" +
                " 购书时间 < #" + time + " 23:59:00" +
                "# AND 购书时间 > #" + time + " 0:00:01" +
                "#";
            }

            String str = "SELECT * FROM [order] WHERE " + condition;
            DataTable dt = OleDbHelper.GetDataTable(str);

            str = "SELECT COUNT(*) FROM [order] WHERE " + condition;

            int n = OleDbHelper.GetScalar(str);
            int count = size - n % size;

            int i = 0;
            for (; i < count && (n == 0 || count != size); i++)
            {
                DataRow dr = null;
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
