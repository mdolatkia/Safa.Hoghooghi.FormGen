using ModelEntites;
using MyCommonWPFControls;

using MyModelManager;
using MyUILibrary.EntityArea;
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
    /// Interaction logic for frmEntityGridViewReport.xaml
    /// </summary>
    /// 

    public partial class frmEntityGridViewReport : UserControl
    {
        EntityGridViewReportDTO Message { set; get; }
        BizEntityGridViewReport bizEntityGridViewReport = new BizEntityGridViewReport();
        int EntityID { set; get; }
        public frmEntityGridViewReport(int entityID, int entityViewReportID)
        {
            InitializeComponent();
            EntityID = entityID;
            //  SetSubReports();
            //    SetSubReportRelationships();
            SetEntityPreDefinedSearchList();
            SetEntityListViews();
            if (entityViewReportID != 0)
            {
                GetEntityGridViewReport(entityViewReportID);
            }
            else
            {
                Message = new EntityGridViewReportDTO();
                ShowMessage();
            }

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
            view.EntityPreDefinedSearchUpdated +=(sender1,e1)=> View_EntityPreDefinedSearchUpdated(sender1,e1, lookup);
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "تنظیمات نامه",Enum_WindowSize.Maximized);
        }

        private void View_EntityPreDefinedSearchUpdated(object sender, EntityPreDefinedSearchUpdatedArg e, MyStaticLookup lookup)
        {
            SetEntityPreDefinedSearchList();
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
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "تنظیمات نامه");
        }
        private void View_EntityListViewUpdated(object sender, EntityListViewUpdatedArg e, MyStaticLookup lookup)
        {
            SetEntityListViews();
            lookup.SelectedValue = e.ID;
        }
        //private void SetSubReportRelationships()
        //{
        //    BizRelationship bizRelationship = new BizRelationship();
        //    BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        //    var entity = bizTableDrivedEntity.GetTableDrivedEntity(EntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithRelationships);
        //    var relatinships = entity.Relationships;
        //    var rel = dtgSubReports.Columns[2] as GridViewComboBoxColumn;
        //    rel.ItemsSource = relatinships;
        //    rel.DisplayMemberPath = "Alias";
        //    rel.SelectedValueMemberPath = "ID";
        //}

        //private void SetSubReports()
        //{

        //    var listGridViewReports = bizEntityGridViewReport.GetEntityGridViewReports();

        //    var rel = dtgSubReports.Columns[3] as GridViewComboBoxColumn;
        //    rel.ItemsSource = listGridViewReports;
        //    rel.DisplayMemberPath = "ReportTitle";
        //    rel.SelectedValueMemberPath = "ID";
        //}

        private void ShowMessage()
        {
            txtReportName.Text = Message.ReportTitle;
            lokEntityListView.SelectedValue = Message.DefaultEntityListViewID;
            lokSearchRepository.SelectedValue = Message.SearchRepositoryID;

            //  dtgSubReports.ItemsSource = Message.EntityGridViewReportSubs;
            //SetGridSearch();
        }

        //private void SetGridSearch()
        //{
        //    //grdSearch.Children.Clear();
        //    //grdSearch.Children.Add(new frmSearchEntityArea(EntityID, Message.EntitySearchID)
        //}

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
            Message.TableDrivedEntityID = EntityID;
            Message.ReportTitle = txtReportName.Text;
            Message.DefaultEntityListViewID = (int)lokEntityListView.SelectedValue;
           
            if (lokSearchRepository.SelectedItem != null)
                Message.SearchRepositoryID = (int)lokSearchRepository.SelectedValue;
            else
                Message.SearchRepositoryID = 0;
            bizEntityGridViewReport.UpdateEntityGridViewReports(Message);
            MessageBox.Show("اطلاعات ثبت شد");
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Message = new EntityGridViewReportDTO();
            ShowMessage();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            frmEntityGridViewReportSelect view = new MyProject_WPF.frmEntityGridViewReportSelect(EntityID);
            view.EntityGridViewReportSelected += View_EntityGridViewReportSelected1;
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "frmEntityGridViewReportSelect");
        }

        private void View_EntityGridViewReportSelected1(object sender, EntityGridViewReportSelectedArg e)
        {
            if (e.EntityGridViewReportID != 0)
            {
                GetEntityGridViewReport(e.EntityGridViewReportID);
            }
        }

        private void GetEntityGridViewReport(int entityGridViewReportID)
        {
            Message = bizEntityGridViewReport.GetEntityGridViewReport(MyProjectManager.GetMyProjectManager.GetRequester(), entityGridViewReportID, true);
            ShowMessage();
        }
        //SearchEntityArea SearchEntityArea;
        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    if (SearchEntityArea == null)
        //    {
        //        SearchEntityArea = new SearchEntityArea();
        //        var searchViewInitializer = new SearchEntityAreaInitializer();
        //        searchViewInitializer.EntityID = EntityID;
        //        SearchEntityArea.SetAreaInitializer(searchViewInitializer);
        //        SearchEntityArea.SearchDataDefined += SearchEntityArea_SearchDataDefined;
        //    }
        //    MyProjectManager.GetMyProjectManager.ShowDialog(SearchEntityArea.SearchView, "جستجو");
        //}

        private void SearchEntityArea_SearchDataDefined(object sender, SearchDataArg e)
        {
            throw new NotImplementedException();
        }
    }
}
