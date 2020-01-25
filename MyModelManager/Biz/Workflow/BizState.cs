using DataAccess;
using ModelEntites;
using ProxyLibrary.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace MyModelManager
{
    public class BizState
    {
        public List<WFStateDTO> GetStates(int processID, bool withDetails)
        {
            List<WFStateDTO> result = new List<WFStateDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var listState = projectContext.State.Where(x => x.ProcessID == processID);
                foreach (var item in listState)
                    result.Add(ToStateDTO(item, withDetails));
            }
            return result;
        }
        public WFStateDTO GetState(int StatesID, bool withDetails)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var States = projectContext.State.First(x => x.ID == StatesID);
                return ToStateDTO(States, withDetails);
            }
        }
        public WFStateDTO ToStateDTO(State item, bool withDetails)
        {
            WFStateDTO result = new WFStateDTO();
            result.ID = item.ID;
            result.Name = item.Name;
            result.Description = item.Description;
            result.StateType = (StateType)item.StateTypeID;
            result.ProcessID = item.ProcessID;
            if (withDetails)
            {
                foreach (var sbsf in item.State_Formula)
                {
                    var sf = new StateFormulaDTO();
                    sf.FormulaID = sbsf.FormulaID;
                    sf.Message = sbsf.Message;
                    result.Formulas.Add(sf);
                }
            }
            if (withDetails)
            {
                BizActivity bizActivity = new BizActivity();
                foreach (var act in item.StateActivity)
                {
                    result.Activities.Add(bizActivity.ToActivityDTO(act.Activity, true));
                }

            }
            return result;
        }
        public int UpdateStates(WFStateDTO message)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbState = projectContext.State.FirstOrDefault(x => x.ID == message.ID);
                if (dbState == null)
                    dbState = new DataAccess.State();
                dbState.ID = message.ID;
                dbState.Name = message.Name;
                dbState.StateTypeID = Convert.ToInt16(message.StateType);
                dbState.ProcessID = message.ProcessID;
                dbState.Description = message.Description;
                if (dbState.ID == 0)
                    projectContext.State.Add(dbState);

                while (dbState.StateActivity.Any())
                    projectContext.StateActivity.Remove(dbState.StateActivity.First());
                foreach (var msgActionTarget in message.Activities)
                {
                    var dbStateActivity = new StateActivity();
                    dbStateActivity.ActivityID = msgActionTarget.ID;
                    dbState.StateActivity.Add(dbStateActivity);
                }
                while (dbState.State_Formula.Any())
                    projectContext.State_Formula.Remove(dbState.State_Formula.First());
                foreach (var msgActionTarget in message.Formulas)
                {
                    var dbState_Formula = new State_Formula();
                    dbState_Formula.FormulaID = msgActionTarget.FormulaID;
                    dbState_Formula.Message  = msgActionTarget.Message;
                    dbState.State_Formula.Add(dbState_Formula);
                }

                projectContext.SaveChanges();
                return dbState.ID;
            }
        }

        public List<StateType> GetStateTypes()
        {
            return Enum.GetValues(typeof(StateType)).Cast<StateType>().ToList();
        }

        public List<WFStateDTO> GetProcessInitializeStates(int processID)
        {
            List<WFStateDTO> result = new List<WFStateDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var startType = Convert.ToInt16(StateType.Start);
                var listState = projectContext.State.Where(x => x.ProcessID == processID && x.StateTypeID == startType);
                foreach (var item in listState)
                    result.Add(ToStateDTO(item, false));
            }
            return result;
        }
    }
}
