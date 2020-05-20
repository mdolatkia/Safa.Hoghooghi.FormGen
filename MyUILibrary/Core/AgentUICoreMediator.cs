
using CommonDefinitions.UISettings;
using ModelEntites;
using MyDataManagerService;
using MyFormulaManagerService;
using MyInterfaces;
using MyRelationshipDataManager;

using MyUILibrary;
using MyUILibrary.DataViewArea;
using MyUILibrary.EntityArea;

using MyWorkflowService;

using ProxyLibrary;
using ProxyLibrary.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyUILibraryInterfaces.DataMenuArea;
using MyUILibraryInterfaces.GridViewArea;
using MyUILibrary.WorkflowArea;
using MyDataManager;
using MyUILibraryInterfaces.LogReportArea;
using MyUILibraryInterfaces.DataViewArea;

namespace MyUILibrary
{
    public class AgentUICoreMediator : IAgentUICoreMediator, IAG_RequestManager
    {
        public RelationshipTailDataManager RelationshipTailDataManager = new RelationshipTailDataManager();
        public RelationshipManagerService RelationshipManager = new RelationshipManagerService();
        public RelationshipDataManager RelationshipDataManager = new RelationshipDataManager();
        public OrganizationManagerService organizationManager = new OrganizationManagerService();
        public UserInfo UserInfo { set; get; }
        public UserManagerService userManagerService = new UserManagerService();
        public WorkflowService workflowService = new WorkflowService();
        public LogManagerService logManagerService = new LogManagerService();
        public SecurityManagerService securityManagerService = new SecurityManagerService();
        //SecurityService securityManager = new SecurityService();
        public EntityUICompositionService entityUICompositionService = new EntityUICompositionService();
        public TableDrivedEntityManagerService tableDrivedEntityManagerService = new TableDrivedEntityManagerService();
        public EntityValidationManagerService entityValidationManagerService = new EntityValidationManagerService();
        //public PackageManagerService packageManager = new PackageManagerService();
        public RelationshipFilterManagerService relationshipFilterManagerService = new RelationshipFilterManagerService();
        public FormulaManagerService formulaManager = new FormulaManagerService();
        public RequestRegistrationService requestRegistration = new RequestRegistrationService();
        public NavigationTreeManagerService navigationTreeManagerService = new NavigationTreeManagerService();
        //CodeFunctionManagerService codeFunctionManager = new CodeFunctionManagerService();
        public EntityStateManagerService entityStateManagerService = new EntityStateManagerService();
        public EntityCommandManagerService entityCommandManagerService = new EntityCommandManagerService();
        public DatabaseFunctionManagerService DatabaseFunctionManager = new DatabaseFunctionManagerService();
        public StateManagerService StateManager = new StateManagerService();
        public LetterManagerService LetterManager = new LetterManagerService();
        public DataItemManagerService DataItemManager = new DataItemManagerService();
        public DataViewManagerService DataViewManager = new DataViewManagerService();
        public EntityListViewManagerService EntityListViewManager = new EntityListViewManagerService();
        public DataSearchManagerService DataSearchManager = new DataSearchManagerService();
        public GridViewManagerService GridViewManager = new GridViewManagerService();
        public DataLinkManagerService DataLinkManager = new DataLinkManagerService();
        public ReportManagerService ReportManager = new ReportManagerService();
        public DataMenuManagerService DataMenuManager = new DataMenuManagerService();
        public ArchiveManagerService ArchiveManager = new ArchiveManagerService();
        private static AgentUICoreMediator instance;


        //private Singleton() { }
        public static AgentUICoreMediator GetAgentUICoreMediator
        {
            get
            {
                if (instance == null)
                {
                    instance = new AgentUICoreMediator();
                }
                return instance;
            }
        }







        //internal RR_ReportSourceResult SendReportSourceRequest(RR_ReportSourceRequest request)
        //{
        //    return RequestManager.SendRequest(request) as RR_ReportSourceResult;
        //}

        public AG_RequestManager RequestManager
        {
            get;
            set;
        }
        private AgentUICoreMediator()
        {
            RequestManager = new AG_RequestManager();
            //به محض شروع شدن برنامه تنظیمات پیش فرض دریافت میشود
            //UISettingHelper.GetDefaultSettings();
        }
        I_Login LoginForm { set; get; }

        public void SetUIManager(IAgentUIManager uiMnager)
        {
            UIManager = uiMnager;
            //UIManager.NavigationItemSelected += UIManager_NavigationItemSelected;
            UIManager.SearchTextChanged += UIManager_SearchTextChanged;
        }



        //  bool loginSucceeded = false;
        public void StartApp()
        {
           
            LoginForm = UIManager.GetLoginForm();
            LoginForm.LoginRequested += LoginForm_LoginRequested;
            LoginForm.ShowForm();

        }
        private void LoginForm_LoginRequested(object sender, LoginRequestedArg e)
        {
            var result = AgentUICoreMediator.GetAgentUICoreMediator.securityManagerService.Login(e.UserName, e.Password);
            if (result.Successful)
            {
                UIManager.PrepareMainForm();
                UserInfo = ConveSecurityUserInfo(result.UserID);
                SetUserInfo();
                AddGeneralToolsMenu();

                UIManager.ShowMainForm();
                ShowNavigationTree();
                //UIManager.NavigationTreeRequested += UIManager_NavigationTreeRequested;
                //UIManager.DatabaseListRequested += UIManager_DatabaseListRequested;

                //if (AgentHelper.GetAppMode() != AppMode.Paper)
                ShowCartable();
                LoginForm.Close();
                //  loginSucceeded = true;
            }
            else
            {
                LoginForm.ShowMessage(result.Message);
                //  loginSucceeded = false;
            }
        }

        private void AddGeneralToolsMenu()
        {
            I_MainFormMenu workflowCreateMenu = UIManager.AddMainFormMenu("ایجاد جریان کار", "images\\createworkflow.png");
            workflowCreateMenu.Clicked += WorkflowCreateMenu_Clicked;
            I_MainFormMenu workflowCartable = UIManager.AddMainFormMenu("کارتابل", "images\\file.png");
            workflowCartable.Clicked += WorkflowCartable_Clicked;

            I_MainFormMenu securitySettings = UIManager.AddMainFormMenu("تنظیمات امنیتی", "images\\security.png");
            securitySettings.Clicked += SecuritySettings_Clicked;
            I_MainFormMenu workflowReportCartable = UIManager.AddMainFormMenu("گزارش جریان کار", "images\\archiveFolder1.png");
            workflowReportCartable.Clicked += WorkflowReportCartable_Clicked;
            I_MainFormMenu logReport = UIManager.AddMainFormMenu("گزارش لاگ", "images\\zoom_extend.png");
            logReport.Clicked += LogReport_Clicked;
            I_MainFormMenu archiveMenu = UIManager.AddMainFormMenu("آرشیو", "images\\archive.png");
            archiveMenu.Clicked += ArchiveMenu_Clicked;
            I_MainFormMenu letterMenu = UIManager.AddMainFormMenu("نامه ها", "images\\document.png");
            letterMenu.Clicked += LetterMenu_Clicked;
            I_MainFormMenu dataViewMenu = UIManager.AddMainFormMenu("نمایش داده ها", "images\\folder.png");
            dataViewMenu.Clicked += DataViewMenu_Clicked;
            I_MainFormMenu gridViewMenu = UIManager.AddMainFormMenu("گرید داده ها", "images\\folder.png");
            gridViewMenu.Clicked += GridViewMenu_Clicked;
            I_MainFormMenu dataLinkMenu = UIManager.AddMainFormMenu("لینک داده ها", "images\\folder.png");
            dataLinkMenu.Clicked += DataLinkMenu_Clicked;

            I_MainFormMenu exitMenu = UIManager.AddMainFormMenu("خروج", "images\\close.png");
            exitMenu.Clicked += ExitMenu_Clicked;

            I_MainFormMenu reLoginMenu = UIManager.AddMainFormMenu("ورود مجدد", "images\\refresh.png");
            reLoginMenu.Clicked += ReLoginMenu_Clicked;
        }

