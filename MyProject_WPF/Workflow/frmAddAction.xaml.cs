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
    public partial class frmAddAction : UserControl
    {
        int ProcessID { set; get; }
        BizAction bizAction = new BizAction();
      
        BizProcess bizProcess = new BizProcess();
        WFActionDTO Message = new WFActionDTO();
        public event EventHandler<SavedItemArg> ItemSaved;
        public frmAddAction(int processID, int actionID)
        {
            InitializeComponent();
            ProcessID = processID;
            //dtgActionTargets.SelectionChanged += DtgActionTargets_SelectionChanged;
            SetBaseData();
            if (actionID != 0)
                GetAction(actionID);
            else
            {
                Message = new WFActionDTO();
                ShowMessage();
            }
          
        }

    
       

    
        //private void DtgActionTargets_SelectionChanged(object sender, SelectionChangeEventArgs e)
        //{
        //    if (dtgActionTargets.SelectedItem is ActionTargetDTO)
        //    {
        //        dtgTargetRoles.ItemsSource = (dtgActionTargets.SelectedItem as ActionTargetDTO).Roles;
        //    }
        //}

        private void SetBaseData()
        {
            //cmbType.DisplayMemberPath = "Name";
            //cmbType.SelectedValuePath = "ID";
            cmbType.ItemsSource = bizAction.GetActionTypes();

           
            //col7.DisplayMemberPath = "Name";
            //col7.SelectedValueMemberPath = "ID";

       
        }

        private void GetAction(int actionID)
        {
            Message = bizAction.GetAction(actionID);
            ShowMessage();
        }

        private void ShowMessage()
        {
            //if (Message.ID != 0)
            //    ProcessID = Message.ProcessID;
            txtName.Text = Message.Name;
            txtDescription.Text = Message.Description;
            cmbType.SelectedItem = Message.ActionType;
         
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
            Message.ActionType = (ActionType)cmbType.SelectedItem;
            Message.ID = bizAction.UpdateActions(Message);
            MessageBox.Show("با موفقیت ثبت شد");
            if (ItemSaved != null)
                ItemSaved(this, new SavedItemArg() { ID = Message.ID });
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Message = new WFActionDTO();
            ShowMessage();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }

}
