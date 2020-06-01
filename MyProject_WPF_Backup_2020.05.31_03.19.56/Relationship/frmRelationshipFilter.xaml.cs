using ModelEntites;
using MyCommonWPFControls;
using MyGeneralLibrary;
using MyModelManager;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for frmCodeSelector.xaml
    /// </summary>
    public partial class frmRelationshipFilter : UserControl
    {
        BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();

        //public event EventHandler<RelationshipFilterSelectedArg> RelationshipFilterSelected;
        BizRelationshipFilter bizRelationshipFilter = new BizRelationshipFilter();
        RelationshipDTO Relatoinship { set; get; }
        int RelatoinshipID { set; get; }
        RelationshipFilterDTO Message { set; get; }
        public frmRelationshipFilter(int relatoinshipID)
        {
            InitializeComponent();
            RelatoinshipID = relatoinshipID;
            BizRelationship bizRelationship = new BizRelationship();
            Relatoinship = bizRelationship.GetRelationship(relatoinshipID);
            //SetSearchRelationshipTails();
            SetValueRelationshipTails();
            //SetSearchRelationshipTails();
            var list = bizRelationshipFilter.GetRelationshipFilters(MyProjectManager.GetMyProjectManager.GetRequester(), relatoinshipID);
            foreach (var item in list)
            {
                SetValueColumns(item);
                SetSearchColumns(item);
            }
            dtgRelationshipFilterColumns.ItemsSource = list;

            dtgRelationshipFilterColumns.RowLoaded += DtgConditions_RowLoaded;
            dtgRelationshipFilterColumns.CellEditEnded += DtgConditions_CellEditEnded;
            colValueRelationshipTail.EditItemClicked += ColValueRelationshipTail_EditItemClicked;
         //   colSearchRelationshipTail.EditItemClicked += ColSearchRelationshipTail_EditItemClicked;
            ControlHelper.GenerateContextMenu(dtgRelationshipFilterColumns);
        }
        private void DtgConditions_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (e.DataElement is RelationshipFilterDTO)
            {
                var condition = (e.DataElement as RelationshipFilterDTO);
                SetValueColumns(condition);
                SetSearchColumns(condition);
            }
        }
        private void DtgConditions_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            if (e.Cell.Column == colValueRelationshipTail)
            {
                if (e.Cell.DataContext is RelationshipFilterDTO)
                {
                    var condition = (e.Cell.DataContext as RelationshipFilterDTO);
                    SetValueColumns(condition);
                }
            }
            //if (e.Cell.Column == colSearchRelationshipTail)
            //{
            //    if (e.Cell.DataContext is RelationshipFilterDTO)
            //    {
            //        var condition = (e.Cell.DataContext as RelationshipFilterDTO);
            //        SetSearchColumns(condition);
            //    }
            //}
        }

        private void SetValueColumns(RelationshipFilterDTO condition)
        {
            colValueColumns.DisplayMemberPath = "Name";
            colValueColumns.SelectedValueMemberPath = "ID";
            BizTableDrivedEntity biz = new BizTableDrivedEntity();

            if (condition.ValueRelationshipTailID == 0)
            {
                var entity = biz.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), Relatoinship.EntityID1, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
                condition.vwValueColumns = entity.Columns; ;
            }
            else
            {
                var relationshipTail = bizEntityRelationshipTail.GetEntityRelationshipTail(MyProjectManager.GetMyProjectManager.GetRequester(), condition.ValueRelationshipTailID);
                var entity = biz.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), relationshipTail.TargetEntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
                condition.vwValueColumns = entity.Columns;
            }
        }
        private void ColValueRelationshipTail_EditItemClicked(object sender, MyCommonWPFControls.EditItemClickEventArg e)
        {
            frmEntityRelationshipTail frm = null;
            frm = new frmEntityRelationshipTail(Relatoinship.EntityID1);

            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "رابطه های مرتبط");
            frm.ItemSelected += (sender1, e1) => Frm_TailSelected(sender1, e1, (sender as MyStaticLookup));
        }
        private void Frm_TailSelected(object sender1, EntityRelationshipTailSelectedArg e1, MyStaticLookup myStaticLookup)
        {
            SetValueRelationshipTails();
            myStaticLookup.SelectedValue = e1.EntityRelationshipTailID;
        }


        private void SetSearchColumns(RelationshipFilterDTO condition)
        {
            colSearchColumns.DisplayMemberPath = "Name";
            colSearchColumns.SelectedValueMemberPath = "ID";
            BizTableDrivedEntity biz = new BizTableDrivedEntity();

            if (condition.SearchRelationshipTailID == 0)
            {
                var entity = biz.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), Relatoinship.EntityID2, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
                condition.vwSearchColumns = entity.Columns; ;
            }
            else
            {
                var relationshipTail = bizEntityRelationshipTail.GetEntityRelationshipTail(MyProjectManager.GetMyProjectManager.GetRequester(), condition.SearchRelationshipTailID);
                var entity = biz.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), relationshipTail.TargetEntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
                condition.vwSearchColumns = entity.Columns;
            }
        }
        //private void ColSearchRelationshipTail_EditItemClicked(object sender, MyCommonWPFControls.EditItemClickEventArg e)
        //{
        //    frmEntityRelationshipTail frm = null;
        //    frm = new frmEntityRelationshipTail(Relatoinship.EntityID2);

        //    MyProjectManager.GetMyProjectManager.ShowDialog(frm, "رابطه های مرتبط");
        //    frm.ItemSelected += (sender1, e1) => Frm_TailSelected(sender1, e1, (sender as MyStaticLookup));
        //}
        //private void Frm_SearchTailSelected(object sender1, EntityRelationshipTailSelectedArg e1, MyStaticLookup myStaticLookup)
        //{
        //    SetSearchRelationshipTails();
        //    myStaticLookup.SelectedValue = e1.EntityRelationshipTailID;
        //}
        //private void SetSearchRelationshipTails()
        //{
        //    if (cmbSearchRelationshipTail.DisplayMember == null)
        //    {
        //        //cmbSearchRelationshipTail.AllDataArePresent = true;
        //        cmbSearchRelationshipTail.DisplayMember = "RelationshipPath";
        //        cmbSearchRelationshipTail.SelectedValueMember = "ID";
        //        cmbSearchRelationshipTail.NewItemEnabled = true;
        //        //cmbSearchRelationshipTail.NewItemClicked += CmbSearchRelationshipTail_NewItemClicked;
        //    }
        //    BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
        //    cmbSearchRelationshipTail.ItemsSource = bizEntityRelationshipTail.GetEntityRelationshipTails(Relatoinship.EntityID2);
        //}

        //private void CmbSearchRelationshipTail_NewItemClicked(object sender, EventArgs e)
        //{
        //    frmEntityRelationshipTail view = new frmEntityRelationshipTail(Relatoinship.EntityID2);
        //    view.ItemSelected += View_ItemSelected;
        //    MyProjectManager.GetMyProjectManager.ShowDialog(view, "رابطه های مرتبط");
        //}
        //private void View_ItemSelected(object sender, EntityRelationshipTailSelectedArg e)
        //{
        //    SetSearchRelationshipTails();
        //    cmbSearchRelationshipTail.SelectedValue = e.EntityRelationshipTailID;
        //}

        private void SetValueRelationshipTails()
        {
            BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
            colValueRelationshipTail.DisplayMemberPath = "EntityPath";
            colValueRelationshipTail.SelectedValueMemberPath = "ID";
            colValueRelationshipTail.ItemsSource = bizEntityRelationshipTail.GetEntityRelationshipTails(MyProjectManager.GetMyProjectManager.GetRequester(), Relatoinship.EntityID1);

        }

        //private void SetSearchRelationshipTails()
        //{
        //    BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
        //    colSearchRelationshipTail.DisplayMemberPath = "EntityPath";
        //    colSearchRelationshipTail.SelectedValueMemberPath = "ID";
        //    colSearchRelationshipTail.ItemsSource = bizEntityRelationshipTail.GetEntityRelationshipTails(Relatoinship.EntityID2);

        //}
        //private void GetRelationshipFilter(int ID)
        //{
        //    Message = bizRelationshipFilter.GetRelationshipFilter(ID);
        //    ShowRelationshipFilter();
        //}
        //private void ShowRelationshipFilter()
        //{
        //    //  cmbSearchRelationshipTail.SelectedValue = Message.SearchRelationshipTailID;
        //    cmbValueRelationshipTail.SelectedValue = Message.ValueRelationshipTailID;
        //    chkEnable.IsChecked = Message.Enabled;
        //    //     dtgRelationshipFilterColumns.ItemsSource = Message.RelationshipFilterColumns;
        //}
        private void btnSaveAndSelect_Click(object sender, RoutedEventArgs e)
        {

            //باید رابطه ولیو یک به چند داخلش نباشد.چند به یک  میشود باشد
            bizRelationshipFilter.UpdateRelationshipFilters(RelatoinshipID, dtgRelationshipFilterColumns.ItemsSource as List<RelationshipFilterDTO>);
            MessageBox.Show("اطلاعات ثبت شد");

        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }



        //private void btnList_Click(object sender, RoutedEventArgs e)
        //{
        //    frmRelationshipFilterSelect view = new frmRelationshipFilterSelect(Relatoinship.ID);
        //    view.RelationshipFilterSelected += View_RelationshipFilterSelected;
        //    MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form");
        //}

        //private void View_RelationshipFilterSelected(object sender, RelationshipFilterSelectedArg e)
        //{
        //    if (e.ID != 0)
        //    {
        //        GetRelationshipFilter(e.ID);
        //    }
        //}






        //private void btnValueRelationshipTail_Click(object sender, RoutedEventArgs e)
        //{
        //    frmEntityRelationshipTail view = new frmEntityRelationshipTail(Relatoinship.EntityID1);
        //    view.ItemSelected += View_ItemSelected1;
        //    MyProjectManager.GetMyProjectManager.ShowDialog(view, "رابطه های مرتبط");
        //}

        //private void View_ItemSelected1(object sender, EntityRelationshipTailSelectedArg e)
        //{
        //    SetValueRelationshipTails();
        //    cmbValueRelationshipTail.SelectedValue = e.EntityRelationshipTailID;
        //}
        //BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        //private void cmbSearchRelationshipTail_SelectionChanged(object sender, MyCommonWPFControls.SelectionChangedArg e)
        //{
        //    var column = dtgRelationshipFilterColumns.Columns[0] as GridViewComboBoxColumn;
        //    TableDrivedEntityDTO targetEntity = null;
        //    if (cmbSearchRelationshipTail.SelectedItem != null)
        //    {
        //        var item = GetLastChild(cmbSearchRelationshipTail.SelectedItem as EntityRelationshipTailDTO);

        //        targetEntity = bizTableDrivedEntity.GetTableDrivedEntity(item.RelationshipTargetEntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
        //    }
        //    else
        //        targetEntity = bizTableDrivedEntity.GetTableDrivedEntity(Relatoinship.EntityID1, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);

        //    column.ItemsSource = targetEntity.Columns;
        //    column.DisplayMemberPath = "Alias";
        //    column.SelectedValueMemberPath = "ID";
        //}

        //private EntityRelationshipTailDTO GetLastChild(EntityRelationshipTailDTO entityRelationshipTailDTO)
        //{
        //    if (entityRelationshipTailDTO.ChildTail == null)
        //        return entityRelationshipTailDTO;
        //    else
        //        return GetLastChild(entityRelationshipTailDTO.ChildTail);
        //}

        //private void cmbValueRelationshipTail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (cmbSearchRelationshipTail.SelectedItem != null)
        //    {
        //        var column = dtgRelationshipFilterColumns.Columns[1] as GridViewComboBoxColumn;
        //        var item = GetLastChild(cmbSearchRelationshipTail.SelectedItem as EntityRelationshipTailDTO);
        //        var targetEntity = bizTableDrivedEntity.GetTableDrivedEntity(item.RelationshipTargetEntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
        //        column.ItemsSource = targetEntity.Columns;
        //        column.DisplayMemberPath = "Alias";
        //        column.SelectedValueMemberPath = "ID";
        //    }
        //}


    }


}
