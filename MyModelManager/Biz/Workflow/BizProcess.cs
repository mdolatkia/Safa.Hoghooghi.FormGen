using DataAccess;
using ModelEntites;
using MyModelManager;

using ProxyLibrary;
using ProxyLibrary.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizProcess
    {
        public List<ProcessDTO> GetProcesses(DR_Requester requester)
        {
            List<ProcessDTO> result = new List<ProcessDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var listProcess = projectContext.Process;
                foreach (var item in listProcess)
                    result.Add(ToProcessDTO(requester, item, false));
            }
            return result;
        }
        public ProcessDTO GetProcess(DR_Requester requester, int ProcesssID, bool withDetails)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var Processs = projectContext.Process.First(x => x.ID == ProcesssID);
                if (DataIsAccessable(requester, Processs))
                    return ToProcessDTO(requester, Processs, withDetails);
                else
                    return null;
            }
        }
        SecurityHelper securityHelper = new SecurityHelper();
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        internal bool DataIsAccessable(DR_Requester requester, Process process)
        {
            //if (!entity.IsEnabled)
            //    return false;
            //else
            //{

            //دسترسی به موجودیت هم چی شود
            if (process.TableDrivedEntityID != 0)
            {
                if (!bizTableDrivedEntity.DataIsAccessable(requester, process.TableDrivedEntity))
                    return false;
                else
                    return true;
            }
            return true;
            //}
        }
        public ProcessDTO ToProcessDTO(DR_Requester requester, Process item, bool withDetails)
        {
            ProcessDTO result = new ProcessDTO();
            result.ID = item.ID;
            result.Name = item.Name;
            result.EntityID = item.TableDrivedEntityID ?? 0;
            result.TransitionFlowSTR = item.TransitionFlowSTR;
            if (withDetails)
            {
                BizRoleType bizRoleType = new BizRoleType();
                //result.AdminRoleGroup = bizRole.ToRoleGroupDTO(item.RoleGroup);

                BizAction bizAction = new BizAction();
                foreach (var citem in item.Action)
                {
                    result.Actions.Add(bizAction.ToActionDTO(citem));
                }
                BizActivity bizActivity = new BizActivity();
                foreach (var citem in item.Activity)
                {
                    result.Activities.Add(bizActivity.ToActivityDTO(citem, false));
                }
                BizEntityGroup bizEntityGroup = new BizEntityGroup();
                foreach (var citem in item.EntityGroup)
                {
                    result.EntityGroups.Add(bizEntityGroup.ToEntityGroupDTO(requester, citem, withDetails));
                }
                BizState bizState = new BizState();
                foreach (var citem in item.State)
                {
                    result.States.Add(bizState.ToStateDTO(citem, false));
                }

                //foreach (var citem in item.ProcessAdminRoleTypes)
                //{
                //    result.AdminRoleTypes.Add(bizRoleType.ToRoleTypeDTO(citem.RoleType));
                //}
                //foreach (var citem in item.RequestInitializers)
                //{
                //    result.ProcessInitializerRoleGroups.Add(bizRole.ToRoleGroupDTO(citem.RoleGroup));
                //}

            }
            return result;
        }

        public List<ProcessDTO> SearchProcess(DR_Requester requester, string singleFilterValue)
        {
            List<ProcessDTO> result = new List<ProcessDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var list = projectContext.Process as IQueryable<Process>;
                if (singleFilterValue != "")
                    list = list.Where(x => x.Name.Contains(singleFilterValue));
                foreach (var item in list)
                    result.Add(ToProcessDTO(requester, item, false));
            }
            return result;
        }

        //public List<int> GetProcessAdmins(int processID)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var process = projectContext.Process.First(x => x.ID == processID);
        //        return process.ProcessAdminRoleTypes.Select(x => x.RoleTypeID).ToList();
        //    }

        //}

        public int UpdateProcesss(ProcessDTO message)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbProcess = projectContext.Process.FirstOrDefault(x => x.ID == message.ID);
                if (dbProcess == null)
                    dbProcess = new DataAccess.Process();
                dbProcess.ID = message.ID;
                dbProcess.Name = message.Name;
                //dbProcess.AdminRoleGroupID = message.AdminRoleGroupID;

                if (message.EntityID != 0)
                {
                    dbProcess.TableDrivedEntityID = message.EntityID;
                    List<EntityGroup> removeListEntityGroup = new List<EntityGroup>();

                    foreach (var entityGroup in dbProcess.EntityGroup)
                    {
                        if (!message.EntityGroups.Any(x => x.ID == entityGroup.ID))
                            removeListEntityGroup.Add(entityGroup);
                    }
                    foreach (var entityGroup in removeListEntityGroup)
                    {
                        while (entityGroup.EntityGroup_Relationship.Any())
                            projectContext.EntityGroup_Relationship.Remove(entityGroup.EntityGroup_Relationship.First());
                        while (entityGroup.TransitionAction_EntityGroup.Any())
                            projectContext.TransitionAction_EntityGroup.Remove(entityGroup.TransitionAction_EntityGroup.First());
                        projectContext.EntityGroup.Remove(entityGroup);
                    }
                    foreach (var mGroup in message.EntityGroups)
                    {
                        EntityGroup dbGroup = null;
                        if (mGroup.ID == 0)
                        {
                            dbGroup = new EntityGroup();
                            dbProcess.EntityGroup.Add(dbGroup);
                        }
                        else
                            dbGroup = dbProcess.EntityGroup.First(x => x.ID == mGroup.ID);

                        dbGroup.Name = mGroup.Name;
                        List<EntityGroup_Relationship> removeListRel = new List<EntityGroup_Relationship>();
                        foreach (var rel in dbGroup.EntityGroup_Relationship)
                        {
                            if (!mGroup.Relationships.Any(x => x.ID == rel.ID))
                                removeListRel.Add(rel);
                        }
                        foreach (var rel in removeListRel)
                            projectContext.EntityGroup_Relationship.Remove(rel);

                        foreach (var mRel in mGroup.Relationships)
                        {
                            EntityGroup_Relationship dbRel = null;
                            if (mRel.ID == 0)
                            {
                                dbRel = new EntityGroup_Relationship();
                                dbGroup.EntityGroup_Relationship.Add(dbRel);
                            }
                            else
                                dbRel = dbGroup.EntityGroup_Relationship.First(x => x.ID == mRel.ID);
                            if (mRel.RelationshipTailID != 0)
                                dbRel.EntityRelationshipTailID = mRel.RelationshipTailID;
                            else
                                dbRel.EntityRelationshipTailID = null;
                        }
                    }
                }
                else
                {
                    dbProcess.TableDrivedEntityID = null;
                    while (dbProcess.EntityGroup.Any())
                    {
                        while (dbProcess.EntityGroup.First().EntityGroup_Relationship.Any())
                            projectContext.EntityGroup_Relationship.Remove(dbProcess.EntityGroup.First().EntityGroup_Relationship.First());
                        while (dbProcess.EntityGroup.First().TransitionAction_EntityGroup.Any())
                            projectContext.TransitionAction_EntityGroup.Remove(dbProcess.EntityGroup.First().TransitionAction_EntityGroup.First());
                        projectContext.EntityGroup.Remove(dbProcess.EntityGroup.First());
                    }
                }
                dbProcess.TransitionFlowSTR = message.TransitionFlowSTR;


                //while (dbProcess.ProcessAdminRoleTypes.Any())
                //    projectContext.ProcessAdminRoleTypes.Remove(dbProcess.ProcessAdminRoleTypes.First());
                //foreach (var roletype in message.AdminRoleTypes)
                //{
                //    dbProcess.ProcessAdminRoleTypes.Add(new ProcessAdminRoleTypes() { RoleTypeID = roletype.ID });
                //}

                //////while (dbProcess.RequestInitializers.Any())
                //////    projectContext.RequestInitializers.Remove(dbProcess.RequestInitializers.First());
                //////foreach (var roleGroup in message.ProcessInitializerRoleGroups)
                //////{
                //////    dbProcess.RequestInitializers.Add(new RequestInitializers() { RoleGroupID = roleGroup.ID });
                //////}


                if (dbProcess.ID == 0)
                    projectContext.Process.Add(dbProcess);
                projectContext.SaveChanges();
                return dbProcess.ID;
            }
        }

        public bool EntityHasAnyProcess(int targetEntityID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                return projectContext.Process.Any(x => x.TableDrivedEntityID == targetEntityID);
            }
        }

        ////public List<RoleDTO> GetProcessAdminRoles(int processID)
        ////{
        ////    BizRole bizRole = new BizRole();
        ////    using (var projectContext = new DataAccess.MyProjectEntities())
        ////    {
        ////        var Processs = projectContext.Process.First(x => x.ID == processID);
        ////        return bizRole.GetRolesOfRoleGroup(Processs.AdminRoleGroupID);
        ////    }

        ////}


        //public List<RoleDTO> GetProcessRoles(int iD)
        //{
        //    List<RoleDTO> result = new List<RoleDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        BizRole bizRole = new BizRole();
        //        var roles = projectContext.Process_Role.Where(x => x.ProcessID == iD);
        //        foreach (var item in roles)
        //        {
        //            result.Add(bizRole.ToRoleDTO(item.Role));
        //        }
        //    }
        //    return result;
        //}

        public string GetFlowSTR(int processID)
        {
            using (var projectContext = new MyProjectEntities())
            {
                return projectContext.Process
                    .First(x => x.ID == processID).TransitionFlowSTR;

                //بهتره GroupMember سمت کلاینت خوانده شود
            }
        }
    }
}