        private void ReLoginMenu_Clicked(object sender, EventArgs e)
        {
            //   AgentUICoreMediator.GetAgentUICoreMediator.UIManager.StartApp();
            StartApp();
        }

        private void ExitMenu_Clicked(object sender, EventArgs e)
        {

            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseApplication(true);
        }

        private void WorkflowReportCartable_Clicked(object sender, EventArgs e)
        {
            ShowWorkflowReportArea(new WorkflowReportAreaInitializer(), "گزارش جریان کار", false);
        }

        private void WorkflowCartable_Clicked(object sender, EventArgs e)
        {
            if (!AgentUICoreMediator.GetAgentUICoreMediator.UIManager.PaneIsOpen("کارتابل"))
            {
                CartableArea area = new CartableArea();
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowPane(area.View, "کارتابل");
            }
            else
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ActivatePane("کارتابل");
        }

        private void DataLinkMenu_Clicked(object sender, EventArgs e)
        {
            ShowDataLinkArea(0, 0, false, "لینک داده ها", null);
        }

        private void GridViewMenu_Clicked(object sender, EventArgs e)
        {
            ShowDataViewGridViewArea(0, "گرید داده", false, true, false, null, false, 0, null, null);
        }

        private void DataViewMenu_Clicked(object sender, EventArgs e)
        {
            ShowDataViewGridViewArea(0, "گرید داده", false, true, true, null, false, 0, null, null);
        }

        private void LetterMenu_Clicked(object sender, EventArgs e)
        {
            ShowLetterArea(0, "نامه ها", false);
        }

        private void ArchiveMenu_Clicked(object sender, EventArgs e)
        {
            ShowArchiveArea(0, "آرشیو", false);
        }

        private void LogReport_Clicked(object sender, EventArgs e)
        {
            ShowLogReportArea(new LogReportAreaInitializer(), "گزارش لاگ", true);
        }

        private void SecuritySettings_Clicked(object sender, EventArgs e)
        {
            var SecuritySetting = UIManager.GetSecuritySettingForm();

            SecuritySetting.AdminOrganizationPostsConfirmed += SecuritySetting_AdminOrganizationPostsConfirmed;
            SecuritySetting.AssignedOrganizationPostsConfirmed += SecuritySetting_AssignedOrganizationPostsConfirmed1;
            SecuritySetting.OrganizationPostSearchChanged += SecuritySetting_OrganizationPostSearchChanged;
            SecuritySetting.SearchedOrganizationPostSelected += SecuritySetting_SearchedOrganizationPostSelected;

            SecuritySetting.LoadAssignedOrganizationPosts(UserInfo.AssignedOrganizationPosts);

            if (UserInfo.AssignedOrganizationPosts.Any(x => x.IsAdmin || x.IsSuperAdmin))
            {
                SecuritySetting.ShowAdminTab = true;

                if (UserInfo.AssignedOrganizationPosts.Any(x => x.IsSuperAdmin))
                {
                    SecuritySetting.ShowByPassSecurityCheckBox = true;
                }
                else
                    SecuritySetting.ShowByPassSecurityCheckBox = false;

                if (UserInfo.AdminSecurityInfo != null)
                {
                    SecuritySetting.ConfirmedOrganizatoinPosts = UserInfo.AdminSecurityInfo.SelectedOrganizationPosts;
                    SecuritySetting.ByPassSecurityCheckBoxValue = UserInfo.AdminSecurityInfo.ByPassSecurity;
                }
                //else
                //{


                //}

            }
            else
                SecuritySetting.ShowAdminTab = false;

            //}
            //else
            //{
            //    //SecuritySetting.SelectAssignedOrganizations(UserInfo.DefaultOrganization);


            //}
            UIManager.GetDialogWindow().ShowDialog(SecuritySetting, "تنظیمات امنیتی");
        }

        private void WorkflowCreateMenu_Clicked(object sender, EventArgs e)
        {
            ShowWorkflowRequestArea();
        }



        private UserInfo ConveSecurityUserInfo(int userID)
        {
            var user = organizationManager.GetUser(userID);
            UserInfo result = new MyUILibrary.UserInfo();
            result.ID = user.ID;
            result.FirstName = user.FirstName;
            result.LastName = user.LastName;
            result.UserName = user.UserName;
            result.AssignedOrganizationPosts = organizationManager.GetOrganizationPosts(userID);
            return result;
        }

        //List<OrganizationDTO> allOrganizations { set; get; }



        internal void ShowWorkflowDaigram(int requestID)
        {
            WorkflowDiagramArea area = new WorkflowArea.WorkflowDiagramArea(requestID);
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(area.View, "دیاگرام", Enum_WindowSize.Maximized);

        }

        private void SecuritySetting_SearchedOrganizationPostSelected(object sender, OrganizationPostSelectedArg e)
        {
            var confirmedposts = (sender as I_SecuritySetting).ConfirmedOrganizatoinPosts;
            if (confirmedposts == null)
                confirmedposts = new List<OrganizationPostDTO>();
            foreach (var item in e.SelectedOrganizationPosts)
            {
                if (!confirmedposts.Any(x => x.ID == item.ID))
                {
                    confirmedposts.Add(item);
                }
            }
            (sender as I_SecuritySetting).ConfirmedOrganizatoinPosts = confirmedposts;
        }

        private void SecuritySetting_OrganizationPostSearchChanged(object sender, OrganizationPostSearchArg e)
        {
            if (UserInfo.AssignedOrganizationPosts.Any(x => x.IsSuperAdmin))
            {
                (sender as I_SecuritySetting).LoadSearchedAdminOrganizationPosts(organizationManager.GetOrganizationPosts(e.SearchText));
            }
            else
            {
                var adminPosts = UserInfo.AssignedOrganizationPosts.Where(x => x.IsAdmin);
                if (adminPosts.Any())
                {
                    var adminOrgIds = adminPosts.Select(x => x.OrganizationID).ToList();
                    (sender as I_SecuritySetting).LoadSearchedAdminOrganizationPosts(organizationManager.GetOrganizationPostsByOrganizationIds(e.SearchText, adminOrgIds));
                }
            }
        }



