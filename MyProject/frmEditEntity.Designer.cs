namespace MyProject
{
    partial class frmEditEntity
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition2 = new Telerik.WinControls.UI.TableViewDefinition();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition3 = new Telerik.WinControls.UI.TableViewDefinition();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition4 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEditEntity));
            this.dtgRelationships = new Telerik.WinControls.UI.RadGridView();
            this.dtgColumns = new Telerik.WinControls.UI.RadGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnlParticipation = new System.Windows.Forms.Panel();
            this.optIsTolatParticipation = new System.Windows.Forms.RadioButton();
            this.optIsPartialParticipation = new System.Windows.Forms.RadioButton();
            this.btnNewEntity = new System.Windows.Forms.Button();
            this.pnlDisjoint = new System.Windows.Forms.Panel();
            this.optIsDisjoint = new System.Windows.Forms.RadioButton();
            this.optIsOverlap = new System.Windows.Forms.RadioButton();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblEntityName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbDrivedEntities = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtCriteria = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtgColumnsDrived = new Telerik.WinControls.UI.RadGridView();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.dtgRelationshipsDrived = new Telerik.WinControls.UI.RadGridView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnRemoveRelationship = new System.Windows.Forms.Button();
            this.btnRemoveColumn = new System.Windows.Forms.Button();
            this.btnCopyColumn = new System.Windows.Forms.Button();
            this.btnAddRelationship = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dtgRelationships)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgRelationships.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgColumns.MasterTemplate)).BeginInit();
            this.panel2.SuspendLayout();
            this.pnlParticipation.SuspendLayout();
            this.pnlDisjoint.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgColumnsDrived)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgColumnsDrived.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgRelationshipsDrived)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgRelationshipsDrived.MasterTemplate)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtgRelationships
            // 
            this.dtgRelationships.Location = new System.Drawing.Point(12, 120);
            // 
            // 
            // 
            this.dtgRelationships.MasterTemplate.AllowAddNewRow = false;
            this.dtgRelationships.MasterTemplate.EnableFiltering = true;
            this.dtgRelationships.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.dtgRelationships.Name = "dtgRelationships";
            this.dtgRelationships.ShowGroupPanel = false;
            this.dtgRelationships.Size = new System.Drawing.Size(438, 226);
            this.dtgRelationships.TabIndex = 8;
            this.dtgRelationships.Text = "radGridView7";
            // 
            // dtgColumns
            // 
            this.dtgColumns.Location = new System.Drawing.Point(12, 376);
            // 
            // 
            // 
            this.dtgColumns.MasterTemplate.AllowAddNewRow = false;
            this.dtgColumns.MasterTemplate.MultiSelect = true;
            this.dtgColumns.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.dtgColumns.Name = "dtgColumns";
            this.dtgColumns.ShowGroupPanel = false;
            this.dtgColumns.Size = new System.Drawing.Size(438, 226);
            this.dtgColumns.TabIndex = 8;
            this.dtgColumns.Text = "radGridView1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pnlParticipation);
            this.panel2.Controls.Add(this.btnNewEntity);
            this.panel2.Controls.Add(this.pnlDisjoint);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 604);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1036, 43);
            this.panel2.TabIndex = 2;
            // 
            // pnlParticipation
            // 
            this.pnlParticipation.Controls.Add(this.optIsTolatParticipation);
            this.pnlParticipation.Controls.Add(this.optIsPartialParticipation);
            this.pnlParticipation.Location = new System.Drawing.Point(587, 10);
            this.pnlParticipation.Name = "pnlParticipation";
            this.pnlParticipation.Size = new System.Drawing.Size(314, 26);
            this.pnlParticipation.TabIndex = 16;
            // 
            // optIsTolatParticipation
            // 
            this.optIsTolatParticipation.AutoSize = true;
            this.optIsTolatParticipation.Location = new System.Drawing.Point(7, 3);
            this.optIsTolatParticipation.Name = "optIsTolatParticipation";
            this.optIsTolatParticipation.Size = new System.Drawing.Size(143, 21);
            this.optIsTolatParticipation.TabIndex = 4;
            this.optIsTolatParticipation.TabStop = true;
            this.optIsTolatParticipation.Text = "IsTolatParticipation";
            this.optIsTolatParticipation.UseVisualStyleBackColor = true;
            // 
            // optIsPartialParticipation
            // 
            this.optIsPartialParticipation.AutoSize = true;
            this.optIsPartialParticipation.Location = new System.Drawing.Point(152, 3);
            this.optIsPartialParticipation.Name = "optIsPartialParticipation";
            this.optIsPartialParticipation.Size = new System.Drawing.Size(149, 21);
            this.optIsPartialParticipation.TabIndex = 5;
            this.optIsPartialParticipation.TabStop = true;
            this.optIsPartialParticipation.Text = "IsPartialParticipation";
            this.optIsPartialParticipation.UseVisualStyleBackColor = true;
            // 
            // btnNewEntity
            // 
            this.btnNewEntity.Location = new System.Drawing.Point(907, 6);
            this.btnNewEntity.Name = "btnNewEntity";
            this.btnNewEntity.Size = new System.Drawing.Size(115, 32);
            this.btnNewEntity.TabIndex = 14;
            this.btnNewEntity.Text = "New Entity";
            this.btnNewEntity.UseVisualStyleBackColor = true;
            this.btnNewEntity.Click += new System.EventHandler(this.btnNewEntity_Click);
            // 
            // pnlDisjoint
            // 
            this.pnlDisjoint.Controls.Add(this.optIsDisjoint);
            this.pnlDisjoint.Controls.Add(this.optIsOverlap);
            this.pnlDisjoint.Location = new System.Drawing.Point(381, 10);
            this.pnlDisjoint.Name = "pnlDisjoint";
            this.pnlDisjoint.Size = new System.Drawing.Size(191, 26);
            this.pnlDisjoint.TabIndex = 15;
            // 
            // optIsDisjoint
            // 
            this.optIsDisjoint.AutoSize = true;
            this.optIsDisjoint.Location = new System.Drawing.Point(7, 3);
            this.optIsDisjoint.Name = "optIsDisjoint";
            this.optIsDisjoint.Size = new System.Drawing.Size(84, 21);
            this.optIsDisjoint.TabIndex = 6;
            this.optIsDisjoint.TabStop = true;
            this.optIsDisjoint.Text = "IsDisjoint";
            this.optIsDisjoint.UseVisualStyleBackColor = true;
            // 
            // optIsOverlap
            // 
            this.optIsOverlap.AutoSize = true;
            this.optIsOverlap.Location = new System.Drawing.Point(97, 3);
            this.optIsOverlap.Name = "optIsOverlap";
            this.optIsOverlap.Size = new System.Drawing.Size(86, 21);
            this.optIsOverlap.TabIndex = 7;
            this.optIsOverlap.TabStop = true;
            this.optIsOverlap.Text = "IsOverlap";
            this.optIsOverlap.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(12, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(113, 31);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Exit";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(131, 5);
            this.btnSave.Margin = new System.Windows.Forms.Padding(50);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(160, 31);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.dtgColumns);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.dtgRelationships);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(466, 604);
            this.panel1.TabIndex = 9;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblEntityName);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(12, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(438, 100);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            // 
            // lblEntityName
            // 
            this.lblEntityName.Location = new System.Drawing.Point(104, 17);
            this.lblEntityName.Name = "lblEntityName";
            this.lblEntityName.Size = new System.Drawing.Size(170, 20);
            this.lblEntityName.TabIndex = 3;
            this.lblEntityName.Text = "sdasdasd";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Entity Name :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 356);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "Columns :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "Relatoinships :";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Controls.Add(this.dtgColumnsDrived);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.dtgRelationshipsDrived);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(572, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(464, 604);
            this.panel3.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbDrivedEntities);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtCriteria);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, -3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(438, 104);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            // 
            // cmbDrivedEntities
            // 
            this.cmbDrivedEntities.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDrivedEntities.FormattingEnabled = true;
            this.cmbDrivedEntities.Location = new System.Drawing.Point(115, 12);
            this.cmbDrivedEntities.Name = "cmbDrivedEntities";
            this.cmbDrivedEntities.Size = new System.Drawing.Size(165, 25);
            this.cmbDrivedEntities.TabIndex = 17;
            this.cmbDrivedEntities.SelectionChangeCommitted += new System.EventHandler(this.cmbDrivedEntities_SelectionChangeCommitted);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(111, 17);
            this.label9.TabIndex = 16;
            this.label9.Text = "Existing Entities :";
            // 
            // txtCriteria
            // 
            this.txtCriteria.Location = new System.Drawing.Point(115, 65);
            this.txtCriteria.Name = "txtCriteria";
            this.txtCriteria.Size = new System.Drawing.Size(313, 24);
            this.txtCriteria.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(59, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 17);
            this.label2.TabIndex = 14;
            this.label2.Text = "Criteria :";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(115, 39);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(165, 24);
            this.txtName.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(66, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 17);
            this.label1.TabIndex = 12;
            this.label1.Text = "Name :";
            // 
            // dtgColumnsDrived
            // 
            this.dtgColumnsDrived.Location = new System.Drawing.Point(12, 376);
            // 
            // 
            // 
            this.dtgColumnsDrived.MasterTemplate.AllowAddNewRow = false;
            this.dtgColumnsDrived.MasterTemplate.ViewDefinition = tableViewDefinition3;
            this.dtgColumnsDrived.Name = "dtgColumnsDrived";
            this.dtgColumnsDrived.ShowGroupPanel = false;
            this.dtgColumnsDrived.Size = new System.Drawing.Size(438, 226);
            this.dtgColumnsDrived.TabIndex = 8;
            this.dtgColumnsDrived.Text = "radGridView1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 356);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 17);
            this.label6.TabIndex = 9;
            this.label6.Text = "Columns :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 17);
            this.label7.TabIndex = 2;
            this.label7.Text = "Relatoinships :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(91, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 17);
            this.label8.TabIndex = 1;
            // 
            // dtgRelationshipsDrived
            // 
            this.dtgRelationshipsDrived.Location = new System.Drawing.Point(12, 120);
            // 
            // 
            // 
            this.dtgRelationshipsDrived.MasterTemplate.AllowAddNewRow = false;
            this.dtgRelationshipsDrived.MasterTemplate.EnableFiltering = true;
            this.dtgRelationshipsDrived.MasterTemplate.ViewDefinition = tableViewDefinition4;
            this.dtgRelationshipsDrived.Name = "dtgRelationshipsDrived";
            this.dtgRelationshipsDrived.ShowGroupPanel = false;
            this.dtgRelationshipsDrived.Size = new System.Drawing.Size(438, 226);
            this.dtgRelationshipsDrived.TabIndex = 8;
            this.dtgRelationshipsDrived.Text = "radGridView7";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnRemoveRelationship);
            this.panel4.Controls.Add(this.btnRemoveColumn);
            this.panel4.Controls.Add(this.btnCopyColumn);
            this.panel4.Controls.Add(this.btnAddRelationship);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(466, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(106, 604);
            this.panel4.TabIndex = 11;
            // 
            // btnRemoveRelationship
            // 
            this.btnRemoveRelationship.Image = global::MyProject.Properties.Resources.Remove;
            this.btnRemoveRelationship.Location = new System.Drawing.Point(60, 120);
            this.btnRemoveRelationship.Name = "btnRemoveRelationship";
            this.btnRemoveRelationship.Size = new System.Drawing.Size(40, 49);
            this.btnRemoveRelationship.TabIndex = 22;
            this.btnRemoveRelationship.UseVisualStyleBackColor = true;
            this.btnRemoveRelationship.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnRemoveColumn
            // 
            this.btnRemoveColumn.Image = global::MyProject.Properties.Resources.Remove;
            this.btnRemoveColumn.Location = new System.Drawing.Point(60, 376);
            this.btnRemoveColumn.Name = "btnRemoveColumn";
            this.btnRemoveColumn.Size = new System.Drawing.Size(40, 49);
            this.btnRemoveColumn.TabIndex = 19;
            this.btnRemoveColumn.UseVisualStyleBackColor = true;
            this.btnRemoveColumn.Click += new System.EventHandler(this.btnMoveColumn_Click);
            // 
            // btnCopyColumn
            // 
            this.btnCopyColumn.Image = global::MyProject.Properties.Resources.arrow_right;
            this.btnCopyColumn.Location = new System.Drawing.Point(6, 376);
            this.btnCopyColumn.Name = "btnCopyColumn";
            this.btnCopyColumn.Size = new System.Drawing.Size(40, 49);
            this.btnCopyColumn.TabIndex = 18;
            this.btnCopyColumn.UseVisualStyleBackColor = true;
            this.btnCopyColumn.Click += new System.EventHandler(this.btnAddColumn_Click);
            // 
            // btnAddRelationship
            // 
            this.btnAddRelationship.Image = ((System.Drawing.Image)(resources.GetObject("btnAddRelationship.Image")));
            this.btnAddRelationship.Location = new System.Drawing.Point(6, 120);
            this.btnAddRelationship.Name = "btnAddRelationship";
            this.btnAddRelationship.Size = new System.Drawing.Size(40, 49);
            this.btnAddRelationship.TabIndex = 17;
            this.btnAddRelationship.UseVisualStyleBackColor = true;
            this.btnAddRelationship.Click += new System.EventHandler(this.btnAddRelationship_Click);
            // 
            // frmEditEntity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1036, 647);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmEditEntity";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Entity";
            ((System.ComponentModel.ISupportInitialize)(this.dtgRelationships.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgRelationships)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgColumns.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgColumns)).EndInit();
            this.panel2.ResumeLayout(false);
            this.pnlParticipation.ResumeLayout(false);
            this.pnlParticipation.PerformLayout();
            this.pnlDisjoint.ResumeLayout(false);
            this.pnlDisjoint.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgColumnsDrived.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgColumnsDrived)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgRelationshipsDrived.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgRelationshipsDrived)).EndInit();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private Telerik.WinControls.UI.RadGridView dtgRelationships;
        private Telerik.WinControls.UI.RadGridView dtgColumns;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel3;
        private Telerik.WinControls.UI.RadGridView dtgColumnsDrived;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private Telerik.WinControls.UI.RadGridView dtgRelationshipsDrived;
        private System.Windows.Forms.Button btnNewEntity;
        private System.Windows.Forms.Panel pnlParticipation;
        private System.Windows.Forms.RadioButton optIsTolatParticipation;
        private System.Windows.Forms.RadioButton optIsPartialParticipation;
        private System.Windows.Forms.Panel pnlDisjoint;
        private System.Windows.Forms.RadioButton optIsDisjoint;
        private System.Windows.Forms.RadioButton optIsOverlap;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblEntityName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbDrivedEntities;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtCriteria;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnRemoveRelationship;
        private System.Windows.Forms.Button btnRemoveColumn;
        private System.Windows.Forms.Button btnCopyColumn;
        private System.Windows.Forms.Button btnAddRelationship;
    }
}