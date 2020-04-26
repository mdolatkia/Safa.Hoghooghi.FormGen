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
    /// Interaction logic for frmEntityListView.xaml
    /// </summary>
    /// 

    public partial class frmEntityListView : UserControl
    {
        BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();

        private EntityListViewDTO Message { set; get; }
        BizEntityListView bizEntityListView = new BizEntityListView();
        int EntityID { set; get; }
        int EntityListViewID { set; get; }
        public event EventHandler<EntityListViewUpdatedArg> EntityListViewUpdated;
        public frmEntityListView(int entityID, int entityListViewID)
        {
            InitializeComponent();
            EntityID = entityID;
            BizTableDrivedEntity biz = new BizTableDrivedEntity();
            var entity = biz.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
            if (entity.IsView)
                colRelationshipTail.IsVisible = false;
            EntityListViewID = entityListViewID;
            SetRelationshipTails();
            if (EntityListViewID == 0)
            {
                Message = new EntityListViewDTO();
                ShowMessage();
            }
            else
                GetEntityEntityListView(EntityListViewID);
            dtgColumns.RowLoaded += DtgColumns_RowLoaded;
            dtgColumns.CellEditEnded += DtgConditions_CellEditEnded;
            colRelationshipTail.EditItemClicked += ColRelationshipTail_EditItemClicked;
            ControlHelper.GenerateContextMenu(dtgColumns);

        }

        private void DtgColumns_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (e.DataElement is EntityListViewColumnsDTO)
            {
                var data = (e.DataElement as EntityListViewColumnsDTO);
                if (data.vwValueColumns == null || data.vwValueColumns.Count == 0)
                    SetColumns(data);
            }
        }

        private void DtgConditions_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            if (e.Cell.Column == colRelationshipTail)
            {
                if (e.Cell.DataContext is EntityListViewColumnsDTO)
                {
                    var condition = (e.Cell.DataContext as EntityListViewColumnsDTO);
                    SetColumns(condition);
                }
            }
        }

        private void SetColumns(EntityListViewColumnsDTO condition)
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

        private void GetEntityEntityListView(int entityListViewID)
        {
            Message = bizEntityListView.GetEntityListView(MyProjectManager.GetMyProjectManager.GetRequester(), entityListViewID);
            ShowMessage();
        }
        private void ShowMessage()
        {
            if (Message.ID == 0)
                btnSetDefaultListView.IsEnabled = false;
            else
                btnSetDefaultListView.IsEnabled = true;
            txtViewName.Text = Message.Title;
            foreach (var item in Message.EntityListViewAllColumns)
            {
                SetColumns(item);
            }
            dtgColumns.ItemsSource = Message.EntityListViewAllColumns;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtViewName.Text == "")
            {
                MessageBox.Show("عنوان مناسب تعریف نشده است");
                return;
            }

            foreach (var item in Message.EntityListViewAllColumns)
            {
                if (item.RelationshipTailID != 0)
                {
                    var relationshipTail = bizEntityRelationshipTail.GetEntityRelationshipTail(MyProjectManager.GetMyProjectManager.GetRequester(), item.RelationshipTailID);
                    BizTableDrivedEntity bizTableDrivedEntity = new MyModelManager.BizTableDrivedEntity();
                    var entity = bizTableDrivedEntity.GetSimpleEntity(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID);
                    var linkedServerMessage = bizEntityRelationshipTail.CheckLinkedServers(entity, relationshipTail);
                    if (linkedServerMessage != "")
                    {
                        var message = "اشکال در تعریف لینک سرور برای ستون" + " " + (string.IsNullOrEmpty(item.Alias) ? item.ColumnID.ToString() : item.Alias);
                        message += Environment.NewLine + linkedServerMessage;
                        MessageBox.Show(message);
                        return;
                    }
                    if (relationshipTail.IsOneToManyTail)
                    {
                        var message = "رشته رابطه برای ستون" + " " + (string.IsNullOrEmpty(item.Alias) ? item.ColumnID.ToString() : item.Alias) + " " + "نمی تواند یک به چند باشد";
                        MessageBox.Show(message);
                        return;
                    }
                }
            }

            Message.TableDrivedEntityID = EntityID;
            Message.Title = txtViewName.Text;
            Message.ID = bizEntityListView.UpdateEntityListViews(Message);
            if (EntityListViewUpdated != null)
                EntityListViewUpdated(this, new MyProject_WPF.EntityListViewUpdatedArg() { ID = Message.ID });
            MessageBox.Show("اطلاعات ثبت شد");
        }



        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Message = new EntityListViewDTO();
            ShowMessage();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            frmEntityListViewSelect view = new MyProject_WPF.frmEntityListViewSelect(EntityID);
            view.EntityListViewSelected += View_EntityListViewSelected1;
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "frmEntityListViewSelect", Enum_WindowSize.Big);
        }

        private void View_EntityListViewSelected1(object sender, EntityListViewSelectedArg e)
        {
            if (e.EntityListViewID != 0)
            {
                MyProjectManager.GetMyProjectManager.CloseDialog(sender);
                GetEntityEntityListView(e.EntityListViewID);
            }
        }

        private void btnSetDefaultListView_Click(object sender, RoutedEventArgs e)
        {
            if (bizEntityListView.SetDefaultListView(EntityID, Message.ID))
                MessageBox.Show("لیست پیش فرض موجودیت تعیین شد");
        }
    }
    public class EntityListViewUpdatedArg : EventArgs
    {
        public int ID { set; get; }
    }
}
