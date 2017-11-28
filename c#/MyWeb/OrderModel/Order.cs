using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderModel
{
    public class Order
    {
        public Order()
        {
            ID = "";
            OrderTime = "";
            ScheTime = "";
            PeopleNum = "";
            State = "";
            People = "";
            phone = "";
            TableNum = "";
        }
        #region Order
        private String _ID;//单号
        private String _OrderTime;//下单时间
        private String _ScheTime;//预定时间
        private String _PeopleNum;//人数
        private String _State;
        private String _People;//下单人
        private String _phone;//联系电话
        private String _TableNum;//桌号

        public String ID
        {
            set { _ID = value; }
            get { return _ID; }
        }
        public String OrderTime
        {
            set { _OrderTime = value; }
            get { return _OrderTime; }
        }
        public String ScheTime
        {
            set { _ScheTime = value; }
            get { return _ScheTime; }
        }
        public String PeopleNum
        {
            set { _PeopleNum = value; }
            get { return _PeopleNum; }
        }
        public String State
        {
            set { _State = value; }
            get { return _State; }
        }
        public String People
        {
            set { _People = value; }
            get { return _People; }
        }
        public String phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        public String TableNum
        {
            set { _TableNum = value; }
            get { return _TableNum; }
        }
        #endregion
    }
}
