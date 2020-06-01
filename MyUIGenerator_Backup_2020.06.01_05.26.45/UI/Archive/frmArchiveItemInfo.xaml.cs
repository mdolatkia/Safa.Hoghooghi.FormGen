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
    public partial class frmArchiveItemInfo : UserControl, I_View_ArchiveItemInfo
    {
        public event EventHandler ArchiveInfoConfirmed;
        public event EventHandler CloseRequested;

        public frmArchiveItemInfo()
        {
            InitializeComponent();
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
        //public ArchiveItemDataItemDTO Message
        //{
        //    set;get;
        //}
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

        public int ID
        {
            set;get;
        }

        public string ItemName
        {
            get
            {
                return txtName.Text;
            }

            set
            {
                txtName.Text = value;
            }
        }

        public int ItemID
        {
            set
            {
                txtID.Text = value.ToString();
            }
        }

        public DateTime CreateDate
        {
            set
            {
                txtCreateDate.Text = value.ToString();
            }
        }

        public string UserRealName
        {
            set
            {
                txtCreatorUser.Text = value;
            }
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

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            if (CloseRequested != null)
                CloseRequested(this, null);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (ArchiveInfoConfirmed != null)
            {
                ArchiveInfoConfirmed(this, null);
            }
        }

       

        public void SetFolders(List<ArchiveFolderWithNullDTO> items)
        {
            cmbFolder.DisplayMemberPath = "Name";
            cmbFolder.SelectedValuePath = "ID";
            cmbFolder.ItemsSource = items;
        }

     
    }
}
