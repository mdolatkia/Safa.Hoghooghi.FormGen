namespace MyProject
{
    partial class frmArcRelationships
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            Telerik.WinControls.UI.GridViewComboBoxColumn gridViewComboBoxColumn1 = new Telerik.WinControls.UI.GridViewComboBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition2 = new Telerik.WinControls.UI.TableViewDefinition();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dtgArcGroup = new Telerik.WinControls.UI.RadGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnUpdateArcGroup = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnUpdateArcRelationships = new System.Windows.Forms.Button();
            this.dtgArcRelationships = new Telerik.WinControls.UI.RadGridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgArcGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgArcGroup.MasterTemplate)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgArcRelationships)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgArcRelationships.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dtgArcGroup);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(288, 537);
            this.panel1.TabIndex = 0;
            // 
            // dtgArcGroup
            // 
            this.dtgArcGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtgArcGroup.Location = new System.Drawing.Point(0, 42);
            // 
            // 
            // 
            this.dtgArcGroup.MasterTemplate.AllowDeleteRow = false;
            gridViewTextBoxColumn1.FieldName = "ID";
            gridViewTextBoxColumn1.HeaderText = "ID";
            gridViewTextBoxColumn1.Name = "column1";
            gridViewTextBoxColumn1.ReadOnly = true;
            gridViewTextBoxColumn1.Width = 75;
            gridViewTextBoxColumn2.FieldName = "GroupName";
            gridViewTextBoxColumn2.HeaderText = "Group Name";
            gridViewTextBoxColumn2.Name = "column2";
            gridViewTextBoxColumn2.Width = 120;
            this.dtgArcGroup.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2});
            this.dtgArcGroup.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.dtgArcGroup.Name = "dtgArcGroup";
            this.dtgArcGroup.ShowGroupPanel = false;
            this.dtgArcGroup.Size = new System.Drawing.Size(288, 495);
            this.dtgArcGroup.TabIndex = 9;
            this.dtgArcGroup.Text = "radGridView7";
            this.dtgArcGroup.SelectionChanged += new System.EventHandler(this.dtgRuleOnValues_SelectionChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnUpdateArcGroup);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(288, 42);
            this.panel2.TabIndex = 0;
            // 
            // btnUpdateArcGroup
            // 
            this.btnUpdateArcGroup.Location = new System.Drawing.Point(3, 14);
            this.btnUpdateArcGroup.Name = "btnUpdateArcGroup";
            this.btnUpdateArcGroup.Size = new System.Drawing.Size(118, 23);
            this.btnUpdateArcGroup.TabIndex = 3;
            this.btnUpdateArcGroup.Text = "Update";
            this.btnUpdateArcGroup.UseVisualStyleBackColor = true;
            this.btnUpdateArcGroup.Click += new System.EventHandler(this.btnUpdateRuleOnValue_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.dtgArcRelationships);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(288, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(558, 537);
            this.panel3.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnUpdateArcRelationships);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(558, 42);
            this.panel4.TabIndex = 17;
            // 
            // btnUpdateArcRelationships
            // 
            this.btnUpdateArcRelationships.Enabled = false;
            this.btnUpdateArcRelationships.Location = new System.Drawing.Point(6, 13);
            this.btnUpdateArcRelationships.Name = "btnUpdateArcRelationships";
            this.btnUpdateArcRelationships.Size = new System.Drawing.Size(115, 23);
            this.btnUpdateArcRelationships.TabIndex = 3;
            this.btnUpdateArcRelationships.Text = "Update";
            this.btnUpdateArcRelationships.UseVisualStyleBackColor = true;
            this.btnUpdateArcRelationships.Click += new System.EventHandler(this.btnUpdateArcRelationships_Click);
            // 
            // dtgArcRelationships
            // 
            this.dtgArcRelationships.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dtgArcRelationships.Enabled = false;
            this.dtgArcRelationships.Location = new System.Drawing.Point(0, 42);
            // 
            // 
            // 
            gridViewComboBoxColumn1.FieldName = "RelationshipID";
            gridViewComboBoxColumn1.HeaderText = "Relationship";
            gridViewComboBoxColumn1.Name = "colRelationship";
            gridViewComboBoxColumn1.Width = 300;
            this.dtgArcRelationships.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewComboBoxColumn1});
            this.dtgArcRelationships.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.dtgArcRelationships.Name = "dtgArcRelationships";
            this.dtgArcRelationships.ShowGroupPanel = false;
            this.dtgArcRelationships.Size = new System.Drawing.Size(558, 495);
            this.dtgArcRelationships.TabIndex = 16;
            this.dtgArcRelationships.Text = "radGridView7";
            // 
            // frmArcRelationships
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 537);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.Name = "frmArcRelationships";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rule On Value";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgArcGroup.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgArcGroup)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgArcRelationships.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgArcRelationships)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Telerik.WinControls.UI.RadGridView dtgArcGroup;
        private System.Windows.Forms.Button btnUpdateArcGroup;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnUpdateArcRelationships;
        private Telerik.WinControls.UI.RadGridView dtgArcRelationships;
    }
}