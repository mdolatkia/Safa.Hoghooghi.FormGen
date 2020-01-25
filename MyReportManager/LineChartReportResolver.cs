using ModelEntites;
using MyDataSearchManagerBusiness;
using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using Telerik.Reporting;
using Telerik.Reporting.Drawing;

namespace MyReportManager
{
    public class LineChartReportResolver
    {
        //List<ReportGroupDTO> ReportGroups { set; get; }
        public LineChartReportResolver(DR_Requester requester, int reportID, RR_ReportSourceRequest request, Unit initialReportWidth)
        {
            ChartReportReportDTO = bizEntityChartReport.GetEntityChartReport( requester, reportID, true);
            //ReportGroups = groups;
            reportWidth = initialReportWidth;
            Request = request;

        }

        SearchRequestManager searchRequestManager = new SearchRequestManager();
        EntityChartReportDTO ChartReportReportDTO { set; get; }
        BizEntityChartReport bizEntityChartReport = new BizEntityChartReport();
        BizColumn bizColumn = new BizColumn();
        BizEntityReport bizEntityReport = new BizEntityReport();
        BizRelationship bizRelationship = new BizRelationship();
        //RelationshipDTO parentRelationship { set; get; }
        //int ReportLevel { set; get; }
        RR_ReportSourceRequest Request { set; get; }
        //public ReportSource GetChartReport(int reportID, RR_ReportSourceRequest request)
        //{
        //    ReportDTO = reportDTO;
        //    Request = request;
        //    var resultReport = GetReportDesign();
        //    SetReportDataSource(resultReport);

        //    return new Telerik.Reporting.InstanceReportSource { ReportDocument = resultReport };
        //}

        public ReportSource GetChartReport()
        {
            var resultReport = GetReportDesign();
            //resultReport.ItemDataBound += ResultReport_ItemDataBound;
            //if (parentRelationship != null)
            //    SetReportParameters(resultReport);
            SetReportDataSource(resultReport.Item2);
         
            return new Telerik.Reporting.InstanceReportSource { ReportDocument = resultReport.Item1 };
        }

        //private void ResultReport_ItemDataBound(object sender, EventArgs e)
        //{
        //    if (sender is Telerik.Reporting.Processing.Report)
        //        if ((sender as Telerik.Reporting.Processing.Report).Parameters.Count > 0)
        //        {
        //            Telerik.Reporting.Processing.Report subReport = (Telerik.Reporting.Processing.Report)sender;
        //            subReport.Visible = subReport.ChildElements.Find("detail", true).Length > 0;
        //        }
        //}

        //private void SetReportParameters(Report resultReport)
        //{
        //    foreach (var relColumn in parentRelationship.RelationshipColumns)
        //    {
        //        if (parentRelationship.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary)
        //            if (relColumn.FirstSideColumnID == null)
        //                continue;

        //        ReportParameter parameter = new ReportParameter();
        //        parameter.Name = relColumn.FirstSideColumn.Name;
        //        parameter.Type = ConvertParameterType(relColumn.FirstSideColumn.ColumnType);
        //        resultReport.ReportParameters.Add(parameter);
        //        //اگر الیاس داشته باشند چه؟ باید از پرنت الیاسها هم در نظر گرفته شود
        //        var filter = new Telerik.Reporting.Filter();
        //        filter.Expression = string.Format("= Fields.{0}", relColumn.SecondSideColumn.Name);
        //        filter.Operator = Telerik.Reporting.FilterOperator.Equal;
        //        filter.Value = string.Format("= Parameters.{0}.Value", relColumn.FirstSideColumn.Name);
        //        resultReport.Filters.Add(filter);

        //    }
        //}

