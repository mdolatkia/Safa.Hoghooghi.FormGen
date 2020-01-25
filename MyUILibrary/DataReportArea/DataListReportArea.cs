using MyUILibrary.DataReportArea;
using MyUILibraryInterfaces.DataReportArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyUILibrary.EntityArea;
using ProxyLibrary;
using ModelEntites;
using MyRelationshipDataManager;

namespace MyUILibrary.DataReportArea
{
    public class DataListReportArea : I_DataListReportArea
    {
        public int MaxDataItems = 50;
        public DataListReportAreaInitializer AreaInitializer
        {
            set; get;
        }

        TableDrivedEntityDTO Entity { set; get; }
        public I_View_DataListReportArea View
        {
            set; get;
        }

        public EntityDataViewDTO EntityDataView
        {
            set; get;
        }


        //public EntityListViewDTO EntityListView
        //{
        //    set; get;
        //}
        SearchEntityArea SearchEntityArea { set; get; }
        //int LastSelectedListViewID { set; get; }
        DP_DataRepository LastSelectedDataReportItem { set; get; }
        public List<EntityListViewDTO> EntityListViews { get; private set; }
        public MyDataObject DefaultDataReportItem { get; set; }

        public event EventHandler<DataReportAreaRequestedArg> RelatedDataReportArearequested;
        public event EventHandler DataItemsSearchedByUser;

        DP_SearchRepository InitialSearchRepository { set; get; }
        DP_SearchRepository SearchRepository { set; get; }
        public void SetAreaInitializer(DataListReportAreaInitializer initParam)
        {
            AreaInitializer = initParam;
            //آیا لازمه دوباره انتیتی گرفته بشه؟
            Entity = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetFullEntity(AreaInitializer.EntitiyID);
            InitialSearchRepository = initParam.SearchRepository;
            View = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetViewOfDataListReportArea();
            View.EntityListViewChanged += View_EntityListViewChanged;
            View.SearchCommandRequested += View_SearchCommandRequested;
            View.OrderColumnsChanged += View_OrderColumnsChanged;
            View.SearchAreaCommandVisibility = true;
            View.DataItemDoubleClicked += View_DataItemDoubleClicked;
            EntityDataView = AgentUICoreMediator.GetAgentUICoreMediator.DataViewManager.GetEntityDataViewByEntitiyID(initParam.EntitiyID);
            //if (EntityDataReport != null && EntityDataReport.EntityListViewID != 0)
            //    EntityListView = AgentUICoreMediator.GetAgentUICoreMediator.DataReportManager.GetEntityListView(EntityDataReport.EntityListViewID);
            //else
            //    EntityListView = AgentUICoreMediator.GetAgentUICoreMediator.DataReportManager.GetDefaultEntityListView(AreaInitializer.EntitiyID);
            View.Title = initParam.Title;
            SetEntitiyListViews();
            SetEntityOrderColumns();
            if (AreaInitializer.SearchRepository != null)
            {
                GetDataItemsBySearchRepository(AreaInitializer.SearchRepository);
            }
        }



        public bool InitialSearchShouldBeIncluded
        {
            set; get;
        }

