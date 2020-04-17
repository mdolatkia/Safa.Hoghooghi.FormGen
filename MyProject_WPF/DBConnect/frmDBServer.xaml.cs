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
    /// Interaction logic for frmServerCreate.xaml
    /// </summary>
    public partial class frmDBServer : UserControl
    {
        public event EventHandler<ServerUpdatedArg> ServerUpdated;
        BizDatabase bizDatabase = new BizDatabase();
        DbServerDTO Message;
        public frmDBServer(int serverID)
        {
            InitializeComponent();

            if (serverID == 0)
            {
                Message = new DbServerDTO();
            }
            else
            {
                GetDBServer(serverID);
            }

        }
        private void GetDBServer(int serverID)
        {
            Message = bizDatabase.GetDBServer(serverID);
            ShowMessage();
        }
        private void ShowMessage()
        {
            txtServer.Text = Message.Name;
            txtTitle.Text = Message.Title;
            txtIPAddress.Text = Message.IPAddress;

        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            Message.Name = txtServer.Text;
            Message.Title = txtTitle.Text;
            Message.IPAddress = txtIPAddress.Text;

            Message.ID = bizDatabase.SaveServer(Message);

            if (ServerUpdated != null)
                ServerUpdated(this, new MyProject_WPF.ServerUpdatedArg() { ServerID = Message.ID });
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Message = new DbServerDTO();
            ShowMessage();
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            frmDBServerSelect frmSelect = new frmDBServerSelect();
            frmSelect.DBServerSelected += FrmSelect_DBServerSelected;
            MyProjectManager.GetMyProjectManager.ShowDialog(frmSelect, "انتخاب سرور",Enum_WindowSize.Big);
        }

        private void FrmSelect_DBServerSelected(object sender, DBServerSelectedArg e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(sender);
            GetDBServer(e.DBServerID);
        }

        private void btnLinkedServer_Click(object sender, RoutedEventArgs e)
        {
            if (Message.ID != 0)
            {
                frmLinekedServer view = new frmLinekedServer(Message.ID);
                MyProjectManager.GetMyProjectManager.ShowDialog(view, "ارتباط سرور",Enum_WindowSize.Big);

            }
        }
    }
    public class ServerUpdatedArg : EventArgs
    {
        public int ServerID { set; get; }
    }
}
