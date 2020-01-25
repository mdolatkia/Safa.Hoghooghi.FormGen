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
    public class BizEntityChartReport
    {
        BizEntitySearchableReport bizEntitySearchableReport = new BizEntitySearchableReport();
        BizEntityRelationshipTail bizEntityRelationshipTail = new MyModelManager.BizEntityRelationshipTail();
        //public List<EntityChartReportDTO> GetEntityChartReports(DR_Requester requester)
        //{
        //    List<EntityChartReportDTO> result = new List<EntityChartReportDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var listEntityChartReport = projectContext.EntityChartReport;
        //        foreach (var item in listEntityChartReport)
        //            result.Add(ToEntityChartReportDTO(requester, item, false));

        //    }
        //    return result;
        //}
        public List<EntityChartReportDTO> GetEntityChartReports(DR_Requester requester, int entityID)
        {
            //var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.Validation, entityID.ToString());
            //if (cachedItem != null)
            //    return (cachedItem as List<EntityChartReportDTO>);

            List<EntityChartReportDTO> result = new List<EntityChartReportDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var listEntityChartReport = projectContext.EntityChartReport.Where(x => x.EntitySearchableReport.EntityReport.TableDrivedEntityID == entityID);
                foreach (var item in listEntityChartReport)
                    result.Add(ToEntityChartReportDTO(requester, item, false));

            }
            //CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.Validation, entityID.ToString());
            return result;
        }
        public EntityChartReportDTO GetEntityChartReport(DR_Requester requester, int EntityChartReportsID, bool withDetails)
        {
            List<EntityChartReportDTO> result = new List<EntityChartReportDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var EntityChartReports = projectContext.EntityChartReport.First(x => x.ID == EntityChartReportsID);
                return ToEntityChartReportDTO(requester, EntityChartReports, withDetails);
            }
        }

        public List<ChartSerieArrangeType> GetSeriesArrangeTypes()
        {
            return Enum.GetValues(typeof(ChartSerieArrangeType)).Cast<ChartSerieArrangeType>().ToList();
        }
        public List<ChartType> GetChartTypes()
        {
            return Enum.GetValues(typeof(ChartType)).Cast<ChartType>().ToList();
        }
        public List<ChartReportValueFunction> GetFunctionTypes()
        {
            return Enum.GetValues(typeof(ChartReportValueFunction)).Cast<ChartReportValueFunction>().ToList();
        }

        //public List<EnumModel> GetFunctionTypes()
        //{
        //    return Enum.GetValues(typeof(ChartReportValueFunction)).Cast<ChartReportValueFunction>().Select(c => new EnumModel() { Value = (int)c, Name = c.ToString() }).ToList();
        //}
        public EntityChartReportDTO ToEntityChartReportDTO(DR_Requester requester, EntityChartReport item, bool withDetails)
        {
            EntityChartReportDTO result = new EntityChartReportDTO();
            result.ID = item.ID;
            bizEntitySearchableReport.ToEntitySearchableReportDTO(item.EntitySearchableReport, result, withDetails);
            result.ChartType = (ChartType)item.ChartType;
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
                foreach (var sub in item.CharetReportSeries)
                {
                    EntityChartReportSerieDTO rColumn = new EntityChartReportSerieDTO();
                    rColumn.EntityListViewColumnID = sub.EntityListViewColumnsID.Value;
                    rColumn.EntityListViewColumn = result.EntityListView.EntityListViewAllColumns.FirstOrDefault(x => x.ID == sub.EntityListViewColumnsID);
                    if (rColumn.EntityListViewColumn == null)
                    {
                        throw new Exception("Chart Report Serie" + " " + sub.ID + " " + "is not accessable!");
                    }
                    rColumn.ArrangeType = (ChartSerieArrangeType)sub.ArrangeType;
                    result.EntityChartReportSeries.Add(rColumn);
                }
                foreach (var sub in item.CharetReportCategories)
                {
                    EntityChartReportCategoryDTO rColumn = new EntityChartReportCategoryDTO();
                    rColumn.EntityListViewColumnID = sub.EntityListViewColumnsID.Value;
                    rColumn.EntityListViewColumn = result.EntityListView.EntityListViewAllColumns.FirstOrDefault(x => x.ID == sub.EntityListViewColumnsID);
                    if (rColumn.EntityListViewColumn == null)
                    {
                        throw new Exception("Chart Report Category" + " " + sub.ID + " " + "is not accessable!");
                    }
                    result.EntityChartReportCategories.Add(rColumn);
                }
                foreach (var sub in item.CharetReportValues)
                {
                    EntityChartReportValueDTO rColumn = new EntityChartReportValueDTO();
                    rColumn.EntityListViewColumnID = sub.EntityListViewColumnsID.Value;
                    rColumn.EntityListViewColumn = result.EntityListView.EntityListViewAllColumns.FirstOrDefault(x => x.ID == sub.EntityListViewColumnsID);
                    if (rColumn.EntityListViewColumn == null)
                    {
                        throw new Exception("Chart Report Value" + " " + sub.ID + " " + "is not accessable!");
                    }
                    rColumn.FunctionType = (ChartReportValueFunction)sub.FunctoinType;

                    result.EntityChartReportValues.Add(rColumn);
                }
            }

            return result;
        }
        public int UpdateEntityChartReports(EntityChartReportDTO message)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {

                var dbEntitySpecifiedReport = projectContext.EntityChartReport.FirstOrDefault(x => x.ID == message.ID);
                if (dbEntitySpecifiedReport == null)
                {
                    message.ReportType = ReportType.SearchableReport;
                    message.SearchableReportType = SearchableReportType.ChartReport;
                    dbEntitySpecifiedReport = new EntityChartReport();
                    dbEntitySpecifiedReport.EntitySearchableReport = bizEntitySearchableReport.ToNewEntitySearchableReport(message);
                }
                else
                    bizEntitySearchableReport.ToUpdateEntitySearchableReport(dbEntitySpecifiedReport.EntitySearchableReport, message);
                dbEntitySpecifiedReport.ChartType = (Int16)message.ChartType;
                dbEntitySpecifiedReport.EntityListViewID = message.EntityListViewID;

                while (dbEntitySpecifiedReport.CharetReportCategories.Any())
                    projectContext.CharetReportCategories.Remove(dbEntitySpecifiedReport.CharetReportCategories.First());
                if (message.ChartType != ChartType.Pie)
                {
                    var firstCategory = message.EntityChartReportCategories.First();
                    foreach (var sub in message.EntityChartReportCategories)
                    {
                        CharetReportCategories rColumn = new CharetReportCategories();
                        rColumn.EntityListViewColumnsID = sub.EntityListViewColumnID;
                        dbEntitySpecifiedReport.CharetReportCategories.Add(rColumn);
                    }
                    if (message.EntityChartReportCategories.Any() &&
                          !message.EntityChartReportSeries.Any())
                    {   //اگر سری نباشد یک اورلپ باید درست شود
                        EntityChartReportSerieDTO newSerie = new EntityChartReportSerieDTO();
                        newSerie.EntityListViewColumnID = firstCategory.EntityListViewColumnID;
                        newSerie.ArrangeType = ChartSerieArrangeType.Overlapped;
                        //newSerie.RelationshipPath = firstCategory.RelationshipPath;
                        message.EntityChartReportSeries.Add(newSerie);
                    }

                    while (dbEntitySpecifiedReport.CharetReportSeries.Any())
                        projectContext.CharetReportSeries.Remove(dbEntitySpecifiedReport.CharetReportSeries.First());
                    foreach (var sub in message.EntityChartReportSeries)
                    {
                        CharetReportSeries rColumn = new CharetReportSeries();
                        rColumn.EntityListViewColumnsID = sub.EntityListViewColumnID;
                        rColumn.ArrangeType = (Int16)sub.ArrangeType;
                        dbEntitySpecifiedReport.CharetReportSeries.Add(rColumn);
                    }
                }
                else
                {
                    //کتگوری ندارد
                    while (dbEntitySpecifiedReport.CharetReportSeries.Any())
                        projectContext.CharetReportSeries.Remove(dbEntitySpecifiedReport.CharetReportSeries.First());
                    foreach (var sub in message.EntityChartReportSeries)
                    {
                        CharetReportSeries rColumn = new CharetReportSeries();
                        rColumn.EntityListViewColumnsID = sub.EntityListViewColumnID;
                        //ارنج تایپ مهم نیست چون در هر حال موقع ساخت گزارش استک 100 میشود
                        rColumn.ArrangeType = (Int16)ChartSerieArrangeType.Stacked100;

                        dbEntitySpecifiedReport.CharetReportSeries.Add(rColumn);
                    }
                }


                while (dbEntitySpecifiedReport.CharetReportValues.Any())
                    projectContext.CharetReportValues.Remove(dbEntitySpecifiedReport.CharetReportValues.First());
                foreach (var sub in message.EntityChartReportValues)
                {
                    CharetReportValues rColumn = new CharetReportValues();
                    rColumn.EntityListViewColumnsID = sub.EntityListViewColumnID;

                    rColumn.FunctoinType = (short)sub.FunctionType;
                    dbEntitySpecifiedReport.CharetReportValues.Add(rColumn);
                }

                if (dbEntitySpecifiedReport.ID == 0)
                    projectContext.EntityChartReport.Add(dbEntitySpecifiedReport);
                projectContext.SaveChanges();
                return dbEntitySpecifiedReport.ID;
            }
        }
    }

}
