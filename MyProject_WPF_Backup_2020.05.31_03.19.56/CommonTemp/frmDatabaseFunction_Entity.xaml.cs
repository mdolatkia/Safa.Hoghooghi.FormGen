using ModelEntites;
using MyFormulaFunctionStateFunctionLibrary;
using MyModelManager;
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
    /// Interaction logic for frmDatabaseFunction.xaml
    /// </summary>
    public partial class frmDatabaseFunction_Entity : UserControl
    {
        public event EventHandler<DatabaseFunctionEntitySelectedArg> DatabaseFunctionEntitySelected;

        BizDatabaseFunction bizDatabaseFunction = new BizDatabaseFunction();
        BizTableDrivedEntity bizEntity = new BizTableDrivedEntity();
        DatabaseFunctionEntityIntention DatabaseFunctionEntityIntention;
        DatabaseFunction_EntityDTO DatabaseFunctionEntity { set; get; }
        TableDrivedEntityDTO Entity { set; get; }

        public frmDatabaseFunction_Entity(DatabaseFunctionEntityIntention databaseFunctionEntityIntention)
        {
            InitializeComponent();

            DatabaseFunctionEntityIntention = databaseFunctionEntityIntention;
            Entity = bizEntity.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), DatabaseFunctionEntityIntention.EntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);

            if (DatabaseFunctionEntityIntention.Type == Enum_DatabaseFunctionEntityIntention.DatabaseFunctionEntityDefinition)
            {
               
                DatabaseFunctionEntity = new DatabaseFunction_EntityDTO();
                if (DatabaseFunctionEntityIntention.DatabaseFunctionID == 0)
                    SetFunctionsTree();
                else
                {
                    DatabaseFunctionEntity.DatabaseFunctionID = DatabaseFunctionEntityIntention.DatabaseFunctionID;
                    SetDBFunctionsParametersToGrid(DatabaseFunctionEntityIntention.DatabaseFunctionID);
                    grdTree.Visibility = Visibility.Collapsed;

                }

            }
            else
            {
                grdTree.Visibility = Visibility.Collapsed;
                DatabaseFunctionEntity = bizDatabaseFunction.GetDatabaseFunctionEntity(DatabaseFunctionEntityIntention.DatabaseFunctionEntityID);
                SetDBFunctionsParametersToGrid(DatabaseFunctionEntity.DatabaseFunctionID, DatabaseFunctionEntity.DatabaseFunctionEntityColumns);
            }
            SetDataGridColumns();
        }

        private void SetDataGridColumns()
        {
            colEntityColumn.ItemsSource = Entity.Columns;
            colEntityColumn.DisplayMemberPath = "Name";
            colEntityColumn.SelectedValueMemberPath = "ID";

            colFixedParam.ItemsSource = Enum.GetValues(typeof(Enum_FixedParam));
        }

        private void SetFunctionsTree()
        {
            var tree = new frmDatabaseFunctionTree(Entity.DatabaseID);
            tree.ItemSelected += Tree_ItemSelected;
            grdTree.Children.Add(tree);
        }

        private void Tree_ItemSelected(object sender, TreeDatabaseFunctionSelectedArg e)
        {
            DatabaseFunctionEntity.DatabaseFunctionID = e.Function.ID;
            SetDBFunctionsParametersToGrid(e.Function.ID);
        }

        private void SetDBFunctionsParametersToGrid(int dbfunctionID, List<DatabaseFunction_Entity_ColumnDTO> existing = null)
        {
            var functionColumns = bizDatabaseFunction.GetDatabaseFunctionParameter(dbfunctionID);
            DatabaseFunctionEntity.DatabaseFunctionEntityColumns = new List<DatabaseFunction_Entity_ColumnDTO>();
            foreach (var item in functionColumns)
            {
                var row = new DatabaseFunction_Entity_ColumnDTO();
                row.DatabaseFunctionParameterID = item.ID;
                row.FunctionColumnParamName = item.ParameterName;
                row.FunctionDataType = item.DataType;
                row.FunctionColumnDotNetType = item.DotNetType;
                if(existing!=null)
                {
                    var existingRow = existing.FirstOrDefault(x => x.FunctionColumnParamName == item.ParameterName);
                    if(existingRow!=null)
                    {
                        row.ColumnID = existingRow.ColumnID;
                    }
                }
                DatabaseFunctionEntity.DatabaseFunctionEntityColumns.Add(row);
            }

            dtgFunctionColumns.ItemsSource = null;
            dtgFunctionColumns.ItemsSource = DatabaseFunctionEntity.DatabaseFunctionEntityColumns;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (DatabaseFunctionEntity == null)
            {
                MessageBox.Show("فانکشن نامشخص است");
                return;
            }
            //if (txtTitle.Text == "")
            //{
            //    MessageBox.Show("عنوان فانکشن نامشخص است");
            //    return;
            //}
            if (DatabaseFunctionEntity.DatabaseFunctionEntityColumns.Any(x => x.ColumnID == 0 && x.FixedParam==Enum_FixedParam.None))
            {
                MessageBox.Show("ستون معادل برای یکی از پارامترهای فانکشن مشخص نشده است");
                return;
            }


            DatabaseFunctionEntity.EntityID = Entity.ID;
            DatabaseFunctionEntity.ID = bizDatabaseFunction.UpdateDatabaseFunctionEntity(DatabaseFunctionEntity);
            if (DatabaseFunctionEntitySelected != null)
                DatabaseFunctionEntitySelected(this, new DatabaseFunctionEntitySelectedArg() { DatabaseFunctionEntityID = DatabaseFunctionEntity.ID });
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }

     
    }

    public class DatabaseFunctionEntitySelectedArg : EventArgs
    {
        public int DatabaseFunctionEntityID { set; get; }
    }

    public class DatabaseFunctionEntityIntention
    {
        public int DatabaseFunctionEntityID { set; get; }
        public int DatabaseFunctionID { get; internal set; }
        public int EntityID { set; get; }
        public Enum_DatabaseFunctionEntityIntention Type { set; get; }
    }
    public enum Enum_DatabaseFunctionEntityIntention
    {
        DatabaseFunctionEntityDefinition,
        DatabaseFunctionEntityEdit
    }
}
