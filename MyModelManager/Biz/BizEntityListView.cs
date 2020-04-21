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
                    rColumn.Tooltip = column.Tooltip;
                    rColumn.WidthUnit = column.WidthUnit ?? 0;
                    if (column.EntityRelationshipTailID != null)
                    {
                        rColumn.RelationshipTailID = column.EntityRelationshipTailID.Value;
                        rColumn.RelationshipTail = bizEntityRelationshipTail.ToEntityRelationshipTailDTO(column.EntityRelationshipTail);
                    }
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
            result.EntityListViewAllColumns = GenereateDefaultListViewColumns(entity, null, allEntities);
            return result;
        }
        private List<EntityListViewColumnsDTO> GenereateDefaultListViewColumns(TableDrivedEntityDTO sententity, RelationshipDTO relationship, List<TableDrivedEntityDTO> allEntities, string relationshipPath = "", List<RelationshipDTO> relationships = null, List<EntityListViewColumnsDTO> list = null)
        {
            if (list == null)
                list = new List<EntityListViewColumnsDTO>();
            if (relationships == null)
                relationships = new List<RelationshipDTO>();
            if (relationship != null)
                relationships.Add(relationship);
            TableDrivedEntityDTO entity = null;
            List<ColumnDTO> simplecollumns = null;
            if (sententity != null)
            {
                entity = sententity;
                simplecollumns = GetSimpleListViewColumns(entity);
                foreach (var column in entity.Columns.Where(x => x.PrimaryKey))
                {
                    var resultColumn = new EntityListViewColumnsDTO();
                    resultColumn.ColumnID = column.ID;
                    resultColumn.Column = column;
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
                    var resultColumn = new EntityListViewColumnsDTO();
                    resultColumn.ColumnID = column.ID;
                    resultColumn.Column = column;
                    resultColumn.CreateRelationshipTailPath = relationshipPath;
                    resultColumn.AllRelationshipsAreSubTuSuper = relationships.All(x => x.TypeEnum == Enum_RelationshipType.SubToSuper);
                    resultColumn.Alias = (relationship == null || resultColumn.AllRelationshipsAreSubTuSuper ? "" : entity.Alias + ".") + column.Alias;
                    resultColumn.Tooltip = relationship == null ? "" : entity.Alias + "." + column.Alias;
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
                            if (relationship != null)
                            {
                                if (newrelationship.TypeEnum != Enum_RelationshipType.SubToSuper)
                                    continue;
                                //دو لول بالا نمیرود مگر اینکه ارث بری باشد
                            }
                            //کلید های خارجی موجودیت های دیگر مهم نیستند
                            if (sententity != null)
                            {
                                foreach (var relCol in newrelationship.RelationshipColumns)
                                {
                                    bool fkIsValid = false;
                                    if (sententity == null)
                                        fkIsValid = true;
                                    else
                                    {
                                        //چون برای انتیتی اصلی پرایمری ها قبلا اضافه شده اند
                                        fkIsValid = !relCol.FirstSideColumn.PrimaryKey;
                                    }
                                    if (fkIsValid)
                                    {
                                        //چون کلید اصلی ها بالا اضافه شدند
                                        var resultColumn = new EntityListViewColumnsDTO();
                                        resultColumn.Column = relCol.FirstSideColumn;
                                        resultColumn.ColumnID = relCol.FirstSideColumnID;
                                        resultColumn.CreateRelationshipTailPath = relationshipPath;
                                        resultColumn.Alias = (relationship == null || resultColumn.AllRelationshipsAreSubTuSuper ? "" : entity.Alias + ".") + relCol.FirstSideColumn.Alias;
                                        resultColumn.Tooltip = relationship == null ? "" : entity.Alias + "." + relCol.FirstSideColumn.Alias;
                                        list.Add(resultColumn);
                                    }
                                }
                            }

                            GenereateDefaultListViewColumns(null, newrelationship, allEntities, relationshipPath + (relationshipPath == "" ? "" : ",") + newrelationship.ID.ToString(), relationships, list);
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
                CheckDescriptiveColumns(list);
            }
            return list;
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
            if (string.IsNullOrEmpty(item.CreateRelationshipTailPath))
            {
                if (item.Column.PrimaryKey)
                    return true;
                else
                    return CheckDescriptiveColumnName(item.Column.Name);
            }
            else
            {
                if (item.AllRelationshipsAreSubTuSuper)
                {
                    return CheckDescriptiveColumnName(item.Column.Name);
                }
            }
            return false;

        }

        private bool CheckDescriptiveColumnName(string columnName)
        {
            var toLowwer = columnName.ToLower();
            return toLowwer.Contains("code") || toLowwer.Contains("name") || toLowwer.Contains("firstname") || toLowwer.Contains("lastname") || toLowwer.Contains("title") || toLowwer.Contains("number")
                    || toLowwer.Contains("family");
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

                if (CheckDescriptiveColumnName(column.Name))
                    result.Add(column);
                else
                {
                    var toLowwer = column.Name.ToLower();
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

        public EntityListView SaveItem(MyProjectEntities projectContext, EntityListViewDTO message)
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
            List<EntityRelationshipTail> relationshipTails = new List<EntityRelationshipTail>();
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
                    if (relationshipTails.Any(x => x.TableDrivedEntityID == message.TableDrivedEntityID && x.RelationshipPath == column.CreateRelationshipTailPath))
                        rColumn.EntityRelationshipTail = relationshipTails.First(x => x.TableDrivedEntityID == message.TableDrivedEntityID && x.RelationshipPath == column.CreateRelationshipTailPath);
                    else
                    {
                        var relationshipTail = bizEntityRelationshipTail.GetOrCreateEntityRelationshipTail(projectContext, message.TableDrivedEntityID, column.CreateRelationshipTailPath);
                        relationshipTails.Add(relationshipTail);
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

}
