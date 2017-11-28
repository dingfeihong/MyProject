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
    public static class BookInfoBLL
    {

        public static DataTable GetAllTable(int size)
        {

            String str = "select count(*) from [book]"; 
            int n = OleDbHelper.GetScalar(str);

            str = "select * from [book]";           
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
        
        public static bool Update(Book tmp)
        {
            String str = "UPDATE [book] SET 书名 = '" + tmp.Name +
                "',作者 = '" + tmp.Writer +
                "',数量 = " + tmp.Count +
                " where 图书编号 = " + tmp.Id;
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
        public static bool AddBook(Book tmp)
        {
            String str = "INSERT INTO [book](图书编号,书名,作者,数量)" +
                   "values ('" + tmp.Id + "','"
                   + tmp.Name + "','"
                   + tmp.Writer + "','"
                   + tmp.Count + "')";
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
            String str ="delete from [book] where 图书编号 = " + id;         
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
        public static DataTable Search(String tmp, int size)
        {
            if (tmp == "") return GetAllTable(size);
            int num = -1;
            if(OleDbHelper.IsNum(tmp))
                num=int.Parse(tmp);            
            String str = "select * from [book] where"+
                " 书名 = '" + tmp +
                "' OR 作者 = '" + tmp +
                "' OR 图书编号 = " + num;
            DataTable dt=OleDbHelper.GetDataTable(str);

            str = "select count(*) from [book] where" +
                " 书名 = '" + tmp +
                "' OR 作者 = '" + tmp +
                " OR 图书编号 = " + num;
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
