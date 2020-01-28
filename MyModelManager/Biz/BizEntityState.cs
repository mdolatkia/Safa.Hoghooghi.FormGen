using DataAccess;
using ModelEntites;
using MyGeneralLibrary;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizEntityState
    {
        public List<EntityStateDTO> GetEntityStates(int entityID, bool withDetails)
        {
            List<EntityStateDTO> result = new List<EntityStateDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var listEntityState = projectContext.TableDrivedEntityState.Where(x => x.TableDrivedEntityID == entityID);
                foreach (var item in listEntityState)
                    result.Add(ToEntityStateDTO(item, withDetails));

            }
            return result;
        }
        //public List<EntityStateDTO> GetPreservableEntityStates(int entityID, bool withDetails)
        //{
        //    List<EntityStateDTO> result = new List<EntityStateDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var listEntityState = projectContext.TableDrivedEntityState.Where(x => x.TableDrivedEntityID == entityID && x.Preserve == true);
        //        foreach (var item in listEntityState)
        //            result.Add(ToEntityStateDTO(item, withDetails));

        //    }
        //    return result;
        //}

        //public List<EntityStateDTO> GetEntityStatesWithServerSideActionActivities(int entityID, bool withDetails)
        //{
        //    List<EntityStateDTO> result = new List<EntityStateDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var listEntityState = projectContext.TableDrivedEntityState.Where(x => x.TableDrivedEntityID == entityID);
        //        listEntityState = listEntityState.Where(x => x.EntityState_ActionActivity.Any(y => y.ActionActivity.Type == 0 || y.ActionActivity.Type == 1) || x.Preserve == true);
        //        foreach (var item in listEntityState)
        //            foreach (var actionActivity in item.EntityState_ActionActivity.Where(y => y.ActionActivity.Type != 0 && y.ActionActivity.Type != 1).ToList())
        //                item.EntityState_ActionActivity.Remove(actionActivity);
        //        foreach (var item in listEntityState)
        //            result.Add(ToEntityStateDTO(item, withDetails));

        //    }
        //    return result;
        //}
        public EntityStateDTO GetEntityState(int entityStatesID, bool withDetails)
        {
            List<EntityStateDTO> result = new List<EntityStateDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var EntityStates = projectContext.TableDrivedEntityState.First(x => x.ID == entityStatesID);
                return ToEntityStateDTO(EntityStates, withDetails);
            }
        }
        //public bool EntityHasState(DP_DataRepository dataItem, int stateID)
        //{
        //    if (dataItem != null)
        //        using (var projectContext = new DataAccess.MyProjectEntities())
        //        {
        //            var foundDataItem = GetDataItem(projectContext, dataItem);
        //            if (foundDataItem != null)
        //            {
        //                return foundDataItem.DataItem_EntityState.Any(x => x.TableDrivedEntityStateID == stateID);
        //            }
        //        }
        //    return false;
        //}
        //public bool EntityHasState(DP_DataRepository dataItem, string state)
        //{

        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var foundDataItem = GetDataItem(projectContext, dataItem);
        //        if (foundDataItem != null)
        //        {
        //            return foundDataItem.DataItem_EntityState.Any(x => x.TableDrivedEntityState.Title == state);
        //        }
        //    }
        //    return false;
        //}

        //private DataItem GetDataItem(DataAccess.MyProjectEntities projectContext, DP_DataRepository dataItem)
        //{
        //    if (dataItem != null)
        //    {
        //        var keyColumns = dataItem.KeyProperties;
        //        if (keyColumns.Any())
        //        {
        //            var dataItems = projectContext.DataItem.Where(x => x.TableDrivedEntityID == dataItem.TargetEntityID);
        //            foreach (var keyColumn in keyColumns)
        //                dataItems = dataItems.Where(x => x.DataItemKeyColumns.Any(y => y.ColumnID == keyColumn.ColumnID && y.Value == keyColumn.Value));
        //            return dataItems.FirstOrDefault();
        //        }
        //    }
        //    return null;
        //}

        private EntityStateDTO ToEntityStateDTO(TableDrivedEntityState item, bool withDetails)
        {
            EntityStateDTO result = new EntityStateDTO();
            result.FormulaID = item.FormulaID ?? 0;
            if (result.FormulaID != 0 && withDetails)
            {  //??با جزئیات؟؟........................................................................ 
                var bizFormula = new BizFormula();
                result.Formula = bizFormula.GetFormula(item.FormulaID.Value, withDetails);
            }
            result.ColumnID = item.ColumnID ?? 0;
            result.RelationshipIDTailID = item.EntityRelationshipTailID ?? 0;
            if (item.EntityRelationshipTail != null)
                result.RelationshipIDTail = item.EntityRelationshipTail.RelationshipPath;
            result.TableDrivedEntityID = item.TableDrivedEntityID;
            //result.Preserve = item.Preserve;
            //result.ActionActivityID = item.ActionActivityID ?? 0;
            //if (result.ActionActivityID != 0 && withDetails)
            //{
            //    var bizActionActivity = new BizActionActivity();
            //    result.ActionActivity = bizActionActivity.GetActionActivity(item.ActionActivityID.Value);
            //}
            var bizActionActivity = new BizUIActionActivity();
            foreach (var actionActivity in item.EntityState_UIActionActivity)
            {
                result.ActionActivities.Add(bizActionActivity.ToActionActivityDTO(actionActivity.UIActionActivity, withDetails));
            }
            result.ID = item.ID;
            result.Title = item.Title;
            if (item.EntityStateOperator != null)
                result.EntityStateOperator = (Enum_EntityStateOperator)item.EntityStateOperator;
            foreach (var valItem in item.TableDrivedEntityStateValues)
            {
                result.Values.Add(new ModelEntites.EntityStateValueDTO() { Value = valItem.Value });
            }
            return result;
        }

        //public void ClearAndSaveStates(DataItemState item)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var foundDataItem = GetDataItem(projectContext, item.DataItem);
        //        if (foundDataItem != null)
        //        {
        //            foundDataItem = new DataItem() { TableDrivedEntityID = item.DataItem.TargetEntityID };
        //            var keyColumns = item.DataItem.KeyProperties;
        //            foreach (var keyColumn in keyColumns)
        //                foundDataItem.DataItemKeyColumns.Add(new DataItemKeyColumns() { ColumnID = keyColumn.ColumnID, Value = keyColumn.Value });
        //            projectContext.DataItem.Add(foundDataItem);
        //        }
        //        while (foundDataItem.DataItem_EntityState.Any())
        //            projectContext.DataItem_EntityState.Remove(foundDataItem.DataItem_EntityState.First());
        //        foreach (var state in item.States.Where(x => x.Preserve))
        //            foundDataItem.DataItem_EntityState.Add(new DataItem_EntityState() { TableDrivedEntityStateID = state.ID });
        //        projectContext.SaveChanges();
        //    }
        //}

        public void UpdateEntityStates(EntityStateDTO EntityState)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbEntityState = projectContext.TableDrivedEntityState.FirstOrDefault(x => x.ID == EntityState.ID);
                if (dbEntityState == null)
                    dbEntityState = new DataAccess.TableDrivedEntityState();
                if (EntityState.FormulaID != 0)
                    dbEntityState.FormulaID = EntityState.FormulaID;
                else
                    dbEntityState.FormulaID = null;
                if (EntityState.ColumnID != 0)
                {
                    dbEntityState.ColumnID = EntityState.ColumnID;
                    if (EntityState.RelationshipIDTailID == 0)
                        dbEntityState.EntityRelationshipTailID = null;
                    else
                        dbEntityState.EntityRelationshipTailID = EntityState.RelationshipIDTailID;
                }
                else
                {
                    dbEntityState.ColumnID = null;
                    dbEntityState.EntityRelationshipTailID = null;
                }
                dbEntityState.EntityStateOperator = (short)EntityState.EntityStateOperator;
                //if (EntityState.ActionActivityID != 0)
                //    dbEntityState.ActionActivityID = EntityState.ActionActivityID;
                //else
                //    dbEntityState.ActionActivityID = null;
                dbEntityState.TableDrivedEntityID = EntityState.TableDrivedEntityID;
                dbEntityState.ID = EntityState.ID;
                //   dbEntityState.Preserve = EntityState.Preserve;
                dbEntityState.Title = EntityState.Title;
                while (dbEntityState.TableDrivedEntityStateValues.Any())
                    projectContext.TableDrivedEntityStateValues.Remove(dbEntityState.TableDrivedEntityStateValues.First());
                foreach (var nItem in EntityState.Values)
                {
                    dbEntityState.TableDrivedEntityStateValues.Add(new TableDrivedEntityStateValues() { Value = nItem.Value });
                }

                while (dbEntityState.EntityState_UIActionActivity.Any())
                {
                    //dbEntityState.EntityState_ActionActivity.Remove(dbEntityState.EntityState_ActionActivity.First());
                    projectContext.EntityState_UIActionActivity.Remove(dbEntityState.EntityState_UIActionActivity.First());
                }
                foreach (var actionActivity in EntityState.ActionActivities)
                {
                    dbEntityState.EntityState_UIActionActivity.Add(new EntityState_UIActionActivity() { UIActionActivityID = actionActivity.ID });
                }

                if (dbEntityState.ID == 0)
                    projectContext.TableDrivedEntityState.Add(dbEntityState);
                projectContext.SaveChanges();
            }
        }
    }
    public class DataItemState
    {
        public DataItemState()
        {
            States = new List<EntityStateDTO>();
        }
        public DP_DataRepository DataItem { set; get; }
        public List<EntityStateDTO> States { set; get; }

    }
}
