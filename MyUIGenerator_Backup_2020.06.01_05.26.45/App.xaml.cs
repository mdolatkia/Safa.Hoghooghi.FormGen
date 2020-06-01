
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Telerik.Reporting;
using Telerik.Windows.Controls;

namespace MyUIGenerator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //////    var rs = new UriReportSource() { Uri = "aa" };

            //////    var frm = new UI.View.frmReportViewer1("engine=RestService;uri=http://localhost:25667/api/myreport;useDefaultCredentials=False", rs);
            ////////    var frm = new UI.View.frmReportViewer1("engine=RestService;uri=  http://localhost:58083/api/reports;useDefaultCredentials=False", rs);
            //////  frm.ShowDialog();

            //RadWindow window = new RadWindow();
            //var frm = new frmTest(new CommonDefinitions.BasicUISettings.GridSetting());
            //window.Content = frm;
            //window.Show();
            //ReportViewerWindow2 window = new ReportViewerWindow2();
            //window.ShowDialog();
            //frmTest tt = new frmTest();
            //tt.ShowDialog();
            //return;
            var UIManager = UIManagerGenerator.GetUIManager();
            UIManager.StartApp();


        }
    }
}
