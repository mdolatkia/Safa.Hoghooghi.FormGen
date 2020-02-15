
using ModelEntites;
using MyUILibrary;
using MyUILibrary.EntityArea;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Telerik.Windows.Controls;
using MyUILibrary.Temp;
using System.Windows;
using Telerik.Windows.Controls.GridView;
using System.Windows.Threading;

namespace MyUIGenerator.UIControlHelper
{
    public class DataGridHelper
    {
        //public static event EventHandler<Arg_DataContainer> DataCotainerIsReady;
        public RadGridView dataGrid;
        public event EventHandler<DataContainerLoadedArg> DataContainerLoaded;
        public DataGridHelper()
        {
            dataGrid = new RadGridView();
            dataGrid.EnableColumnVirtualization = false;
            dataGrid.EnableRowVirtualization = false;
            dataGrid.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            dataGrid.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            dataGrid.AutoGenerateColumns = false;
            dataGrid.ShowGroupPanel = false;
            //dataGrid.AddingNewDataItem += dataGrid_AddingNewDataItem;
            //     dataGrid.
            dataGrid.RowLoaded += dataGrid_RowLoaded;
            dataGrid.AddingNewDataItem += DataGrid_AddingNewDataItem;
            dataGrid.RowActivated += DataGrid_RowActivated;

            //dataGrid.CellLoaded+=dataGrid_CellLoaded;

        }

        private void DataGrid_RowActivated(object sender, Telerik.Windows.Controls.GridView.RowEventArgs e)
        {

        }

        private void DataGrid_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {

        }

        void dataGrid_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (e.DataElement != null)
                if (DataContainerLoaded != null)
                    DataContainerLoaded(this, new DataContainerLoadedArg() { DataItem = e.DataElement });
        }

