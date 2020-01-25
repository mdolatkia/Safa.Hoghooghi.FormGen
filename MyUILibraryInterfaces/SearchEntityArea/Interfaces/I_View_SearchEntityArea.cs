
using CommonDefinitions.UISettings;
using ModelEntites;
using MyUILibrary.EntityArea.Commands;

using MyUILibrary.Temp;
using System;
using System.Collections.Generic;
using ProxyLibrary;

namespace MyUILibrary.EntityArea
{
    public interface I_View_SimpleSearchEntityArea : I_View_GridContainer
    {





        //bool AddControlPackageToHeader(object uiControlPackage, string title, InfoColor titleColor, string tooltip = "");


        //I_SearchViewEntityArea Controller
        //{
        //    get;
        //    set;
        //}

        //UIControlPackage GenerateControl(ColumnDTO uI_PackagePropertySetting, ColumnSetting columnSetting);

        //void AddControls( UIControlPackage controlPackage, string title);

        //bool ShowControlValue(UIControlPackage controlPackage, ColumnDTO control, string value);
        //string FetchControlValue(UIControlPackage controlPackage, ColumnDTO control);
        //void AddCommands(List<I_SearchAreaCommand> commands);
        //void GenerateSearchTemplate(DataAccess.Entity searchTemplate);


        //void UpdateSearchDataPckage(DataAccess.Entity dataPackage);
        //void ShowSearchDataPckage(DataAccess.Entity dataPackage);



    }
    public interface I_View_SearchEntityArea
    {
        void AddSimpleSearchView(object view);
        void AddAdvancedSearchView(object view);
        //void ActivateAdvancedView();
        //void ActivateSimpleView();
        bool IsSimpleSearchActiveOrAdvancedSearch { set; get; }
        void DisableEnable(bool enable);
    }
    public interface I_View_AdvancedSearchEntityArea : I_View_Area
    {
        I_AdvanceSearchNodeManager AddTreeItem();
       void ClearTreeItems();
    }

    public class AdvanceSearchNode
    {
        public AdvanceSearchNode()
        {
            ChildItems = new List<EntityArea.AdvanceSearchNode>();
            //Phrase = new  LogicPhrase();
        }
        public string Title { set; get; }
        public Phrase Phrase { get; set; }

        public AdvanceSearchNode ParentNode { set; get; }
        public List<AdvanceSearchNode> ChildItems { set; get; }
        public I_AdvanceSearchNodeManager NodeManager { set; get; }
        public int EntityID { set; get; }
    }
    public interface I_AdvanceSearchNodeManager
    {
        void SetHeader(string header);
        I_AdvanceSearchMenu AddMenu(string v);
        I_AdvanceSearchNodeManager AddChildItem();
        void AddExistingChildItem(I_AdvanceSearchNodeManager item);
        void RemoveItem(I_AdvanceSearchNodeManager nodeManager);
        void ClearItems();
    }
    public interface I_AdvanceSearchMenu
    {
        event EventHandler Clicked;
    }
}
