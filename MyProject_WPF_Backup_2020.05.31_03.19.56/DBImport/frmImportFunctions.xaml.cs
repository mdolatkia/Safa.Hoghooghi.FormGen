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
    /// Interaction logic for frmImportFunctions.xaml
    /// </summary>
    public partial class frmImportFunctions : UserControl, ImportWizardForm
    {
        public bool HasData()
        {
            return true;
        }
        BizDatabaseFunction bizDatabaseFunction = new BizDatabaseFunction();
        DatabaseDTO Database { set; get; }
        public frmImportFunctions(DatabaseDTO database)
        {
            InitializeComponent();
            Database = database;
            ImportHelper = ModelGenerator.GetDatabaseImportHelper(Database);
            ImportHelper.ItemImportingStarted += ImportHelper_ItemImportingStarted;
            bizDatabaseFunction.ItemImportingStarted += ImportHelper_ItemImportingStarted;
            dtgNewFunctions.RowLoaded += DtgNewFunctions_RowLoaded;
            dtgEditFunctions.RowLoaded += DtgNewFunctions_RowLoaded;
            dtgExistingFunctions.RowLoaded += DtgNewFunctions_RowLoaded;
            dtgDeletedFunctions.RowLoaded += DtgNewFunctions_RowLoaded;
            dtgExceptionFunctions.RowLoaded += DtgNewFunctions_RowLoaded;

            this.Loaded += FrmImportFunctions_Loaded;
        }

        private void FrmImportFunctions_Loaded(object sender, RoutedEventArgs e)
        {
            SetImportedInfo();
        }

        private void DtgNewFunctions_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (e.DataElement is FunctionImportItem)
            {
                var item = e.DataElement as FunctionImportItem;
                if (!string.IsNullOrEmpty(item.Tooltip))
                    ToolTipService.SetToolTip(e.Row, item.Tooltip);
            }
        }

        I_DatabaseImportHelper ImportHelper { set; get; }
        public event EventHandler<ItemImportingStartedArg> InfoUpdated;
        public event EventHandler FormIsBusy;
        public event EventHandler FormIsFree;

        List<FunctionImportItem> listNew = new List<FunctionImportItem>();
        List<FunctionImportItem> listEdited = new List<FunctionImportItem>();
        List<FunctionImportItem> listDeleted = new List<FunctionImportItem>();
        List<FunctionImportItem> listExisting = new List<FunctionImportItem>();
        List<FunctionImportItem> listException = new List<FunctionImportItem>();

        private void btnExtract_Click(object sender, RoutedEventArgs e)
        {
            SetImportedInfo();
        }
        private async void SetImportedInfo()
        {
            try
            {
                FormIsBusy(this, null);
                var result = await GetFunctionsInfo();
                listNew = new List<FunctionImportItem>();
                listEdited = new List<FunctionImportItem>();
                listDeleted = new List<FunctionImportItem>();
                listExisting = new List<FunctionImportItem>();
                listException = new List<FunctionImportItem>();
                var originalFunctions = bizDatabaseFunction.GetOrginalFunctions(Database.ID);
                foreach (var item in result.Where(x => x.Exception == false))
                {
                    if (originalFunctions.Any(x => x.FunctionName.ToLower() == item.Function.FunctionName.ToLower()))
                    {
                        var existingEntity = originalFunctions.First(x => x.FunctionName.ToLower() == item.Function.FunctionName.ToLower());
                        if (existingEntity.Enable)
                        {
                            var diff = EntityIsModified(item.Function, existingEntity);
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
                foreach (var item in result.Where(x => x.Exception == true))
                {
                    listException.Add(item);
                }
                foreach (var item in originalFunctions.Where(x => x.Enable && !result.Any(y => y.Function.FunctionName == x.FunctionName)))
                {
                    listDeleted.Add(new FunctionImportItem(item, "", true));
                }
                //var result = await GenerateDefaultEntitiesAndColumns();
                //نتیجه خط بالا که برگشت ادامه کار طی میشود
                //    ManageLogs(result, lstFunctions);

                dtgNewFunctions.ItemsSource = listNew;
                dtgEditFunctions.ItemsSource = listEdited;
                dtgExistingFunctions.ItemsSource = listExisting;
                dtgExceptionFunctions.ItemsSource = listException;
                dtgDeletedFunctions.ItemsSource = listDeleted;

                if (listException.Any())
                    tabException.Visibility = Visibility.Visible;
                else
                    tabException.Visibility = Visibility.Collapsed;

                tabNewFunctions.Foreground = listNew.Any() ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
                tabEditFunctions.Foreground = listEdited.Any() ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
                tabDeletedFunctions.Foreground = listDeleted.Any() ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
                tabException.Foreground = listException.Any() ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
                tabExistingFunctions.Foreground = listExisting.Any() ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);

                if (listNew.Any())
                    tabNewFunctions.IsSelected = true;
                else if (listEdited.Any())
                    tabEditFunctions.IsSelected = true;
                else if (listDeleted.Any())
                    tabDeletedFunctions.IsSelected = true;
                else if (listException.Any())
                    tabException.IsSelected = true;
                else if (listExisting.Any())
                    tabExistingFunctions.IsSelected = true;
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
        public Task<List<FunctionImportItem>> GetFunctionsInfo()
        {
            return Task.Run(() =>
            {
                var result = ImportHelper.GetFunctions().OrderBy(x => x.Function.RelatedSchema).ThenBy(x => x.Function.FunctionName).ToList(); ;
                return result;
            });
        }
        private string EntityIsModified(DatabaseFunctionDTO dbItem, DatabaseFunctionDTO existingEntity)
        {
            string result = "";
            if (dbItem.RelatedSchema != existingEntity.RelatedSchema)
                result += (result == "" ? "" : Environment.NewLine) + "شمای موجودیت تغییر کرده است";
            if (dbItem.ReturnType != existingEntity.ReturnType)
                result += (result == "" ? "" : Environment.NewLine) + "خروجی موجودیت تغییر کرده است";
            if (dbItem.DatabaseFunctionParameter.Any(x => !existingEntity.DatabaseFunctionParameter.Any(y => y.ParameterName == x.ParameterName)))
            {
                foreach (var column in dbItem.DatabaseFunctionParameter.Where(x => !existingEntity.DatabaseFunctionParameter.Any(y => y.ParameterName == x.ParameterName)))
                    result += (result == "" ? "" : Environment.NewLine) + "ستون" + " " + column.ParameterName + " " + "اضافه شده است";
            }
            if (existingEntity.DatabaseFunctionParameter.Where(x => x.Enable).Any(x => !dbItem.DatabaseFunctionParameter.Any(y => y.ParameterName == x.ParameterName)))
            {
                foreach (var column in existingEntity.DatabaseFunctionParameter.Where(x => !dbItem.DatabaseFunctionParameter.Any(y => y.ParameterName == x.ParameterName)))
                    result += (result == "" ? "" : Environment.NewLine) + "ستون" + " " + column.ParameterName + " " + "حذف شده است";
            }
            foreach (var column in existingEntity.DatabaseFunctionParameter.Where(x => x.Enable && dbItem.DatabaseFunctionParameter.Any(y => y.ParameterName == x.ParameterName)))
            {
                var dbColumn = dbItem.DatabaseFunctionParameter.First(y => y.ParameterName == column.ParameterName);
                if (column.DataType != dbColumn.DataType)
                    result += (result == "" ? "" : Environment.NewLine) + "نوع ستون" + " " + column.ParameterName + " " + "تغییر کرده است";
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
                var newEntities = listNew.Where(x => x.Selected).Select(x => x.Function).ToList();
                var editedEntities = listEdited.Where(x => x.Selected).Select(x => x.Function).ToList();
                var deletedEntities = listDeleted.Where(x => x.Selected).Select(x => x.Function).ToList();
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
        private Task UpdateModel(int databaseID, List<DatabaseFunctionDTO> listNew, List<DatabaseFunctionDTO> listEdit, List<DatabaseFunctionDTO> listDeleted)
        {
            return Task.Run(() =>
            {
                bizDatabaseFunction.UpdateModel(Database.ID, listNew, listEdit, listDeleted);
            });
        }

        //private Task<List<DatabaseFunctionDTO>> GenerateDefaultEntitiesAndColumns()
        //{
        //    return Task.Run(() =>
        //    {
        //        var result = ImportHelper.GenerateDefaultEntitiesAndColumns();
        //        return result;
        //    });
        //}
    }


}
