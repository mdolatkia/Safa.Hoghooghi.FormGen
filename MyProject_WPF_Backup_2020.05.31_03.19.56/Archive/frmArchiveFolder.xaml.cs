using ModelEntites;
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
    /// Interaction logic for frmArchiveFolder.xaml
    /// </summary>
    public partial class frmArchiveFolder : UserControl
    {
        int? EntityID { set; get; }
        BizArchive bizArchive = new BizArchive();
        ArchiveFolderDTO message;
        public frmArchiveFolder(int? entityID)
        {
            InitializeComponent();
            EntityID = entityID;
            GetItems();
            dtgItems.SelectionChanged += DtgItems_SelectionChanged;
            ControlHelper.GenerateContextMenu(dtgItems);
        }

        private void DtgItems_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (dtgItems.SelectedItem != null)
            {
                message = dtgItems.SelectedItem as ArchiveFolderDTO;
                ShowItem();
            }
        }

        private void ShowItem()
        {
            txtName.Text = message.Name;
        }

        private void GetItems()
        {
            dtgItems.ItemsSource = bizArchive.GetArchiveFolders(EntityID, false);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (message == null)
                message = new ArchiveFolderDTO();
            message.Name = txtName.Text;
            message.EntityID = EntityID;
            bizArchive.UpdateArchiveFolder(message);
            GetItems();
            btnNew_Click(null, null);
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            message = new ArchiveFolderDTO();
            ShowItem();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }

        private void btnGeneralFolders_Click(object sender, RoutedEventArgs e)
        {
            frmArchiveFolder frm = new frmArchiveFolder(null);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "فولدر آرشیو", Enum_WindowSize.Big);
        }
    }
}