        //private ReportParameterType ConvertParameterType(Enum_ColumnType columnType)
        //{
        //    if (columnType == Enum_ColumnType.Numeric)
        //        return ReportParameterType.Integer;
        //    else if (columnType == Enum_ColumnType.String)
        //        return ReportParameterType.String;
        //    else if (columnType == Enum_ColumnType.Boolean)
        //        return ReportParameterType.Boolean;
        //    else
        //        return ReportParameterType.String;
        //}

        ObjectDataSource objectDataSource = null;
        private void SetReportDataSource(Graph report)
        {
            
            if (objectDataSource == null)
            {
                objectDataSource = new Telerik.Reporting.ObjectDataSource();
                //EntityListViewDTO listView = CreateListView();
                //CheckSubReportRelationshipColumnExistsInSelect(ChartReportReportDTO.EntityListView);
                objectDataSource.DataSource = searchRequestManager.GetDataTableBySearchDataItems(Request.Requester, ChartReportReportDTO.TableDrivedEntityID, Request.SearchDataItems,SecurityMode.View, ChartReportReportDTO.EntityListView, 0);

            }
            report.DataSource = objectDataSource;

            //var sqlDataSource1 = new Telerik.Reporting.SqlDataSource();
            //sqlDataSource1.ConnectionString = "Data Source=.;Initial Catalog=SampleDB;Integrated Security=True";
            //sqlDataSource1.Name = "sqlDataSource1";
            //var query = searchRequestManager.GetQuery(request.Requester, reportDTO.EntityChartReport.TableDrivedEntityID, request.SearchDataItems, reportDTO.EntityChartReport.EntityListView);
            //sqlDataSource1.SelectCommand = query;
            //resultReport.DataSource = sqlDataSource1;


            //           // sqlDataSource1.SelectCommand = @"SELECT        Employee_Project.EmployeeSSN, Employee_Project.ProjectNumber, Project.Name, Employee_Project.Hours
            //FROM Employee_Project INNER JOIN
            //                         Project ON Employee_Project.ProjectNumber = Project.Number";
        }

        //private EntityListViewDTO CreateListView()
        //{
        //    EntityListViewDTO result = new EntityListViewDTO();
        //    foreach (var item in ChartReportReportDTO.EntityChartReportSeries)
        //    {
        //        var column = new EntityListViewColumnsDTO();
        //        column.ColumnID = item.ColumnID;
        //        column.Column = bizColumn.GetColumn(item.ColumnID, true);
        //        result.EntityListViewAllColumns.Add(column);
        //    }
        //    foreach (var item in ChartReportReportDTO.EntityChartReportValues)
        //    {
        //        if (!result.EntityListViewAllColumns.Any(x => x.ColumnID == item.ColumnID))
        //        {
        //            var column = new EntityListViewColumnsDTO();
        //            column.ColumnID = item.ColumnID;
        //            column.Column = bizColumn.GetColumn(item.ColumnID, true);
        //            result.EntityListViewAllColumns.Add(column);
        //        }
        //    }
        //    foreach (var item in ChartReportReportDTO.EntityChartReportCategories)
        //    {
        //        if (!result.EntityListViewAllColumns.Any(x => x.ColumnID == item.ColumnID))
        //        {
        //            var column = new EntityListViewColumnsDTO();
        //            column.ColumnID = item.ColumnID;
        //            column.Column = bizColumn.GetColumn(item.ColumnID, true);
        //            result.EntityListViewAllColumns.Add(column);
        //        }
        //    }
        //    return result;
        //}

        //private void CheckSubReportRelationshipColumnExistsInSelect(EntityListViewDTO entityListView)
        //{
        //    foreach (var sub in ChartReportReportDTO.EntityChartReportSubs)
        //    {
        //        var relationship = bizRelationship.GetRelationship(sub.RelationshipID);
        //        foreach (var relColumn in relationship.RelationshipColumns)
        //        {

