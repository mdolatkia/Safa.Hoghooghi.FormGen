using ModelEntites;
using MyCommonWPFControls;
using MyDataManagerService;

using MyUILibrary.EntityArea;
using MyUILibraryInterfaces.DataMenuArea;
using MyWorkflowService;
using ProxyLibrary;
using ProxyLibrary.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.WorkflowArea
{
    public class WorkflowReportArea : I_WorkflowReportArea
    {

        public I_View_WorkflowReport View
        {
            set; get;
        }
        WorkflowReportAreaInitializer AreaInitializer { set; get; }
        public WorkflowReportArea(WorkflowReportAreaInitializer areaInitializer)
        {
            AreaInitializer = areaInitializer;
            View = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetWorkflowReportForm();
            View.Confirmed += View_Confirmed;
            View.MenuClicked += View_MenuClicked;
            View.InfoClicked += View_InfoClicked;
            SetLookups();

            if (areaInitializer.DataItem != null)
            {
                AddDataSelector(areaInitializer.DataItem.TargetEntityID);
                if (EditEntityArea != null)
                {
                    EditEntityArea.ShowDataFromExternalSource(areaInitializer.DataItem);
                    if (EditEntityArea.GetDataList().Count != 0)
                        SearchConfirmed();
                }
            }
        }

        private void View_InfoClicked(object sender, CartableItemSelectedArg e)
        {
            if (e.DataItem != null && e.DataItem is WorkflowRequestDTO)
            {
                var wfRequest = e.DataItem as WorkflowRequestDTO;
                if (wfRequest.DataItem != null)
                {
                    var menuInitializer = new DataMenuAreaInitializer();
                    menuInitializer.SourceView = e.UIElement;
                    menuInitializer.DataItem = wfRequest.DataItem;
                    AgentUICoreMediator.GetAgentUICoreMediator.ShowMenuArea(menuInitializer);
                }
            }
        }

        private void View_MenuClicked(object sender, CartableMenuClickArg e)
        {
            var currentMenuItems = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetCurrentMenuItems(e.ContextMenu);
            var requestDiagramMenu = currentMenuItems.FirstOrDefault(x => x.Name == "requestDiagram");
            if (requestDiagramMenu == null)
            {
                requestDiagramMenu = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetMenuItem(e.ContextMenu, "دیاگرام", "requestDiagram");
                requestDiagramMenu.MenuItemClicked += (sender1, e1) => RequestDiagramMenu_MenuItemClicked(sender1, e1);
            }
            requestDiagramMenu.ItemID = e.Request.ID;

        }
        private void RequestDiagramMenu_MenuItemClicked(object sender, EventArgs e)
        {
            var menuItem = (sender as MyMenuItem);
            if (menuItem != null)
            {
                AgentUICoreMediator.GetAgentUICoreMediator.ShowWorkflowDaigram(menuItem.ItemID);
            }
        }
        MySearchLookup processSearchLookup;
        //MySearchLookup entitySearchLookup;
        MySearchLookup userSearchLookup;
        I_EditEntityAreaOneData EditEntityArea { set; get; }
        private void SetLookups()
        {

            processSearchLookup = new MySearchLookup();
            processSearchLookup.DisplayMember = "Name";
            processSearchLookup.SelectedValueMember = "ID";
            processSearchLookup.SearchFilterChanged += ProcessSearchLookup_SearchFilterChanged;
            processSearchLookup.SelectionChanged += ProcessSearchLookup_SelectionChanged;
            View.AddProcessSelector(processSearchLookup);

            //entitySearchLookup = new MySearchLookup();
            //entitySearchLookup.DisplayMember = "Alias";
            //entitySearchLookup.SelectedValueMember = "ID";
            //entitySearchLookup.SearchFilterChanged += EntitySearchLookup_SearchFilterChanged;
            //entitySearchLookup.SelectionChanged += EntitySearchLookup_SelectionChanged;
            //View.AddEntitySelector(entitySearchLookup);

            userSearchLookup = new MySearchLookup();
            userSearchLookup.DisplayMember = "FullName";
            userSearchLookup.SelectedValueMember = "ID";
            userSearchLookup.SearchFilterChanged += UserSearchLookup_SearchFilterChanged;
            View.AddUserSelector(userSearchLookup);

        }
        private void UserSearchLookup_SearchFilterChanged(object sender, SearchFilterArg e)
        {
            if (!string.IsNullOrEmpty(e.SingleFilterValue))
            {
                if (e.FilterBySelectedValue)
                {
                    var user = AgentUICoreMediator.GetAgentUICoreMediator.userManagerService.GetUser(Convert.ToInt32(e.SingleFilterValue));
                    e.ResultItemsSource = new List<UserDTO> { user };
                }
                else
                {
                    var users = AgentUICoreMediator.GetAgentUICoreMediator.userManagerService.SearchUsersByString(e.SingleFilterValue);
                    e.ResultItemsSource = users;
                }
            }
        }
        private void ProcessSearchLookup_SelectionChanged(object sender, SelectionChangedArg e)
        {
            if (e.SelectedItem != null)
            {
                var process = e.SelectedItem as ProcessDTO;
                var fullProcess = AgentUICoreMediator.GetAgentUICoreMediator.workflowService.GetProcess(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), process.ID, true);
                var transitionActions = AgentUICoreMediator.GetAgentUICoreMediator.workflowService.GetTransitionActionsByProcessID(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), fullProcess.ID);
                transitionActions.Insert(0, new TransitionActionDTO() { ID = 0, Name = "همه اقدامها" });
                View.SetTransitionActionItems(transitionActions);
                fullProcess.States.Insert(0, new WFStateDTO() { ID = 0, Name = "بدون فیلتر" });
                View.SetCurrentStateItems(fullProcess.States);
                View.SetHistoryStateItems(fullProcess.States);

                if (fullProcess.EntityID == 0)
                {
                    EditEntityArea = null;
                    View.RemoveDataSelector();
                }
                else
                {
                    AddDataSelector(fullProcess.EntityID);
                }
            }
            else
            {
                EditEntityArea = null;
                View.RemoveDataSelector();
                View.SetTransitionActionItems(new List<TransitionActionDTO>());
                View.SetCurrentStateItems(new List<WFStateDTO>());
                View.SetHistoryStateItems(new List<WFStateDTO>());
            }
        }

        private void AddDataSelector(int entityID)
        {
            EditEntityAreaInitializer editEntityAreaInitializer1 = new EditEntityAreaInitializer();
            editEntityAreaInitializer1.EntityID = entityID;
            editEntityAreaInitializer1.IntracionMode = CommonDefinitions.UISettings.IntracionMode.Select;
            editEntityAreaInitializer1.DataMode = CommonDefinitions.UISettings.DataMode.One;
            var FirstSideEditEntityAreaResult = EditEntityAreaConstructor.GetEditEntityArea(editEntityAreaInitializer1);
            if (FirstSideEditEntityAreaResult.Item1 != null && FirstSideEditEntityAreaResult.Item1 is I_EditEntityAreaOneData)
            {
                EditEntityArea = FirstSideEditEntityAreaResult.Item1 as I_EditEntityAreaOneData;
                EditEntityArea.SetAreaInitializer(editEntityAreaInitializer1);
                View.AddDataSelector(EditEntityArea.TemporaryDisplayView);
            }
        }

        private void ProcessSearchLookup_SearchFilterChanged(object sender, SearchFilterArg e)
        {
            if (!string.IsNullOrEmpty(e.SingleFilterValue))
            {
                if (e.FilterBySelectedValue)
                {
                    var process = AgentUICoreMediator.GetAgentUICoreMediator.workflowService.GetProcess(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), Convert.ToInt32(e.SingleFilterValue), false);
                    e.ResultItemsSource = new List<ProcessDTO> { process };
                }
                else
                {
                    var process = AgentUICoreMediator.GetAgentUICoreMediator.workflowService.SearchProcess(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), e.SingleFilterValue);
                    e.ResultItemsSource = process;
                }
            }
        }
        //private void EntitySearchLookup_SearchFilterChanged(object sender, SearchFilterArg e)
        //{
        //    if (!string.IsNullOrEmpty(e.SingleFilterValue))
        //    {
        //        if (e.FilterBySelectedValue)
        //        {
        //            var entities = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetSimpleEntity(Convert.ToInt32(e.SingleFilterValue));
        //            e.ResultItemsSource = new List<TableDrivedEntityDTO> { entities };
        //        }
        //        else
        //        {
        //            var entities = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.SearchEntities(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), e.SingleFilterValue);
        //            e.ResultItemsSource = entities;
        //        }
        //    }
        //}
        //private void EntitySearchLookup_SelectionChanged(object sender, SelectionChangedArg e)
        //{
        //    if (e.SelectedItem != null)
        //    {

        //    }
        //    else
        //    {
        //        View.RemoveDataSelector();
        //    }
        //}
        private void View_Confirmed(object sender, EventArgs e)
        {

            SearchConfirmed();
        }

        private void SearchConfirmed()
        {
            int processID = 0;
            if (processSearchLookup.SelectedItem != null)
                processID = (processSearchLookup.SelectedItem as ProcessDTO).ID;
            int userID = 0;
            if (userSearchLookup.SelectedItem != null)
                userID = (userSearchLookup.SelectedItem as UserDTO).ID;
            DP_DataRepository data = null;
            if (EditEntityArea != null && EditEntityArea.GetDataList().Count != 0)
                data = EditEntityArea.GetDataList().First();
            WFStateDTO selectedCurrentState = View.SelectedCurrentState;
            if (selectedCurrentState != null && selectedCurrentState.ID == 0)
                selectedCurrentState = null;

            WFStateDTO selectedHistoryState = View.SelectedHistoryState;
            if (selectedHistoryState != null && selectedHistoryState.ID == 0)
                selectedHistoryState = null;

            TransitionActionDTO selectedAction = View.SelectedTransitionAction;
            if (selectedAction != null && selectedAction.ID == 0)
                selectedAction = null;
            var logs = AgentUICoreMediator.GetAgentUICoreMediator.workflowService.SearchWorkflows(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), processID, View.FromData, View.ToDate, data,
             selectedCurrentState, selectedHistoryState, selectedAction, userID);
            View.SetResult(logs);
        }
    }
}
