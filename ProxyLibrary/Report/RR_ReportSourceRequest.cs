using ModelEntites;
using System;
using System.Collections.Generic;
namespace ProxyLibrary
{
    public class RR_ReportSourceRequest : BaseRequest
    {
        public RR_ReportSourceRequest() : base()
        {

        }
        public RR_ReportSourceRequest(DR_Requester Requester) : base(Requester)
        {

        }
        public int OrderByEntityViewColumnID { set; get; }
        public Enum_OrderBy SortType { set; get; }
        public int ReportID;

        public DP_SearchRepository SearchDataItems;

    }

}
