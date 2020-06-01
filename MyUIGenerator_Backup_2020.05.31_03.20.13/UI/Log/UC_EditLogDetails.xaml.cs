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

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for UC_LogReport.xaml
    /// </summary>
    public partial class UC_EditLogDetails : UserControl, I_View_EditLogReportDetails
    {
        public UC_EditLogDetails()
        {
            InitializeComponent();
        }

        public bool ColumnParameterVisibility
        {
            get
            {
                return tabColumnParameters.Visibility == Visibility.Visible;
            }

            set
            {
                tabColumnParameters.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public bool OldValueColumnVisibility
        {
            get
            {
                return colOldValue.IsVisible;
            }

            set
            {
                colOldValue.IsVisible = value;
            }
        }

        public event EventHandler<EditDataItemColumnDetailsDTO> ColmnSelected;
        public event EventHandler ExitRequested;

        public void AddColumnEditLogs(List<EditDataItemColumnDetailsDTO> columns)
        {
            dtgColumns.ItemsSource = columns;
        }

        public void AddColumnFormulaParameters(List<FormulaUsageParemetersDTO> formulaUsageParemeters)
        {
            dtgColumnFormulaParamters.ItemsSource = formulaUsageParemeters;
        }

        public void AddExceptionLogs(List<EditDataItemExceptionLogDTO> exceptions)
        {
            dtgExceptions.ItemsSource = exceptions;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (ExitRequested != null)
                ExitRequested(this, null);
        }

        private void dtgColumns_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (dtgColumns.SelectedItem is EditDataItemColumnDetailsDTO)
            {
                if(ColmnSelected!=null)
                {
                    ColmnSelected(this, (dtgColumns.SelectedItem as EditDataItemColumnDetailsDTO));
                }
             
            }
        }
    }
}
