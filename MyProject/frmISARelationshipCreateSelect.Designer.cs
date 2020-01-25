namespace MyProject
{
    partial class frmISARelationshipCreateSelect
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.pnlEdit = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.optIsTolatParticipation = new System.Windows.Forms.RadioButton();
            this.optIsPartialParticipation = new System.Windows.Forms.RadioButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.optIsDisjoint = new System.Windows.Forms.RadioButton();
            this.optIsOverlap = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.optIsSpecialization = new System.Windows.Forms.RadioButton();
            this.optIsGeneralization = new System.Windows.Forms.RadioButton();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnChoose = new System.Windows.Forms.Button();
            this.btnCreateMode = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dtgList = new Telerik.WinControls.UI.RadGridView();
            this.pnlEdit.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgList.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlEdit
            // 
            this.pnlEdit.Controls.Add(this.panel4);
            this.pnlEdit.Controls.Add(this.panel3);
            this.pnlEdit.Controls.Add(this.panel2);
            this.pnlEdit.Controls.Add(this.txtName);
            this.pnlEdit.Controls.Add(this.label1);
            this.pnlEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlEdit.Location = new System.Drawing.Point(0, 0);
            this.pnlEdit.Name = "pnlEdit";
            this.pnlEdit.Size = new System.Drawing.Size(746, 448);
            this.pnlEdit.TabIndex = 3;
            this.pnlEdit.Visible = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.optIsTolatParticipation);
            this.panel4.Controls.Add(this.optIsPartialParticipation);
            this.panel4.Location = new System.Drawing.Point(315, 78);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(159, 70);
            this.panel4.TabIndex = 10;
            // 
            // optIsTolatParticipation
            // 
            this.optIsTolatParticipation.AutoSize = true;
            this.optIsTolatParticipation.Location = new System.Drawing.Point(9, 4);
            this.optIsTolatParticipation.Name = "optIsTolatParticipation";
            this.optIsTolatParticipation.Size = new System.Drawing.Size(117, 17);
            this.optIsTolatParticipation.TabIndex = 4;
            this.optIsTolatParticipation.TabStop = true;
            this.optIsTolatParticipation.Text = "IsTolatParticipation";
            this.optIsTolatParticipation.UseVisualStyleBackColor = true;
            // 
            // optIsPartialParticipation
            // 
            this.optIsPartialParticipation.AutoSize = true;
            this.optIsPartialParticipation.Location = new System.Drawing.Point(9, 31);
            this.optIsPartialParticipation.Name = "optIsPartialParticipation";
            this.optIsPartialParticipation.Size = new System.Drawing.Size(123, 17);
            this.optIsPartialParticipation.TabIndex = 5;
            this.optIsPartialParticipation.TabStop = true;
            this.optIsPartialParticipation.Text = "IsPartialParticipation";
            this.optIsPartialParticipation.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.optIsDisjoint);
            this.panel3.Controls.Add(this.optIsOverlap);
            this.panel3.Location = new System.Drawing.Point(98, 163);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(162, 70);
            this.panel3.TabIndex = 9;
            // 
            // optIsDisjoint
            // 
            this.optIsDisjoint.AutoSize = true;
            this.optIsDisjoint.Location = new System.Drawing.Point(3, 3);
            this.optIsDisjoint.Name = "optIsDisjoint";
            this.optIsDisjoint.Size = new System.Drawing.Size(69, 17);
            this.optIsDisjoint.TabIndex = 6;
            this.optIsDisjoint.TabStop = true;
            this.optIsDisjoint.Text = "IsDisjoint";
            this.optIsDisjoint.UseVisualStyleBackColor = true;
            // 
            // optIsOverlap
            // 
            this.optIsOverlap.AutoSize = true;
            this.optIsOverlap.Location = new System.Drawing.Point(3, 26);
            this.optIsOverlap.Name = "optIsOverlap";
            this.optIsOverlap.Size = new System.Drawing.Size(72, 17);
            this.optIsOverlap.TabIndex = 7;
            this.optIsOverlap.TabStop = true;
            this.optIsOverlap.Text = "IsOverlap";
            this.optIsOverlap.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.optIsSpecialization);
            this.panel2.Controls.Add(this.optIsGeneralization);
            this.panel2.Location = new System.Drawing.Point(98, 78);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(162, 70);
            this.panel2.TabIndex = 8;
            // 
            // optIsSpecialization
            // 
            this.optIsSpecialization.AutoSize = true;
            this.optIsSpecialization.Location = new System.Drawing.Point(3, 31);
            this.optIsSpecialization.Name = "optIsSpecialization";
            this.optIsSpecialization.Size = new System.Drawing.Size(98, 17);
            this.optIsSpecialization.TabIndex = 3;
            this.optIsSpecialization.TabStop = true;
            this.optIsSpecialization.Text = "IsSpecialization";
            this.optIsSpecialization.UseVisualStyleBackColor = true;
            // 
            // optIsGeneralization
            // 
            this.optIsGeneralization.AutoSize = true;
            this.optIsGeneralization.Location = new System.Drawing.Point(3, 4);
            this.optIsGeneralization.Name = "optIsGeneralization";
            this.optIsGeneralization.Size = new System.Drawing.Size(102, 17);
            this.optIsGeneralization.TabIndex = 2;
            this.optIsGeneralization.TabStop = true;
            this.optIsGeneralization.Text = "IsGeneralization";
            this.optIsGeneralization.UseVisualStyleBackColor = true;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(98, 36);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(376, 21);
            this.txtName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name :";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnChoose);
            this.panel1.Controls.Add(this.btnCreateMode);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 404);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(746, 44);
            this.panel1.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(12, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(113, 31);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "بازگشت";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click_1);
            // 
            // btnChoose
            // 
            this.btnChoose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChoose.Location = new System.Drawing.Point(621, 7);
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.Size = new System.Drawing.Size(113, 31);
            this.btnChoose.TabIndex = 1;
            this.btnChoose.Text = "انتخاب";
            this.btnChoose.UseVisualStyleBackColor = true;
            this.btnChoose.Click += new System.EventHandler(this.btnChoose_Click);
            // 
            // btnCreateMode
            // 
            this.btnCreateMode.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateMode.Location = new System.Drawing.Point(131, 7);
            this.btnCreateMode.Margin = new System.Windows.Forms.Padding(50);
            this.btnCreateMode.Name = "btnCreateMode";
            this.btnCreateMode.Size = new System.Drawing.Size(160, 31);
            this.btnCreateMode.TabIndex = 5;
            this.btnCreateMode.Text = "ایجاد رابطه ارث بری";
            this.btnCreateMode.UseVisualStyleBackColor = true;
            this.btnCreateMode.Click += new System.EventHandler(this.btnCreateMode_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(131, 7);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(160, 31);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "ثبت و انتخاب";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // dtgList
            // 
            this.dtgList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtgList.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.dtgList.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.dtgList.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.dtgList.Name = "dtgList";
            this.dtgList.ReadOnly = true;
            this.dtgList.ShowGroupPanel = false;
            this.dtgList.Size = new System.Drawing.Size(746, 448);
            this.dtgList.TabIndex = 6;
            this.dtgList.Text = "dtgList";
            // 
            // frmISARelationshipCreateSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 448);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlEdit);
            this.Controls.Add(this.dtgList);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.Name = "frmISARelationshipCreateSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmISARelationshipCreateSelect";
            this.pnlEdit.ResumeLayout(false);
            this.pnlEdit.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgList.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlEdit;
        private System.Windows.Forms.RadioButton optIsOverlap;
        private System.Windows.Forms.RadioButton optIsDisjoint;
        private System.Windows.Forms.RadioButton optIsPartialParticipation;
        private System.Windows.Forms.RadioButton optIsTolatParticipation;
        private System.Windows.Forms.RadioButton optIsSpecialization;
        private System.Windows.Forms.RadioButton optIsGeneralization;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnChoose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCreateMode;
        private Telerik.WinControls.UI.RadGridView dtgList;
    }
}