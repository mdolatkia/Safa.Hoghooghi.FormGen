using ModelEntites;
using MyCommonWPFControls;
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
using Telerik.Windows.Controls;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmEntitySearch.xaml
    /// </summary>
    /// 

    public partial class frmEntitySearch : UserControl
    {
        BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();

        private EntitySearchDTO Message { set; get; }
        BizEntitySearch bizEntitySearch = new BizEntitySearch();
        int EntityID { set; get; }
        int EntitySearchID { set; get; }
        public event EventHandler<EntitySearchUpdatedArg> EntitySearchUpdated;
        public frmEntitySearch(int entityID, int entitySearchID)
        {
            InitializeComponent();
            EntityID = entityID;
            EntitySearchID = entitySearchID;
            SetRelationshipTails();
            if (EntitySearchID == 0)
            {
                Message = new EntitySearchDTO();
                ShowMessage();
            }
            else
                GetEntityEntitySearch(EntitySearchID);
            dtgColumns.CellEditEnded += DtgConditions_CellEditEnded;
            dtgColumns.RowLoaded += DtgColumns_RowLoaded;
            colRelationshipTail.EditItemClicked += ColRelationshipTail_EditItemClicked;
            ControlHelper.GenerateContextMenu(dtgColumns);
        }

        private void DtgColumns_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (e.DataElement is EntitySearchColumnsDTO)
            {
                var data = (e.DataElement as EntitySearchColumnsDTO);
                if (data.vwValueColumns == null || data.vwValueColumns.Count == 0)
                    SetColumns(data);
            }
        }

        private void DtgConditions_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            if (e.Cell.Column == colRelationshipTail)
            {
                if (e.Cell.DataContext is EntitySearchColumnsDTO)
                {
                    var condition = (e.Cell.DataContext as EntitySearchColumnsDTO);
                    SetColumns(condition);
                }
            }
        }

        private void SetColumns(EntitySearchColumnsDTO condition)
        {
            colColumns.DisplayMemberPath = "Name";
            colColumns.SelectedValueMemberPath = "ID";
            BizTableDrivedEntity biz = new BizTableDrivedEntity();
            if (condition.RelationshipTailID == 0)
            {
                var entity = biz.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
                condition.vwValueColumns = entity.Columns; ;
            }
            else
            {
                var relationshipTail = bizEntityRelationshipTail.GetEntityRelationshipTail(MyProjectManager.GetMyProjectManager.GetRequester(), condition.RelationshipTailID);
                var entity = biz.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), relationshipTail.TargetEntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
                condition.vwValueColumns = entity.Columns;
            }
        }

        private void ColRelationshipTail_EditItemClicked(object sender, MyCommonWPFControls.EditItemClickEventArg e)
        {
            frmEntityRelationshipTail frm = null;
            frm = new frmEntityRelationshipTail(EntityID);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "رابطه های مرتبط");
            frm.ItemSelected += (sender1, e1) => Frm_TailSelected(sender1, e1, (sender as MyStaticLookup));
        }
        private void Frm_TailSelected(object sender1, EntityRelationshipTailSelectedArg e1, MyStaticLookup myStaticLookup)
        {
            SetRelationshipTails();
            myStaticLookup.SelectedValue = e1.EntityRelationshipTailID;
        }
        private void SetRelationshipTails()
        {
            BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
            colRelationshipTail.DisplayMemberPath = "EntityPath";
            colRelationshipTail.SelectedValueMemberPath = "ID";
            colRelationshipTail.ItemsSource = bizEntityRelationshipTail.GetEntityRelationshipTails(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID);

        }
        private void GetEntityEntitySearch(int entitySearchID)
        {
            Message = bizEntitySearch.GetEntitySearch(MyProjectManager.GetMyProjectManager.GetRequester(), entitySearchID);
            ShowMessage();
        }
        private void ShowMessage()
        {
            txtViewName.Text = Message.Title;
            //foreach (var item in Message.EntitySearchAllColumns)
            //{
            //    SetColumns(item);
            //}
            dtgColumns.ItemsSource = Message.EntitySearchAllColumns;
        }



        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtViewName.Text == "")
            {
                MessageBox.Show("عنوان مناسب تعریف نشده است");
                return;
            }
            foreach (var item in Message.EntitySearchAllColumns)
            {
                if (item.RelationshipTailID != 0)
                {
                    var relationshipTail = bizEntityRelationshipTail.GetEntityRelationshipTail(MyProjectManager.GetMyProjectManager.GetRequester(), item.RelationshipTailID);
                    BizTableDrivedEntity bizTableDrivedEntity = new MyModelManager.BizTableDrivedEntity();
                    var entity = bizTableDrivedEntity.GetSimpleEntity(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID); ;
                    var linkedServerMessage = bizEntityRelationshipTail.CheckLinkedServers(entity, relationshipTail);
                    if (linkedServerMessage != "")
                    {
                        var message = "اشکال در تعریف لینک سرور برای ستون" + " " + (string.IsNullOrEmpty(item.Alias) ? item.ColumnID.ToString() : item.Alias);
                        message += Environment.NewLine + linkedServerMessage;
                        MessageBox.Show(message);
                        return;
                    }
                }
            }
            foreach (var item in Message.EntitySearchAllColumns)
            {
                if (item.RelationshipTailID != 0)
                {
                    if (item.ColumnID == 0)
                    {
                        BizTableDrivedEntity biz = new BizTableDrivedEntity();
                        var relationshipTail = bizEntityRelationshipTail.GetEntityRelationshipTail(MyProjectManager.GetMyProjectManager.GetRequester(), item.RelationshipTailID);
                        var entity = biz.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), relationshipTail.TargetEntityID, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships);
                        if (entity.IsView)
                        {
                            MessageBox.Show("به منظور استفاده از رشته رابطه"+" "+ relationshipTail.EntityPath+" "+"، به علت منتهی شدن با نما، انتخاب ستون هدف اجباری می باشد");
                            return;
                        }
                    }
                }
            }
            Message.TableDrivedEntityID = EntityID;
            Message.Title = txtViewName.Text;
            Message.ID = bizEntitySearch.UpdateEntitySearchs(Message);
            if (EntitySearchUpdated != null)
                EntitySearchUpdated(this, new MyProject_WPF.EntitySearchUpdatedArg() { ID = Message.ID });
            MessageBox.Show("اطلاعات ثبت شد");
        }



        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Message = new EntitySearchDTO();
            ShowMessage();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            frmEntitySearchSelect view = new MyProject_WPF.frmEntitySearchSelect(EntityID);
            view.EntitySearchSelected += View_EntitySearchSelected1;
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form",Enum_WindowSize.Big);
        }

        private void View_EntitySearchSelected1(object sender, EntitySearchSelectedArg e)
        {
            if (e.EntitySearchID != 0)
            {
                MyProjectManager.GetMyProjectManager.CloseDialog(sender);

                GetEntityEntitySearch(e.EntitySearchID);
            }
        }


    }
    public class EntitySearchUpdatedArg : EventArgs
    {
        public int ID { set; get; }
    }
}
