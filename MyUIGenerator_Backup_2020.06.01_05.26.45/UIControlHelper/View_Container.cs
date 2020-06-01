
//using MyUIGenerator.UIContainerHelper;
using MyUIGenerator.UIControlHelper;
using MyUILibrary;
using MyUILibrary.EntityArea;
using MyUILibrary.Temp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Telerik.Windows.Controls;

namespace MyUIGenerator.UIControlHelper
{
    public abstract class View_Container : UserControl, I_View_Container
    {

        public abstract void AddView(object view, I_LabelControlManager labelControlManager);
        public abstract void AddUIControlPackage(I_SimpleControlManager controlManager, I_LabelControlManager labelControlManager);
        public abstract void ClearControls();
        public int ControlsCount { get; }
        public void EnableDisable(bool enable)
        {
            this.IsEnabled = enable;
        }
        public void DisableEnableDataSection(bool enable)
        {
            ControlArea.IsEnabled = enable;
        }
        public UIControlPackageTree UIControlPackageTreeItem { get; set; }

        public bool IsOpenedTemporary { get; set; }
        //public View_Container(GridSetting gridSetting)
        //{
        //    GridSetting = gridSetting;
        //    Grid = GenerateControl();

        //}

        //public CommonDefinitions.BasicUISettings.GridSetting GridSetting
        //{
        //    get;
        //    set;
        //}

        //public object ContentGrid
        //{
        //    get { return Grid; }
        //}
        //private Grid Grid
        //{
        //    set;
        //    get;
        //}

        //internal void AddControls(Telerik.Windows.Controls.RadGridView radGridView)
        //{
        //    AddControlToGrid(radGridView, new UIControlSetting() { DesieredColumns = ColumnWidth.Full });
        //    Grid.Children.Add(radGridView);
        //}
        //public void AddUIControlPackage(UIControlPackage controlPackage, string title, InfoColor titleColor, string tooltip = "")
        //{
        //    UIControl labelControl = null;
        //    if (!string.IsNullOrEmpty(title))
        //    {
        //        labelControl = LabelHelper.GenerateLabelControl(title, tooltip, titleColor);
        //        controlPackage.RelatedUIControls.Add(new AG_RelatedConttol() { RelationType = AG_ControlRelationType.Label, RelatedUIControl = labelControl });
        //        if (controlPackage.UIControl.Control is I_View_Container)
        //            (controlPackage.UIControl.Control as I_View_Container).SetExpanderInfo(labelControl.Control);
        //        else
        //            AddControlToGrid(labelControl.Control as UIElement, labelControl.UIControlSetting);
        //    }
        //    var uiControl = controlPackage.UIControl;
        //    AddControlToGrid(uiControl.Control as UIElement, uiControl.UIControlSetting);

        //    //controlPackage.Container = this;



        //}


        //public UIControlPackage AddGroupControl()
        //{


        //    controlPackage.Container = this;
        //    var uiControl = controlPackage.UIControl;
        //    AddControlToGrid(uiControl.Control as UIElement, uiControlSetting);
        //    return controlPackage;
        //}

        //internal UIControlPackage GenerateControl(string title, GridSetting basicUISetting, bool groupOrTab)
        //{
        //    if (groupOrTab)
        //    {
        //        return new GroupPanelHelper(title, basicUISetting);
        //    }
        //    else
        //    {
        //        return GroupPanelHelper.GenerateControl(title, basicUISetting);
        //    }
        //}


        //public GridSetting GridSetting { set; get; }
        //private MyUILibrary.EntityArea.FlowDirection FlowDirection;

        //int CurrentColumn = 0;
        //int CurrentRow = 0;


        //internal Grid GenerateControl()
        //{

        //    var grid = new Grid();
        //    //if (FlowDirection == MyUILibrary.EntityArea.FlowDirection.RightToLeft)
        //    //    grid.FlowDirection = System.Windows.FlowDirection.RightToLeft;

        //    //double formWidth = GridSetting.DefaultWidth;
        //    int uiColumnsCount = GridSetting.DefaultColumnCount * 2;
        //    //var calculatedColumnWidth = formWidth / uiColumnsCount;
        //    //while (calculatedColumnWidth < GridSetting.MinimumColumnWidth)
        //    //{
        //    //    uiColumnsCount -= 2;
        //    //    if (uiColumnsCount <= 0)
        //    //        throw (new Exception(""));
        //    //    calculatedColumnWidth = formWidth / uiColumnsCount;
        //    //}

