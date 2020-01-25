
using MyFormulaFunctionStateFunctionLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProxyLibrary;



using ProxyLibrary.Workflow;

using MyModelManager;
using MyWorkflowRequestManager;

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


        //public DP_DataRepository GetDataItem(int requestID)
        //{
        //    return bizRequest.GetDataItem(requestID);
        //}







        public void SaveRequestAction(RequestActionConfirmDTO requestAction, DR_Requester requester)
        {
            bizRequest.SaveRequestAction(requestAction, requester);
        }

        public List<WorkflowRequestDTO> GetUserWorkflowRequests(DR_Requester requester)
        {
            return bizRequest.GetWorkflowRequests(requester);
        }

        public List<WFStateDTO> GetProcessInitializeStates(int processID)
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

        BizProcess bizProcess = new BizProcess();
        public ProcessDTO GetProcess(DR_Requester requester, int processID, bool withDetails)
        {
            return bizProcess.GetProcess(requester, processID, withDetails);
        }

        public List<ProcessDTO> SearchProcess(DR_Requester requester, string singleFilterValue)
        {
            return bizProcess.SearchProcess(requester,singleFilterValue);
        }

        //public List<PossibleTransitionActionDTO> GetNextPossibleTransitionActionByActionID(int actionID, int requestID)
        //{
        //    BizRequest bizRequest = new BizRequest();
        //    return bizRequest.GetNextPossibleTransitionActionByActionID(actionID, requestID);
        //}  
        public PossibleTransitionActionResult GetNextPossibleTransitionActionByStateID(int stateID, OrganizationPostDTO organizationPost)
        {
            BizRequest bizRequest = new BizRequest();
            return bizRequest.GetNextPossibleTransitionActionByStateID(stateID, organizationPost);
        }
        public PossibleTransitionActionResult GetNextPossibleTransitionActionByRequestActionID(int requestActionID, OrganizationPostDTO organizationPost)
        {
            return bizRequest.GetNextPossibleTransitionActionByRequestActionID(requestActionID, organizationPost);
        }
        //public List<TransitionActionDTO> GetRequestPossibleTransitionActions(DR_Requester dR_Requester, int requestID)
        //{
        //    BizRequest bizRequest = new BizRequest();
        //    return bizRequest.GetRequestPossibleTransitionActions(dR_Requester, requestID);
        //}
        //public List<int> GetProcessAdmins(int processID)
        //{
        //    BizProcess bizProcess = new BizProcess();
        //    return bizProcess.GetProcessAdmins(processID);
        //}

        public List<RequestNoteDTO> GetRequestNotes(int requestID, bool withRequestActionDescriptions)
        {
            BizRequest bizRequest = new BizRequest();
            return bizRequest.GetRequestNotes(requestID, withRequestActionDescriptions);
        }
        //public RequestFileDTO GetRequestFile(int requestFileID)
        //{
        //    BizRequest bizRequest = new BizRequest();
        //    return bizRequest.GetRequestFile(requestFileID);
        //}
        //public List<RequestFileDTO> GetRequestFiles(int requestID)
        //{
        //    BizRequest bizRequest = new BizRequest();
        //    return bizRequest.GetRequestFiles(requestID);
        //}
        public void SaveRequestNote(RequestNoteDTO requestNote, DR_Requester requester)
        {
            BizRequest bizRequest = new BizRequest();
            bizRequest.SaveRequestNote(requestNote, requester);
        }

        //public void SaveRequestFile(RequestFileDTO requestFile, DR_Requester requester)
        //{
        //    BizRequest bizRequest = new BizRequest();
        //    bizRequest.SaveRequestFile(requestFile, requester);
        //}

        public RequestDiagramDTO GetRequestDiagram(DR_Requester requester, int requestID)
        {
            BizRequest bizRequest = new BizRequest();
            return bizRequest.GetRequestDiagram(requester, requestID);
        }

        public List<TransitionActionDTO> GetTransitionActionsByProcessID(DR_Requester requester, int iD)
        {
            BizTransition bizTransition = new BizTransition();
            return bizTransition.GetTransitionActionsByProcessID(requester, iD);
        }

        public List<RequestActionDTO> GetRequestActions(DR_Requester requester, List<int> requestActionIDs)
        {
            BizRequest bizRequest = new BizRequest();
            return bizRequest.GetRequestActions(requester, requestActionIDs);
        }

        public List<WorkflowRequestDTO> SearchWorkflows(DR_Requester requester, int processID, DateTime? fromData, DateTime? toDate, DP_DataRepository data, WFStateDTO currentState, WFStateDTO historyState, TransitionActionDTO selectedAction, int userID)
        {
            BizRequest bizRequest = new BizRequest();
            return bizRequest.SearchWorkflows( requester, processID, fromData, toDate, data, currentState, historyState, selectedAction, userID);
        }

        public bool EntityHasAnyProcess(int targetEntityID)
        {
            return bizProcess.EntityHasAnyProcess(targetEntityID);
        }
    }
}