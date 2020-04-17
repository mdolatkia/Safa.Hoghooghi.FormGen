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
using ProxyLibrary;
using MyUIGenerator.UIControlHelper;
using MyUILibraryInterfaces.DataViewArea;
using MyUILibrary.EntityArea;

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for frmGridView.xaml
    /// </summary>
    public partial class frmGridView : View_MultipleDataContainer, I_View_GridViewArea
    {
        public frmGridView()
        {
            InitializeComponent();
            grdArea.Children.Add(dataGridHelper.dataGrid);
            dataGridHelper.dataGrid.MouseDoubleClick += DataGrid_MouseDoubleClick;
            cmbListViews.SelectionChanged += CmbListViews_SelectionChanged;
            cmbOrderColumns.SelectionChanged += CmbOrderColumns_SelectionChanged;
            cmbSort.SelectionChanged += CmbSort_SelectionChanged;
        }
        public object UIElement { get { return this; } }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as RadGridView).SelectedItem != null)
            {
                if (InfoClicked != null)
                {
                    InfoClicked(this, new DataGridSelectedArg() { DataView = (sender as RadGridView).SelectedItem, UIElement = (sender as RadGridView).GetRowForItem((sender as RadGridView).SelectedItem) });
                }
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





        //public string Title
        //{
        //    set
        //    {
        //        lblTitle.Text = value;
        //    }
        //}

        //public bool SearchAreaCommandVisibility
        //{
        //    get
        //    {
        //        return imgSearch.Visibility == Visibility.Visible;
        //    }

        //    set
        //    {
        //        imgSearch.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
        //    }
        //}

        public int GetOrderColumnID
        {
            get
            {
                if (cmbOrderColumns.SelectedItem != null && cmbOrderColumns.SelectedValue != null)
                    return (int)cmbOrderColumns.SelectedValue;
                return 0;
            }
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
        public event EventHandler ReportRequested;
        public event EventHandler<DataGridSelectedArg> InfoClicked;

        public void AddGridViewItems(List<DP_DataView> items)
        {

        }

        public void SetEntityListViews(List<EntityListViewDTO> entitiyListViews, int defaultEntityListViewID)
        {
            cmbListViews.ItemsSource = entitiyListViews;
            cmbListViews.DisplayMemberPath = "Title";
            cmbListViews.SelectedValuePath = "ID";
            cmbListViews.SelectedValue = defaultEntityListViewID;
        }




        private void imgSearch_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SearchCommandRequested != null)
                SearchCommandRequested(this, null);
        }

        private void imgReferesh_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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

        private void imgReport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ReportRequested != null)
                ReportRequested(this, null);
        }

        //public void AddGenerealSearchAreaView(object view)
        //{
        //    grdSearch.Children.Add(view as UIElement);
        //}
    }
}
