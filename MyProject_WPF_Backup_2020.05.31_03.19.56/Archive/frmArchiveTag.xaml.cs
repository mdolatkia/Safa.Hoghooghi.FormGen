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
    /// Interaction logic for frmArchiveTag.xaml
    /// </summary>
    public partial class frmArchiveTag : UserControl
    {
        int? EntityID { set; get; }
        BizArchive bizArchive = new BizArchive();
        ArchiveTagDTO message;
        public frmArchiveTag(int? entityID)
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
                message = dtgItems.SelectedItem as ArchiveTagDTO;
                ShowItem();
            }
        }

        private void ShowItem()
        {
            txtName.Text = message.Name;
        }

        private void GetItems()
        {
            dtgItems.ItemsSource = bizArchive.GetArchiveTags(EntityID,false);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (message == null)
                message = new ArchiveTagDTO();
            message.Name = txtName.Text;
            message.EntityID = EntityID;
            bizArchive.UpdateArchiveTag(message);
            GetItems();
            btnNew_Click(null, null);
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            message = new ArchiveTagDTO();
            ShowItem();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
        private void btnGeneralFolders_Click(object sender, RoutedEventArgs e)
        {
            frmArchiveTag frm = new frmArchiveTag(null);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "فولدر آرشیو", Enum_WindowSize.Big);
        }
    }
}
