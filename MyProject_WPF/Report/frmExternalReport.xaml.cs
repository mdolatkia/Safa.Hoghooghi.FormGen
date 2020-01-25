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
    /// Interaction logic for frmEntityListReport.xaml
    /// </summary>
    /// 

    public partial class frmExternalReport : UserControl
    {
        EntityExternalReportDTO Message { set; get; }
        BizEntityExternalReport bizEntityExternalReport = new BizEntityExternalReport();
        int EntityID { set; get; }
        public frmExternalReport(int entityID, int entityListReportID)
        {
            InitializeComponent();
            EntityID = entityID;
            SetEntityPreDefinedSearchList();
            if (entityListReportID != 0)
            {
                GetEntityExternalReport(entityListReportID);
            }
            else
            {
                Message = new EntityExternalReportDTO();
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
            view.EntityPreDefinedSearchUpdated += (sender1, e1) => View_EntityPreDefinedSearchUpdated(sender1, e1, lookup);
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "تنظیمات نامه", Enum_WindowSize.Maximized);
        }

        private void View_EntityPreDefinedSearchUpdated(object sender, EntityPreDefinedSearchUpdatedArg e, MyStaticLookup lookup)
        {
            SetEntityPreDefinedSearchList();
            lookup.SelectedValue = e.ID;
        }
        private void ShowMessage()
        {
            txtReportName.Text = Message.ReportTitle;
            txtURL.Text = Message.URL;
            lokSearchRepository.SelectedValue = Message.SearchRepositoryID;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (txtReportName.Text == "")
            {
                MessageBox.Show("عنوان مناسب تعریف نشده است");
                return;
            }
            if (txtURL.Text == "")
            {
                MessageBox.Show("آدرس مناسب تعریف نشده است");
                return;
            }
            if (lokSearchRepository.SelectedItem != null)
                Message.SearchRepositoryID = (int)lokSearchRepository.SelectedValue;
            else
                Message.SearchRepositoryID = 0;
            Message.TableDrivedEntityID = EntityID;
            Message.ReportTitle = txtReportName.Text;
            Message.URL = txtURL.Text;
            bizEntityExternalReport.UpdateEntityExternalReports(Message);
            MessageBox.Show("اطلاعات ثبت شد");
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Message = new EntityExternalReportDTO();
            ShowMessage();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            frmExternalReportSelect view = new MyProject_WPF.frmExternalReportSelect(EntityID);
            view.EntityExternalReportSelected += View_EntityListReportSelected1;
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "frmExternalReportSelect");
        }

        private void View_EntityListReportSelected1(object sender, EntityExternalReportSelectedArg e)
        {
            if (e.EntityExternalReportID != 0)
            {
                GetEntityExternalReport(e.EntityExternalReportID);
            }
        }

        private void GetEntityExternalReport(int entityListReportID)
        {
            Message = bizEntityExternalReport.GetEntityExternalReport(entityListReportID, true);
            ShowMessage();
        }
    }
}
