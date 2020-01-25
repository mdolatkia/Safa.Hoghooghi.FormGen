using DataAccess;
using System;
using System.Collections.Generic;
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
    public partial class frmColumnValue : Form
    {
        int ColumnID { set; get; }
        public frmColumnValue(int columnID)
        {
            InitializeComponent();
            ColumnID = columnID;

            SetColumnValueGrid();


        }

        private void SetColumnValueGrid()
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var column = projectContext.Column.First(x => x.ID == ColumnID);
                dtgColumnValues.DataSource = column.ColumnValue.ToList();

                var col = dtgColumnValue_Columns.Columns[0] as GridViewComboBoxColumn;
                col.DataSource = column.Table.Column.ToList();
                col.DisplayMember = "Name";
                col.ValueMember = "ID";

                var rel = dtgColumnValue_Relationships.Columns[0] as GridViewComboBoxColumn;

                List<Relationship> listRelationships = new List<Relationship>();
                foreach (var item in column.Table.TableDrivedEntity)
                {
                    foreach (var relationship in item.Relationship)
                        listRelationships.Add(relationship);
                    foreach (var relationship in item.Relationship1)
                        listRelationships.Add(relationship);

                }
                rel.DataSource = listRelationships;
                rel.DisplayMember = "Name";
                rel.ValueMember = "ID";
            }
        }


        private void dtgColumnValues_SelectionChanged(object sender, EventArgs e)
        {
            if (dtgColumnValues.SelectedRows.Count > 0)
            {
                var ColumnValue = dtgColumnValues.SelectedRows.First().DataBoundItem as ColumnValue;
                if (ColumnValue.ID == 0)
                {
                    pageRuleOnColumns.Enabled = false;
                    pageRuleOnRelationships.Enabled = false;
                }
                else
                {
                    pageRuleOnColumns.Enabled = true;
                    pageRuleOnRelationships.Enabled = true;
                }
                using (var projectContext = new DataAccess.MyProjectEntities())
                {
                    var listCol = projectContext.ColumnValue_Column.Where(x => x.ColumnValueID == ColumnValue.ID);
                    dtgColumnValue_Columns.DataSource = listCol.ToList();

                    var listRel = projectContext.ColumnValue_Relationship.Where(x => x.ColumnValueID == ColumnValue.ID);
                    dtgColumnValue_Relationships.DataSource = listRel.ToList();
                }

            }
            else
            {
                pageRuleOnColumns.Enabled = false;
                pageRuleOnRelationships.Enabled = false;
            }
        }

        private void btnUpdateColumnValue_Columns_Click(object sender, EventArgs e)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var list = dtgColumnValue_Columns.DataSource as List<ColumnValue_Column>;
                var ColumnValue = dtgColumnValues.SelectedRows.First().DataBoundItem as ColumnValue;
                projectContext.ColumnValue_Column.RemoveRange(projectContext.ColumnValue_Column.Where(x => x.ColumnValueID == ColumnValue.ID));
                foreach (var item in list)
                {
                    projectContext.ColumnValue_Column.Add(new ColumnValue_Column() { ColumnValueID = ColumnValue.ID, ColumnID = item.ColumnID, ValidValue = item.ValidValue });
                }
                projectContext.SaveChanges();

                SetColumnValueGrid();
            }
        }

        private void btnUpdateColumnValue_Click(object sender, EventArgs e)
        {

            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var item in dtgColumnValues.DataSource as List<ColumnValue>)
                {
                    ColumnValue dbItem = null;
                    if (item.ID == 0)
                    {
                        dbItem = new ColumnValue();
                        projectContext.ColumnValue.Add(dbItem);
                    }
                    else
                        dbItem = projectContext.ColumnValue.First(x => x.ID == item.ID);
                    dbItem.Value = item.Value;
                    dbItem.ColumnID = ColumnID;
                    projectContext.SaveChanges();
                }
            }
            SetColumnValueGrid();

        }

        private void btnUpdateColumnValue_Relationships_Click(object sender, EventArgs e)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var list = dtgColumnValue_Relationships.DataSource as List<ColumnValue_Relationship>;
                var ColumnValue = dtgColumnValues.SelectedRows.First().DataBoundItem as ColumnValue;
                projectContext.ColumnValue_Relationship.RemoveRange(projectContext.ColumnValue_Relationship.Where(x => x.ColumnValueID == ColumnValue.ID));

                var rel = dtgColumnValue_Relationships.Columns[0] as GridViewComboBoxColumn;
                var allRelationships = rel.DataSource as List<Relationship>;
                List<ColumnValue_Relationship> addList = new List<ColumnValue_Relationship>();
                List<int> visitedIds = new List<int>();
                foreach (var item in list)
                {
                    if (!visitedIds.Contains(item.RelationshipID))
                    {
                        visitedIds.Add(item.RelationshipID);
                        var pariItem = allRelationships.First(x => x.ID == item.RelationshipID);
                        if (pariItem != null)
                        {
                            if (pariItem.RelationshipID != null)
                            {
                                visitedIds.Add(pariItem.RelationshipID.Value);
                                if (!list.Any(x => x.RelationshipID == pariItem.RelationshipID))
                                {
                                    addList.Add(new ColumnValue_Relationship() { RelationshipID = pariItem.RelationshipID.Value, Enabled = item.Enabled });
                                }
                                else
                                    list.First(x => x.RelationshipID == pariItem.RelationshipID).Enabled = item.Enabled;
                            }
                            else
                            {
                                var revRel = allRelationships.First(x => x.RelationshipID == pariItem.ID);

                                visitedIds.Add(revRel.ID);
                                if (!list.Any(x => x.RelationshipID == revRel.ID))
                                {
                                    addList.Add(new ColumnValue_Relationship() { RelationshipID = revRel.ID, Enabled = item.Enabled });
                                }
                                else
                                    list.First(x => x.RelationshipID == revRel.ID).Enabled = item.Enabled;
                            }
                        }
                    }
                }

                foreach (var item in addList)
                {
                    list.Add(item);
                }
                foreach (var item in list)
                {
                    projectContext.ColumnValue_Relationship.Add(new ColumnValue_Relationship() { ColumnValueID = ColumnValue.ID, RelationshipID = item.RelationshipID, Enabled = item.Enabled });
                }
                projectContext.SaveChanges();

                SetColumnValueGrid();
            }
        }
    }
}
