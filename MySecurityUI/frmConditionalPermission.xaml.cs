using ModelEntites;
using MyCommonWPFFroms;
using MyDatabaseToObject;
using MyFormulaFunctionStateFunctionLibrary;
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
    /// Interaction logic for frmObjects.xaml
    /// </summary>
    public partial class frmConditionalPermission : UserControl
    {
        //public ObjectDTO Object { set; get; }
        RoleOrRoleGroupDTO SelectedRoleOrRoleGroup { set; get; }
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        BizColumn bizColumn = new BizColumn();
        ConditionalPermissionDTO Message { set; get; }
        BizPermission bizPermission = new BizPermission();
        public frmConditionalPermission()
        {//کدام دیتابیس؟؟
            InitializeComponent();
            SetEntites();
            SetRoleOrRoleGroups();

        }

        private void SetRoleOrRoleGroups()
        {
            BizRole bizRole = new BizRole();
            cmbRoles.ItemsSource = bizRole.GetRoleOrRoleGroups();
            cmbRoles.DisplayMemberPath = "Name";
            cmbRoles.SelectedValuePath = "ID";
        }

        private void SetEntites()
        {
            cmbEntities.ItemsSource = bizTableDrivedEntity.GetAllEntities();
            cmbEntities.DisplayMemberPath = "Name";
            cmbEntities.SelectedValuePath = "ID";
        }
        private void cmbEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbEntities.SelectedItem != null)
            {
                SetFromulas();
                SetColumns();
                SetActinList(DatabaseObjectCategory.Entity);
            }
        }

        private void SetActinList(DatabaseObjectCategory entity)
        {
            var actions = SecurityHelper.GetActionsByCategory(DatabaseObjectCategory.Column);
            List<ActionDTO> list = new List<ActionDTO>();
            foreach (var action in actions)
            {
                ActionDTO item = new ActionDTO();
                item.Action = action;
                list.Add(item);
            }

            dtgRoleActions.ItemsSource = list;
        }


        private void SetCommands()
        {
            if (cmbEntities.SelectedItem != null)
            {
                var entity = cmbEntities.SelectedItem as TableDrivedEntityDTO;
                BizEntityCommand bizEntityCommand = new BizEntityCommand();
                var commands = bizEntityCommand.GetEntityCommands(entity.ID,false);
                cmbCommands.ItemsSource = commands;
                cmbCommands.DisplayMemberPath = "Title";
                cmbCommands.SelectedValuePath = "ID";
            }
        }
        private void SetColumns()
        {
            if (cmbEntities.SelectedItem != null)
            {
                var entity = cmbEntities.SelectedItem as TableDrivedEntityDTO;
                var columns = bizColumn.GetColumns(entity.ID,true);
                columns.Add(null);
                cmbColumns.ItemsSource = columns;
                cmbColumns.DisplayMemberPath = "Name";
                cmbColumns.SelectedValuePath = "ID";


                cmbConditionalColumns.ItemsSource = bizColumn.GetColumns(entity.ID, true);
                cmbConditionalColumns.DisplayMemberPath = "Name";
                cmbConditionalColumns.SelectedValuePath = "ID";
            }
        }
        private void cmbColumns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbColumns.SelectedItem != null)
                SetActinList(DatabaseObjectCategory.Column);
        }

        private void cmbCommands_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCommands.SelectedItem != null)
                SetActinList(DatabaseObjectCategory.Command);
        }

        //private int GetEntityID()
        //{
        //    int entityID = 0;
        //    if (Object != null)
        //    {
        //        if (Object.ObjectCategory == DatabaseObjectCategory.Entity)
        //        {
        //            entityID = Convert.ToInt32(Object.ObjectIdentity);
        //        }
        //        else if (Object.ObjectCategory == DatabaseObjectCategory.Column)
        //        {

        //            var parent = bizDatabaseToObject.GetParentObject(DatabaseObjectCategory.Column, Convert.ToInt32(Object.ObjectIdentity));
        //            if (parent.ObjectCategory == DatabaseObjectCategory.Entity)
        //            {
        //                entityID = Convert.ToInt32(parent.ObjectIdentity);
        //            }
        //        }
        //    }
        //    return entityID;
        //}

        private void SetFromulas()
        {

            if (cmbEntities.SelectedItem != null)
            {
                var entity = cmbEntities.SelectedItem as TableDrivedEntityDTO;
                cmbFormula.DisplayMemberPath = "Name";
                cmbFormula.SelectedValuePath = "ID";
                BizFormula bizFormula = new BizFormula();
                cmbFormula.ItemsSource = bizFormula.GetFormulas(entity.ID);
            }
        }
        private void btnAddFormula_Click(object sender, RoutedEventArgs e)
        {
            if (cmbEntities.SelectedItem != null)
            {
                var entity = cmbEntities.SelectedItem as TableDrivedEntityDTO;
                FormulaIntention formulaIntention = new FormulaIntention();
                formulaIntention.EntityID = entity.ID;
                formulaIntention.Type = Enum_FormulaIntention.FormulaForParameter;
                frmFormula view = new frmFormula(formulaIntention);
                view.FormulaSelected += View_FormulaSelected;
                view.ShowDialog();
            }

        }

        private void View_FormulaSelected(object sender, FormulaSelectedArg e)
        {
            SetFromulas();
            cmbFormula.SelectedValue = e.FormulaID;
        }

        private void ShowData()
        {
            cmbEntities.SelectedValue = Message.EntityID;
            cmbColumns.SelectedValue = Message.ColumnID;
            cmbCommands.SelectedValue = Message.CommandID;
            cmbRoles.SelectedValue = Message.RoleOrRoleGroup.ID;
            if (Message.FormulaID != 0)
                optFormula.IsChecked = true;
            cmbFormula.SelectedValue = Message.FormulaID;
            if (Message.ConditinColumnID != 0)
                optColumn.IsChecked = true;
            if (Message.HasNotRole)
                optHasRole.IsChecked = true;
            else
                optHasNotRole.IsChecked = true;
            cmbConditionalColumns.SelectedValue = Message.ConditinColumnID;
            txtValue.Text = Message.Value;

            var listActions = dtgRoleActions.ItemsSource as List<ActionDTO>;
            foreach (var action in listActions)
            {
                action.Selected = Message.Actions.Any(x => x.Action == action.Action);
            }
            dtgRoleActions.ItemsSource = null;
            dtgRoleActions.ItemsSource = listActions;
        }






        //void ucObjectEdit_ObjectSaved(object sender, ObjectSavedArg e)
        //{
        //    ucObjectList.ShowObjects(e.Object.ParentID);
        //}

        //private void btnExtractObjectFromDB_Click(object sender, RoutedEventArgs e)
        //{
        //    BizObject bizObject = new BizObject();
        //    bizObject.ExtractObjectsFromDB();
        //    ucObjectList.ShowObjects(null);
        //}



        private void optColumn_Checked(object sender, RoutedEventArgs e)
        {
            cmbConditionalColumns.IsEnabled = true;
            cmbFormula.IsEnabled = false;
            btnAddFormula.IsEnabled = false;
        }

        private void optFormula_Checked(object sender, RoutedEventArgs e)
        {
            cmbConditionalColumns.IsEnabled = false;
            cmbFormula.IsEnabled = true;
            btnAddFormula.IsEnabled = true;

        }

        private void btnList_Click(object sender, RoutedEventArgs e)
        {

            frmConditionalPermissionList view = new MySecurityUI.frmConditionalPermissionList();
            view.ConditionalPermissionSelected += View_ConditionalPermissionSelected;
            view.ShowDialog();

        }

        private void View_ConditionalPermissionSelected(object sender, ConditionalPermissionSelectedArg e)
        {
            Message = bizPermission.GetConditionalPermission(e.ConditionalPermission.ID);
            ShowData();
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var listActions = (dtgRoleActions.ItemsSource as List<ActionDTO>).Where(x => x.Selected);
            if (listActions.Any(x => x.Action == SecurityAction.NoAccess))
            {
                if (listActions.Any(x => x.Action != SecurityAction.NoAccess))
                {
                    MessageBox.Show("امکان انتخاب گزینه های عدم دسترسی و سایر گزینه ها نمی باشد");
                    return;
                }
            }
            if (listActions.Any(x => x.Action == SecurityAction.ReadOnly))
            {
                if (listActions.Any(x => x.Action != SecurityAction.NoAccess && x.Action != SecurityAction.ReadOnly))
                {
                    MessageBox.Show("امکان انتخاب گزینه های فقط خواندنی و سایر گزینه ها نمی باشد");
                    return;
                }
            }
            if (Message == null)
                Message = new ConditionalPermissionDTO();
            Message.RoleOrRoleGroup = cmbRoles.SelectedItem as RoleOrRoleGroupDTO;
            Message.EntityID = (int)cmbEntities.SelectedValue;
            Message.HasNotRole = optHasNotRole.IsChecked == true;
            if (cmbColumns.SelectedItem != null)
                Message.ColumnID = (int)cmbColumns.SelectedValue;
            else
                Message.ColumnID = 0;
            if (cmbCommands.SelectedItem != null)
                Message.CommandID = (int)cmbCommands.SelectedValue;
            else
                Message.CommandID = 0;

            Message.Value = txtValue.Text;
            if (optColumn.IsChecked == true)
            {
                Message.FormulaID = 0;
                Message.ConditinColumnID = (int)cmbConditionalColumns.SelectedValue;
            }
            else if (optFormula.IsChecked == true)
            {
                Message.ConditinColumnID = 0;
                Message.FormulaID = (int)cmbFormula.SelectedValue;
            }
            Message.Actions = listActions.Where(x => x.Selected).ToList();
            bizPermission.SaveConditionalPermission(Message);
            MessageBox.Show("اطلاعات ثبت شد");
        }


    }
}
