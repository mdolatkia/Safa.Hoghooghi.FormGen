
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
    public partial class frmReportViewer : UserControl, I_View_InternalReportArea
    {
        //درست شود مثل بقیه ارث بری
        public event EventHandler OrderColumnsChanged;
        public event EventHandler<Exception> ExceptionThrown;

        public frmReportViewer()
        {
            InitializeComponent();
            ReportViewer1.InteractiveActionEnter += ReportViewer1_InteractiveActionEnter;
            ReportViewer1.InteractiveActionExecuting += ReportViewer1_InteractiveActionExecuting;
            ReportViewer1.InteractiveActionLeave += ReportViewer1_InteractiveActionLeave;
            ReportViewer1.Error += ReportViewer1_Error;
            cmbOrderColumns.SelectionChanged += CmbOrderColumns_SelectionChanged;
            cmbSort.SelectionChanged += CmbSort_SelectionChanged;
            
        }
        public int GetOrderColumnID
        {
            get
            {
                if (cmbOrderColumns.SelectedItem != null && cmbOrderColumns.SelectedValue != null)
                    return (int)cmbOrderColumns.SelectedValue;
                return 0;
            }
        }
        public string GetSortText
        {
            get
            {
                if (cmbSort.SelectedItem != null)
                    return cmbSort.SelectedItem.ToString();
                return "";
            }
        }
        private void CmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckOrderChange();
        }

        private void CmbOrderColumns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckOrderChange();
        }

        private void CheckOrderChange()
        {
            if (cmbOrderColumns.SelectedItem != null && cmbOrderColumns.SelectedValue != null)
            {
                if (cmbSort.SelectedItem != null)
                {
                    if (OrderColumnsChanged != null)
                        OrderColumnsChanged(this, null);
                }
            }
        }

        private void ReportViewer1_InteractiveActionLeave(object sender, Telerik.ReportViewer.Wpf.InteractiveActionEventArgs args)
        {
            throw new NotImplementedException();
        }

        private void ReportViewer1_InteractiveActionExecuting(object sender, Telerik.ReportViewer.Wpf.InteractiveActionCancelEventArgs args)
        {
            throw new NotImplementedException();
        }

        private void ReportViewer1_InteractiveActionEnter(object sender, Telerik.ReportViewer.Wpf.InteractiveActionEventArgs args)
        {
            throw new NotImplementedException();
        }

        private void ReportViewer1_Error(object sender, ErrorEventArgs eventArgs)
        {
            if (ExceptionThrown != null)
            {
                eventArgs.Cancel = true;
                ExceptionThrown(this, eventArgs.Exception);
              
            }
            
        }
        public string Title
        {
            set
            {
                lblTitle.Text = value;
            }
        }

        public bool OrderColumnsVisibility
        {
            set
            {
                lblOrderColumns.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                cmbOrderColumns.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                cmbSort.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
            get
            {
                return cmbOrderColumns.Visibility == Visibility.Visible;
            }
        }

        public void SetOrderColumns(List<Tuple<int, string>> columns)
        {


            cmbOrderColumns.SelectedValuePath = "Item1";
            cmbOrderColumns.DisplayMemberPath = "Item2";
            cmbOrderColumns.ItemsSource = columns;
        }

        public void SetOrderSorts(List<string> list)
        {
            cmbSort.ItemsSource = list;
            cmbSort.SelectedItem = list.FirstOrDefault();
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
            ReportViewer1.ReportSource = reportSource;

            //ReportViewer1.ReportEngineConnection = "engine=RestService;uri=http://localhost:25667/api/myreport;useDefaultCredentials=False";
            //ReportViewer1.ReportSource = new UriReportSource() { Uri = "aa" };
        }

        //public void AddGenerealSearchAreaView(object view)
        //{
        //    grdSearch.Children.Add(view as UIElement);
        //}
    }
}
