using DataAccess;
using ModelEntites;

using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizRoleType
    {
        public List<RoleTypeDTO> GetAllRoleTypes()
        {
            var context = new MyProjectEntities();
            var roleTypes = context.RoleType;
            List<RoleTypeDTO> result = new List<RoleTypeDTO>();
            foreach (var item in roleTypes)
            {
                var RoleDto = ToRoleTypeDTO(item);

                result.Add(RoleDto);
            }
            return result;
        }
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
        public RoleTypeDTO GetRoleType(int roleTypeID)
        {
            using (var context = new MyProjectEntities())
            {
                var role = context.RoleType.First(x => x.ID == roleTypeID);
                return ToRoleTypeDTO(role);
            }
        }

        public RoleTypeDTO ToRoleTypeDTO(RoleType item)
        {
            RoleTypeDTO result = new RoleTypeDTO();
            result.ID = item.ID;
            result.Name = item.Name;
            result.IsSuperAdmin = item.IsSuperAdmin == true;
            return result;
        }


        public void SaveRoleTypes(List<RoleTypeDTO> roleTypes)
        {
            using (var context = new MyProjectEntities())
            {
                var ids = roleTypes.Select(x => x.ID).ToList();
                var removeList = context.RoleType.Where(x => !ids.Contains(x.ID)).ToList();
                foreach (var item in removeList)
                    context.RoleType.Remove(item);
                foreach (var roleTypeDto in roleTypes)
                {
                    RoleType dbRoleType = null;
                    if (roleTypeDto.ID == 0)
                    {
                        dbRoleType = new RoleType();
                        dbRoleType.SecuritySubject = new SecuritySubject();
                        dbRoleType.SecuritySubject.Type = (int)SecuritySubjectType.RoleType;
                    }
                    else
                        dbRoleType = context.RoleType.First(x => x.ID == roleTypeDto.ID);

                    dbRoleType.Name = roleTypeDto.Name;
                    dbRoleType.IsSuperAdmin = roleTypeDto.IsSuperAdmin;

                    if (dbRoleType.ID == 0)
                        context.RoleType.Add(dbRoleType);
                }
                context.SaveChanges();
               
            }
        }



    }




}
