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
    public partial class frmDatabaseSetting : UserControl
    {
        public event EventHandler<DatabaseSettingUpdatedArg> DatabaseUpdated;
        BizDatabase bizDatabase = new BizDatabase();
        DatabaseDTO Message;
        public frmDatabaseSetting(int databaseID)
        {
            InitializeComponent();
            Message = bizDatabase.GetDatabase(databaseID);
            txtDBName.Text = Message.Name;
            txtDBTitle.Text = Message.Title;
            if (Message.DatabaseSetting == null)
                Message.DatabaseSetting = new DatabaseSettingDTO();
            chkFlowDirectionLTR.IsChecked = Message.DatabaseSetting.FlowDirectionLTR;
            chkShowMiladiDateInUI.IsChecked = Message.DatabaseSetting.ShowMiladiDateInUI;
            chkStringDateColumnIsMiladi.IsChecked = Message.DatabaseSetting.StringDateColumnIsMiladi;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Message.DatabaseSetting.FlowDirectionLTR = chkFlowDirectionLTR.IsChecked == true;
            Message.DatabaseSetting.ShowMiladiDateInUI = chkShowMiladiDateInUI.IsChecked == true;
            Message.DatabaseSetting.StringDateColumnIsMiladi = chkStringDateColumnIsMiladi.IsChecked == true;
            bizDatabase.SaveDatabaseSetting(Message.ID, Message.DatabaseSetting);
            MessageBox.Show("عملیات انجام شد");
            if (DatabaseUpdated != null)
                DatabaseUpdated(this, new DatabaseSettingUpdatedArg() { DatabaseID = Message.ID });
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }


    }
    public class DatabaseSettingUpdatedArg : EventArgs
    {
        public int DatabaseID { set; get; }
    }
}
