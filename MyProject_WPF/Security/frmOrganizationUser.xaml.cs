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
    /// Interaction logic for frmOrganizations.xaml
    /// </summary>
    public partial class frmOrganizationUser : UserControl
    {

        public frmOrganizationUser()
        {
            InitializeComponent();

        }
        BizOrganization bizOrganization = new BizOrganization();
        BizUser bizUser = new BizUser();
        OrganizationUserDTO organizationUser { set; get; }
        UserDTO selectedUser { set; get; }
        OrganizationDTO selectedOrganization { set; get; }
        private void ucOrganizationList_OrganizationSelected(object sender, OrganizationSelectedArg e)
        {

            selectedOrganization = e.Organization;
            List<UserDTO> userList = bizUser.GetUserByOrganization(selectedOrganization.ID);
            dtgUsersByOrganization.ItemsSource = userList;
            ClearRoles();
        }

        private void ucUserList_UserSelected(object sender, UserSelectedArg e)
        {

            selectedUser = e.User;
            var OrganizationList = bizOrganization.GetOrganizationsByUser(selectedUser.ID);
            dtgOrganizationsByUser.ItemsSource = OrganizationList;
            ClearRoles();
        }



        private void btnAddOrganizationToUser_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            frmOrganizationList frm = new frmOrganizationList();
            frm.OrganizationSelected += (sender1, e1) => frm_OrganizationSelected(sender1, e1, window);
            window.Content = frm;
            window.Title = "انتخاب نقش";
            window.ShowDialog();
        }

        void frm_OrganizationSelected(object sender, OrganizationSelectedArg e, Window window)
        {
            window.Close();

            bizOrganization.AddUserToOrganization(e.Organization.ID, selectedUser.ID);
            var OrganizationList = bizOrganization.GetOrganizationsByUser(selectedUser.ID);
            dtgOrganizationsByUser.ItemsSource = OrganizationList;

        }

        private void btnAddUserToOrganization_Click(object sender, RoutedEventArgs e)
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

            var organizationUserID = bizOrganization.AddUserToOrganization(selectedOrganization.ID, e.User.ID);


            List<UserDTO> userList = bizUser.GetUserByOrganization(selectedOrganization.ID);
            dtgUsersByOrganization.ItemsSource = userList;
        }


        private void dtgUsersByOrganization_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            selectedUser = dtgUsersByOrganization.SelectedItem as UserDTO;
            ShowRoles();
        }


        private void dtgOrganizationsByUser_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            selectedOrganization = dtgOrganizationsByUser.SelectedItem as OrganizationDTO;
            ShowRoles();
        }

        private void btnAddRoleToOraganizationUser_Click(object sender, RoutedEventArgs e)
        {if(organizationUser==null)
            {
                MessageBox.Show("کاربر و سازمان مرتبط انتخاب نشده اند");
                return;
            }
            Window window = new Window();
            frmRoleList frm = new frmRoleList();
            frm.RoleSelected += (sender1, e1) => frm_RoleSelected(sender1, e1, window);
            window.Content = frm;
            window.Title = "انتخاب کاربر";
            window.ShowDialog();
        }
        void frm_RoleSelected(object sender, RoleSelectedArg e, Window window)
        {
            window.Close();
            bizOrganization.AddRoleToOrganizationUser(organizationUser.ID, e.Role.ID);
            ShowRoles();
        }
        private void ShowRoles()
        {
            organizationUser = bizOrganization.GetOrganizationsUser(selectedOrganization.ID, selectedUser.ID);
            var roleList = bizOrganization.GetRolesByOrganizationUserID(organizationUser.ID);
            if (tabOrganizationUser.IsSelected)
                dtgRoleOrganizationUser.ItemsSource = roleList;
            else if (tabUserOrganization.IsSelected)
                dtgRoleUserOrganization.ItemsSource = roleList;
        }
        private void ClearRoles()
        {
            if (tabOrganizationUser.IsSelected)
                dtgRoleOrganizationUser.ItemsSource = null;
            else if (tabUserOrganization.IsSelected)
                dtgRoleUserOrganization.ItemsSource = null;
        }

        //void ucOrganizationList_OrganizationSelected(object sender, OrganizationSelectedArg e)
        //{
        //    ucOrganizationEdit.ShowOrganization(e.Organization);
        //}

        //void ucOrganizationEdit_OrganizationSaved(object sender, EventArgs e)
        //{
        //    ucOrganizationList.ShowOrganizations();
        //}
    }
}
