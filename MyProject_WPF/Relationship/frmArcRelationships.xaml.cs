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
    /// Interaction logic for frmArcRelationships.xaml
    /// </summary>
    public partial class frmArcRelationships: UserControl
    {
        int TableDrivedEntityID { set; get; }
        public frmArcRelationships(int tableDrivedEntityID)
        {
            InitializeComponent();
            TableDrivedEntityID = tableDrivedEntityID;
            SetArcGroupGrid();
        }

        private void SetArcGroupGrid()
        {
           
               BizArcRelationship biz = new BizArcRelationship();
            dtgArcGroup.ItemsSource = biz.GetArcRelationshipGroups(TableDrivedEntityID);

            //روابط هر دو طرف تکرار میشوند 
            BizRelationship bizRelationship = new BizRelationship();
            BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
            var entity = bizTableDrivedEntity.GetTableDrivedEntity(TableDrivedEntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithRelationships);
            var listRelationships = entity.Relationships;

            var rel = dtgArcRelationships.Columns[0] as GridViewComboBoxColumn;
            rel.ItemsSource = listRelationships;
            rel.DisplayMemberPath = "Name";
            rel.SelectedValueMemberPath = "ID";
        }

        private void dtgArcGroup_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (dtgArcGroup.SelectedItems.Count > 0)
            {
                var arcGroup = dtgArcGroup.SelectedItems.First() as ArcRelationshipGroupDTO;
                dtgArcRelationships.ItemsSource = arcGroup.Relationships;
            }
            else
            {
                dtgArcRelationships.IsEnabled = false;
            }
        }

        private void btnUpdateArcGroup_Click(object sender, RoutedEventArgs e)
        {
            BizArcRelationship biz = new BizArcRelationship();
            foreach (var item in dtgArcGroup.ItemsSource as List<ArcRelationshipGroupDTO>)
            {
                item.TableDrivedEntityID = TableDrivedEntityID;
            }
            biz.SaveArcRelationshipGroup(dtgArcGroup.ItemsSource as List<ArcRelationshipGroupDTO>);
            SetArcGroupGrid();
        }
    }
}
