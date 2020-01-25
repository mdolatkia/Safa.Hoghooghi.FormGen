using MyDataManagerService;
using MySecurity;
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
    public class WorkflowFileArea : I_WorkflowFileArea
    {

        public I_View_RequestFile View
        {
            set; get;
        }
        public WorkflowFileArea(int requestID)
        {
            View = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetRequestFileForm();
            View.RequestID = requestID;
            var list = AgentUICoreMediator.GetAgentUICoreMediator.workflowService.GetRequestFiles(requestID);
            View.ShowRequestFiles(list);
            View.RequestFileSelected += View_RequestFileSelected;
            View.RequestFileConfirmed += View_RequestFileConfirmed;
            View.RequestFileClear += View_RequestFileClear;
            View.CloseRequested += View_CloseRequested;
        }

        private void View_CloseRequested(object sender, EventArgs e)
        {
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog(sender);
        }

        private void View_RequestFileClear(object sender, EventArgs e)
        {
            var View = sender as I_View_RequestFile;
            View.ShowRequestFile(new RequestFileDTO());
        }

        private void View_RequestFileConfirmed(object sender, RequestFileConfirmedArg e)
        {
            var View = sender as I_View_RequestFile;
            e.RequestFile.RequestID = View.RequestID;
            if (e.RequestFile.ID != 0)
                e.RequestFile.FileContent = null;
            AgentUICoreMediator.GetAgentUICoreMediator.workflowService.SaveRequestFile(e.RequestFile, AgentUICoreMediator.GetAgentUICoreMediator.GetRequester());
            var list = AgentUICoreMediator.GetAgentUICoreMediator.workflowService.GetRequestFiles(View.RequestID);
            View.ShowRequestFiles(list);
        }

        private void View_RequestFileSelected(object sender, RequestFileSelectedArg e)
        {
            var View = sender as I_View_RequestFile;
            if (View != null)
            {//کاربر چک شود
                var fullRequestFile = AgentUICoreMediator.GetAgentUICoreMediator.workflowService.GetRequestFile(e.RequestFile.ID);
                View.ShowRequestFile(fullRequestFile);
            }
        }


    }
}
