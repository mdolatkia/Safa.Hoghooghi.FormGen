using System;
using System.Collections.Generic;
namespace ProxyLibrary
{
    public class DR_ResultEdit: BaseResult
    {
        public DR_ResultEdit()
        {
            UpdatedItems = new List<ProxyLibrary.DP_BaseData>();
        
        }
        public List<DP_BaseData> UpdatedItems;


    }
    //public class DR_ResultEditTuple
    //{
    //    public DR_ResultEditTuple(Guid guid, EntityInstanceProperty columnValue)
    //    {
    //        GUID = guid;
    //        ColumnValue = columnValue;
    //    }
    //    public Guid GUID { set; get; }
    //    public EntityInstanceProperty ColumnValue { set; get; }
    //}

    public class DR_ResultDelete : BaseResult
    {
        public DR_ResultDelete()
        {

        }
    }
    public class DR_DeleteInquiryResult : BaseResult
    {
        public DR_DeleteInquiryResult()
        {
            DataTreeItems = new List<ProxyLibrary.DP_DataRepository>();
        }
        public bool Loop { set; get; }
        public List<DP_DataRepository> DataTreeItems { set; get; }
    }
}
