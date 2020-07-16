using ModelEntites;
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

namespace MyProject_WPF
{
    //نوع رابطه نمایش داده شود در روابط بدون تغییر
    //    آدرس ورود اطلاعات
    /// <summary>
    /// Interaction logic for frmImportTables.xaml
    /// </summary>
    public partial class frmImportTables : UserControl, ImportWizardForm
    {
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        DatabaseDTO Database { set; get; }
        BizDatabase bizDatabase = new BizDatabase();
        public frmImportTables(DatabaseDTO database)
        {
            InitializeComponent();
            Database = database;
            ImportHelper = ModelGenerator.GetDatabaseImportHelper(Database);
            ImportHelper.ItemImportingStarted += ImportHelper_ItemImportingStarted;
            bizTableDrivedEntity.ItemImportingStarted += ImportHelper_ItemImportingStarted;
            dtgNewTables.RowLoaded += DtgNewTables_RowLoaded;
            dtgEditTables.RowLoaded += Dtg_RowLoaded;
            dtgExistingTables.RowLoaded += Dtg_RowLoaded;
            dtgDeletedTables.RowLoaded += Dtg_RowLoaded;
            dtgExceptionTables.RowLoaded += Dtg_RowLoaded;
            this.Loaded += FrmImportTables_Loaded;
            // colDataCountLimit.Header = "تعداد داده کمتر از" + " " + DataCountLimit + " " + "مورد است";
        }

        private void FrmImportTables_Loaded(object sender, RoutedEventArgs e)
        {
            SetImportedInfo();
        }

