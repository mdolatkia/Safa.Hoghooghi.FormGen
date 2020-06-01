using Microsoft.Win32;
using ModelEntites;

using MyModelManager;
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
using Microsoft.Office.Interop.Word;
using MyCommonWPFControls;
using MyLetterGenerator;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmLetterTemplate.xaml
    /// </summary>
    /// 

    public partial class frmMainLetterTemplate : UserControl
    {
        LetterGenerator letterGenerator = new LetterGenerator();
        MainLetterTemplateDTO Message = null;
        int EntityID { set; get; }
        BizLetterTemplate bizLetterTemplate = new BizLetterTemplate();
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        BizFormula bizFormula = new BizFormula();
        TableDrivedEntityDTO Entity { set; get; }

        public frmMainLetterTemplate(int entityID, int letterTempleteID)
        {
            InitializeComponent();
            EntityID = entityID;
            Entity = bizTableDrivedEntity.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithRelationships);
            SetLetterTemplateTypes();
            ucFields.SetEntity(entityID);
            if (letterTempleteID == 0)
            {
                Message = new MainLetterTemplateDTO();
                grdViewFile.Visibility = Visibility.Collapsed;
                grdAddFile.Visibility = Visibility.Visible;
            }
            else
                GetLetterTepmplate(letterTempleteID);
        }

        private void SetLetterTemplateTypes()
        {
            cmbType.ItemsSource = bizLetterTemplate.GetLetterTemplateTypes();
        }


        private void GetLetterTepmplate(int letterTempleteID)
        {
            Message = bizLetterTemplate.GetMainLetterTepmplate(MyProjectManager.GetMyProjectManager.GetRequester(), letterTempleteID);
            ShowMessage();
        }

        private void ShowMessage()
        {
            txtName.Text = Message.Name;
            cmbType.SelectedItem = Message.Type;
            txtFilePath.Text = "";
            ucFields.SetFields(Message.EntityListViewID, Message.PlainFields, Message.RelationshipFields);
            if (Message.ID != 0)
            {
                grdViewFile.Visibility = Visibility.Visible;
                grdAddFile.Visibility = Visibility.Collapsed;
            }
            else
            {
                grdViewFile.Visibility = Visibility.Collapsed;
                grdAddFile.Visibility = Visibility.Visible;
            }
        }

        private void btnViewFile_Click(object sender, RoutedEventArgs e)
        {
            string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = path + @"\\test1.doc";
            File.WriteAllBytes(filePath, Message.Content);

            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document doc = new Microsoft.Office.Interop.Word.Document();

            doc = word.Documents.Open(filePath);
            doc.Activate();
            word.Visible = true;
            //foreach (Microsoft.Office.Interop.Word.ContentControl cc in doc.ContentControls)
            //{
            //    //cc.
            //    //MessageBox.Show(cc.Tag);
            //    //MessageBox.Show(cc.Title);
            //}
            //foreach (Microsoft.Office.Interop.Word.FormField field in doc.FormFields)
            //{
            //    switch (field.Name)
            //    {
            //        case "Name":
            //            field.Range.Text = "میثم دولت کیا";
            //            break;

            //        default:
            //            break;
            //    }
            //}

            //   doc.SaveAs2(@"N:\mehler\Ausgefuellt.docx");
            //doc.Save();
            //doc.Close();
            //word.Quit();
            //System.Diagnostics.Process.Start(@"N:\mehler\Ausgefuellt.docx");
        }

        private void btnDeleteFile_Click(object sender, RoutedEventArgs e)
        {
            Message.Content = null;
            grdViewFile.Visibility = Visibility.Collapsed;
            grdAddFile.Visibility = Visibility.Visible;
        }

        private void AddFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MSWord (*.docx,*.doc)|*.docx;*.doc|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                txtFilePath.Text = openFileDialog.FileName;
                GetFileds(txtFilePath.Text);

            }
        }

        private void GetFileds(string text)
        {

            string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = text;
            //File.WriteAllBytes(filePath, Message.Content);

            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document doc = new Microsoft.Office.Interop.Word.Document();

            doc = word.Documents.Open(filePath);
            doc.Activate();

            
            var fields = letterGenerator.GetFields(doc);
            Message.PlainFields = fields.Item1;
            Message.RelationshipFields = fields.Item2;
            
            ucFields.SetFields(-1, Message.PlainFields, Message.RelationshipFields);

            //  doc.Save();
            doc.Close();
            word.Quit();
        }



        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (grdAddFile.Visibility == Visibility.Visible)
            {
                if (txtFilePath.Text != "")
                {
                    Message.Content = File.ReadAllBytes(txtFilePath.Text);
                    Message.FileExtension = System.IO.Path.GetExtension(txtFilePath.Text).Replace(".", "");
                }
            }
            if (txtName.Text == "")
            {
                MyProjectManager.GetMyProjectManager.ShowMessage("عنوان مناسب تعریف نشده است");
                return;
            }
         if(cmbType.SelectedItem==null)
            {
                MyProjectManager.GetMyProjectManager.ShowMessage("نوع قالب تعیین نشده است");
                return;
            }
            if (Message.Content == null)
            {
                MyProjectManager.GetMyProjectManager.ShowMessage("فایل مناسب تعریف نشده است");
                return;
            }
            var listViewID = ucFields.GetListViewID();
            if (listViewID == 0)
            {
                MyProjectManager.GetMyProjectManager.ShowMessage("لیست نمایش مرتبط مشخص نشده است");
                return;
            }
                foreach (var item in Message.PlainFields.ToList())
            {
                if (item.EntityListViewColumnsID == 0 && item.FormulaID == 0)
                    Message.PlainFields.Remove(item);
            }
            foreach (var item in Message.RelationshipFields.ToList())
            {
                if (item.RelationshipTailID == 0)
                    Message.RelationshipFields.Remove(item);
            }
      
            Message.EntityListViewID = listViewID;
            Message.TableDrivedEntityID = EntityID;
            Message.Name = txtName.Text;
            Message.Type = (LetterTemplateType)cmbType.SelectedItem;
            bizLetterTemplate.UpdateMainLetterTemplate(Message);
            MyProjectManager.GetMyProjectManager.ShowMessage("اطلاعات ثبت شد");
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Message = new MainLetterTemplateDTO();
            ShowMessage();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            frmMainLetterTemplateSelect view = new MyProject_WPF.frmMainLetterTemplateSelect(EntityID);
            view.LetterTemplateSelected += View_LetterTemplateSelected;
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form");
        }

        private void View_LetterTemplateSelected(object sender, MainLetterTemplateSelectedArg e)
        {
            GetLetterTepmplate(e.LetterTemplateID);
        }
    }
}
