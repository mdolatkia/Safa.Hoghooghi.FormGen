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
    public class ListReportResolver
    {
        //EntityListReportGroupedDTO ListReportGroupedDTO { set; get; }
        //List<ReportGroupDTO> ReportGroups { set; get; }
        EntityListReportSubsDTO ParentToChildSubReportDTO { set; get; }
        ListReportResolver ParentListReportResolver { set; get; }
        public ListReportResolver(DR_Requester requester, int reportID, RR_ReportSourceRequest request, Unit initialReportWidth)
        {
            //if (!isGroup)
            //{
            ListReportReportDTO = bizEntityListReport.GetEntityListReport( requester, reportID, true);
            //    ReportGroups = null;
            //}
            //else
            //{
            //    ListReportGroupedDTO = bizEntityListReportGrouped.GetEntityListReportGrouped(reportID, true);
            //    ListReportReportDTO = ListReportGroupedDTO.EntityListReport;
            //    ReportGroups = ListReportGroupedDTO.ReportGroups;
            //}
            reportWidth = initialReportWidth;
            Request = request;

        }
        //private ListReportResolver(int reportID, RR_ReportSourceRequest request, Unit initialReportWidth)
        //{
        //    ListReportReportDTO = bizEntityListReport.GetEntityListReport(reportID, true);
        //    ReportGroups = groups;
        //    reportWidth = initialReportWidth;
        //    Request = request;

        //}

        private ListReportResolver(EntityListReportDTO listReportDTO, RR_ReportSourceRequest request, Unit initialReportWidth, ListReportResolver parentListReportResolver, EntityListReportSubsDTO parentToChildSubReportDTO, int reportLevel)
        {
            ParentListReportResolver = parentListReportResolver;
            ParentToChildSubReportDTO = parentToChildSubReportDTO;
            ListReportReportDTO = listReportDTO;
            reportWidth = initialReportWidth;
            Request = request;
            ReportLevel = reportLevel;
        }
        SearchRequestManager searchRequestManager = new SearchRequestManager();
        EntityListReportDTO ListReportReportDTO { set; get; }
        BizEntityListReport bizEntityListReport = new BizEntityListReport();
        //BizEntityListReportGrouped bizEntityListReportGrouped = new BizEntityListReportGrouped();
        BizColumn bizColumn = new BizColumn();
        BizEntityReport bizEntityReport = new BizEntityReport();
        BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
        //EntityRelationshipTailDTO ParentRelationshipTail { set; get; }
        int ReportLevel { set; get; }
        RR_ReportSourceRequest Request { set; get; }
        //public ReportSource GetListReport(int reportID, RR_ReportSourceRequest request)
        //{
        //    ReportDTO = reportDTO;
        //    Request = request;
        //    var resultReport = GetReportDesign();
        //    SetReportDataSource(resultReport);

        //    return new Telerik.Reporting.InstanceReportSource { ReportDocument = resultReport };
        //}
        private Telerik.Reporting.SqlDataSource sqlDataSource1;
        private Telerik.Reporting.ReportHeaderSection reportHeader;
        private Telerik.Reporting.TextBox titleTextBox;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.TextBox nameDataTextBox;
        private Telerik.Reporting.TextBox projectNumberDataTextBox;
        private Telerik.Reporting.TextBox hoursDataTextBox;
        public ReportSource GetListReport(DR_Requester requester)
        {


            var resultReport = GetReportDesign(requester);

            resultReport.ItemDataBound += ResultReport_ItemDataBound;

            SetReportDataSource(resultReport);

            return new Telerik.Reporting.InstanceReportSource { ReportDocument = resultReport };
        }

        private void ResultReport_ItemDataBound(object sender, EventArgs e)
        {
            if (sender is Telerik.Reporting.Processing.Report)
                if ((sender as Telerik.Reporting.Processing.Report).Parameters.Count > 0)
                {
                    Telerik.Reporting.Processing.Report subReport = (Telerik.Reporting.Processing.Report)sender;
                    subReport.Visible = subReport.ChildElements.Find("detail", true).Length > 0;
                }
        }

        private void SetReportParameters(Report resultReport)
        {
            foreach (var relColumn in ParentToChildSubReportDTO.SubsColumnsDTO)
            {
                //if (parentRelationship.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary)
                //    if (relColumn.FirstSideColumnID == null)
                //        continue;

                ReportParameter parameter = new ReportParameter();
                parameter.Name = relColumn.ParentEntityListViewColumnRelativeName;
                parameter.Type = ConvertParameterType(relColumn.ParentEntityListViewColumnType);
                resultReport.ReportParameters.Add(parameter);
                //اگر الیاس داشته باشند چه؟ باید از پرنت الیاسها هم در نظر گرفته شود
                var filter = new Telerik.Reporting.Filter();
                filter.Expression = string.Format("= Fields.{0}", relColumn.ChildEntityListViewColumnRelativeName);
                filter.Operator = Telerik.Reporting.FilterOperator.Equal;
                filter.Value = string.Format("= Parameters.{0}.Value", relColumn.ParentEntityListViewColumnRelativeName);
                resultReport.Filters.Add(filter);

            }
        }

        private ReportParameterType ConvertParameterType(Enum_ColumnType columnType)
        {
            if (columnType == Enum_ColumnType.Numeric)
                return ReportParameterType.Integer;
            else if (columnType == Enum_ColumnType.String)
                return ReportParameterType.String;
            else if (columnType == Enum_ColumnType.Boolean)
                return ReportParameterType.Boolean;
            else
                return ReportParameterType.String;
        }

        ObjectDataSource objectDataSource = null;
        private void SetReportDataSource(Report report)
        {
            if (objectDataSource == null)
            {
                objectDataSource = new Telerik.Reporting.ObjectDataSource();

                //   CheckSubReportRelationshipColumnExistsInSelect(ListReportReportDTO.EntityListView);
                objectDataSource.DataSource = searchRequestManager.GetDataTableBySearchDataItems(Request.Requester, ListReportReportDTO.TableDrivedEntityID, Request.SearchDataItems, SecurityMode.View, ListReportReportDTO.EntityListView, 0, 1000, Request.OrderByEntityViewColumnID, Request.SortType).Item3;
            }
            report.DataSource = objectDataSource;

            //var sqlDataSource1 = new Telerik.Reporting.SqlDataSource();
            //sqlDataSource1.ConnectionString = "Data Source=.;Initial Catalog=SampleDB;Integrated Security=True";
            //sqlDataSource1.Name = "sqlDataSource1";
            //var query = searchRequestManager.GetQuery(request.Requester, reportDTO.EntityListReport.TableDrivedEntityID, request.SearchDataItems, reportDTO.EntityListReport.EntityListView);
            //sqlDataSource1.SelectCommand = query;
            //resultReport.DataSource = sqlDataSource1;


            //           // sqlDataSource1.SelectCommand = @"SELECT        Employee_Project.EmployeeSSN, Employee_Project.ProjectNumber, Project.Name, Employee_Project.Hours
            //FROM Employee_Project INNER JOIN
            //                         Project ON Employee_Project.ProjectNumber = Project.Number";
        }

        //private void CheckSubReportRelationshipColumnExistsInSelect(EntityListViewDTO entityListView)
        //{
        //////foreach (var sub in ListReportReportDTO.EntityListReportSubs)
        //////{
        //////    var relationship = bizRelationship.GetRelationship(sub.RelationshipID);
        //////    foreach (var relColumn in relationship.RelationshipColumns)
        //////    {

        //////        if (relColumn.FirstSideColumnID != null)
        //////        {
        //////            if (!entityListView.EntityListViewAllColumns.Any(x => x.ColumnID == relColumn.FirstSideColumnID))
        //////            {
        //////                var newlistView = new EntityListViewColumnsDTO();
        //////                newlistView.ColumnID = relColumn.FirstSideColumnID.Value;
        //////                newlistView.Column = bizColumn.GetColumn(newlistView.ColumnID, true);
        //////                entityListView.EntityListViewAllColumns.Add(newlistView);
        //////            }
        //////        }
        //////    }
        //////}
        //////if (parentRelationship != null)
        //////{
        //////    foreach (var relColumn in parentRelationship.RelationshipColumns)
        //////    {
        //////        //برای ثابتها نیازی نیست اینجا..باید در زیر ریپورت فیلتر شوند
        //////        if (relColumn.SecondSideColumnID != null)
        //////        {
        //////            if (!entityListView.EntityListViewAllColumns.Any(x => x.ColumnID == relColumn.SecondSideColumnID))
        //////            {
        //////                var newlistView = new EntityListViewColumnsDTO();
        //////                newlistView.ColumnID = relColumn.SecondSideColumnID.Value;
        //////                newlistView.Column = bizColumn.GetColumn(newlistView.ColumnID, true);
        //////                entityListView.EntityListViewAllColumns.Add(newlistView);
        //////            }
        //////        }
        //////    }
        //////}
        //}

        Unit reportWidth;
        private Report GetReportDesign(DR_Requester requester)
        {
            var report = new Report();
            SetReportProperties(report);

            var listColumnSizeLocation = GetColumnsSizeLocation();

            if (ParentToChildSubReportDTO != null)
                SetReportParameters(report);
            SetGroupHeader(report, listColumnSizeLocation);
            SetReportDetails(requester, report, listColumnSizeLocation);
            SetReportHeader(report, listColumnSizeLocation);

            return report;
        }

        private List<ColumnSizeLocation> GetColumnsSizeLocation()
        {
            List<ColumnSizeLocation> result = new List<ColumnSizeLocation>();

            int widthUnits = 0;
            var columns = ListReportReportDTO.EntityListView.EntityListViewAllColumns;
            foreach (var column in columns)
            {
                widthUnits += (column.WidthUnit == 0 ? 1 : column.WidthUnit);
            }
            var width = reportWidth;
            var perUnitWidth = width.Divide(widthUnits);
            var consumedWidthUnits = 0;


            if (ListReportReportDTO.ReportGroups != null)
                foreach (var reportGroup in ListReportReportDTO.ReportGroups)
                {
                    ColumnSizeLocation columnSizeLocation = new ColumnSizeLocation();
                    columnSizeLocation.LictViewColumnID = reportGroup.ListViewColumnID;
                    SetColumnSizePosition(columnSizeLocation, reportGroup.EntityListViewColumn.WidthUnit, ref consumedWidthUnits, perUnitWidth);
                    result.Add(columnSizeLocation);

                }
            foreach (var column in columns)
            {
                if (!result.Any(x => x.LictViewColumnID == column.ID))
                {
                    ColumnSizeLocation columnSizeLocation = new ColumnSizeLocation();
                    columnSizeLocation.LictViewColumnID = column.ID;
                    SetColumnSizePosition(columnSizeLocation, column.WidthUnit, ref consumedWidthUnits, perUnitWidth);
                    result.Add(columnSizeLocation);
                }
            }
            return result;
        }
        private void SetColumnSizePosition(ColumnSizeLocation columnSizeLocation, int currentWidthUnits, ref int consumedWidthUnits, Unit perUnitWidth)
        {
            if (currentWidthUnits == 0)
                currentWidthUnits = 1;

            var width = currentWidthUnits * perUnitWidth;
            columnSizeLocation.Width = width;

            var xlocation = (reportWidth - width) - consumedWidthUnits * perUnitWidth;
            columnSizeLocation.XLocation = xlocation;


            consumedWidthUnits += currentWidthUnits;

        }
        private void SetReportDetails(DR_Requester requester, Report report, List<ColumnSizeLocation> columnsSizeLocation)
        {
            bool hasSub = ListReportReportDTO.EntityListReportSubs.Any();

            var detail = new Telerik.Reporting.DetailSection();
            detail.Height = Telerik.Reporting.Drawing.Unit.Cm(0.5);
            detail.Name = "detail";

            ReportStyles.SetReportDetailStyle(detail.Style);
            report.Items.Add(detail);


            Panel panel = new Panel();
            panel.Width = reportWidth;
            ReportStyles.SetDetailPanelStyle(panel.Style, hasSub);
            detail.Items.Add(panel);
            //سکوریتی بروی ستوها اعمال شود
            var columns = ListReportReportDTO.EntityListView.EntityListViewAllColumns;

            int index = 0;
            foreach (var column in columns)
            {
                if (ListReportReportDTO.ReportGroups != null)
                    if (ListReportReportDTO.ReportGroups.Any(x => x.ListViewColumnID == column.ID))
                        continue;
                var columnTextbox = new TextBox();
                columnTextbox.CanGrow = false;

                columnTextbox.Name = column.Column.Name;
                ReportStyles.SetDetailTextboxStyle(columnTextbox.Style);
                var columnSizeLocation = columnsSizeLocation.First(x => x.LictViewColumnID == column.ID);
                columnTextbox.Width = columnSizeLocation.Width;
                columnTextbox.Location = new PointU(columnSizeLocation.XLocation - Unit.Cm(0.0), Unit.Cm(0));
                columnTextbox.Height = detail.Height;
                //var columnName = "";
                //if (column.RelationshipTailID == 0)
                //{
                //    columnName = column.Column.Name + "0";// + "'";

                //}
                //else
                //{

                //}
                columnTextbox.Value = string.Format("= Fields.{0}", column.RelativeColumnName);
                panel.Items.Add(columnTextbox);
                index++;
            }
            int subIndex = 0;
            foreach (var subDTO in ListReportReportDTO.EntityListReportSubs)
            {
                if (CheckSubReportIsRepeated(subDTO))
                    continue;
                SubReport subReport = new SubReport();
                subReport.Width = reportWidth;
                subReport.Top = detail.Height + Unit.Cm(subIndex * 0.5);
                subIndex++;


                RR_ReportSourceRequest newrequest = new RR_ReportSourceRequest(Request.Requester);
                newrequest.Identity = Request.Identity;
                newrequest.Name = Request.Name;
                var tail = bizEntityRelationshipTail.GetEntityRelationshipTail(requester, subDTO.EntityRelationshipTailID);
                newrequest.SearchDataItems = new DP_SearchRepository(tail.TargetEntityID);

                var entityListReport = bizEntityListReport.GetEntityListReport( requester, subDTO.EntityListReportID, true);
                newrequest.ReportID = entityListReport.ID;
                Unit SubReportWidth = reportWidth - Unit.Cm(1);
                //    var relationshipTail = bizEntityRelationshipTail.GetEntityRelationshipTail(subDTO.EntityRelationshipTailID);

                ListReportResolver listReportResolver = new ListReportResolver(entityListReport, newrequest, SubReportWidth, this, subDTO, ReportLevel + 1);
                var subListReportSource = listReportResolver.GetListReport(requester);
                subReport.ReportSource = subListReportSource;
                subReport.Left = Unit.Cm(0.5);

                ReportStyles.SetSubreportStyle(subReport.Style);


                foreach (var relColumn in subDTO.SubsColumnsDTO)
                {
                    var parameter = new Parameter();
                    parameter.Name = relColumn.ParentEntityListViewColumnRelativeName;
                    parameter.Value = string.Format("= Fields.{0}", relColumn.ParentEntityListViewColumnRelativeName);
                    subReport.Parameters.Add(parameter);
                }


                detail.Items.Add(subReport);
            }


        }

        public bool CheckSubReportIsRepeated(EntityListReportSubsDTO subDTO)
        {
            if (ParentListReportResolver == null)
                return false;
            else
            {
                if (ParentListReportResolver.ListReportReportDTO.ID == subDTO.EntityListReportID)
                    return true;
                else
                    return ParentListReportResolver.CheckSubReportIsRepeated(subDTO);
            }

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

        private void SetReportHeader(Report report, List<ColumnSizeLocation> columnsSizeLocation)
        {


            var reportHeader = new ReportHeaderSection();
            reportHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(1.5);

            reportHeader.Name = "reportHeader";
            ReportStyles.SetReportHeaderStyle(reportHeader.Style, ReportLevel);
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
            titleTextBox.Value = ListReportReportDTO.ReportTitle;

            //بعدا درست شود یک تکست باکس که مقدارش برابر با string.Format("= Parameters.{0}.Value", param.Name) باشد اضافه شود 
            //  string paramHeader = "";
            //if (report.ReportParameters.Count > 0)
            //{
            //    foreach (var param in report.ReportParameters)
            //    {
            //        paramHeader += (paramHeader == "" ? "" : ",") + param.Name + "=" + string.Format("= Parameters.{0}.Value", param.Name); ;
            //    }

            //}
            //if (paramHeader != "")
            //    titleTextBox.Value += paramHeader;




            reportHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            titleTextBox});


        }

        private void SetGroupHeader(Report report, List<ColumnSizeLocation> columnsSizeLocation)
        {

            var groupHeader = new GroupHeaderSection();
            groupHeader.Height = Unit.Mm(7);
            groupHeader.PrintOnEveryPage = true;
            groupHeader.Name = "groupHeader";
            ReportStyles.SetGeoupColumnHeaderStyle(groupHeader.Style, ReportLevel);
            report.Items.Add(groupHeader);

            var columns = ListReportReportDTO.EntityListView.EntityListViewAllColumns;
            //  var columnHeight = groupHeader.Height;

            //گروه عناوین همه ستونها
            int index = 0;
            foreach (var column in columns)
            {
                var columnTextbox = new TextBox();
                columnTextbox.CanGrow = true;
                columnTextbox.Name = column.Column.Name;
                ReportStyles.SetColumnHeaderTextboxStyle(columnTextbox.Style);
                var columnSizeLocation = columnsSizeLocation.First(x => x.LictViewColumnID == column.ID);
                columnTextbox.Width = columnSizeLocation.Width;
                columnTextbox.Location = new PointU(columnSizeLocation.XLocation, Unit.Cm(0));
                columnTextbox.Height = groupHeader.Height;
                var alias = column.Alias;
                if (string.IsNullOrEmpty(alias))
                    alias = column.Column.Alias;
                columnTextbox.Value = alias;
                groupHeader.Items.Add(columnTextbox);
                index++;
            }

            var group = new Group();
            group.GroupHeader = groupHeader;
            report.Groups.Add(group);


            if (ListReportReportDTO.ReportGroups != null)
            {
                foreach (var reportGroup in ListReportReportDTO.ReportGroups)
                {
                    var repotGroupHeader = new GroupHeaderSection();
                    repotGroupHeader.Height = Unit.Mm(7);
                    //repotGroupHeader.PrintOnEveryPage = true;
                    repotGroupHeader.Name = "reportgroupHeader";
                    ReportStyles.SetGeoupHeaderStyle(repotGroupHeader.Style);
                    report.Items.Add(groupHeader);

                    var geoupHeaderTextbox = new TextBox();
                    geoupHeaderTextbox.CanGrow = true;
                    geoupHeaderTextbox.Name = reportGroup.ColumnName;
                    ReportStyles.SetGroupHeaderTextboxStyle(geoupHeaderTextbox.Style);
                    var columnSizeLocation = columnsSizeLocation.First(x => x.LictViewColumnID == reportGroup.ListViewColumnID);
                    geoupHeaderTextbox.Width = columnSizeLocation.Width;
                    geoupHeaderTextbox.Location = new PointU(columnSizeLocation.XLocation, Unit.Cm(0));
                    geoupHeaderTextbox.Height = repotGroupHeader.Height;

                    var columnName = reportGroup.EntityListViewColumn.RelativeColumnName;
                    //if (reportGroup.EntityListViewColumn.RelationshipTailID == 0)
                    //{
                    //    columnName = reportGroup.EntityListViewColumn.Column.Name + "0";// + "'";

                    //}
                    //else
                    //{

                    //}

                    geoupHeaderTextbox.Value = string.Format("= Fields.{0}", columnName);
                    repotGroupHeader.Items.Add(geoupHeaderTextbox);

                    var newGroup = new Group();
                    newGroup.GroupHeader = repotGroupHeader;
                    newGroup.Groupings.Add(new Telerik.Reporting.Grouping(string.Format("= Fields.{0}", columnName)));
                    report.Groups.Add(newGroup);
                }
            }
        }

    }

    public class ColumnSizeLocation
    {
        public int LictViewColumnID { set; get; }
        public Unit XLocation { set; get; }
        public Unit Width { set; get; }
    }
}