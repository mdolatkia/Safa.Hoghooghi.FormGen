namespace MyReportWPFViewer
{
    using MyUILibrary.EntityArea;
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Telerik.Reporting;
    using Telerik.Windows.Controls;

    public partial class MyReportViewer : UserControl, I_View_InternalReportArea
    {
        public MyReportViewer()
        {
            InitializeComponent();
        }
        public string Title
        {
            set
            {
                lblTitle.Text = value;
            }
        }

        //public event EventHandler SearchCommandRequested;

        public void SetReportSource(string reportEngine, ReportSource reportSource)
        {
            ReportViewer1.ReportEngineConnection = reportEngine;
            ReportViewer1.ReportSource = reportSource;
        }

        private void imgReferesh_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ReportViewer1.RefreshReport();
            //ReportViewer1.ReportSource = reportSource as ReportSource;
        }

        //private void imgSearch_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (SearchCommandRequested != null)
        //        SearchCommandRequested(this, null);
        //}

        public void SetReportSource(ReportSource reportSource)
        {
            //////ReportViewer1.ReportSource = reportSource;

            //ReportViewer1.ReportEngineConnection = "engine=RestService;uri=http://localhost:25667/api/myreport;useDefaultCredentials=False";
            //ReportViewer1.ReportSource = new UriReportSource() { Uri = "aa" };
        }
    }
}