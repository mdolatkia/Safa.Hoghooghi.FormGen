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
    public partial class frmAllRelationships : UserControl
    {
        //ModelGenerator_sql ModelGenerator = new ModelGenerator_sql();
        BizRelationship bizRelationship = new BizRelationship();
        BizISARelationship bizISARelationship = new BizISARelationship();
        BizUnionRelationship bizUnionRelationship = new BizUnionRelationship();
        BizUniqueConstraints bizUniqueConstraint = new BizUniqueConstraints();
        BizRelationshipConverter bizRelationshipConverter = new BizRelationshipConverter();
        DatabaseDTO Database { set; get; }
        public frmAllRelationships(DatabaseDTO database)
        {
            InitializeComponent();
            Database = database;
            //     formRelationships.SetRelatoinships(database);

            dtgManyToMany.SelectionChanged += dtgManyToMany_SelectionChanged;



            dtgISARelationship.SelectionChanged += dtgISARelationship_SelectionChanged;
            dtgUnionRelationship.SelectionChanged += dtgUnionRelationship_SelectionChanged;



            var relationshipMenu = new RadContextMenu();
            relationshipMenu.Opened += dtgRelationships_ContextMenuOpening;
            //فعلا منوی همه روابط یکی شده اند..میتوانند جدا شود
            //RadContextMenu.SetContextMenu(dtgRelationships, relationshipMenu);
            //RadContextMenu.SetContextMenu(dtgOneToMany, relationshipMenu);
            //RadContextMenu.SetContextMenu(dtgManyToOne, relationshipMenu);
            //RadContextMenu.SetContextMenu(dtgExplicit, relationshipMenu);
            //RadContextMenu.SetContextMenu(dtgImplicit, relationshipMenu);
            RadContextMenu.SetContextMenu(dtgSuperToSub, relationshipMenu);
            RadContextMenu.SetContextMenu(dtgSubToSuper, relationshipMenu);
            RadContextMenu.SetContextMenu(dtgUnionToSubUnion, relationshipMenu);
            RadContextMenu.SetContextMenu(dtgSubUnionToUnion, relationshipMenu);

            var manytomanyMenu = new RadContextMenu();
            manytomanyMenu.Opened += dtgManyToMany_ContextMenuOpening;
            RadContextMenu.SetContextMenu(dtgManyToMany, manytomanyMenu);

            var isaMenu = new RadContextMenu();
            isaMenu.Opened += dtgISARelationship_ContextMenuOpening;
            RadContextMenu.SetContextMenu(dtgISARelationship, isaMenu);

            var unionMenu = new RadContextMenu();
            unionMenu.Opened += dtgUnionRelationship_ContextMenuOpening;
            RadContextMenu.SetContextMenu(dtgUnionRelationship, isaMenu);

            SetGridViewColumns();
            this.Loaded += FrmAllRelationships_Loaded;
            //dtgRelationships.EnableColumnVirtualization = false;
            //dtgRelationships.EnableRowVirtualization = false;
            //dtgRelationships.RowLoaded += DtgRelationships_RowLoaded;
            //dtgRelationships.CellEditEnded += DtgRelationships_CellEditEnded;
        }

        private void FrmAllRelationships_Loaded(object sender, RoutedEventArgs e)
        {
            btnRefreshISA_Click(null, null);
            btnRefreshUnionRelationship_Click(null, null);
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

            }
        }

        //private void btnRefreshRelationship_Click(object sender, RoutedEventArgs e)
        //{
        //    dtgRelationships.ItemsSource = bizRelationship.GetRelationships(Database.ID);
        //}
        private void SetGridViewColumns()
        {




            //SetGridViewColumns(dtgRelationships);

            //SetGridViewColumns(dtgRelationships);
            //SetGridViewColumns(dtgOneToMany);
            //SetGridViewColumns(dtgManyToOne);
            //SetGridViewColumns(dtgExplicit);
            //SetGridViewColumns(dtgImplicit);
            SetGridViewColumns(dtgManyToMany);
            SetGridViewColumns(dtgManyToMany_ManyToOne);
            SetGridViewColumns(dtgISARelationship);
            SetGridViewColumns(dtgSuperToSub);
            SetGridViewColumns(dtgSubToSuper);
            SetGridViewColumns(dtgUnionRelationship);
            SetGridViewColumns(dtgUnionToSubUnion);
            SetGridViewColumns(dtgSubUnionToUnion);
        }

        private void SetGridViewColumns(RadGridView dataGrid)
        {


            dataGrid.AlternateRowBackground = new SolidColorBrush(Colors.LightBlue);
            dataGrid.AutoGenerateColumns = false;


            //if (dataGrid == dtgRelationships)
            //{

            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "شناسه", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "نام", false,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "شناسه رابطه قرینه", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "رابطه قرینه", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "عنوان", false,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "موجودیت طرف اول", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "موجودیت طرف دوم", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("RelationshipColumns", "ستونهای رابطه", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("TypeStr", "نوع رابطه", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideTransferable", "قابل انتقال بودن طرف دیگر", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideMandatory", "طرف دیگر اجباری", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideCreatable", "قابلیت ایجاد طرف دیگر", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideDirectlyCreatable", "قابلیت ایجاد مستقیم طرف یک", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataEntryEnabled", "قابلیت ورود اطلاعات", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsReadonly", "فقط خواندنی", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "فعال", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DeleteOption", "حذف", false,null, GridViewColumnType.Enum, GetDeleteOptionAsItemsSource()));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SearchInitially", "جستجوی خودکار", false,null, GridViewColumnType.CheckBox));

            //    dataGrid.CanUserDeleteRows = false;
            //}
            // if (dataGrid == dtgOneToMany)
            //{

            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "شناسه", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "نام", false,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "شناسه رابطه قرینه", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "رابطه قرینه", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "عنوان", false,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "موجودیت طرف اول", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "موجودیت طرف دوم", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("MasterDetailState", "نوع رابطه یک به چند", false,null, GridViewColumnType.Numeric));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DetailsCount", "تعداد طرف چند", false,null, GridViewColumnType.Numeric));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideMandatory", "طرف چند اجباری", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideCreatable", "قابلیت ایجاد طرف چند", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideDirectlyCreatable", "قابلیت ایجاد مستقیم طرف چند", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataEntryEnabled", "قابلیت ورود اطلاعات", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsReadonly", "فقط خواندنی", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "فعال", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.CanUserDeleteRows = false;
            //}
            //else if (dataGrid == dtgManyToOne)
            //{

            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "شناسه", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "نام", false,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "شناسه رابطه قرینه", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "رابطه قرینه", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "عنوان", false,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "موجودیت طرف اول", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "موجودیت طرف دوم", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideTransferable", "قابل انتقال بودن طرف یک", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideMandatory", "طرف یک اجباری", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideCreatable", "قابلیت ایجاد طرف یک", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideDirectlyCreatable", "قابلیت ایجاد مستقیم طرف یک", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataEntryEnabled", "قابلیت ورود اطلاعات", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsReadonly", "فقط خواندنی", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "فعال", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.CanUserDeleteRows = false;
            //}
            //else if (dataGrid == dtgExplicit)
            //{

            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "شناسه", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "نام", false,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "شناسه رابطه قرینه", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "رابطه قرینه", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "عنوان", false,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "موجودیت طرف اول", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "موجودیت طرف دوم", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideTransferable", "قابل انتقال بودن طرف دیگر", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideMandatory", "طرف دیگر اجباری", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideCreatable", "قابلیت ایجاد طرف دیگر", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideDirectlyCreatable", "قابلیت ایجاد مستقیم طرف یک", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataEntryEnabled", "قابلیت ورود اطلاعات", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsReadonly", "فقط خواندنی", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "فعال", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.CanUserDeleteRows = false;
            //}
            //else if (dataGrid == dtgImplicit)
            //{

            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "شناسه", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "نام", false,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "شناسه رابطه قرینه", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "رابطه قرینه", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "عنوان", false,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "موجودیت طرف اول", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "موجودیت طرف دوم", true,null, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideTransferable", "قابل انتقال بودن طرف دیگر", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideMandatory", "طرف دیگر اجباری", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideCreatable", "قابلیت ایجاد طرف دیگر", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideDirectlyCreatable", "قابلیت ایجاد مستقیم طرف یک", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataEntryEnabled", "قابلیت ورود اطلاعات", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsReadonly", "فقط خواندنی", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "فعال", false,null, GridViewColumnType.CheckBox));
            //    dataGrid.CanUserDeleteRows = false;
            //}
            if (dataGrid == dtgManyToMany)
            {

                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "شناسه", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "نام", false, null, GridViewColumnType.Text));
            }
            else if (dataGrid == dtgManyToMany_ManyToOne)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "شناسه", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "نام", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "شناسه رابطه قرینه", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "رابطه قرینه", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "عنوان", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "موجودیت طرف اول", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "موجودیت طرف دوم", true, null, GridViewColumnType.Text));
            }
            else if (dataGrid == dtgISARelationship)
            {
                dataGrid.SelectionMode = SelectionMode.Extended;

                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "شناسه", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "نام", false, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SuperTypeEntities", "ابر کلاسها", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SubTypeEntities", "زیر کلاسها", true, null, GridViewColumnType.Text));
                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsGeneralization", "IsGeneralization", false,null, GridViewColumnType.CheckBox));
                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsSpecialization", "IsSpecialization", false,null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsTolatParticipation", "شرکت پذیری کامل", false, null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsDisjoint", "شرکت پذیری انحصاری", false, null, GridViewColumnType.CheckBox));
                dataGrid.CanUserDeleteRows = false;
            }
            else if (dataGrid == dtgSuperToSub)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "شناسه", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "نام", false, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "عنوان", false, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "موجودیت طرف اول", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "موجودیت طرف دوم", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DeterminerColumnID", "ستون تعیین کننده", false, null, GridViewColumnType.ComboBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DeterminerColumnValue", "مقدار تعیین کننده", false, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideTransferable", "قابل انتقال بودن طرف دیگر", false, null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideCreatable", "قابلیت ایجاد طرف دیگر", false, null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideDirectlyCreatable", "قابلیت ایجاد مستقیم طرف یک", false, null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataEntryEnabled", "قابلیت ورود اطلاعات", false, null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsReadonly", "فقط خواندنی", false, null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "فعال", false, null, GridViewColumnType.CheckBox));
              
                dataGrid.CanUserDeleteRows = false;
            }
            else if (dataGrid == dtgSubToSuper)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "شناسه", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "نام", false, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "شناسه رابطه قرینه", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "رابطه قرینه", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "عنوان", false, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "موجودیت طرف اول", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "موجودیت طرف دوم", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideTransferable", "قابل انتقال بودن طرف دیگر", false, null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideCreatable", "قابلیت ایجاد طرف دیگر", false, null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideDirectlyCreatable", "قابلیت ایجاد مستقیم طرف یک", false, null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataEntryEnabled", "قابلیت ورود اطلاعات", false, null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsReadonly", "فقط خواندنی", false, null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "فعال", false, null, GridViewColumnType.CheckBox));
                dataGrid.CanUserDeleteRows = false;
            }
            else if (dataGrid == dtgUnionRelationship)
            {
                dataGrid.SelectionMode = SelectionMode.Extended;

                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "شناسه", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "نام", false, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("UnionTypeEntities", "ابراجتماع ها", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SubUnionTypeEntities", "زیر اجتماع ها", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsTolatParticipation", "شرکت پذیری کامل", false, null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("UnionHoldsKeys", "کلید خارجی در ابر اجتماع", false, null, GridViewColumnType.CheckBox));
                dataGrid.CanUserDeleteRows = false;
            }
            else if (dataGrid == dtgUnionToSubUnion)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "شناسه", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "نام", false, null, GridViewColumnType.Text));
                              dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "عنوان", false, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "موجودیت طرف اول", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "موجودیت طرف دوم", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DeterminerColumnID", "ستون تعیین کننده", false, null, GridViewColumnType.ComboBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DeterminerColumnValue", "مقدار تعیین کننده", false, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideTransferable", "قابل انتقال بودن طرف دیگر", false, null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideCreatable", "قابلیت ایجاد طرف دیگر", false, null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideDirectlyCreatable", "قابلیت ایجاد مستقیم طرف یک", false, null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataEntryEnabled", "قابلیت ورود اطلاعات", false, null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsReadonly", "فقط خواندنی", false, null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "فعال", false, null, GridViewColumnType.CheckBox));
                dataGrid.CanUserDeleteRows = false;
            }
            else if (dataGrid == dtgSubUnionToUnion)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "شناسه", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "نام", false, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "شناسه رابطه قرینه", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "رابطه قرینه", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "عنوان", false, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "موجودیت طرف اول", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "موجودیت طرف دوم", true, null, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideTransferable", "قابل انتقال بودن طرف دیگر", false, null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideCreatable", "قابلیت ایجاد طرف دیگر", false, null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideDirectlyCreatable", "قابلیت ایجاد مستقیم طرف یک", false, null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataEntryEnabled", "قابلیت ورود اطلاعات", false, null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsReadonly", "فقط خواندنی", false, null, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "فعال", false, null, GridViewColumnType.CheckBox));
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
                AddConvertRelationshipContextMenu(contextMenu, source.DataContext as RelationshipDTO);

                var relationshipFilterMenu = AddMenu(contextMenu.Items, "فیلتر رابطه", "", "../Images/filter.png");
                relationshipFilterMenu.Click += (sender1, EventArgs) => MnuRelationshipFilter_Click(sender, EventArgs, (source.DataContext as RelationshipDTO).ID);

                var relationship = (source.DataContext as RelationshipDTO);
                //if (relationship.TypeEnum == Enum_RelationshipType.None)
                //{
                //    var relationshipTypeMenu = AddMenu(contextMenu.Items, "تعیین نوع رابطه", "", "../Images/type.png");
                //    relationshipTypeMenu.Click += (sender1, EventArgs) => MnuRelationshipType_Click(sender, EventArgs, (source.DataContext as RelationshipDTO).ID);
                //}

                if (relationship.Created == true)
                {
                    var relationshipEditMenu = AddMenu(contextMenu.Items, "اصلاح رابطه", "", "../Images/edit.png");
                    relationshipEditMenu.Click += (sender1, EventArgs) => MnuRelationshipEdit_Click(sender, EventArgs, (source.DataContext as RelationshipDTO).ID);

                    var relationshipDeleteMenu = AddMenu(contextMenu.Items, "حذف رابطه", "", "../Images/delete.png");
                    relationshipDeleteMenu.Click += (sender1, EventArgs) => MnuRelationshipDelete_Click(sender, EventArgs, (source.DataContext as RelationshipDTO).ID);

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
        //private void MnuRelationshipType_Click(object sender, Telerik.Windows.RadRoutedEventArgs e, int relationshipID)
        //{

        //    bizRelationship.ImposeSingleRelationshipType(relationshipID);
        //}





        void dtgManyToMany_ContextMenuOpening(object sender, RoutedEventArgs e)
        {
            var contextMenu = sender as RadContextMenu;
            contextMenu.Items.Clear();
            var source = contextMenu.GetClickedElement<GridViewRow>();
            var mtmCreateMenu = AddMenu(contextMenu.Items, "ایجاد رابطه چند به چند", "", "../Images/addnew.png");
            mtmCreateMenu.Click += customMenuItem_Click;
            if (contextMenu != null && source != null)
            {
                //RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
                //contextMenu.Items.Add(separator);
                //var entity = dtgManyToMany.CurrentRow.DataBoundItem as ManyToMany;
                //if (entity != null)
                //{


                var mtmRemoveMenu = AddMenu(contextMenu.Items, "حذف رابطه چند به چند", "", "../Images/remove.png");
                mtmRemoveMenu.Click += (sender1, EventArgs) => customMenuItemRemove_Click(sender, e, source.DataContext as ManyToManyRelationshipDTO);

            }
            MyProjectHelper.SetContectMenuVisibility(contextMenu);
        }

        void customMenuItem_Click(object sender, RoutedEventArgs e)
        {
            BizTable bizTable = new BizTable();
            var list = bizTable.GetTablesWithManyToOneRelationships(Database.ID);
            var selectView = new frmChooseItem();
            selectView.ShowItems<TableDTO>(list, false);
            selectView.ItemSelected += selectView_ItemSelected;
            MyProjectManager.GetMyProjectManager.ShowDialog(selectView, "Select Associative Table");
        }

        void selectView_ItemSelected(object sender, SelectedItemsArg e)
        {
            frmCreateManyToManyRelationship view = new frmCreateManyToManyRelationship((e.Items.First() as TableDTO).ID);
            view.ManyToManyCreated += view_ManyToManyCreated;
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "CreateManyToMany");
        }

        void view_ManyToManyCreated(object sender, ManyToManyCreatedArg e)
        {
            //using (var projectContext = new DataAccess.MyProjectEntities())
            //{
            //    ManyToManyRelationshipType manyToManyRelationshipType = new ManyToManyRelationshipType();
            //    manyToManyRelationshipType.Name = e.Name;
            //    foreach (var id in e.ManyToOneIDs)
            //        manyToManyRelationshipType.ManyToOneRelationshipType.Add(projectContext.ManyToOneRelationshipType.First(x => x.ID == id));

            //    var dbTable = projectContext.Table.First(x => x.ID == e.TableID);
            //    dbTable.IsAssociative = true;

            //    projectContext.ManyToManyRelationshipType.Add(manyToManyRelationshipType);
            //    projectContext.SaveChanges();

            //    var srvdb = GeneralHelper.GetServerNameDatabaseName(dbTable.Catalog);
            //    GetManyToManyRelationships(srvdb.Item1, srvdb.Item2);
            //}
        }

        void customMenuItemRemove_Click(object sender, RoutedEventArgs e, ManyToManyRelationshipDTO manyToMany)
        {
            bizRelationship.RemoveManyToManyRelationships(manyToMany.ID);
        }

        //void dtgManyToMany_ManyToOne_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        //{
        //    RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
        //    contextMenu.Items.Add(separator);
        //    var entity = dtgManyToMany_ManyToOne.CurrentRow.DataBoundItem as ManyToOne;
        //    if (entity != null)
        //    {

        //        RadMenuItem customMenuItem = new RadMenuItem();
        //        customMenuItem.Header = "Remove from Many To Many Relationship";
        //        customMenuItem.Name = "RemoveManyToOne";
        //        customMenuItem.Click += (sender1, EventArgs) => customMenuItem_Click11(sender, e, entity);
        //        contextMenu.Items.Add(customMenuItem);


        //    }
        //}
        //void customMenuItem_Click11(object sender, RoutedEventArgs e, ManyToOne entity)
        //{
        //    MyFormHelper.RemoveManyToOneFromManyToManyRelationship(entity);
        //}

        //void customMenuItem_Click1(object sender, RoutedEventArgs e, ManyToMany entity)
        //{

        //}


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

        void dtgUnionRelationship_ContextMenuOpening(object sender, RoutedEventArgs e)
        {
            var contextMenu = sender as RadContextMenu;
            List<UnionRelationshipDTO> list = new List<UnionRelationshipDTO>();
            foreach (var row in dtgUnionRelationship.SelectedItems)
            {
                list.Add(row as UnionRelationshipDTO);
            }
            if (list.Count > 1)
            {


                var source = contextMenu.GetClickedElement<GridViewRow>();
                contextMenu.Items.Clear();
                if (contextMenu != null && source != null)
                {
                    var mergeUnionMenu = AddMenu(contextMenu.Items, "ادغام روابط اجتماع انتخاب شده", "", "../Images/merge.png");
                    mergeUnionMenu.Click += (sender1, EventArgs) => customMenuItem_Click(sender1, contextMenu, list, source.DataContext as UnionRelationshipDTO);
                }

            }
            MyProjectHelper.SetContectMenuVisibility(contextMenu);
        }
        //private void AddMergeUnionRelationshipContextMenu(RadContextMenu contextMenu, List<UnionRelationshipDTO> relationships, UnionRelationshipDTO selectedOne)
        //{
        //    RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
        //    RadMenuItem customMenuItem = new RadMenuItem();
        //    customMenuItem.Header = "ادغام روابط اجتماع انتخاب شده";
        //    customMenuItem.Name = "MergeUnionRelationships";

        //    contextMenu.Items.Add(separator);
        //    contextMenu.Items.Add(customMenuItem);
        //}

        void customMenuItem_Click(object sender, RadContextMenu contextMenu, List<UnionRelationshipDTO> relationships, UnionRelationshipDTO selectedOne)
        {
            if (relationships.GroupBy(x => new { x.SuperTypeEntities, x.UnionHoldsKeys }).Count() > 1)
            {
                MessageBox.Show("برای ادغام چند رابطه اتحاد تمامی موجودیتهای پدر میباست از یک نوع باشند");
                return;
            }
            bizUnionRelationship.MergeUnionRelationships((sender as RadMenuItem).Name, relationships, selectedOne);
        }


        void dtgISARelationship_ContextMenuOpening(object sender, RoutedEventArgs e)
        {
            var contextMenu = sender as RadContextMenu;
            List<ISARelationshipDTO> list = new List<ISARelationshipDTO>();
            foreach (var row in dtgISARelationship.SelectedItems)
            {
                list.Add(row as ISARelationshipDTO);
            }
            if (list.Count > 1)
            {

                contextMenu.Items.Clear();
                var source = contextMenu.GetClickedElement<GridViewRow>();
                if (contextMenu != null && source != null)
                {
                    var mergeISAMenu = AddMenu(contextMenu.Items, "ادغام روابط ارث بری انتخاب شده", "", "../Images/merge.png");
                    mergeISAMenu.Click += (sender1, EventArgs) => customMenuItem_Click(sender1, contextMenu, list, source.DataContext as ISARelationshipDTO);
                }
            }
            MyProjectHelper.SetContectMenuVisibility(contextMenu);
        }
        //private void AddMergeISARelationshipContextMenu(RadContextMenu contextMenu, List<ISARelationshipDTO> relationships, ISARelationshipDTO selectedOne)
        //{
        //    RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
        //    RadMenuItem customMenuItem = new RadMenuItem();
        //    customMenuItem.Header = "ادغام روابط ارث بری انتخاب شده";
        //    customMenuItem.Name = "MergeISARelationships";

        //    contextMenu.Items.Add(separator);
        //    contextMenu.Items.Add(customMenuItem);
        //}

        void customMenuItem_Click(object sender, RadContextMenu contextMenu, List<ISARelationshipDTO> relationships, ISARelationshipDTO selectedOne)
        {
            if (relationships.GroupBy(x => x.SuperTypeEntities).Count() > 1)
            {
                MessageBox.Show("برای ادغام چند رابطه ارث بری تمامی موجودیتهای پدر میباست از یک نوع باشند");
                return;
            }
            bizISARelationship.MergeISARelationships((sender as RadMenuItem).Name, relationships, selectedOne);
        }


        //private void btnImportRelationships_Click(object sender, RoutedEventArgs e)
        //{
        //    if (MessageBox.Show("فرایند ورود اطلاعات خودکار روابط برای موجودیتهای پیش فرض" + Environment.NewLine + Environment.NewLine + "آیا مطمئن هستید؟", "تائید", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        //    {



        //        //try
        //        //{
        //        var result = ModelGenerator.GenerateRelationships();
        //        if (result)
        //        {

        //            dtgRelationships.ItemsSource = bizRelationship.GetRelationships(Database.ID);
        //            btnUpdateRelationships.IsEnabled = true;

        //            MessageBox.Show("Operation is completed.");
        //        }
        //        else
        //            MessageBox.Show("Operation is not done!");
        //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    MessageBox.Show("Operation failed." + Environment.NewLine + ex.Message);
        //    //}
        //}


        //private void btnUpdateRelationships_Click(object sender, RoutedEventArgs e)
        //{

        //    btnUpdateRelationships.IsEnabled = false;
        //    bizRelationship.UpdateRelationships(dtgRelationships.ItemsSource as List<RelationshipDTO>);
        //    btnUpdateRelationships.IsEnabled = true;
        //}


        //private void btnImposeRuleRelationship_Click(object sender, RoutedEventArgs e)
        //{
        //    if (MessageBox.Show("فرایند اعمال قوانین بروی روابط" + Environment.NewLine + Environment.NewLine + "آیا مطمئن هستید؟", "تائید", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        //    {
        //        bizRelationship.ImposeRelationshipRule(Database.ID);
        //        dtgRelationships.ItemsSource = bizRelationship.GetRelationships(Database.ID);
        //    }
        //}
        //private void btnUpdateOneToMany_Click(object sender, RoutedEventArgs e)
        //{

        //    btnUpdateOneToMany.IsEnabled = false;
        //    bizRelationship.UpdateOneToManyRelationships(dtgOneToMany.ItemsSource as List<OneToManyRelationshipDTO>);
        //    btnUpdateOneToMany.IsEnabled = true;
        //}
        //private void btnRefreshOneToMany_Click(object sender, RoutedEventArgs e)
        //{

        //    dtgOneToMany.ItemsSource = bizRelationship.GetOneToManyRelationships(Database.ID);
        //    btnUpdateOneToMany.IsEnabled = true;
        //}

        //private void btnUpdateManyToOne_Click(object sender, RoutedEventArgs e)
        //{

        //    btnUpdateManyToOne.IsEnabled = false;
        //    bizRelationship.UpdateManyToOneRelationships(dtgManyToOne.ItemsSource as List<ManyToOneRelationshipDTO>);
        //    btnUpdateManyToOne.IsEnabled = true;
        //}
        //private void btnRefreshManyToOne_Click(object sender, RoutedEventArgs e)
        //{

        //    dtgManyToOne.ItemsSource = bizRelationship.GetManyToOneRelationships(Database.ID);
        //    btnUpdateManyToOne.IsEnabled = true;
        //}


        //private void btnUpdateExplicit_Click(object sender, RoutedEventArgs e)
        //{

        //    btnUpdateExplicit.IsEnabled = false;
        //    bizRelationship.UpdateExplicitOneToOneRelationships(dtgExplicit.ItemsSource as List<ExplicitOneToOneRelationshipDTO>);
        //    btnUpdateExplicit.IsEnabled = true;
        //}
        //private void btnRefreshExplicit_Click(object sender, RoutedEventArgs e)
        //{

        //    dtgExplicit.ItemsSource = bizRelationship.GetExplicitOneToOneRelationships(Database.ID);
        //}
        //private void btnUpdateImplicit_Click(object sender, RoutedEventArgs e)
        //{

        //    btnUpdateImplicit.IsEnabled = false;
        //    bizRelationship.UpdateImplicitOneToOneRelationships(dtgImplicit.ItemsSource as List<ImplicitOneToOneRelationshipDTO>);
        //    btnUpdateImplicit.IsEnabled = true;
        //}
        //private void btnRefreshImplicit_Click(object sender, RoutedEventArgs e)
        //{

        //    dtgImplicit.ItemsSource = bizRelationship.GetImplicitOneToOneRelationships(Database.ID);
        //}


        private void btnUpdateManyToMany_Click(object sender, RoutedEventArgs e)
        {

            btnUpdateManyToMany.IsEnabled = false;
            bizRelationship.UpdateManyToManyRelationships(dtgManyToMany.ItemsSource as List<ManyToManyRelationshipDTO>);
            btnUpdateManyToMany.IsEnabled = true;
        }
        private void btnRefreshManyToMany_Click(object sender, RoutedEventArgs e)
        {
            btnUpdateManyToMany.IsEnabled = true;

            dtgManyToMany.ItemsSource = bizRelationship.GetManyToManyRelationships(Database.ID);
        }

        void dtgManyToMany_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (dtgManyToMany.SelectedItem != null)
            {

                ManyToManyRelationshipDTO manyToMany = dtgManyToMany.SelectedItem as ManyToManyRelationshipDTO;
                if (manyToMany != null)
                {

                    dtgManyToMany_ManyToOne.ItemsSource = bizRelationship.GetManyToMany_ManyToOneRelationships(manyToMany.ID);
                }
            }
        }

        private void btnUpdateISA_Click(object sender, RoutedEventArgs e)
        {
            BizISARelationship biz = new BizISARelationship();
            btnUpdateISA.IsEnabled = false;
            biz.UpdateISARelationships(dtgISARelationship.ItemsSource as List<ISARelationshipDTO>);
            btnUpdateISA.IsEnabled = true;
        }
        private void btnRefreshISA_Click(object sender, RoutedEventArgs e)
        {
            btnUpdateISA.IsEnabled = true;
            dtgISARelationship.ItemsSource = bizISARelationship.GetISARelationships(Database.ID);
        }
        void dtgISARelationship_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (dtgISARelationship.SelectedItem != null)
            {
                BizISARelationship biz = new BizISARelationship();
                ISARelationshipDTO iSARelationship = dtgISARelationship.SelectedItem as ISARelationshipDTO;
                if (iSARelationship != null)
                {
                    BizColumn bizColumn = new BizColumn();
                    if (iSARelationship.SuperEntityID != 0)
                    {
                        var clomuns = bizColumn.GetAllColumns(iSARelationship.SuperEntityID, true);
                        ControlHelper.AddComboColumnItemsSource(dtgSuperToSub, "DeterminerColumnID", clomuns as IEnumerable, "Alias", "ID");
                    }
                    dtgSuperToSub.ItemsSource = biz.GetSuperToSubRelationship(iSARelationship.ID);
                    dtgSubToSuper.ItemsSource = biz.GetSubToSuperRelationship(iSARelationship.ID);
                    btnUpdateSuperToSub.IsEnabled = true;
                    btnUpdateSubToSuper.IsEnabled = true;
                }
            }
        }

        private void btnUpdateSuperToSub_Click(object sender, RoutedEventArgs e)
        {
            BizISARelationship biz = new BizISARelationship();
            btnUpdateSuperToSub.IsEnabled = false;
            biz.UpdateSuperToSubRelationships(dtgSuperToSub.ItemsSource as List<SuperToSubRelationshipDTO>);
            btnUpdateSuperToSub.IsEnabled = true;
        }

        private void btnUpdateSubToSuper_Click(object sender, RoutedEventArgs e)
        {
            BizISARelationship biz = new BizISARelationship();
            btnUpdateSubToSuper.IsEnabled = false;
            biz.UpdateSubToSuperRelationships(dtgSubToSuper.ItemsSource as List<SubToSuperRelationshipDTO>);
            btnUpdateSubToSuper.IsEnabled = true;
        }



        private void btnUpdateUnionRelationship_Click(object sender, RoutedEventArgs e)
        {
            BizUnionRelationship biz = new BizUnionRelationship();
            btnUpdateUnionRelationship.IsEnabled = false;
            biz.UpdateUnionRelationships(dtgUnionRelationship.ItemsSource as List<UnionRelationshipDTO>);
            btnUpdateUnionRelationship.IsEnabled = true;
        }
        private void btnRefreshUnionRelationship_Click(object sender, RoutedEventArgs e)
        {
            btnUpdateUnionRelationship.IsEnabled = true;
            BizUnionRelationship biz = new BizUnionRelationship();
            dtgUnionRelationship.ItemsSource = biz.GetUnionRelationships(Database.ID);
        }
        void dtgUnionRelationship_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (dtgUnionRelationship.SelectedItem != null)
            {
                BizUnionRelationship biz = new BizUnionRelationship();
                UnionRelationshipDTO UnionRelationship = dtgUnionRelationship.SelectedItem as UnionRelationshipDTO;
                if (UnionRelationship != null)
                {
                    BizColumn bizColumn = new BizColumn();
                    if (UnionRelationship.SuperEntityID != 0)
                    {
                        var clomuns = bizColumn.GetAllColumns(UnionRelationship.SuperEntityID, true);
                        ControlHelper.AddComboColumnItemsSource(dtgUnionToSubUnion, "DeterminerColumnID", clomuns as IEnumerable, "Alias", "ID");
                    }
                    dtgUnionToSubUnion.ItemsSource = biz.GetSuperUnionToSubUnionRelationship(UnionRelationship.ID);
                    dtgSubUnionToUnion.ItemsSource = biz.GetSubUnionToSuperUnionRelationship(UnionRelationship.ID);
                    btnUpdateSuperToSub.IsEnabled = true;
                    btnUpdateSubToSuper.IsEnabled = true;
                }
            }
        }

        private void btnUpdateSuperUnionToSubUnion_Click(object sender, RoutedEventArgs e)
        {
            BizUnionRelationship biz = new BizUnionRelationship();
            btnUpdateSuperUnionToSubUnion.IsEnabled = false;
            biz.UpdateSuperUnionToSubUnionRelationships(dtgUnionToSubUnion.ItemsSource as List<UnionToSubUnionRelationshipDTO>);
            btnUpdateSuperUnionToSubUnion.IsEnabled = true;
        }

        private void btnUpdateSubUnionToSuperUnion_Click(object sender, RoutedEventArgs e)
        {
            BizUnionRelationship biz = new BizUnionRelationship();
            btnUpdateSubUnionToSuperUnion.IsEnabled = false;
            biz.UpdateSubUnionToSuperUnionRelationships(dtgSubUnionToUnion.ItemsSource as List<SubUnionToSuperUnionRelationshipDTO>);
            btnUpdateSubUnionToSuperUnion.IsEnabled = true;
        }


    }
}
