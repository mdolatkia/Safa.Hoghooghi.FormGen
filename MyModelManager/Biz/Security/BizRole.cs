using DataAccess;
using ModelEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizRole
    {
        //public List<RoleTypeDTO> GetAllRoleTypes()
        //{
        //    var context = new MyProjectEntities();
        //    return ToRoleDTOList(context.RoleType);
        //}
        //public List<RoleTypeDTO> ToRoleDTOList(IQueryable<RoleType> roleType)
        //{
        //    List<RoleTypeDTO> result = new List<RoleTypeDTO>();
        //    foreach (var item in roleType)
        //    {
        //        var RoleDto = ToRoleTypeDTO(item);

        //        result.Add(RoleDto);
        //    }
        //    return result;
        //}
        ////public RoleTypeDTO GetRoleType(int roleTypeID)
        ////{
        ////    using (var context = new MyProjectEntities())
        ////    {
        ////        var role = context.RoleType.First(x => x.ID == roleTypeID);
        ////        return ToRoleTypeDTO(role);
        ////    }
        ////}
     
        //public RoleTypeDTO ToRoleTypeDTO(RoleType item)
        //{
        //    RoleTypeDTO result = new RoleTypeDTO();
        //    result.ID = item.ID;
        //    result.Name = item.Name;
        //    result.IsSuperAdmin = item.IsSuperAdmin == true;
        //    return result;
        //}
   

        //public void SaveRole(RoleTypeDTO roleTypeDto)
        //{
        //    using (var context = new MyProjectEntities())
        //    {
        //        RoleType dbRoleType = null;
        //        if (roleTypeDto.ID == 0)
        //        {
        //            dbRoleType = new RoleType();
        //            dbRoleType.SecuritySubject = new SecuritySubject();
        //            dbRoleType.SecuritySubject.Type = (int)SecuritySubjectType.RoleType;
        //        }
        //        else
        //            dbRoleType = context.RoleType.First(x => x.ID == roleTypeDto.ID);

        //        dbRoleType.Name = roleTypeDto.Name;
        //        //dbRoleType.IsAdmin = roleTypeDto.IsAdmin;
        //        dbRoleType.IsSuperAdmin = roleTypeDto.IsSuperAdmin;


        //        if (dbRoleType.ID == 0)
        //            context.RoleType.Add(dbRoleType);
        //        context.SaveChanges();
        //    }
        //}

      
        //public List<RoleOrRoleGroupDTO> GetRoleOrRoleGroups()
        //{
        //    List<RoleOrRoleGroupDTO> result = new List<RoleOrRoleGroupDTO>();
        //    BizRole bizRole = new BizRole();
        //    var roles = bizRole.GetAllRoles();
        //    foreach (var role in roles)
        //    {
        //        RoleOrRoleGroupDTO item = new RoleOrRoleGroupDTO();
        //        item.Type = RoleOrRoleGroupType.Role;
        //        item.ID = role.ID;
        //        item.Name = role.Name;
        //        result.Add(item);
        //    }
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var roleGroups = projectContext.RoleGroup.ToList();
        //        foreach (var group in roleGroups)
        //        {
        //            RoleOrRoleGroupDTO item = new RoleOrRoleGroupDTO();
        //            item.Type = RoleOrRoleGroupType.RoleGroup;
        //            item.ID = group.ID;
        //            item.Name = group.Name;
        //            result.Add(item);
        //        }
        //    }
        //    return result;
        //}

        //public List<RoleGroupDTO> GetRoleGroups()
        //{
        //    List<RoleGroupDTO> result = new List<RoleGroupDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var list = projectContext.RoleGroup;
        //        foreach (var item in list)
        //            result.Add(ToRoleGroupDTO(item));

        //    }
        //    return result;
        //}
        //public RoleGroupDTO GetRoleGroup(int id)
        //{

        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var item = projectContext.RoleGroup.FirstOrDefault(x => x.ID == id);
        //        if (item != null)
        //            return ToRoleGroupDTO(item);
        //        else
        //            return null;

        //    }
        //}

        //public RoleGroupDTO ToRoleGroupDTO(RoleGroup item)
        //{
        //    BizRole bizRole = new BizRole();
        //    RoleGroupDTO result = new RoleGroupDTO();
        //    result.ID = item.ID;
        //    result.Name = item.Name;
        //    foreach (var role in item.SecurityRoleGroup_Role)
        //    {
        //        result.Roles.Add(bizRole.GetRole(role.RoleID));
        //    }
        //    return result;
        //}


        //public void UpdateRoleGroup(RoleGroupDTO message)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var dbItem = projectContext.RoleGroup.FirstOrDefault(x => x.ID == message.ID);
        //        if (dbItem == null)
        //        {
        //            dbItem = new RoleGroup();
        //            projectContext.RoleGroup.Add(dbItem);
        //        }
        //        while (dbItem.SecurityRoleGroup_Role.Any())
        //            projectContext.SecurityRoleGroup_Role.Remove(dbItem.SecurityRoleGroup_Role.First());
        //        foreach (var item in message.Roles)
        //        {
        //            var rdbItem = new SecurityRoleGroup_Role();
        //            rdbItem.RoleID = item.ID;
        //            dbItem.SecurityRoleGroup_Role.Add(rdbItem);
        //        }
        //        projectContext.SaveChanges();
        //    }

        //}

        //public List<RoleDTO> GetRolesOfRoleGroup(int roleGroupID)
        //{
        //    List<RoleDTO> result = new List<RoleDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var item = projectContext.RoleGroup.FirstOrDefault(x => x.ID == roleGroupID);
        //        if (item != null)
        //        {
        //            foreach (var dbRole in item.SecurityRoleGroup_Role)
        //            {
        //                result.Add(ToRoleDTO(dbRole.Role));
        //            }
        //        }
        //    }
        //    return result;
        //}
        //////public List<RoleDTO> GetRolesByUser(int userID)
        //////{
        //////    using (var context = new MyProjectEntities())
        //////    {
        //////        return ToRoleDTOList(context.Roles.Where(x => x.Users.Any(y => y.ID == userID)).ToList());
        //////    }
        //////}


        //public void AddUserToRole(int roleID, int userID)
        //{
        //    using (var context = new MyProjectEntities())
        //    {
        //        var role = context.Roles.First(x => x.ID == roleID);

        //        if (!role.Users.Any(x => x.ID == userID))
        //            role.Users.Add(context.Users.First(x => x.ID == userID));
        //        context.SaveChanges();
        //    }
        //}

        //public List<RoleActionObjectDTO> GetActionsByObject(int objectID, int roleID)
        //{
        //    List<RoleActionObjectDTO> result = new List<RoleActionObjectDTO>();
        //    using (var context = new MyProjectEntities())
        //    {
        //        var roleActionObjectList = context.Role_Action_Object.Where(x => x.ObjectID == objectID && x.RoleID == roleID);
        //        foreach (var dbItem in roleActionObjectList)
        //        {
        //            RoleActionObjectDTO item = new RoleActionObjectDTO();
        //            item.ActionID = dbItem.ActionID;
        //            item.ObjectID = dbItem.ObjectID;
        //            item.RoleID = dbItem.RoleID;
        //            result.Add(item);
        //        }
        //    }
        //    return result;
        //}



    }


    //public class RoleActionObjectDTO
    //{
    //    public int RoleID { set; get; }
    //    public int ActionID { set; get; }
    //    public int ObjectID { set; get; }

    //}

}
