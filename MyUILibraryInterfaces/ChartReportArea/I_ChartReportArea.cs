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

    public interface I_ChartReportArea
    {
        event EventHandler DataItemsSearchedByUser;
        ChartReportAreaInitializer AreaInitializer { set; get; }
        void SetAreaInitializer(ChartReportAreaInitializer initParam);
        I_View_ChartReportArea View { set; get; }
        bool InitialSearchShouldBeIncluded { set; get; }
    }
    public interface I_View_ChartReportArea
    {
        //void AddDataReportItems(List<MyDataObject> items);
        void SetEntityListViews(List<EntityListViewDTO> entitiyListViews, int defaultEntityListViewID);
        event EventHandler<EntitiListViewChangedArg> EntityListViewChanged;
        event EventHandler OrderColumnsChanged;
        event EventHandler SearchCommandRequested;

        //event EventHandler<DataItemDoubleClickedArg> DataItemDoubleClicked;

        string Title { set; }
        bool SearchAreaCommandVisibility { get; set; }

        //int GetOrderColumnID { get; }
        //string GetSortText { get; }
        //void SetOrderColumns(List<Tuple<int, string>> columns);
        //void SetOrderSorts(List<string> list);
        //void SetDataItemsCount(int count, string tooltip);
        void SetItemsTotalCount(int resultCount);
        int GetEntityListViewID();
        //void ShowDataViewItemMenus(MyDataObject dataObject, List<DataReportMenu> menus);
    }
    public class ChartReportAreaInitializer
    {
        public string Title { set; get; }

        //public RelationshipDTO CausingRelationship { set; get; }
        //public EntityRelationshipTailDTO CausingRelationshipTail { set; get; }
        public int EntitiyID { set; get; }
        //public TableDrivedEntityDTO Entitiy { set; get; }
        public DP_SearchRepository SearchRepository { set; get; }
    }
  
   
   
}
