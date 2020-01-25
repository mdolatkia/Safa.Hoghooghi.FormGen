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
    public partial class frmArchiveTagSelect : UserControl, I_View_ArchiveTagFiltered
    {
        public event EventHandler ArchiveTagsFiltered;

        public frmArchiveTagSelect()
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

       

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ArchiveTagsFiltered != null)
                ArchiveTagsFiltered(this, null);
        }
    }
}
