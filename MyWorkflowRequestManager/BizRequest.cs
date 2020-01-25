using DataAccess;
using ModelEntites;
using MyDataItemManager;
using MyFormulaFunctionStateFunctionLibrary;
using MyModelManager;

using ProxyLibrary;
using ProxyLibrary.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkflowRequestManager
{
    public class BizRequest
    {
        BizDataItem bizDataItem = new BizDataItem();
        public int CreateWorkflowRequest(CreateRequestDTO requestMessage, DR_Requester requester)
        {
            using (var context = new MyIdeaDataDBEntities())
            {
                using (var modelContext = new MyProjectEntities())
                {
                    var request = new Request();
                    request.ProcessID = requestMessage.ProcessID;
                    request.Title = requestMessage.Title;
                    //request.Desc = requestMessage.Desc;
                    request.Date = DateTime.Now;
                    request.CreatorUserID = requester.Identity;
                    request.CurrentStateID = requestMessage.CurrentStateID;
                    //   request.CreatorPostID = requestMessage.CreatorPostID;
                    if (requestMessage.DatItem != null)
                    {
                        if (requestMessage.DatItem.DataItemID == 0)
                        {
                            request.MyDataItemID = bizDataItem.GetOrCreateDataItem(requestMessage.DatItem);
                        }
                        else
                        {
                            request.MyDataItemID = requestMessage.DatItem.DataItemID;

                        }
                    }
                    //foreach (var item in requestMessage.RequestData)
                    //    request.RequestData.Add(new RequestData() { ColumnID = item.ColumnID, Value = item.Value });
                    foreach (var item in requestMessage.RequestNotes)
                    {
                        RequestNote dbNote = GetDBRequestNote(context, item, requester.Identity);
                        request.RequestNote.Add(dbNote);
                    }
                    //foreach (var item in requestMessage.RequestFiles)
                    //{
                    //    var dbFile = ToRequestFile(item, requester.Identity);
                    //    request.RequestFile.Add(dbFile);
                    //}
                    context.Request.Add(request);

                    //var transition = context.Transition.First(x => x.CurrentStateID == null && x.NextStateID == requestMessage.FirstStateID);
                    ////AddNewRequestActions(context, request, transition);
                    //foreach (var requestAction in request.RequestAction)
                    //    RequestActionToCompleted(context, request, requestAction, requester);
                    //CheckRequestActionsForTransition(context, request, requester);

                    RequestEnteredToState(context, modelContext, requestMessage.OutgoingTransitoinActions, request, request.CurrentStateID, requester, null, requestMessage.DatItem);
                    context.SaveChanges();
                    return request.ID;
                }
            }
        }
        private void RequestEnteredToState(MyIdeaDataDBEntities context, MyProjectEntities modelContext, List<PossibleTransitionActionDTO> roleTransitoinActions, Request dbrequest, int currentStateID, DR_Requester requester, RequestAction lastRequestAction, DP_DataView dataItem = null)
        {
            var dbState = modelContext.State.First(x => x.ID == currentStateID);
            if (dataItem == null && dbrequest.MyDataItem != null)
                dataItem = bizDataItem.ToDataViewDTO(requester, dbrequest.MyDataItem, false);
            if (dbState.State_Formula.Any())
            {
                if (dataItem != null)
                {
                    foreach (var formula in dbState.State_Formula)
                    {
                        var validFormula = ValidRequestActionToCompleted(context, modelContext, formula.FormulaID, dataItem, requester);
                        if (!validFormula)
                        {
                            string message = formula.Message;
                            if (string.IsNullOrEmpty(message))
                                message = "فرمول رعایت نشده است";
                            throw (new Exception(message));
                        }
                    }
                }
                else
                    throw new Exception("داده ای برای آزمایش فرمول موجود نیست");
            }



            DoStateActivities(context, dbrequest, currentStateID);
            //    When a Request enters a State, we get all outgoing Transitions from that state.
            //For each Action in those Transitions, we add an entry in RequestAction, with each entry having IsActive = 1 and IsCompleted = 0.
            foreach (var requestAction in dbrequest.RequestAction1.Where(x => x.IsActive == true))
            {
                //ابتدا ریکوئست اکشنهای قبلی رو غیر فعال میکند
                requestAction.IsActive = false;
            }
            var outGoingTransitions = modelContext.Transition.Where(x => x.CurrentStateID == currentStateID);
            foreach (var transition in outGoingTransitions)
            {
                AddNewRequestPossibleActions(context, roleTransitoinActions, dbrequest, transition, lastRequestAction);
            }
        }



        private void AddNewRequestPossibleActions(MyIdeaDataDBEntities context, List<PossibleTransitionActionDTO> sentTransitoinActions, Request dbrequest, Transition transition, RequestAction lastRequestAction)
        {
            foreach (var transitionAction in transition.TransitionAction)
            {
                Dictionary<int, string> selectedPostIDs = new Dictionary<int, string>();
                var sentTransitionAction = sentTransitoinActions.First(x => x.TransitionActionID == transitionAction.ID);
                if (transitionAction.MultipleUserEnabled == true)
                {
                    if (sentTransitionAction.OrganizationPosts.Count == 1)
                    {
                        var sentData = sentTransitionAction.OrganizationPosts.First();
                        selectedPostIDs.Add(sentData.OrganizationPost.ID, sentData.TargetReason);
                    }
                    else
                    {
                        foreach (var item in sentTransitionAction.OrganizationPosts.Where(x => x.Selected))
                        {
                            selectedPostIDs.Add(item.OrganizationPost.ID, item.TargetReason);
                        }
                    }
                }
                else
                {
                    if (sentTransitionAction.OrganizationPosts.Count == 1)
                    {
                        var sentData = sentTransitionAction.OrganizationPosts.First();
                        selectedPostIDs.Add(sentData.OrganizationPost.ID, sentData.TargetReason);
                    }
                    else
                    {
                        var sentData = sentTransitionAction.OrganizationPosts.First(x => x.Selected);
                        selectedPostIDs.Add(sentData.OrganizationPost.ID, sentData.TargetReason);
                    }
                }
                foreach (var selectedPostID in selectedPostIDs)
                {
                    var requestAction = new RequestAction();
                    requestAction.PossibleTransitionActionID = transitionAction.ID;
                    requestAction.PossibleTransitionID = transitionAction.TransitionID;
                    requestAction.PossibleActionID = transitionAction.ActionID;
                    requestAction.StateID = transition.CurrentStateID;
                    requestAction.IsActive = true;
                    requestAction.IsCompleted = false;
                    requestAction.DateTimeCompleted = null;
                    requestAction.DateTimeCreation = DateTime.Now;
                    requestAction.TargetOrganizationPostID = selectedPostID.Key;
                    requestAction.TargetReason = selectedPostID.Value;
                    if (lastRequestAction != null)
                        requestAction.CausingRequestActionID = lastRequestAction.ID;
                    dbrequest.RequestAction1.Add(requestAction);
                }
            }
        }



        public List<WorkflowRequestDTO> SearchWorkflows(DR_Requester requester, int processID, DateTime? fromDate, DateTime? toDate, DP_DataRepository dataItem, WFStateDTO currentState, WFStateDTO historyState, TransitionActionDTO selectedAction, int userID)
        {
            List<WorkflowRequestDTO> result = new List<WorkflowRequestDTO>();
            using (var context = new MyIdeaDataDBEntities())
            {
                using (var projectContext = new MyProjectEntities())
                {
                    var list = context.Request as IQueryable<Request>;
                    if (processID != 0)
                        list = list.Where(x => x.ProcessID == processID);
                    if (fromDate != null)
                    {
                        list = list.Where(x => x.Date >= fromDate);
                    }
                    if (toDate != null)
                    {
                        list = list.Where(x => x.Date <= toDate);
                    }
                    if (dataItem != null)
                    {
                        int dataItemID = bizDataItem.GetDataItemID(dataItem.TargetEntityID, dataItem.KeyProperties);
                        if (dataItemID == 0)
                            dataItemID = -1;
                        list = list.Where(x => x.MyDataItemID == dataItemID);
                    }
                    if (selectedAction != null)
                    {
                        list = list.Where(x => x.RequestAction1.Any(y => y.IsCompleted == true && y.PossibleTransitionActionID == selectedAction.ID));
                    }
                    if (currentState != null)
                    {
                        list = list.Where(x => x.CurrentStateID == currentState.ID);
                    }
                    if (historyState != null)
                    {
                        list = list.Where(x => x.RequestAction1.Any(y => y.StateID == historyState.ID || y.LedToState == historyState.ID));
                    }
                    if (userID != 0)
                    {

                        var orgPosts = projectContext.OrganizationPost.Where(x => x.UserID == userID).Select(x => x.ID).ToList();
                        list = list.Where(x => x.RequestAction1.Any(y => y.IsActive == true && orgPosts.Contains(y.TargetOrganizationPostID)));

                    }
                    foreach (var item in list)
                    {
                        result.Add(ToWorkflowRequestDTO(requester, item, projectContext));
                    }
                }

            }
            return result;
        }

        public List<RequestActionDTO> GetRequestActions(DR_Requester requester, List<int> requestActionIDs)
        {
            List<RequestActionDTO> result = new List<RequestActionDTO>();
            using (var context = new MyIdeaDataDBEntities())
            {
                //  var targetTypeRequester = (int)TargetType.Requester;
                //  var targetTypeAdmin = (int)TargetType.ProcessAdmins;
                // var targetTypeRoleMemeber = (int)TargetType.RoleMembers;
                var list = context.RequestAction.Where(x => requestActionIDs.Contains(x.ID));
                //list = list.Where(y => y.IsActive == true && (
                //  (y.TransitionAction.Action.ActionTarget.Any(z => z.TargetType == targetTypeRequester && requester.PostIds.Contains(y.Request.RequesterPostID)))
                //  //////|| (y.TransitionAction.Action.ActionTarget.Any(z => z.TargetType == targetTypeAdmin) && requester.PostIds.Contains(y.Request.AdminRoleID))
                //  || (y.TransitionAction.Action.ActionTarget.Any(z => z.TargetType == targetTypeRoleMemeber) && requester.PostIds.Contains(y.OrganizationPostID))
                //  ));

                foreach (var item in list)
                {

                    //    new  TransitionActionDTO();
                    //requestAction.TransitionAction.ActionID = item.TransitionAction.ActionID;
                    ////requestAction.TransitionAction.TransitionID = item.TransitionAction.TransitionID;
                    //requestAction.TransitionAction.TransitionActionID = item.TransitionActionID;
                    //requestAction.TransitionAction.ActionName = item.TransitionAction.Action.Name;
                    //requestAction.TransitionAction.EntityGroupRelationships = new List<EntityGroupRelationship>();
                    //foreach (var group in item.TransitionAction.TransitionAction_EntityGroup)
                    //{
                    //    foreach (var relationship in group.EntityGroup.EntityGroup_Relationship)
                    //    {
                    //        EntityGroupRelationship egRelationship = new EntityGroupRelationship();
                    //        egRelationship.EntityGroupName = group.EntityGroup.Name;
                    //        egRelationship.RelationshipID = relationship.RelationshipID;
                    //        egRelationship.EntityID = relationship.Relationship.TableDrivedEntityID2;
                    //        egRelationship.EntityName = relationship.Relationship.TableDrivedEntity1.Name;
                    //        requestAction.TransitionAction.EntityGroupRelationships.Add(egRelationship);
                    //    }
                    //}
                    result.Add(ToRequestActionDTO(requester, item, true));
                }
                return result;
            }
        }

        public RequestDiagramDTO GetRequestDiagram(DR_Requester requester, int requestID)
        {
            RequestDiagramDTO result = new RequestDiagramDTO();
            using (var context = new MyIdeaDataDBEntities())
            {
                using (var modelContext = new MyProjectEntities())
                {
                    var dbRequest = context.Request.First(x => x.ID == requestID);
                    result.ProcessID = dbRequest.ProcessID;
                    result.RequestID = dbRequest.ID;
                    result.Title = dbRequest.Title;
                    if (dbRequest.RequestAction1.Any())
                    {
                        var firstRequestAction = dbRequest.RequestAction1.First();
                        var transitionAction = modelContext.TransitionAction.FirstOrDefault(x => x.ID == firstRequestAction.PossibleTransitionActionID);
                        result.FirstDiagramState = ToDisgramState(requester, transitionAction.Transition.State, firstRequestAction.DateTimeCreation, new List<RequestAction>(), dbRequest.RequestAction1.Where(x => x.CausingRequestActionID == null).ToList());
                        foreach (var item in dbRequest.RequestAction1.Where(x => x.LedToState != null).OrderBy(x => x.DateTimeCompleted))
                        {
                            var state = modelContext.State.First(x => x.ID == item.LedToState.Value);
                            result.DiagramStates.Add(ToDisgramState(requester, state, item.DateTimeCompleted.Value, dbRequest.RequestAction1.Where(x => x.PossibleTransitionID == item.PossibleTransitionID && x.IsCompleted && x.CausingRequestActionID == item.CausingRequestActionID).ToList(), dbRequest.RequestAction1.Where(x => x.CausingRequestActionID == item.ID).ToList()));
                        }
                        //var groupedRequestActoins = dbRequest.RequestAction.GroupBy(x => x.DateTimeCreation);
                        //foreach (var item in groupedRequestActoins)
                        //{
                        //    var groupedByTransition = item.GroupBy(x => x.TransitionID).Where(x => x.All(y => y.IsCompleted));
                        //    if (groupedByTransition.Count() > 1)
                        //        throw (new Exception("two transition!"));
                        //    else if (groupedByTransition.Count() == 1)
                        //    {
                        //        var completedrequestActionGroup = groupedByTransition.First();
                        //        var lastDiagramState = result.DiagramStates.LastOrDefault();
                        //        if (lastDiagramState == null)
                        //            lastDiagramState = result.FirstDiagramState;
                        //        var completedTransition = modelContext.Transition.First(x => x.ID == completedrequestActionGroup.Key);
                        //        if (completedTransition.CurrentStateID != lastDiagramState.StateID)
                        //            throw (new Exception("missing state!"));
                        //        result.DiagramStates.Add(ToDisgramState(completedTransition.State1, item.Key, completedrequestActionGroup.ToList()));
                        //    }
                        //}


                        //  List<Tuple<Transition, DateTime, List<RequestAction>>> listTransition = new List<Tuple<Transition, DateTime, List<RequestAction>>>();
                        //foreach (var item in groupedRequestActoins)
                        //{
                        //    if (item.All(x => x.IsCompleted == true))
                        //    {//انتقالها و تاریخ ایجاد ریکوئست اکشن(یا حالت)و ریکوئست اکشنهای خارج شده از حالت
                        //        listTransition.Add(new Tuple<Transition, DateTime, List<RequestAction>>(item.Key.Transition, item.Key.DateTimeCreation.Value, item.ToList()));
                        //    }
                        //}

                        //State previousState = null;
                        //var causingRequestActoins = new List<RequestAction>();
                        //foreach (var item in listTransition)
                        //{
                        //    if (previousState == null)
                        //    {
                        //        previousState = item.Item1.State;
                        //        result.DiagramStates.Add(ToDisgramState(item.Item1.State, item.Item2, causingRequestActoins));
                        //        causingRequestActoins = item.Item3;
                        //    }


                        //    var secondSideState = item.Item1.State1;
                        //    result.DiagramStates.Add(ToDisgramState(item.Item1.State, item.Item2, item));

                        //}

                    }
                    else
                    {
                        var state = modelContext.State.FirstOrDefault(x => x.ID == dbRequest.CurrentStateID);
                        result.FirstDiagramState = ToDisgramState(requester, state, dbRequest.Date, new List<RequestAction>(), new List<RequestAction>());
                        //result.CurrentDiagramState = ToDisgramState(dbRequest.State, dbRequest.Date.Value, new List<RequestAction>());
                    }

                }
            }
            return result;
        }

        private DiagramStateDTO ToDisgramState(DR_Requester requester, State state, DateTime item2, List<RequestAction> listCausingReuestActions, List<RequestAction> listPossibleReuestActions)
        {
            DiagramStateDTO result = new DiagramStateDTO();
            result.StateID = state.ID;
            result.Name = state.Name;
            result.CreationDate = item2;
            result.StateType = (StateType)state.StateTypeID;
            foreach (var item in listCausingReuestActions)
            {
                result.CausingRequestActions.Add(ToRequestActionDTO(requester, item, true));
                //result.CausingRequestActions.Add(ToRequestActionDTO(item, true));
                //result.CausingRequestActions.Add(ToRequestActionDTO(item, true));
            }
            foreach (var item in listPossibleReuestActions)
            {
                result.PossibleRequestActions.Add(ToRequestActionDTO(requester, item, true));
                //result.CausingRequestActions.Add(ToRequestActionDTO(item, true));
                //result.CausingRequestActions.Add(ToRequestActionDTO(item, true));
            }
            return result;
        }

        public void SaveRequestNote(RequestNoteDTO requestNote, DR_Requester requester)
        {
            using (var context = new MyIdeaDataDBEntities())
            {
                requestNote.Date = DateTime.Now;
                RequestNote dbRequestNote = GetDBRequestNote(context, requestNote, requester.Identity);
                if (dbRequestNote.ID == 0)
                    context.RequestNote.Add(dbRequestNote);
                context.SaveChanges();
            }
        }
        private RequestNote GetDBRequestNote(MyIdeaDataDBEntities context, RequestNoteDTO requestNoteDTO, int userID)
        {
            RequestNote dbRequestNote = new RequestNote();
            if (requestNoteDTO.ID == 0)
            {
                dbRequestNote = new RequestNote();
                dbRequestNote.UserID = userID;
                dbRequestNote.RequestID = requestNoteDTO.RequestID;
            }
            else
                dbRequestNote = context.RequestNote.First(x => x.ID == requestNoteDTO.ID);

            dbRequestNote.Date = DateTime.Now;
            dbRequestNote.Note = requestNoteDTO.Note;
            dbRequestNote.NoteTitle = requestNoteDTO.NoteTitle;
            return dbRequestNote;
        }
        public List<RequestNoteDTO> GetRequestNotes(int requestID, bool withRequestActionDescriptions)
        {
            List<RequestNoteDTO> result = new List<RequestNoteDTO>();
            using (var context = new MyIdeaDataDBEntities())
            {
                using (var modelContext = new MyProjectEntities())
                {
                    var dbRequest = context.Request.First(x => x.ID == requestID);
                    foreach (var item in dbRequest.RequestNote)
                    {
                        result.Add(ToRequestNoteDTO(item));
                    }

                    foreach (var item in dbRequest.RequestAction1.Where(x => x.IsCompleted && !string.IsNullOrEmpty(x.Description)))
                    {
                        result.Add(ToRequestNoteDTO(item, modelContext));
                    }
                }
            }
            return result.OrderBy(x => x.Date).ToList();
        }
        BizUser bizUser = new BizUser();
        private RequestNoteDTO ToRequestNoteDTO(RequestAction item, MyProjectEntities context)
        {
            RequestNoteDTO result = new RequestNoteDTO();
            result.UserID = item.DoerUserID.Value;
            if (result.UserID != 0)
                result.UserFullName = bizUser.GetUser(result.UserID);
            result.RequestID = item.RequestID;
            result.Note = item.Description;
            result.Date = item.DateTimeCompleted.Value;
            var transitionAction = context.TransitionAction.First(x => x.ID == item.PossibleTransitionActionID);
            result.NoteTitle = transitionAction.Name;
            return result;
        }

        private RequestNoteDTO ToRequestNoteDTO(RequestNote item)
        {
            RequestNoteDTO result = new RequestNoteDTO();
            result.ID = item.ID;
            result.UserID = item.UserID;
            if (result.UserID != 0)
                result.UserFullName = bizUser.GetUser(result.UserID);
            result.RequestID = item.RequestID;
            result.Note = item.Note;
            result.Date = item.Date;
            result.NoteTitle = item.NoteTitle;
            return result;
        }
        //public void SaveRequestFile(RequestFileDTO requestFile, DR_Requester requester)
        //{
        //    using (var context = new MyIdeaDataDBEntities())
        //    {
        //        requestFile.Date = DateTime.Now;
        //        RequestFile dbRequestFile = GetDBRequestFile(context, requestFile, requester.Identity);
        //        if (dbRequestFile.ID == 0)
        //            context.RequestFile.Add(dbRequestFile);
        //        context.SaveChanges();
        //    }
        //}
        //private RequestFile GetDBRequestFile(MyIdeaDataDBEntities context, RequestFileDTO requestFileDTO, int userID)
        //{
        //    RequestFile dbRequestFile = new RequestFile();
        //    if (requestFileDTO.ID == 0)
        //    {
        //        dbRequestFile = new RequestFile();
        //        dbRequestFile.FileContent = requestFileDTO.FileContent;
        //        dbRequestFile.MIMEType = requestFileDTO.MIMEType;
        //        dbRequestFile.UserID = userID;
        //        dbRequestFile.RequestID = requestFileDTO.RequestID;
        //        dbRequestFile.Date = requestFileDTO.Date;
        //    }
        //    else
        //        dbRequestFile = context.RequestFile.First(x => x.ID == requestFileDTO.ID);

        //    dbRequestFile.FileName = requestFileDTO.FileName;
        //    dbRequestFile.FileDesc = requestFileDTO.FileDesc;
        //    return dbRequestFile;
        //}
        //public RequestFileDTO GetRequestFile(int requestFileID)
        //{
        //    using (var context = new MyIdeaDataDBEntities())
        //    {
        //        var item = context.RequestFile.First(x => x.ID == requestFileID);
        //        return ToRequestFileDTO(item, true);
        //    }
        //}
        //public List<RequestFileDTO> GetRequestFiles(int requestID)
        //{
        //    List<RequestFileDTO> result = new List<RequestFileDTO>();
        //    using (var context = new MyIdeaDataDBEntities())
        //    {
        //        var list = context.RequestFile.Where(x => x.RequestID == requestID);
        //        foreach (var item in list)
        //        {
        //            result.Add(ToRequestFileDTO(item, false));
        //        }
        //    }
        //    return result;
        //}
        //private RequestFileDTO ToRequestFileDTO(RequestFile item, bool withContent)
        //{
        //    RequestFileDTO result = new RequestFileDTO();
        //    result.ID = item.ID;
        //    result.UserID = item.UserID;
        //    result.RequestID = item.RequestID;
        //    result.MIMEType = item.MIMEType;
        //    result.FileName = item.FileName;
        //    result.Date = item.Date;
        //    result.FileDesc = item.FileDesc;
        //    if (withContent)
        //        result.FileContent = item.FileContent;
        //    return result;
        //}
        private void DoStateActivities(MyIdeaDataDBEntities context, Request dbrequest, int currentStateID)
        {
            //var state = context.State.First(x => x.ID == currentStateID);
            //foreach (var activity in state.StateActivity)
            //{

            //}
        }

        public List<ProcessDTO> GetProcessesForExecution(DR_Requester requester)
        {
            List<ProcessDTO> result = new List<ProcessDTO>();
            using (var context = new MyProjectEntities())
            {
                BizProcess bizProcess = new BizProcess();
                var list = context.Process;//.Where(x => x.RequestInitializers.Any(y => requester.RoleGroupIds.Contains(y.RoleGroupID)));
                foreach (var item in context.Process)
                {
                    if (item.TableDrivedEntityID == null || bizTableDrivedEntity.DataIsAccessable(requester, item.TableDrivedEntity))
                        result.Add(bizProcess.ToProcessDTO(requester, item, false));
                }

            }
            return result;
        }




        //private RequestFile ToRequestFile(RequestFileDTO item, int userID)
        //{
        //    RequestFile result = new RequestFile();
        //    result.UserID = userID;
        //    result.Date = DateTime.Now;
        //    result.FileName = item.FileName;
        //    result.MIMEType = item.MIMEType;
        //    result.FileContent = item.FileContent;
        //    return result;
        //}

        //public List<WFActionDTO> GetRequestPossibleActions(DR_Requester requester, int requestID, bool withDetails)
        //{
        //    List<WFActionDTO> result = new List<WFActionDTO>();
        //    using (var context = new MyIdeaDataDBEntities())
        //    {
        //        using (var modelContext = new MyProjectEntities())
        //        {
        //            //  var targetTypeRequester = (int)TargetType.Requester;
        //            //  var targetTypeAdmin = (int)TargetType.ProcessAdmins;
        //            // var targetTypeRoleMemeber = (int)TargetType.RoleMembers;
        //            var list = context.RequestAction.Where(x => x.IsActive == true && x.RequestID == requestID && requester.PostIds.Contains(x.OrganizationPostID));
        //            //list = list.Where(y => y.IsActive == true && (
        //            //  (y.TransitionAction.Action.ActionTarget.Any(z => z.TargetType == targetTypeRequester && requester.PostIds.Contains(y.Request.RequesterPostID)))
        //            //  //////|| (y.TransitionAction.Action.ActionTarget.Any(z => z.TargetType == targetTypeAdmin) && requester.PostIds.Contains(y.Request.AdminRoleID))
        //            //  || (y.TransitionAction.Action.ActionTarget.Any(z => z.TargetType == targetTypeRoleMemeber) && requester.PostIds.Contains(y.OrganizationPostID))
        //            //  ));

        //            foreach (var item in list)
        //            {

        //                //    new  TransitionActionDTO();
        //                //requestAction.TransitionAction.ActionID = item.TransitionAction.ActionID;
        //                ////requestAction.TransitionAction.TransitionID = item.TransitionAction.TransitionID;
        //                //requestAction.TransitionAction.TransitionActionID = item.TransitionActionID;
        //                //requestAction.TransitionAction.ActionName = item.TransitionAction.Action.Name;
        //                //requestAction.TransitionAction.EntityGroupRelationships = new List<EntityGroupRelationship>();
        //                //foreach (var group in item.TransitionAction.TransitionAction_EntityGroup)
        //                //{
        //                //    foreach (var relationship in group.EntityGroup.EntityGroup_Relationship)
        //                //    {
        //                //        EntityGroupRelationship egRelationship = new EntityGroupRelationship();
        //                //        egRelationship.EntityGroupName = group.EntityGroup.Name;
        //                //        egRelationship.RelationshipID = relationship.RelationshipID;
        //                //        egRelationship.EntityID = relationship.Relationship.TableDrivedEntityID2;
        //                //        egRelationship.EntityName = relationship.Relationship.TableDrivedEntity1.Name;
        //                //        requestAction.TransitionAction.EntityGroupRelationships.Add(egRelationship);
        //                //    }
        //                //}\
        //                var transactionAction = modelContext.TransitionAction.First(x => x.ID == item.TransitionActionID);
        //                BizAction bizAction = new BizAction();
        //                result.Add(bizAction.ToActionDTO(transactionAction.Action, withDetails));
        //            }
        //            return result;
        //        }
        //    }
        //}
        public List<TransitionActionDTO> GetRequestPossibleTransitionActions(DR_Requester requester, int requestID)
        {
            List<TransitionActionDTO> result = new List<TransitionActionDTO>();
            using (var context = new MyIdeaDataDBEntities())
            {
                using (var modelContext = new MyProjectEntities())
                {
                    //  var targetTypeRequester = (int)TargetType.Requester;
                    //  var targetTypeAdmin = (int)TargetType.ProcessAdmins;
                    // var targetTypeRoleMemeber = (int)TargetType.RoleMembers;
                    var list = context.RequestAction.Where(x => x.IsActive == true && x.RequestID == requestID && requester.PostIds.Contains(x.TargetOrganizationPostID));
                    //list = list.Where(y => y.IsActive == true && (
                    //  (y.TransitionAction.Action.ActionTarget.Any(z => z.TargetType == targetTypeRequester && requester.PostIds.Contains(y.Request.RequesterPostID)))
                    //  //////|| (y.TransitionAction.Action.ActionTarget.Any(z => z.TargetType == targetTypeAdmin) && requester.PostIds.Contains(y.Request.AdminRoleID))
                    //  || (y.TransitionAction.Action.ActionTarget.Any(z => z.TargetType == targetTypeRoleMemeber) && requester.PostIds.Contains(y.OrganizationPostID))
                    //  ));
                    BizTransition bizTransition = new BizTransition();

                    foreach (var item in list)
                    {
                        result.Add(bizTransition.GetTransitionAction(requester, item.PossibleTransitionActionID));
                    }
                    return result;
                }
            }
        }
        private RequestActionDTO ToRequestActionDTO(DR_Requester requester, RequestAction item, bool withDetails)
        {
            RequestActionDTO requestAction = new RequestActionDTO();
            requestAction.ID = item.ID;
            requestAction.RequestID = item.RequestID;
            requestAction.PossibleActionID = item.PossibleActionID;
            requestAction.PossibleTransitionID = item.PossibleTransitionID;
            requestAction.IsActive = item.IsActive;
            requestAction.IsCompleted = item.IsCompleted;
            requestAction.TargetOrganizationPostID = item.TargetOrganizationPostID;
            requestAction.TargetReason = item.TargetReason;
            requestAction.Description = item.Description;
            if (item.DoerUserID != null)
                requestAction.DoerUserID = item.DoerUserID.Value;
            requestAction.DateTimeCreation = item.DateTimeCreation;
            requestAction.PossibleTransitionActionID = item.PossibleTransitionActionID;
            if (item.DateTimeCompleted != null)
                requestAction.DateTimeCompleted = item.DateTimeCompleted.Value;
            else
                requestAction.DateTimeCompleted = null;
            if (item.CausingRequestActionID != null)
                requestAction.CausingRequestActionID = item.CausingRequestActionID.Value;
            else
                requestAction.CausingRequestActionID = 0;
            if (item.LedToState != null)
                requestAction.LedToState = item.LedToState.Value;
            else
                requestAction.LedToState = 0;
            if (withDetails)
            {
                BizOrganization bizOrganization = new BizOrganization();
                BizUser bizUser = new BizUser();
                if (requestAction.DoerUserID != 0)
                    requestAction.User = bizUser.GetUser(requestAction.DoerUserID);
                requestAction.OrganizationPost = bizOrganization.GetOrganizationPostsByID(item.TargetOrganizationPostID);
                BizTransition bizTransition = new BizTransition();
                requestAction.TransitionAction = bizTransition.GetTransitionAction(requester, item.PossibleTransitionActionID);
            }
            return requestAction;
        }
        BizOrganization bizOrganization = new BizOrganization();
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        public List<WorkflowRequestDTO> GetWorkflowRequests(DR_Requester requester)
        {
            List<WorkflowRequestDTO> result = new List<WorkflowRequestDTO>();
            using (var context = new MyIdeaDataDBEntities())
            {
                using (var modelContext = new MyProjectEntities())
                {
                    var requestActions = context.RequestAction.Where(y => y.IsActive == true && y.IsCompleted == false && requester.PostIds.Contains(y.TargetOrganizationPostID));

                    foreach (var request in requestActions.GroupBy(x => x.Request1))
                    {
                        var wfRequest = ToWorkflowRequestDTO(requester, request.Key, modelContext);
                        foreach (var rc in request)
                        {
                            wfRequest.RequestActionIDs.Add(rc.ID);
                        }
                        result.Add(wfRequest);
                    }
                }
                return result;
            }
        }

        private WorkflowRequestDTO ToWorkflowRequestDTO(DR_Requester requester, Request request, MyProjectEntities modelContext)
        {
            WorkflowRequestDTO wfRequest = new WorkflowRequestDTO();
            wfRequest.ID = request.ID;
            wfRequest.ProcessID = request.ProcessID;
            var process = modelContext.Process.FirstOrDefault(x => x.ID == wfRequest.ProcessID);

            if (process != null)
                wfRequest.Process = process.Name;
            //wfRequest.Description = request.Desc;
            wfRequest.CurrentStateID = request.CurrentStateID;
            var state = modelContext.State.FirstOrDefault(x => x.ID == wfRequest.CurrentStateID);
            if (state != null)
                wfRequest.CurrentState = state.Name;
            wfRequest.Title = request.Title;
            //wfRequest.AdminPostIDs = request.RequestAdmins.Select(x => x.OrganizationPostID).ToList();
            if (request.MyDataItem != null)
            {

                wfRequest.DataItem = bizDataItem.ToDataViewDTO(requester, request.MyDataItem, true);
                //روش خوب است؟
                wfRequest.DataItemInfo = wfRequest.DataItem.ViewInfo;
                wfRequest.EntityName = bizTableDrivedEntity.GetTableDrivedEntity(requester, request.MyDataItem.TableDrivedEntityID, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships).Alias;
            }
            // wfRequest.CreatorPostID = request.CreatorPostID;
            //   var orgPost = bizOrganization.GetOrganizationPostsByID(request.CreatorPostID);
            // wfRequest.CreatorPostName = orgPost.Name;
            //wfRequest.RecieverPostName = request.o;
            wfRequest.CreatorUserID = request.CreatorUserID;
            wfRequest.CreatorUserFullName = bizUser.GetUserFullName(wfRequest.CreatorUserID);
            if (request.LastRequestActionID != null)
            {
                var transitionAction = modelContext.TransitionAction.First(x => x.ID == request.RequestAction.PossibleTransitionActionID);
                wfRequest.LastActionName = transitionAction.Name;
                wfRequest.LastActionDate = request.RequestAction.DateTimeCompleted.Value;
                wfRequest.LastActionUser = bizUser.GetUserFullName(request.RequestAction.DoerUserID.Value);
            }
            wfRequest.Date = request.Date.ToString();
            return wfRequest;
        }



        public void SaveRequestAction(RequestActionConfirmDTO requestAction, DR_Requester requester)
        {
            using (var context = new MyIdeaDataDBEntities())
            {
                using (var modelContext = new MyProjectEntities())
                {
                    //var dbRequestAction = context.RequestAction.First(x => x.ActionID == requestAction.ActionID && x.RequestID == requestAction.RequestID && x.OrganizationPostID == requestAction.RequesterPostID);
                    var dbRequestAction = context.RequestAction.First(x => x.ID == requestAction.RequestActionID);
                    var transitionAction = modelContext.TransitionAction.First(x => x.ID == dbRequestAction.PossibleTransitionActionID);
                    if (transitionAction.TransitionAction_Formula.Any())
                    {
                        var dataItem = bizDataItem.ToDataViewDTO(requester, dbRequestAction.Request1.MyDataItem, false);
                        if (dataItem != null)
                        {
                            foreach (var formula in transitionAction.TransitionAction_Formula)
                            {
                                var validFormula = ValidRequestActionToCompleted(context, modelContext, formula.FormulaID, dataItem, requester);
                                if (!validFormula)
                                {
                                    string message = formula.Message;
                                    if (string.IsNullOrEmpty(message))
                                        message = "فرمول رعایت نشده است";
                                    throw (new Exception(message));
                                }
                            }
                        }
                        else
                            throw new Exception("داده ای برای آزمایش فرمول موجود نیست");
                    }

                    var transition = transitionAction.Transition;
                    dbRequestAction.DateTimeCompleted = DateTime.Now;
                    dbRequestAction.Description = requestAction.Description;
                    dbRequestAction.IsCompleted = true;
                    dbRequestAction.DoerUserID = requester.Identity;
                    bool transitoinIsCompleted = TransitionIsCompleted(dbRequestAction.Request1, transition, dbRequestAction);
                    //    var alltransitionActions = dbRequestAction.Request.RequestAction.Where(x => x.TransitionID == transitionID);
                    //if (alltransitionActions.All(x => x.IsCompleted))
                    if (transitoinIsCompleted)
                    {
                        dbRequestAction.LedToState = transition.NextStateID;
                        DoTransition(context, modelContext, requestAction.OutgoingTransitoinActions, dbRequestAction.Request1, transition, requester, dbRequestAction);
                    }
                    //if (transition.TransitionAction.Where(x=> x.RequestAction))

                    //   var request = dbRequestAction.Request;
                    //CheckRequestActionsForTransition(context, request, requester);
                    context.SaveChanges();



                }
            }
        }

        private bool ValidRequestActionToCompleted(MyIdeaDataDBEntities context, MyProjectEntities modelContext, int formulaID, DP_DataView dataItem, DR_Requester requester)
        {
            bool isValid = true;

            FormulaFunctionHandler formulaFunctionHandler = new FormulaFunctionHandler();
            BizTableDrivedEntity bizEntity = new BizTableDrivedEntity();
            var result = formulaFunctionHandler.CalculateFormula(formulaID, dataItem, requester);
            if (Convert.ToBoolean(result.Result))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //private bool CheckRequestActionsForTransition(MyIdeaDataDBEntities context, RequestAction requestAction)
        //{
        //    //چک میکنه که آیا همه اکشن ها برای انتقال انجام شده؟
        //    return requestAction.Request.
        //    foreach (var transitionGroup in request.RequestAction.Where(x => x.IsActive == true).GroupBy(x => x.TransitionAction.TransitionID))
        //    {
        //        if (transitionGroup.All(x => x.IsCompleted == true))
        //        {

        //            var transition = context.Transition.First(x => x.ID == transitionGroup.Key);
        //            DoTransition(context, request, transition);
        //        }
        //    }
        //}

        private void DoTransition(MyIdeaDataDBEntities context, MyProjectEntities modelContext, List<PossibleTransitionActionDTO> roleTransitoinActions, Request request, Transition transition, DR_Requester requester, RequestAction lastRequestAction)
        {

            request.CurrentStateID = transition.NextStateID;
            request.LastRequestActionID = lastRequestAction.ID;
            //////foreach (var TransitionActivity in transition.TransitionActivity)
            //////    DoTransitionActivity(context, TransitionActivity);

            RequestEnteredToState(context, modelContext, roleTransitoinActions, request, request.CurrentStateID, requester, lastRequestAction);

            //foreach (var requestAction in request.RequestAction)
            //    requestAction.IsActive = false;

            //foreach (var nextTransition in transition.State1.Transition)
            //{
            //    AddNewRequestActions(context, request, nextTransition);
            //}



            //var state = context.State.First(x => x.ID == request.CurrentStateID);
            //foreach (var stateActivity in state.StateActivity)
            //    DoStateActivity(context, stateActivity);
        }

        //public DP_DataRepository GetDataItem(int requestID)
        //{
        //    using (var context = new MyIdeaDataDBEntities())
        //    {
        //        var request = context.Request.First(x => x.ID == requestID);
        //        BizTableDrivedEntity bizEntity = new BizTableDrivedEntity();

        //        return GetDataItem(bizEntity.GetTableDrivedEntity(request.Process.TableDrivedEntityID.Value, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships), request.RequestData);
        //    }
        //}
        //private DP_DataRepository GetDataItem(TableDrivedEntityDTO tableDrivedEntity, ICollection<RequestData> requestData)
        //{
        //    DP_DataRepository result = new DP_DataRepository();
        //    result.TargetEntityID = tableDrivedEntity.ID;
        //    foreach (var item in requestData)
        //        result.AddProperty(tableDrivedEntity.Columns.First(x => x.ID == item.ColumnID), item.Value);
        //    return result;
        //}

        private bool TransitionIsCompleted(Request request, Transition transition, RequestAction excluderequestAction)
        {
            bool isCompleted = true;
            foreach (var transitionAction in transition.TransitionAction)
            {
                if (transitionAction.ID != excluderequestAction.PossibleTransitionActionID)
                {
                    if (!request.RequestAction1.Any(x => x.IsActive && x.PossibleTransitionActionID == transitionAction.ID && x.IsCompleted))
                    {
                        isCompleted = false;
                        break;
                    }
                }
            }
            return isCompleted;
        }

        //این فرضیه که کارها با اکشن پیش بروند و نه به ازای هر ریکوئست اکشن یه ایرادی داره
        // اگه اکشن مشترک باشه بین دو تا ترنزیشن برای مثال یک ترنزیشن اکشن آ را بصورت تنها نیاز داشته باشه و نرنزیشن دیگری اکشن راآ را به همرا اکشن ب نیاز داشته باشه
        //حال اگر فرص شود که اول اکشن ب انجام بشه و بعد آ اونوقت هر دو ترنزیشن کامل میشه!! که این نقصه

        //public List<PossibleTransitionActionDTO> GetNextPossibleTransitionActionByActionID(int actionID, int requestID)
        //{
        //    List<PossibleTransitionActionDTO> result = new List<PossibleTransitionActionDTO>();
        //    using (var context = new MyIdeaDataDBEntities())
        //    {
        //        using (var modelContext = new MyProjectEntities())
        //        {

        //            List<int> adminPosts = null;
        //            int requesterPostId = 0;

        //            var dbrequest = context.Request.FirstOrDefault(x => x.ID == requestID);
        //            //درخاست دهنده یا شروع کننده جریان کار
        //            requesterPostId = dbrequest.RequesterPostID;
        //            adminPosts = dbrequest.RequestAdmins.Select(x => x.ID).ToList();


        //            BizRole bizRole = new BizRole();
        //            List<int> completedTransition = new List<int>();
        //            var requestActions = context.RequestAction.Where(x => x.RequestID == requestID && x.ActionID == actionID && x.IsActive == true);
        //            foreach (var item in requestActions)
        //            {
        //                if (!context.RequestAction.Any(x => x.RequestID == requestID && x.TransitionID == item.TransitionID && x.ActionID != actionID && !x.IsCompleted && x.IsActive == true))
        //                    completedTransition.Add(item.TransitionID);
        //            }
        //            if (completedTransition.Count > 1)
        //            {
        //                throw (new Exception("بیش از یک انتقال امکانپذیر است"));
        //            }
        //            else if (completedTransition.Count == 1)
        //            {
        //                var transitionID = completedTransition.First();
        //                var outGoingTransitions = modelContext.Transition.Where(x => x.ID == transitionID);
        //                return GetNextPossibleTransitionActions(outGoingTransitions, requesterPostId, adminPosts);


        //            }
        //        }
        //    }
        //    return result;
        //}
        public PossibleTransitionActionResult GetNextPossibleTransitionActionByRequestActionID(int requestActionID, OrganizationPostDTO organizationPost)
        {
            PossibleTransitionActionResult result = new PossibleTransitionActionResult();
            using (var context = new MyIdeaDataDBEntities())
            {
                using (var modelContext = new MyProjectEntities())
                {

                    //List<int> adminPosts = null;
                    // int creatorPostId = 0;
                    var dbRequestAction = context.RequestAction.FirstOrDefault(x => x.ID == requestActionID);
                    //   var dbrequest = dbRequestAction.Request;
                    //درخاست دهنده یا شروع کننده جریان کار
                    //  creatorPostId = dbRequestAction.Request1.CreatorPostID;
                    //adminPosts = dbRequestAction.Request1.RequestAdmins.Select(x => x.ID).ToList();

                    BizRole bizRole = new BizRole();
                    //List<int> completedTransition = new List<int>();
                    // bool transitoinIsCompleted = false;
                    var transition = modelContext.Transition.First(x => x.ID == dbRequestAction.PossibleTransitionID);

                    bool transitoinIsCompleted = TransitionIsCompleted(dbRequestAction.Request1, transition, dbRequestAction);
                    //  !context.RequestAction.Any(x => !x.IsCompleted && x.IsActive == true && x.RequestID == dbRequestAction.ID && x.ID != dbRequestAction.ID && x.TransitionID == dbRequestAction.TransitionID);
                    //foreach (var item in requestActions)
                    //{
                    //    if (!context.RequestAction.Any(x => x.RequestID == requestID && x.TransitionID == item.TransitionID && x.ActionID != actionID && !x.IsCompleted && x.IsActive == true))
                    //        completedTransition.Add(item.TransitionID);
                    //}
                    //if (completedTransition.Count > 1)
                    //{
                    //    throw (new Exception("بیش از یک انتقال امکانپذیر است"));
                    //}
                    //else if (completedTransition.Count == 1)
                    //{
                    result.NextState = transition.State1.Name;
                    if (((StateType)transition.State1.StateTypeID) == StateType.End)
                    {
                        result.IsEndingState = true;
                    }
                    else
                    {
                        if (transitoinIsCompleted)
                        {
                            var possibleTransitionActions = GetNextPossibleTransitionActions(transition.State1, organizationPost, context, dbRequestAction.RequestID);
                            result.PossibleTransitionActions = possibleTransitionActions.Item1;
                            result.SharedOrganizationPosts = possibleTransitionActions.Item2;
                        }
                        else
                            result.IsNotTransitioningAction = true;
                    }
                    //}
                }
            }
            return result;
        }

        public PossibleTransitionActionResult GetNextPossibleTransitionActionByStateID(int stateID, OrganizationPostDTO organizationPost)
        {
            PossibleTransitionActionResult result = new PossibleTransitionActionResult();
            using (var context = new MyIdeaDataDBEntities())
            {
                using (var modelContext = new MyProjectEntities())
                {
                    BizRole bizRole = new BizRole();
                    //var outGoingTransition = modelContext.Transition.First(x => x.CurrentStateID == stateID);
                    var state = modelContext.State.First(x => x.ID == stateID);

                    //از وضعیت شروع فقط یدونه ترنزیشن باید بره
                    var possibleTransitionActions = GetNextPossibleTransitionActions(state, organizationPost, context, 0);
                    result.PossibleTransitionActions = possibleTransitionActions.Item1;
                    result.SharedOrganizationPosts = possibleTransitionActions.Item2;
                }
            }
            return result;
        }
        private Tuple<List<PossibleTransitionActionDTO>, List<TransitionActionOrganizationPostDTO>> GetNextPossibleTransitionActions(State state, OrganizationPostDTO organizationPost, MyIdeaDataDBEntities context, int requestID)
        {
            List<PossibleTransitionActionDTO> result = new List<PossibleTransitionActionDTO>();
            foreach (var transition in state.Transition)
            {
                foreach (var transitionAction in transition.TransitionAction)
                {
                    Dictionary<int, string> postIDs = new Dictionary<int, string>();
                    foreach (var actiontarget in transitionAction.TransitionActionTarget)
                    {
                        var targetType = (TargetType)actiontarget.TargetType;
                        //if (targetType == TargetType.ProcessAdmins)
                        //{
                        //    foreach (var adminID in adminPosts)
                        //    {
                        //        var title = "راهبر جریان کار";
                        //        if (postIDs.ContainsKey(adminID))
                        //        {
                        //            postIDs[adminID] = postIDs[adminID] += "," + title;
                        //        }
                        //        else
                        //        {
                        //            postIDs.Add(adminID, title);
                        //        }
                        //    }
                        //}
                        //else if (targetType == TargetType.Requester)
                        //{
                        //    var title = "ایجاد کننده جریان کار";
                        //    if (postIDs.ContainsKey(creatorPostId))
                        //    {
                        //        postIDs[creatorPostId] = postIDs[creatorPostId] += "," + title;

                        //    }
                        //    else
                        //    {
                        //        postIDs.Add(creatorPostId, title);
                        //    }
                        //}
                        if (targetType == TargetType.RoleMembers)
                        {
                            foreach (var orgtypetoletype in actiontarget.RoleType.OrganizationType_RoleType)
                            {

                                //foreach (var orguser in item.RoleType.OrganizationUser_Role)
                                //{
                                //    if (!users.Any(x => x.ID == orguser.Organization_User.UserID))
                                //        users.Add(orguser.Organization_User.User);
                                //}
                                foreach (var post in orgtypetoletype.OrganizationPost)
                                {

                                    if (!transitionAction.CanSendOtherOrganizations)
                                    {
                                        if (post.OrganizationID != organizationPost.OrganizationID)
                                            continue;
                                    }
                                    var title = "نقش سازمانی";
                                    if (postIDs.ContainsKey(post.ID))
                                    {
                                        postIDs[post.ID] = postIDs[post.ID] += "," + title;
                                    }
                                    else
                                    {
                                        postIDs.Add(post.ID, title);
                                    }

                                }
                            }
                        }
                    }

                    //اینجا میشه با تغییراتی در جریان کار از خود داده هم به منظور انتخاب پیش فرض مقصد ها استفاده کرد.

                    //فعلا از سابقه تایید ها استفاده میکنیم
                    int defaultPostID = 0;
                    if (requestID != 0)
                    {
                        var onlyPostIDs = postIDs.Select(x => x.Key).ToList();
                        var doneRequestActions = context.RequestAction.Where(x => x.RequestID == requestID && x.IsCompleted == true).OrderByDescending(x => x.DateTimeCompleted);
                        if (doneRequestActions.Any(x => onlyPostIDs.Contains(x.TargetOrganizationPostID)))
                        {
                            if (doneRequestActions.Any(x => x.PossibleTransitionActionID == transitionAction.ID && onlyPostIDs.Contains(x.TargetOrganizationPostID)))
                            {
                                defaultPostID = doneRequestActions.First(x => x.PossibleTransitionActionID == transitionAction.ID && onlyPostIDs.Contains(x.TargetOrganizationPostID)).TargetOrganizationPostID;
                            }
                            else if (doneRequestActions.Any(x => x.PossibleTransitionID == transitionAction.TransitionID && onlyPostIDs.Contains(x.TargetOrganizationPostID)))
                            {
                                defaultPostID = doneRequestActions.First(x => x.PossibleTransitionID == transitionAction.TransitionID && onlyPostIDs.Contains(x.TargetOrganizationPostID)).TargetOrganizationPostID;
                            }
                            else if (doneRequestActions.Any(x => x.StateID == state.ID && onlyPostIDs.Contains(x.TargetOrganizationPostID)))
                            {
                                defaultPostID = doneRequestActions.First(x => x.StateID == state.ID && onlyPostIDs.Contains(x.TargetOrganizationPostID)).TargetOrganizationPostID;
                            }
                            else
                            {
                                defaultPostID = doneRequestActions.First(x => onlyPostIDs.Contains(x.TargetOrganizationPostID)).TargetOrganizationPostID;
                            }
                        }
                    }
                    PossibleTransitionActionDTO transitoinActionPost = new PossibleTransitionActionDTO();
                    // transitoinActionPost.Name = transitionAction.Action.Name;
                    transitoinActionPost.MultipleUserEnabled = transitionAction.MultipleUserEnabled;
                    transitoinActionPost.CanSendOtherOrganizations = transitionAction.CanSendOtherOrganizations;
                    transitoinActionPost.TransitionActionID = transitionAction.ID;
                    transitoinActionPost.TransitionID = transition.ID;
                    transitoinActionPost.TransitionName = transition.Name;
                    //    transitoinActionPost.Title = transition.Name + ">>" + transition.State1.Name + " - " + transitionAction.Action.Name; ;
                    transitoinActionPost.Title = transition.Name + " - " + transitionAction.Name;

                    BizOrganization bizOrganization = new BizOrganization();

                    foreach (var post in postIDs)
                    {
                        var nPost = new TransitionActionOrganizationPostDTO();
                        nPost.OrganizationPost = bizOrganization.GetOrganizationPostsByID(post.Key);
                        nPost.TargetReason = post.Value;
                        if (defaultPostID != 0 && defaultPostID == post.Key)
                            nPost.Selected = true;
                        transitoinActionPost.OrganizationPosts.Add(nPost);
                    }
                    result.Add(transitoinActionPost);
                }
            }
            var sharedPostsAll = result.SelectMany(x => x.OrganizationPosts).Where(x => result.All(y => y.OrganizationPosts.Any(z => z.OrganizationPost.ID == x.OrganizationPost.ID)));
            List<TransitionActionOrganizationPostDTO> sharedOneList = new List<TransitionActionOrganizationPostDTO>();
            foreach (var item in sharedPostsAll.GroupBy(x => x.OrganizationPost.ID))
            {
                TransitionActionOrganizationPostDTO sharedOne = new TransitionActionOrganizationPostDTO();
                sharedOne.OrganizationPost = bizOrganization.GetOrganizationPostsByID(item.Key);
                string reason = "";
                foreach (var reasonGroup in item.GroupBy(x => x.TargetReason))
                {
                    reason += (reason == "" ? "" : ",") + reasonGroup.Key;
                }
                sharedOne.TargetReason = reason;
                sharedOne.Selected = item.All(x => x.Selected);
                sharedOneList.Add(sharedOne);
            }
            return new Tuple<List<PossibleTransitionActionDTO>, List<TransitionActionOrganizationPostDTO>>(result, sharedOneList);
        }
    }
}
