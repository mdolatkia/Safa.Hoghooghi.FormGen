using MyWorkflowLibrary;
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

namespace MyProject_WPF
{
    /// <summary>
    /// InterGroup logic for frmAddSelectGroup.xaml
    /// </summary>
    public partial class frmAddSelectGroup : Window
    {
        int ProcessID { set; get; }
        int GroupID { set; get; }
        Group Message = new Group();
        public event EventHandler<SavedItemArg> ItemSaved;
        public frmAddSelectGroup(int processID, int groupID)
        {
            InitializeComponent();
            ProcessID = processID;
            GroupID = groupID;
            SetBaseData();
            if (GroupID != 0)
                GetGroup();
            else
                SetNewItem();
        }

        private void SetBaseData()
        {
            var col4 = dtgGroupUsers.Columns[0] as GridViewComboBoxColumn;
            col4.ItemsSource = WorkflowHelper.GetUsers();
            col4.DisplayMemberPath = "UserName";
            col4.SelectedValueMemberPath = "ID";
        }

        private void GetGroup()
        {
            if (GroupID != 0)
            {
                Message = WorkflowHelper.GetGroup(GroupID);
                LoadItem();
            }

        }

        private void LoadItem()
        {
            if (Message.ID != 0)
                ProcessID = Message.ProcessID;
            txtName.Text = Message.Name;
          
            dtgGroupUsers.ItemsSource = Message.GroupMember;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("نام وارد نشده است");
                return;
            }
          
            Message.ProcessID = ProcessID;
            Message.Name = txtName.Text;
          
            Message.ID = WorkflowHelper.SaveGroup(Message);
            if (ItemSaved != null)
                ItemSaved(this, new SavedItemArg() { ID = Message.ID });
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            SetNewItem();
        }

        private void SetNewItem()
        {
            Message = new Group();
            LoadItem();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
    
}
