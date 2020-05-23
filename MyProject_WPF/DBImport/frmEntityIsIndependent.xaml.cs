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
using Telerik.Windows.Controls.GridView;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmImportTables.xaml
    /// </summary>
    public partial class frmEntityIsIndependent : UserControl, ImportWizardForm
    {
        BizRelationship bizRelationship = new BizRelationship();
        public List<RelationshipDTO> listAllEnabledRelationships { get; private set; }

        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        DatabaseDTO Database { set; get; }
        public List<TableDrivedEntityDTO> listAllEntitis { get; private set; }
        public event EventHandler DataUpdated;
        int DataCountLimit = 15;

        //کلا بروزرسانی مدل باید تست بشه مرحله به مرحله در فرمهای موجودیت و روابط دیسیبل اینیبل و دیتااینتری اینیبل بشن و تست شود
        public frmEntityIsIndependent(DatabaseDTO database)
        {
            InitializeComponent();
            Database = database;
            dtgTables.RowLoaded += DtgNewTables_RowLoaded;
            //    dtgNewTablesNonSuspicious.RowLoaded += DtgNewTables_RowLoaded;
            //dtgNewTables.CellEditEnded += DtgNewTables_CellEditEnded;
            if (!Database.DBHasData)
            {
                //colSearchInitiallySuspicious.IsVisible = false;
                //colSearchInitiallyNonSuspicious.IsVisible = false;
            }
            bizTableDrivedEntity.ItemImportingStarted += BizTableDrivedEntity_ItemImportingStarted;
            bizRelationship.ItemImportingStarted += BizTableDrivedEntity_ItemImportingStarted;
            dtgTables.CellEditEnded += DtgRelationship_CellEditEnded;
            //      dtgNewTablesSuspicious.CellLoaded += DtgRelationship_CellLoaded;

            // dtgNewTablesNonSuspicious.CellEditEnded += DtgRelationship_CellEditEnded;
            this.Loaded += FrmTableProperty_BaseTable_Loaded;
            // dtgNewTablesNonSuspicious.CellLoaded += DtgRelationship_CellLoaded;
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

        private void DtgRelationship_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            //e.Cell.
            if (e.Cell.Column == colIndependentDataEntryNonSuspicious)
            {
                if (e.Cell.DataContext is TableImportItem)
                {
                    var tableImportItem = e.Cell.DataContext as TableImportItem;

                    var cell = e.Cell.ParentRow.Cells.FirstOrDefault(x => x.Column.Name == "colRelationshipsSuspicious"
                    || x.Column.Name == "colRelationshipsNonSuspicious");
                    if (cell != null)
                        DecideCellEnabled(tableImportItem, cell);

                }
            }
            if (e.Cell.Column == colRelationshipsNonSuspicious)
            {
                if (e.Cell.DataContext is TableImportItem)
                {
                    var tableImportItem = e.Cell.DataContext as TableImportItem;
                    SetRelatedEntityNames(tableImportItem);
                }
            }
        }

        private void DecideCellEnabled(TableImportItem item, GridViewCellBase cell)
        {
            if (cell != null)
                cell.IsEnabled = item.Entity.IndependentDataEntry != true;
            if (!cell.IsEnabled)
                item.RelatedEntityNames = "";
            //if (item.Entity.IndependentDataEntry != true)
            //    if (item.Relationships.Count <= 1)
            //        cell.IsEnabled = false;
        }

        private void SetRelatedEntityNames(TableImportItem tableImportItem)
        {
            var relNames = "";
            foreach (var item in tableImportItem.Relationships.Where(x => x.Select))
            {
                relNames += (relNames == "" ? "" : ",") + item.Entity1;
            }
            tableImportItem.RelatedEntityNames = relNames;
        }

        private void DtgNewTables_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (e.DataElement is TableImportItem)
            {
                var tableImportItem = e.DataElement as TableImportItem;

                var tooltip = tableImportItem.Tooltip;
                if (!string.IsNullOrEmpty(tableImportItem.ValidationTooltip))
                {
                    tooltip += Environment.NewLine + tableImportItem.ValidationTooltip;
                    e.Row.Background = new SolidColorBrush(Colors.Pink);
                }
                else
                    e.Row.Background = new SolidColorBrush(Colors.White);
                if (!string.IsNullOrEmpty(tooltip))
                    ToolTipService.SetToolTip(e.Row, tooltip);

                if (Database.Name.ToLower().StartsWith("DBProductService".ToLower()))
                {
                    if(tableImportItem.Entity.Name== "GenericPersonAddress")
                    {
                        tableImportItem.Entity.IndependentDataEntry = false;
                        tableImportItem.Relationships.First(x => x.Entity1 == "GenericPerson").Select = true;
                    }
                    else if(tableImportItem.Entity.Name == "ProductItem")
                    {
                        tableImportItem.Entity.IndependentDataEntry = false;
                        tableImportItem.Relationships.First(x => x.Entity1 == "ServiceRequest").Select = true;
                    }
                    else if (tableImportItem.Entity.Name == "ServiceAdditionalItem")
                    {
                        tableImportItem.Entity.IndependentDataEntry = false;
                        tableImportItem.Relationships.First(x => x.Entity1 == "ServiceConclusionItem").Select = true;
                    }
                    else if (tableImportItem.Entity.Name == "RequestProductPart")
                    {
                        tableImportItem.Entity.IndependentDataEntry = false;
                        tableImportItem.Relationships.First(x => x.Entity1 == "ServiceItem").Select = true;
                    }
                    else if (tableImportItem.Entity.Name == "ServiceConclusionItem")
                    {
                        tableImportItem.Entity.IndependentDataEntry = false;
                        tableImportItem.Relationships.First(x => x.Entity1 == "ServiceConclusion").Select = true;
                    }
                    else if (tableImportItem.Entity.Name == "ServiceRequest_RequestType")
                    {
                        tableImportItem.Entity.IndependentDataEntry = false;
                        tableImportItem.Relationships.First(x => x.Entity1 == "ServiceRequest").Select = true;
                    }
                    else if (tableImportItem.Entity.Name == "ServiceRequestReviewItems")
                    {
                        tableImportItem.Entity.IndependentDataEntry = false;
                        tableImportItem.Relationships.First(x => x.Entity1 == "ServiceRequestReview").Select = true;
                    }
                }
                    //if (!string.IsNullOrEmpty(tableImportItem.Tooltip))
                    //    ToolTipService.SetToolTip(e.Row, tableImportItem.Tooltip);
                    var cell = e.Row.Cells.FirstOrDefault(x => x.Column.Name == "colRelationshipsSuspicious"
                 || x.Column.Name == "colRelationshipsNonSuspicious");
                if (cell != null)
                    DecideCellEnabled(tableImportItem, cell);
            }
        }

        public event EventHandler<ItemImportingStartedArg> InfoUpdated;
        public event EventHandler FormIsBusy;
        public event EventHandler FormIsFree;

        List<TableImportItem> finalList = new List<TableImportItem>();
        private void btnExtract_Click(object sender, RoutedEventArgs e)
        {
            SetImportedInfo();
        }
        private async void SetImportedInfo()
        {
            try
            {
                FormIsBusy(this, null);
                finalList = new List<TableImportItem>();
                //List<TableImportItem> listNewNonSuspicious = new List<TableImportItem>();
                if (bizTableDrivedEntity.ExistsEnabledEntitiesWithNullIndependentProperty(Database.ID))
                {
                    listAllEntitis = await GetOrginalEntities();
                    listAllEnabledRelationships = await GetEnabledRelationships();

                    foreach (var entity in listAllEntitis.Where(x => !x.IsDisabled && x.IndependentDataEntry == null))
                    {
                        var relationships = new List<RelationshipDTO>();
                        foreach (var rel in listAllEnabledRelationships.Where(x => x.EntityID2 == entity.ID))
                        {
                            //  var oneSideEntity = listAllEntitis.First(x => x.ID == rel.EntityID1);
                            //    if (oneSideEntity.IndependentDataEntry == true)
                            relationships.Add(rel);
                        }
                        var item = new TableImportItem(entity, false, "");
                        item.Relationships = relationships;
                        SetInfo(item);
                        finalList.Add(item);

                        //if (IsSuspicious(entity))
                        //{
                        //    item.Entity.IndependentDataEntry = true;
                        //    listNewSuspicious.Add(item);
                        //}
                        //else
                        //{
                        //    listNewNonSuspicious.Add(item);
                        //}
                    }



                    await DecideProperties(finalList.ToList());
                    //finalList = listNewSuspicious.Union(listNewNonSuspicious).OrderByDescending(x => x.Entity.IndependentDataEntry).ToList();
                    //foreach (var item in listNewNonSuspicious)
                    //{
                    //    if (item.Relationships.Count == 1)
                    //    {
                    //        item.Relationships.First().Select = true;
                    //        SetRelatedEntityNames(item);
                    //    }
                    //}
                }
                //if (listNewNonSuspicious.Any())
                //    tabNewTablesNonSuspicious.IsSelected = true;
                //else if (listNewSuspicious.Any())
                //    tabNewTablesSuspicious.IsSelected = true;
                dtgTables.ItemsSource = finalList.OrderByDescending(x => x.Entity.IndependentDataEntry).ToList();
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
                    lblMessage.Text = "موجودیتی به منظور تعیین  مستقل بودن و یا نبودن موجود نمی باشد";
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
        public Task<List<RelationshipDTO>> GetEnabledRelationships()
        {
            return Task.Run(() =>
            {
                var result = bizRelationship.GetEnabledRelationships(Database.ID);
                return result;
            });
        }
        //private void SetInitialSearch(TableDrivedEntityDTO entity)
        //{
        //    if (Database.DBHasData)
        //    {
        //        if (entity.IndependentDataEntry == true)
        //        {
        //            var dataHelper = new ModelDataHelper();
        //            var count = dataHelper.GetDataCount(entity);
        //            if (count != 0 && count <= DataCountLimit)
        //            {
        //                entity.SearchInitially = true;
        //            }
        //        }
        //    }
        //}
        //private bool IsSuspicious(TableDrivedEntityDTO entity)
        //{

        //    return false;
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
        private Task DecideProperties(List<TableImportItem> list)
        {
            return Task.Run(() =>
            {
                var listReviewed = new List<int>();
                foreach (var tableImportItem in list)
                {
                    var entity = tableImportItem.Entity;
                    if (InfoUpdated != null)
                        InfoUpdated(this, new ItemImportingStartedArg() { ItemName = "Determining Properties" + " " + entity.Name, TotalProgressCount = list.Count, CurrentProgress = list.IndexOf(tableImportItem) + 1 });

                    //var independentDataEntry = 
                    entity.IndependentDataEntry = IsIndependant(entity, listReviewed);

                    SelectRelationships(tableImportItem);
                    //  SetInitialSearch(entity);
                }
                foreach (var entity in list)
                {
                    //bool isAssociative = false;
                    //var notDataRef = new List<TableDrivedEntityDTO>();
                    //var manyToones = entity.Relationships.Where(x => x.TypeEnum == Enum_RelationshipType.ManyToOne
                    //&& x.IsOtherSideMandatory && x.EntityID1 != x.EntityID2);
                    //foreach (var item in manyToones)
                    //{
                    //    var entity2 = list.FirstOrDefault(x => x.ID == item.EntityID2);
                    //    if (entity2 == null)
                    //        entity2 = bizTableDrivedEntity.GetTableDrivedEntity(item.EntityID2, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithRelationships);

                    //    //if (entity2.IsDataReference)
                    //        notDataRef.Add(entity2);
                    //}
                    //if (notDataRef.Count > 1)
                    //{
                    //    if (entity.Relationships.Count < 4)
                    //    {
                    //        isAssociative = true;

                    //    }
                    //}
                    //entity.IsAssociative = isAssociative;
                    //entity.IndependentDataEntry = !isAssociative;
                }
            });
        }

        private void SelectRelationships(TableImportItem tableImportItem)
        {
            //foreach (var rel in tableImportItem.Relationships)
            //{
            //    if (rel.DataEntryEnabled)
            //        rel.Select = true;
            //}
            if (tableImportItem.Entity.IndependentDataEntry != true)
            {
                if (tableImportItem.Relationships.Count == 1)
                {
                    tableImportItem.Relationships.First().Select = true;
                }
                else if (tableImportItem.Relationships.Count > 1)
                {
                    foreach (var rel in tableImportItem.Relationships)
                    {
                        if (tableImportItem.Entity.Name.ToLower().StartsWith(rel.Entity1.ToLower()))
                        {
                            rel.Select = true;
                            //   break;
                        }

                    }
                    if (!tableImportItem.Relationships.Any(x => x.Select))
                    {
                        foreach (var rel in tableImportItem.Relationships)
                        {
                            if (rel.TypeEnum == Enum_RelationshipType.OneToMany && rel.RelationshipColumns.Any(y => y.SecondSideColumn.PrimaryKey))
                            {
                                rel.Select = true;
                                //   break;
                            }
                        }
                    }


                    //if (!tableImportItem.Relationships.Any(x => x.Select))
                    //{
                    //    List<Tuple<RelationshipDTO, int>> listRelEntityColumns = new List<Tuple<RelationshipDTO, int>>();
                    //    foreach (var rel in tableImportItem.Relationships)
                    //    {
                    //        var fEntity = listAllEntitis.First(x => x.ID == rel.EntityID1);
                    //        listRelEntityColumns.Add(new Tuple<RelationshipDTO, int>(rel, fEntity.Columns.Count));
                    //    }
                    //    var most = listRelEntityColumns.OrderByDescending(x => x.Item2).First().Item2;
                    //    foreach (var item in listRelEntityColumns.Where(x => x.Item2 == most))
                    //    {
                    //        item.Item1.Select = true;
                    //    }
                    //}
                }
                else
                {

                }
                SetRelatedEntityNames(tableImportItem);
            }

        }

        private bool? IsIndependant(TableDrivedEntityDTO entity, List<int> reviewedEntityIDs)
        {
            //if (reviewedOnes == null)
            //    reviewedOnes = new Dictionary<TableDrivedEntityDTO, bool?>();

            //خود این خصوصیت چی یعنی قبلن ست شده بوده باشهentity.IndependentDataEntry 




            bool? isIndependant = null;
            if (entity.IndependentDataEntry != null)
                isIndependant = entity.IndependentDataEntry.Value;
            else
            {
                if (reviewedEntityIDs.Any(x => x == entity.ID))
                    return null;
                reviewedEntityIDs.Add(entity.ID);

                //if (!entity.Relationships.Any())
                //    isIndependant = true;

                //if (entity.Columns.Count >= 10)
                //    isIndependant = true;
                if (entity.IsDataReference == true)
                    isIndependant = true;
                //var aa = false;
                //if (isIndependant == null)
                //{
                //    //تمرکز بروی چند به یک هاست چون روابط یک به ان غیر فعال شده اند و برای مستقیم بودن یا نبودنشون در تب دیگر کاربر باید تصمیم بگیرد
                //    if (entity.Relationships.Any(x => x.TypeEnum == Enum_RelationshipType.ManyToOne && !IsBaseTable(x.EntityID2) && x.IsOtherSideMandatory))
                //    {
                //    }
                //    else
                //    {
                //        if (entity.Columns.Count >= 6 || entity.Relationships.Any(x => x.TypeEnum != Enum_RelationshipType.ImplicitOneToOne && x.TypeEnum != Enum_RelationshipType.SubToSuper))
                //        {
                //        //    isIndependant = true;
                //            aa = true;
                //        }
                //    }
                //}
                var columns = entity.Columns.Where(x => string.IsNullOrEmpty(x.DefaultValue) && x.IsDBCalculatedColumn == false && x.IsIdentity == false && x.PrimaryKey == false && !entity.Relationships.Any(y => y.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary && y.RelationshipColumns.Any(z => z.FirstSideColumnID == x.ID)));
                if (columns.Count() >= 10)
                    isIndependant = true;
                if (!entity.Relationships.Any())
                    isIndependant = true;
                if (!entity.Relationships.Any(x => x.TypeEnum == Enum_RelationshipType.ManyToOne))
                    isIndependant = true;

                else if (isIndependant == null)
                {
                    foreach (var item in entity.Relationships.Where(x => x.TypeEnum == Enum_RelationshipType.ManyToOne && x.RelationshipColumns.Any(y => y.FirstSideColumn.PrimaryKey)))
                    {
                        if (!entity.Relationships.Any(x => x.ID != item.ID && x.TypeEnum == Enum_RelationshipType.ManyToOne && x.RelationshipColumns.Any(y => y.FirstSideColumn.PrimaryKey)))
                            isIndependant = false;
                    }
                    //if (entity.Relationships.Any(x => x.IsOtherSideMandatory && x.TypeEnum == Enum_RelationshipType.ManyToOne && !IsBaseTable(x.EntityID2)))
                    //{
                    //    if (entity.Relationships.Any(x => x.TypeEnum == Enum_RelationshipType.ManyToOne && x.RelationshipColumns.Any(y => y.FirstSideColumn.PrimaryKey)))
                    //    {
                    //        isIndependant = false;
                    //    }
                    //    else
                    //    {
                    ////اینجا عوض بشه.اولا روابط دونه دونه شمرده بشن. دوما تعداد رابطه برای هر رابطه مشخص بشه
                    //این شرط زیر بررسی بشه و اگه بشه حذف بشه
                    //    جدول workorder ,...
                    //bool oneToManyWithIndependent = false;

                    //foreach (var relationship in entity.Relationships.Where(x => x.TypeEnum == Enum_RelationshipType.OneToMany))
                    //{
                    //    TableDrivedEntityDTO sentity = listAllEntitis.First(x => x.ID == relationship.EntityID2);
                    //    var isOtherSideIndependent = IsIndependant(sentity, reviewedEntityIDs);
                    //    if (isOtherSideIndependent == null)
                    //        oneToManyWithIndependent = true;
                    //    else
                    //        oneToManyWithIndependent = isOtherSideIndependent.Value;
                    //}
                    //if (!oneToManyWithIndependent)
                    //{

                    //    //List<ManyToOneRelatiopshipProperty> manyToOneMandatoryRels = new List<ManyToOneRelatiopshipProperty>();
                    //    //foreach (var rel in entity.Relationships.Where(x => x.TypeEnum == Enum_RelationshipType.ManyToOne)) // x.IsOtherSideMandatory == true &&
                    //    //{
                    //    //    ManyToOneRelatiopshipProperty rItem = new ManyToOneRelatiopshipProperty();
                    //    //    rItem.IsOtherSideMandatory = rel.IsOtherSideMandatory;
                    //    //    rItem.OtherSideIsBaseTable = IsBaseTable(rel.EntityID2);
                    //    //    manyToOneMandatoryRels.Add(rItem);
                    //    //}

                    //    //if (manyToOneMandatoryRels.Count(x => x.IsOtherSideMandatory && !x.OtherSideIsBaseTable) > 0)
                    //    //{
                    //    //    isIndependant = false;
                    //    //}
                    //}
                    //}
                    //برای اون واسطهایی که خودشون با یک واسط دیگه یک به ان دارند. مثل دوسیرپارتی و طرفین دادخواست

                    //var rel = entity.Relationships.First(x => x.TypeEnum != Enum_RelationshipType.OneToMany && x.TypeEnum != Enum_RelationshipType.ManyToOne);
                    //isIndependant = true;


                }
                //else
                //{
                //    isIndependant = false;
                //}

                //else
                //{
                //    if (entity.Columns.Count < 6 && !entity.Relationships.Any(x => x.TypeEnum != Enum_RelationshipType.ImplicitOneToOne && x.TypeEnum != Enum_RelationshipType.SubToSuper))
                //        isIndependant = false;
                //}


                //reviewedOnes[reviewedOnes.First(x => x.Key.ID == entity.ID).Key] = isIndependant;



                //}
            }
            if (isIndependant == null)
                isIndependant = true;
            if (entity.IndependentDataEntry == null)
                entity.IndependentDataEntry = isIndependant;

            return isIndependant.Value;
            //if (entity.Columns.Count < 10)
            //{
            //    List<ManyToOneRelatiopshipProperty> manyToOneRels = new List<ManyToOneRelatiopshipProperty>();
            //    foreach (var rel in entity.Relationships.Where(x => x.TypeEnum == Enum_RelationshipType.ManyToOne))
            //    {
            //        ManyToOneRelatiopshipProperty rItem = new ManyToOneRelatiopshipProperty();
            //        rItem.IsOtherSideMandatory = rel.IsOtherSideMandatory;
            //        rItem.OtherSideIsBaseTable = IsBaseTable(rel.EntityID2);
            //        //       rItem.IsPrimaryKeys = rel.RelationshipColumns.Any(x => x.FirstSideColumn.PrimaryKey);
            //        manyToOneRels.Add(rItem);
            //    }

            //    if (manyToOneRels.Count == 1)
            //    {
            //        if (manyToOneRels.First().OtherSideIsBaseTable)
            //            independentDataEntry = true;
            //    }
            //    else
            //    {
            //        if (manyToOneRels.Count(x => x.IsOtherSideMandatory && !x.OtherSideIsBaseTable) >= 2)
            //        {

            //        }
            //        else
            //            independentDataEntry = true;
            //    }

            //}
            //else
            //    independentDataEntry = true;

            //return independentDataEntry;
        }

        private bool IsBaseTable(int entityID)
        {
            TableDrivedEntityDTO entity = listAllEntitis.First(x => x.ID == entityID);
            return entity.IsDataReference == true;
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
                //   var newEntities = listNewSuspicious.Union(listNewNonSuspicious).ToList();
                if (finalList.Any())
                {
                    foreach (var item in finalList)
                    {
                        item.IsValid = true;
                        item.ValidationTooltip = "";
                        var validationTooltip = "";
                        var relationships = listAllEnabledRelationships.Where(x => x.EntityID2 == item.Entity.ID);
                        if (item.Entity.IndependentDataEntry == false)
                        {
                            bool isValid = false;

                            if (relationships.Any(x => !x.IsDisabled && x.DataEntryEnabled))
                            {
                                isValid = true;
                            }
                            if (!isValid)
                            {
                                if (item.Relationships.Any(x => x.Select))
                                    isValid = true;
                            }
                            if (!isValid)
                                validationTooltip += (validationTooltip == "" ? "" : Environment.NewLine) + "موجودیت امکان ورود اطلاعات مستقیم و یا از طریق رابطه را ندارد";
                            item.ValidationTooltip = validationTooltip;
                            item.IsValid = string.IsNullOrEmpty(validationTooltip);
                        }
                    }
                    dtgTables.ItemsSource = null;
                    dtgTables.ItemsSource = finalList.OrderByDescending(x => x.Entity.IndependentDataEntry).ToList(); ;
                    //dtgNewTablesNonSuspicious.ItemsSource = null;
                    //dtgNewTablesNonSuspicious.ItemsSource = listNewNonSuspicious;

                    if (finalList.All(x => x.IsValid))
                    {
                        //foreach (var item in finalList)
                        //{
                        //    if (item.Entity.IndependentDataEntry == true)
                        //        SetInitialSearch(item.Entity);
                        //}
                        await UpdateModel(Database.ID, finalList);
                        if (DataUpdated != null)
                            DataUpdated(this, null);
                        MessageBox.Show("انتقال اطلاعات انجام شد");
                        updated = true;
                    }
                    else
                    {
                        MessageBox.Show("برخی از موجودیت ها نیاز به تعیین تکلیف دارند که با رنگ قرمز مشخص شده اند");

                    }
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
        private Task UpdateModel(int databaseID, List<TableImportItem> newEntities)
        {
            return Task.Run(() =>
            {
                bizTableDrivedEntity.UpdateTableIndependentDataEntryProperty(databaseID, newEntities);
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

    public class ManyToOneRelatiopshipProperty
    {
        public bool IsOtherSideMandatory { set; get; }
        public bool OtherSideIsBaseTable { set; get; }
        //public bool IsPrimaryKeys { set; get; }

    }
}
