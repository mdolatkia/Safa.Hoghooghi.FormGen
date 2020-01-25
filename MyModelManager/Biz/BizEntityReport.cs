using DataAccess;
using ModelEntites;
using MyCacheManager;
using MyGeneralLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProxyLibrary;


namespace MyModelManager
{
    public class BizEntityReport
    {
        //BizEntityListReport bizEntityListReport = new BizEntityListReport();
        //BizEntityListReportGrouped bizEntityListReportGrouped = new BizEntityListReportGrouped();
        //BizEntityChartReport bizEntityChartReport = new BizEntityChartReport();
        //BizEntityCrosstabReport bizEntityCrosstabReport = new BizEntityCrosstabReport();
        //BizEntityExternalReport bizEntityExternalReport = new BizEntityExternalReport();
        SecurityHelper securityHelper = new SecurityHelper();

        public List<EntityReportDTO> GetEntityReports(DR_Requester requester, int entityID)
        {
            //var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.Validation, entityID.ToString());
            //if (cachedItem != null)
            //    return (cachedItem as List<EntityReportDTO>);

            List<EntityReportDTO> result = new List<EntityReportDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var listEntityReport = projectContext.EntityReport.Where(x => x.TableDrivedEntityID == entityID);
                foreach (var dbItem in listEntityReport)
                {
                    if (DataIsAccessable(requester, dbItem))
                    {
                        var nItem = new EntityReportDTO();
                        ToEntityReportDTO(dbItem, nItem);
                        result.Add(nItem);
                    }
                }

            }
            //CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.Validation, entityID.ToString());
            return result;
        }
        public EntityReportDTO GetEntityReport(DR_Requester requester, int EntityReportID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbItem = projectContext.EntityReport.First(x => x.ID == EntityReportID);
                if (DataIsAccessable(requester, dbItem))
                {
                    var nItem = new EntityReportDTO();
                    ToEntityReportDTO(dbItem, nItem);
                    return nItem;
                }
                else
                    return null;
            }
        }
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        internal bool DataIsAccessable(DR_Requester requester, int reportID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var report = projectContext.EntityReport.First(x => x.ID == reportID);
                return DataIsAccessable(requester, report);
            }
        }
        internal bool DataIsAccessable(DR_Requester requester, EntityReport entityReport)
        {
            //if (!entity.IsEnabled)
            //    return false;
            //else
            //{

            if (!bizTableDrivedEntity.DataIsAccessable(requester, entityReport.TableDrivedEntity))
                return false;
            var permission = securityHelper.GetAssignedPermissions(requester, entityReport.ID, false);
            if (permission.GrantedActions.Any(y => y == SecurityAction.NoAccess))
                return false;
            else
                return true;
            //}
        }


        //private EntityReportDTO ToEntityReportDTO(EntityReport item)
        //{
        //    EntityReportDTO result = new EntityReportDTO();
        //    result.TableDrivedEntityID = item.TableDrivedEntityID;
        //    result.ID = item.ID;
        //    result.ReportType = (ReportType)item.ReportType;
        //    result.ReportTitle = item.Title;

        //        return result;
        //}

        internal void ToEntityReportDTO(EntityReport entityReport, EntityReportDTO entityReportDTO)
        {
            entityReportDTO.ID = entityReport.ID;
            entityReportDTO.ReportType = (ReportType)entityReport.ReportType;
            entityReportDTO.TableDrivedEntityID = entityReport.TableDrivedEntityID;
            entityReportDTO.ReportTitle = entityReport.Title;
            if(entityReport.EntitySearchableReport!=null)
            {
                entityReportDTO.SearchableReportType = (SearchableReportType)entityReport.EntitySearchableReport.SearchableReportType;
            }
        }


        internal EntityReport ToNewEntityReport(EntityReportDTO message)
        {
            var result = new EntityReport();
            result.SecurityObject = new SecurityObject();
            result.SecurityObject.Type = (int)DatabaseObjectCategory.Report;
            result.TableDrivedEntityID = message.TableDrivedEntityID;
            result.ReportType = (Int16)message.ReportType;
            result.Title = message.ReportTitle;

            return result;
        }



        internal void ToUpdateEntityReport(EntityReport entityReport, EntityReportDTO message)
        {
            entityReport.Title = message.ReportTitle;
        }
    }

}
