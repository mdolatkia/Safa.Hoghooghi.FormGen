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
    public class WorkflowDiagramArea : I_WorkflowDiagramArea
    {

        public I_View_RequestDiagram View
        {
            set; get;
        }

        public WorkflowDiagramArea(int requestID)
        {
            View = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetRequestDiagramForm();
            View.RequestID = requestID;
            var requestDiagram = AgentUICoreMediator.GetAgentUICoreMediator.workflowService.GetRequestDiagram(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(),requestID);

            I_View_StateShape shape = null;
            int index = 0;
            if (requestDiagram.FirstDiagramState != null)
            {
                shape = View.AddStateShape(index);
                SetShapeInfo(shape, requestDiagram.FirstDiagramState, View, null);
            }
            foreach (var item in requestDiagram.DiagramStates)
            {
                var lastShape = shape;
                index++;
                shape = View.AddStateShape(index);
                SetShapeInfo(shape, item, View, lastShape);
            }
        }

        private void SetShapeInfo(I_View_StateShape shape, DiagramStateDTO state, I_View_RequestDiagram view, I_View_StateShape previousShape)
        {
            shape.Title = state.Name;
            shape.CreationDate = state.CreationDate.ToString();
            shape.Clicked += (sender, e) => Shape_Clicked(sender, e, state);
            if (previousShape != null)
            {
                int index = 0;
                var causingRequestActions = state.CausingRequestActions.Where(x => x.IsCompleted);
                foreach (var item in causingRequestActions.OrderBy(x => x.DateTimeCompleted))
                {
                    var connector = view.AddConnection(shape, previousShape, causingRequestActions.Count(), index);
                    connector.Clicked += Connector_Clicked;
                    connector.OrgnizatoinPostUserInfo = item.OrganizationPost.Name ;
                    if (causingRequestActions.Count() > 1)
                    {
                        if (item.LedToState != 0)
                            connector.Highlight = true;
                    }
                    var dif = item.DateTimeCompleted.Value - item.DateTimeCreation;
                    string duration = "";
                    if (dif.Days != 0)
                        duration += dif.Days + "روز";
                    if (dif.Hours != 0)
                        duration += (duration == "" ? "" : " و ") + dif.Hours + "ساعت";
                    if (dif.Minutes != 0)
                        duration += (duration == "" ? "" : " و ") + dif.Minutes + "دقیقه";

                    if (duration == "")
                        duration = dif.Seconds + "ثانیه";
                    connector.Duration = duration;

                    var tooltip = "اقدام :" + " " + item.TransitionAction.Action.Name;
                    tooltip += Environment.NewLine + "پست سازمانی اقدام کننده :" + " " + item.OrganizationPost.Name;
                    if (item.User != null)
                        tooltip += Environment.NewLine + "کاربر اقدام کننده :" + " " + item.User.FullName;
                    tooltip += Environment.NewLine + "نوع اقدام کننده :" + " " + item.TargetReason;
                    tooltip += Environment.NewLine + "زمان شروع :" + " " + item.DateTimeCreation.ToString();
                    tooltip += Environment.NewLine + "زمان اتمام :" + " " + item.DateTimeCompleted.Value.ToString();
                    if (!string.IsNullOrEmpty(item.Description))
                        tooltip += Environment.NewLine + "توضیحات :" + " " + item.Description;
                    connector.Tooltip = tooltip;
                    connector.Action = item.TransitionAction.Action.Name;
                    index++;
                }
            }
        }

        private void Connector_Clicked(object sender, EventArgs e)
        {

        }

        private void Shape_Clicked(object sender, EventArgs e, DiagramStateDTO state)
        {
            var view = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetViewOfDiagramStateInfo();
            view.ExisRequested += View_ExisRequested;
            view.StateName = state.Name;
            view.Date = state.CreationDate.ToString();
            view.CausingRequestActions = state.CausingRequestActions;
            view.PossibleRequestActions = state.PossibleRequestActions;
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(view, "اطلاعات وضعیت",Enum_WindowSize.Maximized);
        }

        private void View_ExisRequested(object sender, EventArgs e)
        {
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog(sender);
        }
    }
}
