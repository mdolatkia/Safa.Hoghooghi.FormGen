using ModelEntites;

using MyFormulaFunctionStateFunctionLibrary;
using MyInterfaces;
using MyModelManager;
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
    /// Interaction logic for frmActionActivitys.xaml
    /// </summary>
    public partial class frmUIActionActivity : UserControl
    {
        UIActionActivityDTO Message { set; get; }
        BizDatabaseFunction bizDatabaseFunction = new BizDatabaseFunction();
        BizUIActionActivity bizActionActivity = new BizUIActionActivity();
        public event EventHandler<SavedItemArg> ItemSaved;
        int ActionActivityID { set; get; }
        //string Catalog { set; get; }
        int EntityID { set; get; }
        //Enum_CodeFunctionParamType CodeFunctionParamType { set; get; }
        List<ColumnValueValidValuesDTO> ValidValues = new List<ColumnValueValidValuesDTO>();
        //bool VisualActions { set; get; }
        //bool IncludeSteps { set; get; }
        //فرم جدا هم صدا زده شود 
        public frmUIActionActivity(int actionActivityID, int entityID)
        {
            InitializeComponent();
            //EntityID = entityID;
            //Catalog = catalog;


            //IncludeSteps = includeSteps;
            //   SetAllowedActivityTypes();

            ActionActivityID = actionActivityID;
            EntityID = entityID;
            //CodeFunctionParamType = codeFunctionParamType;
            //if(EntityID==0)
            //{
            //    optRelationshipEnablity.Visibility = Visibility.Collapsed;
            //    tabRelationshipEnablity.Visibility = Visibility.Collapsed;
            //    optColumnValue.Visibility = Visibility.Collapsed;
            //    tabColumnValue.Visibility = Visibility.Collapsed;
            //}
            //else
            //{
            SetCombos();

            //if (ActionActivityTypes.Contains(Enum_ActionActivityType.CodeFunction))
            //    SetCodeFunctions();
            if (ActionActivityID == 0)
            {
                Message = new UIActionActivityDTO();
                dtgDetails.ItemsSource = Message.UIEnablityDetails;
                dtgColumnValue.ItemsSource = Message.UIColumnValue;
                dtgColumnValueRange.ItemsSource = Message.UIColumnValueRange;
                dtgColumnValueRangeReset.ItemsSource = Message.UIColumnValueRangeReset;
            }
            else
                GetActionActivity(ActionActivityID);
            ControlHelper.GenerateContextMenu(dtgDetails);
            ControlHelper.GenerateContextMenu(dtgColumnValue);
            ControlHelper.GenerateContextMenu(dtgColumnValueRange);
            ControlHelper.GenerateContextMenu(dtgColumnValueRangeReset);
            dtgColumnValueRange.CellEditEnded += DtgColumnValueRange_CellEditEnded;
            dtgColumnValueRange.RowLoaded += DtgColumnValueRange_RowLoaded;
        }

        private void DtgColumnValueRange_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (e.Row != null && e.Row.DataContext is UIColumnValueRangeDTO)
            {
                var item = e.Row.DataContext as UIColumnValueRangeDTO;
                if (item.ColumnValueRangeID != 0)
                {
                    BizColumnValueRange bizColumnValueRange = new BizColumnValueRange();
                    var list = bizColumnValueRange.GetColumnValueRangeValues(item.ColumnValueRangeID, item.EnumTag);
                    item.vwCandidateValues = list;
                }
            }
        }

        private void DtgColumnValueRange_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            if (e.Cell.ParentRow != null && e.Cell.ParentRow.DataContext is UIColumnValueRangeDTO)
            {
                var item = e.Cell.ParentRow.DataContext as UIColumnValueRangeDTO;
                if (item.ColumnValueRangeID != 0)
                {
                    BizColumnValueRange bizColumnValueRange = new BizColumnValueRange();
                    var list = bizColumnValueRange.GetColumnValueRangeValues(item.ColumnValueRangeID, item.EnumTag);
                    item.vwCandidateValues = list;
                }

            }
        }

        //private void SetAllowedActivityTypes()
        //{

        //}

        private void SetCombos()
        {

            //optCode.Visibility = Visibility.Collapsed;
            //tabCode.Visibility = Visibility.Collapsed;

            //optCode.Visibility = Visibility.Collapsed;
            //tabCode.Visibility = Visibility.Collapsed;


            //chkReslutSensetive.Visibility = Visibility.Collapsed;

            //SetRelationshipEnablityRelationshipTails();
            SetColumnValueColumns();
            SetUIEnablityColumns();
            SetUIColumnValueRangeColumns();

            dtgColumnValue.ItemsSource = ValidValues;
        }

        private void SetUIColumnValueRangeColumns()
        {
             TableDrivedEntityDTO targetEntity = bizTableDrivedEntity.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID, EntityColumnInfoType.WithFullColumns, EntityRelationshipInfoType.WithoutRelationships);
            colColumnValueRange.ItemsSource = targetEntity.Columns.Where(x => x.ColumnValueRange != null);
            colColumnValueRange.DisplayMemberPath = "Alias";
            colColumnValueRange.SelectedValueMemberPath = "ID";

            colEnumTag.ItemsSource = Enum.GetValues(typeof(EnumColumnValueRangeTag));

            colColumnValueRangeReset.ItemsSource = targetEntity.Columns.Where(x => x.ColumnValueRange != null);
            colColumnValueRangeReset.DisplayMemberPath = "Alias";
            colColumnValueRangeReset.SelectedValueMemberPath = "ID";

        }










        //private void SetColumnValueRelationshipTails()
        //{
        //    cmbColumnValueRelationshipTail.DisplayMemberPath = "RelationshipPath ";
        //    cmbColumnValueRelationshipTail.SelectedValuePath = "ID";
        //    BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
        //    cmbColumnValueRelationshipTail.ItemsSource = bizEntityRelationshipTail.GetEntityRelationshipTails(EntityID);
        //    SetColumns();


        //}
        //private void SetRelationshipEnablityRelationshipTails()
        //{
        //    BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
        //    cmbRelationshipEnablityRelationshipTail.DisplayMemberPath = "RelationshipPath";
        //    cmbRelationshipEnablityRelationshipTail.SelectedValuePath = "ID";
        //    cmbRelationshipEnablityRelationshipTail.ItemsSource = bizEntityRelationshipTail.GetEntityRelationshipTails(EntityID);
        //}

        //private void SetUIEnablityRelationshipTails()
        //{
        //    //cmbUIEnablityRelationshipTail.DisplayMemberPath = "RelationshipPath";
        //    //cmbUIEnablityRelationshipTail.SelectedValuePath = "ID";
        //    //BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
        //    //cmbUIEnablityRelationshipTail.ItemsSource = bizEntityRelationshipTail.GetEntityRelationshipTails(EntityID);
        //    SetUIEnablityColumns();
        ////    SetUIEnablityUICompositions();

        //}
        //private void SetActionActivityType()
        //{
        //    cmbActionActivityType.ItemsSource = Enum.GetValues(typeof(Enum_ActionActivityType));
        //}


        private void GetActionActivity(int actionActivityID)
        {
            Message = bizActionActivity.GetActionActivity(actionActivityID);
            ShowMessage();
        }

        private void ShowMessage()
        {
            txtTitle.Text = Message.Title;

            dtgDetails.ItemsSource = Message.UIEnablityDetails;
            dtgColumnValue.ItemsSource = Message.UIColumnValue;
            dtgColumnValueRange.ItemsSource = Message.UIColumnValueRange;
            dtgColumnValueRangeReset.ItemsSource = Message.UIColumnValueRangeReset;
            if (Message.Type == Enum_ActionActivityType.ColumnValue)
            {
                optColumnValue.IsChecked = true;
                //cmbColumnValueRelationshipTail.SelectedValue = Message.ColumnValue.EntityRelationshipTailID;
                //cmbColumns.SelectedValue = Message.ColumnValue.ColumnID;
                //txtColumnValue.Text = Message.ColumnValue.ExactValue;
                //ValidValues = Message.ColumnValue.ValidValues;
            }
            if (Message.Type == Enum_ActionActivityType.UIEnablity)
            {
                optUIEnablity.IsChecked = true;
                //cmbUIEnablityRelationshipTail.SelectedValue = Message.UIEnablity.EntityRelationshipTailID;

            }
            if (Message.Type == Enum_ActionActivityType.ColumnValueRange)
            {
                optUIColumnValueRange.IsChecked = true;
                //cmbUIEnablityRelationshipTail.SelectedValue = Message.UIEnablity.EntityRelationshipTailID;

            }
            if (Message.Type == Enum_ActionActivityType.ColumnValueRangeReset)
            {
                optUIColumnValueRangeReset.IsChecked = true;
                //cmbUIEnablityRelationshipTail.SelectedValue = Message.UIEnablity.EntityRelationshipTailID;

            }
            //if (Message.Type == Enum_ActionActivityType.RelationshipEnablity)
            //{
            //    optRelationshipEnablity.IsChecked = true;
            //    cmbRelationshipEnablityRelationshipTail.SelectedValue = Message.RelationshipEnablity.EntityRelationshipTailID;
            //    if (Message.RelationshipEnablity.Enable == true)
            //        optRelationshipEnable.IsChecked = true;
            //    else if (Message.RelationshipEnablity.Enable == false)
            //        optRelationshipDisable.IsChecked = true;
            //    else
            //    {
            //        optRelationshipEnable.IsChecked = false;
            //        optRelationshipDisable.IsChecked = false;
            //    }

            //    if (Message.RelationshipEnablity.Readonly == true)
            //        optRelationshipReadonly.IsChecked = true;
            //    else if (Message.RelationshipEnablity.Readonly == false)
            //        optRelationshipNotReadonly.IsChecked = true;
            //    else
            //    {
            //        optRelationshipReadonly.IsChecked = false;
            //        optRelationshipNotReadonly.IsChecked = false;
            //    }

            //}
        }


        private void HideTabs()
        {

            tabColumnValue.Visibility = Visibility.Collapsed;

            tabUIEnablity.Visibility = Visibility.Collapsed;
            tabUIColumnValueRange.Visibility = Visibility.Collapsed;
            tabUIColumnValueRangeReset.Visibility = Visibility.Collapsed;
            //    tabRelationshipEnablity.Visibility = Visibility.Collapsed;
        }


        //private void GetCodePath(int codeFunctionID)
        //{
        //    BizCodeFunction bizCodeFunction = new BizCodeFunction();
        //    var codeFunction = bizCodeFunction.GetCodeFunction(codeFunctionID);
        //    txtCodePath.Text = codeFunction.Path;
        //}


        private void optColumnValue_Checked(object sender, RoutedEventArgs e)
        {
            HideTabs();
            tabColumnValue.Visibility = Visibility.Visible;
            tabColumnValue.IsSelected = true;
        }
        private void optUIEnablity_Checked(object sender, RoutedEventArgs e)
        {
            HideTabs();
            tabUIEnablity.Visibility = Visibility.Visible;
            tabUIEnablity.IsSelected = true;
        }
        private void optUIColumnValueRange_Checked(object sender, RoutedEventArgs e)
        {
            HideTabs();
            tabUIColumnValueRange.Visibility = Visibility.Visible;
            tabUIColumnValueRange.IsSelected = true;
        }
        //private void optRelationshipEnablity_Checked(object sender, RoutedEventArgs e)
        //{
        //    HideTabs();
        //    //tabRelationshipEnablity.Visibility = Visibility.Visible;
        //    //tabRelationshipEnablity.IsSelected = true;
        //}



        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtTitle.Text == "")
            {
                MessageBox.Show("عنوان مناسب تعریف نشده است");
                return;
            }

            if (optColumnValue.IsChecked == false && optUIEnablity.IsChecked == false
                && optUIColumnValueRange.IsChecked == false && optUIColumnValueRangeReset.IsChecked == false)
            {
                MessageBox.Show("یکی از حالات شروط را انتخاب نمایید");
                return;
            }


            else if (optUIEnablity.IsChecked == true)
            {

            }
            Message.EntityID = EntityID;
            Message.Title = txtTitle.Text;

            if (optColumnValue.IsChecked == true)
            {
                Message.Type = Enum_ActionActivityType.ColumnValue;
            }
            else if (optUIEnablity.IsChecked == true)
            {
                Message.Type = Enum_ActionActivityType.UIEnablity;
            }
            else if (optUIColumnValueRange.IsChecked == true)
            {
                Message.Type = Enum_ActionActivityType.ColumnValueRange;
            }
            else if (optUIColumnValueRangeReset.IsChecked == true)
            {
                Message.Type = Enum_ActionActivityType.ColumnValueRangeReset;
            }
            Message.ID = bizActionActivity.UpdateActionActivitys(Message);
            if (ItemSaved != null)
                ItemSaved(this, new SavedItemArg() { ID = Message.ID });
            MessageBox.Show("اطلاعات ثبت شد");


        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Message = new UIActionActivityDTO();
            ShowMessage();
        }

        //private void btnSearch_Click(object sender, RoutedEventArgs e)
        //{
        //    frmActionActivitySelect view = new MyProject_WPF.frmActionActivitySelect(Catalog);
        //    view.ActionActivitySelected += View_ActionActivitySelected;
        //       MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form");
        //}

        //private void View_ActionActivitySelected(object sender, ActionActivitySelectedArg e)
        //{
        //    if (e.ActionActivityID != 0)
        //    {
        //        GetActionActivity(e.ActionActivityID);
        //    }
        //}


        //private void btnRelationshipTail_Click(object sender, RoutedEventArgs e)
        //{
        //    frmEntityRelationshipTail view = new frmEntityRelationshipTail(EntityID);
        //    view.ItemSelected += View_ItemSelected;
        //    MyProjectManager.GetMyProjectManager.ShowDialog(view, "رابطه های مرتبط");
        //}

        //private void View_ItemSelected(object sender, EntityRelationshipTailSelectedArg e)
        //{
        //    SetColumnValueRelationshipTails();
        //    cmbColumnValueRelationshipTail.SelectedValue = e.EntityRelationshipTailID;
        //}
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        //private void cmbRelationshipTail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    SetColumnValueColumns();
        //}

        private void SetColumnValueColumns()
        {
            TableDrivedEntityDTO targetEntity = null;
            //if (cmbColumnValueRelationshipTail.SelectedItem != null)
            //{
            //    var item = cmbColumnValueRelationshipTail.SelectedItem as EntityRelationshipTailDTO;
            //    targetEntity = bizTableDrivedEntity.GetTableDrivedEntity(item.RelationshipTargetEntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);

            //}
            //else
            targetEntity = bizTableDrivedEntity.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);

            colColumnValueColumn.ItemsSource = targetEntity.Columns;
            colColumnValueColumn.DisplayMemberPath = "Alias";
            colColumnValueColumn.SelectedValueMemberPath = "ID";
        }

        //private void btnRelationshipEnablityRelationshipTail_Click(object sender, RoutedEventArgs e)
        //{
        //    frmEntityRelationshipTail view = new frmEntityRelationshipTail(EntityID);
        //    view.ItemSelected += View_ItemSelected1;
        //    MyProjectManager.GetMyProjectManager.ShowDialog(view, "رابطه های مرتبط");
        //}

        //private void View_ItemSelected1(object sender, EntityRelationshipTailSelectedArg e)
        //{
        //    SetRelationshipEnablityRelationshipTails();
        //    cmbRelationshipEnablityRelationshipTail.SelectedValue = e.EntityRelationshipTailID;
        //}

        //private void optUIEnablityColumn_Checked(object sender, RoutedEventArgs e)
        //{
        //    cmbUIEnablityColumn.IsEnabled = false;
        //    optUIEnablityColumnEnable.IsEnabled = false;
        //    optUIEnablityColumnDisable.IsEnabled = false;
        //    cmbUIEnablityUIComposition.IsEnabled = false;
        //    optUIEnablityUICompositionEnable.IsEnabled = false;
        //    optUIEnablityUICompositionDisable.IsEnabled = false;
        //    if (optUIEnablityColumn.IsChecked == true)
        //    {
        //        cmbUIEnablityColumn.IsEnabled = true;
        //        optUIEnablityColumnEnable.IsEnabled = true;
        //        optUIEnablityColumnDisable.IsEnabled = true;
        //    }
        //    else if (optUIEnablityUIComposition.IsChecked == true)
        //    {
        //        cmbUIEnablityUIComposition.IsEnabled = true;
        //        optUIEnablityUICompositionEnable.IsEnabled = true;
        //        optUIEnablityUICompositionDisable.IsEnabled = true;
        //    }

        //}

        private void cmbUIEnablityRelationshipTail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetUIEnablityColumns();
        }

        private void SetUIEnablityColumns()
        {
            TableDrivedEntityDTO targetEntity = null;
            //if (cmbUIEnablityRelationshipTail.SelectedItem != null)
            //{
            //    var item = cmbUIEnablityRelationshipTail.SelectedItem as EntityRelationshipTailDTO;
            //    targetEntity = bizTableDrivedEntity.GetTableDrivedEntity(item.RelationshipTargetEntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
            //}
            //else
            targetEntity = bizTableDrivedEntity.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithRelationships);


            colColumn.ItemsSource = targetEntity.Columns;
            colColumn.DisplayMemberPath = "Alias";
            colColumn.SelectedValueMemberPath = "ID";

            colRelationship.ItemsSource = targetEntity.Relationships;
            colRelationship.DisplayMemberPath = "Alias";
            colRelationship.SelectedValueMemberPath = "ID";

            BizEntityUIComposition bizEntityUIComposition = new BizEntityUIComposition();
            var uiCompositions = bizEntityUIComposition.GetListEntityUIComposition(targetEntity.ID);

            colUIComposition.DisplayMemberPath = "Title";
            colUIComposition.SelectedValueMemberPath = "ID";
            colUIComposition.ItemsSource = uiCompositions;
        }

        private void optUIColumnValueRangeReset_Checked(object sender, RoutedEventArgs e)
        {
            HideTabs();
            tabUIColumnValueRangeReset.Visibility = Visibility.Visible;
            tabUIColumnValueRangeReset.IsSelected = true;
        }




        //private void btnUIEnablityRelationshipTail_Click(object sender, RoutedEventArgs e)
        //{
        //    frmEntityRelationshipTail view = new frmEntityRelationshipTail(EntityID);
        //    view.ItemSelected += View_ItemSelectedUIEnablity;
        //    MyProjectManager.GetMyProjectManager.ShowDialog(view, "رابطه های مرتبط");
        //}

        //private void View_ItemSelectedUIEnablity(object sender, EntityRelationshipTailSelectedArg e)
        //{
        //    SetUIEnablityRelationshipTails();
        //    cmbUIEnablityRelationshipTail.SelectedValue = e.EntityRelationshipTailID;
        //}


    }
    public class SavedItemArg : EventArgs
    {
        public int ID { set; get; }
    }

}
