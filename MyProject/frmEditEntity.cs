using DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace MyProject
{
    public partial class frmEditEntity : Form
    {
        List<SimpleEntityDTO> DrivedEntities { set; get; }
        SimpleEntityDTO BaseEntity { set; get; }
        bool Inheritance { set; get; }
        public frmEditEntity(int entityID, bool inheritance)
        {
            InitializeComponent();
            Inheritance = inheritance;
            if (Inheritance)
            {
                //btnCopyColumn.Enabled = false;
                //btnCopyColumnReverse.Enabled = false;

            }
            else
            {
                pnlDisjoint.Visible = false;
                pnlParticipation.Visible = false;
            }
            SetGridViewColumns(dtgColumns);
            SetGridViewColumns(dtgRelationships);
            SetGridViewColumns(dtgColumnsDrived);
            SetGridViewColumns(dtgRelationshipsDrived);
            dtgRelationships.RowFormatting += dtgRelationships_RowFormatting;
            SetEntities(entityID);
            //using (var projectContext = new DataAccess.MyProjectEntities())
            //{
            //    var mainEntity = projectContext.TableDrivedEntity.First(x => x.ID == entityID);

            //    if (string.IsNullOrEmpty(mainEntity.TableDrivedEntity.Criteria))
            //    {
            //        SetEntities(mainEntity.ID, 0);

            //    }
            //    else
            //    {

            //        var dbBaseEntity = projectContext.TableDrivedEntity.First(x => (x.TableDrivedEntity.Criteria == null || x.TableDrivedEntity.Criteria == "") && x.TableDrivedEntity.TableID == mainEntity.TableDrivedEntity.TableID);
            //        SetEntities(dbBaseEntity.ID, mainEntity.ID);
            //    }
            //}



        }

        private void SetEntities(int entityID)
        {
            dtgRelationships.DataSource = new ObservableCollection<RelationshipDTO>();
            dtgColumns.DataSource = new ObservableCollection<ColumnDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                TableDrivedEntity baseEntity = null;
                var entity = projectContext.TableDrivedEntity.First(x => x.ID == entityID);

                if (string.IsNullOrEmpty(entity.Criteria))
                    baseEntity = entity;
                else
                    baseEntity = projectContext.TableDrivedEntity.First(x => (x.Criteria == null || x.Criteria == "") && x.TableID == entity.TableID);

                BaseEntity = ToSimpleEntityDTO(baseEntity);
                lblEntityName.Text = BaseEntity.Name;



                ObservableCollection<ColumnDTO> columns = new ObservableCollection<ColumnDTO>();
                foreach (var dbColumn in baseEntity.Table.Column)
                    columns.Add(GeneralHelper.ToColumnDTO(dbColumn));
                dtgColumns.DataSource = columns;

                ObservableCollection<RelationshipDTO> relationships = new ObservableCollection<RelationshipDTO>();
                foreach (var dbRelationship in projectContext.Relationship.Where(x => x.TableDrivedEntity.TableID == BaseEntity.TableID
                    &&
                    !(x.TableDrivedEntity.TableID == x.TableDrivedEntity1.TableID && (x.RelationshipType.SuperToSubRelationshipType != null || x.RelationshipType.SubToSuperRelationshipType != null))
                    ))
                    relationships.Add(GeneralHelper.ToRelationshipDTO(dbRelationship));
                dtgRelationships.DataSource = relationships;


                DrivedEntities = new List<SimpleEntityDTO>();
                DrivedEntities.Add(BaseEntity);
                if (Inheritance)
                {
                    foreach (var drived in projectContext.TableDrivedEntity.Where(x => x.ID != BaseEntity.ID && x.TableID == BaseEntity.TableID
                        && x.Relationship.Any(y => y.TableDrivedEntityID1 == x.ID && y.TableDrivedEntityID2 == BaseEntity.ID && y.RelationshipType != null && y.RelationshipType.SubToSuperRelationshipType != null)))
                    {
                        DrivedEntities.Add(ToSimpleEntityDTO(drived));
                        var isaRelationship = drived.Relationship.First(x => x.TableDrivedEntityID2 == BaseEntity.ID && x.RelationshipType.SubToSuperRelationshipType != null).RelationshipType.SubToSuperRelationshipType.ISARelationship;
                        optIsTolatParticipation.Checked = isaRelationship.IsTolatParticipation;
                        optIsDisjoint.Checked = isaRelationship.IsDisjoint == true;
                    }
                }
                else
                {
                    foreach (var drived in projectContext.TableDrivedEntity.Where(x => x.ID != BaseEntity.ID && x.TableID == BaseEntity.TableID))
                    {
                        DrivedEntities.Add(ToSimpleEntityDTO(drived));
                    }
                }

                cmbDrivedEntities.DisplayMember = "Name";
                cmbDrivedEntities.ValueMember = "ID";
                //cmbDrivedEntities.SelectedIndexChanged -= new System.EventHandler(this.cmbDrivedEntities_SelectedIndexChanged);
                cmbDrivedEntities.DataSource = DrivedEntities;
                //if (defaultEntityID != 0)
                cmbDrivedEntities.SelectedValue = entityID;
                cmbDrivedEntities_SelectionChangeCommitted(null, null);
                //if (DrivedEntities.Count > 0)
                //{
                //    if (DrivedEntities.Any(x => x.ID == mainEntity.ID))
                //        SetDrivedEntites(mainEntity.ID);
                //    else
                //        SetDrivedEntites(0);
                //}
                //else
                //{


                //}
            }
        }

        //private void SetDrivedEntites(int defaultEntityID)
        //{





        //}

        private void cmbDrivedEntities_SelectionChangeCommitted(object sender, EventArgs e)
        {
            txtName.Text = "";
            txtCriteria.Text = "";
            dtgRelationshipsDrived.DataSource = new ObservableCollection<RelationshipDTO>();
            dtgColumnsDrived.DataSource = new ObservableCollection<ColumnDTO>();

            var id = (cmbDrivedEntities.SelectedValue == null ? 0 : (int)cmbDrivedEntities.SelectedValue);
            var drivedEntity = DrivedEntities.FirstOrDefault(x => x.ID == id);
            if (drivedEntity != null)
            {
                txtName.Text = drivedEntity.Name;
                txtCriteria.Text = drivedEntity.Criteria;
                using (var projectContext = new DataAccess.MyProjectEntities())
                {
                    var dbDrivedEntity = projectContext.TableDrivedEntity.First(x => x.ID == drivedEntity.ID);



                    ObservableCollection<RelationshipDTO> drivedRelationships = new ObservableCollection<RelationshipDTO>();
                    foreach (var dbRelationship in dbDrivedEntity.Relationship.Where(
                        x => !(x.TableDrivedEntity.TableID == x.TableDrivedEntity1.TableID && (x.RelationshipType.SuperToSubRelationshipType != null || x.RelationshipType.SubToSuperRelationshipType != null))
                        ))

                        drivedRelationships.Add(GeneralHelper.ToRelationshipDTO(dbRelationship));
                    dtgRelationshipsDrived.DataSource = drivedRelationships;

                    ObservableCollection<ColumnDTO> drivedColumns = new ObservableCollection<ColumnDTO>();
                    foreach (var dbColumn in dbDrivedEntity.Column)
                        drivedColumns.Add(GeneralHelper.ToColumnDTO(dbColumn));
                    dtgColumnsDrived.DataSource = drivedColumns;
                }

            }

        }
        //private void cmbDrivedEntities_SelectedIndexChanged(object sender, EventArgs e)
        //{


        //}
        //void dtgRelationships_ValueChanged(object sender, EventArgs e)
        //{
        //    if (this.dtgRelationships.ActiveEditor is RadCheckBoxEditor)
        //    {
        //        if (dtgRelationships.CurrentCell.RowInfo.DataBoundItem is RelationshipDTO)
        //        {
        //            CheckColumnThroughRelationship((dtgRelationships.CurrentCell.RowInfo.DataBoundItem as RelationshipDTO), Convert.ToBoolean(dtgRelationships.ActiveEditor.Value));

        //        }
        //    }
        //}

        //private void CheckColumnThroughRelationship(RelationshipDTO relationshipDTO, bool value)
        //{
        //    if (value)
        //    {
        //        using (var projectContext = new DataAccess.MyProjectEntities())
        //        {
        //            var dbRelationship = projectContext.Relationship.First(x => x.ID == relationshipDTO.ID);
        //            foreach (var relColumn in dbRelationship.RelationshipColumns)
        //            {

        //            }
        //        }
        //    }
        //}



        void dtgRelationships_RowFormatting(object sender, RowFormattingEventArgs e)
        {
            if (e.RowElement.RowInfo.DataBoundItem != null)
            {
                if (e.RowElement.RowInfo.DataBoundItem is RelationshipDTO)
                    if ((e.RowElement.RowInfo.DataBoundItem as RelationshipDTO).Enabled == false)
                        e.RowElement.Enabled = false;
            }
        }
        private void SetGridViewColumns(RadGridView dataGrid)
        {
            if (dataGrid == dtgColumns || (dataGrid == dtgColumnsDrived))
            {

                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Select", "Select", false, 50, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 20, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsNull", "IsNull", true, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsMandatory", "IsMandatory", true, 100, GridViewColumnType.CheckBox));
            }
            else if (dataGrid == dtgRelationships || (dataGrid == dtgRelationshipsDrived))
            {

                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Select", "Select", false, 50, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 20, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", true, 100, GridViewColumnType.Text));
                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "Pair Relationship ID", true, 100, GridViewColumnType.Text));
                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "Pair Relationship", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "Entity1", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "Entity2", true, 100, GridViewColumnType.Text));
                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("RelationshipColumns", "RelationshipColumns", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Type", "Type", true, 100, GridViewColumnType.Text));
                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataEntryEnabled", "DataEntryEnabled", true, 100, GridViewColumnType.CheckBox));
                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SearchEnabled", "SearchEnabled", true, 100, GridViewColumnType.CheckBox));
                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ViewEnabled", "ViewEnabled", true, 100, GridViewColumnType.CheckBox));
                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "Enabled", true, 100, GridViewColumnType.CheckBox));
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //ستونهای رابطه برای مشتقها از موجودیت پایه حذف شوند
            //کد نوشته شود
            if (txtName.Text == "")
            {
                MessageBox.Show("نام موجودیت مشخص نشده است", "نام موجودیت");
                return;
            }
            if ((cmbDrivedEntities.SelectedValue == null ? 0 : (int)cmbDrivedEntities.SelectedValue) == BaseEntity.ID)
            {
                if (txtCriteria.Text != "")
                {
                    MessageBox.Show("برای موجودیت پایه شرط نمیتوان تعریف نمود", "شرط موجودیت");
                    return;
                }
            }
            else
            {
                if (txtCriteria.Text == "")
                {
                    MessageBox.Show("شرط موجودیت مشخص نشده است", "شرط موجودیت");
                    return;
                }
            }
            if (Inheritance)
            {
                if (!optIsDisjoint.Checked && !optIsOverlap.Checked)
                {
                    MessageBox.Show("نوع رابطه ارث بری مشخص نشده است", "Disjoint or Overlap");
                    return;
                }
                if (!optIsTolatParticipation.Checked && !optIsPartialParticipation.Checked)
                {
                    MessageBox.Show("نوع رابطه ارث بری مشخص نشده است", "TolatParticipation or PartialParticipatio");
                    return;
                }
            }
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                if (!Inheritance || (cmbDrivedEntities.SelectedValue == null ? 0 : (int)cmbDrivedEntities.SelectedValue) == BaseEntity.ID)
                {
                    var columnMessage = "";
                    var baseColumns = dtgColumns.DataSource as ObservableCollection<ColumnDTO>;
                    var derivedColumns = dtgColumnsDrived.DataSource as ObservableCollection<ColumnDTO>;
                    List<ColumnDTO> unSelectedAndNotNullable = new List<ColumnDTO>();
                    foreach (var column in baseColumns)
                    {
                        var dbColumn = projectContext.Column.First(x => x.ID == column.ID);
                        if (dbColumn.IsNull == false)
                        {
                            if (!derivedColumns.Any(x => x.ID == column.ID))
                            {
                                unSelectedAndNotNullable.Add(column);
                                columnMessage += (columnMessage == "" ? "" : Environment.NewLine) + "ستون " + column.Name;
                            }
                        }
                    }
                    if (columnMessage != "")
                    {
                        var message = " ستونهای زیر مقادیر اجباری میگیرند و انتخاب آنها اجباری میباشد" + Environment.NewLine + columnMessage;
                        MessageBox.Show(message, "ستونهای اجباری");
                        return;
                    }

                }








                //انتخاب ستونهای روابط 

                //if (Duplicate)
                //{
                //var baseEntity = projectContext.TableDrivedEntity.First(x => x.ID == BaseEntity.ID);
                //baseEntity.Column.Clear();
                //foreach (var item in dtgColumns.DataSource as ObservableCollection<ColumnDTO>)
                //{
                //    baseEntity.Column.Add(projectContext.Column.First(x => x.ID == item.ID));
                //}






                var id = (cmbDrivedEntities.SelectedValue == null ? 0 : (int)cmbDrivedEntities.SelectedValue);
                var drivedEntity = projectContext.TableDrivedEntity.FirstOrDefault(x => x.ID == id);
                if (drivedEntity == null)
                {
                    drivedEntity = new TableDrivedEntity();
                  
                    drivedEntity.IndependentDataEntry = BaseEntity.IndependentDataEntry;
                    drivedEntity.TableID = BaseEntity.TableID;
                }
                drivedEntity.Name = txtName.Text;
                drivedEntity.Criteria = txtCriteria.Text;
                drivedEntity.Column.Clear();
                foreach (var item in dtgColumnsDrived.DataSource as ObservableCollection<ColumnDTO>)
                {
                    drivedEntity.Column.Add(projectContext.Column.First(x => x.ID == item.ID));
                }
                foreach (var item in dtgRelationshipsDrived.DataSource as ObservableCollection<RelationshipDTO>)
                {

                    Relationship dbReverseRelationship = null;
                    var dbRelationship = projectContext.Relationship.First(x => x.ID == item.ID);
                    if (dbRelationship.Relationship2 != null)
                        dbReverseRelationship = dbRelationship.Relationship2;
                    else
                        dbReverseRelationship = projectContext.Relationship.First(x => x.RelationshipID == item.ID);

                    if (dbRelationship.TableDrivedEntity.TableID == BaseEntity.TableID)
                    {
                        if (dbRelationship.Relationship2 == null)
                            dbRelationship.Name = dbRelationship.Name.Replace("(PK)" + dbRelationship.TableDrivedEntity.Name + ".", "(PK)" + drivedEntity.Name + ".");
                        else
                            dbRelationship.Name = dbRelationship.Name.Replace("(FK)" + dbRelationship.TableDrivedEntity.Name + ".", "(FK)" + drivedEntity.Name + ".");

                        dbRelationship.TableDrivedEntity = drivedEntity;
                    }
                    if (dbRelationship.TableDrivedEntity1.TableID == BaseEntity.TableID)
                    {
                        if (dbRelationship.Relationship2 == null)
                            dbRelationship.Name = dbRelationship.Name.Replace("(FK)" + dbRelationship.TableDrivedEntity.Name + ".", "(FK)" + drivedEntity.Name + ".");
                        else
                            dbRelationship.Name = dbRelationship.Name.Replace("(PK)" + dbRelationship.TableDrivedEntity.Name + ".", "(PK)" + drivedEntity.Name + ".");
                        dbRelationship.TableDrivedEntity1 = drivedEntity;
                    }

                    if (dbReverseRelationship.TableDrivedEntity.TableID == BaseEntity.TableID)
                    {
                        if (dbReverseRelationship.Relationship2 == null)
                            dbReverseRelationship.Name = dbReverseRelationship.Name.Replace("(PK)" + dbRelationship.TableDrivedEntity.Name + ".", "(PK)" + drivedEntity.Name + ".");
                        else
                            dbReverseRelationship.Name = dbReverseRelationship.Name.Replace("(FK)" + dbRelationship.TableDrivedEntity.Name + ".", "(FK)" + drivedEntity.Name + ".");
                        dbReverseRelationship.TableDrivedEntity = drivedEntity;
                    }
                    if (dbReverseRelationship.TableDrivedEntity1.TableID == BaseEntity.TableID)
                    {
                        if (dbReverseRelationship.Relationship2 == null)
                            dbReverseRelationship.Name = dbReverseRelationship.Name.Replace("(FK)" + dbRelationship.TableDrivedEntity.Name + ".", "(FK)" + drivedEntity.Name + ".");
                        else
                            dbReverseRelationship.Name = dbReverseRelationship.Name.Replace("(PK)" + dbRelationship.TableDrivedEntity.Name + ".", "(PK)" + drivedEntity.Name + ".");
                        dbReverseRelationship.TableDrivedEntity1 = drivedEntity;
                    }



                }
                if (Inheritance && !((cmbDrivedEntities.SelectedValue == null ? 0 : (int)cmbDrivedEntities.SelectedValue) == BaseEntity.ID))
                {
                    DataAccess.ISARelationship isaRelationship = null;
                    var existingEntity = DrivedEntities.FirstOrDefault(x => x.ID != BaseEntity.ID && x.ID != 0);
                    if (existingEntity != null)
                    {
                        var sampleSuperToSub = projectContext.Relationship.FirstOrDefault(x => x.TableDrivedEntityID1 == BaseEntity.ID && x.TableDrivedEntityID2 == existingEntity.ID && x.RelationshipType != null &&
                            x.RelationshipType.SuperToSubRelationshipType != null);
                        if (sampleSuperToSub != null)
                            isaRelationship = sampleSuperToSub.RelationshipType.SuperToSubRelationshipType.ISARelationship;
                        else
                            throw (new Exception("ISARelationship cound not be found!"));
                    }

                    if (isaRelationship == null)
                    {
                        isaRelationship = new ISARelationship();
                    }
                    isaRelationship.IsTolatParticipation = optIsTolatParticipation.Checked == true;
                    isaRelationship.IsDisjoint = optIsDisjoint.Checked == true;

                    string subTypesStr = "";
                    foreach (var drivedentity in DrivedEntities)
                    {
                        if (drivedentity.ID != BaseEntity.ID)
                            subTypesStr += (subTypesStr == "" ? "" : ",") + drivedentity.Name;
                    }
                    if (drivedEntity.ID == 0)
                        subTypesStr += (subTypesStr == "" ? "" : ",") + drivedEntity.Name;
                    isaRelationship.Name = BaseEntity.Name + ">" + subTypesStr;
                    if (drivedEntity.ID == 0)
                    {

                        var relationship = new Relationship();
                        relationship.RelationshipType = new RelationshipType();
                        relationship.RelationshipType.SuperToSubRelationshipType = new SuperToSubRelationshipType();
                        relationship.RelationshipType.IsOtherSideCreatable = true;
                        relationship.TableDrivedEntityID1 = BaseEntity.ID;
                        relationship.RelationshipType.SuperToSubRelationshipType.ISARelationship = isaRelationship;
                        drivedEntity.Relationship1.Add(relationship);








                        var relationshipReverse = new Relationship();
                        relationshipReverse.RelationshipType = new RelationshipType();
                        relationshipReverse.RelationshipType.SubToSuperRelationshipType = new SubToSuperRelationshipType();
                        relationshipReverse.RelationshipType.IsOtherSideCreatable = true;
                        relationshipReverse.TableDrivedEntityID2 = BaseEntity.ID;
                        relationshipReverse.RelationshipType.SubToSuperRelationshipType.ISARelationship = isaRelationship;
                        relationshipReverse.Relationship2 = relationship;
                        drivedEntity.Relationship.Add(relationshipReverse);

                        var dbBaseEntity = projectContext.TableDrivedEntity.First(x => x.ID == BaseEntity.ID);
                        string PKColumns = "";
                        string FKColumns = "";
                        foreach (var primaryCol in dbBaseEntity.Table.Column.Where(x => x.PrimaryKey == true))
                        {
                            PKColumns += (PKColumns == "" ? "" : ",") + primaryCol.Name;
                            FKColumns += (FKColumns == "" ? "" : ",") + primaryCol.Name;
                            relationship.RelationshipColumns.Add(new RelationshipColumns() { Column = primaryCol, Column1 = primaryCol });
                            relationshipReverse.RelationshipColumns.Add(new RelationshipColumns() { Column = primaryCol, Column1 = primaryCol });
                        }
                        relationship.Name = "(PK)" + BaseEntity.Name + "." + PKColumns + ">(FK)" + drivedEntity.Name + "." + FKColumns;
                        relationshipReverse.Name = "(FK)" + drivedEntity.Name + "." + FKColumns + ">(PK)" + BaseEntity.Name + "." + PKColumns;

                    }

                }
                if (drivedEntity.ID == 0)
                    projectContext.TableDrivedEntity.Add(drivedEntity);
                projectContext.SaveChanges();
                SetEntities(drivedEntity.ID);
            }


            //this.Close();
            //}
            //else
            //{
            //    var editEntity = projectContext.TableDrivedEntity.First(x => x.ID == MainEntity.ID);
            //    editEntity.Column.Clear();
            //    foreach (var item in dtgColumns.DataSource as List<ColumnDTO>)
            //    {
            //        if (item.Select)
            //            editEntity.Column.Add(projectContext.Column.First(x => x.ID == item.ID));
            //    }
            //    if (tabControl1.TabPages.Contains(tabRelationship))
            //    {
            //        foreach (var item in dtgRelationships.DataSource as List<RelationshipDTO>)
            //        {
            //            if (item.Select)
            //            {
            //                Relationship dbReverseRelationship = null;
            //                var dbRelationship = projectContext.Relationship.First(x => x.ID == item.ID);
            //                if (dbRelationship.Relationship2 != null)
            //                    dbReverseRelationship = dbRelationship.Relationship2;
            //                else
            //                    dbReverseRelationship = projectContext.Relationship.First(x => x.RelationshipID == item.ID);

            //                if (dbRelationship.TableDrivedEntity.TableDrivedEntity.TableID == MainEntity.TableID)
            //                {
            //                    if (dbRelationship.Relationship2 == null)
            //                        dbRelationship.Name = dbRelationship.Name.Replace("(PK)" + dbRelationship.TableDrivedEntity.Name + ".", "(PK)" + MainEntity.Name + ".");
            //                    else
            //                        dbRelationship.Name = dbRelationship.Name.Replace("(FK)" + dbRelationship.TableDrivedEntity.Name + ".", "(FK)" + MainEntity.Name + ".");
            //                    dbRelationship.TableDrivedEntity = editEntity;
            //                }
            //                if (dbRelationship.Entity1.TableDrivedEntity.TableID == MainEntity.TableID)
            //                {
            //                    if (dbRelationship.Relationship2 == null)
            //                        dbRelationship.Name = dbRelationship.Name.Replace("(FK)" + dbRelationship.TableDrivedEntity.Name + ".", "(FK)" + MainEntity.Name + ".");
            //                    else
            //                        dbRelationship.Name = dbRelationship.Name.Replace("(PK)" + dbRelationship.TableDrivedEntity.Name + ".", "(PK)" + MainEntity.Name + ".");
            //                    dbRelationship.Entity1 = editEntity;
            //                }
            //                if (dbReverseRelationship.TableDrivedEntity.TableDrivedEntity.TableID == MainEntity.TableID)
            //                {
            //                    if (dbReverseRelationship.Relationship2 == null)
            //                        dbReverseRelationship.Name = dbReverseRelationship.Name.Replace("(PK)" + dbRelationship.TableDrivedEntity.Name + ".", "(PK)" + MainEntity.Name + ".");
            //                    else
            //                        dbReverseRelationship.Name = dbReverseRelationship.Name.Replace("(FK)" + dbRelationship.TableDrivedEntity.Name + ".", "(FK)" + MainEntity.Name + ".");
            //                    dbReverseRelationship.TableDrivedEntity = editEntity;
            //                }
            //                if (dbReverseRelationship.Entity1.TableDrivedEntity.TableID == MainEntity.TableID)
            //                {
            //                    if (dbReverseRelationship.Relationship2 == null)
            //                        dbReverseRelationship.Name = dbReverseRelationship.Name.Replace("(FK)" + dbRelationship.TableDrivedEntity.Name + ".", "(FK)" + MainEntity.Name + ".");
            //                    else
            //                        dbReverseRelationship.Name = dbReverseRelationship.Name.Replace("(PK)" + dbRelationship.TableDrivedEntity.Name + ".", "(PK)" + MainEntity.Name + ".");
            //                    dbReverseRelationship.Entity1 = editEntity;
            //                }

            //            }
            //        }
            //    }


            //    //try
            //    //{
            //    //projectContext.SaveChanges();
            //    //}
            //    //catch (System.Data.TableDrivedEntity.Validation.DbEntityValidationException ed)
            //    //{
            //    //    foreach (var eve in ed.EntityValidationErrors)
            //    //    {
            //    //        Console.WriteLine("TableDrivedEntity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //    //            eve.Entry.TableDrivedEntity.GetType().Name, eve.Entry.State);
            //    //        foreach (var ve in eve.ValidationErrors)
            //    //        {
            //    //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //    //                ve.PropertyName, ve.ErrorMessage);
            //    //        }
            //    //    }
            //    //    throw;
            //    //}
            //    // this.Close();
            //}
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddRelationship_Click(object sender, EventArgs e)
        {
            if (dtgRelationships.SelectedRows.Count > 0)
            {
                var columns = dtgColumns.DataSource as ObservableCollection<ColumnDTO>;
                var selectedRelationshipRow = dtgRelationships.SelectedRows[0];
                var relationship = (selectedRelationshipRow.DataBoundItem as RelationshipDTO);

                using (var projectContext = new DataAccess.MyProjectEntities())
                {

                    var dbRelationship = projectContext.Relationship.First(x => relationship.ID == x.ID);




                    // برای موجودیتهای مشتق شده روابط اجباری نمیتوانند انتخاب شوند
                    string columnMessage = "";

                    if (dbRelationship.RelationshipColumns.Any(y => (y.Column.PrimaryKey == null || y.Column.PrimaryKey == false) && y.Column.IsNull == false))
                    {
                        columnMessage += (columnMessage == "" ? "" : Environment.NewLine) + "رابطه " + dbRelationship.Name;
                        var message = " این رابطه اجباری میباشند و امکان انتخاب شدن برای موجودیت های مشتق شده را ندارند" + Environment.NewLine + columnMessage;
                        MessageBox.Show(message, "روابط اجباری");
                        return;
                    }
                    //if 
                    //{
                    List<ColumnDTO> relationshipColumns = new List<ColumnDTO>();
                    foreach (var relCol in dbRelationship.RelationshipColumns)
                    {
                        var column = columns.First(x => x.ID == relCol.FirstSideColumnID);
                        if (column != null)
                            relationshipColumns.Add(column);
                    }
                    CopyColumn(relationshipColumns, false);
                    AddRelationship(relationship);
                    //}
                    //else
                    //{
                    //    List<ColumnDTO> relationshipColumns = new List<ColumnDTO>();
                    //    foreach (var relCol in dbRelationship.RelationshipColumns)
                    //    {
                    //        var column = columns.First(x => x.ID == relCol.ColumnID2);
                    //        if (column != null)
                    //            relationshipColumns.Add(column);
                    //    }
                    //    CopyColumn(relationshipColumns, false);
                    //}

                }


            }


        }
        private void button1_Click(object sender, EventArgs e)
        {
            //var baseRelationships = dtgRelationships.DataSource as ObservableCollection<RelationshipDTO>;
            //var derivedRelationship = dtgRelationshipsDrived.DataSource as ObservableCollection<RelationshipDTO>;
            if (dtgRelationshipsDrived.SelectedRows.Count > 0)
            {
                // ستون انتخاب شده اما رابطه معادل خیر!
                var relationship = dtgRelationshipsDrived.SelectedRows[0].DataBoundItem as RelationshipDTO;

                using (var projectContext = new DataAccess.MyProjectEntities())
                {
                    var dbRelationship = projectContext.Relationship.First(x => relationship.ID == x.ID);
                    //var baseColumns = dtgColumns.DataSource as ObservableCollection<ColumnDTO>;
                    //var derivedColumns = dtgColumnsDrived.DataSource as ObservableCollection<ColumnDTO>;
                    //foreach (var relCol in dbRelationship.RelationshipColumns)
                    //{
                    //    var column = derivedColumns.First(x => x.ID == relCol.ColumnID1);
                    //    if (!baseColumns.Any(x => x.ID == column.ID))
                    //        baseColumns.Add(column);
                    //    derivedColumns.Remove(column);
                    //}
                    //baseRelationships.Add(relationship);
                    //derivedRelationship.Remove(relationship);

                    var columns = dtgColumnsDrived.DataSource as ObservableCollection<ColumnDTO>;
                    List<ColumnDTO> relationshipColumns = new List<ColumnDTO>();
                    foreach (var relCol in dbRelationship.RelationshipColumns)
                    {
                        bool hasOtherRelation = false;
                        var otherRelationships = (dtgRelationshipsDrived.DataSource as ObservableCollection<RelationshipDTO>).Where(x => x.ID != relationship.ID);
                        foreach (var otherRelationship in otherRelationships)
                        {
                            var dbOtherRelationship = projectContext.Relationship.First(x => otherRelationship.ID == x.ID);
                            if (dbOtherRelationship.RelationshipColumns.Any(x => x.FirstSideColumnID == relCol.FirstSideColumnID))
                            {
                                hasOtherRelation = true;
                                break;
                            }
                        }
                        if (!hasOtherRelation)
                        {
                            var column = columns.First(x => x.ID == relCol.FirstSideColumnID);
                            if (column != null)
                                relationshipColumns.Add(column);
                        }
                    }
                    RemoveColumn(relationshipColumns, false);
                    RemoveRelationship(relationship);
                }


            }
        }
        private void AddRelationship(RelationshipDTO relationship)
        {
            // ObservableCollection<RelationshipDTO> source = null;
            var destination = dtgRelationshipsDrived.DataSource as ObservableCollection<RelationshipDTO>;
            //if (operationMode == OperationMode.MoveFromBaseToDrived)
            //{
            // source = dtgRelationships.DataSource as ObservableCollection<RelationshipDTO>;

            //}
            //else if (operationMode == OperationMode.MoveFromDrivedToBase)
            //{
            //    destination = dtgRelationships.DataSource as ObservableCollection<RelationshipDTO>;
            //    source = dtgRelationshipsDrived.DataSource as ObservableCollection<RelationshipDTO>;
            //}
            if (!destination.Any(x => x.ID == relationship.ID))
                destination.Add(relationship);
            //source.Remove(relationship);
        }
        private void RemoveRelationship(RelationshipDTO relationship)
        {
            // ObservableCollection<RelationshipDTO> source = null;
            var source = dtgRelationshipsDrived.DataSource as ObservableCollection<RelationshipDTO>;
            //if (operationMode == OperationMode.MoveFromBaseToDrived)
            //{
            // source = dtgRelationships.DataSource as ObservableCollection<RelationshipDTO>;

            //}
            //else if (operationMode == OperationMode.MoveFromDrivedToBase)
            //{
            //    destination = dtgRelationships.DataSource as ObservableCollection<RelationshipDTO>;
            //    source = dtgRelationshipsDrived.DataSource as ObservableCollection<RelationshipDTO>;
            //}

            //destination.Add(relationship);
            source.Remove(relationship);
        }

        private List<ColumnDTO> GetSelectedColumns(RadGridView dtg)
        {
            List<ColumnDTO> columns = new List<ColumnDTO>();
            if (dtg.SelectedRows.Count > 0)
            {
                // ستون انتخاب شده اما رابطه معادل خیر!
                var selectedColumnRows = dtg.SelectedRows;
                foreach (var row in selectedColumnRows)
                {
                    columns.Add(row.DataBoundItem as ColumnDTO);
                }
            }
            return columns;
        }
        private void btnAddColumn_Click(object sender, EventArgs e)
        {
            List<ColumnDTO> columns = GetSelectedColumns(dtgColumns);
            CopyColumn(columns, true);
        }


        private void btnMoveColumn_Click(object sender, EventArgs e)
        {
            List<ColumnDTO> columns = GetSelectedColumns(dtgColumnsDrived);
            RemoveColumn(columns, true);
            //List<ColumnDTO> columns = GetSelectedColumns(dtgColumns);
            //CopyColumn(columns, OperationMode.MoveFromBaseToDrived, true);
        }


        //private void btnCopyColumnReverse_Click(object sender, EventArgs e)
        //{
        //    //List<ColumnDTO> columns = GetSelectedColumns(dtgColumnsDrived);
        //    //CopyColumn(columns, OperationMode.CopyFromDrivedToBase, true);
        //}

        //private void btnMoveColumnReverse_Click(object sender, EventArgs e)
        //{
        //    //List<ColumnDTO> columns = GetSelectedColumns(dtgColumnsDrived);
        //    //CopyColumn(columns, OperationMode.MoveFromDrivedToBase, true);
        //}
        private void CopyColumn(List<ColumnDTO> columns, bool checkRelationshipColumns)
        {

            var relationships = dtgRelationships.DataSource as ObservableCollection<RelationshipDTO>;

            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var columnMessage = "";
                if (checkRelationshipColumns)
                {
                    var relationshipIDs = relationships.Select(y => y.ID).ToList();
                    var dbRelationships = projectContext.Relationship.Where(x => relationshipIDs.Contains(x.ID));


                    foreach (var column in columns)
                    {

                        var columnRelationship = dbRelationships.Where(x => x.RelationshipColumns.Any(y => (y.Column.PrimaryKey == null || y.Column.PrimaryKey == false) && column.ID == y.FirstSideColumnID));
                        if (columnRelationship.Any())
                        {
                            foreach (var dbRelationship in columnRelationship)
                                columnMessage += (columnMessage == "" ? "" : Environment.NewLine) + "ستون " + column.Name + " در رابطه " + dbRelationship.Name + " استفاده شده است ";
                        }
                    }
                    if (columnMessage != "")
                    {
                        var message = " ستونهای انتخاب شده زیر در روابط استفاده شده اند لطفا از افزودن روابط استفاده نمایید " + Environment.NewLine + columnMessage;
                        MessageBox.Show(message, "ستونهای روابط");
                        return;
                    }

                }


                //ستونهای اجباری نباید انتخاب شوند

                //if (!Inheritance)
                //{
                //    if (operationMode == OperationMode.MoveFromBaseToDrived || operationMode == OperationMode.MoveFromDrivedToBase)
                //    {
                //        columnMessage = "";
                //        List<ColumnDTO> unSelectedAndNotNullable = new List<ColumnDTO>();
                //        foreach (var column in columns)
                //        {
                //            var dbColumn = projectContext.Column.First(x => x.ID == column.ID);
                //            if (dbColumn.IsNull == false)
                //            {
                //                unSelectedAndNotNullable.Add(column);
                //                columnMessage += (columnMessage == "" ? "" : Environment.NewLine) + "ستون " + column.Name;
                //            }
                //        }
                //        if (columnMessage != "")
                //        {
                //            var message = " ستونهای زیر مقادیر اجباری میگیرند و انتخاب آنها امکانپذیر نمیباشد" + Environment.NewLine + columnMessage;
                //            MessageBox.Show(message, "ستونهای اجباری");
                //            return false;
                //        }
                //    }
                //}
            }
            foreach (var column in columns)
            {
                AddColumn(column);
            }






        }

        private void AddColumn(ColumnDTO column)
        {
            //var source = dtgColumns.DataSource as ObservableCollection<ColumnDTO>; ;
            var destination = dtgColumnsDrived.DataSource as ObservableCollection<ColumnDTO>; ;

            //source = 
            //destination = dtgColumnsDrived.DataSource as ObservableCollection<ColumnDTO>;
            //}
            //else
            //{
            //    destination = dtgColumns.DataSource as ObservableCollection<ColumnDTO>;
            //    source = dtgColumnsDrived.DataSource as ObservableCollection<ColumnDTO>;
            //}
            //if (operationMode == OperationMode.CopyFromBaseToDrived || operationMode == OperationMode.CopyFromDrivedToBase)
            //{
            if (!destination.Any(x => x.ID == column.ID))
                destination.Add(column);
            //  dtgColumnsDrived.Refresh();
            //}
            //else
            //{
            //    if (!destination.Any(x => x.ID == column.ID))
            //        destination.Add(column);
            //    source.Remove(column);
            //}

        }

        private bool RemoveColumn(List<ColumnDTO> columns, bool checkRelationshipColumns)
        {

            var relationships = dtgRelationshipsDrived.DataSource as ObservableCollection<RelationshipDTO>;

            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var columnMessage = "";
                if (checkRelationshipColumns)
                {
                    var relationshipIDs = relationships.Select(y => y.ID).ToList();
                    var dbRelationships = projectContext.Relationship.Where(x => relationshipIDs.Contains(x.ID));


                    foreach (var column in columns)
                    {

                        var columnRelationship = dbRelationships.Where(x => x.RelationshipColumns.Any(y => (y.Column.PrimaryKey == null || y.Column.PrimaryKey == false) && column.ID == y.FirstSideColumnID));
                        if (columnRelationship.Any())
                        {
                            foreach (var dbRelationship in columnRelationship)
                                columnMessage += (columnMessage == "" ? "" : Environment.NewLine) + "ستون " + column.Name + " در رابطه " + dbRelationship.Name + " استفاده شده است ";
                        }
                    }
                    if (columnMessage != "")
                    {
                        var message = " ستونهای انتخاب شده زیر در روابط استفاده شده اند لطفا از افزودن روابط استفاده نمایید " + Environment.NewLine + columnMessage;
                        MessageBox.Show(message, "ستونهای روابط");
                        return false;
                    }

                }


                //ستونهای اجباری نباید انتخاب شوند

                //if (!Inheritance)
                //{
                //    if (operationMode == OperationMode.MoveFromBaseToDrived || operationMode == OperationMode.MoveFromDrivedToBase)
                //    {
                //        columnMessage = "";
                //        List<ColumnDTO> unSelectedAndNotNullable = new List<ColumnDTO>();
                //        foreach (var column in columns)
                //        {
                //            var dbColumn = projectContext.Column.First(x => x.ID == column.ID);
                //            if (dbColumn.IsNull == false)
                //            {
                //                unSelectedAndNotNullable.Add(column);
                //                columnMessage += (columnMessage == "" ? "" : Environment.NewLine) + "ستون " + column.Name;
                //            }
                //        }
                //        if (columnMessage != "")
                //        {
                //            var message = " ستونهای زیر مقادیر اجباری میگیرند و انتخاب آنها امکانپذیر نمیباشد" + Environment.NewLine + columnMessage;
                //            MessageBox.Show(message, "ستونهای اجباری");
                //            return false;
                //        }
                //    }
                //}
            }
            foreach (var column in columns)
            {
                RemoveColumn(column);
            }
            return true;





        }
        private void RemoveColumn(ColumnDTO column)
        {
            var source = dtgColumnsDrived.DataSource as ObservableCollection<ColumnDTO>; ;
            //var destination = dtgColumnsDrived.DataSource as ObservableCollection<ColumnDTO>; ;
            source.Remove(column);

        }
        private void btnNewEntity_Click(object sender, EventArgs e)
        {
            cmbDrivedEntities.SelectedValue = 0;
            cmbDrivedEntities_SelectionChangeCommitted(null, null);
        }


        private SimpleEntityDTO ToSimpleEntityDTO(TableDrivedEntity item)
        {
            var result = new SimpleEntityDTO();
            result.ID = item.ID;
            result.Name = item.Name;
            result.TableID = item.TableID;
            result.Table = item.Table.Name;
            result.Alias = item.Alias; result.Criteria = item.Criteria;
            result.IndependentDataEntry = item.IndependentDataEntry;
            result.BatchDataEntry = item.BatchDataEntry;
            result.IsAssociative = item.IsAssociative;
            result.IsDataReference = item.IsDataReference;
            result.IsStructurReferencee = item.IsStructurReferencee;
            return result;

        }



    }

    //enum OperationMode
    //{
    //    CopyFromBaseToDrived,
    //    CopyFromDrivedToBase,
    //    MoveFromBaseToDrived,
    //    MoveFromDrivedToBase
    //}


}
