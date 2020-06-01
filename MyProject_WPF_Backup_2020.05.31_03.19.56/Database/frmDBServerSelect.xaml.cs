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
    /// Interaction logic for frmDBServerList.xaml
    /// </summary>
    public partial class frmDBServerSelect : UserControl
    {
        public event EventHandler<DBServerSelectedArg> DBServerSelected;
        SelectorGrid SelectorGrid = null;
        public frmDBServerSelect()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                ShowDBServers();

            var listColumns = new Dictionary<string, string>();
            listColumns.Add("Name", "نام");
            listColumns.Add("Title", "عنوان");
            listColumns.Add("IPAddress", "IP Address");
            SelectorGrid = ControlHelper.SetSelectorGrid(dtgItems, listColumns);
            SelectorGrid.DataItemSelected += SelectorGrid_DataItemSelected;
        }

        private void SelectorGrid_DataItemSelected(object sender, object e)
        {
            CheckSelectedItem(e);
        }

        public void ShowDBServers()
        {
            BizDatabase bizDBServer = new BizDatabase();
            var DBServerList = bizDBServer.GetDBServers();
            dtgItems.ItemsSource = DBServerList;
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            CheckSelectedItem(dtgItems.SelectedItem);
        }

        private void CheckSelectedItem(object item)
        {
            if (item != null)
            {
                var DBServerDTO = item as DbServerDTO;
                if (DBServerDTO != null)
                {
                    DBServerSelectedArg arg = new DBServerSelectedArg();
                    arg.DBServerID = DBServerDTO.ID;
                    if (DBServerSelected != null)
                        DBServerSelected(this, arg);
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }
    public class DBServerSelectedArg : EventArgs
    {
        public int DBServerID { set; get; }
    }
}
