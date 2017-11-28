using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using OrderDAL;
using OrderModel;
using System.Data.OleDb;
namespace OrderBLL
{
    public class OrderInfoBLL
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
        private OrderInfoDAL OInfoDAL = new OrderInfoDAL();
        public DataTable GetAllOrder()
        {
            int n = OInfoDAL.SelectAllNum();
            DataTable dt = OInfoDAL.SelectAll();
            return dt;
        }
        public DataTable GetAllOrder(int size)
        {
            int n = OInfoDAL.SelectAllNum();
            DataTable dt = GetAllOrder();

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
        public DataTable SearchOrder(String infoStr, String TableName, String OrderTime, String ScheTime, String State, int size)
        {
            String where = " where 单号 <> 1 ";
            String num = infoStr;
            if (IsNum(infoStr))//当infoStr非数字时
            {
                num = "-1";
            }
            if (infoStr != "单号/下单人/手机" && infoStr != "")
                where += " and ( 单号 = " + num
                    + " or 下单人='" + infoStr
                    + "' or 联系电话 ='" + infoStr + "')";

            if (TableName != "桌号" && TableName != "")
                where += " and 桌号 = " + TableName;

            if (ScheTime != "预订日期" && ScheTime != "")
                where += " and " +
                " 预定时间 < #" + ScheTime + " 22:00:00" +
                "# AND 预定时间 > #" + ScheTime + " 8:00:00" +
                "#";

            if (OrderTime != "下单日期" && OrderTime != "")
                where += " and " +
                " 下单时间 < #" + OrderTime + " 22:00:00" +
                "# AND 下单时间 > #" + OrderTime + " 8:00:00" +
                "#";

            if (State != "订单状态")
                where += " and 订单状态 = '" + State + "'";


            int n = OInfoDAL.SearchNum(where);//计算搜索到的数量
            DataTable dt = OInfoDAL.Search(where); //搜索

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
        public int DelectOrder(String id)
        {
            return OInfoDAL.DeleteOrder(id);
        }
        public int InsertOrder(Order tmp)
        {
            return OInfoDAL.InsertOrder(tmp);
        }
        public bool UpdateOrder(Order tmp)
        {            
            return OInfoDAL.UpdateOrder(tmp);
        }

        public OleDbDataReader BindDropList()
        {
            return OInfoDAL.BindDropList();
        }
    }
}
