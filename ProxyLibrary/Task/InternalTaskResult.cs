using System;
using System.Collections.Generic;
namespace ProxyLibrary
{
    public class InternalTaskResult
    {


        public InternalTaskResult()
        {
        }


        public bool Result;


        public string Message;

    }


    public class RequestRegisterResult
    {


        public RequestRegisterResult()
        {
            LogGuidPairs = new List<ProxyLibrary.LogGuidPair>();
        }
        public List<LogGuidPair> LogGuidPairs { set; get; }

        public bool Result;


        public string Message;

    }
    public class LogGuidPair
    {
        public LogGuidPair(int logId, Guid guid)
        {
            LogID = logId;
            GUID = guid;
        }
        public int LogID { set; get; }
        public Guid GUID { set; get; }
    }
}
