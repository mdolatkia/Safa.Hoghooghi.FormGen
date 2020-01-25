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
    public class CrosstabReportResolver
    {
        //List<ReportGroupDTO> ReportGroups { set; get; }
        public CrosstabReportResolver(DR_Requester requester, int reportID, RR_ReportSourceRequest request, Unit initialReportWidth)
        {
            CrosstabReportReportDTO = bizEntityCrosstabReport.GetEntityCrosstabReport(requester, reportID, true);
            //ReportGroups = groups;
            reportWidth = initialReportWidth;
            Request = request;

        }

        SearchRequestManager searchRequestManager = new SearchRequestManager();
        EntityCrosstabReportDTO CrosstabReportReportDTO { set; get; }
        BizEntityCrosstabReport bizEntityCrosstabReport = new BizEntityCrosstabReport();
        BizColumn bizColumn = new BizColumn();
        BizEntityReport bizEntityReport = new BizEntityReport();
        BizRelationship bizRelationship = new BizRelationship();
        //RelationshipDTO parentRelationship { set; get; }
        //int ReportLevel { set; get; }
        RR_ReportSourceRequest Request { set; get; }
        //public ReportSource GetCrosstabReport(int reportID, RR_ReportSourceRequest request)
        //{
        //    ReportDTO = reportDTO;
        //    Request = request;
        //    var resultReport = GetReportDesign();
        //    SetReportDataSource(resultReport);

        //    return new Telerik.Reporting.InstanceReportSource { ReportDocument = resultReport };
        //}

        public ReportSource GetCrosstabReport()
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
        private void SetReportDataSource(Crosstab report)
        {
            if (objectDataSource == null)
            {
                objectDataSource = new Telerik.Reporting.ObjectDataSource();
                //   EntityListViewDTO listView = CreateListView();
                //CheckSubReportRelationshipColumnExistsInSelect(CrosstabReportReportDTO.EntityListView);
                objectDataSource.DataSource = searchRequestManager.GetDataTableBySearchDataItems(Request.Requester, CrosstabReportReportDTO.TableDrivedEntityID, Request.SearchDataItems, SecurityMode.View, CrosstabReportReportDTO.EntityListView, 0);

            }
            report.DataSource = objectDataSource;

            //var sqlDataSource1 = new Telerik.Reporting.SqlDataSource();
            //sqlDataSource1.ConnectionString = "Data Source=.;Initial Catalog=SampleDB;Integrated Security=True";
            //sqlDataSource1.Name = "sqlDataSource1";
            //var query = searchRequestManager.GetQuery(request.Requester, reportDTO.EntityCrosstabReport.TableDrivedEntityID, request.SearchDataItems, reportDTO.EntityCrosstabReport.EntityListView);
            //sqlDataSource1.SelectCommand = query;
            //resultReport.DataSource = sqlDataSource1;


            //           // sqlDataSource1.SelectCommand = @"SELECT        Employee_Project.EmployeeSSN, Employee_Project.ProjectNumber, Project.Name, Employee_Project.Hours
            //FROM Employee_Project INNER JOIN
            //                         Project ON Employee_Project.ProjectNumber = Project.Number";
        }

        //private EntityListViewDTO CreateListView()
        //{
        //    EntityListViewDTO result = new EntityListViewDTO();
        //    foreach (var item in CrosstabReportReportDTO.Columns)
        //    {
        //        var column = new EntityListViewColumnsDTO();
        //        column.ColumnID = item.ColumnID;
        //        column.Column = bizColumn.GetColumn(item.ColumnID, true);
        //        result.EntityListViewAllColumns.Add(column);
        //    }
        //    foreach (var item in CrosstabReportReportDTO.Rows)
        //    {
        //        if (!result.EntityListViewAllColumns.Any(x => x.ColumnID == item.ColumnID))
        //        {
        //            var column = new EntityListViewColumnsDTO();
        //            column.ColumnID = item.ColumnID;
        //            column.Column = bizColumn.GetColumn(item.ColumnID, true);
        //            result.EntityListViewAllColumns.Add(column);
        //        }
        //    }
        //    foreach (var item in CrosstabReportReportDTO.Values)
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
        //    foreach (var sub in CrosstabReportReportDTO.EntityCrosstabReportSubs)
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
        private Tuple<Report, Crosstab> GetReportDesign()
        {
            var report = new Report();
            SetReportProperties(report);
            SetReportHeader(report);
            return SetReportDetails(report);
        }

        private Tuple<Report, Crosstab> SetReportDetails(Report report)
        {
            var detail = new Telerik.Reporting.DetailSection();
            detail.Name = "detail";
            ReportStyles.SetReportDetailStyle(detail.Style);
            report.Items.Add(detail);

            var columnWidth = Unit.Cm(2);
            var columnHeight = Unit.Cm(0.7);
            var crosstab = new Crosstab();
            crosstab.Width = reportWidth;
            crosstab.Name = "crosstab";
            // crosstab.Height = report.PageSettings.PaperSize.Height / 2;
            detail.Items.Add(crosstab);



            for (int c = 0; c <= CrosstabReportReportDTO.Columns.Count - 1; c++)
            {
                foreach (var val in CrosstabReportReportDTO.Values)
                {
                    crosstab.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(2)));
                }
            }
            foreach (var val in CrosstabReportReportDTO.Values)
            {
                crosstab.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(2)));
            }
            for (int r = 0; r <= CrosstabReportReportDTO.Rows.Count; r++)
            {
                crosstab.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(1)));
            }
            for (int c = 0; c <= crosstab.Body.Columns.Count - 1; c++)
            {
                for (int r = 0; r <= crosstab.Body.Rows.Count - 1; r++)
                {
                    var mod = c % CrosstabReportReportDTO.Values.Count;
                    Telerik.Reporting.TextBox textbox = new TextBox();
                    var val = CrosstabReportReportDTO.Values[mod];
                    textbox.Name = val.EntityListViewColumn.Alias + c.ToString() + r.ToString();
                    textbox.Width = columnWidth;
                    textbox.Height = columnHeight;
                    textbox.Value = string.Format("= {0}(Fields.{1})", val.ValueFunction.ToString(), val.EntityListViewColumn.RelativeColumnName);
                    crosstab.Items.Add(textbox);
                    crosstab.Body.SetCellContent(r, c, textbox);
                    if (r == crosstab.Body.Rows.Count - 1)
                    {
                        ReportStyles.SetCrossTabRowHeader(textbox.Style);

                    }
                    else
                    {
                        if (c >= crosstab.Body.Columns.Count - CrosstabReportReportDTO.Values.Count)
                        {
                            ReportStyles.SetCrossTabRowHeader(textbox.Style);
                        }
                        else
                        {
                            if (c > CrosstabReportReportDTO.Values.Count - 1)
                            {
                                ReportStyles.SetCrossTabTotalRowColumnStyle(textbox.Style);
                            }
                            else
                            {
                                if (r > 0)
                                {
                                    ReportStyles.SetCrossTabTotalRowColumnStyle(textbox.Style);
                                }
                                else
                                    ReportStyles.SetCrossTabBodyTextboxStyle(textbox.Style);
                            }
                        }
                    }
                }
            }


            if (CrosstabReportReportDTO.Values.Count == 1)
            {
                for (int r = 0; r <= CrosstabReportReportDTO.Rows.Count - 1; r++)
                {
                    var textboxCorner = new TextBox();
                    textboxCorner.Name = "corner" + r;
                    textboxCorner.Width = columnWidth;
                    textboxCorner.Height = columnHeight;
                    textboxCorner.Value = CrosstabReportReportDTO.Rows[r].EntityListViewColumn.Alias;
                    ReportStyles.SetCrossTabRowHeader(textboxCorner.Style);
                    var rowspan = CrosstabReportReportDTO.Columns.Count;
                    crosstab.Items.Add(textboxCorner);
                    crosstab.Corner.SetCellContent(0, r, textboxCorner, rowspan, 1);
                }
            }
            else
            {
                var emptytextboxCorner = new TextBox();
                emptytextboxCorner.Name = "corner";
                emptytextboxCorner.Width = columnWidth;
                emptytextboxCorner.Height = columnHeight;
                emptytextboxCorner.Value = "";
                ReportStyles.SetCrossTabEmptyCornerTextboxStyle(emptytextboxCorner.Style);
                crosstab.Items.Add(emptytextboxCorner);
                crosstab.Corner.SetCellContent(0, 0, emptytextboxCorner, CrosstabReportReportDTO.Columns.Count, CrosstabReportReportDTO.Rows.Count);

                for (int r = 0; r <= CrosstabReportReportDTO.Rows.Count - 1; r++)
                {
                    var textboxCorner = new TextBox();
                    textboxCorner.Name = "corner" + r;
                    textboxCorner.Width = columnWidth;
                    textboxCorner.Height = columnHeight;
                    textboxCorner.Value = CrosstabReportReportDTO.Rows[r].EntityListViewColumn.Alias;
                    ReportStyles.SetCrossTabRowHeader(textboxCorner.Style);
                    crosstab.Items.Add(textboxCorner);
                    crosstab.Corner.SetCellContent(CrosstabReportReportDTO.Columns.Count, r, textboxCorner);

                }
            }











            TableGroup prevDataTableGroup = null;
            TableGroup prevTotalTableGroup = null;
            foreach (var item in CrosstabReportReportDTO.Columns.OrderByDescending(x => x.ID))
            {

                var newDataTableGroup = new TableGroup();
                newDataTableGroup.Name = "col" + item.EntityListViewColumn.Alias;
                newDataTableGroup.Groupings.Add(new Telerik.Reporting.Grouping(string.Format("= Fields.{0}", item.EntityListViewColumn.RelativeColumnName)));
                newDataTableGroup.Sortings.Add(new Telerik.Reporting.Sorting(string.Format("= Fields.{0}", item.EntityListViewColumn.RelativeColumnName), Telerik.Reporting.SortDirection.Asc));
                var textbox = new TextBox();
                textbox.Name = "colt" + item.EntityListViewColumn.Alias;
                textbox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.800000011920929D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
                textbox.Value = string.Format("= Fields.{0}", item.EntityListViewColumn.RelativeColumnName);
                ReportStyles.SetCrossTabRowColumnStyle(textbox.Style);
                crosstab.Items.Add(textbox);
                newDataTableGroup.ReportItem = textbox;

                var newTotalTableGroup = new TableGroup();
                newTotalTableGroup.Name = "total_col_" + item.EntityListViewColumn.Alias;
                var totaltextbox = new TextBox();
                totaltextbox.Name = "coltotal" + item.EntityListViewColumn.Alias;
                totaltextbox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.800000011920929D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
                totaltextbox.Value = "جمع";
                ReportStyles.SetCrossTabRowHeader(totaltextbox.Style);
                crosstab.Items.Add(totaltextbox);
                newTotalTableGroup.ReportItem = totaltextbox;



                if (prevDataTableGroup != null)
                {
                    newDataTableGroup.ChildGroups.Add(prevDataTableGroup);
                    newDataTableGroup.ChildGroups.Add(prevTotalTableGroup);
                }
                else if (CrosstabReportReportDTO.Values.Count > 1)
                {
                    //اولین گروهبندی..جزئی ترین..اگر مقادیر بیشتر از یکی باشند یک زیر گروه جدید برای هر مقدار ایجاد میشود
                    var index = 0;
                    foreach (var val in CrosstabReportReportDTO.Values)
                    {
                        var lastDataTableGroup = new TableGroup();
                        lastDataTableGroup.Name = "last" + val.EntityListViewColumn.Alias + index;
                        var lasttextbox = new TextBox();
                        lasttextbox.Name = "lastTextbox" + val.EntityListViewColumn.Alias + index;
                        lasttextbox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.800000011920929D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
                        lasttextbox.Value = val.EntityListViewColumn.Alias;
                        ReportStyles.SetCrossTabRowHeader(lasttextbox.Style);
                        crosstab.Items.Add(lasttextbox);
                        lastDataTableGroup.ReportItem = lasttextbox;
                        newDataTableGroup.ChildGroups.Add(lastDataTableGroup);
                        index++;
                    }
                }



                if (prevDataTableGroup != null)
                {

                    var index = 0;
                    foreach (var val in CrosstabReportReportDTO.Values)
                    {
                        var lastTotalDataTableGroup = new TableGroup();
                        lastTotalDataTableGroup.Name = "lasttotal" + item.EntityListViewColumn.Alias + val.EntityListViewColumn.Alias + index;
                        var lasttotaltextbox = new TextBox();
                        lasttotaltextbox.Name = "lasttotalTextbox" + item.EntityListViewColumn.Alias + val.EntityListViewColumn.Alias + index;
                        lasttotaltextbox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.800000011920929D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
                        lasttotaltextbox.Value = val.EntityListViewColumn.Alias;
                        ReportStyles.SetCrossTabRowHeader(lasttotaltextbox.Style);
                        crosstab.Items.Add(lasttotaltextbox);
                        lastTotalDataTableGroup.ReportItem = lasttotaltextbox;
                        newTotalTableGroup.ChildGroups.Add(lastTotalDataTableGroup);
                        index++;
                    }
                }
                else if (CrosstabReportReportDTO.Values.Count > 1)
                {
                    //اولین گروهبندی تجمیعی..جزئی ترین..اگر مقادیر بیشتر از یکی باشند یک زیر گروه جدید برای هر مقدار ایجاد میشود
                    var index = 0;
                    foreach (var val in CrosstabReportReportDTO.Values)
                    {
                        var smallestGroup = new TableGroup();
                        smallestGroup.Name = "lasttotal" + item.EntityListViewColumn.Alias + val.EntityListViewColumn.Alias + index;
                        var lasttotaltextbox = new TextBox();
                        lasttotaltextbox.Name = "lasttotalTextbox" + item.EntityListViewColumn.Alias + val.EntityListViewColumn.Alias + index;
                        lasttotaltextbox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.800000011920929D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
                        lasttotaltextbox.Value = val.EntityListViewColumn.Alias;
                        ReportStyles.SetCrossTabRowHeader(lasttotaltextbox.Style);
                        crosstab.Items.Add(lasttotaltextbox);
                        smallestGroup.ReportItem = lasttotaltextbox;
                        newTotalTableGroup.ChildGroups.Add(smallestGroup);


                        index++;

                    }
                }





                prevDataTableGroup = newDataTableGroup;
                prevTotalTableGroup = newTotalTableGroup;

            }
            crosstab.ColumnGroups.Add(prevDataTableGroup);
            crosstab.ColumnGroups.Add(prevTotalTableGroup);


            TableGroup prevRowDataTableGroup = null;
            TableGroup prevRowTotalTableGroup = null;
            foreach (var item in CrosstabReportReportDTO.Rows.OrderByDescending(x => x.ID))
            {

                var newDataTableGroup = new TableGroup();
                newDataTableGroup.Name = "row" + item.EntityListViewColumn.Alias;
                if (prevRowDataTableGroup != null)
                {
                    newDataTableGroup.ChildGroups.Add(prevRowDataTableGroup);
                    newDataTableGroup.ChildGroups.Add(prevRowTotalTableGroup);
                }
                var textbox = new TextBox();
                textbox.Value = string.Format("= Fields.{0}", item.EntityListViewColumn.RelativeColumnName);
                textbox.Name = "rowt" + item.EntityListViewColumn.Alias;
                textbox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.800000011920929D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
                crosstab.Items.Add(textbox);
                ReportStyles.SetCrossTabRowColumnStyle(textbox.Style);
                newDataTableGroup.ReportItem = textbox;
                newDataTableGroup.Groupings.Add(new Telerik.Reporting.Grouping(string.Format("= Fields.{0}", item.EntityListViewColumn.RelativeColumnName)));
                newDataTableGroup.Sortings.Add(new Telerik.Reporting.Sorting(string.Format("= Fields.{0}", item.EntityListViewColumn.RelativeColumnName), Telerik.Reporting.SortDirection.Asc));


                var newTotalTableGroup = new TableGroup();
                newTotalTableGroup.Name = "total_row_" + item.EntityListViewColumn.Alias;
                var totaltextbox = new TextBox();
                totaltextbox.Name = "totalrow" + item.EntityListViewColumn.Alias;
                totaltextbox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.800000011920929D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
                totaltextbox.Value = "جمع";
                ReportStyles.SetCrossTabRowHeader(totaltextbox.Style);
                crosstab.Items.Add(totaltextbox);
                newTotalTableGroup.ReportItem = totaltextbox;



                prevRowDataTableGroup = newDataTableGroup;
                prevRowTotalTableGroup = newTotalTableGroup;

            }
            crosstab.RowGroups.Add(prevRowDataTableGroup);
            crosstab.RowGroups.Add(prevRowTotalTableGroup);
            return new Tuple<Report, Crosstab>(report, crosstab);






        }

        private TableGroup SetColumn(Crosstab crosstab, CrosstabReportValueDTO val)
        {
            TableGroup tableGroup = new TableGroup();
            var textbox = new TextBox();
            textbox.Value = val.EntityListViewColumn.Alias;
            tableGroup.ReportItem = textbox;
            crosstab.Items.Add(textbox);
            return tableGroup;
        }
        private TableGroup SetTotalColumn(Crosstab crosstab, CrosstabReportValueDTO val, CrosstabReportColumnDTO item1, int colindex, CrosstabReportColumnDTO nextColumn, CrosstabReportColumnDTO prevColumn)
        {
            TableGroup tableGroup = new TableGroup();
            var textbox = new TextBox();
            if (prevColumn == null)
                textbox.Value = val.EntityListViewColumn.Alias;
            tableGroup.ReportItem = textbox;
            crosstab.Items.Add(textbox);
            return tableGroup;
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
            ReportStyles.SetCrosstabReportHeaderStyle(reportHeader.Style);
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
            titleTextBox.Value = CrosstabReportReportDTO.ReportTitle;
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

        //    var columns = CrosstabReportReportDTO.EntityListView.EntityListViewAllColumns.Where(x => x.RelationshipTailID == 0);
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
        //            geoupHeaderTextbox.Name = reportGroup.ColumnName;
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