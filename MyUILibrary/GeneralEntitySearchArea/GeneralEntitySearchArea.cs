using MyUILibraryInterfaces.EntityArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyUILibrary.EntityArea;
using ProxyLibrary;
using ModelEntites;
using MyCommonWPFControls;

namespace MyUILibrary.EntitySearchArea
{
    public class GeneralEntitySearchArea : I_GeneralEntitySearchArea
    {
        MySearchLookup entitySearchLookup;

        public GeneralEntitySearchAreaInitializer AreaInitializer { set; get; }

        public GeneralEntitySearchArea()
        {
            View = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateViewOfGeneralEntitySearchArea();
            entitySearchLookup = new MySearchLookup();
            entitySearchLookup.DisplayMember = "Alias";
            entitySearchLookup.SelectedValueMember = "ID";
            entitySearchLookup.SearchFilterChanged += EntitySearchLookup_SearchFilterChanged;
            entitySearchLookup.SelectionChanged += EntitySearchLookup_SelectionChanged;
            View.AddEntitySelector(entitySearchLookup, "موجودیتها");
        }
        public void SetInitializer(GeneralEntitySearchAreaInitializer areaInitializer)
        {
            AreaInitializer = areaInitializer;
            View.AddExternalArea(areaInitializer.ExternalView);
            if (areaInitializer.LockEntitySelector)
                entitySearchLookup.IsEnabledLookup = false;
            if (areaInitializer.EntityID != 0)
            {
                entitySearchLookup.SelectedValue = areaInitializer.EntityID;
            }
            View.SearchLinkClicked += View_SearchLinkClicked;
            //View.AddExternalArea(AreaInitializer.ExternalView);
        }
        private void EntitySearchLookup_SearchFilterChanged(object sender, SearchFilterArg e)
        {
            if (!string.IsNullOrEmpty(e.SingleFilterValue))
            {
                if (e.FilterBySelectedValue)
                {
                    var entity = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetSimpleEntity(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), Convert.ToInt32(e.SingleFilterValue), AreaInitializer.SpecificActions);
                    if (entity != null)
                        e.ResultItemsSource = new List<TableDrivedEntityDTO> { entity };
                }
                else
                {
                    var entities = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.SearchEntities(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), e.SingleFilterValue, null, AreaInitializer.SpecificActions);
                    e.ResultItemsSource = entities;
                }
            }
        }
        private void EntitySearchLookup_SelectionChanged(object sender, SelectionChangedArg e)
        {
            if (e.SelectedItem != null)
            {
                var entity = e.SelectedItem as TableDrivedEntityDTO;
                SearchArea = new SearchEntityArea();
                var searchViewInitializer = new SearchEntityAreaInitializer();
                searchViewInitializer.EntityID = entity.ID;
                //searchViewInitializer.TempEntity = FullEntity;
                searchViewInitializer.PreDefinedSearch = AreaInitializer.PreDefinedSearch;
                if (AreaInitializer.PreDefinedSearch != null)
                {
                    searchViewInitializer.SearchEntityID = AreaInitializer.PreDefinedSearch.EntitySearchID;
                }

                SearchArea.SetAreaInitializer(searchViewInitializer);
                SearchArea.SearchDataDefined += SearchArea_SearchDataDefined;

                if (EntitySelected != null)
                    EntitySelected(this, entity.ID);
            }
            else
            {
                SearchArea = null;
                //if (SearchDataDefined != null)
                //    SearchDataDefined(this, null);

                if (EntitySelected != null)
                    EntitySelected(this, null);
                //View.RemoveDataSelector();
                //if (DataItemSelected != null)
                //    DataItemSelected(this, new EditAreaDataItemArg() { DataItem = null });
            }
        }

        private void View_SearchLinkClicked(object sender, EventArgs e)
        {
            if (SearchArea != null)
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(SearchArea.SearchView, "جستجو", Enum_WindowSize.Big);
        }
        public event EventHandler<SearchDataArg> SearchDataDefined;
        public event EventHandler<int?> EntitySelected;

        private void SearchArea_SearchDataDefined(object sender, SearchDataArg e)
        {
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog(SearchArea.SearchView);
            if (SearchDataDefined != null)
                SearchDataDefined(this, e);
        }


        public TableDrivedEntityDTO SelectedEntity
        {
            get
            {
                return entitySearchLookup.SelectedItem as TableDrivedEntityDTO;
            }
        }
        public void EnableDisableSearchArea(bool enable)
        {
            View.EnableDisableSearchLink(enable);
        }


        public I_View_GeneralEntitySearchArea View
        {
            set; get;
        }

        public I_SearchEntityArea SearchArea
        {
            set; get;
        }
        TableDrivedEntityDTO _FullEntity;
        public TableDrivedEntityDTO FullEntity
        {
            get
            {
                if (_FullEntity == null)
                    _FullEntity = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetFullEntity(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), AreaInitializer.EntityID);
                return _FullEntity;
            }
        }
    }
}
