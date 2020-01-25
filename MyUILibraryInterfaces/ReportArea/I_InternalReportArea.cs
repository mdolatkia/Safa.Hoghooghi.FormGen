using ModelEntites;
using MyUILibrary.EntityArea.Commands;
using MyUILibraryInterfaces.EntityArea;
using ProxyLibrary;
using System;
using System.Collections.Generic;
namespace MyUILibrary.EntityArea
{
    public interface I_InternalReportArea
    {
        I_GeneralEntitySearchArea GeneralEntitySearchArea { set; get; }
        InternalReportAreaInitializer AreaInitializer { set; get; }
        //void SetAreaInitializer();
        //I_SearchEntityArea SearchEntityArea { set; get; }
       object MainView { set; get; }
        I_View_InternalReportArea View { set; get; }
    }


}
