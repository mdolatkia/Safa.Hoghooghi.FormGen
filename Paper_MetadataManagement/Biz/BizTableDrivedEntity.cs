
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paper_MetadataManagement
{
    public class BizTableDrivedEntity
    {




        public bool IndependentDataEntry(int entityID)
        {
            using (var projectContext = new MyIdeaEntities())
            {
                return projectContext.TableDrivedEntity.First(x => x.ID == entityID).IndependentDataEntry == true;
            }
        }

        public List<TableDrivedEntityDTO> GetAllEntities(string generalFilter = "")
        {
            List<TableDrivedEntityDTO> result = new List<TableDrivedEntityDTO>();
            using (var projectContext = new MyIdeaEntities())
            {
                IQueryable<TableDrivedEntity> listEntity = projectContext.TableDrivedEntity;
                if (generalFilter != "")
                    listEntity = listEntity.Where(x => x.ID.ToString() == generalFilter || x.Name.Contains(generalFilter) || x.Alias.Contains(generalFilter));
                foreach (var item in listEntity)
                    result.Add(ToTableDrivedEntityDTO(item, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships));
            }
            return result;
        }

        //public string GetCatalog(int entityID)
        //{
        //    using (var projectContext = new MyIdeaEntities())
        //    {
        //        return projectContext.TableDrivedEntity.First(x => x.ID == entityID).Table.Catalog;
        //    }
        //}
        //public List<TableDrivedEntityDTO> GetAllEntities(int databaseID)
        //{
        //    List<TableDrivedEntityDTO> result = new List<TableDrivedEntityDTO>();
        //    using (var projectContext = new MyIdeaEntities())
        //    {
        //        //string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
        //        var listEntity = projectContext.TableDrivedEntity.Where(x => x.Table.DBSchema.DatabaseInformationID == databaseID);
        //        foreach (var item in listEntity)
        //            result.Add(ToTableDrivedEntityDTO(item, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships));
        //    }
        //    return result;
        //}
        public List<TableDrivedEntityDTO> GetAllEntities(int databaseID)
        {
            //بهتره خود انتیتی با دیتابیس رابطه داشته باشد
            List<TableDrivedEntityDTO> result = new List<TableDrivedEntityDTO>();
            using (var projectContext = new MyIdeaEntities())
            {
                var listEntity = projectContext.TableDrivedEntity.Where(x => x.Table.DBSchema.DatabaseInformationID == databaseID);
                foreach (var item in listEntity)
                    result.Add(ToTableDrivedEntityDTO(item, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships));
            }
            return result;
        }


        public TableDrivedEntityDTO GetBaseEntity(int entityID)
        {

            using (var projectContext = new MyIdeaEntities())
            {
                var entity = projectContext.TableDrivedEntity.First(x => x.ID == entityID);

                if (string.IsNullOrEmpty(entity.Criteria))
                    return ToTableDrivedEntityDTO(entity, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships);
                else
                    return ToTableDrivedEntityDTO(projectContext.TableDrivedEntity.First(x => (x.Criteria == null || x.Criteria == "") && x.TableID == entity.TableID), EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships);
            }
        }
        public List<TableDrivedEntityDTO> GetOtherDrivedEntities(int entityID, int tableID, bool withInheritance)
        {
            List<TableDrivedEntityDTO> result = new List<TableDrivedEntityDTO>();
            using (var projectContext = new MyIdeaEntities())
            {
                if (withInheritance)
                {
                    var list = projectContext.TableDrivedEntity.Where(x => x.ID != entityID && x.TableID == tableID
                    && x.Relationship.Any(y => y.TableDrivedEntityID1 == x.ID && y.TableDrivedEntityID2 == entityID && y.RelationshipType != null && y.RelationshipType.SubToSuperRelationshipType != null));
                    foreach (var item in list)
                    {
                        result.Add(ToTableDrivedEntityDTO(item, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships));
                    }
                }
                else
                {
                    var list = projectContext.TableDrivedEntity.Where(x => x.ID != entityID && x.TableID == tableID);
                    foreach (var item in list)
                    {
                        result.Add(ToTableDrivedEntityDTO(item, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships));
                    }
                }
            }
            return result;
        }
        public int GetDefaultEntityID(int tableID)
        {
            List<TableDrivedEntityDTO> result = new List<TableDrivedEntityDTO>();
            using (var projectContext = new MyIdeaEntities())
            {
                return projectContext.Table.First(x => x.ID == tableID).TableDrivedEntity.First().ID;

            }
        }
        public string GetTableName(int entityID)
        {
            List<TableDrivedEntityDTO> result = new List<TableDrivedEntityDTO>();
            using (var projectContext = new MyIdeaEntities())
            {
                return projectContext.TableDrivedEntity.First(x => x.ID == entityID).Table.Name;

            }
        }
        public TableDrivedEntityDTO GetTableDrivedEntity(int entityID)
        {
            return GetTableDrivedEntity(entityID, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships);
        }
        public TableDrivedEntityDTO GetTableDrivedEntity(int entityID, EntityColumnInfoType columnInfoType, EntityRelationshipInfoType relationshipInfoType)
        {
            List<TableDrivedEntityDTO> result = new List<TableDrivedEntityDTO>();
            using (var projectContext = new MyIdeaEntities())
            {
                var table = projectContext.TableDrivedEntity.First(x => x.ID == entityID);
                return ToTableDrivedEntityDTO(table, columnInfoType, relationshipInfoType);
            }
        }
        private TableDrivedEntityDTO ToTableDrivedEntityDTO(TableDrivedEntity item, EntityColumnInfoType columnInfoType, EntityRelationshipInfoType relationshipInfoType)
        {
            //var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.Entity, item.ID.ToString(), columnInfoType.ToString(), relationshipInfoType.ToString());
            //if (cachedItem != null)
            //    return (cachedItem as TableDrivedEntityDTO);
            TableDrivedEntityDTO result = new TableDrivedEntityDTO();
            result.Name = item.Name;
            result.ID = item.ID;
            result.TableID = item.TableID;
            result.TableName = item.Table.Name;
            //    result.Schema = item.Table.DBSchema.Name;
            result.EntityListViewID = item.EntityListViewID??0;
            result.DatabaseID = item.Table.DBSchema.DatabaseInformationID;
            result.DatabaseName = item.Table.DBSchema.DatabaseInformation.Name;
            result.RelatedSchemaID = item.Table.DBSchemaID;
            result.RelatedSchema = item.Table.DBSchema.Name;
            result.ServerID = item.Table.DBSchema.DatabaseInformation.DBServerID;
            result.Alias = item.Alias;
            result.Criteria = item.Criteria;
            //result.SecurityObjectID = item.SecurityObjectID.Value;

            //if (result.UnionTypeEntities == "")
            //    if (item.Relationship.Any(x => (x.RelationshipType == null && x.Relationship2 != null && x.TableDrivedEntity != x.TableDrivedEntity1 && !x.RelationshipColumns.All(y => y.Column.PrimaryKey == true))
            //        || (x.Relationship2 == null && x.TableDrivedEntity != x.TableDrivedEntity1 && !x.RelationshipColumns.All(y => y.Column1.PrimaryKey == true))))
            //        result.UnionTypeEntities = "Choose UnionType";

            result.BatchDataEntry = item.BatchDataEntry;
            result.IsAssociative = item.IsAssociative;
            result.IsDataReference = item.IsDataReference;
            result.IsStructurReferencee = item.IsStructurReferencee;
            BizColumn bizColumn = new BizColumn();
            if (columnInfoType == EntityColumnInfoType.WithSimpleColumns)
                result.Columns = bizColumn.GetColumns(item, true);
            else if (columnInfoType == EntityColumnInfoType.WithFullColumns)
                result.Columns = bizColumn.GetColumns(item, false);
            if (relationshipInfoType == EntityRelationshipInfoType.WithRelationships)
            {
                BizISARelationship bizISARelationship = new BizISARelationship();
                BizUnionRelationship bizUnionRelationship = new Paper_MetadataManagement.BizUnionRelationship();
                BizRelationship bizRelationship = new BizRelationship();
                foreach (var relationship in item.Relationship)
                {
                    var relationshipDTO = bizRelationship.ToRelationshipDTO(relationship);
                    result.Relationships.Add(relationshipDTO);
                    if (relationship.RelationshipType != null)
                    {
                        if (relationship.RelationshipType.OneToManyRelationshipType != null)
                            result.OneToManyRelationships.Add(bizRelationship.ToOneToManyRelationship(relationship.RelationshipType.OneToManyRelationshipType, relationshipDTO));
                        else if (relationship.RelationshipType.ManyToOneRelationshipType != null)
                            result.ManyToOneRelationships.Add(bizRelationship.ToManyToOneRelationshipDTO(relationship.RelationshipType.ManyToOneRelationshipType, relationshipDTO));
                        else if (relationship.RelationshipType.ImplicitOneToOneRelationshipType != null)
                            result.ImplicitOneToOneRelationships.Add(bizRelationship.ToImplicitOneToOneRelationshipDTO(relationship.RelationshipType.ImplicitOneToOneRelationshipType, relationshipDTO));
                        else if (relationship.RelationshipType.ExplicitOneToOneRelationshipType != null)
                            result.ExplicitOneToOneRelationships.Add(bizRelationship.ToExplicitOneToOneRelationshipDTO(relationship.RelationshipType.ExplicitOneToOneRelationshipType, relationshipDTO));
                        else if (relationship.RelationshipType.SuperToSubRelationshipType != null)
                            result.SuperToSubRelationships.Add(bizISARelationship.ToSuperToSubRelationshipDTO(relationship.RelationshipType.SuperToSubRelationshipType, relationshipDTO));
                        else if (relationship.RelationshipType.SubToSuperRelationshipType != null)
                            result.SubToSuperRelationships.Add(bizISARelationship.ToSubToSuperRelationshipDTO(relationship.RelationshipType.SubToSuperRelationshipType, relationshipDTO));
                        else if (relationship.RelationshipType.UnionToSubUnionRelationshipType != null)
                            result.SuperUnionToSubUnionRelationships.Add(bizUnionRelationship.ToSuperUnionToSubUnionRelationshipDTO(relationship.RelationshipType.UnionToSubUnionRelationshipType, relationshipDTO));
                        else if (relationship.RelationshipType.SubUnionToUnionRelationshipType != null)
                            result.SubUnionToSuperUnionRelationships.Add(bizUnionRelationship.ToSubUnionToSuperUnionRelationshipDTO(relationship.RelationshipType.SubUnionToUnionRelationshipType, relationshipDTO));
                    }
                }
            }

            //CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.Entity, item.ID.ToString(), columnInfoType.ToString(), relationshipInfoType.ToString());
            return result;
        }

        //private string[] GetCacheKeys(EntityColumnInfoType columnInfoType, EntityRelationshipInfoType relationshipInfoType)
        //{
        //    List<string> list = new List<string>();
        //    if(columnInfoType==EntityColumnInfoType.WithFullColumns)
        //    {
        //        list.Add(EntityColumnInfoType.WithoutColumn.ToString());
        //        list.Add(EntityColumnInfoType.WithSimpleColumns.ToString());
        //    }
        //    else if (columnInfoType == EntityColumnInfoType.WithSimpleColumns)
        //    {
        //        list.Add(EntityColumnInfoType.WithoutColumn.ToString());

        //    }
        //}



        //public List<ColumnDTO> GetColumnsSimple(TableDrivedEntity entity)
        //{
        //    using (var projectContext = new MyIdeaEntities())
        //    {
        //        if (entity.Column.Count > 0)
        //        {
        //            return GetColumnsSimple(entity.Column);
        //        }
        //        else
        //        {
        //            return GetColumnsSimple(entity.Table.Column);
        //        }
        //    }
        //}


        public void Save(List<TableDrivedEntityDTO> entities)
        {
            using (var projectContext = new MyIdeaEntities())
            {
                foreach (var entity in entities)
                {
                    var dbEntity = projectContext.TableDrivedEntity.First(x => x.ID == entity.ID);
                    dbEntity.Alias = entity.Alias;
                    dbEntity.Name = entity.Name;
                    dbEntity.Criteria = entity.Criteria;
                    dbEntity.IndependentDataEntry = entity.IndependentDataEntry;
                    dbEntity.BatchDataEntry = entity.BatchDataEntry;
                    dbEntity.IsAssociative = entity.IsAssociative;
                    dbEntity.IsDataReference = entity.IsDataReference;
                    dbEntity.IsStructurReferencee = entity.IsStructurReferencee;
                }
                projectContext.SaveChanges();
            }
        }
        public int Save(TableDrivedEntityDTO baseEntity, TableDrivedEntityDTO drivedEntity, bool inheritance, bool isTolatParticipation, bool isDisjoint)
        {
            using (var projectContext = new MyIdeaEntities())
            {
                foreach (var item in drivedEntity.Relationships)
                {
                    var dbRelationship = projectContext.Relationship.First(x => x.ID == item.ID);
                    Relationship dbReverseRelationship = null;
                    if (dbRelationship.Relationship2 != null)
                        dbReverseRelationship = dbRelationship.Relationship2;
                    else
                        dbReverseRelationship = projectContext.Relationship.First(x => x.RelationshipID == item.ID);

                    if (dbRelationship.TableDrivedEntity.TableID == baseEntity.TableID)
                    {
                        if (dbRelationship.Relationship2 == null)
                            dbRelationship.Name = dbRelationship.Name.Replace("(PK)" + dbRelationship.TableDrivedEntity.Name + ".", "(PK)" + drivedEntity.Name + ".");
                        else
                            dbRelationship.Name = dbRelationship.Name.Replace("(FK)" + dbRelationship.TableDrivedEntity.Name + ".", "(FK)" + drivedEntity.Name + ".");

                        dbRelationship.TableDrivedEntityID1 = drivedEntity.ID;
                    }
                    if (dbRelationship.TableDrivedEntity1.TableID == baseEntity.TableID)
                    {
                        if (dbRelationship.Relationship2 == null)
                            dbRelationship.Name = dbRelationship.Name.Replace("(FK)" + dbRelationship.TableDrivedEntity.Name + ".", "(FK)" + drivedEntity.Name + ".");
                        else
                            dbRelationship.Name = dbRelationship.Name.Replace("(PK)" + dbRelationship.TableDrivedEntity.Name + ".", "(PK)" + drivedEntity.Name + ".");
                        dbRelationship.TableDrivedEntityID2 = drivedEntity.ID;
                    }

                    if (dbReverseRelationship.TableDrivedEntity.TableID == baseEntity.TableID)
                    {
                        if (dbReverseRelationship.Relationship2 == null)
                            dbReverseRelationship.Name = dbReverseRelationship.Name.Replace("(PK)" + dbRelationship.TableDrivedEntity.Name + ".", "(PK)" + drivedEntity.Name + ".");
                        else
                            dbReverseRelationship.Name = dbReverseRelationship.Name.Replace("(FK)" + dbRelationship.TableDrivedEntity.Name + ".", "(FK)" + drivedEntity.Name + ".");
                        dbReverseRelationship.TableDrivedEntityID1 = drivedEntity.ID;
                    }
                    if (dbReverseRelationship.TableDrivedEntity1.TableID == baseEntity.TableID)
                    {
                        if (dbReverseRelationship.Relationship2 == null)
                            dbReverseRelationship.Name = dbReverseRelationship.Name.Replace("(FK)" + dbRelationship.TableDrivedEntity.Name + ".", "(FK)" + drivedEntity.Name + ".");
                        else
                            dbReverseRelationship.Name = dbReverseRelationship.Name.Replace("(PK)" + dbRelationship.TableDrivedEntity.Name + ".", "(PK)" + drivedEntity.Name + ".");
                        dbReverseRelationship.TableDrivedEntityID2 = drivedEntity.ID;
                    }

                }
                if (inheritance && !(drivedEntity.ID == baseEntity.ID))
                {
                    ISARelationship isaRelationship = null;

                    var sampleSuperToSub = projectContext.Relationship.FirstOrDefault(x => x.TableDrivedEntityID1 == baseEntity.ID && x.RelationshipType != null &&
                        x.RelationshipType.SuperToSubRelationshipType != null);
                    if (sampleSuperToSub != null)
                        isaRelationship = sampleSuperToSub.RelationshipType.SuperToSubRelationshipType.ISARelationship;

                    if (isaRelationship == null)
                    {
                        isaRelationship = new ISARelationship();
                    }
                    isaRelationship.IsTolatParticipation = isTolatParticipation;
                    isaRelationship.IsDisjoint = isDisjoint;

                    string subTypesStr = "";
                    foreach (var relationship in isaRelationship.SuperToSubRelationshipType)
                    {
                        if (relationship.RelationshipType.Relationship.TableDrivedEntity1.ID != baseEntity.ID)
                            subTypesStr += (subTypesStr == "" ? "" : ",") + relationship.RelationshipType.Relationship.TableDrivedEntity1.Name;
                    }
                    if (drivedEntity.ID == 0)
                        subTypesStr += (subTypesStr == "" ? "" : ",") + drivedEntity.Name;
                    isaRelationship.Name = baseEntity.Name + ">" + subTypesStr;

                    if (drivedEntity.ID == 0)
                    {
                        var relationship = new Relationship();
                        relationship.RelationshipType = new RelationshipType();
                        relationship.RelationshipType.SuperToSubRelationshipType = new SuperToSubRelationshipType();
                        relationship.RelationshipType.IsOtherSideCreatable = true;
                        relationship.TableDrivedEntityID1 = baseEntity.ID;
                        relationship.RelationshipType.SuperToSubRelationshipType.ISARelationship = isaRelationship;
                        //drivedEntity.Relationship1.Add(relationship);

                        var relationshipReverse = new Relationship();
                        relationshipReverse.RelationshipType = new RelationshipType();
                        relationshipReverse.RelationshipType.SubToSuperRelationshipType = new SubToSuperRelationshipType();
                        relationshipReverse.RelationshipType.IsOtherSideCreatable = true;
                        relationshipReverse.TableDrivedEntityID2 = baseEntity.ID;
                        relationshipReverse.RelationshipType.SubToSuperRelationshipType.ISARelationship = isaRelationship;
                        relationshipReverse.Relationship2 = relationship;
                        //drivedEntity.Relationship.Add(relationshipReverse);

                        var dbBaseEntity = projectContext.TableDrivedEntity.First(x => x.ID == baseEntity.ID);
                        string PKColumns = "";
                        string FKColumns = "";
                        foreach (var primaryCol in dbBaseEntity.Table.Column.Where(x => x.PrimaryKey == true))
                        {
                            PKColumns += (PKColumns == "" ? "" : ",") + primaryCol.Name;
                            FKColumns += (FKColumns == "" ? "" : ",") + primaryCol.Name;
                            relationship.RelationshipColumns.Add(new RelationshipColumns() { Column = primaryCol, Column1 = primaryCol });
                            relationshipReverse.RelationshipColumns.Add(new RelationshipColumns() { Column = primaryCol, Column1 = primaryCol });
                        }
                        relationship.Name = "(PK)" + baseEntity.Name + "." + PKColumns + ">(FK)" + drivedEntity.Name + "." + FKColumns;
                        relationshipReverse.Name = "(FK)" + drivedEntity.Name + "." + FKColumns + ">(PK)" + baseEntity.Name + "." + PKColumns;

                    }

                }
                //if (drivedEntity.ID == 0)
                //    projectContext.TableDrivedEntity.Add(drivedEntity);
                projectContext.SaveChanges();
                return 0;
            }
        }


        //public event EventHandler<SimpleGenerationInfoArg> RuleImposedEvent;
      

    }


}
