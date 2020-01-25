using ModelEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyLibrary
{
    public class SecurityObjectDTO
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public DatabaseObjectCategory Type { set; get; }
        //public int TableDrivedEntityID { set; get; }
        //public int ColumnID { set; get; }
        //public int DatabaseInformationID { set; get; }
        //public int DBSchemaID { set; get; }
        //public int EntityCommandID { set; get; }
        //public int EntityReportID { set; get; }

    }
    public class SecuritySubjectDTO
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public SecuritySubjectType Type { set; get; }

    }
    public class RoleTypeDTO
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public bool Selected { set; get; }
        //public bool IsAdmin { set; get; }
        public bool IsSuperAdmin { set; get; }

    }

    public enum SecuritySubjectType
    {
        OrganizationType,
        RoleType,
        Organization,
        OrganizationTypeRoleType,
        OrganizationPost
    }
    public class OrganizationTypeDTO
    {
        public OrganizationTypeDTO()
        {
            OrganizationTypeRoleTypes = new List<OrganizationTypeRoleTypeDTO>();
        }
        public int ID { set; get; }
        public string Name { set; get; }

        public List<OrganizationTypeRoleTypeDTO> OrganizationTypeRoleTypes { set; get; }
    }
    public class OrganizationTypeRoleTypeDTO
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public bool IsAdmin { set; get; }
        public int RoleTypeID { set; get; }
        public int OrganizationTypeID { set; get; }
    }
    public class OrganizationDTO
    {
        public OrganizationDTO()
        {
            OrganizationPosts = new List<OrganizationPostDTO>();
        }
        public int ID { set; get; }
        public string Name { set; get; }

        public int OrganizationTypeID { set; get; }
        OrganizationTypeDTO OrganizationTypeDTO { set; get; }
        public List<OrganizationPostDTO> OrganizationPosts;

    }
    public class OrganizationPostDTO
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public bool Selected { set; get; }

        public bool IsAdmin { set; get; }
        public bool IsSuperAdmin { set; get; }

        public int CurrentUserID { set; get; }
        public UserDTO CurrentUser { set; get; }
        public int OrganizationTypeRoleTypeID { set; get; }

        public int RoleTypeID { set; get; }
        public RoleTypeDTO RoleType { set; get; }
        public int OrganizationID { set; get; }

        public string OrganizationName { set; get; }
        public int OrganizationTypeID { set; get; }
    }
    public class SecActionDTO
    {

        //public int ID { set; get; }
        public SecurityAction Action { set; get; }
        public bool Selected { set; get; }
    }
    public enum SecurityAction
    {
        NoAccess,
      //  MenuAccess,
        //ArchiveMenuAccess,
        ReadOnly,
        EditAndDelete,
        Edit,
        Access,
        ArchiveView,
        ArchiveEdit,
        LetterView,
        LetterEdit,
        Any
    }




    public class UserInfoDTO
    {
        public UserInfoDTO()
        {
            OrganizationPosts = new List<OrganizationPostDTO>();
        }
        public int ID { set; get; }
        public string UserName { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }

        public List<OrganizationPostDTO> OrganizationPosts { set; get; }
    }

    public class UserDTO
    {
        public int ID { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
        public string FirstName { set; get; }
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public string LastName { set; get; }
        public string Email { set; get; }
        public bool Selected { set; get; }


    }
    public class OrganizationUserDTO
    {
        public int ID { set; get; }
        public int UserID { set; get; }
        public int OrganizationID { set; get; }

        public bool IsDefault { set; get; }
    }

    public class OrganizationWithPostsDTO
    {
        public OrganizationWithPostsDTO()
        {
            Posts = new List<OrganizationPostDTO>();
        }
        public string Name { set; get; }
        public int OrganizationID { set; get; }
        public bool IsDefault { set; get; }
        public List<OrganizationPostDTO> Posts { set; get; }
    }








    public class AssignedPermissionDTO
    {
        public AssignedPermissionDTO()
        {
            GrantedActions = new List<SecurityAction>();
            ChildsPermissions = new List<AssignedPermissionDTO>();
        }
        public List<SecurityAction> GrantedActions { set; get; }
        public int SecurityObjectID { set; get; }
        public DatabaseObjectCategory SecurityObjectType { set; get; }
        //public string ObjectIdentity { set; get; }
        //public DatabaseObjectCategory ObjectCategory { set; get; }
        public List<AssignedPermissionDTO> ChildsPermissions { set; get; }
    }

}
