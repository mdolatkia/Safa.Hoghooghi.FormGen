using ModelEntites;
using MyUILibrary.EntityArea;
using MyUILibraryInterfaces.EntityArea;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibraryInterfaces.DataViewArea
{

    public interface I_DataViewAreaContainer
    {
        object MainView { set; get; }
        I_View_DataViewAreaContainer View { set; get; }
        DataViewAreaContainerInitializer AreaInitializer { set; get; }
        //    void SetAreaInitializer(DataViewAreaContainerInitializer initParam);
        //I_SearchEntityArea SearchEntityArea { set; get; }
        //DP_SearchRepository SearchRepository { set; get; }
        I_GeneralEntitySearchArea GeneralEntitySearchArea { set; get; }
        void AddDataViewAreaFromOutSide(int entityID, string title, DP_SearchRepository searchRepository, I_DataViewItem defaultDataViewItem,bool dataViewOrGridView, int defaultEntityListViewID);
    }
    public interface I_DataArea
    {
        I_DataViewAreaContainer DataViewAreaContainer { set; get; }
        DataViewAreaInitializer AreaInitializer { set; get; }
        void SetAreaInitializer(DataViewAreaInitializer initParam);
        void GetDataItemsBySearchRepository(DP_SearchRepository searchRepository);
        I_View_DataArea View { get; }
    }
    public interface I_DataViewArea : I_DataArea
    {
        //event EventHandler DataItemsSearchedByUser;
        //   event EventHandler<DataViewAreaRequestedArg> RelatedDataViewArearequested;
        DataViewSettingDTO DataViewSetting { set; get; }

        new I_View_DataViewArea View { set; get; }

        //bool InitialSearchShouldBeIncluded { set; get; }
        I_DataViewItem DefaultDataViewItem { set; get; }

        void SetItems(List<DP_DataView> resultDataItems);
        void SetAreaInitializerSpecialized(DataViewAreaInitializer initParam);
    }
    public interface I_GridViewArea : I_DataArea
    {
        GridViewSettingDTO GridViewSetting { set; get; }
        new I_View_GridViewArea View { set; get; }

        void SetItems(List<DP_DataView> resultDataItems);
        void SetAreaInitializerSpecialized(DataViewAreaInitializer initParam);
    }
    public interface I_View_DataViewAreaContainer
    {
        void AddDataViewArea(object view);
        // void AddGenerealSearchAreaView(object view);
        void ShowLinks(List<DataViewLink> links);
        void EnableDisable(bool v);
        void ClearDataView();
    }
    public interface I_View_DataArea
    {
        void SetEntityListViews(List<EntityListViewDTO> entitiyListViews, int defaultEntityListViewID);
        event EventHandler<EntitiListViewChangedArg> EntityListViewChanged;
        event EventHandler OrderColumnsChanged;
        //event EventHandler SearchCommandRequested;
        int GetOrderColumnID { get; }
        string GetSortText { get; }
        void SetOrderColumns(List<Tuple<int, string>> columns);
        void SetOrderSorts(List<string> list);
        void SetDataItemsCount(int count, string tooltip);
        void SetItemsTotalCount(int resultCount);
    }
    public interface I_View_DataViewArea : I_View_DataArea
    {
        void AddDataViewItems(List<I_DataViewItem> items);
        void ClearDataViewItemInfo();
        void AddDataViewItemInfo(string name, object value);
        void ShowDataViewItemInfo();
        void HideDataViewItemInfo();
        void BringIntoView(I_DataViewItem defaultDataViewItem);
    }
    public interface I_View_GridViewArea : I_View_ViewEntityAreaMultiple, I_View_DataArea
    {
        void AddGridViewItems(List<DP_DataView> items);

        //event EventHandler SearchCommandRequested;
        //     string Title { set; }
        //bool SearchAreaCommandVisibility { get; set; }

        event EventHandler<DataGridSelectedArg> InfoClicked;

        //   void AddGenerealSearchAreaView(object view);
    }
    public class DataGridSelectedArg : EventArgs
    {
        public object DataView { set; get; }
        public object UIElement { set; get; }
    }
    public interface I_DataViewItem
    {
        event EventHandler Selected;
        event EventHandler InfoClicked;
        DP_DataView DataView { set; get; }
        //   void AddTitleRow(string title, string value);
        string Title { set; get; }
        string Body { set; get; }
        // void ShowDataViewItemMenus(List<DataViewMenu> menus);
        void OnSelected();
    }
    //public class GridViewAreaInitializer
    //{
    //    public string Title { set; get; }
    //    public bool UserCanChangeSearch { set; get; }
    //    public int EntityID { set; get; }
    //    public int EntityListViewID { get; set; }
    //    public DP_SearchRepository SearchRepository { set; get; }
    //    public EntityPreDefinedSearchDTO PreDefinedSearch { get; set; }
    //}
    public class DataViewAreaInitializer
    {
        public string Title { set; get; }
        public int EntityID { set; get; }
        public int EntityListViewID { get; set; }
        //public TableDrivedEntityDTO Entitiy { set; get; }
        //public DP_SearchRepository SearchRepository { set; get; }
    }
    public class DataViewAreaContainerInitializer
    {
        public string Title { set; get; }
        public int EntityID { set; get; }
        public bool DataViewOrGridView { set; get; }
        public DP_SearchRepository InitialSearchRepository { set; get; }
        public bool UserCanChangeSearchRepository { get; set; }
        //public DP_SearchRepository PreDefinedSearch { get; set; }
        public bool ShowInitializeSearchRepository { get; set; }
        public int EntityListViewID { get; set; }
        //public bool InitialSearchShouldBeIncluded { get; set; }
    }
    public class DataViewAreaRequestedArg : EventArgs
    {
        public I_DataViewItem SourceDataViewItem { set; get; }
        public string Title { set; get; }
        public DP_SearchRepository SearchRepository { set; get; }

        public RelationshipDTO Relationship { set; get; }
        public EntityRelationshipTailDTO RelationshipTail { set; get; }
        public int EntitiyID { set; get; }
    }
    public class EntitiListViewChangedArg : EventArgs
    {
        public int ListViewID { set; get; }
    }
    //public class OrderColumnChangedArg : EventArgs
    //{
    //    public int ID { set; get; }
    //    public string Sort { set; get; }
    //}

    public class DataViewLink
    {
        public void OnClicked()
        {
            if (DataViewLinkClicked != null)
                DataViewLinkClicked(this, null);
        }
        public event EventHandler DataViewLinkClicked;
        public string Title { set; get; }
        public string Tooltip { set; get; }
    }

}
