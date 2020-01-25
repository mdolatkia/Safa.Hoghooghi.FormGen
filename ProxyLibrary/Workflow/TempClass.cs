using ModelEntites;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyLibrary.Workflow
{
    public class ProcessDTO
    {
        public ProcessDTO()
        {
            //AdminRoles = new List<RoleDTO>();
            //AdminRoleTypes = new List<RoleTypeDTO>();
            States = new List<Workflow.WFStateDTO>();
            Actions = new List<Workflow.WFActionDTO>();
            Activities = new List<Workflow.ActivityDTO>();
            EntityGroups = new List<Workflow.EntityGroupDTO>();

            //ProcessInitializerRoleGroups = new List<RoleGroupDTO>();
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string TransitionFlowSTR { get; set; }
        public int EntityID { get; set; }

        //public int AdminRoleGroupID { get; set; }
        //public List<RoleTypeDTO> AdminRoleTypes { get; set; }

        //public List<RoleDTO> ProcessRoles { get; set; }
        //public List<RoleGroupDTO> ProcessInitializerRoleGroups { get; set; }
        public List<WFStateDTO> States { get; set; }
        public List<WFActionDTO> Actions { get; set; }
        public List<ActivityDTO> Activities { get; set; }
        public List<EntityGroupDTO> EntityGroups { get; set; }

    }


    public class WFStateDTO
    {
        public WFStateDTO()
        {
            Activities = new List<ActivityDTO>();
            Formulas = new List<Workflow.StateFormulaDTO>();
        }
        public int ID { get; set; }
        public int ProcessID { get; set; }
        //public int StateTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public StateType StateType { get; set; }
        public List<ActivityDTO> Activities { get; set; }
        public List<StateFormulaDTO> Formulas { set; get; }
        //public  ICollection<Transition> Transition { get; set; }
        //public  ICollection<Transition> Transition1 { get; set; }
    }
    public class StateFormulaDTO
    {
        public int FormulaID { set; get; }
        //public FormulaDTO Formula { set; get; }
        public string Message { set; get; }
    }
    public class RequestDiagramDTO
    {
        public RequestDiagramDTO()
        {
            DiagramStates = new List<Workflow.DiagramStateDTO>();
        }
        public int ProcessID { get; set; }
        public int RequestID { get; set; }
        public string Title { get; set; }

        public DiagramStateDTO FirstDiagramState { set; get; }
        //public DiagramStateDTO CurrentDiagramState { set; get; }
        public List<DiagramStateDTO> DiagramStates { set; get; }
    }

    public class DiagramStateDTO
    {
        public DiagramStateDTO()
        {
            CausingRequestActions = new List<Workflow.RequestActionDTO>();
            PossibleRequestActions = new List<Workflow.RequestActionDTO>();
        }
        //public int ID { get; set; }
        public DateTime CreationDate { get; set; }
        public string Name { get; set; }
        public StateType StateType { get; set; }

        public int StateID { set; get; }
        public List<RequestActionDTO> CausingRequestActions { set; get; }
        public List<RequestActionDTO> PossibleRequestActions { set; get; }
    }

    public enum StateType
    {
        Start,
        Normal,
        End
    }

    public class ActivityDTO
    {
        public ActivityDTO()
        {
            this.Targets = new List<ActivityTargetDTO>();

        }

        public int ID { get; set; }
        //public int ActivityTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<bool> vwSelected { get; set; }
        public ActivityType ActivityType { get; set; }
        public List<ActivityTargetDTO> Targets { get; set; }
        public int ProcessID { get; set; }
    }
    public enum ActivityType
    {
        SendMessage,
        SendEmail
    }
    public class ActivityTargetDTO
    {
        public ActivityTargetDTO()
        {
            RoleTypes = new List<RoleTypeDTO>();
        }
        public int ID { get; set; }
        public List<RoleTypeDTO> RoleTypes { get; set; }
        public TargetType TargetType { get; set; }
    }


    public enum TargetType
    {
        //Requester,
        //Stackholder,
        //ProcessAdmins,
        RoleMembers
    }

    public class TransitionDTO
    {
        public TransitionDTO()
        {
            TransitionActions = new List<TransitionActionDTO>();
            TransitionActivities = new List<ActivityDTO>();
        }

        public int ID { get; set; }
        public int ProcessID { get; set; }
        public WFStateDTO CurrentState { get; set; }
        public int CurrentStateID { get; set; }
        public int NextStateID { get; set; }
        public WFStateDTO NextState { get; set; }
        //public  State State { get; set; }
        //public  State State1 { get; set; }
        public List<TransitionActionDTO> TransitionActions { get; set; }
        public List<ActivityDTO> TransitionActivities { get; set; }
        public string Name { get; set; }
    }

    public class TransitionActionDTO
    {
        public TransitionActionDTO()
        {
            Formulas = new List<Workflow.TransitionActionFormulaDTO>();
            EntityGroups = new List<EntityGroupDTO>();
            Targets = new List<Workflow.TransitionActionTargetDTO>();
        }

        public int ID { get; set; }
        public int ActionID { set; get; }
        public WFActionDTO Action { set; get; }
        public SimpleTransitionDTO Transition { set; get; }
        public List<EntityGroupDTO> EntityGroups { set; get; }
        public List<TransitionActionFormulaDTO> Formulas { set; get; }
        public bool MultipleUserEnabled { set; get; }
        public string Name { set; get; }
        public List<TransitionActionTargetDTO> Targets { get; set; }
        public bool CanSendOtherOrganizations { get; set; }
    }
    public class SimpleTransitionDTO
    {
        public SimpleTransitionDTO()
        {
        }

        public int ID { get; set; }
        public int ProcessID { get; set; }
        public string FirstStateName { get; set; }
        public string NextStateName { get; set; }
        public string Name { get; set; }
    }
    public class TransitionActionFormulaDTO
    {
        public int FormulaID { set; get; }
        //public FormulaDTO Formula { set; get; }
        public string Message { set; get; }
    }
    public class WFActionDTO
    {
        public WFActionDTO()
        {

        }

        public int ID { get; set; }
        public int ProcessID { get; set; }
        //public int ActionTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<bool> vwSelected { get; set; }
        public ActionType ActionType { get; set; }
        //public  Process Process { get; set; }


        //public  ICollection<TransitionAction> TransitionAction { get; set; }
    }
    public class TransitionActionTargetDTO
    {
        public TransitionActionTargetDTO()
        {
            //Roles = new List<RoleDTO>();
        }
        public int ID { get; set; }
        public TargetType TargetType { get; set; }
        //public RoleGroupDTO RoleGroup { get; set; }
        public int RoleTypeID { get; set; }
        public RoleTypeDTO RoleType { get; set; }
    }
    public enum ActionType
    {
        Approve,
        Deny,
        Cancel,
        Restart,
        Resolve

    }

    public class EntityGroupDTO
    {
        public EntityGroupDTO()
        {
            Relationships = new List<Workflow.EntityGroupRelationshipDTO>();
            //this.EntityGroup_Relationship = new HashSet<EntityGroup_Relationship>();
            //this.TransitionAction_EntityGroup = new HashSet<TransitionAction_EntityGroup>();
        }

        public string Name { get; set; }
        public int ProcessID { get; set; }
        public int ID { get; set; }
        public List<EntityGroupRelationshipDTO> Relationships { get; set; }

        //public  ICollection<EntityGroup_Relationship> EntityGroup_Relationship { get; set; }
        //public  Process Process { get; set; }
        //public  ICollection<TransitionAction_EntityGroup> TransitionAction_EntityGroup { get; set; }
    }

    public class EntityGroupRelationshipDTO
    {


        //public string Name { get; set; }
        public int RelationshipTailID { get; set; }
        public EntityRelationshipTailDTO RelationshipTail { get; set; }
        //public int EntityID { get; set; }
        public int ID { get; set; }
        public string vwName { get; set; }
    }
    public class CreateRequestDTO
    {
        public CreateRequestDTO()
        {
            //RequestData = new List<Workflow.RequestDataDTO>();
            RequestNotes = new List<Workflow.RequestNoteDTO>();
            //RequestFiles = new List<Workflow.RequestFileDTO>();
            //StackHolderPostIDs = new List<int>();
            OutgoingTransitoinActions = new List<PossibleTransitionActionDTO>();
        }
        public int ProcessID { set; get; }
        public int CurrentStateID { set; get; }
        //public int CreatorPostID { set; get; }
        //public int RequesterUserID { set; get; }
        //public List<int> AdminPostIDs { set; get; }
        //public List<int> StackHolderPostIDs { set; get; }
        public string Title { set; get; }
        public DP_DataView DatItem { set; get; }
        //public List<RequestDataDTO> RequestData { set; get; }
        public List<RequestNoteDTO> RequestNotes { set; get; }
        //public List<RequestFileDTO> RequestFiles { set; get; }
        public List<PossibleTransitionActionDTO> OutgoingTransitoinActions { set; get; }
        //public string Desc { get; set; }
    }
    public class WorkflowRequestDTO
    {
        public WorkflowRequestDTO()
        {
            //RequestData = new List<Workflow.RequestDataDTO>();
            RequestNotes = new List<Workflow.RequestNoteDTO>();
            RequestFiles = new List<Workflow.RequestFileDTO>();
            OutgoingTransitoinActions = new List<PossibleTransitionActionDTO>();
            RequestActionIDs = new List<int>();
        }
        public List<int> RequestActionIDs { set; get; }
        //public DP_DataRepository DatItem { set; get; }
        public int ID { set; get; }
        public int ProcessID { set; get; }
        public string Process { set; get; }
        public int CurrentStateID { set; get; }
        //public int CreatorPostID { set; get; }
        public int CreatorUserID { set; get; }
        //  public string CreatorPostName { set; get; }
        public string CreatorUserFullName { set; get; }
        //public string RecieverPostName { set; get; }
        //public List<int> AdminPostIDs { set; get; }
        public string Title { set; get; }
        public string CurrentState { set; get; }
        public string Date { set; get; }
        //public string Description { set; get; }
        public DP_DataView DataItem { set; get; }
        public string DataItemInfo { set; get; }
        public string EntityName { set; get; }
        //public List<RequestDataDTO> RequestData { set; get; }
        public List<RequestNoteDTO> RequestNotes { set; get; }
        public List<RequestFileDTO> RequestFiles { set; get; }
        public List<PossibleTransitionActionDTO> OutgoingTransitoinActions { set; get; }

        public string LastActionName { get; set; }
        public DateTime? LastActionDate { get; set; }
        public string LastActionUser { get; set; }
    }
    //public class PossibleTransactionDTO
    //{
    //    public PossibleTransactionDTO()
    //    {
    //        PossibleTransactionActions = new List<Workflow.PossibleTransactionActionDTO>();
    //    }
    //    public int Transition { set; get; }
    //    public string TransitionTitle { set; get; }
    //   public List<PossibleTransactionActionDTO> PossibleTransactionActions { set; get; }
    //}
    public class PossibleTransitionActionResult
    {
        public PossibleTransitionActionResult()
        {
            PossibleTransitionActions = new List<Workflow.PossibleTransitionActionDTO>();
            SharedOrganizationPosts = new List<TransitionActionOrganizationPostDTO>();
        }
        public string NextState { set; get; }
        public bool IsNotTransitioningAction { set; get; }
        public bool IsEndingState { set; get; }
        public List<PossibleTransitionActionDTO> PossibleTransitionActions { set; get; }

        public List<Workflow.TransitionActionOrganizationPostDTO> SharedOrganizationPosts { set; get; }
    }
    public class PossibleTransitionActionDTO
    {
        public PossibleTransitionActionDTO()
        {
            OrganizationPosts = new List<Workflow.TransitionActionOrganizationPostDTO>();
        }
        public int TransitionID { set; get; }
        public string TransitionName { set; get; }
        public string Title { set; get; }
        public int TransitionActionID { set; get; }
        public bool MultipleUserEnabled { set; get; }
        public List<TransitionActionOrganizationPostDTO> OrganizationPosts { set; get; }
        public bool CanSendOtherOrganizations { get; set; }
        public ItemColor Color { set; get; }
    }
    public enum ItemColor
    {
        Normal,
        Green,
        Red
    }
    public class TransitionActionOrganizationPostDTO
    {
        public event EventHandler SelectedChanged;
        public OrganizationPostDTO OrganizationPost { set; get; }
        bool _Selected;
        public bool Selected
        {
            get { return _Selected; }
            set
            {
                _Selected = value;
                if (SelectedChanged != null)
                    SelectedChanged(this, null);
            }
        }
        public string TargetReason { set; get; }
        //public string Title { set; get; }
    }
    public class RequestDataDTO
    {
        public int ColumnID { set; get; }

        public string Value { set; get; }
    }

    public class RequestNoteDTO
    {
        public int ID { set; get; }
        public int UserID { set; get; }
        public int RequestID { set; get; }
        public string Note { set; get; }
        public string NoteTitle { set; get; }
        public DateTime Date { set; get; }
        public UserDTO UserFullName { get; set; }
    }
    public class RequestFileDTO
    {
        public int ID { set; get; }
        public int UserID { set; get; }
        public int RequestID { set; get; }
        public DateTime Date { set; get; }
        public string FileName { set; get; }
        public string FileDesc { set; get; }
        public byte[] FileContent { set; get; }
        public string MIMEType { set; get; }

    }

    public class RequestActionDTO
    {
        public RequestActionDTO()
        {

        }
        public int ID { set; get; }
        public int RequestID { set; get; }
        public bool IsActive { set; get; }
        public bool IsCompleted { set; get; }
        public int DoerUserID { set; get; }
        public UserDTO User { set; get; }
        public int TargetOrganizationPostID { set; get; }
        public OrganizationPostDTO OrganizationPost { set; get; }
        public DateTime DateTimeCreation { set; get; }
        public DateTime? DateTimeCompleted { set; get; }
        public TransitionActionDTO TransitionAction { set; get; }
        public int PossibleTransitionActionID { get; set; }
        public int PossibleActionID { get; set; }
        public int PossibleTransitionID { get; set; }
        public int CausingRequestActionID { get; set; }
        public int LedToState { get; set; }
        public string Description { set; get; }
        public string TargetReason { get; set; }
    }
    public class RequestActionConfirmDTO
    {
        public int RequestActionID { set; get; }
        public List<PossibleTransitionActionDTO> OutgoingTransitoinActions { set; get; }
        public string Description { get; set; }
    }
    public class ActionConfirmDTO
    {
        public int RequesterPostID { set; get; }
        public int RequestID { set; get; }

        public int ActionID { set; get; }
        public List<PossibleTransitionActionDTO> OutgoingTransitoinActions { set; get; }

    }
    //public class TransitionActionMessage

    //{
    //    public int TransitionActionID { set; get; }
    //    public int TransitionID { set; get; }

    //    public int ActionID { set; get; }
    //    public List<EntityGroupRelationship> EntityGroupRelationships { set; get; }

    //    public string ActionName { set; get; }
    //}

    //public class EntityGroupRelationship

    //{
    //    public string EntityGroupName { set; get; }
    //    public int RelationshipID { set; get; }

    //    public int EntityID { set; get; }
    //    public string EntityName { set; get; }
    //}



}
