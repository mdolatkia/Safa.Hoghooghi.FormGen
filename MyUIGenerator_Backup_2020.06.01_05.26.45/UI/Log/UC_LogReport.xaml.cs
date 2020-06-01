using MyUILibraryInterfaces.LogReportArea;
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
using ModelEntites;
using MyCommonWPFControls;
using ProxyLibrary;
using Telerik.Windows.Controls.GridView;

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for UC_LogReport.xaml
    /// </summary>
    public partial class UC_LogReport : UserControl, I_View_LogReportArea
    {
        public DateTime? FromData
        {
            get
            {
                return txtFromData.SelectedDate;
            }

            set
            {
                txtFromData.SelectedDate = value;
            }
        }

        public DateTime? ToDate
        {
            get
            {
                return txtToData.SelectedDate;
            }

            set
            {
                txtToData.SelectedDate = value;
            }
        }

        public ColumnDTO SelectedColumn
        {
            get
            {
                return cmbColumns.SelectedItem as ColumnDTO;
            }

            set
            {
                cmbColumns.SelectedItem = value;
            }
        }

        public bool? withMajorException
        {
            get
            {
                return chkMajorException.IsChecked;
            }

            set
            {
                chkMajorException.IsChecked = value;
            }
        }

        public bool? withMinorException
        {
            get
            {
                return chkMinorException.IsChecked;
            }

            set
            {
                chkMinorException.IsChecked = value;
            }
        }

        public bool PackageDatagridVisiblity
        {
            get
            {
                return tabPackageLogs.Visibility == Visibility.Visible;
            }

            set
            {
                tabPackageLogs.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                if (value)
                    tabPackageLogs.IsSelected = true;
            }
        }

        public DataLogType? SelectedMainType
        {
            get
            {
                if (cmbMainType.SelectedItem != null)
                    return (DataLogType)cmbMainType.SelectedItem;
                else
                    return null;
            }

            set
            {
                cmbMainType.SelectedItem = value;
            }
        }

        public event EventHandler<MyCommonWPFControls.SearchFilterArg> EntitySearchFilterChanged;
        public event EventHandler<SearchFilterArg> UserSearchFilterChanged;
        public event EventHandler Confirmed;
        public event EventHandler ExitRequested;
        public event EventHandler<DataLogDTO> DetailsClicked;
        public event EventHandler<DataLogDTO> PackageClicked;
        public UC_LogReport()
        {
            InitializeComponent();

        }

        private void lokEntity_SearchFilterChanged(object sender, MyCommonWPFControls.SearchFilterArg e)
        {
            if (EntitySearchFilterChanged != null)
                EntitySearchFilterChanged(this, e);
        }

        private void lokUser_SearchFilterChanged(object sender, MyCommonWPFControls.SearchFilterArg e)
        {
            if (UserSearchFilterChanged != null)
                UserSearchFilterChanged(this, e);
        }



        public void SetColumnsItems(List<ColumnDTO> columns)
        {
            cmbColumns.DisplayMemberPath = "Alias";
            cmbColumns.ItemsSource = columns;
        }

        public void SetMainTypeItems(List<DataLogType> columns)
        {
            cmbMainType.ItemsSource = columns;
        }
        public void SetLogs(List<DataLogDTO> list)
        {
            dtgLog.ItemsSource = list;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (Confirmed != null)
                Confirmed(this, null);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (ExitRequested != null)
                ExitRequested(this, null);
        }


        public void AddEntitySelector(object view)
        {
            grdEntity.Children.Add(view as UIElement);
        }

        public void AddUserSelector(object view)
        {
            grdUser.Children.Add(view as UIElement);
        }
        public void AddDataSelector(object view)
        {
            RemoveDataSelector();
            grdData.Children.Add(view as UIElement);
        }
        public void RemoveDataSelector()
        {
            grdData.Children.Clear();
        }


        private void mnuDetails_Click(object sender, RoutedEventArgs e)
        {
            var row = GridContextMenu.GetClickedElement<GridViewRow>();
            if (row != null)
            {
                if (DetailsClicked != null)
                    DetailsClicked(sender, row.DataContext as DataLogDTO);
            }

        }

        private void mnuPackage_Click(object sender, RoutedEventArgs e)
        {
            var row = GridContextMenu.GetClickedElement<GridViewRow>();
            if (row != null)
            {
                if (PackageClicked != null)
                    PackageClicked(sender, row.DataContext as DataLogDTO);
            }
        }

        public void AddPackageDataLogs(List<DataLogDTO> logData)
        {
            dtgPackageLog.ItemsSource = logData;
        }

        private void mnuPackageDetails_Click(object sender, RoutedEventArgs e)
        {
            var row = PackageContextMenu.GetClickedElement<GridViewRow>();
            if (row != null)
            {
                if (DetailsClicked != null)
                    DetailsClicked(sender, row.DataContext as DataLogDTO);
            }
        }
    }
}
