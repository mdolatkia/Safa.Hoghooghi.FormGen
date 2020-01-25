using MyUILibrary.WorkflowArea;
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
using ProxyLibrary.Workflow;

namespace MyUIGenerator.UI.Workflow
{
    /// <summary>
    /// Interaction logic for frmDiagramStateInfo.xaml
    /// </summary>
    public partial class frmDiagramStateInfo : UserControl, I_View_DiagramStateInfo
    {
        public frmDiagramStateInfo()
        {
            InitializeComponent();
        }

        public List<RequestActionDTO> CausingRequestActions
        {
            get
            {
                return dtgCausingRequestActions.ItemsSource as List<RequestActionDTO>;
            }

            set
            {
                dtgCausingRequestActions.ItemsSource = value;
            }
        }

        public string Date
        {
            get
            {
                return txtDate.Text;
            }

            set
            {
                txtDate.Text = value;
            }
        }

        public List<RequestActionDTO> PossibleRequestActions
        {
            get
            {
                return dtgPossibleRequestActions.ItemsSource as List<RequestActionDTO>;
            }

            set
            {
                dtgPossibleRequestActions.ItemsSource = value;
            }
        }

        public string StateName
        {
            get
            {
                return txtStateName.Text;
            }

            set
            {
                txtStateName.Text = value;
            }
        }

        public event EventHandler ExisRequested;

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (ExisRequested != null)
                ExisRequested(this, null);
        }
    }
}
