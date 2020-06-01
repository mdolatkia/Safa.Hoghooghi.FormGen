using MyUILibrary;
using ProxyLibrary;
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


namespace MyUIGenerator.Security
{
    /// <summary>
    /// Interaction logic for frmLogin.xaml
    /// </summary>
    public partial class frmSecuritySetting : Window, I_SecuritySetting
    {
        //public event EventHandler<AdminOrganizationChangedArg> AdmindOrganizationChanged;
        //public event EventHandler<AdminOrganizationConfirmedArg> AdminOrganizationsConfirmed;
        //public event EventHandler<AssignedOrganizationChangedArg> AssignedOrganizationChanged;
        //public event EventHandler<AssignedOrganizationConfirmedArg> AssignedOrganizationConfirmed;
        public event EventHandler<AdminOrganizationPostConfirmedArg> AdminOrganizationPostsConfirmed;
        public event EventHandler AssignedOrganizationPostsConfirmed;
        public event EventHandler<OrganizationPostSelectedArg> SearchedOrganizationPostSelected;
        public event EventHandler<OrganizationPostSearchArg> OrganizationPostSearchChanged;

        public frmSecuritySetting()
        {
            InitializeComponent();
            //cmbAssignedOrganization.SelectionChanged += CmbAssignedOrganization_SelectionChanged;
            //cmbAdminOrganization.SelectionChanged += CmbAdminOrganization_SelectionChanged;
        }

        //private void CmbAdminOrganization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var item = cmbAdminOrganization.SelectedItem as OrganizationDTO;
        //    if (item != null)
        //    {
        //        AdmindOrganizationChanged(this, new AdminOrganizationChangedArg() { Organization = item });
        //    }
        //}

        //private void CmbAssignedOrganization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var item = cmbAssignedOrganization.SelectedItem as OrganizationWithRolesDTO;
        //    if (item != null)
        //    {
        //        AssignedOrganizationChanged(this, new AssignedOrganizationChangedArg() { Organization = item });
        //    }
        //}

        public bool ShowAdminTab
        {
            set
            {
                tabAdmin.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public bool ShowByPassSecurityCheckBox
        {
            set
            {
                lblByPassSecurity.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                chkByPassSecurity.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public bool ByPassSecurityCheckBoxValue
        {
            set
            {
                chkByPassSecurity.IsChecked = value;
            }
            get
            {
                return chkByPassSecurity.IsChecked == true;
            }
        }


        //public void LoadAdminOrganizations(List<OrganizationDTO> organizations)
        //{
        //    cmbAdminOrganization.DisplayMemberPath = "Name";
        //    cmbAdminOrganization.SelectedValuePath = "ID";
        //    cmbAdminOrganization.ItemsSource = organizations;
        //}



        //public void LoadAssignedOrganizations(List<OrganizationWithRolesDTO> organizations)
        //{
        //    cmbAssignedOrganization.DisplayMemberPath = "Name";
        //    cmbAssignedOrganization.SelectedValuePath = "OrganizationID";
        //    cmbAssignedOrganization.ItemsSource = organizations;
        //}

        public void LoadAssignedOrganizationPosts(List<OrganizationPostDTO> organizationPosts)
        {
            lstAssignedRoles.DisplayMemberPath = "Name";
            lstAssignedRoles.SelectedValuePath = "ID";
            lstAssignedRoles.ItemsSource = organizationPosts;
        }

        //public void LoadMultiSelectAdminRoles(List<OrganizationPostDTO> roles)
        //{
        //    //lstAdminRoles.DisplayMemberPath = "Name";
        //    lstAdminRoles.SelectedValuePath = "ID";
        //    lstAdminRoles.ItemsSource = roles;
        //}

        //public void SelectAdminOrganizations(OrganizationDTO organization)
        //{
        //    cmbAdminOrganization.SelectedItem = organization;
        //}

        //public void SelectAssignedOrganizations(OrganizationWithRolesDTO organization)
        //{
        //    cmbAssignedOrganization.SelectedItem = organization;
        //}

        //public void ShowAssignedRoles(List<RoleDTO> roles)
        //{
        //    lstAssignedRoles.DisplayMemberPath = "Name";
        //    lstAssignedRoles.SelectedValuePath = "ID";
        //    lstAssignedRoles.ItemsSource = roles;
        //}

        private void btnConfirmAssignedOrganization_Click(object sender, RoutedEventArgs e)
        {
            AssignedOrganizationPostsConfirmed(this, null);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void LoadSearchedAdminOrganizationPosts(List<OrganizationPostDTO> posts)
        {
            //lstSearchedPosts.DisplayMemberPath = "Name";
            lstSearchedPosts.SelectedValuePath = "ID";
            lstSearchedPosts.ItemsSource = posts;
        }

        public void LoadConfirmedAdminOrganizationPosts(List<OrganizationPostDTO> posts)
        {
            lstConfirmedPosts.DisplayMemberPath = "Name";
            lstConfirmedPosts.SelectedValuePath = "ID";
            lstConfirmedPosts.ItemsSource = posts;
        }
        public List<OrganizationPostDTO> ConfirmedOrganizatoinPosts
        {
            get
            {
                return lstConfirmedPosts.ItemsSource as List<OrganizationPostDTO>;
            }

            set
            {
                lstConfirmedPosts.DisplayMemberPath = "Name";
                lstConfirmedPosts.SelectedValuePath = "ID";
                lstConfirmedPosts.ItemsSource = null;
                lstConfirmedPosts.ItemsSource = value;
            }
        }

        private void btnAddSearchedPostToConfirmed_Click(object sender, RoutedEventArgs e)
        {
            var searchedPosts = lstSearchedPosts.ItemsSource as List<OrganizationPostDTO>;
            if (searchedPosts != null && searchedPosts.Any(x => x.Selected == true))
                if (SearchedOrganizationPostSelected != null)
                {
                    List<OrganizationPostDTO> selectedItems = new List<OrganizationPostDTO>();
                    foreach (var item in searchedPosts.Where(x => x.Selected == true))
                    {
                        selectedItems.Add(item as OrganizationPostDTO);
                    }
                    SearchedOrganizationPostSelected(this, new OrganizationPostSelectedArg() { SelectedOrganizationPosts = selectedItems });
                }

        }

        private void cmbAdminOrganization_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (OrganizationPostSearchChanged != null)
                OrganizationPostSearchChanged(this, new OrganizationPostSearchArg() { SearchText = txtSearch.Text });
        }

        private void btnConfirmAdmindOrganization_Click(object sender, RoutedEventArgs e)
        {
           AdminOrganizationPostsConfirmed(this, null);
        }

        //private void btnConfirmAdmindOrganization_Click(object sender, RoutedEventArgs e)
        //{
        //    var item = cmbAdminOrganization.SelectedItem as OrganizationDTO;
        //    if (item != null)
        //    {
        //        var roles = lstAdminRoles.ItemsSource as List<RoleDTO>;
        //        if (chkByPassSecurity.IsChecked == true || roles.Count(x => x.Selected) > 0)
        //        {
        //            var arg = new AdminOrganizationConfirmedArg();
        //            arg.Organization = item;
        //            arg.ByPassSecurity = chkByPassSecurity.IsChecked == true;

        //            arg.SelectedRoleIds = roles.Where(x => x.Selected).Select(x => x.ID).ToList();
        //            AdminOrganizationsConfirmed(this, arg);
        //        }

        //    }
        //}


    }
}
