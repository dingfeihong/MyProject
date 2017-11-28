using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Account
    {
        public Account(){
            User = "";
            Password = 0;
            Phone = "";
            AType= "";
        }
        #region Account
        private String _User;//用户名
        private int _Password;//密码
        private String _Phone;//手机号
        private String _AType;//类型
        public String User
        {
            set { _User = value; }
            get { return _User; }
        }
        public int Password
        {
            set { _Password = value; }
            get { return _Password; }
        }
        public String Phone
        {
            set { _Phone = value; }
            get { return _Phone; }
        }
        public String AType
        {
            set { _AType = value; }
            get { return _AType; }
        }

        #endregion
    }


}
