using ModelEntites;
using MyCommonWPFControls;

using MyModelManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Telerik.Windows.Controls.GridView;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmEntityListReport.xaml
    /// </summary>
    /// 

    public partial class frmEntityListReport : UserControl
    {
        BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
        EntityListReportDTO Message { set; get; }
        BizEntityListReport bizEntityListReport = new BizEntityListReport();
        int EntityID { set; get; }
        public frmEntityListReport(int entityID, int entityListReportID)
        {
            InitializeComponent();
            EntityID = entityID;
            //    SetSubReports();
            SetEntityPreDefinedSearchList();

            SetSubReportRelationshipTails();
            SetEntityListViews();
            lokEntityListView.SelectionChanged += LokEntityListView_SelectionChanged;
            if (entityListReportID != 0)
            {
                GetEntityEntityListReport(entityListReportID);
            }
            else
            {
                Message = new EntityListReportDTO();
                ShowMessage();
            }
            ControlHelper.GenerateContextMenu(dtgSubReports);
            ControlHelper.GenerateContextMenu(dtgGroups);
            dtgSubReports.CellEditEnded += DtgSubReports_CellEditEnded;
            dtgSubReports.RowLoaded += DtgSubReports_RowLoaded;
        }
        private void SetEntityPreDefinedSearchList()
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
            SetEntityPreDefinedSearchList();
            lookup.SelectedValue = e.ID;
        }
        private void LokEntityListView_SelectionChanged(object sender, SelectionChangedArg e)
        {
            if (lokEntityListView.SelectedItem != null)
            {
                BizEntityListView biz = new BizEntityListView();
                var listView = biz.GetEntityListView(MyProjectManager.GetMyProjectManager.GetRequester(), (int)lokEntityListView.SelectedValue);
                SetGroupColumns(listView.EntityListViewAllColumns);
            }
        }
        private void SetGroupColumns(List<EntityListViewColumnsDTO> columns)
        {
            var rel = dtgGroups.Columns[0] as GridViewComboBoxColumn;
            rel.DisplayMemberPath = "Column.Alias";
            rel.SelectedValueMemberPath = "ID";
            rel.ItemsSource = columns;
        }
        private void DtgSubReports_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (e.DataElement is EntityListReportSubsDTO)
            {
                var data = (e.DataElement as EntityListReportSubsDTO);
                if (data.vwListReports == null || data.vwListReports.Count == 0)
                    SetSubListReports(data);

                SetSubColumns(e.Row);
            }

        }

        private void SetSubColumns(GridViewRowItem row)
        {
            if (row.DataContext is EntityListReportSubsDTO)
            {
                var data = (row.DataContext as EntityListReportSubsDTO);
                var subsColumnsText = "";
                if (data.SubsColumnsDTO.Any())
                {
                    foreach (var item in data.SubsColumnsDTO)
                    {
                        subsColumnsText += (subsColumnsText == "" ? "" : ",") + item.ParentEntityListViewColumnAlias + "=" + item.ChildEntityListViewColumnAlias;
                    }
                }
                var col = row.Cells.FirstOrDefault(x => x.Column.Name == "colSubColumns");
                var button = new Button();
                button.Click += (sender1, e1) => buttonClick(sender1, e1, row, data);
                if (subsColumnsText == "")
                    subsColumnsText = "تعریف ستونها";
                button.Content = subsColumnsText;
                col.Content = button;
            }
        }



        private void buttonClick(object sender1, RoutedEventArgs e1, GridViewRowItem row, EntityListReportSubsDTO data)
        {
            if (lokEntityListView.SelectedItem == null)
            {
                MessageBox.Show("لیست نمایشی اصلی انتخاب نشده است");
                return;
            }
            if (data.EntityListReportID == 0)
            {
                MessageBox.Show("زیر گزارش انتخاب نشده است");
                return;
            }
            var childListView = bizEntityListReport.GetEntityListReport(MyProjectManager.GetMyProjectManager.GetRequester(), data.EntityListReportID, true);
            frmEntitySubListReportColumns frm = new MyProject_WPF.frmEntitySubListReportColumns(data.SubsColumnsDTO, (int)lokEntityListView.SelectedValue, childListView.EntityListViewID);
            var dialog = MyProjectManager.GetMyProjectManager.ShowDialog(frm, "frmEntitySubListReportColumns", Enum_WindowSize.Big);
            dialog.Closed += (sender, e) => Dialog_Closed(sender, e, row);
        }

        private void Dialog_Closed(object sender, WindowClosedEventArgs e, GridViewRowItem row)
        {
            SetSubColumns(row);
        }

        private void DtgSubReports_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            if (e.Cell.Column == colRelationshipTail)
            {
                if (e.Cell.DataContext is EntityListReportSubsDTO)
                {
                    var subReport = (e.Cell.DataContext as EntityListReportSubsDTO);
                    SetSubListReports(subReport);
                }
            }
        }
        private void SetSubListReports(EntityListReportSubsDTO subReport)
        {
            if (subReport.EntityRelationshipTailID != 0)
            {
                colSubListReport.DisplayMemberPath = "ReportTitle";
                colSubListReport.SelectedValueMemberPath = "ID";

                var relationshipTail = bizEntityRelationshipTail.GetEntityRelationshipTail(MyProjectManager.GetMyProjectManager.GetRequester(), subReport.EntityRelationshipTailID);
                var listReports = bizEntityListReport.GetEntityListReports(MyProjectManager.GetMyProjectManager.GetRequester(), relationshipTail.TargetEntityID);
                ObservableCollection<EntityListReportDTO> list = new ObservableCollection<EntityListReportDTO>();
                listReports.ForEach(x => list.Add(x));
                subReport.vwListReports = listReports;
                //   colSubListReport.ItemsSourceBinding = new Binding("vwListReports");
            }
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
        private void SetSubReportRelationshipTails()
        {
            BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
            BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
            //var entity = bizTableDrivedEntity.GetTableDrivedEntity(EntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithRelationships);
            var relatinships = bizEntityRelationshipTail.GetEntityRelationshipTails(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID);
            colRelationshipTail.ItemsSource = relatinships;
            colRelationshipTail.DisplayMemberPath = "EntityPath";
            colRelationshipTail.SelectedValueMemberPath = "ID";
        }

        //private void SetSubReports()
        //{

        //    var listListReports = bizEntityListReport.GetEntityListReports();

        //    var rel = dtgSubReports.Columns[3] as GridViewComboBoxColumn;
        //    rel.ItemsSource = listListReports;
        //    rel.DisplayMemberPath = "ReportTitle";
        //    rel.SelectedValueMemberPath = "ID";
        //}

        private void ShowMessage()
        {
            txtReportName.Text = Message.ReportTitle;
            dtgSubReports.ItemsSource = Message.EntityListReportSubs;
            dtgGroups.ItemsSource = Message.ReportGroups;
            lokEntityListView.SelectedValue = Message.EntityListViewID;
            lokSearchRepository.SelectedValue = Message.SearchRepositoryID;

            foreach (var item in Message.EntityListReportSubs)
                SetSubListReports(item);
        }



        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (lokEntityListView.SelectedItem == null)
            {
                MessageBox.Show("لطفا ابتدا لیست ستونها را ثبت و انتخاب نمایید");
                return;
            }
            if (txtReportName.Text == "")
            {
                MessageBox.Show("عنوان مناسب تعریف نشده است");
                return;
            }
            if (lokSearchRepository.SelectedItem != null)
                Message.SearchRepositoryID = (int)lokSearchRepository.SelectedValue;
            else
                Message.SearchRepositoryID = 0;

            Message.TableDrivedEntityID = EntityID;
            Message.ReportTitle = txtReportName.Text;
            Message.EntityListViewID = (int)lokEntityListView.SelectedValue;
            bizEntityListReport.UpdateEntityListReports(Message);
            MessageBox.Show("اطلاعات ثبت شد");
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Message = new EntityListReportDTO();
            ShowMessage();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            frmEntityListReportSelect view = new MyProject_WPF.frmEntityListReportSelect(EntityID);
            view.EntityListReportSelected += View_EntityListReportSelected1;
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "frmEntityListReportSelect");
        }

        private void View_EntityListReportSelected1(object sender, EntityListReportSelectedArg e)
        {
            if (e.EntityListReportID != 0)
            {
                GetEntityEntityListReport(e.EntityListReportID);
            }
        }

        private void GetEntityEntityListReport(int entityListReportID)
        {
            Message = bizEntityListReport.GetEntityListReport(MyProjectManager.GetMyProjectManager.GetRequester(), entityListReportID, true);
            ShowMessage();
        }
    }
}
