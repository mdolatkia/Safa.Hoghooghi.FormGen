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
    public partial class frmEntitySettings : UserControl, ImportWizardForm
    {
        BizEntitySettings bizEntitySettings = new BizEntitySettings();
        //BizEntityListView bizEntityListView = new BizEntityListView();
        //BizEntitySearch bizEntitySearch = new BizEntitySearch();
        //BizEntityUIComposition bizEntityUIComposition = new BizEntityUIComposition();
        BizDatabaseFunction bizDatabaseFunction = new BizDatabaseFunction();

        public event EventHandler<ItemImportingStartedArg> InfoUpdated;
        public event EventHandler FormIsBusy;
        public event EventHandler FormIsFree;

        DatabaseDTO Database { set; get; }
        public frmEntitySettings(DatabaseDTO database)
        {
            InitializeComponent();
            Database = database;
            bizEntitySettings.ItemImportingStarted += BizEntityUIComposition_ItemImportingStarted;

            //bizEntityUIComposition.ItemImportingStarted += BizEntityUIComposition_ItemImportingStarted;
            //bizEntityListView.ItemImportingStarted += BizEntityUIComposition_ItemImportingStarted;
            //bizEntitySearch.ItemImportingStarted += BizEntityUIComposition_ItemImportingStarted;

            //    this.Loaded += FrmTableFinalSettings_Loaded;
            //var frmTableUIComposition = new frmTableUIComposition(database);
            //grdMain.Children.Add(frmTableUIComposition);
            //Grid.SetColumn(frmTableUIComposition, 1);

            //var frmViews = new frmTableDefaultListView(database);
            //grdMain.Children.Add(frmViews);

            //frmTableUIComposition.InfoUpdated += FrmViews_InfoUpdated;
            //frmViews.InfoUpdated += FrmViews_InfoUpdated;
            this.Loaded += FrmEntitySettings_Loaded;
        }

        private void FrmEntitySettings_Loaded(object sender, RoutedEventArgs e)
        {
            CheckFromState();
        }



        private void BizEntityUIComposition_ItemImportingStarted(object sender, ItemImportingStartedArg e)
        {
            if (InfoUpdated != null)
                InfoUpdated(this, e);
        }

        private async void GenerateDefaultSettings()
        {
            try
            {
                FormIsBusy(this, null);
                btnUpdateSettings.IsEnabled = false;
                chkUiComposition.IsChecked = false;
                chkUiComposition.Foreground = new SolidColorBrush(Colors.Black);
                chkListView.IsChecked = false;
                chkListView.Foreground = new SolidColorBrush(Colors.Black);
                chkListSearch.IsChecked = false;
                chkListSearch.Foreground = new SolidColorBrush(Colors.Black);
                chkInitialSearch.IsChecked = false;
                chkInitialSearch.Foreground = new SolidColorBrush(Colors.Black);
                await UpdateDefaultSettingsInModel();
                MessageBox.Show("تنظیمات اولیه اطلاعات انجام شد");
                chkUiComposition.IsChecked = true;
                chkUiComposition.Foreground = new SolidColorBrush(Colors.Green);
                chkListView.IsChecked = true;
                chkListView.Foreground = new SolidColorBrush(Colors.Green);
                chkListSearch.IsChecked = true;
                chkListSearch.Foreground = new SolidColorBrush(Colors.Green);
                chkInitialSearch.IsChecked = true;
                chkInitialSearch.Foreground = new SolidColorBrush(Colors.Green);
         
            }
            catch (Exception ex)
            {
                MessageBox.Show("انتقال اطلاعات انجام نشد" + Environment.NewLine + ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                FormIsBusy(this, null);
                CheckFromState();
            }
        }

        public Task UpdateDefaultSettingsInModel()
        {
            return Task.Run(() =>
            {
                bizEntitySettings.UpdateDefaultSettingsInModel(MyProjectManager.GetMyProjectManager.GetRequester(), Database.ID);
            });
        }


        //private void btnInsertDefaultSearch_Click(object sender, RoutedEventArgs e)
        //{
        //    GenerateDefaultEntitySearch();
        //}

        private void btnUpdateSettings_Click(object sender, RoutedEventArgs e)
        {
            GenerateDefaultSettings();
        }

        private void CheckFromState()
        {
            FormIsBusy(this, null);
            if (bizEntitySettings.EntityWithoutSetting(Database.ID))
            {
                FormIsFree(this, null);
                lblMessage.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblMessage.Text = "موجودیتی برای تعیین وضعیت جدول پایه موجود نمی باشد";
                lblMessage.Visibility = Visibility.Visible;
            }
        }

        //public bool HasData()
        //{

        //}
    }
}
