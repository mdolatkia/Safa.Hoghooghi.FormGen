using MyUILibraryInterfaces.DataViewArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProxyLibrary;
using ModelEntites;


namespace MyUILibrary.DataViewArea
{
    public class DataArea : I_DataArea
    {
        public int MaxDataItems = 5;
        public I_View_DataArea View
        {
            get
            {
                if (this is I_DataViewArea)
                    return (this as I_DataViewArea).View;
                else if (this is I_GridViewArea)
                    return (this as I_GridViewArea).View;
                return null;
            }
        }
        public DataViewAreaInitializer AreaInitializer
        {
            set; get;
        }
        public I_DataViewAreaContainer DataViewAreaContainer
        {
            set; get;
        }
        DP_SearchRepository SearchRepository { set; get; }

        public void GetDataItemsBySearchRepository(DP_SearchRepository searchRepository)
        {
            SearchRepository = searchRepository;
            var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();
            //سکوریتی داده اعمال میشود
            var countRequest = new DR_SearchCountRequest(requester);
            countRequest.EntityID = AreaInitializer.EntityID;
            countRequest.SearchDataItems = SearchRepository;
            var countResult = AgentUICoreMediator.GetAgentUICoreMediator.requestRegistration.SendSearchCountRequest(countRequest);
            if (countResult.Result != Enum_DR_ResultType.ExceptionThrown)
                View.SetItemsTotalCount(countResult.ResultCount);
            else
            {
                  AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo(countResult.Message,"",Temp.InfoColor.Red);
                return;
            }
            //سکوریتی داده اعمال میشود
            var searchRequest = new DR_SearchViewRequest(requester, SearchRepository);
            searchRequest.MaxDataItems = MaxDataItems;
            searchRequest.OrderByEntityViewColumnID = View.GetOrderColumnID;
            if (View.GetSortText == "Ascending")
                searchRequest.SortType = Enum_OrderBy.Ascending;
            else if (View.GetSortText == "Descending")
                searchRequest.SortType = Enum_OrderBy.Descending;

            searchRequest.EntityViewID = SelectedListView == null ? 0 : SelectedListView.ID;
            var result = AgentUICoreMediator.GetAgentUICoreMediator.requestRegistration.SendSearchViewRequest(searchRequest);
            if (countResult.Result != Enum_DR_ResultType.ExceptionThrown)
            {
                string tooltip = "";
                if (result.ResultDataItems.Count == MaxDataItems && MaxDataItems != 0)
                    tooltip = "توجه شود که تنها " + MaxDataItems + " " + "مورد اول نمایش داده می شوند";
                View.SetDataItemsCount(result.ResultDataItems.Count, tooltip);

                if (this is I_DataViewArea)
                {
                    (this as I_DataViewArea).SetItems(result.ResultDataItems);
                }
                else if (this is I_GridViewArea)
                {
                    (this as I_GridViewArea).SetItems(result.ResultDataItems);
                }
            }
            else
            {
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo(countResult.Message, "", Temp.InfoColor.Red);
                return;
            }

        }
        EntityListViewDTO _SelectedListView;

        public EntityListViewDTO SelectedListView
        {
            set
            {
                _SelectedListView = value;
            }
            get
            {
                return _SelectedListView;
            }
        }
        //private bool CheckRelationshipTailPermission(EntityRelationshipTailDTO relationshipTail, bool first = true)
        //{
        //    if (first)
        //    {
        //        var entityPermission = AgentUICoreMediator.GetAgentUICoreMediator.SecurityHelper.GetAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), relationshipTail.Relationship.EntityID2, false);

        //        if (entityPermission.GrantedActions.Any(y => y == SecurityAction.NoAccess))
        //            return false;
        //    }

        //    var relationshipPermission = AgentUICoreMediator.GetAgentUICoreMediator.SecurityHelper.GetAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), relationshipTail.Relationship.ID, false);

        //    if (relationshipPermission.GrantedActions.Any(y => y == SecurityAction.NoAccess))
        //        return false;

        //    if (relationshipTail.ChildTail != null)
        //        return CheckRelationshipTailPermission(relationshipTail.ChildTail, false);
        //    else
        //        return true;
        //}
        private void View_EntityListViewChanged(object sender, EntitiListViewChangedArg e)
        {
            SelectedListView = AgentUICoreMediator.GetAgentUICoreMediator.EntityListViewManager.GetEntityListView(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), e.ListViewID);
            //ShowDataViewItemInfo(LastSelectedDataViewItem, LastSelectedListViewID);
            SetEntityOrderColumns();
            GetDataItemsBySearchRepository(SearchRepository);
        }
        //AssignedPermissionDTO _Permission;
        //public AssignedPermissionDTO Permission
        //{
        //    get
        //    {
        //        if (_Permission == null)
        //            _Permission = AgentUICoreMediator.GetAgentUICoreMediator.SecurityHelper.GetAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), AreaInitializer.EntityID, false);
        //        return _Permission;
        //    }
        //}
        public bool SecurityNoAccess { set; get; }
        public bool SecurityReadonly { set; get; }
        public bool SecurityEdit { set; get; }
        //private void ManageSecurity()
        //{
        //    if (Permission.GrantedActions.Any(x => x == SecurityAction.NoAccess))
        //    {
        //        SecurityNoAccess = true;
        //    }
        //    else
        //    {
        //        if (Permission.GrantedActions.Any(x => x == SecurityAction.EditAndDelete || x == SecurityAction.Edit))
        //        {
        //            SecurityEdit = true;
        //        }
        //        else if (Permission.GrantedActions.Any(x => x == SecurityAction.ReadOnly))
        //        {
        //            SecurityReadonly = true;
        //        }
        //        else
        //            SecurityNoAccess = true;
        //    }
        //    ImposeSecurity();
        //}

