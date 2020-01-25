using ModelEntites;
using MyModelManager;
using MySecurity;
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

namespace MySecurityUI
{
    /// <summary>
    /// Interaction logic for frmEntityRoleSecurity.xaml
    /// </summary>
    public partial class frmEntityRoleSecurityDirect : UserControl
    {
        BizDatabaseFunction bizDatabaseFunction = new BizDatabaseFunction();
        EntityRoleSecurityDirectDTO Message;
        BizRoleSecurity bizRoleSecurity = new BizRoleSecurity();
        int EntityID { set; get; }
        int EntityRoleSecurityDirectID { set; get; }
        public frmEntityRoleSecurityDirect(int entityID, int entityRoleSecurityDirectId)
        {
            InitializeComponent();
            EntityID = entityID;
            EntityRoleSecurityDirectID = entityRoleSecurityDirectId;
            if (entityRoleSecurityDirectId != 0)
                GetEntityRoleSecurityDirect(EntityRoleSecurityDirectID);
            else
                Message = new EntityRoleSecurityDirectDTO();
            SetRoles();
            SetDatabaseFunctions();
            SetColumns();
            SetOperators();
            ShowMessage();
        }
        private void GetEntityRoleSecurityDirect(int id)
        {
            Message = bizRoleSecurity.GetEntityRoleSecurityDirect(id, false);
            ShowMessage();
        }
        private void SetOperators()
        {
            cmbOperator.ItemsSource = Enum.GetValues(typeof(EntitySecurityOperator));
        }

        private void SetColumns()
        {
            BizColumn bizColumn = new BizColumn();
            cmbColumns.ItemsSource = bizColumn.GetColumns(EntityID, true);
            cmbColumns.DisplayMemberPath = "Name";
            cmbColumns.SelectedValuePath = "ID";
        }
        private void SetDatabaseFunctions()
        {
            cmbDatabaseFunction.DisplayMemberPath = "FunctionName";
            cmbDatabaseFunction.SelectedValuePath = "ID";
            cmbDatabaseFunction.ItemsSource = bizDatabaseFunction.GetDatabaseFunctions(Enum_DatabaseFunctionType.None);
        }


        private void SetRoles()
        {
            BizRole bizRole = new BizRole();
            cmbRole.ItemsSource = bizRole.GetRoleOrRoleGroups();
            cmbRole.DisplayMemberPath = "Name";
            cmbRole.SelectedValuePath = "ID";

        }
        private void btnAddRoleGroup_Click(object sender, RoutedEventArgs e)
        {
            frmRoleGroup view = new frmRoleGroup(0);
            view.GroupSaved += View_GroupSaved;
            view.ShowDialog();
        }

        private void View_GroupSaved(object sender, EventArgs e)
        {
            SetRoles();
        }

