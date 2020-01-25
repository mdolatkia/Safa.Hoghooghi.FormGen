using ModelEntites;

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

    public partial class frmEntityListReportGrouped: UserControl
    {
        EntityListReportGroupedDTO Message { set; get; }
        BizEntityListReport bizEntityListReport = new BizEntityListReport();
        BizEntityListReportGrouped bizEntityListReportGrouped = new BizEntityListReportGrouped();
        int EntityID { set; get; }
        public frmEntityListReportGrouped(int entityID, int entityListReportGroupedID)
        {
            InitializeComponent();
            EntityID = entityID;
            SetListReports();
            lokListReports.SelectionChanged += LokListReports_SelectionChanged;
            //frmEntityListView.SetEntityID(EntityID);
            if (entityListReportGroupedID != 0)
            {
                GetEntityListReportGrouped(entityListReportGroupedID);
            }
            else
            {
                Message = new EntityListReportGroupedDTO();
                ShowMessage();
            }
            ControlHelper.GenerateContextMenu(dtgGroups);
        }

        private void SetListReports()
        {
            lokListReports.SelectedValueMember = "ID";
            lokListReports.DisplayMember = "ReportTitle";
            lokListReports.ItemsSource = bizEntityListReport.GetEntityListReports(EntityID);
        }

        private void LokListReports_SelectionChanged(object sender, MyCommonWPFControls.SelectionChangedArg e)
        {
            if (e.SelectedItem != null)
            {

                var listReport = e.SelectedItem as EntityListReportDTO;
                if (listReport != null)
                {
                    var fullListReport = bizEntityListReport.GetEntityListReport(listReport.ID, true);
                    SetGroupColumns(fullListReport.EntityListView.EntityListViewAllColumns);
                }
            }
        }

        private void SetGroupColumns(List<EntityListViewColumnsDTO> columns)
        {
            var rel = dtgGroups.Columns[0] as GridViewComboBoxColumn;

            rel.DisplayMemberPath = "Column.Alias";
            rel.SelectedValueMemberPath = "ID";
            rel.ItemsSource = columns;
        }


        private void ShowMessage()
        {
            txtReportName.Text = Message.ReportTitle;
            lokListReports.SelectedValue = Message.EntityListReportID;
            dtgGroups.ItemsSource = Message.ReportGroups;

        }



        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (lokListReports.SelectedItem == null)
            {
                MessageBox.Show("لطفا گزارش لیست پایه را انتخاب نمایید");
                return;
            }
            if (txtReportName.Text == "")
            {
                MessageBox.Show("عنوان مناسب تعریف نشده است");
                return;
            }
            Message.TableDrivedEntityID = EntityID;
            Message.ReportTitle = txtReportName.Text;
            Message.EntityListReportID = (int)lokListReports.SelectedValue;
            bizEntityListReportGrouped.UpdateEntityListReportGroupeds(Message);
            MessageBox.Show("اطلاعات ثبت شد");
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager().CloseDialog(this);
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Message = new EntityListReportGroupedDTO();
            ShowMessage();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            frmEntityListReportGroupedSelect view = new MyProject_WPF.frmEntityListReportGroupedSelect(EntityID);
            view.EntityListReportGroupedSelected += View_EntityListReportSelected1;
            MyProjectManager.GetMyProjectManager().ShowDialog(view, "frmEntityListReportGroupedSelect");
        }

        private void View_EntityListReportSelected1(object sender, EntityListReportGroupedSelectedArg e)
        {
            if (e.EntityListReportGroupedID != 0)
            {
                GetEntityListReportGrouped(e.EntityListReportGroupedID);
            }
        }

        private void GetEntityListReportGrouped(int ID)
        {
            Message = bizEntityListReportGrouped.GetEntityListReportGrouped(ID, true);
            ShowMessage();
        }
    }
}
