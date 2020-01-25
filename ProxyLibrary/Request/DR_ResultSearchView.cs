using System;
using System.Collections.Generic;
namespace ProxyLibrary
{
    public class DR_ResultSearchView : BaseResult
    {


        public DR_ResultSearchView()
        {
            ResultDataItems = new List<DP_DataView>();
        }


        public List<DP_DataView> ResultDataItems;

    }
    public class DR_ResultSearchExists : BaseResult
    {

        public bool ExistsResult { set; get; }
    }
    public class DR_ResultSearchCount : BaseResult
    {

        public int ResultCount { set; get; }
    }
    //public class DR_ResultSearchEdit : BaseResult
    //{


    //    public DR_ResultSearchEdit()
    //    {
    //        ResultDataItems = new List<DP_DataRepository>();
    //    }


    //    public List<DP_DataRepository> ResultDataItems;


    //    public int ResultCount;

    //}
    public class DR_ResultSearchKeysOnly : BaseResult
    {


        public DR_ResultSearchKeysOnly()
        {
            ResultDataItems = new List<DP_DataView>();
        }


        public List<DP_DataView> ResultDataItems;


        public int ResultCount;

    }
    public class DR_ResultSearchFullData : BaseResult
    {
        public DR_ResultSearchFullData()
        {
            ResultDataItems = new List<DP_DataRepository>();
        }
        public List<DP_DataRepository> ResultDataItems;
        public int ResultCount;

    }
    //public class DR_ResultSearchByRelatinoshipTail : BaseResult
    //{

    //    public DR_ResultSearchByRelatinoshipTail()
    //    {
    //        ResultDataItems = new List<DP_DataView>();
    //    }

    //    public List<DP_DataView> ResultDataItems;

    //    public int ResultCount;

    //}
    //public class DR_ResultSearchViewByRelatinoshipTail : BaseResult
    //{


    //    public DR_ResultSearchViewByRelatinoshipTail()
    //    {
    //        ResultDataItems = new List<DP_DataView>();
    //    }


    //    public List<DP_DataView> ResultDataItems;


    //    public int ResultCount;

    //}
    public class DR_ResultByRelationView : BaseResult
    {


        public DR_ResultByRelationView()
        {
            ResultDataItems = new List<DP_DataRepository>();
        }


        public List<DP_DataRepository> ResultDataItems;


        public int ResultCount;

    }
}
