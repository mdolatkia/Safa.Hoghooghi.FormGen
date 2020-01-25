using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Reporting;
using Telerik.Reporting.Drawing;

namespace MyReportManager
{
    public class ReportResolver
    {
        BizEntityReport bizEntityReport = new BizEntityReport();

        public ReportSource GetReportSource(DR_Requester requester, RR_ReportSourceRequest request)
        {
            var report = bizEntityReport.GetEntityReport(request.Requester, request.ReportID);
            if(report==null)
            {
                throw new Exception("دسترسی به گزارش به شناسه" + " " + request.ReportID + " " + "امکانپذیر نمی باشد");
            }
            if (report.SearchableReportType == ModelEntites.SearchableReportType.ListReport)
            {
                ListReportResolver listReportResolver = new ListReportResolver(requester, report.ID, request, Unit.Empty);
                return listReportResolver.GetListReport(requester);
            }
            else if (report.SearchableReportType == ModelEntites.SearchableReportType.CrosstabReport)
            {
                CrosstabReportResolver listReportResolver = new CrosstabReportResolver(requester, report.ID, request, Unit.Empty);
                return listReportResolver.GetCrosstabReport();
            }
            else if (report.SearchableReportType == ModelEntites.SearchableReportType.ChartReport)
            {
                var chartReport = new BizEntityChartReport().GetEntityChartReport(requester, report.ID, false);
                if (chartReport.ChartType == ModelEntites.ChartType.Column)
                {
                    ChartReportResolver chartReportResolver = new ChartReportResolver(requester, report.ID, request, Unit.Empty);
                    return chartReportResolver.GetChartReport();
                }
                else if (chartReport.ChartType == ModelEntites.ChartType.Pie)
                {
                    PieChartReportResolver chartReportResolver = new PieChartReportResolver(requester, report.ID, request, Unit.Empty);
                    return chartReportResolver.GetChartReport();
                }
                else if (chartReport.ChartType == ModelEntites.ChartType.Line)
                {
                    LineChartReportResolver chartReportResolver = new LineChartReportResolver(requester, report.ID, request, Unit.Empty);
                    return chartReportResolver.GetChartReport();
                }
                else if (chartReport.ChartType == ModelEntites.ChartType.Radar)
                {
                    RadarChartReportResolver chartReportResolver = new RadarChartReportResolver(requester, report.ID, request, Unit.Empty);
                    return chartReportResolver.GetChartReport();
                }

            }

            throw new Exception("Report type not found!");
        }
    }
}
