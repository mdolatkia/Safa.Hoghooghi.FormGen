using ModelEntites;
using MyModelManager;
using ProxyLibrary;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmDataSelect.xaml
    /// </summary>
    public partial class frmDataSelect : UserControl
    {
        public event EventHandler<DataSelectedArg> DataSelected;

        TableDrivedEntityDTO Entity { set; get; }
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        List<EntityInstanceProperty> keyColumns = new List<EntityInstanceProperty>();
        public frmDataSelect(int entityID)
        {
            InitializeComponent();
            Entity = bizTableDrivedEntity.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), entityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
            lblEntity.Text = Entity.Alias;
            foreach (var col in Entity.Columns.Where(x => x.PrimaryKey))
            {
                EntityInstanceProperty keyColumn = new EntityInstanceProperty(col);
              //  keyColumn.ColumnID = col.ID;
                //keyColumn.IsKey = true;
                //keyColumn.Name = col.Name;
                keyColumns.Add(keyColumn);
            }
            dtgKeyColumns.ItemsSource = keyColumns;
        }
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (keyColumns.Any(x => x.Value==null|| string.IsNullOrEmpty(x.Value.ToString())))
            {
                MessageBox.Show("تمامی فیلدها باید دارای مقدار باشند");
                return;
            }
            DataSelectedArg arg = new DataSelectedArg() { Columns = keyColumns };
            if (DataSelected != null)
                DataSelected(this, arg);
        }
    }
    public class DataSelectedArg : EventArgs
    {
        public List<EntityInstanceProperty> Columns { set; get; }
    }
}