        private void SecuritySetting_AssignedOrganizationPostsConfirmed1(object sender, EventArgs e)
        {
            if (UserInfo.AdminSecurityInfo != null)
            {
                UserInfo.AdminSecurityInfo.IsActive = false;
                UIManager.CloseDialog(sender as I_SecuritySetting);
                ShowNavigationTree();
            }
            else
                UIManager.CloseDialog(sender as I_SecuritySetting);
        }
        private void SecuritySetting_AdminOrganizationPostsConfirmed(object sender, AdminOrganizationPostConfirmedArg e)
        {
            if (UserInfo.AdminSecurityInfo == null)
                UserInfo.AdminSecurityInfo = new AdminSecurityInfo();
            UserInfo.AdminSecurityInfo.IsActive = true;
            UserInfo.AdminSecurityInfo.ByPassSecurity = (sender as I_SecuritySetting).ByPassSecurityCheckBoxValue;
            UserInfo.AdminSecurityInfo.SelectedOrganizationPosts = (sender as I_SecuritySetting).ConfirmedOrganizatoinPosts;
            UIManager.CloseDialog(sender as I_SecuritySetting);
            ShowNavigationTree();
        }


        //private void SecuritySetting_AssignedOrganizationConfirmed(object sender, AssignedOrganizationConfirmedArg e)
        //{
        //    UserInfo.DefaultOrganization = e.Organization;
        //    if (UserInfo.AdminSecurityInfo != null)
        //        UserInfo.AdminSecurityInfo.IsActive = false;
        //    {
        //        UIManager.CloseDialog(sender as I_SecuritySetting);
        //        ShowNavigationTree();

        //    }
        //}

        //private void SecuritySetting_AssignedOrganizationChanged(object sender, AssignedOrganizationChangedArg e)
        //{
        //    var SecuritySetting = sender as I_SecuritySetting;
        //    SecuritySetting.ShowAssignedRoles(e.Organization.Roles);

        //}

        //private void SecuritySetting_AdminOrganizationsConfirmed(object sender, AdminOrganizationConfirmedArg e)
        //{

        //}

        //private void SecuritySetting_AdmindOrganizationChanged(object sender, AdminOrganizationChangedArg e)
        //{
        //    var roles = securityHelper.GetAllOrganizationRoles(e.Organization.ID);
        //    if (UserInfo.AdminSecurityInfo != null)
        //    {
        //        foreach (var id in UserInfo.AdminSecurityInfo.RoleIds)
        //        {
        //            var role = roles.FirstOrDefault(x => x.ID == id);
        //            if (role != null)
        //                role.Selected = true;
        //        }
        //    }
        //    var SecuritySetting = sender as I_SecuritySetting;
        //    SecuritySetting.LoadMultiSelectAdminRoles(roles);
        //}

        private void SetUserInfo()
        {
            string userName = UserInfo.UserName;
            string organizationName = "";
            string roles = "";
            //var defaultOrganization = UserInfo.DefaultOrganization;
            //if (defaultOrganization != null)
            //{
            //    organizationName = defaultOrganization.Name;
            foreach (var item in UserInfo.AssignedOrganizationPosts)
            {
                roles += (roles == "" ? "" : ",") + item.Name;
            }
            //}
            UIManager.SetUserInfo(userName, roles);
        }
        public DR_Requester GetRequester()
        {
            DR_Requester requester = new DR_Requester(2);
            requester.Identity = UserInfo.ID;
            if (UserInfo.AdminSecurityInfo != null && UserInfo.AdminSecurityInfo.IsActive)
            {
                requester.Posts = UserInfo.AdminSecurityInfo.SelectedOrganizationPosts;
                requester.SkipSecurity = UserInfo.AdminSecurityInfo.ByPassSecurity;
            }
            else
            {
                requester.Posts = UserInfo.AssignedOrganizationPosts;
                //requester.SkipSecurity = false;
            }
            if (requester.Posts != null)
                requester.PostIds = requester.Posts.Select(x => x.ID).ToList();
            return requester;
        }
        List<NavigationItemDTO> allNavigationTreeItems { set; get; }
        private void ShowNavigationTree()
        {
            allNavigationTreeItems = navigationTreeManagerService.GetNavigationTree(GetRequester());
            UIManager.ClearNavigationTree();
            foreach (var item in allNavigationTreeItems.Where(x => x.ParentItem == null))
            {
                AddNavigationTree(null, allNavigationTreeItems, item);
            }
        }

        private void AddNavigationTree(I_NavigationMenu parent, List<NavigationItemDTO> allItems, NavigationItemDTO item)
        {
            if (!string.IsNullOrEmpty(item.Name))
                item.Tooltip += (!string.IsNullOrEmpty(item.Tooltip) ? Environment.NewLine : "") + item.Name;
            item.Menu = UIManager.AddNavigationTree(parent, item, true);
            item.Menu.Clicked += (sender, e) => Menu_Clicked(sender, e, item);
            foreach (var cItem in allItems.Where(x => x.ParentItem == item))
            {
                AddNavigationTree(item.Menu, allItems, cItem);
            }
        }

        private void Menu_Clicked(object sender, EventArgs e, NavigationItemDTO item)
        {
            //گرفتن موجودیت بوسیله شناسه
            NavigationMenuClicked(item);
        }

        private void NavigationMenuClicked(NavigationItemDTO item)
        {
            if (item.ObjectCategory == DatabaseObjectCategory.Folder)
                return;
            else if (item.ObjectCategory == DatabaseObjectCategory.Entity)
            {
                ShowEditEntityArea(Convert.ToInt32(item.ObjectIdentity), false);
            }
            else if (item.ObjectCategory == DatabaseObjectCategory.Report)
            {
                ShowReportAreaFromMenu(Convert.ToInt32(item.ObjectIdentity), false);
            }
            else if (item.ObjectCategory == DatabaseObjectCategory.Archive)
            {
                ShowArchiveArea(Convert.ToInt32(item.ObjectIdentity), item.Title, false);
            }
            else if (item.ObjectCategory == DatabaseObjectCategory.Letter)
            {
                ShowLetterArea(Convert.ToInt32(item.ObjectIdentity), item.Title, false);
            }
            else if (item.ObjectCategory == DatabaseObjectCategory.DataView)
            {
                ShowDataViewGridViewArea(Convert.ToInt32(item.ObjectIdentity), item.Title, false, true, true, null, false, 0, null, null);
            }
            else if (item.ObjectCategory == DatabaseObjectCategory.GridView)
            {
                ShowDataViewGridViewArea(Convert.ToInt32(item.ObjectIdentity), item.Title, false, true, false, null, false, 0, null, null);
            }
        }



        private void UIManager_SearchTextChanged(object sender, string text)
        {
            if (text == "")
            {
                UIManager.SearchNavigationTreeVisiblity = false;
                UIManager.NavigationTreeVisiblity = true;

            }
            else
            {
                UIManager.SearchNavigationTreeVisiblity = true;
                UIManager.NavigationTreeVisiblity = false;
                UIManager.ClearSearchNavigationTree();
                var validItems = allNavigationTreeItems.Where(x => x.ObjectCategory != DatabaseObjectCategory.Folder &&
               ((x.Title != null && x.Title.ToLower().Contains(text.ToLower())) || (x.Name != null && x.Name.ToLower().Contains(text.ToLower()))));
                foreach (var item in validItems)
                {
                    var node = UIManager.AddSearchNavigationTree(null, item, true);
                    node.Clicked += (sender1, e1) => MenuSearch_Clicked(sender1, e1, item);
                }
            }
        }

