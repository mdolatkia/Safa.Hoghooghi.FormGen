using ModelEntites;
using MyUILibrary.EntityArea;
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

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for frmArchiveTag.xaml
    /// </summary>
    public partial class frmMultipleArchiveItemsInfo : UserControl, I_View_MultipleArchiveItemsInfo
    {



        public event EventHandler MultipleArchiveInfosConfirmed;
        public frmMultipleArchiveItemsInfo()
        {
            InitializeComponent();
        }

        public int? SelectedFolder
        {
            set
            {
                cmbFolder.SelectedValue = value;
            }
            get
            {
                return (int?)cmbFolder.SelectedValue;
            }
        }
        public bool CanChangeTags
        {
            set
            {
                chkChangeTagIDs.IsChecked = value;
                dtgItems.IsEnabled = value;
            }
            get
            {
                return chkChangeTagIDs.IsChecked == true;
            }
        }
        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (MultipleArchiveInfosConfirmed != null)
                MultipleArchiveInfosConfirmed(this, null);
        }



        public void SetFolders(List<ArchiveFolderWithNullDTO> items)
        {
            cmbFolder.DisplayMemberPath = "Name";
            cmbFolder.SelectedValuePath = "ID";
            cmbFolder.ItemsSource = items;
        }
        List<ArchiveTagDTO> _ArchiveTags;
        public List<ArchiveTagDTO> ArchiveTags
        {
            set
            {
                _ArchiveTags = value;
                dtgItems.ItemsSource = null;
                dtgItems.ItemsSource = value;

            }
            get
            {
                return _ArchiveTags;
            }
        }

        public List<int> IDs
        {
            set; get;
        }

        public bool AllowSave
        {
            get
            {
                return btnSave.IsEnabled;
            }

            set
            {
                btnSave.IsEnabled = value;
            }
        }

        private void chkChangeTagIDs_Checked(object sender, RoutedEventArgs e)
        {
            if (chkChangeTagIDs.IsChecked == true)
            {
                dtgItems.IsEnabled = true;
            }
            else
                dtgItems.IsEnabled = false;
        }
    }
}
