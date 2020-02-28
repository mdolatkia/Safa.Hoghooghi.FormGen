
using ModelEntites;
using MyUIGenerator;
using MyUIGenerator.UIControlHelper;
using MyUIGenerator.View;
using MyUILibrary;
using MyUILibrary.EntityArea;
using MyUILibrary.Temp;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace MyUIGenerator.UIControlHelper
{
    public class DataGridViewColumn : Telerik.Windows.Controls.GridViewColumn
    {
        public event EventHandler<Arg_MultipleTemporaryDisplayLoaded> TemporaryViewLoaded;
        public event EventHandler FocusLost;

        public event EventHandler<Arg_MultipleTemporaryDisplayViewRequested> TemporaryViewRequested;
        public event EventHandler<Arg_TemporaryDisplaySerachText> TemporaryViewSerchTextChanged;
        // DataMaster.EntityDefinition.ND_Type_Property TypeProperty { set; get; }
        //public ColumnDTO Column { set; get; }
        //public ColumnUISettingDTO ColumnSetting { set; get; }

        //I_View_EditEntityAreaDataView ViewEditNDTypeArea { set; get; }
        //public event EventHandler<Arg_TemporaryDisplayViewRequested> TemporaryViewRequested;
        //public event EventHandler<Arg_TemporaryDisplayViewRequested> TemporarySearchViewRequested;
        //I_View_DataDependentControl TemporaryArg { set; get; }

        //public DataGridTextColumn(IAG_View_TemporaryDisplayView temporaryViewLink)
        //{
        //    // TypeProperty = correspondingTypeProperty;

        //    TemporaryViewLink = temporaryViewLink;
        //}
        TemporaryLinkState TemporaryLinkState { set; get; }
        //    TemporaryLinkType LinkType { set; get; }
        RelationshipUISettingDTO RelationshipSetting { set; get; }
        public DataGridViewColumn(TemporaryLinkState temporaryLinkState, RelationshipUISettingDTO relationshipSetting)
        {
            //ColumnSetting = columnSetting;
            //   LinkType = linkType;
            RelationshipSetting = relationshipSetting;
            TemporaryLinkState = temporaryLinkState;
            // TypeProperty = correspondingTypeProperty;
            //   Column = column;
            //UnSetValue = null;
            //this.Loaded += DataGridTextColumn_Loaded;
        }


        //void DataGridTextColumn_Loaded(object sender, RoutedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}
        List<Tuple<object, string>> dataItems = new List<Tuple<object, string>>();
        //string UnSetValue { set; get; }
        public override FrameworkElement CreateCellElement(GridViewCell cell, object dataItem)
        {
            var control = GenerateTemporaryView(dataItem);
            //control.Column = Column;
            control.SearchTextChanged += Control_SearchTextChanged1;
            control.TemporaryDisplayViewRequested += (sender, e) => control_TemporaryDisplayViewRequested(sender, e, dataItem);
            control.FocusLost += Control_FocusLost;
            return control as FrameworkElement;

        }

        private void Control_FocusLost(object sender, EventArgs e)
        {
            if (FocusLost != null)
                FocusLost(sender, e);
        }

        private void Control_SearchTextChanged1(object sender, Arg_TemporaryDisplaySerachText e)
        {
            if (TemporaryViewSerchTextChanged != null)
            {
                TemporaryViewSerchTextChanged(sender, new Arg_TemporaryDisplaySerachText() { Text = e.Text });

            }
        }

        void control_TemporaryDisplayViewRequested(object sender, Arg_TemporaryDisplayViewRequested e, object dataItem)
        {
            if (TemporaryViewRequested != null)
            {
                TemporaryViewRequested(sender, new Arg_MultipleTemporaryDisplayViewRequested() { LinkType = e.LinkType, DataItem = dataItem });

            }
            //OnTemporaryViewRequested(sender, e);
        }


        public override FrameworkElement CreateCellEditElement(GridViewCell cell, object dataItem)
        {
            //cell.Loaded += cell_Loaded;

            var control = GenerateTemporaryView(dataItem);
            //control.ParentDataItem = dataItem as object;
            //control.Column = Column;
            control.TemporaryDisplayViewRequested += (sender, e) => control_TemporaryDisplayViewRequested(sender, e, dataItem);
            control.SearchTextChanged += Control_SearchTextChanged;
            control.FocusLost += Control_FocusLost;

            return control as FrameworkElement;

        }

        private void Control_SearchTextChanged(object sender, Arg_TemporaryDisplaySerachText e)
        {

        }

        //////void cell_Loaded(object sender, RoutedEventArgs e)
        //////{
        //////    var cell = (e.Source as GridViewCell);
        //////    var dataItem = cell.DataContext;
        //////    if (dataItem != null)
        //////    {
        //////        var cellItem = dataItems.Where(x => x.Item1 == dataItem).FirstOrDefault();
        //////        if (cellItem != null)
        //////        {
        //////            ControlHelper.SetValue(Column, cell.Tag as UIControlPackage, cellItem.Item2);
        //////        }
        //////    }

        //////}



        //internal bool SetValue(object dataItem, string value, ColumnSetting columnSetting)
        //{
        //    //UnSetValue = null;
        //    //var dataRow = GetDataRow(dataGrid, dataItem);

        //    var dataRow = this.DataControl.GetRowForItem(dataItem);
        //    //  var dataRow = this.DataControl.ItemContainerGenerator.ContainerFromItem(dataItem) as GridViewRow;
        //    var cell = dataRow.GetCell(this);
        //    if (dataRow != null)
        //    {
        //        if (cell != null)
        //        {
        //            return ControlHelper.SetValue(Column, cell.Tag as UIControlPackage, value, columnSetting);
        //        }
        //    }
        //    else
        //    {
        //        if (dataItems.Any(x => x.Item1 == dataItem))
        //        {
        //            var fitem = dataItems.First(x => x.Item1 == dataItem);
        //            dataItems.Remove(fitem);
        //        }
        //        dataItems.Add(new Tuple<object, string>(dataItem, value));
        //        return true;
        //    }

        //    //dataItems.Add(new Tuple<object, string>(dataItem, value));

        //    //var cellItem = dataItems.Where(x => x.Item1 == dataItem).FirstOrDefault();
        //    //if (cellItem != null)
        //    //{
        //    //    ControlHelper.SetValue(Column, control, cellItem.Item2);
        //    //}
        //    //else
        //    //    dataItems.Add(new Tuple<object, string>(dataItem, value));
        //    return true;

        //}
        //internal string GetValue(object dataItem)
        //{
        //    //var dataRow = GetDataRow(dataGrid, dataItem);
        //    var dataRow = this.DataControl.GetRowForItem(dataItem);

        //    if (dataRow != null)
        //    {
        //        var cell = dataRow.GetCell(this);

        //        if (cell != null)
        //        {
        //            return ControlHelper.GetValue(Column, cell.Tag as UIControlPackage);

        //        }
        //    }
        //    return "";
        //}


        //public IAG_View_TemporaryView GenerateTemporaryView()
        //{
        //    throw new NotImplementedException();
        //}

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
                if (!string.IsNullOrEmpty(tooltip))
                    ToolTipService.SetToolTip(cell.Content as FrameworkElement, tooltip);
                else
                    ToolTipService.SetToolTip(cell.Content as FrameworkElement, null);
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
        //            ToolTipService.SetToolTip(cell.Content as Control, null);
        //    }
        //}

        //internal void SetColor(InfoColor color)
        //{
        //    this.Background = UIManager.GetColorFromInfoColor(color);
        //}
        //internal void ClearColor()
        //{
        //    this.Background = null;
        //}
        //internal void SetColor(object dataItem, InfoColor color)
        //{
        //    var dataRow = this.DataControl.GetRowForItem(dataItem);
        //    var cell = dataRow.GetCell(this);
        //    if (cell != null)
        //    {
        //        if (cell.Content != null)
        //        {
        //            (cell.Content as Control).BorderBrush = UIManager.GetColorFromInfoColor(color);
        //            (cell.Content as Control).BorderThickness = new Thickness(1);
        //        }
        //    }
        //}
        //internal void ClearColor(object dataItem)
        //{
        //    var dataRow = this.DataControl.GetRowForItem(dataItem);
        //    var cell = dataRow.GetCell(this);
        //    if (cell != null)
        //    {
        //        if (cell.Content != null)
        //        {
        //            (cell.Content as Control).BorderBrush = null;// new SolidColorBrush(UIManager.GetColorFromInfoColor(InfoColor.Black));
        //            (cell.Content as Control).BorderThickness = new Thickness(1);
        //        }
        //    }
        //}
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




        //public TemporaryLinkType LinkType
        //{
        //    set;
        //    get;
        //}

        //public void OnTemporaryViewRequested(object sender, Arg_TemporaryDisplayViewRequested arg)
        //{
        //    throw new NotImplementedException();
        //}

        public I_View_TemporaryView GenerateTemporaryView(object dataItem)
        {
            UC_TemporaryDataSearchLink view = new UC_TemporaryDataSearchLink(TemporaryLinkState);
            if (TemporaryViewLoaded != null)
                this.TemporaryViewLoaded(this, new Arg_MultipleTemporaryDisplayLoaded() { DataItem = dataItem });
            return view;
        }
        public void SetTemporaryViewText(object dataItem, string text)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                var dataRow = this.DataControl.GetRowForItem(dataItem);
                var cell = dataRow.GetCell(this);
                if (cell != null && cell.Content != null)
                {
                    if (cell.Content is I_View_TemporaryView)
                        (cell.Content as I_View_TemporaryView).SetLinkText(text);
                }
            }));
        }
        public void EnableDisable( bool enable)
        {
            this.IsEnabled = enable;
        }
        public void EnableDisable(object dataItem, bool enable)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                var dataRow = this.DataControl.GetRowForItem(dataItem);
                var cell = dataRow.GetCell(this);
                if (cell != null && cell.Content != null)
                {
                    if (cell.Content is I_View_TemporaryView)
                        (cell.Content as I_View_TemporaryView).DisableEnable(enable);
                }
            }));
        }
        public void DisableEnable(object dataItem, TemporaryLinkType link, bool enable)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                var dataRow = this.DataControl.GetRowForItem(dataItem);
                var cell = dataRow.GetCell(this);
                if (cell != null && cell.Content != null)
                {
                    if (cell.Content is I_View_TemporaryView)
                        (cell.Content as I_View_TemporaryView).DisableEnable(link, enable);
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
                var cell = dataRow.GetCell(this);
                if (cell != null && cell.Content != null)
                {
                    if (cell.Content is FrameworkElement)
                        (cell.Content as FrameworkElement).Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
                }
            }));
        }

        internal void SetQuickSearchVisibility(object dataItem, bool visible)
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                var dataRow = this.DataControl.GetRowForItem(dataItem);
                var cell = dataRow.GetCell(this);
                if (cell != null && cell.Content != null)
                {
                    if (cell.Content is I_View_TemporaryView)
                        (cell.Content as I_View_TemporaryView).QuickSearchVisibility = visible;
                }
            }));
        }
        public I_View_TemporaryView GetTemporaryView(object dataItem)
        {
            var dataRow = this.DataControl.GetRowForItem(dataItem);
            var cell = dataRow.GetCell(this);
            if (cell != null && cell.Content != null)
            {
                return (cell.Content as I_View_TemporaryView);
            }
            return null;
        }

        //public void OnTemporaryViewRequested(object sender, Arg_TemporaryDisplayViewRequested arg)
        //{

        //}

        //public TemporaryLinkType LinkType
        //{
        //    set;
        //    get;
        //}
    }



}
