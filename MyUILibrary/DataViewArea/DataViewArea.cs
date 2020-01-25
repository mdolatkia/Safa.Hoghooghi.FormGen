using MyUILibrary.DataViewArea;
using MyUILibraryInterfaces.DataViewArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyUILibrary.EntityArea;
using ProxyLibrary;
using ModelEntites;
using MyRelationshipDataManager;
using MyUILibraryInterfaces.DataMenuArea;
using MyUILibraryInterfaces.EntityArea;
using MyUILibrary.EntitySearchArea;


namespace MyUILibrary.DataViewArea
{
    public class DataViewArea :DataArea, I_DataViewArea
    {
     
      

        //TableDrivedEntityDTO Entity { set; get; }
        public new I_View_DataViewArea View
        {
            set; get;
        }

        public void SetAreaInitializerSpecialized(DataViewAreaInitializer initParam)
        {
            DataViewSetting = AgentUICoreMediator.GetAgentUICoreMediator.DataViewManager.GetDataViewSetting(initParam.EntityID);
            View = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetViewOfDataViewArea();
        }

        SearchEntityArea SearchEntityArea { set; get; }
        //  int LastSelectedListViewID { set; get; }
        I_DataViewItem LastSelectedDataViewItem { set; get; }
        public I_DataViewItem DefaultDataViewItem { get; set; }

       


        public event EventHandler DataItemsSearchedByUser;
        I_View_ReportList ViewReports { set; get; }
        //DP_SearchRepository InitialSearchRepository { set; get; }

     

      

        //List<EntityReportDTO> EntityReports { set; get; }
        //private void View_ReportRequested(object sender, EventArgs e)
        //{
        //    if (EntityReports == null)
        //        EntityReports = AgentUICoreMediator.GetAgentUICoreMediator.ReportManager.GetReports(AreaInitializer.EntityID);
        //    if (ViewReports == null)
        //    {
        //        ViewReports = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetViewOfReportList();
        //        ViewReports.ReportClicked += ViewReports_ReportClicked;
        //        ViewReports.SetReportList(EntityReports);
        //    }
        //    if (EntityReports.Count > 0)
        //        AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(ViewReports, "گزارشات");
        //}
        //   ReportArea ReportArea { set; get; }
        //private void ViewReports_ReportClicked(object sender, ReportClickedArg e)
        //{
        //    if (ReportArea == null)
        //        ReportArea = new ReportArea();
        //    ReportArea.InitialSearchShouldBeIncluded = true;
        //    //var request = new RR_ReportRequest();
        //    //request.ReportID = e.ReportID;
        //    //باید بشه سرچ ریپورت
        //    ReportAreaInitializer initializer = new ReportAreaInitializer();
        //    initializer.ReportID = e.ReportID;
        //    initializer.SearchRepository = SearchRepository;
        //    initializer.EntityID = AreaInitializer.EntityID;
        //    ReportArea.SetAreaInitializer(initializer);
        //    if (ReportArea.View != null)
        //        AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(ReportArea.View, "گزارش", Enum_WindowSize.Maximized);
        //}
      



        //public bool InitialSearchShouldBeIncluded
        //{
        //    set; get;
        //}

    
        public bool UserCanChangeSearch { get; internal set; }

       

        private void View_SearchCommandRequested(object sender, EventArgs e)
        {
            if (SearchEntityArea == null)
            {
                SearchEntityArea = new SearchEntityArea();
                var searchViewInitializer = new SearchEntityAreaInitializer();

                searchViewInitializer.EntityID = AreaInitializer.EntityID;
                //if (AreaInitializer.Entitiy==null)
                //{
                //    AreaInitializer.Entitiy = AgentUICoreMediator.GetAgentUICoreMediator.GetEntity(AreaInitializer.EntitiyID, EntityColumnInfoType.WithFullColumns, EntityRelationshipInfoType.WithRelationships, false, false);
                //}

                SearchEntityArea.SetAreaInitializer(searchViewInitializer);
                //SearchEntityArea.GenerateSearchView();
                SearchEntityArea.SearchDataDefined += SearchEntityArea_SearchDataDefined;

            }
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(SearchEntityArea.SearchView, "جستجو");
        }

