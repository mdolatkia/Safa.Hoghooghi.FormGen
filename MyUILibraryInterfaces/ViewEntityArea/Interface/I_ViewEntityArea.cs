using ModelEntites;
using MyUILibrary.EntityArea.Commands;

using ProxyLibrary;
using System;
using System.Collections.Generic;
namespace MyUILibrary.EntityArea
{
    public interface I_ViewEntityArea
    {
        event EventHandler<DataSelectedEventArg> DataSelected;
        ViewEntityAreaInitializer ViewInitializer { set; get; }
        List<SimpleViewColumnControl> ViewColumnControls { set; get; }
        I_View_ViewEntityArea ViewView { set; get; }
        EntityListViewDTO EntityListView {  get; }
        List<I_ViewAreaCommand> ViewCommands
        {
            get;
            set;
        }
        void ClearSelectedData();
        List<DP_DataView> GetSelectedData();
        void AddData(List<DP_DataView> data, bool show);
        void ShowData1(List<DP_DataView> specificDate);
        void SetAreaInitializer(ViewEntityAreaInitializer initParam);
        void OnDataSelected(List<DP_DataView> dataItems);

    }
    public class DataSelectedEventArg : EventArgs
    {
        public DataSelectedEventArg()
        {
            DataItem = new List<DP_DataView>();
        }
        public List<DP_DataView> DataItem { set; get; }
        //public bool FromDataView { set; get; }
    }

    public class SimpleViewColumnControl : BaseColumnControl
    {
        //public event EventHandler<ColumnValueChangeArg> ValueChanged;
        public SimpleViewColumnControl()
        {

        }
        public I_SimpleControlManager ControlManager { set; get; }
        public EntityListViewColumnsDTO ListViewColumn { set; get; }
        //public ColumnDTO Column { set; get; }
        public string RelativeColumnName { set; get; }

        //public List<AG_RelatedConttol> RelatedUIControls { set; get; }
    }

    //public class RelationshipViewColumnControl : BaseColumnControl
    //{
    //    //public event EventHandler<ColumnValueChangeArg> ValueChanged;
    //    public RelationshipViewColumnControl()
    //    {
    //        Columns = new List<ColumnDTO>();
    //    }
    //    public List<ColumnDTO> Columns { set; get; }
    //    public RelationshipDTO Relationship { set; get; }


    //    //public List<AG_RelatedConttol> RelatedUIControls { set; get; }
    //}


    //public class ViewAreaRelationSource
    //{
    //    public ViewAreaRelationSource()
    //    {
    //        //RelationData = new List<DP_DataRepository>();
    //        SourceRelationColumns = new List<ColumnDTO>();
    //        TargetRelationColumns = new List<ColumnDTO>();
    //    }
    //    //public Enum_DP_RelationSide SourceRelationSide { set; get; }
    //    public int SourceEntityID { set; get; }
    //    public int SourceTableID { set; get; }
    //    public List<ColumnDTO> SourceRelationColumns { set; get; }
    //    public RelationshipDTO Relationship { set; get; }
    //    public int TargetEntityID { set; get; }
    //    public int TargetTableID { set; get; }
    //    public List<ColumnDTO> TargetRelationColumns { set; get; }

    //    public Enum_RelationshipType RelationshipType
    //    {
    //        get
    //        {
    //            return Relationship.TypeEnum;
    //        }
    //    }
    //    public I_ViewEntityArea SourceSearchArea { set; get; }
    //    public DP_DataRepository RelatedData { set; get; }

    //    public bool TargetSideIsMandatory { set; get; }

    //    //public bool SourceHoldsKeys { set; get; }
    //}


    //public class AG_View_SearchViewEntityArea
    //{
    //    public CommonDefinitions.BasicUISettings.PackageAreaUISetting BasicUISetting
    //    {
    //        get;
    //        set;
    //    }



    //}


}
