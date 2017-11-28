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
    public static class AccountInfoBLL
    {
        public static DataTable GetAllTable(int size)
        {

            String str = "select count(*) from [account]";
            int n = OleDbHelper.GetScalar(str);

            str = "select * from [account]";
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
        public static int login(String user, String psd)
        {
            String str = "select 账户类型  from [account] where 用户名 = '"
                + user+"' and 密码 = "
                + psd;

            OleDbDataReader reader =OleDbHelper.GetReader(str);//dt = OleDbHelper.GetDataTable(str);
                
            reader.Read();
            try {
                String type = reader["账户类型"].ToString();
                if (type == "管理员") return 1;
                else if (type == "用户") return 0;
               
            }catch(Exception)
            {
                return -1;
            }
       
            return -1;
        }
        public static bool Delete(String id)
        {
            String str = "delete from [account] where 图书编号 = '" + id+"'";
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
        public static bool Update(Account tmp)
        {
            String str = "UPDATE [account] SET 密码 = " + tmp.Password +
                ",联系电话 = '" + tmp.Phone+
                "',账户类型 = '"+tmp.AType+
                "' where 用户名 = '" + tmp.User+"'";
             // OleDbHelper.ExecuteCommand(str);
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
        public static bool AddAccount(Account tmp)
        {
            String str = "INSERT INTO [account](用户名,密码,联系电话,账户类型)" +
                   "values ('" + tmp.User + "',"
                   + tmp.Password + ",'"
                   + tmp.Phone + "','" 
                   + tmp.AType+"')";
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
        public static OleDbDataReader BindDropList()
        {
            String strSQL = "select 账户类型 from [account]";
            return OleDbHelper.GetReader(strSQL);
        }
        public static DataTable Search(String info,String type, int size)
        {
            String condition = " 用户名 <> '1'";
            if (info != "用户名"&& info !="")
            {
                condition += " AND (" +
                    " 用户名 = '" + info+
                    "')";
            }
            if (type != "全部类型")
                condition += " AND 账户类型 = '"+type+"'";

            String str = "SELECT * FROM [account] WHERE " + condition;
            DataTable dt = OleDbHelper.GetDataTable(str);

            str = "SELECT COUNT(*) FROM [account] WHERE " + condition;

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
        public static bool Register(Account tmp)
        {
            String str = "INSERT INTO [Account](用户名,密码,联系电话,账户类型)" +
                   "values ('" + tmp.User + "',"
                   + tmp.Password + ",'"
                   + tmp.Phone + "','"
                   + tmp.AType + "')";
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
    }
}
