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
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace Paper_MetadataManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BizRelationship bizRelationship = new BizRelationship();
        BizColumn bizColumn = new BizColumn();
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        BizTable bizTable = new BizTable();
        BizDatabase bizDatabase = new BizDatabase();
        int DatabaseID { set; get; }
        public MainWindow()
        {
            InitializeComponent();

            dtgEntity.SelectionChanged += dtgEntity_SelectionChanged;
            dtgColumns.SelectionChanged += dtgColumns_SelectionChanged;
            dtgColumnKeyValue.CanUserInsertRows = true;
            dtgColumnKeyValue.CanUserDeleteRows = true;

            var entityMenu = new RadContextMenu();
            entityMenu.Opened += dtgEntity_ContextMenuOpening;
            RadContextMenu.SetContextMenu(dtgEntity, entityMenu);

            var columnMenu = new RadContextMenu();
            columnMenu.Opened += dtgColumns_ContextMenuOpening;
            RadContextMenu.SetContextMenu(dtgColumns, columnMenu);
            SetGridViewColumns();


            dtgManyToMany.SelectionChanged += dtgManyToMany_SelectionChanged;
            dtgISARelationship.SelectionChanged += dtgISARelationship_SelectionChanged;
            dtgUnionRelationship.SelectionChanged += dtgUnionRelationship_SelectionChanged;



            var relationshipMenu = new RadContextMenu();
            relationshipMenu.Opened += dtgRelationships_ContextMenuOpening;
            //فعلا منوی همه روابط یکی شده اند..میتوانند جدا شود
            RadContextMenu.SetContextMenu(dtgRelationships, relationshipMenu);


            RadContextMenu.SetContextMenu(dtgOneToMany, relationshipMenu);
            RadContextMenu.SetContextMenu(dtgManyToOne, relationshipMenu);
            RadContextMenu.SetContextMenu(dtgExplicit, relationshipMenu);
            RadContextMenu.SetContextMenu(dtgImplicit, relationshipMenu);
            RadContextMenu.SetContextMenu(dtgSuperToSub, relationshipMenu);
            RadContextMenu.SetContextMenu(dtgSubToSuper, relationshipMenu);
            RadContextMenu.SetContextMenu(dtgUnionToSubUnion, relationshipMenu);
            RadContextMenu.SetContextMenu(dtgSubUnionToUnion, relationshipMenu);

            var manytomanyMenu = new RadContextMenu();
            manytomanyMenu.Opened += dtgManyToMany_ContextMenuOpening;
            RadContextMenu.SetContextMenu(dtgManyToMany, manytomanyMenu);

            var isaMenu = new RadContextMenu();
            isaMenu.Opened += dtgISARelationship_ContextMenuOpening;
            RadContextMenu.SetContextMenu(dtgISARelationship, isaMenu);

            var unionMenu = new RadContextMenu();
            unionMenu.Opened += dtgUnionRelationship_ContextMenuOpening;
            RadContextMenu.SetContextMenu(dtgUnionRelationship, isaMenu);

            tabManyToManyRelationships.Visibility = Visibility.Collapsed;
            tabManyToOneRelationships.Visibility = Visibility.Collapsed;
            tabOneToManyRelationships.Visibility = Visibility.Collapsed;
            tabExplicitRelationships.Visibility = Visibility.Collapsed;
            tabImplicitRelationships.Visibility = Visibility.Collapsed;
            tabUnionRelationships.Visibility = Visibility.Collapsed;
            tabInheritanceRelationships.Visibility = Visibility.Collapsed;
            btnImposeRelationshipRule.Visibility = Visibility.Collapsed;

        }
        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (txtDBName.Text != "" && txtUsername.Text != "" && txtPassword.Password != "")
            {
                var database = bizDatabase.GetDatabase(txtDBName.Text);
                DatabaseID = database.ID;
                var connection = ConnectionManager.GetDBHelper(database.ID);
                var testConnection = connection.TestConnection();
                if (testConnection.Successful)
                {

                    //Properties.Settings.Default.LastDatabaseID = databaseID;
                    //Properties.Settings.Default.Save();
                    //if (testResult)
                    //{
                    //Properties.Settings.Default.LastDatabaseName = txtDBName.Text;
                    //Properties.Settings.Default.LastServerName = txtServerName.Text;
                    //Properties.Settings.Default.Save();
                    //SetFormMode(FormMode.ConnectionSucceed);

                }
                else
                {
                    MessageBox.Show("Connection Failed!" + Environment.NewLine + testConnection.Message);
                }
                //   MessageBox.Show("Connection Successfull!");
                //}
                //else
                //{
                //    SetFormMode(FormMode.ConnectionFailed);
                //    MessageBox.Show("Connection failed!");
                //}
            }
            else
                MessageBox.Show("اطلاعات اتصال وارد نشده اند");
        }
        private void SetGridViewColumns()
        {
            SetGridViewColumns(dtgEntity);
            SetGridViewColumns(dtgColumns);
            SetGridViewColumns(dtgColumnKeyValue);
            SetGridViewColumns(dtgStringColumnType);
            SetGridViewColumns(dtgNumericColumnType);
            SetGridViewColumns(dtgDateColumnType);

            SetGridViewColumns(dtgRelationships);
            SetGridViewColumns(dtgManyToMany);
            SetGridViewColumns(dtgISARelationship);
            SetGridViewColumns(dtgUnionRelationship);
            SetGridViewColumns(dtgOneToMany);
            SetGridViewColumns(dtgManyToOne);
            SetGridViewColumns(dtgExplicit);
            SetGridViewColumns(dtgImplicit);
            SetGridViewColumns(dtgSuperToSub);
            SetGridViewColumns(dtgSubToSuper);
            SetGridViewColumns(dtgUnionToSubUnion);
            SetGridViewColumns(dtgSubUnionToUnion);

        }
        private void SetGridViewColumns(RadGridView dataGrid)
        {
            dataGrid.AlternateRowBackground = new SolidColorBrush(Colors.LightBlue);
            dataGrid.AutoGenerateColumns = false;

            if (dataGrid == dtgEntity)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("TableName", "TableName", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("RelatedSchema", "RelatedSchema", false, 100, GridViewColumnType.Text));

                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DatabaseName", "DatabaseName", false, 100, GridViewColumnType.Text));


                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Criteria", "Criteria", false, 200, GridViewColumnType.Text));
                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IndependentDataEntry", "IndependentDataEntry", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("BatchDataEntry", "BatchDataEntry", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsDataReference", "IsDataReference", false, 160, GridViewColumnType.CheckBox));
                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsStructurReferencee", "IsStructurReferencee", false, 160, GridViewColumnType.CheckBox));
                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsAssociative", "IsAssociative", false, 160, GridViewColumnType.CheckBox));
            }
            else if (dataGrid == dtgColumns)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataEntryEnabled", "DataEntryEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PrimaryKey", "PrimaryKey", true, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsIdentity", "IsIdentity", true, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsNull", "IsNull", true, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsMandatory", "IsMandatory", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DefaultValue", "DefaultValue", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataType", "DataType", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Position", "Position", false, 100, GridViewColumnType.Numeric));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SearchEnabled", "SearchEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ViewEnabled", "ViewEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ViewAggregatedData", "ViewAggregatedData", false, 100, GridViewColumnType.CheckBox));
            }
            else if (dataGrid == dtgStringColumnType)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ColumnID", "ColumnID", true, 100, GridViewColumnType.Numeric));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ColumnName", "ColumnName", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("MaxLength", "MaxLength", false, 100, GridViewColumnType.Numeric));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Format", "Format", false, 200, GridViewColumnType.Text));
            }
            else if (dataGrid == dtgNumericColumnType)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ColumnID", "ColumnID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Precision", "Precision", false, 100, GridViewColumnType.Numeric));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Scale", "Scale", false, 200, GridViewColumnType.Numeric));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("MinValue", "MinValue", false, 100, GridViewColumnType.Numeric));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("MaxValue", "MaxValue", false, 200, GridViewColumnType.Numeric));
            }
            else if (dataGrid == dtgDateColumnType)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ColumnID", "ColumnID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsPersianDate", "IsPersianDate", false, 100, GridViewColumnType.CheckBox));
            }
            else if (dataGrid == dtgColumnKeyValue)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ColumnName", "Column", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Value", "Value", false, 100, GridViewColumnType.Numeric));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("KeyTitle", "KeyTitle", false, 200, GridViewColumnType.Text));
            }

            if (dataGrid == dtgRelationships)
            {

                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "Entity1", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "Entity2", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataEntryEnabled", "DataEntryView", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "PairRelationshipID", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "PairRelationship", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("RelationshipColumns", "RelationshipColumns", true, 160, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("TypeStr", "TypeStr", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideTransferable", "IsOtherSideTransferable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideMandatory", "IsOtherSideMandatory", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideCreatable", "IsOtherSideCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideDirectlyCreatable", "IsOtherSideDirectlyCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SearchEnabled", "SearchEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ViewEnabled", "ViewEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "Enabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.CanUserDeleteRows = false;
            }
            else if (dataGrid == dtgOneToMany)
            {

                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "PairRelationshipID", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "PairRelationship", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "Entity1", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "Entity2", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("MasterDetailState", "MasterDetailState", false, 100, GridViewColumnType.Numeric));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DetailsCount", "DetailsCount", false, 100, GridViewColumnType.Numeric));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideMandatory", "IsOtherSideMandatory", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideCreatable", "IsOtherSideCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideDirectlyCreatable", "IsOtherSideDirectlyCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataEntryEnabled", "DataEntryEnabled", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SearchEnabled", "SearchEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ViewEnabled", "ViewEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "Enabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.CanUserDeleteRows = false;
            }
            else if (dataGrid == dtgManyToOne)
            {

                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "PairRelationshipID", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "PairRelationship", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "Entity1", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "Entity2", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideTransferable", "IsOtherSideTransferable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideMandatory", "IsOtherSideMandatory", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideCreatable", "IsOtherSideCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideDirectlyCreatable", "IsOtherSideDirectlyCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataEntryEnabled", "DataEntryEnabled", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SearchEnabled", "SearchEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ViewEnabled", "ViewEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "Enabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.CanUserDeleteRows = false;
            }
            else if (dataGrid == dtgExplicit)
            {

                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "PairRelationshipID", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "PairRelationship", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "Entity1", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "Entity2", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideTransferable", "IsOtherSideTransferable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideMandatory", "IsOtherSideMandatory", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideCreatable", "IsOtherSideCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideDirectlyCreatable", "IsOtherSideDirectlyCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataEntryEnabled", "DataEntryEnabled", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SearchEnabled", "SearchEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ViewEnabled", "ViewEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "Enabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.CanUserDeleteRows = false;
            }
            else if (dataGrid == dtgImplicit)
            {

                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "PairRelationshipID", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "PairRelationship", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "Entity1", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "Entity2", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideTransferable", "IsOtherSideTransferable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideMandatory", "IsOtherSideMandatory", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideCreatable", "IsOtherSideCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideDirectlyCreatable", "IsOtherSideDirectlyCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataEntryEnabled", "DataEntryEnabled", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SearchEnabled", "SearchEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ViewEnabled", "ViewEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "Enabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.CanUserDeleteRows = false;
            }
            else if (dataGrid == dtgManyToMany)
            {

                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
            }
            else if (dataGrid == dtgManyToMany_ManyToOne)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "PairRelationshipID", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "PairRelationship", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "Entity1", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "Entity2", true, 100, GridViewColumnType.Text));
            }
            else if (dataGrid == dtgISARelationship)
            {
                dataGrid.SelectionMode = SelectionMode.Extended;

                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SuperTypeEntities", "SuperTypeEntities", true, 120, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SubTypeEntities", "SubTypeEntities", true, 120, GridViewColumnType.Text));
                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsGeneralization", "IsGeneralization", false, 100, GridViewColumnType.CheckBox));
                //dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsSpecialization", "IsSpecialization", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsTolatParticipation", "IsTolatParticipation", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsDisjoint", "IsDisjoint", false, 100, GridViewColumnType.CheckBox));
                dataGrid.CanUserDeleteRows = false;
            }
            else if (dataGrid == dtgSuperToSub)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "PairRelationshipID", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "PairRelationship", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "Entity1", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "Entity2", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideTransferable", "IsOtherSideTransferable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideCreatable", "IsOtherSideCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideDirectlyCreatable", "IsOtherSideDirectlyCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataEntryEnabled", "DataEntryEnabled", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SearchEnabled", "SearchEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ViewEnabled", "ViewEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "Enabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.CanUserDeleteRows = false;
            }
            else if (dataGrid == dtgSubToSuper)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "PairRelationshipID", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "PairRelationship", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "Entity1", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "Entity2", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideTransferable", "IsOtherSideTransferable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideCreatable", "IsOtherSideCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideDirectlyCreatable", "IsOtherSideDirectlyCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataEntryEnabled", "DataEntryEnabled", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SearchEnabled", "SearchEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ViewEnabled", "ViewEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "Enabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.CanUserDeleteRows = false;
            }
            else if (dataGrid == dtgUnionRelationship)
            {
                dataGrid.SelectionMode = SelectionMode.Extended;

                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("UnionTypeEntities", "UnionTypeEntities", true, 120, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SubUnionTypeEntities", "SubUnionTypeEntities", true, 160, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsTolatParticipation", "IsTolatParticipation", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("UnionHoldsKeys", "UnionHoldsKeys", false, 130, GridViewColumnType.CheckBox));
                dataGrid.CanUserDeleteRows = false;
            }
            else if (dataGrid == dtgUnionToSubUnion)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "PairRelationshipID", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "PairRelationship", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "Entity1", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "Entity2", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideTransferable", "IsOtherSideTransferable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideCreatable", "IsOtherSideCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideDirectlyCreatable", "IsOtherSideDirectlyCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataEntryEnabled", "DataEntryEnabled", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SearchEnabled", "SearchEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ViewEnabled", "ViewEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "Enabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.CanUserDeleteRows = false;
            }
            else if (dataGrid == dtgSubUnionToUnion)
            {
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ID", "ID", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Name", "Name", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationshipID", "PairRelationshipID", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("PairRelationship", "PairRelationship", true, 130, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Alias", "Alias", false, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity1", "Entity1", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Entity2", "Entity2", true, 100, GridViewColumnType.Text));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideTransferable", "IsOtherSideTransferable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideCreatable", "IsOtherSideCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("IsOtherSideDirectlyCreatable", "IsOtherSideDirectlyCreatable", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("DataEntryEnabled", "DataEntryEnabled", false, 160, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("SearchEnabled", "SearchEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("ViewEnabled", "ViewEnabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.Columns.Add(ControlHelper.GenerateGridviewColumn("Enabled", "Enabled", false, 100, GridViewColumnType.CheckBox));
                dataGrid.CanUserDeleteRows = false;
            }
        }

        void dtgEntity_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (dtgEntity.SelectedItem != null)
            {
                TableDrivedEntityDTO entity = dtgEntity.SelectedItem as TableDrivedEntityDTO;
                if (entity != null)
                {//بهتره ستونها کامل گرفته نشوند و تک به تک کامل شوند
                    dtgColumns.ItemsSource = bizColumn.GetColumns(entity.ID, false);
                    SetColumnTabs();
                    btnUpdateColumns.IsEnabled = true;
                }
            }
        }


        private void btnUpdateColumns_Click(object sender, RoutedEventArgs e)
        {
            btnUpdateColumns.IsEnabled = false;
            bizColumn.UpdateColumns(dtgColumns.ItemsSource as List<ColumnDTO>);
            btnUpdateColumns.IsEnabled = true;
        }
        void dtgColumns_SelectionChanged(object sender, RoutedEventArgs e)
        {
            SetColumnTabs();
            if (dtgColumns.SelectedItem != null)
            {
                var column = dtgColumns.SelectedItem as ColumnDTO;
                if (column != null)
                {



                    if (column.ColumnType == Enum_ColumnType.String)
                    {
                        dtgStringColumnType.ItemsSource = bizColumn.GetStringColumType(column.ID);
                    }
                    else if (column.ColumnType == Enum_ColumnType.Numeric)
                    {
                        dtgNumericColumnType.ItemsSource = bizColumn.GetNumericColumType(column.ID);
                    }
                    else if (column.ColumnType == Enum_ColumnType.Date)
                    {
                        dtgDateColumnType.ItemsSource = bizColumn.GetDateColumType(column.ID);
                    }
                    var columnKeyValue = bizColumn.GetColumnKeyValue(column.ID);
                    if (columnKeyValue != null)
                    {
                        optValueComesFromKey.IsChecked = columnKeyValue.ValueFromKeyOrValue;
                        optValueComesFromValue.IsChecked = !columnKeyValue.ValueFromKeyOrValue;
                        dtgColumnKeyValue.ItemsSource = columnKeyValue.ColumnKeyValueRange;
                    }
                    else
                    {
                        optValueComesFromKey.IsChecked = false;
                        optValueComesFromValue.IsChecked = false;
                        dtgColumnKeyValue.ItemsSource = new List<ColumnKeyValueRangeDTO>();
                    }

                    //var formulaDTO = bizColumn.GetCustomCalculationFormula(column.ID);
                    //tabColumnFormula.DataContext = formulaDTO;
                    //if (formulaDTO != null)
                    //{
                    //    txtColumnFormula.Text = formulaDTO.Formula;
                    //}
                    //else
                    //    txtColumnFormula.Text = "";

                }
            }
        }

        private void SetColumnTabs()
        {
            tabStringColumnType.Visibility = System.Windows.Visibility.Collapsed;
            tabNumericColumnType.Visibility = System.Windows.Visibility.Collapsed;
            tabDateColumnType.Visibility = System.Windows.Visibility.Collapsed;
            tabKeyValueRange.Visibility = System.Windows.Visibility.Collapsed;
            if (dtgColumns.SelectedItem != null)
            {
                var column = dtgColumns.SelectedItem as ColumnDTO;
                tabKeyValueRange.Visibility = System.Windows.Visibility.Visible;
              
                btnUpdateStringColumnType.IsEnabled = true;
                btnUpdateNumericColumnType.IsEnabled = true;
                btnUpdateDateColumnType.IsEnabled = true;
                btnUpdateKeyValue.IsEnabled = true;
                btnImportKeyValues.IsEnabled = true;
                optValueComesFromKey.IsEnabled = true;
                optValueComesFromValue.IsEnabled = true;

                if (column.ColumnType == Enum_ColumnType.String)
                {
                    tabStringColumnType.Visibility = System.Windows.Visibility.Visible;
                }
                else if (column.ColumnType == Enum_ColumnType.Numeric)
                {
                    tabNumericColumnType.Visibility = System.Windows.Visibility.Visible;
                }
                else if (column.ColumnType == Enum_ColumnType.Date)
                {
                    tabDateColumnType.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        private void btnUpdateKeyValue_Click(object sender, RoutedEventArgs e)
        {
            var column = dtgColumns.SelectedItem as ColumnDTO;
            if (column != null)
            {

                btnUpdateKeyValue.IsEnabled = false;
                bizColumn.UpdateColumnKeyValue(column.ID, optValueComesFromKey.IsChecked == true, dtgColumnKeyValue.ItemsSource as List<ColumnKeyValueRangeDTO>);
                btnUpdateKeyValue.IsEnabled = true;
            }
        }
        private void btnImportKeyValues_Click(object sender, RoutedEventArgs e)
        {
            if (optValueComesFromKey.IsChecked == false && optValueComesFromValue.IsChecked == false)
            {
                MessageBox.Show("لطفا نوع جایگذاری مقادیر ستون در عنوان یا مقدار را مشخص نمایید");
                return;
            }
            var column = dtgColumns.SelectedItem as ColumnDTO;
            if (column != null)
            {
                //try
                //{
                //////var result = ModelGenerator.GenerateKeyColumns(column.ID, optValueComesFromKey.IsChecked == true);
                //////if (result)
                //////{
                //////    var columnKeyValue = bizColumn.GetColumnKeyValue(column.ID);
                //////    if (columnKeyValue != null)
                //////    {
                //////        optValueComesFromKey.IsChecked = columnKeyValue.ValueFromKeyOrValue;
                //////        optValueComesFromValue.IsChecked = !columnKeyValue.ValueFromKeyOrValue;
                //////        dtgColumnKeyValue.ItemsSource = columnKeyValue.ColumnKeyValueRange;
                //////    }
                //////    else
                //////    {
                //////        optValueComesFromKey.IsChecked = false;
                //////        optValueComesFromValue.IsChecked = false;
                //////        dtgColumnKeyValue.ItemsSource = new List<ColumnKeyValueRangeDTO>();
                //////    }

                //////    MessageBox.Show("Operation is completed.");
                //////}
                //////else
                //////    MessageBox.Show("Operation is not done!");

            }
        }

        void dtgEntity_ContextMenuOpening(object sender, RoutedEventArgs e)
        {

        }

        void dtgColumns_ContextMenuOpening(object sender, RoutedEventArgs e)
        {
            var contextMenu = sender as RadContextMenu;
            contextMenu.Items.Clear();
            var source = contextMenu.GetClickedElement<GridViewRow>();
            if (contextMenu != null && source != null)
            {
                //RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
                //contextMenu.Items.Add(separator);
                var column = source.DataContext as ColumnDTO;
                if (column.ColumnType == Enum_ColumnType.String)
                {
                    var convertToDateColumnMenu = AddMenu(contextMenu.Items, "تبدیل به نوع تاریخ", "", "../Images/date.png");
                    //convertToDateColumnMenu.Click += (sender1, EventArgs) => ConvertToDateColumnType_Click1(sender, e, column.ID);
                }
                else if (column.ColumnType == Enum_ColumnType.Date)
                {
                    var convertToStringColumnMenu = AddMenu(contextMenu.Items, "Convert to string type", "", "../Images/string.png");
                    //convertToStringColumnMenu.Click += (sender1, EventArgs) => ConvertToStringColumnType_Click1(sender, e, column.ID);
                }

                var columnRuleMenu = AddMenu(contextMenu.Items, "اصلاح شروط بری ستون", "", "../Images/columnrule.png");
                //columnRuleMenu.Click += (sender1, EventArgs) => ConvertToDateColumnType_Click1RuleOnValue(sender, e, column.ID);

                var columnFormulaMenu = AddMenu(contextMenu.Items, "تعریف فرمول محاسباتی بروی ستون", "", "../Images/formula1.png");
                //columnFormulaMenu.Click += (sender1, EventArgs) => DefineFormulaForColumn(sender, e, column.ID);
            }
            //MyProjectHelper.SetContectMenuVisibility(contextMenu);
        }
        private RadMenuItem AddMenu(ItemCollection itemCollection, string title, string name, string imagePath)
        {
            RadMenuItem menu = new RadMenuItem();

            //StackPanel pnl = new StackPanel();
            //pnl.Orientation = Orientation.Horizontal;
            //pnl.Children.Add(new TextBlock() { Text = title });
            Image img = new Image();
            img.Width = 15;
            var uriSource = new Uri(imagePath, UriKind.Relative);
            img.Source = new BitmapImage(uriSource);
            menu.Icon = img;
            //pnl.Children.Add(img);
            menu.Header = title;
            if (!string.IsNullOrEmpty(name))
                menu.Name = name;
            itemCollection.Add(menu);
            return menu;
        }
        private void btnUpdateEntities_Click(object sender, RoutedEventArgs e)
        {
            BizTableDrivedEntity biz = new BizTableDrivedEntity();
            btnUpdateEntities.IsEnabled = false;
            biz.Save(dtgEntity.ItemsSource as List<TableDrivedEntityDTO>);
            btnUpdateEntities.IsEnabled = true;
        }

        private void btnRefreshEntityRules_Click(object sender, RoutedEventArgs e)
        {
            var list = bizTableDrivedEntity.GetAllEntities(DatabaseID);
            dtgEntity.ItemsSource = list;
            lblEntitiesCount.Text = "Count : " + list.Count;
        }

        void dtgRelationships_ContextMenuOpening(object sender, RoutedEventArgs e)
        {
            var contextMenu = sender as RadContextMenu;
            contextMenu.Items.Clear();
            var source = contextMenu.GetClickedElement<GridViewRow>();
            if (contextMenu != null && source != null)
            {
                //AddConvertRelationshipContextMenu(contextMenu, source.DataContext as RelationshipDTO);
                AddMenu(contextMenu.Items, "Convert to one-to-many relationship", "", "../Images/filter.png");
                AddMenu(contextMenu.Items, "Convert to one-to-one relationship", "", "../Images/filter.png");
                //relationshipFilterMenu.Click += (sender1, EventArgs) => MnuRelationshipFilter_Click(sender, EventArgs, (source.DataContext as RelationshipDTO).ID);

                //var relationship = (source.DataContext as RelationshipDTO);
                //if (relationship.TypeEnum == Enum_RelationshipType.None)
                //{
                //    var relationshipTypeMenu = AddMenu(contextMenu.Items, "تعیین نوع رابطه", "", "../Images/type.png");
                //    relationshipTypeMenu.Click += (sender1, EventArgs) => MnuRelationshipType_Click(sender, EventArgs, (source.DataContext as RelationshipDTO).ID);
                //}

                //if (relationship.Created == true)
                //{
                //    var relationshipEditMenu = AddMenu(contextMenu.Items, "اصلاح رابطه", "", "../Images/edit.png");
                //    relationshipEditMenu.Click += (sender1, EventArgs) => MnuRelationshipEdit_Click(sender, EventArgs, (source.DataContext as RelationshipDTO).ID);
                //}
            }
            //var relationshipCreateMenu = AddMenu(contextMenu.Items, "تعریف رابطه جدید", "", "../Images/edit.png");
            //relationshipCreateMenu.Click += MnuRelationshipCreate_Click;


        }
        private void MnuRelationshipCreate_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            //frmRelationshipCreate view = new frmRelationshipCreate(0);
            //view.ShowDialog();
        }
        private void MnuRelationshipEdit_Click(object sender, Telerik.Windows.RadRoutedEventArgs e, int relationshipID)
        {
            //frmRelationshipCreate view = new frmRelationshipCreate(relationshipID);
            //view.ShowDialog();
        }
        private void MnuRelationshipFilter_Click(object sender, Telerik.Windows.RadRoutedEventArgs e, int relationshipID)
        {
            //frmRelationshipFilter view = new frmRelationshipFilter(relationshipID, 0);
            //view.ShowDialog();
        }
        private void MnuRelationshipType_Click(object sender, Telerik.Windows.RadRoutedEventArgs e, int relationshipID)
        {
            //bizRelationship.ImposeSingleRelationshipType(relationshipID);
        }





        void dtgManyToMany_ContextMenuOpening(object sender, RoutedEventArgs e)
        {
            var contextMenu = sender as RadContextMenu;
            contextMenu.Items.Clear();
            var source = contextMenu.GetClickedElement<GridViewRow>();
            var mtmCreateMenu = AddMenu(contextMenu.Items, "ایجاد رابطه چند به چند", "", "../Images/addnew.png");
            mtmCreateMenu.Click += customMenuItem_Click;
            if (contextMenu != null && source != null)
            {
                //RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
                //contextMenu.Items.Add(separator);
                //var entity = dtgManyToMany.CurrentRow.DataBoundItem as ManyToMany;
                //if (entity != null)
                //{


                var mtmRemoveMenu = AddMenu(contextMenu.Items, "حذف رابطه چند به چند", "", "../Images/remove.png");
                mtmRemoveMenu.Click += (sender1, EventArgs) => customMenuItemRemove_Click(sender, e, source.DataContext as ManyToManyRelationshipDTO);

            }

        }

        void customMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //BizTable bizTable = new BizTable();
            //var list = bizTable.GetTablesWithManyToOneRelationships(DatabaseID);
            //var selectView = new frmChooseItem();
            //selectView.ShowItems<TableDTO>(list, false);
            //selectView.ItemSelected += selectView_ItemSelected;
            //selectView.Title = "Select Associative Table";
            //selectView.ShowDialog();
        }

        //void selectView_ItemSelected(object sender, SelectedItemsArg e)
        //{
        //    //frmCreateManyToManyRelationship view = new frmCreateManyToManyRelationship((e.Items.First() as TableDTO).ID);
        //    //view.ManyToManyCreated += view_ManyToManyCreated;
        //    //view.ShowDialog();
        //}

        //void view_ManyToManyCreated(object sender, ManyToManyCreatedArg e)
        //{
        //    //using (var projectContext = new DataAccess.MyProjectEntities())
        //    //{
        //    //    ManyToManyRelationshipType manyToManyRelationshipType = new ManyToManyRelationshipType();
        //    //    manyToManyRelationshipType.Name = e.Name;
        //    //    foreach (var id in e.ManyToOneIDs)
        //    //        manyToManyRelationshipType.ManyToOneRelationshipType.Add(projectContext.ManyToOneRelationshipType.First(x => x.ID == id));

        //    //    var dbTable = projectContext.Table.First(x => x.ID == e.TableID);
        //    //    dbTable.IsAssociative = true;

        //    //    projectContext.ManyToManyRelationshipType.Add(manyToManyRelationshipType);
        //    //    projectContext.SaveChanges();

        //    //    var srvdb = GeneralHelper.GetServerNameDatabaseName(dbTable.Catalog);
        //    //    GetManyToManyRelationships(srvdb.Item1, srvdb.Item2);
        //    //}
        //}

        void customMenuItemRemove_Click(object sender, RoutedEventArgs e, ManyToManyRelationshipDTO manyToMany)
        {
            bizRelationship.RemoveManyToManyRelationships(manyToMany.ID);
        }

        //void dtgManyToMany_ManyToOne_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        //{
        //    RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
        //    contextMenu.Items.Add(separator);
        //    var entity = dtgManyToMany_ManyToOne.CurrentRow.DataBoundItem as ManyToOne;
        //    if (entity != null)
        //    {

        //        RadMenuItem customMenuItem = new RadMenuItem();
        //        customMenuItem.Header = "Remove from Many To Many Relationship";
        //        customMenuItem.Name = "RemoveManyToOne";
        //        customMenuItem.Click += (sender1, EventArgs) => customMenuItem_Click11(sender, e, entity);
        //        contextMenu.Items.Add(customMenuItem);


        //    }
        //}
        //void customMenuItem_Click11(object sender, RoutedEventArgs e, ManyToOne entity)
        //{
        //    MyFormHelper.RemoveManyToOneFromManyToManyRelationship(entity);
        //}

        //void customMenuItem_Click1(object sender, RoutedEventArgs e, ManyToMany entity)
        //{

        //}


        //private void AddConvertRelationshipContextMenu(RadContextMenu contextMenu, RelationshipDTO relationship)
        //{
        //    if (relationship == null)
        //        return;
        //    //RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
        //    //contextMenu.Items.Add(separator);
        //    BizRelationshipConverter bizRelationshipConverter = new BizRelationshipConverter();
        //    var targetTypes = bizRelationshipConverter.GetRelationshipConvertOptions(relationship);
        //    if (targetTypes.Any())
        //    {
        //        var relationshipConvertMenu = AddMenu(contextMenu.Items, "تبدیل رابطه", "", "../Images/relationship1.png");

        //        foreach (var item in targetTypes)
        //        {
        //            var title = "تبدیل به رابطه " + bizRelationship.GetRelationshipTypeTitle(item);
        //            var convertMenu = AddMenu(relationshipConvertMenu.Items, title, "", "../Images/convert.png", item);
        //            convertMenu.Click += (sender, EventArgs) => customMenuItem_Click(sender, relationship);
        //        }
        //    }
        //}

        void customMenuItem_Click(object sender, RelationshipDTO relationship)
        {

        }

        void dtgUnionRelationship_ContextMenuOpening(object sender, RoutedEventArgs e)
        {
            var contextMenu = sender as RadContextMenu;
            List<UnionRelationshipDTO> list = new List<UnionRelationshipDTO>();
            foreach (var row in dtgUnionRelationship.SelectedItems)
            {
                list.Add(row as UnionRelationshipDTO);
            }
            if (list.Count > 1)
            {


                var source = contextMenu.GetClickedElement<GridViewRow>();
                contextMenu.Items.Clear();
                if (contextMenu != null && source != null)
                {
                    var mergeUnionMenu = AddMenu(contextMenu.Items, "ادغام روابط اجتماع انتخاب شده", "", "../Images/merge.png");
                    mergeUnionMenu.Click += (sender1, EventArgs) => customMenuItem_Click(sender1, contextMenu, list, source.DataContext as UnionRelationshipDTO);
                }

            }

        }
        //private void AddMergeUnionRelationshipContextMenu(RadContextMenu contextMenu, List<UnionRelationshipDTO> relationships, UnionRelationshipDTO selectedOne)
        //{
        //    RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
        //    RadMenuItem customMenuItem = new RadMenuItem();
        //    customMenuItem.Header = "ادغام روابط اجتماع انتخاب شده";
        //    customMenuItem.Name = "MergeUnionRelationships";

        //    contextMenu.Items.Add(separator);
        //    contextMenu.Items.Add(customMenuItem);
        //}

        void customMenuItem_Click(object sender, RadContextMenu contextMenu, List<UnionRelationshipDTO> relationships, UnionRelationshipDTO selectedOne)
        {
            if (relationships.GroupBy(x => new { x.SuperTypeEntities, x.UnionHoldsKeys }).Count() > 1)
            {
                MessageBox.Show("برای ادغام چند رابطه اتحاد تمامی موجودیتهای پدر میباست از یک نوع باشند");
                return;
            }
            //bizUnionRelationship.MergeUnionRelationships((sender as RadMenuItem).Name, relationships, selectedOne);
        }


        void dtgISARelationship_ContextMenuOpening(object sender, RoutedEventArgs e)
        {
            var contextMenu = sender as RadContextMenu;
            List<ISARelationshipDTO> list = new List<ISARelationshipDTO>();
            foreach (var row in dtgISARelationship.SelectedItems)
            {
                list.Add(row as ISARelationshipDTO);
            }
            if (list.Count > 1)
            {

                contextMenu.Items.Clear();
                var source = contextMenu.GetClickedElement<GridViewRow>();
                if (contextMenu != null && source != null)
                {
                    var mergeISAMenu = AddMenu(contextMenu.Items, "ادغام روابط ارث بری انتخاب شده", "", "../Images/merge.png");
                    mergeISAMenu.Click += (sender1, EventArgs) => customMenuItem_Click(sender1, contextMenu, list, source.DataContext as ISARelationshipDTO);
                }
            }

        }
        //private void AddMergeISARelationshipContextMenu(RadContextMenu contextMenu, List<ISARelationshipDTO> relationships, ISARelationshipDTO selectedOne)
        //{
        //    RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
        //    RadMenuItem customMenuItem = new RadMenuItem();
        //    customMenuItem.Header = "ادغام روابط ارث بری انتخاب شده";
        //    customMenuItem.Name = "MergeISARelationships";

        //    contextMenu.Items.Add(separator);
        //    contextMenu.Items.Add(customMenuItem);
        //}

        void customMenuItem_Click(object sender, RadContextMenu contextMenu, List<ISARelationshipDTO> relationships, ISARelationshipDTO selectedOne)
        {
            if (relationships.GroupBy(x => x.SuperTypeEntities).Count() > 1)
            {
                MessageBox.Show("برای ادغام چند رابطه ارث بری تمامی موجودیتهای پدر میباست از یک نوع باشند");
                return;
            }
            //bizISARelationship.MergeISARelationships((sender as RadMenuItem).Name, relationships, selectedOne);
        }


        //private void btnImportRelationships_Click(object sender, RoutedEventArgs e)
        //{
        //    if (MessageBox.Show("فرایند ورود اطلاعات خودکار روابط برای موجودیتهای پیش فرض" + Environment.NewLine + Environment.NewLine + "آیا مطمئن هستید؟", "تائید", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        //    {



        //        //try
        //        //{
        //        var result = ModelGenerator.GenerateRelationships();
        //        if (result)
        //        {

        //            dtgRelationships.ItemsSource = bizRelationship.GetRelationships(DatabaseID);
        //            btnUpdateRelationships.IsEnabled = true;

        //            MessageBox.Show("Operation is completed.");
        //        }
        //        else
        //            MessageBox.Show("Operation is not done!");
        //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    MessageBox.Show("Operation failed." + Environment.NewLine + ex.Message);
        //    //}
        //}

        private void btnRefreshRelationship_Click(object sender, RoutedEventArgs e)
        {
            dtgRelationships.ItemsSource = bizRelationship.GetRelationships(DatabaseID);
        }
        private void btnUpdateRelationships_Click(object sender, RoutedEventArgs e)
        {

            btnUpdateRelationships.IsEnabled = false;
            bizRelationship.UpdateRelationships(dtgRelationships.ItemsSource as List<RelationshipDTO>);
            btnUpdateRelationships.IsEnabled = true;
        }


        private void btnImposeRuleRelationship_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("فرایند اعمال قوانین بروی روابط" + Environment.NewLine + Environment.NewLine + "آیا مطمئن هستید؟", "تائید", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                //bizRelationship.ImposeRelationshipRule(DatabaseID);
                //dtgRelationships.ItemsSource = bizRelationship.GetRelationships(DatabaseID);
            }
        }
        private void btnUpdateOneToMany_Click(object sender, RoutedEventArgs e)
        {

            btnUpdateOneToMany.IsEnabled = false;
            bizRelationship.UpdateOneToManyRelationships(dtgOneToMany.ItemsSource as List<OneToManyRelationshipDTO>);
            btnUpdateOneToMany.IsEnabled = true;
        }
        private void btnRefreshOneToMany_Click(object sender, RoutedEventArgs e)
        {

            dtgOneToMany.ItemsSource = bizRelationship.GetOneToManyRelationships(DatabaseID);
            btnUpdateOneToMany.IsEnabled = true;
        }

        private void btnUpdateManyToOne_Click(object sender, RoutedEventArgs e)
        {

            btnUpdateManyToOne.IsEnabled = false;
            bizRelationship.UpdateManyToOneRelationships(dtgManyToOne.ItemsSource as List<ManyToOneRelationshipDTO>);
            btnUpdateManyToOne.IsEnabled = true;
        }
        private void btnRefreshManyToOne_Click(object sender, RoutedEventArgs e)
        {

            dtgManyToOne.ItemsSource = bizRelationship.GetManyToOneRelationships(DatabaseID);
            btnUpdateManyToOne.IsEnabled = true;
        }


        private void btnUpdateExplicit_Click(object sender, RoutedEventArgs e)
        {

            btnUpdateExplicit.IsEnabled = false;
            bizRelationship.UpdateExplicitOneToOneRelationships(dtgExplicit.ItemsSource as List<ExplicitOneToOneRelationshipDTO>);
            btnUpdateExplicit.IsEnabled = true;
        }
        private void btnRefreshExplicit_Click(object sender, RoutedEventArgs e)
        {

            dtgExplicit.ItemsSource = bizRelationship.GetExplicitOneToOneRelationships(DatabaseID);
        }
        private void btnUpdateImplicit_Click(object sender, RoutedEventArgs e)
        {

            btnUpdateImplicit.IsEnabled = false;
            bizRelationship.UpdateImplicitOneToOneRelationships(dtgImplicit.ItemsSource as List<ImplicitOneToOneRelationshipDTO>);
            btnUpdateImplicit.IsEnabled = true;
        }
        private void btnRefreshImplicit_Click(object sender, RoutedEventArgs e)
        {

            dtgImplicit.ItemsSource = bizRelationship.GetImplicitOneToOneRelationships(DatabaseID);
        }


        private void btnUpdateManyToMany_Click(object sender, RoutedEventArgs e)
        {

            btnUpdateManyToMany.IsEnabled = false;
            bizRelationship.UpdateManyToManyRelationships(dtgManyToMany.ItemsSource as List<ManyToManyRelationshipDTO>);
            btnUpdateManyToMany.IsEnabled = true;
        }
        private void btnRefreshManyToMany_Click(object sender, RoutedEventArgs e)
        {
            btnUpdateManyToMany.IsEnabled = true;

            dtgManyToMany.ItemsSource = bizRelationship.GetManyToManyRelationships(DatabaseID);
        }

        void dtgManyToMany_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (dtgManyToMany.SelectedItem != null)
            {

                ManyToManyRelationshipDTO manyToMany = dtgManyToMany.SelectedItem as ManyToManyRelationshipDTO;
                if (manyToMany != null)
                {

                    dtgManyToMany_ManyToOne.ItemsSource = bizRelationship.GetManyToMany_ManyToOneRelationships(manyToMany.ID);
                }
            }
        }





        private void btnUpdateISA_Click(object sender, RoutedEventArgs e)
        {
            BizISARelationship biz = new BizISARelationship();
            btnUpdateISA.IsEnabled = false;
            biz.UpdateISARelationships(dtgISARelationship.ItemsSource as List<ISARelationshipDTO>);
            btnUpdateISA.IsEnabled = true;
        }
        private void btnRefreshISA_Click(object sender, RoutedEventArgs e)
        {
            btnUpdateISA.IsEnabled = true;
            BizISARelationship biz = new BizISARelationship();
            dtgISARelationship.ItemsSource = biz.GetISARelationships(DatabaseID);
        }
        void dtgISARelationship_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (dtgISARelationship.SelectedItem != null)
            {
                BizISARelationship biz = new BizISARelationship();
                ISARelationshipDTO iSARelationship = dtgISARelationship.SelectedItem as ISARelationshipDTO;
                if (iSARelationship != null)
                {
                    dtgSuperToSub.ItemsSource = biz.GetSuperToSubRelationship(iSARelationship.ID);
                    dtgSubToSuper.ItemsSource = biz.GetSubToSuperRelationship(iSARelationship.ID);
                    btnUpdateSuperToSub.IsEnabled = true;
                    btnUpdateSubToSuper.IsEnabled = true;
                }
            }
        }

        private void btnUpdateSuperToSub_Click(object sender, RoutedEventArgs e)
        {
            BizISARelationship biz = new BizISARelationship();
            btnUpdateSuperToSub.IsEnabled = false;
            biz.UpdateSuperToSubRelationships(dtgSuperToSub.ItemsSource as List<SuperToSubRelationshipDTO>);
            btnUpdateSuperToSub.IsEnabled = true;
        }

        private void btnUpdateSubToSuper_Click(object sender, RoutedEventArgs e)
        {
            BizISARelationship biz = new BizISARelationship();
            btnUpdateSubToSuper.IsEnabled = false;
            biz.UpdateSubToSuperRelationships(dtgSubToSuper.ItemsSource as List<SubToSuperRelationshipDTO>);
            btnUpdateSubToSuper.IsEnabled = true;
        }



        private void btnUpdateUnionRelationship_Click(object sender, RoutedEventArgs e)
        {
            BizUnionRelationship biz = new BizUnionRelationship();
            btnUpdateUnionRelationship.IsEnabled = false;
            biz.UpdateUnionRelationships(dtgUnionRelationship.ItemsSource as List<UnionRelationshipDTO>);
            btnUpdateUnionRelationship.IsEnabled = true;
        }
        private void btnRefreshUnionRelationship_Click(object sender, RoutedEventArgs e)
        {
            btnUpdateUnionRelationship.IsEnabled = true;
            BizUnionRelationship biz = new BizUnionRelationship();
            dtgUnionRelationship.ItemsSource = biz.GetUnionRelationships(DatabaseID);
        }
        void dtgUnionRelationship_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (dtgUnionRelationship.SelectedItem != null)
            {
                BizUnionRelationship biz = new BizUnionRelationship();
                UnionRelationshipDTO UnionRelationship = dtgUnionRelationship.SelectedItem as UnionRelationshipDTO;
                if (UnionRelationship != null)
                {
                    dtgUnionToSubUnion.ItemsSource = biz.GetSuperUnionToSubUnionRelationship(UnionRelationship.ID);
                    dtgSubUnionToUnion.ItemsSource = biz.GetSubUnionToSuperUnionRelationship(UnionRelationship.ID);
                    btnUpdateSuperToSub.IsEnabled = true;
                    btnUpdateSubToSuper.IsEnabled = true;
                }
            }
        }

        private void btnUpdateSuperUnionToSubUnion_Click(object sender, RoutedEventArgs e)
        {
            BizUnionRelationship biz = new BizUnionRelationship();
            btnUpdateSuperUnionToSubUnion.IsEnabled = false;
            biz.UpdateSuperUnionToSubUnionRelationships(dtgSuperToSub.ItemsSource as List<SuperUnionToSubUnionRelationshipDTO>);
            btnUpdateSuperUnionToSubUnion.IsEnabled = true;
        }

        private void btnUpdateSubUnionToSuperUnion_Click(object sender, RoutedEventArgs e)
        {
            BizUnionRelationship biz = new BizUnionRelationship();
            btnUpdateSubUnionToSuperUnion.IsEnabled = false;
            biz.UpdateSubUnionToSuperUnionRelationships(dtgSubToSuper.ItemsSource as List<SubUnionToSuperUnionRelationshipDTO>);
            btnUpdateSubUnionToSuperUnion.IsEnabled = true;
        }
    }
}
