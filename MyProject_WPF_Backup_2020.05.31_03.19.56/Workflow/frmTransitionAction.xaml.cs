using ModelEntites;
using MyModelManager;

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

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmAddSelectAction.xaml
    /// </summary>
    public partial class frmTransitionAction : UserControl
    {

        BizAction bizAction = new BizAction();
        BizProcess bizProcess = new BizProcess();
        BizEntityGroup bizEntityGroup = new BizEntityGroup();
        TransitionDTO Transition { set; get; }
        public frmTransitionAction(TransitionDTO transition)
        {
            InitializeComponent();
            Transition = transition;
            //TransitionActions = transitionActions;
            dtgList.SelectionChanged += DtgList_SelectionChanged;
            SetBaseData();

            tabControl.IsEnabled = false;
            //Actions = bizAction.GetActions(ProcessID,).Where(x => TransitionActions.Select(y => y.ActionID).Contains(x.ID)).ToList();
            dtgList.ItemsSource = Transition.TransitionActions;
            if (Transition.TransitionActions.Count == 1)
            {
                TransitionActionSelected(Transition.TransitionActions.First());
            }
            ControlHelper.GenerateContextMenu(dtgFormList);
            ControlHelper.GenerateContextMenu(dtgFormulaList);
            if (Transition.CurrentState != null)
            {
                if (Transition.CurrentState.StateType == StateType.Start)
                {
                    tabForms.Visibility = Visibility.Collapsed;
                }
            }
        }

      

        private void DtgList_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            TransitionActionSelected(dtgList.SelectedItem as TransitionActionDTO);
        }

        private void TransitionActionSelected(TransitionActionDTO transactionAction)
        {
            if (transactionAction != null)
            {
                tabControl.IsEnabled = true;
                dtgFormulaList.ItemsSource = transactionAction.Formulas;
                dtgFormList.ItemsSource = transactionAction.EntityGroups;
            }
            else
            {
                dtgFormulaList.ItemsSource = null;
                dtgFormList.ItemsSource = null;
                tabControl.IsEnabled = false;
            }
        }

        private void SetBaseData()
        {
            //var col2 = dtgList.Columns[0] as GridViewComboBoxColumn;
            //col2.ItemsSource = WorkflowHelper.GetActionTypes();
            //col2.DisplayMemberPath = "Name";
            //col2.SelectedValueMemberPath = "ID";
          
        }
        //private void GetActions()
        //{

        //}


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager().CloseDialog(this);
        }




        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager().CloseDialog(this);
        }
    }

}
