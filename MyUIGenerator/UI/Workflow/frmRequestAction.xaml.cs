
using MyUILibrary.EntityArea;
using MyUILibrary.WorkflowArea;
using ProxyLibrary;
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
using System.Windows.Threading;

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for frmRequestAction.xaml
    /// </summary>
    public partial class frmRequestAction : UserControl, I_View_RequestAction
    {
        //public event EventHandler RequestActionUpdated;
        //  RequestActionDTO Action { set; get; }
        public event EventHandler RequestActionConfirmed;
        public event EventHandler<OrganizationPostDTO> CurrentUserOrganizationPostChanged;
        public event EventHandler<PossibleTransitionActionDTO> TargetTransitionActionSelected;
        public event EventHandler CloseRequested;
        public event EventHandler<string> OganizationPostsSearchChanged;

        public frmRequestAction()
        {
            InitializeComponent();
        }
        private void DtgOutgoingTransitoinActions_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (e.DataElement is PossibleTransitionActionDTO)
            {
                var item = e.DataElement as PossibleTransitionActionDTO;
                if (item.Color == ItemColor.Red)
                    e.Row.Foreground = new SolidColorBrush(Colors.Red);
                else if (item.Color == ItemColor.Green)
                    e.Row.Foreground = new SolidColorBrush(Colors.Green);
            }
        }

     
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //  UIManagerGenerator.GetUIManager().SaveRequestAction(RequestAction, UIManagerGenerator.GetUIManager().AgentUIMediator.GetRequester());

            if (RequestActionConfirmed != null)
                RequestActionConfirmed(this, null);
        }
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (CloseRequested != null)
                CloseRequested(this, null);
        }

        //public void ShowRequestAction(RequestActionDTO requestAction)
        //{
        //    //Action = action;

        //}
        public List<OrganizationPostDTO> OrganizationPosts
        {
            set
            {
                cmbRequesterRole.SelectedValuePath = "ID";
                cmbRequesterRole.DisplayMemberPath = "Name";
                cmbRequesterRole.ItemsSource = value;
            }
        }
        public OrganizationPostDTO CurrentUserSelectedOrganizationPost
        {
            get
            {
                if (cmbRequesterRole.SelectedItem != null)
                    return cmbRequesterRole.SelectedItem as OrganizationPostDTO;
                else return null;
            }

            set
            {
                cmbRequesterRole.SelectedItem = value;
            }
        }
        private void cmbRequesterRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CurrentUserOrganizationPostChanged != null)
            {
                CurrentUserOrganizationPostChanged(this, cmbRequesterRole.SelectedItem as OrganizationPostDTO);
            }
        }
     
        public string Description
        {
            get
            {
                return txtDescription.Text;
            }

            set
            {
                txtDescription.Text = value;
            }
        }


        public string ActionTitle
        {
            set
            {
                lblAction.Text = value;
            }
        }
        public string TargetReason
        {
            set
            {
                lblTargetReason.Text = value;
            }
        }
        public string NextState
        {
            set
            {
                lblNextState.Text = value;
            }
        }


        public bool OutgoingTransitoinActionEnablity
        {

            set
            {
                grdTargets.IsEnabled = value;
            }
            get
            {
                return grdTargets.IsEnabled;
            }
        }
        public void AddTargetSelectionView(I_View_WorkflowTransitionTargetSelection view)
        {
            grdTargets.Children.Add(view as UIElement);
        }
    }
}
