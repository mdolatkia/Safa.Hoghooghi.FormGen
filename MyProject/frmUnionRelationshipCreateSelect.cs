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
    public partial class frmUnionRelationshipCreateSelect : Form
    {
        public event EventHandler<UnionRelationshipSelectedArg> UnionRelationshipSelected;
        public frmUnionRelationshipCreateSelect(List<DataAccess.UnionRelationshipType> list ,bool? unionHoldsKeys,string defaultName="")
        {
            InitializeComponent();
            txtName.Text = defaultName;
            if (list.Count == 0)
            {
                GotoCreateMode();
            }
            else
            {
                dtgList.DataSource = list;
                //dtgList.SelectionChanged += dtgList_SelectionChanged;
            }
            if (unionHoldsKeys == true || unionHoldsKeys == false)
                chkUnionHoldsKeys.Enabled = false;
            chkUnionHoldsKeys.Checked = unionHoldsKeys==true;
        }
        DataAccess.UnionRelationshipType unionRelationship { set; get; }
        //void dtgList_SelectionChanged(object sender, EventArgs e)
        //{
           
        //}

        private void GotoCreateMode()
        {
            dtgList.Visible = false;
            btnCreateMode.Visible = false;
            pnlEdit.Visible = true;
            btnSave.Visible = true;
            btnChoose.Visible = false;
        }

     

      
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCreateMode_Click(object sender, EventArgs e)
        {
            GotoCreateMode();
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            if (dtgList.SelectedRows.Count > 0)
            {
                var row = dtgList.SelectedRows[0];
                unionRelationship = row.DataBoundItem as DataAccess.UnionRelationshipType;
                if (UnionRelationshipSelected != null)
                    UnionRelationshipSelected(this, new UnionRelationshipSelectedArg() { UnionRelationship = unionRelationship });
                this.Close();
                //txtName.Text = unionRelationship.Name;
                //if (unionRelationship.IsGeneralization == true)
                //    optIsGeneralization.Checked = true;
                //else if (unionRelationship.IsSpecialization == true)
                //    optIsSpecialization.Checked = true;
                //if (unionRelationship.IsTolatParticipation == true)
                //    optIsTolatParticipation.Checked = true;
                //else if (unionRelationship.IsPartialParticipation == true)
                //    optIsPartialParticipation.Checked = true;
                //if (unionRelationship.IsDisjoint == true)
                //    optIsDisjoint.Checked = true;
                //else if (unionRelationship.IsOverlap == true)
                //    optIsOverlap.Checked = true;


            }
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("نام رابطه مشخص نشده است");
                return;
            }
            if (optIsTolatParticipation.Checked == false && optIsPartialParticipation.Checked == false)
            {
                MessageBox.Show("نوع " + "TolatParticipation/PartialParticipation" + "مشخص نشده است");
                return;
            }

            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                if (unionRelationship == null)
                    unionRelationship = new DataAccess.UnionRelationshipType();
                unionRelationship.Name = txtName.Text;

              
              
                    unionRelationship.IsTolatParticipation = optIsTolatParticipation.Checked == true;
          

                unionRelationship.UnionHoldsKeys = chkUnionHoldsKeys.Checked;

                if (unionRelationship.ID == 0)
                    projectContext.UnionRelationshipType.Add(unionRelationship);

                projectContext.SaveChanges();

                if (UnionRelationshipSelected != null)
                    UnionRelationshipSelected(this, new UnionRelationshipSelectedArg() { UnionRelationship = unionRelationship });
                this.Close();
            }
        }
    }
    public class UnionRelationshipSelectedArg : EventArgs
    {
        public DataAccess.UnionRelationshipType UnionRelationship { set; get; }
    }
}
