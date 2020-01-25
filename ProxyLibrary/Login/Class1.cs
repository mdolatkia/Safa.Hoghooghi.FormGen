using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyLibrary
{
    public class LoginResult
    {
        public bool Successful { set; get; }
        public string Message { set; get; }

        public int UserID { set; get; }
    }
}
