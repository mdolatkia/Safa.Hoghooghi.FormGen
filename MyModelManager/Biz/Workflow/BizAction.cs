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

namespace MyModelManager
{
    public class BizAction
    {
        public List<WFActionDTO> GetActions(int processID)
        {
            List<WFActionDTO> result = new List<WFActionDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var listAction = projectContext.Action.Where(x => x.ProcessID == processID);
                foreach (var item in listAction)
                    result.Add(ToActionDTO(item));
            }
            return result;
        }
        public WFActionDTO GetAction(int ActionsID)
        {

            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbAction = projectContext.Action.First(x => x.ID == ActionsID);
                return ToActionDTO(dbAction);
            }
        }
        public WFActionDTO ToActionDTO(DataAccess.Action item)
        {
            WFActionDTO result = new WFActionDTO();
            result.ID = item.ID;
            result.Name = item.Name;
            result.ActionType = (ActionType)item.ActionTypeID;
            result.ProcessID = item.ProcessID;
            result.Description = item.Description;

          
            return result;
        }

        public List<ActionType> GetActionTypes()
        {
            //List<ActionTypeDTO> result = new List<ActionTypeDTO>();
            //using (var projectContext = new DataAccess.MyProjectEntities())
            //{
            //    var list = projectContext.ActionType;
            //    foreach (var item in list)
            //    {
            //        var rItem = new ActionTypeDTO();
            //        rItem.ID = item.ID;
            //        rItem.Name = item.Name;
            //        result.Add(rItem);
            //    }
            //}
            //return result;
            return Enum.GetValues(typeof(ActionType)).Cast<ActionType>().ToList();
        }

        public int UpdateActions(WFActionDTO message)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbAction = projectContext.Action.FirstOrDefault(x => x.ID == message.ID);
                if (dbAction == null)
                    dbAction = new DataAccess.Action();
                dbAction.ID = message.ID;
                dbAction.Name = message.Name;
                dbAction.ActionTypeID = Convert.ToInt16(message.ActionType);
                dbAction.ProcessID = message.ProcessID;
                dbAction.Description = message.Description;
              
                if (dbAction.ID == 0)
                    projectContext.Action.Add(dbAction);
                projectContext.SaveChanges();
                return dbAction.ID;
            }
        }
    }
}
