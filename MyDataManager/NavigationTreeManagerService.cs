

using ModelEntites;
using MyModelManager;

using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyDataManagerService
{
    public class NavigationTreeManagerService
    {
        SecurityHelper securityHelper = new SecurityHelper();
        BizPermission bizPermission = new BizPermission();
        BizRoleSecurity bizRoleSecurity = new BizRoleSecurity();
        public List<NavigationItemDTO> GetNavigationTree(DR_Requester requester)
        {
            BizNavigationTree biz = new BizNavigationTree();

            var fullNavigation = biz.GetFullNavigatoinTree(requester);

            //ValidateTree(request.Requester, fullNavigation.TreeItems);

            var emptyFolders = fullNavigation.TreeItems.Where(x => x.ObjectCategory == DatabaseObjectCategory.Folder && !fullNavigation.TreeItems.Any(y => y.ParentID == x.ID)).ToList();
            foreach (var removeItem in emptyFolders)
                RemoveTreeItem(fullNavigation.TreeItems, removeItem);

            //     result.Structure = new List<DP_PackageTreeStructure>();

            //foreach (var item in context.TableDrivedEntity.Where(x => x.Table.Catalog == request.DatabaseName && x.IndependentDataEntry == true))
            //{
            //    DP_PackageTreeStructure nitem = new DP_PackageTreeStructure();
            //    nitem.Package = item;
            //    if (string.IsNullOrEmpty(item.Alias))
            //        nitem.Name = item.Name;
            //    else
            //        nitem.Name = item.Alias;
            //    result.Structure.Add(nitem);
            //    //result.Structure.Add(DP_PackageTreeStructureDBToND(item));
            //}
            return fullNavigation.TreeItems;

        }

        //private void ValidateTree(DR_Requester requester, List<NavigationItemDTO> treeItems)
        //{
        //    CheckPermissions(requester, treeItems);
        //}

        //private void CheckPermissions(DR_Requester requester, List<NavigationItemDTO> treeItems)
        //{
        //    CheckEntityPermissions(requester, treeItems);

        //}
        //BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        //private void CheckEntityPermissions(DR_Requester requester, List<NavigationItemDTO> treeItems)
        //{
        //    var checkItems = treeItems.Where(x => x.TableDrivedEntityID != 0).ToList();
        //    var checkEntityIDs = checkItems.Select(x => x.TableDrivedEntityID).Distinct().ToList();

        //    var listActions = new List<SecurityAction>() { SecurityAction.NoAccess };
        //    //   var objectIds = checkSecurityItems.Select(x => x.ObjectIdentity).Distinct().ToList();
        //    var permissoinResult = bizTableDrivedEntity.ObjectsHaveSpecificPermissions(requester, checkEntityIDs, listActions);
        //    foreach (var permission in permissoinResult)
        //    {
        //        if (permission.Permitted)
        //        {
        //            foreach (var item in checkItems.Where(x => x.TableDrivedEntityID == permission.ObjectSecurityID))
        //            {
        //                RemoveTreeItem(treeItems, item);
        //            }
        //        }
        //    }

        //}



        private void RemoveTreeItem(List<NavigationItemDTO> treeItems, NavigationItemDTO removeItem)
        {
            var item = treeItems.FirstOrDefault(x => x == removeItem);
            if (item != null)
            {
                foreach (var citem in treeItems.Where(x => x.ParentItem == item).ToList())
                {
                    RemoveTreeItem(treeItems, citem);
                }
                treeItems.Remove(item);
            }
        }





        //public DP_EntityActionActivitiesResult GetEntityActionActivities(DP_EntityActionActivitiesRequest request)
        //{
        //    BizActionActivity bizActionActivity = new BizActionActivity();
        //    DP_EntityActionActivitiesResult result = new DP_EntityActionActivitiesResult();
        //    result.ActionActivities = bizActionActivity.GetActionActivities(request.EntityID,
        //            new List<Enum_EntityActionActivityStep>() { Enum_EntityActionActivityStep.BeforeDelete, Enum_EntityActionActivityStep.BeforeLoad, Enum_EntityActionActivityStep.BeforeSave }, true);
        //    return result;
        //}




    }


}