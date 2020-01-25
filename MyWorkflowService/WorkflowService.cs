
using MyFormulaFunctionStateFunctionLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProxyLibrary;
using MyWorkflowLibrary;


using ProxyLibrary.Workflow;
using MySecurity;
using MyModelManager;

namespace MyWorkflowService
{
    public class WorkflowService
    {
        BizRequest bizRequest = new BizRequest();
        public List<ProcessDTO> GetProcessesForExecution(DR_Requester requester)
        {
            return bizRequest.GetProcessesForExecution(requester);

        }


        public int CreateWorkflowRequest(CreateRequestDTO requestMessage, DR_Requester requester)
        {
            return bizRequest.CreateWorkflowRequest(requestMessage, requester);

        }


        public DP_DataRepository GetDataItem(int requestID)
        {
            return bizRequest.GetDataItem(requestID);
        }







        public void SaveRequestAction(RequestActionConfirmDTO requestAction, DR_Requester requester)
        {
            bizRequest.SaveRequestAction(requestAction, requester);
        }

        public List<WorkflowRequestDTO> GetUserWorkflowRequests(DR_Requester requester)
        {
            return bizRequest.GetWorkflowRequests(requester);

        }

        public List<RequestActionDTO> GetRequestActions(DR_Requester requester, int requestID)
        {
            return bizRequest.GetRequestActions(requester, requestID,true);
        }

        public List<StateDTO> GetProcessInitializeStates(int processID)
        {
            BizState bizState = new BizState();
            return bizState.GetProcessInitializeStates(processID);
        }
        //public List<RoleDTO> GetProcessAdminRoles(int processID)
        //{
        //    BizProcess bizProcess = new BizProcess();
        //    return bizProcess.GetProcessAdminRoles(processID);
        //}

        //private void DoStateActivity(MyProjectEntities context, StateActivity stateActivity)
        //{

        //}
        //private void DoTransitionActivity(MyProjectEntities context, TransitionActivity TransitionActivity)
        //{

        //}
        //private int GetInitialState(MyProjectEntities context, int processID)
        //{
        //    var process = context.Process.First(x => x.ID == processID);
        //    return process.State.First().ID;
        //}

        public List<OutgoingTransitoinActionsDTO> GetOutgoingTransitoinActionPosts(int nextstateID, int requestID, int entityID, List<EntityInstanceProperty> keyProperties, int requesterOrganizaiotnPostID, List<int> requrtAdminIds)
        {
            BizRequest bizRequest = new BizRequest();
            return bizRequest.GetOutgoingTransitoinActionPosts(nextstateID, requestID,entityID,keyProperties, requesterOrganizaiotnPostID, requrtAdminIds);
        }

        public List<int> GetProcessAdmins(int processID)
        {
            BizProcess bizProcess = new BizProcess();
            return bizProcess.GetProcessAdmins(processID);
        }

        public List<RequestNoteDTO> GetRequestNotes(int requestID)
        {
            BizRequest bizRequest = new BizRequest();
            return bizRequest.GetRequestNotes(requestID);
        }
        public RequestFileDTO GetRequestFile(int requestFileID)
        {
            BizRequest bizRequest = new BizRequest();
            return bizRequest.GetRequestFile(requestFileID);
        }
        public List<RequestFileDTO> GetRequestFiles(int requestID)
        {
            BizRequest bizRequest = new BizRequest();
            return bizRequest.GetRequestFiles(requestID);
        }
        public void SaveRequestNote(RequestNoteDTO requestNote, DR_Requester requester)
        {
            BizRequest bizRequest = new BizRequest();
            bizRequest.SaveRequestNote(requestNote,  requester);
        }

        public WorkflowRequestDTO GetRequest(int requestID)
        {
            BizRequest bizRequest = new BizRequest();
            bizRequest.GetWorkflowRequests(requestNote, requester);
        }

        public void SaveRequestFile(RequestFileDTO requestFile, DR_Requester requester)
        {
            BizRequest bizRequest = new BizRequest();
            bizRequest.SaveRequestFile(requestFile, requester);
        }

        public RequestDiagramDTO GetRequestDiagram(int requestID, DR_Requester requester)
        {
            BizRequest bizRequest = new BizRequest();
            return bizRequest.GetRequestDiagram(requestID, requester);
        }
    }
}