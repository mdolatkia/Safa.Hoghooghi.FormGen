


using MyUILibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MyUIGenerator.View;
using System.Linq;
using MyUILibrary.EntityArea;
using Telerik.Windows.Controls;
using System.Windows.Media;
using ProxyLibrary;
using System.Windows.Controls;
using MyUIGenerator.UIControlHelper;

using ModelEntites;
using CommonDefinitions.UISettings;
using MyUILibrary.Temp;
using MyUIGenerator.Login;
using MyUIGenerator.Security;

using Telerik.Reporting;
using ProxyLibrary.Workflow;
using Microsoft.Win32;
using System.IO;
using MyUILibraryInterfaces.DataViewArea;
using MyUILibraryInterfaces.DataReportArea;
using MyUILibraryInterfaces.DataLinkArea;
using MyUILibraryInterfaces.DataTreeArea;
using MyUIGenerator.UI.DataTree;
using MyUILibraryInterfaces.EntityArea;
using MyUILibraryInterfaces.DataMenuArea;
using System.Collections.ObjectModel;
using MyUILibraryInterfaces.GridViewArea;
using MyUILibrary.WorkflowArea;
using MyUILibraryInterfaces.LogReportArea;
using MyUILibraryInterfaces.FormulaCalculationArea;
using MyUIGenerator.UI.Workflow;
using MyUILibraryInterfaces.ContextMenu;

//using MyUIGenerator.UIContainerHelper;


namespace MyUIGenerator
{
    public static class UIManagerGenerator
    {
        static UIManager _UIManager = null;
        public static UIManager GetUIManager()
        {
            if (_UIManager == null)
                _UIManager = new MyUIGenerator.UIManager();
            return _UIManager;
        }
    }
    public class UIManager : IAgentUIManager
    {

        //public event EventHandler<Arg_NavigationTreeRequest> NavigationTreeRequested;



        //public event EventHandler DatabaseListRequested;
        //public event EventHandler WorkflowRequestCreationForm;
        //public event EventHandler SecuritySettingRequested;



        //public event EventHandler<Arg_NavigationItemRequest> NavigationItemSelected;
        //  public event EventHandler<WorkflowRequestCreationArg> WorkflowRequestCreate;


        WPF_MyIdea.frmMain MainForm { set; get; }
        public void StartApp()
        {
            AgentUICoreMediator.GetAgentUICoreMediator.SetUIManager(this);
            AgentUICoreMediator.GetAgentUICoreMediator.StartApp();
        }
        public void PrepareMainForm()
        {
            var lastMainForm = MainForm;
            MainForm = new WPF_MyIdea.frmMain(this);
            if (lastMainForm != null)
                lastMainForm.Close();
        }
        public void ShowMainForm()
        {

            MainForm.Show();
        }
        public I_SecuritySetting GetSecuritySettingForm()
        {
            frmSecuritySetting frm = new frmSecuritySetting();
            return frm;
        }
        public I_Login GetLoginForm()
        {
            frmLogin frm = new Login.frmLogin();
            return frm;
        }
        //public UIManager()
        //{
        //    AgentUIMediator = AgentUICoreMediator.GetAgentUICoreMediator;
        //}
        //public UIManager(WPF_MyIdea.frmMain form)
        //{
        //    MainForm = form;
        //}



        //public void OnDatabaseListRequested()
        //{
        //    if (DatabaseListRequested != null)
        //        DatabaseListRequested(this, null);
        //}
        //public void OnNavigationTreeRequested()
        //{
        //    if (NavigationTreeRequested != null)
        //        NavigationTreeRequested(this, new Arg_NavigationTreeRequest());
        //}





        //public void OnNavigationItemSelected(NavigationItemDTO navigationItem)
        //{
        //    if (NavigationItemSelected != null)
        //    {
        //        Arg_NavigationItemRequest arg = new Arg_NavigationItemRequest();
        //        arg.NavigationItem = navigationItem;
        //        NavigationItemSelected(this, arg);
        //    }
        //}

        public I_View_WorkflowRequestCreator GetWorkflowReauestCreationForm(List<ProcessDTO> processList)
        {
            frmWorkflowRequestCreation view = new frmWorkflowRequestCreation(processList);
            return view;
        }

        public I_View_RequestAction GetRequestActionForm()
        {
            frmRequestAction view = new frmRequestAction();

            return view;
        }






        public void ClearNavigationTree()
        {
            MainForm.ClearNavigationTree();
        }
        public void ClearSearchNavigationTree()
        {
            MainForm.ClearSearchNavigationTree();
        }
        public I_NavigationMenu AddNavigationTree(I_NavigationMenu parentItem, NavigationItemDTO item, bool expanded)
        {
            return MainForm.AddNavigationTree(parentItem, item, expanded);
        }
        public I_NavigationMenu AddSearchNavigationTree(I_NavigationMenu parentItem, NavigationItemDTO item, bool expanded)
        {
            return MainForm.AddSearchNavigationTree(parentItem, item, expanded);
        }
        public bool SearchNavigationTreeVisiblity
        {
            get
            {
                return MainForm.SearchNavigationTreeVisiblity;
            }
            set
            {
                MainForm.SearchNavigationTreeVisiblity = value;
            }

        }
        public bool NavigationTreeVisiblity
        {
            get
            {
                return MainForm.NavigationTreeVisiblity;
            }
            set
            {
                MainForm.NavigationTreeVisiblity = value;
            }
        }

