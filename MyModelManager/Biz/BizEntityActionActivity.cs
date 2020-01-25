using DataAccess;
using ModelEntites;
using MyGeneralLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizEntityActionActivity
    {
        BizActionActivity bizActionActivity = new BizActionActivity();
        public List<EntityActionActivityDTO> GetEntityActionActivities(int entityID, Enum_ActionActivityType type, bool withDetails)
        {
            List<EntityActionActivityDTO> result = new List<EntityActionActivityDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var listEntityActionActivity = projectContext.ActionActivity_TableDrivedEntity.Where(x => x.TableDrivedEntityID == entityID);
                if (type == Enum_ActionActivityType.CodeFunctionOrFormulaFunction)
                    listEntityActionActivity = listEntityActionActivity.Where(x => x.ActionActivity.CodeFunctionID != null || x.ActionActivity.FormulaFunctionID != null);
                else if (type == Enum_ActionActivityType.UIActions)
                    listEntityActionActivity = listEntityActionActivity.Where(x => x.ActionActivity.RelationshipEnablityID != null || x.ActionActivity.ColumnValueID != null);

                foreach (var item in listEntityActionActivity)
                    result.Add(ToEntityActionActivityDTO(item, withDetails));
            }
            return result;
        }
        //public EntityActionActivityDTO GetEntityActionActivity(int EntityActionActivitysID)
        //{
        //    List<EntityActionActivityDTO> result = new List<EntityActionActivityDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var EntityActionActivitys = projectContext.ActionActivity_TableDrivedEntity.First(x => x.ID == EntityActionActivitysID);
        //        return ToEntityActionActivityDTO(EntityActionActivitys);
        //    }
        //}
        private EntityActionActivityDTO ToEntityActionActivityDTO(ActionActivity_TableDrivedEntity item, bool withDetails)
        {
            EntityActionActivityDTO result = new EntityActionActivityDTO();
            result.Step = (Enum_EntityActionActivityStep)item.StepType;
            result.ActionActivityID = item.ActionActivityID;
            if (withDetails)
                result.ActionActivity = bizActionActivity.ToActionActivityDTO(item.ActionActivity, withDetails);
            result.EntityID = item.TableDrivedEntityID;
            result.ID = item.ID;
            result.ResultSensetive = item.ResultSensetive;

            return result;
        }
        public void UpdateEntityActionActivities(int entityId, List<EntityActionActivityDTO> entityActionActivities)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {

                var dbEntityActionActivities = projectContext.ActionActivity_TableDrivedEntity.Where(x => x.TableDrivedEntityID == entityId);
                var sentIds = entityActionActivities.Select(y => y.ID).ToList();
                var removeList = dbEntityActionActivities.Where(x => !sentIds.Contains(x.ID)).ToList();
                foreach (var item in removeList)
                    projectContext.ActionActivity_TableDrivedEntity.Remove(item);
                foreach (var entityActionActivity in entityActionActivities)
                {
                    var dbEntityActionActivity = projectContext.ActionActivity_TableDrivedEntity.FirstOrDefault(x => x.ID == entityActionActivity.ID);
                    if (dbEntityActionActivity == null)
                        dbEntityActionActivity = new DataAccess.ActionActivity_TableDrivedEntity();
                    dbEntityActionActivity.StepType = (short)entityActionActivity.Step;
                    dbEntityActionActivity.ActionActivityID = entityActionActivity.ActionActivityID;
                    dbEntityActionActivity.TableDrivedEntityID = entityId;
                    dbEntityActionActivity.ID = entityActionActivity.ID;
                    dbEntityActionActivity.ResultSensetive = entityActionActivity.ResultSensetive;
                    if (dbEntityActionActivity.ID == 0)
                        projectContext.ActionActivity_TableDrivedEntity.Add(dbEntityActionActivity);
                }
                projectContext.SaveChanges();
            }
        }
    }
    public enum Enum_ActionActivityType
    {
        CodeFunctionOrFormulaFunction,
        UIActions,
        All
    }
}
