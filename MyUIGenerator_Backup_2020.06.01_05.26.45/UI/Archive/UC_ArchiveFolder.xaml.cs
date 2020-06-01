using ModelEntites;
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
    /// Interaction logic for UC_ArchiveFolder.xaml
    /// </summary>
    public partial class UC_ArchiveFolder : UserControl
    {
        //public event EventHandler Clicked;
     public ArchiveFolderWithNullDTO ArchiveFolder { set; get; }
        public UC_ArchiveFolder(ArchiveFolderWithNullDTO archiveFolder)
        {
            InitializeComponent();
            //imgFolder.MouseLeftButtonDown += ImgFolder_MouseLeftButtonUp;
            ArchiveFolder = archiveFolder;
            lblTitle.Text = ArchiveFolder.Name;
            lblCount.Text = ArchiveFolder.tmpCount.ToString();
        }

        //private void ImgFolder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (Clicked != null)
        //        Clicked(this, null);
        //}
    }
}
