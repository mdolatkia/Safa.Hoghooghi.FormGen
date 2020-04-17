using ModelEntites;
using MyModelManager;

using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmDatabaseList.xaml
    /// </summary>
    public partial class frmDatabaseSelect : UserControl
    {
        SelectorGrid SelectorGrid = null;
        public event EventHandler<DatabaseSelectedArg> DatabaseSelected;
        public frmDatabaseSelect()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                ShowDatabases();

            var listColumns = new Dictionary<string, string>();
            listColumns.Add("Name", "نام");
            listColumns.Add("Title", "عنوان");
            listColumns.Add("DBServerTitle", "عنوان سرور");
            listColumns.Add("DBServerName", "نام سرور");
            listColumns.Add("DBType", "نوع");

            SelectorGrid = ControlHelper.SetSelectorGrid(dtgItems, listColumns);
            SelectorGrid.DataItemSelected += SelectorGrid_DataItemSelected;
        }
        private void SelectorGrid_DataItemSelected(object sender, object e)
        {
            CheckSelectedItem(e);
        }
        private void CheckSelectedItem(object item)
        {
            if (item != null)
            {
                var databaseDTO = item as DatabaseDTO;
                if (databaseDTO != null)
                {
                    DatabaseSelectedArg arg = new DatabaseSelectedArg();
                    arg.DatabaseID = databaseDTO.ID;
                    if (DatabaseSelected != null)
                        DatabaseSelected(this, arg);
                }
            }
        }
        public void ShowDatabases()
        {
            BizDatabase bizDatabase = new BizDatabase();
            var DatabaseList = bizDatabase.GetDatabases();
            dtgItems.ItemsSource = DatabaseList;
        }


        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            CheckSelectedItem(dtgItems.SelectedItem);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }
    public class DatabaseSelectedArg : EventArgs
    {
        public int DatabaseID { set; get; }
    }
}
