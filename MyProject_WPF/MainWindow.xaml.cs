using ModelEntites;

using MyFormulaFunctionStateFunctionLibrary;
using MyGeneralLibrary;
using MyModelGenerator;
using MyModelManager;
using MyProject_WPF.Biz;
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
using System.Windows.Threading;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private ModelGenerator_sql ModelGenerator = new ModelGenerator_sql();

        //int CurrentDatabaseID { set; get; }
        public MainWindow()
        {
            InitializeComponent();

            SetMenus();
            //DisableMenus();

            SetUserInfo();
            //frmDBConnector.DatabaseConnected += FrmDBConnector_DatabaseConnected;

            this.Loaded += MainWindow_Loaded;
            //   SetGridViewColumns();
            //SetFormMode(FormMode.Initialize);
        }
        RadTreeViewItem dbDataEntryNode;
        private void SetMenus()
        {
            RadTreeViewItem rootNode = AddTreeItem(treeMenu.Items, "منوها", "../Images/folder.png");
            if (MyProjectManager.GetMyProjectManager.UserInfo.OrganizationPosts.Any(x => x.IsSuperAdmin))
            {
                var dbServerNode = AddTreeItem(rootNode.Items, "مدیریت سرور پایگاه داده", "../Images/server.png");
                dbServerNode.MouseLeftButtonUp += DbServerNode_MouseLeftButtonUp;

                var dbConnectoinNode = AddTreeItem(rootNode.Items, "مدیریت پایگاه های داده", "../Images/database.png");
                dbConnectoinNode.MouseLeftButtonUp += DbConnectoinNode_MouseLeftButtonUp;

                dbDataEntryNode = AddTreeItem(rootNode.Items, "فرمها و روابط", "../Images/folder.png");
                dbDataEntryNode.IsExpanded = true;
                SetDatabases(dbDataEntryNode);
                var navigationTreeNode = AddTreeItem(rootNode.Items, "تعریف درخت منو", "../Images/navigationtree.png");
                navigationTreeNode.MouseLeftButtonUp += NavigationTreeNode_MouseLeftButtonUp;
                var letterNode = AddTreeItem(rootNode.Items, "تنظیمات نامه", "../Images/mail.png");
                letterNode.MouseLeftButtonUp += LetterNode_MouseLeftButtonUp;
                var processNode = AddTreeItem(rootNode.Items, "جریان کار", "../Images/report.png");
                processNode.MouseLeftButtonUp += ProcessNode_MouseLeftButtonUp;

                var securityNode = AddTreeItem(rootNode.Items, "تنظیمات امنیتی", "../Images/folder.png");
                //securityNode.MouseLeftButtonUp += SecurityNode_MouseLeftButtonUp;
                var userNode = AddTreeItem(securityNode.Items, "کاربران", "../Images/user.png");
                userNode.MouseLeftButtonUp += UserNode_MouseLeftButtonUp;
                var roleTypesNode = AddTreeItem(securityNode.Items, "انواع نقش", "../Images/role.png");
                roleTypesNode.MouseLeftButtonUp += RoleTypesNode_MouseLeftButtonUp;
                var organizationtypeNode = AddTreeItem(securityNode.Items, "انواع سازمان", "../Images/organizationtype.png");
                organizationtypeNode.MouseLeftButtonUp += OrganizationtypeNode_MouseLeftButtonUp;
                var organizationNode = AddTreeItem(securityNode.Items, "سازمان و پستها", "../Images/organization.png");
                organizationNode.MouseLeftButtonUp += OrganizationNode_MouseLeftButtonUp;
                var permissionNode = AddTreeItem(securityNode.Items, "دسترسی ها", "../Images/permission.png");
                permissionNode.MouseLeftButtonUp += PermissionNode_MouseLeftButtonUp;
                var datasecurityNode = AddTreeItem(securityNode.Items, "دسترسی موجودیت/داده مستقیم", "../Images/datasecurity.png");
                datasecurityNode.MouseLeftButtonUp += DatasecurityNode_MouseLeftButtonUp;
                var datasecurityindeirectNode = AddTreeItem(securityNode.Items, "دسترسی موجودیت/داده غیر مستقیم", "../Images/datasecurityindeirect.png");
                datasecurityindeirectNode.MouseLeftButtonUp += DatasecurityindeirectNode_MouseLeftButtonUp;
                //var conditionalsecurityNode = AddTreeItem(securityNode.Items, "دسترسی های شرطی", "../Images/conditionalsecurity.png");
                //conditionalsecurityNode.MouseLeftButtonUp += ConditionalsecurityNode_MouseLeftButtonUp;
                securityNode.IsExpanded = true;
                rootNode.IsExpanded = true;
            }
            else if (MyProjectManager.GetMyProjectManager.UserInfo.OrganizationPosts.Any(x => x.IsAdmin))
            {
                var securityNode = AddTreeItem(rootNode.Items, "تنظیمات امنیتی", "../Images/folder.png");
                var adminuserNode = AddTreeItem(securityNode.Items, "کاربران", "../Images/user.png");
                adminuserNode.MouseLeftButtonUp += AdminuserNode_MouseLeftButtonUp;
                var adminOrganizationNode = AddTreeItem(securityNode.Items, "سازمان و پستها", "../Images/organization.png");
                adminOrganizationNode.MouseLeftButtonUp += AdminOrganizationNode_MouseLeftButtonUp;
                rootNode.IsExpanded = true;
            }
        }

        private void SetDatabases(RadTreeViewItem dbDataEntryNode)
        {
            dbDataEntryNode.Items.Clear();
            BizDatabase bizDatabase = new BizDatabase();
            var databases = bizDatabase.GetDatabases();
            foreach (var item in databases)
            {
                var dbNode = AddTreeItem(dbDataEntryNode.Items, item.Title, "../Images/database.png", item);
                var contextMenu = GetContextMenu(dbNode, "استخراج منابع");
                contextMenu.ItemClick += (sender, e) => ContextMenu_ItemClick(sender, e, item);
                dbNode.Items.Add("Loading...");
                dbNode.Expanded += DbNode_Expanded;
            }
        }

        private void ContextMenu_ItemClick(object sender, Telerik.Windows.RadRoutedEventArgs e, DatabaseDTO db)
        {
            var frm = new DatabaseImportWizard(db.ID);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "استخراج منابع" + " - " + db.Name, Enum_WindowSize.Maximized);

            //AddPane(frm, "استخراج منابع" + " - " + db.Name);
            //var frm1 = new DatabaseImport(db.ID);
            //AddPane(frm1, "استخراج منابع" + " - " + db.Name);

        }

        private RadContextMenu GetContextMenu(RadTreeViewItem dbNode, string header)
        {
            var menu = RadContextMenu.GetContextMenu(dbNode);

            if (menu == null)
            {
                menu = new RadContextMenu();
                RadContextMenu.SetContextMenu(dbNode, menu);
            }
            RadMenuItem item = new RadMenuItem();
            item.Header = header;
            menu.Items.Add(item);
            return menu;
        }


        private void DbServerNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var title = "مدیریت سرور پایگاه داده";
            if (!CheckPaneExists(title))
            {
                frmDBServer frm = new frmDBServer(0);
                AddPane(frm, title);
            }
        }

        private void AdminuserNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var title = "کاربران";
            if (!CheckPaneExists(title))
            {
                frmUsers frm = new frmUsers(true);
                AddPane(frm, title);
            }
        }

        private void AdminOrganizationNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var title = "سازمانها ها";
            if (!CheckPaneExists(title))
            {
                frmOrganization frm = new frmOrganization(true);
                AddPane(frm, title);
            }
        }

        //private void ConditionalsecurityNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    frmConditionalPermission frm = new frmConditionalPermission();
        //    AddPane(frm, "دسترسی های شرطی");
        //}

        private void DatasecurityindeirectNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var title = "دسترسی موجودیت/داده غیر مستقیم";
            if (!CheckPaneExists(title))
            {
                frmEntitySecurityIndirect frm = new frmEntitySecurityIndirect();
                AddPane(frm, title);
            }
        }

        private void DatasecurityNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var title = "دسترسی موجودیت/داده مستقیم";
            if (!CheckPaneExists(title))
            {
                frmEntitySecurityDirect frm = new frmEntitySecurityDirect(0);
                AddPane(frm, title);
            }
        }

        private void PermissionNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var title = "دسترسی ها";
            if (!CheckPaneExists(title))
            {
                frmPermission frm = new frmPermission();
                AddPane(frm, title);
            }
        }

        private void OrganizationNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var title = "سازمانها ها";
            if (!CheckPaneExists(title))
            {
                frmOrganization frm = new frmOrganization();
                AddPane(frm, title);
            }
        }

        private void OrganizationtypeNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var title = "نوع سازمان";
            if (!CheckPaneExists(title))
            {
                frmOrganizationType frm = new frmOrganizationType(0);
                AddPane(frm, title);
            }
        }

        private void RoleTypesNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var title = "نوع نقش";
            if (!CheckPaneExists(title))
            {
                frmRoleType frm = new frmRoleType(0);
                AddPane(frm, title);
            }
        }

        private void UserNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var title = "کاربران";
            if (!CheckPaneExists(title))
            {
                frmUsers frm = new frmUsers();
                AddPane(frm, title);
            }
        }

        private void DbNode_Expanded(object sender, RoutedEventArgs e)
        {
            var treeItem = e.Source as RadTreeViewItem;
            if (treeItem != null)
            {
                if (treeItem.Tag != null)
                {
                    bool firstTime = false;
                    if (treeItem.Items.Count > 0)
                    {
                        var firstItem = treeItem.Items[0];
                        if (firstItem is string && firstItem.ToString() == "Loading...")
                            firstTime = true;

                    }
                    if (firstTime)
                    {
                        treeItem.Items.Clear();

                        var entitiesNode = AddTreeItem(treeItem.Items, "مدیریت موجودیت ها", "../Images/form.png");
                        entitiesNode.MouseLeftButtonUp += (sender1, e1) => EntitiesNode_MouseLeftButtonUp(sender1, e1, (DatabaseDTO)treeItem.Tag);
                        var relationshipNode = AddTreeItem(treeItem.Items, "روابط مفهومی", "../Images/relationship1.png");
                        relationshipNode.MouseLeftButtonUp += (sender1, e1) => RelationshipNode_MouseLeftButtonUp(sender1, e1, (DatabaseDTO)treeItem.Tag);
                        var dbOthersNode = AddTreeItem(treeItem.Items, "سایر موارد", "../Images/state.png");
                        dbOthersNode.MouseLeftButtonUp += (sender1, e1) => DbOthersNode_MouseLeftButtonUp(sender1, e1, (DatabaseDTO)treeItem.Tag);
                    }
                }
            }
        }

        private void EntitiesNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e, DatabaseDTO database)
        {
            var title = "موجودیتها" + " - " + database.Name;
            if (!CheckPaneExists(title))
            {
                frmEntities frm = new frmEntities(database.ID);
                AddPane(frm, title);
            }
            else
            {
                var frm = GetPaneForm(title) as frmEntities;
                if (frm != null)
                    frm.ActivateEntities();
            }
        }

        private UserControl GetPaneForm(string title)
        {
            foreach (var item in pnlForms.Items)
            {
                if ((item as RadPane).Header.ToString() == title)
                {
                    return (item as RadPane).Content as UserControl;
                }
            }
            return null;
        }

        private bool CheckPaneExists(string title, bool select = true)
        {
            foreach (var item in pnlForms.Items)
            {
                if ((item as RadPane).Header.ToString() == title)
                {
                    if (select)
                        (item as RadPane).IsSelected = true; ;
                    return true;
                }
            }
            return false;
        }

        private void RelationshipNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e, DatabaseDTO database)
        {
            var title = "روابط" + " - " + database.Name;
            if (!CheckPaneExists(title))
            {
                frmAllRelationships frm = new frmAllRelationships(database);
                AddPane(frm, title);
            }
        }
        private void DbOthersNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e, DatabaseDTO database)
        {
            var title = "سایر موارد" + " - " + database.Name;
            if (!CheckPaneExists(title))
            {
                frmDBViewFunctions frm = new frmDBViewFunctions(database.ID);
                AddPane(frm, title);
            }
        }

        private void LetterNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var title = "تنظیمات نامه";
            if (!CheckPaneExists(title))
            {
                frmLetterSetting view = new MyProject_WPF.frmLetterSetting();
                AddPane(view, title);
            }
        }

        //private void SecurityNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    SecurityMainWindow frm = new SecurityMainWindow();
        //    AddPane(frm, "تنظیمات امنیتی");
        //}

        private void ProcessNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var title = "جریان کار";
            if (!CheckPaneExists(title))
            {
                frmProcess frm = new frmProcess(0);
                AddPane(frm, title);
            }
        }

        private void NavigationTreeNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var title = "درخت منو";
            if (!CheckPaneExists(title))
            {
                frmNavigationTree frm = new MyProject_WPF.frmNavigationTree();
                AddPane(frm, title);
            }
        }






        private void DbConnectoinNode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var title = "مدیریت پایگاه داده";
            if (!CheckPaneExists(title))
            {
                frmDatabase frm = new frmDatabase(0);
                frm.DatabaseUpdated += Frm_DatabaseUpdated;
                AddPane(frm, title);
            }
        }

        private void Frm_DatabaseUpdated(object sender, DatabaseUpdatedArg e)
        {
            SetDatabases(dbDataEntryNode);
        }

        private RadTreeViewItem AddTreeItem(ItemCollection items, string title, string iconPath, object tag = null)
        {
            RadTreeViewItem newNode = new RadTreeViewItem();
            newNode.Header = GetNodeHeader(title, iconPath);
            if (tag != null)
                newNode.Tag = tag;
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

        bool _loaded = false;
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_loaded)
            {
                _loaded = true;
                //ShowDatabaseConnectionForm();
            }
        }

        private void SetUserInfo()
        {
            string tooltip = "";
            if (MyProjectManager.GetMyProjectManager.UserInfo != null)
            {
                var userInfo = MyProjectManager.GetMyProjectManager.UserInfo;
                tooltip += (tooltip == "" ? "" : Environment.NewLine) + "نام کاربری" + " : " + userInfo.UserName;
                tooltip += (tooltip == "" ? "" : Environment.NewLine) + "نام شخص" + " : " + userInfo.FirstName + " " + userInfo.LastName;
                foreach (var post in userInfo.OrganizationPosts)
                {
                    tooltip += (tooltip == "" ? "" : Environment.NewLine) + "نقش" + " : " + post.Name + " ، " + "سازمان" + " : " + post.OrganizationName;
                }
            }
            ToolTipService.SetToolTip(imgUser, tooltip);
        }

        //private void ShowDatabaseConnectionForm()
        //{

        //    //frm.DatabaseConnected += FrmDBConnector_DatabaseConnected1;
        //    //MyProjectManager.GetMyProjectManager.ShowDialog(frm, "انتخاب پایگاه داده");
        //    //RadPane pane = new RadPane();
        //    //pane.Content = frm;
        //    //pane.Header = "اتصال";
        //    //grpDatabase.Items.Add(pane);

        //}



        //private void FrmDBConnector_DatabaseConnected1(object sender, DatabaseConnectionArg e)
        //{
        //    CurrentDatabaseID = e.DatabaseID;
        //    //btnEntities.IsEnabled = true;
        //    //btnRelationships.IsEnabled = true;
        //    //btnOthers.IsEnabled = true;

        //    MyProjectManager.GetMyProjectManager.CloseDialog(sender);
        //}

        //private void SetFormMode(FormMode formMode)
        //{
        //    if (formMode == FormMode.Initialize)
        //    {
        //        SetDefaultValues();
        //        txtUserName.Text = "sa";
        //        txtPassword.Password = "123";
        //        btnEditConnection.IsEnabled = false;
        //        btnEditConnection_Click(null, null);
        //    }
        //    else if (formMode == FormMode.ConnectionDetailsEntered)
        //    {
        //        btnTestConnection.IsEnabled = true;
        //    }
        //    else if (formMode == FormMode.ConnectionSucceed)
        //    {
        //        tabcontrolMain.IsEnabled = true;
        //        btnTestConnection.IsEnabled = false;
        //        txtServerName.IsEnabled = false;
        //        txtDBName.IsEnabled = false;
        //        txtUserName.IsEnabled = false;
        //        txtPassword.IsEnabled = false;
        //        btnEditConnection.IsEnabled = true;
        //    }
        //    else if (formMode == FormMode.ConnectionFailed)
        //    {
        //        tabcontrolMain.IsEnabled = false;
        //    }
        //    else if (formMode == FormMode.ConnectionEdit)
        //    {
        //        tabcontrolMain.IsEnabled = false;
        //        btnTestConnection.IsEnabled = true;
        //        txtServerName.IsEnabled = true;
        //        txtDBName.IsEnabled = true;
        //        txtUserName.IsEnabled = true;
        //        txtPassword.IsEnabled = true;
        //        btnEditConnection.IsEnabled = false;
        //    }
        //}

        //private void SetEvents()
        //{










        //    //ModelGenerator.ItemGenerationEvent += helper_ItemGenerationEvent;

        //    //bizTableDrivedEntity.RuleImposedEvent += bizRuleImposedEvent;
        //    //bizRelationship.RuleImposedEvent += bizRuleImposedEvent;

        //    //infoTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
        //    //infoTimer.Tick += infoTimer_Tick;



        //    //dtgRuleEntity.CommandCellClick += dtgRuleEntity_CommandCellClick;
        //    //dtgRuleEntity.CellFormatting += dtgRuleEntity_CellFormatting;
        //}
        //int DatabaseID { set; get; }
        //private void FrmDBConnector_DatabaseConnected(object sender, DatabaseConnectionArg e)
        //{
        //    DatabaseID = e.DatabaseID;
        //}




        //private void SetDefaultValues()
        //{
        //    if (!string.IsNullOrEmpty(Properties.Settings.Default.LastDatabaseName))
        //        txtDBName.Text = Properties.Settings.Default.LastDatabaseName;
        //    if (!string.IsNullOrEmpty(Properties.Settings.Default.LastServerName))
        //        txtServerName.Text = Properties.Settings.Default.LastServerName;
        //}

        //public string ServerName
        //{
        //    get { return txtServerName.Text; }
        //}

        //public string DatabaseName
        //{
        //    get { return txtDBName.Text; }
        //}
        BizColumn bizColumn = new BizColumn();
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        BizTable bizTable = new BizTable();



        //private void txtServerName_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    SetFormMode(FormMode.ConnectionDetailsEntered);
        //}
        //private void txtPassword_TextChanged(object sender, RoutedEventArgs e)
        //{
        //    txtServerName_TextChanged(null, null);

        //}









        //if (relationship is OneToManyRelationshipDTO || relationship.TypeEnum == Enum_RelationshipType.OneToMany)
        //{
        //    RadMenuItem customMenuItem = new RadMenuItem();
        //    customMenuItem.Tag = Enum_RelationshipType.ImplicitOneToOne;
        //    customMenuItem.Header = "Convert To Implicit OneToOne Relationship";
        //    customMenuItem.Name = "OneToMany_ImplicitOneToOne";
        //    customMenuItem.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //    RadMenuItem customMenuItem1 = new RadMenuItem();
        //    customMenuItem1.Header = "Convert To SupertType To SubType Relationship";
        //    customMenuItem1.Name = "OneToMany_SuperToSub";
        //    customMenuItem1.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //    RadMenuItem customMenuItem2 = new RadMenuItem();
        //    customMenuItem2.Header = "Convert To SubUnion To Union Relationship";
        //    customMenuItem2.Name = "OneToMany_SubUnionToUnion";
        //    customMenuItem2.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //    RadMenuItem customMenuItem3 = new RadMenuItem();
        //    customMenuItem3.Header = "Convert To Union To SubUnion Relationship";
        //    customMenuItem3.Name = "OneToMany_UnionToSubUnion";
        //    customMenuItem3.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;


        //    contextMenu.Items.Add(customMenuItem);
        //    contextMenu.Items.Add(customMenuItem1);
        //    contextMenu.Items.Add(customMenuItem2);
        //    contextMenu.Items.Add(customMenuItem3);

        //}
        //else if (relationship is ManyToOneRelationshipDTO || relationship.TypeEnum == Enum_RelationshipType.ManyToOne)
        //{
        //    RadMenuItem customMenuItem = new RadMenuItem();
        //    customMenuItem.Header = "Convert To Explicit OneToOne Relationship";
        //    customMenuItem.Name = "ManyToOne_ExplicitOneToOne";
        //    customMenuItem.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //    RadMenuItem customMenuItem1 = new RadMenuItem();
        //    customMenuItem1.Header = "Convert To SubType To SupertType Relationship";
        //    customMenuItem1.Name = "ManyToOne_SuperToSub";
        //    customMenuItem1.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //    RadMenuItem customMenuItem2 = new RadMenuItem();
        //    customMenuItem2.Header = "Convert To SubUnion To Union Relationship";
        //    customMenuItem2.Name = "ManyToOne_SubUnionToUnion";
        //    customMenuItem2.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //    RadMenuItem customMenuItem3 = new RadMenuItem();
        //    customMenuItem3.Header = "Convert To Union To SubUnion Relationship";
        //    customMenuItem3.Name = "ManyToOne_UnionToSubUnion";
        //    customMenuItem3.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;

        //    contextMenu.Items.Add(customMenuItem);
        //    contextMenu.Items.Add(customMenuItem1);
        //    contextMenu.Items.Add(customMenuItem2);
        //    contextMenu.Items.Add(customMenuItem3);

        //}
        //else if (relationship is ImplicitOneToOneRelationshipDTO || relationship.TypeEnum == Enum_RelationshipType.ImplicitOneToOne)
        //{
        //    RadMenuItem customMenuItem = new RadMenuItem();
        //    customMenuItem.Header = "Convert To One To Many Relationship";
        //    customMenuItem.Name = "ImplicitOneToOne_OneToMany";
        //    customMenuItem.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //    RadMenuItem customMenuItem1 = new RadMenuItem();
        //    customMenuItem1.Header = "Convert To SupertType To SubType Relationship";
        //    customMenuItem1.Name = "ImplicitOneToOne_SuperToSub";
        //    customMenuItem1.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //    RadMenuItem customMenuItem2 = new RadMenuItem();
        //    customMenuItem2.Header = "Convert To SubUnion To Union Relationship";
        //    customMenuItem2.Name = "ImplicitOneToOne_SubUnionToUnion";
        //    customMenuItem2.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //    RadMenuItem customMenuItem3 = new RadMenuItem();
        //    customMenuItem3.Header = "Convert To Union To SubUnion Relationship";
        //    customMenuItem3.Name = "ImplicitOneToOne_UnionToSubUnion";
        //    customMenuItem3.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;

        //    contextMenu.Items.Add(customMenuItem);
        //    contextMenu.Items.Add(customMenuItem1);
        //    contextMenu.Items.Add(customMenuItem2);
        //    contextMenu.Items.Add(customMenuItem3);

        //}
        //else if (relationship is ExplicitOneToOneRelationshipDTO || relationship.TypeEnum == Enum_RelationshipType.ExplicitOneToOne)
        //{
        //    RadMenuItem customMenuItem = new RadMenuItem();
        //    customMenuItem.Header = "Convert To Many To One Relationship";
        //    customMenuItem.Name = "ExplicitOneToOne_ManyToOne";
        //    customMenuItem.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //    RadMenuItem customMenuItem1 = new RadMenuItem();
        //    customMenuItem1.Header = "Convert To SubType To SupertType Relationship";
        //    customMenuItem1.Name = "ExplicitOneToOne_SuperToSub";
        //    customMenuItem1.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //    RadMenuItem customMenuItem2 = new RadMenuItem();
        //    customMenuItem2.Header = "Convert To SubUnion To Union Relationship";
        //    customMenuItem2.Name = "ExplicitOneToOne_SubUnionToUnion";
        //    customMenuItem2.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //    RadMenuItem customMenuItem3 = new RadMenuItem();
        //    customMenuItem3.Header = "Convert To Union To SubUnion Relationship";
        //    customMenuItem3.Name = "ExplicitOneToOne_UnionToSubUnion";
        //    customMenuItem3.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;

        //    contextMenu.Items.Add(customMenuItem);
        //    contextMenu.Items.Add(customMenuItem1);
        //    contextMenu.Items.Add(customMenuItem2);
        //    contextMenu.Items.Add(customMenuItem3);

        //}
        //else if (relationship is SuperToSubRelationshipDTO || relationship.TypeEnum == Enum_RelationshipType.SuperToSub)
        //{
        //    RadMenuItem customMenuItem = new RadMenuItem();
        //    customMenuItem.Header = "Convert To One To Many Relationship";
        //    customMenuItem.Name = "SuperToSub_OneToMany";
        //    customMenuItem.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //    RadMenuItem customMenuItem1 = new RadMenuItem();
        //    customMenuItem1.Header = "Convert To Implicit One To One Relationship";
        //    customMenuItem1.Name = "SuperToSub_ImplicitOneToOne";
        //    customMenuItem1.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //    RadMenuItem customMenuItem2 = new RadMenuItem();
        //    customMenuItem2.Header = "Convert To SubUnion To Union Relationship";
        //    customMenuItem2.Name = "SuperToSub_SubUnionToUnion";
        //    customMenuItem2.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //    RadMenuItem customMenuItem3 = new RadMenuItem();
        //    customMenuItem3.Header = "Convert To Union To SubUnion Relationship";
        //    customMenuItem3.Name = "SuperToSub_UnionToSubUnion";
        //    customMenuItem3.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;



        //    contextMenu.Items.Add(customMenuItem);
        //    contextMenu.Items.Add(customMenuItem1);
        //    contextMenu.Items.Add(customMenuItem2);
        //    contextMenu.Items.Add(customMenuItem3);

        //    RadMenuSeparatorItem separator1 = new RadMenuSeparatorItem();
        //    contextMenu.Items.Add(separator1);


        //    RadMenuItem customMenuItem4 = new RadMenuItem();
        //    customMenuItem4.Header = "Add To another ISA Relationship";
        //    customMenuItem4.Name = "SuperToSub_SuperToSub";
        //    customMenuItem4.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //    contextMenu.Items.Add(customMenuItem4);
        //}
        //else if (relationship is SubToSuperRelationshipDTO || relationship.TypeEnum == Enum_RelationshipType.SubToSuper)
        //{

        //    RadMenuItem customMenuItem = new RadMenuItem();
        //    customMenuItem.Header = "Convert To Many To One Relationship";
        //    customMenuItem.Name = "SubToSuper_ManyToOne";
        //    customMenuItem.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //    RadMenuItem customMenuItem1 = new RadMenuItem();
        //    customMenuItem1.Header = "Convert To Explicit One To One Relationship";
        //    customMenuItem1.Name = "SubToSuper_ExplicitOneToOne";
        //    customMenuItem1.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //    RadMenuItem customMenuItem2 = new RadMenuItem();
        //    customMenuItem2.Header = "Convert To SubUnion To Union Relationship";
        //    customMenuItem2.Name = "SubToSuper_SubUnionToUnion";
        //    customMenuItem2.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //    RadMenuItem customMenuItem3 = new RadMenuItem();
        //    customMenuItem3.Header = "Convert To Union To SubUnion Relationship";
        //    customMenuItem3.Name = "SubToSuper_UnionToSubUnion";
        //    customMenuItem3.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;

        //    contextMenu.Items.Add(customMenuItem);
        //    contextMenu.Items.Add(customMenuItem1);
        //    contextMenu.Items.Add(customMenuItem2);
        //    contextMenu.Items.Add(customMenuItem3);

        //    RadMenuSeparatorItem separator1 = new RadMenuSeparatorItem();
        //    contextMenu.Items.Add(separator1);

        //    RadMenuItem customMenuItem4 = new RadMenuItem();
        //    customMenuItem4.Header = "Add To another ISA Relationship";
        //    customMenuItem4.Name = "SubToSuper_SubToSuper";
        //    customMenuItem4.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //    contextMenu.Items.Add(customMenuItem4);
        //}
        //else if (relationship is SuperUnionToSubUnionRelationshipDTO || relationship.TypeEnum == Enum_RelationshipType.UnionToSubUnion_SubUnionHoldsKeys || relationship.TypeEnum == Enum_RelationshipType.UnionToSubUnion_UnionHoldsKeys)
        //{
        //    if (!(relationship as SuperUnionToSubUnionRelationshipDTO).UnionHoldsKeys || relationship.TypeEnum == Enum_RelationshipType.UnionToSubUnion_SubUnionHoldsKeys)
        //    {
        //        RadMenuItem customMenuItem = new RadMenuItem();
        //        customMenuItem.Header = "Convert To One To Many Relationship";
        //        customMenuItem.Name = "UnionToSubUnion_!UnionHoldsKeys_OneToMany";
        //        customMenuItem.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //        RadMenuItem customMenuItem1 = new RadMenuItem();
        //        customMenuItem1.Header = "Convert To Implicit One To One Relationship";
        //        customMenuItem1.Name = "UnionToSubUnion_!UnionHoldsKeys_ImplicitOneToOne";
        //        customMenuItem1.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //        RadMenuItem customMenuItem2 = new RadMenuItem();
        //        customMenuItem2.Header = "Convert To SupertType To SubType Relationship";
        //        customMenuItem2.Name = "UnionToSubUnion_!UnionHoldsKeys_SuperToSub";
        //        customMenuItem2.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //        RadMenuItem customMenuItem3 = new RadMenuItem();
        //        customMenuItem3.Header = "Convert To SubUnion To Union Relationship";
        //        customMenuItem3.Name = "UnionToSubUnion_!UnionHoldsKeys_SubUnionToUnion";
        //        customMenuItem3.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;


        //        contextMenu.Items.Add(customMenuItem);
        //        contextMenu.Items.Add(customMenuItem1);
        //        contextMenu.Items.Add(customMenuItem2);
        //        contextMenu.Items.Add(customMenuItem3);

        //        RadMenuSeparatorItem separator1 = new RadMenuSeparatorItem();
        //        contextMenu.Items.Add(separator1);


        //        RadMenuItem customMenuItem4 = new RadMenuItem();
        //        customMenuItem4.Header = "Add To another Union Relationship";
        //        customMenuItem4.Name = "UnionToSubUnion_!UnionHoldsKeys_UnionToSubUnion";
        //        customMenuItem4.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //        contextMenu.Items.Add(customMenuItem4);

        //    }
        //    else
        //    {
        //        RadMenuItem customMenuItem = new RadMenuItem();
        //        customMenuItem.Header = "Convert To Many To One Relationship";
        //        customMenuItem.Name = "UnionToSubUnion_UnionHoldsKeys_ManyToOne";
        //        customMenuItem.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //        RadMenuItem customMenuItem1 = new RadMenuItem();
        //        customMenuItem1.Header = "Convert To Explicit One To One Relationship";
        //        customMenuItem1.Name = "UnionToSubUnion_UnionHoldsKeys_ExplicitOneToOne";
        //        customMenuItem1.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship);
        //        RadMenuItem customMenuItem2 = new RadMenuItem();
        //        customMenuItem2.Header = "Convert To SubType To SuperType Relationship";
        //        customMenuItem2.Name = "UnionToSubUnion_UnionHoldsKeys_SubToSuper";
        //        customMenuItem2.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //        RadMenuItem customMenuItem3 = new RadMenuItem();
        //        customMenuItem3.Header = "Convert To SubUnion To Union Relationship";
        //        customMenuItem3.Name = "UnionToSubUnion_UnionHoldsKeys_SubUnionToUnion";
        //        customMenuItem3.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;


        //        contextMenu.Items.Add(customMenuItem);
        //        contextMenu.Items.Add(customMenuItem1);
        //        contextMenu.Items.Add(customMenuItem2);
        //        contextMenu.Items.Add(customMenuItem3);


        //        RadMenuSeparatorItem separator1 = new RadMenuSeparatorItem();
        //        contextMenu.Items.Add(separator1);


        //        RadMenuItem customMenuItem4 = new RadMenuItem();
        //        customMenuItem4.Header = "Add To another Union Relationship";
        //        customMenuItem4.Name = "UnionToSubUnion_UnionHoldsKeys_UnionToSubUnion";
        //        customMenuItem4.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //        contextMenu.Items.Add(customMenuItem4);
        //    }

        //}
        //else if (relationship is SubUnionToSuperUnionRelationshipDTO || relationship.TypeEnum == Enum_RelationshipType.SubUnionToUnion_UnionHoldsKeys || relationship.TypeEnum == Enum_RelationshipType.SubUnionToUnion_SubUnionHoldsKeys)
        //{
        //    if (!(relationship as SubUnionToSuperUnionRelationshipDTO).UnionHoldsKeys || relationship.TypeEnum == Enum_RelationshipType.SubUnionToUnion_SubUnionHoldsKeys)
        //    {
        //        RadMenuItem customMenuItem = new RadMenuItem();
        //        customMenuItem.Header = "Convert To Many To One Relationship";
        //        customMenuItem.Name = "SubUnionToUnion_!UnionHoldsKeys_ManyToOne";
        //        customMenuItem.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //        RadMenuItem customMenuItem1 = new RadMenuItem();
        //        customMenuItem1.Header = "Convert To Explicit One To One Relationship";
        //        customMenuItem1.Name = "SubUnionToUnion_!UnionHoldsKeys_ExplicitOneToOne";
        //        customMenuItem1.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //        RadMenuItem customMenuItem2 = new RadMenuItem();
        //        customMenuItem2.Header = "Convert To SubType To SuperType Relationship";
        //        customMenuItem2.Name = "SubUnionToUnion_!UnionHoldsKeys_SubToSuper";
        //        customMenuItem2.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //        RadMenuItem customMenuItem3 = new RadMenuItem();
        //        customMenuItem3.Header = "Convert To Union To SubUnion Relationship";
        //        customMenuItem3.Name = "SubUnionToUnion_!UnionHoldsKeys_UnionToSubUnion";
        //        customMenuItem3.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;


        //        contextMenu.Items.Add(customMenuItem);
        //        contextMenu.Items.Add(customMenuItem1);
        //        contextMenu.Items.Add(customMenuItem2);
        //        contextMenu.Items.Add(customMenuItem3);


        //        RadMenuSeparatorItem separator1 = new RadMenuSeparatorItem();
        //        contextMenu.Items.Add(separator1);


        //        RadMenuItem customMenuItem4 = new RadMenuItem();
        //        customMenuItem4.Header = "Add To another Union Relationship";
        //        customMenuItem4.Name = "SubUnionToUnion_!UnionHoldsKeys_SubUnionToUnion";
        //        customMenuItem4.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //        contextMenu.Items.Add(customMenuItem4);
        //    }
        //    else
        //    {
        //        RadMenuItem customMenuItem = new RadMenuItem();
        //        customMenuItem.Header = "Convert To One To Many Relationship";
        //        customMenuItem.Name = "SubUnionToUnion_UnionHoldsKeys_OneToMany";
        //        customMenuItem.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //        RadMenuItem customMenuItem1 = new RadMenuItem();
        //        customMenuItem1.Header = "Convert To Implicit One To One Relationship";
        //        customMenuItem1.Name = "SubUnionToUnion_UnionHoldsKeys_ImplicitOneToOne";
        //        customMenuItem1.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //        RadMenuItem customMenuItem2 = new RadMenuItem();
        //        customMenuItem2.Header = "Convert To SupertType To SubType Relationship";
        //        customMenuItem2.Name = "SubUnionToUnion_UnionHoldsKeys_SuperToSub";
        //        customMenuItem2.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //        RadMenuItem customMenuItem3 = new RadMenuItem();
        //        customMenuItem3.Header = "Convert To Union To SubUnion Relationship";
        //        customMenuItem3.Name = "SubUnionToUnion_UnionHoldsKeys_UnionToSubUnion";
        //        customMenuItem3.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;


        //        contextMenu.Items.Add(customMenuItem);
        //        contextMenu.Items.Add(customMenuItem1);
        //        contextMenu.Items.Add(customMenuItem2);
        //        contextMenu.Items.Add(customMenuItem3);

        //        RadMenuItem customMenuItem4 = new RadMenuItem();
        //        customMenuItem4.Header = "Add To another Union Relationship";
        //        customMenuItem4.Name = "SubUnionToUnion_UnionHoldsKeys_SubUnionToUnion";
        //        customMenuItem4.Click += (sender, EventArgs) => customMenuItem_Click(sender, contextMenu, relationship); ;
        //        contextMenu.Items.Add(customMenuItem4);
        //    }
        //}




        //void dtgEntityRelations_HyperlinkOpening(object sender, HyperlinkOpeningEventArgs e)
        //{
        //    if (e.Column.FieldName == "SuperTypeEntities")
        //    {
        //        var data = e.Row.DataBoundItem as EntityDTO;
        //        if (data != null)
        //        {
        //            MyFormHelper.ChooseSuperEntityFor(data);
        //        }
        //    }
        //    else if (e.Column.FieldName == "UnionTypeEntities")
        //    {
        //        var data = e.Row.DataBoundItem as EntityDTO;
        //        if (data != null)
        //        {
        //            MyFormHelper.ChooseUnionEntityFor(data);
        //        }
        //    }
        //}

        //void dtgRuleEntity_CellFormatting(object sender, CellFormattingEventArgs e)
        //{
        //    //if (e.Column.FieldName == "SuperTypeEntities" || e.Column.FieldName == "UnionTypeEntities")
        //    //{
        //    //    if (e.CellElement.Children.Count == 0) 
        //    //    { 
        //    //        progressBarElement = new RadProgressBarElement(); 
        //    //        e.CellElement.Children.Add(progressBarElement);
        //    //        progressBarElement.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; 
        //    //    } 
        //    //    else {
        //    //        progressBarElement = e.CellElement.Children[0] as RadProgressBarElement; 
        //    //    } 


        //    //    GridCommandCellElement commandCell = e.CellElement as GridCommandCellElement;
        //    //    if (e.CellElement.Value != null && e.CellElement.Value.ToString() != "")
        //    //    {
        //    //        commandCell.Text = e.CellElement.Value + "";
        //    //        commandCell.CommandButton.Visibility = ElementVisibility.Visible;
        //    //        commandCell.CommandButton.Text = e.CellElement.Value + "";
        //    //    }
        //    //    else
        //    //        e.CellElement.Children.Clear();//.Enabled=false;//.Visibility = ElementVisibility.Collapsed;
        //    //}
        //}









        //private void button2_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //void helper_ItemGenerationEvent(object sender, SimpleGenerationInfoArg e)
        //{

        //    SetInfo(e.TotalProgressCount, e.CurrentProgress, e.Title);
        //}


        //internal void SetRelationships(List<RelationshipDTO> list)
        //{
        //    dtgRelationships.ItemsSource = list;
        //}
        //private void btnRefreshRelationships_Click(object sender, RoutedEventArgs e)
        //{
        //    MyFormHelper.GetRelationships(databaseID);
        //}



        //void helper_NDRelationGenerationStarted(object sender, SimpleGenerationInfoArg e)
        //{
        //    lblInfo.Header = "Relation '" + e.Title + "' is being generated.";
        //    lblInfo.Refresh();
        //}









        //void bizRuleImposedEvent(object sender, SimpleGenerationInfoArg e)
        //{
        //    SetInfo(e.TotalProgressCount, e.CurrentProgress, e.Title);
        //}











        //   DispatcherTimer infoTimer = new DispatcherTimer();
        //     int timerCounter = 0;
        public string Info { set; get; }
        //public int ProgressBarMax { set; get; }
        //public int ProgressBarValue { set; get; }
        public void SetInfo(int totalProgressCount, int currentProgress, string info)
        {
            //if (!infoTimer.IsEnabled)
            //{
            //    timerCounter = 0;
            //    //infoTimer.Start();
            //}
            //ProgressBarMax = totalProgressCount;
            //ProgressBarValue = currentProgress;
            Info = info;

            //if (ProgressBarMax == ProgressBarValue)
            //    infoTimer.Stop();
        }




        private void AddPane(UIElement frm, string title)
        {
            RadPane pane = null;
            foreach (var item in pnlForms.Items)
            {
                if ((item as RadPane).Header.ToString() == title)
                {
                    pane = item as RadPane;
                    break;
                }
            }
            if (pane == null)
            {
                pane = new RadPane();
                pane.Content = frm;
                pane.Header = title;
                pane.IsDockable = false;
                pane.CanFloat = false;
                pane.CanUserPin = false;
                pnlForms.Items.Add(pane);
                pane.CanUserClose = true;
            }
            else
                pane.IsSelected = true;
        }


        //private void btnDBConnector_Click(object sender, RoutedEventArgs e)
        //{
        //    ShowDatabaseConnectionForm();
        //}
        //void infoTimer_Tick(object sender, EventArgs e)
        //{
        //    timerCounter++;
        //    if (timerCounter >= 50000)
        //    {
        //        infoTimer.Stop();
        //    }
        //    progressBar1.Maximum = ProgressBarMax;
        //    progressBar1.Value = ProgressBarValue;
        //    lblInfo.Text = Info;
        //}


































        //private void pageViewMain_SelectedPageChanged(object sender, RoutedEventArgs e)
        //{
        //    return;
        //    if (MyFormHelper != null)
        //        if (pageViewMain.SelectedPage == pageImportTables)
        //        {
        //            if (dtgTables.ItemsSource == null)
        //                MyFormHelper.GetTables(databaseID);
        //        }
        //        else if (pageViewMain.SelectedPage == pageImportUniqueConstraints)
        //        {
        //            if (dtgUniqueConstraint.ItemsSource == null)
        //                MyFormHelper.GetUniqueConstraints(databaseID);
        //        }
        //        else if (pageViewMain.SelectedPage == pageGenerateDefaultEntities)
        //        {
        //            if (dtgDefaultEntity.ItemsSource == null)
        //                MyFormHelper.GetEntities(databaseID);
        //        }
        //        //else if (pageViewMain.SelectedPage == pageImportRelationships)
        //        //{
        //        //    if (dtgRelationships.ItemsSource == null)
        //        //        MyFormHelper.GetRelationships(databaseID);
        //        //}
        //        else if (pageViewMain.SelectedPage == pageImposeEntityRules)
        //        {
        //            if (dtgRuleEntity.ItemsSource == null)
        //                MyFormHelper.GetRuleEntities(databaseID);
        //        }
        //        else if (pageViewMain.SelectedPage == pageImposeRelationshipRules)
        //        {
        //            if (dtgRelationships.ItemsSource == null)
        //                MyFormHelper.GetRuleRelationships(databaseID);
        //        }
        //}





    }
    public enum FormMode
    {
        Initialize,
        ConnectionDetailsEntered,
        ConnectionSucceed,
        ConnectionFailed,
        ConnectionEdit
    }
    public enum GridViewColumnType
    {
        Text,
        Numeric,
        CheckBox,
        Command,
        Color,
        Link,
        Enum
    }

}
