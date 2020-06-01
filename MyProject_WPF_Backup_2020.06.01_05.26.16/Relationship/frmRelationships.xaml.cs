using ModelEntites;
using MyModelGenerator;
using MyModelManager;
using MyProject_WPF.Biz;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using System.Collections;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmRelationship.xaml
    /// </summary>
    public partial class frmRelationships : UserControl
    {
        //ModelGenerator_sql ModelGenerator = new ModelGenerator_sql();
        BizRelationship bizRelationship = new BizRelationship();
        BizISARelationship bizISARelationship = new BizISARelationship();
        BizUnionRelationship bizUnionRelationship = new BizUnionRelationship();
        BizUniqueConstraints bizUniqueConstraint = new BizUniqueConstraints();
        BizRelationshipConverter bizRelationshipConverter = new BizRelationshipConverter();
        //DatabaseDTO Database { set; get; }
        public frmRelationships()
        {
            InitializeComponent();

            var relationshipMenu = new RadContextMenu();
            relationshipMenu.Opened += dtgRelationships_ContextMenuOpening;
            //فعلا منوی همه روابط یکی شده اند..میتوانند جدا شود
            RadContextMenu.SetContextMenu(dtgRelationships, relationshipMenu);

            SetGridViewColumns();

            dtgRelationships.EnableColumnVirtualization = false;
            dtgRelationships.EnableRowVirtualization = false;
            dtgRelationships.RowLoaded += DtgRelationships_RowLoaded;
            dtgRelationships.CellEditEnded += DtgRelationships_CellEditEnded;
        }
        //public void SetRelatoinships(DatabaseDTO database)
        //{
        //    Database = database;
        //    SetRelationshipsFromDatabase();
        //}

        //private async void SetRelationshipsFromDatabase()
        //{
        //    btnRefreshRelationship.IsEnabled = false;
        //    try
        //    {
        //        var result = await GetRelationshipsFromDatabase();
        //        dtgRelationships.ItemsSource = result;
        //    }
        //    catch
        //    {

        //    }
        //    finally
        //    {
        //        btnRefreshRelationship.IsEnabled = true;
        //    }
        //}
        //private Task<List<RelationshipDTO>> GetRelationshipsFromDatabase()
        //{
        //    return Task.Run(() =>
        //    {
        //        var result = bizRelationship.GetRelationships(Database.ID);
        //        return result;
        //    });
        //}
        int EntityID { set; get; }
        public void SetRelatoinships(int entityID)
        {
            EntityID = entityID;
            SetRelationshipsFromEntity();
        }

        private async void SetRelationshipsFromEntity()
        {
            btnRefreshRelationship.IsEnabled = false;
            try
            {
                var result = await GetRelationshipsFromEntity();
                dtgRelationships.ItemsSource = result;
            }
            catch
            {

            }
            finally
            {
                btnRefreshRelationship.IsEnabled = true;
            }
        }
        private Task<List<RelationshipDTO>> GetRelationshipsFromEntity()
        {
            return Task.Run(() =>
            {
                var result = bizRelationship.GetRelationshipsByEntityID(EntityID);
                return result;
            });
        }

        private void DtgRelationships_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            if (e.Cell.Column.UniqueName == "DeleteOption")
            {
                var rel = (e.Cell.DataContext as RelationshipDTO);
                if (rel != null)
                {
                    if (!rel.RelationshipColumns.Any(x => x.SecondSideColumn.IsNull))
                    {
                        //if ((RelationshipDeleteOption)e.NewData == RelationshipDeleteOption.SetNull)
                        //{
                        //    rel.DeleteOption = RelationshipDeleteOption.None;
                        //}
                    }
                }
            }
        }

        private void DtgRelationships_RowLoaded(object sender, RowLoadedEventArgs e)
        {
            if (e.DataElement is RelationshipDTO)
            {
                var rel = (e.DataElement as RelationshipDTO);
                var cell = e.Row.Cells.FirstOrDefault(x => x.Column.UniqueName == "DeleteOption");
                if (cell != null)
                {
                    cell.IsEnabled = false;
                    if (rel.TypeEnum != Enum_RelationshipType.None && rel.MastertTypeEnum == Enum_MasterRelationshipType.FromPrimartyToForeign)
                        cell.IsEnabled = true;
                }
                if (rel.AnyForeignKeyIsPrimaryKey)
                {
                    cell = e.Row.Cells.FirstOrDefault(x => x.Column.UniqueName == "IsDisabled");
                    if (cell != null)
                        cell.IsEnabled = false;

                    if (rel.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary)
                    {
                        cell = e.Row.Cells.FirstOrDefault(x => x.Column.UniqueName == "DataEntryEnabled");
                        if (cell != null)
                            cell.IsEnabled = false;
                    }

                    cell = e.Row.Cells.FirstOrDefault(x => x.Column.UniqueName == "IsReadonly");
                    if (cell != null)
                        cell.IsEnabled = false;

                    cell = e.Row.Cells.FirstOrDefault(x => x.Column.UniqueName == "IsNotTransferable");
                    if (cell != null)
                        cell.IsEnabled = false;
                }
            }
        }

        private void btnRefreshRelationship_Click(object sender, RoutedEventArgs e)
        {
            if (EntityID != 0)
                SetRelationshipsFromEntity();
            //else if (Database != null)
            //    SetRelationshipsFromDatabase();
        }
        private void SetGridViewColumns()
        {
            SetGridViewColumns(dtgRelationships);
        }

        private void SetGridViewColumns(RadGridView dataGrid)
        {

            dataGrid.AlternateRowBackground = new SolidColorBrush(Colors.LightBlue);
            dataGrid.AutoGenerateColumns = false;

            if (dataGrid == dtgRelationships)
            {

                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "شناسه", true,null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "نام", false,null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "شناسه رابطه قرینه", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "رابطه قرینه", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "عنوان", false, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "موجودیت طرف اول", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "موجودیت طرف دوم", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("RelationshipColumns", "ستونهای رابطه", true,null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("TypeStr", "نوع رابطه", true,null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsNotSkippable", "غیر قابل صرف نظر", false, null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsNotTransferable", "غیر قابل انتقال", false,null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideMandatory", "طرف دیگر اجباری", false,null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideCreatable", "قابلیت ایجاد طرف دیگر", false,null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideDirectlyCreatable", "قابلیت ایجاد مستقیم طرف یک", false,null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataEntryEnabled", "قابلیت ورود اطلاعات", false,null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsReadonly", "فقط خواندنی", false,null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsDisabled", "غیر فعال", false,null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DeleteOption", "حذف", false,null, GridViewColumnType.Enum, GetDeleteOptionAsItemsSource()));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SearchInitially", "جستجوی خودکار", false,null, GridViewColumnType.CheckBox));

                dataGrid.CanUserDeleteRows = false;
            }

        }

        private IEnumerable GetDeleteOptionAsItemsSource()
        {
            return Enum.GetValues(typeof(RelationshipDeleteOption))
                        .Cast<object>()
                        ;
        }

        void dtgRelationships_ContextMenuOpening(object sender, RoutedEventArgs e)
        {
            var contextMenu = sender as RadContextMenu;
            contextMenu.Items.Clear();
            var source = contextMenu.GetClickedElement<GridViewRow>();
            if (contextMenu != null && source != null)
            {
                var relationship = source.DataContext as RelationshipDTO;
                AddConvertRelationshipContextMenu(contextMenu, relationship);

                if (relationship.DataEntryEnabled)
                {
                    var relationshipFilterMenu = AddMenu(contextMenu.Items, "فیلتر رابطه", "", "../Images/filter.png");
                    relationshipFilterMenu.Click += (sender1, EventArgs) => MnuRelationshipFilter_Click(sender, EventArgs, (relationship).ID);
                }

                //if (relationship.TypeEnum == Enum_RelationshipType.None)
                //{
                //    var relationshipTypeMenu = AddMenu(contextMenu.Items, "تعیین نوع رابطه", "", "../Images/type.png");
                //    relationshipTypeMenu.Click += (sender1, EventArgs) => MnuRelationshipType_Click(sender, EventArgs, (relationship).ID);
                //}

                if (relationship.Created == true)
                {
                    var relationshipEditMenu = AddMenu(contextMenu.Items, "اصلاح رابطه", "", "../Images/edit.png");
                    relationshipEditMenu.Click += (sender1, EventArgs) => MnuRelationshipEdit_Click(sender, EventArgs, (relationship).ID);

                    var relationshipDeleteMenu = AddMenu(contextMenu.Items, "حذف رابطه", "", "../Images/delete.png");
                    relationshipDeleteMenu.Click += (sender1, EventArgs) => MnuRelationshipDelete_Click(sender, EventArgs, (relationship).ID);

                }
            }
            var relationshipCreateMenu = AddMenu(contextMenu.Items, "تعریف رابطه جدید", "", "../Images/edit.png");
            relationshipCreateMenu.Click += MnuRelationshipCreate_Click;

            MyProjectHelper.SetContectMenuVisibility(contextMenu);
        }
        private void MnuRelationshipCreate_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            frmRelationshipCreate view = new frmRelationshipCreate(0);
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form");
        }
        private void MnuRelationshipEdit_Click(object sender, Telerik.Windows.RadRoutedEventArgs e, int relationshipID)
        {
            frmRelationshipCreate view = new frmRelationshipCreate(relationshipID);
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form");
        }
        private void MnuRelationshipDelete_Click(object sender, Telerik.Windows.RadRoutedEventArgs e, int relationshipID)
        {
            bizRelationship.DeleteRelationship(relationshipID);
        }
        private void MnuRelationshipFilter_Click(object sender, Telerik.Windows.RadRoutedEventArgs e, int relationshipID)
        {
            frmRelationshipFilter view = new frmRelationshipFilter(relationshipID);
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form");
        }
        private void AddConvertRelationshipContextMenu(RadContextMenu contextMenu, RelationshipDTO relationship)
        {
            if (relationship == null)
                return;
            //RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
            //contextMenu.Items.Add(separator);

            var targetTypes = bizRelationshipConverter.GetRelationshipConvertOptions(relationship);
            if (targetTypes.Any())
            {
                var relationshipConvertMenu = AddMenu(contextMenu.Items, "تبدیل رابطه", "", "../Images/relationship1.png");

                foreach (var item in targetTypes)
                {
                    var title = "تبدیل به رابطه " + bizRelationship.GetRelationshipTypeTitle(item);
                    var convertMenu = AddMenu(relationshipConvertMenu.Items, title, "", "../Images/convert.png", item);
                    convertMenu.Click += (sender, EventArgs) => customMenuItem_Click(sender, relationship);
                }
            }
        }
        private RadMenuItem AddMenu(ItemCollection itemCollection, string title, string name, string imagePath, object tag = null)
        {
            RadMenuItem menu = new RadMenuItem();

            //StackPanel pnl = new StackPanel();
            //pnl.Orientation = Orientation.Horizontal;
            //pnl.Children.Add(new TextBlock() { Text = title });
            Image img = new Image();
            img.Width = 15;
            var uriSource = new Uri(imagePath, UriKind.Relative);
            img.Source = new BitmapImage(uriSource);
            menu.Icon = img;
            if (tag != null)
                menu.Tag = tag;
            //pnl.Children.Add(img);
            menu.Header = title;
            if (!string.IsNullOrEmpty(name))
                menu.Name = name;
            itemCollection.Add(menu);
            return menu;
        }
        void customMenuItem_Click(object sender, RelationshipDTO relationship)
        {
            //try
            //{
            bizRelationshipConverter.ConvertRelationship(relationship, (Enum_RelationshipType)(sender as RadMenuItem).Tag);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }



        private void btnUpdateRelationships_Click(object sender, RoutedEventArgs e)
        {
            btnUpdateRelationships.IsEnabled = false;
            bizRelationship.UpdateRelationships(dtgRelationships.ItemsSource as List<RelationshipDTO>);
            btnUpdateRelationships.IsEnabled = true;
        }




    }
}
