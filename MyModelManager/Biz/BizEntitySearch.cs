
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
                    if (!string.IsNullOrEmpty(column.Tooltip))
                        rColumn.Tooltip = column.Tooltip;
                    else
                    {
                        if (rColumn.RelationshipTail != null && rColumn.Column != null)
                            rColumn.Tooltip = rColumn.RelationshipTail.ReverseRelationshipTail.TargetEntityAlias + "." + rColumn.Column.Alias;
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
            result.EntitySearchAllColumns = GenereateDefaultSearchColumns(entity, allEntities);

            return result;
        }
        private List<EntitySearchColumnsDTO> GenereateDefaultSearchColumns(TableDrivedEntityDTO entity, List<TableDrivedEntityDTO> allEntities, List<EntitySearchColumnsDTO> list = null)
        {
            if (list == null)
                list = new List<EntitySearchColumnsDTO>();
            foreach (var column in entity.Columns.Where(x => x.PrimaryKey))
            {
                AddSearchColumn(list, column);
            }
            var simplecollumns = GetSimpleSearchColumns(entity);
            foreach (var column in simplecollumns)
            {
                AddSearchColumn(list, column);
            }
            AddRelationshipDefaultColumns(entity, allEntities, list);
            if (entity != null)
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
        private void AddRelationshipDefaultColumns(TableDrivedEntityDTO entity, List<TableDrivedEntityDTO> allEntities, List<EntitySearchColumnsDTO> list, string relationshipPath = "", List<RelationshipDTO> relationships = null)
        {
            var reviewedFKRels = new List<RelationshipDTO>();
            foreach (var column in entity.Columns)
            {
                if (entity.Relationships.Any(z => z.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary && z.RelationshipColumns.Any(y => y.FirstSideColumnID == column.ID)))
                {
                    var newrelationship = entity.Relationships.First(z => z.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary && z.RelationshipColumns.Any(y => y.FirstSideColumnID == column.ID));
                    if (newrelationship.TypeEnum == Enum_RelationshipType.SubToSuper
                        || newrelationship.TypeEnum == Enum_RelationshipType.UnionToSubUnion)
                    {
                        if (!reviewedFKRels.Any(x => x.ID == newrelationship.ID))
                        {
                            reviewedFKRels.Add(newrelationship);
                            //جلوگیری از لوپ
                            if (relationships == null || !relationships.Any(x => x.ID == newrelationship.ID))
                            {
                                int isaID = 0;
                                int uinonID = 0;
                                if (newrelationship.TypeEnum == Enum_RelationshipType.SubToSuper)
                                {
                                    isaID = (newrelationship as SubToSuperRelationshipDTO).ISARelationship.ID;
                                }
                                else if (newrelationship.TypeEnum == Enum_RelationshipType.UnionToSubUnion)
                                {
                                    uinonID = (newrelationship as UnionToSubUnionRelationshipDTO).UnionRelationship.ID;
                                }
                                if ((isaID == 0 || isaID != GetLastISAID(relationships)) &&
                                 (uinonID == 0 || isaID != GetLastUnionID(relationships)))
                                {
                                    List<RelationshipDTO> relationshipsTail = new List<RelationshipDTO>();
                                    if (relationships != null)
                                    {
                                        foreach (var relItem in relationships)
                                            relationshipsTail.Add(relItem);
                                    }
                                    relationshipsTail.Add(newrelationship);
                                    //کلید های خارجی موجودیت های دیگر مهم نیستند
                                    foreach (var relCol in newrelationship.RelationshipColumns)
                                    {
                                        if (!relCol.FirstSideColumn.PrimaryKey)
                                        {
                                            AddSearchColumn(list, relCol.FirstSideColumn, relationshipPath, relationships);
                                        }
                                    }
                                    GenereateDefaultSearchColumnsFromRelationship(newrelationship, allEntities, list, relationshipPath + (relationshipPath == "" ? "" : ",") + newrelationship.ID.ToString(), relationshipsTail);
                                }
                            }
                        }
                    }
                    else if (relationships == null)
                    {
                        var resultColumn = new EntitySearchColumnsDTO();
                        resultColumn.CreateRelationshipTailPath = relationshipPath + (relationshipPath == "" ? "" : ",") + newrelationship.ID.ToString();
                        resultColumn.Alias = newrelationship.Entity2Alias;
                        list.Add(resultColumn);
                    }
                }
            }

            foreach (var newrelationship in entity.Relationships.Where(x => x.MastertTypeEnum == Enum_MasterRelationshipType.FromPrimartyToForeign))
            {
                if (newrelationship.TypeEnum == Enum_RelationshipType.SubUnionToUnion
                    || (newrelationship.TypeEnum == Enum_RelationshipType.SuperToSub && (newrelationship as SuperToSubRelationshipDTO).ISARelationship.IsTolatParticipation))
                {
                    //جلوگیری از لوپ
                    if (relationships == null || !relationships.Any(x => x.ID == newrelationship.ID))
                    {
                        int isaID = 0;
                        int uinonID = 0;
                        if (newrelationship.TypeEnum == Enum_RelationshipType.SuperToSub)
                        {
                            isaID = (newrelationship as SuperToSubRelationshipDTO).ISARelationship.ID;
                        }
                        else if (newrelationship.TypeEnum == Enum_RelationshipType.SubUnionToUnion)
                        {
                            uinonID = (newrelationship as SubUnionToSuperUnionRelationshipDTO).UnionRelationship.ID;
                        }
                        if ((isaID == 0 || isaID != GetLastISAID(relationships)) &&
                            (uinonID == 0 || isaID != GetLastUnionID(relationships)))
                        {
                            List<RelationshipDTO> relationshipsTail = new List<RelationshipDTO>();
                            if (relationships != null)
                            {
                                foreach (var relItem in relationships)
                                    relationshipsTail.Add(relItem);
                            }
                            relationshipsTail.Add(newrelationship);
                            GenereateDefaultSearchColumnsFromRelationship(newrelationship, allEntities, list, relationshipPath + (relationshipPath == "" ? "" : ",") + newrelationship.ID.ToString(), relationshipsTail);
                        }
                    }
                }
            }
        }
        private int GetLastUnionID(List<RelationshipDTO> relationships)
        {
            if (relationships == null || relationships.Count == 0)
                return 0;
            else
            {
                var lastRel = relationships.Last();
                if (lastRel.TypeEnum == Enum_RelationshipType.UnionToSubUnion)
                {
                    return (lastRel as UnionToSubUnionRelationshipDTO).UnionRelationship.ID;
                }
                else if (lastRel.TypeEnum == Enum_RelationshipType.SubUnionToUnion)
                {
                    return (lastRel as SubUnionToSuperUnionRelationshipDTO).UnionRelationship.ID;
                }
                else
                    return 0;
            }
        }

        private int GetLastISAID(List<RelationshipDTO> relationships)
        {
            if (relationships == null || relationships.Count == 0)
                return 0;
            else
            {
                var lastRel = relationships.Last();
                if (lastRel.TypeEnum == Enum_RelationshipType.SubToSuper)
                {
                    return (lastRel as SubToSuperRelationshipDTO).ISARelationship.ID;
                }
                else if (lastRel.TypeEnum == Enum_RelationshipType.SuperToSub)
                {
                    return (lastRel as SuperToSubRelationshipDTO).ISARelationship.ID;
                }
                else
                    return 0;
            }
        }

        private void GenereateDefaultSearchColumnsFromRelationship(RelationshipDTO relationship, List<TableDrivedEntityDTO> allEntities, List<EntitySearchColumnsDTO> list, string relationshipPath, List<RelationshipDTO> relationships)
        {

            TableDrivedEntityDTO entity = allEntities.First(x => x.ID == relationship.EntityID2);
            var skipRelColumnIDs = relationship.RelationshipColumns.Select(x => x.SecondSideColumn.ID).ToList();
            foreach (var column in entity.Columns.Where(x => x.PrimaryKey && !skipRelColumnIDs.Contains(x.ID)))
            {
                AddSearchColumn(list, column);
            }
            List<ColumnDTO> simplecollumns = GetSimpleSearchColumns(entity);
            foreach (var column in simplecollumns)
            {
                AddSearchColumn(list, column, relationshipPath, relationships);
            }
            AddRelationshipDefaultColumns(entity, allEntities, list, relationshipPath, relationships);
        }
        private void AddSearchColumn(List<EntitySearchColumnsDTO> list, ColumnDTO column, string relationshipPath = null, List<RelationshipDTO> relationships = null)
        {
            var resultColumn = new EntitySearchColumnsDTO();
            resultColumn.ColumnID = column.ID;
            resultColumn.Column = column;
            resultColumn.CreateRelationshipTailPath = relationshipPath;
            resultColumn.AllRelationshipsAreSubToSuper = relationships != null && relationships.All(x => x.TypeEnum == Enum_RelationshipType.SubToSuper || x.TypeEnum == Enum_RelationshipType.SubUnionToUnion);
            string entityAlias = "";
            if (relationships == null || relationships.Count == 0)
                entityAlias = "";
            else if (resultColumn.AllRelationshipsAreSubToSuper)
                entityAlias = "";
            else
            {
                var firstNotISAOrUnionEntity = GetLastNotISAOrUnionRelationship(relationships);
                if (firstNotISAOrUnionEntity != null)
                    entityAlias = firstNotISAOrUnionEntity.Entity2Alias + ".";
            }
            resultColumn.Alias = entityAlias + column.Alias;
            list.Add(resultColumn);
        }

        private RelationshipDTO GetLastNotISAOrUnionRelationship(List<RelationshipDTO> relationships)
        {
            var list = relationships.ToList();
            list.Reverse();
            foreach (var rel in list)
            {
                if (rel.TypeEnum != Enum_RelationshipType.SubToSuper && rel.TypeEnum != Enum_RelationshipType.SubUnionToUnion)
                    return rel;
            }
            return null;
        }



        //private List<EntitySearchColumnsDTO> GenereateDefaultSearchColumns(TableDrivedEntityDTO sententity, RelationshipDTO relationship, List<TableDrivedEntityDTO> allEntities, string relationshipPath = "", List<RelationshipDTO> relationships = null, List<EntitySearchColumnsDTO> list = null)
        //{
        //    if (list == null)
        //        list = new List<EntitySearchColumnsDTO>();

        //    TableDrivedEntityDTO entity = null;
        //    List<ColumnDTO> simplecollumns = null;
        //    if (sententity != null)
        //    {
        //        entity = sententity;
        //        simplecollumns = GetEntitySimpleColumnsColumns(entity);
        //        foreach (var column in entity.Columns.Where(x => x.PrimaryKey))
        //        {
        //            var resultColumn = new EntitySearchColumnsDTO();
        //            resultColumn.ColumnID = column.ID;
        //            resultColumn.Alias = column.Alias;
        //            list.Add(resultColumn);
        //        }
        //    }
        //    else if (relationship != null)
        //    {
        //        entity = allEntities.First(x => x.ID == relationship.EntityID2);
        //        simplecollumns = GetRelationColumns(entity);
        //    }
        //    var reviewedRels = new List<RelationshipDTO>();
        //    foreach (var column in entity.Columns)
        //    {
        //        if (simplecollumns.Any(x => x.ID == column.ID))
        //        {
        //            var resultColumn = new EntitySearchColumnsDTO();
        //            resultColumn.ColumnID = column.ID;
        //            resultColumn.CreateRelationshipTailPath = relationshipPath;
        //            resultColumn.AllRelationshipsAreSubTuSuper = relationships != null && relationships.All(x => x.TypeEnum == Enum_RelationshipType.SubToSuper);
        //            resultColumn.Alias = (relationship == null || resultColumn.AllRelationshipsAreSubTuSuper ? "" : entity.Alias + ".") + column.Alias;
        //            //resultColumn.Tooltip = relationship == null ? "" : entity.Alias + "." + column.Alias;
        //            list.Add(resultColumn);
        //        }
        //        else
        //        {
        //            if (entity.Relationships.Any(z => z.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary && z.RelationshipColumns.Any(y => y.FirstSideColumnID == column.ID)))
        //            {
        //                var newrelationship = entity.Relationships.First(z => z.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary && z.RelationshipColumns.Any(y => y.FirstSideColumnID == column.ID));
        //                if (!reviewedRels.Any(x => x.ID == newrelationship.ID))
        //                {
        //                    reviewedRels.Add(newrelationship);
        //                    if (newrelationship.TypeEnum == Enum_RelationshipType.SubToSuper)
        //                    {
        //                        //کلید های خارجی موجودیت های دیگر مهم نیستند
        //                        if (sententity != null)
        //                        {
        //                            foreach (var relCol in newrelationship.RelationshipColumns)
        //                            {
        //                                bool fkIsValid = false;
        //                                if (sententity == null)
        //                                    fkIsValid = true;
        //                                else
        //                                {     //چون برای انتیتی اصلی پرایمری ها قبلا اضافه شده اند
        //                                    fkIsValid = !relCol.FirstSideColumn.PrimaryKey;
        //                                }
        //                                if (fkIsValid)
        //                                {
        //                                    var resultColumn = new EntitySearchColumnsDTO();
        //                                    resultColumn.ColumnID = relCol.FirstSideColumnID;
        //                                    resultColumn.CreateRelationshipTailPath = relationshipPath;
        //                                    string entityAlias = "";
        //                                    if (relationship != null)
        //                                    {
        //                                        entityAlias = entity.Alias + ".";
        //                                    }
        //                                    resultColumn.Alias = entityAlias + relCol.FirstSideColumn.Alias;
        //                                    //resultColumn.Tooltip = relationship == null ? "" : entity.Alias + "." + relCol.FirstSideColumn.Alias;
        //                                    list.Add(resultColumn);
        //                                }
        //                            }
        //                        }
        //                        List<RelationshipDTO> relationshipsTail = new List<RelationshipDTO>();
        //                        if (relationships != null)
        //                        {
        //                            foreach (var relItem in relationships)
        //                                relationshipsTail.Add(relItem);

        //                        }
        //                        relationshipsTail.Add(newrelationship);
        //                        GenereateDefaultSearchColumns(null, newrelationship, allEntities, relationshipPath + (relationshipPath == "" ? "" : ",") + newrelationship.ID.ToString(), relationshipsTail, list);
        //                    }
        //                    else if (relationship == null)
        //                    {
        //                        var resultColumn = new EntitySearchColumnsDTO();
        //                        resultColumn.CreateRelationshipTailPath = relationshipPath + (relationshipPath == "" ? "" : ",") + newrelationship.ID.ToString();
        //                        resultColumn.Alias = newrelationship.Entity2Alias;
        //                        list.Add(resultColumn);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    foreach (var newrelationship in entity.Relationships.Where(x => x.MastertTypeEnum == Enum_MasterRelationshipType.FromPrimartyToForeign))
        //    {
        //        if (newrelationship.TypeEnum == Enum_RelationshipType.SubUnionToUnion_UnionHoldsKeys
        //            || (newrelationship.TypeEnum == Enum_RelationshipType.SuperToSub && (newrelationship as SuperToSubRelationshipDTO).ISARelationship.IsTolatParticipation))
        //        {
        //            List<RelationshipDTO> relationshipsTail = new List<RelationshipDTO>();
        //            if (relationships != null)
        //            {
        //                foreach (var relItem in relationships)
        //                    relationshipsTail.Add(relItem);
        //            }
        //            relationshipsTail.Add(newrelationship);
        //            GenereateDefaultSearchColumns(null, newrelationship, allEntities, relationshipPath + (relationshipPath == "" ? "" : ",") + newrelationship.ID.ToString(), relationshipsTail, list);
        //        }
        //    }

        //    if (sententity != null)
        //    {
        //        short index = 0;
        //        foreach (var item in list)
        //        {
        //            item.OrderID = index;
        //            index++;
        //        }

        //    }
        //    return list;
        //}
        //private void GenereateDefaultSearchColumnsFromRelationship(TableDrivedEntityDTO sententity, RelationshipDTO relationship, List<TableDrivedEntityDTO> allEntities, string relationshipPath = "", List<RelationshipDTO> relationships = null, List<EntitySearchColumnsDTO> list = null)
        //{
        //}
        private List<ColumnDTO> GetSimpleSearchColumns(TableDrivedEntityDTO entity)
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
                if (CheckFirstPriorityColumnName(column))
                    result.Add(column);
                else
                {
                    var key = string.IsNullOrEmpty(column.Alias) ? column.Name : column.Alias;
                    if ((key.Contains("date") || key.Contains("تاریخ")) && indexer < columns.Count / 2)
                        result.Add(column);
                }
                indexer++;
            }
            return result;
        }

        private bool CheckFirstPriorityColumnName(ColumnDTO column)
        {
            var key = string.IsNullOrEmpty(column.Alias) ? column.Name : column.Alias;
            return CheckColumnDetection(GetPriorityColumnNames(), key);
        }
        private bool CheckColumnDetection(List<PriorityColumnDetection> list, string columnAlias)
        {
            return list.Any(x =>
              (x.CompareType == PriorityCompareType.Equals && x.Key.ToLower() == columnAlias.ToLower())
              || (x.CompareType == PriorityCompareType.ColumnAliasContainsKey && columnAlias.ToLower().Contains(x.Key.ToLower()))
                );
        }
        List<PriorityColumnDetection> _GetPriorityColumnNames;
        List<PriorityColumnDetection> GetPriorityColumnNames()
        {
            if (_GetPriorityColumnNames == null)
            {
                _GetPriorityColumnNames = new List<PriorityColumnDetection>()
                {
                    new PriorityColumnDetection("code",PriorityCompareType.ColumnAliasContainsKey),
                    new PriorityColumnDetection("کد",PriorityCompareType.Equals),
                    new PriorityColumnDetection("کد"+" ",PriorityCompareType.ColumnAliasContainsKey),
                    new PriorityColumnDetection("name",PriorityCompareType.ColumnAliasContainsKey),
                    new PriorityColumnDetection("نام",PriorityCompareType.Equals),
                    new PriorityColumnDetection("نام"+" ",PriorityCompareType.ColumnAliasContainsKey),
                    new PriorityColumnDetection("title",PriorityCompareType.ColumnAliasContainsKey),
                    new PriorityColumnDetection("عنوان",PriorityCompareType.ColumnAliasContainsKey),
                    new PriorityColumnDetection("number",PriorityCompareType.ColumnAliasContainsKey),
                    new PriorityColumnDetection("شماره",PriorityCompareType.ColumnAliasContainsKey),
                    new PriorityColumnDetection("family",PriorityCompareType.ColumnAliasContainsKey),
                    new PriorityColumnDetection("نوع",PriorityCompareType.Equals),
                      new PriorityColumnDetection("نوع"+" ",PriorityCompareType.ColumnAliasContainsKey),
                    new PriorityColumnDetection("type",PriorityCompareType.ColumnAliasContainsKey)
                };

            }
            return _GetPriorityColumnNames;
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
        public EntitySearch SaveItem(MyProjectEntities projectContext, EntitySearchDTO message, List<EntityRelationshipTail> createdRelationshipTails = null)
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
            if (createdRelationshipTails == null)
                createdRelationshipTails = new List<EntityRelationshipTail>();
            BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
            foreach (var column in message.EntitySearchAllColumns)
            {
                EntitySearchColumns rColumn = new EntitySearchColumns();
                if (column.ColumnID != 0)
                    rColumn.ColumnID = column.ColumnID;
                else
                    rColumn.ColumnID = null;
                rColumn.Alias = column.Alias;
                rColumn.Tooltip = column.Tooltip;

                rColumn.OrderID = column.OrderID;
                // rColumn.WidthUnit = column.WidthUnit;

                if (string.IsNullOrEmpty(column.CreateRelationshipTailPath))
                    rColumn.EntityRelationshipTailID = column.RelationshipTailID == 0 ? (int?)null : column.RelationshipTailID;
                else
                {
                    if (createdRelationshipTails.Any(x => x.TableDrivedEntityID == message.TableDrivedEntityID && x.RelationshipPath == column.CreateRelationshipTailPath))
                        rColumn.EntityRelationshipTail = createdRelationshipTails.First(x => x.TableDrivedEntityID == message.TableDrivedEntityID && x.RelationshipPath == column.CreateRelationshipTailPath);
                    else
                    {
                        var relationshipTail = bizEntityRelationshipTail.GetOrCreateEntityRelationshipTail(projectContext, message.TableDrivedEntityID, column.CreateRelationshipTailPath);
                        createdRelationshipTails.Add(relationshipTail);
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
