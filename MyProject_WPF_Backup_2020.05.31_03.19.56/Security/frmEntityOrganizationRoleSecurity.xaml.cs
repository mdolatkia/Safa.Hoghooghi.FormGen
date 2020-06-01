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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows;
using Telerik.Windows.Controls;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmEntityOrganizationRoleSecurity.xaml
    /// </summary>
    public partial class frmEntityOrganizationRoleSecurity : UserControl
    {
        BizDatabase bizDatabase = new BizDatabase();
        public frmEntityOrganizationRoleSecurity()
        {
            InitializeComponent();
            ucObjectTree.ObjectRightClick += UcObjectTree_ObjectRightClick;
            var databases = bizDatabase.GetDatabases();
            ucObjectTree.ShowDatabaseObjects(databases.Select(x => x.ID).ToList());
        }

        private void UcObjectTree_ObjectRightClick(object sender, ObjectRightClickedArg e)
        {
            if (e.Object.ObjectCategory == ModelEntites.DatabaseObjectCategory.Entity)
            {
                ucObjectTree.SetContextMenu(e.Node, GetParametersContextMenu(Convert.ToInt32(e.Object.ObjectIdentity)));
            }

        }
        private RadContextMenu GetParametersContextMenu(int entityID)
        {
            RadContextMenu menu = new RadContextMenu();



            RadMenuItem customMenuItemOrganizationSecurityDirect = new RadMenuItem();
            customMenuItemOrganizationSecurityDirect.Header = "تعریف دسترسی سازمانی مستقیم";
            customMenuItemOrganizationSecurityDirect.Name = "DirectOrganizationSecurity";
            customMenuItemOrganizationSecurityDirect.Click += (sender1, EventArgs) => customMenuItem_ClickOrganizationSecurityDirect(sender1, EventArgs, entityID, customMenuItemOrganizationSecurityDirect.Header.ToString());
            menu.Items.Add(customMenuItemOrganizationSecurityDirect);


            RadMenuItem customMenuItemOrganizationSecurityInDirect = new RadMenuItem();
            customMenuItemOrganizationSecurityInDirect.Header = "تعریف دسترسی سازمانی غیر مستقیم";
            customMenuItemOrganizationSecurityInDirect.Name = "InDirectOrganizationSecurity";
            customMenuItemOrganizationSecurityInDirect.Click += (sender1, EventArgs) => customMenuItem_ClickOrganizationSecurityInDirect(sender1, EventArgs, entityID, customMenuItemOrganizationSecurityInDirect.Header.ToString());
            menu.Items.Add(customMenuItemOrganizationSecurityInDirect);

            RadMenuItem customMenuItemRoleSecurity = new RadMenuItem();
            customMenuItemRoleSecurity.Header = "تعریف دسترسی نقش کلی";
            customMenuItemRoleSecurity.Name = "RoleSecurity";
            customMenuItemRoleSecurity.Click += (sender1, EventArgs) => customMenuItem_ClickRoleSecurity(sender1, EventArgs, entityID, customMenuItemRoleSecurity.Header.ToString());
            menu.Items.Add(customMenuItemRoleSecurity);

            RadMenuItem customMenuItemRoleSecurityDirect = new RadMenuItem();
            customMenuItemRoleSecurityDirect.Header = "تعریف دسترسی نقش مستقیم";
            customMenuItemRoleSecurityDirect.Name = "DirectRoleSecurity";
            customMenuItemRoleSecurityDirect.Click += (sender1, EventArgs) => customMenuItem_ClickRoleSecurityDirect(sender1, EventArgs, entityID, customMenuItemRoleSecurityDirect.Header.ToString());
            menu.Items.Add(customMenuItemRoleSecurityDirect);


            RadMenuItem customMenuItemRoleSecurityInDirect = new RadMenuItem();
            customMenuItemRoleSecurityInDirect.Header = "تعریف دسترسی نقش غیر مستقیم";
            customMenuItemRoleSecurityInDirect.Name = "InDirectRoleSecurity";
            customMenuItemRoleSecurityInDirect.Click += (sender1, EventArgs) => customMenuItem_ClickRoleSecurityInDirect(sender1, EventArgs, entityID, customMenuItemRoleSecurityInDirect.Header.ToString());
            menu.Items.Add(customMenuItemRoleSecurityInDirect);

            return menu;
        }

        void customMenuItem_ClickOrganizationSecurityDirect(object sender, RoutedEventArgs e, int entityID, string header)
        {
            //frmEntityOrganizationSecurityDirect frm = new frmEntityOrganizationSecurityDirect(entityID);
            //AddTab(frm, header);
        }

        private void AddTab(UserControl frm, string header)
        {
            TabItem tabItem = new TabItem();
            tabItem.Header = header;
            tabItem.Content = frm;
            tabMain.Items.Add(tabItem);
            tabItem.IsSelected = true;
        }

        void customMenuItem_ClickOrganizationSecurityInDirect(object sender, RoutedEventArgs e, int entityID, string header)
        {
            //frmEntityOrganizationSecurityIndirect frm = new frmEntityOrganizationSecurityIndirect(entityID);
            //AddTab(frm, header);
        }
        void customMenuItem_ClickRoleSecurityDirect(object sender, RoutedEventArgs e, int entityID, string header)
        {
            //frmEntityRoleSecurityDirect frm = new frmEntityRoleSecurityDirect(entityID, 0);
            //AddTab(frm, header);
        }
        void customMenuItem_ClickRoleSecurityInDirect(object sender, RoutedEventArgs e, int entityID, string header)
        {
            //frmEntityRoleSecurityIndirect frm = new frmEntityRoleSecurityIndirect(entityID);
            //AddTab(frm, header);
        }
        void customMenuItem_ClickRoleSecurity(object sender, RoutedEventArgs e, int entityID, string header)
        {
            //frmRoleSecurity frm = new frmRoleSecurity(entityID);
            //AddTab(frm, header);
        }
    }
}
