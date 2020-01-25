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
    public class BizDataLink
    {
        SecurityHelper securityHelper = new SecurityHelper();
        BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();

        public DataLinkDTO GetDataLink(DR_Requester requester, int ID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbItem = projectContext.DataLinkDefinition.First(x => x.ID == ID);
                if (!DataIsAccessable(requester, dbItem))
                    return null;
                else
                    return ToDataLinkDTO(  dbItem, true);
            }
        }
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        public bool DataIsAccessable(DR_Requester requester, DataLinkDefinition dataLink)
        {
            if (requester.SkipSecurity)
                return true;
            if (!bizTableDrivedEntity.DataIsAccessable(requester, dataLink.TableDrivedEntity))
                return false;

            if (!bizTableDrivedEntity.DataIsAccessable(requester, dataLink.TableDrivedEntity1))
                return false;

            //اینجا تیل چک نمیشه
            foreach (var tail in dataLink.DataLinkDefinition_EntityRelationshipTail)
            {
                if (!bizEntityRelationshipTail.DataIsAccessable(requester, tail.EntityRelationshipTail))
                    return false;
            }
            return true;
        }

        public List<DataLinkDTO> GetDataLinkByEntitiyID(DR_Requester requester, int entitiyID)
        {
            List<DataLinkDTO> result = new List<DataLinkDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var items = projectContext.DataLinkDefinition.Where(x => x.FirstSideEntityID == entitiyID
                || x.SecondSideEntityID == entitiyID);
                foreach (var dbItem in items)
                {
                    if (DataIsAccessable(requester, dbItem))
                        result.Add(ToDataLinkDTO( dbItem, false));
                }
            }

            return result;
        }

        public List<DataLinkDTO> SearchDatalinks(DR_Requester requester, string singleFilterValue)
        {
            List<DataLinkDTO> result = new List<DataLinkDTO>();
            using (var projectContext = new MyProjectEntities())
            {
                var items = projectContext.DataLinkDefinition.Where(x => x.Name.Contains(singleFilterValue) || x.TableDrivedEntity.Alias.Contains(singleFilterValue)
                || x.TableDrivedEntity.Name.Contains(singleFilterValue) || x.TableDrivedEntity1.Alias.Contains(singleFilterValue) || x.TableDrivedEntity1.Name.Contains(singleFilterValue));
                foreach (var dbItem in items)
                {
                    if (DataIsAccessable(requester, dbItem))
                        result.Add(ToDataLinkDTO(  dbItem, false));
                }
            }
            return result;
        }



        private DataLinkDTO ToDataLinkDTO( DataLinkDefinition item, bool withDetails)
        {
            DataLinkDTO result = new DataLinkDTO();
            result.ID = item.ID;
            result.Name = item.Name;
            result.FirstSideEntityID = item.FirstSideEntityID;
            result.SecondSideEntityID = item.SecondSideEntityID;
            if (withDetails)
            {
                BizEntityRelationshipTail bizEntityRelationshipTail = new MyModelManager.BizEntityRelationshipTail();
                foreach (var dbRel in item.DataLinkDefinition_EntityRelationshipTail)
                {
                    var rel = new DataLinkRelationshipTailDTO();
                    rel.RelationshipTailID = dbRel.EntityRelationshipTailID;
                    //   rel.FromFirstSideToSecondSide = dbRel.FromFirstSideToSecondSide;
                    rel.ID = dbRel.ID;
                    rel.RelationshipTail = bizEntityRelationshipTail.ToEntityRelationshipTailDTO( dbRel.EntityRelationshipTail);
                    result.RelationshipsTails.Add(rel);
                }
            }



            return result;
        }
        public int UpdateDataLink(DataLinkDTO message)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                DataLinkDefinition dbEntity;
                if (message.ID == 0)
                    dbEntity = new DataLinkDefinition();
                else
                    dbEntity = projectContext.DataLinkDefinition.FirstOrDefault(x => x.ID == message.ID);
                dbEntity.Name = message.Name;
                dbEntity.FirstSideEntityID = message.FirstSideEntityID;
                dbEntity.SecondSideEntityID = message.SecondSideEntityID;
                while (dbEntity.DataLinkDefinition_EntityRelationshipTail.Any())
                    projectContext.DataLinkDefinition_EntityRelationshipTail.Remove(dbEntity.DataLinkDefinition_EntityRelationshipTail.First());
                foreach (var item in message.RelationshipsTails)
                {
                    var dbRel = new DataLinkDefinition_EntityRelationshipTail();
                    dbRel.EntityRelationshipTailID = item.RelationshipTailID;
                    //   dbRel.FromFirstSideToSecondSide = item.FromFirstSideToSecondSide;
                    dbEntity.DataLinkDefinition_EntityRelationshipTail.Add(dbRel);
                }

                if (dbEntity.ID == 0)
                    projectContext.DataLinkDefinition.Add(dbEntity);
                projectContext.SaveChanges();
                return dbEntity.ID;
            }
        }
    }

}
