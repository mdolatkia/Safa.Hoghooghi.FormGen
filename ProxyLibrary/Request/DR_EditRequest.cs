using System;
using System.Collections.Generic;
namespace ProxyLibrary
{
    public class DR_EditRequest : BaseRequest
    {

        public DR_EditRequest(DR_Requester Requester) : base(Requester)
        {

        }
        //public List<DataManager.DataPackage.DP_Package> AddPackages;


        public List<DP_DataRepository> EditPackages;
        //public List<DP_DataRepository> RemovedPackages;

        //public List<DataManager.DataPackage.DP_Package> DeletePackages;

    }
    public class DR_DeleteRequest : BaseRequest
    {
        public DR_DeleteRequest(DR_Requester Requester) : base(Requester)
        {

        }

        //public List<DataManager.DataPackage.DP_Package> AddPackages;


        public List<DP_DataRepository> DataItems;
        //public List<DP_DataRepository> RemovedPackages;

        //public List<DataManager.DataPackage.DP_Package> DeletePackages;

    }

    public class DR_DeleteInquiryRequest : BaseRequest
    {
        public DR_DeleteInquiryRequest(DR_Requester Requester) : base(Requester)
        {

        }
        public List<DP_BaseData> DataItems;
    }
}
