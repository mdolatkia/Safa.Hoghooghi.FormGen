using MyDataManagerService;
using MyGeneralLibrary;

using MyUILibrary.EntityArea;
using MyUILibrary.Temp;
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
    public class WorkflowTransitionTargetSelectionArea
    {
        OrganizationManagerService organizationManager = new OrganizationManagerService();
        public I_View_WorkflowTransitionTargetSelection ViewTargetSelection { set; get; }
        public WorkflowTransitionTargetSelectionArea()
        {
            ViewTargetSelection = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetViewOfWorkflowTransitionTargetSelection();
            ViewTargetSelection.TargetTransitionActionSelected += View_TargetTransitionActionSelected;
            ViewTargetSelection.OganizationPostsSearchChanged += View_OganizationPostsSearchChanged;
            ViewTargetSelection.SimpleSearchChanged += ViewTargetSelection_SimpleSearchChanged;
        }


        public bool ValidateTargetSelection()
        {
            if (ViewTargetSelection.OutgoingTransitoinActions.Count > 0)
            {
                foreach (var item in ViewTargetSelection.OutgoingTransitoinActions)
                {

                    if (item.OrganizationPosts.Count(x => x.Selected) == 0)
                    {
                        AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowMessage("برای اقدام" + " " + item.Title + " " + "کاربر هدف انتخاب نشده است");
                        return false;
                    }
                    else if (item.OrganizationPosts.Count(x => x.Selected) > 1)
                    {
                        if (!item.MultipleUserEnabled)
                        {
                            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowMessage("برای اقدام" + " " + item.Title + " " + "بیش از یک کاربر هدف انتخاب شده است");
                            return false;
                        }
                    }
                }
            }
            else
            {
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowMessage("اقدامی در مرحله بعد بروی این جریان کار امکانپذیر نمی باشد");
                return false;
            }
            return true;
        }

        public PossibleTransitionActionResult CheckOutgoingTransitionActions(I_WorkflowRequesActionArea requestActionArea)
        {
            var trainsitionActionUesrs = AgentUICoreMediator.GetAgentUICoreMediator.workflowService.GetNextPossibleTransitionActionByRequestActionID(requestActionArea.RequestAction.ID, requestActionArea.View.CurrentUserSelectedOrganizationPost);
            SetTargetViewProperties(trainsitionActionUesrs);
            return trainsitionActionUesrs;
        }
        public void CheckOutgoingTransitionActions(I_WorkflowCreateRequestArea workflowCreateRequestArea)
        {
            if (workflowCreateRequestArea.View.SelectedStateID != 0 && workflowCreateRequestArea.View.CurrentUserSelectedOrganizationPost != null)
            {
                var trainsitionActionUesrs = AgentUICoreMediator.GetAgentUICoreMediator.workflowService.GetNextPossibleTransitionActionByStateID(workflowCreateRequestArea.View.SelectedStateID, workflowCreateRequestArea.View.CurrentUserSelectedOrganizationPost);
                SetTargetViewProperties(trainsitionActionUesrs);
            }
            else
                ViewTargetSelection.OutgoingTransitoinActions = new List<PossibleTransitionActionDTO>();
        }
        PossibleTransitionActionResult TrainsitionActionUesrs { set; get; }
        List<TransitionActionOrganizationPostDTO> SharedTargets { get; set; }
        private void SetTargetViewProperties(PossibleTransitionActionResult trainsitionActionUesrs)
        {
            TrainsitionActionUesrs = trainsitionActionUesrs;
            ViewTargetSelection.SharedTargetsVisibility = TrainsitionActionUesrs.SharedOrganizationPosts.Any();
            if (TrainsitionActionUesrs.SharedOrganizationPosts.Any())
            {
                SharedTargets = TrainsitionActionUesrs.SharedOrganizationPosts.OrderByDescending(x => x.Selected).ToList(); ;
                ViewTargetSelection.SharedTargets = SharedTargets;
                foreach (var item in TrainsitionActionUesrs.SharedOrganizationPosts)
                {
                    item.SelectedChanged += Item_SelectedChanged;
                }
            }
            foreach (var item in TrainsitionActionUesrs.PossibleTransitionActions)
            {
                if (item.OrganizationPosts.Count == 0)
                    item.Color = ItemColor.Red;
                else if (item.OrganizationPosts.Count == 1)
                {
                    item.OrganizationPosts.First().Selected = true;
                    item.Color = ItemColor.Green;
                }
            }
            ViewTargetSelection.OutgoingTransitoinActions = TrainsitionActionUesrs.PossibleTransitionActions;

        }

        private void Item_SelectedChanged(object sender, EventArgs e)
        {
            TransitionActionOrganizationPostDTO item = sender as TransitionActionOrganizationPostDTO;
            foreach (var tAction in TrainsitionActionUesrs.PossibleTransitionActions)
            {
                foreach (var post in tAction.OrganizationPosts.Where(x => x.OrganizationPost.ID == item.OrganizationPost.ID))
                    post.Selected = item.Selected;
            }
            ViewTargetSelection.OutgoingTransitoinActions = null;
            ViewTargetSelection.OutgoingTransitoinActions = TrainsitionActionUesrs.PossibleTransitionActions;

        }

        private void View_TargetTransitionActionSelected(object sender, PossibleTransitionActionDTO e)
        {
            if (e != null)
            {
                ViewTargetSelection.TargetOrganizationPosts = e.OrganizationPosts;
            }
            else
                ViewTargetSelection.TargetOrganizationPosts = new List<TransitionActionOrganizationPostDTO>();
        }

        private void View_OganizationPostsSearchChanged(object sender, string e)
        {
            if (ViewTargetSelection.SelectedTransitionAction != null)
            {
                List<TransitionActionOrganizationPostDTO> posts = null;
                if (!string.IsNullOrEmpty(e))
                    posts = ViewTargetSelection.SelectedTransitionAction.OrganizationPosts.Where(x => x.OrganizationPost.Name.Contains(e)).ToList();
                else
                    posts = ViewTargetSelection.SelectedTransitionAction.OrganizationPosts;
                ViewTargetSelection.TargetOrganizationPosts = posts.OrderByDescending(x => x.Selected).ToList();
            }
        }
        private void ViewTargetSelection_SimpleSearchChanged(object sender, string e)
        {
            List<TransitionActionOrganizationPostDTO> posts = null;
            if (!string.IsNullOrEmpty(e))
                posts = SharedTargets.Where(x => x.OrganizationPost.Name.Contains(e)).ToList();
            else
                posts = SharedTargets;
            ViewTargetSelection.SharedTargets = posts.OrderByDescending(x => x.Selected).ToList();
        }


    }
}