        //private void ImposeSecurity()
        //{
        //    //if (SecurityNoAccess)
        //    //{
        //    //    View = null;
        //    //    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عدم دسترسی به آرشیو");
        //    //}
        //    //else
        //    //{
        //    //    if (!SecurityReadonly && !SecurityEdit)
        //    //    {
        //    //        View = null;
        //    //        AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عدم دسترسی به آرشیو");

        //    //    }
        //    //}
        //}
        public DataViewSettingDTO DataViewSetting
        {
            set; get;
        }
        public void SetAreaInitializer(DataViewAreaInitializer initParam)
        {
            AreaInitializer = initParam;
            if (this is I_DataViewArea)
                (this as I_DataViewArea).SetAreaInitializerSpecialized(initParam);
            else if (this is I_GridViewArea)
                (this as I_GridViewArea).SetAreaInitializerSpecialized(initParam);

            View.EntityListViewChanged += View_EntityListViewChanged;
            View.OrderColumnsChanged += View_OrderColumnsChanged;

            //بهتر نوشته شود
            if (AreaInitializer.EntityListViewID != 0 || (DataViewSetting != null && DataViewSetting.EntityListViewID != 0))
            {
                if (AreaInitializer.EntityListViewID != 0)
                    SelectedListView = AgentUICoreMediator.GetAgentUICoreMediator.EntityListViewManager.GetEntityListView(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), AreaInitializer.EntityListViewID);
                else
                    SelectedListView = AgentUICoreMediator.GetAgentUICoreMediator.EntityListViewManager.GetEntityListView(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), DataViewSetting.EntityListViewID);
            }
            else
                SelectedListView = AgentUICoreMediator.GetAgentUICoreMediator.EntityListViewManager.GetDefaultEntityListView(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), AreaInitializer.EntityID);

            SetEntitiyListViews();
            SetEntityOrderColumns();

            //ManageSecurity();
          
        }

        private void View_OrderColumnsChanged(object sender, EventArgs e)
        {
            GetDataItemsBySearchRepository(SearchRepository);
        }
        public List<EntityListViewDTO> EntitiyListViews { get; private set; }

        private void SetEntitiyListViews()
        {
            EntitiyListViews = AgentUICoreMediator.GetAgentUICoreMediator.EntityListViewManager.GetEntityListViews(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), AreaInitializer.EntityID);

            //   LastSelectedListViewID = MainEntityListView.ID;
            //if (EntityDataView != null)
            //    LastSelectedListViewID = EntityDataView.EntityListViewID;
            //if (LastSelectedListViewID != 0)
            //{
            //    if (!EntitiyListViews.Any(x => x.ID == LastSelectedListViewID))
            //    {
            //        EntitiyListViews.Insert(0, new EntityListViewDTO() { ID = LastSelectedListViewID, Title = "پیش فرض" });
            //    }
            View.SetEntityListViews(EntitiyListViews, SelectedListView == null ? 0 : SelectedListView.ID);
            //}
        }
        private void SetEntityOrderColumns()
        {
            List<Tuple<int, string>> columns = new List<Tuple<int, string>>();
            List<EntityListViewColumnsDTO> listColumns = null;
            if (SelectedListView != null)
                listColumns = SelectedListView.EntityListViewAllColumns;
            //else
            //{
            //    var selected = EntitiyListViews.First(x => x.ID == LastSelectedListViewID);
            //    if (!selected.EntityListViewAllColumns.Any())
            //    {
            //        if (selected.ID == MainEntityListView.ID)
            //        {
            //            //چون موجوده و قبلا گرفته شده
            //            listColumns = MainEntityListView.EntityListViewAllColumns;
            //        }
            //        else
            //        {
            //            var getList = AgentUICoreMediator.GetAgentUICoreMediator.DataViewManager.GetEntityListView(selected.ID);
            //            selected.EntityListViewAllColumns = getList.EntityListViewAllColumns;
            //            listColumns = selected.EntityListViewAllColumns;
            //        }
            //    }
            //    else
            //        listColumns = selected.EntityListViewAllColumns;

            //}
            foreach (var col in listColumns)
            {
                columns.Add(new Tuple<int, string>(col.ColumnID, col.Column.Alias));
            }



            View.SetOrderColumns(columns);

            View.SetOrderSorts(new List<string>() { "Ascending", "Descending" });
        }

    }
}
