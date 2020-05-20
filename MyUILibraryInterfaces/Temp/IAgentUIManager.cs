
using CommonDefinitions.UISettings;
using ModelEntites;
using MyUILibrary.EntityArea;
using MyUILibrary.PackageArea;
using MyUILibrary.Temp;

using ProxyLibrary;
using ProxyLibrary.Workflow;
using System;
using System.Collections.Generic;
using MyUILibraryInterfaces.DataViewArea;
using MyUILibraryInterfaces.DataReportArea;
using MyUILibraryInterfaces.DataLinkArea;
using MyUILibraryInterfaces.DataTreeArea;
using MyUILibraryInterfaces.EntityArea;
using MyUILibraryInterfaces.DataMenuArea;
using MyUILibraryInterfaces.GridViewArea;
using MyUILibrary.WorkflowArea;
using MyUILibraryInterfaces.LogReportArea;
using MyUILibraryInterfaces.FormulaCalculationArea;
using MyUILibraryInterfaces.ContextMenu;

namespace MyUILibrary
{
    public interface IAgentUIManager
    {
        bool SearchNavigationTreeVisiblity { get; set; }
        bool NavigationTreeVisiblity { get; set; }

        //void ShowTestReport(string reportEngine, object reportSource);
        I_View_EntityLettersArea GenerateViewOfEntityLettersArea();
        I_View_FormulaCalculationArea GetViewOfFormulaCalculationArea();
        I_View_EntitySelectArea GenerateViewOfEntityselectArea();
        I_View_LetterArea GenerateViewOfLetterArea();
        I_View_ArchiveArea GenerateViewOfArchiveArea();
    //    void SetMenuItems(object contextMenu, List<ContextMenuItem> items);
        I_DialogWindow GetDialogWindow();
        void StartApp();
        UserDialogResult ShowPromptDialog(I_ViewDialog view, string title, Enum_WindowSize windowSize = Enum_WindowSize.None);
        //bool DataViewIsOpen(object dataView);
        bool ViewIsVisible(object view);

        //////IAgentUICoreMediator AgentUIMediator {
        //////    get;
        //////    set;
        //////}
        //event EventHandler SecuritySettingRequested;
        // event EventHandler WorkflowRequestCreate;
        //event EventHandler WorkflowRequestCreationForm;
         event EventHandler<string> SearchTextChanged;

        I_View_DataViewAreaContainer GetViewOfDataViewAreaContainer();
        I_View_DataViewArea GetViewOfDataViewArea();

        //event EventHandler<Arg_NavigationTreeRequest> NavigationTreeRequested;

        I_RelationshipControlManager GenerateRelationshipControlManagerForMultipleDataForm(TemporaryLinkState temporaryLinkState, RelationshipUISettingDTO relationshipUISetting, bool labelControlManager, string labelText);
        I_RelationshipControlManager GenerateRelationshipControlManagerForOneDataForm(object view, RelationshipUISettingDTO relationshipUISetting, bool labelControlManager, string labelText);
        I_View_EditLogReportDetails GenereateViewOfEditLogReportDetails();
        I_View_ArchiveLogReportDetails GenereateViewOfArchiveLogReportDetails();
        I_SimpleControlManager GenerateSimpleControlManagerForOneDataForm(ColumnDTO column, ColumnUISettingDTO columnSetting, bool hasRangeOfValues, List<SimpleSearchOperator> operators, bool labelControlManager, string labelText);
        I_SimpleControlManager GenerateSimpleControlManagerForMultipleDataForm(ColumnDTO column, ColumnUISettingDTO columnUISettingDTO, bool hasRangeOfValues,  bool labelControlManager, string labelText);

        I_UICompositionContainer GenerateGroup(GroupUISettingDTO groupUISettingDTO);


        I_TabGroupContainer GenerateTabGroup(TabGroupUISettingDTO groupUISettingDTO);



        I_TabPageContainer GenerateTabPage(TabPageUISettingDTO groupUISettingDTO);


        I_ConfirmUpdate GetConfirmUpdateForm();
        //I_View_DataListReportAreaContainer GetViewOfDataListReportAreaContainer();
        I_View_SearchViewEntityArea GenerateViewOfSearchViewEntityArea();
        I_View_InternalReportArea GenerateViewOfInternalReportArea();
        I_View_ExternalReportArea GenerateViewOfExternalReportArea();
        //event EventHandler DatabaseListRequested;

  //      event EventHandler<Arg_NavigationItemRequest> NavigationItemSelected;

        I_ViewDeleteInquiry GetDeleteInquiryView();

