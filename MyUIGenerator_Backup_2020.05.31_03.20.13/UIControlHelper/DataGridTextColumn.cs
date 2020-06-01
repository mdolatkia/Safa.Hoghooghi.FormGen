
using ModelEntites;
using MyUIGenerator;
using MyUIGenerator.UIControlHelper;
using MyUILibrary;
using MyUILibrary.EntityArea;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows;
using System.Windows.Threading;
using MyUILibrary.Temp;
using System.Windows.Media;

namespace MyUIGenerator.UIControlHelper
{
    public class DataGridTextColumn : Telerik.Windows.Controls.GridViewColumn
    {
        public event EventHandler<ColumnValueChangeArg> ValueChanged;
        // DataMaster.EntityDefinition.ND_Type_Property TypeProperty { set; get; }
        public ColumnDTO Column { set; get; }
        public ColumnUISettingDTO ColumnSetting { set; get; }

        bool HasRangeOfValues;
     //   bool ValueIsTitleOrValue;

        ConrolPackageMenu CPMenu { set; get; }
        internal void GenerateMenu(ConrolPackageMenu cpMenu)
        {
            CPMenu = cpMenu;


        }


        public DataGridTextColumn(ColumnDTO column, ColumnUISettingDTO columnSetting, bool hasRangeOfValues)
        {

            ColumnSetting = columnSetting;

            Column = column;
            HasRangeOfValues = hasRangeOfValues;
       //     ValueIsTitleOrValue = valueIsTitleOrValue;
            if (columnSetting != null)
            {
                if (columnSetting.UIColumnsType == Enum_UIColumnsType.Full)
                    Width = 200;
                else if (columnSetting.UIColumnsType == Enum_UIColumnsType.Half)
                    Width = 140;
                else
                    Width = 80;
            }

        }
        public override FrameworkElement CreateCellElement(GridViewCell cell, object dataItem)
        {
            I_ControlHelper MyControlHelper = null;
            if (HasRangeOfValues)
                MyControlHelper = ControlHelper.KeyValueControlHelper(Column);
            else
                MyControlHelper = ControlHelper.GetControlHelper(Column, ColumnSetting, null);
            MyControlHelper.SetReadonly(IsReadOnly);
            if (MyControlHelper is I_ControlHelperValueRange)
            {
                if (ColumnValueRange != null)
                {
                    (MyControlHelper as I_ControlHelperValueRange).SetColumnValueRange(ColumnValueRange);
                }
            }
            cell.Tag = MyControlHelper;
            if (ButtonMenus.Any())
            {
                foreach (var item in ButtonMenus)
                {
                    var newMenu = new ConrolPackageMenu();
                    newMenu.Name = item.Name;
                    newMenu.Title = item.Title;
                    newMenu.MenuClicked += (sender, e) => Item_MenuClicked(sender, e, item, dataItem);
                    MyControlHelper.AddButtonMenu(newMenu);
                }
            }
            return MyControlHelper.WholeControl;
        }

        internal object GetUIControl(object dataItem)
        {
            var dataRow = this.DataControl.GetRowForItem(dataItem);

            if (dataRow != null)
            {
                var cell = dataRow.GetCell(this);

                if (cell != null)
                {
                    return (cell.Tag as I_ControlHelper).MainControl;
                }
            }
            return null;
        }

        private void Item_MenuClicked(object sender, ConrolPackageMenuArg e, ConrolPackageMenu mainMenu, object dataItem)
        {
            e.data = dataItem;
            mainMenu.OnMenuClicked(sender, e);

        }

        internal void SetReadonly(bool isreadonly)
        {
            this.IsReadOnly = isreadonly;
        }

        internal void SetReadonly(object dataItem, bool isreadonly)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                var dataRow = this.DataControl.GetRowForItem(dataItem);

