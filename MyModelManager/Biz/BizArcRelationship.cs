using DataAccess;
using ModelEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    //public class BizArcRelationship
    //{
    //    public List<ArcRelationshipGroupDTO> GetArcRelationshipGroups(int tableDrivedEntityID)
    //    {
    //        List<ArcRelationshipGroupDTO> result = new List<ArcRelationshipGroupDTO>();
    //        using (var projectContext = new DataAccess.MyProjectEntities())
    //        {
    //            var list = projectContext.ArcRelationshipGroup.Where(x => x.TableDrivedEntityID == tableDrivedEntityID);
    //            foreach (var item in list)
    //            {
    //                result.Add(ToArcRelationshipGroupDTO(item));
    //            }
    //        }
    //        return result;
    //    }

    //    private ArcRelationshipGroupDTO ToArcRelationshipGroupDTO(DataAccess.ArcRelationshipGroup item)
    //    {
    //        ArcRelationshipGroupDTO result = new ArcRelationshipGroupDTO();
    //        result.GroupName = item.GroupName;
    //        result.TableDrivedEntityID = item.TableDrivedEntityID;
    //        result.ID = item.ID;
    //        result.Relationships = new List<ArcRelationshipGroup_RelationshipDTO>();
    //        foreach (var relation in item.ArcRelationshipGroup_Relationship)
    //        {
    //            result.Relationships.Add(ToArcRelationshipGroup_RelationshipDTO(relation));
    //        }
    //        return result;
    //    }

    //    //public List<ArcRelationshipGroup_RelationshipDTO> GetArcRelationshipGroup_RelationshipDTO(int arcGroupID)
    //    //{
    //    //    List<ArcRelationshipGroup_RelationshipDTO> result = new List<ArcRelationshipGroup_RelationshipDTO>();
    //    //    using (var projectContext = new DataAccess.MyProjectEntities())
    //    //    {
    //    //        var listRel = projectContext.ArcRelationshipGroup_Relationship.Where(x => x.ArcRelationshipGroupID == arcGroupID);
    //    //        foreach (var item in listRel)
    //    //        {
    //    //            result.Add(ToArcRelationshipGroup_RelationshipDTO(item));
    //    //        }
    //    //    }
    //    //    return result;
    //    //}

    //    private ArcRelationshipGroup_RelationshipDTO ToArcRelationshipGroup_RelationshipDTO(DataAccess.ArcRelationshipGroup_Relationship item)
    //    {
    //        ArcRelationshipGroup_RelationshipDTO result = new ArcRelationshipGroup_RelationshipDTO();
    //        result.ArcRelationshipGroupID = item.ArcRelationshipGroupID;
    //        result.RelationshipID = item.RelationshipID;

    //        return result;
    //    }


    //    public void SaveArcRelationshipGroup(List<ArcRelationshipGroupDTO> list)
    //    {
    //        using (var projectContext = new DataAccess.MyProjectEntities())
    //        {
    //            foreach (var item in list)
    //            {
    //                ArcRelationshipGroup dbItem = null;
    //                if (item.ID == 0)
    //                {
    //                    dbItem = new ArcRelationshipGroup();
    //                    projectContext.ArcRelationshipGroup.Add(dbItem);
    //                }
    //                else
    //                    dbItem = projectContext.ArcRelationshipGroup.First(x => x.ID == item.ID);
    //                dbItem.GroupName = item.GroupName;
    //                dbItem.TableDrivedEntityID = item.TableDrivedEntityID;
    //                while (dbItem.ArcRelationshipGroup_Relationship.Any())
    //                    dbItem.ArcRelationshipGroup_Relationship.Remove(dbItem.ArcRelationshipGroup_Relationship.First());
    //                foreach(var relation in item.Relationships)
    //                {
    //                    dbItem.ArcRelationshipGroup_Relationship.Add(new ArcRelationshipGroup_Relationship() { RelationshipID = relation.RelationshipID });
    //                }
    //                projectContext.SaveChanges();
    //            }
    //        }
    //    }
    //}
  
}
