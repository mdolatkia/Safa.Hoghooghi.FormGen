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

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmLetterRelationshipRangeTemplate.xaml
    /// </summary>
    /// 

    public partial class frmPartialLetterTemplate : UserControl
    {
        PartialLetterTemplateDTO Message = null;

        BizLetterTemplate bizLetterTemplate = new BizLetterTemplate();
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        BizFormula bizFormula = new BizFormula();
        //TableDrivedEntityDTO Entity { set; get; }
        int EntityID { set; get; }
        List<RelationshipDTO> Relationships { set; get; }
        //int InternalLetterTemplate { set; get; }
        public event EventHandler<LetterRelationshipTemplateUpdatedArg> LetterRelationshipTemplateUpdated;

        //List<LetterTemplateFieldDTO> Fields { set; get; }

        List<LetterTemplatePlainFieldDTO> sentPlainfields { set; get; }
        List<LetterTemplateRelationshipFieldDTO> sentRelationshipfields { set; get; }

        public frmPartialLetterTemplate(int internalLetterTemplate, int entityID, List<LetterTemplatePlainFieldDTO> plainfields, List<LetterTemplateRelationshipFieldDTO> relationshipfields)
        {
            InitializeComponent();
            //Fields = fields;
            EntityID = entityID;
            //Entity = bizTableDrivedEntity.GetTableDrivedEntity(EntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithRelationships);
            ucFields.SetEntity(entityID);
            sentPlainfields = plainfields;
            sentRelationshipfields = relationshipfields;
            if (internalLetterTemplate == 0)
            {
                Message = new PartialLetterTemplateDTO();
                Message.PlainFields = sentPlainfields;
                Message.RelationshipFields = sentRelationshipfields;
                ucFields.SetFields(0, Message.PlainFields, Message.RelationshipFields);
            }
            else
                GetLetterRelationshipTepmplate(internalLetterTemplate);
        }




        private void GetLetterRelationshipTepmplate(int letterTempleteID)
        {
            Message = bizLetterTemplate.GetPartialLetterTepmplate(MyProjectManager.GetMyProjectManager.GetRequester(), letterTempleteID);
            ShowMessage();
        }

        private void ShowMessage()
        {
            txtName.Text = Message.Name;

            if (sentPlainfields != null)
            {
                foreach(var item in sentPlainfields)
                {
                    if (!Message.PlainFields.Any(x=>x.FieldName==item.FieldName))
                    {
                        Message.PlainFields.Add(item);
                    }
                }
            }

            if (sentRelationshipfields != null)
            {
                foreach (var item in sentRelationshipfields)
                {
                    if (!Message.RelationshipFields.Any(x => x.FieldName == item.FieldName))
                    {
                        Message.RelationshipFields.Add(item);
                    }
                }
            }
            ucFields.SetFields(Message.EntityListViewID, Message.PlainFields, Message.RelationshipFields);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (txtName.Text == "")
            {
                MyProjectManager.GetMyProjectManager.ShowMessage("عنوان مناسب تعریف نشده است");
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
            var id = bizLetterTemplate.UpdatePartialLetterTemplate(Message);
            if (LetterRelationshipTemplateUpdated != null)
                LetterRelationshipTemplateUpdated(this, new MyProject_WPF.LetterRelationshipTemplateUpdatedArg() { ID = id });
            //MyProjectManager.GetMyProjectManager.ShowMessage("اطلاعات ثبت شد");
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Message = new PartialLetterTemplateDTO();
            ShowMessage();
        }

        //private void btnSearch_Click(object sender, RoutedEventArgs e)
        //{

        //    frmLetterRelationshipTemplateSelect view = new MyProject_WPF.frmLetterRelationshipTemplateSelect(rela);
        //    view.LetterRelationshipTemplateSelected += View_LetterRelationshipTemplateSelected;
        //       MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form");
        //}

        //private void View_LetterRelationshipTemplateSelected(object sender, LetterRelationshipTemplateSelectedArg e)
        //{
        //    GetLetterRelationshipTepmplate(e.ID);
        //}
    }
    public class LetterRelationshipTemplateUpdatedArg : EventArgs
    {
        public int ID { set; get; }
    }
}
