using ModelEntites;
using MyModelManager;
using MyProject_WPF.Biz;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace MyProject_WPF
{
    public partial class frmEditSubEntity : UserControl
    {
        public event EventHandler<string> NameChanged;
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        List<TableDrivedEntityDTO> DrivedEntities { set; get; }
        int EntityID { set; get; }
        public TableDrivedEntityDTO Message { set; get; }
        public ColumnDTO SelectedColumn
        {

            get
            {
                return dtgColumnsDrived.SelectedItem as ColumnDTO;
            }
        }

        public RelationshipDTO SelectedRelationship
        {
            get
            {
                return dtgRelationshipsDrived.SelectedItem as RelationshipDTO;
            }
        }

        public frmEditSubEntity(TableDrivedEntityDTO TableDrivedEntityDTO, List<ColumnDTO> columns)
        {
            InitializeComponent();
            Message = TableDrivedEntityDTO;
            cmbValue.DisplayMemberPath = "Alias";
            cmbValue.SelectedValuePath = "ID";
            cmbValue.ItemsSource = columns;
            cmbValue.SelectedValue = TableDrivedEntityDTO.DeterminerColumnID;
            txtName.Text = TableDrivedEntityDTO.Name;
            txtAlias.Text = TableDrivedEntityDTO.Alias;
            //cmbValue.SelectedValue = TableDrivedEntityDTO.DeterminerColumnID;
            //txtValue.Text = TableDrivedEntityDTO.DeterminerColumnValue;
            dtgColumnsDrived.ItemsSource = Message.Columns;
            dtgRelationshipsDrived.ItemsSource = Message.Relationships;
            dtgValues.ItemsSource = Message.EntityDeterminers;

            ControlHelper.GenerateContextMenu(dtgValues);

            txtName.TextChanged += TxtName_TextChanged;

        }

        private void TxtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NameChanged != null)
                NameChanged(this, txtName.Text);
        }

        public void UpdateDrivedEntity()
        {
            Message.Name = txtName.Text;
            Message.Alias = txtAlias.Text;
            if (cmbValue.SelectedItem != null)
                Message.DeterminerColumnID = (int)cmbValue.SelectedValue;
            else
                Message.DeterminerColumnID = 0;
            //Message.DeterminerColumnValue = txtValue.Text;
        }

        //internal void AddColumn(ColumnDTO column)
        //{
          
        //}

        internal void RemoveColumn(ColumnDTO column)
        {
            Message.Columns.Remove(column);
            dtgColumnsDrived.ItemsSource = null;
            dtgColumnsDrived.ItemsSource = Message.Columns;
        }

        //internal void AddRelationship(RelationshipDTO relationship)
        //{
        //    Message.Relationships.Add(relationship);
        //    dtgRelationshipsDrived.ItemsSource = null;
        //    dtgRelationshipsDrived.ItemsSource = Message.Relationships;
        //}

        internal void RemoveRelationship(RelationshipDTO relationship)
        {
            Message.Relationships.Remove(relationship);
            dtgRelationshipsDrived.ItemsSource = null;
            dtgRelationshipsDrived.ItemsSource = Message.Relationships;
        }
    }
}
