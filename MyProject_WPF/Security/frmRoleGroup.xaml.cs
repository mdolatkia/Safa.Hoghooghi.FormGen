using ModelEntites;
using MyModelManager;
using MySecurity;
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
    /// Interaction logic for frmEntityRoleSecurity.xaml
    /// </summary>
    public partial class frmRoleGroup : Window
    {
        public event EventHandler GroupSaved;
        RoleGroupDTO Message;
        BizRole bizRoleSecurity = new BizRole();
        //int roleGroupID { set; get; }
        public frmRoleGroup(int roleGroupID)
        {
            InitializeComponent();
            if (roleGroupID != 0)
                Message = bizRoleSecurity.GetRoleGroup(roleGroupID);
            else
                Message = new RoleGroupDTO();
            SetRoles();
            ShowMessage();
        }

        private void SetRoles()
        {
            BizRole bizRole = new BizRole();
            var rel = dtgRoles.Columns[0] as GridViewComboBoxColumn;
            rel.ItemsSource = bizRole.GetAllRoles();
            rel.DisplayMemberPath = "Name";
            rel.SelectedValueMemberPath = "ID";

        }

        private void ShowMessage()
        {
            if (Message != null)
            {
                dtgRoles.ItemsSource = Message;
                txtName.Text = Message.Name;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Message.Roles.Count == 0)
            {
                MessageBox.Show("نقشی انتخاب نشده است");
                return;
            }
            Message.Name = txtName.Text;
            bizRoleSecurity.UpdateRoleGroup(Message);
            if (GroupSaved != null)
                GroupSaved(this, null);
            MessageBox.Show("اطلاعات ثبت شد");
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