        private void MenuSearch_Clicked(object sender1, EventArgs e1, NavigationItemDTO item)
        {
            NavigationMenuClicked(item);
        }

        public void ShowWorkflowRequestArea()
        {
            WorkflowCreateRequestArea area = new WorkflowArea.WorkflowCreateRequestArea();
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(area.View, "ایجاد جریان کار", Enum_WindowSize.Big);

        }

        public void ShowCartable()
        {
            CartableArea area = new CartableArea();
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowPane(area.View, "کارتابل");
        }



        //public static List<DP_DataRepository> SerachDataViewFromParentRelation(int relationshipID, DP_DataRepository parentRelationData)
        //{
        //    if (parentRelationData != null)
        //    {
        //        RelationshipManagerService relationshipManager = new RelationshipManagerService();
        //        DR_SearchViewRequest request = new DR_SearchViewRequest();
        //        request.Requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();
        //        request.SearchDataItems = relationshipManager.GetSearchDataItemByRelationship(RelationshipSreachType.SecondSideBasedOnFirstRelationshhipColumn, parentRelationData, relationshipID, AgentUICoreMediator.GetAgentUICoreMediator.GetRequester());
        //        request.EntityID = request.SearchDataItems.TargetEntityID;
        //        request.Requester.SkipSecurity = true;
        //        return AgentUICoreMediator.GetAgentUICoreMediator.SendSearchViewRequest(request).ResultDataItems;
        //    }
        //    return null;
        //}

        //public static List<DP_DataRepository> SearchDataForEditFromProperties(int entityID, List<EntityInstanceProperty> relationColumns)
        //{

        //    //DP_DataRepository sItem = new DP_DataRepository();
        //    //sItem.TargetEntityID = entityID;
        //    //foreach (var item in relationColumns)
        //    //{
        //    //    sItem.Properties.Add(item);
        //    //}


        //}




















        //void UIManager_NavigationTreeRequested(object sender, Arg_NavigationTreeRequest e)
        //{

        //}
        //void UIManager_NavigationItemSelected(object sender, Arg_NavigationItemRequest e)
        //{

        //}
        private void ShowReportAreaFromMenu(int reportID, bool dialog)
        {
            var report = ReportManager.GetReport(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), reportID);
            if (report == null)
            {
                UIManager.ShowInfo("دسترسی به گزارش به شناسه" + " " + reportID + " " + "امکانپذیر نمی باشد", "", Temp.InfoColor.Red);
                return;
            }
            //ShowSearchableReportArea(report, dialog, userCanChange, null, report.PreDefinedSearch);
            if (report.ReportType == ReportType.DirectReport)
            {
                ShowDirectReport(reportID, dialog, null);
            }
            else if (report.ReportType == ReportType.SearchableReport)
            {
                ShowSearchableReportArea(report, dialog, null, true, false, null, null);
            }

        }
        public void ShowDirectReport(int reportID, bool dialog, DP_DataView dataItem)
        {
            DirectReportAreaInitializer areaInitializer = new DirectReportAreaInitializer();
            areaInitializer.ReportID = reportID;
            areaInitializer.DataInstance = dataItem;

            var area = new DirectReportArea(areaInitializer);
            if (area.MainView != null)
            {
                if (!dialog)
                    UIManager.ShowPane(area.MainView, "گزارش");
                else
                    UIManager.GetDialogWindow().ShowDialog(area.MainView, "گزارش", Enum_WindowSize.Maximized);
            }
        }
        public void ShowSearchableReportArea(EntityReportDTO report, bool dialog, DP_SearchRepository initializeSearchRepository, bool userCanChangeSearch, bool showInitializeSearchRepository, I_DataArea hostDataViewArea, I_DataViewItem defaultDataViewItem)
        {
            if (report.SearchableReportType == SearchableReportType.DataView)
            {
                var dvreport = ReportManager.GetDataViewReport(GetRequester(), report.ID);
                if (dvreport == null)
                {
                    UIManager.ShowInfo("دسترسی به گزارش به شناسه" + " " + report.ID + " " + "امکانپذیر نمی باشد", "", Temp.InfoColor.Red);
                    return;
                }
                ShowDataViewGridViewArea(report.TableDrivedEntityID, report.ReportTitle, dialog, userCanChangeSearch, true, initializeSearchRepository, showInitializeSearchRepository, dvreport.DefaultEntityListViewID, hostDataViewArea, defaultDataViewItem);

            }
            else if (report.SearchableReportType == SearchableReportType.GridView)
            {
                var dvreport = ReportManager.GetGridViewReport(GetRequester(), report.ID);
                if (dvreport == null)
                {
                    UIManager.ShowInfo("دسترسی به گزارش به شناسه" + " " + report.ID + " " + "امکانپذیر نمی باشد", "", Temp.InfoColor.Red);
                    return;
                }
                ShowDataViewGridViewArea(report.TableDrivedEntityID, report.ReportTitle, dialog, userCanChangeSearch, false, initializeSearchRepository, showInitializeSearchRepository, dvreport.DefaultEntityListViewID, hostDataViewArea, defaultDataViewItem);
            }
            else if (report.SearchableReportType == SearchableReportType.ExternalReport)
            {
                ShowExternalReport(report, userCanChangeSearch, initializeSearchRepository, showInitializeSearchRepository, dialog);
            }
            else if (report.SearchableReportType == SearchableReportType.ChartReport ||
                report.SearchableReportType == SearchableReportType.CrosstabReport ||
                report.SearchableReportType == SearchableReportType.ListReport)

            {
                ShowListChartCrosstabReport(report, report.SearchableReportType, userCanChangeSearch, initializeSearchRepository, showInitializeSearchRepository, dialog);
            }
        }

        //private void ShowSearchableReportArea(EntitySearchableReportDTO report, bool dialog, bool userCanChange, DP_SearchRepository initializeSearchRepository, EntityPreDefinedSearchDTO preDefinedSearch)
        //{

        //}




        //private void ShowDirectReport(int iD, string reportTitle, DP_SearchRepository initializeSearchRepository, bool dialog)
        //{

        //    ////اگر ویو بود چی؟
        //    //var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();
        //    //var requestSearchView = new DR_SearchViewRequest(requester, initializeSearchRepository);
        //    //var result = requestRegistration.SendSearchViewRequest(requestSearchView);
        //    //if (result.Result != Enum_DR_ResultType.ExceptionThrown)
        //    //{
        //    //    var foundItem = result.ResultDataItems;
        //    //    if (foundItem.Count > 1)
        //    //        AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("بیش از یک داده برای این نوع گزارش موجود است!", "", Temp.InfoColor.Red);
        //    //    else if (foundItem.Count == 0)
        //    //        AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("داده ای جهت نمایش موجود نمی باشد", "", Temp.InfoColor.Red);
        //    //    else
        //    //        ShowDirecrReport(iD, foundItem[0].KeyProperties);
        //    //}
        //    //else
        //    //{
        //    //    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo(result.Message, "", Temp.InfoColor.Red);
        //    //    return;
        //    //}

