
using CommonDefinitions.UISettings;
using ModelEntites;
using MyUILibrary.EntityArea.Commands;

using ProxyLibrary;
using System;
using System.Collections.Generic;
using Telerik.Reporting;
using MyUILibraryInterfaces.EntityArea;

namespace MyUILibrary.EntityArea
{
    public interface I_View_ReportArea
    {
        string Title { set; }
       
        //event EventHandler SearchCommandRequested;
    }


    public interface I_View_InternalReportArea : I_View_ReportArea
    {
        event EventHandler OrderColumnsChanged;
        event EventHandler<Exception> ExceptionThrown;
        string GetSortText { get; }
        int GetOrderColumnID { get; }
        void SetOrderSorts(List<string> list);
        void SetOrderColumns(List<Tuple<int, string>> columns);
        bool OrderColumnsVisibility { set; get; }
        void SetReportSource(string reportEngine, ReportSource reportSource);
    //    void AddGenerealSearchAreaView(object view);
    }

    public interface I_View_ExternalReportArea : I_View_ReportArea
    {
        void SetReportSource(string URL);
     //   void AddGenerealSearchAreaView(object view);
    }
}
