using DataAccess;
using ModelEntites;
using MyCacheManager;
using MyDatabaseToObject;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySecurity
{
    public class SecurityHelper
    {
      

        private List<SecurityAction> GetPersmissionByPosts(MyProjectEntities context, IQueryable<OrganizationPost> posts, SecurityObject securityObject, bool goUpward = true)
        {
            var actions = new List<SecurityAction>();
            //بهتر است راه حل کلی برای ذخیره دسترسی های کلی تر مانند نوع سازمان و یا آبجکتهای بالاتر مانن دیتابیس پیاده شود که هر دفعه خوانده نشوند
            var objectCategory = (DatabaseObjectCategory)securityObject.Type;
            var possibleActionTree = GetActionsByCategory(objectCategory);
            List<List<SecurityAction>> AllPostAccess = new List<List<SecurityAction>>();
            foreach (var post in posts)
            {
                List<SecurityAction> postAccess;
                var postActions = GetPersmissionByPost(context, post.SecuritySubject, securityObject, 0, goUpward);
                var finalPostActions = GetPossibleActions(postActions, GetActionsByCategory(objectCategory));

                //اولویت دسترسی های تعریف شده برای پست بالاتر از همه است
                if (finalPostActions.Any())
                    postAccess = finalPostActions;
                else
                {
                    var orgTypeRoleTypeActions = GetPersmissionByPost(context, post.OrganizationType_RoleType.SecuritySubject, securityObject, 0, goUpward);
                    var organizationActions = GetPersmissionByPost(context, post.Organization.SecuritySubject, securityObject, 0, goUpward);
                    var finalOrgTypeRoleTypeActions = GetPossibleActions(orgTypeRoleTypeActions, GetActionsByCategory(objectCategory));
                    var finalOrganizationActions = GetPossibleActions(organizationActions, GetActionsByCategory(objectCategory));
                    if (finalOrgTypeRoleTypeActions.Any())
                    {
                        //جمع سازمان و نوع نقش/نوع سازمان که همسطح هستند
                        postAccess = Combination(new List<List<SecurityAction>>() { finalOrgTypeRoleTypeActions, finalOrganizationActions }, possibleActionTree);
                    }
                    else
                    {
                        var roleTypeActions = GetPersmissionByPost(context, post.OrganizationType_RoleType.RoleType.SecuritySubject, securityObject, 0, goUpward);
                        var finalRoleTypeActions = GetPossibleActions(roleTypeActions, GetActionsByCategory(objectCategory));
                        if (finalOrganizationActions.Any())
                        {
                            //ادغام سازمان و نوع نقش
                            //اداغام دسترسی ها موازی برای موضوعات موازی 
                            postAccess = Combination(new List<List<SecurityAction>>() { finalOrganizationActions, finalRoleTypeActions }, possibleActionTree);
                        }
                        else
                        {
                            //ادغام نوع سازمان و نوع نقش

                            var organizationTypeActions = GetPersmissionByPost(context, post.OrganizationType_RoleType.OrganizationType.SecuritySubject, securityObject, 0, goUpward);
                            var finalOrganizationTypeActions = GetPossibleActions(organizationTypeActions, GetActionsByCategory(objectCategory));
                            postAccess = Combination(new List<List<SecurityAction>>() { finalRoleTypeActions, finalOrganizationTypeActions }, possibleActionTree);
                        }
                    }
                }
                AllPostAccess.Add(postAccess);
            }
            return Combination(AllPostAccess, possibleActionTree);
        }

        private List<SecurityAction> Combination(List<List<SecurityAction>> listActions, List<SecurityActionTreeItem> possibleActionTree, List<SecurityAction> result = null)
        {//وقتی لیست دسترسی ها هم سطح هستند
            if (result == null)
                result = new List<SecurityAction>();
            foreach (var possibleAction in possibleActionTree)
            {
                if (listActions.Any(x => x.Any(y => y == possibleAction.Action)))
                {
                    bool childAccessExists = false;
                    foreach (var listActoin in listActions)
                    {
                        if (AnySubAccessAssigned(listActions, possibleAction.ChildItems))
                        {
                            childAccessExists = true;
                            break;
                        }
                    }
                    if (childAccessExists)
                        Combination(listActions, possibleAction.ChildItems, result);
                    else if (!result.Any(x => x == possibleAction.Action))
                        result.Add(possibleAction.Action);
                }
                else
                    Combination(listActions, possibleAction.ChildItems, result);

            }
            return result;
        }

        private bool AnySubAccessAssigned(List<List<SecurityAction>> listActions, List<SecurityActionTreeItem> childItems)
        {
            foreach (var possibleAction in childItems)
            {
                if (listActions.Any(x => x.Any(y => y == possibleAction.Action)))
                    return true;
                else
                {
                    var result = AnySubAccessAssigned(listActions, possibleAction.ChildItems);
                    if (result)
                        return true;
                }
            }
            return false;
        }

        private List<SecurityAction> GetPossibleActions(List<Tuple<int, SecurityAction>> postActions, List<SecurityActionTreeItem> possibleActionTree, List<SecurityAction> result = null)
        {

            //نتیجه یک موضوع را تعیین میکند.هم نو اکسس و هم آپدیت غیر ممکن است
            if (result == null)
                result = new List<SecurityAction>();
            List<SecurityActionTreeItem> ignorableActions = new List<SecurityActionTreeItem>();
            for (int i = 0; i <= 5; i++)
            {
                if (possibleActionTree.Any())
                {
                    var actionTreeItems = GetPossibleActions(i, postActions, possibleActionTree, ignorableActions);
                    foreach (var actionTreeItem in actionTreeItems)
                    {
                        result.Add(actionTreeItem.Action);
                        if (actionTreeItem.ParentItem != null)
                            actionTreeItem.ParentItem.ChildItems.Remove(actionTreeItem);
                        else
                            possibleActionTree.Remove(actionTreeItem);

                        //RemoveActionAndChilds(levelAction, possibleActionTree);
                        SetParentsAsIgnorableActions(ignorableActions, actionTreeItem);
                    }
                }
                else
                    break;
            }
            return result;
        }

        private void SetParentsAsIgnorableActions(List<SecurityActionTreeItem> ignorableActions, SecurityActionTreeItem actionTreeItem)
        {
            if (actionTreeItem != null)
            {
                ignorableActions.Add(actionTreeItem.ParentItem);
                SetParentsAsIgnorableActions(ignorableActions, actionTreeItem.ParentItem);
            }
        }
        private List<SecurityActionTreeItem> GetPossibleActions(int i, List<Tuple<int, SecurityAction>> postActions, List<SecurityActionTreeItem> possibleActionTree, List<SecurityActionTreeItem> ignorableActions, List<SecurityActionTreeItem> result = null)
        {
            if (result == null)
                result = new List<SecurityActionTreeItem>();
            foreach (var possibleAction in possibleActionTree)
            {
                if (!ignorableActions.Contains(possibleAction))
                {
                    if (postActions.Any(x => x.Item1 == i && x.Item2 == possibleAction.Action))
                    {
                        result.Add(possibleAction);
                    }
                    else
                    {
                        if (possibleAction.ChildItems.Any())
                            GetPossibleActions(i, postActions, possibleAction.ChildItems, ignorableActions, result);
                    }
                }
                else
                {
                    if (possibleAction.ChildItems.Any())
                        GetPossibleActions(i, postActions, possibleAction.ChildItems, ignorableActions, result);
                }
            }
            return result;
        }

        private List<Tuple<int, SecurityAction>> GetPersmissionByPost(MyProjectEntities context, SecuritySubject securitySubject, SecurityObject securityObject, int level, bool goUpward = true, List<Tuple<int, SecurityAction>> distantactions = null)
        {
            if (distantactions == null)
                distantactions = new List<Tuple<int, SecurityAction>>();
            //کش شود cache
            //if(cachedItem!=null)


            var actions = GetAssignedPermissions(context, securityObject, securitySubject);
            foreach (var action in actions)
            {
                distantactions.Add(new Tuple<int, SecurityAction>(level, action));
            }

            if (goUpward == true)
            {
                SecurityObject parentSecurityObject = null;
                var type = (DatabaseObjectCategory)securityObject.Type;
                if (type == DatabaseObjectCategory.Schema)
                    parentSecurityObject = securityObject.DBSchema.DatabaseInformation.SecurityObject;
                else if (type == DatabaseObjectCategory.Entity)
                    parentSecurityObject = securityObject.TableDrivedEntity.Table.DBSchema.SecurityObject;
                //else if (securityObject.ColumnID != null)
                //    parentSecurityObject = securityObject.Column..Table.DBSchema.SecurityObject.First();
                if (parentSecurityObject != null)
                    GetPersmissionByPost(context, securitySubject, parentSecurityObject, level + 1, goUpward, distantactions);
                //else
                //     return new List<ActionDTO>();
            }



            return distantactions;
        }

        private List<SecurityAction> GetAssignedPermissions(MyProjectEntities context, SecurityObject securityObject, SecuritySubject securitySubject)
        {
            //var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.Permission, securitySubject.ID + "_" + securityObject.ID);
            //if (cachedItem != null)
            //    return cachedItem as List<SecurityAction>;
            List<SecurityAction> result = new List<SecurityAction>();
            //RoleType roleType = null;
            //if (securitySubject.OrganizationType_RoleType != null)
            //    roleType = securitySubject.OrganizationType_RoleType.RoleType;
            //else if (securitySubject.RoleType != null)
            //    roleType = securitySubject.RoleType;
            //else if (securitySubject.OrganizationPost != null)
            //    roleType = securitySubject.OrganizationPost.OrganizationType_RoleType.RoleType;

            //if (roleType!=null &&roleType.IsSuperAdmin == true)
            //    {
            //        result.Add(SecurityAction.ArchiveEdit);
            //        result.Add(SecurityAction.ArchiveMenuAccess);
            //        result.Add(SecurityAction.ArchiveView);
            //        result.Add(SecurityAction.EditAndDelete);
            //        result.Add(SecurityAction.MenuAccess);


            //    }
            //    else
            //    {
            var dbPermission = (securitySubject.Permission.FirstOrDefault(p => p.SecurityObjectID == securityObject.ID));
            if (dbPermission != null)
            {
                foreach (var dbAction in dbPermission.Permission_Action)
                {
                    var dbActionType = (SecurityAction)Enum.Parse(typeof(SecurityAction), dbAction.Action);
                    result.Add(dbActionType);
                }
            }
            //}
            CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.Permission, securitySubject.ID + "_" + securityObject.ID);
            return result;
        }

        public static List<SecurityActionTreeItem> GetActionsByCategory(DatabaseObjectCategory category)
        {
            if (category == DatabaseObjectCategory.Database)
                return DefaultActionTree();
            else if (category == DatabaseObjectCategory.Schema)
                return DefaultActionTree();
            else if (category == DatabaseObjectCategory.Entity)
                return DefaultActionTree();
            else if (category == DatabaseObjectCategory.Column)
                return ColumnActoinTree();
            else if (category == DatabaseObjectCategory.Relationship)
                return RelationshipActoinTree();
            else if (category == DatabaseObjectCategory.Command)
                return CommandActionTree();
            else if (category == DatabaseObjectCategory.Archive)
                return ArchiveActionTree();
            else if (category == DatabaseObjectCategory.Report)
                return ReportActionTree();
            else
                return new List<SecurityActionTreeItem>();
        }

        private static List<SecurityActionTreeItem> ArchiveActionTree()
        {
            List<SecurityActionTreeItem> result = new List<SecurityActionTreeItem>();
            SecurityActionTreeItem noaccess = new SecurityActionTreeItem();
            noaccess.Action = SecurityAction.NoAccess;
            result.Add(noaccess);

            SecurityActionTreeItem readonlyaccess = new SecurityActionTreeItem();
            readonlyaccess.Action = SecurityAction.ReadOnly;
            readonlyaccess.ParentItem = noaccess;
            noaccess.ChildItems.Add(readonlyaccess);

            SecurityActionTreeItem editAndDelete = new SecurityActionTreeItem();
            editAndDelete.Action = SecurityAction.EditAndDelete;
            editAndDelete.ParentItem = readonlyaccess;
            readonlyaccess.ChildItems.Add(editAndDelete);
            return result;
        }

        private static List<SecurityActionTreeItem> CommandActionTree()
        {
            List<SecurityActionTreeItem> result = new List<SecurityActionTreeItem>();

            SecurityActionTreeItem noaccess = new SecurityActionTreeItem();
            noaccess.Action = SecurityAction.NoAccess;
            result.Add(noaccess);


            SecurityActionTreeItem access = new SecurityActionTreeItem();
            access.Action = SecurityAction.Access;
            access.ParentItem = noaccess;
            noaccess.ChildItems.Add(access);
            return result;
        }
        private static List<SecurityActionTreeItem> ReportActionTree()
        {
            List<SecurityActionTreeItem> result = new List<SecurityActionTreeItem>();

            SecurityActionTreeItem noaccess = new SecurityActionTreeItem();
            noaccess.Action = SecurityAction.NoAccess;
            result.Add(noaccess);


            SecurityActionTreeItem access = new SecurityActionTreeItem();
            access.Action = SecurityAction.Access;
            access.ParentItem = noaccess;
            noaccess.ChildItems.Add(access);
            return result;
        }
        private static List<SecurityActionTreeItem> ColumnActoinTree()
        {
            List<SecurityActionTreeItem> result = new List<SecurityActionTreeItem>();

            SecurityActionTreeItem noaccess = new SecurityActionTreeItem();
            noaccess.Action = SecurityAction.NoAccess;
            result.Add(noaccess);

            SecurityActionTreeItem readonlyaccess = new SecurityActionTreeItem();
            readonlyaccess.Action = SecurityAction.ReadOnly;
            readonlyaccess.ParentItem = noaccess;
            noaccess.ChildItems.Add(readonlyaccess);

            //SecurityActionTreeItem editaccess = new SecurityActionTreeItem();
            //editaccess.Action = SecurityAction.Edit;
            //editaccess.ParentItem = readonlyaccess;
            //readonlyaccess.ChildItems.Add(editaccess);

            return result;

        }
        private static List<SecurityActionTreeItem> RelationshipActoinTree()
        {
            List<SecurityActionTreeItem> result = new List<SecurityActionTreeItem>();

            SecurityActionTreeItem noaccess = new SecurityActionTreeItem();
            noaccess.Action = SecurityAction.NoAccess;
            result.Add(noaccess);

            SecurityActionTreeItem readonlyaccess = new SecurityActionTreeItem();
            readonlyaccess.Action = SecurityAction.ReadOnly;
            readonlyaccess.ParentItem = noaccess;
            noaccess.ChildItems.Add(readonlyaccess);

            //SecurityActionTreeItem editaccess = new SecurityActionTreeItem();
            //editaccess.Action = SecurityAction.Edit;
            //editaccess.ParentItem = readonlyaccess;
            //readonlyaccess.ChildItems.Add(editaccess);

            return result;

        }
        private static List<SecurityActionTreeItem> DefaultActionTree()
        {
            List<SecurityActionTreeItem> result = new List<SecurityActionTreeItem>();

            SecurityActionTreeItem noaccess = new SecurityActionTreeItem();
            noaccess.Action = SecurityAction.NoAccess;
            result.Add(noaccess);

            //SecurityActionTreeItem menuaccess = new SecurityActionTreeItem();
            //menuaccess.Action = SecurityAction.MenuAccess;
            //menuaccess.ParentItem = noaccess;
            //noaccess.ChildItems.Add(menuaccess);

            //SecurityActionTreeItem archiveMenuaccess = new SecurityActionTreeItem();
            //archiveMenuaccess.Action = SecurityAction.ArchiveMenuAccess;
            //archiveMenuaccess.ParentItem = noaccess;
            //noaccess.ChildItems.Add(archiveMenuaccess);

            SecurityActionTreeItem readonlyaccess = new SecurityActionTreeItem();
            readonlyaccess.Action = SecurityAction.ReadOnly;
            readonlyaccess.ParentItem = noaccess;
            noaccess.ChildItems.Add(readonlyaccess);

            SecurityActionTreeItem editonlyaccess = new SecurityActionTreeItem();
            editonlyaccess.Action = SecurityAction.Edit;
            editonlyaccess.ParentItem = readonlyaccess;
            readonlyaccess.ChildItems.Add(editonlyaccess);

            SecurityActionTreeItem editanddeleteaccess = new SecurityActionTreeItem();
            editanddeleteaccess.Action = SecurityAction.EditAndDelete;
            editanddeleteaccess.ParentItem = editonlyaccess;
            editonlyaccess.ChildItems.Add(editanddeleteaccess);


            SecurityActionTreeItem archiveviewaccess = new SecurityActionTreeItem();
            archiveviewaccess.Action = SecurityAction.ArchiveView;
            archiveviewaccess.ParentItem = noaccess;
            noaccess.ChildItems.Add(archiveviewaccess);

            SecurityActionTreeItem archiveeditaccess = new SecurityActionTreeItem();
            archiveeditaccess.Action = SecurityAction.ArchiveEdit;
            archiveeditaccess.ParentItem = archiveviewaccess;
            archiveviewaccess.ChildItems.Add(archiveeditaccess);



            SecurityActionTreeItem letterviewaccess = new SecurityActionTreeItem();
            letterviewaccess.Action = SecurityAction.LetterView;
            letterviewaccess.ParentItem = noaccess;
            noaccess.ChildItems.Add(letterviewaccess);

            SecurityActionTreeItem lettereditaccess = new SecurityActionTreeItem();
            lettereditaccess.Action = SecurityAction.LetterEdit;
            lettereditaccess.ParentItem = letterviewaccess;
            letterviewaccess.ChildItems.Add(lettereditaccess);


            return result;
            //new List<SecurityAction>() { SecurityAction.NoAccess, SecurityAction.MenuAccess, SecurityAction.ReadOnly, SecurityAction.Delete, SecurityAction.Edit };
        }



        //private bool? UserHasPermission(MyProjectEntities context, int userID, string ActionName, Object dbObject)
        //{
        //    var user = context.Users.First(x => x.ID == userID);
        //    foreach (var role in user.Roles)
        //    {

        //        var bFound = (role.Role_Action_Object.Any(
        //                      p => (ActionName == "any" || p.Action.ActionName == ActionName) && p.ObjectID == dbObject.ID));
        //        if (bFound)
        //            return true;
        //        else
        //        {
        //            if (dbObject.NeedsExplicitPermission == true)
        //                return false;
        //            else
        //            {
        //                if (dbObject.ParentID != null)
        //                    return UserHasPermission(context, userID, ActionName, dbObject.Object2);
        //                else
        //                    return false;
        //            }
        //        }
        //    }
        //    return false;
        //}

        //////public bool UserHasRole(int userID, string roleName)
        //////{
        //////    var context = new MyProjectEntities();
        //////    var user = context.Users.First(x => x.ID == userID);
        //////    return user.Roles.Any(p => p.RoleName == roleName);
        //////}

        //public bool? PermissionGranted(List<int> roleIds, string actionName, string objectIdentity, string objectCategory)
        //{


        //}
        public List<ImposePermissionResult> ObjectsHaveSpecificPermissions(DR_Requester requester, List<int> securityObjectIDs, List<SecurityAction> actionNames)
        {
            List<ImposePermissionResult> result = new List<MySecurity.ImposePermissionResult>();

            using (var context = new MyProjectEntities())
            {
                //var user = context.Users.First(x => x.ID == requester.Identity);
                IQueryable<OrganizationPost> organizationPosts = GetDBOrganizationPosts(context, requester);



                foreach (var securityObjectID in securityObjectIDs)
                {
                    SecurityObject securityObject = context.SecurityObject.First(x => x.ID == securityObjectID);

                    var permissoins = GetPersmissionByPosts(context, organizationPosts, securityObject);
                    ImposePermissionResult item = new MySecurity.ImposePermissionResult();
                    item.Permitted = permissoins.Any(x => actionNames.Contains(x));
                    item.ObjectSecurityID = securityObjectID;
                    result.Add(item);
                    //if (actionName == SecurityAction.Any)
                    //    item.Permitted = permissoins.Any();
                    //else
                    //    item.Permitted = permissoins.Any(x => x.Action == actionName);

                }
                //    if (dbObject != null)
                //        return UserHasPermission(context, requester.Identity, ActionName, dbObject);
                //    else
                //        return null;
            }
            return result;
        }





        //private IQueryable<ConditionalPermission> GetConditionalPermissions(MyProjectEntities context, int entityID, int securitySubjectID)
        //{
        //    return 
        //}

        public AssignedPermissionDTO GetAssignedPermissions(DR_Requester requester, int securityObjectID, bool withChildObjects)
        {
            AssignedPermissionDTO result = new AssignedPermissionDTO();
            //var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.Permission, requester.Identity.ToString(), securityObjectID.ToString(), withChildObjects.ToString());
            //if (cachedItem != null)
            //    return (cachedItem as AssignedPermissionDTO);
            using (var context = new MyProjectEntities())
            {
                SecurityObject securityObject = context.SecurityObject.First(x => x.ID == securityObjectID);

                var organizationPosts = GetDBOrganizationPosts(context, requester);
                //if (organizationPosts.Any(x => x.OrganizationType_RoleType.RoleType.IsSuperAdmin == true))
                //    requester.SkipSecurity = true;
                if (requester.SkipSecurity == true)
                {
                    result.GrantedActions.Add(SecurityAction.ArchiveEdit);
                    result.GrantedActions.Add(SecurityAction.LetterEdit);
                    //result.GrantedActions.Add(SecurityAction.ArchiveMenuAccess);
                    result.GrantedActions.Add(SecurityAction.EditAndDelete);
                    //result.GrantedActions.Add(SecurityAction.MenuAccess);

                }
                else
                {
                    var allowedActions = GetPersmissionByPosts(context, organizationPosts, securityObject, true);
                    result.GrantedActions = allowedActions;
                    result.SecurityObjectID = securityObjectID;

                }
                if (withChildObjects)
                {
                    var childObjects = GetChildObjects(securityObject);
                    if (childObjects != null)
                    {
                        SetChildPermissions(requester,context, result, childObjects, organizationPosts);
                    }
                }
                // result = GetAssignedPermissions(context, organizationPosts, securityObject, withChildObjects);
            }
            CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.Permission, requester.Identity.ToString(), securityObjectID.ToString(), withChildObjects.ToString());
            return result;
        }

        private void SetChildPermissions(DR_Requester requester,MyProjectEntities context, AssignedPermissionDTO parentItem, List<SecurityObject> childObjects, IQueryable<OrganizationPost> posts)
        {
            foreach (var childObject in childObjects)
            {
                AssignedPermissionDTO assignedPermissionDTO = new AssignedPermissionDTO();
                assignedPermissionDTO.SecurityObjectID = childObject.ID;
                assignedPermissionDTO.SecurityObjectType = (DatabaseObjectCategory)childObject.Type;
                var objectCategory = (DatabaseObjectCategory)childObject.Type;
                var possibleActionTree = GetActionsByCategory(objectCategory);
                List<SecurityAction> permissions = null;
                if (requester.SkipSecurity)
                {


                }
                else
                    permissions = GetPersmissionByPosts(context, posts, childObject, false);
                if (permissions != null && permissions.Any() && parentItem.GrantedActions.Any())
                {
                    List<Tuple<int, SecurityAction>> distantChildParentPermissions = new List<Tuple<int, SecurityAction>>();
                    foreach (var permission in permissions)
                    {
                        distantChildParentPermissions.Add(new Tuple<int, SecurityAction>(0, permission));
                    }
                    foreach (var permission in parentItem.GrantedActions)
                    {
                        distantChildParentPermissions.Add(new Tuple<int, SecurityAction>(1, permission));
                    }
                    assignedPermissionDTO.GrantedActions = GetPossibleActions(distantChildParentPermissions, possibleActionTree);
                }
                else if (permissions != null && permissions.Any())
                    assignedPermissionDTO.GrantedActions = permissions;
                else if (parentItem.GrantedActions.Any())
                    assignedPermissionDTO.GrantedActions = parentItem.GrantedActions;

                parentItem.ChildsPermissions.Add(assignedPermissionDTO);
                var newchildObjects = GetChildObjects(childObject);
                if (newchildObjects != null)
                {
                    SetChildPermissions(requester,context, assignedPermissionDTO, newchildObjects, posts);
                }
            }
        }

        //private AssignedPermissionDTO GetAssignedPermissions(MyProjectEntities context, IQueryable<OrganizationPost> posts, SecurityObject securityObject, bool withChildObjects, List<SecurityAction> parentActions = null)
        //{
        //    AssignedPermissionDTO result = new AssignedPermissionDTO();
        //    var permissions = GetPersmissionByPosts(context, posts, securityObject, (parentActions == null));
        //    result.SecurityObjectID = securityObject.ID;
        //    if (permissions != null && permissions.Count > 0)
        //    {
        //        foreach (var permission in permissions)
        //        {
        //            result.GrantedActions.Add(permission);
        //        }
        //    }
        //    if (parentActions != null && parentActions.Count > 0)
        //    {
        //        foreach (var parentAction in parentActions)
        //        {
        //            bool add = false;
        //            if (parentAction == SecurityAction.NoAccess)
        //            {
        //                if (!result.GrantedActions.Any(x => x != SecurityAction.NoAccess))
        //                {
        //                    add = true;
        //                }
        //            }
        //            else if (parentAction == SecurityAction.ReadOnly)
        //            {
        //                if (!result.GrantedActions.Any(x => x != SecurityAction.ReadOnly))
        //                {
        //                    add = true;
        //                }
        //            }
        //            else
        //            {
        //                if (!result.GrantedActions.Any(x => x == SecurityAction.NoAccess
        //                || x == SecurityAction.ReadOnly))
        //                {
        //                    add = true;
        //                }
        //            }
        //            if (add)
        //            {
        //                if (!result.GrantedActions.Any(x => x == parentAction))
        //                    result.GrantedActions.Add(parentAction);
        //            }
        //        }
        //    }

        //    if (!withChildObjects)
        //    {
        //        return result;
        //    }
        //    else
        //    {


        //    }



        //    return result;

        //}
        private List<SecurityObject> GetChildObjects(SecurityObject securityObject)
        {
            List<SecurityObject> result = null;
            DatabaseObjectCategory type = (DatabaseObjectCategory)securityObject.Type;
            if (type == DatabaseObjectCategory.Database)
                result = securityObject.DatabaseInformation.DBSchema.Select(x => x.SecurityObject).ToList();
            else if (type == DatabaseObjectCategory.Schema)
                result = securityObject.DatabaseInformation.DBSchema.Select(x => x.SecurityObject).ToList();
            else if (type == DatabaseObjectCategory.Entity)
            {//گزارش و ستون چی؟

                if (securityObject.TableDrivedEntity.TableDrivedEntity_Columns.Any())
                    result = securityObject.TableDrivedEntity.TableDrivedEntity_Columns.Select(x => x.Column.SecurityObject).ToList();
                else
                    result = securityObject.TableDrivedEntity.Table.Column.Select(x => x.SecurityObject).ToList();

                result.AddRange(securityObject.TableDrivedEntity.Relationship.Select(x => x.SecurityObject).ToList());
            }
            return result;
        }
        private IQueryable<OrganizationPost> GetDBOrganizationPosts(MyProjectEntities context, DR_Requester requester)
        {

            if (requester.PostIds != null && requester.PostIds.Any())
            {
                return context.OrganizationPost.Where(x => requester.PostIds.Contains(x.ID));
            }
            else
            {
                //BizOrganization bizOrganization = new BizOrganization();
                return context.OrganizationPost.Where(x => x.UserID == requester.Identity);
            }
        }
        //public List<OrganizationPostDTO> GetDBOrganizationPosts(DR_Requester requester)
        //{
        //    List<OrganizationPostDTO> result = new List<OrganizationPostDTO>();
        //    BizOrganization bizOrganization = new MySecurity.BizOrganization();

        //    using (var context = new MyProjectEntities())
        //    {
        //        var posts = GetDBOrganizationPosts(context, requester);
        //        foreach (var dbpost in posts)
        //        {
        //            result.Add(bizOrganization.ToOrganizationPostDTO(dbpost));
        //        }
        //    }
        //    return result;
        //}
        //private void GetChildPermissions(MyProjectEntities context, List<Role> roles, List<AssignedPermissionDTO> parentCollection, DatabaseObjectCategory objectCategory, string objectIdentity, List<ActionDTO> parentPermissions = null)
        //{

        //    AssignedPermissionDTO result = new AssignedPermissionDTO();
        //    result.ObjectCategory = objectCategory;
        //    result.ObjectIdentity = objectIdentity.ToString();

        //    var getPersmissionFromParent = !bizDatabase.GetUpwardCondition(objectCategory, Convert.ToInt32(objectIdentity));
        //    if (getPersmissionFromParent == true && parentPermissions != null)
        //        parentPermissions.ForEach(x => result.GrantedActions.Add(x));
        //    var permissions = GetPersmissionByRoles(context, roles, objectCategory, objectIdentity.ToString());
        //    if (permissions.Any())
        //    {
        //        foreach (var permission in permissions)
        //        {
        //            if (!result.GrantedActions.Any(x => x.Action == permission.Action))
        //            {
        //                ActionDTO action = new ActionDTO();
        //                action.Action = permission.Action;
        //                result.GrantedActions.Add(action);
        //            }
        //        }
        //    }
        //    parentCollection.Add(result);

        //    List<ObjectDTO> childObjects = null;
        //    if (objectCategory == DatabaseObjectCategory.Database || objectCategory == DatabaseObjectCategory.Schema || objectCategory == DatabaseObjectCategory.Entity || objectCategory == DatabaseObjectCategory.Column)
        //    {

        //        childObjects = bizDatabase.GetDatabaseChildObjects(objectCategory, Convert.ToInt32(objectIdentity));
        //    }
        //    if (childObjects != null)
        //    {
        //        foreach (var childObject in childObjects)
        //        {
        //            GetChildPermissions(context, roles, result.ChildsPermissions, childObject.ObjectCategory, childObject.ObjectIdentity, result.GrantedActions);
        //        }
        //    }


        //}

        public bool EntityHasSecurity(int entityID, SecurityMode securityMode)
        {
            using (var context = new MyProjectEntities())
            {
                var disrectSecurities = context.EntitySecurityDirect.Where(x => x.TableDrivedEntityID == entityID && x.Mode == (short)securityMode);
                if (disrectSecurities.Any())
                {
                    return true;
                }
                else
                {
                    var indisrectSecurity = context.EntitySecurityInDirect.FirstOrDefault(x => x.TableDrivedEntityID == entityID);
                    if (indisrectSecurity == null)
                        return false;
                    else
                    {
                        var targetEntity = indisrectSecurity.EntityRelationshipTail.TableDrivedEntity;
                        return targetEntity.EntitySecurityDirect.Any(x => x.Mode == (short)securityMode);
                    }
                }
            }
        }




        //private List<EntitySecurityDirectDTO> ToDirectSecurities(IQueryable<EntitySecurityDirect> postDisrectSecurities)
        //{

        //}

        public LoginResult Login(string userName, string passWord)
        {
            LoginResult restul = new LoginResult();
            using (var context = new MyProjectEntities())
            {
                var user = context.User.FirstOrDefault(x => x.UserName == userName && x.Password == passWord);
                if (user != null)
                {
                    restul.Successful = true;
                    restul.UserID = user.ID;
                }
                else
                {
                    restul.Message = "کاربری با چنین مشخصات موجود نمیباشد";
                }
            }
            return restul;
        }

        public AssignedPermissionDTO GetSubSystemAssignedPermissions(DR_Requester requester, string objectName)
        {
            AssignedPermissionDTO result = new AssignedPermissionDTO();
            var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.Permission, requester.Identity.ToString(), objectName);
            if (cachedItem != null)
                return (cachedItem as AssignedPermissionDTO);
            using (var context = new MyProjectEntities())
            {
                var organizationPosts = GetDBOrganizationPosts(context, requester);
                SecurityObject securityObject = context.SubSystems.FirstOrDefault(x => x.Name == objectName)?.SecurityObject;
                if (securityObject != null)
                {
                    var allowedActions = GetPersmissionByPosts(context, organizationPosts, securityObject, true);
                    result.GrantedActions = allowedActions;
                    result.SecurityObjectID = securityObject.ID;
                }

            }
            CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.Permission, requester.Identity.ToString(), objectName);
            return result;
        }







        //public List<RoleDTO> GetAllOrganizationRoles(int organizationID)
        //{
        //    using (var context = new MyProjectEntities())
        //    {
        //        var roles = context.Role.Where(x => x.OrganizationUser_Role.Any(y => y.Organization_User.OrganizationID == organizationID));
        //        BizRole bizRole = new MySecurity.BizRole();
        //        return bizRole.ToRoleDTOList(roles.ToList());
        //    }
        //}
        //public List<int> GetAllOrganizationRoleIds(int organizationID)
        //{
        //    using (var context = new MyProjectEntities())
        //    {
        //        var roles = context.Role.Where(x => x.OrganizationUser_Role.Any(y => y.Organization_User.OrganizationID == organizationID));
        //        BizRole bizRole = new MySecurity.BizRole();
        //        return roles.Select(x => x.ID).ToList();
        //    }
        //}
        //private List<Role> GetRoles(MyProjectEntities context, int userid, int organizationid)
        //{
        //    List<Role> result = new List<Role>();

        //    var roles = context.OrganizationUser_Role.Where(x => x.Organization_User.OrganizationID == organizationid && x.Organization_User.UserID == userid);
        //    foreach (var role in roles)
        //        result.Add(role.Role);
        //    return result;

        //}
    }
    public class SecurityActionTreeItem
    {
        public SecurityActionTreeItem()
        {
            ChildItems = new List<SecurityActionTreeItem>();
        }
        public SecurityActionTreeItem ParentItem { set; get; }
        public bool Skip { set; get; }
        public SecurityAction Action { set; get; }
        public List<SecurityActionTreeItem> ChildItems { set; get; }
    }
   
    public class ImposePermissionResult
    {
        public int ObjectSecurityID { set; get; }
        public bool Permitted { set; get; }
    }
}

