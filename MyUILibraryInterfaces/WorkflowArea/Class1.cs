

using MyUILibrary.EntityArea;
using ProxyLibrary;
using ProxyLibrary.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.WorkflowArea
{
    public interface I_CartableArea
    {
        I_View_Cartable View { set; get; }
    }
    public interface I_WorkflowDiagramArea
    {
        I_View_RequestDiagram View { set; get; }
    }
    public interface I_WorkflowCreateRequestArea
    {
       
        I_View_WorkflowRequestCreator View { set; get; }
    }
    public interface I_WorkflowRequesActionArea
    {
        RequestActionDTO RequestAction { set; get; }
        I_View_RequestAction View { set; get; }
    }
    public interface I_WorkflowNoteArea
    {
        I_View_RequestNote View { set; get; }
    }

    public interface I_WorkflowFileArea
    {
        I_View_RequestFile View { set; get; }
    }

    public interface I_WorkflowReportArea
    {
        I_View_WorkflowReport View { set; get; }
    }
    public interface I_View_WorkflowReport
    {
        event EventHandler Confirmed;
        event EventHandler<CartableMenuClickArg> MenuClicked;
        event EventHandler<CartableItemSelectedArg> InfoClicked;
        void SetTransitionActionItems(List<TransitionActionDTO> columns);
        void SetCurrentStateItems(List<WFStateDTO> columns);
        void SetHistoryStateItems(List<WFStateDTO> columns);
        TransitionActionDTO SelectedTransitionAction { set; get; }
        WFStateDTO SelectedCurrentState { set; get; }
        WFStateDTO SelectedHistoryState { set; get; }
        //ProcessDTO SelectedProcess { set; get; }
        DateTime? FromData { set; get; }
        DateTime? ToDate { set; get; }
        void AddProcessSelector(object view);
        void AddDataSelector(object view);
        //void AddEntitySelector(object view);
        void AddUserSelector(object view);
        void SetResult(List<WorkflowRequestDTO> list);
        void RemoveDataSelector();
    }

    public interface I_View_WorkflowTransitionTargetSelection
    {
        event EventHandler<PossibleTransitionActionDTO> TargetTransitionActionSelected;
        List<PossibleTransitionActionDTO> OutgoingTransitoinActions { set; get; }
        event EventHandler<string> OganizationPostsSearchChanged;
        event EventHandler<string> SimpleSearchChanged;

        PossibleTransitionActionDTO SelectedTransitionAction { get; }
        List<TransitionActionOrganizationPostDTO> TargetOrganizationPosts { get; set; }
        bool SharedTargetsVisibility { get; set; }
        List<TransitionActionOrganizationPostDTO> SharedTargets { get; set; }
    }
    public interface I_View_WorkflowTransition
    {
        event EventHandler<OrganizationPostDTO> CurrentUserOrganizationPostChanged;
        OrganizationPostDTO CurrentUserSelectedOrganizationPost { set; get; }
        void AddTargetSelectionView(I_View_WorkflowTransitionTargetSelection view);
    }


    public interface I_View_WorkflowRequestCreator : I_View_WorkflowTransition
    {
        event EventHandler WorkflowRequestCreate;
        event EventHandler<ProcessSelectedArg> ProcessSelected;
        event EventHandler<StateSelectedArg> StateSelected;
        event EventHandler CloseRequested;
        void ShowMessage(string message);
        List<OrganizationPostDTO> CreatorOrganizationPosts { set; }
        I_View_TemporaryView SearchView { set; get; }
        List<WFStateDTO> States { set; }
        ProcessDTO SelectedProcess { get; }
        DateTime? Date { get; set; }
        string Title { get; set; }
        //string Description { get; set; }
        int SelectedStateID { get; set; }
        //List<RequestFileDTO> RequestFiles { get; }
        List<RequestNoteDTO> RequestNotes { get; }

        void RemoveEntitySelector();

    }
    public interface I_View_RequestAction : I_View_WorkflowTransition
    {
        event EventHandler CloseRequested;
        event EventHandler RequestActionConfirmed;
        List<OrganizationPostDTO> OrganizationPosts { set; }
        //OrganizationPostDTO CurrentUserSelectedOrganizationPost { set; get; }
        //void ShowRequestAction(RequestActionDTO wfActionDTO);
        void ShowMessage(string v);
        string ActionTitle { set; }
        string NextState { set; }
        string TargetReason { set; }
        string Description { set; get; }
        bool OutgoingTransitoinActionEnablity { set; get; }
    }
    public interface I_View_RequestNote
    {
        event EventHandler RequestNoteConfirmed;
        event EventHandler<RequestNoteSelectedArg> RequestNoteSelected;
        event EventHandler RequestNoteClear;
        event EventHandler CloseRequested;
        void ShowRequestNotes(List<RequestNoteDTO> requestNotes);
        //void ShowRequestNote(RequestNoteDTO requestNote);

        void ShowMessage(string message);
        string Title { set; get; }
        string Note { set; get; }
        int ID { set; get; }
        bool EditEnabled { get; set; }
    }
    public interface I_View_RequestFile
    {
        event EventHandler<RequestFileConfirmedArg> RequestFileConfirmed;
        event EventHandler<RequestFileSelectedArg> RequestFileSelected;
        event EventHandler RequestFileClear;
        event EventHandler CloseRequested;
        void ShowRequestFiles(List<RequestFileDTO> requestFiles);
        void ShowRequestFile(RequestFileDTO requestFile);
        int RequestID { set; get; }

    }
    public interface I_View_RequestDiagram
    {
        //event EventHandler<RequestNoteConfirmedArg> RequestNoteConfirmed;
        //event EventHandler<RequestNoteSelectedArg> RequestNoteSelected;
        //event EventHandler RequestNoteClear;
        I_View_StateShape AddStateShape(int index);
        int RequestID { set; get; }

        I_View_StateConnection AddConnection(I_View_StateShape shape, I_View_StateShape previousShape, int connectionCount, int index);
    }

    public interface I_View_StateShape
    {
        //event EventHandler<RequestNoteConfirmedArg> RequestNoteConfirmed;
        //event EventHandler<RequestNoteSelectedArg> RequestNoteSelected;
        event EventHandler Clicked;
        string Title { set; }
        string CreationDate { set; }
        //int StateID { set; get; }
    }
    public interface I_View_DiagramStateInfo
    {
        string StateName { set; get; }
        string Date { set; get; }
        List<RequestActionDTO> CausingRequestActions { set; get; }
        List<RequestActionDTO> PossibleRequestActions { set; get; }
        event EventHandler ExisRequested;
    }
    public interface I_View_StateConnection
    {
        //event EventHandler<RequestNoteConfirmedArg> RequestNoteConfirmed;
        //event EventHandler<RequestNoteSelectedArg> RequestNoteSelected;
        event EventHandler Clicked;
        string Action { set; }
        string Duration { set; }
        string Tooltip { set; }
        string OrgnizatoinPostUserInfo { set; }
        bool Highlight { set; }
    }
    public interface I_View_Cartable
    {
        //event EventHandler<CartableEntityClickArg> EntitiyClicked;
        event EventHandler CartableRefreshClicked;
        event EventHandler<CartableMenuClickArg> MenuClicked;

        void ShowMessage(string v);
        event EventHandler<CartableItemSelectedArg> InfoClicked;

        //event EventHandler<CartableActionConfirmClickArg> MenuActionConfirmClicked;
        void ShowWorkflowRequests(List<WorkflowRequestDTO> requests);
        //void ShowMenuItems(List<RequestActionDTO> requestActions,bool );
    }
    public class CartableItemSelectedArg : EventArgs
    {
        public object DataItem { set; get; }
        public object UIElement { set; get; }
    }
    //public class WorkflowRequestCreationArg : EventArgs
    //{
    //    public CreateRequestDTO Request { set; get; }

    //}
    public class WorkflowAdminSearchArg : EventArgs
    {
        public string Search { set; get; }

    }
    public class WorkflowAdminSelectedArg : EventArgs
    {
        public List<OrganizationPostDTO> Posts { set; get; }

    }
    public class WorkflowStackholderSearchArg : EventArgs
    {
        public string Search { set; get; }

    }
    public class WorkflowStackholderSelectedArg : EventArgs
    {
        public List<OrganizationPostDTO> Posts { set; get; }

    }

    public class WorkflowReportAreaInitializer
    {
        public DP_DataView DataItem { get; set; }
        //public int EntityID { get; set; }
    }

}
