using ModelEntites;

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
using Telerik.Windows.Controls;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmObjects.xaml
    /// </summary>
    public partial class frmConditionalPermission : UserControl
    {
        //public ObjectDTO Object { set; get; }
        BizSecurityObjects bizSecurityObjects = new BizSecurityObjects();
        BizSecuritySubject bizSecuritySubject = new BizSecuritySubject();
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        BizColumn bizColumn = new BizColumn();
        ConditionalPermissionDTO Message { set; get; }
        BizPermission bizPermission = new BizPermission();
        public frmConditionalPermission()
        {//کدام دیتابیس؟؟
            InitializeComponent();
            SetEntites();
            SetSecuritySubjects();
            AddSecurityActionPane();
        }
        frmSecurityAction frmSecurityAction;
        private void AddSecurityActionPane()
        {
            frmSecurityAction = new frmSecurityAction();
            RadPane pane = new RadPane();
            pane.CanUserClose = false;
            pane.Header = "دسترسی";
            pane.Content = frmSecurityAction;
            pnlActionList.Items.Add(pane);
        }
        private void SetSecuritySubjects()
        {
            lokSubject.DisplayMember = "Name";
            lokSubject.SelectedValueMember = "ID";
            lokSubject.AddColumn("Type", "نوع");
            lokSubject.SearchFilterChanged += LokSubject_SearchFilterChanged;
        }

        private void LokSubject_SearchFilterChanged(object sender, MyCommonWPFControls.SearchFilterArg e)
        {
            if (e.SingleFilterValue != null)
            {
                if (!e.FilerBySelectedValue)
                {
                    var subjects = bizSecuritySubject.GetSecuritySubjects(e.SingleFilterValue);
                    e.ResultItemsSource = subjects;
                }
                else
                {
                    var id = Convert.ToInt32(e.SingleFilterValue);
                    if (id > 0)
                    {
                        var subject = bizSecuritySubject.GetSecuritySubject(id);
                        e.ResultItemsSource = new List<SecuritySubjectDTO> { subject };
                    }
                    else
                        e.ResultItemsSource = null;
                }
            }
        }

        private void SetEntites()
        {
            lokEntities.DisplayMember = "Name";
            lokEntities.SelectedValueMember = "ID";
            lokEntities.SearchFilterChanged += LokEntities_SearchFilterChanged;
            lokEntities.SelectionChanged += LokEntities_SelectionChanged;
        }

        private void LokEntities_SelectionChanged(object sender, MyCommonWPFControls.SelectionChangedArg e)
        {
            if (lokEntities.SelectedItem != null)
            {
                var entityID = Convert.ToInt32(lokEntities.SelectedValue);
                SetSecurityObjects(entityID);
                SetFromulas(entityID);
                SetColumns(entityID);

            }
        }


        private void LokEntities_SearchFilterChanged(object sender, MyCommonWPFControls.SearchFilterArg e)
        {
            if (e.SingleFilterValue != null)
            {
                if (!e.FilerBySelectedValue)
                {

                    var list = bizTableDrivedEntity.GetAllEntities(e.SingleFilterValue);
                    e.ResultItemsSource = list;
                }
                else
                {
                    var id = Convert.ToInt32(e.SingleFilterValue);
                    if (id > 0)
                    {
                        lokEntities.ItemsSource = bizTableDrivedEntity.GetAllEntities();
                        var entity = bizTableDrivedEntity.GetTableDrivedEntity(id);
                        e.ResultItemsSource = new List<TableDrivedEntityDTO> { entity };
                    }
                    else
                        e.ResultItemsSource = null;
                }
            }

        }

        //private void cmbEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (lokEntities.SelectedItem != null)
        //    {
        //        var entityID = Convert.ToInt32(lokEntities.SelectedValue);
        //        SetSecurityObjects(entityID);
        //        SetFromulas(entityID);
        //        SetColumns(entityID);

        //    }
        //}

        private void SetSecurityObjects(int entityID)
        {
            var objects = bizSecurityObjects.GetSecurityObjectsOfEntity(entityID);
            if (lokObject.DisplayMember == null)
            {
                lokObject.AddColumn("Type", "نوع");
                lokObject.DisplayMember = "Name";
                lokObject.SelectedValueMember = "ID";
            }
            lokObject.ItemsSource = objects;
        }
        private void lokObject_SelectionChanged(object sender, MyCommonWPFControls.SelectionChangedArg e)
        {
            var securityObject = lokObject.SelectedItem as SecurityObjectDTO;
            if (securityObject != null)
            {

                if (securityObject.Type == DatabaseObjectCategory.Entity ||
                    securityObject.Type == DatabaseObjectCategory.Column ||
                    securityObject.Type == DatabaseObjectCategory.Command)
                    SetActinList(securityObject.Type);

            }
        }
        private void SetActinList(DatabaseObjectCategory entity)
        {
            var actions = SecurityHelper.GetActionsByCategory(DatabaseObjectCategory.Column);
            frmSecurityAction.SetActionTree(actions);
        }


        //private void SetCommands()
        //{
        //    if (cmbEntities.SelectedItem != null)
        //    {
        //        var entity = cmbEntities.SelectedItem as TableDrivedEntityDTO;
        //        BizEntityCommand bizEntityCommand = new BizEntityCommand();
        //        var commands = bizEntityCommand.GetEntityCommands(entity.ID, false);
        //        cmbCommands.ItemsSource = commands;
        //        cmbCommands.DisplayMemberPath = "Title";
        //        cmbCommands.SelectedValuePath = "ID";
        //    }
        //}
        private void SetColumns(int entityID)
        {
            //if (lokEntities.SelectedItem != null)
            //{
            //    var entity = lokEntities.SelectedItem as TableDrivedEntityDTO;
            //var columns = bizColumn.GetColumns(entity.ID, true);
            //columns.Add(null);
            //cmbColumns.ItemsSource = columns;
            //cmbColumns.DisplayMemberPath = "Name";
            //cmbColumns.SelectedValuePath = "ID";

            BizTableDrivedEntity biz = new BizTableDrivedEntity();
            var entity = biz.GetTableDrivedEntity(entityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);

            cmbConditionalColumns.ItemsSource = entity.Columns;
            cmbConditionalColumns.DisplayMemberPath = "Name";
            cmbConditionalColumns.SelectedValuePath = "ID";
            //}
        }
        //private void cmbColumns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (cmbColumns.SelectedItem != null)
        //        SetActinList(DatabaseObjectCategory.Column);
        //}

        //private void cmbCommands_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (cmbCommands.SelectedItem != null)
        //        SetActinList(DatabaseObjectCategory.Command);
        //}

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

        private void SetFromulas(int entityID)
        {
            cmbFormula.DisplayMemberPath = "Name";
            cmbFormula.SelectedValuePath = "ID";
            BizFormula bizFormula = new BizFormula();
            cmbFormula.ItemsSource = bizFormula.GetFormulas(entityID);
        }
        private void btnAddFormula_Click(object sender, RoutedEventArgs e)
        {

            var entity = lokEntities.SelectedItem as TableDrivedEntityDTO;
          
            frmFormula view = new frmFormula(0, entity.ID);
            view.FormulaSelected += View_FormulaSelected;
               MyProjectManager.GetMyProjectManager().ShowDialog(view, "Form");


        }

        private void View_FormulaSelected(object sender, FormulaSelectedArg e)
        {
            var entity = lokEntities.SelectedItem as TableDrivedEntityDTO;
            if (entity != null)
            {
                SetFromulas(entity.ID);
                cmbFormula.SelectedValue = e.FormulaID;
            }
        }

        private void ShowData()
        {
            lokEntities.SelectedValue = Message.EntityID;
            lokObject.SelectedValue = Message.SecurityObjectID;
            lokSubject.SelectedValue = Message.SecuritySubjectID;
            if (Message.FormulaID != 0)
                optFormula.IsChecked = true;
            cmbFormula.SelectedValue = Message.FormulaID;
            if (Message.ConditinColumnID != 0)
                optColumn.IsChecked = true;
            if (Message.HasNotRole)
                optHasNotRole.IsChecked = true;
            else
                optHasRole.IsChecked = true;
            cmbConditionalColumns.SelectedValue = Message.ConditinColumnID;
            txtValue.Text = Message.Value;

            frmSecurityAction.ShowData(Message.Actions);
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

            frmConditionalPermissionSelect view = new MyProject_WPF.frmConditionalPermissionSelect();
            view.ConditionalPermissionSelected += View_ConditionalPermissionSelected;
               MyProjectManager.GetMyProjectManager().ShowDialog(view, "Form");

        }

        private void View_ConditionalPermissionSelected(object sender, ConditionalPermissionSelectedArg e)
        {
            MyProjectManager.GetMyProjectManager().CloseDialog(sender);
            Message = bizPermission.GetConditionalPermission(e.ConditionalPermissionID);
            ShowData();
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var listActions = frmSecurityAction.GetCheckedActions();
            if (listActions.Any(x => x == SecurityAction.NoAccess))
            {
                if (listActions.Any(x => x != SecurityAction.NoAccess))
                {
                    MessageBox.Show("امکان انتخاب گزینه های عدم دسترسی و سایر گزینه ها نمی باشد");
                    return;
                }
            }
            if (listActions.Any(x => x == SecurityAction.ReadOnly))
            {
                if (listActions.Any(x => x != SecurityAction.NoAccess && x != SecurityAction.ReadOnly))
                {
                    MessageBox.Show("امکان انتخاب گزینه های فقط خواندنی و سایر گزینه ها نمی باشد");
                    return;
                }
            }
            if (Message == null)
                Message = new ConditionalPermissionDTO();
            Message.SecurityObjectID = Convert.ToInt32(lokObject.SelectedValue);
            Message.SecuritySubjectID = Convert.ToInt32(lokSubject.SelectedValue);
            Message.EntityID = (int)lokEntities.SelectedValue;
            Message.HasNotRole = optHasNotRole.IsChecked == true;
            //if (cmbColumns.SelectedItem != null)
            //    Message.ColumnID = (int)cmbColumns.SelectedValue;
            //else
            //    Message.ColumnID = 0;
            //if (cmbCommands.SelectedItem != null)
            //    Message.CommandID = (int)cmbCommands.SelectedValue;
            //else
            //    Message.CommandID = 0;

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
            Message.Actions = listActions;
            bizPermission.SaveConditionalPermission(Message);
            MessageBox.Show("اطلاعات ثبت شد");
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Message = new ConditionalPermissionDTO();
            ShowData();
        }
    }
}
