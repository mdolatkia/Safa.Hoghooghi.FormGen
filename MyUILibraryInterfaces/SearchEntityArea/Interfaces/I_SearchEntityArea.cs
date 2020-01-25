using ModelEntites;
using MyUILibrary.EntityArea.Commands;

using ProxyLibrary;
using System;
using System.Collections.Generic;
namespace MyUILibrary.EntityArea
{
    public interface I_BaseSearchEntityArea
    {


        SearchEntityAreaInitializer SearchInitializer { set; get; }
        void ClearSearchData();
        void SetAreaInitializer(SearchEntityAreaInitializer initParam);
    }
    public interface I_SearchEntityArea : I_BaseSearchEntityArea
    {
        void OnSearchDataDefined(DP_SearchRepository logicPhrase);
        DP_SearchRepository LastSearch { set; get; }

        event EventHandler<SearchDataArg> SearchDataDefined;
        I_View_SearchEntityArea SearchView { set; get; }
        I_SimpleSearchEntityArea SimpleSearchEntityArea { set; get; }
        I_AdvancedSearchEntityArea AdvancedSearchEntityAre { set; get; }
        bool IsSimpleSearchActiveOrAdvancedSearch { get; }
        void ShowSearchRepository(DP_SearchRepository searchRepository);

    }
    public interface I_AdvancedAndRawSearchEntityArea : I_BaseSearchEntityArea
    {
        //باید از این ایونت استفاده شود بجای کار با متغیر های داخلی مانندRawSearchEntityArea 
        //بهتر است در اینشیالایزر رابطه گنجانده شود و به هنگام بازگرداندن 
        //DP_SearchRepository
        //در صورت وجود رابطه مقادیر سورس ریلیشن و .. ست وشود

        event EventHandler<SearchDataArg> SearchDataDefined;
        //event EventHandler<AdvanceOrRawArg> SearchDataDefined;
        I_View_SearchEntityArea SearchView { set; get; }
        I_RawSearchEntityArea RawSearchEntityArea { set; get; }
        I_AdvancedSearchEntityArea AdvancedSearchEntityAre { set; get; }

    }
    public interface I_SimpleSearchEntityArea : I_BaseSearchEntityArea
    {
        EntitySearchDTO EntitySearch { get; }
        void OnSearchDataDefined(DP_SearchRepository logicPhrase);
        DP_SearchRepository GetSearchRepository();
        bool ShowSearchRepository(DP_SearchRepository item);
        LogicPhrase GetQuickSearchLogicPhrase(string text, EntitySearchDTO entitySearch);
        event EventHandler<SearchDataArg> SearchDataDefined;
        List<I_Command> SearchCommands
        {
            get;
            set;
        }

        I_View_SimpleSearchEntityArea SimpleSearchView { set; get; }

        List<RelationshipSearchColumnControl> RelationshipColumnControls
        {
            set;
            get;
        }
        List<SimpleSearchColumnControl> SimpleColumnControls
        {
            set;
            get;
        }

        //  bool PreDefinedSearchIsApplicable(EntityPreDefinedSearchDTO message);
  //      bool ApplyPreDefinedSearch(EntityPreDefinedSearchDTO message);
        //List<PreDefinedSearchColumns> GetSearchColumns();
    }
    public interface I_RawSearchEntityArea : I_BaseSearchEntityArea
    {
        void OnSearchDataDefined(List<SearchProperty> logicPhrase);
        List<SearchProperty> GetSearchRepository();

        event EventHandler<SearchPropertyArg> SearchDataDefined;
        List<I_Command> SearchCommands
        {
            get;
            set;
        }
        I_View_SimpleSearchEntityArea RawSearchView { set; get; }
        List<SimpleSearchColumnControl> SimpleColumnControls
        {
            set;
            get;
        }

    }
    public interface I_AdvancedSearchEntityArea : I_BaseSearchEntityArea
    {
        void OnSearchDataDefined(DP_SearchRepository logicPhrase);
        DP_SearchRepository GetSearchRepository();

        bool ShowSearchRepository(DP_SearchRepository item);
      //  void ApplyPreDefinedSearch(EntityPreDefinedSearchDTO message);

