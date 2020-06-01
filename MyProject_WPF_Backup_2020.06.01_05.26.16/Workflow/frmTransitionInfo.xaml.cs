using ModelEntites;
using MyCommonWPFControls;
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
    public partial class frmTransitionInfo : UserControl
    {
        BizActivity bizActivity = new BizActivity();
        public event EventHandler<TransitoinInfoArg> InfoConfirmed;
        BizAction bizAction = new BizAction();
        BizProcess bizProcess = new BizProcess();
        BizEntityGroup bizEntityGroup = new BizEntityGroup();
        TransitionDTO Transition { set; get; }

        public frmTransitionInfo(TransitionDTO transition)
        {
            InitializeComponent();
            Transition = transition;
            //TransitionActions = transitionActions;
            ShowMessage();
            SetBaseData();
            dtgTransitionActionList.SelectionChanged += dtgTransitionActionList_SelectionChanged;

            ControlHelper.GenerateContextMenu(dtgFormList);
            ControlHelper.GenerateContextMenu(dtgFormulaList);
            ControlHelper.GenerateContextMenu(dtgTransitionActionList);
            ControlHelper.GenerateContextMenu(dtgListActivities);

            ControlHelper.GenerateContextMenu(dtgActionTargets);
            dtgActionTargets.CellEditEnded += DtgActionTargets_CellEditEnded;
            dtgActionTargets.RowLoaded += DtgActionTargets_RowLoaded;

        }
        private void DtgActionTargets_RowLoaded(object sender, RowLoadedEventArgs e)
        {
            CheckRollTypeCell(e.Row);
        }

        private void DtgActionTargets_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            CheckRollTypeCell(e.Cell.ParentRow);
        }
        private void CheckRollTypeCell(GridViewRowItem parentRow)
        {
            if (parentRow.DataContext is TransitionActionTargetDTO)
            {
                var cell = parentRow.Cells.FirstOrDefault(x => x.Column == colRoleType);
                if (cell != null)
                {
                    if ((parentRow.DataContext as TransitionActionTargetDTO).TargetType == TargetType.RoleMembers)
                    {
                        cell.IsEnabled = true;
                    }
                    else
                        cell.IsEnabled = false;
                }
            }
        }
        private void dtgTransitionActionList_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (dtgTransitionActionList.SelectedItem != null)
            {
                var item = dtgTransitionActionList.SelectedItem as TransitionActionDTO;
                dtgFormList.ItemsSource = item.EntityGroups;
                dtgFormulaList.ItemsSource = item.Formulas;
                dtgActionTargets.ItemsSource = item.Targets;
            }
        }
        BizTarget bizTarget = new BizTarget();
        BizRoleType bizRoleType = new BizRoleType();

        private void SetBaseData()
        {
            GetActions();
            GetActivitys();
            GetFormulaAndForms();

            colTargetType.ItemsSource = bizTarget.GetTargetTypes();
            colRoleType.ItemsSource = bizRoleType.GetAllRoleTypes();
            colRoleType.DisplayMemberPath = "Name";
            colRoleType.SelectedValueMemberPath = "ID";
        }

        private void GetFormulaAndForms()
        {
            var process = bizProcess.GetProcess(MyProjectManager.GetMyProjectManager.GetRequester(), Transition.ProcessID, false);
            BizFormula bizFormula = new BizFormula();

            var listAllFormula = bizFormula.GetFormulas(process.EntityID);
            List<FormulaDTO> listValidFormula = new List<FormulaDTO>();
            foreach (var formula in listAllFormula)
            {
                if (formula.ResultDotNetType == typeof(bool) ||
                    formula.ResultDotNetType == typeof(Boolean))
                {
                    listValidFormula.Add(formula);
                }
            }
            colFormula.ItemsSource = listValidFormula;
            colFormula.DisplayMemberPath = "Name";
            colFormula.SelectedValueMemberPath = "ID";

            colForm.ItemsSource = bizEntityGroup.GetEntityGroups(MyProjectManager.GetMyProjectManager.GetRequester(), Transition.ProcessID);
            colForm.DisplayMemberPath = "Name";
            colForm.SelectedValueMemberPath = "ID";
        }

        private void GetActions()
        {
            if (colAction.ItemsSource == null)
            {
                colAction.DisplayMemberPath = "Name";
                colAction.SelectedValueMemberPath = "ID";
                colAction.EditItemClicked += ColAction_EditItemClicked;
            }
            var actions = bizAction.GetActions(Transition.ProcessID);
            colAction.ItemsSource = actions;
        }

        private void ColAction_EditItemClicked(object sender, MyCommonWPFControls.EditItemClickEventArg e)
        {
            frmAddAction view;
            if ((sender as MyStaticLookup).SelectedItem == null)
                view = new MyProject_WPF.frmAddAction(Transition.ProcessID, 0);
            else
            {
                var id = ((sender as MyStaticLookup).SelectedItem as WFActionDTO).ID;
                view = new MyProject_WPF.frmAddAction(Transition.ProcessID, id);
            }
            view.ItemSaved += (sender1, e1) => View_ItemSaved1(sender1, e1, (sender as MyStaticLookup));
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form", Enum_WindowSize.Big);
        }
        private void View_ItemSaved1(object sender, SavedItemArg e, MyStaticLookup lookup)
        {
            GetActions();
            lookup.SelectedValue = e.ID;
        }

        private void GetActivitys()
        {
            if (colActivity.ItemsSource == null)
            {
                colActivity.DisplayMemberPath = "Name";
                colActivity.SelectedValueMemberPath = "ID";
                colActivity.EditItemClicked += ColActivity_EditItemClicked;
            }
            var Activitys = bizActivity.GetActivities(Transition.ProcessID, false);
            colActivity.ItemsSource = Activitys;
        }

        private void ColActivity_EditItemClicked(object sender, MyCommonWPFControls.EditItemClickEventArg e)
        {
            frmAddActivity view;
            if ((sender as MyStaticLookup).SelectedItem == null)
                view = new MyProject_WPF.frmAddActivity(Transition.ProcessID, 0);
            else
            {
                var id = ((sender as MyStaticLookup).SelectedItem as ActivityDTO).ID;
                view = new MyProject_WPF.frmAddActivity(Transition.ProcessID, id);
            }
            view.ItemSaved += (sender1, e1) => View_ItemSaved11(sender1, e1, (sender as MyStaticLookup));
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form", Enum_WindowSize.Big);
        }
        private void View_ItemSaved11(object sender, SavedItemArg e, MyStaticLookup lookup)
        {
            GetActivitys();
            lookup.SelectedValue = e.ID;
        }

        private void ShowMessage()
        {
            txtTitle.Text = Transition.Name;
            dtgTransitionActionList.ItemsSource = Transition.TransitionActions;
            dtgListActivities.ItemsSource = Transition.TransitionActivities;
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }




        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (txtTitle.Text == "")
            {
                MessageBox.Show("عنوان انتقال مشخص نشده است");
                return;
            }
            if (Transition.TransitionActions.Any(x => string.IsNullOrEmpty(x.Name)))
            {
                MessageBox.Show("عنوان یک یا چند اقدام مشخص نشده است");
                return;
            }
            Transition.Name = txtTitle.Text;
            if (InfoConfirmed != null)
                InfoConfirmed(this, new MyProject_WPF.TransitoinInfoArg() { Transition = Transition });
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }
    public class TransitoinInfoArg : EventArgs
    {
        public TransitionDTO Transition { set; get; }
    }
}
