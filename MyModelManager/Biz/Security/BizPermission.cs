using DataAccess;
using ModelEntites;
using MyCacheManager;
using MyModelManager;

using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizPermission
    {
        BizRelationship bizRelationship = new BizRelationship();

        public List<ConditionalPermissionDTO> GetConditionalPermissions()
        {
            List<ConditionalPermissionDTO> result = new List<ConditionalPermissionDTO>();
            using (var context = new MyProjectEntities())
            {
                var list = context.ConditionalPermission;
                foreach (var dbitem in list)
                {
                    result.Add(ToConditionalPermission(dbitem, false));
                }
            }
            return result;
        }
        public ConditionalPermissionDTO GetConditionalPermission(int id)
        {
            using (var context = new MyProjectEntities())
            {
                var dbitem = context.ConditionalPermission.First(x => x.ID == id);
                return (ToConditionalPermission(dbitem, true));

            }
        }
        public ConditionalPermissionDTO ToConditionalPermission(ConditionalPermission dbitem, bool withDetails)
        {
            ConditionalPermissionDTO result = new ConditionalPermissionDTO();
            //if (dbitem.ColumnID != null)
            //    result.ColumnID = dbitem.ColumnID.Value;
            //if (dbitem.EntityCommandID != null)
            //    result.CommandID = dbitem.EntityCommandID.Value;
            result.SecurityObjectID = dbitem.SecurityObjectID;
            result.SecuritySubjectID = dbitem.SecuritySubjectID;


            result.ID = dbitem.ID;
            result.EntityID = dbitem.TableDrivedEntityID;
            if (dbitem.ConditinColumnID != null)
                result.ConditinColumnID = dbitem.ConditinColumnID.Value;
            if (dbitem.FormulaID != null)
            {  //??با جزئیات؟؟........................................................................ 
                BizFormula bizFormula = new BizFormula();
                result.FormulaID = dbitem.FormulaID.Value;
                result.Formula = bizFormula.ToFormulaDTO(dbitem.Formula, false);
            }
            //if (dbitem.RoleID != null)
            //{
            //    result.RoleOrRoleGroup = new RoleOrRoleGroupDTO();
            //    result.RoleOrRoleGroup.ID = dbitem.RoleID.Value;
            //    result.RoleOrRoleGroup.Name = dbitem.Role.Name;
            //    result.RoleOrRoleGroup.Type = RoleOrRoleGroupType.Role;
            //}
            //else if (dbitem.SecurityRoleGroupID != null)
            //{
            //    result.RoleOrRoleGroup = new RoleOrRoleGroupDTO();
            //    result.RoleOrRoleGroup.ID = dbitem.SecurityRoleGroupID.Value;
            //    result.RoleOrRoleGroup.Name = dbitem.RoleGroup.Name;
            //    result.RoleOrRoleGroup.Type = RoleOrRoleGroupType.RoleGroup;
            //}

            result.Value = dbitem.Value;
            result.HasNotRole = dbitem.HasNotRole;
            if (withDetails)
            {
                foreach (var action in dbitem.ConditionalPermission_Action)
                {
                    //SecActionDTO item = new SecActionDTO();
                    //item.Action = ;
                    //item.ID = dbItem.ID;
                    result.Actions.Add((SecurityAction)Enum.Parse(typeof(SecurityAction), action.Action));
                }
                BizSecuritySubject bizSecuritySubject = new BizSecuritySubject();
                result.SecuritySubject = bizSecuritySubject.ToSecuritySubjectDTO(dbitem.SecuritySubject);
            }
            return result;
        }

        //public int GetPermissionId(RoleOrRoleGroupDTO roleOrRoleGroup, DatabaseObjectCategory databaseObjectCategory, string objectID)
        //{
        //    using (var context = new MyProjectEntities())
        //    {

        //        Permission found = null;
        //        if (roleOrRoleGroup.Type == RoleOrRoleGroupType.Role)
        //            found = context.Permission.FirstOrDefault(x => x.RoleID == roleOrRoleGroup.ID && x.ObjectCategory == databaseObjectCategory.ToString() && x.GeneralObjectIdentity == objectID);
        //        else
        //            found = context.Permission.FirstOrDefault(x => x.SecurityRoleGroupID == roleOrRoleGroup.ID && x.ObjectCategory == databaseObjectCategory.ToString() && x.GeneralObjectIdentity == objectID);
        //        if (found == null)
        //            return 0;
        //        else
        //            return found.ID;
        //    }
        //}
        public List<ConditionalPermissionDTO> GetConditionalPermissions(DR_Requester requester, int entityID)
        {
            List<ConditionalPermissionDTO> result = new List<ConditionalPermissionDTO>();
            var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.ConditionalPermission, requester.Identity.ToString(), entityID.ToString());
            if (cachedItem != null)
                return (cachedItem as List<ConditionalPermissionDTO>);

            //List<int> organizationTypeIDs = new List<int>();
            //List<int> organizationIDs = new List<int>();
            //List<int> roleTypeIDs = new List<int>();
            //List<int> orgTypeRoleTypeIDs = new List<int>();

            //List<ConditionalPermission> orgTypeRoleTypeActions = new List<ConditionalPermission>(); ;
            //List<ConditionalPermission> organizationActions = new List<ConditionalPermission>();
            //List<ConditionalPermission> roleTypeActions = new List<ConditionalPermission>();
            //List<ConditionalPermission> organizationTypeActions = new List<ConditionalPermission>();

            //List<ConditionalPermission> dbresult = new List<ConditionalPermission>();
            using (var context = new MyProjectEntities())
            {
                var organizationPosts = new BizRoleSecurity().GetDBOrganizationPosts(context, requester);
                foreach (var post in organizationPosts)
                {
                    //List<ConditionalPermission> postAccess = new List<ConditionalPermission>();
                    var postConditionalPermissions = GetConditionalPermissions(context, entityID, post.SecuritySubject.ID);
                    if (postConditionalPermissions.Any())
                        AddConditionalPermissionsToResult(postConditionalPermissions, result);
                    else
                    {
                        var orgTypeRoleTypeActions = GetConditionalPermissions(context, entityID, post.OrganizationType_RoleType.SecuritySubject.ID);
                        var organizationActions = GetConditionalPermissions(context, entityID, post.Organization.SecuritySubject.ID);
                        if (orgTypeRoleTypeActions.Any())
                        {
                            //اینجا دسترسی های موازی با هم جمع میشوند زیرا معلوم نیست بروی کدام آبجکت دارند اعمال میشوند و تصمیم گیری در مورد تداخل دسترسی بروی یک آبجکت به کلاینت واگذار میشود
                            AddConditionalPermissionsToResult(orgTypeRoleTypeActions, result);
                            AddConditionalPermissionsToResult(organizationActions, result);
                        }
                        else
                        {
                            var roleTypeActions = GetConditionalPermissions(context, entityID, post.OrganizationType_RoleType.RoleType.SecuritySubject.ID);
                            if (organizationActions.Any())
                            {
                                AddConditionalPermissionsToResult(organizationActions, result);
                                AddConditionalPermissionsToResult(roleTypeActions, result);
                            }
                            else
                            {
                                var organizationTypeActions = GetConditionalPermissions(context, entityID, post.OrganizationType_RoleType.OrganizationType.SecuritySubject.ID);
                                AddConditionalPermissionsToResult(organizationTypeActions, result);
                                AddConditionalPermissionsToResult(roleTypeActions, result);
                            }
                        }
                    }

                }
            }
            CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.ConditionalPermission, requester.Identity.ToString(), entityID.ToString());
            return result;
        }

        private void AddConditionalPermissionsToResult(List<ConditionalPermissionDTO> postConditionalPermissions, List<ConditionalPermissionDTO> result)
        {
            foreach (var item in postConditionalPermissions)
                if (result.Any(x => x.ID == item.ID))
                    result.Add(item);
        }
        private List<ConditionalPermissionDTO> GetConditionalPermissions(MyProjectEntities context, int entityID, int securitySubjectID)
        {
            var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.ConditionalPermission, securitySubjectID.ToString(), entityID.ToString());
            if (cachedItem != null)
                return (cachedItem as List<ConditionalPermissionDTO>);

            List<ConditionalPermissionDTO> result = new List<ConditionalPermissionDTO>();
            BizPermission bizPermission = new BizPermission();
            var conditionalPermissions = context.ConditionalPermission.Where(x => x.TableDrivedEntityID == entityID && x.SecuritySubjectID == securitySubjectID);
            foreach (var dbitem in conditionalPermissions)
            {
                result.Add(bizPermission.ToConditionalPermission(dbitem, true));
            }
            CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.ConditionalPermission, securitySubjectID.ToString(), entityID.ToString());
            return result;
        }
        public PermissionDTO GetPermission(int securitySubjectID, int securityObjectID)
        {
            using (var context = new MyProjectEntities())
            {
                var dbSecurityObject = context.SecurityObject.FirstOrDefault(x => x.ID == securityObjectID);
                if (dbSecurityObject != null)
                {
                    var category = (DatabaseObjectCategory)dbSecurityObject.Type;
                    if (category == DatabaseObjectCategory.Relationship)
                    {
                        var relationship = bizRelationship.GetRelationship(securityObjectID);

                        if (relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary)
                            securityObjectID = relationship.RelationshipColumns.First().FirstSideColumnID;
                        else
                            securityObjectID = relationship.RelationshipColumns.First().SecondSideColumnID;
                    }
                }
                else
                    return null;

                var dbitem = context.Permission.FirstOrDefault(x => x.SecuritySubjectID == securitySubjectID && x.SecurityObjectID == securityObjectID);
                if (dbitem != null)
                    return (ToPermission(dbitem));
                else
                    return null;


            }
        }

        private PermissionDTO ToPermission(Permission dbitem)
        {
            PermissionDTO result = new PermissionDTO();

            result.ID = dbitem.ID;
            //   result.ObjectCategory = (DatabaseObjectCategory)Enum.Parse(typeof(DatabaseObjectCategory), dbitem.ObjectCategory);
            result.SecurityObjectID = dbitem.SecurityObjectID;
            result.SecuritySubjectID = dbitem.SecuritySubjectID;
            result.SecurityObjectCategory = (DatabaseObjectCategory)dbitem.SecurityObject.Type;
            //if (dbitem.RoleID != null)
            //{
            //    result.RoleOrRoleGroup = new RoleOrRoleGroupDTO();
            //    result.RoleOrRoleGroup.ID = dbitem.RoleID.Value;
            //    result.RoleOrRoleGroup.Name = dbitem.Role.Name;
            //    result.RoleOrRoleGroup.Type = RoleOrRoleGroupType.Role;
            //}
            //else if (dbitem.SecurityRoleGroupID != null)
            //{
            //    result.RoleOrRoleGroup = new RoleOrRoleGroupDTO();
            //    result.RoleOrRoleGroup.ID = dbitem.SecurityRoleGroupID.Value;
            //    result.RoleOrRoleGroup.Name = dbitem.RoleGroup.Name;
            //    result.RoleOrRoleGroup.Type = RoleOrRoleGroupType.RoleGroup;
            //}
            //if (withActions)
            //{
            foreach (var action in dbitem.Permission_Action)
            {
                //  SecActionDTO item = new SecActionDTO();
                var securityAction = (SecurityAction)Enum.Parse(typeof(SecurityAction), action.Action);
                //item.ID = dbItem.ID;
                result.Actions.Add(securityAction);
            }
            //}
            return result;
        }

        //public List<ActionDTO> GetActions(DatabaseObjectCategory objectCategory, string objectIdentity, RoleOrRoleGroupDTO roleOrRoleGroup)
        //{
        //    List<ActionDTO> result = new List<ActionDTO>();
        //    var actions = SecurityHelper.GetActionsByCategory(objectCategory);
        //    using (var context = new MyProjectEntities())
        //    {
        //        foreach (var action in actions)
        //        {
        //            ActionDTO item = new ActionDTO();
        //            if (roleOrRoleGroup.Type == RoleOrRoleGroupType.Role)
        //                item.Selected = context.Role_Action_Object.Any(x => x.Action == action.ToString() && x.GeneralObjectIdentity == objectIdentity && x.ObjectCategory == objectCategory.ToString() && x.RoleID == roleOrRoleGroup.ID);
        //            else
        //                item.Selected = context.Role_Action_Object.Any(x => x.Action == action.ToString() && x.GeneralObjectIdentity == objectIdentity && x.ObjectCategory == objectCategory.ToString() && x.SecurityRoleGroupID == roleOrRoleGroup.ID);
        //            item.Action = action;
        //            //item.ID = dbItem.ID;
        //            result.Add(item);
        //        }
        //    }
        //    return result;
        //}

        public BaseResult SavePermission(DR_Requester requester, PermissionDTO permission)
        {
            BaseResult result = new BaseResult();
            try
            {
                using (var context = new MyProjectEntities())
                {
                    if (permission.SecurityObjectCategory == DatabaseObjectCategory.Relationship)
                    {
                        BizTableDrivedEntity bizTableDrivedEntity = new MyModelManager.BizTableDrivedEntity();
                        var relationship = bizRelationship.GetRelationship(permission.SecurityObjectID);
                        //TableDrivedEntityDTO fkEntity = null;
                        List<ColumnDTO> listFKColumns = null;
                        if (relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary)
                        {
                            //fkEntity = bizTableDrivedEntity.GetTableDrivedEntity(requester, relationship.EntityID1, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
                            listFKColumns = relationship.RelationshipColumns.Select(x => x.FirstSideColumn).ToList();
                        }
                        else
                        {
                            //fkEntity = bizTableDrivedEntity.GetTableDrivedEntity(requester, relationship.EntityID2, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
                            listFKColumns = relationship.RelationshipColumns.Select(x => x.SecondSideColumn).ToList();
                        }
                        foreach (var fkColumn in listFKColumns)
                        {
                            PermissionDTO colPermission = new PermissionDTO();
                            colPermission.SecurityObjectCategory = DatabaseObjectCategory.Column;
                            colPermission.Actions = permission.Actions;
                            var currentPermission = context.Permission.FirstOrDefault(x => x.SecurityObjectID == fkColumn.ID && x.SecuritySubjectID == permission.SecuritySubjectID);
                            if (currentPermission != null)
                                colPermission.ID = currentPermission.ID;
                            colPermission.SecurityObjectID = fkColumn.ID;
                            colPermission.SecuritySubjectID = permission.SecuritySubjectID;
                            SavePermission(context, colPermission);
                        }

                    }
                    else
                    {
                        SavePermission(context, permission);
                    }
                    context.SaveChanges();
                    result.Result = Enum_DR_ResultType.SeccessfullyDone;
                }
            }
            catch (Exception ex)
            {
                result.Result = Enum_DR_ResultType.ExceptionThrown;
                result.Message = "خطا در ثبت" + Environment.NewLine + ex.Message;
            }
            return result;
        }

        private void SavePermission(MyProjectEntities context, PermissionDTO permission)
        {
            if (permission.SecurityObjectCategory == DatabaseObjectCategory.Column)
            {
                if (permission.Actions.Any(x => x == SecurityAction.NoAccess))
                {
                    BizColumn bizColumn = new BizColumn();
                    var column = bizColumn.GetColumn(permission.SecurityObjectID, true);
                    if (column.PrimaryKey)
                    {
                        throw new Exception("امکان تعیین عدم دسترسی به ستونهای کلید اصلی وجود ندارد");
                    }
                }
            }

            Permission dbItem = null;
            if (permission.ID != 0)
                dbItem = context.Permission.First(x => x.ID == permission.ID);
            else
            {
                dbItem = new Permission();
                context.Permission.Add(dbItem);
            }

            dbItem.SecurityObjectID = permission.SecurityObjectID;
            dbItem.SecuritySubjectID = permission.SecuritySubjectID;
            //dbItem.SecurityObjectType = (short)permission.SecurityObjectCategory;

            while (dbItem.Permission_Action.Any())
                context.Permission_Action.Remove(dbItem.Permission_Action.First());

            foreach (var item in permission.Actions)
            {
                Permission_Action action = new Permission_Action();
                action.Action = item.ToString();
                dbItem.Permission_Action.Add(action);
            }

        }

        public void SaveConditionalPermission(ConditionalPermissionDTO conditionalPermission)
        {
            using (var context = new MyProjectEntities())
            {
                ConditionalPermission dbItem = null;
                if (conditionalPermission.ID != 0)
                    dbItem = context.ConditionalPermission.First(x => x.ID == conditionalPermission.ID);
                else
                {
                    dbItem = new ConditionalPermission();
                    context.ConditionalPermission.Add(dbItem);
                }
                dbItem.SecuritySubjectID = conditionalPermission.SecuritySubjectID;
                dbItem.SecurityObjectID = conditionalPermission.SecurityObjectID;
                //if (conditionalPermission.ColumnID != 0)
                //    dbItem.ColumnID = conditionalPermission.ColumnID;
                //else
                //    dbItem.ColumnID = null;
                //if (conditionalPermission.CommandID != 0)
                //    dbItem.EntityCommandID = conditionalPermission.CommandID;
                //else
                //    dbItem.EntityCommandID = null;

                if (conditionalPermission.ConditinColumnID != 0)
                    dbItem.ConditinColumnID = conditionalPermission.ConditinColumnID;
                if (conditionalPermission.FormulaID != 0)
                    dbItem.FormulaID = conditionalPermission.FormulaID;
                dbItem.Value = conditionalPermission.Value;
                dbItem.HasNotRole = conditionalPermission.HasNotRole;
                dbItem.TableDrivedEntityID = conditionalPermission.EntityID;
                //if (conditionalPermission.RoleOrRoleGroup.Type == RoleOrRoleGroupType.Role)
                //{
                //    dbItem.RoleID = conditionalPermission.RoleOrRoleGroup.ID;
                //    dbItem.SecurityRoleGroupID = null;
                //}
                //else
                //{
                //    dbItem.RoleID = null;
                //    dbItem.SecurityRoleGroupID = conditionalPermission.RoleOrRoleGroup.ID;
                //}

                while (dbItem.ConditionalPermission_Action.Any())
                    context.ConditionalPermission_Action.Remove(dbItem.ConditionalPermission_Action.First());

                foreach (var item in conditionalPermission.Actions)
                {
                    ConditionalPermission_Action action = new ConditionalPermission_Action();
                    action.Action = item.ToString();
                    dbItem.ConditionalPermission_Action.Add(action);
                }
                context.SaveChanges();
            }
        }
    }




}
