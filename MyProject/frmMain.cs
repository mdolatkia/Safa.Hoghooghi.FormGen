using DataAccess;
using MyRuleEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Telerik.WinControls;
namespace MyProject
{
    public partial class frmMain : Form
    {
        FormHelper MyFormHelper { set; get; }
        public frmMain()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(Properties.Settings.Default.LastDatabase))
                txtDBName.Text = Properties.Settings.Default.LastDatabase;
            MyFormHelper = new FormHelper(this);

            dtgTables.SelectionChanged += dtgTables_SelectionChanged;
            dtgColumns.SelectionChanged += dtgColumns_SelectionChanged;
            dtgColumnKeyValue.AllowAddNewRow = true;
            dtgColumnKeyValue.AddNewRowPosition = SystemRowPosition.Bottom;
            dtgColumnKeyValue.AllowDeleteRow = true;

            dtgISARelationship.SelectionChanged += dtgISARelationship_SelectionChanged;
            dtgUnionRelationshipType.SelectionChanged += dtgUnionRelationshipType_SelectionChanged;
            dtgManyToMany.SelectionChanged += dtgManyToMany_SelectionChanged;

            dtgRuleEntity.ContextMenuOpening += dtgRuleEntity_ContextMenuOpening;


            dtgColumns.ContextMenuOpening += dtgColumns_ContextMenuOpening;
            dtgRuleRelationships.ContextMenuOpening += dtgRuleRelationships_ContextMenuOpening;
            dtgOneToMany.ContextMenuOpening += dtgOneToMany_ContextMenuOpening;
            dtgManyToOne.ContextMenuOpening += dtgManyToOne_ContextMenuOpening;
            dtgExplicit.ContextMenuOpening += dtgExplicit_ContextMenuOpening;
            dtgImplicit.ContextMenuOpening += dtgImplicit_ContextMenuOpening;
            dtgManyToMany.ContextMenuOpening += dtgManyToMany_ContextMenuOpening;
            //dtgManyToMany_ManyToOne.ContextMenuOpening += dtgManyToMany_ManyToOne_ContextMenuOpening;
            dtgSuperToSub.ContextMenuOpening += dtgSuperToSub_ContextMenuOpening;
            dtgSubToSuper.ContextMenuOpening += dtgSubToSuper_ContextMenuOpening;
            dtgUnionToSubUnion.ContextMenuOpening += dtgUnionToSubUnion_ContextMenuOpening;
            dtgSubUnionToUnion.ContextMenuOpening += dtgSubUnionToUnion_ContextMenuOpening;
            dtgISARelationship.ContextMenuOpening += dtgISARelationship_ContextMenuOpening;
            dtgUnionRelationshipType.ContextMenuOpening += dtgUnionRelationship_ContextMenuOpening;
            //dtgRuleEntity.CommandCellClick += dtgRuleEntity_CommandCellClick;
            //dtgRuleEntity.CellFormatting += dtgRuleEntity_CellFormatting;
            SetGridViewColumns();
        }

        public string ServerName
        {
            get { return txtServerName.Text; }
        }

        public string DatabaseName
        {
            get { return txtDBName.Text; }
        }





        void dtgColumns_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
            e.ContextMenu.Items.Add(separator);
            var entity = dtgColumns.CurrentRow.DataBoundItem as Column;
            if (entity != null)
            {
                if (MyFormHelper.GetTypeOfColumn(entity.ID) == enum_ColumnType.StringColumnType)
                {
                    RadMenuItem customMenuItem1 = new RadMenuItem();
                    customMenuItem1.Text = "Convert to date column type";
                    customMenuItem1.Name = "ConvertToDateColumnType";
                    customMenuItem1.Click += (sender1, EventArgs) => ConvertToDateColumnType_Click1(sender, e, entity.ID);
                    e.ContextMenu.Items.Add(customMenuItem1);
                }
                else if (MyFormHelper.GetTypeOfColumn(entity.ID) == enum_ColumnType.DataColumnType)
                {
                    RadMenuItem customMenuItem = new RadMenuItem();
                    customMenuItem.Text = "Convert to string column type";
                    customMenuItem.Name = "ConvertToStringColumnType";
                    customMenuItem.Click += (sender1, EventArgs) => ConvertToStringColumnType_Click1(sender, e, entity.ID);
                    e.ContextMenu.Items.Add(customMenuItem);


                }
                RadMenuItem customMenuItemRule = new RadMenuItem();
                customMenuItemRule.Text = "Edit Rule on value";
                customMenuItemRule.Name = "ColumnRuleOnValue";
                customMenuItemRule.Click += (sender1, EventArgs) => ConvertToDateColumnType_Click1RuleOnValue(sender, e, entity.ID);
                e.ContextMenu.Items.Add(customMenuItemRule);
            }
        }
        void ConvertToStringColumnType_Click1(object sender, EventArgs e, int columnID)
        {
            MyFormHelper.ConvertToStringColumnType(columnID);
        }
        void ConvertToDateColumnType_Click1(object sender, EventArgs e, int columnID)
        {
            MyFormHelper.ConvertToDateColumnType(columnID);
        }
        void ConvertToDateColumnType_Click1RuleOnValue(object sender, EventArgs e, int columnID)
        {
            MyFormHelper.EditColumnColumnValue(columnID);
        }
        void dtgRuleEntity_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
            e.ContextMenu.Items.Add(separator);
            var entity = dtgRuleEntity.CurrentRow.DataBoundItem as EntityDTO;
            if (entity != null)
            {

                RadMenuItem customMenuItem = new RadMenuItem();
                customMenuItem.Text = "Duplicate Entity with inheritance relationship";
                customMenuItem.Name = "DuplicateEntity_inheritance";
                customMenuItem.Click += (sender1, EventArgs) => customMenuItem_Click1(sender, e, entity);
                e.ContextMenu.Items.Add(customMenuItem);

                RadMenuItem customMenuItem1 = new RadMenuItem();
                customMenuItem1.Text = "Duplicate Entity";
                customMenuItem1.Name = "DuplicateEntity";
                customMenuItem1.Click += (sender1, EventArgs) => customMenuItem_Click11(sender, e, entity);
                e.ContextMenu.Items.Add(customMenuItem1);

                RadMenuItem customMenuItemArc = new RadMenuItem();
                customMenuItemArc.Text = "Define Arc Relationships";
                customMenuItemArc.Name = "ArcRelatoinships";
                customMenuItemArc.Click += (sender1, EventArgs) => customMenuItem_ClickArc(sender, e, entity);
                e.ContextMenu.Items.Add(customMenuItemArc);
            }
        }

        void customMenuItem_Click1(object sender, EventArgs e, EntityDTO entity)
        {
            MyFormHelper.DuplicateEntity(entity, true);
        }
        void customMenuItem_Click11(object sender, EventArgs e, EntityDTO entity)
        {
            MyFormHelper.DuplicateEntity(entity, false);
        }
        void customMenuItem_ClickArc(object sender, EventArgs e, EntityDTO entity)
        {
            MyFormHelper.DefineArcRelationships(entity);
        }

        void dtgRuleRelationships_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            AddConvertRelationshipContextMenu(e, dtgRuleRelationships.CurrentRow.DataBoundItem as RelationshipDTO);
        }
        void dtgSubUnionToUnion_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            AddConvertRelationshipContextMenu(e, dtgSubUnionToUnion.CurrentRow.DataBoundItem as SubUnionToUnion);
        }

        void dtgUnionToSubUnion_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            AddConvertRelationshipContextMenu(e, dtgUnionToSubUnion.CurrentRow.DataBoundItem as UnionToSubUnion);
        }

        void dtgSubToSuper_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            AddConvertRelationshipContextMenu(e, dtgSubToSuper.CurrentRow.DataBoundItem as SubToSuper);
        }

        void dtgSuperToSub_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            AddConvertRelationshipContextMenu(e, dtgSuperToSub.CurrentRow.DataBoundItem as SuperToSub);
        }

        void dtgImplicit_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            AddConvertRelationshipContextMenu(e, dtgImplicit.CurrentRow.DataBoundItem as ImplicitOneToOne);
        }

        void dtgExplicit_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            AddConvertRelationshipContextMenu(e, dtgExplicit.CurrentRow.DataBoundItem as ExplicitOneToOne);
        }

        void dtgManyToOne_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            AddConvertRelationshipContextMenu(e, dtgManyToOne.CurrentRow.DataBoundItem as ManyToOne);
        }
        void dtgManyToMany_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
            e.ContextMenu.Items.Add(separator);
            //var entity = dtgManyToMany.CurrentRow.DataBoundItem as ManyToMany;
            //if (entity != null)
            //{

            RadMenuItem customMenuItem = new RadMenuItem();
            customMenuItem.Text = "Create Many To Many Relationship";
            customMenuItem.Name = "CreateManyToMany";
            customMenuItem.Click += customMenuItem_Click;
            e.ContextMenu.Items.Add(customMenuItem);


            RadMenuItem customMenuItem1 = new RadMenuItem();
            customMenuItem1.Text = "Remove Many To Many Relationship";
            customMenuItem1.Name = "RemoveManyToMany";
            customMenuItem1.Click += customMenuItemRemove_Click;
            e.ContextMenu.Items.Add(customMenuItem1);

            //}
        }

        void customMenuItem_Click(object sender, EventArgs e)
        {
            MyFormHelper.CreateManyToManyRelationships(txtServerName.Text, txtDBName.Text);
        }
        void customMenuItemRemove_Click(object sender, EventArgs e)
        {
            var entity = dtgManyToMany.CurrentRow.DataBoundItem as ManyToMany;
            if (entity != null)
            {

                MyFormHelper.RemoveManyToManyRelationships(entity.ID);
            }
        }

        //void dtgManyToMany_ManyToOne_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        //{
        //    RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
        //    e.ContextMenu.Items.Add(separator);
        //    var entity = dtgManyToMany_ManyToOne.CurrentRow.DataBoundItem as ManyToOne;
        //    if (entity != null)
        //    {

        //        RadMenuItem customMenuItem = new RadMenuItem();
        //        customMenuItem.Text = "Remove from Many To Many Relationship";
        //        customMenuItem.Name = "RemoveManyToOne";
        //        customMenuItem.Click += (sender1, EventArgs) => customMenuItem_Click11(sender, e, entity);
        //        e.ContextMenu.Items.Add(customMenuItem);


        //    }
        //}
        //void customMenuItem_Click11(object sender, EventArgs e, ManyToOne entity)
        //{
        //    MyFormHelper.RemoveManyToOneFromManyToManyRelationship(entity);
        //}

        //void customMenuItem_Click1(object sender, EventArgs e, ManyToMany entity)
        //{

        //}

        void dtgOneToMany_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            AddConvertRelationshipContextMenu(e, dtgOneToMany.CurrentRow.DataBoundItem as OneToMany);
        }

        private void AddConvertRelationshipContextMenu(ContextMenuOpeningEventArgs e, RelationshipDTO relationship)
        {
            if (relationship == null)
                return;
            RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
            e.ContextMenu.Items.Add(separator);

            if (relationship is OneToMany || relationship.TypeEnum == Enum_RelationshipType.OneToMany)
            {
                RadMenuItem customMenuItem = new RadMenuItem();
                customMenuItem.Text = "Convert To Implicit OneToOne Relationship";
                customMenuItem.Name = "OneToMany_ImplicitOneToOne";
                customMenuItem.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                RadMenuItem customMenuItem1 = new RadMenuItem();
                customMenuItem1.Text = "Convert To SupertType To SubType Relationship";
                customMenuItem1.Name = "OneToMany_SuperToSub";
                customMenuItem1.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                RadMenuItem customMenuItem2 = new RadMenuItem();
                customMenuItem2.Text = "Convert To SubUnion To Union Relationship";
                customMenuItem2.Name = "OneToMany_SubUnionToUnion";
                customMenuItem2.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                RadMenuItem customMenuItem3 = new RadMenuItem();
                customMenuItem3.Text = "Convert To Union To SubUnion Relationship";
                customMenuItem3.Name = "OneToMany_UnionToSubUnion";
                customMenuItem3.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;


                e.ContextMenu.Items.Add(customMenuItem);
                e.ContextMenu.Items.Add(customMenuItem1);
                e.ContextMenu.Items.Add(customMenuItem2);
                e.ContextMenu.Items.Add(customMenuItem3);

            }
            else if (relationship is ManyToOne || relationship.TypeEnum == Enum_RelationshipType.ManyToOne)
            {
                RadMenuItem customMenuItem = new RadMenuItem();
                customMenuItem.Text = "Convert To Explicit OneToOne Relationship";
                customMenuItem.Name = "ManyToOne_ExplicitOneToOne";
                customMenuItem.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                RadMenuItem customMenuItem1 = new RadMenuItem();
                customMenuItem1.Text = "Convert To SubType To SupertType Relationship";
                customMenuItem1.Name = "ManyToOne_SuperToSub";
                customMenuItem1.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                RadMenuItem customMenuItem2 = new RadMenuItem();
                customMenuItem2.Text = "Convert To SubUnion To Union Relationship";
                customMenuItem2.Name = "ManyToOne_SubUnionToUnion";
                customMenuItem2.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                RadMenuItem customMenuItem3 = new RadMenuItem();
                customMenuItem3.Text = "Convert To Union To SubUnion Relationship";
                customMenuItem3.Name = "ManyToOne_UnionToSubUnion";
                customMenuItem3.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;

                e.ContextMenu.Items.Add(customMenuItem);
                e.ContextMenu.Items.Add(customMenuItem1);
                e.ContextMenu.Items.Add(customMenuItem2);
                e.ContextMenu.Items.Add(customMenuItem3);

            }
            else if (relationship is ImplicitOneToOne || relationship.TypeEnum == Enum_RelationshipType.ImplicitOneToOne)
            {
                RadMenuItem customMenuItem = new RadMenuItem();
                customMenuItem.Text = "Convert To One To Many Relationship";
                customMenuItem.Name = "ImplicitOneToOne_OneToMany";
                customMenuItem.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                RadMenuItem customMenuItem1 = new RadMenuItem();
                customMenuItem1.Text = "Convert To SupertType To SubType Relationship";
                customMenuItem1.Name = "ImplicitOneToOne_SuperToSub";
                customMenuItem1.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                RadMenuItem customMenuItem2 = new RadMenuItem();
                customMenuItem2.Text = "Convert To SubUnion To Union Relationship";
                customMenuItem2.Name = "ImplicitOneToOne_SubUnionToUnion";
                customMenuItem2.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                RadMenuItem customMenuItem3 = new RadMenuItem();
                customMenuItem3.Text = "Convert To Union To SubUnion Relationship";
                customMenuItem3.Name = "ImplicitOneToOne_UnionToSubUnion";
                customMenuItem3.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;

                e.ContextMenu.Items.Add(customMenuItem);
                e.ContextMenu.Items.Add(customMenuItem1);
                e.ContextMenu.Items.Add(customMenuItem2);
                e.ContextMenu.Items.Add(customMenuItem3);

            }
            else if (relationship is ExplicitOneToOne || relationship.TypeEnum == Enum_RelationshipType.ExplicitOneToOne)
            {
                RadMenuItem customMenuItem = new RadMenuItem();
                customMenuItem.Text = "Convert To Many To One Relationship";
                customMenuItem.Name = "ExplicitOneToOne_ManyToOne";
                customMenuItem.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                RadMenuItem customMenuItem1 = new RadMenuItem();
                customMenuItem1.Text = "Convert To SubType To SupertType Relationship";
                customMenuItem1.Name = "ExplicitOneToOne_SuperToSub";
                customMenuItem1.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                RadMenuItem customMenuItem2 = new RadMenuItem();
                customMenuItem2.Text = "Convert To SubUnion To Union Relationship";
                customMenuItem2.Name = "ExplicitOneToOne_SubUnionToUnion";
                customMenuItem2.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                RadMenuItem customMenuItem3 = new RadMenuItem();
                customMenuItem3.Text = "Convert To Union To SubUnion Relationship";
                customMenuItem3.Name = "ExplicitOneToOne_UnionToSubUnion";
                customMenuItem3.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;

                e.ContextMenu.Items.Add(customMenuItem);
                e.ContextMenu.Items.Add(customMenuItem1);
                e.ContextMenu.Items.Add(customMenuItem2);
                e.ContextMenu.Items.Add(customMenuItem3);

            }
            else if (relationship is SuperToSub || relationship.TypeEnum == Enum_RelationshipType.SuperToSub)
            {
                RadMenuItem customMenuItem = new RadMenuItem();
                customMenuItem.Text = "Convert To One To Many Relationship";
                customMenuItem.Name = "SuperToSub_OneToMany";
                customMenuItem.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                RadMenuItem customMenuItem1 = new RadMenuItem();
                customMenuItem1.Text = "Convert To Implicit One To One Relationship";
                customMenuItem1.Name = "SuperToSub_ImplicitOneToOne";
                customMenuItem1.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                RadMenuItem customMenuItem2 = new RadMenuItem();
                customMenuItem2.Text = "Convert To SubUnion To Union Relationship";
                customMenuItem2.Name = "SuperToSub_SubUnionToUnion";
                customMenuItem2.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                RadMenuItem customMenuItem3 = new RadMenuItem();
                customMenuItem3.Text = "Convert To Union To SubUnion Relationship";
                customMenuItem3.Name = "SuperToSub_UnionToSubUnion";
                customMenuItem3.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;



                e.ContextMenu.Items.Add(customMenuItem);
                e.ContextMenu.Items.Add(customMenuItem1);
                e.ContextMenu.Items.Add(customMenuItem2);
                e.ContextMenu.Items.Add(customMenuItem3);

                RadMenuSeparatorItem separator1 = new RadMenuSeparatorItem();
                e.ContextMenu.Items.Add(separator1);


                RadMenuItem customMenuItem4 = new RadMenuItem();
                customMenuItem4.Text = "Add To another ISA Relationship";
                customMenuItem4.Name = "SuperToSub_SuperToSub";
                customMenuItem4.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                e.ContextMenu.Items.Add(customMenuItem4);
            }
            else if (relationship is SubToSuper || relationship.TypeEnum == Enum_RelationshipType.SubToSuper)
            {

                RadMenuItem customMenuItem = new RadMenuItem();
                customMenuItem.Text = "Convert To Many To One Relationship";
                customMenuItem.Name = "SubToSuper_ManyToOne";
                customMenuItem.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                RadMenuItem customMenuItem1 = new RadMenuItem();
                customMenuItem1.Text = "Convert To Explicit One To One Relationship";
                customMenuItem1.Name = "SubToSuper_ExplicitOneToOne";
                customMenuItem1.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                RadMenuItem customMenuItem2 = new RadMenuItem();
                customMenuItem2.Text = "Convert To SubUnion To Union Relationship";
                customMenuItem2.Name = "SubToSuper_SubUnionToUnion";
                customMenuItem2.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                RadMenuItem customMenuItem3 = new RadMenuItem();
                customMenuItem3.Text = "Convert To Union To SubUnion Relationship";
                customMenuItem3.Name = "SubToSuper_UnionToSubUnion";
                customMenuItem3.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;

                e.ContextMenu.Items.Add(customMenuItem);
                e.ContextMenu.Items.Add(customMenuItem1);
                e.ContextMenu.Items.Add(customMenuItem2);
                e.ContextMenu.Items.Add(customMenuItem3);

                RadMenuSeparatorItem separator1 = new RadMenuSeparatorItem();
                e.ContextMenu.Items.Add(separator1);

                RadMenuItem customMenuItem4 = new RadMenuItem();
                customMenuItem4.Text = "Add To another ISA Relationship";
                customMenuItem4.Name = "SubToSuper_SubToSuper";
                customMenuItem4.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                e.ContextMenu.Items.Add(customMenuItem4);
            }
            else if (relationship is UnionToSubUnion || relationship.TypeEnum == Enum_RelationshipType.UnionToSubUnion_SubUnionHoldsKeys || relationship.TypeEnum == Enum_RelationshipType.UnionToSubUnion_UnionHoldsKeys)
            {
                if (!(relationship as UnionToSubUnion).UnionHoldsKeys || relationship.TypeEnum == Enum_RelationshipType.UnionToSubUnion_SubUnionHoldsKeys)
                {
                    RadMenuItem customMenuItem = new RadMenuItem();
                    customMenuItem.Text = "Convert To One To Many Relationship";
                    customMenuItem.Name = "UnionToSubUnion_!UnionHoldsKeys_OneToMany";
                    customMenuItem.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                    RadMenuItem customMenuItem1 = new RadMenuItem();
                    customMenuItem1.Text = "Convert To Implicit One To One Relationship";
                    customMenuItem1.Name = "UnionToSubUnion_!UnionHoldsKeys_ImplicitOneToOne";
                    customMenuItem1.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                    RadMenuItem customMenuItem2 = new RadMenuItem();
                    customMenuItem2.Text = "Convert To SupertType To SubType Relationship";
                    customMenuItem2.Name = "UnionToSubUnion_!UnionHoldsKeys_SuperToSub";
                    customMenuItem2.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                    RadMenuItem customMenuItem3 = new RadMenuItem();
                    customMenuItem3.Text = "Convert To SubUnion To Union Relationship";
                    customMenuItem3.Name = "UnionToSubUnion_!UnionHoldsKeys_SubUnionToUnion";
                    customMenuItem3.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;


                    e.ContextMenu.Items.Add(customMenuItem);
                    e.ContextMenu.Items.Add(customMenuItem1);
                    e.ContextMenu.Items.Add(customMenuItem2);
                    e.ContextMenu.Items.Add(customMenuItem3);

                    RadMenuSeparatorItem separator1 = new RadMenuSeparatorItem();
                    e.ContextMenu.Items.Add(separator1);


                    RadMenuItem customMenuItem4 = new RadMenuItem();
                    customMenuItem4.Text = "Add To another Union Relationship";
                    customMenuItem4.Name = "UnionToSubUnion_!UnionHoldsKeys_UnionToSubUnion";
                    customMenuItem4.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                    e.ContextMenu.Items.Add(customMenuItem4);

                }
                else
                {
                    RadMenuItem customMenuItem = new RadMenuItem();
                    customMenuItem.Text = "Convert To Many To One Relationship";
                    customMenuItem.Name = "UnionToSubUnion_UnionHoldsKeys_ManyToOne";
                    customMenuItem.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                    RadMenuItem customMenuItem1 = new RadMenuItem();
                    customMenuItem1.Text = "Convert To Explicit One To One Relationship";
                    customMenuItem1.Name = "UnionToSubUnion_UnionHoldsKeys_ExplicitOneToOne";
                    customMenuItem1.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship);
                    RadMenuItem customMenuItem2 = new RadMenuItem();
                    customMenuItem2.Text = "Convert To SubType To SuperType Relationship";
                    customMenuItem2.Name = "UnionToSubUnion_UnionHoldsKeys_SubToSuper";
                    customMenuItem2.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                    RadMenuItem customMenuItem3 = new RadMenuItem();
                    customMenuItem3.Text = "Convert To SubUnion To Union Relationship";
                    customMenuItem3.Name = "UnionToSubUnion_UnionHoldsKeys_SubUnionToUnion";
                    customMenuItem3.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;


                    e.ContextMenu.Items.Add(customMenuItem);
                    e.ContextMenu.Items.Add(customMenuItem1);
                    e.ContextMenu.Items.Add(customMenuItem2);
                    e.ContextMenu.Items.Add(customMenuItem3);


                    RadMenuSeparatorItem separator1 = new RadMenuSeparatorItem();
                    e.ContextMenu.Items.Add(separator1);


                    RadMenuItem customMenuItem4 = new RadMenuItem();
                    customMenuItem4.Text = "Add To another Union Relationship";
                    customMenuItem4.Name = "UnionToSubUnion_UnionHoldsKeys_UnionToSubUnion";
                    customMenuItem4.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                    e.ContextMenu.Items.Add(customMenuItem4);
                }

            }
            else if (relationship is SubUnionToUnion || relationship.TypeEnum == Enum_RelationshipType.SubUnionToUnion_UnionHoldsKeys || relationship.TypeEnum == Enum_RelationshipType.SubUnionToUnion_SubUnionHoldsKeys)
            {
                if (!(relationship as SubUnionToUnion).UnionHoldsKeys || relationship.TypeEnum == Enum_RelationshipType.SubUnionToUnion_SubUnionHoldsKeys)
                {
                    RadMenuItem customMenuItem = new RadMenuItem();
                    customMenuItem.Text = "Convert To Many To One Relationship";
                    customMenuItem.Name = "SubUnionToUnion_!UnionHoldsKeys_ManyToOne";
                    customMenuItem.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                    RadMenuItem customMenuItem1 = new RadMenuItem();
                    customMenuItem1.Text = "Convert To Explicit One To One Relationship";
                    customMenuItem1.Name = "SubUnionToUnion_!UnionHoldsKeys_ExplicitOneToOne";
                    customMenuItem1.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                    RadMenuItem customMenuItem2 = new RadMenuItem();
                    customMenuItem2.Text = "Convert To SubType To SuperType Relationship";
                    customMenuItem2.Name = "SubUnionToUnion_!UnionHoldsKeys_SubToSuper";
                    customMenuItem2.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                    RadMenuItem customMenuItem3 = new RadMenuItem();
                    customMenuItem3.Text = "Convert To Union To SubUnion Relationship";
                    customMenuItem3.Name = "SubUnionToUnion_!UnionHoldsKeys_UnionToSubUnion";
                    customMenuItem3.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;


                    e.ContextMenu.Items.Add(customMenuItem);
                    e.ContextMenu.Items.Add(customMenuItem1);
                    e.ContextMenu.Items.Add(customMenuItem2);
                    e.ContextMenu.Items.Add(customMenuItem3);


                    RadMenuSeparatorItem separator1 = new RadMenuSeparatorItem();
                    e.ContextMenu.Items.Add(separator1);


                    RadMenuItem customMenuItem4 = new RadMenuItem();
                    customMenuItem4.Text = "Add To another Union Relationship";
                    customMenuItem4.Name = "SubUnionToUnion_!UnionHoldsKeys_SubUnionToUnion";
                    customMenuItem4.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                    e.ContextMenu.Items.Add(customMenuItem4);
                }
                else
                {
                    RadMenuItem customMenuItem = new RadMenuItem();
                    customMenuItem.Text = "Convert To One To Many Relationship";
                    customMenuItem.Name = "SubUnionToUnion_UnionHoldsKeys_OneToMany";
                    customMenuItem.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                    RadMenuItem customMenuItem1 = new RadMenuItem();
                    customMenuItem1.Text = "Convert To Implicit One To One Relationship";
                    customMenuItem1.Name = "SubUnionToUnion_UnionHoldsKeys_ImplicitOneToOne";
                    customMenuItem1.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                    RadMenuItem customMenuItem2 = new RadMenuItem();
                    customMenuItem2.Text = "Convert To SupertType To SubType Relationship";
                    customMenuItem2.Name = "SubUnionToUnion_UnionHoldsKeys_SuperToSub";
                    customMenuItem2.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                    RadMenuItem customMenuItem3 = new RadMenuItem();
                    customMenuItem3.Text = "Convert To Union To SubUnion Relationship";
                    customMenuItem3.Name = "SubUnionToUnion_UnionHoldsKeys_UnionToSubUnion";
                    customMenuItem3.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;


                    e.ContextMenu.Items.Add(customMenuItem);
                    e.ContextMenu.Items.Add(customMenuItem1);
                    e.ContextMenu.Items.Add(customMenuItem2);
                    e.ContextMenu.Items.Add(customMenuItem3);

                    RadMenuItem customMenuItem4 = new RadMenuItem();
                    customMenuItem4.Text = "Add To another Union Relationship";
                    customMenuItem4.Name = "SubUnionToUnion_UnionHoldsKeys_SubUnionToUnion";
                    customMenuItem4.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationship); ;
                    e.ContextMenu.Items.Add(customMenuItem4);
                }
            }
        }

        void customMenuItem_Click(object sender, EventArgs e, RelationshipDTO relationship)
        {
            MyFormHelper.ConvertRelationship((sender as RadMenuItem).Name, relationship);
        }

        void dtgUnionRelationship_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            List<UnionRelationship> list = new List<UnionRelationship>();
            foreach (var row in dtgUnionRelationshipType.SelectedRows)
            {
                list.Add(row.DataBoundItem as UnionRelationship);
            }
            if (list.Count > 1)
                AddMergeUnionRelationshipContextMenu(e, list, dtgUnionRelationshipType.CurrentRow.DataBoundItem as UnionRelationship);
        }
        private void AddMergeUnionRelationshipContextMenu(ContextMenuOpeningEventArgs e, List<UnionRelationship> relationships, UnionRelationship selectedOne)
        {
            RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
            RadMenuItem customMenuItem = new RadMenuItem();
            customMenuItem.Text = "Merge Union Relationships";
            customMenuItem.Name = "MergeUnionRelationships";
            customMenuItem.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationships, selectedOne);
            e.ContextMenu.Items.Add(separator);
            e.ContextMenu.Items.Add(customMenuItem);
        }

        void customMenuItem_Click(object sender, ContextMenuOpeningEventArgs e, List<UnionRelationship> relationships, UnionRelationship selectedOne)
        {
            MyFormHelper.MergeUnionRelationships((sender as RadMenuItem).Name, relationships, selectedOne);
        }



        void dtgISARelationship_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            List<ISARelationshipDTO> list = new List<ISARelationshipDTO>();
            foreach (var row in dtgISARelationship.SelectedRows)
            {
                list.Add(row.DataBoundItem as ISARelationshipDTO);
            }
            if (list.Count > 1)
                AddMergeISARelationshipContextMenu(e, list, dtgISARelationship.CurrentRow.DataBoundItem as ISARelationshipDTO);
        }
        private void AddMergeISARelationshipContextMenu(ContextMenuOpeningEventArgs e, List<ISARelationshipDTO> relationships, ISARelationshipDTO selectedOne)
        {
            RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
            RadMenuItem customMenuItem = new RadMenuItem();
            customMenuItem.Text = "Merge ISA Relationships";
            customMenuItem.Name = "MergeISARelationships";
            customMenuItem.Click += (sender, EventArgs) => customMenuItem_Click(sender, e, relationships, selectedOne);
            e.ContextMenu.Items.Add(separator);
            e.ContextMenu.Items.Add(customMenuItem);
        }

        void customMenuItem_Click(object sender, ContextMenuOpeningEventArgs e, List<ISARelationshipDTO> relationships, ISARelationshipDTO selectedOne)
        {
            MyFormHelper.MergeISARelationships((sender as RadMenuItem).Name, relationships, selectedOne);
        }

        //void dtgEntityRelations_HyperlinkOpening(object sender, HyperlinkOpeningEventArgs e)
        //{
        //    if (e.Column.FieldName == "SuperTypeEntities")
        //    {
        //        var data = e.Row.DataBoundItem as EntityDTO;
        //        if (data != null)
        //        {
        //            MyFormHelper.ChooseSuperEntityFor(data);
        //        }
        //    }
        //    else if (e.Column.FieldName == "UnionTypeEntities")
        //    {
        //        var data = e.Row.DataBoundItem as EntityDTO;
        //        if (data != null)
        //        {
        //            MyFormHelper.ChooseUnionEntityFor(data);
        //        }
        //    }
        //}

        void dtgRuleEntity_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            //if (e.Column.FieldName == "SuperTypeEntities" || e.Column.FieldName == "UnionTypeEntities")
            //{
            //    if (e.CellElement.Children.Count == 0) 
            //    { 
            //        progressBarElement = new RadProgressBarElement(); 
            //        e.CellElement.Children.Add(progressBarElement);
            //        progressBarElement.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; 
            //    } 
            //    else {
            //        progressBarElement = e.CellElement.Children[0] as RadProgressBarElement; 
            //    } 


            //    GridCommandCellElement commandCell = e.CellElement as GridCommandCellElement;
            //    if (e.CellElement.Value != null && e.CellElement.Value.ToString() != "")
            //    {
            //        commandCell.Text = e.CellElement.Value + "";
            //        commandCell.CommandButton.Visibility = ElementVisibility.Visible;
            //        commandCell.CommandButton.Text = e.CellElement.Value + "";
            //    }
            //    else
            //        e.CellElement.Children.Clear();//.Enabled=false;//.Visibility = ElementVisibility.Collapsed;
            //}
        }

        private void SetGridViewColumns()
        {
            SetGridViewColumns(dtgTables);
            SetGridViewColumns(dtgColumns);
            SetGridViewColumns(dtgColumnKeyValue);
            SetGridViewColumns(dtgStringColumnType);
            SetGridViewColumns(dtgNumericColumnType);
            SetGridViewColumns(dtgDateColumnType);
            SetGridViewColumns(dtgUniqueConstraint);
            SetGridViewColumns(dtgDefaultEntity);

            //SetGridViewColumns(dtgRelationships);
            SetGridViewColumns(dtgRuleEntity);
            SetGridViewColumns(dtgRuleRelationships);
            SetGridViewColumns(dtgOneToMany);
            SetGridViewColumns(dtgManyToOne);
            SetGridViewColumns(dtgExplicit);
            SetGridViewColumns(dtgImplicit);
            SetGridViewColumns(dtgManyToMany);
            SetGridViewColumns(dtgManyToMany_ManyToOne);
            SetGridViewColumns(dtgISARelationship);
            SetGridViewColumns(dtgSuperToSub);
            SetGridViewColumns(dtgSubToSuper);
            SetGridViewColumns(dtgUnionRelationshipType);
            SetGridViewColumns(dtgUnionToSubUnion);
            SetGridViewColumns(dtgSubUnionToUnion);
        }

        private void SetGridViewColumns(RadGridView dataGrid)
        {
            dataGrid.EnableAlternatingRowColor = true;
            dataGrid.AutoGenerateColumns = false;
            if (dataGrid == dtgTables)
            {
                dataGrid.EnableFiltering = true;
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", true, 120, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("RelatedSchema", "Schema", true, 120, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Catalog", "Catalog", true, 120, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", false, 150, GridViewColumnType.Text));
                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("BatchDataEntry", "BatchDataEntry", false, 120, GridViewColumnType.CheckBox));
                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsDataReference", "IsDataReference", false, 120, GridViewColumnType.CheckBox));
                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsStructurReferencee", "IsStructurReferencee", false, 120, GridViewColumnType.CheckBox));
                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsAssociative", "IsAssociative", false, 120, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsInheritanceImplementation", "IsInheritanceImplementation", false, 120, GridViewColumnType.CheckBox));
            }
            else if (dataGrid == dtgColumns)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PrimaryKey", "PrimaryKey", true, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsIdentity", "IsIdentity", true, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsNull", "IsNull", true, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsMandatory", "IsMandatory", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DefaultValue", "DefaultValue", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataType", "DataType", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Position", "Position", false, 100, GridViewColumnType.Numeric));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataEntryEnabled", "DataEntryEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SearchEnabled", "SearchEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ViewEnabled", "ViewEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ViewAggregatedData", "ViewAggregatedData", false, 100, GridViewColumnType.CheckBox));
            }
            else if (dataGrid == dtgStringColumnType)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ColumnID", "ColumnID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("MaxLength", "MaxLength", false, 100, GridViewColumnType.Numeric));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Format", "Format", false, 200, GridViewColumnType.Text));
            }
            else if (dataGrid == dtgNumericColumnType)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ColumnID", "ColumnID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Precision", "Precision", false, 100, GridViewColumnType.Numeric));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Scale", "Scale", false, 200, GridViewColumnType.Numeric));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("MinValue", "MinValue", false, 100, GridViewColumnType.Numeric));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("MaxValue", "MaxValue", false, 200, GridViewColumnType.Numeric));
            }
            else if (dataGrid == dtgDateColumnType)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ColumnID", "ColumnID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsPersianDate", "IsPersianDate", false, 100, GridViewColumnType.CheckBox));

            }
            else if (dataGrid == dtgColumnKeyValue)
            {
                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ColumnID", "ColumnID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Value", "Value", false, 100, GridViewColumnType.Numeric));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("KeyTitle", "KeyTitle", false, 200, GridViewColumnType.Text));
            }
            else if (dataGrid == dtgUniqueConstraint)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Table", "Table", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Columns", "Columns", true, 100, GridViewColumnType.Text));
            }
            else if (dataGrid == dtgDefaultEntity)
            {
                dataGrid.EnableFiltering = true;
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Table", "Table", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Criteria", "Criteria", false, 200, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IndependentDataEntry", "IndependentDataEntry", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("BatchDataEntry", "BatchDataEntry", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsDataReference  ", "IsDataReference", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsStructurReferencee", "IsStructurReferencee", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsAssociative", "IsAssociative", false, 160, GridViewColumnType.CheckBox));
            }

            else if (dataGrid == dtgRuleEntity)
            {
                dataGrid.EnableFiltering = true;
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Table", "Table", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Criteria", "Criteria", false, 200, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IndependentDataEntry", "IndependentDataEntry", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("BatchDataEntry", "BatchDataEntry", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsDataReference", "IsDataReference", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsStructurReferencee", "IsStructurReferencee", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsAssociative", "IsAssociative", false, 160, GridViewColumnType.CheckBox));

            }
            else if (dataGrid == dtgRuleRelationships)
            {
                dataGrid.EnableFiltering = true;
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "Pair Relationship ID", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "Pair Relationship", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "Entity1", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "Entity2", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("RelationshipColumns", "RelationshipColumns", true, 160, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("TypeStr", "Type", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataEntryEnabled", "DataEntryEnabled", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SearchEnabled", "SearchEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ViewEnabled", "ViewEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "Enabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.AllowDeleteRow = false;
            }
            else if (dataGrid == dtgOneToMany)
            {
                dataGrid.EnableFiltering = true;
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "Pair Relationship ID", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "Pair Relationship", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "Entity1", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "Entity2", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("MasterDetailState", "MasterDetailState", false, 100, GridViewColumnType.Numeric));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DetailsCount", "DetailsCount", false, 100, GridViewColumnType.Numeric));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsManySideMadatory", "IsManySideMadatory", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsManySideDirectlyCreatable", "IsManySideDirectlyCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsManySideCreatable", "IsManySideCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "Enabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.AllowDeleteRow = false;
            }
            else if (dataGrid == dtgManyToOne)
            {
                dataGrid.EnableFiltering = true;
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "Pair Relationship ID", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "Pair Relationship", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "Entity1", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "Entity2", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOneSideTransferable", "IsOneSideTransferable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOneSideCreatable", "IsOneSideCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOneSideMadatory", "IsOneSideMadatory", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOneSideDirectlyCreatable", "IsOneSideDirectlyCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "Enabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.AllowDeleteRow = false;
            }
            else if (dataGrid == dtgExplicit)
            {
                dataGrid.EnableFiltering = true;
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "Pair Relationship ID", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "Pair Relationship", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "Entity1", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "Entity2", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideTransferable", "IsOtherSideTransferable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideCreatable", "IsOtherSideCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideMandatory", "IsOtherSideMandatory", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideDirectlyCreatable", "IsOtherSideDirectlyCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "Enabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.AllowDeleteRow = false;
            }
            else if (dataGrid == dtgImplicit)
            {
                dataGrid.EnableFiltering = true;
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "Pair Relationship ID", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "Pair Relationship", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "Entity1", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "Entity2", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideTransferable", "IsOtherSideTransferable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideCreatable", "IsOtherSideCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideMandatory", "IsOtherSideMandatory", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideDirectlyCreatable", "IsOtherSideDirectlyCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "Enabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.AllowDeleteRow = false;
            }
            else if (dataGrid == dtgManyToMany)
            {
                dataGrid.EnableFiltering = true;
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
            }
            else if (dataGrid == dtgManyToMany_ManyToOne)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "Pair Relationship ID", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "Pair Relationship", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "Entity1", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "Entity2", true, 100, GridViewColumnType.Text));
            }
            else if (dataGrid == dtgISARelationship)
            {
                dataGrid.EnableFiltering = true;
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SuperTypeEntities", "SuperTypeEntities", true, 120, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SubTypeEntities", "SubTypeEntities", true, 120, GridViewColumnType.Text));
                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsGeneralization", "IsGeneralization", false, 100, GridViewColumnType.CheckBox));
                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsSpecialization", "IsSpecialization", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsTolatParticipation", "IsTolatParticipation", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsDisjoint", "IsDisjoint", false, 100, GridViewColumnType.CheckBox));
                dataGrid.AllowDeleteRow = false;
            }
            else if (dataGrid == dtgSuperToSub)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "Pair Relationship ID", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "Pair Relationship", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "Entity1", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "Entity2", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideTransferable", "IsOtherSideTransferable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideCreatable", "IsOtherSideCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideDirectlyCreatable", "IsOtherSideDirectlyCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "Enabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.AllowDeleteRow = false;
            }
            else if (dataGrid == dtgSubToSuper)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "Pair Relationship ID", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "Pair Relationship", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "Entity1", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "Entity2", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideTransferable", "IsOtherSideTransferable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideCreatable", "IsOtherSideCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideDirectlyCreatable", "IsOtherSideDirectlyCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "Enabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.AllowDeleteRow = false;
            }
            else if (dataGrid == dtgUnionRelationshipType)
            {
                dataGrid.EnableFiltering = true;
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("UnionTypeEntities", "UnionTypeEntities", true, 120, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SubUnionTypeEntities", "SubUnionTypeEntities", true, 160, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsTolatParticipation", "IsTolatParticipation", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("UnionHoldsKeys", "UnionHoldsKeys", false, 130, GridViewColumnType.CheckBox));
                dataGrid.AllowDeleteRow = false;
            }
            else if (dataGrid == dtgUnionToSubUnion)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "Pair Relationship ID", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "Pair Relationship", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "Entity1", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "Entity2", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideTransferable", "IsOtherSideTransferable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideCreatable", "IsOtherSideCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideDirectlyCreatable", "IsOtherSideDirectlyCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "Enabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.AllowDeleteRow = false;
            }
            else if (dataGrid == dtgSubUnionToUnion)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "Pair Relationship ID", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "Pair Relationship", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "Entity1", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "Entity2", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideTransferable", "IsOtherSideTransferable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideCreatable", "IsOtherSideCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideDirectlyCreatable", "IsOtherSideDirectlyCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "Enabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.AllowDeleteRow = false;
            }
        }














        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            var connectionString = SQLServerHelper.GetConnectionString(txtServerName.Text, txtDBName.Text, txtUserName.Text, txtPassword.Text);
            using (SqlConnection testConn = new SqlConnection(connectionString))
            {
                try
                {
                    testConn.Open();
                    Properties.Settings.Default.LastDatabase = txtDBName.Text;
                    Properties.Settings.Default.Save();
                    MessageBox.Show("Connection Successfull!");



                }
                catch (SqlException)
                {
                    MessageBox.Show("Connection failed!");
                }
            }
        }

        //private void button2_Click(object sender, EventArgs e)
        //{

        //}

        void helper_ItemGenerationEvent(object sender, SimpleGenerationInfoArg e)
        {

            SetInfo(e.TotalProgressCount, e.CurrentProgress, e.Title);
        }

        private void btnImportRelationships_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("فرایند ورود اطلاعات خودکار روابط برای موجودیتهای پیش فرض" + Environment.NewLine + Environment.NewLine + "آیا مطمئن هستید؟", "تائید", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                SQLServerHelper helper = new SQLServerHelper();
                helper.ItemGenerationEvent += helper_ItemGenerationEvent;
                var connectionString = SQLServerHelper.GetConnectionString(txtServerName.Text, txtDBName.Text, txtUserName.Text, txtPassword.Text);
                //try
                //{
                var result = helper.GenerateRelationships(string.Format(connectionString, txtServerName.Text, txtDBName.Text, txtUserName.Text, txtPassword.Text), txtServerName.Text, txtDBName.Text);
                if (result)
                {
                    MyFormHelper.GetRuleRelationships(txtServerName.Text, txtDBName.Text);
                    MessageBox.Show("Operation is completed.");
                }
                else
                    MessageBox.Show("Operation is not done!");
            }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Operation failed." + Environment.NewLine + ex.Message);
            //}
        }
        //internal void SetRelationships(List<RelationshipDTO> list)
        //{
        //    dtgRelationships.DataSource = list;
        //}
        //private void btnRefreshRelationships_Click(object sender, EventArgs e)
        //{
        //    MyFormHelper.GetRelationships(txtServerName.Text, txtDBName.Text);
        //}



        //void helper_NDRelationGenerationStarted(object sender, SimpleGenerationInfoArg e)
        //{
        //    lblInfo.Text = "Relation '" + e.Title + "' is being generated.";
        //    lblInfo.Refresh();
        //}

        private void btnDefaultEntities_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("فرایند ورود اطلاعات خودکار موجودیتهای پیش فرض" + Environment.NewLine + Environment.NewLine + "آیا مطمئن هستید؟", "تائید", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                SQLServerHelper helper = new SQLServerHelper();
                helper.ItemGenerationEvent += helper_ItemGenerationEvent;
                //var connectionString = GetConnectionString();
                try
                {
                    var result = helper.GenerateDefaultEntities();
                    if (result)
                    {
                        MyFormHelper.GetEntities(txtServerName.Text, txtDBName.Text);
                        MessageBox.Show("Operation is completed.");
                    }
                    else
                        MessageBox.Show("Operation is not done!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Operation failed." + Environment.NewLine + ex.Message);
                }
            }
        }
        internal void SetEntities(List<EntityDTO> list)
        {
            dtgDefaultEntity.DataSource = list;
        }
        private void btnRefreshDefaultEntities_Click(object sender, EventArgs e)
        {
            MyFormHelper.GetEntities(txtServerName.Text, txtDBName.Text);
        }




        private void btnImportTables_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("فرایند ورود اطلاعات خودکار جداول و ستونها" + Environment.NewLine + Environment.NewLine + "آیا مطمئن هستید؟", "تائید", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                SQLServerHelper helper = new SQLServerHelper();
                helper.ItemGenerationEvent += helper_ItemGenerationEvent;
                var connectionString = SQLServerHelper.GetConnectionString(txtServerName.Text, txtDBName.Text, txtUserName.Text, txtPassword.Text);
                //try
                //{
                var result = helper.GenerateTablesAndColumns(connectionString, txtServerName.Text, txtDBName.Text);
                if (result)
                {
                    MyFormHelper.GetTables(txtServerName.Text, txtDBName.Text);
                    MessageBox.Show("Operation is completed.");
                }
                else
                    MessageBox.Show("Operation is not done!");
            }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Operation failed." + Environment.NewLine + ex.Message);
            //}
        }
        private void btnRefreshTables_Click(object sender, EventArgs e)
        {
            MyFormHelper.GetTables(txtServerName.Text, txtDBName.Text);
        }

        void dtgTables_SelectionChanged(object sender, EventArgs e)
        {
            if (dtgTables.SelectedRows.Count != 0)
            {
                var selectedRow = dtgTables.SelectedRows[0];
                Table table = selectedRow.DataBoundItem as Table;
                if (table != null)
                {
                    MyFormHelper.GetColumns(table.ID);
                }
            }
        }

        void dtgColumns_SelectionChanged(object sender, EventArgs e)
        {
            if (dtgColumns.SelectedRows.Count != 0)
            {
                var selectedRow = dtgColumns.SelectedRows[0];
                Column column = selectedRow.DataBoundItem as Column;
                if (column != null)
                {
                    MyFormHelper.GetColumnType(column.ID);
                    MyFormHelper.GetKeyValue(column.ID);
                }
            }
        }

        private void btnUpdateTables_Click(object sender, EventArgs e)
        {
            btnUpdateTables.Enabled = false;
            MyFormHelper.UpdateTables(dtgTables.DataSource);
            btnUpdateTables.Enabled = true;
        }

        internal void SetTables(List<DataAccess.Table> list)
        {
            btnUpdateTables.Enabled = true;
            dtgTables.DataSource = list;

        }



        internal void SetColumns(List<Column> list)
        {
            btnUpdateColumns.Enabled = true;
            dtgColumns.DataSource = list;
        }
        private void btnUpdateColumns_Click(object sender, EventArgs e)
        {
            btnUpdateColumns.Enabled = false;
            MyFormHelper.UpdateColumns(dtgColumns.DataSource);
            btnUpdateColumns.Enabled = true;
        }

        internal void SetColumnKeyValue(ColumnKeyValue columnKeyValue)
        {
            if (columnKeyValue != null)
            {
                chkValueFromKeyOrValue.Checked = columnKeyValue.ValueFromKeyOrValue;
                dtgColumnKeyValue.DataSource = columnKeyValue.ColumnKeyValueRange.ToList();
            }
            else
            {
                chkValueFromKeyOrValue.Checked = false;
                dtgColumnKeyValue.DataSource = new List<ColumnKeyValueRange>();
            }
        }

        private void btnUpdateKeyValue_Click(object sender, EventArgs e)
        {
            var selectedRow = dtgColumns.SelectedRows[0];
            Column column = selectedRow.DataBoundItem as Column;
            if (column != null)
            {
                btnUpdateKeyValue.Enabled = false;
                MyFormHelper.UpdateColumnKeyValue(column.ID, chkValueFromKeyOrValue.Checked, dtgColumnKeyValue.DataSource);
                btnUpdateKeyValue.Enabled = true;
            }
        }
        internal void RemoveColumnType()
        {
            if (pageViewTables.Pages.Contains(pageNumericColumnType))
                pageViewTables.Pages.Remove(pageNumericColumnType);
            if (pageViewTables.Pages.Contains(pageDateColumnType))
                pageViewTables.Pages.Remove(pageDateColumnType);
            if (pageViewTables.Pages.Contains(pageStringColumnType))
                pageViewTables.Pages.Remove(pageStringColumnType);
        }
        internal void SetStringColumnType(List<StringColumnType> stringColumnType)
        {
            btnUpdateStringColumnType.Enabled = true;
            dtgStringColumnType.DataSource = stringColumnType;

            if (!pageViewTables.Pages.Contains(pageStringColumnType))
                pageViewTables.Pages.Insert(pageViewTables.Pages.Count, pageStringColumnType);
        }

        internal void SetNumericColumnType(List<NumericColumnType> numericColumnType)
        {
            btnUpdateNumericColumnType.Enabled = true;
            dtgNumericColumnType.DataSource = numericColumnType;

            if (!pageViewTables.Pages.Contains(pageNumericColumnType))
                pageViewTables.Pages.Insert(pageViewTables.Pages.Count, pageNumericColumnType);
        }
        internal void SetDateColumnType(List<DateColumnType> dateColumnType)
        {
            btnUpdateDateColumnType.Enabled = true;
            dtgDateColumnType.DataSource = dateColumnType;

            if (!pageViewTables.Pages.Contains(pageDateColumnType))
                pageViewTables.Pages.Insert(pageViewTables.Pages.Count, pageDateColumnType);
        }
        private void btnUpdateNumericColumnType_Click(object sender, EventArgs e)
        {
            btnUpdateNumericColumnType.Enabled = false;
            MyFormHelper.UpdateNumericColumnType(dtgNumericColumnType.DataSource);
            btnUpdateNumericColumnType.Enabled = true;
        }

        private void btnUpdateStringColumnType_Click(object sender, EventArgs e)
        {
            btnUpdateStringColumnType.Enabled = false;
            MyFormHelper.UpdateStringColumnType(dtgStringColumnType.DataSource);
            btnUpdateStringColumnType.Enabled = true;
        }

        private void btnUpdateDateColumnType_Click(object sender, EventArgs e)
        {
            btnUpdateDateColumnType.Enabled = false;
            MyFormHelper.UpdateDateColumnType(dtgDateColumnType.DataSource);
            btnUpdateDateColumnType.Enabled = true;
        }




        private void btnUniqueConstraints_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("فرایند ورود اطلاعات خودکار شروط یکتایی" + Environment.NewLine + Environment.NewLine + "آیا مطمئن هستید؟", "تائید", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                SQLServerHelper helper = new SQLServerHelper();
                helper.ItemGenerationEvent += helper_ItemGenerationEvent;
                var connectionString = SQLServerHelper.GetConnectionString(txtServerName.Text, txtDBName.Text, txtUserName.Text, txtPassword.Text);
                //try
                //{
                var result = helper.GenerateUniqueConstraints(connectionString, txtServerName.Text, txtDBName.Text);
                if (result)
                {
                    MyFormHelper.GetUniqueConstraints(txtServerName.Text, txtDBName.Text);
                    MessageBox.Show("Operation is completed.");
                }
                else
                    MessageBox.Show("Operation is not done!");
            }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Operation failed." + Environment.NewLine + ex.Message);
            //}
        }

        internal void UniqueConstraints(List<UniqueConstraintDTO> list)
        {

            dtgUniqueConstraint.DataSource = list;
        }

        private void btnRefreshUniqueConstraints_Click(object sender, EventArgs e)
        {
            MyFormHelper.GetUniqueConstraints(txtServerName.Text, txtDBName.Text);
        }






        private void btnImposeEntityRules_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("فرایند اعمال قوانین بروی موجودیتها" + Environment.NewLine + Environment.NewLine + "آیا مطمئن هستید؟", "تائید", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                MyFormHelper.ImposeEntitTableRule(txtServerName.Text, txtDBName.Text);
                MyFormHelper.GetRuleEntities(txtServerName.Text, txtDBName.Text);
            }
        }

        public void SetInfo(int totalProgressCount, int currentProgress, string info)
        {
            progressBar1.Maximum = totalProgressCount;
            progressBar1.Value = currentProgress;
            lblInfo.Text = info;
            lblInfo.Refresh();
        }


        private void btnRefreshEntityRules_Click(object sender, EventArgs e)
        {
            MyFormHelper.GetRuleEntities(txtServerName.Text, txtDBName.Text);
        }

        internal void SetRuleEntities(List<EntityDTO> list)
        {
            btnUpdateEntities.Enabled = true;
            dtgRuleEntity.DataSource = list;
        }
        private void btnUpdateEntities_Click(object sender, EventArgs e)
        {

            btnUpdateEntities.Enabled = false;
            MyFormHelper.UpdateRuleEntities(dtgRuleEntity.DataSource);
            btnUpdateEntities.Enabled = true;
        }


        private void btnImposeRuleRelationship_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("فرایند اعمال قوانین بروی روابط" + Environment.NewLine + Environment.NewLine + "آیا مطمئن هستید؟", "تائید", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                MyFormHelper.ImposeRelationshipRule(txtServerName.Text, txtDBName.Text);
                MyFormHelper.GetRuleRelationships(txtServerName.Text, txtDBName.Text);
            }

        }

        private void btnRefreshRuleRelationship_Click(object sender, EventArgs e)
        {
            MyFormHelper.GetRuleRelationships(txtServerName.Text, txtDBName.Text);
        }
        private void btnUpdateRelationships_Click(object sender, EventArgs e)
        {
            btnUpdateRelationships.Enabled = false;
            MyFormHelper.UpdateRuleRelationships(dtgRuleRelationships.DataSource);
            btnUpdateRelationships.Enabled = true;
        }
        internal void SetRuleRelationships(List<RelationshipDTO> list)
        {
            btnUpdateRelationships.Enabled = true;
            dtgRuleRelationships.DataSource = list;
        }



        private void btnUpdateOneToMany_Click(object sender, EventArgs e)
        {
            btnUpdateOneToMany.Enabled = false;
            MyFormHelper.UpdateOneToManyRelationships(dtgOneToMany.DataSource);
            btnUpdateOneToMany.Enabled = true;
        }
        private void btnRefreshOneToMany_Click(object sender, EventArgs e)
        {

            MyFormHelper.GetOneToManyRelationships(txtServerName.Text, txtDBName.Text);
        }
        internal void SetRuleRelationships(List<OneToMany> OneToManyList)
        {
            btnUpdateOneToMany.Enabled = true;
            dtgOneToMany.DataSource = OneToManyList;
        }


        private void btnUpdateManyToOne_Click(object sender, EventArgs e)
        {
            btnUpdateManyToOne.Enabled = false;
            MyFormHelper.UpdateManyToOneRelationships(dtgManyToOne.DataSource);
            btnUpdateManyToOne.Enabled = true;
        }
        private void btnRefreshManyToOne_Click(object sender, EventArgs e)
        {

            MyFormHelper.GetManyToOneRelationships(txtServerName.Text, txtDBName.Text);
        }
        internal void SetRuleRelationships(List<ManyToOne> ManyToOneList)
        {
            btnUpdateManyToOne.Enabled = true;
            dtgManyToOne.DataSource = ManyToOneList;
        }


        private void btnUpdateExplicit_Click(object sender, EventArgs e)
        {
            btnUpdateExplicit.Enabled = false;
            MyFormHelper.UpdateExplicitRelationships(dtgExplicit.DataSource);
            btnUpdateExplicit.Enabled = true;
        }
        private void btnRefreshExplicit_Click(object sender, EventArgs e)
        {

            MyFormHelper.GetExplicitOneToOneRelationships(txtServerName.Text, txtDBName.Text);
        }
        internal void SetRuleRelationships(List<ExplicitOneToOne> ExplicitOneToOneList)
        {
            btnUpdateExplicit.Enabled = true;
            dtgExplicit.DataSource = ExplicitOneToOneList;
        }


        private void btnUpdateImplicit_Click(object sender, EventArgs e)
        {
            btnUpdateImplicit.Enabled = false;
            MyFormHelper.UpdateImplicitRelationships(dtgImplicit.DataSource);
            btnUpdateImplicit.Enabled = true;
        }
        private void btnRefreshImplicit_Click(object sender, EventArgs e)
        {
            MyFormHelper.GetImplicitOneToOneRelationships(txtServerName.Text, txtDBName.Text);
        }
        internal void SetRuleRelationships(List<ImplicitOneToOne> ImplicitOneToOneList)
        {
            btnUpdateImplicit.Enabled = true;
            dtgImplicit.DataSource = ImplicitOneToOneList;
        }




        private void btnUpdateISA_Click(object sender, EventArgs e)
        {
            btnUpdateISA.Enabled = false;
            MyFormHelper.UpdateISARelationships(dtgISARelationship.DataSource);
            btnUpdateISA.Enabled = true;
        }
        private void btnRefreshISA_Click(object sender, EventArgs e)
        {
            MyFormHelper.GetISARelationships(txtServerName.Text, txtDBName.Text);
        }
        internal void SetRuleRelationships(List<ISARelationshipDTO> ISARelationshipList)
        {
            btnUpdateISA.Enabled = true;
            dtgISARelationship.DataSource = ISARelationshipList;
        }

        void dtgISARelationship_SelectionChanged(object sender, EventArgs e)
        {
            if (dtgISARelationship.SelectedRows.Count != 0)
            {
                var selectedRow = dtgISARelationship.SelectedRows[0];
                ISARelationshipDTO iSARelationship = selectedRow.DataBoundItem as ISARelationshipDTO;
                if (iSARelationship != null)
                {
                    MyFormHelper.GetSubSuperRelationshipTypes(iSARelationship.ID);
                }
            }
        }

        internal void SetSubSuperRelationships(List<SuperToSub> SuperToSubList, List<SubToSuper> SubToSuperList)
        {
            btnUpdateSuperToSub.Enabled = true;
            btnUpdateSubToSuper.Enabled = true;
            dtgSuperToSub.DataSource = SuperToSubList;
            dtgSubToSuper.DataSource = SubToSuperList;
        }
        private void btnUpdateSuperToSub_Click(object sender, EventArgs e)
        {
            btnUpdateSuperToSub.Enabled = false;
            MyFormHelper.UpdateSuperToSubRelationships(dtgSuperToSub.DataSource);
            btnUpdateSuperToSub.Enabled = true;
        }

        private void btnUpdateSubToSuper_Click(object sender, EventArgs e)
        {
            btnUpdateSubToSuper.Enabled = false;
            MyFormHelper.UpdateSubToSuperRelationships(dtgSubToSuper.DataSource);
            btnUpdateSubToSuper.Enabled = true;
        }





        private void btnUpdateUnionRelationship_Click(object sender, EventArgs e)
        {
            btnUpdateUnionRelationship.Enabled = false;
            MyFormHelper.UpdateUnionRelationships(dtgUnionRelationshipType.DataSource);
            btnUpdateUnionRelationship.Enabled = true;
        }
        private void btnRefreshUnion_Click(object sender, EventArgs e)
        {

            MyFormHelper.GetUnionRelationships(txtServerName.Text, txtDBName.Text);
        }

        internal void SetRuleRelationships(List<UnionRelationship> UnionRelationshipTypeList)
        {
            btnUpdateUnionRelationship.Enabled = true;
            dtgUnionRelationshipType.DataSource = UnionRelationshipTypeList;
        }




        void dtgUnionRelationshipType_SelectionChanged(object sender, EventArgs e)
        {
            if (dtgUnionRelationshipType.SelectedRows.Count != 0)
            {
                var selectedRow = dtgUnionRelationshipType.SelectedRows[0];
                UnionRelationship unionRelationship = selectedRow.DataBoundItem as UnionRelationship;
                if (unionRelationship != null)
                {
                    MyFormHelper.GetUnionRelationshipTypes(unionRelationship.ID);
                }
            }
        }



        internal void SetUnionRelationships(List<UnionToSubUnion> UnionToList, List<SubUnionToUnion> SubUnionToUnionList)
        {
            btnUpdateSubUnionToUnion.Enabled = true;
            btnUpdateUnionToSubUnion.Enabled = true;
            dtgUnionToSubUnion.DataSource = UnionToList;
            dtgSubUnionToUnion.DataSource = SubUnionToUnionList;
        }



        private void btnUpdateUnionToSubUnion_Click(object sender, EventArgs e)
        {
            btnUpdateUnionToSubUnion.Enabled = false;
            MyFormHelper.UpdateUnionToSubUnionRelationships(dtgUnionToSubUnion.DataSource);
            btnUpdateUnionToSubUnion.Enabled = true;
        }

        private void btnUpdateSubUnionToUnion_Click(object sender, EventArgs e)
        {
            btnUpdateSubUnionToUnion.Enabled = false;
            MyFormHelper.UpdateSubUnionToUnionRelationships(dtgSubUnionToUnion.DataSource);
            btnUpdateSubUnionToUnion.Enabled = true;
        }

        private void pageViewMain_SelectedPageChanged(object sender, EventArgs e)
        {
            return;
            if (MyFormHelper != null)
                if (pageViewMain.SelectedPage == pageImportTables)
                {
                    if (dtgTables.DataSource == null)
                        MyFormHelper.GetTables(txtServerName.Text, txtDBName.Text);
                }
                else if (pageViewMain.SelectedPage == pageImportUniqueConstraints)
                {
                    if (dtgUniqueConstraint.DataSource == null)
                        MyFormHelper.GetUniqueConstraints(txtServerName.Text, txtDBName.Text);
                }
                else if (pageViewMain.SelectedPage == pageGenerateDefaultEntities)
                {
                    if (dtgDefaultEntity.DataSource == null)
                        MyFormHelper.GetEntities(txtServerName.Text, txtDBName.Text);
                }
                //else if (pageViewMain.SelectedPage == pageImportRelationships)
                //{
                //    if (dtgRelationships.DataSource == null)
                //        MyFormHelper.GetRelationships(txtServerName.Text, txtDBName.Text);
                //}
                else if (pageViewMain.SelectedPage == pageImposeEntityRules)
                {
                    if (dtgRuleEntity.DataSource == null)
                        MyFormHelper.GetRuleEntities(txtServerName.Text, txtDBName.Text);
                }
                else if (pageViewMain.SelectedPage == pageImposeRelationshipRules)
                {
                    if (dtgRuleRelationships.DataSource == null)
                        MyFormHelper.GetRuleRelationships(txtServerName.Text, txtDBName.Text);
                }
        }

        private void btnUpdateManyToMany_Click(object sender, EventArgs e)
        {
            btnUpdateManyToMany.Enabled = false;
            MyFormHelper.UpdateManyToManyRelationships(dtgManyToMany.DataSource);
            btnUpdateManyToMany.Enabled = true;
        }
        private void btnRefreshManyToMany_Click(object sender, EventArgs e)
        {
            MyFormHelper.GetManyToManyRelationships(txtServerName.Text, txtDBName.Text);
        }
        internal void SetManyToManyRelationships(List<ManyToMany> ManyToManyList)
        {
            btnUpdateManyToMany.Enabled = true;
            dtgManyToMany.DataSource = ManyToManyList;
        }

        void dtgManyToMany_SelectionChanged(object sender, EventArgs e)
        {
            if (dtgManyToMany.SelectedRows.Count != 0)
            {
                var selectedRow = dtgManyToMany.SelectedRows[0];
                ManyToMany manyToMany = selectedRow.DataBoundItem as ManyToMany;
                if (manyToMany != null)
                {
                    MyFormHelper.GetManyToMany_ManyToOneRelationships(manyToMany.ID);
                }
            }
        }


        internal void SetManyToMany_ManyToOneRelationships(List<ManyToOne> ManyToOneList)
        {
            dtgManyToMany_ManyToOne.DataSource = ManyToOneList;
        }

        private void btnImportKeyValues_Click(object sender, EventArgs e)
        {


            var selectedRow = dtgColumns.SelectedRows[0];
            Column column = selectedRow.DataBoundItem as Column;
            if (column != null)
            {
                btnImportKeyValues.Enabled = false;
                var connectionString = SQLServerHelper.GetConnectionString(txtServerName.Text, txtDBName.Text, txtUserName.Text, txtPassword.Text);
                MyFormHelper.ImportColumnKeyValues(connectionString,column.ID,chkValueFromKeyOrValue.Checked);
                btnImportKeyValues.Enabled = true;
            }
        }
    }
    public enum GridViewColumnType
    {
        Text,
        Numeric,
        CheckBox,
        Command,
        Link
    }
}
