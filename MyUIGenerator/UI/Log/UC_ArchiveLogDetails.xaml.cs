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
    public partial class UC_ArchiveLogDetails : UserControl, I_View_ArchiveLogReportDetails
    {
        public UC_ArchiveLogDetails()
        {
            InitializeComponent();
        }

        public event EventHandler ExitRequested;

      
        public void AddExceptionLogs(List<ArchiveItemLogDTO> exceptions)
        {
            dtgExceptions.ItemsSource = exceptions;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (ExitRequested != null)
                ExitRequested(this, null);
        }

      
    }
}
