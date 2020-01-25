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
    public class BizEntityListReport
    {
        BizEntitySearchableReport bizEntitySearchableReport = new BizEntitySearchableReport();
        public BizEntityListReport()
        {

        }
        public List<EntityListReportDTO> GetEntityListReports(DR_Requester requester)
        {
            List<EntityListReportDTO> result = new List<EntityListReportDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var listEntityListReport = projectContext.EntityListReport;
                foreach (var item in listEntityListReport)
                    result.Add(ToEntityListReportDTO(requester, item, false));

            }
            return result;
        }
        public List<EntityListReportDTO> GetEntityListReports(DR_Requester requester, int entityID)
        {
            //var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.Validation, entityID.ToString());
            //if (cachedItem != null)
            //    return (cachedItem as List<EntityListReportDTO>);

            List<EntityListReportDTO> result = new List<EntityListReportDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var listEntityListReport = projectContext.EntityListReport.Where(x => x.EntitySearchableReport.EntityReport.TableDrivedEntityID == entityID);
                foreach (var item in listEntityListReport)
                    result.Add(ToEntityListReportDTO(requester, item, false));

            }
            //CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.Validation, entityID.ToString());
            return result;
        }
        public EntityListReportDTO GetEntityListReport(DR_Requester requester, int EntityListReportsID, bool withDetails)
        {
            List<EntityListReportDTO> result = new List<EntityListReportDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var EntityListReports = projectContext.EntityListReport.First(x => x.ID == EntityListReportsID);
                return ToEntityListReportDTO(requester, EntityListReports, withDetails);
            }
        }
        BizEntityReport bizEntityReport = new MyModelManager.BizEntityReport();

        public EntityListReportDTO ToEntityListReportDTO(DR_Requester requester, EntityListReport item, bool withDetails)
        {
            EntityListReportDTO result = new EntityListReportDTO();
            result.ID = item.ID;
            bizEntitySearchableReport.ToEntitySearchableReportDTO(item.EntitySearchableReport, result, withDetails);
            result.EntityListViewID = item.EntityListViewID;

            if (withDetails)
            {


                BizEntityRelationshipTail bizEntityRelationshipTail = new MyModelManager.BizEntityRelationshipTail();
                BizEntityListView bizEntityListView = new BizEntityListView();
                result.EntityListView = bizEntityListView.GetEntityListView(requester, item.EntityListViewID);
                if (result.EntityListView == null)
                {
                    throw new Exception("عدم دسترسی به لیست نمایش به شناسه" + " " + item.EntityListViewID);
                }
                foreach (var dbSubReport in item.EntityListReportSubs1)
                {
                    EntityListReportSubsDTO subReport = new EntityListReportSubsDTO();
                    subReport.Title = dbSubReport.Title;
                    subReport.OrderID = dbSubReport.OrderID ?? 0;
                    subReport.EntityRelationshipTailID = dbSubReport.EntityRelationshipTailID.Value;
                    subReport.EntityListReportID = dbSubReport.ChildEntityListReportID;
                    foreach (var subColumn in dbSubReport.EntityListReportSubsColumns)
                    {
                        EntityListReportSubsColumnsDTO rSubsColumn = new EntityListReportSubsColumnsDTO();
                        rSubsColumn.ParentEntityListViewColumnsID = subColumn.ParentEntityListViewColumnsID;
                        rSubsColumn.ParentEntityListViewColumnAlias = subColumn.EntityListViewColumns1.Alias ?? subColumn.EntityListViewColumns1.Column.Name;
                        rSubsColumn.ParentEntityListViewColumnRelativeName = subColumn.EntityListViewColumns1.Column.Name + (subColumn.EntityListViewColumns1.EntityRelationshipTailID ?? 0);
                        rSubsColumn.ParentEntityListViewColumnType = (Enum_ColumnType)subColumn.EntityListViewColumns1.Column.TypeEnum;

                        rSubsColumn.ChildEntityListViewColumnsID = subColumn.ChildEntityListViewColumnsID;
                        rSubsColumn.ChildEntityListViewColumnAlias = subColumn.EntityListViewColumns.Alias ?? subColumn.EntityListViewColumns.Column.Name;
                        rSubsColumn.ChildEntityListViewColumnRelativeName = subColumn.EntityListViewColumns.Column.Name + (subColumn.EntityListViewColumns.EntityRelationshipTailID ?? 0);
                        rSubsColumn.ChildEntityListViewColumnType = (Enum_ColumnType)subColumn.EntityListViewColumns.Column.TypeEnum;

                        subReport.SubsColumnsDTO.Add(rSubsColumn);
                    }
                    result.EntityListReportSubs.Add(subReport);

                }
                foreach (var sub in item.ReportGroups)
                {
                    ReportGroupDTO rColumn = new ReportGroupDTO();
                    rColumn.ID = sub.ID;
                    rColumn.ListViewColumnID = sub.EntityListViewColumnsID;
                    rColumn.EntityListViewColumn = result.EntityListView.EntityListViewAllColumns.First(x => x.ID == sub.EntityListViewColumnsID);
                    rColumn.ColumnName = rColumn.EntityListViewColumn.Column.Alias;
                    result.ReportGroups.Add(rColumn);
                }
            }

            return result;
        }
        public void UpdateEntityListReports(EntityListReportDTO message)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                BizEntityReport bizEntityReport = new MyModelManager.BizEntityReport();

                var dbEntitySpecifiedReport = projectContext.EntityListReport.FirstOrDefault(x => x.ID == message.ID);
                if (dbEntitySpecifiedReport == null)
                {
                    message.ReportType = ReportType.SearchableReport;
                    message.SearchableReportType = SearchableReportType.ListReport;
                    dbEntitySpecifiedReport = new EntityListReport();
                    dbEntitySpecifiedReport.EntitySearchableReport = bizEntitySearchableReport.ToNewEntitySearchableReport(message);
                }
                else
                    bizEntitySearchableReport.ToUpdateEntitySearchableReport(dbEntitySpecifiedReport.EntitySearchableReport, message);

                dbEntitySpecifiedReport.EntityListViewID = message.EntityListViewID;
                while (dbEntitySpecifiedReport.EntityListReportSubs1.Any())
                {
                    while (dbEntitySpecifiedReport.EntityListReportSubs1.First().EntityListReportSubsColumns.Any())
                        projectContext.EntityListReportSubsColumns.Remove(dbEntitySpecifiedReport.EntityListReportSubs1.First().EntityListReportSubsColumns.First());
                    projectContext.EntityListReportSubs.Remove(dbEntitySpecifiedReport.EntityListReportSubs1.First());
                }
                foreach (var sub in message.EntityListReportSubs)
                {
                    EntityListReportSubs dbSubReport = new EntityListReportSubs();
                    dbSubReport.Title = sub.Title;
                    dbSubReport.ChildEntityListReportID = sub.EntityListReportID;
                    dbSubReport.OrderID = sub.OrderID;
                    dbSubReport.EntityRelationshipTailID = sub.EntityRelationshipTailID;
                    foreach (var subColumn in sub.SubsColumnsDTO)
                    {
                        EntityListReportSubsColumns rSubsColumn = new EntityListReportSubsColumns();
                        rSubsColumn.ParentEntityListViewColumnsID = subColumn.ParentEntityListViewColumnsID;
                        rSubsColumn.ChildEntityListViewColumnsID = subColumn.ChildEntityListViewColumnsID;
                        dbSubReport.EntityListReportSubsColumns.Add(rSubsColumn);
                    }

                    dbEntitySpecifiedReport.EntityListReportSubs1.Add(dbSubReport);
                }
                while (dbEntitySpecifiedReport.ReportGroups.Any())
                    projectContext.ReportGroups.Remove(dbEntitySpecifiedReport.ReportGroups.First());
                foreach (var sub in message.ReportGroups)
                {
                    ReportGroups rColumn = new ReportGroups();
                    rColumn.EntityListViewColumnsID = sub.ListViewColumnID;
                    dbEntitySpecifiedReport.ReportGroups.Add(rColumn);
                }
                if (dbEntitySpecifiedReport.ID == 0)
                    projectContext.EntityListReport.Add(dbEntitySpecifiedReport);
                projectContext.SaveChanges();
            }
        }
    }

}
