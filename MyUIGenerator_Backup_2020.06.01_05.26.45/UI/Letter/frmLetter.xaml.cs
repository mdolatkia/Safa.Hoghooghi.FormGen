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
using System.IO;

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for frmLetter.xaml
    /// </summary>
    public partial class frmLetter : UserControl, I_View_LetterArea
    {
        LetterDTO message;
        public frmLetter()
        {
            InitializeComponent();
        }


        //  public event EventHandler SelectFileClicked;
        public event EventHandler<LetterGenerateArg> GenerateFileClicked;
        public event EventHandler<LetterConfirmedArg> LetterConfirmed;
        public event EventHandler NewClicked;
        public event EventHandler ShowFileClicked;
        public event EventHandler DownloadFileClicked;
        public event EventHandler ShowExternalFileClicked;
        //public event EventHandler<LetterInternalExternalArg> InternalOrExternalClicked;
        public event EventHandler<LetterFileSelectGenerateArg> GenerateOrSelectClicked;
        public event EventHandler<LetterInternalExternalArg> ExternalOrInternalClicked;
        public event EventHandler<LetterExternalInfoRequestedArg> ExternalInfoRequested;
        public event EventHandler ConvertToExternalClicked;
        public event EventHandler ShowGeneratedFileClicked;
        public event EventHandler CloseRequested;

        private void optSelective_Checked(object sender, RoutedEventArgs e)
        {
            if (GenerateOrSelectClicked != null)
                GenerateOrSelectClicked(this, new LetterFileSelectGenerateArg() { GenerateOrSelect = optGenerate.IsChecked == true });
            //////if ()
            //////{
            //////    lblGenerateFile.Visibility = Visibility.Visible;
            //////    grdGenerateFile.Visibility = Visibility.Visible;
            //////    grdSelectFile.Visibility = Visibility.Collapsed;
            //////    lblSelectFile.Visibility = Visibility.Collapsed;
            //////}
            //////else if (optSelective.IsChecked == true)
            //////{
            //////    lblGenerateFile.Visibility = Visibility.Collapsed;
            //////    grdGenerateFile.Visibility = Visibility.Collapsed;
            //////    grdSelectFile.Visibility = Visibility.Visible;
            //////    lblSelectFile.Visibility = Visibility.Visible;
            //////}
        }
        System.Windows.Forms.OpenFileDialog fileDialog;
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (fileDialog == null)
                fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Filter = GetExtensions();
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
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            if (lokLetterTemplate.SelectedItem != null)
                if (GenerateFileClicked != null)
                    GenerateFileClicked(this, new LetterGenerateArg() { LetterTemplate = lokLetterTemplate.SelectedItem as MainLetterTemplateDTO });
        }


        public List<MainLetterTemplateDTO> LetterTemplates
        {
            set
            {
                lokLetterTemplate.DisplayMember = "Name";
                lokLetterTemplate.SelectedValueMember = "ID";
                lokLetterTemplate.ItemsSource = value;
            }
        }

        public List<LetterTypeDTO> LetterTypes
        {
            set
            {
                cmbType.SelectedValuePath = "ID";
                cmbType.DisplayMemberPath = "Name";
                cmbType.ItemsSource = value;
            }
        }

        public List<Tuple<string, List<string>>> Extentions
        {
            set; get;
        }

        public bool ExternalInfoPanelVisibility
        {
            get
            {
                return grdLetterInfoFromExternalSource.Visibility == Visibility.Visible;
            }
            set
            {
                grdLetterInfoFromExternalSource.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);
            }
        }

        //public bool InternalPanelVisibility
        //{
        //    get
        //    {
        //        return tabInternal.Visibility == Visibility.Visible;
        //    }

        //    set
        //    {
        //        tabInternal.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);
        //        if (value)
        //            tabInternal.IsSelected = true;
        //    }
        //}
        //public bool ConvertToExternalPanelVisibility
        //{
        //    get
        //    {
        //        return grdConvertToExternal.Visibility == Visibility.Visible;
        //    }

        //    set
        //    {
        //        grdConvertToExternal.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);
        //    }
        //}
        public bool ViewExistingFilePanelVisibility
        {
            get
            {
                return grdExistingFile.Visibility == Visibility.Visible;
            }

            set
            {
                lblShowFile.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);
                grdExistingFile.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);
            }
        }
        public bool ExternalCodeDisabled
        {
            get
            {
                return !txtExternalCode.IsEnabled;
            }

            set
            {
                txtExternalCode.IsEnabled = !value;
            }
        }

        public bool ViewExternalFilePanelVisibility
        {
            get
            {
                return grdExternalFile.Visibility == Visibility.Visible;
            }

            set
            {
                lblExternalFile.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);
                grdExternalFile.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);
            }
        }

        public bool FileGenerationPanelVisibility
        {
            get
            {
                return grdGenerateFile.Visibility == Visibility.Visible;
            }

            set
            {
                grdGenerateFile.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);
            }
        }

        public bool FileSelectionPanelVisibility
        {
            get
            {
                return grdSelectFile.Visibility == Visibility.Visible;
            }

            set
            {
                grdSelectFile.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);
            }
        }

        public bool ExternalInternalPanelEnabled
        {
            get
            {
                return pnlInternalExternal.IsEnabled;
            }

            set
            {
                //lblInternalExternal.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);
                pnlInternalExternal.IsEnabled = value;
            }
        }

        public bool GenerateOrSelectPanelVisible
        {
            get
            {
                return grdSelectOrGenerateFile.Visibility == Visibility.Visible;
            }

            set
            {
                grdSelectOrGenerateFile.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);
            }
        }

        public bool ConvertToExternalVisibility
        {
            get
            {
                return btnConvertToExternal.Visibility == Visibility.Visible;
            }

            set
            {
                btnConvertToExternal.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);
            }
        }

        public bool ShowGeneretedFileVisibility
        {
            get
            {
                return btnOpenGeneratedFile.Visibility == Visibility.Visible;
            }

            set
            {
                btnOpenGeneratedFile.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);
            }
        }

        public bool CreateDateAndUserVisibility
        {
            get
            {
                return txtCreateUser.Visibility == Visibility.Visible;
            }
            set
            {
                txtCreateDate.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);
                lblCreateDate.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);
                txtCreateUser.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);
                lblCreateUser.Visibility = (value ? Visibility.Visible : Visibility.Collapsed);
            }
        }

        public void UpdateMessage()
        {
            //if (message == null)
            //    message = new LetterDTO();
            message.Title = txtTitle.Text;
            message.Desc = txtDesc.Text;
            message.LetterDate = DateTime.Now;// txtTitle.LetterDate;
            message.ExternalCode = txtExternalCode.Text;
            message.LetterNumber = txtNumber.Text;

            if (optExternal.IsChecked == true)
            {
                message.IsExternalOrInternal = true;
                message.IsGeneratedOrSelected = null;
            }
            else if (optInternal.IsChecked == true)
            {
                optInternal.IsChecked = true;

                if (optGenerate.IsChecked == true)
                    message.IsGeneratedOrSelected = true;
                else if (optSelective.IsChecked == true)
                    message.IsGeneratedOrSelected = false;
                else
                    message.IsGeneratedOrSelected = null;
            }

            //if (lokRelatedLetter.SelectedItem != null)
            //    message.RelatedLetterID = (int)lokRelatedLetter.SelectedValue;
            if (cmbType.SelectedItem != null)
                message.LetterTypeID = (int)cmbType.SelectedValue;

            if (optInternal.IsChecked == true)
            {
                string filePath = "";
                if (optGenerate.IsChecked == true)
                {
                    if (lokLetterTemplate.SelectedItem != null)
                        message.LetterTemplateID = (int)lokLetterTemplate.SelectedValue;
                    filePath = txtGenerateFilePath.Text;
                }
                else if (optSelective.IsChecked == true)
                    filePath = txtFilePath.Text;

                if (filePath != "")
                {
                    message.AttechedFile = new FileRepositoryDTO();
                    message.AttechedFile.FileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
                    message.AttechedFile.FileExtension = System.IO.Path.GetExtension(filePath).Replace(".", "");
                    message.AttechedFile.Content = System.IO.File.ReadAllBytes(filePath);
                }
            }
        }
        public void ShowMessage(LetterDTO letter)
        {
            message = letter;
            txtTitle.Text = message.Title;
            txtNumber.Text = message.LetterNumber;
            txtDate.SelectedDate = message.LetterDate;
            txtDesc.Text = message.Desc;
            txtCreateDate.SelectedDate = message.CreationDate;
            txtCreateUser.Text = message.vwUser;
            txtExternalCode.Text = message.ExternalCode;
            cmbType.SelectedValue = message.LetterTypeID;
            //lokRelatedLetter.SelectedValue = message.RelatedLetterID;
            if (message.IsExternalOrInternal)
                optExternal.IsChecked = true;
            else
                optInternal.IsChecked = true;
            if (message.IsGeneratedOrSelected == true)
                optGenerate.IsChecked = true;
            else if (message.IsGeneratedOrSelected == false)
                optSelective.IsChecked = true;
            else
            {
                optGenerate.IsChecked = false;
                optSelective.IsChecked = false;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (LetterConfirmed != null)
                LetterConfirmed(this, new LetterConfirmedArg());
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            if (NewClicked != null)
                NewClicked(this, null);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (CloseRequested != null)
                CloseRequested(this, null);
        }

        //public void MakeGenerateModeHidden()
        //{
        //    optGenerate.Visibility = Visibility.Collapsed;
        //    grdGenerateFile.Visibility = Visibility.Collapsed;
        //}

        //public void DisableFileAttachment()
        //{
        //    optGenerate.IsEnabled = false;
        //    optSelective.IsEnabled = false;
        //    grdGenerateFile.Visibility = Visibility.Collapsed;
        //    grdSelectFile.Visibility = Visibility.Collapsed;
        //    lblGenerateFile.Visibility = Visibility.Collapsed;
        //    lblSelectFile.Visibility = Visibility.Collapsed;
        //}
        public void ViewExistingFile(string fileName)
        {
            //grdExistingFile.Visibility = Visibility.Visible;
            //lblExistingFile.Visibility = Visibility.Visible;
            lblExistingFile.Text = fileName;
        }
        public void ViewExternalFileInfo(string fileName)
        {
            txtExternalFileName.Text = fileName;
        }
        private void btnShowFile_Click(object sender, RoutedEventArgs e)
        {
            if (message.AttechedFile != null)
            {

                if (ShowFileClicked != null)
                    ShowFileClicked(this, null);
            }
        }
        private void btnDownloadFile_Click(object sender, RoutedEventArgs e)
        {
            if (message.AttechedFile != null)
            {

                if (DownloadFileClicked != null)
                    DownloadFileClicked(this, null);
            }
        }
        //public void EnableFileAttachmentToDefault()
        //{
        //    optGenerate.IsEnabled = true;
        //    optSelective.IsEnabled = true;
        //    grdExistingFile.Visibility = Visibility.Collapsed;
        //    lblShowFile.Visibility = Visibility.Collapsed;
        //    optSelective.IsChecked = true;
        //    lblGenerateFile.Visibility = Visibility.Collapsed;
        //    grdGenerateFile.Visibility = Visibility.Collapsed;
        //    grdSelectFile.Visibility = Visibility.Visible;
        //    lblSelectFile.Visibility = Visibility.Visible;
        //}

        public GenerateResult ShowGeneratedFile(byte[] generatedFile, string fileName, string fileExtension)
        {
            GenerateResult result = new GenerateResult();
            try
            {
                string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string filePath = path + @"\\" + fileName + "." + fileExtension;
                File.WriteAllBytes(filePath, generatedFile);
                txtGenerateFilePath.Text = filePath;
                Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document doc = new Microsoft.Office.Interop.Word.Document();

                doc = word.Documents.Open(filePath);
                doc.Activate();
                word.Visible = true;
                result.Result = true;
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Exception = ex.Message;
            }
            return result;
            //doc.Save();
            //doc.Close();
            //word.Quit();
        }
        public GenerateResult ShowGeneratedFile()
        {
            GenerateResult result = new GenerateResult();

            try
            {
                if (txtGenerateFilePath.Text != "")
                {
                    Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
                    Microsoft.Office.Interop.Word.Document doc = new Microsoft.Office.Interop.Word.Document();

                    doc = word.Documents.Open(txtGenerateFilePath.Text);
                    doc.Activate();
                    word.Visible = true;
                    result.Result = true;
                }
                else
                {
                    result.Result = false;
                    result.Exception = "فایلی موجود نمی باشد";
                }
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.Exception = ex.Message;
            }
            return result;
        }
        public void OpenFile(byte[] content, string fileName)
        {
            string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = path + @"\\" + fileName;
            File.WriteAllBytes(filePath, content);
            System.Diagnostics.Process.Start(filePath);
        }

        private void optInternal_Checked(object sender, RoutedEventArgs e)
        {
            if (ExternalOrInternalClicked != null)
                ExternalOrInternalClicked(this, new LetterInternalExternalArg() { ExternalInternal = optExternal.IsChecked == true });
            //if (optInternal.IsChecked == true)
            //{
            //    tabExternal.Visibility = Visibility.Collapsed;
            //    lblExternalCode.Visibility = Visibility.Collapsed;
            //    grdExternalInfo.Visibility = Visibility.Collapsed;
            //    tabInternal.Visibility = Visibility.Visible;
            //    tabInternal.IsSelected = true;
            //}
            //else if (optInternal.IsChecked == true)
            //{
            //    tabExternal.IsSelected = true;
            //    tabExternal.Visibility = Visibility.Visible;
            //    lblExternalCode.Visibility = Visibility.Visible;
            //    grdExternalInfo.Visibility = Visibility.Visible;
            //    tabInternal.Visibility = Visibility.Collapsed;
            //}
        }

        private void btnExternalInfo_Click(object sender, RoutedEventArgs e)
        {
            if (txtExternalCode.Text != "")
                if (ExternalInfoRequested != null)
                    ExternalInfoRequested(this, new LetterExternalInfoRequestedArg() { ExternalCode = txtExternalCode.Text });
        }

        private void btnShowExternalFile_Click(object sender, RoutedEventArgs e)
        {
            if (message.AttechedFile != null)
            {
                if (ShowExternalFileClicked != null)
                    ShowExternalFileClicked(this, null);
            }
        }

        public void ClearSelectedFiles()
        {
            txtFilePath.Text = "";
            txtGenerateFilePath.Text = "";
        }

        private void btnConvertToExternal_Click(object sender, RoutedEventArgs e)
        {
            if (ConvertToExternalClicked != null)
                ConvertToExternalClicked(this, null);
        }

        public void AddRelatedLetterSelector(object control)
        {
            grdRelatedLetter.Children.Add(control as UIElement);
        }

        private void btnOpenGeneratedFile_Click(object sender, RoutedEventArgs e)
        {
            if (ShowGeneratedFileClicked != null)
                ShowGeneratedFileClicked(this, null);
        }


    }
}
