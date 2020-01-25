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
    /// Interaction logic for frmEntityCrosstabReport.xaml
    /// </summary>
    /// 

    public partial class frmEntityCrosstabReport: UserControl
    {
        private EntityCrosstabReportDTO Message { set; get; }
        BizEntityCrosstabReport bizEntityCrosstabReport = new BizEntityCrosstabReport();
        int EntityID { set; get; }
        public int SelectedEntityCrosstabReportID
        {
            get
            {
                if (Message == null)
                    return 0;
                else
                    return Message.ID;
            }
        }
        public frmEntityCrosstabReport(int entityID, int CrosstabReportID)
        {
            InitializeComponent();
            Message = new ModelEntites.EntityCrosstabReportDTO();
            EntityID = entityID;
            SetEntityListViews();
            SetEntityPreDefinedSearchList();
            SetFunctoinTypes();
            if (CrosstabReportID != 0)
            {
                GetEntityCrosstabReport(CrosstabReportID);
            }
            else
            {
                Message = new EntityCrosstabReportDTO();
                ShowMessage();
            }
            lokEntityListView.SelectionChanged += LokEntityListView_SelectionChanged;

            ControlHelper.GenerateContextMenu(dtgColumns);
            ControlHelper.GenerateContextMenu(dtgRows);
            ControlHelper.GenerateContextMenu(dtgValues);
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
                SetRowColumns(listView.EntityListViewAllColumns);
                SetColumnColumns(listView.EntityListViewAllColumns);
                SetValuesColumns(listView.EntityListViewAllColumns);
            }
        }
        private void SetColumnColumns(List<EntityListViewColumnsDTO> columns)
        {
            colColumnColumns.DisplayMemberPath = "Alias";
            colColumnColumns.SelectedValueMemberPath = "ID";
            colColumnColumns.ItemsSource = columns;
        }

        private void SetValuesColumns(List<EntityListViewColumnsDTO> columns)
        {
            colValueColumns.DisplayMemberPath = "Alias";
            colValueColumns.SelectedValueMemberPath = "ID";
            colValueColumns.ItemsSource = columns;
        }


        private void SetRowColumns(List<EntityListViewColumnsDTO> columns)
        {
            colRowColumns.DisplayMemberPath = "Alias";
            colRowColumns.SelectedValueMemberPath = "ID";
            colRowColumns.ItemsSource = columns;
        }


        private void SetFunctoinTypes()
        {
            var listFunctoins = bizEntityCrosstabReport.GetFunctionTypes();

            var rel = dtgValues.Columns[1] as GridViewComboBoxColumn;
            rel.ItemsSource = listFunctoins;
            //rel.DisplayMemberPath = "Name";
            //rel.SelectedValueMemberPath = "Value";
        }

    
        private void GetEntityCrosstabReport(int EntityCrosstabReportID)
        {
            Message = bizEntityCrosstabReport.GetEntityCrosstabReport(MyProjectManager.GetMyProjectManager.GetRequester(), EntityCrosstabReportID, true);
            ShowMessage();
        }
        private void ShowMessage()
        {
            lokEntityListView.SelectedValue = Message.EntityListViewID;

            txtTitle.Text = Message.ReportTitle;
            dtgColumns.ItemsSource = Message.Columns;
            dtgRows.ItemsSource = Message.Rows;
            dtgValues.ItemsSource = Message.Values;
            lokSearchRepository.SelectedValue = Message.SearchRepositoryID;

        }



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
         
            if (Message.Columns.Count == 0)
            {
                MessageBox.Show("ستون تعریف نشده است");
                return;
            }
            if (Message.Rows.Count == 0)
            {
                MessageBox.Show("ردیف تعریف نشده است");
                return;
            }
            if (Message.Values.Count == 0)
            {
                MessageBox.Show("مقدار تعریف نشده است");
                return;
            }
            Message.EntityListViewID = (int)lokEntityListView.SelectedValue;
            if (lokSearchRepository.SelectedItem != null)
                Message.SearchRepositoryID = (int)lokSearchRepository.SelectedValue;
            else
                Message.SearchRepositoryID = 0;
            Message.TableDrivedEntityID = EntityID;
            Message.ReportTitle = txtTitle.Text;
            Message.ID = bizEntityCrosstabReport.UpdateEntityCrosstabReports(Message);
            MessageBox.Show("اطلاعات ثبت شد");
        }



        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Message = new EntityCrosstabReportDTO();
            ShowMessage();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            frmEntityCrosstabReportSelect view = new MyProject_WPF.frmEntityCrosstabReportSelect(EntityID);
            view.EntityCrosstabReportSelected += View_EntityCrosstabReportSelected1;
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "frmEntityCrosstabReportSelect");
        }

        private void View_EntityCrosstabReportSelected1(object sender, EntityCrosstabReportSelectedArg e)
        {
            if (e.EntityCrosstabReportID != 0)
            {
                GetEntityCrosstabReport(e.EntityCrosstabReportID);
            }
        }


    }
}
