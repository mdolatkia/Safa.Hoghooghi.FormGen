using MyDataManagerService;

using MyUILibrary.EntityArea;
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
    public class WorkflowRequesActionArea : WorkflowTransitionTargetSelectionArea, I_WorkflowRequesActionArea
    {
        public event EventHandler<RequestActionDTO> RequesActionConfirmed;
        public I_View_RequestAction View
        {
            get; set;
        }
        // public int RequestID { set; get; }
        public RequestActionDTO RequestAction { set; get; }
        public WorkflowRequesActionArea(RequestActionDTO requestAction)
        {
            RequestAction = requestAction;
            // var requestAction = menuItem.DataContext as RequestActionDTO;
            View = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetRequestActionForm();
            View.RequestActionConfirmed += View_RequestActionConfirmed;
            View.CurrentUserOrganizationPostChanged += View_OrganizationPostChanged;
            View.AddTargetSelectionView(ViewTargetSelection);
            View.CloseRequested += View_CloseRequested;
            View.ActionTitle = requestAction.TransitionAction.Name;
            View.TargetReason = requestAction.TargetReason;
            var posts = new List<OrganizationPostDTO>() { requestAction.OrganizationPost };
            View.OrganizationPosts = posts;
            if (posts.Count == 1)
                View.CurrentUserSelectedOrganizationPost = posts.First();

        }
        private void View_CloseRequested(object sender, EventArgs e)
        {
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog(sender);
        }
        private void View_OrganizationPostChanged(object sender, OrganizationPostDTO e)
        {
            CheckOutgoingTransitionActions();
        }

        private void CheckOutgoingTransitionActions()
        {

            var result = CheckOutgoingTransitionActions(this);
            if (result.IsNotTransitioningAction)
                View.NextState = result.NextState + "،" + " " + "در صورت تایید سایر اقدامات";
            else
                View.NextState = result.NextState;

            if (result.IsEndingState || result.IsNotTransitioningAction)
            {
                View.OutgoingTransitoinActionEnablity = false;
            }
        }


        private void View_RequestActionConfirmed(object sender, EventArgs e)
        {
            if (View.OutgoingTransitoinActionEnablity == true)
            {
                if (!ValidateTargetSelection())
                    return;

                string confirmMessage = "در مرحله بعد اقدامات توسط این اشخاص امکانپذیر است";
                foreach (var item in ViewTargetSelection.OutgoingTransitoinActions)
                {
                    confirmMessage += (Environment.NewLine) + "اقدام" + " : " + item.Title;
                    foreach (var post in item.OrganizationPosts.Where(x => x.Selected))
                    {
                        confirmMessage += (Environment.NewLine) + "    " + post.OrganizationPost.Name;
                    }
                }

                if (AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowConfirm("تایید مرحله بعد", confirmMessage, UserPromptMode.YesNo) == Temp.ConfirmResul.No)
                {
                    return;
                }
            }

            RequestActionConfirmDTO message = new RequestActionConfirmDTO();
            message.RequestActionID = RequestAction.ID;
            message.Description = View.Description;

            message.OutgoingTransitoinActions = ViewTargetSelection.OutgoingTransitoinActions;

            AgentUICoreMediator.GetAgentUICoreMediator.workflowService.SaveRequestAction(message, AgentUICoreMediator.GetAgentUICoreMediator.GetRequester());
            if (RequesActionConfirmed != null)
                RequesActionConfirmed(this, RequestAction);

        }

    }
}