        event EventHandler<SearchDataArg> SearchDataDefined;
        List<I_Command> SearchCommands
        {
            get;
            set;
        }
        I_View_AdvancedSearchEntityArea AdvancedSearchView { set; get; }

    }
    public class SimpleSearchColumnControl : BaseColumnControl
    {
        public SimpleSearchColumnControl()
        {

            Operators = new List<SimpleSearchOperator>();
        }
        public ColumnDTO Column { set; get; }
        public EntitySearchColumnsDTO EntitySearchColumn { get; set; }
        public I_SimpleControlManager ControlManager { get; set; }
        public List<SimpleSearchOperator> Operators { get; set; }
        //public SearchEnumerableType SearchEnumerableType { get; set; }
    }
    public class RelationshipSearchColumnControl : BaseColumnControl
    {
        public I_EditEntityArea EditNdTypeArea { set; get; }
        public RelationshipDTO Relationship { set; get; }
        public List<ColumnDTO> Columns { set; get; }
        //public event EventHandler<ColumnValueChangeArg> ValueChanged;
        public RelationshipSearchColumnControl()
        {
            //ColumnSetting = new ColumnSetting();

            //Columns = new List<ColumnDTO>();
        }
        public EntitySearchColumnsDTO EntitySearchColumn { get; set; }

        public I_RelationshipControlManager ControlManager { get; set; }
        public EntityRelationshipTailDTO RelationshipTail { set; get; }
    
        //public bool IsFake { set; get; }
        //public SearchEnumerableType SearchEnumerableType { get; set; }

        //public List<AG_RelatedConttol> RelatedUIControls { set; get; }
    }

    public class NullColumnControl : SimpleSearchColumnControl
    {
        public NullColumnControl()
        {

        }
        public ColumnDTO FakeColumn { set; get; }

    }
    public class RelationCheckColumnControl : SimpleSearchColumnControl
    {
        public RelationCheckColumnControl()
        {

        }
        public ColumnDTO FakeColumn { set; get; }

    }
    public class RelationCountCheckColumnControl : SimpleSearchColumnControl
    {
        public RelationCountCheckColumnControl()
        {

        }
        //   public SearchEnumerableType EnumerableType { get; set;}
        public ColumnDTO FakeColumn { set; get; }

    }


    //public class UISearchNullControlPackage : UIControlPackageForSimpleColumn
    //{
    //    public event EventHandler NullSelected;
    //}
    //public enum NumericOperator
    //{
    //    Equals,
    //    BiggerThan,
    //    SmallerThan,
    //    EqualAndBiggerThan,
    //    EqualAndSmallerThan,
    //    NotEquals,
    //    NullValue
    //}
    public enum BooleanOperator
    {
        Equals,
        NullValue
    }
    //public class SearchAreaRelationSource
    //{
    //    public SearchAreaRelationSource()
    //    {
    //        //RelationData = new List<DP_SearchRepository>();
    //        //SourceRelationColumns = new List<ColumnDTO>();
    //        //RelationshipColumns = new List<RelationshipColumnDTO>();
    //    }
    //    //public Enum_DP_RelationSide SourceRelationSide { set; get; }
    //    public int SourceEntityID { set; get; }
    //    public int SourceTableID { set; get; }
    //    //public List<RelationshipColumnDTO> RelationshipColumns { set; get; }
    //    //public RelationshipDTO Relationship { set; get; }
    //    public int TargetEntityID { set; get; }
    //    public int TargetTableID { set; get; }
    //    public int RelationshipID { set; get; }
    //    //public List<ColumnDTO> TargetRelationColumns { set; get; }

    //    public Enum_RelationshipType RelationshipType
    //    {
    //        set;get;
    //    }
    //    public Enum_MasterRelationshipType MasterRelationshipType
    //    {
    //        get
    //        {
    //            return Relationship.MastertTypeEnum;
    //        }
    //    }
    //    public I_SearchEntityArea SourceSearchArea { set; get; }
    //    public DP_SearchRepository RelatedData { set; get; }

    //    public bool TargetSideIsMandatory { set; get; }

    //    //public bool SourceHoldsKeys { set; get; }
    //}


    public class SearchDataArg : EventArgs
    {
        public DP_SearchRepository SearchItems
        {
            get;
            set;
        }



    }
    public class SearchLogicArg : EventArgs
    {
        public LogicPhrase SearchItems
        {
            get;
            set;
        }

        //public AdvanceSearchNode AdvanceSearchNode
        //{
        //    get;
        //    set;
        //}


    }
    public class SearchPropertyArg
    {
        public List<SearchProperty> SearchItems
        {
            get;
            set;
        }
    }
    public class AdvanceOrRawArg
    {
        public AdvanceOrRaw SearchItems
        {
            get;
            set;
        }
    }
    public class AdvanceOrRaw
    {
        public LogicPhrase LogicPhrase { set; get; }
        public List<SearchProperty> SearchProperties { set; get; }
    }
}
