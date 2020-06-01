namespace MyUIGenerator.UI.View
{
    using MyReportManager;
    using ProxyLibrary;
    using System;
    using System.Windows;
    using Telerik.Reporting;
    using Telerik.Windows.Controls;

    public partial class frmReportViewer1 : Window
    {
        public frmReportViewer1()
        {
            //var clientReportSource = new Telerik.ReportViewer.Wpf.WebForms.ReportSource();
            InitializeComponent();
            //ReportViewer1.ReportEngineConnection = "engine=RestService;uri=http://localhost:25667/api/myreport;useDefaultCredentials=False";
            //ReportViewer1.ReportSource = new UriReportSource() { Uri = "aa" };
            //var request = new RR_ReportSourceRequest();
            //request.ReportID = 77;
            ////ReportViewer1.Error += ReportViewer1_Error;
            //request.SearchDataItems = new DP_SearchRepository() { TargetEntityID = 35 };
            //ReportResolver reportResolver = new ReportResolver();
            //var rpSource = reportResolver.GetReportSource(request);
            //ReportViewer1.ReportSource = rpSource;
              
        }

       

        public frmReportViewer1(string reportEngine, ReportSource reportSource)
        {
            //var clientReportSource = new Telerik.ReportViewer.Wpf.WebForms.ReportSource();
            InitializeComponent();

            ReportViewer1.ReportEngineConnection = reportEngine;
            ReportViewer1.ReportSource = reportSource;
        }

        private void ReportViewer1_Error(object sender, ErrorEventArgs eventArgs)
        {
          
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            //ReportViewer1.RefreshReport();
        }
    }
}