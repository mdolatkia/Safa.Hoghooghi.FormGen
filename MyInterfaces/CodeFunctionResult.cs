using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyInterfaces
{
    public class CodeFunctionResult
    {
        //public Type ResultType { set; get; }
        //public string Message { set; get; }
        //public bool ExceptionWithMessage { set; get; }
        public Exception ExecutionException { set; get; }
        public  Object Result { set; get; }
    }
}
