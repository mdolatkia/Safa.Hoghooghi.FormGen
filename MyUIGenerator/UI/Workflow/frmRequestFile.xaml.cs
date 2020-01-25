using MyUILibrary.EntityArea;
using MyUILibrary.WorkflowArea;

using ProxyLibrary.Workflow;
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
using System.Windows.Shapes;

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for frmRequestAction.xaml
    /// </summary>
    public partial class frmRequestFile : UserControl, I_View_RequestFile
    {
        //public event EventHandler RequestActionUpdated;

        public frmRequestFile()
        {
            InitializeComponent();
            grdExistingFile.Visibility = Visibility.Collapsed;
            grdAddFile.Visibility = Visibility.Visible;
        }

        public event EventHandler<RequestFileConfirmedArg> RequestFileConfirmed;
        public event EventHandler<RequestFileSelectedArg> RequestFileSelected;
        public event EventHandler RequestFileClear;
        public event EventHandler CloseRequested;

        RequestFileDTO requestFileMessage;

        public int RequestID
        {
            set; get;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (requestFileMessage == null)
                requestFileMessage = new RequestFileDTO();
            requestFileMessage.FileName = System.IO.Path.GetFileName(txtFilePath.Text);
            requestFileMessage.FileDesc = txtFileDesc.Text;
            requestFileMessage.MIMEType = "";
            requestFileMessage.FileContent = System.IO.File.ReadAllBytes(txtFilePath.Text);
            if (RequestFileConfirmed != null)
                RequestFileConfirmed(this, new RequestFileConfirmedArg() { RequestFile = requestFileMessage });
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (CloseRequested != null)
                CloseRequested(this, null);
        }

        public void ShowRequestFile(RequestFileDTO requestFile)
        {
            requestFileMessage = requestFile;
            if (requestFileMessage.ID == 0)
            {
                grdExistingFile.Visibility = Visibility.Collapsed;
                grdAddFile.Visibility = Visibility.Visible;
                txtFilePath.Text = "";
            }
            else
            {
                grdExistingFile.Visibility = Visibility.Visible;
                grdAddFile.Visibility = Visibility.Collapsed;
                txtFileName.Text = requestFileMessage.FileName;
            }
            txtFileDesc.Text = requestFileMessage.FileDesc;
        }

        private void dtgFiles_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            var selectedFile = dtgFiles.SelectedItem as RequestFileDTO;
            if (selectedFile != null)
            {
                if (RequestFileSelected != null)
                    RequestFileSelected(this, new RequestFileSelectedArg() { RequestFile = selectedFile });
            }
        }

        public void ShowRequestFiles(List<RequestFileDTO> requestFiles)
        {
            dtgFiles.ItemsSource = requestFiles;
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            if (RequestFileClear != null)
                RequestFileClear(this, null);
        }

        private void btnViewFile_Click(object sender, RoutedEventArgs e)
        {

        }
        System.Windows.Forms.OpenFileDialog fileDialog;
        private void btnChooseFile_Click(object sender, RoutedEventArgs e)
        {
            if (fileDialog == null)
                fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Filter =
          "PDF (*.pdf)|*.pdf";
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    txtFilePath.Text = fileDialog.FileName;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    break;
            }
        }
    }
}
