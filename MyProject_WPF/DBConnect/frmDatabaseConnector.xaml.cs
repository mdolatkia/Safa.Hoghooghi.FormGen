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
    /// Interaction logic for frmDatabaseConnector.xaml
    /// </summary>
    public partial class frmDatabaseConnector : UserControl
    {
        public frmDatabaseConnector(bool userCanExtract)
        {
            InitializeComponent();
            SetDatabaseLookup();
            if (!userCanExtract)
                btnImport.Visibility = Visibility.Collapsed;
            //if(!allowEdit)
            //{
            //    lokDatabase.
            //}
        }
        BizDatabase bizDatabase = new BizDatabase();
        private void SetDatabaseLookup()
        {
            bool firstTime = false;
            if (lokDatabase.SelectedValueMember == null)
            {
                firstTime = true;
                lokDatabase.SelectedValueMember = "ID";
                lokDatabase.DisplayMember = "Title";
                ////lokDatabase.EditItemClicked += LokDatabase_NewItemClicked;
                lokDatabase.NewItemEnabled = true;
                lokDatabase.EditItemEnabled = true;
                lokDatabase.EditItemClicked += LokDatabase_EditItemClicked;


            }

            //   قابل دسترسی کاربر     //
            var databases = bizDatabase.GetDatabases();
            lokDatabase.ItemsSource = databases;

            if (firstTime && Properties.Settings.Default.LastDatabaseID != 0)
                lokDatabase.SelectedValue = Properties.Settings.Default.LastDatabaseID;

        }

        private void LokDatabase_EditItemClicked(object sender, MyCommonWPFControls.EditItemClickEventArg e)
        {
            int dbId = 0;
            if (lokDatabase.SelectedItem != null)
                dbId = (int)lokDatabase.SelectedValue;
            frmDatabaseCreate frm = new frmDatabaseCreate(dbId);
            frm.DatabaseUpdated += Frm_DatabaseUpdated;
            MyProjectManager.GetMyProjectManager().ShowDialog(frm, "اصلاح پایگاه داده");
        }

        private void LokDatabase_NewItemClicked(object sender, EventArgs e)
        {
            frmDatabaseCreate frm = new frmDatabaseCreate(0);
            frm.DatabaseUpdated += Frm_DatabaseUpdated;
            MyProjectManager.GetMyProjectManager().ShowDialog(frm, "تعریف پایگاه داده");
        }

        private void Frm_DatabaseUpdated(object sender, DatabaseUpdatedArg e)
        {
            SetDatabaseLookup();
            lokDatabase.SelectedValue = e.DatabaseID;
        }

        public event EventHandler<DatabaseConnectionArg> DatabaseConnected;
        private void btnConnection_Click(object sender, RoutedEventArgs e)
        {
            if (lokDatabase.SelectedItem != null)
            {
                var databaseID = (int)lokDatabase.SelectedValue;
                var connection = ConnectionManager.GetDBHelper(databaseID);
                var testConnection = connection.TestConnection();

                if (testConnection.Successful)
                {
                    if (DatabaseConnected != null)
                        DatabaseConnected(this, new DatabaseConnectionArg() { DatabaseID = databaseID });
                    Properties.Settings.Default.LastDatabaseID = databaseID;
                    Properties.Settings.Default.Save();
                    //if (testResult)
                    //{
                    //Properties.Settings.Default.LastDatabaseName = txtDBName.Text;
                    //Properties.Settings.Default.LastServerName = txtServerName.Text;
                    //Properties.Settings.Default.Save();
                    //SetFormMode(FormMode.ConnectionSucceed);
                }
                else
                {
                    MessageBox.Show("Connection Failed!" + Environment.NewLine + testConnection.Message);
                }
                //   MessageBox.Show("Connection Successfull!");
                //}
                //else
                //{
                //    SetFormMode(FormMode.ConnectionFailed);
                //    MessageBox.Show("Connection failed!");
                //}
            }
            else
                MessageBox.Show("اطلاعات اتصال وارد نشده اند");
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            if (lokDatabase.SelectedItem != null)
            {
                var frm = new DatabaseImport(new MyProject_WPF.DBImportIntention() { DatabaseID = (int)lokDatabase.SelectedValue });
                MyProjectManager.GetMyProjectManager().ShowDialog(frm, "استخراج پایگاه داده");
            }
        }
    }
    public class DatabaseConnectionArg : EventArgs
    {
        public int DatabaseID { set; get; }
    }
  
}