        //}
        //internal void ShowDirecrReport(int iD, List<EntityInstanceProperty> keyProperties)
        //{

        //}
        private void ShowListChartCrosstabReport(EntityReportDTO report, SearchableReportType reportType, bool userCanChange, DP_SearchRepository initializeSearchRepository, bool showInitializeSearchRepository, bool dialog)
        {
            InternalReportAreaInitializer initializer = new InternalReportAreaInitializer();
            initializer.ReportID = report.ID;
            initializer.ReportType = reportType;
            initializer.UserCanChangeSearch = userCanChange;
            initializer.EntityID = report.TableDrivedEntityID;
            initializer.InitialSearchRepository = initializeSearchRepository;
            initializer.Title = report.ReportTitle;
            initializer.ShowInitializeSearchRepository = showInitializeSearchRepository;

            I_InternalReportArea reportArea = new InternalReportArea(initializer);
            if (reportArea.MainView != null)
            {
                if (!dialog)
                    UIManager.ShowPane(reportArea.MainView, report.ReportTitle);
                else
                    UIManager.GetDialogWindow().ShowDialog(reportArea.MainView, report.ReportTitle, Enum_WindowSize.Maximized);
            }
        }

        private void ShowExternalReport(EntityReportDTO report, bool userCanChange, DP_SearchRepository initializeSearchRepository, bool showInitializeSearchRepository, bool dialog)
        {
            ExternalReportAreaInitializer initializer = new ExternalReportAreaInitializer();
            initializer.ReportID = report.ID;
            initializer.UserCanChangeSearch = userCanChange;
            initializer.EntityID = report.TableDrivedEntityID;
            initializer.Title = report.ReportTitle;
            initializer.InitialSearchRepository = initializeSearchRepository;
            initializer.ShowInitializeSearchRepository = showInitializeSearchRepository;
            I_ExternalReportArea reportArea = new ExternalReportArea(initializer);
            if (reportArea.View != null)
            {
                if (!dialog)
                    UIManager.ShowPane(reportArea.MainView, report.ReportTitle);
                else
                    UIManager.GetDialogWindow().ShowDialog(reportArea.View, report.ReportTitle, Enum_WindowSize.Maximized);
            }
        }


        //private RR_ReportResult GetReport(RR_ReportRequest request, bool withDetails)
        //{
        //    request.Requester = GetRequester();
        //    var result = packageManager.GetReport(request, withDetails);
        //    return result;
        //}

        public void ShowDataLinkArea(int entityId, int dataLinkID, bool dialog, string title, DP_DataView firstData)// چون جایی نداریم فعلا که دوتا داده انتخاب کنیم و ارتباطشون رو بخوایم, DP_DataView otherData)
        {

            var initializer = new MyUILibraryInterfaces.DataLinkArea.DataLinkAreaInitializer();
            initializer.DataLinkID = dataLinkID;
            initializer.EntityID = entityId;
            // initializer.UserCanChange = userCanChangeSearch;
            initializer.FirstDataItem = firstData;
            //   initializer.OtherDataItem = otherData;
            DataLinkArea.DataLinkArea linkArea = new DataLinkArea.DataLinkArea(initializer);
            if (linkArea.View != null)
            {
                if (!dialog)
                    UIManager.ShowPane(linkArea.View, title);
                else
                    UIManager.GetDialogWindow().ShowDialog(linkArea.View, title, Enum_WindowSize.Maximized);
            }


        }

        //public void ShowGridViewArea(int entityId, string title, bool dialog, bool userCanChangeSearch, DP_SearchRepository initializeSearchRepository = null, EntityPreDefinedSearchDTO preDefinedSearch = null, int entityListViewID = 0)
        //{
        //    //DP_SearchRepository searchRepository = new DP_SearchRepository(EditArea.AreaInitializer.EntityID);
        //    //searchRepository = searchDP;

        //    //EditArea.DataViewAreaContainer = new DataViewArea.DataViewAreaContainer();
        //    var initializer = new GridViewAreaInitializer();
        //    initializer.EntityID = entityId;
        //    initializer.Title = title;
        //    initializer.EntityListViewID = entityListViewID;
        //    initializer.PreDefinedSearch = preDefinedSearch;
        //    initializer.UserCanChangeSearch = userCanChangeSearch;
        //    initializer.SearchRepository = initializeSearchRepository;
        //    I_GridViewArea gridViewArea = new MyUILibrary.GridViewArea.GridViewArea(initializer);
        //    if (gridViewArea.MainView != null)
        //    {
        //        if (!dialog)
        //            UIManager.ShowPane(gridViewArea.MainView, title);
        //        else
        //            UIManager.GetDialogWindow().ShowDialog(gridViewArea.MainView, title, Enum_WindowSize.Maximized);
        //    }


