using ModelEntites;
using MyCommonWPFControls;

using MyModelManager;
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
using Telerik.Windows.Controls;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for UC_LetterTemplateFields.xaml
    /// </summary>
    public partial class UC_LetterTemplateFields : UserControl
    {
        BizEntityListView bizEntityListView = new BizEntityListView();
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        BizFormula bizFormula = new BizFormula();
        BizLetterTemplate bizLetterTemplate = new BizLetterTemplate();
        BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
        // TableDrivedEntityDTO Entity { set; get; }
        List<EntityRelationshipTailDTO> RelationshipTails { set; get; }
        List<FormulaDTO> FormulaParameters { set; get; }
        int EntityID { set; get; }
        public UC_LetterTemplateFields()
        {
            InitializeComponent();

            //colRelationshipFilterd.SelectedValueMemberPath = "ID";
            //colRelationshipFilterd.DisplayMemberPath = "Name";
            lokEntityListView.SelectionChanged += LokEntityListView_SelectionChanged;
            colRelationshipTail.NewItemEnabled = true;
            colRelationshipTail.EditItemEnabled = true;
            colRelationshipTail.EditItemClicked += ColRelationshipTail_EditItemClicked;

            colPartialLetterTemplates.DisplayMemberPath = "Name";
            colPartialLetterTemplates.SelectedValueMemberPath = "ID";
            colPartialLetterTemplates.NewItemEnabled = true;
            colPartialLetterTemplates.EditItemEnabled = true;
            colPartialLetterTemplates.EditItemClicked += colPartialLetterTemplates_EditItemClicked;
            dtgRelationshipFields.CellEditEnded += DtgFields_CellEditEnded;
        }
        EntityListViewDTO SelectedEntityListView { set; get; }

        private void LokEntityListView_SelectionChanged(object sender, SelectionChangedArg e)
        {
            if (lokEntityListView.SelectedItem != null)
            {
                BizEntityListView biz = new BizEntityListView();
                SelectedEntityListView = biz.GetEntityListView(MyProjectManager.GetMyProjectManager.GetRequester(), (int)lokEntityListView.SelectedValue);
                SetColumns(SelectedEntityListView.EntityListViewAllColumns);
                SetDefaultFieldsValue(dtgPlainFields.ItemsSource as List<LetterTemplatePlainFieldDTO>, dtgRelationshipFields.ItemsSource as List<LetterTemplateRelationshipFieldDTO>);
            }
            else
                SelectedEntityListView = null;
        }
        private void SetColumns(List<EntityListViewColumnsDTO> columns)
        {
            colColumns.DisplayMemberPath = "Column.Alias";
            colColumns.SelectedValueMemberPath = "ID";
            colColumns.ItemsSource = columns;
        }

        private void ColRelationshipTail_EditItemClicked(object sender, EditItemClickEventArg e)
        {
            frmEntityRelationshipTail view = new frmEntityRelationshipTail(EntityID);
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "رابطه", Enum_WindowSize.Big);
            view.ItemSelected += (sender1, e1) => View_ItemSelected(sender1, e1, (sender as MyStaticLookup));

        }

        private void View_ItemSelected(object sender, EntityRelationshipTailSelectedArg e, MyStaticLookup lookup)
        {
            SetRelationshipTails();
            lookup.SelectedValue = e.EntityRelationshipTailID;
        }

        public void SetEntity(int entityID)
        {
            EntityID = entityID;
            //  Entity = bizTableDrivedEntity.GetTableDrivedEntity(EntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithRelationships);
            //SetColumns();
            SetParameters();
            SetRelationshipTails();
            SetEntityListViews();
        }
        private void SetEntityListViews()
        {
            if (lokEntityListView.ItemsSource == null)
            {
                lokEntityListView.EditItemClicked += LokEntityListView_EditItemClicked;
            }

            lokEntityListView.DisplayMember = "Title";
            lokEntityListView.SelectedValueMember = "ID";
            lokEntityListView.ItemsSource = bizEntityListView.GetEntityListViews(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID);
        }
        private void LokEntityListView_EditItemClicked(object sender, MyCommonWPFControls.EditItemClickEventArg e)
        {
            var lookup = (sender as MyStaticLookup);
            frmEntityListView view;
            if (lookup.SelectedItem == null)
            {
                view = new frmEntityListView(EntityID, 0);
            }
            else
            {
                view = new frmEntityListView(EntityID, (int)lookup.SelectedValue);
            }
            view.EntityListViewUpdated += (sender1, e1) => View_EntityListViewUpdated(sender1, e1, lookup);
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "تنظیمات نامه", Enum_WindowSize.Big);
        }
        private void View_EntityListViewUpdated(object sender, EntityListViewUpdatedArg e, MyStaticLookup lookup)
        {
            SetEntityListViews();
            lookup.SelectedValue = e.ID;
        }
        public void SetFields(int listViewID, List<LetterTemplatePlainFieldDTO> plainfields, List<LetterTemplateRelationshipFieldDTO> relationshipfields)
        {
            if (listViewID != -1)
                lokEntityListView.SelectedValue = listViewID;
            SetDefaultFieldsValue(plainfields, relationshipfields);
            dtgPlainFields.ItemsSource = null;
            dtgPlainFields.ItemsSource = plainfields;
            dtgRelationshipFields.ItemsSource = null;
            dtgRelationshipFields.ItemsSource = relationshipfields;
            foreach (var field in relationshipfields)
            {
                if (field.RelationshipTailID != 0 && field.tmpPartialLetterTemplates.Count == 0)
                {
                    SetLetterRelationshipTemplates(field);
                }
            }

            //if (!plainfields.Any(x => x.EntityListViewColumnsID != 0 || x.FormulaID != 0))
            //{
            //    if(lokEntityListView.SelectedItem!=null)
            //    {
            //        SetDefaultFieldsValue()
            //    }
            //}
        }



        private void SetDefaultFieldsValue(List<LetterTemplatePlainFieldDTO> plainfields, List<LetterTemplateRelationshipFieldDTO> relationshipfields)
        {

            if (plainfields != null)
            {
                foreach (var field in plainfields)
                {
                    if (field.EntityListViewColumnsID == 0 && field.FormulaID == 0)
                    {
                        if (field.FieldName.ToLower().StartsWith("col_"))
                        {
                            if (SelectedEntityListView != null)
                            {
                                var columnName = field.FieldName.Split('_')[1];
                                //برای الیاس ها هم فکری شود
                                var column = SelectedEntityListView.EntityListViewAllColumns.FirstOrDefault(x => x.Alias!=null && x.Alias.ToLower() == columnName.ToLower());
                                if (column != null)
                                    field.EntityListViewColumnsID = column.ID;
                            }
                        }
                        else if (field.FieldName.ToLower().StartsWith("prm_"))
                        {
                            if (FormulaParameters != null)
                            {
                                //var paramters = bizFormula.GetFormulas(EntityID);
                                var parameterName = field.FieldName.Split('_')[1];
                                var parameter = FormulaParameters.FirstOrDefault(x => x.Name.ToLower() == parameterName.ToLower());
                                if (parameter != null)
                                    field.FormulaID = parameter.ID;
                            }
                        }
                    }
                }
            }
            if (relationshipfields != null)
            {
                foreach (var field in relationshipfields)
                {
                    if (field.PartialLetterTemplateID == 0)
                    {
                        if (RelationshipTails != null)
                        {
                            var relName = field.FieldName.Split('_')[1];
                            var relationshipTail = RelationshipTails.FirstOrDefault(x => x.EntityPath.ToLower() == relName.ToLower());
                            if (relationshipTail != null && field.tmpPartialLetterTemplates.Count == 0)
                            {
                                field.RelationshipTailID = relationshipTail.ID;
                                SetLetterRelationshipTemplates(field);
                                //SetEntityPreDefinedSearches(field);
                                //var firstTemplate = field.tmpPartialLetterTemplates.FirstOrDefault();
                                //if (firstTemplate != null)
                                //    field.PartialLetterTemplateID = firstTemplate.ID;
                            }
                        }
                    }
                }
            }

        }

        internal int GetListViewID()
        {
            if (lokEntityListView.SelectedItem == null)
                return 0;
            else
                return (int)lokEntityListView.SelectedValue    ;
        }

        private void SetLetterRelationshipTemplates(LetterTemplateRelationshipFieldDTO field)
        {
            if (field.RelationshipTailID != 0)
            {
                if (field.RelationshipTail == null || field.RelationshipTail.ID != field.RelationshipTailID)
                    field.RelationshipTail = bizEntityRelationshipTail.GetEntityRelationshipTail(MyProjectManager.GetMyProjectManager.GetRequester(), field.RelationshipTailID);
                field.tmpPartialLetterTemplates = bizLetterTemplate.GetPartialLetterTemplates(MyProjectManager.GetMyProjectManager.GetRequester(), field.RelationshipTail.TargetEntityID);
            }
            else
            {
                field.tmpPartialLetterTemplates = new List<PartialLetterTemplateDTO>();
                field.RelationshipTail = null;
            }
        }
        //private void SetEntityPreDefinedSearches(LetterTemplateRelationshipFieldDTO field)
        //{
        //    if (field.RelationshipTailID != 0)
        //    {
        //        //var relationshipTail = bizEntityRelationshipTail.GetEntityRelationshipTail(field.RelationshipTailID);
        //        //field.tmpEntityPreDefinedSearches = bizLetterTemplate.GetInternalLetterTemplates(relationshipTail.TargetEntityID);
        //    }
        //    else
        //        field.tmpEntityPreDefinedSearches = new List<EntityPreDefinedSearchDTO>();
        //}
        //private void SetRelationshipFiltered(LetterTemplateRelationshipFieldDTO field)
        //{
        //    var relationship = Entity.Relationships.FirstOrDefault(x => x.ID == field.RelationshipID);
        //    if (relationship != null)
        //    {
        //        //field.tmpRelatoinshipsFiltered = bizRelationshipFilter.GetRelationshipFilters(field.RelationshipID);
        //    }
        //}
        private void SetRelationshipTails()
        {
            RelationshipTails = bizEntityRelationshipTail.GetEntityRelationshipTails(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID);
            colRelationshipTail.ItemsSource = RelationshipTails;
            colRelationshipTail.SelectedValueMemberPath = "ID";
            colRelationshipTail.DisplayMemberPath = "EntityPath";

        }
        private void SetParameters()
        {
            colParameters.DisplayMemberPath = "Name";
            colParameters.SelectedValueMemberPath = "ID";
            FormulaParameters = bizFormula.GetFormulas(EntityID);
            FormulaParameters.Add(new FormulaDTO() { ID = 0, Name = " " });
            colParameters.ItemsSource = FormulaParameters;
        }
        //private void SetColumns()
        //{
        //    colColumns.DisplayMemberPath = "Name";
        //    colColumns.SelectedValueMemberPath = "ID";
        //    Entity.Columns.Add(new ColumnDTO() { ID = 0, Name = " " });
        //    colColumns.ItemsSource = Entity.Columns;
        //}

        private void colPartialLetterTemplates_EditItemClicked(object sender, MyCommonWPFControls.EditItemClickEventArg e)
        {
            frmPartialLetterTemplate view;
            var selectedField = e.DataConext as LetterTemplateRelationshipFieldDTO;
            if (selectedField != null && selectedField.RelationshipTail != null)
            {
                if ((sender as MyStaticLookup).SelectedItem == null)
                    view = new MyProject_WPF.frmPartialLetterTemplate(0, selectedField.RelationshipTail.TargetEntityID, selectedField.PartialLetterTemplate.PlainFields, selectedField.PartialLetterTemplate.RelationshipFields);
                else
                {
                    var id = ((sender as MyStaticLookup).SelectedItem as PartialLetterTemplateDTO).ID;
                    view = new MyProject_WPF.frmPartialLetterTemplate(id, selectedField.RelationshipTail.TargetEntityID, selectedField.PartialLetterTemplate.PlainFields, selectedField.PartialLetterTemplate.RelationshipFields);
                }
                MyProjectManager.GetMyProjectManager.ShowDialog(view, "قالب نامه", Enum_WindowSize.Big);
                view.LetterRelationshipTemplateUpdated += (sender1, e1) => View_LetterRelationshipTemplateUpdated(sender1, e1, selectedField, (sender as MyStaticLookup));
            }
        }
        private void View_LetterRelationshipTemplateUpdated(object sender, LetterRelationshipTemplateUpdatedArg e, LetterTemplateRelationshipFieldDTO relatedField, MyStaticLookup lookup)
        {
            SetLetterRelationshipTemplates(relatedField);
            lookup.SelectedValue = e.ID;
        }

        private void DtgFields_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            if (e.Cell.Column.Name == "colRelationshipTail")
            {
                var field = e.Cell.DataContext as LetterTemplateRelationshipFieldDTO;
                if (field != null)
                {
                    if (field.RelationshipTailID != 0)
                    {
                        SetLetterRelationshipTemplates(field);
                        //SetEntityPreDefinedSearches(field);
                    }
                }
            }
        }





    }
}
