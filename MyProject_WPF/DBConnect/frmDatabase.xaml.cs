using ModelEntites;
using MyConnectionManager;
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
    /// Interaction logic for frmDatabaseCreate.xaml
    /// </summary>
    public partial class frmDatabase : UserControl
    {
        public event EventHandler<DatabaseUpdatedArg> DatabaseUpdated;
        public frmDatabase(int databaseID)
        {
            InitializeComponent();

            SetServerLookup();
            if (databaseID == 0)
            {
                Message = new DatabaseDTO();
            }
            else
            {
                GetDatabase(databaseID);
            }
        }

        private void GetDatabase(int databaseID)
        {
            Message = bizDatabase.GetDatabase(databaseID);
            ShowMessage();
        }

        private void ShowMessage()
        {
            lokServer.SelectedValue = Message.DBServerID;
            txtDBName.Text = Message.Name;
            txtTitle.Text = Message.Title;
            txtConnectionString.Text = Message.ConnectionString;
            chkDBHasData.IsChecked = Message.DBHasData;
        }

        BizDatabase bizDatabase = new BizDatabase();
        DatabaseDTO Message;
        private void SetServerLookup()
        {
            if (lokServer.SelectedValueMember == null)
            {
                lokServer.SelectedValueMember = "ID";
                lokServer.DisplayMember = "Title";
                lokServer.NewItemEnabled = true;
                //lokServer.NewItemClicked += LokServer_NewItemClicked;
                lokServer.EditItemEnabled = true;
                lokServer.EditItemClicked += LokServer_EditItemClicked;
            }
            BizDatabase bizDatabase = new BizDatabase();
            var servers = bizDatabase.GetDBServers();
            lokServer.ItemsSource = servers;

        }

        private void LokServer_EditItemClicked(object sender, MyCommonWPFControls.EditItemClickEventArg e)
        {
            int serverID = 0;
            if (lokServer.SelectedItem != null)
                serverID = (int)lokServer.SelectedValue;
            frmDBServer frm = new frmDBServer(serverID);
            frm.ServerUpdated += Frm_ServerUpdated;
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "اصلاح سرور");
        }

        private void LokServer_NewItemClicked(object sender, EventArgs e)
        {
            frmDBServer frm = new frmDBServer(0);
            frm.ServerUpdated += Frm_ServerUpdated;
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "تعریف سرور");
        }
        private void Frm_ServerUpdated(object sender, ServerUpdatedArg e)
        {
            SetServerLookup();
            lokServer.SelectedValue = e.ServerID;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (lokServer.SelectedItem == null)
            {
                return;
            }
            Message.DBServerID = (int)lokServer.SelectedValue;
            Message.Name = txtDBName.Text;
            Message.Title = txtTitle.Text;
            Message.ConnectionString = txtConnectionString.Text;
            Message.DBHasData = chkDBHasData.IsChecked == true;
            Message.ID = bizDatabase.SaveDatabase(Message);
        
            if (DatabaseUpdated != null)
                DatabaseUpdated(this, new MyProject_WPF.DatabaseUpdatedArg() { DatabaseID = Message.ID });
        }

        //private void btnClose_Click(object sender, RoutedEventArgs e)
        //{
        //    MyProjectManager.GetMyProjectManager.CloseDialog(this);
        //}

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Message = new  DatabaseDTO();
            ShowMessage();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            frmDatabaseSelect frmOrganizationSelect = new frmDatabaseSelect();
            frmOrganizationSelect.DatabaseSelected += FrmOrganizationSelect_DatabaseSelected;
            MyProjectManager.GetMyProjectManager.ShowDialog(frmOrganizationSelect, "انتخاب پایگاه داده");
        }

        private void FrmOrganizationSelect_DatabaseSelected(object sender, DatabaseSelectedArg e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(sender);
            GetDatabase(e.DatabaseID);
        }
    }
    public class DatabaseUpdatedArg : EventArgs
    {
        public int DatabaseID { set; get; }
    }
}
