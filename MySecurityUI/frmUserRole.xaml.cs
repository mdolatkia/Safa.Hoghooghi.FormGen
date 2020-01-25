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

namespace MySecurityUI
{
    /// <summary>
    /// Interaction logic for frmRoles.xaml
    /// </summary>
    public partial class frmUserRole : UserControl
    {
        public frmUserRole()
        {
            InitializeComponent();

        }
        UserDTO selectedUser { set; get; }
        RoleDTO selectedRole { set; get; }
        private void ucRoleList_RoleSelected(object sender, RoleSelectedArg e)
        {
            BizUser bizUser = new BizUser();
            selectedRole = e.Role;
            List<UserDTO> userList = bizUser.GetUserByRole(selectedRole.ID);
            dtgUsersByRole.ItemsSource = userList;
        }

        private void ucUserList_UserSelected(object sender, UserSelectedArg e)
        {
            BizRole bizRole = new BizRole();
            selectedUser = e.User;
            var roleList = bizRole.GetRolesByUser(selectedUser.ID);
            dtgRolesByUser.ItemsSource = roleList;
        }

        private void btnAddRoleToUser_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            frmRoleList frm = new frmRoleList();
            frm.RoleSelected += (sender1, e1) => frm_RoleSelected(sender1, e1, window);
            window.Content = frm;
            window.Title = "انتخاب نقش";
            window.ShowDialog();
        }

        void frm_RoleSelected(object sender, RoleSelectedArg e, Window window)
        {
            window.Close();
            BizRole bizRole = new BizRole();
            bizRole.AddUserToRole(e.Role.ID, selectedUser.ID);
            var roleList = bizRole.GetRolesByUser(selectedUser.ID);
            dtgRolesByUser.ItemsSource = roleList;

        }

        private void btnAddUserToRole_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            frmUserList frm = new frmUserList();
            frm.UserSelected += (sender1, e1) => frm_UserSelected(sender1, e1, window);
            window.Content = frm;
            window.Title = "انتخاب کاربر";
            window.ShowDialog();

        }

        void frm_UserSelected(object sender, UserSelectedArg e, Window window)
        {
            window.Close();
            BizRole bizRole = new BizRole();
            bizRole.AddUserToRole(selectedRole.ID, e.User.ID);

            BizUser bizUser = new BizUser();
            List<UserDTO> userList = bizUser.GetUserByRole(selectedRole.ID);
            dtgUsersByRole.ItemsSource = userList;
        }

        //void ucRoleList_RoleSelected(object sender, RoleSelectedArg e)
        //{
        //    ucRoleEdit.ShowRole(e.Role);
        //}

        //void ucRoleEdit_RoleSaved(object sender, EventArgs e)
        //{
        //    ucRoleList.ShowRoles();
        //}
    }
}
