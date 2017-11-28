using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using OrderDAL;
using OrderModel;
namespace OrderBLL
{
    public class TableInfoBLL
    {
        public static bool IsNum(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (!Char.IsNumber(text, i))
                    return true; //输入的不是数字  
            }
            return false; //否则是数字
        }
        private TableInfoDAL TInfoDAL = new TableInfoDAL();
        public DataTable GetAllTable()
        {
            return TInfoDAL.SelectAll();
        }
        public DataTable GetAllTable(int size)
        {
            int n = TInfoDAL.SelectAllNum();
            DataTable dt = GetAllTable();

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
        public DataTable SearchTable(String infoStr, int size)
        {
            String where = " where 桌号 <> -1 ";
            String num = infoStr;
            if (IsNum(infoStr))//当infoStr非数字时
            {
                num = "-1";
            }
            if (infoStr != "桌号/名字" && infoStr != "")
                where += " and ( 桌号 = " + num
                    + " or 名字='" + infoStr + "')";

            int n = TInfoDAL.SearchNum(where);//计算搜索到的数量
            DataTable dt = TInfoDAL.Search(where); //搜索

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
        public int DelectTable(String id)
        {
            return TInfoDAL.DeleteTable(id);
        }
        public bool InsertTable(Table tmp)
        {
            return TInfoDAL.InsertTable(tmp);
        }
        public bool UpdateTable(Table tmp)
        {
            return TInfoDAL.UpdateTable(tmp);
        }
    }
}
