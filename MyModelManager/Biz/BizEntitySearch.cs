
using DataAccess;
using ModelEntites;
using MyCacheManager;
using MyGeneralLibrary;

using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizEntitySearch
    {
        SecurityHelper securityHelper = new SecurityHelper();

        public event EventHandler<ItemImportingStartedArg> ItemImportingStarted;
        BizEntityRelationshipTail bizEntityRelationshipTail = new MyModelManager.BizEntityRelationshipTail();
        public List<EntitySearchDTO> GetEntitySearchs(DR_Requester requester, int entityID)
        {
            //var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.Validation, entityID.ToString());
            //if (cachedItem != null)
            //    return (cachedItem as List<EntitySearchDTO>);

            List<EntitySearchDTO> result = new List<EntitySearchDTO>();
            using (var projectContext = new MyProjectEntities())
            {
                var listEntitySearch = projectContext.EntitySearch.Where(x => x.TableDrivedEntityID == entityID);
                foreach (var item in listEntitySearch)
                    if (DataIsAccessable(requester, item))
                        result.Add(ToEntitySearchDTO(requester, item, false));

            }
            //CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.Validation, entityID.ToString());
            return result;
        }

        public EntitySearchDTO GetDefaultEntitySearch(DR_Requester requester, int entityID)
        {
            EntitySearchDTO result = null;
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var entity = projectContext.TableDrivedEntity.First(x => x.ID == entityID);
                if (entity.EntitySearch != null)
                    if (DataIsAccessable(requester, entity.EntitySearch))
                        result = ToEntitySearchDTO(requester, entity.EntitySearch, true);
                    else
                        return null;
                else
                {
                    var defaultListView = entity.EntitySearch1.FirstOrDefault();
                    if (defaultListView != null)
                    {
                        if (DataIsAccessable(requester, defaultListView))
                            result = ToEntitySearchDTO(requester, defaultListView, true);
                        else
                            return null;
                    }
                    else
                    {
                        //باید یک دیفالت ساخته و فرستاده شه
                        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
                        var entityDTO = bizTableDrivedEntity.GetPermissionedEntity(requester, entityID);
                        result = ToEntitySimpleSearch(entityDTO);
                    }
                }
            }
            return result;
        }
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        private bool DataIsAccessable(DR_Requester requester, EntitySearch entitySearch)
        {
            if (requester.SkipSecurity)
                return true;

            return bizTableDrivedEntity.DataIsAccessable(requester, entitySearch.TableDrivedEntity1);

        }
        private bool ImposeSecurity(DR_Requester requester, EntitySearchDTO entitySearchDTO, TableDrivedEntity entity)
        {
            BizColumn bizColumn = new BizColumn();

            if (requester.SkipSecurity)
                return true;

            if (!bizTableDrivedEntity.DataIsAccessable(requester, entity))
            {
                return false;
            }
            var permission = bizTableDrivedEntity.GetEntityAssignedPermissions(requester, entitySearchDTO.TableDrivedEntityID, true);

            List<EntitySearchColumnsDTO> removeList = new List<ModelEntites.EntitySearchColumnsDTO>();
            foreach (var columnGroup in entitySearchDTO.EntitySearchAllColumns.GroupBy(x => x.RelationshipTailID))
            {
                bool pathPermission = true;
                if (columnGroup.Key == 0)
                {
                    pathPermission = true;
                }
                else
                {
                    var relationshipTail = columnGroup.First(x => x.RelationshipTailID == columnGroup.Key).RelationshipTail;
                    pathPermission = bizEntityRelationshipTail.DataIsAccessable(requester, relationshipTail);
                }
                if (!pathPermission)
                {
                    foreach (var column in columnGroup)
                    {
                        removeList.Add(column);
                    }
                }
                else
                {
                    foreach (var column in columnGroup)
                    {
                        if (column.ColumnID != 0 && !bizColumn.DataIsAccessable(requester, column.ColumnID))
                            removeList.Add(column);
                    }
                }
            }
            foreach (var remove in removeList)
            {
                entitySearchDTO.EntitySearchAllColumns.Remove(remove);
            }
            return true;

        }
        private EntitySearchDTO ToEntitySimpleSearch(TableDrivedEntityDTO entityDTO)
        {
            EntitySearchDTO result = new EntitySearchDTO();
            result.TableDrivedEntityID = entityDTO.ID;
            result.ID = 0;
            result.Title = "ستونهای ساخته شده";
            foreach (var column in entityDTO.Columns)
            {
                EntitySearchColumnsDTO rColumn = new EntitySearchColumnsDTO();
                rColumn.ID = 0;
                rColumn.ColumnID = column.ID;
                rColumn.Column = column;
                rColumn.Alias = column.Alias;
                rColumn.OrderID = (short)column.Position;
                result.EntitySearchAllColumns.Add(rColumn);
            }
            return result;
        }
        //public EntitySearchDTO GetEntityDefaultSearch(int entityID)
        //{
        //    List<EntitySearchDTO> result = new List<EntitySearchDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var entity = projectContext.TableDrivedEntity.First(x => x.ID == entityID);
        //        var defaultSearch = entity.EntitySearch.FirstOrDefault();
        //        if (defaultSearch != null)
        //            return ToEntitySearchDTO(defaultSearch, true);
        //        else
        //            return null;
        //    }
        //}

        public EntitySearchDTO GetEntitySearch(DR_Requester requester, int EntitySearchsID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var EntitySearchs = projectContext.EntitySearch.First(x => x.ID == EntitySearchsID);
                if (DataIsAccessable(requester, EntitySearchs))
                {
                    var result = ToEntitySearchDTO(requester, EntitySearchs, true);
                    return result;
                }
                else
                    return null;
            }

        }

        public EntitySearchDTO ToEntitySearchDTO(DR_Requester requester, EntitySearch item, bool withDetails)
        {
            EntitySearchDTO result = new EntitySearchDTO();
            result.TableDrivedEntityID = item.TableDrivedEntityID;
            result.ID = item.ID;
            result.Title = item.Title;
            BizEntityRelationshipTail bizEntityRelationshipTail = new MyModelManager.BizEntityRelationshipTail();
            if (withDetails)
            {
                BizColumn bizColumn = new MyModelManager.BizColumn();
                foreach (var column in item.EntitySearchColumns)
                {
                    EntitySearchColumnsDTO rColumn = new EntitySearchColumnsDTO();
                    rColumn.ID = column.ID;
                    rColumn.ColumnID = column.ColumnID ?? 0;
                    if (column.Column != null)
                        rColumn.Column = bizColumn.ToColumnDTO(column.Column, true);
                    if (column.ColumnID != null)
                        rColumn.Alias = column.Alias ?? column.Column.Alias ?? column.Column.Name;
                    else
                        rColumn.Alias = column.Alias ?? column.EntityRelationshipTail.TableDrivedEntity.Alias ?? column.EntityRelationshipTail.TableDrivedEntity.Name;
                    rColumn.OrderID = column.OrderID ?? 0;
                    //rColumn.WidthUnit = column.WidthUnit ?? 0;
                    if (column.EntityRelationshipTailID != null)
                    {
                        rColumn.RelationshipTailID = column.EntityRelationshipTailID.Value;
                        rColumn.RelationshipTail = bizEntityRelationshipTail.ToEntityRelationshipTailDTO(column.EntityRelationshipTail);
                    }
                    result.EntitySearchAllColumns.Add(rColumn);
                }
                //foreach (var tail in item.EntitySearchRelationshipTails)
                //{
                //    EntitySearchRelationshipTailDTO rTail = new EntitySearchRelationshipTailDTO();
                //    rTail.ID = tail.ID;
                //    rTail.EntityRelationshipTailID = tail.EntityRelationshipTailID;
                //    rTail.EntityRelationshipTail = bizEntityRelationshipTail.ToEntityRelationshipTailDTO(tail.EntityRelationshipTail);
                //    foreach (var tailColumn in tail.EntitySearchColumns)
                //    {
                //        rTail.EntitySearchColumns.Add(result.EntitySearchAllColumns.First(x => x.ID == tailColumn.ID));
                //    }
                //    result.EntitySearchRelationshipTails.Add(rTail);
                //}
            }
            ImposeSecurity(requester, result, item.TableDrivedEntity1);
            return result;
        }

        //public void UpdateDefaultSearchInModel(int databaseID)
        //{

        //}
        public EntitySearchDTO GenerateDefaultSearchList(TableDrivedEntityDTO entity, List<TableDrivedEntityDTO> allEntities)
        {
            EntitySearchDTO result = new EntitySearchDTO();
            result.TableDrivedEntityID = entity.ID;
            result.Title = "لیست جستجوی پیش فرض";
            result.EntitySearchAllColumns = GenereateDefaultSearchColumns(entity, null, allEntities);

            return result;
        }
        private List<EntitySearchColumnsDTO> GenereateDefaultSearchColumns(TableDrivedEntityDTO sententity, RelationshipDTO relationship, List<TableDrivedEntityDTO> allEntities, string relationshipPath = "", List<EntitySearchColumnsDTO> list = null)
        {
            if (list == null)
                list = new List<EntitySearchColumnsDTO>();
            TableDrivedEntityDTO entity = null;
            List<ColumnDTO> simplecollumns = null;
            if (sententity != null)
            {
                entity = sententity;
                simplecollumns = GetSimpleListViewColumns(entity);
                foreach (var column in entity.Columns.Where(x => x.PrimaryKey))
                {
                    var resultColumn = new EntitySearchColumnsDTO();
                    resultColumn.ColumnID = column.ID;
                    resultColumn.Alias = column.Alias;
                    list.Add(resultColumn);
                }
            }
            else if (relationship != null)
            {
                entity = allEntities.First(x => x.ID == relationship.EntityID2);
                simplecollumns = GetRelationColumns(entity);
            }
            var reviewedRels = new List<RelationshipDTO>();
            foreach (var column in entity.Columns)
            {
                if (simplecollumns.Any(x => x.ID == column.ID))
                {
                    var resultColumn = new EntitySearchColumnsDTO();
                    resultColumn.ColumnID = column.ID;
                    resultColumn.CreateRelationshipTailPath = relationshipPath;
                    resultColumn.Alias = (relationship == null ? "" : entity.Alias + ".") + column.Alias;
                    list.Add(resultColumn);
                }
                else
                {
                    if (entity.Relationships.Any(z => z.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary && z.RelationshipColumns.Any(y => y.FirstSideColumnID == column.ID)))
                    {
                        var newrelationship = entity.Relationships.First(z => z.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary && z.RelationshipColumns.Any(y => y.FirstSideColumnID == column.ID));
                        if (!reviewedRels.Any(x => x.ID == newrelationship.ID))
                        {
                            reviewedRels.Add(newrelationship);


                            if (newrelationship.TypeEnum == Enum_RelationshipType.SubToSuper)
                            {
                                if (!newrelationship.AllForeignKeysArePrimaryKey)
                                {
                                    foreach (var relCol in newrelationship.RelationshipColumns)
                                    {
                                        var resultColumn = new EntitySearchColumnsDTO();
                                        resultColumn.ColumnID = relCol.FirstSideColumnID;
                                        resultColumn.CreateRelationshipTailPath = relationshipPath;
                                        resultColumn.Alias = (relationship == null ? "" : entity.Alias + ".") + relCol.FirstSideColumn.Alias;
                                        list.Add(resultColumn);
                                    }
                                }
                                GenereateDefaultSearchColumns(null, newrelationship, allEntities, relationshipPath + (relationshipPath == "" ? "" : ",") + newrelationship.ID.ToString(), list);
                            }
                            else if (relationship == null)
                            {
                                var resultColumn = new EntitySearchColumnsDTO();
                                resultColumn.CreateRelationshipTailPath = relationshipPath + (relationshipPath == "" ? "" : ",") + newrelationship.ID.ToString();
                                resultColumn.Alias = (relationship == null ? "" : entity.Alias + ".") + column.Alias;
                                list.Add(resultColumn);
                            }


                        }
                    }
                }
            }
            if (sententity != null)
            {
                short index = 0;
                foreach (var item in list)
                {
                    item.OrderID = index;
                    index++;
                }

            }
            return list;
        }
        private List<ColumnDTO> GetSimpleListViewColumns(TableDrivedEntityDTO entity)
        {
            var simplecollumns = entity.Columns.Where(x => !x.PrimaryKey && !entity.Relationships.Any(z => z.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary && z.RelationshipColumns.Any(y => y.FirstSideColumnID == x.ID))).ToList();
            var countLimit = simplecollumns.Count(x => x.ColumnType == Enum_ColumnType.String || x.ColumnType == Enum_ColumnType.Date);
            List<ColumnDTO> selectedColumns = null;
            selectedColumns = GetFirstPriorityColumns(simplecollumns);
            if (selectedColumns.Count < countLimit / 3)
                selectedColumns = GetSecondPriorityColumns(simplecollumns, selectedColumns);
            if (selectedColumns.Count < countLimit / 3)
                selectedColumns = GetThirdPriorityColumns(simplecollumns, selectedColumns);
            return selectedColumns;
        }

        //public EntitySearchDTO GenerateDefaultSearchList(TableDrivedEntityDTO entity, List<TableDrivedEntityDTO> allEntities)
        //{


        //    short index = 0;
        //    foreach (var column in entity.Columns.Where(x => x.PrimaryKey))
        //    {
        //        var resultColumn = new EntitySearchColumnsDTO();
        //        resultColumn.OrderID = index;
        //        resultColumn.ColumnID = column.ID;
        //        result.EntitySearchAllColumns.Add(resultColumn);
        //        index++;
        //    }
        //    var reviewedRels = new List<RelationshipDTO>();
        //    foreach (var column in entity.Columns)
        //    {
        //        if (selectedColumns.Any(x => x.ID == column.ID))
        //        {
        //            var resultColumn = new EntitySearchColumnsDTO();
        //            resultColumn.OrderID = index;
        //            resultColumn.ColumnID = column.ID;
        //            result.EntitySearchAllColumns.Add(resultColumn);
        //            index++;
        //        }
        //        else
        //        {
        //            if (entity.Relationships.Any(z => z.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary && z.RelationshipColumns.Any(y => y.FirstSideColumnID == column.ID)))
        //            {

        //                var relationship = entity.Relationships.First(z => z.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary && z.RelationshipColumns.Any(y => y.FirstSideColumnID == column.ID));
        //                if (!reviewedRels.Any(x => x.ID == relationship.ID))
        //                {
        //                    reviewedRels.Add(relationship);
        //                    //foreach (var relCol in relationship.RelationshipColumns)
        //                    //{
        //                    //    var resultColumn = new EntityListViewColumnsDTO();
        //                    //    resultColumn.OrderID = index;
        //                    //    resultColumn.ColumnID = column.ID;
        //                    //    result.EntityListViewAllColumns.Add(resultColumn);
        //                    //    index++;
        //                    //}
        //                    var targetEntity = allEntities.First(x => x.ID == relationship.EntityID2);
        //                    //List<ColumnDTO> relationColumns = GetRelationColumns(entity, targetEntity, relationship);
        //                    //if (relationColumns.Any())
        //                    //{
        //                    //foreach (var relCol in relationColumns)
        //                    //{
        //                    var resultColumn = new EntitySearchColumnsDTO();
        //                    resultColumn.OrderID = index;
        //                    //   resultColumn.ColumnID = relCol.ID;
        //                    resultColumn.Alias = targetEntity.Alias;
        //                    resultColumn.CreateRelationshipID = relationship.ID;
        //                    resultColumn.CreateRelationshipTargetEntityID = relationship.EntityID2;
        //                    result.EntitySearchAllColumns.Add(resultColumn);
        //                    index++;
        //                    //}
        //                    //}
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}

        private List<ColumnDTO> GetRelationColumns(TableDrivedEntityDTO targetEntity)
        {
            List<ColumnDTO> result = new List<ColumnDTO>();
            var simplecollumns = targetEntity.Columns.Where(x => !x.PrimaryKey && !targetEntity.Relationships.Any(z => z.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary && z.RelationshipColumns.Any(y => y.FirstSideColumnID == x.ID))).ToList();
            return GetFirstPriorityColumns(simplecollumns);
        }

        private List<ColumnDTO> GetFirstPriorityColumns(List<ColumnDTO> columns)
        {
            var indexer = 0;
            List<ColumnDTO> result = new List<ColumnDTO>();
            foreach (var column in columns)
            {
                var toLowwer = column.Name.ToLower();
                if (toLowwer.Contains("name") || toLowwer.Contains("firstname") || toLowwer.Contains("lastname") || toLowwer.Contains("title") || toLowwer.Contains("number")
                      || toLowwer.Contains("family"))
                    result.Add(column);
                else
                {
                    if (toLowwer.Contains("date") && indexer < columns.Count / 2)
                        result.Add(column);
                }
                indexer++;
            }
            return result;
        }
        private List<ColumnDTO> GetSecondPriorityColumns(List<ColumnDTO> columns, List<ColumnDTO> alreadyColumns)
        {
            var indexer = 0;
            List<ColumnDTO> result = new List<ColumnDTO>();
            foreach (var column in columns)
            {
                if (alreadyColumns.Any(x => x.ID == column.ID))
                    result.Add(column);
                else if ((column.ColumnType == Enum_ColumnType.String || column.ColumnType == Enum_ColumnType.Numeric) && indexer <= columns.Count / 2)
                    result.Add(column);
                indexer++;
            }
            return result;
        }
        private List<ColumnDTO> GetThirdPriorityColumns(List<ColumnDTO> columns, List<ColumnDTO> alreadyColumns)
        {
            var indexer = 0;
            List<ColumnDTO> result = new List<ColumnDTO>();
            foreach (var column in columns)
            {
                if (alreadyColumns.Any(x => x.ID == column.ID))
                    result.Add(column);
                else if (indexer <= columns.Count / 2)
                    result.Add(column);
                indexer++;
            }
            return result;
        }
        public EntitySearch SaveItem(MyProjectEntities projectContext, EntitySearchDTO message)
        {
            var dbEntitySearch = projectContext.EntitySearch.FirstOrDefault(x => x.ID == message.ID);
            if (dbEntitySearch == null)
            {
                dbEntitySearch = new DataAccess.EntitySearch();


            }
            dbEntitySearch.TableDrivedEntityID = message.TableDrivedEntityID;
            dbEntitySearch.Title = message.Title;

            //تیلهای گزارش را از روی تیلهای ستونها میسازد
            //هر دفعه پاک نشن بهتره..اصلاح بشن
            while (dbEntitySearch.EntitySearchColumns.Any())
                projectContext.EntitySearchColumns.Remove(dbEntitySearch.EntitySearchColumns.First());
            //while (dbEntitySearch.EntitySearchRelationshipTails.Any())
            //    projectContext.EntitySearchRelationshipTails.Remove(dbEntitySearch.EntitySearchRelationshipTails.First());
            List<EntityRelationshipTail> relationshipTails = new List<EntityRelationshipTail>();
            BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
            foreach (var column in message.EntitySearchAllColumns)
            {
                EntitySearchColumns rColumn = new EntitySearchColumns();
                if (column.ColumnID != 0)
                    rColumn.ColumnID = column.ColumnID;
                else
                    rColumn.ColumnID = null;
                rColumn.Alias = column.Alias;
                rColumn.OrderID = column.OrderID;
                // rColumn.WidthUnit = column.WidthUnit;

                if (string.IsNullOrEmpty(column.CreateRelationshipTailPath))
                    rColumn.EntityRelationshipTailID = column.RelationshipTailID == 0 ? (int?)null : column.RelationshipTailID;
                else
                {
                    if (relationshipTails.Any(x => x.TableDrivedEntityID == message.TableDrivedEntityID && x.RelationshipPath == column.CreateRelationshipTailPath))
                        rColumn.EntityRelationshipTail = relationshipTails.First(x => x.TableDrivedEntityID == message.TableDrivedEntityID && x.RelationshipPath == column.CreateRelationshipTailPath);
                    else
                    {
                        var relationshipTail = bizEntityRelationshipTail.GetOrCreateEntityRelationshipTail(projectContext, message.TableDrivedEntityID, column.CreateRelationshipTailPath);
                        relationshipTails.Add(relationshipTail);
                        rColumn.EntityRelationshipTail = relationshipTail;
                    }
                }


                //if (column.RelationshipTailID != 0 || !string.IsNullOrEmpty(column.RelationshipPath))
                //{
                //    int tailID = 0;
                //    if (column.RelationshipTailID != 0)
                //        tailID = column.RelationshipTailID;
                //    else
                //        tailID = bizEntityRelationshipTail.GetOrCreateEntityRelationshipTail(message.TableDrivedEntityID, column.RelationshipPath);
                //    //var relatedListReportTail = dbEntitySearch.EntitySearchRelationshipTails.FirstOrDefault(x => x.EntityRelationshipTailID == tailID);
                //    //if (relatedListReportTail == null)
                //    //{
                //    //    relatedListReportTail = new EntitySearchRelationshipTails();
                //    //    relatedListReportTail.EntityRelationshipTailID = tailID;
                //    //    dbEntitySearch.EntitySearchRelationshipTails.Add(relatedListReportTail);
                //    //}

                //    //rColumn.EntitySearchRelationshipTails = relatedListReportTail;
                //}
                dbEntitySearch.EntitySearchColumns.Add(rColumn);
            }

            if (dbEntitySearch.ID == 0)
                projectContext.EntitySearch.Add(dbEntitySearch);
            return dbEntitySearch;
        }
        public int UpdateEntitySearchs(EntitySearchDTO message)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbItem = SaveItem(projectContext, message);
                projectContext.SaveChanges();
                return dbItem.ID;
            }
        }
    }

}