        //public void GenerateViewOfEditPackageArea(EditPackageAreaInitializer initializer)
        //{


        //    //////initializer.Mode = Enum_AG_EntityRequestGranularity.OneByOne;
        //    //////initializer.DataPackageTemplate = packages[0];
        //    initializer.View = new frmEditPackageArea();

        //    //container.CommandRequested += container_CommandRequested;

        //    //return container;
        //}

        //public I_View_SearchViewArea GenerateSearchViewArea()
        //{

        //    // SearchViewPackageAreaInitializer initParam = new SearchViewPackageAreaInitializer();

        //    return new UC_SearchViewArea();

        //    //container.CommandRequested += container_CommandRequested;

        //    //return container;
        //}
        //void container_CommandRequested(object sender, Arg_CommandRequest e)
        //{
        //    var request = new AG_CommandExecutionRequest();
        //    request.Command = new AG_PackageAreaCommand();
        //    //request.SourcePackageArea = sender;
        //    request.Command.Packages = e.Packages;
        //    request.Command.CommandGoal = Enum_AG_PackageAreaCommand.Add;
        //    AgentUIMediator.ExecuteCommand(request);
        //}



        public I_SimpleControlManager GenerateSimpleControlManagerForOneDataForm(ColumnDTO column, ColumnUISettingDTO columnSetting, bool hasRangeOfValues, List<SimpleSearchOperator> operators, bool labelControlManager, string labelText)
        {
            var controlManager = new SimpleControlManagerForOneDataForm(column, columnSetting, hasRangeOfValues, operators);
            if (labelControlManager)
            {
                controlManager.LabelControlManager = new LabelControlManager(labelText, true);
            }
            return controlManager;
        }

        public I_RelationshipControlManager GenerateRelationshipControlManagerForOneDataForm(object view, RelationshipUISettingDTO relationshipUISetting, bool labelControlManager, string labelText)
        {
            var controlManager = new RelationshipControlManagerForOneDataForm(view as FrameworkElement, relationshipUISetting);
            if (labelControlManager)
            {
                controlManager.LabelControlManager = new LabelControlManager(labelText, true);
            }
            return controlManager;
        }

        public I_SimpleControlManager GenerateSimpleControlManagerForMultipleDataForm(ColumnDTO column, ColumnUISettingDTO columnUISettingDTO, bool hasRangeOfValues, bool labelControlManager, string labelText)
        {
            var controlManager = new SimpleControlManagerForMultipleDataForm(column, columnUISettingDTO, hasRangeOfValues);
            if (labelControlManager)
            {
                controlManager.LabelControlManager = new LabelControlManager(labelText, false);
            }
            return controlManager;
        }

        internal void ShowDetail(string title, List<ResultDetail> details)
        {
            UC_InfoDetails view = new MyUIGenerator.UC_InfoDetails(details);
            var window = GetDialogWindow();
            window.ShowDialog(view, title);
        }

        public I_RelationshipControlManager GenerateRelationshipControlManagerForMultipleDataForm(TemporaryLinkState temporaryLinkState, RelationshipUISettingDTO relationshipUISetting, bool labelControlManager, string labelText)
        {
            var controlManager = new RelationshipControlManagerForMultipleDataForm(temporaryLinkState, relationshipUISetting);
            if (labelControlManager)
            {
                controlManager.LabelControlManager = new LabelControlManager(labelText, false);
            }
            return controlManager;
        }
        //public I_LabelControlManager GenerateLabelControlManager()
        //{
        //    return new LabelControlManager("");
        //}
        public I_UICompositionContainer GenerateGroup(GroupUISettingDTO groupUISettingDTO)
        {
            return new LocalContainerManager(groupUISettingDTO, groupUISettingDTO.InternalColumnsCount);
        }

        public I_TabGroupContainer GenerateTabGroup(TabGroupUISettingDTO groupUISettingDTO)
        {
            return new TabGroupContainerManager(groupUISettingDTO);
        }

        public I_TabPageContainer GenerateTabPage(TabPageUISettingDTO groupUISettingDTO)
        {
            return new TabPageContainerManager(groupUISettingDTO, groupUISettingDTO.InternalColumnsCount);
        }
        public event EventHandler<string> SearchTextChanged;

        internal void OnSeatchTextChanged(string text)
        {
            if (SearchTextChanged != null)
                SearchTextChanged(this, text);
        }

        //public UIControlPackageForSimpleColumn GenerateControlPackage(object view, ColumnUISettingDTO columnSetting)
        //{
        //    if (view is UserControl)
        //    {
        //        Random rnd = new Random();
        //        //if (rnd.Next(8) == 1)
        //        (view as UserControl).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF0F0F0"));
        //        //else if (rnd.Next(8) == 2)
        //        //    (view as UserControl).Background = Brushes.LightGreen);
        //        //else if (rnd.Next(8) == 3)
        //        //    (view as UserControl).Background = Brushes.LightPink);
        //        //else if (rnd.Next(8) == 4)
        //        //    (view as UserControl).Background = Brushes.LightSteelBlue);
        //        //else if (rnd.Next(8) == 5)
        //        //    (view as UserControl).Background = Brushes.LightCoral);
        //        //else if (rnd.Next(8) == 6)
        //        //    (view as UserControl).Background = Brushes.LightBlue);
        //        //else if (rnd.Next(8) == 7)
        //        //    (view as UserControl).Background = Brushes.LightSalmon);
        //        //else if (rnd.Next(8) == 8)
        //        //    (view as UserControl).Background = Brushes.LightSeaGreen);
        //        //else
        //        //    (view as UserControl).Background = Brushes.LightYellow);
        //        //     (view as UserControl).Margin = new Thickness(7);
        //        if (view.GetType() != typeof(UC_TemporaryDataSearchLink))
        //        {
        //            (view as UserControl).BorderThickness = new Thickness(1);
        //            (view as UserControl).BorderBrush = Brushes.Black;
        //        }
        //    }
        //    //  return ControlHelper.GenerateControlPackage(view, columnSetting);
        //    return null;
        //}


