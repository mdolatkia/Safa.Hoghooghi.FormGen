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
using Telerik.Windows.Controls;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmDBViewFunctions.xaml
    /// </summary>
    public partial class frmDBViewFunctions : UserControl
    {
        //private ModelGenerator_sql ModelGenerator = new ModelGenerator_sql();
        BizUniqueConstraints bizUniqueConstraint = new BizUniqueConstraints();
        int DatabaseID { set; get; }
        public frmDBViewFunctions(int databaseID)
        {
            InitializeComponent();
            DatabaseID = databaseID;
            SetGridViewColumns();
        }


        private void SetGridViewColumns()
        {
            SetGridViewColumns(dtgUniqueConstraint);
          //  SetGridViewColumns(dtgViews);
            SetGridViewColumns(dtgFunctions);

        }
        private void SetGridViewColumns(RadGridView dataGrid)
        {
            dataGrid.AlternateRowBackground = new SolidColorBrush(Colors.LightBlue);
            dataGrid.AutoGenerateColumns = false;
            //if (dataGrid == dtgViews)
            //{
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "شناسه", true, 100, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "نام", true, 100, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Catalog", "کاتالوگ", true, 100, GridViewColumnType.Text));
            //    dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("RelatedSchema", "شمای مرتبط", true, 100, GridViewColumnType.Text));
            //}
            //else 
            if (dataGrid == dtgFunctions)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "شناسه", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("FunctionName", "نام", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Title", "عنوان", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Catalog", "کاتالوگ", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("RelatedSchema", "شمای مرتبط", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ReturnType", "نوع نتیجه", true, 100, GridViewColumnType.Text));
                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ValueCustomType", "نوع خاص", false, 100, GridViewColumnType.Enum, Enum.GetValues(typeof(ValueCustomType)).Cast<object>()));
            }

            else if (dataGrid == dtgUniqueConstraint)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "شناسه", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "نام", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Table", "جدول", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Columns", "ستونها", true, 100, GridViewColumnType.Text));
            }
        }

        private void btnRefreshViews_Click(object sender, RoutedEventArgs e)
        {
            //BizView bizView = new BizView();
            //dtgViews.ItemsSource = bizView.GetViews(DatabaseID);
        }

        private void btnImportViews_Click(object sender, RoutedEventArgs e)
        {
            //if (MessageBox.Show("فرایند ورود اطلاعات خودکار نماها" + Environment.NewLine + Environment.NewLine + "آیا مطمئن هستید؟", "تائید", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            //{
            //    //try
            //    //{
            //    var result = ModelGenerator.GenerateViews();
            //    if (result)
            //    {
            //        BizView bizView = new BizView();
            //        dtgViews.ItemsSource = bizView.GetViews(DatabaseID);
            //        MessageBox.Show("Operation is completed.");
            //    }
            //    else
            //        MessageBox.Show("Operation is not done!");
            //}
        }

        private void btnRefreshFunctions_Click(object sender, RoutedEventArgs e)
        {
            BizDatabaseFunction bizDatabaseFunction = new BizDatabaseFunction();
            dtgFunctions.ItemsSource = bizDatabaseFunction.GetDatabaseFunctions(Enum_DatabaseFunctionType.None, DatabaseID);
        }

        private void btnImportFunctions_Click(object sender, RoutedEventArgs e)
        {
            BizDatabase bizDatabase = new BizDatabase();
            var database = bizDatabase.GetDatabase(DatabaseID);
            var ImportHelper = ModelGenerator.GetDatabaseImportHelper(database);
            //ImportHelper.GenerateFunctions();
            //if (MessageBox.Show("فرایند ورود اطلاعات خودکار توابع" + Environment.NewLine + Environment.NewLine + "آیا مطمئن هستید؟", "تائید", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            //{
            //    //try
            //    //{
            //    var result = ModelGenerator.GenerateFunctions();
            //    if (result)
            //    {
            //        BizDatabaseFunction bizDatabaseFunction = new BizDatabaseFunction();
            //        dtgFunctions.ItemsSource = bizDatabaseFunction.GetDatabaseFunctions(Enum_DatabaseFunctionType.None, DatabaseID);
            //        MessageBox.Show("Operation is completed.");
            //    }
            //    else
            //        MessageBox.Show("Operation is not done!");
            //}
        }
        private void btnImportUniqueConstraints_Click(object sender, RoutedEventArgs e)
        {
            //if (MessageBox.Show("فرایند ورود اطلاعات خودکار شروط یکتایی" + Environment.NewLine + Environment.NewLine + "آیا مطمئن هستید؟", "تائید", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            //{
            //    //try
            //    //{
            //    var result = ModelGenerator.GenerateUniqueConstraints();
            //    if (result)
            //    {
            //        BizUniqueConstraints bizUniqueConstraints = new BizUniqueConstraints();
            //        dtgUniqueConstraint.ItemsSource = bizUniqueConstraints.GetUniqueConstraints(DatabaseID);
            //        MessageBox.Show("Operation is completed.");
            //    }
            //    else
            //        MessageBox.Show("Operation is not done!");
            //}
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Operation failed." + Environment.NewLine + ex.Message);
            //}
        }
        private void btnRefreshUniqueConstraints_Click(object sender, RoutedEventArgs e)
        {
            dtgUniqueConstraint.ItemsSource = bizUniqueConstraint.GetUniqueConstraints(DatabaseID);
        }

        private void btnUpdateDBFunctions_Click(object sender, RoutedEventArgs e)
        {
            BizDatabaseFunction biz = new BizDatabaseFunction();
            btnUpdateDBFunctions.IsEnabled = false;
            biz.Save(dtgFunctions.ItemsSource as List<DatabaseFunctionDTO>);
            btnUpdateDBFunctions.IsEnabled = true;
        }
    }
}
