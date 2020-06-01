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
    public partial class frmTableProperty_Relation : UserControl, ImportWizardForm
    {
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        BizRelationship bizRelationship = new BizRelationship();
        DatabaseDTO Database { set; get; }
        public frmTableProperty_Relation(DatabaseDTO database)
        {
            InitializeComponent();
            Database = database;
            dtgRelationship.RowLoaded += DtgNewTables_RowLoaded;

            //if (!Database.DBHasData)
            //    colSearchInitially.IsVisible = false;
        }



        private void DtgNewTables_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (e.DataElement is RelationshipImportItem)
            {
                var item = e.DataElement as RelationshipImportItem;
                if (!string.IsNullOrEmpty(item.Tooltip))
                    ToolTipService.SetToolTip(e.Row, item.Tooltip);
                //var cellDataEntryEnabled = e.Row.Cells.FirstOrDefault(x => x.Column.Name == "colDataEntryEnabled");
                //if (cellDataEntryEnabled != null)
                //{
                //    if (item.Relationship.TypeEnum == Enum_RelationshipType.OneToMany
                //    || item.Relationship.TypeEnum == Enum_RelationshipType.ImplicitOneToOne)
                //    {
                //        cellDataEntryEnabled.IsEnabled = true;
                //    }
                //    else
                //        cellDataEntryEnabled.IsEnabled = false;
                //}
                //var cellIsForeignSideMandatory = e.Row.Cells.FirstOrDefault(x => x.Column.Name == "colIsForeignSideMandatory");
                //if (cellIsForeignSideMandatory != null)
                //{
                //    if (item.Relationship.TypeEnum == Enum_RelationshipType.OneToMany
                //    || item.Relationship.TypeEnum == Enum_RelationshipType.ImplicitOneToOne)
                //    {
                //        cellIsForeignSideMandatory.IsEnabled = true;
                //    }
                //    else
                //        cellIsForeignSideMandatory.IsEnabled = false;
                //}
                //var cellIsPrimarySideMandatory = e.Row.Cells.FirstOrDefault(x => x.Column.Name == "colIsPrimarySideMandatory");
                //if (cellIsPrimarySideMandatory != null)
                //{
                //    if (item.Relationship.TypeEnum == Enum_RelationshipType.OneToMany
                //    || item.Relationship.TypeEnum == Enum_RelationshipType.ImplicitOneToOne)
                //    {
                //        cellIsPrimarySideMandatory.IsEnabled = true;
                //    }
                //    else
                //        cellIsPrimarySideMandatory.IsEnabled = false;
                //}

            }
        }

        public event EventHandler<ItemImportingStartedArg> InfoUpdated;
        List<RelationshipImportItem> listNew = new List<RelationshipImportItem>();
        private void btnExtract_Click(object sender, RoutedEventArgs e)
        {
            SetImportedInfo();
        }
        private async void SetImportedInfo()
        {
            listNew = bizRelationship.GetEnabledNotReviewedRelationships(Database.ID);
            foreach (var item in listNew.Where(x => x.Exception == false))
                SetInfo(item);

            await DecideProperties(listNew.Where(x => x.Exception == false).Select(x => x.Relationship).ToList());
            dtgRelationship.ItemsSource = listNew;
            btnInsert.IsEnabled = listNew.Any();


        }

        private void SetInfo(RelationshipImportItem item)
        {
            var tooltip = "";
            foreach (var column in item.Relationship.RelationshipColumns)
            {
                tooltip += Environment.NewLine + column.FirstSideColumn.Name + " = " + column.SecondSideColumn.Name + (column.SecondSideColumn.IsNull ? " " + "(Nullable)" : "");
            }
            item.Tooltip += (string.IsNullOrEmpty(item.Tooltip) ? "" : Environment.NewLine) + tooltip;
        }
        //private async void DecideProperties(List<TableDrivedEntityDTO> list)
        //{
        //    await DecideProperties1(list);
        //}
        private Task DecideProperties(List<RelationshipDTO> list)
        {
            return Task.Run(() =>
            {
                var entities = bizTableDrivedEntity.GetOrginalEntities(Database.ID, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships);
                var disabledOneToManyRels = new List<RelationshipDTO>();
                foreach (var relationship in list)
                {
                    if (InfoUpdated != null)
                        InfoUpdated(this, new ItemImportingStartedArg() { ItemName = "Determining Properties" + " " + relationship.Name, TotalProgressCount = list.Count, CurrentProgress = list.IndexOf(relationship) });
                    var entity1 = entities.First(x => x.ID == relationship.EntityID1);
                    var entity2 = entities.First(x => x.ID == relationship.EntityID2);
                    if (entity2.IndependentDataEntry == false)
                    {
                        if (relationship.TypeEnum == Enum_RelationshipType.OneToMany && relationship.DataEntryEnabled == false)
                        {
                            disabledOneToManyRels.Add(relationship);
                            //relationship.DataEntryEnabled = true;
                        }

                    }
                    //if (!entity1.IndependentDataEntry)
                    //{
                    //    //    relationship.IsPrimarySideDirectlyCreatable = true;
                    //}
                }
                foreach (var item in disabledOneToManyRels.GroupBy(x => x.EntityID2))
                {
                    var entity = entities.First(x => x.ID == item.Key);
                    if (item.Count() == 1)
                    {
                        item.First().DataEntryEnabled = true;
                    }
                    else
                    {

                    }
                }
            });
        }

        //private void SetNewEntitiesProperties(List<RelationshipImportItem> listNew)
        //{
        //   foreach(var item in listNew)
        //    {
        //        item.Entity.PropertyChanged += Entity_PropertyChanged;
        //        DetermineEntityIsDataRef(item);
        //    }
        //}

        //private void DetermineEntityIsDataRef(RelationshipImportItem item)
        //{

        //}

        //private void Entity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //   // throw new NotImplementedException();
        //}

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in listNew)
                {
                    var validationTooltip = "";
                    if (!item.Relationship.FKCoumnIsNullable && item.Relationship.IsPrimarySideMandatory == false)
                    {
                        validationTooltip += (validationTooltip == "" ? "" : Environment.NewLine) + "ستونهای رابطه مقدار خالی قبول نمی کنند، بنابراین رابطه باید اجباری باشد";
                    }
                    item.ValidationTooltip = validationTooltip;
                    item.IsValid = string.IsNullOrEmpty(validationTooltip);
                }
                dtgRelationship.ItemsSource = null;
                dtgRelationship.ItemsSource = listNew;
                if (listNew.Any() && listNew.All(x => x.IsValid))
                {
                    var newEntities = listNew.Select(x => x.Relationship).ToList();
                    bizRelationship.UpdateRelationshipProperties(Database.ID, newEntities);
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
