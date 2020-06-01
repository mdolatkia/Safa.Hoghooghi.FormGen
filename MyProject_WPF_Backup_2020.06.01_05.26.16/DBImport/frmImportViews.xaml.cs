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
    /// <summary>
    /// Interaction logic for frmImportViews.xaml
    /// </summary>
    public partial class frmImportViews : UserControl, ImportWizardForm
    {
        public bool HasData()
        {
            return true;
        }
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        DatabaseDTO Database { set; get; }
        public frmImportViews(DatabaseDTO database)
        {
            InitializeComponent();
            Database = database;
            ImportHelper = ModelGenerator.GetDatabaseImportHelper(Database);
            ImportHelper.ItemImportingStarted += ImportHelper_ItemImportingStarted;
            bizTableDrivedEntity.ItemImportingStarted += ImportHelper_ItemImportingStarted;
            dtgNewViews.RowLoaded += DtgNewViews_RowLoaded;
            dtgEditViews.RowLoaded += DtgNewViews_RowLoaded;
            dtgExistingViews.RowLoaded += DtgNewViews_RowLoaded;
            dtgDeletedViews.RowLoaded += DtgNewViews_RowLoaded;
            dtgExceptionViews.RowLoaded += DtgNewViews_RowLoaded;

            this.Loaded += FrmImportViews_Loaded;
        }

        private void FrmImportViews_Loaded(object sender, RoutedEventArgs e)
        {
            SetImportedInfo();
        }

        private void DtgNewViews_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (e.DataElement is TableImportItem)
            {
                var item = e.DataElement as TableImportItem;
                if (!string.IsNullOrEmpty(item.Tooltip))
                    ToolTipService.SetToolTip(e.Row, item.Tooltip);
            }
        }

        I_DatabaseImportHelper ImportHelper { set; get; }
        public event EventHandler<ItemImportingStartedArg> InfoUpdated;
        public event EventHandler FormIsBusy;
        public event EventHandler FormIsFree;

        List<TableImportItem> listNew = new List<TableImportItem>();
        List<TableImportItem> listEdited = new List<TableImportItem>();
        List<TableImportItem> listDeleted = new List<TableImportItem>();
        List<TableImportItem> listExisting = new List<TableImportItem>();
        List<TableImportItem> listException = new List<TableImportItem>();

        private void btnExtract_Click(object sender, RoutedEventArgs e)
        {
            SetImportedInfo();
        }
        private async void SetImportedInfo()
        {
            try
            {
                FormIsBusy(this, null);
                var result = await GetViewsInfo();
                result.ForEach(x => x.Entity.IsView = true);
                listNew = new List<TableImportItem>();
                listEdited = new List<TableImportItem>();
                listDeleted = new List<TableImportItem>();
                listExisting = new List<TableImportItem>();
                listException = new List<TableImportItem>();
                var originalViews = bizTableDrivedEntity.GetOrginalEntities(Database.ID,EntityColumnInfoType.WithSimpleColumns,EntityRelationshipInfoType.WithoutRelationships,true);
                foreach (var item in result.Where(x => x.Exception == false))
                {
                    if (originalViews.Any(x => x.Name.ToLower() == item.Entity.Name.ToLower()))
                    {
                        //باید اصلاح شود که موجودیت ارث بری داخلی همه ستونهای جدول را بگیرد و نه اختصاصی شده ها را
                        var existingView = originalViews.First(x => x.Name.ToLower() == item.Entity.Name.ToLower());
                        if (!existingView.IsDisabled)
                        {
                            var diff = ViewIsModified(item.Entity, existingView);
                            if (!string.IsNullOrEmpty(diff))
                            {
                                item.Selected = true;
                                item.Tooltip = diff;
                                listEdited.Add(item);
                            }
                            else
                                listExisting.Add(item);
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
                            WizardHelper.SetColumnAliasAnadDescription(column, columnTags.Item1, columnTags.Item2);
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
                                WizardHelper.SetColumnAliasAnadDescription(column, tags.Item1, tags.Item2);
                        }
                    }
                }

                foreach (var item in result.Where(x => x.Exception == true))
                {
                    listException.Add(item);
                }
                //var existingViews = bizTableDrivedEntity.GetEnabledOrginalViews(Database.ID);
                foreach (var item in originalViews.Where(x =>! x.IsDisabled && !result.Any(y => y.Entity.Name == x.Name)))
                {
                    listDeleted.Add(new TableImportItem(item, "", true));
                }
                //var result = await GenerateDefaultViewsAndColumns();
                //نتیجه خط بالا که برگشت ادامه کار طی میشود
                //    ManageLogs(result, lstViews);

                dtgNewViews.ItemsSource = listNew;
                dtgEditViews.ItemsSource = listEdited;
                dtgExistingViews.ItemsSource = listExisting;
                dtgExceptionViews.ItemsSource = listException;
                dtgDeletedViews.ItemsSource = listDeleted;

                if (listException.Any())
                    tabExceptionViews.Visibility = Visibility.Visible;
                else
                    tabExceptionViews.Visibility = Visibility.Collapsed;

                tabNewViews.Foreground = listNew.Any() ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
                tabEditViews.Foreground = listEdited.Any() ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
                tabDeletedViews.Foreground = listDeleted.Any() ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
                tabExceptionViews.Foreground = listException.Any() ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
                tabExistingViews.Foreground = listExisting.Any() ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);

                if (listNew.Any())
                    tabNewViews.IsSelected = true;
                else if (listEdited.Any())
                    tabEditViews.IsSelected = true;
                else if (listDeleted.Any())
                    tabDeletedViews.IsSelected = true;
                else if (listException.Any())
                    tabExceptionViews.IsSelected = true;
                else if (listExisting.Any())
                    tabExistingViews.IsSelected = true;

                btnInsert.IsEnabled = listNew.Any() || listEdited.Any() || listDeleted.Any();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در پردازش اطلاعات" + Environment.NewLine + ex.Message);
            }
            finally
            {
                FormIsFree(this, null);
            }
        }
        private string GetNewItemTooltip(TableImportItem item)
        {
            string tooltip = item.Entity.Name + ((!string.IsNullOrEmpty(item.Entity.Alias) && item.Entity.Name != item.Entity.Alias) ? " As " + "'" + item.Entity.Alias + "'" : "");
            tooltip += Environment.NewLine + "Columns : ";
            foreach (var column in item.Entity.Columns)
            {
                tooltip += Environment.NewLine + column.Name + ((!string.IsNullOrEmpty(column.Alias) && column.Name != column.Alias) ? " As " + "'" + column.Alias + "'" : "");
            }
            return tooltip;
        }
        public Task<List<TableImportItem>> GetViewsInfo()
        {
            return Task.Run(() =>
            {
                var result = ImportHelper.GetViewsInfo().OrderBy(x => x.Entity.RelatedSchema).ThenBy(x => x.Entity.Name).ToList();
                return result;
            });
        }

        private string ViewIsModified(TableDrivedEntityDTO dbItem, TableDrivedEntityDTO existingEntity)
        {
            string result = "";
            if (dbItem.RelatedSchema != existingEntity.RelatedSchema)
                result += (result == "" ? "" : Environment.NewLine) + "شمای موجودیت تغییر کرده است";
            if (string.IsNullOrEmpty(existingEntity.Alias))
                if (dbItem.Alias != existingEntity.Alias)
                    result += (result == "" ? "" : Environment.NewLine) + "توضیحات موجودیت تغییر کرده است";
            if (dbItem.Columns.Any(x => !existingEntity.Columns.Any(y => y.Name.Replace("ي", "ی") == x.Name.Replace("ي", "ی"))))
            {
                foreach (var column in dbItem.Columns.Where(x => !existingEntity.Columns.Any(y => y.Name.Replace("ي", "ی") == x.Name.Replace("ي", "ی"))))
                    result += (result == "" ? "" : Environment.NewLine) + "ستون" + " " + column.Name + " " + "اضافه شده است";
            }
            if (existingEntity.Columns.Where(x => !x.IsDisabled).Any(x => !dbItem.Columns.Any(y => y.Name.Replace("ي", "ی") == x.Name.Replace("ي", "ی"))))
            {
                foreach (var column in existingEntity.Columns.Where(x => !dbItem.Columns.Any(y => y.Name.Replace("ي", "ی") == x.Name.Replace("ي", "ی"))))
                    result += (result == "" ? "" : Environment.NewLine) + "ستون" + " " + column.Name + " " + "حذف شده است";
            }

            foreach (var column in existingEntity.Columns.Where(x => !x.IsDisabled && dbItem.Columns.Any(y => y.Name == x.Name)))
            {
                var dbColumn = dbItem.Columns.First(y => y.Name == column.Name);
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


            }
            return result;
        }

        private void ImportHelper_ItemImportingStarted(object sender, ItemImportingStartedArg e)
        {
            //چون از ترد دیگر می آید اجازه دسترسی به لیبل را ندارد.بنابراین باید یک دلیگت جدید در صف دیسپچر خود
            //لیبل که همان دیسپچر اصلی یو آی ترد است قرار گیرد
            if (InfoUpdated != null)
                InfoUpdated(this, e);
            //lblJobDetail.Dispatcher.Invoke(
            //          new UpdateDetailInfoDelegate(this.UpdateDetailInfo),
            //          new object[] { e.ItemName, e.CurrentProgress, e.TotalProgressCount }
            //      );
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
        //private Task<List<ViewDTO>> GenerateDefaultViewsAndColumns()
        //{
        //    return Task.Run(() =>
        //    {
        //        var result = ImportHelper.GenerateDefaultViewsAndColumns();
        //        return result;
        //    });
        //}
    }


}
