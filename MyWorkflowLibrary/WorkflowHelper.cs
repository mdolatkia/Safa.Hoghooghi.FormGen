
using MySecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using MyModelManager;
using ModelEntites;
using DataAccess;

namespace MyWorkflowLibrary
{
    public class WorkflowHelper
    {





        //public static List<FormulaDTO> GetFormulaList(int processID)
        //{
        //    using (var projectContext = new MyProjectEntities())
        //    {
        //        var process = projectContext.Process.First(x => x.ID == processID);
        //        if (process.TableDrivedEntityID != null)
        //        {
        //            BizFormula bizFormula = new BizFormula();
        //            return bizFormula.GetFormulas(process.TableDrivedEntityID.Value);
        //        }
        //    }
        //    return null;
        //}


        //public static List<Transition> GetTransitions(int processID)
        //{

        //    using (var projectContext = new MyProjectEntities())
        //    {
        //        var transitions = projectContext.Transition.Include("TransitionAction.TransitionAction_Formula").Include("TransitionAction.TransitionAction_EntityGroup").Include("TransitionActivity").Where(x => x.ProcessID == processID).ToList();
        //        foreach (var transition in transitions)
        //        {
        //            if (transition.CurrentStateID == null)
        //                transition.CurrentStateID = -1;
        //            if (transition.NextStateID == null)
        //                transition.NextStateID = -99;
        //        }
        //        return transitions;
        //    }
        //}


        //public static List<State> GetProcessInitializeStates(int processID)
        //{
        //    //using (var projectContext = new MyProjectEntities())
        //    //{
        //    //    return projectContext.State.Where(x => x.ProcessID == processID && x.Transition1.Any(y => y.CurrentStateID == null)).ToList();
        //    //}
        //}
        //public static List<State> GetStates(int processID)
        //{
        //    using (var projectContext = new MyProjectEntities())
        //    {
        //        return projectContext.State.Include("StateActivity").Where(x => x.ProcessID == processID).ToList();
        //    }
        //}








        //public static int SaveEntityGroup(EntityGroup message)
        //{
        //    using (var projectContext = new MyProjectEntities())
        //    {
        //        EntityGroup dbEntityGroup = null;
        //        if (message.ID == 0)
        //        {
        //            dbEntityGroup = new EntityGroup();
        //            projectContext.EntityGroup.Add(dbEntityGroup);
        //        }
        //        else
        //            dbEntityGroup = projectContext.EntityGroup.First(x => x.ID == message.ID);
        //        dbEntityGroup.ProcessID = message.ProcessID;
        //        dbEntityGroup.Name = message.Name;

        //        List<EntityGroup_Relationship> removeEntityGroup_TableDrivedEntity = new List<EntityGroup_Relationship>();
        //        foreach (var dcEntityGroup_TableDrivedEntity in dbEntityGroup.EntityGroup_Relationship)
        //        {
        //            if (!message.EntityGroup_Relationship.Any(x => x.RelationshipID == dcEntityGroup_TableDrivedEntity.RelationshipID))
        //                removeEntityGroup_TableDrivedEntity.Add(dcEntityGroup_TableDrivedEntity);
        //        }
        //        foreach (var item in removeEntityGroup_TableDrivedEntity)
        //        {
        //            dbEntityGroup.EntityGroup_Relationship.Remove(item);
        //        }


        //        foreach (var msgEntityGroup_TableDrivedEntity in message.EntityGroup_Relationship)
        //        {
        //            EntityGroup_Relationship dbEntityGroup_TableDrivedEntity = dbEntityGroup.EntityGroup_Relationship.FirstOrDefault(x => x.RelationshipID == msgEntityGroup_TableDrivedEntity.RelationshipID);
        //            if (dbEntityGroup_TableDrivedEntity==null)
        //            {
        //                dbEntityGroup_TableDrivedEntity = new EntityGroup_Relationship();
        //                dbEntityGroup.EntityGroup_Relationship.Add(dbEntityGroup_TableDrivedEntity);
        //            }

        //            dbEntityGroup_TableDrivedEntity.RelationshipID = msgEntityGroup_TableDrivedEntity.RelationshipID;
        //        }

        //        projectContext.SaveChanges();
        //        return dbEntityGroup.ID;
        //    }

        //}







        //public static void SaveTransition(int ProcessID, List<Transition> list, string flowSTR)
        //{
        //    using (var projectContext = new MyProjectEntities())
        //    {

        //        var dbProcess = projectContext.Process.FirstOrDefault(x => x.ID == ProcessID);
        //        dbProcess.TransitionFlowSTR = flowSTR;
        //        var existingTransitions = list.Where(x => x.ID != 0).Select(x => x.ID).ToList();
        //        var listRemove = dbProcess.Transition.Where(x => !existingTransitions.Contains(x.ID));
        //        foreach (var item in listRemove)
        //        {
        //            item.TransitionAction.Clear();
        //            item.TransitionActivity.Clear();
        //            dbProcess.Transition.Remove(item);
        //        }
        //        foreach (var message in list)
        //        {
        //            Transition dbTransition = null;
        //            if (message.ID == 0)
        //            {
        //                dbTransition = new Transition();
        //                projectContext.Transition.Add(dbTransition);
        //            }
        //            else
        //                dbTransition = projectContext.Transition.FirstOrDefault(x => x.ID == message.ID);

