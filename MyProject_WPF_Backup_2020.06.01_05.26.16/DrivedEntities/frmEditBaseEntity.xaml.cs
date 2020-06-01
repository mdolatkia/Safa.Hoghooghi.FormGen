using ModelEntites;
using MyModelManager;
using MyProject_WPF.Biz;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for frmEditEntity.xaml
    /// </summary>
    public partial class frmEditBaseEntity : UserControl
    {
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        int EntityID { set; get; }
        EditBaseEntityDTO Message { set; get; }
        public frmEditBaseEntity(int entityID)
        {
            InitializeComponent();
            EntityID = entityID;
            //  Inheritance = inheritance;
            //if (Inheritance)
            //{
            //}
            //else
            //{
            //    pnlDisjoint.Visibility = System.Windows.Visibility.Collapsed;
            //    pnlParticipation.Visibility = System.Windows.Visibility.Collapsed;
            //}
            Message = new EditBaseEntityDTO();
            //   SetBaseEntity();
            SetEntities();

        }

        private void SetEntities()
        {
            Message.BaseEntity = bizTableDrivedEntity.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithRelationships);
            lblEntityName.Text = Message.BaseEntity.Name;

            BizISARelationship bizISARelationship = new BizISARelationship();
            Message.ISARelationship = bizISARelationship.GetInternalTableISARelationships(EntityID);
            if (Message.ISARelationship != null)
            {
                optIsTolatParticipation.IsChecked = Message.ISARelationship.IsTolatParticipation;
                optIsDisjoint.IsChecked = Message.ISARelationship.IsDisjoint;
            }
            else
            {
                Message.ISARelationship = new ISARelationshipDTO();
            }

            if (Message.ISARelationship.ID != 0)
            {
                var drivedEntities = bizTableDrivedEntity.GetOtherDrivedEntityIDs(Message.ISARelationship);
                foreach (var id in drivedEntities)
                {
                    var entity = bizTableDrivedEntity.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), id, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithRelationships);

                    var removeRelationships = entity.Relationships.Where(x => (x.TypeEnum == Enum_RelationshipType.SuperToSub ||
                         x.TypeEnum == Enum_RelationshipType.SubToSuper) && x.TableID1 == x.TableID2).ToList();
                    foreach (var item in removeRelationships)
                        entity.Relationships.Remove(item);



                    AddDrivedEntityTab(entity);
                }

            }
            var removeRelationshipsFromBase = Message.BaseEntity.Relationships.Where(x => (x.TypeEnum == Enum_RelationshipType.SuperToSub ||
       x.TypeEnum == Enum_RelationshipType.SubToSuper) && x.TableID1 == x.TableID2).ToList();
            foreach (var item in removeRelationshipsFromBase)
                Message.BaseEntity.Relationships.Remove(item);

            dtgRelationships.ItemsSource = Message.BaseEntity.Relationships;
            dtgColumns.ItemsSource = Message.BaseEntity.Columns;

        }

        private void AddDrivedEntityTab(TableDrivedEntityDTO item)
        {
            Message.DrivedEntities.Add(item);
            frmEditSubEntity view = new frmEditSubEntity(item, Message.BaseEntity.Columns);

            TabItem tab = new TabItem();
            tab.Header = item.Name;
            tab.Content = view;
            tabControl.Items.Add(tab);
            tab.IsSelected = true;
            view.NameChanged += (sender, e) => View_NameChanged(sender, e, tab);
        }

        private void View_NameChanged(object sender, string e, TabItem tab)
        {
            tab.Header = e;
        }

  

        //private List<TableDrivedEntityDTO> Message.DrivedEntities
        //{
        //    List<TableDrivedEntityDTO> result = new List<TableDrivedEntityDTO>();
        //    foreach (var item in tabControl.Items)
        //    {
        //        if ((item as TabItem).Content is frmEditSubEntity)
        //        {
        //            TableDrivedEntityDTO subEntity = ((item as TabItem).Content as frmEditSubEntity).Message;
        //            result.Add(subEntity);
        //        }
        //    }
        //    return result;
        //}

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }

        void AddColumn(TableDrivedEntityDTO sourceEntity, ColumnDTO sourceColumn, TableDrivedEntityDTO targetEntity, bool move, RadGridView sourceDataGrid, RadGridView targetDataGrid, bool checkRelationship)
        {
            if (sourceEntity == Message.BaseEntity && sourceColumn.IsNull == false)
            {
                MessageBox.Show("ستون اجباری میباشد و امکان انتقال به موجودیت دیگر وجود ندارد");
                return;
            }
            if (sourceColumn.PrimaryKey)
            {
                MessageBox.Show("ستون کلید اصلی میباشد");
                return;
            }
            if (checkRelationship)
            {
                if (sourceEntity.Relationships.Any(x => x.RelationshipColumns.Any(y => y.FirstSideColumnID == sourceColumn.ID)))
                {
                    MessageBox.Show("ستون مربوط به یک رابطه میباشد");
                    return;
                }
            }
            if (!targetEntity.Columns.Any(x => x.ID == sourceColumn.ID))
            {
                targetEntity.Columns.Add(sourceColumn);
                targetDataGrid.ItemsSource = null;
                targetDataGrid.ItemsSource = targetEntity.Columns;
            }
            if (move)
            {
                sourceEntity.Columns.Remove(sourceColumn);
                sourceDataGrid.ItemsSource = null;
                sourceDataGrid.ItemsSource = sourceEntity.Columns;
            }
        }
        private void btnMoveColumnFromBaseToDrived_Click(object sender, RoutedEventArgs e)
        {
            frmEditSubEntity view = GetCurrentSubEntityForm();
            if (view != null)
            {
                if (dtgColumns.SelectedItem != null)
                {
                    var column = dtgColumns.SelectedItem as ColumnDTO;
                    AddColumn(Message.BaseEntity, column, view.Message, true, dtgColumns, view.dtgColumnsDrived, true);
                }
            }
        }
        private void btnMoveColumnFromDrivedToBase_Click(object sender, RoutedEventArgs e)
        {
            frmEditSubEntity view = GetCurrentSubEntityForm();
            if (view != null)
            {
                if (view.SelectedColumn != null)
                {
                    AddColumn(view.Message, view.SelectedColumn, Message.BaseEntity, true, view.dtgColumnsDrived, dtgColumns, true);
                }
            }
        }
        private void btnCopyColumnFromDrivedToAnother_Click(object sender, RoutedEventArgs e)
        {
            frmEditSubEntity view = GetCurrentSubEntityForm();
            if (view != null)
            {
                if (view.SelectedColumn != null)
                {

                    var otherSubEntities = GetAllSubEntityForm().Where(x => x != view).ToList(); ;
                    if (otherSubEntities.Any())
                    {
                        UpdateSubEntities();
                        SelectDrivedEntites viewSelect = new MyProject_WPF.SelectDrivedEntites(otherSubEntities);
                        viewSelect.EntitySelected += (sender1, e1) => ViewSelect_EntitySelected(sender1, e1, view, view.SelectedColumn);
                        MyProjectManager.GetMyProjectManager.ShowDialog(viewSelect, "Select", Enum_WindowSize.None);
                    }
                }
            }
        }
        private void ViewSelect_EntitySelected(object sender, frmEditSubEntity e, frmEditSubEntity sourceView, ColumnDTO column)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(sender);
            AddColumn(sourceView.Message, column, e.Message, false, sourceView.dtgColumnsDrived, e.dtgColumnsDrived, true);

        }

        private void UpdateSubEntities()
        {
            foreach (var item in tabControl.Items)
            {
                if ((item as TabItem).Content is frmEditSubEntity)
                {
                    ((item as TabItem).Content as frmEditSubEntity).UpdateDrivedEntity();
                }
            }
        }


        private frmEditSubEntity GetSubEntityForm(TableDrivedEntityDTO e)
        {
            foreach (var item in tabControl.Items)
            {
                if ((item as TabItem).Content is frmEditSubEntity)
                {
                    if (((item as TabItem).Content as frmEditSubEntity).Message == e)
                        return (item as TabItem).Content as frmEditSubEntity;
                }
            }
            return null;
        }

        //private void MoveColumnFromBaseToDrived(frmEditSubEntity view, ColumnDTO column, bool showMessage)
        //{


        //}


        //private void MoveColumnFromDrivedToBase(frmEditSubEntity view, ColumnDTO column, bool showMessage)
        //{

        //    if (column.PrimaryKey)
        //    {
        //        if (showMessage)
        //            MessageBox.Show("ستون کلید اصلی میباشد");
        //        return;
        //    }
        //    Message.BaseEntity.Columns.Add(column);
        //    view.RemoveColumn(column);
        //    dtgColumns.ItemsSource = null;
        //    dtgColumns.ItemsSource = Message.BaseEntity.Columns;
        //}

        void AddRelationship(TableDrivedEntityDTO sourceEntity, RelationshipDTO sourceRelationship, TableDrivedEntityDTO targetEntity, bool move
            , RadGridView sourceRelationshipDataGrid, RadGridView targetRelationshipDataGrid, RadGridView sourceColumnDataGrid, RadGridView targetColumnDataGrid)
        {
            BizRelationship bizRelationship = new BizRelationship();

            if (sourceEntity == Message.BaseEntity && bizRelationship.RelationshipIsNotNulable(sourceRelationship.ID))
            {
                var message = " این رابطه اجباری میباشند و امکان انتخاب شدن برای موجودیت های مشتق شده را ندارند";
                MessageBox.Show(message, "روابط اجباری");
                return;
            }

            if (sourceRelationship.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary)
            {
                if (!sourceRelationship.RelationshipColumns.Any(x => x.FirstSideColumn.PrimaryKey))
                {
                    foreach (var col in sourceRelationship.RelationshipColumns)
                    {
                        var fColumn = sourceEntity.Columns.FirstOrDefault(x => x.ID == col.FirstSideColumnID);
                        if (fColumn != null)
                            AddColumn(sourceEntity, fColumn, targetEntity, move, sourceColumnDataGrid, targetColumnDataGrid, false);
                    }
                }
            }
            if (!targetEntity.Relationships.Any(x => x.ID == sourceRelationship.ID))
            {
                targetEntity.Relationships.Add(sourceRelationship);
                targetRelationshipDataGrid.ItemsSource = null;
                targetRelationshipDataGrid.ItemsSource = targetEntity.Relationships;
            }
            if (move)
            {
                sourceEntity.Relationships.Remove(sourceRelationship);
                sourceRelationshipDataGrid.ItemsSource = null;
                sourceRelationshipDataGrid.ItemsSource = sourceEntity.Relationships;
            }
            
        }

        private void btnMoveRelationshipFromBaseToDerived_Click(object sender, RoutedEventArgs e)
        {
            if (dtgRelationships.SelectedItem != null)
            {
                var relationship = dtgRelationships.SelectedItem as RelationshipDTO;
                frmEditSubEntity view = GetCurrentSubEntityForm();
                if (view != null)
                {
                    AddRelationship(Message.BaseEntity, relationship, view.Message, true, dtgRelationships, view.dtgRelationshipsDrived, dtgColumns, view.dtgColumnsDrived);
                }
            }
        }

        private void btnMoveRelationshipFromDrivedToBase_Click(object sender, RoutedEventArgs e)
        {
            frmEditSubEntity view = GetCurrentSubEntityForm();
            if (view != null)
            {
                if (view.SelectedRelationship != null)
                {
                    AddRelationship(view.Message, view.SelectedRelationship, Message.BaseEntity, true, view.dtgRelationshipsDrived, dtgRelationships, view.dtgColumnsDrived, dtgColumns);
                }
            }
        }
        private void btnCopyRelationshipFromDrivedToAnother_Click(object sender, RoutedEventArgs e)
        {
            frmEditSubEntity view = GetCurrentSubEntityForm();
            if (view != null)
            {
                if (view.SelectedRelationship != null)
                {
                    var relationship = view.SelectedRelationship as RelationshipDTO;
                    var otherSubEntities = GetAllSubEntityForm().Where(x => x != view).ToList();
                    if (otherSubEntities.Any())
                    {
                        UpdateSubEntities();
                        SelectDrivedEntites viewSelect = new MyProject_WPF.SelectDrivedEntites(otherSubEntities);
                        viewSelect.EntitySelected += (sender1, e1) => ViewSelectRelationship_EntitySelected(sender1, e1, view, relationship);
                        MyProjectManager.GetMyProjectManager.ShowDialog(viewSelect, "Select", Enum_WindowSize.None);
                    }
                }
            }
        }
        private void ViewSelectRelationship_EntitySelected(object sender, frmEditSubEntity e, frmEditSubEntity sourceView, RelationshipDTO relationship)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(sender);
            AddRelationship(sourceView.Message, relationship, e.Message, false, sourceView.dtgRelationshipsDrived, e.dtgRelationshipsDrived, sourceView.dtgColumnsDrived, e.dtgColumnsDrived);

        }

        private void btnNewEntity_Click_1(object sender, RoutedEventArgs e)
        {
            var newItem = new TableDrivedEntityDTO() { Name = "موجودیت جدید" };
            foreach (var item in Message.BaseEntity.Columns.Where(x => x.PrimaryKey))
                newItem.Columns.Add(item);
            AddDrivedEntityTab(newItem);
        }
        private frmEditSubEntity GetCurrentSubEntityForm()
        {
            if (tabControl.Items.Count > 0)
            {
                var tabItem = tabControl.SelectedItem as TabItem;
                if (tabItem != null)
                {
                    return tabItem.Content as frmEditSubEntity;
                }
            }
            return null;
        }
        private List<frmEditSubEntity> GetAllSubEntityForm()
        {
            List<frmEditSubEntity> result = new List<MyProject_WPF.frmEditSubEntity>();
            if (tabControl.Items.Count > 0)
            {
                foreach (TabItem tabItem in tabControl.Items)
                {
                    if (tabItem != null)
                    {
                        result.Add(tabItem.Content as frmEditSubEntity);
                    }
                }
            }
            return result;
        }
        private void btnDeleteEntity_Click(object sender, RoutedEventArgs e)
        {
            frmEditSubEntity view = GetCurrentSubEntityForm();
            if (view != null)
            {
                if (view.Message.Columns.Any())
                {
                    MessageBox.Show("لطفا ابتدا وضعیت ستونها موجودیت مشتق شده را تعیین نمایید");
                    return;
                }
                if (view.Message.Relationships.Any())
                {
                    MessageBox.Show("لطفا ابتدا وضعیت روابط موجودیت مشتق شده را تعیین نمایید");
                    return;
                }
                Message.DrivedEntities.Remove(view.Message);
                var tabItem = tabControl.SelectedItem as TabItem;
                if (tabItem != null)
                {
                    tabControl.Items.Remove(tabItem);
                }
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //ستونهای رابطه برای مشتقها از موجودیت پایه حذف شوند
            //کد نوشته شود
            //if (txtName.Text == "")
            //{
            //    MessageBox.Show("نام موجودیت مشخص نشده است", "نام موجودیت");
            //    return;
            //}
            //if ((cmbDrivedEntities.SelectedValue == null ? 0 : (int)cmbDrivedEntities.SelectedValue) == BaseEntity.ID)
            //{
            //    if (txtCriteria.Text != "")
            //    {
            //        MessageBox.Show("برای موجودیت پایه شرط نمیتوان تعریف نمود", "شرط موجودیت");
            //        return;
            //    }
            //}
            //else
            //{
            //    if (txtCriteria.Text == "")
            //    {
            //        MessageBox.Show("شرط موجودیت مشخص نشده است", "شرط موجودیت");
            //        return;
            //    }
            //}
            //if (Inheritance)
            //{
            //if (!Message.DeterminerValues.Any())
            //{
            //    MessageBox.Show("مقادیر تعیین کننده نوع موجودیت مشخص نشده است");
            //    return;
            //}
            if (optIsDisjoint.IsChecked == false && optIsOverlap.IsChecked == false)
            {
                MessageBox.Show("نوع رابطه ارث بری مشخص نشده است", "Disjoint or Overlap");
                return;
            }
            if (optIsTolatParticipation.IsChecked == false && optIsPartialParticipation.IsChecked == false)
            {
                MessageBox.Show("نوع رابطه ارث بری مشخص نشده است", "TolatParticipation or PartialParticipatio");
                return;
            }
            foreach (var entity in Message.DrivedEntities)
            {
                foreach (var column in entity.Columns)
                {
                    if (!column.PrimaryKey)
                    {
                        if (Message.BaseEntity.Columns.Any(x => x.ID == column.ID))
                        {
                            MessageBox.Show("ستون" + " " + column.Name + " " + "همزمان در موجودیت پایه و همچنین زیر موجودیت" + " " + entity.Name + " " + "تعریف شده است");
                            return;
                        }
                    }
                }
                foreach (var rel in entity.Relationships)
                {
                    if (Message.BaseEntity.Relationships.Any(x => x.ID == rel.ID))
                    {
                        MessageBox.Show("رابطه" + " " + rel.Name + " " + "همزمان در موجودیت پایه و همچنین زیر موجودیت" + " " + entity.Name + " " + "تعریف شده است");
                        return;
                    }
                }
            }
            if (optIsOverlap.IsChecked == true)
            {
                foreach (var entity in Message.DrivedEntities)
                {
                    foreach (var column in entity.Columns)
                    {
                        if (Message.DrivedEntities.Any(x => x != entity && x.Columns.Any(y => y.ID == column.ID)))
                        {
                            MessageBox.Show("ستون" + " " + column.Name + " " + "در بیش از یک زیر موجودیت تعریف شده و این برای روابط ارث بری" + " " + "Overlap" + " " + "امکان پذیر نمی باشد");
                            return;
                        }
                    }
                }
                foreach (var entity in Message.DrivedEntities)
                {
                    foreach (var relationship in entity.Relationships)
                    {
                        if (Message.DrivedEntities.Any(x => x != entity && x.Relationships.Any(y => y.ID == relationship.ID)))
                        {
                            MessageBox.Show("رابطه" + " " + relationship.Name + " " + "در بیش از یک زیر موجودیت تعریف شده و این برای روابط ارث بری" + " " + "Overlap" + " " + "امکان پذیر نمی باشد");
                            return;
                        }
                    }
                }
            }

            foreach (var entity in Message.DrivedEntities)
            {
                if (!entity.EntityDeterminers.Any())
                {
                    var message = "برای یک یا چند موجودیت مشتق ستون و یا مقدار تعیین کننده مشخص نشده است";
                    MessageBox.Show(message);
                    return;
                }
                foreach (var values in entity.EntityDeterminers)
                {
                    if (Message.DrivedEntities.Any(x => x != entity && x.EntityDeterminers.Any(y => y.Value == values.Value)))
                    {
                        var message = "مقدار تعیین کننده" + " '" + values.Value + "' " + "برای بیش از یک زیر موجودیت تعریف شده است";
                        MessageBox.Show(message);
                        return;
                    }
                }
            }


            UpdateSubEntities();
            foreach (var DrivedEntity in Message.DrivedEntities)
            {
                if (DrivedEntity.DeterminerColumnID == 0)
                {
                    MessageBox.Show("برای زیر موجودیت" + " " + DrivedEntity.Name + " " + "ستون تعیین موجودیت مشخص نشده است");
                    return;
                }
                if (!DrivedEntity.EntityDeterminers.Any())
                {
                    MessageBox.Show("برای زیر موجودیت" + " " + DrivedEntity.Name + " " + "مقادیر تعیین موجودیت مشخص نشده است");
                    return;
                }
                if (!Message.BaseEntity.Columns.Any(x => x.ID == DrivedEntity.DeterminerColumnID))
                {
                    MessageBox.Show("ستون تعیین کننده برای زیر موجودیت" + " " + DrivedEntity.Name + " " + "در موجودیت پایه موجود نمی باشد");
                    return;
                }
            }
            foreach (var DrivedEntity in Message.DrivedEntities)
            {
                if (DrivedEntity.Name == "")
                {
                    var message = "برای یک یا چند موجودیت مشتق نام موجودیت تعریف نشده است";
                    MessageBox.Show(message);
                    return;
                }
            }
           
            Message.ISARelationship.IsDisjoint = optIsDisjoint.IsChecked == true;
            Message.ISARelationship.IsTolatParticipation = optIsTolatParticipation.IsChecked == true;

            BizTableDrivedEntity biz = new BizTableDrivedEntity();
            var id = biz.Save(MyProjectManager.GetMyProjectManager.GetRequester(), Message);
            MessageBox.Show("اطلاعات ثبت شد");
        }

    }
}
