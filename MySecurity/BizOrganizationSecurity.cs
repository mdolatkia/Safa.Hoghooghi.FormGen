using DataAccess;
using ModelEntites;
using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySecurity
{
    public class BizOrganizationSecurity
    {
        public List<EntityOrganizationSecurityDirectDTO> GetEntityOrganizationSecurityDirects()
        {
            List<EntityOrganizationSecurityDirectDTO> result = new List<EntityOrganizationSecurityDirectDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var list = projectContext.EntityOrganizationSecurityDirect;
                foreach (var item in list)
                    result.Add(ToEntityOrganizationSecurityDirectDTO(item));

                return result;
            }
        }
        public EntityOrganizationSecurityDirectDTO GetEntityOrganizationSecurityDirect(int entityID, bool withDetails)
        {
            EntityOrganizationSecurityDirectDTO result = new EntityOrganizationSecurityDirectDTO();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var item = projectContext.EntityOrganizationSecurityDirect.FirstOrDefault(x => x.TableDrivedEntityID == entityID);
                if (item != null)
                    return ToEntityOrganizationSecurityDirectDTO(item);
                else
                    return null;

            }
        }

        private EntityOrganizationSecurityDirectDTO ToEntityOrganizationSecurityDirectDTO(EntityOrganizationSecurityDirect item)
        {
            EntityOrganizationSecurityDirectDTO result = new EntityOrganizationSecurityDirectDTO();
            result.ID = item.ID;
            result.ColumnID = item.ColumnID;
            result.TableDrivedEntityID = item.TableDrivedEntityID;
            result.EntityName = item.TableDrivedEntity.Name;
            if (item.DatabaseFunctionID != null)
                result.DBFunctionID = item.DatabaseFunctionID.Value;
            if (item.Operator != null)
                result.Operator = (EntitySecurityOperator)item.Operator;
            return result;
        }

        public EntityOrganizationSecurityInDirectDTO GetEntityOrganizationSecurityInDirect(int entityID, bool withDetails)
        {
            EntityOrganizationSecurityInDirectDTO result = new EntityOrganizationSecurityInDirectDTO();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var item = projectContext.EntityOrganizationSecurityInDirect.FirstOrDefault(x => x.TableDrivedEntityID == entityID);
                if (item != null)
                    return ToEntityOrganizationSecurityInDirectDTO(item, withDetails);
                else
                    return null;

            }
        }

        private EntityOrganizationSecurityInDirectDTO ToEntityOrganizationSecurityInDirectDTO(EntityOrganizationSecurityInDirect item, bool withDetails)
        {
            EntityOrganizationSecurityInDirectDTO result = new EntityOrganizationSecurityInDirectDTO();
            result.ID = item.ID;
            result.DirectOrganizationSecurityID = item.EntityOrganizationSecurityDirectID;
            result.RelationshipTailID = item.EntityRelationshipTailID;
            result.TableDrivedEntityID = item.TableDrivedEntityID;
            if (withDetails)
            {
                BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
                result.RelationshipTail = bizEntityRelationshipTail.ToEntityRelationshipTailDTO(item.EntityRelationshipTail);
                result.DirectOrganizationSecurity = ToEntityOrganizationSecurityDirectDTO(item.EntityOrganizationSecurityDirect);
            }
            return result;
        }

        public void UpdateEntityOrganizationSecurityDirect(EntityOrganizationSecurityDirectDTO message)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbItem = projectContext.EntityOrganizationSecurityDirect.FirstOrDefault(x => x.ID == message.ID);
                if (dbItem == null)
                {
                    dbItem = new DataAccess.EntityOrganizationSecurityDirect();
                    projectContext.EntityOrganizationSecurityDirect.Add(dbItem);
                }
                dbItem.TableDrivedEntityID = message.TableDrivedEntityID;

                dbItem.ColumnID = message.ColumnID;
                if (message.DBFunctionID != 0)
                {
                    dbItem.DatabaseFunctionID = message.DBFunctionID;
                    dbItem.Operator = (short)message.Operator;
                }
                else
                {
                    dbItem.DatabaseFunctionID = null;
                    dbItem.Operator = null;
                }
                projectContext.SaveChanges();
            }
        }
        public void UpdateEntityOrganizationSecurityInDirect(EntityOrganizationSecurityInDirectDTO message)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbItem = projectContext.EntityOrganizationSecurityInDirect.FirstOrDefault(x => x.ID == message.ID);
                if (dbItem == null)
                {
                    dbItem = new DataAccess.EntityOrganizationSecurityInDirect();
                    projectContext.EntityOrganizationSecurityInDirect.Add(dbItem);
                }
                dbItem.TableDrivedEntityID = message.TableDrivedEntityID;

                dbItem.EntityOrganizationSecurityDirectID = message.DirectOrganizationSecurityID;
                dbItem.EntityRelationshipTailID = message.RelationshipTailID;
                projectContext.SaveChanges();
            }
        }
    }
}