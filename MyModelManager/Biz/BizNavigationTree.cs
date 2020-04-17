using DataAccess;
using ModelEntites;
using MyGeneralLibrary;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizNavigationTree
    {
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        BizEntityReport bizEntityReport = new BizEntityReport();
        public NavigationTreeDTO GetFullNavigatoinTree(DR_Requester requester)
        {
            NavigationTreeDTO result = new NavigationTreeDTO();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var list = projectContext.NavigationTree.Where(x => x.ParentID == null);
                foreach (var item in list)
                {
                    if (DataIsAccessable(requester, item))
                        ToNavigationTreeDTO(requester, result.TreeItems, item, null, true);
                }
            }
            return result;
        }
        private bool DataIsAccessable(DR_Requester requester, NavigationTree navigationTree)
        {
            if (requester.SkipSecurity)
                return true;
            if (navigationTree.TableDrivedEntityID != null)
            {
                if (!bizTableDrivedEntity.DataIsAccessable(requester, navigationTree.TableDrivedEntity))
                    return false;
            }
            var category = (DatabaseObjectCategory)Enum.Parse(typeof(DatabaseObjectCategory), navigationTree.Category);
            if (category == DatabaseObjectCategory.Report)
            {
                if (!bizEntityReport.DataIsAccessable(requester, navigationTree.ItemIdentity))
                    return false;
            }
            if (category == DatabaseObjectCategory.Letter)
            {
                if (!bizTableDrivedEntity.DataIsAccessable(requester, navigationTree.TableDrivedEntity, new List<SecurityAction>() { SecurityAction.LetterView, SecurityAction.LetterEdit })) 
                    return false;
            }
            if (category == DatabaseObjectCategory.Archive)
            {
                if (!bizTableDrivedEntity.DataIsAccessable(requester, navigationTree.TableDrivedEntity, new List<SecurityAction>() { SecurityAction.ArchiveView, SecurityAction.ArchiveEdit }))
                    return false;
            }
            return true;
        }
        private void ToNavigationTreeDTO(DR_Requester requester, List<ModelEntites.NavigationItemDTO> treeItems, NavigationTree item, NavigationItemDTO parentNavigationItem, bool withChilds)
        {
            if (item.TableDrivedEntityID != null)
            {
                var entityEnabled = bizTableDrivedEntity.IsEntityEnabled(item.TableDrivedEntityID.Value);
                if (!entityEnabled)
                    return;
            }
            var result = new NavigationItemDTO();
            result.ID = item.ID;
            result.ParentID = item.ParentID;
            result.ObjectIdentity = item.ItemIdentity;
            result.Title = item.ItemTitle;
            result.TableDrivedEntityID = item.TableDrivedEntityID ?? 0;
            result.Tooltip = item.Tooltip;
            result.Name = item.Name;
            result.ObjectCategory = (DatabaseObjectCategory)Enum.Parse(typeof(DatabaseObjectCategory), item.Category);
            result.ParentItem = parentNavigationItem;
            //if (!plainItems.Any(x => x.ObjectIdentity == item.ItemIdentity && x.ObjectCategory.ToString() == item.Category))
            //{
            //    var plainItem = new PlainNavigationDTO();
            //    plainItem.ObjectCategory = (DatabaseObjectCategory)Enum.Parse(typeof(DatabaseObjectCategory), item.Category);
            //    plainItem.ObjectIdentity = item.ItemIdentity;
            //    if (item.TableDrivedEntityID != null)
            //        plainItem.SecurityObjectID = item.TableDrivedEntityID.Value;
            //    else if (item.EntityReportID != null)
            //        plainItem.SecurityObjectID = item.EntityReportID.Value;
            //    plainItems.Add(plainItem);
            //}
            treeItems.Add(result);
            if (withChilds)
            {
                //result.ChildItems = new List<NavigationItemDTO>();
                foreach (var citem in item.NavigationTree1)
                {


                     if (DataIsAccessable(requester, citem))
                         ToNavigationTreeDTO(requester, treeItems, citem, result, true);
                }
            }

        }

        //public List<NavigationTreeDTO> GetNavigatoinTree(int? parentID)
        //{
        //    List<NavigationTreeDTO> result = new List<NavigationTreeDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var list = projectContext.NavigationTree.Where(x => x.ParentID == parentID);
        //        foreach (var item in list)
        //        {
        //            result.Add(ToNavigationTreeDTO(item, false));
        //        }
        //    }
        //    return result;
        //}

        //private NavigationTreeDTO ToNavigationTreeDTO(DataAccess.NavigationTree item,bool withChilds)
        //{

        //}

        public void Save(List<NavigationItemDTO> items)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var existingIds = items.Where(x => x.ID != 0).Select(x => x.ID).ToList();
                if (existingIds.Count > 0)
                {
                    var removeList = projectContext.NavigationTree.Where(x => !existingIds.Contains(x.ID));
                    foreach (var item in removeList.ToList())
                        RemoveItem(item, projectContext);
                }
                CheckAddItems(items, null, null, projectContext);
                projectContext.SaveChanges();
            }

        }

        private void CheckAddItems(List<NavigationItemDTO> items, NavigationItemDTO parentItem, NavigationTree parentDBItem, DataAccess.MyProjectEntities projectContext)
        {
            foreach (var item in items.Where(x => x.ParentItem == parentItem))
            {
                NavigationTree dbItem = null;
                if (item.ID != 0)
                {
                    dbItem = projectContext.NavigationTree.First(x => x.ID == item.ID);
                }
                else
                {
                    dbItem = new NavigationTree();
                    projectContext.NavigationTree.Add(dbItem);
                }
                dbItem.Category = item.ObjectCategory.ToString();
                dbItem.ItemTitle = item.Title;
                dbItem.Tooltip = item.Tooltip;
                dbItem.Name = item.Name;
                dbItem.ItemIdentity = item.ObjectIdentity;
                if (item.TableDrivedEntityID != 0)
                    dbItem.TableDrivedEntityID = item.TableDrivedEntityID;
                else
                    dbItem.TableDrivedEntityID = null;
                //if (item.EntityReportID != 0)
                //    dbItem.EntityReportID = item.EntityReportID;
                //else
                //    dbItem.EntityReportID = null;
                //if (item.ObjectCategory == DatabaseObjectCategory.Entity)
                //    dbItem.TableDrivedEntityID = Convert.ToInt32(item.ObjectIdentity);
                //else if (item.ObjectCategory == DatabaseObjectCategory.Report)
                //    dbItem.EntityReportID = Convert.ToInt32(item.ObjectIdentity);

                dbItem.NavigationTree2 = parentDBItem;
                CheckAddItems(items, item, dbItem, projectContext);
            }
        }

        private void RemoveItem(DataAccess.NavigationTree item, DataAccess.MyProjectEntities projectContext)
        {
            foreach (var cItem in item.NavigationTree1.ToList())
            {
                RemoveItem(cItem, projectContext);
            }
            projectContext.NavigationTree.Remove(item);
        }

        public bool HasEntityNotInNavigationTree(int databaseID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                return projectContext.TableDrivedEntity.Any(x => x.IsOrginal == true && x.IsView == false && x.IsDisabled == false && x.Table.DBSchema.DatabaseInformationID == databaseID && x.IndependentDataEntry == true && !projectContext.NavigationTree.Any(y => y.Category == "Entity" && y.ItemIdentity == x.ID));
            }
        }
    }

}
