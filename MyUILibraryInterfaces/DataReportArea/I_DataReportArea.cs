using ModelEntites;
using MyUILibrary.EntityArea;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibraryInterfaces.DataReportArea
{
    public interface I_DataListReportAreaContainer
    {
        I_View_DataListReportAreaContainer View { set; get; }
        DataListReportAreaContainerInitializer AreaInitializer { set; get; }
        void SetAreaInitializer(DataListReportAreaContainerInitializer initParam);
        I_SearchEntityArea SearchEntityArea { set; get; }
        DP_SearchRepository SearchRepository { set; get; }
        List<I_DataListReportArea> DataListReportAreas { set; get; }
    }
    public interface I_DataListReportArea
    {
        event EventHandler DataItemsSearchedByUser;
        event EventHandler<DataReportAreaRequestedArg> RelatedDataReportArearequested;
        DataListReportAreaInitializer AreaInitializer { set; get; }
        void SetAreaInitializer(DataListReportAreaInitializer initParam);
        I_View_DataListReportArea View { set; get; }

        bool InitialSearchShouldBeIncluded { set; get; }
        MyDataObject DefaultDataReportItem { set; get; }
    }
    public interface I_View_DataListReportAreaContainer
    {
        void AddDataListReportArea(object view);
        void ShowLinks(List<DataReportLink> links);
    }
    public interface I_View_DataListReportArea
    {
        void AddDataReportItems(List<MyDataObject> items);
        void SetEntityListViews(List<EntityListViewDTO> entitiyListViews, int defaultEntityListViewID);
        event EventHandler<EntitiListViewChangedArg> EntityListViewChanged;
        event EventHandler OrderColumnsChanged;
        event EventHandler SearchCommandRequested;

        event EventHandler<DataItemDoubleClickedArg> DataItemDoubleClicked;

        string Title { set; }
        bool SearchAreaCommandVisibility { get; set; }

        void BringIntoView(MyDataObject defaultDataReportItem);

        int GetOrderColumnID { get; }
        string GetSortText { get; }
        void SetOrderColumns(List<Tuple<int, string>> columns);
        void SetOrderSorts(List<string> list);
        void SetDataItemsCount(int count, string tooltip);
        void SetItemsTotalCount(int resultCount);
        int GetEntityListViewID();
        void ShowDataViewItemMenus(MyDataObject dataObject, List<DataReportMenu> menus);
    }

    //public interface I_DataReportItem
    //{
    //    event EventHandler Selected;
    //    event EventHandler InfoClicked;
    //    DP_DataRepository DataItem { set; get; }
    //    void AddTitleRow(string title, string value);

    //    void ShowDataReportItemMenus(List<DataReportMenu> menus);
    //    void OnSelected();
    //}

    public class DataListReportAreaInitializer
    {
        public string Title { set; get; }

        public RelationshipDTO CausingRelationship { set; get; }
        public EntityRelationshipTailDTO CausingRelationshipTail { set; get; }
        public int EntitiyID { set; get; }
        //public TableDrivedEntityDTO Entitiy { set; get; }
        public DP_SearchRepository SearchRepository { set; get; }
    }
    public class DataListReportAreaContainerInitializer
    {
        public string Title { set; get; }
        public int EntitiyID { set; get; }
        public DP_SearchRepository SearchRepository { set; get; }
    }
    public class DataReportAreaRequestedArg : EventArgs
    {
        public MyDataObject SourceDataReportItem { set; get; }
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

    public class DataItemDoubleClickedArg : EventArgs
    {
        public MyDataObject DataObject{ set; get; }
    }
    //public class OrderColumnChangedArg : EventArgs
    //{
    //    public int ID { set; get; }
    //    public string Sort { set; get; }
    //}
    public class DataReportMenu
    {
        public event EventHandler MenuClicked;
        public DataReportMenu()
        {
            SubMenus = new List<DataReportArea.DataReportMenu>();
        }

        public void OnMenuClicked()
        {
            if (MenuClicked != null)
                MenuClicked(this, null);
        }
        public string Tooltip { set; get; }
        public string Title { set; get; }
        public List<DataReportMenu> SubMenus { set; get; }
        public DataReportMenuType Type { set; get; }
    }
    public class DataReportLink
    {
        public void OnClicked()
        {
            if (DataReportLinkClicked != null)
                DataReportLinkClicked(this, null);
        }
        public event EventHandler DataReportLinkClicked;
        public string Title { set; get; }
        public string Tooltip { set; get; }
    }
    public enum DataReportMenuType
    {
        Report,
        Relationship,
        Folder

    }
}
