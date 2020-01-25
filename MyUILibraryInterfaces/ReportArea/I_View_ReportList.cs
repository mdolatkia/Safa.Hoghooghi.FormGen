
using CommonDefinitions.UISettings;
using ModelEntites;

using System;
using System.Collections.Generic;
namespace MyUILibrary.EntityArea
{
    public interface I_View_ReportList
    {
        void SetReportList(List<EntityReportDTO> reports);
        event EventHandler<ReportClickedArg> ReportClicked;
    }
    public class ReportClickedArg : EventArgs
    {
        public int ReportID { set; get; }
    }
}