        //            if (relColumn.FirstSideColumnID != null)
        //            {
        //                if (!entityListView.EntityListViewAllColumns.Any(x => x.ColumnID == relColumn.FirstSideColumnID))
        //                {
        //                    var newlistView = new EntityListViewColumnsDTO();
        //                    newlistView.ColumnID = relColumn.FirstSideColumnID.Value;
        //                    newlistView.Column = bizColumn.GetColumn(newlistView.ColumnID, true);
        //                    entityListView.EntityListViewAllColumns.Add(newlistView);
        //                }
        //            }
        //        }
        //    }
        //    if (parentRelationship != null)
        //    {
        //        foreach (var relColumn in parentRelationship.RelationshipColumns)
        //        {
        //            //برای ثابتها نیازی نیست اینجا..باید در زیر ریپورت فیلتر شوند
        //            if (relColumn.SecondSideColumnID != null)
        //            {
        //                if (!entityListView.EntityListViewAllColumns.Any(x => x.ColumnID == relColumn.SecondSideColumnID))
        //                {
        //                    var newlistView = new EntityListViewColumnsDTO();
        //                    newlistView.ColumnID = relColumn.SecondSideColumnID.Value;
        //                    newlistView.Column = bizColumn.GetColumn(newlistView.ColumnID, true);
        //                    entityListView.EntityListViewAllColumns.Add(newlistView);
        //                }
        //            }
        //        }
        //    }
        //}

        Unit reportWidth;
        private Tuple<Report, Graph> GetReportDesign()
        {
            var report = new Report();
            SetReportProperties(report);

            //var listColumnSizeLocation = GetColumnsSizeLocation();

            SetReportHeader(report);
            //SetGroupHeader(report, listColumnSizeLocation);

            return SetReportDetails(report);
        }

        //private List<ColumnSizeLocation> GetColumnsSizeLocation()
        //{
        //    List<ColumnSizeLocation> result = new List<ColumnSizeLocation>();

        //    int widthUnits = 0;
        //    var columns = ChartReportReportDTO.EntityListView.EntityListViewAllColumns.Where(x => x.RelationshipTailID == 0);
        //    foreach (var column in columns)
        //    {
        //        widthUnits += (column.WidthUnit == 0 ? 1 : column.WidthUnit);
        //    }
        //    var width = reportWidth;
        //    var perUnitWidth = width.Divide(widthUnits);
        //    var consumedWidthUnits = 0;


        //    if (ReportGroups != null)
        //        foreach (var reportGroup in ReportGroups)
        //        {
        //            ColumnSizeLocation columnSizeLocation = new ColumnSizeLocation();
        //            columnSizeLocation.LictViewColumnID = reportGroup.ListViewColumnID;
        //            SetColumnSizePosition(columnSizeLocation, reportGroup.EntityListViewColumn.WidthUnit, ref consumedWidthUnits, perUnitWidth);
        //            result.Add(columnSizeLocation);

        //        }
        //    foreach (var column in columns)
        //    {
        //        if (!result.Any(x => x.LictViewColumnID == column.ID))
        //        {
        //            ColumnSizeLocation columnSizeLocation = new ColumnSizeLocation();
        //            columnSizeLocation.LictViewColumnID = column.ID;
        //            SetColumnSizePosition(columnSizeLocation, column.WidthUnit, ref consumedWidthUnits, perUnitWidth);
        //            result.Add(columnSizeLocation);
        //        }
        //    }
        //    return result;
        //}
        //private void SetColumnSizePosition(ColumnSizeLocation columnSizeLocation, int currentWidthUnits, ref int consumedWidthUnits, Unit perUnitWidth)
        //{
        //    if (currentWidthUnits == 0)
        //        currentWidthUnits = 1;

        //    var width = currentWidthUnits * perUnitWidth;
        //    columnSizeLocation.Width = width;

        //    var xlocation = (reportWidth - width) - consumedWidthUnits * perUnitWidth;
        //    columnSizeLocation.XLocation = xlocation;


        //    consumedWidthUnits += currentWidthUnits;

