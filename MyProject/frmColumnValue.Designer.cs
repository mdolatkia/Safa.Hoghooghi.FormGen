namespace MyProject
{
    partial class frmColumnValue
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn9 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn10 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn11 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition7 = new Telerik.WinControls.UI.TableViewDefinition();
            Telerik.WinControls.UI.GridViewComboBoxColumn gridViewComboBoxColumn5 = new Telerik.WinControls.UI.GridViewComboBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn12 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition8 = new Telerik.WinControls.UI.TableViewDefinition();
            Telerik.WinControls.UI.GridViewComboBoxColumn gridViewComboBoxColumn6 = new Telerik.WinControls.UI.GridViewComboBoxColumn();
            Telerik.WinControls.UI.GridViewCheckBoxColumn gridViewCheckBoxColumn3 = new Telerik.WinControls.UI.GridViewCheckBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition9 = new Telerik.WinControls.UI.TableViewDefinition();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dtgColumnValues = new Telerik.WinControls.UI.RadGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnUpdateColumnValue = new System.Windows.Forms.Button();
            this.radPageView1 = new Telerik.WinControls.UI.RadPageView();
            this.pageRuleOnColumns = new Telerik.WinControls.UI.RadPageViewPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnUpdateColumnValue_Columns = new System.Windows.Forms.Button();
            this.dtgColumnValue_Columns = new Telerik.WinControls.UI.RadGridView();
            this.pageRuleOnRelationships = new Telerik.WinControls.UI.RadPageViewPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnUpdateColumnValue_Relationships = new System.Windows.Forms.Button();
            this.dtgColumnValue_Relationships = new Telerik.WinControls.UI.RadGridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgColumnValues)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgColumnValues.MasterTemplate)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).BeginInit();
            this.radPageView1.SuspendLayout();
            this.pageRuleOnColumns.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgColumnValue_Columns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgColumnValue_Columns.MasterTemplate)).BeginInit();
            this.pageRuleOnRelationships.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgColumnValue_Relationships)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgColumnValue_Relationships.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dtgColumnValues);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(288, 537);
            this.panel1.TabIndex = 0;
            // 
            // dtgColumnValues
            // 
            this.dtgColumnValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtgColumnValues.Location = new System.Drawing.Point(0, 42);
            // 
            // 
            // 
            this.dtgColumnValues.MasterTemplate.AllowDeleteRow = false;
            gridViewTextBoxColumn9.FieldName = "ID";
            gridViewTextBoxColumn9.HeaderText = "ID";
            gridViewTextBoxColumn9.Name = "column1";
            gridViewTextBoxColumn9.ReadOnly = true;
            gridViewTextBoxColumn9.Width = 75;
            gridViewTextBoxColumn10.FieldName = "ColumnID";
            gridViewTextBoxColumn10.HeaderText = "ColumnID";
            gridViewTextBoxColumn10.Name = "column3";
            gridViewTextBoxColumn10.ReadOnly = true;
            gridViewTextBoxColumn10.Width = 75;
            gridViewTextBoxColumn11.FieldName = "Value";
            gridViewTextBoxColumn11.HeaderText = "Value";
            gridViewTextBoxColumn11.Name = "column2";
            gridViewTextBoxColumn11.Width = 75;
            this.dtgColumnValues.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn9,
            gridViewTextBoxColumn10,
            gridViewTextBoxColumn11});
            this.dtgColumnValues.MasterTemplate.ViewDefinition = tableViewDefinition7;
            this.dtgColumnValues.Name = "dtgColumnValues";
            this.dtgColumnValues.ShowGroupPanel = false;
            this.dtgColumnValues.Size = new System.Drawing.Size(288, 495);
            this.dtgColumnValues.TabIndex = 9;
            this.dtgColumnValues.Text = "radGridView7";
            this.dtgColumnValues.SelectionChanged += new System.EventHandler(this.dtgColumnValues_SelectionChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnUpdateColumnValue);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(288, 42);
            this.panel2.TabIndex = 0;
            // 
            // btnUpdateColumnValue
            // 
            this.btnUpdateColumnValue.Location = new System.Drawing.Point(3, 12);
            this.btnUpdateColumnValue.Name = "btnUpdateColumnValue";
            this.btnUpdateColumnValue.Size = new System.Drawing.Size(118, 23);
            this.btnUpdateColumnValue.TabIndex = 3;
            this.btnUpdateColumnValue.Text = "Update";
            this.btnUpdateColumnValue.UseVisualStyleBackColor = true;
            this.btnUpdateColumnValue.Click += new System.EventHandler(this.btnUpdateColumnValue_Click);
            // 
            // radPageView1
            // 
            this.radPageView1.Controls.Add(this.pageRuleOnColumns);
            this.radPageView1.Controls.Add(this.pageRuleOnRelationships);
            this.radPageView1.DefaultPage = this.pageRuleOnColumns;
            this.radPageView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPageView1.Location = new System.Drawing.Point(288, 0);
            this.radPageView1.Name = "radPageView1";
            this.radPageView1.SelectedPage = this.pageRuleOnColumns;
            this.radPageView1.Size = new System.Drawing.Size(558, 537);
            this.radPageView1.TabIndex = 1;
            this.radPageView1.Text = "radPageView1";
            // 
            // pageRuleOnColumns
            // 
            this.pageRuleOnColumns.Controls.Add(this.panel3);
            this.pageRuleOnColumns.Controls.Add(this.dtgColumnValue_Columns);
            this.pageRuleOnColumns.Enabled = false;
            this.pageRuleOnColumns.ItemSize = new System.Drawing.SizeF(132F, 28F);
            this.pageRuleOnColumns.Location = new System.Drawing.Point(10, 37);
            this.pageRuleOnColumns.Name = "pageRuleOnColumns";
            this.pageRuleOnColumns.Size = new System.Drawing.Size(537, 489);
            this.pageRuleOnColumns.Text = "ColumnValue_Columns";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnUpdateColumnValue_Columns);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(537, 32);
            this.panel3.TabIndex = 11;
            // 
            // btnUpdateColumnValue_Columns
            // 
            this.btnUpdateColumnValue_Columns.Location = new System.Drawing.Point(3, 4);
            this.btnUpdateColumnValue_Columns.Name = "btnUpdateColumnValue_Columns";
            this.btnUpdateColumnValue_Columns.Size = new System.Drawing.Size(115, 23);
            this.btnUpdateColumnValue_Columns.TabIndex = 3;
            this.btnUpdateColumnValue_Columns.Text = "Update";
            this.btnUpdateColumnValue_Columns.UseVisualStyleBackColor = true;
            this.btnUpdateColumnValue_Columns.Click += new System.EventHandler(this.btnUpdateColumnValue_Columns_Click);
            // 
            // dtgColumnValue_Columns
            // 
            this.dtgColumnValue_Columns.Location = new System.Drawing.Point(0, 33);
            // 
            // 
            // 
            gridViewComboBoxColumn5.FieldName = "ColumnID";
            gridViewComboBoxColumn5.HeaderText = "Column";
            gridViewComboBoxColumn5.Name = "colColumns";
            gridViewComboBoxColumn5.Width = 200;
            gridViewTextBoxColumn12.FieldName = "ValidValue";
            gridViewTextBoxColumn12.HeaderText = "Valid Value";
            gridViewTextBoxColumn12.Name = "column1";
            gridViewTextBoxColumn12.Width = 200;
            this.dtgColumnValue_Columns.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewComboBoxColumn5,
            gridViewTextBoxColumn12});
            this.dtgColumnValue_Columns.MasterTemplate.ViewDefinition = tableViewDefinition8;
            this.dtgColumnValue_Columns.Name = "dtgColumnValue_Columns";
            this.dtgColumnValue_Columns.ShowGroupPanel = false;
            this.dtgColumnValue_Columns.Size = new System.Drawing.Size(537, 456);
            this.dtgColumnValue_Columns.TabIndex = 10;
            this.dtgColumnValue_Columns.Text = "radGridView7";
            // 
            // pageRuleOnRelationships
            // 
            this.pageRuleOnRelationships.Controls.Add(this.panel4);
            this.pageRuleOnRelationships.Controls.Add(this.dtgColumnValue_Relationships);
            this.pageRuleOnRelationships.Enabled = false;
            this.pageRuleOnRelationships.ItemSize = new System.Drawing.SizeF(154F, 28F);
            this.pageRuleOnRelationships.Location = new System.Drawing.Point(10, 37);
            this.pageRuleOnRelationships.Name = "pageRuleOnRelationships";
            this.pageRuleOnRelationships.Size = new System.Drawing.Size(537, 489);
            this.pageRuleOnRelationships.Text = "ColumnValue_Relationships";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnUpdateColumnValue_Relationships);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(537, 32);
            this.panel4.TabIndex = 13;
            // 
            // btnUpdateColumnValue_Relationships
            // 
            this.btnUpdateColumnValue_Relationships.Location = new System.Drawing.Point(3, 4);
            this.btnUpdateColumnValue_Relationships.Name = "btnUpdateColumnValue_Relationships";
            this.btnUpdateColumnValue_Relationships.Size = new System.Drawing.Size(115, 23);
            this.btnUpdateColumnValue_Relationships.TabIndex = 3;
            this.btnUpdateColumnValue_Relationships.Text = "Update";
            this.btnUpdateColumnValue_Relationships.UseVisualStyleBackColor = true;
            this.btnUpdateColumnValue_Relationships.Click += new System.EventHandler(this.btnUpdateColumnValue_Relationships_Click);
            // 
            // dtgColumnValue_Relationships
            // 
            this.dtgColumnValue_Relationships.Location = new System.Drawing.Point(0, 33);
            // 
            // 
            // 
            gridViewComboBoxColumn6.FieldName = "RelationshipID";
            gridViewComboBoxColumn6.HeaderText = "Relationship";
            gridViewComboBoxColumn6.Name = "colRelationship";
            gridViewComboBoxColumn6.Width = 300;
            gridViewCheckBoxColumn3.FieldName = "Enabled";
            gridViewCheckBoxColumn3.HeaderText = "Enabled";
            gridViewCheckBoxColumn3.Name = "column1";
            this.dtgColumnValue_Relationships.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewComboBoxColumn6,
            gridViewCheckBoxColumn3});
            this.dtgColumnValue_Relationships.MasterTemplate.ViewDefinition = tableViewDefinition9;
            this.dtgColumnValue_Relationships.Name = "dtgColumnValue_Relationships";
            this.dtgColumnValue_Relationships.ShowGroupPanel = false;
            this.dtgColumnValue_Relationships.Size = new System.Drawing.Size(537, 456);
            this.dtgColumnValue_Relationships.TabIndex = 12;
            this.dtgColumnValue_Relationships.Text = "radGridView7";
            // 
            // frmColumnValue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 537);
            this.Controls.Add(this.radPageView1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.Name = "frmColumnValue";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rule On Value";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgColumnValues.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgColumnValues)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPageView1)).EndInit();
            this.radPageView1.ResumeLayout(false);
            this.pageRuleOnColumns.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgColumnValue_Columns.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgColumnValue_Columns)).EndInit();
            this.pageRuleOnRelationships.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgColumnValue_Relationships.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgColumnValue_Relationships)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Telerik.WinControls.UI.RadGridView dtgColumnValues;
        private Telerik.WinControls.UI.RadPageView radPageView1;
        private Telerik.WinControls.UI.RadPageViewPage pageRuleOnColumns;
        private Telerik.WinControls.UI.RadGridView dtgColumnValue_Columns;
        private Telerik.WinControls.UI.RadPageViewPage pageRuleOnRelationships;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnUpdateColumnValue_Columns;
        private System.Windows.Forms.Button btnUpdateColumnValue;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnUpdateColumnValue_Relationships;
        private Telerik.WinControls.UI.RadGridView dtgColumnValue_Relationships;
    }
}