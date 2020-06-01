
using ModelEntites;
using MyCommonWPFControls;
using MyModelManager;


using ProxyLibrary.Workflow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Telerik.Windows.Controls.GridView;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class frmProcess : UserControl
    {
        ProcessDTO Message = new ProcessDTO();
        BizProcess bizProcess = new BizProcess();
        BizAction bizAction = new BizAction();
        BizActivity bizActivity = new BizActivity();
        BizEntityGroup bizEntityGroup = new BizEntityGroup();
        BizState bizState = new BizState();
        BizRoleType bizRoleType = new BizRoleType();
        BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();

        public frmProcess(int processID)
        {
            InitializeComponent();
            tabControlMain.IsEnabled = false;
            dtgEntityGroup.SelectionChanged += DtgEntityGroup_SelectionChanged;
            SetEntities();
            SetRoles();
            if (processID == 0)
            {
                tabForm.IsEnabled = false;
                Message = new ProcessDTO();
                ShowProcessDTO();
            }
            else
                GetProcess(processID);

            //ControlHelper.GenerateContextMenu(dtgAdminRoleTypes);
            lokEntities.SelectionChanged += LokEntities_SelectionChanged;
            tabForm.Visibility = Visibility.Collapsed;

            lokEntities.SelectionChanged += LokEntities_SelectionChanged1;
            colRelationshipTail.NewItemEnabled = true;
            colRelationshipTail.EditItemEnabled = true;
            colRelationshipTail.EditItemClicked += ColRelationshipTail_EditItemClicked;
            ControlHelper.GenerateContextMenu(dtgEntityGroup);
            ControlHelper.GenerateContextMenu(dtgRelationshipTails);

        }

        private void LokEntities_SelectionChanged1(object sender, SelectionChangedArg e)
        {
            if (e.SelectedItem == null)
            {
                tabForm.IsEnabled = false;
            }
            else
                tabForm.IsEnabled = true;
        }

        private void ColRelationshipTail_EditItemClicked(object sender, EditItemClickEventArg e)
        {
            if (lokEntities.SelectedItem != null)
            {
                frmEntityRelationshipTail view = new frmEntityRelationshipTail((int)lokEntities.SelectedValue);
                MyProjectManager.GetMyProjectManager.ShowDialog(view, "رابطه");
                view.ItemSelected += (sender1, e1) => View_ItemSelected(sender1, e1, (sender as MyStaticLookup));
            }
        }
        private void View_ItemSelected(object sender, EntityRelationshipTailSelectedArg e, MyStaticLookup lookup)
        {
            SetRelationshipTails();
            lookup.SelectedValue = e.EntityRelationshipTailID;
        }

        private void LokEntities_SelectionChanged(object sender, MyCommonWPFControls.SelectionChangedArg e)
        {
            if (lokEntities.SelectedItem == null)
                tabForm.Visibility = Visibility.Collapsed;
            else
            {
                tabForm.Visibility = Visibility.Visible;
                SetRelationshipTails();
            }
        }

        private void SetRoles()
        {
            ////lokAdminRoleGroup.ItemsSource = bizRole.GetRoleGroups();
            ////lokAdminRoleGroup.DisplayMember = "Name";
            ////lokAdminRoleGroup.SelectedValueMember = "ID";


            //colRoleType.ItemsSource = bizRoleType.GetAllRoleTypes();
            //colRoleType.DisplayMemberPath = "Name";
            //colRoleType.SelectedValueMemberPath = "ID";

            ////var col3 = dtgInitializerRoles.Columns[0] as GridViewComboBoxColumn;
            ////col3.ItemsSource = bizRole.GetRoleGroups();
            ////col3.DisplayMemberPath = "Name";
            ////col3.SelectedValueMemberPath = "ID";
        }
        private void SetRelationshipTails()
        {
            if (lokEntities.SelectedItem != null)
            {
                var relationshipTails = bizEntityRelationshipTail.GetEntityRelationshipTails(MyProjectManager.GetMyProjectManager.GetRequester(), (int)lokEntities.SelectedValue);
                relationshipTails.Add(new ModelEntites.EntityRelationshipTailDTO() { ID = 0, EntityPath = "موجودیت اصلی" });
                colRelationshipTail.DisplayMemberPath = "EntityPath";
                colRelationshipTail.SelectedValueMemberPath = "ID";
                colRelationshipTail.ItemsSource = relationshipTails;
            }
        }
        private void DtgEntityGroup_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (dtgEntityGroup.SelectedItem is EntityGroupDTO)
            {
                //var id = (dtgEntityGroup.SelectedItem as EntityGroupDTO).ID;
                //var fullEmtityGroup = bizEntityGroup.GetEntityGroup(id, true);
                dtgRelationshipTails.ItemsSource = (dtgEntityGroup.SelectedItem as EntityGroupDTO).Relationships;
            }
        }



        //private void DtgGroups_SelectionChanged(object sender, SelectionChangeEventArgs e)
        //{
        //    if (dtgRoles.SelectedItem is RoleDTO)
        //    {
        //        var id = (dtgActions.SelectedItem as RoleDTO).ID;
        //        var fullActivity = bizRole.GetRole(id, true);
        //        dtgActivityTargets.ItemsSource = fullActivity.Targets;

        //    }

        //    if (dtgRoles.SelectedItem is RoleDTO)
        //    {
        //        //dtgGroupUsers.ItemsSource = (dtgGroups.SelectedItem as Group).GroupMember;
        //    }
        //}

        //private void FrmProcess_Loaded(object sender, RoutedEventArgs e)
        //{


        //}
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        private void SetEntities()
        {
            // 

            lokEntities.DisplayMember = "Name";
            lokEntities.SelectedValueMember = "ID";
            lokEntities.SearchFilterChanged += LokEntities_SearchFilterChanged;

            //  lokEntities.ItemsSource = bizTableDrivedEntity.GetAllEntities();
        }
        private void LokEntities_SearchFilterChanged(object sender, MyCommonWPFControls.SearchFilterArg e)
        {
            if (e.SingleFilterValue != null)
            {
                if (!e.FilterBySelectedValue)
                {
                    var list = bizTableDrivedEntity.GetAllEntities(MyProjectManager.GetMyProjectManager.GetRequester(), e.SingleFilterValue, false);
                    e.ResultItemsSource = list;
                }
                else
                {
                    var id = Convert.ToInt32(e.SingleFilterValue);
                    if (id > 0)
                    {
                        //  lokEntities.ItemsSource = bizTableDrivedEntity.GetAllEntities();
                        var entity = bizTableDrivedEntity.GetSimpleEntity(MyProjectManager.GetMyProjectManager.GetRequester(), id); ;
                        e.ResultItemsSource = new List<TableDrivedEntityDTO> { entity };
                    }
                    else
                        e.ResultItemsSource = null;
                }
            }
        }
        //private void LoadDependedBaseTables()
        //{

        //    var col8 = dtgActionTargets.Columns[1] as GridViewComboBoxColumn;
        //    col8.ItemsSource = WorkflowHelper.GetGroups(Message.ID);
        //    col8.DisplayMemberPath = "Name";
        //    col8.SelectedValueMemberPath = "ID";


        //    var col6 = dtgActivityTargets.Columns[1] as GridViewComboBoxColumn;
        //    col6.ItemsSource = WorkflowHelper.GetGroups(Message.ID);
        //    col6.DisplayMemberPath = "Name";
        //    col6.SelectedValueMemberPath = "ID";
        //}
        //private void CreateDefaultMessage()
        //{
        //    Message = new Process();
        //    LoadItem();
        //}

        private void ShowProcessDTO()
        {
            if (Message.ID != 0)
            {
                tabControlMain.IsEnabled = true;
                btnTransition.IsEnabled = true;
            }
            else
            {
                tabControlMain.IsEnabled = false;
                btnTransition.IsEnabled = false;
            }
            txtName.Text = Message.Name;
            lokEntities.SelectedValue = Message.EntityID;

            //lokAdminRoleGroup.SelectedValue = Message.AdminRoleGroupID;
            dtgActions.ItemsSource = Message.Actions;
            dtgActivities.ItemsSource = Message.Activities;
            dtgEntityGroup.ItemsSource = Message.EntityGroups;
            dtgRelationshipTails.ItemsSource = null;
            dtgStates.ItemsSource = Message.States;
            //dtgAdminRoleTypes.ItemsSource = Message.AdminRoleTypes;
            //dtgInitializerRoles.ItemsSource = Message.ProcessInitializerRoleGroups;

            //this.DataContext = Message;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //if (lokAdminRoleGroup.SelectedItem == null)
            //{
            //    MessageBox.Show("نقش راهبر مشخص نشده است",Enum_WindowSize.Big);
            //    return;
            //}
            if (lokEntities.SelectedItem != null)
                Message.EntityID = (int)lokEntities.SelectedValue;
            else
                Message.EntityID = 0;

            Message.Name = txtName.Text;
            //Message.AdminRoleGroupID = (int)lokAdminRoleGroup.SelectedValue;
            Message.ID = bizProcess.UpdateProcesss(Message);
            MessageBox.Show("با موفقیت ثبت شد");
            GetProcess(Message.ID);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            frmProcessSelect view = new MyProject_WPF.frmProcessSelect();
            view.ItemSelected += View_ItemSelected;
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form", Enum_WindowSize.Big);
        }

        private void View_ItemSelected(object sender, ProcessSelectedArg e)
        {
            GetProcess(e.ID);
        }

        private void GetProcess(int iD)
        {
            Message = bizProcess.GetProcess(MyProjectManager.GetMyProjectManager.GetRequester(), iD, true);
            ShowProcessDTO();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Message = new ProcessDTO();
            ShowProcessDTO();
        }
        private void mnuAddNewAction_Click(object sender, RoutedEventArgs e)
        {
            frmAddAction view = new frmAddAction(Message.ID, 0);
            view.ItemSaved += View_ItemSaved1;
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form", Enum_WindowSize.Big);
        }
        private void mnuEditAction_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as RadMenuItem;
            var contextMenu = menuItem.Parent as RadContextMenu;
            var source = contextMenu.GetClickedElement<GridViewRow>();
            if (contextMenu != null && source != null)
            {
                frmAddAction view = new frmAddAction(Message.ID, (source.DataContext as WFActionDTO).ID);
                view.ItemSaved += View_ItemSaved1;
                MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form", Enum_WindowSize.Big);
            }
        }
        private void View_ItemSaved1(object sender, SavedItemArg e)
        {
            dtgActions.ItemsSource = bizAction.GetActions(Message.ID);
        }

        private void mnuAddNewActivity_Click(object sender, RoutedEventArgs e)
        {
            frmAddActivity view = new frmAddActivity(Message.ID, 0);
            view.ItemSaved += View_ItemSaved;
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form", Enum_WindowSize.Big);
        }
        private void mnuEditActivity_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as RadMenuItem;
            var contextMenu = menuItem.Parent as RadContextMenu;
            var source = contextMenu.GetClickedElement<GridViewRow>();
            if (contextMenu != null && source != null)
            {
                frmAddActivity view = new frmAddActivity(Message.ID, (source.DataContext as ActivityDTO).ID);
                view.ItemSaved += View_ItemSaved;
                MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form", Enum_WindowSize.Big);
            }
        }
        private void View_ItemSaved(object sender, SavedItemArg e)
        {
            dtgActivities.ItemsSource = bizActivity.GetActivities(Message.ID, false);
        }


        //private void mnuAddNewGroup_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        //{
        //    frmAddSelectGroup view = new frmAddSelectGroup(Message.ID, 0);
        //    view.ItemSaved += View_ItemSaved2;
        //       MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form",Enum_WindowSize.Big);
        //}

        //private void mnuEditGroup_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        //{
        //    var menuItem = sender as RadMenuItem;
        //    var contextMenu = menuItem.Parent as RadContextMenu;
        //    var source = contextMenu.GetClickedElement<GridViewRow>();
        //    if (contextMenu != null && source != null)
        //    {
        //        frmAddSelectGroup view = new frmAddSelectGroup(Message.ID, (source.DataContext as Group).ID);
        //        view.ItemSaved += View_ItemSaved2;
        //           MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form",Enum_WindowSize.Big);
        //    }
        //}
        //private void View_ItemSaved2(object sender, SavedItemArg e)
        //{
        //    dtgRoles.ItemsSource = bizProcess.GetProcessRoles(Message.ID);
        //}

        private void mnuAddNewState_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            frmAddSelectState view = new frmAddSelectState(Message.ID, 0);
            view.ItemSaved += View_ItemSaved3;
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form", Enum_WindowSize.Big);
        }

        private void mnuEditState_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var menuItem = sender as RadMenuItem;
            var contextMenu = menuItem.Parent as RadContextMenu;
            var source = contextMenu.GetClickedElement<GridViewRow>();
            if (contextMenu != null && source != null)
            {
                frmAddSelectState view = new frmAddSelectState(Message.ID, (source.DataContext as WFStateDTO).ID);
                view.ItemSaved += View_ItemSaved3;
                MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form", Enum_WindowSize.Big);
            }
        }
        private void View_ItemSaved3(object sender, SavedItemArg e)
        {
            dtgStates.ItemsSource = bizState.GetStates(Message.ID, false);
        }


        private void btnTransition_Click(object sender, RoutedEventArgs e)
        {

            frmTransitions view = new frmTransitions(Message.ID);
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form", Enum_WindowSize.Maximized);
        }

        //private void mnuAddNewEntityGroup_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        //{
        //    if (lokEntities.SelectedItem != null)
        //    {
        //        frmAddEntityGroup view = new frmAddEntityGroup(Message.ID, (int)lokEntities.SelectedValue, 0);
        //        view.ItemSaved += View_ItemSaved4;
        //        MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form", Enum_WindowSize.Big);
        //    }
        //}
        //private void mnuEditEntityGroup_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        //{
        //    if (lokEntities.SelectedItem != null)
        //    {
        //        var menuItem = sender as RadMenuItem;
        //        var contextMenu = menuItem.Parent as RadContextMenu;
        //        var source = contextMenu.GetClickedElement<GridViewRow>();
        //        if (contextMenu != null && source != null)
        //        {
        //            frmAddEntityGroup view = new frmAddEntityGroup(Message.ID, (int)lokEntities.SelectedValue, (source.DataContext as EntityGroupDTO).ID);
        //            view.ItemSaved += View_ItemSaved4;
        //            MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form", Enum_WindowSize.Big);
        //        }
        //    }
        //}

        //private void View_ItemSaved4(object sender, SavedItemArg e)
        //{
        //    dtgEntityGroup.ItemsSource = bizEntityGroup.GetEntityGroups(Message.ID, false);
        //}

        private void lokAdminRoleGroup_EditItemClicked(object sender, MyCommonWPFControls.EditItemClickEventArg e)
        {

        }
    }
}
