using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Order
    {
        public Order()
        {
            ID = 0;
            OrderTime = "";
            Customer = "";
           // phone = 0;
            BookNum = 0;
        }
        #region Order
        private int _ID;//单号
        private String _OrderTime;//购书时间
        private String _Customer;//购书人
      //  private int _phone;//联系电话
        private int _BookNum;//图书编号

        public int ID
        {
            set { _ID = value; }
            get { return _ID; }
        }
        public String OrderTime
        {
            set { _OrderTime = value; }
            get { return _OrderTime; }
        }
        public String Customer
        {
            set { _Customer = value; }
            get { return _Customer; }
        }
        //public int phone
        //{
        //    set { _phone = value; }
        //    get { return _phone; }
        //}
        public int BookNum
        {
            set { _BookNum = value; }
            get { return _BookNum; }
        }
        #endregion
    }
}
