using ModelEntites;
using MyUILibrary.EntityArea.Commands;
using MyUILibraryInterfaces.EntityArea;
using ProxyLibrary;
using System;
using System.Collections.Generic;
namespace MyUILibrary.EntityArea
{
    public interface I_ExternalReportArea
    {
        I_GeneralEntitySearchArea GeneralEntitySearchArea { set; get; }
        ExternalReportAreaInitializer AreaInitializer { set; get; }
        //void SetAreaInitializer();
        //I_SearchEntityArea SearchEntityArea { set; get; }
        object MainView { set; get; }
        I_View_ExternalReportArea View { set; get; }
    }

    public interface I_DirectReportArea
    {
        DirectReportAreaInitializer AreaInitializer { set; get; }
        object MainView { set; get; }
      
    }
    public class DirectReportAreaInitializer
    {
        public int ReportID { set; get; }
        public DP_DataView DataInstance { set; get; }
    }
}
