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
using MyUILibrary.EntitySearchArea;
using MyUILibraryInterfaces.EntityArea;


namespace MyUILibrary.DataViewArea
{
    public class DataViewAreaContainer : I_DataViewAreaContainer
    {
        public bool SecurityNoAccess { set; get; }
        public bool SecurityReadonly { set; get; }
        public bool SecurityEdit { set; get; }
        public object MainView { set; get; }

        public I_GeneralEntitySearchArea GeneralEntitySearchArea { set; get; }

        public DataViewAreaContainer(DataViewAreaContainerInitializer initParam)
        {
            DataViewAreas = new List<I_DataArea>();

            AreaInitializer = initParam;
            View = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetViewOfDataViewAreaContainer();

            GeneralEntitySearchAreaInitializer selectAreaInitializer = new GeneralEntitySearchAreaInitializer();
            selectAreaInitializer.ExternalView = View;
            selectAreaInitializer.EntityID = AreaInitializer.EntityID;
            if (AreaInitializer.EntityID != 0)
                selectAreaInitializer.LockEntitySelector = true;
            if (initParam.InitialSearchRepository != null && !initParam.ShowInitializeSearchRepository)
                selectAreaInitializer.PreDefinedSearch = AreaInitializer.InitialSearchRepository;
            GeneralEntitySearchArea = new GeneralEntitySearchArea();
            GeneralEntitySearchArea.SearchDataDefined += GeneralEntitySearchArea_SearchDataDefined;
            GeneralEntitySearchArea.EntitySelected += GeneralEntitySearchArea_EntitySelected;
            GeneralEntitySearchArea.SetInitializer(selectAreaInitializer);
            MainView = GeneralEntitySearchArea.View;

            //      AddFirstDataViewArea();
            //   View.AddGenerealSearchAreaView(GeneralEntitySearchArea.View);

            //    ManageSecurity();
            //SecurityEdit = true;
            //if (!SecurityNoAccess && (SecurityReadonly || SecurityEdit))
            //{
            if (!AreaInitializer.UserCanChangeSearchRepository)
            {
                GeneralEntitySearchArea.EnableDisableSearchArea(false);
            }
            if (AreaInitializer.InitialSearchRepository != null && initParam.ShowInitializeSearchRepository)
            {
                GeneralEntitySearchArea_SearchDataDefined(this, new SearchDataArg() { SearchItems = AreaInitializer.InitialSearchRepository });
            }
            //    //    دیتا ویوها با سرچ یا بدون سرچ بصورت سکوریتی آبجت ذخبره شود برای دسترسی و منو
            //}
        }
        private void GeneralEntitySearchArea_EntitySelected(object sender, int? e)
        {
            if (e == null)
            {
                DataViewAreas.Clear();
                CurrentDataViewArea = null;
                SetLinks();
                View.ClearDataView();
                View.EnableDisable(false);
            }
            else
            {
                View.EnableDisable(true);
                AddFirstDataViewArea();
            }
        }

        private void GeneralEntitySearchArea_SearchDataDefined(object sender, SearchDataArg e)
        {
            foreach (var item in DataViewAreas.Where(x => x != FirstDataView).ToList())
                DataViewAreas.Remove(item);
            CurrentDataViewArea = FirstDataView;
            View.AddDataViewArea(FirstDataView.View);
            SetLinks();
            FirstDataView.GetDataItemsBySearchRepository(e.SearchItems);
        }

        //AssignedPermissionDTO _Permission;
        //public AssignedPermissionDTO Permission
        //{
        //    get
        //    {
        //        if (_Permission == null)
        //            _Permission = AgentUICoreMediator.GetAgentUICoreMediator.SecurityHelper.GetAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), GeneralEntitySearchArea.SelectedEntity.ID, false);
        //        return _Permission;
        //    }
        //}
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
        //    if (SecurityNoAccess)
        //    {
        //        View = null;
        //        AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عدم دسترسی به آرشیو");
        //    }
        //    else
        //    {
        //        if (!SecurityReadonly && !SecurityEdit)
        //        {
        //            View = null;
        //            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عدم دسترسی به آرشیو");

        //        }
        //    }
        //}


        public DataViewAreaContainerInitializer AreaInitializer
        {
            set; get;
        }






