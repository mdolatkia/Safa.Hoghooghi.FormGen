using MyModelManager;

using ProxyLibrary.Workflow;
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
    /// InterActivity logic for frmAddSelectActivity.xaml
    /// </summary>
    public partial class frmAddActivity : UserControl
    {
        int ProcessID { set; get; }
        BizActivity bizActivity = new BizActivity();
        BizTarget bizTarget = new BizTarget();
        BizRoleType bizRoleType = new BizRoleType();
        BizProcess bizProcess = new BizProcess();
        ActivityDTO Message = new ActivityDTO();
        public event EventHandler<SavedItemArg> ItemSaved;
        public frmAddActivity(int processID, int ActivityID)
        {
            InitializeComponent();
            ProcessID = processID;
            dtgActivityTargets.SelectionChanged += DtgActivityTargets_SelectionChanged;
            SetBaseData();
            if (ActivityID != 0)
                GetActivity(ActivityID);
            else
            {
                Message = new ActivityDTO();
                ShowMessage();
            }
            ShowTargetRoleTab(null);
            ControlHelper.GenerateContextMenu(dtgActivityTargets);
            ControlHelper.GenerateContextMenu(dtgTargetRoles);
            dtgActivityTargets.CellEditEnded += DtgActivityTargets_CellEditEnded;
        }

        private void DtgActivityTargets_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            ShowTargetRoleTab(e.Cell.DataContext as ActivityTargetDTO);
        }

        private void DtgActivityTargets_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            ShowTargetRoleTab(dtgActivityTargets.SelectedItem as ActivityTargetDTO);
        }

        private void ShowTargetRoleTab(ActivityTargetDTO item)
        {

            tabRoles.Visibility = Visibility.Collapsed;
            dtgTargetRoles.ItemsSource = null;
            if (item != null)
            {
                if (item.TargetType == TargetType.RoleMembers)
                {
                    tabRoles.Visibility = Visibility.Visible;
                    dtgTargetRoles.ItemsSource = item.RoleTypes;
                }
            }
        }

        private void SetBaseData()
        {
            //cmbType.DisplayMemberPath = "Name";
            //cmbType.SelectedValuePath = "ID";
            cmbType.ItemsSource = bizActivity.GetActivityTypes();

            var col7 = dtgActivityTargets.Columns[0] as GridViewComboBoxColumn;
            col7.ItemsSource = bizTarget.GetTargetTypes();
            //col7.DisplayMemberPath = "Name";
            //col7.SelectedValueMemberPath = "ID";

            ////var col8 = dtgTargetRoles.Columns[0] as GridViewComboBoxColumn;
            colRoleTypes.ItemsSource = bizRoleType.GetAllRoleTypes();
            colRoleTypes.DisplayMemberPath = "Name";
            colRoleTypes.SelectedValueMemberPath = "ID";
        }

        private void GetActivity(int ActivityID)
        {
            Message = bizActivity.GetActivity(ActivityID, true);
            ShowMessage();
        }

        private void ShowMessage()
        {
            //if (Message.ID != 0)
            //    ProcessID = Message.ProcessID;
            txtName.Text = Message.Name;
            txtDescription.Text = Message.Description;
            cmbType.SelectedItem = Message.ActivityType;
            dtgActivityTargets.ItemsSource = Message.Targets;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("نام وارد نشده است");
                return;
            }
            if (cmbType.SelectedItem == null)
            {
                MessageBox.Show("نوع وارد نشده است");
                return;
            }
            Message.ProcessID = ProcessID;
            Message.Name = txtName.Text;
            Message.Description = txtDescription.Text;
            Message.ActivityType = (ActivityType)cmbType.SelectedItem;
            Message.ID = bizActivity.UpdateActivitys(Message);
            MessageBox.Show("با موفقیت ثبت شد");
            if (ItemSaved != null)
                ItemSaved(this, new SavedItemArg() { ID = Message.ID });
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Message = new ActivityDTO();
            ShowMessage();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }

}
