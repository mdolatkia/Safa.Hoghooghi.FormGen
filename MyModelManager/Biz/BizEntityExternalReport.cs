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
    public class BizEntityExternalReport
    {
        BizEntitySearchableReport bizEntitySearchableReport = new BizEntitySearchableReport();

        //public List<EntityExternalReportDTO> GetEntityExternalReports()
        //{
        //    List<EntityExternalReportDTO> result = new List<EntityExternalReportDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var listEntityListReport = projectContext.EntityExternalReport;
        //        foreach (var item in listEntityListReport)
        //            result.Add(ToEntityExternalReportDTO(item, false));

        //    }
        //    return result;
        //}
        public List<EntityExternalReportDTO> GetEntityExternalReports(int entityID)
        {
            //var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.Validation, entityID.ToString());
            //if (cachedItem != null)
            //    return (cachedItem as List<EntityExternalReportDTO>);

            List<EntityExternalReportDTO> result = new List<EntityExternalReportDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var listEntityListReport = projectContext.EntityExternalReport.Where(x => x.EntitySearchableReport.EntityReport.TableDrivedEntityID == entityID);
                foreach (var item in listEntityListReport)
                    result.Add(ToEntityExternalReportDTO(item, false));

            }
            //CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.Validation, entityID.ToString());
            return result;
        }
        public EntityExternalReportDTO GetEntityExternalReport(int EntityListReportsID, bool withDetails)
        {
            List<EntityExternalReportDTO> result = new List<EntityExternalReportDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var EntityListReports = projectContext.EntityExternalReport.First(x => x.ID == EntityListReportsID);
                return ToEntityExternalReportDTO(EntityListReports, withDetails);
            }
        }
        BizEntityReport bizEntityReport = new MyModelManager.BizEntityReport();

        public EntityExternalReportDTO ToEntityExternalReportDTO(EntityExternalReport item, bool withDetails)
        {
            EntityExternalReportDTO result = new EntityExternalReportDTO();
            result.ID = item.ID;
            result.URL = item.URL;
            bizEntitySearchableReport.ToEntitySearchableReportDTO(item.EntitySearchableReport, result, withDetails);


            return result;
        }
        public void UpdateEntityExternalReports(EntityExternalReportDTO message)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                BizEntityReport bizEntityReport = new MyModelManager.BizEntityReport();

                var dbEntitySpecifiedReport = projectContext.EntityExternalReport.FirstOrDefault(x => x.ID == message.ID);
                if (dbEntitySpecifiedReport == null)
                {
                    message.ReportType = ReportType.SearchableReport;
                    message.SearchableReportType = SearchableReportType.ExternalReport;
                    dbEntitySpecifiedReport = new EntityExternalReport();
                    dbEntitySpecifiedReport.EntitySearchableReport = bizEntitySearchableReport.ToNewEntitySearchableReport(message);
                }
                else
                    bizEntitySearchableReport.ToUpdateEntitySearchableReport(dbEntitySpecifiedReport.EntitySearchableReport, message);

                dbEntitySpecifiedReport.URL = message.URL;
                if (dbEntitySpecifiedReport.ID == 0)
                    projectContext.EntityExternalReport.Add(dbEntitySpecifiedReport);
                projectContext.SaveChanges();
            }
        }
    }

}
