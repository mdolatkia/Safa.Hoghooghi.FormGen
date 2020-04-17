
using CommonDefinitions.UISettings;
using ModelEntites;
using MyUILibrary.EntityArea.Commands;
using MyUILibrary.Temp;

using ProxyLibrary;
using ProxyLibrary.Workflow;
using System;
using System.Collections.Generic;


namespace MyUILibrary.EntityArea
{
    public enum UserPromptMode
    {
        YesNo
    }
    public enum UserDialogMode
    {
        Ok,
        YesNo
    }
    public enum UserDialogResult
    {
        None,
        Ok,
        Yes,
        No
    }
    public interface I_ViewDialog
    {
        event EventHandler<ConfirmModeClickedArg> ButtonClicked;
    }
    public interface I_ViewDeleteInquiry : I_ViewDialog
    {
        void SetUserConfirmMode(UserDialogMode mode);
        void SetMessage(string message);
        void SetTreeItems(List<DP_DataRepository> dataItems);
    }
    public class ConfirmModeClickedArg : EventArgs
    {
        public UserDialogResult Result { set; get; }
    }
    public interface I_View_EditEntityAreaDataView : I_View_GridContainer, I_View_DataContainer
    {

    }


    public interface I_View_GridContainer : I_View_Container
    {
        //void ClearControls();
        //object ContentGrid { get; }
        //GridSetting GridSetting { set; get; }
        void AddGroup(I_UICompositionContainer view, string title, GroupUISettingDTO groupUISettingDTO);
        void AddTabGroup(I_TabGroupContainer view, string title, TabGroupUISettingDTO groupUISettingDTO);
        //void AddTabPage(I_TabGroupContainer tabGroupContainer, I_TabPageContainer view, string title, TabPageUISettingDTO groupUISettingDTO);
        //    I_UICompositionContainer GenerateGroup(GroupUISettingDTO groupUISettingDTO);
        //    I_TabGroupContainer GenerateTabGroup(TabGroupUISettingDTO groupUISettingDTO);
        //    I_TabPageContainer GenerateTabPage(TabPageUISettingDTO groupUISettingDTO);

        // void RemoveUIControlPackage(I_SimpleControlManager control);
        void RemoveView(object view);

        //     I_SimpleControlManager GenerateControlManager(ColumnDTO column, ColumnUISettingDTO columnSetting, bool hasRangeOfValues, bool valueIsTitleOrValue, List<SimpleSearchOperator> operators);
        //   I_RelationshipControlManager GenerateRelationshipControlManager(object view, RelationshipUISettingDTO relationshipUISetting);
        void AddEmptySpace(EmptySpaceUISettingDTO setting);
    }

    public interface I_View_Area
    {
    //    object UIElement { get; }
        void DeHighlightCommands();
        void AddCommand(I_CommandManager command, bool indirect = false);
        void SetBackgroundColor(string color);
        void DisableEnableDataSection(bool enable);
        void Visiblity(bool visible);
        void EnableDisable(bool enable);
    }
    public interface I_View_Container : I_View_Area
    {
        void AddView(object view, I_LabelControlManager labelControlManager);
        void AddUIControlPackage(I_SimpleControlManager controlManager, I_LabelControlManager labelControlManager);
        void ClearControls();
        int ControlsCount { get; }
        void SetTooltip(string tooltip);
        void SetBorderColor(InfoColor color);
        void SetBackgroundColor(InfoColor color);
        void SetForegroundColor(InfoColor color);
        bool IsOpenedTemporary { get; set; }
        //event EventHandler<Arg_CommandExecuted> CommandExecuted;
        //void AddCommands(List<I_Command> Commands, TemplateEntityUISettings templateEntityUISettings);
        UIControlPackageTree UIControlPackageTreeItem { get; set; }

        //bool AllowExpand { set; get; }
        //void SetExpanderInfo(object header);
        //void Expand();
        //void Collapse();
        //void SetVisibility(bool visible);

    }
    public interface I_View_DataContainer : I_View_Container, I_DataControlManager
    {

    }
    public interface I_View_MultipleDataContainer : I_View_DataContainer
    {
        event EventHandler<DataContainerLoadedArg> DataContainerLoaded;
        event EventHandler<DataContainerLoadedArg> ItemDoubleClicked;
        //I_RelationshipControlManager GenerateRelationshipControlManager(TemporaryLinkState temporaryLinkState, RelationshipUISettingDTO relationshipUISettingDTO);


        //I_SimpleControlManager GenerateControlManager(ColumnDTO column, ColumnUISettingDTO columnUISettingDTO, bool hasRangeOfValues, bool valueIsTitleOrValue);
        //bool ShowMultipleDateItemControlValue(DP_DataRepository dataItem, UIControlPackageMultipleData controlPackage, string value);

        //string FetchMultipleDateItemControlValue(DP_DataRepository dataItem, UIControlPackageMultipleData controlPackage);
        void CleraUIControlPackages();
        bool MultipleSelection { get; set; }
        void AddDataContainer(object specificDate);

        List<object> RemoveDataContainers();

