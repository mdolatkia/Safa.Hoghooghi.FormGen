using ModelEntites;
using MyModelManager;
using MyProject_WPF.Biz;
using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Interaction logic for frmColumnValue.xaml
    /// </summary>
    public partial class frmColumnValue : UserControl
    {
        int ColumnID { set; get; }
        public frmColumnValue(int columnID)
        {
            InitializeComponent();
            ColumnID = columnID;
            SetColumnValueGrid();
        }

        private void SetColumnValueGrid()
        {
         //   BizColumnValue biz = new BizColumnValue();
            ////dtgColumnValues.ItemsSource = biz.GetColumnValues(ColumnID);


            BizColumn bizColumn = new BizColumn();
            var col = dtgColumnValue_Columns.Columns[0] as GridViewComboBoxColumn;
            col.ItemsSource = bizColumn.GetOtherColums(ColumnID);
            col.DisplayMemberPath = "Name";
            col.SelectedValueMemberPath = "ID";
            var rel = dtgColumnValue_Relationships.Columns[0] as GridViewComboBoxColumn;
            var column = bizColumn.GetColumn(ColumnID,true);

            BizRelationship bizRelationship = new BizRelationship();
            rel.ItemsSource = bizRelationship.GetRelationshipsByTableID(column.TableID);
            rel.DisplayMemberPath = "Name";
            rel.SelectedValueMemberPath = "ID";

        }

        private void dtgColumnValues_SelectionChanged_1(object sender, SelectionChangeEventArgs e)
        {
            if (dtgColumnValues.SelectedItem != null)
            {
                var ColumnValue = dtgColumnValues.SelectedItem as UIColumnValueDTO;
               //اصلاح شود
                //////using (var projectContext = new DataAccess.MyProjectEntities())
                //////{
                //////    dtgColumnValue_Columns.ItemsSource = ColumnValue.Columns;
                //////    dtgColumnValue_Relationships.ItemsSource = ColumnValue.Relationships;
                //////}
            }
            else
            {
                dtgColumnValue_Columns.IsEnabled = false;
                dtgColumnValue_Relationships.IsEnabled = false;
            }
        }

        private void btnUpdateColumnValue_Columns_Click(object sender, EventArgs e)
        {
            //BizColumnValue biz = new BizColumnValue();
            foreach (var item in dtgColumnValue_Columns.ItemsSource as List<UIColumnValueDTO>)
            {
                item.ColumnID = ColumnID;
            }
            ////biz.SaveColumnValue(dtgColumnValue_Columns.ItemsSource as List<ColumnValueDTO>);
            SetColumnValueGrid();
        }

       
        //private void btnUpdateColumnValue_Relationships_Click(object sender, EventArgs e)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var list = dtgColumnValue_Relationships.DataSource as List<ColumnValue_Relationship>;
        //        var ColumnValue = dtgColumnValues.SelectedRows.First().DataBoundItem as ColumnValue;
        //        projectContext.ColumnValue_Relationship.RemoveRange(projectContext.ColumnValue_Relationship.Where(x => x.ColumnValueID == ColumnValue.ID));

        //        var rel = dtgColumnValue_Relationships.Columns[0] as GridViewComboBoxColumn;
        //        var allRelationships = rel.DataSource as List<Relationship>;
        //        List<ColumnValue_Relationship> addList = new List<ColumnValue_Relationship>();
        //        List<int> visitedIds = new List<int>();
        //        foreach (var item in list)
        //        {
        //            if (!visitedIds.Contains(item.RelationshipID))
        //            {
        //                visitedIds.Add(item.RelationshipID);
        //                var pariItem = allRelationships.First(x => x.ID == item.RelationshipID);
        //                if (pariItem != null)
        //                {
        //                    if (pariItem.RelationshipID != null)
        //                    {
        //                        visitedIds.Add(pariItem.RelationshipID.Value);
        //                        if (!list.Any(x => x.RelationshipID == pariItem.RelationshipID))
        //                        {
        //                            addList.Add(new ColumnValue_Relationship() { RelationshipID = pariItem.RelationshipID.Value, Enabled = item.Enabled });
        //                        }
        //                        else
        //                            list.First(x => x.RelationshipID == pariItem.RelationshipID).Enabled = item.Enabled;
        //                    }
        //                    else
        //                    {
        //                        var revRel = allRelationships.First(x => x.RelationshipID == pariItem.ID);

        //                        visitedIds.Add(revRel.ID);
        //                        if (!list.Any(x => x.RelationshipID == revRel.ID))
        //                        {
        //                            addList.Add(new ColumnValue_Relationship() { RelationshipID = revRel.ID, Enabled = item.Enabled });
        //                        }
        //                        else
        //                            list.First(x => x.RelationshipID == revRel.ID).Enabled = item.Enabled;
        //                    }
        //                }
        //            }
        //        }

        //        foreach (var item in addList)
        //        {
        //            list.Add(item);
        //        }
        //        foreach (var item in list)
        //        {
        //            projectContext.ColumnValue_Relationship.Add(new ColumnValue_Relationship() { ColumnValueID = ColumnValue.ID, RelationshipID = item.RelationshipID, Enabled = item.Enabled });
        //        }
        //        projectContext.SaveChanges();

        //        SetColumnValueGrid();
        //    }
        //}


    }
}
