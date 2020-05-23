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
    /// Interaction logic for frmImportRelationships.xaml
    /// </summary>
    public partial class frmImportRelationships : UserControl, ImportWizardForm
    {
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();

        BizRelationship bizRelationship = new BizRelationship();
        DatabaseDTO Database { set; get; }
        public frmImportRelationships(DatabaseDTO database)
        {
            InitializeComponent();
            Database = database;
            ImportHelper = ModelGenerator.GetDatabaseImportHelper(Database);
            ImportHelper.ItemImportingStarted += ImportHelper_ItemImportingStarted;
            bizTableDrivedEntity.ItemImportingStarted += ImportHelper_ItemImportingStarted;
            bizRelationship.ItemImportingStarted += ImportHelper_ItemImportingStarted;
            dtgNewRelationships.RowLoaded += DtgNewRelationships_RowLoaded;
            dtgExistingRelationships.RowLoaded += DtgNewRelationships_RowLoaded;
            dtgDeletedRelationships.RowLoaded += DtgNewRelationships_RowLoaded;
            dtgExceptionRelationships.RowLoaded += DtgNewRelationships_RowLoaded;

            colRelationshipType.ItemsSource = Enum.GetValues(typeof(Enum_OrginalRelationshipType)).Cast<Enum_OrginalRelationshipType>();
            dtgNewRelationships.CellEditEnded += DtgNewRelationships_CellEditEnded;
            this.Loaded += FrmImportRelationships_Loaded;
        }

        private void FrmImportRelationships_Loaded(object sender, RoutedEventArgs e)
        {
            SetImportedInfo();
        }

        private void DtgNewRelationships_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            if (e.Cell.DataContext is RelationshipImportItem)
            {
                if (e.Cell.Column.Name == "colRelationshipType")
                {
                    var data = (e.Cell.DataContext as RelationshipImportItem);
                    if (data.Relationship.OrginalTypeEnum == Enum_OrginalRelationshipType.SuperToSub)
                    {
                        if (string.IsNullOrEmpty(data.Relationship.OrginalRelationshipGroup))
                        {
                            string name = "";
                            if (data.Relationship.FKSidePKColumnsAreFkColumns)
                            {
                                var eRel = listNew.FirstOrDefault(x => !string.IsNullOrEmpty(x.Relationship.OrginalRelationshipGroup) && x.Relationship.FKSidePKColumnsAreFkColumns && x.Relationship.Entity1 == data.Relationship.Entity1 && x.Relationship.OrginalTypeEnum == Enum_OrginalRelationshipType.SuperToSub);
                                if (eRel == null)
                                    name = "ISA_OnPK_" + data.Relationship.Entity1;
                                else
                                    name = eRel.Relationship.OrginalRelationshipGroup;
                            }
                            else
                            {
                                var eRel = listNew.FirstOrDefault(x => !string.IsNullOrEmpty(x.Relationship.OrginalRelationshipGroup) && !x.Relationship.FKSidePKColumnsAreFkColumns && x.Relationship.Entity1 == data.Relationship.Entity1 && x.Relationship.OrginalTypeEnum == Enum_OrginalRelationshipType.SuperToSub);
                                if (eRel == null)
                                    name = "ISA_" + data.Relationship.Entity1;
                                else
                                    name = eRel.Relationship.OrginalRelationshipGroup;
                            }
                            data.Relationship.OrginalRelationshipGroup = name;
                        }
                    }
                    if (data.Relationship.OrginalTypeEnum == Enum_OrginalRelationshipType.SubUnionToUnion)
                    {
                        if (string.IsNullOrEmpty(data.Relationship.OrginalRelationshipGroup))
                        {
                            string name = "";

                            var eRel = listNew.FirstOrDefault(x => !string.IsNullOrEmpty(x.Relationship.OrginalRelationshipGroup) && x.Relationship.Entity2 == data.Relationship.Entity2 && x.Relationship.OrginalTypeEnum == Enum_OrginalRelationshipType.SubUnionToUnion);
                            if (eRel == null)
                                name = "Union_" + data.Relationship.Entity2;
                            else
                                name = eRel.Relationship.OrginalRelationshipGroup;

                            data.Relationship.OrginalRelationshipGroup = name;

                        }
                    }
                }
            }
        }

        private void DtgNewRelationships_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (e.DataElement is RelationshipImportItem)
            {
                var item = e.DataElement as RelationshipImportItem;
                var tooltip = item.Tooltip;
                if (!string.IsNullOrEmpty(item.ValidationTooltip))
                {
                    tooltip += Environment.NewLine + item.ValidationTooltip;
                    e.Row.Background = new SolidColorBrush(Colors.Pink);
                }
                else
                    e.Row.Background = new SolidColorBrush(Colors.White);
                if (!string.IsNullOrEmpty(tooltip))
                    ToolTipService.SetToolTip(e.Row, tooltip);

                if(Database.Name.ToLower().StartsWith("DBProductService".ToLower()))
                {
                    if (item.Relationship.Entity1 == "GenericPerson" && item.Relationship.Entity2 == "Customer")
                        item.Relationship.OrginalRelationshipGroup = "GenericPerson_Customer";

                    if (item.Relationship.Entity1 == "ServiceItem" && item.Relationship.Entity2 == "ServiceConclusionItem")
                    {
                        item.Relationship.OrginalTypeEnum = Enum_OrginalRelationshipType.SubUnionToUnion;
                        item.Relationship.OrginalRelationshipGroup = "Union_ServiceConclusionItem";
                    }
                    if (item.Relationship.Entity1 == "ServiceAdditionalItem" && item.Relationship.Entity2 == "ServiceConclusionItem")
                    {
                        item.Relationship.OrginalTypeEnum = Enum_OrginalRelationshipType.SubUnionToUnion;
                        item.Relationship.OrginalRelationshipGroup = "Union_ServiceConclusionItem";
                    }

                    if (item.Relationship.Entity1 == "ServiceRequest" && item.Relationship.Entity2 == "ServiceRequestReview")
                        item.Relationship.OrginalTypeEnum = Enum_OrginalRelationshipType.OneToOne;
                    if (item.Relationship.Entity1 == "ServiceRequest" && item.Relationship.Entity2 == "ServiceConclusion")
                        item.Relationship.OrginalTypeEnum = Enum_OrginalRelationshipType.OneToOne;
                }
            }
        }

        I_DatabaseImportHelper ImportHelper { set; get; }
        public event EventHandler<ItemImportingStartedArg> InfoUpdated;
        public event EventHandler FormIsBusy;
        public event EventHandler FormIsFree;

        List<RelationshipImportItem> listNew = new List<RelationshipImportItem>();
        List<RelationshipImportItem> listDeleted = new List<RelationshipImportItem>();
        List<RelationshipImportItem> listExisting = new List<RelationshipImportItem>();
        List<RelationshipImportItem> listException = new List<RelationshipImportItem>();
        private void btnExtract_Click(object sender, RoutedEventArgs e)
        {
            SetImportedInfo();
        }
        private async void SetImportedInfo()
        {
            try
            {
                FormIsBusy(this, null);
                var result = await GetRelationshipsInfo();
                listNew = new List<RelationshipImportItem>();
                listDeleted = new List<RelationshipImportItem>();
                listExisting = new List<RelationshipImportItem>();
                listException = new List<RelationshipImportItem>();
                var originalRelationships = await GetOrginalRelationships();
                var originalEntities = await GetOrginalEntities();
                foreach (var item in result.Where(x => x.Exception == false))
                {
                    var existingRelationship = OrginalRelationshipExists(item.Relationship, originalRelationships);
                    if (existingRelationship != null)
                    {
                        item.Relationship = existingRelationship;
                        listExisting.Add(item);
                    }
                    else
                    {
                        var res = OrginalRelationshipEntitiesExists(item.Relationship, originalEntities);
                        if (string.IsNullOrEmpty(res))
                        {

                            item.Selected = true;
                            listNew.Add(item);
                        }
                        else
                            listException.Add(new RelationshipImportItem(item.Relationship, true, res));
                    }
                }

                foreach (var item in result.Where(x => x.Exception == true))
                {
                    listException.Add(item);
                }
                //بعدا دقت شود که با غیر فعال کردن یک رابطه و حتی کلید خارجی کل رابطه در هر دو طرف غیرفعال فرض شود
                //var existingRelationships = bizRelationship.GetEnabledOrginalRelationships(Database.ID);
                var listImportedRelationship = result.Select(x => x.Relationship).ToList();
                foreach (var item in originalRelationships)
                {
                    var existingRelationship = OrginalRelationshipExists(item, listImportedRelationship);
                    if (existingRelationship == null)
                        listDeleted.Add(new RelationshipImportItem(item, ""));
                }

                await DetermineRelationshipType(listNew, originalEntities);
                SetRelationshipAlias(listNew, originalEntities);

                dtgNewRelationships.ItemsSource = listNew;
                dtgExistingRelationships.ItemsSource = listExisting;
                dtgExceptionRelationships.ItemsSource = listException;
                dtgDeletedRelationships.ItemsSource = listDeleted;

                if (listException.Any())
                    tabExceptionRelationships.Visibility = Visibility.Visible;
                else
                    tabExceptionRelationships.Visibility = Visibility.Collapsed;

                tabNewRelationships.Foreground = listNew.Any() ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
                tabDeletedRelationships.Foreground = listDeleted.Any() ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
                tabExceptionRelationships.Foreground = listException.Any() ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
                tabExistingRelationships.Foreground = listExisting.Any() ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);

                if (listNew.Any())
                    tabNewRelationships.IsSelected = true;
                else if (listDeleted.Any())
                    tabDeletedRelationships.IsSelected = true;
                else if (listException.Any())
                    tabExceptionRelationships.IsSelected = true;
                else if (listExisting.Any())
                    tabExistingRelationships.IsSelected = true;

                btnInsert.IsEnabled = listNew.Any() || listDeleted.Any();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در پردازش اطلاعات" + Environment.NewLine + ex.Message);
            }
            finally
            {
                FormIsFree(this, null);
            }
        }
        public Task<List<TableDrivedEntityDTO>> GetOrginalEntities()
        {
            return Task.Run(() =>
            {
                var result = bizTableDrivedEntity.GetOrginalEntities(Database.ID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships,false);
                return result;
            });
        }
        public Task<List<RelationshipDTO>> GetOrginalRelationships()
        {
            return Task.Run(() =>
            {
                var result = bizRelationship.GetOrginalRelationships(Database.ID);
                return result;
            });
        }
        private void SetRelationshipAlias(List<RelationshipImportItem> relationshipItems, List<TableDrivedEntityDTO> entities)
        {
            foreach (var item in relationshipItems)
            {
                var firstSideEntity = entities.First(x => x.Name == item.Relationship.Entity1);
                var secondSideEntity = entities.First(x => x.Name == item.Relationship.Entity2);
                string otherSideName = secondSideEntity.Alias;
                //bool shouldIncludForeignKeyNames = false;
                //if (relationshipItems.Any(x => x != item && x.Relationship.Entity1 == item.Relationship.Entity1 && x.Relationship.Entity2 == item.Relationship.Entity2)
                //    || entities.Any(x => x.Relationships.Any(z => z.Entity1 == item.Relationship.Entity1 && z.Entity2 == item.Relationship.Entity2)))
                //    shouldIncludForeignKeyNames = true;
                //var aa = relationshipItems.FirstOrDefault(x => x != item && x.Relationship.Entity1 == item.Relationship.Entity1 && x.Relationship.Entity2 == item.Relationship.Entity2);
                //if (shouldIncludForeignKeyNames)
                //{
                //    string colNames = "";
                //    foreach (var relCol in item.Relationship.RelationshipColumns)
                //    {
                //        colNames += (colNames == "" ? "" : ",") + secondSideEntity.Columns.First(x => x.ID == relCol.SecondSideColumnID).Alias;
                //    }
                //    item.Relationship.Alias = otherSideName + ":" + colNames;
                //}
                //else
                item.Relationship.Alias = otherSideName;
            }
        }

        private string OrginalRelationshipEntitiesExists(RelationshipDTO relationship, List<TableDrivedEntityDTO> originalEntities)
        {
            string result = "";

            if (!originalEntities.Any(x => x.Name == relationship.Entity1))
                result += (result == "" ? "" : Environment.NewLine) + "موجودیت کلید اصلی به نام" + " " + relationship.Entity1 + " " + "وجود ندارد";
            if (!originalEntities.Any(x => x.Name == relationship.Entity2))
                result += (result == "" ? "" : Environment.NewLine) + "موجودیت کلید فرعی به نام" + " " + relationship.Entity2 + " " + "وجود ندارد";

            if (result == "")
            {
                foreach (var col in relationship.RelationshipColumns)
                {
                    if (!originalEntities.Any(x => x.Name == relationship.Entity1 && x.Columns.Any(y => y.Name == col.FirstSideColumn.Name)))
                        result += (result == "" ? "" : Environment.NewLine) + "ستون" + " " + col.FirstSideColumn.Name + "در موجودیت" + " " + relationship.Entity1 + " " + "وجود ندارد";
                    if (!originalEntities.Any(x => x.Name == relationship.Entity2 && x.Columns.Any(y => y.Name == col.SecondSideColumn.Name)))
                        result += (result == "" ? "" : Environment.NewLine) + "ستون" + " " + col.FirstSideColumn.Name + "در موجودیت" + " " + relationship.Entity1 + " " + "وجود ندارد";
                }
            }

            return result;
        }

        private RelationshipDTO OrginalRelationshipExists(RelationshipDTO relationship, List<RelationshipDTO> originalRelationships)
        {
            List<string> firstSideSecondSideColumnNames = relationship.RelationshipColumns.Select(x => x.FirstSideColumn.Name + ">" + x.SecondSideColumn.Name).ToList();

            if (originalRelationships.Any(x => x.Entity1 == relationship.Entity1 && x.Entity2 == relationship.Entity2
            && x.RelationshipColumns.All(z => firstSideSecondSideColumnNames.Contains(z.FirstSideColumn.Name + ">" + z.SecondSideColumn.Name))
            && firstSideSecondSideColumnNames.All(z => x.RelationshipColumns.Any(y => z == y.FirstSideColumn.Name + ">" + y.SecondSideColumn.Name))))
            {
                return originalRelationships.First(x => x.Entity1 == relationship.Entity1 && x.Entity2 == relationship.Entity2
            && x.RelationshipColumns.All(z => firstSideSecondSideColumnNames.Contains(z.FirstSideColumn.Name + ">" + z.SecondSideColumn.Name))
            && firstSideSecondSideColumnNames.All(z => x.RelationshipColumns.Any(y => z == y.FirstSideColumn.Name + ">" + y.SecondSideColumn.Name)));
            }
            return null;
        }

        private Task DetermineRelationshipType(List<RelationshipImportItem> listNew, List<TableDrivedEntityDTO> entities)
        {
            return Task.Run(() =>
           {
               BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
               ModelDataHelper dataHelper = new ModelDataHelper();
               Dictionary<string, string> isaKeys = new Dictionary<string, string>();
               foreach (var item in listNew)
               {
                   var pkEntity = entities.First(x => x.Name == item.Relationship.Entity1);
                   var fkEntity = entities.First(x => x.Name == item.Relationship.Entity2);
                   item.Relationship.EntityID1 = pkEntity.ID;
                   item.Relationship.EntityID2 = fkEntity.ID;
                   item.Relationship.Entity1 = pkEntity.Name;
                   item.Relationship.Entity2 = fkEntity.Name;
                   item.Relationship.DatabaseID1 = pkEntity.DatabaseID;
                   item.Relationship.DatabaseID2 = fkEntity.DatabaseID;

                   foreach (var relcol in item.Relationship.RelationshipColumns)
                   {
                       var col1 = pkEntity.Columns.First(x => x.Name == relcol.FirstSideColumn.Name);
                       relcol.FirstSideColumnID = col1.ID;
                       relcol.FirstSideColumn = col1;

                       var col2 = fkEntity.Columns.First(x => x.Name == relcol.SecondSideColumn.Name);
                       relcol.SecondSideColumnID = col2.ID;
                       relcol.SecondSideColumn = col2;
                   }
                   //item.Relationship.RelatesOnPrimaryKeys = bizRelationship.RelatesOnPrimaryKeys(item.Relationship, new List<TableDrivedEntityDTO>() { pkEntity, fkEntity });
                   //item.Relationship.FKCoumnIsNullable = item.Relationship.RelationshipColumns.Any(x => x.SecondSideColumn.IsNull);


                   InfoUpdated(this, new ItemImportingStartedArg() { CurrentProgress = listNew.IndexOf(item) + 1, TotalProgressCount = listNew.Count, ItemName = "Determinig Relationship Type" + " " + item.Relationship.Name });
                   item.Relationship.RelationInfo = dataHelper.GetRelationshipsInfo(item.Relationship, pkEntity, fkEntity);


                   //if (relationInfo.FKRelatesOnPrimaryKey)
                   //{
                   //    //var subEntity = bizTableDrivedEntity.GetTableDrivedEntity(relationship.EntityID2, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
                   //    //if (isaRelationship == null)
                   //    //{
                   //    //    isaRelationship = new Tuple<TableDrivedEntityDTO, List<Tuple<RelationshipDTO, TableDrivedEntityDTO>>>(entity, new List<Tuple<RelationshipDTO, TableDrivedEntityDTO>>() { new Tuple<RelationshipDTO, TableDrivedEntityDTO>(relationship, subEntity) });
                   //    //}
                   //    //else
                   //    //    isaRelationship.Item2.Add(new Tuple<RelationshipDTO, TableDrivedEntityDTO>(relationship, subEntity));
                   //    item.Relationship.TypeEnum = Enum_RelationshipType.SuperToSub;
                   //}
                   //else
                   //{
                   Enum_OrginalRelationshipType originalType = Enum_OrginalRelationshipType.None;
                   if (item.Relationship.FKSidePKColumnsAreFkColumns)
                       originalType = Enum_OrginalRelationshipType.SuperToSub;
                   else
                   {
                       if (Database.DBHasData)
                       {

                           if (item.Relationship.RelationInfo.MoreThanOneFkForEachPK == false)
                               originalType = Enum_OrginalRelationshipType.OneToOne;
                           else
                               originalType = Enum_OrginalRelationshipType.OneToMany;
                       }
                       else
                       {
                           originalType = Enum_OrginalRelationshipType.OneToMany;
                       }
                   }
                   if (originalType == Enum_OrginalRelationshipType.SuperToSub)
                   {
                       item.Relationship.OrginalTypeEnum = Enum_OrginalRelationshipType.SuperToSub;
                       if (isaKeys.ContainsKey(item.Relationship.Entity1))
                       {
                           item.Relationship.OrginalRelationshipGroup = isaKeys[item.Relationship.Entity1];
                       }
                       else
                       {
                           var key = "ISA_OnPK_" + item.Relationship.Entity1;
                           isaKeys.Add(item.Relationship.Entity1, key);
                           item.Relationship.OrginalRelationshipGroup = key;
                       }
                   }
                   else if (originalType == Enum_OrginalRelationshipType.OneToOne)
                   {

                       bool otoCondition = false;
                       if (item.Relationship.RelationInfo.FKHasData == true)
                       {
                           //این شرط جدید اضافه شد. برای اینکه اگر کلید خارجی جزو پرایمری ها باشد معلوم است که ارتباط چند به چند یا مالکیت منظور بوده است 
                           if (!item.Relationship.RelationshipColumns.Any(x => x.SecondSideColumn.PrimaryKey))
                           {
                               long relcount = dataHelper.GetRelationDataCount(fkEntity, item.Relationship);
                               if (relcount > 10)
                               {
                                   long fkcount = dataHelper.GetDataCount(fkEntity);
                                   long pkcount = dataHelper.GetDataCount(pkEntity);

                                   long biggerCount = 0;
                                   long smallerCount = 0;
                                   if (fkcount >= pkcount)
                                   {
                                       biggerCount = fkcount;
                                       smallerCount = pkcount;
                                   }
                                   else
                                   {
                                       biggerCount = pkcount;
                                       smallerCount = fkcount;
                                   }
                                   //if (pkcount > relcount)
                                   //{
                                   if (biggerCount > 50000)
                                   {
                                       if (relcount >= biggerCount / 10)
                                           otoCondition = true;
                                   }
                                   else if (pkcount > 10000)
                                   {
                                       if (relcount >= biggerCount / 5)
                                           otoCondition = true;
                                   }
                                   else if (pkcount > 2000)
                                   {
                                       if (relcount >= biggerCount / 2.5)
                                           otoCondition = true;
                                   }
                                   else
                                   {
                                       if (relcount >= biggerCount / 1.5)
                                           otoCondition = true;
                                   }
                               }
                               //}
                               //else
                               //{
                               //    if(relcount>10)
                               //        otoCondition = true;
                               //}
                           }
                       }
                       if (otoCondition)
                           item.Relationship.OrginalTypeEnum = Enum_OrginalRelationshipType.OneToOne;
                       else
                           item.Relationship.OrginalTypeEnum = Enum_OrginalRelationshipType.OneToMany;
                   }
                   else
                   {
                       item.Relationship.OrginalTypeEnum = Enum_OrginalRelationshipType.OneToMany;
                   }
                   //}
               }
           });
        }

        private bool RelationshipExists(RelationshipDTO item, List<RelationshipDTO> result)
        {
            foreach (var importedRel in result)
            {
                if (importedRel.Entity1 == item.TableName1
                    && importedRel.Entity2 == item.TableName2)
                {
                    if (importedRel.RelationshipColumns.All(x => item.RelationshipColumns.Any(y => x.FirstSideColumn.Name == y.FirstSideColumn.Name)))
                    {
                        if (item.RelationshipColumns.All(x => importedRel.RelationshipColumns.Any(y => x.FirstSideColumn.Name == y.FirstSideColumn.Name)))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public Task<List<RelationshipImportItem>> GetRelationshipsInfo()
        {
            return Task.Run(() =>
            {
                var result = ImportHelper.GetRelationships().OrderBy(x => x.Relationship.Entity1).ToList();
                return result;
            });
        }

        private void ImportHelper_ItemImportingStarted(object sender, ItemImportingStartedArg e)
        {
            if (InfoUpdated != null)
                InfoUpdated(this, e);
        }

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
                foreach (var item in listNew.Where(x => x.Selected))
                {
                    var validationTooltip = "";
                    if (item.Relationship.OrginalTypeEnum == Enum_OrginalRelationshipType.None)
                    {
                        validationTooltip += (validationTooltip == "" ? "" : Environment.NewLine) + "نوع رابطه تعیین نشده است";
                    }
                    if (item.Relationship.OrginalTypeEnum == Enum_OrginalRelationshipType.OneToMany && item.Relationship.FKSidePKColumnsAreFkColumns)
                    {
                        validationTooltip += (validationTooltip == "" ? "" : Environment.NewLine) + "ارتباط بروی کلیدهای اصلی است، بنابراین رابطه نمیتواند یک به چند باشد";
                    }
                    if (item.Relationship.OrginalTypeEnum == Enum_OrginalRelationshipType.OneToOne && item.Relationship.RelationInfo.FKHasData == true && item.Relationship.RelationInfo.MoreThanOneFkForEachPK == true)
                    {
                        validationTooltip += (validationTooltip == "" ? "" : Environment.NewLine) + "برخی داده ها برای این ارتباط بصورت یک به چند می باشند، بنابراین رابطه نمیتواند یک به یک باشد";
                    }
                    if (item.Relationship.OrginalTypeEnum == Enum_OrginalRelationshipType.SuperToSub && item.Relationship.RelationInfo.FKHasData == true && item.Relationship.RelationInfo.MoreThanOneFkForEachPK == true)
                    {
                        validationTooltip += (validationTooltip == "" ? "" : Environment.NewLine) + "برخی داده ها برای این ارتباط بصورت یک به چند می باشند، بنابراین رابطه نمیتواند ارث بری باشد";
                    }
                    if (item.Relationship.OrginalTypeEnum == Enum_OrginalRelationshipType.SubUnionToUnion && item.Relationship.RelationInfo.FKHasData == true && item.Relationship.RelationInfo.MoreThanOneFkForEachPK == true)
                    {
                        validationTooltip += (validationTooltip == "" ? "" : Environment.NewLine) + "برخی داده ها برای این ارتباط بصورت یک به چند می باشند، بنابراین رابطه نمیتواند اتحاد باشد";
                    }
                    if (item.Relationship.OrginalTypeEnum == Enum_OrginalRelationshipType.SuperToSub || item.Relationship.OrginalTypeEnum == Enum_OrginalRelationshipType.SubUnionToUnion)
                    {
                        if (string.IsNullOrEmpty(item.Relationship.OrginalRelationshipGroup))
                            validationTooltip += (validationTooltip == "" ? "" : Environment.NewLine) + "عنوان گروه مشخص نشده است";
                    }
                    item.ValidationTooltip = validationTooltip;
                    item.IsValid = string.IsNullOrEmpty(validationTooltip);
                }
                dtgNewRelationships.ItemsSource = null;
                dtgNewRelationships.ItemsSource = listNew;
                if (listNew.Where(x => x.Selected).Any() && listNew.Where(x => x.Selected).All(x => x.IsValid))
                {
                    var newRelationships = listNew.Where(x => x.Selected).Select(x => x.Relationship).ToList();
                    var deletedRelationships = listDeleted.Where(x => x.Selected).Select(x => x.Relationship).ToList();
                    if (newRelationships.Any() || deletedRelationships.Any())
                    {
                        await UpdateModel(Database.ID, newRelationships, deletedRelationships);
                        updated = true;
                        MessageBox.Show("انتقال اطلاعات انجام شد");
                    }
                    else
                    {
                        MessageBox.Show("موردی جهت انتقال انتخاب نشده است");
                    }
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
        private Task UpdateModel(int databaseID, List<RelationshipDTO> listNew, List<RelationshipDTO> listDeleted)
        {
            return Task.Run(() =>
            {
                bizRelationship.UpdateModel(MyProjectManager.GetMyProjectManager.GetRequester(), databaseID, listNew, listDeleted);
            });
        }

        public bool HasData()
        {
            return true;
        }
    }


}