        internal void SetColor(object dataItem, InfoColor color)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                GridViewRow row = (GridViewRow)dataGrid.ItemContainerGenerator
                                                  .ContainerFromItem(dataItem);
                if (row != null)
                {
                    row.BorderBrush = UIManager.GetColorFromInfoColor(color);
                    row.BorderThickness = new Thickness(1);
                }
            }));
        }
        internal void ClearColor(object dataItem)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                GridViewRow row = (GridViewRow)dataGrid.ItemContainerGenerator
                                                  .ContainerFromItem(dataItem);
            if (row != null)
            {
                row.BorderBrush = null;// new SolidColorBrush(UIManager.GetColorFromInfoColor(InfoColor.Black));
                row.BorderThickness = new Thickness(1);
                }
            }));
        }
        //static void dataGrid_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        //{
        //    if (e.NewObject is DP_DataRepository)
        //    {
        //        if (DataCotainerIsReady != null)
        //        {
        //            Arg_DataContainer arg = new Arg_DataContainer();
        //            arg.DataItem = (e.NewObject as DP_DataRepository);
        //            DataCotainerIsReady(sender, arg);
        //        }
        //    }
        //}



        //internal UIControlPackageMultipleData GenerateContolPackageOfView(object view, ColumnSetting columnSetting)
        //{
        //    //UIControlPackageMultipleData viewPackage = new UIControlPackageMultipleData();
        //    //viewPackage.UIControls = new List<UIControl>();
        //    //UIControlSetting controlUISettingviewPackage = new UIControlSetting();
        //    //controlUISettingviewPackage.DesieredColumns = 1;
        //    //controlUISettingviewPackage.DesieredRows = 1;
        //    //viewPackage.UIControls.Add(ag_UIControl);



        //    UIControlPackageMultipleData package = new UIControlPackageMultipleData();


        //    //package.UIControls = new List<UIControl>();
        //    //UIControlSetting 


        //    //column.lo
        //    //UIControlSetting controlUISetting = new UIControlSetting();
        //    //controlUISetting.DesieredColumns = 1;
        //    //controlUISetting.DesieredRows = 1;


        //    package.DataDependentControl = view;
        //    //package.DataDependentControl.UIControlSetting = controlUISetting;

        //    return package;
        //}


        //internal bool SetValue(DP_DataRepository dataItem, UIControlPackageMultipleData controlPackage, string value)
        //{
        //    //(typePropertyControl.ControlPackage.UIControls.First().Control as TextBox).Text = value;
        //    ////var dataRow = GetDataRow(dataGrid, dataItem);
        //    //var dataRow = dataGrid.GetRowForItem(dataItem);
        //    //if (dataRow == null)
        //    //{
        //    //    //   dataGrid.Items.Add(dataItem);

        //    //    //dataGrid.ref.BeginInsert();
        //    //    dataRow = dataGrid.RowInEditMode;
        //    //    dataRow.DataContext = dataItem;

        //    ////}
        //    //if (dataRow != null)
        //    //{
        //    //    var cell = dataRow.GetCell(controlPackage.UIControls.First().Control as Telerik.Windows.Controls.GridViewColumn);

        //    //    if (cell != null)

        //    //////return (controlPackage.UIControl as DataGridTextColumn).SetValue(dataItem, value);
        //    return false;


        //    //cell.Content = value;
        //    //    return true;
        //    //}
        //    //return false;
        //}

        //internal string GetValue(DP_DataRepository dataItem, UIControlPackageMultipleData controlPackage)
        //{
        //    //////return (controlPackage.UIControl as DataGridTextColumn).GetValue(dataItem);
        //    return null;
        //}
        internal void AddDataContainer(object dataItem)
        {

            if (!dataGrid.Items.Contains(dataItem))
                dataGrid.Items.Add(dataItem);
            else
                throw (new Exception("zzdfdxf"));
            //     dataGrid.BeginInsert();
            //dataGrid.ItemsSource = dataItems;
        }
        internal void AddDataContainers(List<object> dataItems)
        {
            foreach (var item in dataItems)
                AddDataContainer(item);
            //     dataGrid.BeginInsert();
            //dataGrid.ItemsSource = dataItems;
        }

        internal void ClearTooltip(object dataItem)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                GridViewRow row = (GridViewRow)dataGrid.ItemContainerGenerator
                                                  .ContainerFromItem(dataItem);
            if (row != null)
                ToolTipService.SetToolTip(row, null);
            }));
        }

        internal void SetTooltip(object dataItem, string tooltip)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                GridViewRow row = (GridViewRow)dataGrid.ItemContainerGenerator
                                                    .ContainerFromItem(dataItem);
            if (row != null)
            {
                if (!string.IsNullOrEmpty(tooltip))
                    ToolTipService.SetToolTip(row, tooltip);
                else
                    ToolTipService.SetToolTip(row, null);
            }
            }));
        }
        internal void SetBorderColor(object dataItem, InfoColor color)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                GridViewRow row = (GridViewRow)dataGrid.ItemContainerGenerator
                                                     .ContainerFromItem(dataItem);
            if (row != null)
            {
                row.BorderBrush = UIManager.GetColorFromInfoColor(color);
                row.BorderThickness = new Thickness(1);
                }
            }));
        }
        internal void SetBackgroundColor(object dataItem, InfoColor color)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                GridViewRow row = (GridViewRow)dataGrid.ItemContainerGenerator
                                                     .ContainerFromItem(dataItem);
            if (row != null)
            {
                row.Background = UIManager.GetColorFromInfoColor(color);
            }
            }));
        }
        internal void SetForegroundColor(object dataItem, InfoColor color)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                GridViewRow row = (GridViewRow)dataGrid.ItemContainerGenerator
                                                     .ContainerFromItem(dataItem);
            if (row != null)
            {
                row.Foreground = UIManager.GetColorFromInfoColor(color);
            }
            }));
        }
        internal void Visiblity(object dataItem, bool visible)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                GridViewRow row = (GridViewRow)dataGrid.ItemContainerGenerator
                                                      .ContainerFromItem(dataItem);
            if (row != null)
            {
                row.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
                }
            }));
        }
        internal void EnableDisable(object dataItem, bool enable)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                GridViewRow row = (GridViewRow)dataGrid.ItemContainerGenerator
                                                      .ContainerFromItem(dataItem);
            if (row != null)
            {
                row.IsEnabled = enable;
                }
            }));
        }
        internal List<object> RemoveDataContainers()
        {
            List<object> result = new List<object>();
            foreach (var item in dataGrid.Items)
                result.Add(item);
            dataGrid.Items.Clear();
            return result;
        }
        internal void RemoveDataContainer(object dataItem)
        {
            //foreach (var item in dataItems)
            dataGrid.Items.Remove(dataItem);
            //dataGrid.Items.Remove(item);
        }
        //internal void RemoveDataContainers(List<object> dataItems)
        //{
        //    //foreach (var item in dataItems)
        //    foreach (var item in dataItems)
        //        RemoveDataContainer(item);
        //    //dataGrid.Items.Remove(item);
        //}
        internal void RemoveSelectedDataContainers()
        {
            var selectedItems = GetSelectedData();
            foreach (var item in selectedItems)
                dataGrid.Items.Remove(item);
        }

        internal List<object> GetSelectedData()
        {
            List<object> result = new List<object>();
            foreach (var item in dataGrid.SelectedItems)
                result.Add(item as object);
            return result;
        }






        //internal static bool SetValue(DataGrid dataGrid, DP_DataRepository dataItem, ColumnControl typePropertyControl, string value)
        //{
        //    //(typePropertyControl.ControlPackage.UIControls.First().Control as TextBox).Text = value;
        //    ////var dataRow = GetDataRow(dataGrid, dataItem);
        //    var dataRow = dataGrid.GetDataRow(dataItem);
        //    if (dataRow != null)
        //    {
        //        var cell = dataGrid.GetCell(dataRow, typePropertyControl.ControlPackage.UIControls.First().Control as DataGridColumn);
        //        cell.Content = value;
        //        return true;
        //    }
        //    return false;
        //}

        //internal static string GetValue(DataGrid dataGrid, DP_DataRepository dataItem, ColumnControl typePropertyControl)
        //{
        //    var dataRow = dataGrid.GetDataRow(dataItem);
        //    if (dataRow != null)
        //    {
        //        var cell = dataGrid.GetCell(dataRow, typePropertyControl.ControlPackage.UIControls.First().Control as DataGridColumn);
        //        if (cell.Content != null)
        //            return cell.Content.ToString();
        //    }
        //    return "";
        //}



        //public static T GetVisualChild<T>(Visual parent) where T : Visual
        //{
        //    T child = default(T);
        //    int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
        //    for (int i = 0; i < numVisuals; i++)
        //    {
        //        Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
        //        child = v as T;
        //        if (child == null)
        //        {
        //            child = GetVisualChild<T>(v);
        //        }
        //        if (child != null)
        //        {
        //            break;
        //        }
        //    }
        //    return child;
        //}

        //public static DataGridRow GetSelectedRow(this DataGrid grid)
        //{
        //    return (DataGridRow)grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem);
        //}
        //public static DataGridRow GetDataRow(this DataGrid grid, int index)
        //{
        //    DataGridRow row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
        //    if (row == null)
        //    {
        //        // May be virtualized, bring into view and try again.
        //        grid.UpdateLayout();
        //        grid.ScrollIntoView(grid.Items[index]);
        //        row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
        //    }
        //    return row;
        //}
        //private static DataGridRow GetDataRow(this DataGrid dataGrid, object dataItem)
        //{
        //    foreach (var item in dataGrid.Items)
        //    {
        //        if (item == dataItem)
        //        {
        //            DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator
        //                                                       .ContainerFromItem(item);
        //            return row;
        //        }
        //    }
        //    return null;
        //}
        //public static DataGridCell GetCell(this DataGrid grid, DataGridRow row, int column)
        //{
        //    if (row != null)
        //    {
        //        DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);

        //        if (presenter == null)
        //        {
        //            grid.ScrollIntoView(row, grid.Columns[column]);
        //            presenter = GetVisualChild<DataGridCellsPresenter>(row);
        //        }

        //        DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
        //        return cell;
        //    }
        //    return null;
        //}
        //public static DataGridCell GetCell(this DataGrid grid, DataGridRow row, DataGridColumn column)
        //{
        //    if (row != null)
        //    {
        //        DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);

        //        if (presenter == null)
        //        {
        //            grid.ScrollIntoView(row, column);
        //            presenter = GetVisualChild<DataGridCellsPresenter>(row);
        //        }

        //        DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(grid.Columns.IndexOf(column));
        //        return cell;
        //    }
        //    return null;
        //}



        //public static DataGridCell GetCell(this DataGrid grid, int row, int column)
        //{
        //    DataGridRow rowContainer = grid.GetDataRow(row);
        //    return grid.GetCell(rowContainer, column);
        //}

        ////internal static UIControlSetting GenerateUISetting(DataMaster.EntityDefinition.ND_Type_Property nD_Type_Property, UISetting.DataPackageUISetting.UI_PackagePropertySetting uI_PackagePropertySetting)
        ////{
        ////    throw new NotImplementedException();
        ////}


        //internal static bool SetValue(ColumnControl typePropertyControl, string value)
        //{
        //    (typePropertyControl.ControlPackage.UIControls.First().Control as TextBox).Text = value;
        //    return true;
        //}

        //internal static string GetValue(ColumnControl typePropertyControl)
        //{
        //    return (typePropertyControl.ControlPackage.UIControls.First().Control as TextBox).Text;
        //}



        //internal void AddControlToLayout(UIControlPackageMultipleData iAG_DataDependentControl, string title, MyUILibrary.Temp.InfoColor titleColor, string tooltip)
        //{
        //    //////var labelUIControl = LabelHelper.GenerateLabelControl(title, tooltip, titleColor);
        //    //////UIControl labelControl = new UIControl();
        //    //////labelControl.Control = labelUIControl;
        //    //////(iAG_DataDependentControl.UIControl as LocalControlManager).RelatedControl.Add(labelControl);

        //    //////var column = (iAG_DataDependentControl.UIControl as LocalControlManager).MainControl.Control as Telerik.Windows.Controls.GridViewColumn;
        //    //////column.Header = labelUIControl.Control;
        //    ////////(labelUIControl.Control as System.Windows.Controls.Label).Foreground = Brushes.White;
        //    //////dataGrid.Columns.Add(column);
        //}

        internal void SetSelectedData(List<object> dataItems)
        {
            if (dataItems != null)
            {
                ObservableCollection<object> items = new ObservableCollection<object>();
                dataItems.ForEach(x => items.Add(x));
                dataGrid.SelectedItem = items.FirstOrDefault();
            }
            else
                dataGrid.SelectedItem = null;
        }




        //internal static UIControlPackageMultipleData GenerateMultipleDataDependentControl(ColumnDTO correspondingTypeProperty, ColumnUISettingDTO columnSetting)
        //{
        //    UIControlPackageMultipleData package = new UIControlPackageMultipleData();
        //    //package.UIControls = new List<UIControl>();
        //    //UIControlSetting controlUISetting = new UIControlSetting();
        //    //controlUISetting.DesieredColumns = 1;
        //    //controlUISetting.DesieredRows = 1;
        //    //  Telerik.Windows.Controls.GridViewColumn column;
        //    //if (AgentHelper.GetColumnType(correspondingTypeProperty) == Enum_UIColumnType.Text)
        //    //{
        //    //    column = new DataGridTextColumn(correspondingTypeProperty, columnSetting);
        //    //}
        //    //else
        //    //{
        //    var column = new DataGridTextColumn(correspondingTypeProperty, columnSetting);
        //    column.ValueChanged += (sender, e) => column_ValueChanged(sender, e, package);
        //    //}
        //    //column.IsReadOnly = columnSetting.IsReadOnly;

        //    //////package.UIControl = column;



        //    //package.DataDependentControl.UIControlSetting = controlUISetting;
        //    //var textBox = new TextBox();
        //    //textBox.Margin = new System.Windows.Thickness(5);
        //    //textBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
        //    //textBox.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
        //    //  package.UIControls.Add(new UIControl() { Control = column, UIControlSetting = controlUISetting });


        //    return package;
        //}

        //internal static void GenerateMenu(UIControlPackageMultipleData controlPackage, ConrolPackageMenu cpMenu)
        //{
        //    //////(controlPackage.UIControl as DataGridTextColumn).GenerateMenu(cpMenu);
        //}

        //static void column_ValueChanged(object sender, ColumnValueChangeArg e, UIControlPackageMultipleData UIControlPackageMultipleData)
        //{
        //    UIControlPackageMultipleData.OnValueChanged(sender, e);
        //}
        //internal static I_View_DataDependentControl GenerateMultipleDataDependentViewControl(TemporaryLinkType linkType)
        //{
        //    return new DataGridViewColumn(linkType);
        //}

        //internal static void SetReadonly(UIControlPackageMultipleData controlPackage, bool isReadOnly)
        //{
        //    //////((controlPackage as UIControlPackageMultipleData).UIControl as Telerik.Windows.Controls.GridViewColumn).IsReadOnly = isReadOnly;
        //}

        //internal static void DisableEnableRow(DataGridHelper gridHelper, DP_DataRepository dataItem, bool enable)
        //{
        //    var dataRow = gridHelper.dataGrid.GetRowForItem(dataItem);

        //    if (dataRow != null)
        //    {
        //        dataRow.IsEnabled = enable;
        //    }
        //}

        //internal static void DisableEnableCell(DP_DataRepository dataItem, UIControlPackageMultipleData controlPackage, ColumnDTO column, bool enable)
        //{
        //    //////(((controlPackage.UIControl as LocalControlManager).MainControl.Control) as DataGridTextColumn).DisableEnableCell(dataItem, enable);
        //}

        //internal static void SetReadonlyRow(DataGridHelper gridHelper, DP_DataRepository dataItem, bool readonlity)
        //{
        //    var dataRow = gridHelper.dataGrid.GetRowForItem(dataItem);

        //    if (dataRow != null)
        //    {
        //        //بعدا درست شود
        //        dataRow.IsEnabled = !readonlity;
        //    }
        //}

        //internal static void SetReadonlyCell(DP_DataRepository dataItem, UIControlPackageMultipleData controlPackage, ColumnDTO column, bool readonlity)
        //{
        //    //////(((controlPackage.UIControl as LocalControlManager).MainControl.Control) as DataGridTextColumn).SetReadonlyCell(dataItem, readonlity);
        //}
    }
    public static class GridStatic
    {
        public static Telerik.Windows.Controls.GridView.GridViewCellBase GetCell(this Telerik.Windows.Controls.GridView.GridViewRow row, Telerik.Windows.Controls.GridViewColumn column)
        {
            if (row != null)
            {
                foreach (var cell in row.Cells)
                    if (cell.Column == column)
                        return cell;
            }
            return null;
        }
    }
}