        //public I_View_DataDependentControl GenerateMultipleDataDependentViewControl(ColumnUISettingDTO columnSetting, TemporaryLinkType linkType)
        //{
        //    return DataGridHelper.GenerateMultipleDataDependentViewControl(linkType);
        //}
        //public DataDependentControlPackage GenerateDataDependentControlPackage(object view, ColumnUISettingDTO columnSetting)
        //{
        //    DataDependentControlPackage package = new DataDependentControlPackage();
        //    //package.DataDependentControl = view;
        //   // package.UIControlSetting = columnSetting.UISetting;
        //    package.UIControl = new UIControl() { Control = view };
        //    return package;
        //}
        //public bool DataViewIsOpen(object dataView)
        //{
        //    foreach (var item in MainForm.pnlForms.Items)
        //        if (item is RadPane)
        //        {
        //            if ((item as RadPane).Content == dataView)
        //                return true;
        //        }
        //    return RadWindowManager.Current.GetWindows().Any(x => x.Content == dataView);
        //}
        public void ShowPane(object view, string title)
        {
            if (view is Control)
                MainForm.ShowPane((view as Control), title);
            //ShowDialog(view, title);

            //frmTest frm = new MyUIGenerator.frmTest(new CommonDefinitions.BasicUISettings.GridSetting());
            //ShowDialog(frm, "ASdasd");
        }

        public bool PaneIsOpen(string title)
        {
            return MainForm.GetPaneTitles().Any(x => x == title);
        }

        public void ActivatePane(string title)
        {
            MainForm.ActivatePane(title);
        }

        //public void ShowSearchViewPackageArea(I_View_SearchViewPackageArea view)
        //{
        //    if (view is Window)
        //        (view as Window).ShowDialog();
        //}



        //public void CloseEditPackageArea(I_View_EditPackageArea view)
        //{
        //    if (view is Window)
        //        (view as Window).Close();
        //}

        //public void CloseSearchViewPackageArea(I_View_SearchViewPackageArea view)
        //{
        //    if (view is Window)
        //        (view as Window).Close();
        //}


        //public I_View_EditPackageArea GenerateEditPackageAreaView()
        //{
        //    return new frmEditPackageArea();
        //}




        public I_View_EditEntityAreaMultiple GenerateEditEntityAreaMultipleDataView()
        {
            I_View_EditEntityAreaMultiple view = new UC_EditEntityAreaMultiple();
            (view as UserControl).Margin = new Thickness(0, 0, 0, 5);
            //  view.grdArea.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightPink);
            return view;
        }
        public I_View_EditEntityAreaDataView GenerateEditEntityAreaOneDataView(EntityUISettingDTO entityUISettingDTO)
        {
            //GridSetting gridSetting = new UIControlHelper.GridSetting();
            short columnsCount = 0;
            if (entityUISettingDTO == null || entityUISettingDTO.UIColumnsCount == 0)
                columnsCount = 4;
            else
                columnsCount = entityUISettingDTO.UIColumnsCount;
            var view = new UC_EditEntityArea(columnsCount);
            (view as UserControl).Margin = new Thickness(0, 0, 0, 5);
            //  view.grdArea.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightPink);
            return view;
        }


        public I_View_AdvancedSearchEntityArea GenerateViewOfAdvancedSearch()
        {
            var view = new UC_AdvancedSearchEntityArea();
            return view;
        }
        public I_View_SearchViewEntityArea GenerateViewOfSearchViewEntityArea()
        {
            var view = new UC_SearchViewEntityArea();
            //  view.grdArea.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightPink);
            return view;
        }
        public I_View_SimpleSearchEntityArea GenerateViewOfSearchEntityArea(EntityUISettingDTO entityUISettingDTO)
        {
            short columnsCount = 0;
            if (entityUISettingDTO == null || entityUISettingDTO.UIColumnsCount == 0)
                columnsCount = 4;
            else
                columnsCount = entityUISettingDTO.UIColumnsCount;
            var view = new UC_SimpleSearchEntityArea(columnsCount);
            //  view.grdArea.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightPink);
            return view;
        }
        public I_View_SearchEntityArea GenerateViewOfSearchEntityArea()
        {

            var view = new UC_SearchEntityArea();
            return view;

        }

        public I_View_ViewEntityArea GenerateViewOfViewEntityArea()
        {
            var view = new UC_ViewEntityArea();
            //  view.grdArea.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightPink);
            return view;
        }

        //public I_View_ViewReportArea GenerateViewOfViewReportArea()
        //{
        //    var view = new UC_ViewReportArea();
        //    return view;
        //}

        //public I_View_SearchReportEntityArea GenerateViewOfSearchReportEntityArea()
        //{
        //    var view = new UC_SearchReportEntityArea();
        //    return view;
        //}

