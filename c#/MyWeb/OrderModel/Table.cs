using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderModel
{
    public class Table
    {
        public Table() 
        {
            Name = "";
            MaxNoP = "";
            TableNum = "";
        }
        #region Table
        private String _Name;//名字
        private String _MaxNoP;//容纳人数
        private String _TableNum;//桌号

        public String Name
        {
            set { _Name = value; }
            get { return _Name; }
        }
        public String MaxNoP
        {
            set { _MaxNoP = value; }
            get { return _MaxNoP; }
        }
        public String TableNum
        {
            set { _TableNum = value; }
            get { return _TableNum; }
        }
        #endregion
    }
}