                if (dataRow != null)
                {
                    var cell = dataRow.GetCell(this);

                    if (cell != null)
                    {
                        (cell.Tag as I_ControlHelper).SetReadonly(isreadonly);
                    }
                }
            }));
        }
        internal void EnableDisable(bool enable)
        {
            this.IsEnabled = enable;
        }
        internal void EnableDisable(object dataItem, bool enable)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                var dataRow = this.DataControl.GetRowForItem(dataItem);
                if (dataRow != null)
                {
                    var cell = dataRow.GetCell(this);

                    if (cell != null)
                    {
                        (cell.Tag as I_ControlHelper).EnableDisable(enable);

                    }
                }
            }));
        }
        internal void Visiblity(bool visible)
        {
            this.IsVisible = visible;
        }
        internal void Visiblity(object dataItem, bool visible)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                var dataRow = this.DataControl.GetRowForItem(dataItem);
                if (dataRow != null)
                {
                    var cell = dataRow.GetCell(this);

                    if (cell != null)
                    {
                        (cell.Tag as I_ControlHelper).Visiblity(visible);

                    }
                }
            }));
        }
        private void MenuItem_Click(object sender, RadRoutedEventArgs e, ConrolPackageMenu cpMenu, Tuple<object, string> cellItem)
        {
            if (cpMenu != null)
            {
                ConrolPackageMenuArg arg = new ConrolPackageMenuArg();
                arg.data = cellItem;
                cpMenu.OnMenuClicked(sender, arg);
            }
        }


        void control_ValueChanged(object sender, ColumnValueChangeArg e, object dataItem)
        {
            e.DataItem = dataItem;
            if (ValueChanged != null)
                ValueChanged(sender, e);
        }



        //void control_TemporaryDisplayViewRequested(object sender, Arg_TemporaryDisplayViewRequested e)
        //{
        //    TemporaryArg.OnTemporaryViewRequested(sender, e);
        //}


        public override FrameworkElement CreateCellEditElement(GridViewCell cell, object dataItem)
        {

            I_ControlHelper MyControlHelper = null;
            if (HasRangeOfValues)
                MyControlHelper = ControlHelper.KeyValueControlHelper(Column);
            else
                MyControlHelper = ControlHelper.GetControlHelper(Column, ColumnSetting, null);

            if (MyControlHelper is I_ControlHelperValueRange)
            {
                if (ColumnValueRange != null)
                {
                    (MyControlHelper as I_ControlHelperValueRange).SetColumnValueRange(ColumnValueRange);
                }
            }

            return MyControlHelper.WholeControl;
        }

        internal void RemoveButtonMenu(string name, object dataItem)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                var dataRow = this.DataControl.GetRowForItem(dataItem);
                var cell = dataRow.GetCell(this);
                if (cell != null)
                {
                    if (cell.Content != null)
                    {
                        (cell.Tag as I_ControlHelper).RemoveButtonMenu(name);
                    }
                }
            }));
        }

        internal void RemoveButtonMenu(string name)
        {
            ButtonMenus.Remove(ButtonMenus.FirstOrDefault(x => x.Name == "name"));
        }
        internal void AddButtonMenu(ConrolPackageMenu menu)
        {
            ButtonMenus.Add(menu);
        }
        internal void AddButtonMenu(ConrolPackageMenu menu, object dataItem)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                var dataRow = this.DataControl.GetRowForItem(dataItem);
                var cell = dataRow.GetCell(this);
                if (cell != null)
                {
                    if (cell.Content != null)
                    {
                        (cell.Tag as I_ControlHelper).AddButtonMenu(menu);
                    }
                }
            }));
        }

        internal bool SetValue(object dataItem, object value)
        {

            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                var dataRow = this.DataControl.GetRowForItem(dataItem);
                var cell = dataRow.GetCell(this);
                if (cell != null && cell.Content != null)
                    (cell.Tag as I_ControlHelper).SetValue(value);
            }));


            return true;

        }



        internal void SetBinding(object dataItem, EntityInstanceProperty property)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                var dataRow = this.DataControl.GetRowForItem(dataItem);
                var cell = dataRow.GetCell(this);
                if (cell != null)
                {
                    if (cell.Content != null)
                    {
                        (cell.Tag as I_ControlHelper).SetBinding(property);
                        if (property.Name == "Name")
                            (cell.Tag as I_ControlHelper).SetBackgroundColor(InfoColor.Blue);

                    }
                }
            }));
        }

        internal void SetColumnValueRange(List<ColumnValueRangeDetailsDTO> details, object dataItem)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
           {
               var dataRow = this.DataControl.GetRowForItem(dataItem);
               var cell = dataRow.GetCell(this);
               if (cell != null)
               {
                   if ((cell.Tag as I_ControlHelper) is I_ControlHelperValueRange)
                   {
                       ((cell.Tag as I_ControlHelper) as I_ControlHelperValueRange).SetColumnValueRange(details);
                   }
               }
           }));
        }
        List<ColumnValueRangeDetailsDTO> ColumnValueRange { set; get; }
        internal void SetColumnValueRange(List<ColumnValueRangeDetailsDTO> details)
        {
            ColumnValueRange = details;
        }


        List<ConrolPackageMenu> ButtonMenus = new List<ConrolPackageMenu>();

        internal void SetTooltip(string tooltip)
        {
            ToolTipService.SetToolTip(this, tooltip);
        }
        internal void ClearTooltip()
        {
            ToolTipService.SetToolTip(this, null);
        }


        internal void SetTooltip(object dataItem, string tooltip)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                var dataRow = this.DataControl.GetRowForItem(dataItem);
                var cell = dataRow.GetCell(this);
                if (cell != null)
                {
                    if (cell.Content != null)
                    {
                        if (!string.IsNullOrEmpty(tooltip))
                            ToolTipService.SetToolTip(cell.Content as FrameworkElement, tooltip);
                        else
                            ToolTipService.SetToolTip(cell.Content as FrameworkElement, null);
                    }
                }
            }));
        }
        //internal void ClearTooltip(object dataItem)
        //{
        //    var dataRow = this.DataControl.GetRowForItem(dataItem);
        //    var cell = dataRow.GetCell(this);
        //    if (cell != null)
        //    {
        //        if (cell.Content != null)
        //            ToolTipService.SetToolTip(cell.Content as FrameworkElement, null);
        //    }
        //}

        //internal void SetColor(InfoColor color)
        //{
        //    this.Background = UIManager.GetColorFromInfoColor(color);
        //}
        internal void ClearColor()
        {
            this.Background = null;
        }
        internal void SetBorderColor(object dataItem, InfoColor color)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                var dataRow = this.DataControl.GetRowForItem(dataItem);
                var cell = dataRow.GetCell(this);
                if (cell != null)
                {
                    if (cell.Content != null)
                    {
                        cell.BorderBrush = UIManager.GetColorFromInfoColor(color);
                        cell.BorderThickness = new Thickness(1);
                    }
                }
            }));
        }
        internal void SetBackgroundColor(object dataItem, InfoColor color)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                var dataRow = this.DataControl.GetRowForItem(dataItem);
                var cell = dataRow.GetCell(this);
                if (cell != null)
                {
                    if (cell.Content != null)
                    {
                        cell.Background = UIManager.GetColorFromInfoColor(color);
                    }
                }
            }));
        }
        internal void SetForegroundColor(object dataItem, InfoColor color)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                var dataRow = this.DataControl.GetRowForItem(dataItem);
                var cell = dataRow.GetCell(this);
                if (cell != null)
                {
                    if (cell.Content != null)
                    {
                        cell.Foreground = UIManager.GetColorFromInfoColor(color);
                    }
                }
            }));
        }
        //internal void ClearColor(object dataItem)
        //{
        //    var dataRow = this.DataControl.GetRowForItem(dataItem);
        //    var cell = dataRow.GetCell(this);
        //    if (cell != null)
        //    {
        //        if (cell.Content != null)
        //        {
        //            (cell.Content as Grid).Background = null;// new SolidColorBrush(UIManager.GetColorFromInfoColor(InfoColor.Black));
        //                                                     // (cell.Content as Control).BorderThickness = new Thickness(1);
        //        }
        //    }
        //}
        internal object GetValue(object dataItem)
        {
            //var dataRow = GetDataRow(dataGrid, dataItem);
            //var dataRow = this.DataControl.GetRowForItem(dataItem);

            //if (dataRow != null)
            //{
            //    var cell = dataRow.GetCell(this);

            //    if (cell != null)
            //    {
            //        //////return (cell.Tag as UIControlPackage).BaseControlHelper.GetValue( cell.Tag as UIControlPackage);

            //    }
            //}
            //return "";


            var dataRow = this.DataControl.GetRowForItem(dataItem);
            var cell = dataRow.GetCell(this);
            if (cell.Content != null)
                return (cell.Tag as I_ControlHelper).GetValue();
            return "";

        }

        //internal void DisableEnableCell(object dataItem, bool enable)
        //{
        //    var dataRow = this.DataControl.GetRowForItem(dataItem);

        //    if (dataRow != null)
        //    {
        //        var cell = dataRow.GetCell(this);

        //        if (cell != null)
        //        {
        //            cell.IsEnabled = enable;

        //        }
        //    }

        //}
        //internal void SetReadonlyCell(object dataItem, bool isreadonly)
        //{
        //    var dataRow = this.DataControl.GetRowForItem(dataItem);

        //    if (dataRow != null)
        //    {
        //        var cell = dataRow.GetCell(this);

        //        if (cell != null)
        //        {//درست شود
        //            cell.IsEnabled = !isreadonly;

        //        }
        //    }

        //}



        //public IAG_View_TemporaryView GenerateTemporaryView()
        //{
        //    throw new NotImplementedException();
        //}


        //public TemporaryLinkType LinkType
        //{
        //    set;
        //    get;
        //}

        //public void OnTemporaryViewRequested(object sender, Arg_TemporaryDisplayViewRequested arg)
        //{
        //    throw new NotImplementedException();
        //}
    }



}
