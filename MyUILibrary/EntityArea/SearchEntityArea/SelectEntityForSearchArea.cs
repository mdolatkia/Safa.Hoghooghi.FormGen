using AutoMapper;
using CommonDefinitions.UISettings;
using ModelEntites;
using MyUILibrary;
using MyUILibrary.EntityArea;
using MyUILibrary.EntityArea.Commands;

using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.EntityArea
{
    public class SelectEntityForSearchArea : I_SearchViewEntityArea
    {

        public SearchViewEntityArea()
        {



        }
        public void SetAreaInitializer(SearchViewAreaInitializer initParam)
        {
            AreaInitializer = initParam;
            //GetRelationshipFilters();
            SearchEntityArea = GenereateSearchArea();
            SearchEntityArea.SearchDataDefined += SearchEntityArea_SearchDataDefined;
            ViewEntityArea = GenereateViewArea();
            ViewEntityArea.DataSelected += ViewEntityArea_DataSelected;
            View = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateViewOfSearchViewEntityArea();
            View.AddSearchAreaView(SearchEntityArea.SearchView);
            View.AddViewAreaView(ViewEntityArea.ViewView);
        }
        private SearchEntityArea GenereateSearchArea()
        {
            var searchEntityArea = new SearchEntityArea();
            var searchViewInitializer = new SearchEntityAreaInitializer();
            searchViewInitializer.EntityID = AreaInitializer.EntityID;
            searchViewInitializer.TempEntity = AreaInitializer.TempEntity;
            searchEntityArea.SetAreaInitializer(searchViewInitializer);
            //searchEntityArea.GenerateSearchView();
            return searchEntityArea;
        }

        private ViewEntityArea GenereateViewArea()
        {
            var viewEntityArea = new ViewEntityArea();
            var viewAreaInitializer = new ViewEntityAreaInitializer();
            viewAreaInitializer.EntityID = AreaInitializer.EntityID;
            viewAreaInitializer.TempEntity = AreaInitializer.TempEntity;
         
            viewAreaInitializer.MultipleSelection = AreaInitializer.MultipleSelection;
            viewEntityArea.SetAreaInitializer(viewAreaInitializer);
            //viewEntityArea.GenerateView();
            return viewEntityArea;
        }

        //private void GetRelationshipFilters()
        //{

        //}

        private void ViewEntityArea_DataSelected(object sender, DataSelectedEventArg e)
        {
            if (DataSelected != null)
            {
                DataSelected(this, new DataSelectedEventArg() { DataItem = e.DataItem });
            }
        }

        private void SearchEntityArea_SearchDataDefined(object sender, SearchDataArg e)
        {
            SearchConfirmed(e.SearchItems, true, false);
        }
        public List<Tuple<int, string>> LastFilterValues = new List<Tuple<int, string>>();
        public List<Tuple<int, string>> CurrentValues = new List<Tuple<int, string>>();
        public void SearchConfirmed(DP_SearchRepository searchItems, bool filterValuesClaculated, bool select)
        {
            //try
            //{
                if (filterValuesClaculated == false)
                    CalculateFilterValues();
                if (FilterCalculationError != null)
                {
                    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("خطا در جستجو", FilterCalculationError.Message, Temp.InfoColor.Red);
                    return;
                }
                LastFilterValues.Clear();
                foreach (var item in CurrentValues)
                    LastFilterValues.Add(new Tuple<int, string>(item.Item1, item.Item2));

                if (RelationshipFilters != null)
                {
                    foreach (var filter in RelationshipFilters)
                    {
                        var valueRow = CurrentValues.First(x => x.Item1 == filter.ID);
                        // var value = AgentHelper.GetValueSomeHow(AreaInitializer.SourceEditArea.ChildRelationshipInfo.ParentData, filter.ValueRelationshipTail, filter.ValueColumnID);

                        DP_SearchRepository searchItem = CreateSearchItem(searchItems, filter.SearchRelationshipTail);
                        //var searchColumn = searchItem.Phrases.FirstOrDefault(x => x is SearchProperty && (x as SearchProperty).ColumnID == filter.SearchColumnID) as SearchProperty;
                        //if (searchColumn == null)
                        //{
                        var searchColumn = new SearchProperty() { ColumnID = filter.SearchColumnID };
                        searchItem.Phrases.Add(searchColumn);
                        //}
                        searchColumn.Value = valueRow.Item2;

                    }
                }

                var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();
                //سکوریتی داده اعمال میشود
                DR_SearchViewRequest request = new DR_SearchViewRequest(requester, searchItems);
                //request.EntityID = AreaInitializer.EntityID;

                var reuslt = AgentUICoreMediator.GetAgentUICoreMediator.SendSearchViewRequest(request);

                if (!select)
                    ViewEntityArea.AddData(reuslt.ResultDataItems, true);
                else
                {
                    if (DataSelected != null)
                    {
                        DataSelected(this, new DataSelectedEventArg() { DataItem = reuslt.ResultDataItems });
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("خطا در جستجو", ex.Message, Temp.InfoColor.Red);
            //}
        }

        private DP_SearchRepository CreateSearchItem(DP_SearchRepository mainItem, EntityRelationshipTailDTO searchRelationshipTail)
        {
            if (searchRelationshipTail == null)
                return mainItem;
            else
            {
                var foundItem = new DP_SearchRepository(searchRelationshipTail.Relationship.EntityID2);
                foundItem.SourceRelationship = searchRelationshipTail.Relationship;
                //foundItem.SourceEntityID = searchRelationshipTail.Relationship.EntityID1;
                //foundItem.SourceToTargetRelationshipType = searchRelationshipTail.Relationship.TypeEnum;
                //foundItem.SourceToTargetMasterRelationshipType = searchRelationshipTail.Relationship.MastertTypeEnum;
                mainItem.Phrases.Add(foundItem);
                return CreateSearchItem(foundItem, searchRelationshipTail.ChildTail);
            }
        }




        public I_View_SearchViewEntityArea View
        {
            set; get;
        }

        //public List<I_Command> SearchViewCommands
        //{
        //    set; get;
        //}

        public I_SearchEntityArea SearchEntityArea
        {
            set; get;
        }

        public I_ViewEntityArea ViewEntityArea
        {
            set; get;
        }

        public SearchViewAreaInitializer AreaInitializer
        {
            set; get;
        }
        List<RelationshipFilterDTO> _RelationshipFilters;
        public List<RelationshipFilterDTO> RelationshipFilters
        {
            get
            {
                if (_RelationshipFilters == null)
                {
                    if (AreaInitializer.SourceEditArea.ChildRelationshipInfo != null)
                    {
                        if (AreaInitializer.SourceEditArea.AreaInitializer.SourceRelation != null)
                        {
                            _RelationshipFilters = AgentUICoreMediator.GetAgentUICoreMediator.GetRelationshipFilters(AreaInitializer.SourceEditArea.AreaInitializer.SourceRelation.Relationship.ID);
                        }
                    }
                    else
                        return null;
                }
                return _RelationshipFilters;
            }
        }

        public bool IsCalledFromDataView
        {
            set; get;
        }



        public event EventHandler<DataSelectedEventArg> DataSelected;

        bool searchInitialyDone;
        public void ShowTemporarySearchView()
        {
            CalculateFilterValues();
            bool filtersChanged = false;
            if (CurrentValues.Any(x => !LastFilterValues.Any(y => x.Item1 == y.Item1 && x.Item2 == y.Item2)) ||
             LastFilterValues.Any(x => !CurrentValues.Any(y => x.Item1 == y.Item1 && x.Item2 == y.Item2)))
                filtersChanged = true;
            //بعدا که حالت کمبو هم اضافه شد اینها اعمال شوند
            if (filtersChanged)
            {
                searchInitialyDone = false;
                ViewEntityArea.AddData(new List<DP_DataView>(), true);
            }
            bool? sarchInitially = null;
            if (AreaInitializer.SourceEditArea.AreaInitializer.SourceRelation != null)
            {
                sarchInitially = AreaInitializer.SourceEditArea.AreaInitializer.SourceRelation.Relationship.SearchInitially;
            }
            if (sarchInitially == null)
            {
                sarchInitially = AreaInitializer.SourceEditArea.SimpleEntity.SearchInitially;
            }

            //if (searchInitialyDone)
            //    sarchInitially = false;
            //if (sarchInitially == true)
            //{
            //    if (RelationshipFilters == null || RelationshipFilters.Count == 0)
            //    {
            //        if (searchInitialyDone)
            //            sarchInitially = false;
            //    }
            //}

            if (sarchInitially == true && searchInitialyDone == false)
            {
                DP_SearchRepository searchItems = new DP_SearchRepository(AreaInitializer.EntityID);
                searchInitialyDone = true;
                SearchConfirmed(searchItems, true, false);
            }
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowDialog(View, AreaInitializer.TempEntity.Alias, Enum_WindowSize.Big);
        }

        Exception FilterCalculationError = null;
        private void CalculateFilterValues()
        {
            FilterCalculationError = null;
            CurrentValues.Clear();
            try
            {
                if (AreaInitializer.SourceEditArea.ChildRelationshipInfo != null)
                {
                    if (RelationshipFilters != null && RelationshipFilters.Any())
                    {
                        foreach (var filter in RelationshipFilters)
                        {
                            var value = AgentHelper.GetValueSomeHow(AreaInitializer.SourceEditArea.ChildRelationshipInfo.ParentData, filter.ValueRelationshipTail, filter.ValueColumnID);
                            if (!string.IsNullOrEmpty(value))
                            {
                                CurrentValues.Add(new Tuple<int, string>(filter.ID, value));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FilterCalculationError = ex;
            }
        }

        public void SelectFromParent(bool isCalledFromDataView, RelationshipDTO relationship, DP_DataRepository parentDataItem, List<Tuple<int, string>> colAndValues)
        {
            IsCalledFromDataView = isCalledFromDataView;
            DP_SearchRepository searchItems = new DP_SearchRepository(AreaInitializer.EntityID);
            foreach (var item in relationship.RelationshipColumns)
            {
                var sentCol = colAndValues.FirstOrDefault(x => x.Item1 == item.FirstSideColumnID);
                if (sentCol != null)
                {
                    searchItems.Phrases.Add(new SearchProperty() { ColumnID = item.SecondSideColumnID, Value = sentCol.Item2 });
                }
            }
            SearchConfirmed(searchItems, false, true);
        }
    }
}