        private void DtgNewTables_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (e.DataElement is TableImportItem)
            {
                var item = e.DataElement as TableImportItem;
                if (!string.IsNullOrEmpty(item.Tooltip))
                {
                    ToolTipService.SetToolTip(e.Row, ControlHelper.GetTooltip(item.Tooltip, false));
                }
            }
        }
        private void Dtg_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (e.DataElement is TableImportItem)
            {
                var item = e.DataElement as TableImportItem;
                if (!string.IsNullOrEmpty(item.Tooltip))
                {
                    ToolTipService.SetToolTip(e.Row, ControlHelper.GetTooltip(item.Tooltip, true));
                }
            }
        }
        I_DatabaseImportHelper ImportHelper { set; get; }
        //public event EventHandler<ItemImportingStartedArg> InfoUpdated;
        List<TableImportItem> listNew = new List<TableImportItem>();
        List<TableImportItem> listEdited = new List<TableImportItem>();
        List<TableImportItem> listDeleted = new List<TableImportItem>();
        List<TableImportItem> listExisting = new List<TableImportItem>();
        List<TableImportItem> listException = new List<TableImportItem>();
        public event EventHandler NewEntitiesAdded;
        public event EventHandler<ItemImportingStartedArg> InfoUpdated;
        public event EventHandler FormIsBusy;
        public event EventHandler FormIsFree;

        private void btnExtract_Click(object sender, RoutedEventArgs e)
        {
            SetImportedInfo();
        }
        private async void SetImportedInfo()
        {
            //try
            //{
            FormIsBusy(this, null);
            //به این که میرسه برمیگرده به تردی که این متود اسینک رو صدا زده که اینجا ترد یو آی میباشد
            // بنابراین کارهای یو آی را که تا اینجا غیر فعال کردن فرم می باشد انجام میدهد
            // زمانی که متود زیر کامل شد بر میگرده به ادامه این کد
            var result = await GetTablesAndColumnInfo();

            listNew = new List<TableImportItem>();
            listEdited = new List<TableImportItem>();
            listDeleted = new List<TableImportItem>();
            listExisting = new List<TableImportItem>();
            listException = new List<TableImportItem>();
            var originalEntities = bizTableDrivedEntity.GetOrginalEntities(Database.ID, EntityColumnInfoType.WithFullColumns, EntityRelationshipInfoType.WithoutRelationships, false);
            foreach (var item in result.Where(x => x.Exception == false))
            {
                if (originalEntities.Any(x => x.Name.ToLower() == item.Entity.Name.ToLower()))
                {
                    //باید اصلاح شود که موجودیت ارث بری داخلی همه ستونهای جدول را بگیرد و نه اختصاصی شده ها را
                    var existingEntity = originalEntities.First(x => x.Name.ToLower() == item.Entity.Name.ToLower());
                    if (!existingEntity.IsDisabled)
                    {
                        var diff = EntityIsModified(item, existingEntity);
                        if (!string.IsNullOrEmpty(diff))
                        {
                            item.Entity.Alias = existingEntity.Alias;
                            item.Entity.Description = existingEntity.Description;
                            item.Selected = true;
                            item.Tooltip = diff;
                            listEdited.Add(item);
                        }
                        else
                        {
                            item.Entity = existingEntity;
                            listExisting.Add(item);
                        }
                    }
                }
                else
                {
                    item.Selected = true;
                    listNew.Add(item);
                }
            }
            if (listNew.Any())
            {
                var columnTags = WizardHelper.GetColumnsAliasAndDescriptions(listNew.SelectMany(x => x.Entity.Columns).ToList());
                var tags = WizardHelper.GetTablesAliasAndDescriptions(listNew.Select(x => x.Entity).ToList());
                foreach (var item in listNew)
                {
                    if (!string.IsNullOrEmpty(tags.Item1) || !string.IsNullOrEmpty(tags.Item2))
                        WizardHelper.SetEntityAliasAndDescription(item, tags.Item1, tags.Item2);
                    foreach (var column in item.Entity.Columns)
                    {
                        WizardHelper.SetColumnAliasAnadDescription(column, columnTags.Item1, columnTags.Item2);
                        CheckNewColumnPersianDate(column);
                    }
                    item.Tooltip = GetNewItemTooltip(item);
                }
            }
            if (listEdited.Any(x => x.Entity.Columns.Any(y => y.ColumnsAdded)))
            {
                var tags = WizardHelper.GetColumnsAliasAndDescriptions(listEdited.SelectMany(x => x.Entity.Columns).Where(y => y.ColumnsAdded).ToList());
                if (!string.IsNullOrEmpty(tags.Item1) || !string.IsNullOrEmpty(tags.Item2))
                {
                    foreach (var item in listEdited.Where(x => x.Entity.Columns.Any(y => y.ColumnsAdded)))
                    {
                        foreach (var column in item.Entity.Columns.Where(x => x.ColumnsAdded))
                        {
                            WizardHelper.SetColumnAliasAnadDescription(column, tags.Item1, tags.Item2);
                            CheckNewColumnPersianDate(column);
                        }
                    }
                }
            }
            foreach (var item in result.Where(x => x.Exception == true))
            {
                listException.Add(item);
            }
            //var existingEntities = GetOrginalEntities;
            foreach (var item in originalEntities.Where(x => !x.IsDisabled && !result.Any(y => y.Entity.Name == x.Name)))
            {
                listDeleted.Add(new TableImportItem(item, "", true));
            }

            //var result = await GenerateDefaultEntitiesAndColumns();
            //نتیجه خط بالا که برگشت ادامه کار طی میشود
            //    ManageLogs(result, lstTables);

            //if (listNew.Count > 0)
            //    SetNewEntitiesProperties(listNew);
            dtgNewTables.ItemsSource = listNew;
            dtgEditTables.ItemsSource = listEdited;
            dtgDeletedTables.ItemsSource = listDeleted;
            dtgExceptionTables.ItemsSource = listException;
            dtgExistingTables.ItemsSource = listExisting;

            if (listException.Any())
                tabExceptionTables.Visibility = Visibility.Visible;
            else
                tabExceptionTables.Visibility = Visibility.Collapsed;

            tabNewTables.Foreground = listNew.Any() ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
            tabEditTables.Foreground = listEdited.Any() ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
            tabDeletedTables.Foreground = listDeleted.Any() ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
            tabExceptionTables.Foreground = listException.Any() ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
            tabExistingTables.Foreground = listExisting.Any() ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);

            if (listNew.Any())
                tabNewTables.IsSelected = true;
            else if (listEdited.Any())
                tabEditTables.IsSelected = true;
            else if (listDeleted.Any())
                tabDeletedTables.IsSelected = true;
            else if (listException.Any())
                tabExceptionTables.IsSelected = true;
            else if (listExisting.Any())
                tabExistingTables.IsSelected = true;

            btnInsert.IsEnabled = listNew.Any() || listEdited.Any() || listDeleted.Any();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("خطا در پردازش اطلاعات" + Environment.NewLine + ex.Message);
            //}
            //finally
            //{
            FormIsFree(this, null);
            //}

        }

        private void CheckNewColumnPersianDate(ColumnDTO column)
        {
            if (column.OriginalColumnType == Enum_ColumnType.String)
            {
                if (column.StringColumnType.MaxLength <= 50 &&
                    (column.Name.ToLower().StartsWith("datetime") ||
                    column.Name.ToLower().EndsWith("datetime"))
                      )
                {
                    column.ColumnType = Enum_ColumnType.DateTime;
                    column.DateTimeColumnType = new DateTimeColumnTypeDTO();
                }
                else if (column.StringColumnType.MaxLength <= 50 &&
                    (column.Name.ToLower().StartsWith("date") ||
                    column.Name.ToLower().EndsWith("date") ||
                     column.Name.ToLower().StartsWith("tarikh") ||
                      column.Name.ToLower().EndsWith("tarikh"))
                      )
                {
                    column.ColumnType = Enum_ColumnType.Date;
                    column.DateColumnType = new DateColumnTypeDTO();

                }
                else if (column.StringColumnType.MaxLength <= 20 &&
                   (column.Name.ToLower().StartsWith("time") ||
                   column.Name.ToLower().EndsWith("time") ||
                   column.Name.ToLower().StartsWith("time") ||
                   column.Name.ToLower().EndsWith("time") ||
                    column.Name.ToLower().StartsWith("zaman") ||
                     column.Name.ToLower().EndsWith("zaman"))
                     )
                {
                    column.ColumnType = Enum_ColumnType.Time;
                    column.TimeColumnType = new TimeColumnTypeDTO();
                }
            }
            if (column.ColumnType == Enum_ColumnType.DateTime)
            {
                var database = bizDatabase.GetDatabase(Database.ID, true);
                column.DateTimeColumnType.ShowMiladiDateInUI = database.DatabaseSetting != null ? database.DatabaseSetting.ShowMiladiDateInUI : false;
            }
            else if (column.ColumnType == Enum_ColumnType.Date)
            {
                var database = bizDatabase.GetDatabase(Database.ID, true);
                column.DateColumnType.ShowMiladiDateInUI = database.DatabaseSetting != null ? database.DatabaseSetting.ShowMiladiDateInUI : false;
                if (column.OriginalColumnType == Enum_ColumnType.String)
                    column.DateColumnType.StringDateIsMiladi = database.DatabaseSetting != null ? database.DatabaseSetting.StringDateColumnIsMiladi : false;
            }
            else if (column.ColumnType == Enum_ColumnType.Time)
            {
                var database = bizDatabase.GetDatabase(Database.ID, true);
                column.TimeColumnType.ShowMiladiTime = database.DatabaseSetting != null ? database.DatabaseSetting.ShowMiladiDateInUI : false;
                if (column.OriginalColumnType == Enum_ColumnType.String)
                {
                    column.TimeColumnType.StringTimeIsMiladi = database.DatabaseSetting != null ? database.DatabaseSetting.StringDateColumnIsMiladi : false;
                }
            }
        }

        private string GetNewItemTooltip(TableImportItem item)
        {
            string tooltip = item.Entity.Name + ((!string.IsNullOrEmpty(item.Entity.Alias) && item.Entity.Name != item.Entity.Alias) ? " As " + "'" + item.Entity.Alias + "'" : "");
            tooltip += Environment.NewLine + "Columns : ";
            foreach (var column in item.Entity.Columns)
            {
                tooltip += Environment.NewLine + column.Name + ((!string.IsNullOrEmpty(column.Alias) && column.Name != column.Alias) ? " As " + "'" + column.Alias + "'" : "")
                    + "  " + column.DataType;
            }
            return tooltip;
        }




        //private void SetNewEntitiesProperties(List<TableImportItem> listNew)
        //{
        //   foreach(var item in listNew)
        //    {
        //        item.Entity.PropertyChanged += Entity_PropertyChanged;
        //        DetermineEntityIsDataRef(item);
        //    }
        //}

        //private void DetermineEntityIsDataRef(TableImportItem item)
        //{

        //}

        //private void Entity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //   // throw new NotImplementedException();
        //}

        public Task<List<TableImportItem>> GetTablesAndColumnInfo()
        {
            return Task.Run(() =>
            {
                var result = ImportHelper.GetTablesAndColumnInfo().OrderBy(x => x.Entity.RelatedSchema).ThenBy(x => x.Entity.Name).ToList();
                return result;
            });
        }

        private string EntityIsModified(TableImportItem dbItem, TableDrivedEntityDTO existingEntity)
        {
            string result = "";
            if (dbItem.Entity.RelatedSchema != existingEntity.RelatedSchema)
                result += (result == "" ? "" : Environment.NewLine) + "شمای موجودیت تغییر کرده است";
            //if (!string.IsNullOrEmpty(dbItem.Alias))
            //    if (string.IsNullOrEmpty(existingEntity.Alias) || existingEntity.Alias == existingEntity.Name)
            //        if (dbItem.Alias != existingEntity.Alias)
            //if (existingEntity.Reviewed == false)
            //{
            //    //if (!string.IsNullOrEmpty(dbItem.Alias))
            //    if (dbItem.Alias != existingEntity.Alias)
            //        result += (result == "" ? "" : Environment.NewLine) + "عنوان معادل موجودیت تغییر کرده است";

            //    //if (!string.IsNullOrEmpty(dbItem.Description))
            //    if (dbItem.Description != existingEntity.Description)
            //        result += (result == "" ? "" : Environment.NewLine) + "توضیحات موجودیت تغییر کرده است";
            //}
            if (dbItem.Entity.Columns.Any(x => !existingEntity.Columns.Any(y => y.Name.Replace("ي", "ی") == x.Name.Replace("ي", "ی"))))
            {
                foreach (var column in dbItem.Entity.Columns.Where(x => !existingEntity.Columns.Any(y => y.Name.Replace("ي", "ی") == x.Name.Replace("ي", "ی"))))
                {
                    column.ColumnsAdded = true;
                    result += (result == "" ? "" : Environment.NewLine) + "ستون" + " " + column.Name + " " + "اضافه شده است";
                }
            }
            if (existingEntity.Columns.Where(x => !x.IsDisabled).Any(x => !dbItem.Entity.Columns.Any(y => y.Name.Replace("ي", "ی") == x.Name.Replace("ي", "ی"))))
            {
                foreach (var column in existingEntity.Columns.Where(x => !dbItem.Entity.Columns.Any(y => y.Name.Replace("ي", "ی") == x.Name.Replace("ي", "ی"))))
                    result += (result == "" ? "" : Environment.NewLine) + "ستون" + " " + column.Name + " " + "حذف شده است";
            }

            foreach (var column in existingEntity.Columns.Where(x => !x.IsDisabled && dbItem.Entity.Columns.Any(y => y.Name == x.Name)))
            {
                var dbColumn = dbItem.Entity.Columns.First(y => y.Name == column.Name);
                if (column.DataType != dbColumn.DataType)
                    result += (result == "" ? "" : Environment.NewLine) + "نوع ستون" + " " + column.Name + " " + "تغییر کرده است";
                if (column.IsIdentity != dbColumn.IsIdentity)
                    result += (result == "" ? "" : Environment.NewLine) + "شمارش ستون" + " " + column.Name + " " + "تغییر کرده است";
                if (column.PrimaryKey != dbColumn.PrimaryKey)
                    result += (result == "" ? "" : Environment.NewLine) + "کلید ستون" + " " + column.Name + " " + "تغییر کرده است";
                //if (column.Position != dbColumn.Position)
                //    result += (result == "" ? "" : Environment.NewLine) + "موقعیت ستون" + " " + column.Name + " " + "تغییر کرده است";
                if (column.IsDBCalculatedColumn != dbColumn.IsDBCalculatedColumn)
                    result += (result == "" ? "" : Environment.NewLine) + "محاسباتی بودن ستون" + " " + column.Name + " " + "تغییر کرده است";
                if (column.DBFormula != dbColumn.DBFormula)
                    result += (result == "" ? "" : Environment.NewLine) + "فرمول محاسباتی ستون" + " " + column.Name + " " + "تغییر کرده است";
                if (column.IsNull != dbColumn.IsNull)
                    result += (result == "" ? "" : Environment.NewLine) + "Null" + " " + "پذیر بودن ستون" + " " + column.Name + " " + "تغییر کرده است";
                if (column.DefaultValue != dbColumn.DefaultValue)
                    result += (result == "" ? "" : Environment.NewLine) + "مقدار پیش فرض ستون" + " " + column.Name + " " + "تغییر کرده است";

                if ((column.NumericColumnType != null && dbColumn.NumericColumnType != null) &&
                    ((dbColumn.NumericColumnType.Precision != 0 && column.NumericColumnType.Precision != dbColumn.NumericColumnType.Precision)
                       || (dbColumn.NumericColumnType.Scale != 0 && column.NumericColumnType.Scale != dbColumn.NumericColumnType.Scale)))
                    result += (result == "" ? "" : Environment.NewLine) + "خصوصیات عددی ستون" + " " + column.Name + " " + "تغییر کرده است";

                if ((column.StringColumnType != null && dbColumn.StringColumnType != null) &&
                   (dbColumn.StringColumnType.MaxLength != 0 && column.StringColumnType.MaxLength > dbColumn.StringColumnType.MaxLength))
                    result += (result == "" ? "" : Environment.NewLine) + "خصوصیات رشته ای ستون" + " " + column.Name + " " + "تغییر کرده است";

                //if (existingEntity.ColumnsReviewed == false)
                //{
                //    //if (!string.IsNullOrEmpty(dbColumn.Alias))
                //    if (dbColumn.Alias != column.Alias)
                //        result += (result == "" ? "" : Environment.NewLine) + "عنوان معادل ستون" + " " + column.Name + " " + "تغییر کرده است";
                //    //if (!string.IsNullOrEmpty(dbColumn.Description))
                //    if (dbColumn.Description != column.Description)
                //        result += (result == "" ? "" : Environment.NewLine) + "توضیحات ستون" + " " + column.Name + " " + "تغییر کرده است";
                //}
            }
            return result;
        }

        private void ImportHelper_ItemImportingStarted(object sender, ItemImportingStartedArg e)
        {

            if (InfoUpdated != null)
                InfoUpdated(this, e);


        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            UpdateModel();
        }

        async private void UpdateModel()
        {
            bool updated = false;
            try
            {

                FormIsBusy(this, null);
                var newEntities = listNew.Where(x => x.Selected).Select(x => x.Entity).ToList();
                var editedEntities = listEdited.Where(x => x.Selected).Select(x => x.Entity).ToList();
                var deletedEntities = listDeleted.Where(x => x.Selected).Select(x => x.Entity).ToList();
                if (newEntities.Any() || editedEntities.Any() || deletedEntities.Any())
                {
                    await UpdateModel(Database.ID, newEntities, editedEntities, deletedEntities);
                    updated = true;
                    MessageBox.Show("انتقال اطلاعات انجام شد");
                    if (InfoUpdated != null)
                        InfoUpdated(this, new ItemImportingStartedArg() { ItemName = "Model is updated." });
                    if (newEntities.Any())
                    {
                        if (NewEntitiesAdded != null)
                            NewEntitiesAdded(this, null);
                    }
                }
                else
                {
                    MessageBox.Show("موردی جهت انتقال انتخاب نشده است");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("انتقال اطلاعات انجام نشد" + Environment.NewLine + ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                FormIsFree(this, null);
                if (updated)
                    SetImportedInfo();
            }
        }

        private Task UpdateModel(int databaseID, List<TableDrivedEntityDTO> listNew, List<TableDrivedEntityDTO> listEdit, List<TableDrivedEntityDTO> listDeleted)
        {
            return Task.Run(() =>
            {
                bizTableDrivedEntity.UpdateModel(databaseID, listNew, listEdit, listDeleted);
            });

        }


        public bool HasData()
        {
            return true;
        }


        //private void btnDatabaseSetting_Click(object sender, RoutedEventArgs e)
        //{
        //    frmDatabaseSetting frm = new MyProject_WPF.frmDatabaseSetting(Database.ID);
        //    MyProjectManager.GetMyProjectManager.ShowDialog(frm, "تنظیمات پایگاه داده", Enum_WindowSize.None);
        //}



        //private Task<List<TableDrivedEntityDTO>> GenerateDefaultEntitiesAndColumns()
        //{
        //    return Task.Run(() =>
        //    {
        //        var result = ImportHelper.GenerateDefaultEntitiesAndColumns();
        //        return result;
        //    });
        //}
    }


}