        //}
        private Tuple<Report, Graph> SetReportDetails(Report report)
        {

            var detail = new Telerik.Reporting.DetailSection();
            detail.Height = Telerik.Reporting.Drawing.Unit.Cm(0.5);
            detail.Name = "detail";

            ReportStyles.SetReportDetailStyle(detail.Style);
            report.Items.Add(detail);


            var graph = new Graph();
            graph.Width = reportWidth;
            graph.Height = report.PageSettings.PaperSize.Height/2 ;
            //reportWidth = report.Width;
            detail.Items.Add(graph);

            graph.Legend.Width = Unit.Cm(3);
            graph.Legend.Style.LineColor = System.Drawing.Color.LightGray;
            graph.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            graph.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray;
            graph.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            //graph.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(6D), Telerik.Reporting.Drawing.Unit.Inch(3D));

            GraphAxis categoryAxis = new GraphAxis();
            categoryAxis.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            categoryAxis.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            categoryAxis.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            categoryAxis.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            categoryAxis.MinorGridLineStyle.Visible = false;
            categoryAxis.Scale = new CategoryScale();

            GraphAxis valueAxis = new GraphAxis();
            valueAxis.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            valueAxis.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            valueAxis.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            valueAxis.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            valueAxis.MinorGridLineStyle.Visible = false;
            valueAxis.Scale = new NumericalScale();

            var cartesianCoordinateSystem1 = new CartesianCoordinateSystem();
            cartesianCoordinateSystem1.Name = "cartesianCoordinateSystem1";
            cartesianCoordinateSystem1.XAxis = categoryAxis;
            cartesianCoordinateSystem1.YAxis = valueAxis;
            graph.CoordinateSystems.Add(cartesianCoordinateSystem1);

            var categoryColumn = ChartReportReportDTO.EntityChartReportCategories.First();
            var categoryGroup = new GraphGroup();
            categoryGroup.Name = categoryColumn.EntityListViewColumn.RelativeColumnName;
            categoryGroup.Groupings.Add(new Grouping(string.Format("= Fields.{0}", categoryColumn.EntityListViewColumn.RelativeColumnName)));
            graph.CategoryGroups.Add(categoryGroup);

            foreach (var column in ChartReportReportDTO.EntityChartReportSeries)
            {
                foreach (var valuecolumn in ChartReportReportDTO.EntityChartReportValues)
                {

                    var barSeries = new Telerik.Reporting.LineSeries();
                    //if (column.EntityListViewColumnID == categoryColumn.EntityListViewColumnID)
                    //    barSeries.ArrangeMode = GraphSeriesArrangeMode.Clustered;
                    //else
                    //{
                        //if (column.ArrangeType == ChartSerieArrangeType.Clustered)
                            barSeries.ArrangeMode = GraphSeriesArrangeMode.Clustered;
                        //else if (column.ArrangeType == ChartSerieArrangeType.Overlapped)
                        //    barSeries.ArrangeMode = GraphSeriesArrangeMode.Overlapped;
                        //else if (column.ArrangeType == ChartSerieArrangeType.Stacked)
                        //    barSeries.ArrangeMode = GraphSeriesArrangeMode.Stacked;
                        //else if (column.ArrangeType == ChartSerieArrangeType.Stacked100)
                        //    barSeries.ArrangeMode = GraphSeriesArrangeMode.Stacked100;
                    //}

                    barSeries.ToolTip.Text = string.Format("= {0}(Fields.{1})", valuecolumn.FunctionType.ToString(), valuecolumn.EntityListViewColumn.RelativeColumnName);
                    barSeries.ToolTip.Title = string.Format("= Fields.{0}", column.EntityListViewColumn.RelativeColumnName);
                    barSeries.Y = string.Format("= {0}(Fields.{1})", valuecolumn.FunctionType.ToString(), valuecolumn.EntityListViewColumn.RelativeColumnName);

                   



                    //categoryGroup.Groupings.Add(new Telerik.Reporting.Grouping());
                    //ChartSeries

                    var serieGroup = new GraphGroup();
                    serieGroup.Name = column.EntityListViewColumn.RelativeColumnName;
                    serieGroup.Groupings.Add(new Telerik.Reporting.Grouping(string.Format("= Fields.{0}", column.EntityListViewColumn.RelativeColumnName)));
                    serieGroup.Sortings.Add(new Telerik.Reporting.Sorting(string.Format("= Fields.{0}", column.EntityListViewColumn.RelativeColumnName), Telerik.Reporting.SortDirection.Asc));
                    graph.SeriesGroups.Add(serieGroup);

                    barSeries.SeriesGroup = serieGroup;
                    barSeries.CategoryGroup = categoryGroup;

                    barSeries.CoordinateSystem = cartesianCoordinateSystem1;
                    barSeries.DataPointLabel = string.Format("= {0}(Fields.{1})", valuecolumn.FunctionType.ToString(), valuecolumn.EntityListViewColumn.RelativeColumnName);
                    barSeries.DataPointLabelStyle.Visible = true;
                    barSeries.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0);
                    barSeries.DataPointStyle.Visible = true;
                    barSeries.LegendItem.Value = string.Format("= Fields.{0}", column.EntityListViewColumn.RelativeColumnName);
                    barSeries.LineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
                    barSeries.MarkerMaxSize = Telerik.Reporting.Drawing.Unit.Pixel(50D);
                    barSeries.MarkerMinSize = Telerik.Reporting.Drawing.Unit.Pixel(5D);
                    barSeries.MarkerSize = Telerik.Reporting.Drawing.Unit.Pixel(5D);
                    barSeries.Name = column.EntityListViewColumn.RelativeColumnName;

                    graph.Series.Add(barSeries);

                }
            }

