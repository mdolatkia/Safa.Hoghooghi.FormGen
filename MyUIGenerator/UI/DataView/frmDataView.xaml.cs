using MyUILibraryInterfaces.DataViewArea;
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

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for frmDataView.xaml
    /// </summary>
    public partial class frmDataView : UserControl, I_View_DataViewArea
    {
        DispatcherTimer timer = new DispatcherTimer();
        public frmDataView()
        {
            InitializeComponent();
            BackgroundGrid.SetIsGridVisible(diagram, false);
            this.Loaded += FrmDataView_Loaded;
            diagram.SelectionChanged += Diagram_SelectionChanged;
            cmbListViews.SelectionChanged += CmbListViews_SelectionChanged;
            cmbOrderColumns.SelectionChanged += CmbOrderColumns_SelectionChanged;
            cmbSort.SelectionChanged += CmbSort_SelectionChanged;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Tick += Timer_Tick;
            //      diagram.Visibility = Visibility.Collapsed;
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

        private void Diagram_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null)
                if (e.AddedItems.Count == 1)
                {
                    var item = e.AddedItems[0] as I_DataViewItem;
                    if (item != null)
                    {
                        item.OnSelected();
                    }

                }
        }

        bool _loaded = false;
        private void FrmDataView_Loaded(object sender, RoutedEventArgs e)
        {

            if (!_loaded)
                DrawItems();
            _loaded = true;
        }
        List<I_DataViewItem> DataViewItems;

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
        //   public event EventHandler ReportRequested;

        private void DrawItems()
        {
            double columnMarginWidth = 30;
            double rowMarginHeight = 50;
            diagram.Items.Clear();
            if (DataViewItems != null)
                if (DataViewItems.Count > 0)
                {
                    double width = this.ActualWidth;// diagram.ActualWidth;
                    double itemWidth = (DataViewItems[0] as UserControl).Width;
                    double columnWidth = itemWidth + columnMarginWidth;
                    int columnsCount = (int)(width / columnWidth);
                    //double columnWidth = width / columnsCount;
                    double rowHeight = (DataViewItems[0] as UserControl).Height + rowMarginHeight;
                    int index = 0;
                    foreach (var item in DataViewItems)
                    {
                        diagram.Items.Add(item);
                        var column = index % columnsCount;
                        var row = index / columnsCount;
                        var ltrPosition = ((column * columnWidth) + columnMarginWidth / 2);
                        var xPosition = width - (ltrPosition + itemWidth);
                        var yPosition = row * rowHeight;
                        var shape = (diagram.Items[diagram.Items.Count - 1] as UserControl).Parent as RadDiagramShape;
                        shape.Position = new Point(xPosition, yPosition);
                        shape.IsDraggingEnabled = false;
                        shape.IsConnectorsManipulationEnabled = false;
                        shape.IsRotationEnabled = false;
                        shape.IsResizingEnabled = false;
                        index++;
                        shape.MouseDoubleClick += Shape_MouseDoubleClick;

                    }
                }
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();
            //   diagram.AutoFit();
        }
        private void Shape_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var shape = sender as RadDiagramShape;
            diagram.BringIntoView(new Rect(new Point(shape.X, shape.Y), new Size(shape.ActualWidth, shape.ActualHeight)));
        }


        public void AddDataViewItems(List<I_DataViewItem> items)
        {
            DataViewItems = items;
            if (_loaded)
                DrawItems();
        }

        public void SetEntityListViews(List<EntityListViewDTO> entitiyListViews, int defaultEntityListViewID)
        {
            cmbListViews.ItemsSource = entitiyListViews;
            cmbListViews.DisplayMemberPath = "Title";
            cmbListViews.SelectedValuePath = "ID";
            cmbListViews.SelectedValue = defaultEntityListViewID;
        }

        public void ClearDataViewItemInfo()
        {
            grdInfo.Children.Clear();
            grdInfo.RowDefinitions.Clear();
        }

        public void AddDataViewItemInfo(string name, object value)
        {
            var rowDef = new RowDefinition();
            rowDef.Height = GridLength.Auto;
            rowDef.MaxHeight = 25;
            grdInfo.RowDefinitions.Add(rowDef);
            var mod = grdInfo.RowDefinitions.Count % 2;
            SolidColorBrush backColor;
            if (mod == 0)
                backColor = new SolidColorBrush(Colors.Wheat);
            else
                backColor = new SolidColorBrush(Colors.LightBlue);

            var lblTitle = new TextBlock();
            lblTitle.Text = name;
            lblTitle.Background = backColor;
            lblTitle.HorizontalAlignment = HorizontalAlignment.Stretch;
            lblTitle.TextAlignment = TextAlignment.Right;
            lblTitle.Margin = new Thickness(0);

            var lblValue = new TextBlock();
            lblValue.Text = value == null ? "<Null>" : value.ToString();
            lblValue.Background = backColor;
            lblValue.HorizontalAlignment = HorizontalAlignment.Stretch;
            lblValue.TextAlignment = TextAlignment.Left;
            lblValue.Margin = new Thickness(0);

            grdInfo.Children.Add(lblTitle);
            grdInfo.Children.Add(lblValue);
            Grid.SetColumn(lblValue, 2);
            Grid.SetRow(lblTitle, grdInfo.RowDefinitions.Count - 1);
            Grid.SetRow(lblValue, grdInfo.RowDefinitions.Count - 1);
        }

        public void BringIntoView(I_DataViewItem defaultDataViewItem)
        {
            var shape = (defaultDataViewItem as UserControl).Parent as RadDiagramShape;
            if (shape != null)
                diagram.BringIntoView(new Rect(new Point(shape.X, shape.Y), new Size(shape.ActualWidth, shape.ActualHeight)));
        }

        //private void imgSearch_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (SearchCommandRequested != null)
        //        SearchCommandRequested(this, null);
        //}

        private void imgReferesh_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            diagram.AutoFit();
            //diagram.ResetViewport();
        }

        public void ShowDataViewItemInfo()
        {
            brdInfo.Visibility = Visibility.Visible;
        }

        public void HideDataViewItemInfo()
        {
            brdInfo.Visibility = Visibility.Collapsed;
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

        //private void imgReport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (ReportRequested != null)
        //        ReportRequested(this, null);
        //}
    }
}
