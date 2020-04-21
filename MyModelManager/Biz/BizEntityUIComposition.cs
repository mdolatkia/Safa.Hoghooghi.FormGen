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
    public class BizEntityUIComposition
    {
        public event EventHandler<ItemImportingStartedArg> ItemImportingStarted;
        //public EntityUICompositionDTO GetEntityUICompositionTree(int entityID)
        //{
        //    //////var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.EntityUICompositionTree, entityID.ToString());
        //    //////if (cachedItem != null)
        //    //////    return (cachedItem as List<EntityUICompositionDTO>);
        //    //  List<EntityUICompositionDTO> result = new List<EntityUICompositionDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var rootdb = projectContext.EntityUIComposition.FirstOrDefault(x => x.TableDrivedEntityID == entityID && x.ParentID == null);
        //        if (rootdb != null)
        //        {
        //            var result = ToEntityUICompositionDTO(rootdb, true);
        //            return result;
        //        }
        //        else
        //            return null;
        //    }
        //    //CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.EntityUICompositionTree, entityID.ToString());

        //}
        public EntityUICompositionCompositeDTO GetEntityUICompositionTree(int entityID)
        {
            EntityUICompositionCompositeDTO result = new EntityUICompositionCompositeDTO();
            //////var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.EntityUICompositionTree, entityID.ToString());
            //////if (cachedItem != null)
            //////    return (cachedItem as List<EntityUICompositionDTO>);

            result.ColumnItems = new List<ColumnUISettingDTO>();
            result.RelationshipItems = new List<RelationshipUISettingDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbroot = projectContext.EntityUIComposition.FirstOrDefault(x => x.TableDrivedEntityID == entityID && x.ParentID == null);
                //foreach (var item in list)
                //{
                if (dbroot != null)
                    result.RootItem = ToEntityUICompositionDTO(dbroot, true, result.ColumnItems, result.RelationshipItems);
                //}
            }
            //CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.EntityUICompositionTree, entityID.ToString());
            return result;
        }
        public List<EntityUICompositionDTO> GetListEntityUIComposition(int entityID)
        {
            //var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.EntityUICompositionTree, entityID.ToString());
            //if (cachedItem != null)
            //    return (cachedItem as List<EntityUICompositionDTO>);
            List<EntityUICompositionDTO> result = new List<EntityUICompositionDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var list = projectContext.EntityUIComposition.Where(x => x.TableDrivedEntityID == entityID);
                foreach (var item in list)
                {
                    result.Add(ToEntityUICompositionDTO(item, false));
                }
            }
            //CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.EntityUICompositionTree, entityID.ToString());
            return result;
        }
        //public List<EntityUICompositionDTO> GetEntityUIComposition(int? parentID)
        //{
        //    List<EntityUICompositionDTO> result = new List<EntityUICompositionDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var list = projectContext.EntityUIComposition.Where(x => x.ParentID == parentID);
        //        foreach (var item in list)
        //        {
        //            result.Add(ToEntityUICompositionDTO(item, false));
        //        }
        //    }
        //    return result;
        //}

        private EntityUICompositionDTO ToEntityUICompositionDTO(DataAccess.EntityUIComposition item, bool withChilds, List<ColumnUISettingDTO> columnItems = null, List<RelationshipUISettingDTO> relationshipItems = null)
        {
            var result = new EntityUICompositionDTO();
            result.ID = item.ID;
            result.ParentID = item.ParentID;
            result.ObjectIdentity = item.ItemIdentity;
            result.Title = item.ItemTitle;
            result.ObjectCategory = (DatabaseObjectCategory)Enum.Parse(typeof(DatabaseObjectCategory), item.Category);
            result.Position = item.Position;
            if (result.ObjectCategory == DatabaseObjectCategory.Entity)
            {
                if (item.EntityUISetting != null)
                {
                    result.EntityUISetting = new EntityUISettingDTO();
                    result.EntityUISetting.ID = item.EntityUISetting.ID;
                    result.EntityUISetting.UIColumnsCount = item.EntityUISetting.UIColumnsCount;
                }
            }
            else if (result.ObjectCategory == DatabaseObjectCategory.Column)
            {
                if (item.ColumnUISetting != null)
                {
                    result.ColumnUISetting = new ColumnUISettingDTO();
                    result.ColumnUISetting.ID = item.ColumnUISetting.ID;
                    result.ColumnUISetting.ColumnID = Convert.ToInt32(item.ItemIdentity);
                    result.ColumnUISetting.UIColumnsType = (Enum_UIColumnsType)item.ColumnUISetting.UIColumnsType;
                    result.ColumnUISetting.UIRowsCount = item.ColumnUISetting.UIRowsCount;
                    if (columnItems != null)
                        columnItems.Add(result.ColumnUISetting);
                }
            }
            else if (result.ObjectCategory == DatabaseObjectCategory.Relationship)
            {
                if (item.RelationshipUISetting != null)
                {
                    result.RelationshipUISetting = new RelationshipUISettingDTO();
                    result.RelationshipUISetting.ID = item.RelationshipUISetting.ID;
                    result.RelationshipUISetting.RelationshipID = Convert.ToInt32(item.ItemIdentity);
                    result.RelationshipUISetting.UIColumnsType = (Enum_UIColumnsType)item.RelationshipUISetting.UIColumnsType;
                    result.RelationshipUISetting.Expander = item.RelationshipUISetting.Expander;
                    result.RelationshipUISetting.IsExpanded = item.RelationshipUISetting.IsExpanded == true;
                    result.RelationshipUISetting.UIRowsCount = Convert.ToInt16(item.RelationshipUISetting.UIRowsCount);
                    if (relationshipItems != null)
                        relationshipItems.Add(result.RelationshipUISetting);
                }
            }
            else if (result.ObjectCategory == DatabaseObjectCategory.Group)
            {
                if (item.GroupUISetting != null)
                {
                    result.GroupUISetting = new GroupUISettingDTO();
                    result.GroupUISetting.ID = item.GroupUISetting.ID;
                    result.GroupUISetting.UIColumnsType = (Enum_UIColumnsType)item.GroupUISetting.UIColumnsType;
                    result.GroupUISetting.Expander = item.GroupUISetting.Expander;
                    result.GroupUISetting.IsExpanded = item.GroupUISetting.IsExpanded == true;

                    result.GroupUISetting.InternalColumnsCount = item.GroupUISetting.InternalColumnsCount;
                    result.GroupUISetting.UIRowsCount = Convert.ToInt16(item.GroupUISetting.UIRowsCount);

                }
            }
            else if (result.ObjectCategory == DatabaseObjectCategory.TabControl)
            {
                if (item.TabGroupUISetting != null)
                {
                    result.TabGroupUISetting = new TabGroupUISettingDTO();
                    result.TabGroupUISetting.ID = item.TabGroupUISetting.ID;
                    result.TabGroupUISetting.UIColumnsType = (Enum_UIColumnsType)item.TabGroupUISetting.UIColumnsType;
                    result.TabGroupUISetting.Expander = item.TabGroupUISetting.Expander;
                    result.TabGroupUISetting.IsExpanded = item.TabGroupUISetting.IsExpanded == true;

                    result.TabGroupUISetting.UIRowsCount = Convert.ToInt16(item.TabGroupUISetting.UIRowsCount);

                }
            }
            else if (result.ObjectCategory == DatabaseObjectCategory.TabPage)
            {
                if (item.TabPageUISetting != null)
                {
                    result.TabPageUISetting = new TabPageUISettingDTO();
                    result.TabPageUISetting.ID = item.TabPageUISetting.ID;
                    result.TabPageUISetting.InternalColumnsCount = item.TabPageUISetting.InternalColumnsCount;
                }
            }
            else if (result.ObjectCategory == DatabaseObjectCategory.EmptySpace)
            {
                if (item.EmptySpaceUISetting != null)
                {
                    result.EmptySpaceUISetting = new EmptySpaceUISettingDTO();
                    result.EmptySpaceUISetting.ID = item.EmptySpaceUISetting.ID;
                    result.EmptySpaceUISetting.UIColumnsType = (Enum_UIColumnsType)item.EmptySpaceUISetting.UIColumnsType;
                    result.EmptySpaceUISetting.ExpandToEnd = item.EmptySpaceUISetting.ExtendToEnd;
                }
            }
            result.ChildItems = new List<EntityUICompositionDTO>();
            if (withChilds)
                foreach (var citem in item.EntityUIComposition1)
                {
                    result.ChildItems.Add(ToEntityUICompositionDTO(citem, withChilds, columnItems, relationshipItems));
                }

            return result;
        }

        //public void UpdateUICompositionsInModel(int databaseID)
        //{

        //}

        private void BizTableDrivedEntity_ItemImportingStarted(object sender, ItemImportingStartedArg e)
        {
            if (ItemImportingStarted != null)
                ItemImportingStarted(this, e);
        }

        public List<EntityUICompositionDTO> GenerateUIComposition(TableDrivedEntityDTO entity)
        {
            List<EntityUICompositionDTO> result = new List<EntityUICompositionDTO>();
            var entityUiComposition = new EntityUICompositionDTO();
            entityUiComposition.ObjectCategory = DatabaseObjectCategory.Entity;
            entityUiComposition.EntityUISetting = new EntityUISettingDTO() { UIColumnsCount = 4 };
            entityUiComposition.ObjectIdentity = entity.ID.ToString();
            result.Add(entityUiComposition);
            List<RelationshipDTO> listFTPRelationships = new List<RelationshipDTO>();
            int index = 0;
            foreach (var item in entity.Columns.OrderBy(x => x.Position))
            {
                if (!listFTPRelationships.Any(x => x.RelationshipColumns.Any(y => y.FirstSideColumnID == item.ID)))
                {
                    if (entity.Relationships.Any(x => x.DataEntryEnabled == true && x.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary && x.RelationshipColumns.Any(y => y.FirstSideColumnID == item.ID)))
                    {
                        var relationship = entity.Relationships.First(x => x.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary && x.RelationshipColumns.Any(y => y.FirstSideColumnID == item.ID));
                        listFTPRelationships.Add(relationship);
                        AddRelationshipUICompositionItem(result[0], result, relationship, index);
                        index++;
                    }
                    else
                    {
                        AddColumnUICompositionItem(result[0], result, item, index);
                        index++;
                    }
                }

            }
            AddPrimaryToForeignRelationshipUICompositionItem(result[0], result, entity.Relationships.Where(x => x.DataEntryEnabled == true && x.MastertTypeEnum == Enum_MasterRelationshipType.FromPrimartyToForeign).ToList(), index);
            return result;
        }

        private void AddPrimaryToForeignRelationshipUICompositionItem(EntityUICompositionDTO parentItem, List<EntityUICompositionDTO> result, List<RelationshipDTO> list, int index)
        {
            EntityUICompositionDTO tabConrol = null;
            foreach (var relationship in list)
            {
                if (relationship.IsOtherSideCreatable && relationship.IsOtherSideDirectlyCreatable)
                {
                    if (tabConrol == null)
                    {
                        tabConrol = CreateRelationshipTabControl(parentItem, index);
                    }
                    AddTabPageRelationship(tabConrol, relationship, result);
                }
                else
                {
                    AddRelationshipUICompositionItem(parentItem, result, relationship, index);
                    index++;
                }
            }
            if (tabConrol != null)
            {
                //tabConrol.Position = index;
                result.Add(tabConrol);
            }
        }

        private void AddTabPageRelationship(EntityUICompositionDTO tabConrol, RelationshipDTO relationship, List<EntityUICompositionDTO> result)
        {
            var tabPage = new EntityUICompositionDTO();
            tabPage.ObjectCategory = DatabaseObjectCategory.TabPage;
            tabPage.ParentItem = tabConrol;
            tabPage.Title = relationship.Alias;
            tabPage.TabPageUISetting = new TabPageUISettingDTO() { InternalColumnsCount = 4 };
            result.Add(tabPage);
            AddRelationshipUICompositionItem(tabPage, result, relationship, 0);
        }

        private void AddRelationshipUICompositionItem(EntityUICompositionDTO parentItem, List<EntityUICompositionDTO> result, RelationshipDTO relationship, int index)
        {
            var childItem = new EntityUICompositionDTO();
            childItem.ObjectCategory = DatabaseObjectCategory.Relationship;
            childItem.ObjectIdentity = relationship.ID.ToString();
            childItem.Position = index;
            childItem.Title = relationship.Alias;
            childItem.ParentItem = parentItem;
            childItem.RelationshipUISetting = new RelationshipUISettingDTO();
            if (relationship.IsOtherSideCreatable && relationship.IsOtherSideDirectlyCreatable)
            {
                //if (relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary)
                //{
                childItem.RelationshipUISetting.Expander = relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary;
                childItem.RelationshipUISetting.IsExpanded = childItem.RelationshipUISetting.Expander == true;
                childItem.RelationshipUISetting.UIColumnsType = Enum_UIColumnsType.Full;
                //}
                //else
                //{
                //if (relationship.TypeEnum == Enum_RelationshipType.OneToMany)
                //{
                //    AddTabPageUICompositionItem(result[0], result, relationship, index)
                //}
                //}
            }
            else
            {
                childItem.RelationshipUISetting.UIColumnsType = Enum_UIColumnsType.Half;
            }
            result.Add(childItem);
        }

        private void AddColumnUICompositionItem(EntityUICompositionDTO parentItem, List<EntityUICompositionDTO> result, ColumnDTO column, int index)
        {
            var childItem = new EntityUICompositionDTO();
            childItem.ObjectCategory = DatabaseObjectCategory.Column;
            childItem.ObjectIdentity = column.ID.ToString();
            childItem.Position = index;
            childItem.Title = column.Alias;
            childItem.ParentItem = parentItem;
            childItem.ColumnUISetting = new ColumnUISettingDTO();
            if (column.ColumnType != Enum_ColumnType.String)
            {
                childItem.ColumnUISetting.UIColumnsType = Enum_UIColumnsType.Normal;
            }
            else
            {
                if (column.StringColumnType.MaxLength == -1)
                {
                    childItem.ColumnUISetting.UIColumnsType = Enum_UIColumnsType.Full;
                    childItem.ColumnUISetting.UIRowsCount = 3;
                }
                else
                {
                    if (column.StringColumnType.MaxLength <= 64)
                        childItem.ColumnUISetting.UIColumnsType = Enum_UIColumnsType.Normal;
                    else if (column.StringColumnType.MaxLength > 64 && column.StringColumnType.MaxLength <= 512)
                        childItem.ColumnUISetting.UIColumnsType = Enum_UIColumnsType.Half;
                    else if (column.StringColumnType.MaxLength > 512 && column.StringColumnType.MaxLength <= 1024)
                        childItem.ColumnUISetting.UIColumnsType = Enum_UIColumnsType.Full;
                    else if (column.StringColumnType.MaxLength > 1024)
                    {
                        childItem.ColumnUISetting.UIColumnsType = Enum_UIColumnsType.Full;
                        childItem.ColumnUISetting.UIRowsCount = 2;
                    }
                }
            }
            result.Add(childItem);
        }

        public void UpdateUIComposition(DR_Requester requester, int entityID)
        {
            BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
            var entity = bizTableDrivedEntity.GetTableDrivedEntity(requester, entityID, EntityColumnInfoType.WithFullColumns, EntityRelationshipInfoType.WithRelationships);
            //اینجا کش شدن روابط توجه شود
            List<EntityUICompositionDTO> generetedUIComposition = null;

            if (!EntityHasUIComposition(entity))
            {
                generetedUIComposition = GenerateUIComposition(entity);
                Save(entityID, generetedUIComposition);
            }
            else
            {
                var entityUICompositions = GetEntityUICompositionTree(entityID);
                generetedUIComposition = new List<EntityUICompositionDTO>();
                //var rootItem = new List<EntityUICompositionDTO>();
                //List<EntityUICompositionDTO> result = new List<EntityUICompositionDTO>();
                List<RelationshipDTO> candidateRelationships = GetCandidUpdateRelationships(entityUICompositions, entity);
                if (candidateRelationships.Any())
                {
                    var tabControl = GetOrCreateRelationshipTabControl(entityUICompositions.RootItem);
                    if (tabControl.ID == 0)
                        generetedUIComposition.Add(tabControl);
                    foreach (var relationship in candidateRelationships)
                    {
                        AddTabPageRelationship(tabControl, relationship, generetedUIComposition);

                    }
                }
                if (generetedUIComposition.Any())
                {
                    using (var projectContext = new DataAccess.MyProjectEntities())
                    {
                        foreach (var parentGroup in generetedUIComposition.Where(x => x.ParentItem.ID != 0).GroupBy(x => x.ParentItem))
                        {
                            var parentDBItem = projectContext.EntityUIComposition.First(x => x.ID == parentGroup.Key.ID);
                            CheckAddItems(entityID, generetedUIComposition, parentGroup.Key, parentDBItem, projectContext);
                        }
                        projectContext.SaveChanges();
                    }
                }
                //AddPrimaryToForeignRelationshipUICompositionItem(result[0], result, entity.Relationships.Where(x => x.DataEntryEnabled == true && x.MastertTypeEnum == Enum_MasterRelationshipType.FromPrimartyToForeign).ToList(), index);
            }


        }

        private EntityUICompositionDTO GetOrCreateRelationshipTabControl(EntityUICompositionDTO rootItem)
        {
            var foundTabControl = FindTabControl(rootItem);
            if (foundTabControl == null)
            {
                foundTabControl = CreateRelationshipTabControl(rootItem, rootItem.ChildItems.Max(x => x.Position) + 1);
            }
            return foundTabControl;
        }

        private EntityUICompositionDTO CreateRelationshipTabControl(EntityUICompositionDTO rootItem, int index)
        {
            var tabConrol = new EntityUICompositionDTO();
            tabConrol.ObjectCategory = DatabaseObjectCategory.TabControl;
            tabConrol.TabGroupUISetting = new TabGroupUISettingDTO() { UIColumnsType = Enum_UIColumnsType.Full };
            tabConrol.Title = "روابط";
            tabConrol.ObjectIdentity = "-99";
            tabConrol.ParentItem = rootItem;
            tabConrol.Position = index;
            return tabConrol;
        }

        private EntityUICompositionDTO FindTabControl(EntityUICompositionDTO rootItem)
        {
            if (rootItem.ObjectCategory == DatabaseObjectCategory.TabControl)
            {
                if (rootItem.ObjectIdentity == "-99")
                {
                    return rootItem;
                }
            }
            foreach (var item in rootItem.ChildItems)
            {
                var findInChild = FindTabControl(item);
                if (findInChild != null)
                    return findInChild;
            }
            return null;
        }

        private List<RelationshipDTO> GetCandidUpdateRelationships(EntityUICompositionCompositeDTO entityUICompositions, TableDrivedEntityDTO entity)
        {
            List<RelationshipDTO> rels = new List<RelationshipDTO>();
            foreach (var relationship in entity.Relationships.Where(x => x.MastertTypeEnum == Enum_MasterRelationshipType.FromPrimartyToForeign &&
            x.DataEntryEnabled == true && x.IsOtherSideCreatable && x.IsOtherSideDirectlyCreatable))
            {
                if (!RelationshipExistsInTree(entityUICompositions.RootItem, relationship))
                {
                    rels.Add(relationship);
                }
            }
            return rels;
        }

        private bool RelationshipExistsInTree(EntityUICompositionDTO rootItem, RelationshipDTO relationship)
        {
            if (rootItem.ObjectCategory == DatabaseObjectCategory.Relationship)
            {
                if (rootItem.ObjectIdentity == relationship.ID.ToString())
                {
                    return true;
                }
            }

            foreach (var item in rootItem.ChildItems)
            {
                if (RelationshipExistsInTree(item, relationship))
                    return true;
            }
            return false;
        }



        public bool EntityHasUIComposition(TableDrivedEntityDTO entity)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                return projectContext.TableDrivedEntity.First(x => x.ID == entity.ID).EntityUIComposition.Any();
            }
        }

        public void Save(int entityID, List<EntityUICompositionDTO> items)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                SaveItem(projectContext, entityID, items);

                try
                {
                    projectContext.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
        }

        public void SaveItem(MyProjectEntities projectContext, int entityID, List<EntityUICompositionDTO> items)
        {
            var existingIds = items.Where(x => x.ID != 0).Select(x => x.ID).ToList();
            if (existingIds.Count > 0)
            {
                var removeList = projectContext.EntityUIComposition.Where(x => x.TableDrivedEntityID == entityID && !existingIds.Contains(x.ID)).ToList();
                foreach (var item in removeList)
                    RemoveItem(item, projectContext);
            }
            CheckAddItems(entityID, items, null, null, projectContext);

        }

        private void CheckAddItems(int entityID, List<EntityUICompositionDTO> items, EntityUICompositionDTO parentItem, EntityUIComposition parentDBItem, DataAccess.MyProjectEntities projectContext)
        {
            foreach (var item in items.Where(x => x.ParentItem == parentItem))
            {
                EntityUIComposition dbItem = null;
                if (item.ID != 0)
                {
                    dbItem = projectContext.EntityUIComposition.First(x => x.ID == item.ID);
                }
                else
                {
                    dbItem = new EntityUIComposition();
                    projectContext.EntityUIComposition.Add(dbItem);
                }
                dbItem.TableDrivedEntityID = entityID;
                dbItem.Category = item.ObjectCategory.ToString();
                if (item.ObjectCategory == DatabaseObjectCategory.Entity)
                {
                    if (dbItem.EntityUISetting == null)
                        dbItem.EntityUISetting = new EntityUISetting();
                    dbItem.EntityUISetting.UIColumnsCount = item.EntityUISetting.UIColumnsCount;
                }
                else if (item.ObjectCategory == DatabaseObjectCategory.Column)
                {
                    if (dbItem.ColumnUISetting == null)
                        dbItem.ColumnUISetting = new ColumnUISetting();
                    dbItem.ColumnUISetting.UIColumnsType = (short)item.ColumnUISetting.UIColumnsType;
                    dbItem.ColumnUISetting.UIRowsCount = item.ColumnUISetting.UIRowsCount;
                }
                else if (item.ObjectCategory == DatabaseObjectCategory.Relationship)
                {
                    if (dbItem.RelationshipUISetting == null)
                        dbItem.RelationshipUISetting = new RelationshipUISetting();
                    dbItem.RelationshipUISetting.UIColumnsType = (short)item.RelationshipUISetting.UIColumnsType;
                    dbItem.RelationshipUISetting.Expander = item.RelationshipUISetting.Expander;
                    dbItem.RelationshipUISetting.IsExpanded = item.RelationshipUISetting.IsExpanded;
                    dbItem.RelationshipUISetting.UIRowsCount = item.RelationshipUISetting.UIRowsCount;

                }
                else if (item.ObjectCategory == DatabaseObjectCategory.Group)
                {
                    if (dbItem.GroupUISetting == null)
                        dbItem.GroupUISetting = new GroupUISetting();
                    dbItem.GroupUISetting.UIColumnsType = (short)item.GroupUISetting.UIColumnsType;
                    dbItem.GroupUISetting.Expander = item.GroupUISetting.Expander;
                    dbItem.GroupUISetting.IsExpanded = item.GroupUISetting.IsExpanded;

                    dbItem.GroupUISetting.InternalColumnsCount = item.GroupUISetting.InternalColumnsCount;
                    dbItem.GroupUISetting.UIRowsCount = item.GroupUISetting.UIRowsCount;

                }
                else if (item.ObjectCategory == DatabaseObjectCategory.TabControl)
                {
                    if (dbItem.TabGroupUISetting == null)
                        dbItem.TabGroupUISetting = new TabGroupUISetting();
                    dbItem.TabGroupUISetting.UIColumnsType = (short)item.TabGroupUISetting.UIColumnsType;
                    dbItem.TabGroupUISetting.Expander = item.TabGroupUISetting.Expander;
                    dbItem.TabGroupUISetting.IsExpanded = item.TabGroupUISetting.IsExpanded;

                    dbItem.TabGroupUISetting.UIRowsCount = item.TabGroupUISetting.UIRowsCount;

                }
                else if (item.ObjectCategory == DatabaseObjectCategory.TabPage)
                {
                    if (dbItem.TabPageUISetting == null)
                        dbItem.TabPageUISetting = new TabPageUISetting();
                    dbItem.TabPageUISetting.InternalColumnsCount = item.TabPageUISetting.InternalColumnsCount;

                }
                else if (item.ObjectCategory == DatabaseObjectCategory.EmptySpace)
                {
                    if (dbItem.EmptySpaceUISetting == null)
                        dbItem.EmptySpaceUISetting = new EmptySpaceUISetting();
                    dbItem.EmptySpaceUISetting.UIColumnsType = (short)item.EmptySpaceUISetting.UIColumnsType;
                    dbItem.EmptySpaceUISetting.ExtendToEnd = item.EmptySpaceUISetting.ExpandToEnd;

                }
                dbItem.ItemTitle = item.Title;
                dbItem.Position = item.Position;
                dbItem.ItemIdentity = item.ObjectIdentity ?? item.Title;
                dbItem.EntityUIComposition2 = parentDBItem;
                CheckAddItems(entityID, items, item, dbItem, projectContext);
            }
        }

        private void RemoveItem(DataAccess.EntityUIComposition item, DataAccess.MyProjectEntities projectContext)
        {
            foreach (var cItem in item.EntityUIComposition1)
            {
                RemoveItem(cItem, projectContext);
            }
            if (item.ColumnUISetting != null)
                projectContext.ColumnUISetting.Remove(item.ColumnUISetting);
            if (item.RelationshipUISetting != null)
                projectContext.RelationshipUISetting.Remove(item.RelationshipUISetting);
            if (item.TabGroupUISetting != null)
                projectContext.TabGroupUISetting.Remove(item.TabGroupUISetting);
            if (item.EmptySpaceUISetting != null)
                projectContext.EmptySpaceUISetting.Remove(item.EmptySpaceUISetting);
            if (item.GroupUISetting != null)
                projectContext.GroupUISetting.Remove(item.GroupUISetting);
            if (item.TabPageUISetting != null)
                projectContext.TabPageUISetting.Remove(item.TabPageUISetting);
            if (item.EntityUISetting != null)
                projectContext.EntityUISetting.Remove(item.EntityUISetting);
            projectContext.EntityUIComposition.Remove(item);
        }
    }

}
