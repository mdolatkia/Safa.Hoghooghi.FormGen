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
    public partial class frmISARelationshipCreateSelect : Form
    {
        public event EventHandler<ISARelationshipSelectedArg> ISARelationshipSelected;
        public frmISARelationshipCreateSelect(List<DataAccess.ISARelationship> list,string defaultName="")
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
            }
        }
        DataAccess.ISARelationship isaRelationship { set; get; }
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
                isaRelationship = row.DataBoundItem as DataAccess.ISARelationship;
                if (ISARelationshipSelected != null)
                    ISARelationshipSelected(this, new ISARelationshipSelectedArg() { ISARelationship = isaRelationship });
                this.Close();
                //txtName.Text = isaRelationship.Name;
                //if (isaRelationship.IsGeneralization == true)
                //    optIsGeneralization.Checked = true;
                //else if (isaRelationship.IsSpecialization == true)
                //    optIsSpecialization.Checked = true;
                //if (isaRelationship.IsTolatParticipation == true)
                //    optIsTolatParticipation.Checked = true;
                //else if (isaRelationship.IsPartialParticipation == true)
                //    optIsPartialParticipation.Checked = true;
                //if (isaRelationship.IsDisjoint == true)
                //    optIsDisjoint.Checked = true;
                //else if (isaRelationship.IsOverlap == true)
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
            if (optIsDisjoint.Checked == false && optIsOverlap.Checked == false)
            {
                MessageBox.Show("نوع " + "Disjoint/IsOverlap" + "مشخص نشده است");
                return;
            }
            if (optIsTolatParticipation.Checked == false && optIsPartialParticipation.Checked == false)
            {
                MessageBox.Show("نوع " + "TolatParticipation/PartialParticipation" + "مشخص نشده است");
                return;
            }
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                if (isaRelationship == null)
                    isaRelationship = new DataAccess.ISARelationship();
                isaRelationship.Name = txtName.Text;

                if (optIsGeneralization.Checked == true)
                    isaRelationship.IsGeneralization = true;
                else if (optIsSpecialization.Checked == true)
                    isaRelationship.IsSpecialization = true;

                isaRelationship.IsTolatParticipation = optIsTolatParticipation.Checked == true;

                isaRelationship.IsDisjoint = optIsDisjoint.Checked == true;


                if (isaRelationship.ID == 0)
                    projectContext.ISARelationship.Add(isaRelationship);

                projectContext.SaveChanges();

                if (ISARelationshipSelected != null)
                    ISARelationshipSelected(this, new ISARelationshipSelectedArg() { ISARelationship = isaRelationship });
                this.Close();
            }
        }
    }
    public class ISARelationshipSelectedArg : EventArgs
    {
        public DataAccess.ISARelationship ISARelationship { set; get; }
    }
}
