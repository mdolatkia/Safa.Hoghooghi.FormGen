
using CommonDefinitions.UISettings;
using ModelEntites;
using MyUILibrary.EntityArea.Commands;
using ProxyLibrary;
using System;
using System.Collections.Generic;
namespace MyUILibrary.EntityArea
{
    public interface I_View_TemporaryView : I_DataControlManager
    {
        // ColumnDTO Column { set; get; }
        //DP_DataRepository ParentDataItem { set; get; }
        event EventHandler<Arg_TemporaryDisplayViewRequested> TemporaryDisplayViewRequested;
        event EventHandler<Arg_TemporaryDisplaySerachText> SearchTextChanged;
        event EventHandler FocusLost;
        //void SetTextReadonly(bool isreadonly);
        void SetLinkText(string text);

        //List<DP_DataRepository> CurrentDataItems { set; get; }
        //TemporaryLinkType LinkType { get; }
        bool HasPopupView { get; }

        //void SetComboBoxVisibile(bool combo);
        void DisableEnable(bool enable);
        void DisableEnable(TemporaryLinkType link, bool enable);
        string GetSearchText();
        void ClearSearchText();
        void AddPopupView(I_View_ViewEntityArea viewView);
        void RemovePopupView(I_View_ViewEntityArea viewView);
        bool PopupVisibility { set; get; }
        bool QuickSearchVisibility { set; get; }
        void QuickSearchSelectAll();
    }
    public interface I_DialogWindow
    {
        void CloseDialog();
        event EventHandler WindowClosed;
        void ShowDialog(object view, string title, Enum_WindowSize windowSize = Enum_WindowSize.None, bool hideMaximizeButton = false);
        void ShowWindow(object view, string title, Enum_WindowSize windowSize = Enum_WindowSize.None, bool hideMaximizeButton = false);
    }
}