        private void SearchEntityArea_SearchDataDefined(object sender, SearchDataArg e)
        {
            GetDataItemsBySearchRepository(e.SearchItems);
            if (DataItemsSearchedByUser != null)
                DataItemsSearchedByUser(this, null);
        }



        public void SetItems(List<DP_DataView> resultDataItems)
        {
            List<I_DataViewItem> list = new List<I_DataViewItem>();

            foreach (var item in resultDataItems)
            {
                list.Add(GetDataViewItem(item));
            }

            View.AddDataViewItems(list);
            View.HideDataViewItemInfo();
        }
        private I_DataViewItem GetDataViewItem(DP_DataView item)
        {
            I_DataViewItem dataViewItem = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDataViewItem();
            dataViewItem.DataView = item;
            dataViewItem.InfoClicked += DataViewItem_InfoClicked;
            dataViewItem.Selected += DataViewItem_Selected;
            //foreach (var column in item.Properties.Take(3))
            //{
            //    dataViewItem.AddTitleRow(column.Name, column.Value);
            //}

            if (!string.IsNullOrEmpty(item.TargetEntityAlias))
                dataViewItem.Title = item.TargetEntityAlias;
            dataViewItem.Body = item.ViewInfo;
            return dataViewItem;
        }

        private void DataViewItem_Selected(object sender, EventArgs e)
        {
            I_DataViewItem dataViewItem = sender as I_DataViewItem;
            LastSelectedDataViewItem = dataViewItem;
            ShowDataViewItemInfo(LastSelectedDataViewItem, SelectedListView == null ? 0 : SelectedListView.ID);
        }

        private void ShowDataViewItemInfo(I_DataViewItem dataViewItem, int listViewID)
        {
            if (dataViewItem != null)
            {
                //int defaultListViewID = 0;
                //if (EntityDataView != null)
                //    defaultListViewID = EntityDataView.EntityListViewID;
                //if (listViewID != defaultListViewID)
                //{
                //    var seatchItem = new DP_SearchRepository(AreaInitializer.EntitiyID);
                //    var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();
                //    //سکوریتی داده اعمال میشود
                //    var searchRequest = new DR_SearchViewRequest(requester, seatchItem);
                //    //searchRequest.EntityID = AreaInitializer.EntitiyID;
                //    searchRequest.EntityViewID = listViewID;

                //    foreach (var prop in dataViewItem.DataView.Properties.Where(x => x.IsKey))
                //    {
                //        SearchProperty searchProperty = new SearchProperty();
                //        searchProperty.ColumnID = prop.ColumnID;
                //        searchProperty.Value = prop.Value;
                //        searchRequest.SearchDataItems.Phrases.Add(searchProperty);
                //    }
                //    var result = AgentUICoreMediator.GetAgentUICoreMediator.SendSearchViewRequest(searchRequest);
                //    if (result.ResultDataItems.Any())
                //    {
                //        var found = result.ResultDataItems.First();

                //        //درست شود
                //        //////ShowDataViewItemInfo(found);
                //    }
                //}
                //else
                // {
                ShowDataViewItemInfo(dataViewItem.DataView);
                //   }
            }
        }


        private void ShowDataViewItemInfo(DP_DataView found)
        {
            View.ClearDataViewItemInfo();
            foreach (var prop in found.Properties)
            {
                View.AddDataViewItemInfo(prop.Column.Alias, prop.Value);
            }
            View.ShowDataViewItemInfo();
        }

        private void DataViewItem_InfoClicked(object sender, EventArgs e)
        {
            var dataViewItem = (sender as I_DataViewItem);
            if (dataViewItem != null)
            {
                //var menus = GetDataViewItemMenus(dataViewItem);
                //dataViewItem.ShowDataViewItemMenus(menus);
                var menuInitializer = new DataMenuAreaInitializer();
                menuInitializer.HostDataViewArea = this;
                menuInitializer.HostDataViewItem = dataViewItem;
                menuInitializer.SourceView = sender;


                menuInitializer.DataItem = dataViewItem.DataView;


                AgentUICoreMediator.GetAgentUICoreMediator.ShowMenuArea(menuInitializer);

            }
        }

   



        //private void View_DataViewItemDoubleClicked(object sender, DataViewItemClickArg e)
        //{

        //}




    }
    //public enum DataViewRelationshipType
    //{
    //    Relationship,
    //    RelationshipTail
    //}

}
