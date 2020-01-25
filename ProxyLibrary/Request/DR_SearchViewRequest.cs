using ModelEntites;
using System;
using System.Collections.Generic;
namespace ProxyLibrary
{
    public class DR_SearchViewRequest : BaseRequest
    {
        public DR_SearchViewRequest(DR_Requester Requester, DP_SearchRepository searchDataItems) : base(Requester)
        {
            SearchDataItems = searchDataItems;
            SecurityMode = SecurityMode.View;
        }
        public DP_SearchRepository SearchDataItems;
        //public int EntityID;
        public int EntityViewID { set; get; }
        public int OrderByEntityViewColumnID { set; get; }
        public Enum_OrderBy SortType { set; get; }

        public int MaxDataItems { set; get; }
    }

    //public class DR_SearchViewByRelationshipTailRequest : BaseRequest
    //{
    //    public DR_SearchViewByRelationshipTailRequest(DR_Requester Requester) : base(Requester)
    //    {
    //        SecurityMode = SecurityMode.View;
    //    }
    //    public int EntityViewID { set; get; }
    //    public EntityRelationshipTailDTO RelationshipTail { set; get; }
    //    public List<EntityInstanceProperty> FirstRelationshipFirstSideKeyColumns { set; get; }
    //}

    public class DR_SearchExistsRequest : BaseRequest
    {
        public DR_SearchExistsRequest(DR_Requester Requester) : base(Requester)
        {
            SecurityMode = SecurityMode.View;
        }
        public DP_SearchRepository SearchDataItems;
        public int EntityID;
    }
    public class DR_SearchCountRequest : BaseRequest
    {
        public DR_SearchCountRequest(DR_Requester Requester) : base(Requester)
        {
            SecurityMode = SecurityMode.View;
        }
        public DP_SearchRepository SearchDataItems;
        public int EntityID;
    }
    //public class DR_SearchByRelationViewRequest : BaseRequest
    //{
    //    public DR_SearchByRelationViewRequest(DR_Requester Requester) : base(Requester)
    //    {

    //    }
    //    public DP_SearchRepository SourceRelationData;
    //    public int SourceRelationshipID;
    //}

    public class DR_SearchEditRequest : BaseRequest
    {
        public DR_SearchEditRequest(DR_Requester Requester, DP_SearchRepository searchDataItem, bool viewMode,bool withDataView) : base(Requester)
        {
            SearchDataItem = searchDataItem;
            if (viewMode)
                SecurityMode = SecurityMode.View;
            else
                SecurityMode = SecurityMode.Edit;
            WithDataView = withDataView;
        }
        public bool WithDataView { set; get; }
        //public DR_SearchEditRequest()
        //{

        //    //Properties = new List<List<EntityInstanceProperty>>();
        //}
        public DP_SearchRepository SearchDataItem;

    }
    public class DR_SearchFullDataRequest : BaseRequest
    {
        public DR_SearchFullDataRequest(DR_Requester Requester, DP_SearchRepository searchDataItem) : base(Requester)
        {
            SecurityMode = SecurityMode.View;
            SearchDataItem = searchDataItem;
        }
        //public DR_SearchEditRequest()
        //{

        //    //Properties = new List<List<EntityInstanceProperty>>();
        //}
        public DP_SearchRepository SearchDataItem;

    }
    public class DR_SearchKeysOnlyRequest : BaseRequest
    {
        public DR_SearchKeysOnlyRequest(DR_Requester Requester, DP_SearchRepository searchDataItem) : base(Requester)
        {
            SecurityMode = SecurityMode.View;
            SearchDataItem = searchDataItem;
        }
        public DP_SearchRepository SearchDataItem;
    }
    //بهتره این حذف شه. یعنی ابتدا مثل خیلی از جاها سرچ آیتم ساخته شه و بعد بر حسب نیاز هر کدوم از جستجو ها صدا زده بشه
    //public class DR_SearchByRelationshipTailRequest : BaseRequest
    //{
    //    public DR_SearchByRelationshipTailRequest(DR_Requester Requester) : base(Requester)
    //    {
    //        SecurityMode = SecurityMode.View;
    //    }
    //    public EntityRelationshipTailDTO RelationshipTail { set; get; }
    //    public DP_BaseData FirstDataItem { set; get; }
    //}

    //public class DR_SearchViewByRelationshipTailRequest : BaseRequest
    //{
    //    public DR_SearchViewByRelationshipTailRequest(DR_Requester Requester) : base(Requester)
    //    {
    //        SecurityMode = SecurityMode.View;
    //    }
    //    public int EntityViewID { set; get; }

    //    public EntityRelationshipTailDTO RelationshipTail { set; get; }
    //    public DP_BaseData FirstDataItem { set; get; }
    //}
    public enum Enum_OrderBy
    {
        Ascending,
        Descending
    }
}