        public List<I_DataArea> DataViewAreas
        {
            set; get;
        }
        public I_DataArea CurrentDataViewArea { set; get; }
        public I_View_DataViewAreaContainer View
        {
            set; get;
        }
        I_DataArea FirstDataView = null;
        private void AddFirstDataViewArea()
        {
            DataViewAreas.Clear();
            CurrentDataViewArea = null;
            AddDataViewArea(GeneralEntitySearchArea.SelectedEntity.ID, GeneralEntitySearchArea.SelectedEntity.Alias, null, AreaInitializer.EntityListViewID, AreaInitializer.DataViewOrGridView);
            FirstDataView = DataViewAreas[0];
        }
        public void AddDataViewAreaFromOutSide(int entityID, string title, DP_SearchRepository searchRepository, I_DataViewItem causingDataViewItem, bool dataViewOrGridView, int defaultEntityListViewID)
        {
            if (CurrentDataViewArea != null && CurrentDataViewArea is I_DataViewArea)
                (CurrentDataViewArea as I_DataViewArea).DefaultDataViewItem = causingDataViewItem;

            AddDataViewArea(entityID, title, searchRepository, defaultEntityListViewID, dataViewOrGridView);
        }
        private void AddDataViewArea(int entityID, string title, DP_SearchRepository searchRepository, int entityListViewID, bool dataViewOrGridView)
        {
            DataArea dataArea = null;
            if (dataViewOrGridView)
                dataArea = new DataViewArea();
            else
                dataArea = new GridViewArea();
            dataArea.DataViewAreaContainer = this;
            // dataViewArea.InitialSearchShouldBeIncluded = initialSearchShouldBeIncluded;
            //  dataViewArea.RelatedDataViewArearequested += FirstDataViewArea_RelatedDataViewArearequested;
            //dataViewArea.DataItemsSearchedByUser += DataViewArea_DataItemsSearchedByUser;
            var firstInit = new DataViewAreaInitializer();
            //  firstInit.UserCanChangeSearch = userCanChangeSearch;
            //   firstInit.SearchRepository = searchRepository;
            firstInit.EntityListViewID = entityListViewID;
            firstInit.EntityID = entityID;
            firstInit.Title = title;
            //firstInit.CausingRelationship = causingRelationship;
            //firstInit.CausingRelationshipTail = causingRelationshipTail;
            dataArea.SetAreaInitializer(firstInit);
            if (searchRepository != null)
                dataArea.GetDataItemsBySearchRepository(searchRepository);
            View.AddDataViewArea(dataArea.View);

            if (CurrentDataViewArea == null)
            {
                DataViewAreas.Add(dataArea);
            }
            else
            {
                var currentIndex = DataViewAreas.IndexOf(CurrentDataViewArea);
                DataViewAreas.Insert(currentIndex + 1, dataArea);
            }
            CurrentDataViewArea = dataArea;

            SetLinks();
        }

        //private void DataViewArea_DataItemsSearchedByUser(object sender, EventArgs e)
        //{
        //    var hostDataViewArea = sender as DataViewArea;
        //    hostDataViewArea.DefaultDataViewItem = null;
        //    CurrentDataViewArea = hostDataViewArea;
        //    SetLinks();
        //}


        private void SetLinks()
        {
            var lastindex = DataViewAreas.IndexOf(CurrentDataViewArea);
            if (lastindex != -1)
            {
                List<I_DataArea> listRemove = new List<I_DataArea>();
                var index = 0;
                foreach (var item in DataViewAreas)
                {
                    if (index > lastindex)
                        listRemove.Add(item);
                    index++;
                }
                foreach (var item in listRemove)
                {
                    DataViewAreas.Remove(item);
                }
            }
            List<DataViewLink> links = new List<DataViewLink>();
            foreach (var item in DataViewAreas)
            {
                DataViewLink link = new DataViewLink();
                link.Title = item.AreaInitializer.Title.ToString();
                //link.Tooltip item.AreaInitializer
                link.DataViewLinkClicked += (sender, e) => Link_DataViewLinkClicked(sender, e, item);
                links.Add(link);
            }
            View.ShowLinks(links);
        }

        private void Link_DataViewLinkClicked(object sender, EventArgs e, I_DataArea dataViewArea)
        {
            CurrentDataViewArea = dataViewArea;
            //LastDataViewArea = dataViewArea;
            View.AddDataViewArea(dataViewArea.View);
            if (dataViewArea is I_DataViewArea)
            {
                if ((dataViewArea as I_DataViewArea).DefaultDataViewItem != null)
                    (dataViewArea as I_DataViewArea).View.BringIntoView((dataViewArea as I_DataViewArea).DefaultDataViewItem);
            }
            //SetLinks();
        }
    }

}
