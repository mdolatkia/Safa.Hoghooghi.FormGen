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
using Telerik.Windows.Controls;

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for frmAddArchiveItems.xaml
    /// </summary>
    public partial class frmAddArchiveItems : UserControl, I_View_AddArchiveItems
    {
        public event EventHandler FilesConfirmed;
        public event EventHandler<FileTagsRequestedArg> FileTagsRequested;
        public frmAddArchiveItems()
        {
            InitializeComponent();
            Files = new List<FileRepositoryDTO>();
            dtgFiles.ItemsSource = Files;
            dtgFiles.RowLoaded += DtgFiles_RowLoaded;
            dtgFiles.CellLoaded += DtgFiles_CellLoaded;
        }

        private void DtgFiles_CellLoaded(object sender, Telerik.Windows.Controls.GridView.CellEventArgs e)
        {
            if (e.Cell.DataContext is FileRepositoryDTO)
            {
                if (e.Cell.Column is GridViewHyperlinkColumn)
                {
                    //if (e.Cell.Content == null)
                    //{
                    if ((e.Cell.DataContext as FileRepositoryDTO).tmpState != FileRepositoryState.Succeed)
                    {
                        var link = new Button();
                        link.Content = "اصلاح";
                        link.Click += (sender1, e1) => Link_Click(sender1, e1, (e.Cell.DataContext as FileRepositoryDTO));
                        e.Cell.Content = link;
                    }
                    //}
                }
            }
        }

        private void Link_Click(object sender, RoutedEventArgs e, FileRepositoryDTO file)
        {
            if (FileTagsRequested != null)
            {
                FileTagsRequested(this, new FileTagsRequestedArg() { File = file });
            }
        }

        private void DtgFiles_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (e.DataElement is FileRepositoryDTO)
            {
                var fileRepository = (e.DataElement as FileRepositoryDTO);
                if (!string.IsNullOrEmpty(fileRepository.tmpUploadMessage))
                    ToolTipService.SetToolTip(e.Row, fileRepository.tmpUploadMessage);
                else
                    ToolTipService.SetToolTip(e.Row, null);
                if (fileRepository.tmpState == FileRepositoryState.Normal)
                    e.Row.Foreground = new SolidColorBrush(Colors.Black);
                else if (fileRepository.tmpState == FileRepositoryState.Succeed)
                    e.Row.Foreground = new SolidColorBrush(Colors.Green);
                else if (fileRepository.tmpState == FileRepositoryState.Failed)
                    e.Row.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        public List<FileRepositoryDTO> Files
        {
            set; get;
        }


        System.Windows.Forms.OpenFileDialog fileDialog;
        private void btnAddFiles_Click(object sender, RoutedEventArgs e)
        {
            if (fileDialog == null)
                fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Filter = GetExtensions();
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    foreach (var selectedfile in fileDialog.FileNames)
                    {
                        var file = new FileRepositoryDTO();
                        file.FileName = System.IO.Path.GetFileNameWithoutExtension(selectedfile);
                        file.FileExtension = System.IO.Path.GetExtension(selectedfile).Replace(".", "");
                        file.tmpPath = selectedfile;
                        file.Content = System.IO.File.ReadAllBytes(selectedfile);
                        //بهتره با ایونت به ادیتور فرستاده شود که اونجا هم تصمیم گیری شود
                        Files.Add(file);
                    }
                    dtgFiles.ItemsSource = null;
                    dtgFiles.ItemsSource = Files;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    break;
            }
        }
        public List<Tuple<string, List<string>>> Extentions
        {
            set; get;
        }

        public List<ArchiveFolderWithNullDTO> Folders
        {
            set
            {
                cmbFolder.SelectedValuePath = "ID";
                cmbFolder.DisplayMemberPath = "Name";
                cmbFolder.ItemsSource = value;
            }
        }

        public int? SelectedFolder
        {
            get
            {
                return (int?)cmbFolder.SelectedValue;
            }

            set
            {
                cmbFolder.SelectedValue = value;
            }
        }

        private string GetExtensions()
        {
            string result = "";
            foreach (var item in Extentions)
            {
                var str = item.Item1;
                var childs1 = "";
                var childs2 = "";
                foreach (var child in item.Item2)
                {
                    childs1 += (childs1 == "" ? "" : ",") + child;
                    childs2 += (childs2 == "" ? "" : ";") + child;
                }
                if (childs1 != "")
                {
                    str += "(" + childs1 + ")" + "|" + childs2;
                    result += (result == "" ? "" : "|") + str;
                }
            }
            return result;
            // "Text file (*.txt,*.aaa)|*.txt;*.aaa|C# file (*.cs)|*.cs";
        }
        private void btnClearFiles_Click(object sender, RoutedEventArgs e)
        {
            ClearItems();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (FilesConfirmed != null)
                FilesConfirmed(this, null);
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {

        }

        public void RefreshFiles()
        {
            dtgFiles.ItemsSource = null;
            dtgFiles.ItemsSource = Files;
        }

        public void ClearItems()
        {
            Files = new List<FileRepositoryDTO>();
            dtgFiles.ItemsSource = null;
            dtgFiles.ItemsSource = Files;
        }
    }

}
