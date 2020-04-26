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
    public partial class frmLinekedServer : UserControl
    {
        //  public event EventHandler<ServerUpdatedArg> ServerUpdated;
        LinkedServerDTO Message;
        BizDatabase bizDatabase = new BizDatabase();
        DbServerDTO SourceServer { set; get; }
        public frmLinekedServer(int sourceServerID)
        {
            InitializeComponent();

            SourceServer = bizDatabase.GetDBServer(sourceServerID);
            lblSourceServer.Text = SourceServer.Name;
            SetLinkedServers();
            SetLookups();
            dtgItems.SelectionChanged += DtgItems_SelectionChanged;
        }

        private void DtgItems_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (dtgItems.SelectedItem != null)
            {
                var linkedServer = dtgItems.SelectedItem as LinkedServerDTO;
                Message = linkedServer;
                ShowMessage();
            }
        }

        private void SetLinkedServers()
        {
            var linkedServers = bizDatabase.GetLinkedServerBySourceServerID(SourceServer.ID);
            dtgItems.ItemsSource = linkedServers;
        }

        private void SetLookups()
        {
            var servers = bizDatabase.GetDBServers();
            lokTargetServer.SelectedValueMember = "ID";
            lokTargetServer.DisplayMember = "Name";
            lokTargetServer.ItemsSource = servers;
        }

        private void GetDBServer(int linkedServerID)
        {
            Message = bizDatabase.GetLinkedServer(linkedServerID);
            ShowMessage();
        }
        private void ShowMessage()
        {
            lokTargetServer.SelectedValue = Message.TargetDBServerID;
            txtTitle.Text = Message.Name;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (Message == null)
                Message = new LinkedServerDTO();
            Message.SourceDBServerID = SourceServer.ID;
            Message.TargetDBServerID = (int)lokTargetServer.SelectedValue;
            Message.Name = txtTitle.Text;
            Message.ID = bizDatabase.SaveLinkedServer(Message);
            SetLinkedServers();
            btnNew_Click(null, null);
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Message = new LinkedServerDTO();
            ShowMessage();
        }





    }

}
