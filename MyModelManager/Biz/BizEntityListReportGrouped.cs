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
    public class BizEntityListReportGrouped
    {
        public List<EntityListReportGroupedDTO> GetEntityListReportGroupeds()
        {
            List<EntityListReportGroupedDTO> result = new List<EntityListReportGroupedDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var listEntityListReportGrouped = projectContext.EntityListReportGrouped;
                foreach (var item in listEntityListReportGrouped)
                    result.Add(ToEntityListReportGroupedDTO(item, false));

            }
            return result;
        }
        public List<EntityListReportGroupedDTO> GetEntityListReportGroupeds(int entityID)
        {
            //var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.Validation, entityID.ToString());
            //if (cachedItem != null)
            //    return (cachedItem as List<EntityListReportGroupedDTO>);

            List<EntityListReportGroupedDTO> result = new List<EntityListReportGroupedDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var listEntityListReportGrouped = projectContext.EntityListReportGrouped.Where(x => x.EntityReport.TableDrivedEntityID == entityID);
                foreach (var item in listEntityListReportGrouped)
                    result.Add(ToEntityListReportGroupedDTO(item, false));

            }
            //CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.Validation, entityID.ToString());
            return result;
        }
        public EntityListReportGroupedDTO GetEntityListReportGrouped(int EntityListReportGroupedsID, bool withDetails)
        {
            List<EntityListReportGroupedDTO> result = new List<EntityListReportGroupedDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var EntityListReportGroupeds = projectContext.EntityListReportGrouped.First(x => x.ID == EntityListReportGroupedsID);
                return ToEntityListReportGroupedDTO(EntityListReportGroupeds, withDetails);
            }
        }
        BizEntityReport bizEntityReport = new MyModelManager.BizEntityReport();
        public EntityListReportGroupedDTO ToEntityListReportGroupedDTO(EntityListReportGrouped item, bool withDetails)
        {
            EntityListReportGroupedDTO result = new EntityListReportGroupedDTO();
            result.ID = item.ID;
            result.EntityListReportID = item.EntityListReportID;
            bizEntityReport.ToEntityReportDTO(item.EntityReport, result as EntityReportDTO, withDetails);


            if (withDetails)
            {


                BizEntityListReport bizEntityListReport = new BizEntityListReport();



                result.EntityListReport = bizEntityListReport.ToEntityListReportDTO(item.EntityListReport, true);
                //foreach (var sub in item.ReportGroups)
                //{
                //    ReportGroupDTO rColumn = new ReportGroupDTO();
                //    rColumn.ID = sub.ID;
                //    rColumn.ListViewColumnID = sub.EntityListViewColumnsID;
                //    rColumn.EntityListViewColumn = result.EntityListReport.EntityListView.EntityListViewAllColumns.First(x => x.ID == sub.EntityListViewColumnsID);
                //    rColumn.ColumnName = rColumn.EntityListViewColumn.Column.Alias;
                //    result.ReportGroups.Add(rColumn);
                //}
            }

            return result;
        }
        public void UpdateEntityListReportGroupeds(EntityListReportGroupedDTO message)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                BizEntityReport bizEntityReport = new MyModelManager.BizEntityReport();

                var dbEntityListReportGrouped = projectContext.EntityListReportGrouped.FirstOrDefault(x => x.ID == message.ID);
                if (dbEntityListReportGrouped == null)
                {
                    dbEntityListReportGrouped = new DataAccess.EntityListReportGrouped();
                    dbEntityListReportGrouped.EntityReport = bizEntityReport.ToNewEntityReport(message as EntityReportDTO,ReportType.ListReportGrouped);
                }
                else
                    bizEntityReport.ToUpdateEntityReport(dbEntityListReportGrouped.EntityReport, message as EntityReportDTO);

                dbEntityListReportGrouped.EntityReport.Title = message.ReportTitle;

                dbEntityListReportGrouped.EntityListReportID = message.EntityListReportID;
                //while (dbEntityListReportGrouped.ReportGroups.Any())
                //    projectContext.ReportGroups.Remove(dbEntityListReportGrouped.ReportGroups.First());
                //foreach (var sub in message.ReportGroups)
                //{
                //    ReportGroups rColumn = new ReportGroups();
                //    rColumn.EntityListViewColumnsID = sub.ListViewColumnID;
                //    dbEntityListReportGrouped.ReportGroups.Add(rColumn);
                //}

                if (dbEntityListReportGrouped.ID == 0)
                    projectContext.EntityListReportGrouped.Add(dbEntityListReportGrouped);
                projectContext.SaveChanges();
            }
        }
    }

}
