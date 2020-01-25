using CommonDefinitions.UISettings;
using MyDataManagerService;
using MyRelationshipDataManager;
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
    public class CartableArea : I_CartableArea
    {
        public I_View_Cartable View
        {
            set; get;
        }
        public CartableArea()
        {

            View = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetCartable();
            //Cartable.EntitiyClicked += Cartable_EntitiyClicked;
            View.CartableRefreshClicked += Cartable_CartableRefreshClicked;
            View.MenuClicked += Cartable_MenuActionClicked;
            View.InfoClicked += View_InfoClicked;
            //Cartable.MenuActionConfirmClicked += Cartable_MenuActionConfirmClicked;

            ShowCartableData();
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
        private void ShowCartableData()
        {
            var requests = AgentUICoreMediator.GetAgentUICoreMediator.workflowService.GetUserWorkflowRequests(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester());
            View.ShowWorkflowRequests(requests);
        }

        private void Cartable_CartableRefreshClicked(object sender, EventArgs e)
        {
            ShowCartableData();
        }

        private void Cartable_MenuActionClicked(object sender, CartableMenuClickArg e)
        {
            //فرم ادمین یه جریان کار اضافه شود
            var currentMenuItems = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetCurrentMenuItems(e.ContextMenu);
            var requestNoteMenu = currentMenuItems.FirstOrDefault(x => x.Name == "requestNote");
            if (requestNoteMenu == null)
            {
                requestNoteMenu = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetMenuItem(e.ContextMenu, "یادداشتها", "requestNote");
                requestNoteMenu.MenuItemClicked += (sender1, e1) => RequestNoteMenu_MenuItemClicked(sender1, e1);
            }
            requestNoteMenu.ItemID = e.Request.ID;

            //var requestFileMenu = currentMenuItems.FirstOrDefault(x => x.Name == "requestFile");
            //if (requestFileMenu == null)
            //{
            //    requestFileMenu = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetMenuItem(e.ContextMenu, "فایلها", "requestFile");
            //    requestFileMenu.MenuItemClicked += (sender1, e1) => RequestFileMenu_MenuItemClicked(sender1, e1);
            //}
            //requestFileMenu.ItemID = e.Request.ID;

            //requestFileMenu.DataContext = e.RequestID;

            var requestDiagramMenu = currentMenuItems.FirstOrDefault(x => x.Name == "requestDiagram");
            if (requestDiagramMenu == null)
            {
                requestDiagramMenu = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetMenuItem(e.ContextMenu, "دیاگرام", "requestDiagram");
                requestDiagramMenu.MenuItemClicked += (sender1, e1) => RequestDiagramMenu_MenuItemClicked(sender1, e1);
            }
            requestDiagramMenu.ItemID = e.Request.ID;

            foreach (var item in currentMenuItems)
            {

                if (item.IsDeletable)
                {
                    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.RemoveMenuItem(e.ContextMenu, item.MenuItem);
                }
            }

            //   var request = workflowService.GetRequest(e.RequestID);

            //رایت کلیک بروی کارها و نمایش اقدامات ممکن
            //  var possibleActions = workflowService.GetRequestPossibleActions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), e.RequestID);

            //////foreach (var possibleAction in possibleActions)
            //////{
            //////    var mnuAction = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetMenuItem(e.ContextMenu, possibleAction.Name, "actionOK" + possibleAction.ID);
            //////    mnuAction.IsDeletable = true;
            //////    mnuAction.MenuItemClicked += (sender1, e1) => MnuAction_MenuItemClicked(sender1, e1, possibleAction, e.RequestID);
            //////}
            //////var possibleTransitionActions = workflowService.GetRequestPossibleTransitionActions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), e.RequestID);
            //////foreach (var transitionAction in possibleTransitionActions)
            //////{
            //////    foreach (var entityGroup in transitionAction.EntityGroups)
            //////    {
            //////        var mnuGroup = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetMenuItem(e.ContextMenu, entityGroup.Name, "entityGroup" + entityGroup.ID);
            //////        mnuGroup.IsDeletable = true;
            //////        foreach (var relationship in entityGroup.Relationships)
            //////        {
            //////            var mnuEntity = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetMenuItem(mnuGroup.MenuItem, relationship.Name, "relationship" + relationship.ID);
            //////            //mnuEntity.DataContext = new CartableEntityClick() { Title = mnuGroup.Name, EnitityID = relationship.EntityID, RequestID = e.RequestID, RelationshipID = relationship.RelationshipID };
            //////            mnuEntity.MenuItemClicked += (sender1, e1) => MnuEntity_MenuItemClicked(sender1, e, relationship.EntityID, mnuGroup.Name, relationship.RelationshipID, e.RequestID);
            //////        }
            //////    }
            //////}


            var requestActions = AgentUICoreMediator.GetAgentUICoreMediator.workflowService.GetRequestActions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), e.Request.RequestActionIDs);

            foreach (var requestAction in requestActions)
            {
                var requestActionMenu = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetMenuItem(e.ContextMenu, requestAction.TransitionAction.Name, "requestAction" + requestAction.ID, requestAction.TargetReason);
                requestActionMenu.IsDeletable = true;

                var mnuAction = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetMenuItem(requestActionMenu.MenuItem, "تایید اقدام", "requestActionOK" + requestAction.ID);
                mnuAction.IsDeletable = true;
                mnuAction.MenuItemClicked += (sender1, e1) => MnuAction_MenuItemClicked(sender1, e1, requestAction);
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.AddMenuSeprator(requestActionMenu.MenuItem);
                foreach (var entityGroup in requestAction.TransitionAction.EntityGroups)
                {
                    var mnuGroup = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetMenuItem(requestActionMenu.MenuItem, entityGroup.Name, "entityGroup" + entityGroup.ID);
                    mnuGroup.IsDeletable = true;
                    foreach (var entityGroupRelationship in entityGroup.Relationships)
                    {

                        var mnuEntity = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetMenuItem(mnuGroup.MenuItem, entityGroupRelationship.vwName, "relationship" + entityGroupRelationship.ID);
                        mnuEntity.IsDeletable = true;
                        mnuEntity.MenuItemClicked += (sender1, e1) => MnuEntity_MenuItemClicked(sender1, e, entityGroupRelationship, e.Request);
                    }
                }
            }
        }

        private void RequestDiagramMenu_MenuItemClicked(object sender, EventArgs e)
        {
            var menuItem = (sender as MyMenuItem);
            if (menuItem != null)
            {
                AgentUICoreMediator.GetAgentUICoreMediator.ShowWorkflowDaigram(menuItem.ItemID);
            }
        }



        private void RequestNoteMenu_MenuItemClicked(object sender, EventArgs e)
        {
            var menuItem = (sender as MyMenuItem);
            if (menuItem != null)
            {
                WorkflowNoteArea area = new WorkflowArea.WorkflowNoteArea(menuItem.ItemID);
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(area.View, "یادداشتها", Enum_WindowSize.Big);
            }

        }


        //private void RequestFileMenu_MenuItemClicked(object sender, EventArgs e)
        //{
        //    var menuItem = (sender as MyMenuItem);
        //    if (menuItem != null)
        //    {
        //        WorkflowFileArea area = new WorkflowArea.WorkflowFileArea(menuItem.ItemID);
        //        AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(area.View, "فایلها", Enum_WindowSize.Big);
        //    }

        //}
        //internal void ShowWorkflowNote(int requestID)
        //{

        //}
        //internal void ShowWorkflowFile(int requestID)
        //{

        //}
        private void MnuEntity_MenuItemClicked(object sender, EventArgs e, EntityGroupRelationshipDTO entityGroupRelationship, WorkflowRequestDTO wfRequest)
        {
            var menuItem = (sender as MyMenuItem);
            if (menuItem != null)
            {
                if (wfRequest.DataItem != null)
                {
                    //   var title = entityGroupRelationship.Name;

                    List<DP_DataView> initializeData = null;
                    if (entityGroupRelationship.RelationshipTail == null)
                    {
                        initializeData = new List<DP_DataView>();
                        initializeData.Add(wfRequest.DataItem);
                        AgentUICoreMediator.GetAgentUICoreMediator.ShowEditEntityArea(wfRequest.DataItem.TargetEntityID, true, CommonDefinitions.UISettings.DataMode.One, initializeData);

                        //سکوریتی داده اعمال میشود
                        //DP_SearchRepository searchRepository = new DP_SearchRepository(dataItem.TargetEntityID);
                        //foreach (var col in dataItem.KeyProperties)
                        //{
                        //    searchRepository.Phrases.Add(new SearchProperty() { ColumnID = col.ColumnID, Value = col.Value });
                        //}
                        //var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();
                        //var request = new DR_SearchFullDataRequest(requester, searchRepository);
                        //initializeData = AgentUICoreMediator.GetAgentUICoreMediator.SendSearchFullDataRequest(request).ResultDataItems;

                    }
                    else
                    {
                        //سکوریتی داده اعمال میشود

                        var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();
                        var searchItem = AgentUICoreMediator.GetAgentUICoreMediator.RelationshipTailDataManager.GetTargetSearchItemFromRelationshipTail(wfRequest.DataItem, entityGroupRelationship.RelationshipTail);
                        DR_SearchKeysOnlyRequest request = new DR_SearchKeysOnlyRequest(requester, searchItem);
                        var searchResult = AgentUICoreMediator.GetAgentUICoreMediator.requestRegistration.SendSearchKeysOnlyRequest(request);
                        if (searchResult.Result == Enum_DR_ResultType.SeccessfullyDone)
                            initializeData = searchResult.ResultDataItems;
                        else if (searchResult.Result == Enum_DR_ResultType.ExceptionThrown)
                        {
                            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowMessage(searchResult.Message);
                            return;
                        }
                        //اینجا یه ایرادی هست
                        //وقتی فرم با داده های مرتبط باز میشه باید یجوری ارتباط با داده اصلی نمایش داده بشه و غیرقابل تغییر باشه
                        DataMode dataMode;
                        if (entityGroupRelationship.RelationshipTail.IsOneToManyTail)
                            dataMode = DataMode.Multiple;
                        else
                            dataMode = DataMode.One;
                        AgentUICoreMediator.GetAgentUICoreMediator.ShowEditEntityArea(entityGroupRelationship.RelationshipTail.TargetEntityID, true, dataMode, initializeData, new Tuple<DP_DataView, ModelEntites.EntityRelationshipTailDTO>(wfRequest.DataItem, entityGroupRelationship.RelationshipTail.ReverseRelationshipTail));

                        //DR_SearchFullDataRequest request = new DR_SearchFullDataRequest(requester, searchItem);
                        //initializeData = AgentUICoreMediator.GetAgentUICoreMediator.(request).ResultDataItems;
                    }
                }
            }
        }

        //////private void MnuAction_MenuItemClicked(object sender, EventArgs e, WFActionDTO wfActionDTO, int requestID)
        //////{
        //////    WorkflowRequesActionArea workflowRequesActionArea = new WorkflowArea.WorkflowRequesActionArea(wfActionDTO, requestID);
        //////    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(workflowRequesActionArea.View, wfActionDTO.Name, Enum_WindowSize.Big);

        //////}
        private void MnuAction_MenuItemClicked(object sender, EventArgs e, RequestActionDTO requestAction)
        {
            WorkflowRequesActionArea workflowRequesActionArea = new WorkflowArea.WorkflowRequesActionArea(requestAction);
            workflowRequesActionArea.RequesActionConfirmed += WorkflowRequesActionArea_RequesActionConfirmed;
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(workflowRequesActionArea.View, requestAction.TransitionAction.Action.Name, Enum_WindowSize.Big);

        }

        private void WorkflowRequesActionArea_RequesActionConfirmed(object sender, RequestActionDTO e)
        {
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عملیات با عنوان" + " " + "'" + e.TransitionAction.Action.Name + "'" + " " + "انجام شد");
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog((sender as WorkflowRequesActionArea).View);
            ShowCartableData();
        }
    }
}
