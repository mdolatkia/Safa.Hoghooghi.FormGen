using ModelEntites;
using System;
using System.Collections.Generic;
namespace ProxyLibrary
{
    public class RR_ReportRequest : BaseRequest
    {
        public RR_ReportRequest(DR_Requester Requester) : base(Requester)
        {

        }

        public int ReportID;

        //////public List<DataManager.DataPackage.DP_PackageCategory> Categories;

    }

}
