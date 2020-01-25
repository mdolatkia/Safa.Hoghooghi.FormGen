using ModelEntites;
using MyCommonWPFControls;

using MyModelManager;

using ProxyLibrary;
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

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmEntityRoleSecurity.xaml
    /// </summary>
    public partial class frmEntitySecurityDirect : UserControl
    {
        BizColumn bizColumn = new BizColumn();
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        BizDatabaseFunction bizDatabaseFunction = new BizDatabaseFunction();
        BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
        EntitySecurityDirectDTO Message;
        BizSecuritySubject bizSecuritySubject = new BizSecuritySubject();
        BizRoleSecurity bizRoleSecurity = new BizRoleSecurity();
        int EntitySecurityDirectID { set; get; }
        public frmEntitySecurityDirect(int EntitySecurityDirectId)
        {
            InitializeComponent();
            EntitySecurityDirectID = EntitySecurityDirectId;
            if (EntitySecurityDirectId != 0)
                GetEntitySecurityDirect(MyProjectManager.GetMyProjectManager.GetRequester(), EntitySecurityDirectID);
            else
                Message = new EntitySecurityDirectDTO();
            SetEntites();
            SetSecuritySubjects();
            //  SetAndOrType();
            SetDatabaseFunctions();
            SetOperators();
            SetReservedValues();
            SetMode();
            dtgConditions.RowLoaded += DtgConditions_RowLoaded;
            dtgConditions.CellEditEnded += DtgConditions_CellEditEnded;
            colRelationshipTail.EditItemClicked += ColRelationshipTail_EditItemClicked;
            ControlHelper.GenerateContextMenu(dtgConditions);
            ShowMessage();
        }

        private void SetMode()
        {
            cmbMode.ItemsSource = Enum.GetValues(typeof(SecurityMode));
        }

        private void ColRelationshipTail_EditItemClicked(object sender, MyCommonWPFControls.EditItemClickEventArg e)
        {

            frmEntityRelationshipTail frm = null;

            frm = new frmEntityRelationshipTail((int)lokEntities.SelectedValue);

            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "رابطه های مرتبط");
            frm.ItemSelected += (sender1, e1) => Frm_TailSelected(sender1, e1, (sender as MyStaticLookup));
        }

        private void Frm_TailSelected(object sender1, EntityRelationshipTailSelectedArg e1, MyStaticLookup myStaticLookup)
        {
            SetRelationshipTails();
            myStaticLookup.SelectedValue = e1.EntityRelationshipTailID;
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
            SetRelationshipTails();
        }

        private void LokEntities_SearchFilterChanged(object sender, MyCommonWPFControls.SearchFilterArg e)
        {
            if (e.SingleFilterValue != null)
            {
                if (!e.FilterBySelectedValue)
                {
                    var list = bizTableDrivedEntity.GetAllEntities(MyProjectManager.GetMyProjectManager.GetRequester(), e.SingleFilterValue, null);
                    e.ResultItemsSource = list;
                }
                else
                {
                    var id = Convert.ToInt32(e.SingleFilterValue);
                    if (id > 0)
                    {
                        //  lokEntities.ItemsSource = bizTableDrivedEntity.GetAllEntities();
                        var entity = bizTableDrivedEntity.GetSimpleEntity(MyProjectManager.GetMyProjectManager.GetRequester(), id); ;
                        e.ResultItemsSource = new List<TableDrivedEntityDTO> { entity };
                    }
                    else
                        e.ResultItemsSource = null;
                }
            }
        }

        private void SetReservedValues()
        {
            colReservedValue.ItemsSource = Enum.GetValues(typeof(SecurityReservedValue));
        }

        private void DtgConditions_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            if (e.Cell.Column == colRelationshipTail)
            {
                if (e.Cell.DataContext is EntitySecurityConditionDTO)
                {
                    var condition = (e.Cell.DataContext as EntitySecurityConditionDTO);
                    SetConditionColumns(condition);
                }
            }
        }

        private void DtgConditions_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (e.DataElement is EntitySecurityConditionDTO)
            {
                var condition = (e.DataElement as EntitySecurityConditionDTO);
                SetConditionColumns(condition);
            }
        }

        private void SetConditionColumns(EntitySecurityConditionDTO condition)
        {
            colColumns.DisplayMemberPath = "Name";
            colColumns.SelectedValueMemberPath = "ID";
            BizTableDrivedEntity biz = new BizTableDrivedEntity();

            if (condition.RelationshipTailID == 0)
            {
                if (lokEntities.SelectedItem != null)
                {
                    var entity = biz.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), (int)lokEntities.SelectedValue, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
                    condition.Columns = entity.Columns; ;
                }
                else
                    condition.Columns = new List<ColumnDTO>();
            }
            else
            {
                var relationshipTail = bizEntityRelationshipTail.GetEntityRelationshipTail(MyProjectManager.GetMyProjectManager.GetRequester(), condition.RelationshipTailID);
                var entity = biz.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), relationshipTail.TargetEntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
                condition.Columns = entity.Columns;
            }
        }

        private void SetRelationshipTails()
        {
            if (lokEntities.SelectedItem != null)
            {
                var tails = bizEntityRelationshipTail.GetEntityRelationshipTails(MyProjectManager.GetMyProjectManager.GetRequester(), (int)lokEntities.SelectedValue);
                colRelationshipTail.DisplayMemberPath = "EntityPath";
                colRelationshipTail.SelectedValueMemberPath = "ID";
                colRelationshipTail.ItemsSource = tails;

            }
            else
            {
                colRelationshipTail.ItemsSource = null;
            }
        }

        //private void SetAndOrType()
        //{
        //    cmbAndOR.ItemsSource = Enum.GetValues(typeof(AndORType));
        //}

        private void GetEntitySecurityDirect(DR_Requester requester, int id)
        {
            Message = bizRoleSecurity.GetEntitySecurityDirect( requester, id, true);
            ShowMessage();
        }
        private void SetOperators()
        {
            colOperator.ItemsSource = Enum.GetValues(typeof(EntitySecurityOperator));
        }


        private void SetDatabaseFunctions()
        {
            colDatabaseFunction.DisplayMemberPath = "FunctionName";
            colDatabaseFunction.SelectedValueMemberPath = "ID";
            colDatabaseFunction.ItemsSource = bizDatabaseFunction.GetDatabaseFunctions(Enum_DatabaseFunctionType.None);
        }


        private void SetSecuritySubjects()
        {
            lokSubject.DisplayMember = "Name";
            lokSubject.SelectedValueMember = "ID";
            lokSubject.SearchFilterChanged += LokSubject_SearchFilterChanged;
        }
        private void LokSubject_SearchFilterChanged(object sender, MyCommonWPFControls.SearchFilterArg e)
        {
            if (e.SingleFilterValue != null)
            {
                if (!e.FilterBySelectedValue)
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

        //private void btnAddRoleGroup_Click(object sender, RoutedEventArgs e)
        //{
        //    frmRoleGroup view = new frmRoleGroup(0);
        //    view.GroupSaved += View_GroupSaved;
        //       MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form");
        //}

        //private void View_GroupSaved(object sender, EventArgs e)
        //{
        //    SetSecuritySubjects();
        //}

        //private void btnEditRoleGroup_Click(object sender, RoutedEventArgs e)
        //{
        //    var selected = cmbRole.SelectedItem as RoleOrRoleGroupDTO;
        //    if (selected != null)
        //    {
        //        frmRoleGroup view = new frmRoleGroup(selected.ID);
        //        view.GroupSaved += View_GroupSaved;
        //           MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form");
        //    }
        //}
        //private void cmbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    btnEditRoleGroup.Visibility = Visibility.Collapsed;
        //    var selected = cmbRole.SelectedItem as RoleOrRoleGroupDTO;
        //    if (selected != null)
        //    {
        //        if (selected.Type == RoleOrRoleGroupType.RoleGroup)
        //        {
        //            btnEditRoleGroup.Visibility = Visibility.Visible;
        //        }
        //    }
        //}
        private void ShowMessage()
        {
            lokSubject.SelectedValue = Message.SecuritySubjectID;
            //cmbAndOR.SelectedItem = Message.ConditionAndORType;
            dtgConditions.ItemsSource = Message.Conditions;
            lokEntities.SelectedValue = Message.TableDrivedEntityID;
            cmbMode.SelectedItem = Message.Mode;
            chkIgnoreSecurity.IsChecked = Message.IgnoreSecurity;

            //cmbColumns.SelectedValue = Message.ColumnID;
            //if (Message.RoleID != 0)
            //    cmbRole.SelectedValue = Message.RoleID;
            //else if (Message.RoleGroupID != 0)
            //    cmbRole.SelectedValue = Message.RoleGroupID;
            //if (!string.IsNullOrEmpty(Message.Value))
            //{
            //    txtValue.Text = Message.Value;
            //    optValue.IsChecked = true;
            //}
            //else if (Message.DBFunctionID != 0)
            //{
            //    cmbDatabaseFunction.SelectedValue = Message.DBFunctionID;
            //    optDBFunction.IsChecked = true;
            //}
            //cmbOperator.SelectedItem = Message.Operator;
            //cmbRole.SelectedItem= Message.Operator ;

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (lokEntities.SelectedItem == null)
            {
                MessageBox.Show("موجودیت انتخاب نشده است");
                return;
            }
            //if (cmbColumns.SelectedItem == null)
            //{
            //    MessageBox.Show("ستون معادل انتخاب نشده است");
            //    return;
            //}
            //if (cmbRole.SelectedItem == null)
            //{
            //    MessageBox.Show("نفش معادل انتخاب نشده است");
            //    return;
            //}
            //if (cmbOperator.SelectedItem == null)
            //{
            //    MessageBox.Show("عملگر معادل انتخاب نشده است");
            //    return;
            //}
            Message.IgnoreSecurity = chkIgnoreSecurity.IsChecked == true;
            Message.Mode = (SecurityMode)cmbMode.SelectedItem;
            Message.TableDrivedEntityID = (int)lokEntities.SelectedValue;
            //Message.ConditionAndORType = (AndORType)cmbAndOR.SelectedItem;
            if (lokSubject.SelectedItem == null)
                Message.SecuritySubjectID = 0;
            else
                Message.SecuritySubjectID = (int)lokSubject.SelectedValue;
            //var selected = cmbRole.SelectedItem as RoleOrRoleGroupDTO;
            //if (selected.Type == RoleOrRoleGroupType.Role)
            //{
            //    Message.RoleID = (int)cmbRole.SelectedValue;
            //    Message.RoleGroupID = 0;
            //}
            //else
            //{
            //    Message.RoleID = 0;
            //    Message.RoleGroupID = (int)cmbRole.SelectedValue;
            //}

            //if (optDBFunction.IsChecked == true)
            //{
            //    Message.Value = "";
            //    Message.DBFunctionID = (int)cmbDatabaseFunction.SelectedValue;
            //}
            //else
            //{
            //    Message.DBFunctionID = 0;
            //    Message.Value = txtValue.Text;
            //}
            //Message.Operator = (EntitySecurityOperator)cmbOperator.SelectedItem;
            Message.ID = bizRoleSecurity.UpdateEntitySecurityDirect(Message);
            MessageBox.Show("اطلاعات ثبت شد");
        }

        //private void btnReturn_Click(object sender, RoutedEventArgs e)
        //{
        //    MyProjectManager.GetMyProjectManager.CloseDialog(this);
        //}

        //private void optValue_Checked(object sender, RoutedEventArgs e)
        //{
        //    cmbDatabaseFunction.IsEnabled = false;
        //    txtValue.IsEnabled = true;
        //}

        //private void optDBFunction_Checked(object sender, RoutedEventArgs e)
        //{
        //    txtValue.IsEnabled = false;
        //    cmbDatabaseFunction.IsEnabled = true;

        //}
        frmEntitySecurityDirectSelect searchView = null;

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (searchView == null)
            {
                searchView = new frmEntitySecurityDirectSelect();
                searchView.EntitySecurityDirectSelected += View_EntitySecurityDirectSelected;
            }
            MyProjectManager.GetMyProjectManager.ShowDialog(searchView, "جستجوی دسترسی داده");
        }

        private void View_EntitySecurityDirectSelected(object sender, EntitySecurityDirectSelectArg e)
        {
            if (e.ID != 0)
            {
                MyProjectManager.GetMyProjectManager.CloseDialog(sender);
                GetEntitySecurityDirect(MyProjectManager.GetMyProjectManager.GetRequester(), e.ID);
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Message = new EntitySecurityDirectDTO();
            ShowMessage();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (Message.ID != 0)
            {
                bizRoleSecurity.DeleteEntitySecurityDirect(Message.ID);
                btnNew_Click(null, null);
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
        //           MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form");

        //    }
        //}
    }


}
