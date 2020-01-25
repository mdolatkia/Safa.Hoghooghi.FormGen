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
    public partial class frmArcRelationships : Form
    {
        int TableDrivedEntityID { set; get; }
        public frmArcRelationships(int tableDrivedEntityID)
        {
            InitializeComponent();
            TableDrivedEntityID = tableDrivedEntityID;

            SetArcGroupGrid();


        }

        private void SetArcGroupGrid()
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var entity = projectContext.TableDrivedEntity.First(x => x.ID == TableDrivedEntityID);
                dtgArcGroup.DataSource = entity.ArcRelationshipGroup.ToList();

                List<Relationship> listRelationships = new List<Relationship>();

                foreach (var relationship in entity.Relationship)
                    listRelationships.Add(relationship);
                foreach (var relationship in entity.Relationship1)
                    listRelationships.Add(relationship);

                var rel = dtgArcRelationships.Columns[0] as GridViewComboBoxColumn;
                rel.DataSource = listRelationships;
                rel.DisplayMember = "Name";
                rel.ValueMember = "ID";
            }
        }


        private void dtgRuleOnValues_SelectionChanged(object sender, EventArgs e)
        {
            if (dtgArcGroup.SelectedRows.Count > 0)
            {
                var arcGroup = dtgArcGroup.SelectedRows.First().DataBoundItem as ArcRelationshipGroup;
                if (arcGroup.ID == 0)
                {
                    btnUpdateArcRelationships.Enabled = false;
                    dtgArcRelationships.Enabled = false;
                }
                else
                {
                    btnUpdateArcRelationships.Enabled = true;
                    dtgArcRelationships.Enabled = true;
                }
                using (var projectContext = new DataAccess.MyProjectEntities())
                {
                    var listRel = projectContext.ArcRelationshipGroup_Relationship.Where(x => x.ArcRelationshipGroupID == arcGroup.ID);
                    dtgArcRelationships.DataSource = listRel.ToList();
                }

            }
            else
            {
                btnUpdateArcRelationships.Enabled = false;
                dtgArcRelationships.Enabled = false;
            }
        }



        private void btnUpdateRuleOnValue_Click(object sender, EventArgs e)
        {

            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var item in dtgArcGroup.DataSource as List<ArcRelationshipGroup>)
                {
                    ArcRelationshipGroup dbItem = null;
                    if (item.ID == 0)
                    {
                        dbItem = new ArcRelationshipGroup();
                        projectContext.ArcRelationshipGroup.Add(dbItem);
                    }
                    else
                        dbItem = projectContext.ArcRelationshipGroup.First(x => x.ID == item.ID);
                    dbItem.GroupName = item.GroupName;
                    dbItem.TableDrivedEntityID = TableDrivedEntityID;
                    projectContext.SaveChanges();
                }
            }
            SetArcGroupGrid();

        }

        private void btnUpdateArcRelationships_Click(object sender, EventArgs e)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var list = dtgArcRelationships.DataSource as List<ArcRelationshipGroup_Relationship>;
                var arcGroup = dtgArcGroup.SelectedRows.First().DataBoundItem as ArcRelationshipGroup;
                projectContext.ArcRelationshipGroup_Relationship.RemoveRange(projectContext.ArcRelationshipGroup_Relationship.Where(x => x.ArcRelationshipGroupID == arcGroup.ID));

                var rel = dtgArcRelationships.Columns[0] as GridViewComboBoxColumn;
                var allRelationships = rel.DataSource as List<Relationship>;
                List<ArcRelationshipGroup_Relationship> addList = new List<ArcRelationshipGroup_Relationship>();
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
                                    addList.Add(new ArcRelationshipGroup_Relationship() { RelationshipID = pariItem.RelationshipID.Value });
                                }

                            }
                            else
                            {
                                var revRel = allRelationships.First(x => x.RelationshipID == pariItem.ID);

                                visitedIds.Add(revRel.ID);
                                if (!list.Any(x => x.RelationshipID == revRel.ID))
                                {
                                    addList.Add(new ArcRelationshipGroup_Relationship() { RelationshipID = revRel.ID });
                                }

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
                    projectContext.ArcRelationshipGroup_Relationship.Add(new ArcRelationshipGroup_Relationship() { ArcRelationshipGroupID = arcGroup.ID, RelationshipID = item.RelationshipID });
                }
                projectContext.SaveChanges();

                SetArcGroupGrid();
            }
        }
    }
}
