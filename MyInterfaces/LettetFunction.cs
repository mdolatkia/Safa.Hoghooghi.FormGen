using ModelEntites;

using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyInterfaces
{

    public class LetterFunctionParam : BaseCodeFunctionParam
    {
        public LetterDTO Letter { set; get; }
        public LetterFunctionParam(LetterDTO letter, DR_Requester requester) : base(requester)
        {
            Letter = letter;
        }
        public object[] OtherParams { set; get; }
    }
    //public class LetterFunctionResult
    //{
    //    //public Type ResultType { set; get; }
    //    public string Message { set; get; }
    //    public Object Result { set; get; }
    //}
    public class LetterConvertToExternalResult : CodeFunctionResult
    {
        //public bool ExternalResult { set; get; }
        //public string Message { set; get; }
        public string ExternalCode { set; get; }
    }
}
