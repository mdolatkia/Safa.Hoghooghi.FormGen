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
    public class SearchViewEntityArea : I_SearchViewEntityArea
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

            CheckSearchInitially();
        }
        private SearchEntityArea GenereateSearchArea()
        {
            var searchEntityArea = new SearchEntityArea();
            var searchViewInitializer = new SearchEntityAreaInitializer();
            searchViewInitializer.EntityID = AreaInitializer.EntityID;
            // searchViewInitializer.TempEntity = AreaInitializer.TempEntity;
            searchEntityArea.SetAreaInitializer(searchViewInitializer);
            //searchEntityArea.GenerateSearchView();
            return searchEntityArea;
        }

        private ViewEntityArea GenereateViewArea()
        {
            var viewEntityArea = new ViewEntityArea();
            var viewAreaInitializer = new ViewEntityAreaInitializer();
            viewAreaInitializer.EntityID = AreaInitializer.EntityID;
            //    viewAreaInitializer.TempEntity = AreaInitializer.TempEntity;

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
                if (View.HaseViewAreaView)
                    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog(View);
                DataSelected(this, new DataSelectedEventArg() { DataItem = e.DataItem });
            }
        }

        private void SearchEntityArea_SearchDataDefined(object sender, SearchDataArg e)
        {
            SearchConfirmed(e.SearchItems, false);
        }
        public List<Tuple<int, object>> LastFilterValues = new List<Tuple<int, object>>();
        public List<Tuple<int, object>> CurrentValues = new List<Tuple<int, object>>();
        private DR_ResultSearchView GetSearchResult(DP_SearchRepository searchItems)
        {
            CalculateFilterValues();
            if (FilterCalculationError != null)
            {
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("خطا در جستجو", FilterCalculationError.Message, Temp.InfoColor.Red);
                return null;
            }
            LastFilterValues.Clear();
            foreach (var item in CurrentValues)
                LastFilterValues.Add(new Tuple<int, object>(item.Item1, item.Item2));

            if (RelationshipFilters != null)
            {
                foreach (var filter in RelationshipFilters)
                {
                    var valueRow = CurrentValues.FirstOrDefault(x => x.Item1 == filter.ID);
                    // var value = AgentHelper.GetValueSomeHow(AreaInitializer.SourceEditArea.ChildRelationshipInfo.ParentData, filter.ValueRelationshipTail, filter.ValueColumnID);
                    if (valueRow == null)
                        return null;
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
            request.EntityViewID = ViewEntityArea.EntityListView.ID;
            var result = AgentUICoreMediator.GetAgentUICoreMediator.requestRegistration.SendSearchViewRequest(request);
            return result;
        }
        public void SearchConfirmed(DP_SearchRepository searchItems, bool select)
        {
            //try
            //{
            var result = GetSearchResult(searchItems);

            UseSearchResult(result, select);

            //}
            //catch (Exception ex)
            //{
            //    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("خطا در جستجو", ex.Message, Temp.InfoColor.Red);
            //}
        }

        private void UseSearchResult(DR_ResultSearchView result, bool select)
        {
            if (result.Result != Enum_DR_ResultType.ExceptionThrown)
            {
                if (result != null)
                {
                    if (!select)
                        ViewEntityArea.AddData(result.ResultDataItems, true);
                    else
                    {
                        if (DataSelected != null)
                        {
                            DataSelected(this, new DataSelectedEventArg() { DataItem = result.ResultDataItems });
                        }
                    }
                }
            }
            else
            {
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowMessage(result.Message);
            }
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
                            _RelationshipFilters = AgentUICoreMediator.GetAgentUICoreMediator.relationshipFilterManagerService.GetRelationshipFilters(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), AreaInitializer.SourceEditArea.AreaInitializer.SourceRelation.Relationship.ID);
                        }
                    }
                    else
                        return null;
                }
                return _RelationshipFilters;
            }
            set
            {
                _RelationshipFilters = value;
            }
        }

        public bool IsCalledFromDataView
        {
            set; get;
        }



        public event EventHandler<DataSelectedEventArg> DataSelected;

        public bool SearchInitialyDone { get; set; }
        public void ShowTemporarySearchView()
        {
            //CalculateFilterValues();
            //bool filtersChanged = false;
            //if (CurrentValues.Any(x => !LastFilterValues.Any(y => x.Item1 == y.Item1 && x.Item2 == y.Item2)) ||
            // LastFilterValues.Any(x => !CurrentValues.Any(y => x.Item1 == y.Item1 && x.Item2 == y.Item2)))
            //    filtersChanged = true;
            ////بعدا که حالت کمبو هم اضافه شد اینها اعمال شوند
            //if (filtersChanged)
            //{
            //    searchInitialyDone = false;
            //    ViewEntityArea.AddData(new List<DP_DataView>(), true);
            //}


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

            View.RemoveViewAreaView(ViewEntityArea.ViewView);
            if (!View.HaseViewAreaView)
                View.AddViewAreaView(ViewEntityArea.ViewView);
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(View, AreaInitializer.SourceEditArea.SimpleEntity.Alias, Enum_WindowSize.Big);
        }

        public void CheckSearchInitially()
        {
            if (SearchInitialyDone == false)
            {
                bool? sarchInitially = null;
                if (AreaInitializer.SourceEditArea.SimpleEntity.SearchInitially == true || (AreaInitializer.SourceEditArea.AreaInitializer.SourceRelation != null && AreaInitializer.SourceEditArea.AreaInitializer.SourceRelation.Relationship.SearchInitially))
                    sarchInitially = AreaInitializer.SourceEditArea.SimpleEntity.SearchInitially;
                if (sarchInitially == true)
                {
                    SearchInitialy();
                }
            }
        }

        public void SearchInitialy()
        {
            DP_SearchRepository searchItems = new DP_SearchRepository(AreaInitializer.EntityID);
            SearchInitialyDone = true;
            SearchConfirmed(searchItems, false);
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
                            var value = AgentUICoreMediator.GetAgentUICoreMediator.formulaManager.GetValueSomeHow(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), AreaInitializer.SourceEditArea.ChildRelationshipInfo.SourceData, filter.ValueRelationshipTail, filter.ValueColumnID);
                            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                            {
                                CurrentValues.Add(new Tuple<int, object>(filter.ID, value));
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

        public void SelectFromParent(bool isCalledFromDataView, RelationshipDTO relationship, DP_DataRepository parentDataItem, Dictionary<int, string> colAndValues)
        {
            IsCalledFromDataView = isCalledFromDataView;
            DP_SearchRepository searchItems = new DP_SearchRepository(AreaInitializer.EntityID);
            foreach (var item in relationship.RelationshipColumns)
            {
                if (colAndValues.ContainsKey(item.FirstSideColumnID))
                {
                    searchItems.Phrases.Add(new SearchProperty() { ColumnID = item.SecondSideColumnID, Value = colAndValues[item.FirstSideColumnID] });
                }
            }
            SearchConfirmed(searchItems, true);
        }

        //public void SearchAsComboBox()
        //{
        //    CalculateFilterValues();
        //    bool filtersChanged = false;
        //    if (CurrentValues.Any(x => !LastFilterValues.Any(y => x.Item1 == y.Item1 && x.Item2 == y.Item2)) ||
        //     LastFilterValues.Any(x => !CurrentValues.Any(y => x.Item1 == y.Item1 && x.Item2 == y.Item2)))
        //        filtersChanged = true;
        //    //بعدا که حالت کمبو هم اضافه شد اینها اعمال شوند
        //    //if (filtersChanged)
        //    //{
        //    //    searchInitialyDone = false;
        //    //    ViewEntityArea.AddData(new List<DP_DataView>(), true);
        //    //}

        //    //searchInitialyDone = true;
        //    DP_SearchRepository searchItems = new DP_SearchRepository(AreaInitializer.EntityID);
        //    searchInitialyDone = true;
        //    SearchConfirmed(searchItems, true, false);

        //}

        public async void SearchTextBox(string text)
        {
            SearchInitialyDone = true;
            DP_SearchRepository searchItems = new DP_SearchRepository(AreaInitializer.EntityID);
            var logicPhrase = SearchEntityArea.SimpleSearchEntityArea.GetQuickSearchLogicPhrase(text, SearchEntityArea.SimpleSearchEntityArea.EntitySearch);
            searchItems.Phrases.Add(logicPhrase);
            var result = await SearchAsync(searchItems);
            UseSearchResult(result, false);
        }
        private Task<DR_ResultSearchView> SearchAsync(DP_SearchRepository searchItems)
        {
            return Task.Run(() =>
            {
                var result = GetSearchResult(searchItems);
                return result;
            });
        }
        public void RemoveViewEntityAreaView()
        {
            View.RemoveViewAreaView(ViewEntityArea.ViewView);
        }
    }
}
