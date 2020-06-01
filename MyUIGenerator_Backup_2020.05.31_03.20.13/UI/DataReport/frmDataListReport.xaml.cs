using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams.Primitives;
using ModelEntites;
using System.Windows.Threading;
using MyUILibraryInterfaces.DataReportArea;
using ProxyLibrary;
using System.Collections.ObjectModel;

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for frmDataView.xaml
    /// </summary>
    public partial class frmDataListReport : UserControl, I_View_DataListReportArea
    {
        //DispatcherTimer timer = new DispatcherTimer();
        public frmDataListReport()
        {
            InitializeComponent();

            this.Loaded += FrmDataView_Loaded;

            cmbListViews.SelectionChanged += CmbListViews_SelectionChanged;
            cmbOrderColumns.SelectionChanged += CmbOrderColumns_SelectionChanged;
            cmbSort.SelectionChanged += CmbSort_SelectionChanged;

            dtgReport.MouseDoubleClick += DtgReport_MouseDoubleClick;
            //timer.Tick += Timer_Tick;

        }

        private void DtgReport_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtgReport.SelectedItem != null)
            {
                if (DataItemDoubleClicked != null)
                    DataItemDoubleClicked(this, new DataItemDoubleClickedArg() { DataObject = dtgReport.SelectedItem as MyDataObject });
            }
        }

        private void CmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckOrderChange();
        }

        private void CmbOrderColumns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckOrderChange();
        }

        private void CheckOrderChange()
        {
            if (cmbOrderColumns.SelectedItem != null && cmbOrderColumns.SelectedValue != null)
            {
                if (cmbSort.SelectedItem != null)
                {
                    if (OrderColumnsChanged != null)
                        OrderColumnsChanged(this, null);
                }
            }
        }

        private void CmbListViews_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbListViews.SelectedItem != null && cmbListViews.SelectedValue != null)
            {
                if (EntityListViewChanged != null)
                    EntityListViewChanged(this, new EntitiListViewChangedArg() { ListViewID = (int)cmbListViews.SelectedValue });
            }
        }



        bool _loaded = false;
        private void FrmDataView_Loaded(object sender, RoutedEventArgs e)
        {

            //if (!_loaded)
            //    DrawItems();
            //_loaded = true;
        }
        List<DP_DataRepository> DataViewItems;

        public string Title
        {
            set
            {
                lblTitle.Text = value;
            }
        }

        public bool SearchAreaCommandVisibility
        {
            get
            {
                return imgSearch.Visibility == Visibility.Visible;
            }

            set
            {
                imgSearch.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public int GetOrderColumnID
        {
            get
            {
                if (cmbOrderColumns.SelectedItem != null && cmbOrderColumns.SelectedValue != null)
                    return (int)cmbOrderColumns.SelectedValue;
                return 0;
            }
        }

        public int GetEntityListViewID()
        {
            if (cmbListViews.SelectedItem != null && cmbListViews.SelectedValue != null)
                return (int)cmbListViews.SelectedValue;
            return 0;
        }

        public string GetSortText
        {
            get
            {
                if (cmbSort.SelectedItem != null)
                    return cmbSort.SelectedItem.ToString();
                return "";
            }
        }

        public event EventHandler<EntitiListViewChangedArg> EntityListViewChanged;
        public event EventHandler SearchCommandRequested;
        public event EventHandler OrderColumnsChanged;
        public event EventHandler<DataItemDoubleClickedArg> DataItemDoubleClicked;

        public void SetEntityListViews(List<EntityListViewDTO> entitiyListViews, int defaultEntityListViewID)
        {
            cmbListViews.ItemsSource = entitiyListViews;
            cmbListViews.DisplayMemberPath = "Title";
            cmbListViews.SelectedValuePath = "ID";
            cmbListViews.SelectedValue = defaultEntityListViewID;
        }

        public void ClearDataViewItemInfo()
        {
            //grdInfo.Children.Clear();
            //grdInfo.RowDefinitions.Clear();
        }




        private void imgSearch_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SearchCommandRequested != null)
                SearchCommandRequested(this, null);
        }

        private void imgReferesh_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            //diagram.ResetViewport();
        }





        public void SetOrderColumns(List<ColumnDTO> columns)
        {

        }

        public void SetOrderColumns(List<Tuple<int, string>> columns)
        {
            cmbOrderColumns.SelectedValuePath = "Item1";
            cmbOrderColumns.DisplayMemberPath = "Item2";
            cmbOrderColumns.ItemsSource = columns;
        }

        public void SetOrderSorts(List<string> list)
        {
            cmbSort.ItemsSource = list;
            cmbSort.SelectedItem = list.FirstOrDefault();
        }

        public void SetDataItemsCount(int count, string tooltip)
        {
            lblCount.Text = "تعداد نمایش : " + count.ToString();
            if (!string.IsNullOrEmpty(tooltip))
                ToolTipService.SetToolTip(lblCount, tooltip);
        }

        public void SetItemsTotalCount(int resultCount)
        {
            lblTotalCount.Text = "تعداد کل : " + resultCount.ToString();
        }

        public void AddDataReportItems(List<MyDataObject> items)
        {

            dtgReport.ItemsSource = items;

        }











        public void BringIntoView(MyDataObject defaultDataReportItem)
        {
        }

        public void ShowDataViewItemMenus(MyDataObject dataObject, List<DataReportMenu> menus)
        {

            RadRadialMenu menu = new RadRadialMenu();
            //menu.ShowEventName = "MouseEnter";
            //menu.HideEventName = "MouseLeave";
            foreach (var item in menus)
            {
                AddMenu(menu.Items, item);
            }
            menu.PopupPlacement = System.Windows.Controls.Primitives.PlacementMode.Center;
            //(dataViewItem as UC_DataViewItem).SetMenu(menu);
            RadRadialMenu.SetRadialContextMenu(this, menu);
            RadialMenuCommands.Show.Execute(null, this);
            menu.PopupPlacement = System.Windows.Controls.Primitives.PlacementMode.Center;
            menu.IsOpen = true;
            menu.PopupPlacement = System.Windows.Controls.Primitives.PlacementMode.Center;
            //menu.PopupHorizontalOffset = 115;
            //menu.PopupVerticalOffset = 5;

        }

        private void AddMenu(ObservableCollection<RadRadialMenuItem> items, DataReportMenu item)
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
        private void SubMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e, DataReportMenu dataViewMenu)
        {
            dataViewMenu.OnMenuClicked();
        }
    }
}
