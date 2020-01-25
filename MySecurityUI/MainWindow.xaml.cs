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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace MySecurityUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            TabItem tabItem = new TabItem();
            frmUsers frm = new frmUsers();
            tabItem.Header = "کاربران";
            tabItem.Content = frm;
            tabForms.Items.Add(tabItem);
            tabItem.IsSelected = true;
        }

        private void btnRoles_Click(object sender, RoutedEventArgs e)
        {
            TabItem tabItem = new TabItem();
            frmRoles frm = new frmRoles();
            tabItem.Header = "نقش ها";
            tabItem.Content = frm;
            tabForms.Items.Add(tabItem);
            tabItem.IsSelected = true;
        }

        //private void btnUserRoles_Click(object sender, RoutedEventArgs e)
        //{
        //    TabItem tabItem = new TabItem();
        //    frmUserRole frm = new frmUserRole();
        //    tabItem.Header = "نقش ها";
        //    tabItem.Content = frm;
        //    tabForms.Items.Add(tabItem);
        //    tabItem.IsSelected = true;

        //}

        //private void btnObjects_Click(object sender, RoutedEventArgs e)
        //{
        //    //TabItem tabItem = new TabItem();
        //    //frmObjects frm = new frmObjects();
        //    //tabItem.Header = "منابع";
        //    //tabItem.Content = frm;
        //    //tabForms.Items.Add(tabItem);
        //    //tabItem.IsSelected = true;
        //}

        private void btnPermissions_Click(object sender, RoutedEventArgs e)
        {
            TabItem tabItem = new TabItem();
            frmPermission frm = new frmPermission();
            tabItem.Header = "دسترسی ها";
            tabItem.Content = frm;
            tabForms.Items.Add(tabItem);
            tabItem.IsSelected = true;

        }

        private void btnOrganizations_Click(object sender, RoutedEventArgs e)
        {
            TabItem tabItem = new TabItem();
            frmOrganizations frm = new frmOrganizations();
            tabItem.Header = "سازمانها";
            tabItem.Content = frm;
            tabForms.Items.Add(tabItem);
            tabItem.IsSelected = true;
        }

        private void btnOrganizationUser_Click(object sender, RoutedEventArgs e)
        {
            TabItem tabItem = new TabItem();
            frmOrganizationUser frm = new frmOrganizationUser();
            tabItem.Header = "کاربر و سازمان مرتبط";
            tabItem.Content = frm;
            tabForms.Items.Add(tabItem);
            tabItem.IsSelected = true;
        }



        private void btnConditionalPermission_Click(object sender, RoutedEventArgs e)
        {
            TabItem tabItem = new TabItem();
            frmConditionalPermission frm = new frmConditionalPermission();
            tabItem.Header = "دسترسی های شرطی";
            tabItem.Content = frm;
            tabForms.Items.Add(tabItem);
            tabItem.IsSelected = true;
        }

        private void btnEntityRoleOrganizationSecurity_Click(object sender, RoutedEventArgs e)
        {
            TabItem tabItem = new TabItem();
            frmEntityOrganizationRoleSecurity frm = new frmEntityOrganizationRoleSecurity();
            tabItem.Header = "دسترسی به داده بر اساس نقش و سازمان";
            tabItem.Content = frm;
            tabForms.Items.Add(tabItem);
            tabItem.IsSelected = true;
        }
    }
}
