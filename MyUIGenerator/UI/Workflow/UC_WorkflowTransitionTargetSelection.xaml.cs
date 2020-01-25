using MyUILibrary.EntityArea;

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

using MyUILibrary.WorkflowArea;
using System.Windows.Threading;

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for frmWorkflowRequestCreation.xaml
    /// </summary>
    public partial class UC_WorkflowTransitionTargetSelection : UserControl, I_View_WorkflowTransitionTargetSelection
    {
      
        public event EventHandler<PossibleTransitionActionDTO> TargetTransitionActionSelected;
        public event EventHandler<string> OganizationPostsSearchChanged;
        public event EventHandler<string> SimpleSearchChanged;

        public UC_WorkflowTransitionTargetSelection()
        {
            InitializeComponent();
            dtgOutgoingTransitoinActions.SelectionChanged += DtgOutgoingTransitoinActions_SelectionChanged;
            dtgOutgoingTransitoinActions.RowLoaded += DtgOutgoingTransitoinActions_RowLoaded;

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timerSimple.Interval = TimeSpan.FromSeconds(1);
            timerSimple.Tick += TimerSimple_Tick; ;
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

      

      
        public List<PossibleTransitionActionDTO> OutgoingTransitoinActions
        {
            get
            {
                return dtgOutgoingTransitoinActions.ItemsSource as List<PossibleTransitionActionDTO>;
            }

            set
            {
                //dtgOutgoingTransitoinActions.SelectedValuePath = "ID";
                //dtgOutgoingTransitoinActions.DisplayMemberPath = "Title";
                dtgOutgoingTransitoinActions.ItemsSource = value;
                dtgOutgoingTransitoinActions.SelectedItem = null;
                dtgTransitoinActionPosts.ItemsSource = null;
            }
        }

     
        public List<TransitionActionOrganizationPostDTO> TargetOrganizationPosts
        {
            get
            {
                return dtgTransitoinActionPosts.ItemsSource as List<TransitionActionOrganizationPostDTO>;
            }

            set
            {
                dtgTransitoinActionPosts.ItemsSource = value;
            }
        }

        public PossibleTransitionActionDTO SelectedTransitionAction
        {
            get
            {
                return dtgOutgoingTransitoinActions.SelectedItem as PossibleTransitionActionDTO;
            }


        }

        public bool SharedTargetsVisibility
        {
            get
            {
                return tabSharedTargets.Visibility == Visibility.Visible;
            }

            set
            {
                tabSharedTargets.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public List<TransitionActionOrganizationPostDTO> SharedTargets
        {
            get
            {
                return dtgSharedTargets.ItemsSource as List<TransitionActionOrganizationPostDTO>;
            }

            set
            {
                dtgSharedTargets.ItemsSource = value;
            }
        }

        private void DtgOutgoingTransitoinActions_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (dtgOutgoingTransitoinActions.SelectedItem is PossibleTransitionActionDTO)
            {
                if (TargetTransitionActionSelected != null)
                    TargetTransitionActionSelected(this, dtgOutgoingTransitoinActions.SelectedItem as PossibleTransitionActionDTO);
            }
            else
                  if (TargetTransitionActionSelected != null)
                TargetTransitionActionSelected(this, null);
        }
        DispatcherTimer timerSimple = new DispatcherTimer();
        DispatcherTimer timer = new DispatcherTimer();
        private void Timer_Tick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();
            if (OganizationPostsSearchChanged != null)
                OganizationPostsSearchChanged(this, txtSearch.Text);
        }
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            timer.Stop();
            timer.Start();
        }
        private void TimerSimple_Tick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();
            if (SimpleSearchChanged != null)
                SimpleSearchChanged(this, txtSearchSimple.Text);
        }
        private void txtSearchSimple_TextChanged(object sender, TextChangedEventArgs e)
        {
            timerSimple.Stop();
            timerSimple.Start();
        }
    }

}
