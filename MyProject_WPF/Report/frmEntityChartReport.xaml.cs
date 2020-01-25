using ModelEntites;
using MyCommonWPFControls;
using MyModelManager;
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
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmEntityChartReport.xaml
    /// </summary>
    /// 

    public partial class frmEntityChartReport : UserControl
    {
        BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();

        private EntityChartReportDTO Message { set; get; }
        BizEntityChartReport bizEntityChartReport = new BizEntityChartReport();
        int EntityID { set; get; }
        public int SelectedEntityChartReportID
        {
            get
            {
                if (Message == null)
                    return 0;
                else
                    return Message.ID;
            }
        }
        public frmEntityChartReport(int entityID, int chartReportID)
        {
            InitializeComponent();
            Message = new ModelEntites.EntityChartReportDTO();
            EntityID = entityID;
            //  SetValueColumns();
            SetEntityListViews();
            SetSearchRepositoryList();
            SetChartTypes();
            SetFunctoinTypes();
            SetSeriesArrangeTypes();
            if (chartReportID != 0)
            {
                GetEntityChartReport(chartReportID);
            }
            else
            {
                Message = new EntityChartReportDTO();
                ShowMessage();
            }
            lokEntityListView.SelectionChanged += LokEntityListView_SelectionChanged;


            cmbChartType.SelectionChanged += CmbChartType_SelectionChanged;

            ControlHelper.GenerateContextMenu(dtgCategories);
            ControlHelper.GenerateContextMenu(dtgSeries);
            ControlHelper.GenerateContextMenu(dtgValues);
        }
        private void SetSearchRepositoryList()
        {
            if (lokSearchRepository.ItemsSource == null)
            {
                lokSearchRepository.EditItemClicked += LokEntityPreDefined_EditItemClicked; ;
            }
            BizSearchRepository biz = new BizSearchRepository();
            lokSearchRepository.DisplayMember = "Title";
            lokSearchRepository.SelectedValueMember = "ID";
            lokSearchRepository.ItemsSource = biz.GetSearchRepositories(EntityID);
        }
        private void LokEntityPreDefined_EditItemClicked(object sender, EditItemClickEventArg e)
        {
            var lookup = (sender as MyStaticLookup);
            frmSearchRepository view;

            if (lookup.SelectedItem == null)
            {
                view = new frmSearchRepository(EntityID, 0);
            }
            else
            {
                view = new frmSearchRepository(EntityID, (int)lookup.SelectedValue);
            }
            view.EntityPreDefinedSearchUpdated += (sender1, e1) => View_EntityPreDefinedSearchUpdated(sender1, e1, lookup);
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "تنظیمات نامه", Enum_WindowSize.Maximized);
        }

        private void View_EntityPreDefinedSearchUpdated(object sender, EntityPreDefinedSearchUpdatedArg e, MyStaticLookup lookup)
        {
            SetSearchRepositoryList();
            lookup.SelectedValue = e.ID;
        }
        private void SetEntityListViews()
        {
            if (lokEntityListView.ItemsSource == null)
            {
                lokEntityListView.EditItemClicked += LokEntityListView_EditItemClicked;
            }
            BizEntityListView biz = new BizEntityListView();
            lokEntityListView.DisplayMember = "Title";
            lokEntityListView.SelectedValueMember = "ID";
            lokEntityListView.ItemsSource = biz.GetEntityListViews(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID);
        }
        private void LokEntityListView_EditItemClicked(object sender, MyCommonWPFControls.EditItemClickEventArg e)
        {
            var lookup = (sender as MyStaticLookup);
            frmEntityListView view;
            if (lookup.SelectedItem == null)
            {
                view = new frmEntityListView(EntityID, 0);
            }
            else
            {
                view = new frmEntityListView(EntityID, (int)lookup.SelectedValue);
            }
            view.EntityListViewUpdated += (sender1, e1) => View_EntityListViewUpdated(sender1, e1, lookup);
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "تنظیمات نامه", Enum_WindowSize.Big);
        }
        private void View_EntityListViewUpdated(object sender, EntityListViewUpdatedArg e, MyStaticLookup lookup)
        {
            SetEntityListViews();
            lookup.SelectedValue = e.ID;
        }
        private void LokEntityListView_SelectionChanged(object sender, SelectionChangedArg e)
        {
            if (lokEntityListView.SelectedItem != null)
            {
                BizEntityListView biz = new BizEntityListView();
                var listView = biz.GetEntityListView(MyProjectManager.GetMyProjectManager.GetRequester(), (int)lokEntityListView.SelectedValue);
                SetCategoryColumns(listView.EntityListViewAllColumns);
                SetSeriesColumns(listView.EntityListViewAllColumns);
                SetValuesColumns(listView.EntityListViewAllColumns);
            }
        }
        //private void SetValueColumns()
        //{
        //    BizTableDrivedEntity biz = new BizTableDrivedEntity();
        //    var entity = biz.GetTableDrivedEntity(EntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
        //    colValue.DisplayMemberPath = "Name";
        //    colValue.SelectedValueMemberPath = "ID";
        //    colValue.ItemsSource = entity.Columns; ;
        //}



        private void SetSeriesColumns(List<EntityListViewColumnsDTO> columns)
        {
            colSeriesColumns.DisplayMemberPath = "Alias";
            colSeriesColumns.SelectedValueMemberPath = "ID";
            colSeriesColumns.ItemsSource = columns;

        }

        private void SetValuesColumns(List<EntityListViewColumnsDTO> columns)
        {
            colValue.DisplayMemberPath = "Alias";
            colValue.SelectedValueMemberPath = "ID";
            colValue.ItemsSource = columns;
        }


        private void SetCategoryColumns(List<EntityListViewColumnsDTO> columns)
        {
            colCategoryColumns.DisplayMemberPath = "Alias";
            colCategoryColumns.SelectedValueMemberPath = "ID";
            colCategoryColumns.ItemsSource = columns;

        }



        private void CmbChartType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var type = (ChartType)cmbChartType.SelectedItem;
            if (type == ChartType.Column)
            {
                rowCategort1.Height = new GridLength(20);
                rowCategort2.Height = new GridLength(1, GridUnitType.Star);

                lblCategory.Text = "دسته بندی اصلی";
                lblSerie.Text = "دسته بندی فرعی";
                dtgSeries.Columns[1].IsVisible = true;
            }
            else if (type == ChartType.Pie)
            {
                rowCategort1.Height = new GridLength(0);
                rowCategort2.Height = new GridLength(0);
                lblSerie.Text = "دسته بندی اصلی";
                dtgSeries.Columns[1].IsVisible = false;
            }
            else if (type == ChartType.Line)
            {
                rowCategort1.Height = new GridLength(20);
                rowCategort2.Height = new GridLength(1, GridUnitType.Star);
                lblCategory.Text = "دسته بندی اصلی";
                lblSerie.Text = "دسته بندی فرعی";
                dtgSeries.Columns[1].IsVisible = false;
            }
            else if (type == ChartType.Radar)
            {
                rowCategort1.Height = new GridLength(20);
                rowCategort2.Height = new GridLength(1, GridUnitType.Star);
                lblCategory.Text = "دسته بندی اصلی";
                lblSerie.Text = "دسته بندی فرعی";
                dtgSeries.Columns[1].IsVisible = false;
            }
        }

        private void SetChartTypes()
        {
            var listTypes = bizEntityChartReport.GetChartTypes();
            cmbChartType.ItemsSource = listTypes;
        }

        private void SetSeriesArrangeTypes()
        {
            var listTypes = bizEntityChartReport.GetSeriesArrangeTypes();

            //var rel = dtgSeries.Columns[1] as GridViewComboBoxColumn;
            colArrangeType.ItemsSource = listTypes;
        }

        private void SetFunctoinTypes()
        {
            var listFunctoins = bizEntityChartReport.GetFunctionTypes();

            //    var rel = dtgValues.Columns[2] as GridViewComboBoxColumn;
            colFunctionType.ItemsSource = listFunctoins;
            //rel.DisplayMemberPath = "Name";
            //rel.SelectedValueMemberPath = "Value";
        }

        private void GetEntityChartReport(int chartReportID)
        {
            Message = bizEntityChartReport.GetEntityChartReport(MyProjectManager.GetMyProjectManager.GetRequester(), chartReportID, true);
            ShowMessage();
        }



        //public void SetEntityID(int entityID)
        //{
        //    EntityID = entityID;


        //}
        private void GetEntityEntityChartReport(int EntityChartReportID)
        {
            Message = bizEntityChartReport.GetEntityChartReport(MyProjectManager.GetMyProjectManager.GetRequester(), EntityChartReportID, true);
            ShowMessage();
        }
        private void ShowMessage()
        {
            txtTitle.Text = Message.ReportTitle;
            dtgSeries.ItemsSource = Message.EntityChartReportSeries;
            lokEntityListView.SelectedValue = Message.EntityListViewID;
            dtgCategories.ItemsSource = Message.EntityChartReportCategories;
            dtgValues.ItemsSource = Message.EntityChartReportValues;
            cmbChartType.SelectedItem = Message.ChartType;
            lokSearchRepository.SelectedValue = Message.SearchRepositoryID;


        }

        //private void DtgColumns_Drop(object sender, DragEventArgs e)
        //{
        //    if (e.Data.GetDataPresent(typeof(TreeViewItem)))
        //    {
        //        var sourceNode = (TreeViewItem)e.Data.GetData(typeof(TreeViewItem));
        //        var target = e.Source as DependencyObject;
        //        while (target != null && !(target is RadGridView))
        //            target = VisualTreeHelper.GetParent(target);

        //        var gridView = target as RadGridView;
        //        if (sourceNode != null && gridView != null)
        //        {
        //            CloneTreeNode(sourceNode, gridView);

        //        }

        //    }
        //}

        //private void CloneTreeNode(TreeViewItem sourceNode, RadGridView gridView)
        //{
        //    if (sourceNode.DataContext is TreeColumnItem)
        //    {
        //        var treeColumn = sourceNode.DataContext as TreeColumnItem;
        //        if (gridView == dtgSeries)
        //        {
        //            var reportColumn = new EntityChartReportSerieDTO();
        //            BizColumn bizColumn = new BizColumn();
        //            reportColumn.ColumnName = treeColumn.ColumnName;
        //            reportColumn.ColumnID = treeColumn.ColumnID;
        //            reportColumn.RelationshipPath = treeColumn.RelationshipPath;
        //            Message.EntityChartReportSeries.Add(reportColumn);
        //            dtgSeries.ItemsSource = null;
        //            dtgSeries.ItemsSource = Message.EntityChartReportSeries;
        //        }
        //        else if (gridView == dtgCategories)
        //        {
        //            var reportColumn = new EntityChartReportCategoryDTO();
        //            BizColumn bizColumn = new BizColumn();
        //            reportColumn.ColumnName = treeColumn.ColumnName;
        //            reportColumn.ColumnID = treeColumn.ColumnID;
        //            reportColumn.RelationshipPath = treeColumn.RelationshipPath;
        //            Message.EntityChartReportCategories.Add(reportColumn);
        //            dtgCategories.ItemsSource = null;
        //            dtgCategories.ItemsSource = Message.EntityChartReportCategories;
        //        }
        //        else if (gridView == dtgValues)
        //        {
        //            var reportColumn = new EntityChartReportValueDTO();
        //            BizColumn bizColumn = new BizColumn();
        //            reportColumn.ColumnName = treeColumn.ColumnName;
        //            reportColumn.ColumnID = treeColumn.ColumnID;
        //            reportColumn.RelationshipPath = treeColumn.RelationshipPath;
        //            Message.EntityChartReportValues.Add(reportColumn);
        //            dtgValues.ItemsSource = null;
        //            dtgValues.ItemsSource = Message.EntityChartReportValues;
        //        }
        //    }
        //}

        //private void DtgColumns_DragOver(object sender, DragEventArgs e)
        //{
        //    if (e.Data.GetDataPresent(typeof(TreeViewItem)))
        //    {
        //        e.Effects = DragDropEffects.Copy;
        //    }
        //    else
        //    {
        //        e.Effects = DragDropEffects.None;
        //    }
        //}

        //bool _isDragging = false;
        //private void DtgColumns_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (e.LeftButton != MouseButtonState.Pressed)
        //        _isDragging = false;
        //    if (!_isDragging && e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        if (columnTree.SelectedItem != null)
        //        {
        //            _isDragging = true;
        //            DragDrop.DoDragDrop(columnTree.TreeItems, columnTree.SelectedItem,
        //                DragDropEffects.Copy);
        //        }
        //    }
        //}


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (lokEntityListView.SelectedItem == null)
            {
                MessageBox.Show("لطفا ابتدا لیست ستونها را ثبت و انتخاب نمایید");
                return;
            }
            if (txtTitle.Text == "")
            {
                MessageBox.Show("عنوان مناسب تعریف نشده است");
                return;
            }
            if ((ChartType)cmbChartType.SelectedItem == ChartType.Column)
            {
                if (Message.EntityChartReportCategories.Count == 0)
                {
                    MessageBox.Show("دسته بندی اصلی تعریف نشده است");
                    return;
                }
                if (Message.EntityChartReportCategories.Count > 1)
                {
                    MessageBox.Show("فقط یک مورد دسته بندی اصلی میتوان تعریف نمود");
                    return;
                }
            }
            else if ((ChartType)cmbChartType.SelectedItem == ChartType.Pie)
            {
                if (Message.EntityChartReportSeries.Count == 0)
                {
                    MessageBox.Show("دسته بندی اصلی تعریف نشده است");
                    return;
                }
            }
            if (Message.EntityChartReportValues.Count == 0)
            {
                MessageBox.Show("فیلد مقداری تعریف نشده است");
                return;
            }
            Message.EntityListViewID = (int)lokEntityListView.SelectedValue;
            if (lokSearchRepository.SelectedItem != null)
                Message.SearchRepositoryID = (int)lokSearchRepository.SelectedValue;
            else
                Message.SearchRepositoryID = 0;
         Message.ChartType = (ChartType)cmbChartType.SelectedItem;
            Message.TableDrivedEntityID = EntityID;
            Message.ReportTitle = txtTitle.Text;
            Message.ID = bizEntityChartReport.UpdateEntityChartReports(Message);
            MessageBox.Show("اطلاعات ثبت شد");
        }



        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Message = new EntityChartReportDTO();
            ShowMessage();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            frmEntityChartReportSelect view = new MyProject_WPF.frmEntityChartReportSelect(EntityID);
            view.EntityChartReportSelected += View_EntityChartReportSelected1;
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "frmEntityChartReportSelect");
        }

        private void View_EntityChartReportSelected1(object sender, EntityChartReportSelectedArg e)
        {
            if (e.EntityChartReportID != 0)
            {
                GetEntityEntityChartReport(e.EntityChartReportID);
            }
        }


    }
}