        //I_View_ArchiveTag GenerateViewOfArchiveTag();
        I_View_ArchiveTagFiltered GenerateViewOfArchiveTagFilter();
        I_NavigationMenu AddNavigationTree(I_NavigationMenu parentItem, NavigationItemDTO item, bool expanded);
        I_NavigationMenu AddSearchNavigationTree(I_NavigationMenu parentItem, NavigationItemDTO item, bool expanded);
        //I_View_EditPackageArea GenerateEditPackageAreaView();

        I_View_EditEntityAreaMultiple GenerateEditEntityAreaMultipleDataView();
        I_View_EditEntityAreaDataView GenerateEditEntityAreaOneDataView(EntityUISettingDTO entityUISettingDTO);
        I_View_DiagramStateInfo GetViewOfDiagramStateInfo();
        I_View_ArchiveItemInfo GenerateViewOfArchiveItemInfo();
        //I_View_DataListReportArea GetViewOfDataListReportArea();
        I_View_SimpleSearchEntityArea GenerateViewOfSearchEntityArea(EntityUISettingDTO entityUISettingDTO);
        I_View_GeneralEntitySearchArea GenerateViewOfGeneralEntitySearchArea();
        I_View_SearchEntityArea GenerateViewOfSearchEntityArea();

        I_View_ViewEntityArea GenerateViewOfViewEntityArea();
        I_View_AdvancedSearchEntityArea GenerateViewOfAdvancedSearch();
        I_View_DataLinkArea GenerateViewOfDataLinkArea();
        I_DataViewItem GetDataViewItem();
        I_DataTree GetDataTreeForm();

        void ShowDataViewItemMenus(List<DataMenuUI> menus, object sourceObject);

        //UIControlPackageMultipleData GenerateMultipleDataDependentControl(ColumnDTO column, ColumnUISettingDTO ColumnSetting);
        //void ShowDataViewItemMenus(I_View_DataViewArea dataViewArea, I_DataViewItem item, List<DataViewMenu> menus);
        I_View_MultipleArchiveItemsInfo GenerateViewOfMultipleArchiveItemInfo();
        void PrepareMainForm();

        //I_View_DataDependentControl GenerateMultipleDataDependentViewControl(ColumnUISettingDTO columnSetting, TemporaryLinkType linkType);
        //I_ControlManager GenerateControlManager(ColumnDTO column, ColumnUISettingDTO columnSetting);
        //  I_SimpleControlManager GenerateSearchControlManager(ColumnDTO column, List<SimpleSearchOperator> operators, ColumnUISettingDTO columnSetting);
        //UIControlPackagemu GenerateControlPackage(object view, ColumnUISettingDTO columnSetting);
        //object GenerateRelationshipControlManager(object view, RelationshipUISettingDTO columnSetting);
        //DataDependentControlPackage GenerateDataDependentControlPackage(object view, ColumnUISettingDTO columnSetting);
        I_View_AddArchiveItems GenerateViewOfAddArchiveItem();
        I_FormulaDataTree GetViewOdFormulaTree();

        //UIControl GenerateLabelControlPackage(string text, string tooltip, InfoColor color);
        void ShowMainForm();
        I_Login GetLoginForm();

        //void GenerateSearchViewPackageArea(SearchViewPackageAreaInitializer initializer);
        void ShowPane(object view, string title);
        I_View_WorkflowRequestCreator GetWorkflowReauestCreationForm(List<ProcessDTO> processList);
        I_View_ReportList GetViewOfReportList();
        I_MainFormMenu AddMainFormMenu(string title, string image);


        I_View_WorkflowTransitionTargetSelection GetViewOfWorkflowTransitionTargetSelection();
        I_View_Cartable GetCartable();

        //void CloseEditPackageArea(I_View_EditPackageArea view);
        bool DownloadFile(FileRepositoryDTO attechedFile, bool fileNameWithTimeStamp);


        //void ShowSearchViewPackageArea(I_View_SearchViewPackageArea view);
        //void CloseSearchViewPackageArea(I_View_SearchViewPackageArea view);

        I_View_TemporaryView GenerateTemporaryLinkUI(TemporaryLinkState temporaryLinkState);
        I_View_GridViewArea GetViewOfGridViewArea();

        //I_View_SearchViewArea GenerateSearchViewArea();
        //////UIContainerPackage GenerateGroup(I_View_GridContainer parentGridContainer, string groupTitle);
        I_View_EditEntityAreaInfo GenerateViewOfEditEntityAreaInfo();
        I_SecuritySetting GetSecuritySettingForm();

        //IAG_View_TemporaryView GenerateTemporarySearchViewLinkUI();

        //IAG_View_TemporaryView GenerateTemporaryDataSearchViewLinkUI();

