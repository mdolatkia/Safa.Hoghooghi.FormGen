using System;
using System.Collections.Generic;
namespace ProxyLibrary
{
    public class BaseResult
    {
        public BaseResult()
        {
            Details = new List<ProxyLibrary.ResultDetail>();
        }
        public string Message { set; get; }
        public Enum_DR_ResultType Result { set; get; }
        public List<ResultDetail> Details { set; get; }
    }
    public class ResultDetail
    {
        public string Title { set; get; }
        public string Description { set; get; }
        public string TechnicalDescription { set; get; }
    }
    //public class BaseDataItemResult: BaseResult
    //{

    //    public BaseDataItemResult()
    //    {
    //        //ResultItems = new List<DataItemResult>();
    //    }

    //    //public List<DataItemResult> ResultItems { set; get; }
    //}
    //public class DataItemResult
    //{
    //    public DP_DataRepository DataItem { set; get; }
    //    public string Message { set; get; }
    //    public Enum_DR_ResultType Result { set; get; }

    //}
}