        public I_View_WorkflowTransitionTargetSelection GetViewOfWorkflowTransitionTargetSelection()
        {
            return new UC_WorkflowTransitionTargetSelection();
        }
        public I_View_TemporaryView GenerateTemporaryLinkUI(TemporaryLinkState temporaryLinkState)
        {
            var view = new UC_TemporaryDataSearchLink(temporaryLinkState);
            view.VerticalAlignment = VerticalAlignment.Center;
            return view;
        }


        public I_View_EditEntityAreaInfo GenerateViewOfEditEntityAreaInfo()
        {
            var view = new UC_EditEntityAreaInfo();
            return view;
        }
        //public IAG_View_TemporaryView GenerateTemporarySearchViewLinkUI()
        //{
        //    var view = new UC_TemporarySearchViewLink();
        //    //  view.grdArea.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightPink);
        //    return view;
        //}
        //public IAG_View_TemporaryView GenerateTemporaryDataSearchViewLinkUI()
        //{
        //    var view = new UC_TemporaryDataSearchLink();
        //    //  view.grdArea.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightPink);
        //    return view;
        //}



        public I_DialogWindow GetDialogWindow()
        {
            //maximized = true;
            DialogWindow dialogWindow = new MyUIGenerator.DialogWindow();
            return dialogWindow;
        }

        public void CloseDialog(object view)
        {
            if (view is Window)
            {
                (view as Window).Close();
            }
            else if (view is UIElement)
            {
                var window = (view as UIElement).ParentOfType<RadWindow>();
                if (window != null)
                    window.Close();
            }
        }
        //public void HideDialog(object view)
        //{
        //    if (view is Window)
        //    {
        //        (view as Window).Hide();
        //    }
        //    else if (view is UIElement)
        //    {
        //        var window = (view as UIElement).ParentOfType<RadWindow>();
        //        if (window != null)
        //            window.Visibility = Visibility.Visible;
        //    }
        //}

        //////public void ShowValidationMessage(UIControlPackage uIControlPackage, string message)
        //////{
        //////    if (uIControlPackage.UIControl != null)
        //////    {
        //////        if (uIControlPackage.RelatedUIControls.Count > 0)
        //////        {
        //////            var label = uIControlPackage.RelatedUIControls.FirstOrDefault(x => x.RelationType == AG_ControlRelationType.Label);
        //////            if (label != null)
        //////            {
        //////                LabelHelper.Highlight(label.RelatedUIControl, message);
        //////            }
        //////        }
        //////    }

        //////}
        public void ShowValidationMessage(I_EditEntityArea editEntityArea, string message)
        {
            MessageBox.Show(message);
        }
        //////public void ClearValidationMessage(UIControlPackage uIControlPackage)
        //////{
        //////    if (uIControlPackage.RelatedUIControls != null)
        //////    {
        //////        if (uIControlPackage.RelatedUIControls.Count > 0)
        //////        {
        //////            var label = uIControlPackage.RelatedUIControls.FirstOrDefault(x => x.RelationType == AG_ControlRelationType.Label);
        //////            if (label != null)
        //////            {
        //////                LabelHelper.DeHighlight(label.RelatedUIControl);
        //////            }
        //////        }
        //////    }
        //////}

        //public string GetGroupControlKey(string mainName, string itemName)
        //{
        //    return TabHelper.GetTabKey(mainName, itemName);
        //}
        public void ShowDataViewItemMenus(List<DataMenuUI> menus, object sourceObject)
        {
            if (menus.Any())
            {
                RadRadialMenu menu = new RadRadialMenu();
                foreach (var item in menus)
                {
                    AddMenu(menu.Items, item);
                }
                menu.HideEventName = "LostFocus";
                menu.PopupPlacement = System.Windows.Controls.Primitives.PlacementMode.Center;
                RadRadialMenu.SetRadialContextMenu(sourceObject as UIElement, menu);
                RadialMenuCommands.Show.Execute(null, sourceObject as UIElement);
                menu.PopupPlacement = System.Windows.Controls.Primitives.PlacementMode.Center;
                menu.IsOpen = true;
                menu.PopupPlacement = System.Windows.Controls.Primitives.PlacementMode.Center;
            }
        }

        private void AddMenu(ObservableCollection<RadRadialMenuItem> items, DataMenuUI item)
        {
            RadRadialMenuItem menuItem = new RadRadialMenuItem();
            menuItem.Header = item.Title;
            if (!string.IsNullOrEmpty(item.Tooltip))
                ToolTipService.SetToolTip(menuItem, item.Tooltip);
            menuItem.Click += (sender, e) => SubMenuItem_Click(sender, e, item);
            foreach (var subItem in item.SubMenus)
            {
                AddMenu(menuItem.ChildItems, subItem);
            }
            items.Add(menuItem);
        }