        void CloseDialog(object view);
        Tuple<List<Enum_ArchiveItemFileType>, I_View_ViewArchiveItem> GetArhiveItemViewer(Enum_ArchiveItemMainType mainType, Enum_ArchiveItemFileType fileType);
        bool PaneIsOpen(string v);
        void ShowMessage(string title, string message, List<ResultDetail> details = null);
        void ShowMessage(string message);
        void ActivatePane(string v);
        void CloseApplication(bool ask);

        //////void ShowValidationMessage(UIControlPackage uIControlPackage, string p);
        //////void ClearValidationMessage(UIControlPackage uIControlPackage);
        //string GetGroupControlKey(string mainName, string itemName);

        Temp.ConfirmResul ShowConfirm(string title, string message, UserPromptMode dialogMode);
        void SetUserInfo(string userName, string roles);
        void ShowInfo(string title, string detail = "", Temp.InfoColor infoColor = Temp.InfoColor.Black);
        void ShowInfo(string title, List<ResultDetail> details, InfoColor infoColor);

        //bool ShowControlValue(UIControlPackage controlPackage, ColumnDTO control, string value, ColumnSetting columnSetting);
        //string FetchControlValue(UIControlPackage controlPackage, ColumnDTO control);
        //SimpleSearchOperator FetchOperatorValue(UIControlPackage controlPackage, ColumnDTO control);
        //void GenerateMenuForControlPackageOneData(UIControlPackageOneDataSimpleColumn controlPackage, ConrolPackageMenu cpMenu);
        //void GenerateMenuForControlPackageMultipleData(UIControlPackageMultipleData controlPackage, ConrolPackageMenu cpMenu);

        void ShowValidationMessage(I_EditEntityArea editEntityArea, string message);
        //  void EnableDisableColumnControlOneData(UIControlPackageForSimpleColumn controlPackage, ColumnDTO control, bool enable);
        //void EnableDisableColumnControlMultipleData(UIControlPackageMultipleData controlPackage, ColumnDTO control, bool enable);
        //void SetReadonlyOneData(UIControlPackageOneDataSimpleColumn controlPackage, ColumnDTO column, bool readonlity);
        //void SetReadonlyMultipleData(UIControlPackageMultipleData controlPackage, ColumnDTO column, bool readonlity);
        //void SetMultipleDataPackageReadonly(UIControlPackageMultipleData ControlPackage, bool isPermanentReadOnly);

        //UISearchNullControlPackage GenerateSearchNullControlPackage(string title, ColumnSetting columnSetting);

        //void DisableEnableMultiple(I_View_MultipleDataContainer container, DP_DataRepository dataItem, bool enable);
        //void DisableEnableMultipleColumn(DP_DataRepository dataItem, BaseSimpleColumn column, bool enable);
        //void SetReadonlyMultiple(I_View_MultipleDataContainer container, DP_DataRepository dataItem, bool readonlity);
        //void SetReadonlyMultipleColumn(DP_DataRepository dataItem, BaseSimpleColumn column, bool readonlity);
        I_View_RequestAction GetRequestActionForm();
        MyMenuItem GetMenuItem(object parentMenu, string header, string name, string tooltip = null);
        void AddMenuSeprator(object contextMenu);
        List<MyMenuItem> GetCurrentMenuItems(object contextMenu);
        void RemoveMenuItem(object contextMenu, object menuItem);
        I_View_RequestNote GetRequestNoteForm();
        I_View_RequestFile GetRequestFileForm();

        I_View_RequestDiagram GetRequestDiagramForm();
        I_CommandManager GetCommandManager();
        //I_FormulaOptions GetFormulaOptionForm();
        //I_FormulaUsageParameters GetFormulaUsageParametersForm();
        I_DataTreeView GetViewOfDataTree();
        //I_LabelControlManager GenerateLabelControlManager();
        I_View_LogReportArea GenerateViewOfLogReportArea();
        I_View_WorkflowReport GetWorkflowReportForm();
        void ClearNavigationTree();
        void ClearSearchNavigationTree();
        bool ControlIsVisible(object control);
        void SetContaierVisiblity(object control,bool visible);
        //void ShowDatabaseList(DP_ResultDatabaseList result);
    }
    public interface I_MainFormMenu
    {
        event EventHandler Clicked;
    }
    public class MyMenuItem
    {
        public int ItemID { set; get; }
        public object MenuItem { set; get; }
        public bool IsDeletable { set; get; }
        public string Name { get; set; }

        public event EventHandler MenuItemClicked;

        public void OnClick()
        {
            if (MenuItemClicked != null)
                MenuItemClicked(this, null);
        }
    }
    public enum Enum_WindowSize
    {
        None,
        Big,
        Maximized
    }
  
}