        //}
        public void ShowDataViewGridViewArea(int entityId, string title, bool dialog, bool userCanChangeSearch, bool dataViewOrGridView, DP_SearchRepository initializeSearchRepository, bool showInitializeSearchRepository, int entityListViewID, I_DataArea hostDataViewArea, I_DataViewItem defaultDataViewItem)
        {

            if (hostDataViewArea != null)
            {
                hostDataViewArea.DataViewAreaContainer.AddDataViewAreaFromOutSide(entityId, title, initializeSearchRepository, defaultDataViewItem, true, entityListViewID);
            }
            else
            {



                var initializer = new MyUILibraryInterfaces.DataViewArea.DataViewAreaContainerInitializer();
                initializer.EntityID = entityId;
                initializer.Title = title;
                initializer.DataViewOrGridView = dataViewOrGridView;
                initializer.EntityListViewID = entityListViewID;
                //   initializer.PreDefinedSearch = preDefinedSearch;
                initializer.ShowInitializeSearchRepository = showInitializeSearchRepository;
                initializer.UserCanChangeSearchRepository = userCanChangeSearch;
                initializer.InitialSearchRepository = initializeSearchRepository;
                I_DataViewAreaContainer dataViewAreaContainer = new DataViewAreaContainer(initializer);
                if (dataViewAreaContainer.View != null)
                {
                    if (!dialog)
                        UIManager.ShowPane(dataViewAreaContainer.MainView, title);
                    else
                        UIManager.GetDialogWindow().ShowDialog(dataViewAreaContainer.View, title, Enum_WindowSize.Maximized);
                }
            }

        }
        internal void ShowMenuArea(DataMenuAreaInitializer menuInitializer)
        {
            //میشه بتدیل به گت بشه و برای هر آبجکت که استفاده میکنده ذخیره بشه
            var menuArea = new MyUILibrary.DataMenuArea.DataMenuArea(menuInitializer);
            if (menuArea != null)
            {
                menuArea.ShowMenu();
            }
        }
        public void ShowLetterArea(int entityId, string title, bool dialog, DP_DataView initializeData = null)
        {
            LettersAreaInitializer areaInitializer = new LettersAreaInitializer();
            areaInitializer.EntityID = entityId;
            areaInitializer.DataInstance = initializeData;
            //     areaInitializer.UserCanChangeDataItem = userCanChangeDataItem;

            //var entity = tableDrivedEntityManagerService.GetEntity(entityId, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships);
            //areaInitializer.LoadRelatedItems = entity.LoadLetterRelatedItems;


            var editLetterArea = new EntityLettersArea(areaInitializer);

            if (editLetterArea.MainView != null)
            {
                if (!dialog)
                    UIManager.ShowPane(editLetterArea.MainView, title);
                else
                    UIManager.GetDialogWindow().ShowDialog(editLetterArea.MainView, title, Enum_WindowSize.Maximized);
            }

        }
        public void ShowArchiveArea(int entityId, string title, bool dialog, DP_DataView initializeData = null)
        {
            ArchiveAreaInitializer areaInitializer = new ArchiveAreaInitializer();
            areaInitializer.EntityID = entityId;
            areaInitializer.DataInstance = initializeData;
            //  areaInitializer.UserCanChangeDataItem = userCanChangeDataItem;
            //if (entityId != 0)
            //{
            //    var entity = tableDrivedEntityManagerService.GetEntity(entityId, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships);
            // //   areaInitializer.LoadRelatedItems = entity.LoadArchiveRelatedItems;
            //}
            var editArchiveArea = new EditArchiveArea(areaInitializer);
            //DP_EntityRequest request = new DP_EntityRequest(GetRequester());
            //request.EntityID = entityId;
            //var entity = tableDrivedEntityManagerService.GetEntity(request, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships);
            //if (initializeData != null)
            //    editArchiveArea.ShowDataItemArchives(initializeData, entity.Entity.LoadArchiveRelatedItems);
            if (editArchiveArea.MainView != null)
            {
                if (!dialog)
                    UIManager.ShowPane(editArchiveArea.MainView, title);
                else
                    UIManager.GetDialogWindow().ShowDialog(editArchiveArea.MainView, title, Enum_WindowSize.Maximized);
            }

        }
        public void ShowWorkflowReportArea(WorkflowReportAreaInitializer areaInitializer, string title, bool dialog)
        {
            var editArchiveArea = new WorkflowReportArea(areaInitializer);
            //DP_EntityRequest request = new DP_EntityRequest(GetRequester());
            //request.EntityID = entityId;
            //var entity = tableDrivedEntityManagerService.GetEntity(request, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships);
            //if (initializeData != null)
            //    editArchiveArea.ShowDataItemArchives(initializeData, entity.Entity.LoadArchiveRelatedItems);
            if (editArchiveArea.View != null)
            {
                if (!dialog)
                    UIManager.ShowPane(editArchiveArea.View, title);
                else
                    UIManager.GetDialogWindow().ShowDialog(editArchiveArea.View, title, Enum_WindowSize.Maximized);
            }
        }

        public void ShowLogReportArea(LogReportAreaInitializer areaInitializer, string title, bool dialog)
        {
            var editArchiveArea = new LogReportArea(areaInitializer);
            //DP_EntityRequest request = new DP_EntityRequest(GetRequester());
            //request.EntityID = entityId;
            //var entity = tableDrivedEntityManagerService.GetEntity(request, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships);
            //if (initializeData != null)
            //    editArchiveArea.ShowDataItemArchives(initializeData, entity.Entity.LoadArchiveRelatedItems);
            if (editArchiveArea.View != null)
            {
                if (!dialog)
                    UIManager.ShowPane(editArchiveArea.View, title);
                else
                    UIManager.GetDialogWindow().ShowDialog(editArchiveArea.View, title, Enum_WindowSize.Maximized);
            }
        }
        public void ShowEditEntityArea(int entityId, bool dialog, DataMode dataMode = DataMode.None, List<DP_DataView> initializeData = null, Tuple<DP_DataView, EntityRelationshipTailDTO> tailDataValidation = null)
        {
            //var request = new DP_EntityRequest();
            //request.EntityID = entityId;
            //var result = GetEntity(request, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships, false, false, false, false, false, false);
            if (initializeData != null)
            {
                if (initializeData.Count > 1)
                {
                    if (dataMode == DataMode.One)
                    {
                        throw (new Exception("multiple data!"));
                    }
                }
            }
            EditEntityAreaInitializer editEntityAreaInitializer = new EditEntityAreaInitializer();
            //editEntityAreaInitializer.TemplateEntity = new TableDrivedEntityDTO();
            editEntityAreaInitializer.EntityID = entityId;
            editEntityAreaInitializer.DataMode = dataMode;
            editEntityAreaInitializer.TailDataValidation = tailDataValidation;

            var editAreaResult = EditEntityAreaConstructor.GetEditEntityArea(editEntityAreaInitializer);
            //MainEntityArea.DisableEnableChanged += MainEntityArea_DisableEnableChanged;
            //MainEntityArea.DisableEnableCommandByTypeChanged += MainEntityArea_DisableEnableCommandByTypeChanged;
            //editEntityAreaInitializer.IntracionMode = CommonDefinitions.UISettings.IntracionMode.Select;
            if (editAreaResult.Item1 == null)
            {
                if (!string.IsNullOrEmpty(editAreaResult.Item2))
                    UIManager.ShowMessage(editAreaResult.Item2);

                return;
            }
            var MainEntityArea = editAreaResult.Item1;
            MainEntityArea.SetAreaInitializer(editEntityAreaInitializer);
            if (initializeData != null && initializeData.Any())
            {
                var result = initializeData;
                if (result.Count > 0)
                {
                    if (MainEntityArea is I_EditEntityAreaOneData)
                    {
                        var lastData = result.Last();
                        (MainEntityArea as I_EditEntityAreaOneData).ShowDataFromExternalSource(lastData);

                    }
                    else
                    {
                        (MainEntityArea as I_EditEntityAreaMultipleData).ShowDataFromExternalSource(result);
                    }
                }
            }
            else
            {
                if (MainEntityArea is I_EditEntityAreaOneData)
                {
                    (MainEntityArea as I_EditEntityAreaOneData).ShowDataFromExternalSource(null);

                }
                else
                {
                    (MainEntityArea as I_EditEntityAreaMultipleData).ShowDataFromExternalSource(null);
                }
            }

            //if (dialog)
            //{
            //    if (dataView)
            //        AgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(DataView, SimpleEntity.Alias, Enum_WindowSize.Big);
            //    else
            //        AgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(TemporaryDisplayView, SimpleEntity.Alias, Enum_WindowSize.Big);

            //}
            //else
            //{
            object view = null;
            if (MainEntityArea is I_EditEntityAreaOneData)
            {
                if (MainEntityArea.AreaInitializer.IntracionMode == IntracionMode.CreateDirect ||
                        MainEntityArea.AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect)
                    view = (MainEntityArea as I_EditEntityAreaOneData).DataView;
                else if (MainEntityArea.AreaInitializer.IntracionMode == IntracionMode.CreateInDirect ||
                        MainEntityArea.AreaInitializer.IntracionMode == IntracionMode.CreateSelectInDirect
                        || MainEntityArea.AreaInitializer.IntracionMode == IntracionMode.Select)
                    view = (MainEntityArea as I_EditEntityAreaOneData).TemporaryDisplayView;

            }
            else if (MainEntityArea is I_EditEntityAreaMultipleData)
            {
                if (MainEntityArea.AreaInitializer.IntracionMode == IntracionMode.CreateDirect ||
                    MainEntityArea.AreaInitializer.IntracionMode == IntracionMode.CreateSelectDirect)
                    view = (MainEntityArea as I_EditEntityAreaMultipleData).DataView;
                else if (MainEntityArea.AreaInitializer.IntracionMode == IntracionMode.CreateInDirect ||
                        MainEntityArea.AreaInitializer.IntracionMode == IntracionMode.CreateSelectInDirect
                        || MainEntityArea.AreaInitializer.IntracionMode == IntracionMode.Select)
                    view = (MainEntityArea as I_EditEntityAreaMultipleData).TemporaryDisplayView;
            }
            if (view != null)
            {
                if (!dialog)
                    UIManager.ShowPane(view, MainEntityArea.SimpleEntity.Alias);
                else
                    UIManager.GetDialogWindow().ShowDialog(view, MainEntityArea.SimpleEntity.Alias, Enum_WindowSize.Maximized);
            }

            //}


            //initializer.InitializeData = initializeData;
            //EditPackageArea container = new EditPackageArea();
            //container.LoadTemplate(initializer);
            //به صورت مستقیم نمایش داده شود View باید  LoadTemplate بعد از
        }

