using MyInterfaces;
using MyUILibrary.EntityArea;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibraryInterfaces
{

    public class CommandFunctionParam: BaseCodeFunctionParam
    {
        public CommandFunctionParam(I_EditEntityArea editEntityArea, DR_Requester requester) : base(requester)
        {
            EditEntityArea = editEntityArea;
        }
        public I_EditEntityArea EditEntityArea { set; get; }
        public object[] OtherParams { set; get; }
    }
    //public class CommandFunctionResult
    //{
    //    public Type ResultType { set; get; }
    //    public string Message { set; get; }
    //    public Object Result { set; get; }
    //}

}
