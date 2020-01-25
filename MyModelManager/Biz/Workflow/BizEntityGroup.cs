using DataAccess;
using ModelEntites;
using ProxyLibrary;
using ProxyLibrary.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizEntityGroup
    {
        public List<EntityGroupDTO> GetEntityGroups(DR_Requester requester, int processID)
        {
            List<EntityGroupDTO> result = new List<EntityGroupDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var listEntityGroup = projectContext.EntityGroup.Where(x => x.ProcessID == processID);
                foreach (var item in listEntityGroup)
                {
                    if (DataIsAccessable(requester, item))
                        result.Add(ToEntityGroupDTO(requester,item,false));
                }
            }
            return result;
        }
        //public EntityGroupDTO GetEntityGroup(DR_Requester requester, int id, bool withDetails)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var entityGroup = projectContext.EntityGroup.First(x => x.ID == id);
        //        return GetEntityGroup(requester, entityGroup, withDetails);
        //    }
        //}
        //public EntityGroupDTO GetEntityGroup(DR_Requester requester, EntityGroup entityGroup, bool withDetails)
        //{
        //    if (DataIsAccessable(requester, entityGroup))
        //        return ToEntityGroupDTO(requester, entityGroup, withDetails);
        //    else
        //        return null;
        //}
        private bool DataIsAccessable(DR_Requester requester, EntityGroup entityGroup)
        {
            return true;
        }

        BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        public EntityGroupDTO ToEntityGroupDTO(DR_Requester requester, EntityGroup item, bool withDetails)
        {
            EntityGroupDTO result = new EntityGroupDTO();
            result.ID = item.ID;
            result.Name = item.Name;
            if (withDetails)
            {
                foreach (var ritem in item.EntityGroup_Relationship)
                {
                    if (DataIsAccessable(requester, ritem))
                    {
                        EntityGroupRelationshipDTO egr = ToEntityGroupRelationshipDTO(ritem, withDetails);
                        result.Relationships.Add(egr);
                    }
                }
            }
            return result;
        }

        private bool DataIsAccessable(DR_Requester requester, EntityGroup_Relationship ritem)
        {
            bool isvalid = true;
            if (ritem.EntityRelationshipTailID != null)
            {
                if (!bizEntityRelationshipTail.DataIsAccessable(requester, ritem.EntityRelationshipTail))
                    isvalid = false;
            }
            else if (ritem.EntityGroup.Process.TableDrivedEntity != null)
            {
                if (!bizTableDrivedEntity.DataIsAccessable(requester, ritem.EntityGroup.Process.TableDrivedEntity))
                    isvalid = false;
            }
            return isvalid;
        }

        //public List<EntityGroupRelationshipDTO> GetEntityGroupRelationships(int entityGroupID)
        //{
        //    List<EntityGroupRelationshipDTO> result = new List<EntityGroupRelationshipDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var list = projectContext.EntityGroup_Relationship.Where(x => x.EntityGroupID == entityGroupID);
        //        foreach (var ritem in list)
        //        {
        //            EntityGroupRelationshipDTO egr = ToEntityGroupRelationshipDTO(ritem);
        //            result.Add(egr);
        //        }
        //    }
        //    return result;
        //}
        private EntityGroupRelationshipDTO ToEntityGroupRelationshipDTO(EntityGroup_Relationship ritem, bool withDetails)
        {
            var egr = new EntityGroupRelationshipDTO();
            egr.ID = ritem.ID;
            egr.RelationshipTailID = ritem.EntityRelationshipTailID ?? 0;
            if (withDetails)
            {
                if (ritem.EntityRelationshipTail != null)
                {
                    BizEntityRelationshipTail bizEntityRelationshipTail = new MyModelManager.BizEntityRelationshipTail();
                    egr.RelationshipTail = bizEntityRelationshipTail.ToEntityRelationshipTailDTO(ritem.EntityRelationshipTail);
                    egr.vwName = egr.RelationshipTail.TargetEntityAlias;
                }
                else
                {
                    egr.vwName = ritem.EntityGroup.Process.TableDrivedEntity.Alias;
                }
            }
            //if (ritem.EntityRelationshipTail != null)
            //    egr.Name = ritem.EntityRelationshipTail.RelationshipPath;
            //else
            //    egr.Name = "موجودیت اصلی";
            return egr;
        }

        //public int UpdateEntityGroups(EntityGroupDTO message)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var dbEntityGroup = projectContext.EntityGroup.FirstOrDefault(x => x.ID == message.ID);
        //        if (dbEntityGroup == null)
        //            dbEntityGroup = new DataAccess.EntityGroup();
        //        dbEntityGroup.ID = message.ID;
        //        dbEntityGroup.ProcessID = message.ProcessID;
        //        dbEntityGroup.Name = message.Name;
        //        while (dbEntityGroup.EntityGroup_Relationship.Any())
        //            projectContext.EntityGroup_Relationship.Remove(dbEntityGroup.EntityGroup_Relationship.First());
        //        foreach (var msg in message.Relationships)
        //        {
        //            var db = new EntityGroup_Relationship();
        //            if (msg.RelationshipTailID != 0)
        //                db.EntityRelationshipTailID = msg.RelationshipTailID;
        //            else
        //                db.EntityRelationshipTailID = null;
        //            dbEntityGroup.EntityGroup_Relationship.Add(db);
        //        }
        //        if (dbEntityGroup.ID == 0)
        //            projectContext.EntityGroup.Add(dbEntityGroup);
        //        projectContext.SaveChanges();
        //        return dbEntityGroup.ID;
        //    }
        //}
    }
}
