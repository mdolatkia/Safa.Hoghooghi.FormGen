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
using ModelEntites;
using Telerik.Windows.Media.Imaging.FormatProviders;
using System.IO;

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for frmViewImage.xaml
    /// </summary>
    public partial class frmViewImage : UserControl, I_View_ViewArchiveItem
    {
        public frmViewImage()
        {

            InitializeComponent();
            pnlFileCommand.NextFile += PnlFileCommand_NextFile;
            pnlFileCommand.PreviousFile += PnlFileCommand_PreviousFile;
            pnlFileCommand.SaveFile += PnlFileCommand_SaveFile;
            pnlFileCommand.DownloadFile += PnlFileCommand_DownloadFile;
        }

        private void PnlFileCommand_DownloadFile(object sender, EventArgs e)
        {
            if (DownloadItemClicked != null)
                DownloadItemClicked(this, null);
        }

        private void PnlFileCommand_SaveFile(object sender, EventArgs e)
        {
            if (SaveItemClicked != null)
                SaveItemClicked(this, null);
        }

        private void PnlFileCommand_PreviousFile(object sender, EventArgs e)
        {
            if (PreviousItemClicked != null)
                PreviousItemClicked(this, null);
        }

        private void PnlFileCommand_NextFile(object sender, EventArgs e)
        {
            if (NextItemClicked != null)
                NextItemClicked(this, null);
        }

        public ArchiveItemDTO ArchiveItemDataItem
        {
            set; get;
        }

        public bool AllowSave
        {
            get
            {
                return pnlFileCommand.AllowSave;
            }

            set
            {
                pnlFileCommand.AllowSave = value;
            }
        }

        public event EventHandler DownloadItemClicked;
        public event EventHandler NextItemClicked;
        public event EventHandler PreviousItemClicked;
        public event EventHandler SaveItemClicked;

        public byte[] GetImage()
        {
            if (formatProvider != null)
            {
                byte[] _selectedPhoto = null;
                using (MemoryStream memStrm = new MemoryStream())
                {
                    formatProvider.Export(imageEditorUI.ImageEditor.Image, memStrm);

                    if (memStrm.Length > int.MaxValue)
                    {
                        throw new ApplicationException("Image is too large.");
                    }
                    _selectedPhoto = new byte[memStrm.Length];
                    memStrm.Position = 0;
                    memStrm.Read(_selectedPhoto, 0, (Int32)memStrm.Length);
                }
                return _selectedPhoto;
            }
            return null;
        }
        IImageFormatProvider formatProvider = null;
        public void ShowArchiveItem()
        {
            formatProvider = ImageFormatProviderManager.GetFormatProviderByExtension(ArchiveItemDataItem.AttechedFile.FileExtension);
            if (formatProvider == null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Unable to find format provider for extension: ")
                  .Append(ArchiveItemDataItem.AttechedFile.FileExtension).Append(" .");
                return;
            }
            else
            {
                imageEditorUI.Image = formatProvider.Import(ArchiveItemDataItem.AttechedFile.Content);
            }
        }
    }
}