        private void SubMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e, DataMenuUI dataViewMenu)
        {
            dataViewMenu.OnMenuClicked();
        }
        public void ShowInfo(string title, string detail, MyUILibrary.Temp.InfoColor infoColor = MyUILibrary.Temp.InfoColor.Black)
        {
            MainForm.ShowInfo(title, detail, UIManager.GetColorFromInfoColor(infoColor));
        }
        public void ShowInfo(string title, List<ResultDetail> details, InfoColor infoColor)
        {
            MainForm.ShowInfo(title, details, UIManager.GetColorFromInfoColor(infoColor));
        }
        public static SolidColorBrush GetColorFromInfoColor(MyUILibrary.Temp.InfoColor infoColor)
        {
            if (infoColor == MyUILibrary.Temp.InfoColor.Null)
            {
                return null;
            }
            else
            {
                Color color = Colors.Black;
                if (infoColor == MyUILibrary.Temp.InfoColor.Blue)
                    color = Colors.Blue;
                else if (infoColor == MyUILibrary.Temp.InfoColor.Green)
                    color = Colors.Green;
                else if (infoColor == MyUILibrary.Temp.InfoColor.Red)
                    color = Colors.Red;
                else if (infoColor == MyUILibrary.Temp.InfoColor.LightGray)
                    color = Colors.LightGray;
                else if (infoColor == MyUILibrary.Temp.InfoColor.DarkRed)
                    color = Colors.DarkRed;
                return new SolidColorBrush(color);
            }
        }





        public bool ViewIsVisible(object view)
        {
            return (view as UIElement).Visibility == Visibility.Visible;
        }


        //public void GenerateMenuForControlPackageOneData(UIControlPackageForSimpleColumn controlPackage, ConrolPackageMenu cpMenu)
        //{
        //    object result = null;
        //    //if (controlPackage is DataDependentControlPackage)
        //    //{
        //    //    DataGridHelper.GenerateMenu(controlPackage, cpMenu);
        //    //}
        //    //else
        //    {
        //        ControlHelper.GenerateMenu(controlPackage, cpMenu);
        //    }
        //    if (cpMenu != null)
        //        controlPackage.MenuItems.Add(cpMenu);


        //}
        //public void GenerateMenuForControlPackageMultipleData(UIControlPackageMultipleData controlPackage, ConrolPackageMenu cpMenu)
        //{
        //    object result = null;
        //    //if (controlPackage is DataDependentControlPackage)
        //    //{
        //    DataGridHelper.GenerateMenu(controlPackage, cpMenu);
        //    //}
        //    //else
        //    //{
        //    //    ControlHelper.GenerateMenu(controlPackage, cpMenu);
        //    //}
        //    if (cpMenu != null)
        //        controlPackage.MenuItems.Add(cpMenu);


        //}

        //public UIControlPackageMultipleData GenerateMultipleDataDependentControl(ColumnDTO column, ColumnUISettingDTO columnSetting)
        //{
        //    //////var controlPackage = DataGridHelper.GenerateMultipleDataDependentControl(column, columnSetting);
        //    //////return controlPackage;
        //    return null;
        //}

        //void DataDependentControl_DataControlGenerated(object sender, Arg_DataDependentControlGeneration e)
        //{
        //    if (DataControlGenerated != null)
        //        DataControlGenerated(sender, e);
        //}

        ControlHelper _ControlHelper;
        public ControlHelper ControlHelper
        {
            get
            {
                if (_ControlHelper == null)
                    _ControlHelper = new ControlHelper();
                return _ControlHelper;
            }
        }
        //KeyValueControlHelper _KeyValueControlHelper;
        //public KeyValueControlHelper KeyValueControlHelper
        //{
        //    get
        //    {
        //        if (_KeyValueControlHelper == null)
        //            _KeyValueControlHelper = new KeyValueControlHelper();
        //        return _KeyValueControlHelper;
        //    }
        //}
        //public I_SimpleControlManager GenerateSearchControlManager(ColumnDTO column, List<SimpleSearchOperator> operators, ColumnUISettingDTO columnSetting)
        //{
        //    return null;
        //    //////if (column.ColumnKeyValue == null)
        //    //////    return ControlHelper.GenerateControl(column, columnSetting, operators);
        //    //////else
        //    //////    return ControlHelper.GenerateKeyValueControl(column.ColumnKeyValue.ColumnKeyValueRange, column.ColumnKeyValue.ValueFromTitleOrValue, column, columnSetting, operators);
        //}


        //public UIControlPackage GenerateGroup(UIControlSetting uiControlSetting, string groupTitle)
        //{
        //    return GroupPanel.GenerateGroup(groupTitle, uiControlSetting);
        //}

        //////public UIContainerPackage GenerateGroup(I_View_GridContainer parent, string groupTitle)
        //////{

        //////        ////MakeGridSettingFromParentAndCurrent(parent, uiControlSetting);
        //////    View_GridContainer panel = new View_GridContainer(parent.ColumnsCount);
        //////    var uiPackage = ContainerHelper.GenerateUIContainerPackage(groupTitle, panel);
        //////    return uiPackage;
        //////}
        //internal GridSetting MakeGridSettingFromParentAndCurrent(I_View_GridContainer parent)
        //{
        //    GridSetting setting = new GridSetting();
        //    //if (uiControlSetting.DesieredColumns == ColumnWidth.Full)
        //    //    setting.ColumnsCount = parentGridSetting.ColumnsCount;
        //    //else if (uiControlSetting.DesieredColumns == ColumnWidth.Normal)
        //    //    setting.ColumnsCount = parentGridSetting.ColumnsCount / 4;
        //    //else if (uiControlSetting.DesieredColumns == ColumnWidth.Half)
        //    //    setting.ColumnsCount = parentGridSetting.ColumnsCount / 2;
        //    //if (setting.ColumnsCount == 0)
        //    //    setting.ColumnsCount = 1;
        //    //setting.MinimumColumnWidth = parentGridSetting.MinimumColumnWidth;
        //    //setting.MinimumRowHeight = parentGridSetting.MinimumRowHeight;
        //    return setting;
        //}

