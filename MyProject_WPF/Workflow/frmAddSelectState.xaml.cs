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
    /// InterState logic for frmAddSelectState.xaml
    /// </summary>
    public partial class frmAddSelectState : UserControl
    {
        int ProcessID { set; get; }
        BizActivity bizActivity = new BizActivity();
        WFStateDTO Message = new WFStateDTO();
        BizState bizState = new BizState();
        public event EventHandler<SavedItemArg> ItemSaved;
        public frmAddSelectState(int processID, int stateID)
        {
            InitializeComponent();
            ProcessID = processID;
            SetBaseData();
            if (stateID != 0)
                GetState(stateID);
            else
                SetNewItem();
            ControlHelper.GenerateContextMenu(dtgFormulaList);
            ControlHelper.GenerateContextMenu(dtgStateActivities);

        }
        BizFormula bizFormula = new BizFormula();

        private void SetBaseData()
        {
            //cmbType.DisplayMemberPath = "Name";
            //cmbType.SelectedValuePath = "ID";
            cmbType.ItemsSource = bizState.GetStateTypes();

            GetActivities();

            BizProcess bizProcess = new BizProcess();
            var process = bizProcess.GetProcess(MyProjectManager.GetMyProjectManager.GetRequester(), ProcessID, false);
            var col3 = dtgFormulaList.Columns[0] as GridViewComboBoxColumn;
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
            col3.ItemsSource = listValidFormula;
            col3.DisplayMemberPath = "Name";
            col3.SelectedValueMemberPath = "ID";
            //var col8 = dtgStateTargets.Columns[1] as GridViewComboBoxColumn;
            //col8.ItemsSource = WorkflowHelper.GetGroups(Message.ProcessID);
            //col8.DisplayMemberPath = "Name";
            //col8.SelectedValueMemberPath = "ID";
        }

        private void GetActivities()
        {
            if (colActivities.ItemsSource == null)
            {
                colActivities.DisplayMemberPath = "Name";
                colActivities.SelectedValueMemberPath = "ID";
                colActivities.NewItemEnabled = true;
                colActivities.EditItemEnabled = true;
                colActivities.EditItemClicked += colPartialLetterTemplates_EditItemClicked;
            }
            colActivities.ItemsSource = bizActivity.GetActivities(ProcessID, false);
        }
        private void colPartialLetterTemplates_EditItemClicked(object sender, MyCommonWPFControls.EditItemClickEventArg e)
        {
            frmAddActivity view;
            var selectedField = e.DataConext as ActivityDTO;
            if (selectedField != null)
            {
                if ((sender as MyStaticLookup).SelectedItem == null)
                    view = new MyProject_WPF.frmAddActivity(ProcessID, 0);
                else
                {
                    var id = ((sender as MyStaticLookup).SelectedItem as ActivityDTO).ID;
                    view = new MyProject_WPF.frmAddActivity(ProcessID, id);
                }
                view.ItemSaved += (sender1, e1) => View_ItemSaved1(sender1, e1, (sender as MyStaticLookup));
                MyProjectManager.GetMyProjectManager.ShowDialog(view, "فعالیتها", Enum_WindowSize.Big);
            }
        }

        private void View_ItemSaved1(object sender, SavedItemArg e, MyStaticLookup lookup)
        {
            GetActivities();
            lookup.SelectedValue = e.ID;
        }


        private void GetState(int stateID)
        {

            Message = bizState.GetState(stateID, true);
            LoadItem();


        }

        private void LoadItem()
        {
            //if (Message.ID != 0)
            //    ProcessID = Message.ProcessID;
            txtName.Text = Message.Name;
            txtDescription.Text = Message.Description;
            cmbType.SelectedItem = Message.StateType;
            dtgStateActivities.ItemsSource = Message.Activities;
            dtgFormulaList.ItemsSource = Message.Formulas;

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("نام وارد نشده است");
                return;
            }
            if (cmbType.SelectedItem == null)
            {
                MessageBox.Show("نوع وارد نشده است");
                return;
            }
            Message.ProcessID = ProcessID;
            Message.Name = txtName.Text;
            Message.Description = txtDescription.Text;
            Message.StateType = (StateType)cmbType.SelectedItem;
            Message.ID = bizState.UpdateStates(Message);
            if (ItemSaved != null)
                ItemSaved(this, new SavedItemArg() { ID = Message.ID });
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            SetNewItem();

        }

        private void SetNewItem()
        {
            Message = new WFStateDTO();
            LoadItem();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }


    }

}
