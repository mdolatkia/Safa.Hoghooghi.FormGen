using AutoMapper;
using CommonDefinitions.UISettings;
using ModelEntites;
using MySecurity;
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
    public class SearchEntityAreaAdvancedAndRaw : I_AdvancedAndRawSearchEntityArea
    {
        public SearchEntityAreaAdvancedAndRaw()
        {
            SearchCommands = new List<I_Command>();
        }

        public I_View_SearchEntityArea SearchView { set; get; }

        public I_RawSearchEntityArea RawSearchEntityArea
        {
            set; get;
        }

        public I_AdvancedSearchEntityArea AdvancedSearchEntityAre
        {
            set; get;
        }

        public List<I_Command> SearchCommands
        {
            set;
            get;
        }

        public SearchEntityAreaInitializer SearchInitializer { set; get; }

        public event EventHandler<SearchDataArg> SearchDataDefined;

        //public event EventHandler<AdvanceOrRawArg> SearchDataDefined;

        //void SearchView_CommandExecuted(object sender, Arg_CommandExecuted e)
        //{
        //    (e.Command as I_Command).Execute(this);
        //}
        public void SetAreaInitializer(SearchEntityAreaInitializer newAreaInitializer)
        {
            SearchInitializer = newAreaInitializer;

            SearchView = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateViewOfSearchEntityArea();
            RawSearchEntityArea = new RawSearchEntityArea();
            RawSearchEntityArea.SetAreaInitializer(newAreaInitializer);
            RawSearchEntityArea.SearchDataDefined += RawSearchEntityArea_SearchDataDefined;
            SearchView.AddSimpleSearchView(RawSearchEntityArea.RawSearchView);
         

            AdvancedSearchEntityAre = new AdvancedSearchEntityArea();
            AdvancedSearchEntityAre.SetAreaInitializer(newAreaInitializer);
            AdvancedSearchEntityAre.SearchDataDefined += AdvancedSearchEntityAre_SearchDataDefined;
            SearchView.AddAdvancedSearchView(AdvancedSearchEntityAre.AdvancedSearchView);

            if (newAreaInitializer.EditSearchRepository!=null)
            {
                if (newAreaInitializer.EditSearchRepository.Phrases.Any(x => !(x is SearchProperty)))
                    SearchView.ActivateAdvancedView(); 
                else
                    SearchView.ActivateRawView();
            }
        }

        private void AdvancedSearchEntityAre_SearchDataDefined(object sender, SearchDataArg e)
        {
            if (SearchDataDefined != null)
            {
                SearchDataArg arg = new SearchDataArg();
                arg.SearchItems = e.SearchItems;
                SearchDataDefined(this, arg);
            }
        }

        private void RawSearchEntityArea_SearchDataDefined(object sender, SearchPropertyArg e)
        {
            if (SearchDataDefined != null)
            {
                SearchDataArg arg = new SearchDataArg();
                arg.SearchItems = GetSearchRepository();
                foreach (var item in e.SearchItems)
                {
                    arg.SearchItems.Phrases.Add(item);
                }
                SearchDataDefined(this, arg);
            }
        }

        private DP_SearchRepository GetSearchRepository()
        {
            DP_SearchRepository result = new DP_SearchRepository(SearchInitializer.EntityID);
            if (SearchInitializer.SourceRelationID != 0)
            {
                result.SourceRelationID = SearchInitializer.SourceRelationID;
                result.SourceToTargetMasterRelationshipType = SearchInitializer.SourceToTargetMasterRelationshipType;
                result.SourceToTargetRelationshipType = SearchInitializer.SourceToTargetRelationshipType;
                //result.TargetEntityID = SearchInitializer.TargetEntityID;
            }
            result.Name = SearchInitializer.Title;
            return result;
        }


        //private void GenerateSearchView()
        //{

        //}
        //private void SimpleSearchEntityArea_SearchDataDefined(object sender, SearchLogicArg e)
        //{
        //    OnSearchDataDefined(e.SearchItems);
        //}

        //public void OnSearchDataDefined(AdvanceOrRaw searchData)
        //{
        //    if (SearchDataDefined != null)
        //    {
        //        SearchDataDefined(this, new SearchLogicArg() { SearchItems = searchData });
        //    }
        //}

        public void ClearSearchData()
        {
            throw new NotImplementedException();
        }

        //public AdvanceOrRaw GetSearchRepository()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