        //public EntityUISettingDTO GetEntityUISettings(int entityID)
        //{
        //    DP_EntityUIsettingsRequest request = new DP_EntityUIsettingsRequest();
        //    request.Requester = GetRequester();
        //    request.EntityID = entityID;
        //    var result = packageManager.GetEntityUISettings(request);
        //    return result.UISetting;
        //}
        //public bool IsEntityEnabled(int entityID)
        //{
        //    return packageManager.IsEntityEnabled(entityID);
        //}
        //public AssignedPermissionDTO GetAssignedPermissions(int objectID, bool withChilds)
        //{
        //    return securityHelper.GetAssignedPermissions(GetRequester(), objectID, withChilds);
        //}
        //public AssignedPermissionDTO GetSubSystemAssignedPermissions(string objectName)
        //{
        //    return securityHelper.GetSubSystemAssignedPermissions(GetRequester(), objectName);
        //}
        //public AssignedPermissionDTO GetAssignedPermissions(int entityID, bool withChilds)
        //{
        //    DP_EntityPermissionRequest request = new DP_EntityPermissionRequest(GetRequester());
        //    //request.Requester = GetRequester();
        //    request.EntityID = entityID;
        //    request.WithChilds = withChilds;
        //    var result = packageManager.GetEntityPermissions(request);
        //    return result.Permissions;
        //}
        //public List<ConditionalPermissionDTO> GetEntityConditionalPermissions(int entityID)
        //{
        //    DP_EntityConditionalPermissionRequest request = new DP_EntityConditionalPermissionRequest(GetRequester());
        //    //request.Requester = GetRequester();
        //    request.EntityID = entityID;
        //    var result = packageManager.GetEntityConditionalPermissions(request);
        //    return result.ConditionalPermissions;
        //}


        //public List<EntityCommandDTO> GetEntityCommands(int entityID)
        //{
        //    DP_EntityCommandsRequest request = new DP_EntityCommandsRequest(GetRequester());

        //    request.EntityID = entityID;
        //    return packageManager.GetEntityCommands(request).Commands;
        //}
        //public List<EntityValidationDTO> GetEntityValidations(int entityID)
        //{
        //    DP_EntityValidationsRequest request = new DP_EntityValidationsRequest(GetRequester());

        //    request.EntityID = entityID;
        //    return packageManager.GetEntityValidations(request).Validations;
        //}

        //public List<EntityStateDTO> GetEntityStates(int entityID)
        //{
        //    DP_EntityStatesRequest request = new DP_EntityStatesRequest(GetRequester());

        //    request.EntityID = entityID;
        //    return packageManager.GetEntityStates(request).States;
        //}

        //public List<ActionActivityDTO> GetEntityActionActivities(int entityID)
        //{
        //    DP_EntityActionActivitiesRequest request = new DP_EntityActionActivitiesRequest(GetRequester());
        //    request.EntityID = entityID;
        //    return packageManager.GetEntityActionActivities(request).ActionActivities;
        //}

        //public TableDrivedEntityDTO GetFullEntity(int entityID)
        //{
        //    DP_EntityRequest request = new DP_EntityRequest(GetRequester());

        //    request.EntityID = entityID;
        //    ///***************************/ رابطه ها فعلا حذف شدند
        //    var result = tableDrivedEntityManagerService.GetEntity(request, EntityColumnInfoType.WithFullColumns, EntityRelationshipInfoType.WithRelationships);
        //    return result.Entity;
        //}

        //public TableDrivedEntityDTO GetEntityWithSimpleColumns(int entityID)
        //{
        //    DP_EntityRequest request = new DP_EntityRequest(GetRequester());

        //    request.EntityID = entityID;
        //    var result = tableDrivedEntityManagerService.GetEntity(request, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
        //    return result.Entity;
        //}

        //public List<TableDrivedEntityDTO> SearchEntities(string search )
        //{
        //    DP_SearchEntitiesRequest request = new DP_SearchEntitiesRequest(GetRequester());
        //    request.SearchText = search;
        //    var result = packageManager.SearchEntities(request);
        //    return result;
        //}
        //public TableDrivedEntityDTO GetSimpleEntity(int entityID)
        //{
        //    DP_EntityRequest request = new DP_EntityRequest(GetRequester());
        //    request.EntityID = entityID;
        //    var result = tableDrivedEntityManagerService.GetEntity(request, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships);
        //    return result.Entity;
        //}

        //public List<RelationshipFilterDTO> GetRelationshipFilters(int relationshipID)
        //{
        //    return packageManager.GetRelationshipFilters(relationshipID);
        //}

        //public RelationshipDTO GetRelationship(int relationshipID)
        //{
        //    return packageManager.GetRelationship(relationshipID);
        //}
        //void UIManager_DatabaseListRequested(object sender, EventArgs e)
        //{
        //    var request = new DP_DatabaseRequest();
        //    request.Requester = new DR_Requester();
        //    request.Requester.Name = "meysam";
        //    request.Requester.Identity = 4;
        //    var result = GetDatabaseList(request);
        //    UIManager.ShowDatabaseList(result);
        //}







        public IAgentUIManager UIManager
        {
            set;
            get;
        }

        //public DR_Requester Requester
        //{
        //    set;
        //    get;
        //}






        //public IAgentCore AgentCore
        //{
        //    set;
        //    get;
        //}







        //////webrefPackageManager.PackageManagerClient packageManager = new webrefPackageManager.PackageManagerClient();



        //public FormulaResult CalculateFormula(int formulaID, DP_DataRepository mainDataItem)
        //{
        //    return formulaManager.CalculateFormula(formulaID, mainDataItem, GetRequester());
        //}

        //public CodeFunctionResult CalculateCodeFunction(int codeFunctionID, List<DP_DataRepository> allDataItems)
        //{

        //    return codeFunctionManager.CalculateCodeFunction(codeFunctionID, allDataItems);
        //}

        //public DatabaseFunctionResult CalculateDatabaseFunction(int DatabaseFunctionID, DP_DataRepository dataItem)
        //{
        //    return DatabaseFunctionManager.GetFunctionValue(GetRequester(), DatabaseFunctionID, dataItem);
        //}

