using ModelEntites;
using MyUILibrary.EntityArea.Commands;

using ProxyLibrary;
using System;
using System.Collections.Generic;
namespace MyUILibrary.EntityArea
{
    public interface I_SearchViewEntityArea
    {
        event EventHandler<DataSelectedEventArg> DataSelected;

        I_View_SearchViewEntityArea View { set; get; }

        //List<I_Command> SearchViewCommands
        //{
        //    get;
        //    set;
        //}
        void CheckSearchInitially();
        List<RelationshipFilterDTO> RelationshipFilters { set; get; }
        SearchViewAreaInitializer AreaInitializer { set; get; }
        void SetAreaInitializer(SearchViewAreaInitializer initParam);
        void SearchInitialy();
        I_SearchEntityArea SearchEntityArea { set; get; }
        I_ViewEntityArea ViewEntityArea { set; get; }
        bool IsCalledFromDataView { get; set; }
        bool SearchInitialyDone { get; set; }

        //void SearchAsComboBox();

        void SearchConfirmed(DP_SearchRepository searchItems, bool select);
        void ShowTemporarySearchView();
        void SelectFromParent(bool isCalledFromDataView, RelationshipDTO relationship, DP_DataRepository parentDataItem, Dictionary<int, string> colAndValues);
        void SearchTextBox(string text);
        void RemoveViewEntityAreaView();
    }


}
