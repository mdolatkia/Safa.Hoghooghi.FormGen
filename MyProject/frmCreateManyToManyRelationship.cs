using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyProject
{
    public partial class frmCreateManyToManyRelationship : Form
    {
        public event EventHandler<ManyToManyCreatedArg> ManyToManyCreated;

        int TableID { set; get; }
        public frmCreateManyToManyRelationship(int tableID)
        {
            InitializeComponent();
            TableID = tableID;

         
            dtgList.EnableFiltering = true;
            dtgList.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
            dtgList.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
            dtgList.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "Pair Relationship ID", true, 100, GridViewColumnType.Text));
            dtgList.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "Pair Relationship", true, 100, GridViewColumnType.Text));
            dtgList.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", false, 100, GridViewColumnType.Text));
            dtgList.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "Entity1", true, 100, GridViewColumnType.Text));
            dtgList.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "Entity2", true, 100, GridViewColumnType.Text));

            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                txtName.Text = projectContext.Table.First(x => x.ID == tableID).Name;

                var manyToOneList = projectContext.ManyToOneRelationshipType.Where(x => x.RelationshipType.Relationship.TableDrivedEntity.TableID == tableID);

                List<ManyToOne> ManyToOneList = new List<ManyToOne>();
                foreach (var item in manyToOneList)
                {
                    RelationshipDTO rItem = GeneralHelper.ToRelationshipDTO(item.RelationshipType.Relationship);
                    ManyToOneList.Add(GeneralHelper.ToManyToOne(rItem, item.RelationshipType.ManyToOneRelationshipType));
                }

                dtgList.DataSource = ManyToOneList;

            }
        }

        private void btnCreateMode_Click(object sender, EventArgs e)
        {
            if(txtName.Text!="")
            {
                if (dtgList.SelectedRows.Count == 2)
                {
                    ManyToManyCreatedArg arg = new ManyToManyCreatedArg();
                    arg.Name = txtName.Text;
                    arg.ManyToOneIDs = new List<int>();
                    arg.TableID = TableID;
                    arg.ManyToOneIDs = new List<int>();
                    foreach(var row in dtgList.SelectedRows)
                    {
                        if(row.DataBoundItem is ManyToOne)
                        {
                            arg.ManyToOneIDs.Add((row.DataBoundItem as ManyToOne).ID);
                        }
                    }
                    ManyToManyCreated(this, arg);
                    this.Close();
                }
                else
                    MessageBox.Show("لطفاً دو رابطه چند به یک را انتخاب نمایید");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    public class ManyToManyCreatedArg : EventArgs
    {
        public int TableID { set; get; }
        public string Name { set; get; }
        public List<int> ManyToOneIDs{ set; get; }
    }
}