        //public StateFunctionResult CalculateStateFunction(int stateID, DP_DataRepository mainDataItem)
        //{

        //    return StateFunctionManager.CalculateFormula(stateID, mainDataItem, GetRequester());
        //}

        //internal bool UserHasRoleCondition(SecuritySubjectDTO securitySubject, bool hasNotRole)
        //{//سازمان ایا چی پس؟
        //    var requester = GetRequester();
        //    bool? any = null;
        //    if (securitySubject.Type == SecuritySubjectType.OrganizationPost)
        //    {
        //        any = requester.Posts.Any(x => x.ID == securitySubject.ID);
        //    }
        //    else if (securitySubject.Type == SecuritySubjectType.OrganizationTypeRoleType)
        //    {
        //        any = !requester.Posts.Any(x => x.OrganizationTypeRoleTypeID == securitySubject.ID);
        //    }
        //    else if (securitySubject.Type == SecuritySubjectType.RoleType)
        //    {
        //        any = !requester.Posts.Any(x => x.RoleTypeID == securitySubject.ID);
        //    }
        //    if (any == null)
        //        return false;
        //    else
        //    {
        //        if (hasNotRole)
        //            return !any.Value;
        //        else
        //            return any.Value;
        //    }

        //}

        //public FormulaDTO GetFormula(int formulaID)
        //{

        //    return formulaManager.GetFormula(formulaID);
        //}
        ////public DP_ResultDatabaseList GetDatabaseList(DP_DatabaseRequest request)
        //{
        //    return packageManager.GetDatabaseList(request);
        //}







        //////public DataManager.PackageManager.DP_ResultRelatedPackage GetRelatedPackage(DataManager.PackageManager.DP_RequestRelatedPackage request)
        //////{
        //////    return packageManager.GetRelatedPackage(request);
        //////}





        //internal  List<DP_DataRepository> GetData(int relationshipID, DP_DataRepository sourceRelationData)
        //{

        //    var command = new AG_Command  ExecutionRequest();
        //    command.Request = new DR_Request();
        //    command.Request.Type = Enum_DR_RequestType.SearchView;
        //    command.Request.RequestExecutionTime = new List<DR_RequestExecutionTime>();
        //    command.Request.RequestExecutionTime.Add(new DR_RequestExecutionTime() { EnumType = Enum_DR_ExecutionTime.Now });
        //    command.Request.SearchViewRequest = new DR_SearchViewRequest();
        //    command.Request.SearchViewRequest.SourceRelationData = sourceRelationData;
        //    command.Request.SearchViewRequest.SourceRelationshipID = relationshipID;
        //    //////command.Request.SearchViewRequest.ViewPackage = editEntityArea.AreaInitializer.TemplateEntity;
        //    //////   command.Request.EditRequest.EditPackages = packageArea.DataPackages;
        //    //command.SourceSearchView = searchViewArea;

        //    var result = AgentUICoreMediator.GetAgentUICoreMediator.ExecuteCommand(command);
        //    return result.SearchViewResult.DPPackages;

        //}

        internal bool ReverseRelationshipIsMandatory(int relationshipID)
        {

            return RelationshipManager.ReverseRelationshipIsMandatory(relationshipID);
        }
        //internal bool ReverseRelationshipIsMandatory(int relationshipID)
        //{
        //    RelationshipManagerService relationshipManager = new RelationshipManagerService();
        //    return relationshipManager.ReverseRelationshipIsMandatory(relationshipID);
        //}
        //internal bool IndependentDataEntry(int entityID)
        //{
        //    return packageManager.IndependentDataEntry(entityID);
        //}

        //public DR_ResultEdit SendEditRequest(DR_EditRequest request)
        //{
        //    //ریکوئستر به داخل منیجر منتقل شود
        //    return RequestManager.SendRequest(request) as DR_ResultEdit;
        //}

        //public DR_ResultDelete SendDeleteRequest(DR_DeleteRequest request)
        //{

        //    return RequestManager.SendRequest(request) as DR_ResultDelete;
        //}

        //public DR_DeleteInquiryResult SendDeleteInquiryRequest(DR_DeleteInquiryRequest request)
        //{

        //    return RequestManager.SendRequest(request) as DR_DeleteInquiryResult;
        //}

        //public DR_ResultSearchView SendSearchViewRequest(DR_SearchViewRequest request)
        //{

        //    return RequestManager.SendRequest(request) as DR_ResultSearchView;
        //}
        //public DR_ResultSearchExists SendSearchExistsRequest(DR_SearchExistsRequest request)
        //{

        //    return RequestManager.SendRequest(request) as DR_ResultSearchExists;
        //}
        //public DR_ResultSearchCount SendSearchCountRequest(DR_SearchCountRequest request)
        //{

        //    return RequestManager.SendRequest(request) as DR_ResultSearchCount;
        //}
        //public DR_ResultSearchEdit SendSearchEditRequest(DR_SearchEditRequest request)
        //{
        //    return RequestManager.SendRequest(request) as DR_ResultSearchEdit;
        //}
        //public DR_ResultSearchFullData SendSearchFullDataRequest(DR_SearchFullDataRequest request)
        //{
        //    return RequestManager.SendRequest(request) as DR_ResultSearchFullData;
        //}
        //public DR_ResultSearchKeysOnly SendSearchKeysOnlyRequest(DR_SearchKeysOnlyRequest request)
        //{
        //    return RequestManager.SendRequest(request) as DR_ResultSearchKeysOnly;
        //}
        //public DR_ResultSearchByRelatinoshipTail SendSearchByRelationshipTailRequest(DR_SearchByRelationshipTailRequest request)
        //{

        //    return RequestManager.SendRequest(request) as DR_ResultSearchByRelatinoshipTail;
        //}


    }

    public class UserInfo
    {
        public UserInfo()
        {
            //Organizations = new List<OrganizationWithRolesDTO>();
        }
        public int ID { set; get; }
        public string UserName { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }

        public AdminSecurityInfo AdminSecurityInfo { set; get; }

        public List<OrganizationPostDTO> AssignedOrganizationPosts { set; get; }
        //OrganizationWithRolesDTO _DefaultOrganization;
        //public OrganizationWithRolesDTO DefaultOrganization
        //{
        //    get
        //    {
        //        if (_DefaultOrganization == null)
        //        {
        //            if (Organizations.Any(x => x.IsDefault))
        //                return Organizations.First(x => x.IsDefault);
        //            else
        //            {
        //                if (Organizations.Any())
        //                    return Organizations.First();
        //                else
        //                    return null;
        //            }
        //        }
        //        else
        //            return _DefaultOrganization;
        //    }
        //    set
        //    {
        //        _DefaultOrganization = value;
        //    }
        //}

        //public List<OrganizationWithRolesDTO> Organizations { set; get; }
    }

    public class AdminSecurityInfo
    {
        //public OrganizationDTO Organizatoin { set; get; }
        public bool ByPassSecurity { set; get; }
        public List<OrganizationPostDTO> SelectedOrganizationPosts { set; get; }
        public bool IsActive { set; get; }
    }

}
