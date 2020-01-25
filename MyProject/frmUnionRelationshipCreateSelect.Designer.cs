namespace MyProject
{
    partial class frmUnionRelationshipCreateSelect
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition3 = new Telerik.WinControls.UI.TableViewDefinition();
            this.pnlEdit = new System.Windows.Forms.Panel();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnChoose = new System.Windows.Forms.Button();
            this.btnCreateMode = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dtgList = new Telerik.WinControls.UI.RadGridView();
            this.chkUnionHoldsKeys = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.optIsTolatParticipation = new System.Windows.Forms.RadioButton();
            this.optIsPartialParticipation = new System.Windows.Forms.RadioButton();
            this.pnlEdit.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgList.MasterTemplate)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlEdit
            // 
            this.pnlEdit.Controls.Add(this.panel4);
            this.pnlEdit.Controls.Add(this.chkUnionHoldsKeys);
            this.pnlEdit.Controls.Add(this.txtName);
            this.pnlEdit.Controls.Add(this.label1);
            this.pnlEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlEdit.Location = new System.Drawing.Point(0, 0);
            this.pnlEdit.Name = "pnlEdit";
            this.pnlEdit.Size = new System.Drawing.Size(746, 448);
            this.pnlEdit.TabIndex = 3;
            this.pnlEdit.Visible = false;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(98, 36);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(376, 24);
            this.txtName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 17);
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
            this.dtgList.MasterTemplate.ViewDefinition = tableViewDefinition3;
            this.dtgList.Name = "dtgList";
            this.dtgList.ReadOnly = true;
            this.dtgList.ShowGroupPanel = false;
            this.dtgList.Size = new System.Drawing.Size(746, 448);
            this.dtgList.TabIndex = 6;
            this.dtgList.Text = "dtgList";
            // 
            // chkUnionHoldsKeys
            // 
            this.chkUnionHoldsKeys.AutoSize = true;
            this.chkUnionHoldsKeys.Location = new System.Drawing.Point(339, 84);
            this.chkUnionHoldsKeys.Name = "chkUnionHoldsKeys";
            this.chkUnionHoldsKeys.Size = new System.Drawing.Size(135, 21);
            this.chkUnionHoldsKeys.TabIndex = 11;
            this.chkUnionHoldsKeys.Text = "Union Holds Keys";
            this.chkUnionHoldsKeys.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.optIsTolatParticipation);
            this.panel4.Controls.Add(this.optIsPartialParticipation);
            this.panel4.Location = new System.Drawing.Point(98, 79);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(159, 70);
            this.panel4.TabIndex = 12;
            // 
            // optIsTolatParticipation
            // 
            this.optIsTolatParticipation.AutoSize = true;
            this.optIsTolatParticipation.Location = new System.Drawing.Point(9, 4);
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
            this.optIsPartialParticipation.Location = new System.Drawing.Point(9, 31);
            this.optIsPartialParticipation.Name = "optIsPartialParticipation";
            this.optIsPartialParticipation.Size = new System.Drawing.Size(149, 21);
            this.optIsPartialParticipation.TabIndex = 5;
            this.optIsPartialParticipation.TabStop = true;
            this.optIsPartialParticipation.Text = "IsPartialParticipation";
            this.optIsPartialParticipation.UseVisualStyleBackColor = true;
            // 
            // frmUnionRelationshipCreateSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 448);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlEdit);
            this.Controls.Add(this.dtgList);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.Name = "frmUnionRelationshipCreateSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmISARelationshipCreateSelect";
            this.pnlEdit.ResumeLayout(false);
            this.pnlEdit.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgList.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgList)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlEdit;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnChoose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCreateMode;
        private Telerik.WinControls.UI.RadGridView dtgList;
        private System.Windows.Forms.CheckBox chkUnionHoldsKeys;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RadioButton optIsTolatParticipation;
        private System.Windows.Forms.RadioButton optIsPartialParticipation;
    }
}