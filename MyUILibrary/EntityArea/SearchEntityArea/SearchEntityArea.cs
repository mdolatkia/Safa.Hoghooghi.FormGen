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
    public class SearchEntityArea : I_SearchEntityArea
    {
        public SearchEntityArea()
        {

        }

        public I_View_SearchEntityArea SearchView { set; get; }

        public I_SimpleSearchEntityArea SimpleSearchEntityArea
        {
            set; get;
        }

        public I_AdvancedSearchEntityArea AdvancedSearchEntityAre
        {
            set; get;
        }

        public SearchEntityAreaInitializer SearchInitializer { set; get; }

        public event EventHandler<SearchDataArg> SearchDataDefined;


        public void SetAreaInitializer(SearchEntityAreaInitializer newAreaInitializer)
        {
            SearchInitializer = newAreaInitializer;

            SearchView = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateViewOfSearchEntityArea();
            SimpleSearchEntityArea = new SimpleSearchEntityArea();
            newAreaInitializer.SearchEntityID = newAreaInitializer.SearchEntityID;
            SimpleSearchEntityArea.SetAreaInitializer(newAreaInitializer);
            SimpleSearchEntityArea.SearchDataDefined += SimpleSearchEntityArea_SearchDataDefined;
            SearchView.AddSimpleSearchView(SimpleSearchEntityArea.SimpleSearchView);

            AdvancedSearchEntityAre = new AdvancedSearchEntityArea();
            AdvancedSearchEntityAre.SetAreaInitializer(newAreaInitializer);
            AdvancedSearchEntityAre.SearchDataDefined += SimpleSearchEntityArea_SearchDataDefined;
            SearchView.AddAdvancedSearchView(AdvancedSearchEntityAre.AdvancedSearchView);
            //if (newAreaInitializer.PreDefinedSearch != null && newAreaInitializer.EditSearchRepository != null)
            //    throw new Exception("sdfsbvxzcv");

            //بررسی شود دوتاشون میتونن باشن یا خطا باید بده
            //if (newAreaInitializer.PreDefinedSearch != null)
            //{
            //    newAreaInitializer.EditSearchRepository = newAreaInitializer.PreDefinedSearch;
            //}
            if (newAreaInitializer.PreDefinedSearch != null)
            {
                ShowSearchRepository(newAreaInitializer.PreDefinedSearch);
            }
        }

        public void ShowSearchRepository(DP_SearchRepository searchRepository)
        {
            if (searchRepository == null)
                return;
            bool showInSimple = false;
            if (searchRepository.IsSimpleSearch == true)
            {
                showInSimple = SimpleSearchEntityArea.ShowSearchRepository(searchRepository);
                if (showInSimple)
                    SearchView.IsSimpleSearchActiveOrAdvancedSearch = true;
            }
            if (!showInSimple)
            {
                if (!AdvancedSearchEntityAre.ShowSearchRepository(searchRepository))
                {
                    throw (new Exception("AsdasdasD"));
                }
                else
                    SearchView.IsSimpleSearchActiveOrAdvancedSearch = false;
            }
        }
        //private DP_SearchRepository ConvertPreDefinedSearch(EntityPreDefinedSearchDTO preDefinedSearch)
        //{
        //    //DP_SearchRepository result = new DP_SearchRepository();
        //    //result.TargetEntityID
        //    return null; 
        //}

        public bool IsSimpleSearchActiveOrAdvancedSearch
        {
            get { return SearchView.IsSimpleSearchActiveOrAdvancedSearch; }
        }
        //public void ShowPreDefinedSearch(EntityPreDefinedSearchDTO message)
        //{
        //    ShowSearchRepository(message.SearchRepository);
        //}

        private void SimpleSearchEntityArea_SearchDataDefined(object sender, SearchDataArg e)
        {
            //DP_SearchRepository searchData = new DP_SearchRepository(SearchInitializer.EntityID);
            //if (e.SearchItems != null)
            //{
            //    searchData.AndOrType = e.SearchItems.AndOrType;
            //    searchData.Phrases = e.SearchItems.Phrases;
            //}
            //OnSearchDataDefined(searchData);

            OnSearchDataDefined(e.SearchItems);
        }
        public DP_SearchRepository LastSearch { set; get; }
        public void OnSearchDataDefined(DP_SearchRepository searchData)
        {
            //if (searchData == null)
            //    searchData = new DP_SearchRepository(SearchInitializer.EntityID);

            //var arg = new SearchDataArg();
            //arg.SearchItems = new DP_SearchRepository(SearchInitializer.EntityID);
            //arg.SearchItems.AndOrType = searchData.AndOrType;
            //arg.SearchItems.Phrases = searchData.Phrases;
            //LastSearch = arg.SearchItems;
            //if (SearchDataDefined != null)
            //    SearchDataDefined(this, searchData);


            LastSearch = searchData;

            if (SearchDataDefined != null)
                SearchDataDefined(this, new SearchDataArg() { SearchItems = searchData });
        }

        public DP_SearchRepository GetSearchRepository()
        {
            if (SearchView.IsSimpleSearchActiveOrAdvancedSearch)
            {
                return SimpleSearchEntityArea.GetSearchRepository();
            }
            else
                return AdvancedSearchEntityAre.GetSearchRepository();
        }

        public void ClearSearchData()
        {
            throw new NotImplementedException();
        }

        //public DP_SearchRepository GetSearchRepository()
        //{
        //    //باید آخرین سرچ انجام شده نگهداری و اینجا برگردانده شود
        //    return lastSearch;
        //}
    }
}
