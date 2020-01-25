using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyInterfaces
{
    public class DatabaseFunctionResult
    {
        //public bool ExceptionWithMessage { set; get; }
        public Exception ExecutionException { get; set; }
        //public string Message { set; get; }
        public Object Result { set; get; }
    }
}
