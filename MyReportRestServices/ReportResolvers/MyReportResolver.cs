

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Reporting;
using Telerik.Reporting.Services.Engine;

using MyReportManager;
using System.Xml.Serialization;
using System.IO;
using ProxyLibrary;

namespace MyReportRestServices.ReportResolvers
{
    public class MyReportResolver : IReportResolver
    {
        //BizEntityReport bizEntityReport = new BizEntityReport();
        //public MyReportResolver()
        //{

        //}
        ReportResolver reportResolver = new ReportResolver();
        //private Telerik.Reporting.SqlDataSource sqlDataSource1;
        //private Telerik.Reporting.ReportHeaderSection reportHeader;
        //private Telerik.Reporting.TextBox titleTextBox;
        //private Telerik.Reporting.DetailSection detail;
        //private Telerik.Reporting.TextBox nameDataTextBox;
        //private Telerik.Reporting.TextBox projectNumberDataTextBox;
        //private Telerik.Reporting.TextBox hoursDataTextBox;

        //private Telerik.Reporting.SqlDataSource sqlDataSource1;
        //private Telerik.Reporting.ReportHeaderSection reportHeader;
        //private Telerik.Reporting.TextBox titleTextBox;
        //private Telerik.Reporting.DetailSection detail;
        //private Telerik.Reporting.TextBox nameDataTextBox;
        //private Telerik.Reporting.TextBox projectNumberDataTextBox;
        //private Telerik.Reporting.TextBox hoursDataTextBox;
        public ReportSource Resolve(string report)
        {
         
            //if the report is in XML format, you need to implement a logic to deserialize the report



            //Telerik.Reporting.Report reportObject;
            //if (report is Telerik.Reporting.Report)
            //    reportObject = report;
            //else
            //    reportObject = Deserialize(report);

            ////get the information you need and pass it in the report
            //var userId = GetUserFromCookie();
            //reportObject.ReportParameters["UserId"].Value = userId;


            //////            var newreport = new Telerik.Reporting.Report();
            //////            this.sqlDataSource1 = new Telerik.Reporting.SqlDataSource();
            //////            this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
            //////            this.titleTextBox = new Telerik.Reporting.TextBox();
            //////            this.detail = new Telerik.Reporting.DetailSection();
            //////            this.nameDataTextBox = new Telerik.Reporting.TextBox();
            //////            this.projectNumberDataTextBox = new Telerik.Reporting.TextBox();
            //////            this.hoursDataTextBox = new Telerik.Reporting.TextBox();

            //////            this.sqlDataSource1.ConnectionString = "Data Source=.;Initial Catalog=SampleDB;Integrated Security=True";
            //////            this.sqlDataSource1.Name = "sqlDataSource1";
            //////            this.sqlDataSource1.SelectCommand = @"SELECT        Employee_Project.EmployeeSSN, Employee_Project.ProjectNumber, Project.Name, Employee_Project.Hours
            //////FROM Employee_Project INNER JOIN
            //////                         Project ON Employee_Project.ProjectNumber = Project.Number";
            //////            // 
            //////            // reportHeader
            //////            // 
            //////            this.reportHeader.Height = Telerik.Reporting.Drawing.Unit.Inch(0.20000001788139343D);
            //////            this.reportHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            //////            this.titleTextBox});
            //////            this.reportHeader.Name = "reportHeader";
            //////            // 
            //////            // titleTextBox
            //////            // 
            //////            this.titleTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            //////            this.titleTextBox.Name = "titleTextBox";
            //////            this.titleTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(5.9999604225158691D), Telerik.Reporting.Drawing.Unit.Inch(0.20000001788139343D));
            //////            this.titleTextBox.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            //////            this.titleTextBox.Style.Font.Name = "B Yekan";
            //////            this.titleTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            //////            this.titleTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            //////            this.titleTextBox.StyleName = "Title";
            //////            this.titleTextBox.Value = "پروژه ها";
            //////            // 
            //////            // detail
            //////            // 
            //////            this.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(0.22083334624767304D);
            //////            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            //////            this.nameDataTextBox,
            //////            this.projectNumberDataTextBox,
            //////            this.hoursDataTextBox});
            //////            this.detail.Name = "detail";
            //////            // 
            //////            // nameDataTextBox
            //////            // 
            //////            this.nameDataTextBox.CanGrow = true;
            //////            this.nameDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            //////            this.nameDataTextBox.Name = "nameDataTextBox";
            //////            this.nameDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.125D), Telerik.Reporting.Drawing.Unit.Inch(0.22083334624767304D));
            //////            this.nameDataTextBox.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            //////            this.nameDataTextBox.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            //////            this.nameDataTextBox.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            //////            this.nameDataTextBox.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            //////            this.nameDataTextBox.Style.Font.Name = "B Yekan";
            //////            this.nameDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            //////            this.nameDataTextBox.StyleName = "Data";
            //////            this.nameDataTextBox.Value = "= Fields.Name";
            //////            // 
            //////            // projectNumberDataTextBox
            //////            // 
            //////            this.projectNumberDataTextBox.CanGrow = true;
            //////            this.projectNumberDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.1250789165496826D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            //////            this.projectNumberDataTextBox.Name = "projectNumberDataTextBox";
            //////            this.projectNumberDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.14575457572937D), Telerik.Reporting.Drawing.Unit.Inch(0.22083334624767304D));
            //////            this.projectNumberDataTextBox.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            //////            this.projectNumberDataTextBox.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            //////            this.projectNumberDataTextBox.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            //////            this.projectNumberDataTextBox.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            //////            this.projectNumberDataTextBox.Style.Font.Name = "B Yekan";
            //////            this.projectNumberDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            //////            this.projectNumberDataTextBox.StyleName = "Data";
            //////            this.projectNumberDataTextBox.Value = "= Fields.ProjectNumber";
            //////            // 
            //////            // hoursDataTextBox
            //////            // 
            //////            this.hoursDataTextBox.CanGrow = true;
            //////            this.hoursDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.2709121704101562D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            //////            this.hoursDataTextBox.Name = "hoursDataTextBox";
            //////            this.hoursDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7290483713150024D), Telerik.Reporting.Drawing.Unit.Inch(0.22083334624767304D));
            //////            this.hoursDataTextBox.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            //////            this.hoursDataTextBox.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            //////            this.hoursDataTextBox.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            //////            this.hoursDataTextBox.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            //////            this.hoursDataTextBox.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            //////            this.hoursDataTextBox.Style.Font.Name = "B Yekan";
            //////            this.hoursDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            //////            this.hoursDataTextBox.StyleName = "Data";
            //////            this.hoursDataTextBox.Value = "= Fields.Hours";
            //////            // 
            //////            // Report2
            //////            // 
            //////            newreport.DataSource = this.sqlDataSource1;
            //////            //newreport.Filters.Add(new Telerik.Reporting.Filter("= Fields.EmployeeSSN", Telerik.Reporting.FilterOperator.Equal, "= Parameters.SSN.Value"));
            //////            newreport.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            //////            this.reportHeader,this.detail });
            //////            newreport.Name = "Report2";
            //////            newreport.PageSettings.ContinuousPaper = false;
            //////            newreport.PageSettings.Landscape = false;
            //////            newreport.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(5D), Telerik.Reporting.Drawing.Unit.Mm(5D), Telerik.Reporting.Drawing.Unit.Mm(5D), Telerik.Reporting.Drawing.Unit.Mm(5D));
            //////            newreport.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            //////            //reportParameter1.Name = "SSN";
            //////            //reportParameter1.Type = Telerik.Reporting.ReportParameterType.Integer;
            //////            //reportParameter1.Value = "0";
            //////            //newreport.ReportParameters.Add(reportParameter1);
            //////            newreport.Style.BackgroundColor = System.Drawing.Color.Empty;
            //////            newreport.Style.BorderColor.Default = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(67)))), ((int)(((byte)(113)))));
            //////            newreport.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            //////            newreport.Style.Font.Name = "B Yekan";
            //////            //styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            //////            //new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            //////            //new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            //////            //styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            //////            //styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            //////            //styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            //////            //new Telerik.Reporting.Drawing.StyleSelector("Title")});
            //////            //styleRule2.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(103)))), ((int)(((byte)(109)))));
            //////            //styleRule2.Style.Font.Name = "Book Antiqua";
            //////            //styleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(18D);
            //////            //styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            //////            //new Telerik.Reporting.Drawing.StyleSelector("Caption")});
            //////            //styleRule3.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(103)))), ((int)(((byte)(109)))));
            //////            //styleRule3.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(185)))), ((int)(((byte)(102)))));
            //////            //styleRule3.Style.Font.Name = "Book Antiqua";
            //////            //styleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            //////            //styleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            //////            //styleRule4.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            //////            //new Telerik.Reporting.Drawing.StyleSelector("Data")});
            //////            //styleRule4.Style.Font.Name = "Book Antiqua";
            //////            //styleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            //////            //styleRule4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            //////            //styleRule5.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            //////            //new Telerik.Reporting.Drawing.StyleSelector("PageInfo")});
            //////            //styleRule5.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            //////            //styleRule5.Style.Font.Name = "Book Antiqua";
            //////            //styleRule5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            //////            //styleRule5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            //////            //newreport.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            //////            //styleRule1,
            //////            //styleRule2,
            //////            //styleRule3,
            //////            //styleRule4,
            //////            //styleRule5});
            //////            newreport.Width = Telerik.Reporting.Drawing.Unit.Inch(6D);

            //////            return new Telerik.Reporting.InstanceReportSource { ReportDocument = newreport };
            List<Type> types = new List<Type>();
            types.Add(typeof(DP_SearchRepository));
            types.Add(typeof(LogicPhrase));
            types.Add(typeof(SearchProperty));

            RR_ReportSourceRequest request = Deserialize<RR_ReportSourceRequest>(report, types);

            return reportResolver.GetReportSource(request.Requester, request);
            //  return new ListReportResolver().GetListReport(null, null);
            //    return null;

        }
        public T Deserialize<T>(string toDeserialize, List<Type> types)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), types.ToArray());
            StringReader textReader = new StringReader(toDeserialize);
            return (T)xmlSerializer.Deserialize(textReader);
        }

        //private RR_ReportSourceRequest ConvertToReportSourceRequest(string strRequest)
        //{
        //    var request = new RR_ReportSourceRequest();
        //    request.ReportID = 76;
        //    request.SearchDataItems=new DP_SearchRepository() { TargetEntityID = 35 };
        //    return request;
        //}
    }
}