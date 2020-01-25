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
    public class BizEntityDataView
    {
        //public List<EntityDataViewDTO> GetEntityDataViews(int entityID, bool withDetails)
        //{
        //    //var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.DataView, entityID.ToString());
        //    //if (cachedItem != null)
        //    //    return (cachedItem as List<EntityDataViewDTO>);

        //    List<EntityDataViewDTO> result = new List<EntityDataViewDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var listEntityDataView = projectContext.EntityDataView.Where(x => x.TableDrivedEntityID == entityID);
        //        foreach (var item in listEntityDataView)
        //            result.Add(ToEntityDataViewDTO(item, withDetails));

        //    }
        //    //CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.DataView, entityID.ToString());
        //    return result;
        //}
        public DataViewSettingDTO GetDataViewSetting(int entitiyID, bool withDetails)
        {
            List<DataViewSettingDTO> result = new List<DataViewSettingDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var item = projectContext.DataViewSetting.FirstOrDefault(x => x.ID == entitiyID);
                if (item != null)
                    return ToDataViewSettingDTO(item, withDetails);
                else
                    return null;
            }
        }
        BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
        private DataViewSettingDTO ToDataViewSettingDTO(DataViewSetting item, bool withDetails)
        {
            DataViewSettingDTO result = new DataViewSettingDTO();
            result.EntityListViewID = item.EntityListViewID ?? 0;
            result.IconContent = item.IconContent;
           
            //if (result.CodeFunctionID != 0 && withDetails)
            //{
            //    var bizCodeFunction = new BizCodeFunction();
            //    result.CodeFunction = bizCodeFunction.GetCodeFunction(item.CodeFunctionID);
            //}
            //result.TableDrivedEntityID = item.TableDrivedEntityID;
            result.ID = item.ID;


            return result;
        }
        public void UpdateEntityDataViews(int entityID, DataViewSettingDTO message)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbEntity = projectContext.TableDrivedEntity.First(x => x.ID == entityID);

                if (dbEntity.DataViewSetting == null)
                {
                    dbEntity.DataViewSetting = new DataAccess.DataViewSetting();
                }
               
                dbEntity.DataViewSetting.EntityListViewID = (message.EntityListViewID != 0 ? message.EntityListViewID : (int?)null);
                dbEntity.DataViewSetting.IconContent = message.IconContent;
                projectContext.SaveChanges();
            }
        }
    }

}
