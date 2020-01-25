namespace MyRuleEngineTest
{
    partial class frmImposeRules
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
            this.btnRuleEntityRelationships = new System.Windows.Forms.Button();
            this.btnRuleEntity = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRuleEntityRelationships
            // 
            this.btnRuleEntityRelationships.Location = new System.Drawing.Point(230, 75);
            this.btnRuleEntityRelationships.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnRuleEntityRelationships.Name = "btnRuleEntityRelationships";
            this.btnRuleEntityRelationships.Size = new System.Drawing.Size(210, 29);
            this.btnRuleEntityRelationships.TabIndex = 0;
            this.btnRuleEntityRelationships.Text = "Impose  Rule For Entity_Relationships";
            this.btnRuleEntityRelationships.UseVisualStyleBackColor = true;
            this.btnRuleEntityRelationships.Click += new System.EventHandler(this.btnRuleEntityRelationships_Click);
            // 
            // btnRuleEntity
            // 
            this.btnRuleEntity.Location = new System.Drawing.Point(11, 75);
            this.btnRuleEntity.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnRuleEntity.Name = "btnRuleEntity";
            this.btnRuleEntity.Size = new System.Drawing.Size(210, 29);
            this.btnRuleEntity.TabIndex = 1;
            this.btnRuleEntity.Text = "Impose  Rule For Entity";
            this.btnRuleEntity.UseVisualStyleBackColor = true;
            this.btnRuleEntity.Click += new System.EventHandler(this.btnRuleEntity_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtDatabase);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Controls.Add(this.btnRuleEntityRelationships);
            this.groupBox1.Controls.Add(this.btnRuleEntity);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(709, 129);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Impose Rules";
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(2, 108);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(705, 19);
            this.progressBar1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Database Name :";
            // 
            // txtDatabase
            // 
            this.txtDatabase.Location = new System.Drawing.Point(118, 28);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(188, 20);
            this.txtDatabase.TabIndex = 4;
            // 
            // frmImposeRules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(709, 437);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.Name = "frmImposeRules";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Impose Rules";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRuleEntityRelationships;
        private System.Windows.Forms.Button btnRuleEntity;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.Label label1;

    }
}

