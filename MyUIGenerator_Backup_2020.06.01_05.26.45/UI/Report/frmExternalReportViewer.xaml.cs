
using ModelEntites;
using MyUIGenerator.UIControlHelper;
using MyUILibrary;
using MyUILibrary.EntityArea;
using MyUILibrary.EntityArea.Commands;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Reporting;
using Telerik.Windows.Controls;

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for UC_ViewPackageArea.xaml
    /// </summary>
    public partial class frmExternalReportViewer : UserControl, I_View_ExternalReportArea
    {
        //درست شود مثل بقیه ارث بری

        public frmExternalReportViewer()
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

        public string GetSortText
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int GetOrderColumnID
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool OrderColumnsVisibility
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler SearchCommandRequested;
        public event EventHandler OrderColumnsChanged;

        public void SetReportSource(string URL)
        {
            System.Diagnostics.Process.Start(URL);
            //URL = "http://www.google.com";
            //var uri = new Uri(URL);
       
            //browser.Navigate(uri);
        }

        private void imgSearch_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SearchCommandRequested != null)
                SearchCommandRequested(this, null);
        }

        private void imgReferesh_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        public void SetOrderSorts(List<string> list)
        {
            throw new NotImplementedException();
        }

        public void SetOrderColumns(List<Tuple<int, string>> columns)
        {
            throw new NotImplementedException();
        }

        //public void AddGenerealSearchAreaView(object view)
        //{
        //    grdSearch.Children.Add(view as UIElement);
        //}

        //public void SetReportSource(ReportSource reportSource)
        //{
        //    ReportViewer1.ReportSource = reportSource;

        //    //ReportViewer1.ReportEngineConnection = "engine=RestService;uri=http://localhost:25667/api/myreport;useDefaultCredentials=False";
        //    //ReportViewer1.ReportSource = new UriReportSource() { Uri = "aa" };
        //}
    }
}
