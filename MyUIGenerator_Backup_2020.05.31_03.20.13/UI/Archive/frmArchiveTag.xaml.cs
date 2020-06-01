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
    public partial class frmArchiveTag : UserControl, I_View_ArchiveTag
    {
       public event EventHandler ArchiveTagsConfirmed;

        public frmArchiveTag()
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

        public ArchiveItemDataItemDTO ArchiveItemDataItem
        {
            set;get;
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ArchiveTagsConfirmed != null)
                ArchiveTagsConfirmed(this, null);
        }
    }
}
