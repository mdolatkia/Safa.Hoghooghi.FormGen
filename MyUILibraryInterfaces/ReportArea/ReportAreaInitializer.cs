
using CommonDefinitions.UISettings;
using ModelEntites;

using ProxyLibrary;
using System;
using System.Collections.Generic;
namespace MyUILibrary.EntityArea
{
    public class InternalReportAreaInitializer
    {
        public int EntityID;
        public DP_SearchRepository InitialSearchRepository { set; get; }
        public int ReportID { get; set; }
        //   public DP_SearchRepository PreDefinedSearch { get; set; }
        public bool ShowInitializeSearchRepository { set; get; }
        public bool UserCanChangeSearch { get; set; }
        public string Title { get; set; }
        public SearchableReportType ReportType { set; get; }


    }
    public class ExternalReportAreaInitializer
    {
        public int EntityID;
        public DP_SearchRepository InitialSearchRepository { set; get; }
        public int ReportID { get; set; }
        public bool ShowInitializeSearchRepository { set; get; }
        public bool UserCanChangeSearch { get; set; }
        public string Title { get; set; }
    }
}
