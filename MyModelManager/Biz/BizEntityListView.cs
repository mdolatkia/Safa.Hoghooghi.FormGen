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
    public class BizEntityListView
    {
        SecurityHelper securityHelper = new SecurityHelper();
        public event EventHandler<ItemImportingStartedArg> ItemImportingStarted;
        BizEntityRelationshipTail bizEntityRelationshipTail = new MyModelManager.BizEntityRelationshipTail();
        public List<EntityListViewDTO> GetEntityListViews(DR_Requester requester, int entityID)
        {
            //var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.Validation, entityID.ToString());
            //if (cachedItem != null)
            //    return (cachedItem as List<EntityListViewDTO>);

            List<EntityListViewDTO> result = new List<EntityListViewDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var listEntityListView = projectContext.EntityListView.Where(x => x.TableDrivedEntityID == entityID);
                foreach (var item in listEntityListView)
                    result.Add(ToEntityListViewDTO(requester, item, false));

            }
            //CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.Validation, entityID.ToString());
            return result;
        }
        public EntityListViewDTO GetEntityEditListView(DR_Requester requester, int entityID)
        {
            BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
            var entityDTO = bizTableDrivedEntity.GetTableDrivedEntity(requester, entityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
            return ToEntitySimpleListView(entityDTO);
        }
        public EntityListViewDTO GetEntityKeysListView(DR_Requester requester, int entityID)
        {
            BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
            var entityDTO = bizTableDrivedEntity.GetTableDrivedEntity(requester, entityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
            return ToEntityKeysListView(entityDTO);
        }
        //public EntityListViewDTO GetEntityEditListView(TableDrivedEntityDTO mainEntity)
        //{
        //    return ToEntitySimpleListView(mainEntity);
        //}
        public EntityListViewDTO GetEntityDefaultListView(DR_Requester requester, int entityID)
        {
            EntityListViewDTO result = null;
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var entity = projectContext.TableDrivedEntity.First(x => x.ID == entityID);
                if (entity.EntityListView1 != null)
                {
                    if (DataIsAccessable(requester, entity.EntityListView1))
                        result = ToEntityListViewDTO(requester, entity.EntityListView1, true);
                    else
                        return null;
                }
                else
                {
                    var defaultListView = entity.EntityListView.FirstOrDefault();
                    if (defaultListView != null)
                    {
                        if (DataIsAccessable(requester, defaultListView))
                            result = ToEntityListViewDTO(requester, defaultListView, true);
                        else
                            return null;
                    }
                    else
                    {
                        //باید یک دیفالت ساخته و فرستاده شه
                        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
                        var entityDTO = bizTableDrivedEntity.GetPermissionedEntity(requester, entityID);
                        result = ToEntitySimpleListView(entityDTO);
                    }
                }

            }
            return result;
        }

        private EntityListViewDTO ToEntitySimpleListView(TableDrivedEntityDTO entityDTO)
        {
            EntityListViewDTO result = new EntityListViewDTO();
            result.TableDrivedEntityID = entityDTO.ID;
            result.ID = 0;
            result.Title = "ستونهای ساخته شده";
            foreach (var column in entityDTO.Columns)
            {
                EntityListViewColumnsDTO rColumn = new EntityListViewColumnsDTO();
                rColumn.ID = 0;
                rColumn.ColumnID = column.ID;
                rColumn.Column = column;
                rColumn.Alias = column.Alias;
                rColumn.OrderID = (short)column.Position;
                result.EntityListViewAllColumns.Add(rColumn);
            }
            return result;
        }



        private EntityListViewDTO ToEntityKeysListView(TableDrivedEntityDTO entityDTO)
        {
            EntityListViewDTO result = new EntityListViewDTO();
            result.TableDrivedEntityID = entityDTO.ID;
            result.ID = 0;
            result.Title = "ستونهای کلید";
            foreach (var column in entityDTO.Columns.Where(x => x.PrimaryKey))
            {
                EntityListViewColumnsDTO rColumn = new EntityListViewColumnsDTO();
                rColumn.ID = 0;
                rColumn.ColumnID = column.ID;
                rColumn.Column = column;
                rColumn.Alias = column.Alias;
                rColumn.OrderID = (short)column.Position;
                result.EntityListViewAllColumns.Add(rColumn);
            }
            return result;
        }
        public EntityListViewDTO GetEntityListView(DR_Requester requester, int EntityListViewsID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var EntityListViews = projectContext.EntityListView.First(x => x.ID == EntityListViewsID);
                if (DataIsAccessable(requester, EntityListViews))
                {
                    var result = ToEntityListViewDTO(requester, EntityListViews, true);

                    return result;
                }
                else
                    return null;
            }
        }

        private bool DataIsAccessable(DR_Requester requester, EntityListView entityListView)
        {
            if (requester.SkipSecurity)
                return true;

            return bizTableDrivedEntity.DataIsAccessable(requester, entityListView.TableDrivedEntity);
        }
        private void ImposeSecurity(DR_Requester requester, EntityListViewDTO entityListViewDTO, TableDrivedEntity entity)
        {
            BizColumn bizColumn = new BizColumn();
            entityListViewDTO.SercurityImposed = true;

            //var permission = bizTableDrivedEntity.GetEntityAssignedPermissions(requester, entityListViewDTO.TableDrivedEntityID, true);
            List<EntityListViewColumnsDTO> removeList = new List<ModelEntites.EntityListViewColumnsDTO>();
            foreach (var columnGroup in entityListViewDTO.EntityListViewAllColumns.GroupBy(x => x.RelationshipTailID))
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
                        if (!bizColumn.DataIsAccessable(requester, column.ColumnID))
                            removeList.Add(column);
                    }
                }
            }
            foreach (var remove in removeList)
            {
                entityListViewDTO.EntityListViewAllColumns.Remove(remove);
            }
        }
        private EntityListViewDTO ToEntityListViewDTO(DR_Requester requester, EntityListView item, bool withDetails)
        {
            EntityListViewDTO result = new EntityListViewDTO();
            result.TableDrivedEntityID = item.TableDrivedEntityID;
            result.ID = item.ID;
            result.Title = item.Title;
            if (withDetails)
            {
                BizColumn bizColumn = new MyModelManager.BizColumn();
                foreach (var column in item.EntityListViewColumns)
                {
                    EntityListViewColumnsDTO rColumn = new EntityListViewColumnsDTO();
                    rColumn.ID = column.ID;
                    rColumn.ColumnID = column.ColumnID;
                    rColumn.Column = bizColumn.ToColumnDTO(column.Column, true);
                    rColumn.IsDescriptive = column.IsDescriptive;
                    rColumn.Alias = column.Alias ?? rColumn.Column.Alias ?? rColumn.Column.Name;
                    rColumn.OrderID = column.OrderID ?? 0;
                    if (column.EntityRelationshipTailID != null)
                    {
                        rColumn.RelationshipTailID = column.EntityRelationshipTailID.Value;
                        rColumn.RelationshipTail = bizEntityRelationshipTail.ToEntityRelationshipTailDTO(column.EntityRelationshipTail);
                    }
                    if (!string.IsNullOrEmpty(column.Tooltip))
                        rColumn.Tooltip = column.Tooltip;
                    else
                    {

                        if (rColumn.RelationshipTail != null)
                            rColumn.Tooltip = rColumn.RelationshipTail.ReverseRelationshipTail.TargetEntityAlias + "." + rColumn.Column.Alias;
                    }
                    rColumn.WidthUnit = column.WidthUnit ?? 0;

                    //rColumn.RelativeColumnName = rColumn.Column.Name + rColumn.RelationshipTailID.ToString();
                    result.EntityListViewAllColumns.Add(rColumn);
                }
                //foreach (var tail in item.EntityListViewRelationshipTails)
                //{
                //    EntityListViewRelationshipTailDTO rTail = new EntityListViewRelationshipTailDTO();
                //    rTail.ID = tail.ID;
                //    rTail.EntityRelationshipTailID = tail.EntityRelationshipTailID;
                //    rTail.EntityRelationshipTail = bizEntityRelationshipTail.ToEntityRelationshipTailDTO(tail.EntityRelationshipTail);
                //    foreach (var tailColumn in tail.EntityListViewColumns)
                //    {
                //        rTail.EntityListViewColumns.Add(result.EntityListViewAllColumns.First(x => x.ID == tailColumn.ID));
                //    }
                //    result.EntityListViewRelationshipTails.Add(rTail);
                //}
            }

            ImposeSecurity(requester, result, item.TableDrivedEntity);
            return result;
        }
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();

        //private bool CheckRelationshipTailPermission(EntityRelationshipTailDTO relationshipTail, bool first = true)
        //{
        //    if (first)
        //    {
        //        var entityEnabled = bizTableDrivedEntity.IsEntityEnabled(relationshipTail.RelationshipTargetEntityID);
        //        if (!entityEnabled)
        //            return false;

        //    }
        //    if (relationshipTail.ChildTail != null)
        //        return CheckRelationshipTailPermission(relationshipTail.ChildTail, false);
        //    else
        //        return true;
        //}


        //public void UpdateDefaultListViewInModel(int databaseID)
        //{

        //}

        public EntityListViewDTO GenerateDefaultListView(TableDrivedEntityDTO entity, List<TableDrivedEntityDTO> allEntities)
        {
            EntityListViewDTO result = new EntityListViewDTO();
            result.TableDrivedEntityID = entity.ID;
            result.Title = "لیست نمایشی پیش فرض";
            result.EntityListViewAllColumns = GenereateDefaultListViewColumns(entity, allEntities);
            return result;
        }
        private List<EntityListViewColumnsDTO> GenereateDefaultListViewColumns(TableDrivedEntityDTO entity, List<TableDrivedEntityDTO> allEntities, List<EntityListViewColumnsDTO> list = null)
        {
            if (list == null)
                list = new List<EntityListViewColumnsDTO>();
            foreach (var column in entity.Columns.Where(x => x.PrimaryKey))
            {
                AddListViewColumn(list, column);
            }
            var simplecollumns = GetSimpleListViewColumns(entity);
            foreach (var column in simplecollumns)
            {
                AddListViewColumn(list, column);
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
                CheckDescriptiveColumns(list);
            }
            return list;
        }
        private void AddRelationshipDefaultColumns(TableDrivedEntityDTO entity, List<TableDrivedEntityDTO> allEntities, List<EntityListViewColumnsDTO> list, string relationshipPath = "", List<RelationshipDTO> relationships = null)
        {
            var reviewedFKRels = new List<RelationshipDTO>();
            foreach (var column in entity.Columns)
            {
                if (entity.Relationships.Any(z => z.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary && z.RelationshipColumns.Any(y => y.FirstSideColumnID == column.ID)))
                {
                    var newrelationship = entity.Relationships.First(z => z.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary && z.RelationshipColumns.Any(y => y.FirstSideColumnID == column.ID));
                    if (relationships == null || (newrelationship.TypeEnum == Enum_RelationshipType.SubToSuper || newrelationship.TypeEnum == Enum_RelationshipType.UnionToSubUnion))
                    {
                        if (!reviewedFKRels.Any(x => x.ID == newrelationship.ID))
                        {
                            reviewedFKRels.Add(newrelationship);
                            //جلوگیری از لوپ
                            if (relationships == null || (!relationships.Any(x => x.ID == newrelationship.ID)))
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
                                            AddListViewColumn(list, relCol.FirstSideColumn, relationshipPath, relationships);
                                        }
                                    }
                                    GenereateDefaultListViewColumnsFromRelationship(newrelationship, allEntities, list, relationshipPath + (relationshipPath == "" ? "" : ",") + newrelationship.ID.ToString(), relationshipsTail);
                                }
                            }
                        }
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
                            //دیگه بقیه روابط یک رابطه ارث بری یا اتحاد را نرود
                            List<RelationshipDTO> relationshipsTail = new List<RelationshipDTO>();
                            if (relationships != null)
                            {
                                foreach (var relItem in relationships)
                                    relationshipsTail.Add(relItem);
                            }
                            relationshipsTail.Add(newrelationship);
                            GenereateDefaultListViewColumnsFromRelationship(newrelationship, allEntities, list, relationshipPath + (relationshipPath == "" ? "" : ",") + newrelationship.ID.ToString(), relationshipsTail);
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

        private void GenereateDefaultListViewColumnsFromRelationship(RelationshipDTO relationship, List<TableDrivedEntityDTO> allEntities, List<EntityListViewColumnsDTO> list, string relationshipPath, List<RelationshipDTO> relationships)
        {
            TableDrivedEntityDTO entity = allEntities.First(x => x.ID == relationship.EntityID2);
            var skipRelColumnIDs = relationship.RelationshipColumns.Select(x => x.SecondSideColumn.ID).ToList();
            foreach (var column in entity.Columns.Where(x => x.PrimaryKey && !skipRelColumnIDs.Contains(x.ID)))
            {
                AddListViewColumn(list, column);
            }
            List<ColumnDTO> simplecollumns = GetEntitySimpleColumnsColumns(entity);
            foreach (var column in simplecollumns)
            {
                AddListViewColumn(list, column, relationshipPath, relationships);
            }
            AddRelationshipDefaultColumns(entity, allEntities, list, relationshipPath, relationships);
        }
        private void AddListViewColumn(List<EntityListViewColumnsDTO> list, ColumnDTO column, string relationshipPath = null, List<RelationshipDTO> relationships = null)
        {
            var resultColumn = new EntityListViewColumnsDTO();
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
        private void CheckDescriptiveColumns(List<EntityListViewColumnsDTO> list)
        {
            foreach (var item in list)
            {
                item.IsDescriptive = CheckIsDescriptive(item);
            }
            if (list.Count(x => x.IsDescriptive) == 1)
            {
                foreach (var item in list.Where(x => x.Column.ColumnType == Enum_ColumnType.String ||
             x.Column.ColumnType == Enum_ColumnType.Numeric || x.Column.ColumnType == Enum_ColumnType.Date).Take(2))
                {
                    item.IsDescriptive = true;
                }
            }
        }

        private bool CheckIsDescriptive(EntityListViewColumnsDTO item)
        {
            if (item.Column.ColumnType != Enum_ColumnType.String
                && item.Column.ColumnType != Enum_ColumnType.Numeric
                && item.Column.ColumnType != Enum_ColumnType.Date)
            {
                return false;
            }
            var key = string.IsNullOrEmpty(item.Column.Alias) ? item.Column.Name : item.Column.Alias;
            if (string.IsNullOrEmpty(item.CreateRelationshipTailPath))
            {
                if (item.Column.PrimaryKey)
                    return true;
                else
                    return CheckColumnDetection(GetDescriptiveColumnNames(), key);
            }
            else
            {
                if (item.AllRelationshipsAreSubToSuper)
                {
                    return CheckColumnDetection(GetDescriptiveColumnNames(), key);
                }
            }
            return false;

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
                _GetPriorityColumnNames = GetDescriptiveColumnNames().ToList();
                _GetPriorityColumnNames.Add(new PriorityColumnDetection("type", PriorityCompareType.ColumnAliasContainsKey));
                _GetPriorityColumnNames.Add(new PriorityColumnDetection("نوع", PriorityCompareType.Equals));
                _GetPriorityColumnNames.Add(new PriorityColumnDetection("نوع" + " ", PriorityCompareType.ColumnAliasContainsKey));

            }
            return _GetPriorityColumnNames;
        }
        List<PriorityColumnDetection> _DescriptiveColumnNames;
        List<PriorityColumnDetection> GetDescriptiveColumnNames()
        {
            if (_DescriptiveColumnNames == null)
            {
                _DescriptiveColumnNames = new List<PriorityColumnDetection>()
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
                    new PriorityColumnDetection("family",PriorityCompareType.ColumnAliasContainsKey)
                };
            }
            return _DescriptiveColumnNames;
        }
        //private bool CheckDescriptiveColumnName(ColumnDTO column)
        //{

        //    return
        //}

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

        private List<ColumnDTO> GetEntitySimpleColumnsColumns(TableDrivedEntityDTO targetEntity)
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
        private List<ColumnDTO> GetSecondPriorityColumns(List<ColumnDTO> columns, List<ColumnDTO> alreadyColumns)
        {
            var indexer = 0;
            List<ColumnDTO> result = new List<ColumnDTO>();
            foreach (var column in columns)
            {
                if (alreadyColumns.Any(x => x.ID == column.ID))
                    result.Add(column);
                else if (column.ColumnType == Enum_ColumnType.String && indexer <= columns.Count / 2)
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

        public EntityListView SaveItem(MyProjectEntities projectContext, EntityListViewDTO message, List<EntityRelationshipTail> createdRelationshipTails = null)
        {
            var dbEntityListView = projectContext.EntityListView.FirstOrDefault(x => x.ID == message.ID);
            if (dbEntityListView == null)
            {
                dbEntityListView = new DataAccess.EntityListView();
            }
            dbEntityListView.TableDrivedEntityID = message.TableDrivedEntityID;
            dbEntityListView.Title = message.Title;

            //تیلهای گزارش را از روی تیلهای ستونها میسازد
            //هر دفعه پاک نشن بهتره..اصلاح بشن
            while (dbEntityListView.EntityListViewColumns.Any())
                projectContext.EntityListViewColumns.Remove(dbEntityListView.EntityListViewColumns.First());
            //while (dbEntityListView.EntityListViewRelationshipTails.Any())
            //    projectContext.EntityListViewRelationshipTails.Remove(dbEntityListView.EntityListViewRelationshipTails.First());
            if (createdRelationshipTails == null)
                createdRelationshipTails = new List<EntityRelationshipTail>();

            BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
            foreach (var column in message.EntityListViewAllColumns)
            {
                EntityListViewColumns rColumn = new EntityListViewColumns();
                rColumn.ColumnID = column.ColumnID;
                rColumn.Alias = column.Alias;
                rColumn.OrderID = column.OrderID;
                rColumn.Tooltip = column.Tooltip;
                rColumn.IsDescriptive = column.IsDescriptive;
                rColumn.WidthUnit = column.WidthUnit;
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
                dbEntityListView.EntityListViewColumns.Add(rColumn);
            }
            if (dbEntityListView.ID == 0)
                projectContext.EntityListView.Add(dbEntityListView);
            return dbEntityListView;


        }

        //public void CheckListViewPermissions(DR_Requester requester, EntityListViewDTO listView, SecurityMode securityMode)
        //{
        //    SecurityHelper securityHelper = new SecurityHelper();
        //    BizEntityRelationshipTail bizEntityRelationshipTail = new MyModelManager.BizEntityRelationshipTail();
        //    List<EntityListViewColumnsDTO> removeList = new List<ModelEntites.EntityListViewColumnsDTO>();
        //    foreach (var columnGroup in listView.EntityListViewAllColumns.GroupBy(x => x.RelationshipTailID))
        //    {

        //        if (columnGroup.Key == 0)
        //        {

        //            var permissions = securityHelper.GetAssignedPermissions(requester, listView.TableDrivedEntityID, true);
        //            foreach (var column in columnGroup)
        //            {
        //                if (!column.Column.Enabled)
        //                    removeList.Add(column);
        //                else if (securityMode == SecurityMode.View)
        //                {
        //                    if (permissions.ChildsPermissions.Any(x => x.SecurityObjectID == column.ColumnID && x.GrantedActions.Any(y => y == SecurityAction.NoAccess)))
        //                        removeList.Add(column);
        //                }
        //                else if (securityMode == SecurityMode.Edit)
        //                {
        //                    //دسترسی فیلدهایی که ریدونلی اند اما ادیت نباید بشوند باید در کلاس ادیت بررسی شود
        //                    //اینجا ریدولی ها هم میروند چون در فرم نیاز است که نمایش داده شوند
        //                    if (permissions.ChildsPermissions.Any(x => x.SecurityObjectID == column.ColumnID && x.GrantedActions.Any(y => y == SecurityAction.NoAccess)))
        //                        removeList.Add(column);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (securityMode == SecurityMode.Edit)
        //                throw new Exception("asdasdff");

        //            var relationshipTail = columnGroup.First(x => x.RelationshipTailID == columnGroup.Key).RelationshipTail;
        //            bool pathPermission = bizEntityRelationshipTail.CheckRelationshipTailPermission(requester, relationshipTail);
        //            if (!pathPermission)
        //            {
        //                foreach (var column in columnGroup)
        //                {
        //                    removeList.Add(column);
        //                }
        //            }
        //            else
        //            {
        //                var entityPermissions = securityHelper.GetAssignedPermissions(requester, relationshipTail.Relationship.ID, true);
        //                foreach (var column in columnGroup)
        //                {
        //                    if (!column.Column.Enabled)
        //                        removeList.Add(column);
        //                    else if (entityPermissions.ChildsPermissions.Any(x => x.SecurityObjectID == column.ColumnID && x.GrantedActions.Any(y => y == SecurityAction.NoAccess)))
        //                        removeList.Add(column);
        //                }
        //            }
        //        }
        //    }
        //    foreach (var remove in removeList)
        //    {
        //        listView.EntityListViewAllColumns.Remove(remove);
        //    }
        //}

        public bool SetDefaultListView(int entityID, int iD)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbEntity = projectContext.TableDrivedEntity.FirstOrDefault(x => x.ID == entityID);
                dbEntity.EntityListViewID = iD;
                projectContext.SaveChanges();
            }
            return true;
        }

        public int UpdateEntityListViews(EntityListViewDTO message)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbEntityListView = SaveItem(projectContext, message);
                projectContext.SaveChanges();
                return dbEntityListView.ID;
            }
        }
    }
    public class PriorityColumnDetection
    {
        public PriorityColumnDetection(string key, PriorityCompareType compareType)
        {
            Key = key;
            CompareType = compareType;
        }
        public string Key { set; get; }
        public PriorityCompareType CompareType { set; get; }
    }
    public enum PriorityCompareType
    {
        Equals,
        ColumnAliasContainsKey,
        //   KeyContainsColumnAlias,
    }
}