        List<object> GetSelectedData();
        void RemoveUIControlPackage(I_SimpleControlManager controlManager);
        void RemoveView(I_RelationshipControlManager controlManager);
        void SetSelectedData(List<object> dataItems);

        void RemoveDataContainer(object data);
        void Visiblity(object dataItem, bool visible);
        void EnableDisable(object dataItem, bool enable);
        //void RemoveSelectedDataContainers();

    }
    public interface I_View_ViewEntityAreaMultiple : I_View_MultipleDataContainer
    {

    }

    public interface I_View_EditEntityAreaMultiple : I_View_MultipleDataContainer
    {
        //void AddValidation(DataMessageItem item);
        //void ClearValidation(DP_DataRepository item);
        //void ClearValidation();

    }

    //public class Arg_DataDependentControlGeneration : EventArgs
    //{


    //    //public Arg_DataContainer()
    //    //{
    //    //    //   DataItems = new List<DP_DataRepository>();
    //    //}
    //    public object DataItem { set; get; }
    //    public UIControlPackage ControlPackage { set; get; }
    //    //public DP_DataRepository DataItem;




    //}
    //public interface IAG_DataDependentControl
    //{
    //    UIControlSetting UIControlSetting { set; get; }
    //    //object DataItem { set; get; }
    // //   event EventHandler<Arg_DataDependentControlGeneration> DataControlGenerated;
    //    //void SetLinkText(string text);
    //}
    public class Arg_DataContainer : EventArgs
    {


        public Arg_DataContainer()
        {
            //   DataItems = new List<DP_DataRepository>();
        }


        public DP_DataRepository DataItem;




    }
    public class Arg_TemporaryDisplayViewRequested : EventArgs
    {
        public TemporaryLinkType LinkType { set; get; }
    }
    public class Arg_TemporaryDisplaySerachText : EventArgs
    {
        public string Text { set; get; }
    }
    public class Arg_MultipleTemporaryDisplayViewRequested : EventArgs
    {
        public object DataItem { set; get; }
        public TemporaryLinkType LinkType { set; get; }
    }
    public class Arg_MultipleTemporaryDisplayLoaded : EventArgs
    {
        public object DataItem { set; get; }
    }
    public class Arg_CommandExecuted : EventArgs
    {
        public I_Command Command { set; get; }


    }
    public class EntitySelectorArg : EventArgs
    {
        public int EnitityID { set; get; }
    }
    public class ProcessSelectedArg : EventArgs
    {
        public int ProcessID { set; get; }
    }
    public class StateSelectedArg : EventArgs
    {
        public int StateID { set; get; }
    }
    public class RequestActionConfirmedArg : EventArgs
    {
        public RequestActionConfirmDTO RequestActionConfirm { set; get; }
    }
    public class RequestNoteConfirmedArg : EventArgs
    {
        public RequestNoteDTO RequestNote { set; get; }
    }
    public class RequestNoteSelectedArg : EventArgs
    {
        public RequestNoteDTO RequestNote { set; get; }
    }
    public class RequestFileConfirmedArg : EventArgs
    {
        public RequestFileDTO RequestFile { set; get; }
    }
    public class RequestFileSelectedArg : EventArgs
    {
        public RequestFileDTO RequestFile { set; get; }
    }
    //public class CartableEntityClick
    //{
    //    public string Title { set; get; }
    //    //public int RequestActionID { set; get; }
    //    public int RequestID { set; get; }
    //    public int RelationshipID { set; get; }
    //    public int EnitityID { set; get; }
    //}
    public class CartableMenuClickArg : EventArgs
    {
        public WorkflowRequestDTO Request { set; get; }
        public object ContextMenu { set; get; }
    }
    public class CartableActionConfirmClickArg : EventArgs
    {
        public RequestActionDTO RequestAction { set; get; }

        //public int RequestActionID { set; get; }
    }


    public class DataContainerLoadedArg : EventArgs
    {
        public object DataItem { set; get; }

    }
    //public interface I_View_Container : I_View_Container
    //{

    //}
    //public interface I_View_Container
    //{
    //    I_Container Container { set; get; }
    //    void AddUIControlPackage(UIControlPackage controlPackage, string title, InfoColor titleColor, string tooltip = "");
    //}
    //public interface I_Container
    //{
    //    GridSetting GridSetting { set; get; }
    //    void AddUIControlPackage(UIControlPackage controlPackage, string title, InfoColor titleColor, string tooltip = "");
    //    UIControlPackage GenerateGroup(UIControlSetting uiControlSetting, string groupTitle);
    //}
    //public class AG_View_EditNDTypeArea
    //{



    //}
    //public class AG_View_TemporaryArg
    //{
    //    public bool TemporaryDataView { set; get; }
    //    public bool TemporarySearchView { set; get; }
    //}
    //public interface IAG_View_TemporaryDisplayView
    //{
    //    object DataItem { set; get; }
    //    event EventHandler<Arg_TemporaryDisplayViewRequested> TemporaryDisplayViewRequested;
    //    void SetLinkText(string text);
    //}

}