        private void GetDataItemsBySearchRepository(DP_SearchRepository searchRepository )
        {
            if (searchRepository != null)
            {
                SearchRepository = searchRepository;
                if (InitialSearchShouldBeIncluded)
                    if (InitialSearchRepository != null)
                        if (InitialSearchRepository != SearchRepository)
                        {
                            SearchRepository.Phrases.Add(InitialSearchRepository);
                        }
                var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();
                //سکوریتی داده اعمال میشود
                var countRequest = new DR_SearchCountRequest(requester);
                countRequest.EntityID = AreaInitializer.EntitiyID;
                countRequest.SearchDataItems = SearchRepository;
                var countResult = AgentUICoreMediator.GetAgentUICoreMediator.requestRegistration.SendSearchCountRequest(countRequest);
                View.SetItemsTotalCount(countResult.ResultCount);

                //سکوریتی داده اعمال میشود
                var searchRequest = new DR_SearchViewRequest(requester,SearchRepository);
                searchRequest.MaxDataItems = MaxDataItems;
                searchRequest.OrderByEntityViewColumnID = View.GetOrderColumnID;
                if (View.GetSortText == "Ascending")
                    searchRequest.SortType = Enum_OrderBy.Ascending;
                else if (View.GetSortText == "Descending")
                    searchRequest.SortType = Enum_OrderBy.Descending;

                //searchRequest.EntityID = AreaInitializer.EntitiyID;
                searchRequest.EntityViewID = View.GetEntityListViewID();
                var result = AgentUICoreMediator.GetAgentUICoreMediator.requestRegistration.SendSearchViewRequest(searchRequest);
                //List<I_DataReportItem> list = new List<I_DataReportItem>();
                //foreach (var item in result.ResultDataItems)
                //{
                //    list.Add(GetDataReportItem(item));
                //}
                string tooltip = "";
                if (result.ResultDataItems.Count == MaxDataItems && MaxDataItems != 0)
                    tooltip = "توجه شود که تنها " + MaxDataItems + " " + "مورد اول نمایش داده می شوند";
                View.SetDataItemsCount(result.ResultDataItems.Count, tooltip);
              //درست شود
                //////View.AddDataReportItems(AgentHelper.GetDataObjects(result.ResultDataItems));
            }

        }