        //ControlHelper _ControlHelper;
        //public ControlHelper ControlHelper
        //{
        //    get
        //    {
        //        if (_ControlHelper == null)
        //            _ControlHelper = new ControlHelper();
        //        return _ControlHelper;
        //    }
        //}

        //public bool ShowControlValue(UIControlPackage controlPackage, ColumnDTO control, string value, ColumnUISettingDTO columnSetting)
        //{
        //    return ControlHelper.SetValue(control, controlPackage, value, columnSetting);
        //}

        //public string FetchControlValue(UIControlPackage controlPackage, ColumnDTO control)
        //{
        //    return ControlHelper.GetValue(control, controlPackage);
        //}
        //public SimpleSearchOperator FetchOperatorValue(UIControlPackage controlPackage, ColumnDTO control)
        //{
        //    return ControlHelper.GetOperator(control, controlPackage);
        //}
        public I_View_Cartable GetCartable()
        {
            UC_Cartable cartable = new UC_Cartable();
            //MainForm.ShowPane(cartable, "کارتابل");
            return cartable;
        }



        //public UIControl GenerateLabelControlPackage(string text, string tooltip, InfoColor color)
        //{

        //    return LabelHelper.GenerateLabelControl(text, tooltip, color);
        //}

        //public void EnableDisableColumnControlOneData(UIControlPackageForSimpleColumn controlPackage, ColumnDTO control, bool enable)
        //{
        //    //////controlPackage.BaseControlHelper.EnableDisable(controlPackage, enable);
        //}
        //public void EnableDisableColumnControlMultipleData(UIControlPackageMultipleData controlPackage, ColumnDTO control, bool enable)
        //{
        //    //////controlPackage.BaseControlHelper.EnableDisable(controlPackage, enable);
        //}

        public void SetUserInfo(string userName, string roles)
        {
            MainForm.SetUserInfo(userName, "", roles);
        }

        //public void SetReadonly(UIControlPackageForSimpleColumn controlPackage, ColumnDTO column, bool readonlity)
        //{
        //    //////controlPackage.BaseControlHelper.SetReadonly(controlPackage, readonlity);
        //}

        //public void SetMultipleDataPackageReadonly(UIControlPackageMultipleData controlPackage, bool isReadOnly)
        //{
        //    DataGridHelper.SetReadonly(controlPackage, isReadOnly);
        //}


        //public void DisableEnableMultiple(I_View_MultipleDataContainer container, DP_DataRepository dataItem, bool enable)
        //{
        //    DataGridHelper.DisableEnableRow((container as View_MultipleDataContainer).dataGridHelper, dataItem, enable);
        //}

        //public void DisableEnableMultipleColumn(DP_DataRepository dataItem, SimpleColumnControlMultipleData column, bool enable)
        //{
        //    DataGridHelper.DisableEnableCell(dataItem, column.ControlPackage, column.Column, enable);
        //}

        //public void SetReadonlyMultiple(I_View_MultipleDataContainer container, DP_DataRepository dataItem, bool readonlity)
        //{

        //    DataGridHelper.SetReadonlyRow((container as View_MultipleDataContainer).dataGridHelper, dataItem, readonlity);
        //}

        //public void SetReadonlyMultipleColumn(DP_DataRepository dataItem, SimpleColumnControlMultipleData column, bool readonlity)
        //{
        //    DataGridHelper.SetReadonlyCell(dataItem, column.ControlPackage, column.Column, readonlity);
        //}

        //public void ShowTestReport(string reportEngine, object reportSource)
        //{
        //    frmReportViewer aa = new frmReportViewer(reportEngine, reportSource as ReportSource);
        //    aa.ShowDialog();
        //}

        public MyMenuItem GetMenuItem(object parentMenu, string header, string name, string tooltip)
        {
            MyMenuItem myMenuItem = new MyMenuItem();
            myMenuItem.Name = name;
            RadMenuItem menuITem = new RadMenuItem();
            menuITem.DataContext = myMenuItem;
            menuITem.Header = header;
            if (!string.IsNullOrEmpty(tooltip))
                ToolTipService.SetToolTip(menuITem, tooltip);
            ItemCollection itemCollection = null;
            if (parentMenu is RadContextMenu)
                itemCollection = (parentMenu as RadContextMenu).Items;
            else if (parentMenu is RadMenuItem)
                itemCollection = (parentMenu as RadMenuItem).Items;
            itemCollection.Add(menuITem);
            menuITem.Click += (sender, e) => MenuITem_Click(sender, e, myMenuItem);
            myMenuItem.MenuItem = menuITem;

            return myMenuItem;
        }