        //    for (var i = 0; i < uiColumnsCount; i++)
        //    {
        //        var columnDefinition = new ColumnDefinition();
        //        if (i % 2 == 0)
        //            columnDefinition.Width = GridLength.Auto;
        //        grid.ColumnDefinitions.Add(columnDefinition);
        //    }

        //    for (var i = 0; i < 40; i++)
        //    {
        //        // LayoutGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(BasicUISetting.MinimumRowHeight) });
        //        grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
        //    }
        //    //LayoutGrid.ShowGridLines = true;
        //    return grid;
        //}
        //private void AddControlToGrid(UIElement uiElement, UIControlSetting uiControlSetting)
        //{
        //    int columnCout = 0;
        //    if (uiControlSetting != null)
        //        if (uiControlSetting.DesieredColumns == ColumnWidth.Full)
        //        {
        //            columnCout = Grid.ColumnDefinitions.Count;
        //        }
        //        else if (uiControlSetting.DesieredColumns == ColumnWidth.half)
        //        {
        //            columnCout = Grid.ColumnDefinitions.Count / 2;

        //        }
        //        else if (uiControlSetting.DesieredColumns == ColumnWidth.Normal)
        //        {
        //            columnCout = Grid.ColumnDefinitions.Count / 4;

        //        }
        //    if (columnCout == 0)
        //        columnCout = 1;
        //    if (columnCout + CurrentColumn > Grid.ColumnDefinitions.Count)
        //        MoveToNewRow();

        //    Grid.SetColumn(uiElement, CurrentColumn);
        //    Grid.SetRow(uiElement, CurrentRow);
        //    if (columnCout != 0)
        //        Grid.SetColumnSpan(uiElement, columnCout);

        //    if (uiControlSetting != null && uiControlSetting.DesieredRows != 0)
        //        Grid.SetRowSpan(uiElement, uiControlSetting.DesieredRows);
        //    CurrentColumn += columnCout;
        //    // CurrentRow += uiControl.UIControlSetting.DesieredRows;

        //    Grid.Children.Add(uiElement);
        //}
        //private void MoveToNewRow()
        //{
        //    CurrentColumn = 0;
        //    CurrentRow++;
        //}

        //bool _AllowExpand;
        //public bool AllowExpand
        //{
        //    get
        //    {
        //        return _AllowExpand;
        //    }
        //    set
        //    {
        //        _AllowExpand = value;
        //    }
        //}
        //public virtual Expander Expander { get; }
        //public void Expand()
        //{
        //    if (Expander != null)
        //        Expander.IsExpanded = true;
        //}
        //public void Collapse()
        //{
        //    if (Expander != null)
        //        Expander.IsExpanded = false;
        //}
        //public virtual Grid expanderHeader { get; }
        //public virtual void SetExpanderInfo(object header)
        //{
        //    if (expanderHeader != null)
        //        expanderHeader.Children.Add(header as UIElement);
        //}
        //internal static UIControlSetting GenerateUISetting(ColumnDTO nD_Type_Property, UISetting.DataPackageUISetting.UI_PackagePropertySetting uI_PackagePropertySetting)
        //{
        //    throw new NotImplementedException();
        //}



        //internal static bool SetValue(ColumnControl typePropertyControl, string value)
        //{
        //    if (typePropertyControl.UI_PropertySetting.PropertyType == CommonDefinitions.CommonUISettings.Enum_UI_PropertyType.Text)
        //    {
        //        return ControlHelpers.TextBoxHelper.SetValue(typePropertyControl, value);
        //    }
        //    else
        //        return ControlHelpers.TextBoxHelper.SetValue(typePropertyControl, value);
        //}



        //public static UIControl GetUIControl(UIControlPackage aG_UIControlPackage)
        //{
        //    return aG_UIControlPackage.DataControls.First();
        //}




        public virtual Grid ControlArea { get; }
        public virtual Grid Toolbar { get; }

        public event EventHandler<Arg_CommandExecuted> CommandExecuted;

