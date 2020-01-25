using DataAccess;
using ModelEntites;
using MyCacheManager;
using MyGeneralLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizEntityGridView
    {
        //public List<EntityGridViewDTO> GetEntityGridViews(int entityID, bool withDetails)
        //{
        //    //var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.GridView, entityID.ToString());
        //    //if (cachedItem != null)
        //    //    return (cachedItem as List<EntityGridViewDTO>);

        //    List<EntityGridViewDTO> result = new List<EntityGridViewDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var listEntityGridView = projectContext.EntityGridView.Where(x => x.TableDrivedEntityID == entityID);
        //        foreach (var item in listEntityGridView)
        //            result.Add(ToEntityGridViewDTO(item, withDetails));

        //    }
        //    //CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.GridView, entityID.ToString());
        //    return result;
        //}
        public GridViewSettingDTO GetGridViewSetting(int entitiyID, bool withDetails)
        {
            List<GridViewSettingDTO> result = new List<GridViewSettingDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var item = projectContext.GridViewSetting.FirstOrDefault(x => x.ID == entitiyID);
                if (item != null)
                    return ToGridViewSettingDTO(item, withDetails);
                else
                    return null;
            }
        }
        BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
        private GridViewSettingDTO ToGridViewSettingDTO(GridViewSetting item, bool withDetails)
        {
            GridViewSettingDTO result = new GridViewSettingDTO();
            result.EntityListViewID = item.EntityListViewID ?? 0;
            //result.IconContent = item.IconContent;
            //foreach (var dbRel in item.EntityGridViewRelationships)
            //{
            //    var rel = new DataMenuGridViewRelationshipDTO();
            //    //   rel.RelationshipID = dbRel.RelationshipID ?? 0;
            //    rel.RelationshipTailID = dbRel.EntityRelationshipTailID ?? 0;
            //    if (dbRel.EntityRelationshipTail != null)
            //        rel.RelationshipTail = bizEntityRelationshipTail.ToEntityRelationshipTailDTO(dbRel.EntityRelationshipTail);
            //    rel.Group1 = dbRel.Group1;
            //    rel.Group2 = dbRel.Group2;
            //    result.EntityGridViewRelationships.Add(rel);
            //}
            //if (result.CodeFunctionID != 0 && withDetails)
            //{
            //    var bizCodeFunction = new BizCodeFunction();
            //    result.CodeFunction = bizCodeFunction.GetCodeFunction(item.CodeFunctionID);
            //}
            //result.TableDrivedEntityID = item.TableDrivedEntityID;
            result.ID = item.ID;
            return result;
        }
        public void UpdateEntityGridViews(int entityID, GridViewSettingDTO message)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbEntity = projectContext.TableDrivedEntity.First(x => x.ID == entityID);

                if (dbEntity.GridViewSetting == null)
                {
                    dbEntity.GridViewSetting = new DataAccess.GridViewSetting();
                }
                //while (dbEntity.EntityGridView.EntityGridViewRelationships.Any())
                //    projectContext.EntityGridViewRelationships.Remove(dbEntity.EntityGridView.EntityGridViewRelationships.First());
                //foreach (var item in message.EntityGridViewRelationships)
                //{
                //    EntityGridViewRelationships dbRel = new EntityGridViewRelationships();
                //    //  dbRel.RelationshipID = (item.RelationshipID == 0 ? (int?)null : item.RelationshipID);
                //    dbRel.EntityRelationshipTailID = (item.RelationshipTailID == 0 ? (int?)null : item.RelationshipTailID);
                //    dbRel.Group1 = item.Group1;
                //    dbRel.Group2 = item.Group2;
                //    dbEntity.EntityGridView.EntityGridViewRelationships.Add(dbRel);
                //}
                dbEntity.GridViewSetting.EntityListViewID = (message.EntityListViewID != 0 ? message.EntityListViewID : (int?)null);
                //dbEntity.EntityGridView.IconContent = message.IconContent;
                projectContext.SaveChanges();
            }
        }
    }

}
