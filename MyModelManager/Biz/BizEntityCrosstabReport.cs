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
    public class BizEntityCrosstabReport
    {
        BizEntityRelationshipTail bizEntityRelationshipTail = new MyModelManager.BizEntityRelationshipTail();
        public List<EntityCrosstabReportDTO> GetEntityCrosstabReports(DR_Requester requester)
        {
            List<EntityCrosstabReportDTO> result = new List<EntityCrosstabReportDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var listEntityCrosstabReport = projectContext.EntityCrosstabReport;
                foreach (var item in listEntityCrosstabReport)
                    result.Add(ToEntityCrosstabReportDTO(requester, item, false));

            }
            return result;
        }
        public List<EntityCrosstabReportDTO> GetEntityCrosstabReports(DR_Requester requester, int entityID)
        {
            //var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.Validation, entityID.ToString());
            //if (cachedItem != null)
            //    return (cachedItem as List<EntityCrosstabReportDTO>);

            List<EntityCrosstabReportDTO> result = new List<EntityCrosstabReportDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var listEntityCrosstabReport = projectContext.EntityCrosstabReport.Where(x => x.EntitySearchableReport.EntityReport.TableDrivedEntityID == entityID);
                foreach (var item in listEntityCrosstabReport)
                    result.Add(ToEntityCrosstabReportDTO(requester, item, false));

            }
            //CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.Validation, entityID.ToString());
            return result;
        }
        public EntityCrosstabReportDTO GetEntityCrosstabReport(DR_Requester requester, int EntityCrosstabReportsID, bool withDetails)
        {
            List<EntityCrosstabReportDTO> result = new List<EntityCrosstabReportDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var EntityCrosstabReports = projectContext.EntityCrosstabReport.First(x => x.ID == EntityCrosstabReportsID);
                return ToEntityCrosstabReportDTO(requester, EntityCrosstabReports, withDetails);
            }
        }



        public List<ChartReportValueFunction> GetFunctionTypes()
        {
            return Enum.GetValues(typeof(ChartReportValueFunction)).Cast<ChartReportValueFunction>().ToList();
        }

        //public List<EnumModel> GetFunctionTypes()
        //{
        //    return Enum.GetValues(typeof(CrosstabReportValueFunction)).Cast<CrosstabReportValueFunction>().Select(c => new EnumModel() { Value = (int)c, Name = c.ToString() }).ToList();
        //}
        BizEntitySearchableReport bizEntitySearchableReport = new BizEntitySearchableReport();

        public EntityCrosstabReportDTO ToEntityCrosstabReportDTO(DR_Requester requester, EntityCrosstabReport item, bool withDetails)
        {
            EntityCrosstabReportDTO result = new EntityCrosstabReportDTO();
            result.ID = item.ID;
            bizEntitySearchableReport.ToEntitySearchableReportDTO(item.EntitySearchableReport, result, withDetails);
            result.EntityListViewID = item.EntityListViewID;
            if (withDetails)
            {
                BizEntityListView bizEntityListView = new BizEntityListView();
                result.EntityListView = bizEntityListView.GetEntityListView(requester, item.EntityListViewID);
                if (result.EntityListView == null)
                {
                    throw new Exception("عدم دسترسی به لیست نمایش به شناسه" + " " + item.EntityListViewID);
                }
                BizEntityRelationshipTail bizEntityRelationshipTail = new MyModelManager.BizEntityRelationshipTail();
                foreach (var sub in item.CrosstabReportColumns)
                {
                    CrosstabReportColumnDTO rColumn = new CrosstabReportColumnDTO();
                    rColumn.ID = sub.ID;
                    rColumn.EntityListViewColumnID = sub.EntityListViewColumnsID;
                    rColumn.EntityListViewColumn = result.EntityListView.EntityListViewAllColumns.FirstOrDefault(x => x.ID == sub.EntityListViewColumnsID);
                    if (rColumn.EntityListViewColumn == null)
                    {
                        throw new Exception("Crosstab report column" + " " + sub.ID + " " + "is not accessable!");
                    }
                    result.Columns.Add(rColumn);
                }
                foreach (var sub in item.CrosstabReportRows)
                {
                    CrosstabReportRowDTO rColumn = new CrosstabReportRowDTO();

                    rColumn.ID = sub.ID;
                    rColumn.EntityListViewColumnID = sub.EntityListViewColumnsID;
                    rColumn.EntityListViewColumn = result.EntityListView.EntityListViewAllColumns.FirstOrDefault(x => x.ID == sub.EntityListViewColumnsID);
                    if (rColumn.EntityListViewColumn == null)
                    {
                        throw new Exception("Crosstab report row" + " " + sub.ID + " " + "is not accessable!");
                    }
                    result.Rows.Add(rColumn);
                }
                foreach (var sub in item.CrosstabReportValues)
                {
                    CrosstabReportValueDTO rColumn = new CrosstabReportValueDTO();

                    rColumn.ID = sub.ID;
                    rColumn.EntityListViewColumnID = sub.EntityListViewColumnsID;
                    rColumn.EntityListViewColumn = result.EntityListView.EntityListViewAllColumns.First(x => x.ID == sub.EntityListViewColumnsID);
                    if (rColumn.EntityListViewColumn == null)
                    {
                        throw new Exception("Crosstab report value" + " " + sub.ID + " " + "is not accessable!");
                    }
                    rColumn.ValueFunction = (ChartReportValueFunction)sub.FunctoinType;

                    result.Values.Add(rColumn);
                }
            }

            return result;
        }
        public int UpdateEntityCrosstabReports(EntityCrosstabReportDTO message)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbEntitySpecifiedReport = projectContext.EntityCrosstabReport.FirstOrDefault(x => x.ID == message.ID);
                if (dbEntitySpecifiedReport == null)
                {
                    message.ReportType = ReportType.SearchableReport;
                    message.SearchableReportType = SearchableReportType.CrosstabReport;
                    dbEntitySpecifiedReport = new DataAccess.EntityCrosstabReport();
                    dbEntitySpecifiedReport.EntitySearchableReport = bizEntitySearchableReport.ToNewEntitySearchableReport(message);
                }
                else
                    bizEntitySearchableReport.ToUpdateEntitySearchableReport(dbEntitySpecifiedReport.EntitySearchableReport, message);
                dbEntitySpecifiedReport.EntityListViewID = message.EntityListViewID;

                while (dbEntitySpecifiedReport.CrosstabReportColumns.Any())
                    projectContext.CrosstabReportColumns.Remove(dbEntitySpecifiedReport.CrosstabReportColumns.First());
                foreach (var sub in message.Columns)
                {
                    CrosstabReportColumns rColumn = new CrosstabReportColumns();
                    rColumn.EntityListViewColumnsID = sub.EntityListViewColumnID;
                    dbEntitySpecifiedReport.CrosstabReportColumns.Add(rColumn);
                }
                while (dbEntitySpecifiedReport.CrosstabReportRows.Any())
                    projectContext.CrosstabReportRows.Remove(dbEntitySpecifiedReport.CrosstabReportRows.First());
                foreach (var sub in message.Rows)
                {
                    CrosstabReportRows rRow = new CrosstabReportRows();
                    rRow.EntityListViewColumnsID = sub.EntityListViewColumnID;
                    dbEntitySpecifiedReport.CrosstabReportRows.Add(rRow);
                }



                while (dbEntitySpecifiedReport.CrosstabReportValues.Any())
                    projectContext.CrosstabReportValues.Remove(dbEntitySpecifiedReport.CrosstabReportValues.First());
                foreach (var sub in message.Values)
                {
                    CrosstabReportValues rColumn = new CrosstabReportValues();
                    rColumn.EntityListViewColumnsID = sub.EntityListViewColumnID;
                    rColumn.FunctoinType = (short)sub.ValueFunction;
                    dbEntitySpecifiedReport.CrosstabReportValues.Add(rColumn);
                }

                if (dbEntitySpecifiedReport.ID == 0)
                    projectContext.EntityCrosstabReport.Add(dbEntitySpecifiedReport);
                projectContext.SaveChanges();
                return dbEntitySpecifiedReport.ID;
            }
        }
    }

}
