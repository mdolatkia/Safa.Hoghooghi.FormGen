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
    public class BizEntityUISetting
    {

        public EntityUISettingDTO GetEntityUISetting(int entityID)
        {
            EntityUISettingDTO result = new EntityUISettingDTO();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var EntityUISettings = projectContext.EntityUISetting.FirstOrDefault(x => x.ID == entityID);
                if(EntityUISettings!=null)
                return ToEntityUISettingDTO(EntityUISettings);
                return null;
            }
        }
        private EntityUISettingDTO ToEntityUISettingDTO(EntityUISetting item)
        {
            EntityUISettingDTO result = new EntityUISettingDTO();
            result.ID = item.ID;
            result.UIColumnsCount = item.UIColumnsCount;
            return result;
        }
        public void UpdateEntityUISettings(int entityID, EntityUISettingDTO EntityUISetting)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbEntityUISetting = projectContext.EntityUISetting.FirstOrDefault(x => x.ID == entityID);
                if (dbEntityUISetting == null)
                    dbEntityUISetting = new DataAccess.EntityUISetting();
                    dbEntityUISetting.ID = entityID;
                dbEntityUISetting.UIColumnsCount = EntityUISetting.UIColumnsCount;
                if (dbEntityUISetting.TableDrivedEntity ==null)
                    projectContext.EntityUISetting.Add(dbEntityUISetting);
                projectContext.SaveChanges();
            }
        }
    }

}
