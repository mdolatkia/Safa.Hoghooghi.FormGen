using DataAccess;
using ModelEntites;

using ProxyLibrary.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using MyModelManager;
using System.Data.Entity.Validation;
using ProxyLibrary;

namespace MyModelManager
{
    public class BizTransition
    {
        public List<TransitionDTO> GetTransitions(DR_Requester requester, int processID, bool withDetails)
        {
            List<TransitionDTO> result = new List<TransitionDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var listTransition = projectContext.Transition.Where(x => x.ProcessID == processID);
                foreach (var item in listTransition)
                    result.Add(ToTransitionDTO(requester,item, withDetails));
            }
            return result;
        }
        //public TransitionDTO GetTransition(int TransitionsID, bool withDetails)
        //{

        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var dbTransition = projectContext.Transition.First(x => x.ID == TransitionsID);
        //        return ToTransitionDTO(dbTransition, withDetails);
        //    }
        //}
        public TransitionActionDTO GetTransitionAction(DR_Requester requester, int ID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbTransitionAction = projectContext.TransitionAction.First(x => x.ID == ID);
                return ToTransitionAction(requester, dbTransitionAction, true);
            }
        }
        public List<TransitionActionDTO> GetTransitionActionsByProcessID(DR_Requester requester, int iD)
        {
            List<TransitionActionDTO> result = new List<TransitionActionDTO>();
            using (var projectContext = new MyProjectEntities())
            {
                var transitionActions = projectContext.Transition.SelectMany(x => x.TransitionAction);
                foreach (var item in transitionActions)
                {
                    result.Add(ToTransitionAction(requester, item, false));
                }
            }
            return result;
        }

        private SimpleTransitionDTO ToSimpleTransitionDTO(Transition item)
        {
            SimpleTransitionDTO result = new SimpleTransitionDTO();
            result.ID = item.ID;
            result.FirstStateName = item.State.Name;
            result.NextStateName = item.State1.Name;
            result.Name = item.Name;
            result.ProcessID = item.ProcessID;
            return result;
        }
        public TransitionDTO ToTransitionDTO(DR_Requester requester, DataAccess.Transition item, bool withDetails)
        {
            TransitionDTO result = new TransitionDTO();
            result.ID = item.ID;
            result.CurrentStateID = item.CurrentStateID;
            result.NextStateID = item.NextStateID;
            result.Name = item.Name;

            result.ProcessID = item.ProcessID;

            if (withDetails)
            {
                BizState bizState = new BizState();
                result.CurrentState = bizState.ToStateDTO(item.State, false);
                result.NextState = bizState.ToStateDTO(item.State1, false);

                BizActivity bizActivity = new BizActivity();
                foreach (var act in item.TransitionActivity)
                {
                    result.TransitionActivities.Add(bizActivity.ToActivityDTO(act.Activity, withDetails));
                }

                foreach (var act in item.TransitionAction)
                {
                    result.TransitionActions.Add(ToTransitionAction(requester,act, withDetails));
                }
            }
            return result;
        }

        public TransitionActionDTO ToTransitionAction(DR_Requester requester, TransitionAction dbTransitionAction, bool withDetails)
        {
            BizAction bizAction = new BizAction();
            BizFormula bizFormula = new BizFormula();
            BizEntityGroup bizEntityGroup = new BizEntityGroup();
            BizTransition bizTransition = new BizTransition();
            var result = new TransitionActionDTO();
            result.ID = dbTransitionAction.ID;
            result.ActionID = dbTransitionAction.ActionID;
            result.MultipleUserEnabled = dbTransitionAction.MultipleUserEnabled;
            result.CanSendOtherOrganizations = dbTransitionAction.CanSendOtherOrganizations;
            result.Name = dbTransitionAction.Name;
            if (withDetails)
            {
                result.Action = bizAction.ToActionDTO(dbTransitionAction.Action);
                result.Transition = bizTransition.ToSimpleTransitionDTO(dbTransitionAction.Transition);

                BizTarget bizTarget = new BizTarget();
                BizRoleType bizRoleType = new BizRoleType();
                foreach (var target in dbTransitionAction.TransitionActionTarget)
                {
                    TransitionActionTargetDTO at = new TransitionActionTargetDTO();
                    at.ID = target.ID;
                    at.TargetType = (TargetType)target.TargetType;
                    if (target.RoleTypeID != null)
                    {
                        at.RoleTypeID = target.RoleTypeID.Value;
                        at.RoleType = bizRoleType.ToRoleTypeDTO(target.RoleType);
                    }
                    result.Targets.Add(at);
                }

                foreach (var sbsf in dbTransitionAction.TransitionAction_Formula)
                {
                    var sf = new TransitionActionFormulaDTO();
                    sf.FormulaID = sbsf.FormulaID;
                    sf.Message = sbsf.Message;
                    result.Formulas.Add(sf);
                }
                foreach (var entityGroup in dbTransitionAction.TransitionAction_EntityGroup)
                {
                    result.EntityGroups.Add(bizEntityGroup.ToEntityGroupDTO(requester, entityGroup.EntityGroup, withDetails));
                }
            }
            return result;
        }



        public bool UpdateTransitions(int ProcessID, List<TransitionDTO> list, string flowSTR)
        {
            try
            {
                //مهم و پیچیده نسبتا؟ اگر از جریان کار استفاده شده باشد امکان تغییر جریان کار بررسی شود
                using (var projectContext = new MyProjectEntities())
                {

                    var dbProcess = projectContext.Process.FirstOrDefault(x => x.ID == ProcessID);
                    dbProcess.TransitionFlowSTR = flowSTR;
                    var existingTransitions = list.Where(x => x.ID != 0).Select(x => x.ID).ToList();
                    var listRemove = dbProcess.Transition.Where(x => !existingTransitions.Contains(x.ID));
                    foreach (var item in listRemove.ToList())
                    {
                        while (item.TransitionActivity.Any())
                            projectContext.TransitionActivity.Remove(item.TransitionActivity.First());

                        while (item.TransitionAction.Any())
                        {
                            while (item.TransitionAction.First().TransitionAction_EntityGroup.Any())
                                projectContext.TransitionAction_EntityGroup.Remove(item.TransitionAction.First().TransitionAction_EntityGroup.First());
                            while (item.TransitionAction.First().TransitionAction_Formula.Any())
                                projectContext.TransitionAction_Formula.Remove(item.TransitionAction.First().TransitionAction_Formula.First());
                            while (item.TransitionAction.First().TransitionActionTarget.Any())
                                projectContext.TransitionActionTarget.Remove(item.TransitionAction.First().TransitionActionTarget.First());
                            projectContext.TransitionAction.Remove(item.TransitionAction.First());

                        }
                        //item.TransitionAction.Clear();
                        //item.TransitionActivity.Clear();

                        projectContext.Transition.Remove(item);
                    }
                    foreach (var message in list)
                    {
                        Transition dbTransition = null;
                        if (message.ID == 0)
                        {
                            dbTransition = new Transition();
                            projectContext.Transition.Add(dbTransition);
                        }
                        else
                            dbTransition = projectContext.Transition.FirstOrDefault(x => x.ID == message.ID);

                        dbTransition.CurrentStateID = message.CurrentStateID;

                        dbTransition.ProcessID = message.ProcessID;
                        dbTransition.NextStateID = message.NextStateID;
                        dbTransition.Name = message.Name;

                        List<TransitionActivity> removeActivity = new List<TransitionActivity>();
                        foreach (var dbActivity in dbTransition.TransitionActivity)
                        {
                            if (!message.TransitionActivities.Any(x => x.ID == dbActivity.ActivityID))
                                removeActivity.Add(dbActivity);
                        }
                        foreach (var citem in removeActivity)
                        {
                            projectContext.TransitionActivity.Remove(citem);
                        }

                        foreach (var msgActivity in message.TransitionActivities)
                        {
                            TransitionActivity dbTransitionActivity = null;
                            if (!dbTransition.TransitionActivity.Any(x => x.ActivityID == msgActivity.ID))
                            {
                                dbTransitionActivity = new TransitionActivity();
                                dbTransition.TransitionActivity.Add(dbTransitionActivity);
                            }
                            else
                                dbTransitionActivity = dbTransition.TransitionActivity.FirstOrDefault(x => x.ActivityID == msgActivity.ID);
                            dbTransitionActivity.ActivityID = msgActivity.ID;
                        }


                        List<TransitionAction> removeAction = new List<TransitionAction>();
                        foreach (var dbAction in dbTransition.TransitionAction)
                        {
                            if (!message.TransitionActions.Any(x => x.ID == dbAction.ID))
                                removeAction.Add(dbAction);
                        }
                        foreach (var citem in removeAction)
                        {
                            while (citem.TransitionAction_EntityGroup.Any())
                                projectContext.TransitionAction_EntityGroup.Remove(citem.TransitionAction_EntityGroup.First());
                            while (citem.TransitionAction_Formula.Any())
                                projectContext.TransitionAction_Formula.Remove(citem.TransitionAction_Formula.First());
                            while (citem.TransitionActionTarget.Any())
                                projectContext.TransitionActionTarget.Remove(citem.TransitionActionTarget.First());
                            projectContext.TransitionAction.Remove(citem);

                        }

                        foreach (var msgTransitionAction in message.TransitionActions)
                        {
                            TransitionAction dbTransitionAction = null;
                            if (!dbTransition.TransitionAction.Any(x => x.ID == msgTransitionAction.ID))
                            {
                                dbTransitionAction = new TransitionAction();
                                dbTransition.TransitionAction.Add(dbTransitionAction);

                            }
                            else
                                dbTransitionAction = dbTransition.TransitionAction.FirstOrDefault(x => x.ID == msgTransitionAction.ID);
                            dbTransitionAction.ActionID = msgTransitionAction.ActionID;
                            dbTransitionAction.MultipleUserEnabled = msgTransitionAction.MultipleUserEnabled;
                            dbTransitionAction.CanSendOtherOrganizations = msgTransitionAction.CanSendOtherOrganizations;

                            dbTransitionAction.Name = msgTransitionAction.Name;

                            List<TransitionActionTarget> removeTransitionActionTarget = new List<TransitionActionTarget>();
                            foreach (var dbTransitionAction_Target in dbTransitionAction.TransitionActionTarget)
                            {
                                if (!msgTransitionAction.Targets.Any(x => x.ID == dbTransitionAction_Target.ID))
                                    removeTransitionActionTarget.Add(dbTransitionAction_Target);
                            }
                            foreach (var citem in removeTransitionActionTarget)
                            {
                                projectContext.TransitionActionTarget.Remove(citem);
                            }

                            foreach (var item in msgTransitionAction.Targets)
                            {
                                TransitionActionTarget dbTransitionActionTarget = dbTransitionAction.TransitionActionTarget.FirstOrDefault(x => x.ID == item.ID);
                                if (dbTransitionActionTarget == null)
                                {
                                    dbTransitionActionTarget = new TransitionActionTarget();
                                    dbTransitionAction.TransitionActionTarget.Add(dbTransitionActionTarget);

                                }
                                dbTransitionActionTarget.TargetType = (Int16)item.TargetType;
                                if (dbTransitionActionTarget.RoleTypeID != 0)
                                    dbTransitionActionTarget.RoleTypeID = item.RoleTypeID;
                                else
                                    dbTransitionActionTarget.RoleTypeID = null;
                            }

                            List<TransitionAction_Formula> removeTransitionAction_Formula = new List<TransitionAction_Formula>();
                            foreach (var dbTransitionAction_Formula in dbTransitionAction.TransitionAction_Formula)
                            {
                                if (!msgTransitionAction.Formulas.Any(x => x.FormulaID == dbTransitionAction_Formula.FormulaID))
                                    removeTransitionAction_Formula.Add(dbTransitionAction_Formula);
                            }
                            foreach (var citem in removeTransitionAction_Formula)
                            {
                                projectContext.TransitionAction_Formula.Remove(citem);
                            }

                            foreach (var item in msgTransitionAction.Formulas)
                            {
                                TransitionAction_Formula dbTransitionAction_Formula = dbTransitionAction.TransitionAction_Formula.FirstOrDefault(x => x.FormulaID == item.FormulaID);
                                if (dbTransitionAction_Formula == null)
                                {
                                    dbTransitionAction_Formula = new TransitionAction_Formula();
                                    dbTransitionAction.TransitionAction_Formula.Add(dbTransitionAction_Formula);

                                }
                                dbTransitionAction_Formula.FormulaID = item.FormulaID;
                                dbTransitionAction_Formula.Message = item.Message;
                            }
                            List<TransitionAction_EntityGroup> removeTransitionAction_EntityGroup = new List<TransitionAction_EntityGroup>();
                            foreach (var dbTransitionAction_EntityGroup in dbTransitionAction.TransitionAction_EntityGroup)
                            {
                                if (!msgTransitionAction.EntityGroups.Any(x => x.ID == dbTransitionAction_EntityGroup.EntityGroupID))
                                    removeTransitionAction_EntityGroup.Add(dbTransitionAction_EntityGroup);
                            }
                            foreach (var citem in removeTransitionAction_EntityGroup)
                            {
                                projectContext.TransitionAction_EntityGroup.Remove(citem);
                            }

                            foreach (var item in msgTransitionAction.EntityGroups)
                            {
                                TransitionAction_EntityGroup dbTransitionAction_EntityGroup = dbTransitionAction.TransitionAction_EntityGroup.FirstOrDefault(x => x.EntityGroupID == item.ID);
                                if (dbTransitionAction_EntityGroup == null)
                                {
                                    dbTransitionAction_EntityGroup = new TransitionAction_EntityGroup();
                                    dbTransitionAction.TransitionAction_EntityGroup.Add(dbTransitionAction_EntityGroup);
                                }
                                dbTransitionAction_EntityGroup.EntityGroupID = item.ID;

                            }
                        }
                    }

                    projectContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        //public int UpdateTransitions(TransitionDTO message)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var dbTransition = projectContext.Transition.FirstOrDefault(x => x.ID == message.ID);
        //        if (dbTransition == null)
        //            dbTransition = new DataAccess.Transition();
        //        dbTransition.ID = message.ID;
        //        dbTransition.Name = message.Name;
        //        dbTransition.TransitionTypeID = message.TransitionTypeID;
        //        dbTransition.ProcessID = message.ProcessID;
        //        dbTransition.Description = message.Description;
        //        while (dbTransition.TransitionTarget.Any())
        //            projectContext.TransitionTarget.Remove(dbTransition.TransitionTarget.First());
        //        foreach (var msgTransitionTarget in message.Targets)
        //        {
        //            var dbTransitionTarget = new TransitionTarget();
        //            dbTransitionTarget.TargetID = msgTransitionTarget.TargetID;
        //            foreach (var role in msgTransitionTarget.Roles)
        //            {
        //                dbTransitionTarget.TransitionTarget_Role.Add(new TransitionTarget_Role() { RoleID = role.ID });
        //            }
        //            dbTransition.TransitionTarget.Add(dbTransitionTarget);
        //        }
        //        if (dbTransition.ID == 0)
        //            projectContext.Transition.Add(dbTransition);
        //        projectContext.SaveChanges();
        //        return dbTransition.ID;
        //    }
        //}
    }
}
