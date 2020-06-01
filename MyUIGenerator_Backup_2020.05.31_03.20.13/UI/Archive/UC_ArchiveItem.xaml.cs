using ModelEntites;
using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class UC_ArchiveItem : UserControl
    {
        //public event EventHandler Clicked;

        public ArchiveItemDTO ArchiveItemDataItem { set; get; }
        public UC_ArchiveItem(ArchiveItemDTO archiveItemDataItem)
        {
            InitializeComponent();
            ArchiveItemDataItem = archiveItemDataItem;
            lblTitle.Text = archiveItemDataItem.Name;
            if (!string.IsNullOrEmpty(archiveItemDataItem.Tooltip))
                ToolTipService.SetToolTip(this, archiveItemDataItem.Tooltip);
            if (!string.IsNullOrEmpty(archiveItemDataItem.Color))
                grdMain.Background = new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString(archiveItemDataItem.Color));
            if (archiveItemDataItem != null)
            {
                if (archiveItemDataItem.ThumbnailFile != null && archiveItemDataItem.ThumbnailFile.Content != null)
                {
                    var imageSource = new BitmapImage();
                    imageSource.BeginInit();
                    imageSource.StreamSource = new MemoryStream(archiveItemDataItem.ThumbnailFile.Content);
                    imageSource.EndInit();
                    // Assign the Source property of your image
                    imgItem.Source = imageSource;
                }
                else
                {
                    if (archiveItemDataItem.MainType == Enum_ArchiveItemMainType.Pdf)
                        imgItem.Source = GetImageSource(@"..\..\images\pdf.png");
                    else if (archiveItemDataItem.MainType == Enum_ArchiveItemMainType.MsWord)
                        imgItem.Source = GetImageSource(@"..\..\images\msword.png");
                }
            }
            else
            {
                imgItem.Source = GetImageSource(@"..\..\images\document.png");
            }
        }

        private ImageSource GetImageSource(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        //private void ImgItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (Clicked != null)
        //        Clicked(this, null);
        //}
    }
}
