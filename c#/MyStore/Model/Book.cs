using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Book
    {
        public Book() 
        {
            Id = 0;
            Name = "";
            Writer = "";
            Count= 0;
        }
        #region Table
        private int _Id;//图书编号
        private String _Name;//书名
        private String _Writer;//作者
        private int _Count;//馆藏量
        public int  Id
        {
            set { _Id = value; }
            get { return _Id; }
        }
        public String Name
        {
            set { _Name = value; }
            get { return _Name; }
        }
        public String Writer
        {
            set { _Writer = value; }
            get { return _Writer; }
        }
        public int Count
        {
            set { _Count = value; }
            get { return _Count; }
        }
        
        #endregion
    }
}