            return new Tuple<Report, Graph>(report, graph);
        }

        private void SetReportProperties(Report report)
        {
            if (reportWidth == Unit.Empty)
            {
                report.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
                report.PageSettings.Margins.Bottom = Unit.Cm(1);
                report.PageSettings.Margins.Top = Unit.Cm(1);
                report.PageSettings.Margins.Left = Unit.Cm(1);
                report.PageSettings.Margins.Right = Unit.Cm(1);
                report.Width = report.PageSettings.PaperSize.Width - Unit.Cm(2);
                reportWidth = report.Width;
            }
            report.Style.BackgroundColor = System.Drawing.Color.Empty;
            report.Style.BorderColor.Default = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(67)))), ((int)(((byte)(113)))));
            report.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            report.Style.Font.Name = "B Yekan";


            //report.Name = "Report2";
            report.PageSettings.ContinuousPaper = false;
            //   report.PageSettings.Landscape = false;
            //  report.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(5D), Telerik.Reporting.Drawing.Unit.Mm(5D), Telerik.Reporting.Drawing.Unit.Mm(5D), Telerik.Reporting.Drawing.Unit.Mm(5D));
            //report.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;

        }

        private void SetReportHeader(Report report)
        {


            var reportHeader = new ReportHeaderSection();
            reportHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(1.5);

            reportHeader.Name = "reportHeader";
            ReportStyles.SetChartReportHeaderStyle(reportHeader.Style);
            report.Items.Add(reportHeader);

            var titleWidth = Unit.Cm(4);
            var titleHeight = Unit.Cm(1);
            var titleTextBox = new TextBox();
            titleTextBox.Location = new Telerik.Reporting.Drawing.PointU(reportWidth - titleWidth, Telerik.Reporting.Drawing.Unit.Cm(0));
            titleTextBox.Name = "txtTitle";
            //titleTextBox.Docking = DockingStyle.Top;
            titleTextBox.Width = titleWidth;
            titleTextBox.Height = titleHeight;


            ReportStyles.SetReportTitleStyle(titleTextBox.Style);
            titleTextBox.Value = ChartReportReportDTO.ReportTitle;
            reportHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            titleTextBox});

        }

        //private void SetGroupHeader(Report report, List<ColumnSizeLocation> columnsSizeLocation)
        //{

        //    var groupHeader = new GroupHeaderSection();
        //    groupHeader.Height = Unit.Mm(7);
        //    groupHeader.PrintOnEveryPage = true;
        //    groupHeader.Name = "groupHeader";
        //    ReportStyles.SetGeoupColumnHeaderStyle(groupHeader.Style, ReportLevel);
        //    report.Items.Add(groupHeader);

        //    var columns = ChartReportReportDTO.EntityListView.EntityListViewAllColumns.Where(x => x.RelationshipTailID == 0);
        //    //  var columnHeight = groupHeader.Height;

        //    //گروه عناوین همه ستونها
        //    int index = 0;
        //    foreach (var column in columns)
        //    {
        //        var columnTextbox = new TextBox();
        //        columnTextbox.CanGrow = true;
        //        columnTextbox.Name = column.Column.Name;
        //        ReportStyles.SetColumnHeaderTextboxStyle(columnTextbox.Style);
        //        var columnSizeLocation = columnsSizeLocation.First(x => x.LictViewColumnID == column.ID);
        //        columnTextbox.Width = columnSizeLocation.Width;
        //        columnTextbox.Location = new PointU(columnSizeLocation.XLocation, Unit.Cm(0));
        //        columnTextbox.Height = groupHeader.Height;
        //        var alias = column.Alias;
        //        if (string.IsNullOrEmpty(alias))
        //            alias = column.Column.Alias;
        //        columnTextbox.Value = alias;
        //        groupHeader.Items.Add(columnTextbox);
        //        index++;
        //    }

        //    var group = new Group();
        //    group.GroupHeader = groupHeader;
        //    report.Groups.Add(group);


        //    if (ReportGroups != null)
        //    {
        //        foreach (var reportGroup in ReportGroups)
        //        {
        //            var repotGroupHeader = new GroupHeaderSection();
        //            repotGroupHeader.Height = Unit.Mm(7);
        //            //repotGroupHeader.PrintOnEveryPage = true;
        //            repotGroupHeader.Name = "reportgroupHeader";
        //            ReportStyles.SetGeoupHeaderStyle(repotGroupHeader.Style);
        //            report.Items.Add(groupHeader);

        //            var geoupHeaderTextbox = new TextBox();
        //            geoupHeaderTextbox.CanGrow = true;
        //            geoupHeaderTextbox.Name = reportGroup.EntityListViewColumn.RelativeColumnName;
        //            ReportStyles.SetGroupHeaderTextboxStyle(geoupHeaderTextbox.Style);
        //            var columnSizeLocation = columnsSizeLocation.First(x => x.LictViewColumnID == reportGroup.ListViewColumnID);
        //            geoupHeaderTextbox.Width = columnSizeLocation.Width;
        //            geoupHeaderTextbox.Location = new PointU(columnSizeLocation.XLocation, Unit.Cm(0));
        //            geoupHeaderTextbox.Height = repotGroupHeader.Height;
        //            geoupHeaderTextbox.Value = string.Format("= Fields.{0}", reportGroup.EntityListViewColumn.Column.Name);
        //            repotGroupHeader.Items.Add(geoupHeaderTextbox);

        //            var newGroup = new Group();
        //            newGroup.GroupHeader = repotGroupHeader;
        //            newGroup.Groupings.Add(new Telerik.Reporting.Grouping(string.Format("= Fields.{0}", reportGroup.EntityListViewColumn.Column.Name)));
        //            report.Groups.Add(newGroup);
        //        }
        //    }
        //}

    }

    //public class ColumnSizeLocation
    //{
    //    public int LictViewColumnID { set; get; }
    //    public Unit XLocation { set; get; }
    //    public Unit Width { set; get; }
    //}
}