using DataAccess;
using ModelEntites;
using MyCacheManager;
using MyGeneralLibrary;

using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizEntityCommand
    {
        public List<EntityCommandDTO> GetEntityCommands(DR_Requester requester, int entityID, bool withDetails)
        {
            //var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.Command, entityID.ToString());
            //if (cachedItem != null)
            //    return (cachedItem as List<EntityCommandDTO>);

            List<EntityCommandDTO> result = new List<EntityCommandDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                IQueryable<EntityCommand> listEntityCommand;
                if (entityID != 0)
                    listEntityCommand = projectContext.EntityCommand.Where(x => x.TableDrivedEntity_EntityCommand.Any(y => y.TableDrivedEntityID == entityID));
                else
                    listEntityCommand = projectContext.EntityCommand;
                foreach (var item in listEntityCommand)
                {
                    if (DataIsAccessable(requester, item))
                        result.Add(ToEntityCommandDTO(item, withDetails));
                }

            }
            //CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.Command, entityID.ToString());
            return result;
        }
        SecurityHelper securityHelper = new SecurityHelper();
        public bool DataIsAccessable(DR_Requester requester, EntityCommand entityCommand)
        {

            var commandPermission = securityHelper.GetAssignedPermissions(requester, entityCommand.ID, false);
            if (commandPermission.GrantedActions.Any(x => x == SecurityAction.NoAccess))
            {
                return false;
            }
            return true;
        }
        public EntityCommandDTO GetEntityCommand(int EntityCommandsID, bool withDetails)
        {
            List<EntityCommandDTO> result = new List<EntityCommandDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var EntityCommands = projectContext.EntityCommand.First(x => x.ID == EntityCommandsID);
                return ToEntityCommandDTO(EntityCommands, withDetails);
            }
        }
        private EntityCommandDTO ToEntityCommandDTO(EntityCommand item, bool withDetails)
        {
            EntityCommandDTO result = new EntityCommandDTO();
            //result.FormulaID = item.FormulaID ?? 0;
            //if (result.FormulaID != 0 && withDetails)
            //{
            //    var bizFormula = new BizFormula();
            //    result.Formula = bizFormula.GetFormula(item.FormulaID.Value);
            //}
            result.CodeFunctionID = item.CodeFunctionID;
            if (result.CodeFunctionID != 0 && withDetails)
            {
                var bizCodeFunction = new BizCodeFunction();
                result.CodeFunction = bizCodeFunction.GetCodeFunction(item.CodeFunctionID);
            }
            //  result.TableDrivedEntityID = item.TableDrivedEntityID;
            foreach (var citem in item.TableDrivedEntity_EntityCommand)
            {
                result.Entities.Add(new EntityCommandEntityDTO() { ID = citem.ID, EntityID = citem.TableDrivedEntityID });
            }
            result.ID = item.ID;
            //result.Message = item.Message;
            result.Title = item.Title;
            result.Type = (EntityCommandType)item.Type;
            //result.Value = item.Value;
            return result;
        }
        public void UpdateEntityCommands(EntityCommandDTO EntityCommand)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbEntityCommand = projectContext.EntityCommand.FirstOrDefault(x => x.ID == EntityCommand.ID);
                if (dbEntityCommand == null)
                {
                    dbEntityCommand = new DataAccess.EntityCommand();
                    dbEntityCommand.SecurityObject = new SecurityObject();
                    dbEntityCommand.SecurityObject.Type = (int)DatabaseObjectCategory.Command;
                }
                //if (EntityCommand.FormulaID != 0)
                //    dbEntityCommand.FormulaID = EntityCommand.FormulaID;
                //else
                //    dbEntityCommand.FormulaID = null;
                //if (EntityCommand.CodeFunctionID != 0)
                //{
                dbEntityCommand.CodeFunctionID = EntityCommand.CodeFunctionID;

                //}
                //else
                //    dbEntityCommand.CodeFunctionID = null;
                //    dbEntityCommand.TableDrivedEntityID = EntityCommand.TableDrivedEntityID;
                dbEntityCommand.ID = EntityCommand.ID;
                //dbEntityCommand.Message = EntityCommand.Message;
                dbEntityCommand.Title = EntityCommand.Title;
                dbEntityCommand.Type = (short)EntityCommand.Type;

                var entityIds = EntityCommand.Entities.Select(x => x.EntityID).ToList();
                var removeList = dbEntityCommand.TableDrivedEntity_EntityCommand.Where(x => !entityIds.Contains(x.TableDrivedEntityID));
                foreach (var item in removeList)
                {
                    projectContext.TableDrivedEntity_EntityCommand.Remove(item);
                }
                foreach (var item in entityIds)
                {
                    if (!dbEntityCommand.TableDrivedEntity_EntityCommand.Any(x => x.TableDrivedEntityID == item))
                        dbEntityCommand.TableDrivedEntity_EntityCommand.Add(new TableDrivedEntity_EntityCommand() { TableDrivedEntityID = item });
                }
                //dbEntityCommand.Value = EntityCommand.Value;
                if (dbEntityCommand.ID == 0)
                    projectContext.EntityCommand.Add(dbEntityCommand);
                projectContext.SaveChanges();
            }
        }
    }

}
