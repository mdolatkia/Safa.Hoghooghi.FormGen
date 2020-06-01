using ModelEntites;
using MyModelGenerator;
using MyModelManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for DatabaseImport.xaml
    /// </summary>
    public partial class DatabaseImport : UserControl
    {
        //System.Threading.Timer infoTimer = new System.Threading.Timer(
        DatabaseDTO Database { set; get; }
        I_DatabaseImportHelper ImportHelper { set; get; }
        BizDatabase bizDatabase = new BizDatabase();
        public DatabaseImport(int databaseID)
        {
            InitializeComponent();
            Database = bizDatabase.GetDatabase(databaseID);
            ImportHelper = ModelGenerator.GetDatabaseImportHelper(Database);
            ImportHelper.ItemImportingStarted += ImportHelper_ItemImportingStarted;
            lblDatabase.Text = Database.Title;
            var server = bizDatabase.GetDBServer(Database.DBServerID);
            lblServer.Text = server.Title;


            //infoTimer.Interval =  100;
            //infoTimer.Elapsed += infoTimer_Tick;
        }
        //string JobTitle { set; get; }
        //string JobDetail { set; get; }
        //string LastJobImagePath { set; get; }
        //string CurrentJobImagePath { set; get; }

        //int timerCounter = 0;
        //void infoTimer_Tick(object sender, EventArgs e)
        //{
        //    timerCounter++;
        //    if (timerCounter >= 50000)
        //    {
        //        infoTimer.Stop();
        //    }
        //    //progressBar1.Maximum = ProgressBarMax;
        //    //progressBar1.Value = ProgressBarValue;
        //    //lblInfo.Text = Info;
        //    lblJobTitle.Text = JobTitle;
        //    lblJobDetail.Text = JobDetail;
        //    if (LastJobImagePath != CurrentJobImagePath)
        //    {
        //        LastJobImagePath = CurrentJobImagePath;
        //        Uri uriSource = new Uri(CurrentJobImagePath, UriKind.Relative);
        //        if (uriSource != null)
        //            imgJob.Source = new BitmapImage(uriSource);
        //    }
        //}
        public delegate void UpdateDetailInfoDelegate(string message, int currentProgress, int totalProgress);
        private void ImportHelper_ItemImportingStarted(object sender, ItemImportingStartedArg e)
        {
            //چون از ترد دیگر می آید اجازه دسترسی به لیبل را ندارد.بنابراین باید یک دلیگت جدید در صف دیسپچر خود
            //لیبل که همان دیسپچر اصلی یو آی ترد است قرار گیرد
            lblJobDetail.Dispatcher.Invoke(
                      new UpdateDetailInfoDelegate(this.UpdateDetailInfo),
                      new object[] { e.ItemName, e.CurrentProgress, e.TotalProgressCount }
                  );
        }
        private void UpdateDetailInfo(string message, int currentProgress, int totalProgress)
        {
            lblJobDetail.Text = message;
            if (totalProgress != 0 && currentProgress != 0)
            {
                int percent = (currentProgress * 100) / totalProgress;
                lblPercent.Text = percent.ToString() + "%";
                if (lblPercent.Visibility == Visibility.Collapsed)
                    lblPercent.Visibility = Visibility.Visible;
            }
            else if (currentProgress != 0)
            {
                lblPercent.Text = currentProgress.ToString();
                if (lblPercent.Visibility == Visibility.Collapsed)
                    lblPercent.Visibility = Visibility.Visible;
            }
            else
                lblPercent.Visibility = Visibility.Collapsed;
        }
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {

            //ButtonClickHandlerAsync(sender, e);
            //timerCounter = 0;


            ImportTablesAndColumns();

            //infoTimer.Stop();
        }



        private async void ImportTablesAndColumns()
        {

            lblJobTitle.Text = "استخراج جداول و ستونها";
            imgJob.Source = GetImage("Images/database.png");

            //فانکشن اینجا تمام شده فرض میشه و خط بد در یک ترد جدید اجرا میشود
            //بنابراین یو آی ترد دیسپچر میره سراغ بقیه کارها و تصویر و عنوان را بروز میکند
            //var result = await GenerateDefaultEntitiesAndColumns();

         //   var result = new Task<ImportResult>();
            //lblJobTitle.Text = "استخراج روابط";
            //imgJob.Source = GetImage("Images/relationship.png");
            //var result = await GenerateRelationships();
            //ManageLogs(result, lstRelationships);

            //////lblJobTitle.Text = "استخراج نماها";
            //////imgJob.Source = GetImage("Images/tab.png");
            //////result = await GenerateViews();
            //////ManageLogs(result, lstViews);

            //////lblJobTitle.Text = "استخراج محدودیتهای یکتایی";
            //////imgJob.Source = GetImage("Images/property.png");
            //////result = await GenerateUniqueConstraints();
            //////ManageLogs(result, lstUniqueConstraints);

            //////lblJobTitle.Text = "استخراج توابع";
            //////imgJob.Source = GetImage("Images/function.png");
            //////result = await GenerateFunctions();
            //////ManageLogs(result, lstDBFunctions);
        }

        //private Task<ImportResult> GenerateDefaultEntitiesAndColumns()
        //{
        //    return Task.Run(() =>
        //    {
        //        var result = ImportHelper.GenerateDefaultEntitiesAndColumns();
        //        return result;
        //    });
        //}
        //private Task<ImportResult> GenerateRelationships()
        //{
        //    return Task.Run(() =>
        //    {
        //        var result = ImportHelper.GenerateRelationships();
        //        return result;
        //    });
        //}
        private Task<ImportResult> GenerateUniqueConstraints()
        {
            return Task.Run(() =>
            {
                var result = ImportHelper.GenerateUniqueConstraints();
                return result;
            });
        }
        //private Task<ImportResult> GenerateViews()
        //{
        //    return Task.Run(() =>
        //    {
        //        var result = ImportHelper.GenerateViews();
        //        return result;
        //    });
        //}
        //private Task<ImportResult> GenerateFunctions()
        //{
        //    return Task.Run(() =>
        //    {
        //        var result = ImportHelper.GenerateFunctions();
        //        return result;
        //    });
        //}
        private ImageSource GetImage(string imagePath)
        {
            Uri uriSource = new Uri(imagePath, UriKind.Relative);
            return new BitmapImage(uriSource);
        }

        private void ManageLogs(ImportResult result, ListBox list)
        {
            list.Items.Clear();
            //List<ListboxItem> listBoxItems = new List<ListboxItem>();
            foreach (var item in result.FailedItems)
            {
                var listboxItem = new MyProject_WPF.ListboxItem();
                listboxItem.Message = item.Title + (string.IsNullOrEmpty(item.Message) ? "" : " : " + item.Message);
                listboxItem.MessageColor = new SolidColorBrush(Colors.Red);
                list.Items.Add(listboxItem);
            }
            foreach (var item in result.SuccessfulItems)
            {
                var listboxItem = new MyProject_WPF.ListboxItem();
                listboxItem.Message = item.Title;
                listboxItem.MessageColor = new SolidColorBrush(Colors.Green);
                list.Items.Add(listboxItem);
            }
        }
    }

    public class DBImportIntention
    {
        public int DatabaseID { get; internal set; }
        public DbImportyType Type { set; get; }
    }
    public enum DbImportyType
    {
        AllDB,
        X
    }
    public class ListboxItem
    {
        public string Message { get; internal set; }
        public Brush MessageColor { set; get; }
    }
}
