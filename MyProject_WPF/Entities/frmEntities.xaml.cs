using ModelEntites;
using MyCommonWPFControls;
using MyFormulaFunctionStateFunctionLibrary;
using MyModelGenerator;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmEntities.xaml
    /// </summary>
    public partial class frmEntities : UserControl
    {
        //private ModelGenerator_sql ModelGenerator = new ModelGenerator_sql();
        BizColumn bizColumn = new BizColumn();
        BizRelationship bizRelationship = new BizRelationship();
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        BizTable bizTable = new BizTable();
        int DatabaseID { set; get; }
        MyStaticLookupColumn formulaColumn;
        public frmEntities(int databaseID)
        {
            InitializeComponent();
            DatabaseID = databaseID;
            dtgRuleEntity.SelectionChanged += dtgRuleEntity_SelectionChanged;
            dtgColumns.SelectionChanged += dtgColumns_SelectionChanged;
            dtgColumns.RowLoaded += DtgColumns_RowLoaded;
            dtgColumns.EnableColumnVirtualization = false;

            var entityMenu = new RadContextMenu();
            entityMenu.Opened += dtgRuleEntity_ContextMenuOpening;
            RadContextMenu.SetContextMenu(dtgRuleEntity, entityMenu);

            var columnMenu = new RadContextMenu();
            columnMenu.Opened += dtgColumns_ContextMenuOpening;
            RadContextMenu.SetContextMenu(dtgColumns, columnMenu);
            SetGridViewColumns();
            this.Loaded += FrmEntities_Loaded;
            SetColumnTabs();
            tabColumns.Visibility = Visibility.Collapsed;
            tabRelationships.Visibility = Visibility.Collapsed;


            //dtgNumericColumnType.CellEditEnded += DtgNumericColumnType_CellEditEnded;
            //dtgNumericColumnType.CellValidating += DtgNumericColumnType_CellValidating;
        }

        //private void DtgNumericColumnType_CellValidating(object sender, GridViewCellValidatingEventArgs e)
        //{
        //    var aa = e.Cell;
        //}

        //private void DtgNumericColumnType_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        //{
        //    var aa = e.EditingElement;
        //    var bb = e.Source;

        //}

        private void DtgColumns_RowLoaded(object sender, RowLoadedEventArgs e)
        {
            if (e.DataElement is ColumnDTO)
            {
                var column = e.DataElement as ColumnDTO;
                if (column.PrimaryKey || column.ForeignKey)
                {
                    var cell = e.Row.Cells.FirstOrDefault(x => x.Column.UniqueName == "IsDisabled");
                    if (cell != null)
                        cell.IsEnabled = false;

                    cell = e.Row.Cells.FirstOrDefault(x => x.Column.UniqueName == "DataEntryEnabled");
                    if (cell != null)
                        cell.IsEnabled = false;

                    cell = e.Row.Cells.FirstOrDefault(x => x.Column.UniqueName == "IsReadonly");
                    if (cell != null)
                        cell.IsEnabled = false;

                    cell = e.Row.Cells.FirstOrDefault(x => x.Column.UniqueName == "IsNotTransferable");
                    if (cell != null)
                        cell.IsEnabled = false;

                }
            }
        }

        private void FrmEntities_Loaded(object sender, RoutedEventArgs e)
        {
            SetImportedInfo();
        }

        private void SetGridViewColumns()
        {
            SetGridViewColumns(dtgRuleEntity);
            SetGridViewColumns(dtgColumns);
            SetGridViewColumns(dtgStringColumnType);
            SetGridViewColumns(dtgNumericColumnType);
            SetGridViewColumns(dtgDateColumnType);

        }

        private void SetGridViewColumns(RadGridView dataGrid)
        {
            dataGrid.AlternateRowBackground = new SolidColorBrush(Colors.LightBlue);
            dataGrid.AutoGenerateColumns = false;

            if (dataGrid == dtgRuleEntity)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "شناسه", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("RelatedSchema", "شمای مرتبط", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "نام", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "عنوان", false, 100, GridViewColumnType.Text));
                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Criteria", "شرط", false, 200, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IndependentDataEntry", "ورود اطلاعات مستقل", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("BatchDataEntry", "ورود اطلاعات دسته ای", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsDataReference", "مرجع داده", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsDisabled", "غیر فعال", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsReadonly", "فقط خواندنی", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsStructurReferencee", "مرجع ساختاری", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsAssociative", "جدول واسط", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SelectAsComboBox", "جستجوی کمبو", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SearchInitially", "جستجوی خودکار", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("LoadArchiveRelatedItems", "نمایش آرشیو مرتبط", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("LoadLetterRelatedItems", "نمایش نامه های مرتبط", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Color", "رنگ مرتبط", false, 160, GridViewColumnType.Color));
                //MyStaticLookupColumn lookup = new MyStaticLookupColumn();
                //lookup.DisplayMemberPath = "Name";
                //lookup.SelectedValueMemberPath = "ID";
                //BizColumnValueRange bizColumnValueRange = new BizColumnValueRange();
                //var list=bizColumnValueRange.GetColumnValueRange
            }
            else if (dataGrid == dtgColumns)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "شناسه", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "نام", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "عنوان", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PrimaryKey", "کلید اصلی", true, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsIdentity", "شماره خودکار", true, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsNull", "مقدار Null", true, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsMandatory", "اجباری", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DefaultValue", "مقدار پیش فرض", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ShowNullValue", "نمایش مقدار Null", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataType", "نوع داده", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Position", "موقعیت", false, 100, GridViewColumnType.Numeric));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsDisabled", "غیر فعال", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataEntryEnabled", "قابلیت ورود اطلاعات", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsReadonly", "فقط خواندنی", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsNotTransferable", "غیر قابل انتقال", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DBFormula", "فرمول پایگاه داده", true, 100, GridViewColumnType.Text));

                formulaColumn = new MyStaticLookupColumn();
                formulaColumn.DataMemberBinding = new Binding("CustomFormulaID");
                formulaColumn.Header = "فرمول";
                formulaColumn.NewItemEnabled = true;
                formulaColumn.EditItemEnabled = true;
                formulaColumn.EditItemClicked += formulaColumn_EditItemClicked;
                dataGrid.Columns.Add(formulaColumn);

                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Description", "توضیحات", false, 100, GridViewColumnType.Text));

                //    dataGrid.Columns MyStaticLookupColumn DataMemberBinding = "{Binding RelationshipTailID}"  x: Name = "colRelationshipTail" Header = "رشته رابطه" ></ MyCommonWPFControls:MyStaticLookupColumn >

            }
            else if (dataGrid == dtgStringColumnType)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ColumnID", "شناسه ستون", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("MaxLength", "حداکثر طول", false, 100, GridViewColumnType.Numeric));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Format", "فرمت", false, 200, GridViewColumnType.Text));
            }
            else if (dataGrid == dtgNumericColumnType)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ColumnID", "شناسه ستون", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Precision", "رقم صحیح", false, 100, GridViewColumnType.Numeric));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Scale", "رقم اعشار", false, 200, GridViewColumnType.Numeric));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("MinValue", "مقدار کمینه", false, 100, GridViewColumnType.Numeric));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("MaxValue", "مقدار بیشینه", false, 200, GridViewColumnType.Numeric));
            }
            else if (dataGrid == dtgDateColumnType)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ColumnID", "شناسه ستون", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ShowMiladiDateInUI", "نمایش تاریخ میلادی", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("StringValueIsMiladi", "مقدار رشته ای میلادی", false, 100, GridViewColumnType.CheckBox));

            }

        }
        private void formulaColumn_EditItemClicked(object sender, EditItemClickEventArg e)
        {
            int formulaID = 0;
            if (e.DataConext is ColumnDTO)
            {
                formulaID = (e.DataConext as ColumnDTO).CustomFormulaID;
            }
            var entity = dtgRuleEntity.SelectedItem as TableDrivedEntityDTO;
            frmFormula view = new frmFormula(formulaID, entity.ID);
            view.FormulaUpdated += (sender1, e1) => View_ItemSelected(sender1, e1, (sender as MyStaticLookup), entity.ID);
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "فرمول", Enum_WindowSize.Big);

        }
        private void SetColumnFormulas(int entityID)
        {
            BizFormula bizFormula = new BizFormula();
            //var entity = dtgRuleEntity.SelectedItem as TableDrivedEntityDTO;
            var formulas = bizFormula.GetFormulas(entityID);
            formulaColumn.DisplayMemberPath = "Name";
            formulaColumn.SelectedValueMemberPath = "ID";
            formulaColumn.ItemsSource = formulas;
        }
        private void View_ItemSelected(object sender, FormulaSelectedArg e, MyStaticLookup lookup, int entityID)
        {
            SetColumnFormulas(entityID);
            if (lookup != null)
            {
                lookup.SelectedValue = e.FormulaID;
            }
            MyProjectManager.GetMyProjectManager.CloseDialog(sender);
        }

        internal void ActivateEntities()
        {
            tabEntities.IsSelected = true;
        }

        //private void btnImportEntities_Click(object sender, RoutedEventArgs e)
        //{
        //    if (MessageBox.Show("فرایند ورود اطلاعات خودکار موجودیتها و ستونها" + Environment.NewLine + Environment.NewLine + "آیا مطمئن هستید؟", "تائید", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        //    {



        //        //try
        //        //{
        //        var result = ModelGenerator.GenerateDefaultEntitiesAndColumns();
        //        if (result)
        //        {

        //            dtgTables.ItemsSource = bizTable.GetTables(DatabaseID);
        //            btnUpdateTables.IsEnabled = true;
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


        void dtgRuleEntity_SelectionChanged(object sender, RoutedEventArgs e)
        {
            string columnsHeader = "ستونها";
            string realtionshipsHeader = "روابط";
            if (dtgRuleEntity.SelectedItem != null &&
                dtgRuleEntity.SelectedItem is TableDrivedEntityDTO)
            {
                TableDrivedEntityDTO entity = dtgRuleEntity.SelectedItem as TableDrivedEntityDTO;

                tabColumns.Visibility = Visibility.Visible;
                tabRelationships.Visibility = Visibility.Visible;

                var columns = bizColumn.GetAllColumns(entity.ID, true);
                //بهتره ستونها کامل گرفته نشوند و تک به تک کامل شوند
                dtgColumns.ItemsSource = columns;
                SetColumnTabs();
                SetColumnFormulas(entity.ID);
                formRelationships.SetRelatoinships(entity.ID);

                columnsHeader = "ستونهای" + " " + "\"" + entity.Alias + "\"";
                realtionshipsHeader = "روابط" + " " + "\"" + entity.Alias + "\"";
                //var relationships = bizRelationship.GetAllRelationships(entity.ID);
                //dtgRelationships.ItemsSource = columns;
                //btnUpdateColumns.IsEnabled = true;

            }
            tabColumns.Header = columnsHeader;
            tabRelationships.Header = realtionshipsHeader;
        }


        private void btnUpdateColumns_Click(object sender, RoutedEventArgs e)
        {
            btnUpdateColumns.IsEnabled = false;
            TableDrivedEntityDTO entity = dtgRuleEntity.SelectedItem as TableDrivedEntityDTO;
            bizColumn.UpdateColumns(entity.ID, dtgColumns.ItemsSource as List<ColumnDTO>);
            btnUpdateColumns.IsEnabled = true;
        }
        void dtgColumns_SelectionChanged(object sender, RoutedEventArgs e)
        {
            SetColumnTabs();
            if (dtgColumns.SelectedItem != null)
            {
                var column = dtgColumns.SelectedItem as ColumnDTO;
                if (column != null)
                {
                    if (column.ColumnType == Enum_ColumnType.String)
                    {
                        dtgStringColumnType.ItemsSource = bizColumn.GetStringColumType(column.ID);
                    }
                    else if (column.ColumnType == Enum_ColumnType.Numeric)
                    {
                        dtgNumericColumnType.ItemsSource = bizColumn.GetNumericColumType(column.ID);
                    }
                    else if (column.ColumnType == Enum_ColumnType.Date)
                    {
                        dtgDateColumnType.ItemsSource = bizColumn.GetDateColumType(column.ID);
                    }

                    //var columnKeyValue = bizColumn.GetColumnKeyValue(column.ID);
                    //var formulaDTO = bizColumn.GetCustomCalculationFormula(column.ID);
                    //tabColumnFormula.DataContext = formulaDTO;
                    //if (formulaDTO != null)
                    //{
                    //    txtColumnFormula.Text = formulaDTO.Formula;
                    //}
                    //else
                    //    txtColumnFormula.Text = "";

                }
            }
        }

        private void SetColumnTabs()
        {
            if (dtgColumns.SelectedItem != null)
            {
                tabStringColumnType.Visibility = System.Windows.Visibility.Collapsed;
                tabNumericColumnType.Visibility = System.Windows.Visibility.Collapsed;
                tabDateColumnType.Visibility = System.Windows.Visibility.Collapsed;

                var column = dtgColumns.SelectedItem as ColumnDTO;


                if (column.ColumnType == Enum_ColumnType.String)
                {
                    tabStringColumnType.Visibility = System.Windows.Visibility.Visible;
                    tabStringColumnType.IsSelected = true;
                    btnUpdateStringColumnType.IsEnabled = true;
                }
                else if (column.ColumnType == Enum_ColumnType.Numeric)
                {
                    tabNumericColumnType.Visibility = System.Windows.Visibility.Visible;
                    tabNumericColumnType.IsSelected = true;
                    btnUpdateNumericColumnType.IsEnabled = true;
                }
                else if (column.ColumnType == Enum_ColumnType.Date)
                {
                    tabDateColumnType.Visibility = System.Windows.Visibility.Visible;
                    tabDateColumnType.IsSelected = true;
                    btnUpdateDateColumnType.IsEnabled = true;
                }
            }
            else
            {
                tabStringColumnType.Visibility = System.Windows.Visibility.Visible;
                tabNumericColumnType.Visibility = System.Windows.Visibility.Visible;
                tabDateColumnType.Visibility = System.Windows.Visibility.Visible;
                btnUpdateStringColumnType.IsEnabled = false;
                btnUpdateNumericColumnType.IsEnabled = false;
                btnUpdateDateColumnType.IsEnabled = false;
                tabStringColumnType.IsSelected = true;
                dtgStringColumnType.ItemsSource = null;
                dtgNumericColumnType.ItemsSource = null;
                dtgDateColumnType.ItemsSource = null;
            }
        }


        //private void View_FormulaSelected(object sender, FormulaSelectedArg e)
        //{
        //    var column = dtgColumns.SelectedItem as ColumnDTO;
        //    var formulaDTO = bizColumn.GetCustomCalculationFormula(column.ID);
        //    //tabColumnFormula.DataContext = formulaDTO;
        //    //if (formulaDTO != null)
        //    //{
        //    //    txtColumnFormula.Text = formulaDTO.Formula;
        //    //}
        //}

        private void btnUpdateNumericColumnType_Click(object sender, RoutedEventArgs e)
        {

            btnUpdateNumericColumnType.IsEnabled = false;
            bizColumn.UpdateNumericColumnType(dtgNumericColumnType.ItemsSource as List<NumericColumnTypeDTO>);
            btnUpdateNumericColumnType.IsEnabled = true;
        }

        private void btnUpdateStringColumnType_Click(object sender, RoutedEventArgs e)
        {

            btnUpdateStringColumnType.IsEnabled = false;
            bizColumn.UpdateStringColumnType(dtgStringColumnType.ItemsSource as List<StringColumnTypeDTO>);
            btnUpdateStringColumnType.IsEnabled = true;
        }

        private void btnUpdateDateColumnType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnUpdateDateColumnType.IsEnabled = false;
                bizColumn.UpdateDateColumnType(dtgDateColumnType.ItemsSource as List<DateColumnTypeDTO>);
                btnUpdateDateColumnType.IsEnabled = true;
            }
            catch (Exception ex)
            {
                btnUpdateDateColumnType.IsEnabled = true;
                MessageBox.Show(ex.Message);
            }
        }



        void dtgRuleEntity_ContextMenuOpening(object sender, RoutedEventArgs e)
        {
            var contextMenu = sender as RadContextMenu;
            contextMenu.Items.Clear();
            var source = contextMenu.GetClickedElement<GridViewRow>();
            if (contextMenu != null && source != null)
            {
                //RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
                //contextMenu.Items.Add(separator);
                var entity = source.DataContext as TableDrivedEntityDTO;
                if (entity.IsView == false)
                {
                    var formuamMenu = AddMenu(contextMenu.Items, "فرمول", "", "../Images/formula.png");
                    var parameterMenu = AddMenu(formuamMenu.Items, "تعریف فرمول", "", "../Images/parameter.png");
                    parameterMenu.Click += (sender1, EventArgs) => customMenuItem_EntityParameters(sender, e, source.DataContext as TableDrivedEntityDTO);


                    //       var duplicateGMenu = AddMenu(contextMenu.Items, "موجودیت مشابه", "", "../Images/duplicate.png");
                    var duplicateHMenu = AddMenu(contextMenu.Items, "ایجاد موجودیت مشابه با رابطه ارث بری", "", "../Images/duplicate.png");
                    duplicateHMenu.Click += (sender1, EventArgs) => customMenuItem_Click1(sender, e, source.DataContext as TableDrivedEntityDTO);
                    //    var duplicateSMenu = AddMenu(duplicateGMenu.Items, "ایجاد موجودیت مشابه", "", "../Images/copy.png");
                    //   duplicateSMenu.Click += (sender1, EventArgs) => customMenuItem_Click11(sender, e, source.DataContext as TableDrivedEntityDTO);



                    var formRequirementsMenu = AddMenu(contextMenu.Items, "نیازمندیهای داخل فرم", "", "../Images/form.png");

                    var uiCompositionMenu = AddMenu(formRequirementsMenu.Items, "چینش داخل فرم", "", "../Images/uicomposition.png");
                    uiCompositionMenu.Click += (sender1, EventArgs) => customMenuItem_ClickUIComposition(sender, e, source.DataContext as TableDrivedEntityDTO);

                    //var uiSettingMenu = AddMenu(formRequirementsMenu.Items, "تنظیمات ظاهری", "", "../Images/uicomposition.png");
                    //uiSettingMenu.Click += (sender1, EventArgs) => customMenuItem_ClickUISetting(sender, e, source.DataContext as TableDrivedEntityDTO);


                    //var arcMenu = AddMenu(formRequirementsMenu.Items, "تعریف روابط انحصاری", "", "../Images/relationship1.png");
                    //arcMenu.Click += (sender1, EventArgs) => customMenuItem_ClickArc(sender, e, source.DataContext as TableDrivedEntityDTO);

                    var validationMenu = AddMenu(formRequirementsMenu.Items, "تعریف روالهای اعتبارسنجی", "", "../Images/validate.png");
                    validationMenu.Click += (sender1, EventArgs) => customMenuItem_ClickValidation(sender, e, source.DataContext as TableDrivedEntityDTO);

                    var stateMenu = AddMenu(formRequirementsMenu.Items, "وضعیتهای موجودیت", "", "../Images/state.png");
                    stateMenu.Click += (sender1, EventArgs) => customMenuItem_ClickState(sender, e, source.DataContext as TableDrivedEntityDTO);

                    var actionActivitMenu = AddMenu(formRequirementsMenu.Items, "اقدامات موجودیت", "", "../Images/action.png");
                    actionActivitMenu.Click += (sender1, EventArgs) => customMenuItem_ClickActionActivity(sender, e, source.DataContext as TableDrivedEntityDTO);

                    var commandMenu = AddMenu(formRequirementsMenu.Items, "دکمه ها", "", "../Images/command.png");
                    commandMenu.Click += (sender1, EventArgs) => customMenuItem_ClickCommnad(sender, e, source.DataContext as TableDrivedEntityDTO);


                    var archiveMenu = AddMenu(contextMenu.Items, "آرشیو", "", "../Images/archive.png.png");

                    var archiveTagMenu = AddMenu(archiveMenu.Items, "تگ آرشیو", "", "../Images/archivetag.png");
                    archiveTagMenu.Click += (sender1, EventArgs) => customMenuItem_ClickArchiveTag(sender, e, source.DataContext as TableDrivedEntityDTO);

                    var archiveeFolderMenu = AddMenu(archiveMenu.Items, "فولدر آرشیو", "", "../Images/folder.png");
                    archiveeFolderMenu.Click += (sender1, EventArgs) => customMenuItem_ClickArchiveFolder(sender, e, source.DataContext as TableDrivedEntityDTO);

                    var archiveeRelationshipTail = AddMenu(archiveMenu.Items, "روابط آرشیو", "", "../Images/relationship1.png");
                    archiveeRelationshipTail.Click += (sender1, EventArgs) => customMenuItem_ClickArchiveRelationshipTail(sender, e, source.DataContext as TableDrivedEntityDTO);


                    var letterMenu = AddMenu(contextMenu.Items, "نامه", "", "../Images/mail.png.png");

                    var letterTemplateMenu = AddMenu(letterMenu.Items, "قالب نامه", "", "../Images/mail.png");
                    letterTemplateMenu.Click += (sender1, EventArgs) => customMenuItem_ClickLetterTemplate(sender, e, source.DataContext as TableDrivedEntityDTO);

                    var letterRelationshipTail = AddMenu(letterMenu.Items, "روابط نامه", "", "../Images/relationship1.png");
                    letterRelationshipTail.Click += (sender1, EventArgs) => customMenuItem_ClickLetterRelationshipTail(sender, e, source.DataContext as TableDrivedEntityDTO);


                    var searchEntityGMenu = AddMenu(contextMenu.Items, "جستجوی داده ها", "", "../Images/search.png");
                    var searchEntityMenu = AddMenu(searchEntityGMenu.Items, "فیلدهای جستجوی سریع", "", "../Images/listView.png");
                    searchEntityMenu.Click += (sender1, EventArgs) => customMenuItem_ClickEntitySearch(sender, e, source.DataContext as TableDrivedEntityDTO);

                    var dataLinkMenu = AddMenu(contextMenu.Items, "لینک داده", "", "../Images/datalink.png");
                    dataLinkMenu.Click += (sender1, EventArgs) => customMenuItem_EntityDataLink(sender, e, source.DataContext as TableDrivedEntityDTO);

                    var listReportSetting = AddMenu(contextMenu.Items, "تنیمات منوی داده", "", "../Images/listView.png");
                    listReportSetting.Click += (sender1, EventArgs) => customMenuItem_ClickMenuSetting(sender, e, source.DataContext as TableDrivedEntityDTO);



                }



                var dataViewGMenu = AddMenu(contextMenu.Items, "نمایش داده ها", "", "../Images/view.png");

                var listViewMenu = AddMenu(dataViewGMenu.Items, "لیست نمایش", "", "../Images/listView.png");
                listViewMenu.Click += (sender1, EventArgs) => customMenuItem_ClickListView(sender, e, source.DataContext as TableDrivedEntityDTO);

                var dataViewMenu = AddMenu(dataViewGMenu.Items, "نمایش داده", "", "../Images/dataview.png");
                dataViewMenu.Click += (sender1, EventArgs) => customMenuItem_EntityDataView(sender, e, source.DataContext as TableDrivedEntityDTO);

                var dataGridMenu = AddMenu(dataViewGMenu.Items, "گرید داده", "", "../Images/listView.png");
                dataGridMenu.Click += (sender1, EventArgs) => customMenuItem_EntityGridView(sender, e, source.DataContext as TableDrivedEntityDTO);




                var reportMenu = AddMenu(contextMenu.Items, "گزارشات", "", "../Images/report.png");

                if (entity.IsView == false)
                {
                    var directReportMenu = AddMenu(reportMenu.Items, "گزارشات مستقیم", "", "../Images/listView.png");
                    directReportMenu.Click += (sender1, EventArgs) => customMenuItem_ClickDirectReport(sender, e, source.DataContext as TableDrivedEntityDTO);

                    //var datalinkReportMenu = AddMenu(reportMenu.Items, "گزارش لینک داده", "", "../Images/dataview.png");
                    //datalinkReportMenu.Click += (sender1, EventArgs) => customMenuItem_ClickDataLinkReport(sender, e, source.DataContext as TableDrivedEntityDTO);

                    var externalReportMenu = AddMenu(reportMenu.Items, "گزارش خارجی", "", "../Images/externalreport.png");
                    externalReportMenu.Click += (sender1, EventArgs) => customMenuItem_ClickExternalReport(sender, e, source.DataContext as TableDrivedEntityDTO);


                }



                var dataviewReportMenu = AddMenu(reportMenu.Items, "گزارش نمایش داده", "", "../Images/dataview.png");
                dataviewReportMenu.Click += (sender1, EventArgs) => customMenuItem_ClickDataViewReport(sender, e, source.DataContext as TableDrivedEntityDTO);

                var gridviewReportMenu = AddMenu(reportMenu.Items, "گزارش گرید داده", "", "../Images/dataview.png");
                gridviewReportMenu.Click += (sender1, EventArgs) => customMenuItem_ClickGridViewReport(sender, e, source.DataContext as TableDrivedEntityDTO);


                var listReportMenu = AddMenu(reportMenu.Items, "گزارش لیستی", "", "../Images/listView.png");
                listReportMenu.Click += (sender1, EventArgs) => customMenuItem_ClickListReport(sender, e, source.DataContext as TableDrivedEntityDTO);

                //var listReportGroupedMenu = AddMenu(reportMenu.Items, "گزارش گروهبندی شده", "", "../Images/listgroup.png");
                //listReportGroupedMenu.Click += (sender1, EventArgs) => customMenuItem_ClickListReportGrouped(sender, e, source.DataContext as TableDrivedEntityDTO);

                var chartReportMenu = AddMenu(reportMenu.Items, "گزارش چارت", "", "../Images/piechart.png");
                chartReportMenu.Click += (sender1, EventArgs) => customMenuItem_ClickChartReport(sender, e, source.DataContext as TableDrivedEntityDTO);

                var crosstabReportMenu = AddMenu(reportMenu.Items, "گزارش کراس تب", "", "../Images/crosstab.png");
                crosstabReportMenu.Click += (sender1, EventArgs) => customMenuItem_ClickCrosstabReport(sender, e, source.DataContext as TableDrivedEntityDTO);










                //RadMenuItem customMenuItemActionActivities = new RadMenuItem();
                //customMenuItemActionActivities.Header = "کلیه اقدامات";
                //customMenuItemActionActivities.Name = "EntityStates";
                //customMenuItemActionActivities.Click += CustomMenuItemActionActivities_Click;
                //contextMenu.Items.Add(customMenuItemActionActivities);
            }
            MyProjectHelper.SetContectMenuVisibility(contextMenu);
        }



        private RadMenuItem AddMenu(ItemCollection itemCollection, string title, string name, string imagePath)
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
            //pnl.Children.Add(img);
            menu.Header = title;
            if (!string.IsNullOrEmpty(name))
                menu.Name = name;
            itemCollection.Add(menu);
            return menu;
        }

        void customMenuItem_EntityDataLink(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        {
            frmDataLink frm = new frmDataLink(entity.ID, 0);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "لینک داده", Enum_WindowSize.Big);
        }
        void customMenuItem_EntityGridView(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        {
            frmEntityGridView frm = new frmEntityGridView(entity.ID);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "گرید داده", Enum_WindowSize.Big);
        }
        void customMenuItem_EntityDataView(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        {
            frmEntityDataView frm = new frmEntityDataView(entity.ID);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "نمایش داده", Enum_WindowSize.Big);
        }
        void customMenuItem_EntityParameters(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        {
            frmFormula frm = new frmFormula(0, entity.ID);
            frm.FormulaUpdated += (sender1, e1) => Frm_FormulaUpdated(sender1, e1, entity.ID);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "frmFormula", Enum_WindowSize.Big);
        }

        private void Frm_FormulaUpdated(object sender, FormulaSelectedArg e, int entityID)
        {
            SetColumnFormulas(entityID);
        }



        //private void CustomMenuItemActionActivities_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        //{
        //    frmActionActivity frm = new frmActionActivity(0, GeneralHelper.GetCatalogName(databaseID), 0);
        //    frm.ShowDialog();
        //}
        void customMenuItem_ClickArchiveTag(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        {
            frmArchiveTag frm = new frmArchiveTag(entity.ID);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "تگ آرشیو", Enum_WindowSize.Big);
        }
        void customMenuItem_ClickArchiveFolder(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        {
            frmArchiveFolder frm = new frmArchiveFolder(entity.ID);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "فولدر آرشیو", Enum_WindowSize.Big);
        }
        void customMenuItem_ClickArchiveRelationshipTail(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        {
            frmArchiveRelationships frm = new frmArchiveRelationships(entity.ID);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "روابط آرشیو", Enum_WindowSize.Big);
        }
        void customMenuItem_ClickLetterRelationshipTail(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        {
            frmLetterRelationships frm = new frmLetterRelationships(entity.ID);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "روابط نامه", Enum_WindowSize.Big);
        }
        void customMenuItem_ClickLetterTemplate(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        {
            frmMainLetterTemplate frm = new frmMainLetterTemplate(entity.ID, 0);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "قالب نامه", Enum_WindowSize.Big);
        }

        void customMenuItem_ClickCrosstabReport(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        {
            frmEntityCrosstabReport frm = new frmEntityCrosstabReport(entity.ID, 0);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "frmEntityCrosstabReport", Enum_WindowSize.Big);
        }

        void customMenuItem_ClickChartReport(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        {
            frmEntityChartReport frm = new frmEntityChartReport(entity.ID, 0);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "frmEntityChartReport", Enum_WindowSize.Big);
        }
        //void customMenuItem_ClickListReportGrouped(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        //{
        //    frmEntityListReportGrouped frm = new frmEntityListReportGrouped(entity.ID, 0);
        //    MyProjectManager.GetMyProjectManager.ShowDialog(frm, "frmEntityListReportGrouped", Enum_WindowSize.Big);
        //}
        void customMenuItem_ClickDataViewReport(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        {
            frmEntityDataViewReport frm = new frmEntityDataViewReport(entity.ID, 0);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "frmEntityDataViewReport", Enum_WindowSize.Big);
        }
        //void customMenuItem_ClickDataLinkReport(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        //{
        //    frmEntityDataLinkReport frm = new frmEntityDataLinkReport(entity.ID, 0);
        //    MyProjectManager.GetMyProjectManager.ShowDialog(frm, "frmEntityDataLinkReport", Enum_WindowSize.Big);
        //}
        void customMenuItem_ClickGridViewReport(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        {
            frmEntityGridViewReport frm = new frmEntityGridViewReport(entity.ID, 0);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "frmEntityGridViewReport", Enum_WindowSize.Big);
        }
        void customMenuItem_ClickDirectReport(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        {
            frmEntityDirectReport frm = new frmEntityDirectReport(entity.ID, 0);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "frmEntityDirectReport", Enum_WindowSize.Big);
        }
        void customMenuItem_ClickListReport(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        {
            frmEntityListReport frm = new frmEntityListReport(entity.ID, 0);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "frmEntityListReport", Enum_WindowSize.Big);
        }

        void customMenuItem_ClickMenuSetting(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        {
            frmDataMenuSetting frm = new frmDataMenuSetting(entity.ID);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "frmDataMenuSetting", Enum_WindowSize.Big);
        }
        void customMenuItem_ClickExternalReport(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        {
            frmExternalReport frm = new frmExternalReport(entity.ID, 0);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "frmExternalReport", Enum_WindowSize.Big);
        }
        void customMenuItem_ClickEntitySearch(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        {
            frmEntitySearch frm = new frmEntitySearch(entity.ID, 0);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "فیلدهای جستجو", Enum_WindowSize.Big);
        }
        void customMenuItem_ClickListView(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        {
            frmEntityListView frm = new frmEntityListView(entity.ID, 0);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "لیست نمایش", Enum_WindowSize.Big);
        }
        void customMenuItem_ClickActionActivity(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        {
            frmBackendActionActivitySelect frm = new frmBackendActionActivitySelect(entity.ID);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "frmEntityActionActivity", Enum_WindowSize.Big);
        }
        void customMenuItem_ClickState(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        {
            frmEntityStates frm = new frmEntityStates(entity.ID, 0);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "frmEntityStates", Enum_WindowSize.Big);
        }

        void customMenuItem_ClickValidation(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        {
            frmEntityValidations frm = new frmEntityValidations(entity.ID, 0);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "frmEntityValidations", Enum_WindowSize.Big);
        }
        void customMenuItem_ClickCommnad(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        {
            frmEntityCommand frm = new frmEntityCommand(entity.ID, 0);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "frmEntityCommand", Enum_WindowSize.Big);
        }

        //void customMenuItem_ClickUISetting(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        //{
        //    frmEntityUISetting frm = new frmEntityUISetting(entity.ID);
        //    frm.ShowDialog();
        //}
        void customMenuItem_Click1(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        {
            frmEditBaseEntity frm = new frmEditBaseEntity(entity.ID);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "EditEntity", Enum_WindowSize.Maximized);
        }
        //void customMenuItem_Click11(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        //{
        //    frmEditBaseEntity frm = new frmEditBaseEntity(entity.ID);
        //    MyProjectManager.GetMyProjectManager.ShowDialog(frm, "EditEntity", Enum_WindowSize.Big);
        //}
        //void customMenuItem_ClickArc(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        //{
        //    frmArcRelationships view = new frmArcRelationships(entity.ID);
        //    MyProjectManager.GetMyProjectManager.ShowDialog(view, "تعریف اقدامات", Enum_WindowSize.Big);
        //}
        void customMenuItem_ClickUIComposition(object sender, RoutedEventArgs e, TableDrivedEntityDTO entity)
        {
            frmEntityUIComposition view = new frmEntityUIComposition(entity.ID);
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "frmEntityUIComposition", Enum_WindowSize.Big);
        }
        void dtgColumns_ContextMenuOpening(object sender, RoutedEventArgs e)
        {
            var contextMenu = sender as RadContextMenu;
            contextMenu.Items.Clear();
            var source = contextMenu.GetClickedElement<GridViewRow>();
            if (contextMenu != null && source != null)
            {
                //RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
                //contextMenu.Items.Add(separator);
                var column = source.DataContext as ColumnDTO;
                if (column.ColumnType == Enum_ColumnType.String)
                {
                    var convertToDateColumnMenu = AddMenu(contextMenu.Items, "تبدیل به تاریخ", "", "../Images/date.png");
                    convertToDateColumnMenu.Click += (sender1, EventArgs) => ConvertToDateColumnType_Click1(sender, e, column.ID);
                }
                else if (column.OriginalColumnType == Enum_ColumnType.String && column.ColumnType == Enum_ColumnType.Date)
                {
                    var convertToStringColumnMenu = AddMenu(contextMenu.Items, "تبدیل به نوع رشته", "", "../Images/string.png");
                    convertToStringColumnMenu.Click += (sender1, EventArgs) => ConvertToStringColumnType_Click1(sender, e, column.ID);
                }

                //var columnRuleMenu = AddMenu(contextMenu.Items, "اصلاح شروط بری ستون", "", "../Images/columnrule.png");
                //columnRuleMenu.Click += (sender1, EventArgs) => ConvertToDateColumnType_Click1RuleOnValue(sender, e, column.ID);
                var columnValueRangeMenu = AddMenu(contextMenu.Items, "تعریف لیست مقادیر", "", "../Images/columnrule.png");
                columnValueRangeMenu.Click += (sender1, EventArgs) => DefineColumnValueForColumn(sender, e, column);

                //var columnValueRangeRempveMenu = AddMenu(contextMenu.Items, "حذف لیست مقادیر", "", "../Images/columnrule.png");
                //columnValueRangeRempveMenu.Click += (sender1, EventArgs) => RemoveColumnValueForColumn(sender, e, column);



                //var columnFormulaMenu = AddMenu(contextMenu.Items, "تعریف فرمول محاسباتی بروی ستون", "", "../Images/formula1.png");
                //columnFormulaMenu.Click += (sender1, EventArgs) => DefineFormulaForColumn(sender, e, column.ID);
            }
            MyProjectHelper.SetContectMenuVisibility(contextMenu);
        }
        void ConvertToStringColumnType_Click1(object sender, RoutedEventArgs e, int columnID)
        {
            bizColumn.ConvertDateColumnToStringColumnType(columnID);
        }
        void ConvertToDateColumnType_Click1(object sender, RoutedEventArgs e, int columnID)
        {
            bizColumn.ConvertStringColumnToDateColumnType(columnID);
        }
        void DefineColumnValueForColumn(object sender, RoutedEventArgs e, ColumnDTO column)
        {
            frmColumnValueRange view = new frmColumnValueRange(column.ID);
            if (column.ColumnValueRangeID == 0)
            {
                view.ItemSaved += (sender1, e1) => View_ItemSaved(sender1, e1, column.ID);
            }
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "ColumnValueRange", Enum_WindowSize.Big);
        }
        //void RemoveColumnValueForColumn(object sender, RoutedEventArgs e, ColumnDTO column)
        //{
        //    BizColumn bizColumn = new BizColumn();
        //    bizColumn.RemoveColumnValueRangeID(column.ID);
        //}
        private void View_ItemSaved(object sender, SavedItemArg e, int columnID)
        {
            //BizColumn bizColumn = new BizColumn();
            //bizColumn.UpdateColumnValueRangeID(columnID, e.ID);
        }

        //void DefineFormulaForColumn(object sender, RoutedEventArgs e, int columnID)
        //{

        //    //بجای منو یک ستون انتخاب فرومول افزوده شود
        //    //////var column = dtgColumns.SelectedItem as ColumnDTO;
        //    //////var entity = dtgRuleEntity.SelectedItem as TableDrivedEntityDTO;
        //    //////if (entity != null && column != null)
        //    //////{
        //    //////    int entityID = entity.ID;
        //    //////    // var formulaDTO = tabColumnFormula.DataContext as FormulaDTO;
        //    //////    FormulaIntention formulaIntention = new FormulaIntention();
        //    //////    formulaIntention.ColumnID = column.ID;
        //    //////    formulaIntention.EntityID = entityID;
        //    //////    formulaIntention.Type = Enum_FormulaIntention.FormulaForColumn;
        //    //////    if (column.CustomFormula != null)
        //    //////    {
        //    //////        formulaIntention.DefaultFormulaID = column.CustomFormula.ID;
        //    //////    }
        //    //////    frmFormula view = new frmFormula(formulaIntention);
        //    //////    view.FormulaSelected += View_FormulaSelected;
        //    //////       MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form");
        //    //////}
        //}

        private void btnUpdateEntities_Click(object sender, RoutedEventArgs e)
        {
            BizTableDrivedEntity biz = new BizTableDrivedEntity();
            btnUpdateEntities.IsEnabled = false;
            biz.Save(dtgRuleEntity.ItemsSource as List<TableDrivedEntityDTO>);
            btnUpdateEntities.IsEnabled = true;
        }

        private void btnRefreshEntityRules_Click(object sender, RoutedEventArgs e)
        {
            SetImportedInfo();
        }
        private async void SetImportedInfo()
        {
            btnRefreshEntityRules.IsEnabled = false;
            try
            {
                var result = await GetEntities();
                dtgRuleEntity.ItemsSource = result;
            }
            catch
            {

            }
            finally
            {
                btnRefreshEntityRules.IsEnabled = true;
            }
        }
        private Task<List<TableDrivedEntityDTO>> GetEntities()
        {
            return Task.Run(() =>
            {
                var result = bizTableDrivedEntity.GetAllEntities(DatabaseID, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships, null);
                return result;
            });
        }

        private void btnUpdateRelationships_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUpdateRelationships_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void btnRefreshRelationship_Click(object sender, RoutedEventArgs e)
        {

        }
    }


}
