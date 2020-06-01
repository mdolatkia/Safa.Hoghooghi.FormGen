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

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SecurityMainWindow : UserControl
    {
        public SecurityMainWindow()
        {
            InitializeComponent();
            SetNavigartionTree();
        }

        private void SetNavigartionTree()
        {
            TreeViewItem node = AddTreeItem(treeNavigation.Items, "فرمهای امنیتی", "../Images/folder.png");
            var userNode = AddTreeItem(node.Items, "کاربران", "../Images/user.png");
            userNode.MouseLeftButtonUp += UserNode_MouseLeftButtonUp;
            var roleTypesNode = AddTreeItem(node.Items, "انواع نقش", "../Images/role.png");
            roleTypesNode.MouseLeftButtonUp += RoleTypesNode_MouseLeftButtonUp;
            var organizationtypeNode = AddTreeItem(node.Items, "انواع سازمان", "../Images/organizationtype.png");
            organizationtypeNode.MouseLeftButtonUp += OrganizationtypeNode_MouseLeftButtonUp;
            var organizationNode = AddTreeItem(node.Items, "سازمانها", "../Images/organization.png");
            organizationNode.MouseLeftButtonUp += OrganizationNode_MouseLeftButtonUp;
            var permissionNode = AddTreeItem(node.Items, "دسترسی ها", "../Images/permission.png");
            permissionNode.MouseLeftButtonUp += PermissionNode_MouseLeftButtonUp;
            var datasecurityNode = AddTreeItem(node.Items, "دسترسی موجودیت/داده مستقیم", "../Images/datasecurity.png");
            datasecurityNode.MouseLeftButtonUp += DatasecurityNode_MouseLeftButtonUp;
            var datasecurityindeirectNode = AddTreeItem(node.Items, "دسترسی موجودیت/داده غیر مستقیم", "../Images/datasecurityindeirect.png");
            datasecurityindeirectNode.MouseLeftButtonUp += DatasecurityindeirectNode_MouseLeftButtonUp;
            var conditionalsecurityNode = AddTreeItem(node.Items, "دسترسی های شرطی", "../Images/conditionalsecurity.png");
            conditionalsecurityNode.MouseLeftButtonUp += ConditionalsecurityNode_MouseLeftButtonUp;
            node.ExpandSubtree();
        }

        private void ConditionalsecurityNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frmConditionalPermission frm = new frmConditionalPermission();
            AddPane(frm, "دسترسی های شرطی");
        }

        private void DatasecurityindeirectNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frmEntitySecurityIndirect frm = new frmEntitySecurityIndirect();
            AddPane(frm, "دسترسی موجودیت/داده غیر مستقیم");
        }

        private void DatasecurityNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frmEntitySecurityDirect frm = new frmEntitySecurityDirect(0);
            AddPane(frm, "دسترسی موجودیت/داده مستقیم");
        }

        private void PermissionNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            frmPermission frm = new frmPermission();
            AddPane(frm, "دسترسی ها");
        }

        private void OrganizationNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frmOrganization frm = new frmOrganization(0);
            AddPane(frm, "سازمانها ها");
        }

        private void OrganizationtypeNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frmOrganizationType frm = new frmOrganizationType(0);
            AddPane(frm, "نوع سازمان");
        }

        private void RoleTypesNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frmRoleType frm = new frmRoleType(0);
            AddPane(frm, "نوع نقش");
        }

        private void UserNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frmUsers frm = new frmUsers(0);
            AddPane(frm, "کاربران");
        }

        private TreeViewItem AddTreeItem(ItemCollection items, string title, string iconPath)
        {
            TreeViewItem newNode = new TreeViewItem();
            newNode.Header = GetNodeHeader(title, iconPath);
            items.Add(newNode);
            return newNode;
        }
        private FrameworkElement GetNodeHeader(string title, string iconPath)
        {
            StackPanel pnlHeader = new StackPanel();
            TextBlock label = new TextBlock();
            label.Text = title;
            Image img = new Image();
            img.Width = 15;
            Uri uriSource = new Uri(iconPath, UriKind.Relative);
            img.Source = new BitmapImage(uriSource);
            pnlHeader.Orientation = Orientation.Horizontal;
            pnlHeader.Children.Add(img);
            pnlHeader.Children.Add(label);
            return pnlHeader;
        }

        private void AddPane(UIElement frm, string title)
        {
            RadPane pane = new RadPane();
            pane.Content = frm;
            pane.Header = title;
            pane.IsDockable = false;

            pane.CanFloat = false;
            pane.CanUserPin = false;
            pnlForms.Items.Add(pane);
            pane.CanUserClose = true;
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




   

 
     
    

     


    }
}