        private void View_SearchCommandRequested(object sender, EventArgs e)
        {
            if (SearchEntityArea == null)
            {
                SearchEntityArea = new SearchEntityArea();
                var searchViewInitializer = new SearchEntityAreaInitializer();
               
                searchViewInitializer.EntityID = AreaInitializer.EntitiyID;
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

        private void View_EntityListViewChanged(object sender, EntitiListViewChangedArg e)
        {
            SetEntityOrderColumns();
            GetDataItemsBySearchRepository(SearchRepository);
            //LastSelectedListViewID = e.ListViewID;
            //ShowDataReportItemInfo(LastSelectedDataReportItem, LastSelectedListViewID);
        }

        private void SetEntitiyListViews()
        {
            int defaultListViewID = 0;
            if (EntityDataView != null)
            {
                defaultListViewID = EntityDataView.EntityListViewID;
            }
            else
                defaultListViewID = Entity.EntityListViewID;

            EntityListViews = AgentUICoreMediator.GetAgentUICoreMediator.DataViewManager.GetEntityListViews(AreaInitializer.EntitiyID);
            View.SetEntityListViews(EntityListViews, defaultListViewID);
        }

        private void SetEntityOrderColumns()
        {
            List<Tuple<int, string>> columns = new List<Tuple<int, string>>();
            var selectedViewEntityListID = View.GetEntityListViewID();
            if (selectedViewEntityListID != 0)
            {
                var entityListView = AgentUICoreMediator.GetAgentUICoreMediator.DataViewManager.GetEntityListView(selectedViewEntityListID);
                foreach (var col in entityListView.EntityListViewAllColumns)
                {
                    columns.Add(new Tuple<int, string>(col.ID, col.Column.Name));
                }
            }
            else
            {
                foreach (var col in Entity.Columns)
                {
                    columns.Add(new Tuple<int, string>(col.ID, col.Alias));
                }
            }
            View.SetOrderColumns(columns);
            View.SetOrderSorts(new List<string>() { "Ascending", "Descending" });
        }
        private void View_OrderColumnsChanged(object sender, EventArgs e)
        {
            GetDataItemsBySearchRepository(SearchRepository);
        }

        private void View_DataItemDoubleClicked(object sender, DataItemDoubleClickedArg e)
        {
            var menus = GetDataReportItemMenus(e.DataObject);
            View.ShowDataViewItemMenus(e.DataObject, menus);
        }

        private List<DataReportMenu> GetDataReportItemMenus(MyDataObject item)
        {
            var result = new List<DataReportMenu>();
            SetRelationshipMenus(result, item);
            return result;
        }
        private void SetRelationshipMenus(List<DataReportMenu> result, MyDataObject DataReportItem)
        {//با فرم ویو یک فانکشن شود
            List<RelationshipMenu> listRelationships = new List<RelationshipMenu>();

            if (EntityDataView != null && EntityDataView.EntityDataViewRelationships.Any())
            {
                foreach (var rel in EntityDataView.EntityDataViewRelationships)
                {
                    //if (rel.RelationshipID != 0)
                    //{

                    //    var relationship = AgentUICoreMediator.GetAgentUICoreMediator.RelationshipManager.GetRelationship(rel.RelationshipID);
                    //    var relationshipMenu = new RelationshipMenu(relationship, relationship.EntityID2
                    //    , relationship.Entity2, relationship.Alias, rel.Group1, rel.Group2);
                    //    listRelationships.Add(relationshipMenu);
                    //}
                     if (rel.RelationshipTailID != 0)
                    {
                        var relationshipTail = AgentUICoreMediator.GetAgentUICoreMediator.RelationshipManager.GetRelationshipTail(rel.RelationshipTailID, false);
                        var relationshipMenu = new RelationshipMenu(relationshipTail, relationshipTail.TargetEntityID
                        , relationshipTail.TargetEntityAlias, relationshipTail.EntityPath, rel.Group1, rel.Group2);
                        listRelationships.Add(relationshipMenu);
                    }
                }
            }
            else
            {
                //var allRelationships = AgentUICoreMediator.GetAgentUICoreMediator.RelationshipManager.GetRelationshipsByEntityID(DataReportItem.DataItem.TargetEntityID);
                var allRelationships = Entity.Relationships;
                foreach (var relationship in allRelationships)
                {

                    var relationshipMenu = new RelationshipMenu(relationship, relationship.EntityID2
                  , relationship.Entity2, relationship.Alias, "", "");
                    listRelationships.Add(relationshipMenu);

                }
                var allRelationshipTails = AgentUICoreMediator.GetAgentUICoreMediator.RelationshipManager.GetRelationshipTails(DataReportItem.DataItem.TargetEntityID);
                foreach (var relationshipTail in allRelationshipTails)
                {
                    var relationshipMenu = new RelationshipMenu(relationshipTail, relationshipTail.TargetEntityID
                       , relationshipTail.TargetEntityAlias, relationshipTail.EntityPath, "", "");
                    listRelationships.Add(relationshipMenu);
                }

            }
            foreach (var rel in listRelationships)
            {
                if (rel.RelType == DataReportRelationshipType.Relationship)
                {
                    if (AreaInitializer.CausingRelationship == null || AreaInitializer.CausingRelationship.PairRelationshipID != rel.Relationship.ID)
                    {
                        var searchRepository = AgentUICoreMediator.GetAgentUICoreMediator.RelationshipDataManager.GetSearchDataItemByRelationship(RelationshipSreachType.SecondSideBasedOnFirstRelationshhipColumn, DataReportItem.DataItem, rel.Relationship.ID);
                        if (searchRepository != null)
                        {
                            var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();
                            //سکوریتی داده اعمال میشود
                            var existsRequest = new DR_SearchExistsRequest(requester);
                            existsRequest.EntityID = rel.EntityID;
                            existsRequest.SearchDataItems = searchRepository;
                            var exists = AgentUICoreMediator.GetAgentUICoreMediator.requestRegistration.SendSearchExistsRequest(existsRequest);
                            if (exists.ExistsResult)
                            {
                                rel.SearchRepository = searchRepository;
                                rel.IsValid = true;
                            }
                        }
                    }
                }
            }

            var validRelationships = listRelationships.Where(x => x.IsValid);
            if (validRelationships.Any())
            {
                var relationshipMenu = new DataReportMenu();
                relationshipMenu.Title = "روابط";
                relationshipMenu.Type = DataReportMenuType.Relationship;
                result.Add(relationshipMenu);

                if (EntityDataView == null || !EntityDataView.EntityDataViewRelationships.Any())
                {
                    if (validRelationships.Count() > 8)
                    {
                        int index = 1;
                        foreach (var item in validRelationships)
                        {
                            var cIndex = index / 5;
                            item.Group1 = "Group" + (cIndex + 1);
                            index++;
                        }
                    }
                }

                foreach (var item in validRelationships.GroupBy(x => new { x.Group1, x.Group2 }))
                {
                    List<DataReportMenu> parentCollection = null;
                    if (string.IsNullOrEmpty(item.Key.Group1))
                    {
                        parentCollection = relationshipMenu.SubMenus;
                    }
                    else
                    {
                        DataReportMenu menuLevel1 = GetOrCreateMenu1(relationshipMenu.SubMenus, item.Key.Group1);
                        parentCollection = menuLevel1.SubMenus;
                    }
                    if (string.IsNullOrEmpty(item.Key.Group2))
                    {
                        //   parentCollection = parentCollection;
                    }
                    else
                    {
                        DataReportMenu menuLevel2 = GetOrCreateMenu1(parentCollection, item.Key.Group2);
                        parentCollection = menuLevel2.SubMenus;
                    }

                    foreach (var rel in item)
                    {
                        var relationshipSubMenu = new DataReportMenu();
                        relationshipSubMenu.Title = rel.EntityName;
                        relationshipSubMenu.Tooltip = rel.Tooltip;
                        relationshipSubMenu.MenuClicked += (sender, e) => RelationshipSubMenu_MenuClicked(sender, e, rel, DataReportItem, rel.SearchRepository);
                        parentCollection.Add(relationshipSubMenu);
                    }


                }
            }


        }

        private DataReportMenu GetOrCreateMenu1(List<DataReportMenu> collection, string key)
        {
            var fITem = collection.FirstOrDefault(x => x.Title == key);
            if (fITem == null)
            {
                fITem = new DataReportMenu();
                fITem.Type = DataReportMenuType.Folder;
                fITem.Title = key;
                collection.Add(fITem);
                return fITem;
            }
            else
                return fITem;
        }


        private void RelationshipSubMenu_MenuClicked(object sender, EventArgs e, RelationshipMenu rel, MyDataObject DataReportItem, DP_SearchRepository searchRepository)
        {
            //if (rel.Item1 == DataReportRelationshipType.Relationship)
            //{
            //var searchRepository = AgentUICoreMediator.GetAgentUICoreMediator.RelationshipManager.GetSearchDataItemByRelationship(DataReportItem.DataItem, rel.Item2, AgentUICoreMediator.GetAgentUICoreMediator.GetRequester());
            if (RelatedDataReportArearequested != null)
                RelatedDataReportArearequested(this, new DataReportAreaRequestedArg() { EntitiyID = rel.EntityID, Relationship = rel.Relationship, RelationshipTail = rel.EntityRelationshipTail, Title = rel.EntityName, SourceDataReportItem = DataReportItem, SearchRepository = searchRepository });
            //}
        }




    }
    public enum DataReportRelationshipType
    {
        Relationship,
        RelationshipTail
    }
    public class RelationshipMenu
    {
        public RelationshipMenu(RelationshipDTO relationship, int entityID, string entityName, string tooltip, string group1, string group2)
        {
            RelType = DataReportRelationshipType.Relationship;
            Relationship = relationship;
            EntityID = entityID;
            EntityName = entityName;
            Tooltip = tooltip;
            Group1 = group1;
            Group2 = group2;
        }
        public RelationshipMenu(EntityRelationshipTailDTO entityRelationshipTail, int entityID, string entityName, string tooltip, string group1, string group2)
        {
            RelType = DataReportRelationshipType.RelationshipTail;
            EntityRelationshipTail = entityRelationshipTail;
            EntityID = entityID;
            EntityName = entityName;
            Tooltip = tooltip;
            Group1 = group1;
            Group2 = group2;
        }
        public DataReportRelationshipType RelType { set; get; }
        public RelationshipDTO Relationship;
        public EntityRelationshipTailDTO EntityRelationshipTail;
        public int EntityID { set; get; }
        public string EntityName { set; get; }
        public string Tooltip { set; get; }
        public string Group1 { set; get; }
        public string Group2 { set; get; }
        public bool IsValid { set; get; }
        public DP_SearchRepository SearchRepository { set; get; }
    }
}
