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
    public class BizEntityDataLinkReport
    {
        public BizEntityDataLinkReport()
        {

        }

        public List<EntityDataLinkReportDTO> GetEntityDataLinkReports(int entityID)
        {
            //var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.Validation, entityID.ToString());
            //if (cachedItem != null)
            //    return (cachedItem as List<EntityDataLinkReportDTO>);

            List<EntityDataLinkReportDTO> result = new List<EntityDataLinkReportDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var listEntityDataLinkReport = projectContext.EntityDataLinkReport.Where(x => x.EntityReport.TableDrivedEntityID == entityID);
                foreach (var item in listEntityDataLinkReport)
                    result.Add(ToEntityDataLinkReportDTO(item, false));

            }
            //CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.Validation, entityID.ToString());
            return result;
        }
        public EntityDataLinkReportDTO GetEntityDataLinkReport(int EntityDataLinkReportsID, bool withDetails)
        {
            List<EntityDataLinkReportDTO> result = new List<EntityDataLinkReportDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var EntityDataLinkReports = projectContext.EntityDataLinkReport.First(x => x.ID == EntityDataLinkReportsID);
                return ToEntityDataLinkReportDTO(EntityDataLinkReports, withDetails);
            }
        }
        BizEntityReport bizEntityReport = new MyModelManager.BizEntityReport();

        public EntityDataLinkReportDTO ToEntityDataLinkReportDTO(EntityDataLinkReport item, bool withDetails)
        {
            EntityDataLinkReportDTO result = new EntityDataLinkReportDTO();
            result.TableDrivedEntityID = item.EntityReport.TableDrivedEntityID;
            result.ID = item.ID;
            result.DataLinkID = item.DataLinkDefinitionID;
            result.ReportTitle = item.EntityReport.Title;
            bizEntityReport.ToEntityReportDTO(item.EntityReport, result as EntityReportDTO, withDetails);

            return result;
        }
        public void UpdateEntityDataLinkReports(EntityDataLinkReportDTO message)
        {
            BizEntityReport bizEntityReport = new MyModelManager.BizEntityReport();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbEntityDataLinkReport = projectContext.EntityDataLinkReport.FirstOrDefault(x => x.ID == message.ID);
                if (dbEntityDataLinkReport == null)
                {
                    dbEntityDataLinkReport = new DataAccess.EntityDataLinkReport();
                    dbEntityDataLinkReport.EntityReport = bizEntityReport.ToNewEntityReport(message as EntityReportDTO, ReportType.DataLink);
                }
                else
                    bizEntityReport.ToUpdateEntityReport(dbEntityDataLinkReport.EntityReport, message as EntityReportDTO);

                dbEntityDataLinkReport.EntityReport.Title = message.ReportTitle;

                dbEntityDataLinkReport.DataLinkDefinitionID = message.DataLinkID;

                //while (dbEntityDataLinkReport.EntityDataLinkReportSubs1.Any())
                //    projectContext.EntityDataLinkReportSubs.Remove(dbEntityDataLinkReport.EntityDataLinkReportSubs1.First());
                //foreach (var sub in message.EntityDataLinkReportSubs)
                //{
                //    EntityDataLinkReportSubs rColumn = new EntityDataLinkReportSubs();
                //    rColumn.Title = sub.Title;
                //    rColumn.ChildEntityDataLinkReportID = sub.EntityDataLinkReportID;
                //    rColumn.OrderID = sub.OrderID;
                //    rColumn.RelationshipID = sub.RelationshipID;
                //    dbEntityDataLinkReport.EntityDataLinkReportSubs1.Add(rColumn);
                //}

                if (dbEntityDataLinkReport.ID == 0)
                    projectContext.EntityDataLinkReport.Add(dbEntityDataLinkReport);
                projectContext.SaveChanges();
            }
        }
    }

}