        //public void AddCommands(List<I_Command> commands, TemplateEntityUISettings templateEntityUISettings)
        //{
        //    foreach (var item in commands.OrderBy(x => x.Position))
        //    {
        //        AddCommand(item);
        //    }
        //}
        public void DeHighlightCommands()
        {
            Toolbar.Background = null;
        }
        Grid grdMain;
        Grid grdOther;
        public void AddCommand(I_CommandManager item, bool indirect)
        {
            if (grdMain == null)
            {
                grdMain = new Grid();
                grdOther = new Grid();
                Toolbar.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                Toolbar.ColumnDefinitions.Add(new ColumnDefinition());
                Toolbar.Children.Add(grdMain);
                Toolbar.Children.Add(grdOther);
                Grid.SetColumn(grdOther, 1);
            }
            //Button btnCommand = UIHelper.GenerateCommand(item);
            ////item.EnabledChanged += (sender, e) => item_EnabledChanged(sender, e, btnCommand);
            //btnCommand.Click += btnCommand_Click;
            //if (Toolbar.ColumnDefinitions.Count == 0)
            //    Toolbar.ColumnDefinitions.Add(new ColumnDefinition());

            if (indirect == false)
            {
                grdMain.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                grdMain.Children.Add((item as CommandManager).Button);
                Grid.SetColumn((item as CommandManager).Button, grdMain.ColumnDefinitions.Count - 1);
            }
            else
            {
                if (dropDownButton == null)
                {
                    dropDownButton = new RadDropDownButton();
                    dropDownButton.Height = 22;
                    dropDownButton.Width = 35;
                    //dropDownButton.HorizontalAlignment = HorizontalAlignment.Left;
                    dropDownButton.Margin = new System.Windows.Thickness(2);
                    listBox = new ListBox();
                    dropDownButton.DropDownContent = listBox;
                    Image img = new Image();
                    img.Width = 15;
                    Uri uriSource =  new Uri("../../Images/report.png", UriKind.Relative);
                    img.Source = new BitmapImage(uriSource);
                    dropDownButton.Content = img;
                    var listitem = new ListBoxItem();
                    dropDownButton.HorizontalAlignment = HorizontalAlignment.Right;
                    grdOther.Children.Add(dropDownButton);

                }
                //(item as CommandManager).Button.HorizontalContentAlignment = HorizontalAlignment.Left;
                listBox.Items.Add((item as CommandManager).Button);

            }
            if (dropDownButton != null)
            {
                //Toolbar.Items.Remove(dropDownButton);

            }

        }
        RadDropDownButton dropDownButton { set; get; }
        ListBox listBox { set; get; }
        //private void item_EnabledChanged(object sender, EventArgs e, Button btnCommand)
        //{
        //    btnCommand.IsEnabled = (sender as I_Command).Enabled;
        //}

        //void btnCommand_Click(object sender, EventArgs e)
        //{
        //    if (CommandExecuted != null)
        //        CommandExecuted(this, new Arg_CommandExecuted() { Command = (sender as Button).Tag as I_Command });
        //    ////  Controller.CommandExecuted(((sender as Button).Tag as I_EntityAreaCommand));
        //}

        public void SetBackgroundColor(string color)
        {
            this.Background = new SolidColorBrush(UIHelper.getColorFromHexString(color));
        }

        public virtual void AddUIControlPackage(object controlPackage, string title, InfoColor titleColor, string tooltip = "")
        {

        }

        public void Visiblity(bool visible)
        {
            this.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public void SetTooltip(string tooltip)
        {
            if (!string.IsNullOrEmpty(tooltip))
                ToolTipService.SetToolTip(this, tooltip);
            else
                ToolTipService.SetToolTip(this, null);
        }

        public void SetBorderColor(InfoColor color)
        {
            this.BorderBrush = UIManager.GetColorFromInfoColor(color);
            this.BorderThickness = new Thickness(1);
        }

        public void SetBackgroundColor(InfoColor color)
        {
            this.Background = UIManager.GetColorFromInfoColor(color);
        }

        public void SetForegroundColor(InfoColor color)
        {
            this.Foreground = UIManager.GetColorFromInfoColor(color);
        }


        //public void AddUIControlPackage(object controlPackage, string title, InfoColor titleColor, string tooltip = "")
        //{
        //    throw new NotImplementedException();
        //}
    }
}
