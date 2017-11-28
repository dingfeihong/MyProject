using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using OrderModel;
using Tool;
namespace OrderDAL
{
    public class TableInfoDAL
    {
        //OleDbHelper.OleDbHelper db=new OleDbHelper.OleDbHelper();
        public TableInfoDAL() { }

        public DataTable SelectAll()
        {
            String str = "select * from [table]";
            return OleDbHelper.GetDataTable(str);
        }
        public int SelectAllNum()
        {
            String str = "select count(*) from [table]";
            return OleDbHelper.GetScalar(str);
        }

        public DataTable Search(String searchStr)
        {
            String str = "select * from [table]" + searchStr;
            return OleDbHelper.GetDataTable(str);
        }
        public int SearchNum(String searchStr)
        {
            String str = "select count(*)  from [table]" + searchStr;
            return OleDbHelper.GetScalar(str);
        }

        public int DeleteTable(String id)
        {
            String str =
                "delete from [table] where 桌号 = " + id;
            return OleDbHelper.ExecuteCommand(str);
        }

        public bool InsertTable(Table tmp)
        {
            bool sign = true;
            String strSQL =
            "insert into [table](桌号,名字,可容人数) " +
            "values ('" + tmp.TableNum + "','"
            + tmp.Name + "','"
            + tmp.MaxNoP + "')";
            OleDbHelper.ExecuteCommand(strSQL);
            return sign;
        }
        public bool UpdateTable(Table tmp)
        {
            bool sign = true;
            String strSQL;

            strSQL = "UPDATE [table] SET " +
                "名字='" + tmp.Name +
                "',可容人数='" + tmp.MaxNoP +
                "' where 桌号=" + tmp.TableNum;

            OleDbHelper.ExecuteCommand(strSQL);
            return sign;
        }
    }
}