        private void btnEditRoleGroup_Click(object sender, RoutedEventArgs e)
        {
            var selected = cmbRole.SelectedItem as RoleOrRoleGroupDTO;
            if (selected != null)
            {
                frmRoleGroup view = new frmRoleGroup(selected.ID);
                view.GroupSaved += View_GroupSaved;
                view.ShowDialog();
            }
        }
        private void cmbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEditRoleGroup.Visibility = Visibility.Collapsed;
            var selected = cmbRole.SelectedItem as RoleOrRoleGroupDTO;
            if (selected != null)
            {
                if (selected.Type == RoleOrRoleGroupType.RoleGroup)
                {
                    btnEditRoleGroup.Visibility = Visibility.Visible;
                }
            }
        }
        private void ShowMessage()
        {
            if (Message != null)
            {
                cmbColumns.SelectedValue = Message.ColumnID;
                if (Message.RoleID != 0)
                    cmbRole.SelectedValue = Message.RoleID;
                else if (Message.RoleGroupID != 0)
                    cmbRole.SelectedValue = Message.RoleGroupID;
                if (!string.IsNullOrEmpty(Message.Value))
                {
                    txtValue.Text = Message.Value;
                    optValue.IsChecked = true;
                }
                else if (Message.DBFunctionID != 0)
                {
                    cmbDatabaseFunction.SelectedValue = Message.DBFunctionID;
                    optDBFunction.IsChecked = true;
                }
                cmbOperator.SelectedItem = Message.Operator;
                cmbRole.SelectedItem= Message.Operator ;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbColumns.SelectedItem == null)
            {
                MessageBox.Show("ستون معادل انتخاب نشده است");
                return;
            }
            if (cmbRole.SelectedItem == null)
            {
                MessageBox.Show("نفش معادل انتخاب نشده است");
                return;
            }
            if (cmbOperator.SelectedItem == null)
            {
                MessageBox.Show("عملگر معادل انتخاب نشده است");
                return;
            }
            if (Message == null)
                Message = new EntityRoleSecurityDirectDTO();
            Message.TableDrivedEntityID = EntityID;
            Message.ColumnID = (int)cmbColumns.SelectedValue;
            var selected = cmbRole.SelectedItem as RoleOrRoleGroupDTO;
            if (selected.Type == RoleOrRoleGroupType.Role)
            {
                Message.RoleID = (int)cmbRole.SelectedValue;
                Message.RoleGroupID = 0;
            }
            else
            {
                Message.RoleID = 0;
                Message.RoleGroupID = (int)cmbRole.SelectedValue;
            }

            if (optDBFunction.IsChecked == true)
            {
                Message.Value = "";
                Message.DBFunctionID = (int)cmbDatabaseFunction.SelectedValue;
            }
            else
            {
                Message.DBFunctionID = 0;
                Message.Value = txtValue.Text;
            }
            Message.Operator = (EntitySecurityOperator)cmbOperator.SelectedItem;
            bizRoleSecurity.UpdateEntityRoleSecurityDirect(Message);
            MessageBox.Show("اطلاعات ثبت شد");
        }

        //private void btnReturn_Click(object sender, RoutedEventArgs e)
        //{
        //    this.Close();
        //}

        private void optValue_Checked(object sender, RoutedEventArgs e)
        {
            cmbDatabaseFunction.IsEnabled = false;
            txtValue.IsEnabled = true;
        }

        private void optDBFunction_Checked(object sender, RoutedEventArgs e)
        {
            txtValue.IsEnabled = false;
            cmbDatabaseFunction.IsEnabled = true;

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            frmEntityRoleSecurityDirectSelect view = new frmEntityRoleSecurityDirectSelect(EntityID);
            view.EntityRoleSecurityDirectSelected += View_EntityRoleSecurityDirectSelected;
            view.ShowDialog();
        }

        private void View_EntityRoleSecurityDirectSelected(object sender, EntityRoleSecurityDirectSelectArg e)
        {
            if (e.ID != 0)
            {
                GetEntityRoleSecurityDirect(e.ID);
            }
        }

        




        //private void btnDatabaseFunctionEntity_Click(object sender, RoutedEventArgs e)
        //{
        //    if (cmbDatabaseFunction.SelectedItem != null)
        //    {
        //        var selected = cmbDatabaseFunction.SelectedItem as DatabaseFunctionDTO;
        //        var DatabaseFunctionEntityID = bizDatabaseFunction.GetDatabaseFunctionEntityID(EntityID, selected.ID);
        //        var DatabaseFunctionIntention = new DatabaseFunctionEntityIntention();
        //        DatabaseFunctionIntention.EntityID = EntityID;
        //        if (DatabaseFunctionEntityID == 0)
        //            DatabaseFunctionIntention.Type = Enum_DatabaseFunctionEntityIntention.DatabaseFunctionEntityDefinition;
        //        else
        //        {
        //            DatabaseFunctionIntention.DatabaseFunctionEntityID = DatabaseFunctionEntityID;
        //            DatabaseFunctionIntention.Type = Enum_DatabaseFunctionEntityIntention.DatabaseFunctionEntityEdit;
        //        }
        //        frmDatabaseFunction_Entity view = new frmDatabaseFunction_Entity(DatabaseFunctionIntention);
        //        view.ShowDialog();

        //    }
        //}
    }


}
