using ModelEntites;
using MyCommonWPFControls;
using MyUILibrary.EntityArea;
using MyUILibraryInterfaces.EntityArea;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibraryInterfaces.LogReportArea
{


    public interface I_LogReportArea
    {
        LogReportAreaInitializer AreaInitializer { set; get; }
        I_View_LogReportArea View { set; get; }
    }

    public interface I_View_LogReportArea
    {
        event EventHandler Confirmed;
        event EventHandler ExitRequested;
        event EventHandler<DataLogDTO> DetailsClicked;
        event EventHandler<DataLogDTO> PackageClicked;
        void SetMainTypeItems(List<DataLogType> columns);
        DataLogType? SelectedMainType { set; get; }
        bool? withMajorException { set; get; }
        bool? withMinorException { set; get; }
        DateTime? FromData { set; get; }
        DateTime? ToDate { set; get; }
        void AddDataSelector(object view);
        void AddEntitySelector(object view);
        void AddUserSelector(object view);
        void SetColumnsItems(List<ColumnDTO> columns);
        ColumnDTO SelectedColumn { set; get; }
        void SetLogs(List<DataLogDTO> list);
        void RemoveDataSelector();
        void AddPackageDataLogs(List<DataLogDTO> logData);
        bool PackageDatagridVisiblity { set; get; }
    }
    public interface I_View_EditLogReportDetails
    {
        event EventHandler<EditDataItemColumnDetailsDTO> ColmnSelected;
        bool ColumnParameterVisibility { set; get; }
        event EventHandler ExitRequested;
        void AddExceptionLogs(List<EditDataItemExceptionLogDTO> exceptions);
        void AddColumnEditLogs(List<EditDataItemColumnDetailsDTO> columns);
        bool OldValueColumnVisibility { set; get; }

        void AddColumnFormulaParameters(List<FormulaUsageParemetersDTO> formulaUsageParemeters);
    }
    public interface I_View_ArchiveLogReportDetails
    {
        event EventHandler ExitRequested;

        void AddExceptionLogs(List<ArchiveItemLogDTO> exceptions);
     
    }
    public class LogReportAreaInitializer
    {
        public DP_DataView DataItem { get; set; }
        public int EntityID { get; set; }
    }



}