        private void MenuITem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e, MyMenuItem myMenuItem)
        {
            myMenuItem.OnClick();
        }

        public void AddMenuSeprator(object parentMenu)
        {

            ItemCollection itemCollection = null;
            if (parentMenu is RadContextMenu)
                itemCollection = (parentMenu as RadContextMenu).Items;
            else if (parentMenu is RadMenuItem)
                itemCollection = (parentMenu as RadMenuItem).Items;
            RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
            itemCollection.Add(separator);

        }
        public List<MyMenuItem> GetCurrentMenuItems(object parentMenu)
        {
            List<MyMenuItem> result = new List<MyMenuItem>();
            ItemCollection itemCollection = null;
            if (parentMenu is RadContextMenu)
                itemCollection = (parentMenu as RadContextMenu).Items;
            else if (parentMenu is RadMenuItem)
                itemCollection = (parentMenu as RadMenuItem).Items;
            foreach (var item in itemCollection)
            {
                if (item is RadMenuItem)
                {
                    var menuItem = (item as RadMenuItem);
                    var myMenuItem = menuItem.DataContext as MyMenuItem;
                    result.Add(myMenuItem);
                }
            }
            return result;
        }
        public void RemoveMenuItem(object parentMenu, object menuItem)
        {
            ItemCollection itemCollection = null;
            if (parentMenu is RadContextMenu)
                itemCollection = (parentMenu as RadContextMenu).Items;
            else if (parentMenu is RadMenuItem)
                itemCollection = (parentMenu as RadMenuItem).Items;
            itemCollection.Remove(menuItem);
        }

        public I_View_RequestNote GetRequestNoteForm()
        {
            frmRequestNote view = new View.frmRequestNote();
            return view;
        }

        public I_View_RequestFile GetRequestFileForm()
        {
            frmRequestFile view = new View.frmRequestFile();
            return view;
        }

        public I_View_RequestDiagram GetRequestDiagramForm()
        {
            frmRequestDiagram view = new View.frmRequestDiagram();
            return view;
        }

        public I_View_EntityLettersArea GenerateViewOfEntityLettersArea()
        {
            return new frmLettersList();
        }

        public I_View_LetterArea GenerateViewOfLetterArea()
        {
            return new frmLetter();
        }


        public I_View_ArchiveArea GenerateViewOfArchiveArea()
        {
            return new frmArchive();
        }

        public I_View_AddArchiveItems GenerateViewOfAddArchiveItem()
        {
            return new frmAddArchiveItems();
        }

        public bool DownloadFile(FileRepositoryDTO attechedFile, bool fileNameWithTimeStamp)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = GetFilter(attechedFile);
            saveFileDialog.DefaultExt = attechedFile.FileExtension;
            var fileName = "";
            if (fileNameWithTimeStamp)
                fileName = attechedFile.FileName + "_" + DateTime.Now.ToString().Replace(" ", "").Replace("/", "").Replace(":", "") + "." + attechedFile.FileExtension;
            else
                fileName = attechedFile.FileName + "." + attechedFile.FileExtension;

            saveFileDialog.FileName = fileName;
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllBytes(saveFileDialog.FileName, attechedFile.Content);
            return true;
        }

        private string GetFilter(FileRepositoryDTO attechedFile)
        {
            return string.Format("file (*.{0})|*.{0}", attechedFile.FileExtension);
        }

        public Tuple<List<Enum_ArchiveItemFileType>, I_View_ViewArchiveItem> GetArhiveItemViewer(Enum_ArchiveItemMainType mainType, Enum_ArchiveItemFileType fileType)
        {
            if (mainType == Enum_ArchiveItemMainType.Image)
            {
                var fileTypes = new List<Enum_ArchiveItemFileType>() { Enum_ArchiveItemFileType.JPEG, Enum_ArchiveItemFileType.BMP, Enum_ArchiveItemFileType.GIF };
                return new Tuple<List<Enum_ArchiveItemFileType>, I_View_ViewArchiveItem>(fileTypes, new frmViewImage());
            }
            return null;

        }

        //public I_View_ArchiveTag GenerateViewOfArchiveTag()
        //{
        //    return new frmArchiveTag();
        //}

        public I_View_ArchiveTagFiltered GenerateViewOfArchiveTagFilter()
        {
            return new frmArchiveTagFilter();
        }

        public I_View_ArchiveItemInfo GenerateViewOfArchiveItemInfo()
        {
            return new frmArchiveItemInfo();
        }

        public I_View_MultipleArchiveItemsInfo GenerateViewOfMultipleArchiveItemInfo()
        {
            return new frmMultipleArchiveItemsInfo();
        }

        public I_View_DataViewArea GetViewOfDataViewArea()
        {
            return new frmDataView();
        }
        public I_View_GridViewArea GetViewOfGridViewArea()
        {
            return new frmGridView();
        }
        public I_DataViewItem GetDataViewItem()
        {
            return new UC_DataViewItem();
        }

        public I_View_DataViewAreaContainer GetViewOfDataViewAreaContainer()
        {
            return new frmDataViewContainer();
        }

        //public I_View_DataListReportAreaContainer GetViewOfDataListReportAreaContainer()
        //{
        //    return new frmDataListReportContainer();
        //}

        //public I_View_DataListReportArea GetViewOfDataListReportArea()
        //{
        //    return new frmDataListReport();
        //}

        public I_View_ReportList GetViewOfReportList()
        {
            return new frmReportList();
        }



        public I_View_DataLinkArea GenerateViewOfDataLinkArea()
        {
            return new frmDataLink();
        }

        public I_View_InternalReportArea GenerateViewOfInternalReportArea()
        {
            //return null;
            //   return new MyReportViewer();
            return new frmReportViewer();
        }

        public I_View_ExternalReportArea GenerateViewOfExternalReportArea()
        {
            return new frmExternalReportViewer();
        }

        //public void SetReadonlyOneData(UIControlPackageForSimpleColumn controlPackage, ColumnDTO column, bool readonlity)
        //{
        //    throw new NotImplementedException();
        //}

        //public void SetReadonlyMultipleData(UIControlPackageMultipleData controlPackage, ColumnDTO column, bool readonlity)
        //{
        //    throw new NotImplementedException();
        //}

        //public void DisableEnableMultipleColumn(DP_DataRepository dataItem, BaseSimpleColumn column, bool enable)
        //{
        //    throw new NotImplementedException();
        //}

        //public void SetReadonlyMultipleColumn(DP_DataRepository dataItem, BaseSimpleColumn column, bool readonlity)
        //{
        //    throw new NotImplementedException();
        //}

        public I_CommandManager GetCommandManager()
        {
            return CommandHelper.GenerateCommand();
        }

        public I_ViewDeleteInquiry GetDeleteInquiryView()
        {
            return new UC_DeleteInquiry();
        }


        public I_ConfirmUpdate GetConfirmUpdateForm()
        {
            return new UC_UpdateConfirm();
        }

        public I_DataTree GetDataTreeForm()
        {
            return new UC_DataUpdateTree();
        }

        //public I_FormulaOptions GetFormulaOptionForm()
        //{
        //    return new UC_FormulaOptions();
        //}

        //public I_FormulaUsageParameters GetFormulaUsageParametersForm()
        //{
        //    return new UC_FormulaUsageParemeters();
        //}
        public I_View_DiagramStateInfo GetViewOfDiagramStateInfo()
        {
            return new frmDiagramStateInfo();
        }
        public I_DataTreeView GetViewOfDataTree()
        {
            return new frmDataTree();
        }

        public I_View_EntitySelectArea GenerateViewOfEntityselectArea()
        {
            return new frmEntitySelector();
        }

        public I_View_GeneralEntitySearchArea GenerateViewOfGeneralEntitySearchArea()
        {

            return new frmGeneralEntitySearch();

        }

        public void ShowMessage(string title, string message, List<ResultDetail> details = null)
        {
            //   System.Windows.MessageBox.Show(message, title);
            UC_Message view = new MyUIGenerator.UC_Message(this, message, details);
            RadWindow radWindow = new RadWindow();
            radWindow.Content = view;
            radWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            radWindow.Width = 380;
            radWindow.Height = 170;
            radWindow.Header = title;
            radWindow.HideMinimizeButton = true;
            radWindow.ShowDialog();
        }
        public void ShowMessage(string message)
        {
            ShowMessage("پیام", message);
        }
        public MyUILibrary.Temp.ConfirmResul ShowConfirm(string title, string message, UserPromptMode dialogMode)
        {
            //if (MessageBox.Show(text, title, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            //    return MyUILibrary.Temp.ConfirmResul.Yes;
            //else
            //    return MyUILibrary.Temp.ConfirmResul.No;

            UC_Prompt view = new MyUIGenerator.UC_Prompt(this, message, dialogMode);
            RadWindow radWindow = new RadWindow();
            radWindow.Content = view;
            radWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            radWindow.MinWidth = 380;
            radWindow.MinHeight = 170;
            radWindow.Header = title;
            radWindow.HideMinimizeButton = true;
            radWindow.ShowDialog();
            if (view.Result == true)
                return ConfirmResul.Yes;
            else if (view.Result == false)
                return ConfirmResul.No;
            else
                return ConfirmResul.Unknown;
        }
        public UserDialogResult ShowPromptDialog(I_ViewDialog view, string title, Enum_WindowSize windowSize = Enum_WindowSize.None)
        {
            var propmpt = new PromptDialogManager();
            return propmpt.GetDialogResult(view, title, windowSize);
        }

        public I_View_LogReportArea GenerateViewOfLogReportArea()
        {
            return new UC_LogReport();
        }

        public I_MainFormMenu AddMainFormMenu(string title, string image)
        {
            return MainForm.AddToolsMenu(title, image);
        }

        public I_View_EditLogReportDetails GenereateViewOfEditLogReportDetails()
        {
            return new UC_EditLogDetails();
        }

        public I_View_FormulaCalculationArea GetViewOfFormulaCalculationArea()
        {
            return new UC_FormulaCalculationArea();
        }

        public I_FormulaDataTree GetViewOdFormulaTree()
        {
            return new UC_FormulaDataTree();
        }

        public I_View_ArchiveLogReportDetails GenereateViewOfArchiveLogReportDetails()
        {
            return new UC_ArchiveLogDetails();
        }

        public I_View_WorkflowReport GetWorkflowReportForm()
        {
            return new frmWorkflowReport();
        }

        public void CloseApplication(bool ask)
        {
            if (ask)
            {
                if (ShowConfirm("خروج", "آیا قصد خروج از برنامه را دارید؟", UserPromptMode.YesNo) == ConfirmResul.Yes)
                    Application.Current.Shutdown();
            }
            else
                Application.Current.Shutdown();
        }

        public bool ControlIsVisible(object control)
        {

            return (control as UIElement).Visibility == Visibility.Visible;
        }

        public void SetContaierVisiblity(object control, bool visible)
        {
            if (!visible && (control as UIElement) is TabItem)
            {
                if ((control as TabItem).IsSelected)
                {
                    var tabControl = (control as TabItem).Parent as TabControl;
                  //  TabItem otherTab = null;
                    foreach (var item in tabControl.Items)
                    {
                        if (item is TabItem && item != control)
                        {

                            if ((item as TabItem).Visibility == Visibility.Visible)
                            {
                                (item as TabItem).IsSelected = true;
                                break;
                            }

                        }
                    }
                  
                     
                }
            }
            (control as UIElement).Visibility = visible ? Visibility.Visible : Visibility.Collapsed;


        }
    }

}
