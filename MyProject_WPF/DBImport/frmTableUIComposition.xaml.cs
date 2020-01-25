using ModelEntites;
using MyModelGenerator;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmImportTables.xaml
    /// </summary>
    public partial class frmTableUIComposition : UserControl, ImportWizardForm
    {
        BizEntityUIComposition bizEntityUIComposition = new BizEntityUIComposition();

           BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        DatabaseDTO Database { set; get; }
        public frmTableUIComposition(DatabaseDTO database)
        {
            InitializeComponent();
            Database = database;
            dtgNewTables.RowLoaded += DtgNewTables_RowLoaded;
           
            //dtgNewTables.CellEditEnded += DtgNewTables_CellEditEnded;
            //if (!Database.DBHasData)
            //    colIsDataReference.IsVisible = false;
        }



        private void DtgNewTables_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (e.DataElement is TableImportItem)
            {
                var item = e.DataElement as TableImportItem;
                if (!string.IsNullOrEmpty(item.Tooltip))
                    ToolTipService.SetToolTip(e.Row, item.Tooltip);
            }
        }

        public event EventHandler<ItemImportingStartedArg> InfoUpdated;
        List<TableImportItem> listNew = new List<TableImportItem>();
        private void btnExtract_Click(object sender, RoutedEventArgs e)
        {
            SetImportedInfo();
        }
        //public List<TableDrivedEntityDTO> listAllEntitis { get; private set; }
        private async void SetImportedInfo()
        {
            listNew = new List<TableImportItem>();
            var listEntity = bizTableDrivedEntity.GetOrginalEntitiesWithoutUIComposition(Database.ID,EntityColumnInfoType.WithFullColumns,EntityRelationshipInfoType.WithRelationships);

            foreach (var entity in listEntity)
            {
                var item = new TableImportItem(entity, false, "");
                SetInfo(item);
                listNew.Add(item);

            }
            await DecideProperties(listNew);


            dtgNewTables.ItemsSource = listNew;
         
            btnInsert.IsEnabled = listNew.Any();
        }



        private void SetInfo(TableImportItem item)
        {
            var tooltip = "Columns :";
            foreach (var column in item.Entity.Columns)
            {
                tooltip += Environment.NewLine + column.Name + (column.PrimaryKey ? " (PK) " : "") + " : " + column.DataType + (column.IsNull ? " " + "(Nullable)" : "");
            }
            tooltip += Environment.NewLine + "Relationship :";
            foreach (var relationship in item.Entity.Relationships)
            {
                tooltip += Environment.NewLine + relationship.Entity2 + " : " + relationship.TypeEnum.ToString();
            }
            item.Tooltip += (string.IsNullOrEmpty(item.Tooltip) ? "" : Environment.NewLine) + tooltip;
        }
        //private async void DecideProperties(List<TableDrivedEntityDTO> list)
        //{
        //    await DecideProperties1(list);
        //}
        private Task DecideProperties(List<TableImportItem> list)
        {
            return Task.Run(() =>
            {
                foreach (var entity in list)
                {
                    if (InfoUpdated != null)
                        InfoUpdated(this, new ItemImportingStartedArg() { ItemName = "Generating UI" + " " + entity.Entity.Name, TotalProgressCount = list.Count, CurrentProgress = list.IndexOf(entity) });

                    SetUIComposition(entity);
                }
            });
        }



        private void SetUIComposition(TableImportItem item)
        {
            item.UIComposition = bizEntityUIComposition.GenerateUIComposition(item.Entity);
         }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listNew.Any())
                {
                    bizEntityUIComposition.UpdateUICompositionsInModel(listNew);
                    MessageBox.Show("انتقال اطلاعات انجام شد");
                    SetImportedInfo();
                }
                else
                {
                    MessageBox.Show("موردی جهت انتقال انتخاب نشده است");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("انتقال اطلاعات انجام نشد" + Environment.NewLine + ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //private Task<List<TableDrivedEntityDTO>> GenerateDefaultEntitiesAndColumns()
        //{
        //    return Task.Run(() =>
        //    {
        //        var result = ImportHelper.GenerateDefaultEntitiesAndColumns();
        //        return result;
        //    });
        //}
    }


}
