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
    public partial class frmEntityIsDataReference : UserControl, ImportWizardForm
    {
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        DatabaseDTO Database { set; get; }
        public frmEntityIsDataReference(DatabaseDTO database)
        {
            InitializeComponent();
            Database = database;
            dtgTables.RowLoaded += DtgNewTables_RowLoaded;
            //dtgNewTablesNonSuspicious.RowLoaded += DtgNewTables_RowLoaded;
            bizTableDrivedEntity.ItemImportingStarted += BizTableDrivedEntity_ItemImportingStarted;

            //dtgNewTables.CellEditEnded += DtgNewTables_CellEditEnded;
            //if (!Database.DBHasData)
            //    colIsDataReference.IsVisible = false;
            this.Loaded += FrmTableProperty_BaseTable_Loaded;
        }

        private void BizTableDrivedEntity_ItemImportingStarted(object sender, ItemImportingStartedArg e)
        {
            if (InfoUpdated != null)
                InfoUpdated(this, e);
        }

        private void FrmTableProperty_BaseTable_Loaded(object sender, RoutedEventArgs e)
        {
            SetImportedInfo();
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
        public event EventHandler FormIsBusy;
        public event EventHandler FormIsFree;
        private void btnExtract_Click(object sender, RoutedEventArgs e)
        {
            SetImportedInfo();
        }
        List<TableImportItem> finalList = new List<TableImportItem>();
        public List<TableDrivedEntityDTO> listAllEntitis { get; private set; }
        private async void SetImportedInfo()
        {
            try
            {
                FormIsBusy(this, null);
                finalList = new List<TableImportItem>();
                //  listNewNonSuspicious = new List<TableImportItem>();
                if (bizTableDrivedEntity.ExistsEnabledEntitiesWithNullDataReference(Database.ID))
                {
                    listAllEntitis = await GetOrginalEntities();
                    //List<TableImportItem> listNewSuspicious = new List<TableImportItem>();
                    //List<TableImportItem> listNewNonSuspicious = new List<TableImportItem>();
                    foreach (var entity in listAllEntitis.Where(x => !x.IsDisabled && x.IsDataReference == null))
                    {
                        var item = new TableImportItem(entity, false, "");
                        SetInfo(item);
                        finalList.Add(item);
                        //if (IsSuspicious(entity))
                        //    listNewSuspicious.Add(item);
                        //else
                        //{
                        //    item.Entity.IsDataReference = false;
                        //    listNewNonSuspicious.Add(item);
                        //}
                    }
                    await DecideProperties(finalList.Select(x => x.Entity).ToList());
                    //finalList = listNewSuspicious.Union(listNewNonSuspicious).OrderByDescending(x => x.Entity.IsDataReference).ToList();

                }
                //if (listNewSuspicious.Any())
                //    tabNewTablesSuspicious.IsSelected = true;
                //else if (listNewNonSuspicious.Any())
                //    tabNewTablesNonSuspicious.IsSelected = true;
                dtgTables.ItemsSource = finalList.OrderByDescending(x => x.Entity.IsDataReference).ToList();
                //dtgNewTablesNonSuspicious.ItemsSource = listNewNonSuspicious;
                btnInsert.IsEnabled = finalList.Any();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در پردازش اطلاعات" + Environment.NewLine + ex.Message);
            }
            finally
            {
                if (finalList.Any())
                {
                    FormIsFree(this, null);
                    lblMessage.Visibility = Visibility.Collapsed;
                }
                else
                {
                    lblMessage.Text = "موجودیتی برای تعیین وضعیت جدول پایه موجود نمی باشد";
                    lblMessage.Visibility = Visibility.Visible;
                }
            }

        }
        public Task<List<TableDrivedEntityDTO>> GetOrginalEntities()
        {
            return Task.Run(() =>
            {
                var result = bizTableDrivedEntity.GetOrginalEntities(Database.ID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithRelationships, false);
                return result;
            });
        }
        //private bool IsSuspicious(TableDrivedEntityDTO entity)
        //{
        //    //var columns = entity.Columns.Where(x => x.IsDBCalculatedColumn == false && x.IsIdentity == false && x.PrimaryKey == false && !entity.Relationships.Any(y => y.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary && y.RelationshipColumns.Any(z => z.FirstSideColumnID == x.ID)));

        //    return true;
        //}

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
        private Task DecideProperties(List<TableDrivedEntityDTO> list)
        {
            return Task.Run(() =>
            {
                var listReviewed = new List<int>();
                List<int> commonOrgIds = GetCommonOrgIds();
                foreach (var entity in list)
                {
                    if (InfoUpdated != null)
                        InfoUpdated(this, new ItemImportingStartedArg() { ItemName = "Determining" + " " + entity.Name + " " + " is base table", TotalProgressCount = list.Count, CurrentProgress = list.IndexOf(entity) + 1 });

                    SetDataReference(entity, commonOrgIds, listReviewed);
                }
            });
        }


        private List<int> GetCommonOrgIds()
        {
            List<int> result = new List<int>();
            List<Tuple<int, string, string>> commonManyToOneEntityID = new List<Tuple<int, string, string>>();

            foreach (var entity in listAllEntitis)
            {
                if (IsDataReferenceBaseCondition(entity))
                {
                    if (entity.Relationships.Count(x => x.TypeEnum == Enum_RelationshipType.ManyToOne) == 1)
                    {
                        var mtoRel = entity.Relationships.First(x => x.TypeEnum == Enum_RelationshipType.ManyToOne);
                        var oneEntity = listAllEntitis.First(x => x.ID == mtoRel.EntityID2);
                        commonManyToOneEntityID.Add(new Tuple<int, string, string>(mtoRel.EntityID2, mtoRel.Entity2, entity.Name));
                    }
                }
            }
            foreach (var group in commonManyToOneEntityID.GroupBy(x => x.Item1).Where(x => x.Count() >= 3))
            {
                result.Add(group.Key);
            }
            return result;
        }

        private bool? SetDataReference(TableDrivedEntityDTO entity, List<int> commonManyToOneEntityIDs, List<int> reviewedEntityIDs)
        {
            bool isDataReference = false;
            if (entity.IsDataReference != null)
                isDataReference = entity.IsDataReference.Value;
            else
            {
                if (reviewedEntityIDs.Any(x => x == entity.ID))
                    return null;
                reviewedEntityIDs.Add(entity.ID);
                if (IsDataReferenceBaseCondition(entity))
                {
                    if (!entity.Relationships.Any(x => x.TypeEnum == Enum_RelationshipType.ManyToOne))
                        isDataReference = true;
                    else
                    {
                        if (entity.Relationships.Count(x => x.TypeEnum == Enum_RelationshipType.ManyToOne) == 1)
                        {
                            var mtoRel = entity.Relationships.First(x => x.TypeEnum == Enum_RelationshipType.ManyToOne);
                            var oneEntity = listAllEntitis.First(x => x.ID == mtoRel.EntityID2);

                            if (commonManyToOneEntityIDs.Contains(mtoRel.EntityID2))
                                isDataReference = true;
                            else
                            {
                                var otherSideIsDataReference = SetDataReference(oneEntity, commonManyToOneEntityIDs, reviewedEntityIDs);
                                if (otherSideIsDataReference == null)
                                    isDataReference = true;
                                else
                                    isDataReference = otherSideIsDataReference.Value;
                            }
                        }
                    }
                }
            }
            if (entity.IsDataReference == null)
                entity.IsDataReference = isDataReference;
            return isDataReference;
        }

        private bool IsDataReferenceBaseCondition(TableDrivedEntityDTO entity)
        {
            if (entity.IsDataReference != null)
                return entity.IsDataReference.Value;

            if (entity.Columns.Count > 10)
                return false;
            if (!entity.Relationships.Any())
                return false;
            if (entity.Relationships.Count(x => x.TypeEnum == Enum_RelationshipType.SubToSuper ||
             x.TypeEnum == Enum_RelationshipType.SuperToSub || x.TypeEnum == Enum_RelationshipType.ExplicitOneToOne ||
             x.TypeEnum == Enum_RelationshipType.ImplicitOneToOne || x.TypeEnum == Enum_RelationshipType.UnionToSubUnion ||
             x.TypeEnum == Enum_RelationshipType.SubUnionToUnion || x.TypeEnum == Enum_RelationshipType.ManyToOne) >= 2)
                return false;
            if (entity.Relationships.Any(x => x.TypeEnum == Enum_RelationshipType.OneToMany))
            {
                var columns = entity.Columns.Where(x => string.IsNullOrEmpty(x.DefaultValue) && x.IsDBCalculatedColumn == false && x.IsIdentity == false && x.PrimaryKey == false && !entity.Relationships.Any(y => y.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary && y.RelationshipColumns.Any(z => z.FirstSideColumnID == x.ID)));
                if (columns.Count() >= 0 && columns.Count() <= 4)
                {
                    if (!entity.Relationships.Any(x => x.TypeEnum == Enum_RelationshipType.SubToSuper)
                    && !entity.Relationships.Any(x => x.TypeEnum == Enum_RelationshipType.SuperToSub)
                     && !entity.Relationships.Any(x => x.TypeEnum == Enum_RelationshipType.ExplicitOneToOne)
                      && !entity.Relationships.Any(x => x.TypeEnum == Enum_RelationshipType.ImplicitOneToOne)
                       && !entity.Relationships.Any(x => x.TypeEnum == Enum_RelationshipType.UnionToSubUnion)
                           && !entity.Relationships.Any(x => x.TypeEnum == Enum_RelationshipType.SubUnionToUnion))
                    {
                        return true;
                    }
                }
            }
            return false;
        }




        //private void SetNewEntitiesProperties(List<TableImportItem> listNew)
        //{
        //   foreach(var item in listNew)
        //    {
        //        item.Entity.PropertyChanged += Entity_PropertyChanged;
        //        DetermineEntityIsDataRef(item);
        //    }
        //}

        //private void DetermineEntityIsDataRef(TableImportItem item)
        //{

        //}

        //private void Entity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //   // throw new NotImplementedException();
        //}

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            UpdateModel();
        }
        async private void UpdateModel()
        {
            bool updated = false;
            try
            {
                FormIsBusy(this, null);
                var newEntities = finalList.Select(x => x.Entity).ToList();
                if (newEntities.Any())
                {
                    await UpdateModel(Database.ID, newEntities);
                    MessageBox.Show("انتقال اطلاعات انجام شد");
                    updated = true;
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
            finally
            {
                FormIsFree(this, null);
                if (updated)
                    SetImportedInfo();
            }
        }
        private Task UpdateModel(int databaseID, List<TableDrivedEntityDTO> newEntities)
        {
            return Task.Run(() =>
            {
                bizTableDrivedEntity.UpdateTablesIsDataReferenceProperty(databaseID, newEntities);
            });
        }

        //public bool HasData()
        //{

        //}
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
