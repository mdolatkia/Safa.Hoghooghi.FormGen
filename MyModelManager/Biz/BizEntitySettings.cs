using DataAccess;
using ModelEntites;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizEntitySettings
    {
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        BizEntityListView bizEntityListView = new BizEntityListView();
        BizEntitySearch bizEntitySearch = new BizEntitySearch();
        BizEntityUIComposition bizEntityUIComposition = new BizEntityUIComposition();
        public event EventHandler<ItemImportingStartedArg> ItemImportingStarted;
        private void BizTableDrivedEntity_ItemImportingStarted(object sender, ItemImportingStartedArg e)
        {
            if (ItemImportingStarted != null)
                ItemImportingStarted(this, e);
        }
        public void UpdateDefaultSettingsInModel(DR_Requester requester, List<int> entityIDs)
        {
            BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
            bizTableDrivedEntity.ItemImportingStarted += BizTableDrivedEntity_ItemImportingStarted;
            List<TableDrivedEntityDTO> listEntities = new List<TableDrivedEntityDTO>();
            foreach (var id in entityIDs)
            {
                listEntities.Add(bizTableDrivedEntity.GetTableDrivedEntity(requester, id, EntityColumnInfoType.WithFullColumns, EntityRelationshipInfoType.WithRelationships));
            }
            var allEntities = bizTableDrivedEntity.GetOrginalEntities(listEntities.First().DatabaseID, EntityColumnInfoType.WithFullColumns, EntityRelationshipInfoType.WithRelationships, false);

            UpdateDefaultSettingsInModel(listEntities, listEntities, listEntities, listEntities, allEntities);
        }
        public void UpdateDefaultSettingsInModel(DR_Requester requester, int databaseID)
        {
            //try
            //{
            BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
            bizTableDrivedEntity.ItemImportingStarted += BizTableDrivedEntity_ItemImportingStarted;

            List<TableDrivedEntityDTO> allEntities = new List<TableDrivedEntityDTO>();
            var listEntityIds = bizTableDrivedEntity.GetEntityIDs(databaseID, false);
            foreach (var id in listEntityIds)
            {
                allEntities.Add(bizTableDrivedEntity.GetTableDrivedEntity(requester, id, EntityColumnInfoType.WithFullColumns, EntityRelationshipInfoType.WithRelationships));
            }

            var uiCompositionEntities = bizTableDrivedEntity.GetOrginalEntitiesWithoutUIComposition(databaseID, EntityColumnInfoType.WithFullColumns, EntityRelationshipInfoType.WithRelationships, false);
            var listViewEntities = allEntities.Where(x => x.EntityListViewID == 0).ToList();

            var searchEntities = allEntities.Where(x => x.EntitySearchID == 0).ToList();

            var initialSearchEntities = allEntities.Where(x => x.SearchInitially == null).ToList();

            UpdateDefaultSettingsInModel(uiCompositionEntities, listViewEntities, searchEntities, initialSearchEntities, allEntities);
        }
        public void UpdateDefaultSettingsInModel(List<TableDrivedEntityDTO> uiCompositionEntities, List<TableDrivedEntityDTO> listViewEntities, List<TableDrivedEntityDTO> searchEntities, List<TableDrivedEntityDTO> initialSearchEntities, List<TableDrivedEntityDTO> allEntities)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                List<Tuple<TableDrivedEntityDTO, List<EntityUICompositionDTO>>> listEntityUIComposition = new List<Tuple<TableDrivedEntityDTO, List<EntityUICompositionDTO>>>();

                foreach (var entity in uiCompositionEntities)
                {
                    if (ItemImportingStarted != null)
                        ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = "Fetching entity" + " " + entity.Name, TotalProgressCount = uiCompositionEntities.Count(), CurrentProgress = uiCompositionEntities.IndexOf(entity) + 1 });
                    var uiComposition = bizEntityUIComposition.GenerateUIComposition(entity);
                    listEntityUIComposition.Add(new Tuple<TableDrivedEntityDTO, List<EntityUICompositionDTO>>(entity, uiComposition));

                }
                foreach (var item in listEntityUIComposition)
                {
                    if (ItemImportingStarted != null)
                        ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = "Setting UI for entity" + " " + item.Item1.Name, TotalProgressCount = listEntityUIComposition.Count(), CurrentProgress = listEntityUIComposition.IndexOf(item) + 1 });
                    bizEntityUIComposition.SaveItem(projectContext, item.Item1.ID, item.Item2);
                }

                List<Tuple<TableDrivedEntityDTO, EntityListViewDTO>> listEntityAndView = new List<Tuple<TableDrivedEntityDTO, EntityListViewDTO>>();
                foreach (var entity in listViewEntities)
                {
                    if (ItemImportingStarted != null)
                        ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = "Fetching entity" + " " + entity.Name, TotalProgressCount = listViewEntities.Count(), CurrentProgress = listViewEntities.IndexOf(entity) + 1 });
                    var viewItem = bizEntityListView.GenerateDefaultListView(entity, allEntities);
                    listEntityAndView.Add(new Tuple<TableDrivedEntityDTO, EntityListViewDTO>(entity, viewItem));
                }


                List<EntityRelationshipTail> createdRelationshipTails = new List<EntityRelationshipTail>();
                foreach (var item in listEntityAndView)
                {
                    if (ItemImportingStarted != null)
                        ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = "Setting default view for entity" + " " + item.Item1.Name, TotalProgressCount = listEntityAndView.Count(), CurrentProgress = listEntityAndView.IndexOf(item) + 1 });
                    var dbListView = bizEntityListView.SaveItem(projectContext, item.Item2, createdRelationshipTails);
                    var dbentity = projectContext.TableDrivedEntity.First(x => x.ID == item.Item1.ID);
                    dbentity.EntityListView1 = dbListView;
                }


                List<Tuple<TableDrivedEntityDTO, EntitySearchDTO>> listEntityAndSearch = new List<Tuple<TableDrivedEntityDTO, EntitySearchDTO>>();
                foreach (var entity in searchEntities)
                {
                    if (ItemImportingStarted != null)
                        ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = "Fetching entity" + " " + entity.Name, TotalProgressCount = searchEntities.Count(), CurrentProgress = searchEntities.IndexOf(entity) + 1 });
                    var searchItem = bizEntitySearch.GenerateDefaultSearchList(entity, allEntities);
                    listEntityAndSearch.Add(new Tuple<TableDrivedEntityDTO, EntitySearchDTO>(entity, searchItem));
                }

                foreach (var item in listEntityAndSearch)
                {
                    if (ItemImportingStarted != null)
                        ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = "Setting default search for entity" + " " + item.Item1.Name, TotalProgressCount = listEntityAndSearch.Count(), CurrentProgress = listEntityAndSearch.IndexOf(item) + 1 });
                    var dbSearch = bizEntitySearch.SaveItem(projectContext, item.Item2, createdRelationshipTails);
                    var dbentity = projectContext.TableDrivedEntity.First(x => x.ID == item.Item1.ID);
                    dbentity.EntitySearch = dbSearch;
                }


                List<Tuple<TableDrivedEntityDTO, bool>> listEntityAndInitialSearch = new List<Tuple<TableDrivedEntityDTO, bool>>();
                foreach (var entity in initialSearchEntities)
                {
                    if (ItemImportingStarted != null)
                        ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = "Fetching entity" + " " + entity.Name, TotalProgressCount = searchEntities.Count(), CurrentProgress = initialSearchEntities.IndexOf(entity) + 1 });
                    bool initiallySearched = bizTableDrivedEntity.DecideEntityIsInitialySearched(entity, allEntities);
                    listEntityAndInitialSearch.Add(new Tuple<TableDrivedEntityDTO, bool>(entity, initiallySearched));
                }
                foreach (var item in listEntityAndInitialSearch)
                {
                    if (ItemImportingStarted != null)
                        ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = "Setting initially searched entity" + " " + item.Item1.Name, TotalProgressCount = listEntityAndInitialSearch.Count(), CurrentProgress = listEntityAndInitialSearch.IndexOf(item) + 1 });
                    bizTableDrivedEntity.UpdateEntityInitiallySearch(projectContext, item.Item1.ID, item.Item2);
                }


                if (ItemImportingStarted != null)
                    ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = "Saving changes" });
                projectContext.SaveChanges();

            }
        }

        public void SetCustomSettings(int databaseID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbDatabase = projectContext.DatabaseInformation.First(x => x.ID == databaseID);
                var realPerson = projectContext.TableDrivedEntity.First(x => x.Name == "RealPerson" && x.Table.DBSchema.DatabaseInformationID == databaseID);
                if (realPerson != null)
                {

                }
                var legalPerson = projectContext.TableDrivedEntity.First(x => x.Name == "LegalPerson" && x.Table.DBSchema.DatabaseInformationID == databaseID);

                var genericPerson = projectContext.TableDrivedEntity.First(x => x.Name == "GenericPerson" && x.Table.DBSchema.DatabaseInformationID == databaseID);
                if (genericPerson != null)
                {
                    var emailColumn = genericPerson.Table.Column.First(x => x.Name == "EmailAddress");
                    if (emailColumn != null)
                        emailColumn.StringColumnType.Format = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

                    var typeColumn = genericPerson.Table.Column.First(x => x.Name == "Type");
                    if (typeColumn != null)
                    {
                        if (typeColumn.ColumnValueRange == null)
                        {
                            typeColumn.ColumnValueRange = new ColumnValueRange();
                            typeColumn.ColumnValueRange.ColumnValueRangeDetails.Add(new ColumnValueRangeDetails() { Value = "1", KeyTitle = "حقیقی" });
                            typeColumn.ColumnValueRange.ColumnValueRangeDetails.Add(new ColumnValueRangeDetails() { Value = "2", KeyTitle = "حقوقی" });
                        }
                    }
                    if (realPerson != null)
                    {
                        var relaitonship = projectContext.Relationship.FirstOrDefault(x => x.TableDrivedEntityID1 == genericPerson.ID && x.TableDrivedEntityID2 == realPerson.ID);
                        if (typeColumn != null && relaitonship != null && relaitonship.RelationshipType != null && relaitonship.RelationshipType.SuperToSubRelationshipType != null)
                        {
                            if (relaitonship.RelationshipType.SuperToSubRelationshipType.DeterminerColumnID == null)
                            {
                                relaitonship.RelationshipType.SuperToSubRelationshipType.DeterminerColumnID = typeColumn.ID;
                                relaitonship.RelationshipType.SuperToSubRelationshipType.DeterminerColumnValue = "1";
                            }
                        }
                    }
                    if (legalPerson != null)
                    {
                        var relaitonship = projectContext.Relationship.FirstOrDefault(x => x.TableDrivedEntityID1 == genericPerson.ID && x.TableDrivedEntityID2 == legalPerson.ID);
                        if (typeColumn != null && relaitonship != null && relaitonship.RelationshipType != null && relaitonship.RelationshipType.SuperToSubRelationshipType != null)
                        {
                            if (relaitonship.RelationshipType.SuperToSubRelationshipType.DeterminerColumnID == null)
                            {
                                relaitonship.RelationshipType.SuperToSubRelationshipType.DeterminerColumnID = typeColumn.ID;
                                relaitonship.RelationshipType.SuperToSubRelationshipType.DeterminerColumnValue = "2";
                            }
                        }
                    }
                }

                var serviceItem = projectContext.TableDrivedEntity.First(x => x.Name == "ServiceItem" && x.Table.DBSchema.DatabaseInformationID == databaseID);
                if (serviceItem != null)
                {
                    var updateDateColumn = serviceItem.Table.Column.First(x => x.Name == "UpdateDate");
                    if (updateDateColumn != null)
                        updateDateColumn.DateColumnType.ShowMiladiDateInUI = true;
                    var updateTimeColumn = serviceItem.Table.Column.First(x => x.Name == "UpdateTime");
                    if (updateTimeColumn != null)
                    {
                    }
                }

                var serviceAdditionalItem = projectContext.TableDrivedEntity.First(x => x.Name == "ServiceAdditionalItem" && x.Table.DBSchema.DatabaseInformationID == databaseID);
                var serviceConclusionItem = projectContext.TableDrivedEntity.First(x => x.Name == "ServiceConclusionItem" && x.Table.DBSchema.DatabaseInformationID == databaseID);
                if (serviceConclusionItem != null)
                {
                    var typeColumn = serviceConclusionItem.Table.Column.First(x => x.Name == "Type");
                    if (typeColumn != null)
                    {
                        if (typeColumn.ColumnValueRange == null)
                        {
                            typeColumn.ColumnValueRange = new ColumnValueRange();
                            typeColumn.ColumnValueRange.ColumnValueRangeDetails.Add(new ColumnValueRangeDetails() { Value = "1", KeyTitle = "سرویس" });
                            typeColumn.ColumnValueRange.ColumnValueRangeDetails.Add(new ColumnValueRangeDetails() { Value = "2", KeyTitle = "خدمات اضافی" });
                        }
                    }
                    if (serviceItem != null)
                    {
                        var relaitonship = projectContext.Relationship.FirstOrDefault(x => x.TableDrivedEntityID1 == serviceConclusionItem.ID && x.TableDrivedEntityID2 == serviceItem.ID);
                        if (typeColumn != null && relaitonship != null && relaitonship.RelationshipType != null && relaitonship.RelationshipType.UnionToSubUnionRelationshipType != null)
                        {
                            if (relaitonship.RelationshipType.UnionToSubUnionRelationshipType.DeterminerColumnID == null)
                            {
                                relaitonship.RelationshipType.UnionToSubUnionRelationshipType.DeterminerColumnID = typeColumn.ID;
                                relaitonship.RelationshipType.UnionToSubUnionRelationshipType.DeterminerColumnValue = "1";
                            }
                        }
                    }
                    if (serviceAdditionalItem != null)
                    {
                        var relaitonship = projectContext.Relationship.FirstOrDefault(x => x.TableDrivedEntityID1 == serviceConclusionItem.ID && x.TableDrivedEntityID2 == serviceAdditionalItem.ID);
                        if (typeColumn != null && relaitonship != null && relaitonship.RelationshipType != null && relaitonship.RelationshipType.UnionToSubUnionRelationshipType != null)
                        {
                            if (relaitonship.RelationshipType.UnionToSubUnionRelationshipType.DeterminerColumnID == null)
                            {
                                relaitonship.RelationshipType.UnionToSubUnionRelationshipType.DeterminerColumnID = typeColumn.ID;
                                relaitonship.RelationshipType.UnionToSubUnionRelationshipType.DeterminerColumnValue = "1";
                            }
                        }
                    }
                }

                var serviceConclusion = projectContext.TableDrivedEntity.First(x => x.Name == "ServiceConclusion" && x.Table.DBSchema.DatabaseInformationID == databaseID);
                if (serviceConclusion != null)
                {
                    var userRateColumn = serviceConclusion.Table.Column.First(x => x.Name == "UserRate");
                    if (userRateColumn != null)
                    {
                        userRateColumn.NumericColumnType.MinValue = 1;
                        userRateColumn.NumericColumnType.MaxValue = 5;
                    }
                    var stringUpdateDateTime = serviceConclusion.Table.Column.First(x => x.Name == "UpdateDateTime");
                    if (stringUpdateDateTime != null)
                    {
                        stringUpdateDateTime.DateTimeColumnType.ShowMiladiDateInUI = true;
                        stringUpdateDateTime.DateTimeColumnType.HideTimePicker = true;
                        stringUpdateDateTime.DateTimeColumnType.StringDateIsMiladi = true;
                        stringUpdateDateTime.DateTimeColumnType.StringTimeIsMiladi = false;
                        stringUpdateDateTime.DateTimeColumnType.StringTimeISAMPMFormat = true;
                        stringUpdateDateTime.DateTimeColumnType.ShowAMPMFormat = true;
                    }
                }
                var requestProductPart = projectContext.TableDrivedEntity.First(x => x.Name == "RequestProductPart" && x.Table.DBSchema.DatabaseInformationID == databaseID);
                if (requestProductPart != null)
                {
                    var dateTimeColumn = requestProductPart.Table.Column.First(x => x.Name == "DateTime");
                    if (dateTimeColumn != null)
                        dateTimeColumn.DateTimeColumnType.ShowMiladiDateInUI = true;
                    var stringDateTimeColumn = requestProductPart.Table.Column.First(x => x.Name == "StringDateTime");
                    if (stringDateTimeColumn != null)
                    {
                        stringDateTimeColumn.DateTimeColumnType.StringDateIsMiladi = true;
                        stringDateTimeColumn.DateTimeColumnType.StringTimeIsMiladi = true;
                        stringDateTimeColumn.DateTimeColumnType.StringTimeISAMPMFormat = true;
                        stringDateTimeColumn.DateTimeColumnType.ShowAMPMFormat = true;
                    }
                }
             
                var serviceRequest = projectContext.TableDrivedEntity.First(x => x.Name == "ServiceRequest" && x.Table.DBSchema.DatabaseInformationID == databaseID);
                if (serviceRequest != null)
                {
                    var persianDateColumn = serviceRequest.Table.Column.First(x => x.Name == "PersianDate");
                    if (persianDateColumn != null)
                        persianDateColumn.DateColumnType.ShowMiladiDateInUI = true;
                    var timeColumn = serviceRequest.Table.Column.First(x => x.Name == "Time");
                    if (timeColumn != null)
                    {
                        timeColumn.TimeColumnType.ShowAMPMFormat = true;
                        timeColumn.TimeColumnType.StringTimeISAMPMFormat = true;
                        timeColumn.TimeColumnType.StringTimeIsMiladi = false;
                        timeColumn.TimeColumnType.ShowMiladiTime = true;
                    }
                }
                var srviceRequestReview = projectContext.TableDrivedEntity.First(x => x.Name == "ServiceRequestReview" && x.Table.DBSchema.DatabaseInformationID == databaseID);
                if (srviceRequestReview != null)
                {
                    var autoDateColumn = srviceRequestReview.Table.Column.First(x => x.Name == "AutoDate");
                    if (autoDateColumn != null)
                        autoDateColumn.DateColumnType.StringDateIsMiladi = true;
                    var autoTimeColumn = srviceRequestReview.Table.Column.First(x => x.Name == "AutoTime");
                    if (autoTimeColumn != null)
                    {
                        autoTimeColumn.TimeColumnType.ShowAMPMFormat = true;
                        autoTimeColumn.TimeColumnType.StringTimeIsMiladi = true;
                        autoTimeColumn.TimeColumnType.StringTimeISAMPMFormat = true;
                    }
                }

                var employee = projectContext.TableDrivedEntity.First(x => x.Name == "Employee" && x.Table.DBSchema.DatabaseInformationID == databaseID);
                if (employee != null)
                {
                    var employeeRoleColumn = employee.Table.Column.First(x => x.Name == "EmployeeRole");
                    if (employeeRoleColumn != null)
                    {
                        if (employeeRoleColumn.ColumnValueRange == null)
                        {
                            employeeRoleColumn.ColumnValueRange = new ColumnValueRange();

                            employeeRoleColumn.ColumnValueRange.ColumnValueRangeDetails.Add(new ColumnValueRangeDetails() { Value = "1", KeyTitle = "مدیر" });
                            employeeRoleColumn.ColumnValueRange.ColumnValueRangeDetails.Add(new ColumnValueRangeDetails() { Value = "2", KeyTitle = "کارشناس" });
                        }
                    }
                }
                var office = projectContext.TableDrivedEntity.First(x => x.Name == "Office" && x.Table.DBSchema.DatabaseInformationID == databaseID);
                if (office != null)
                {
                    var workshopLevelColumn = office.Table.Column.First(x => x.Name == "WorkshopLevel");
                    if (workshopLevelColumn != null)
                    {
                        if (workshopLevelColumn.ColumnValueRange == null)
                        {
                            workshopLevelColumn.ColumnValueRange = new ColumnValueRange();

                            workshopLevelColumn.ColumnValueRange.ColumnValueRangeDetails.Add(new ColumnValueRangeDetails() { Value = "سطح1" });
                            workshopLevelColumn.ColumnValueRange.ColumnValueRangeDetails.Add(new ColumnValueRangeDetails() { Value = "سطح2" });
                        }
                    }
                    var codeColumn = office.Table.Column.First(x => x.Name == "Code");
                    if (codeColumn != null)
                    {
                        codeColumn.StringColumnType.MaxLength = 3;
                        codeColumn.StringColumnType.MinLength = 3;
                    }
                }

                var region = projectContext.TableDrivedEntity.First(x => x.Name == "Region" && x.Table.DBSchema.DatabaseInformationID == databaseID);
                if (region != null)
                {
                    var typeColumn = region.Table.Column.First(x => x.Name == "Type");
                    if (typeColumn != null)
                    {
                        if (typeColumn.ColumnValueRange == null)
                        {
                            typeColumn.ColumnValueRange = new ColumnValueRange();

                            typeColumn.ColumnValueRange.ColumnValueRangeDetails.Add(new ColumnValueRangeDetails() { Value = "کشور" });
                            typeColumn.ColumnValueRange.ColumnValueRangeDetails.Add(new ColumnValueRangeDetails() { Value = "استان" });
                            typeColumn.ColumnValueRange.ColumnValueRangeDetails.Add(new ColumnValueRangeDetails() { Value = "شهر" });

                        }
                    }
                }
                projectContext.SaveChanges();
            }
        }

        public bool EntityWithoutSetting(int databaseID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                return projectContext.TableDrivedEntity.Any(x =>
                x.IsOrginal == true && x.IsView == false && x.IsDisabled == false && x.Table.DBSchema.DatabaseInformationID == databaseID
                && (x.EntitySearchID == null || x.EntityListViewID == null || !x.EntityUIComposition.Any() || x.SearchInitially == null));
            }
        }


    }
}