        //            dbTransition.CurrentStateID = message.CurrentStateID;
        //            dbTransition.ProcessID = message.ProcessID;
        //            dbTransition.NextStateID = message.NextStateID;

        //            List<TransitionActivity> removeActivity = new List<TransitionActivity>();
        //            foreach (var dbActivity in dbTransition.TransitionActivity)
        //            {
        //                if (!message.TransitionActivity.Any(x => x.ActivityID == dbActivity.ActivityID))
        //                    removeActivity.Add(dbActivity);
        //            }
        //            foreach (var citem in removeActivity)
        //            {
        //                dbTransition.TransitionActivity.Remove(citem);
        //            }

        //            foreach (var msgActivity in message.TransitionActivity)
        //            {
        //                TransitionActivity dbTransitionActivity = null;
        //                if (!dbTransition.TransitionActivity.Any(x => x.ActivityID == msgActivity.ActivityID))
        //                {
        //                    dbTransitionActivity = new TransitionActivity();
        //                    dbTransition.TransitionActivity.Add(dbTransitionActivity);
        //                }
        //                else
        //                    dbTransitionActivity = dbTransition.TransitionActivity.FirstOrDefault(x => x.ActivityID == msgActivity.ActivityID);
        //                dbTransitionActivity.ActivityID = msgActivity.ActivityID;
        //            }


        //            List<TransitionAction> removeAction = new List<TransitionAction>();
        //            foreach (var dbAction in dbTransition.TransitionAction)
        //            {
        //                if (!message.TransitionAction.Any(x => x.ActionID == dbAction.ActionID))
        //                    removeAction.Add(dbAction);
        //            }
        //            foreach (var citem in removeAction)
        //            {
        //                dbTransition.TransitionAction.Remove(citem);
        //            }

        //            foreach (var msgTransitionAction in message.TransitionAction)
        //            {
        //                TransitionAction dbTransitionAction = null;
        //                if (!dbTransition.TransitionAction.Any(x => x.ActionID == msgTransitionAction.ActionID))
        //                {
        //                    dbTransitionAction = new TransitionAction();
        //                    dbTransition.TransitionAction.Add(dbTransitionAction);

        //                }
        //                else
        //                    dbTransitionAction = dbTransition.TransitionAction.FirstOrDefault(x => x.ActionID == msgTransitionAction.ActionID);
        //                dbTransitionAction.ActionID = msgTransitionAction.ActionID;


        //                List<TransitionAction_Formula> removeTransitionAction_Formula = new List<TransitionAction_Formula>();
        //                foreach (var dbTransitionAction_Formula in dbTransitionAction.TransitionAction_Formula)
        //                {
        //                    if (!msgTransitionAction.TransitionAction_Formula.Any(x => x.FormulaID == dbTransitionAction_Formula.FormulaID))
        //                        removeTransitionAction_Formula.Add(dbTransitionAction_Formula);
        //                }
        //                foreach (var citem in removeTransitionAction_Formula)
        //                {
        //                    dbTransitionAction.TransitionAction_Formula.Remove(citem);
        //                }



        //                foreach (var item in msgTransitionAction.TransitionAction_Formula)
        //                {
        //                    TransitionAction_Formula dbTransitionAction_Formula = dbTransitionAction.TransitionAction_Formula.FirstOrDefault(x => x.FormulaID == item.FormulaID);
        //                    if (dbTransitionAction_Formula == null)
        //                    {
        //                        dbTransitionAction_Formula = new TransitionAction_Formula();
        //                        dbTransitionAction.TransitionAction_Formula.Add(dbTransitionAction_Formula);

        //                    }
        //                    dbTransitionAction_Formula.FormulaID = item.FormulaID;
        //                }









        //                List<TransitionAction_EntityGroup> removeTransitionAction_EntityGroup = new List<TransitionAction_EntityGroup>();
        //                foreach (var dbTransitionAction_EntityGroup in dbTransitionAction.TransitionAction_EntityGroup)
        //                {
        //                    if (!msgTransitionAction.TransitionAction_EntityGroup.Any(x => x.EntityGroupID == dbTransitionAction_EntityGroup.EntityGroupID))
        //                        removeTransitionAction_EntityGroup.Add(dbTransitionAction_EntityGroup);
        //                }
        //                foreach (var citem in removeTransitionAction_EntityGroup)
        //                {
        //                    dbTransitionAction.TransitionAction_EntityGroup.Remove(citem);
        //                }

        //                foreach (var item in msgTransitionAction.TransitionAction_EntityGroup)
        //                {
        //                    TransitionAction_EntityGroup dbTransitionAction_EntityGroup = dbTransitionAction.TransitionAction_EntityGroup.FirstOrDefault(x => x.EntityGroupID == item.EntityGroupID);
        //                    if (dbTransitionAction_EntityGroup == null)
        //                    {
        //                        dbTransitionAction_EntityGroup = new TransitionAction_EntityGroup();
        //                        dbTransitionAction.TransitionAction_EntityGroup.Add(dbTransitionAction_EntityGroup);

        //                    }
        //                    dbTransitionAction_EntityGroup.EntityGroupID = item.EntityGroupID;

        //                }
        //            }

        //        }
        //        projectContext.SaveChanges();

        //    }
        //}




    }
}
