
using CommonDefinitions.UISettings;
using ModelEntites;
using MyUILibrary.EntityArea.Commands;

using System;
using System.Collections.Generic;
namespace MyUILibrary.EntityArea
{
    //public interface I_View_DataDependentControl
    //{
    //    event EventHandler<Arg_TemporaryDisplayViewRequested> TemporaryViewRequested;
    //    ////ColumnDTO Column { set; get; }
    //    //IAG_View_TemporaryView GenerateTemporaryView();


    //    //void OnTemporaryViewRequested(object sender, Arg_TemporaryDisplayViewRequested arg);

    //    //TemporaryLinkType LinkType {  get; }

    //    void SetTemporaryViewText(object dataItem, string text);
    //}
    public enum TemporaryLinkType
    {
        None,
        DataView,
        SerachView,
        //DataSearchView,
        QuickSearch,
        Info,
        Clear,
        Popup
    }

    public class TemporaryLinkState
    {
        public bool popup;
        public bool quickSearch;
        public bool edit;
        public bool searchView;
        public bool clear;
        public bool info;
    }
}
