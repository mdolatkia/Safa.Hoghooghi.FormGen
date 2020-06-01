using MyUILibrary.EntityArea;
using MyUILibrary.WorkflowArea;

using ProxyLibrary.Workflow;
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
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for frmRequestAction.xaml
    /// </summary>
    public partial class frmWorkflowReport : UserControl, I_View_WorkflowReport
    {
        //public event EventHandler RequestActionUpdated;

        public frmWorkflowReport()
        {
            InitializeComponent();
            dtgResult.MouseDoubleClick += DataGrid_MouseDoubleClick;

        }

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

        public TransitionActionDTO SelectedTransitionAction
        {
            get
            {
                if (cmbTransitionAction != null)
                    return (TransitionActionDTO)cmbTransitionAction.SelectedItem;
                else
                    return null;
            }

            set
            {
                cmbTransitionAction.SelectedItem = value;
            }
        }

        public WFStateDTO SelectedCurrentState
        {
            get
            {
                if (cmbCurrentState != null)
                    return (WFStateDTO)cmbCurrentState.SelectedItem;
                else
                    return null;
            }

            set
            {
                cmbCurrentState.SelectedItem = value;
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

        public WFStateDTO SelectedHistoryState
        {
            get
            {
                if (cmbHistoryState!= null)
                    return (WFStateDTO)cmbHistoryState.SelectedItem;
                else
                    return null;
            }

            set
            {
                cmbHistoryState.SelectedItem = value;
            }
        }

        public event EventHandler Confirmed;
        public event EventHandler<CartableItemSelectedArg> InfoClicked;
        public event EventHandler<CartableMenuClickArg> MenuClicked;

        public void AddDataSelector(object view)
        {
            grdData.Children.Add(view as UIElement);
        }

        //public void AddEntitySelector(object view)
        //{
        //    grdEntity.Children.Add(view as UIElement);

        //}

        public void AddProcessSelector(object view)
        {
            grdProcess.Children.Add(view as UIElement);

        }

        public void AddUserSelector(object view)
        {
            grdUser.Children.Add(view as UIElement);
        }

        public void RemoveDataSelector()
        {
            grdData.Children.Clear();
        }

        public void SetTransitionActionItems(List<TransitionActionDTO> columns)
        {
            cmbTransitionAction.DisplayMemberPath = "Name";
            cmbTransitionAction.SelectedValuePath = "ID";
            cmbTransitionAction.ItemsSource = columns;
        }

        public void SetResult(List<WorkflowRequestDTO> list)
        {
            dtgResult.ItemsSource = list;
        }
        public void SetCurrentStateItems(List<WFStateDTO> columns)
        {
            cmbCurrentState.DisplayMemberPath = "Name";
            cmbCurrentState.SelectedValuePath = "ID";
            cmbCurrentState.ItemsSource = columns;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (Confirmed != null)
                Confirmed(this, null);
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as RadGridView).SelectedItem != null)
            {
                if (InfoClicked != null)
                {
                    InfoClicked(this, new CartableItemSelectedArg() { DataItem = (sender as RadGridView).SelectedItem, UIElement = (sender as RadGridView).GetRowForItem((sender as RadGridView).SelectedItem) });
                }
            }
        }

        private void RadContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

            var contextMenu = sender as RadContextMenu;
            if (contextMenu.GetClickedElement<GridViewRow>() != null)
            {
                var workflowRequest = contextMenu.GetClickedElement<GridViewRow>().DataContext as WorkflowRequestDTO;
                if (contextMenu != null && workflowRequest != null)
                {
                    if (MenuClicked != null)
                        MenuClicked(this, new CartableMenuClickArg() { Request = workflowRequest, ContextMenu = contextMenu });
                }
            }
        }

        public void SetHistoryStateItems(List<WFStateDTO> columns)
        {
            cmbHistoryState.DisplayMemberPath = "Name";
            cmbHistoryState.SelectedValuePath = "ID";
            cmbHistoryState.ItemsSource = columns;
        }
    }
}
