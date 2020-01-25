using DataAccess;

using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizUser
    {
        public List<UserDTO> GetAllUsers(string generalFilter)
        {
            var context = new MyProjectEntities();
            var users = context.User as IQueryable<User>;
            if (generalFilter != "")
                users = users.Where(x => x.UserName.Contains(generalFilter) || x.FirstName.Contains(generalFilter)
                 || x.LastName.Contains(generalFilter));
            return ToUserDTOList(users);
        }
        public object GetAllLocalAdminUsers(string generalFilter, List<int> orgIds)
        {
            //سوپر ادمین را نمی تواند اصلاح کند به علاوه کاربران سازمانهای دیگر را هم نمی تواند اصلاح کند
            using (var context = new MyProjectEntities())
            {
                var users = context.User as IQueryable<User>;
                if (generalFilter != "")
                    users = users.Where(x => x.UserName.Contains(generalFilter) || x.UserName.Contains(generalFilter));
                users = users.Where(x => !x.OrganizationPost.Any(y => y.OrganizationType_RoleType.RoleType.IsSuperAdmin == true));
                var otherOrgIds = context.Organization.Where(x => !orgIds.Contains(x.ID)).Select(x => x.ID).ToList();
                users = users.Where(x => x.OrganizationPost.Any(y => orgIds.Contains(y.OrganizationID)) && !x.OrganizationPost.Any(y => otherOrgIds.Contains(y.OrganizationID)));
                return ToUserDTOList(users);
            }
        }

        public void CheckAdminUserExists()
        {
            var context = new MyProjectEntities();
            if (!context.User.Any(x => x.OrganizationPost.Any(y => y.OrganizationType_RoleType.RoleType.IsSuperAdmin == true)))
            {
                var user = new User();

                user = new User();
                user.FirstName = "راهبر";
                user.LastName = "ارشد";
                user.UserName = "kia";
                user.Password = "1";
                var organizationPost = context.OrganizationPost.FirstOrDefault(x => x.OrganizationType_RoleType.RoleType.IsSuperAdmin == true);
                if (organizationPost == null)
                {
                    organizationPost = new OrganizationPost();
                    organizationPost.Name = "پست راهبر ارشد";
                    var organization = context.Organization.FirstOrDefault(x => x.OrganizationType.OrganizationType_RoleType.Any(y => y.RoleType.IsSuperAdmin == true));
                    OrganizationType organizationType = null;
                    if (organization == null)
                    {
                        organization = new Organization();
                        organization.Name = "سازمان";
                        organizationType = context.OrganizationType.FirstOrDefault(x => x.OrganizationType_RoleType.Any(y => y.RoleType.IsSuperAdmin == true));
                        if (organizationType == null)
                        {
                            organizationType = new OrganizationType();
                            organizationType.Name = "نوع سازمان";
                            organizationType.SecuritySubject = new SecuritySubject() { Type = (int)SecuritySubjectType.OrganizationType };
                            context.OrganizationType.Add(organizationType);
                        }

                        organization.OrganizationType = organizationType;
                        organization.SecuritySubject = new SecuritySubject() { Type = (int)SecuritySubjectType.Organization };
                        context.Organization.Add(organization);
                    }

                    var organizationTypeRoleType = context.OrganizationType_RoleType.FirstOrDefault(x => x.OrganizationTypeID == organizationType.ID && x.RoleType.IsSuperAdmin == true);
                    if (organizationTypeRoleType == null)
                    {
                        organizationTypeRoleType = new OrganizationType_RoleType();
                        organizationTypeRoleType.OrganizationType = organizationType;
                        var superAdminRoleType = context.RoleType.FirstOrDefault(x => x.IsSuperAdmin == true);
                        if (superAdminRoleType == null)
                        {
                            superAdminRoleType = new RoleType();
                            superAdminRoleType.Name = "راهبر ارشد";
                            superAdminRoleType.IsSuperAdmin = true;
                            superAdminRoleType.SecuritySubject = new SecuritySubject() { Type = (int)SecuritySubjectType.RoleType };
                            context.RoleType.Add(superAdminRoleType);
                        }
                        organizationTypeRoleType.RoleType = superAdminRoleType;
                        organizationTypeRoleType.SecuritySubject = new SecuritySubject() { Type = (int)SecuritySubjectType.OrganizationTypeRoleType };
                        context.OrganizationType_RoleType.Add(organizationTypeRoleType);
                    }

                    organizationPost.Organization = organization;
                    organizationPost.OrganizationType_RoleType = organizationTypeRoleType;
                    organizationPost.SecuritySubject = new SecuritySubject() { Type = (int)SecuritySubjectType.OrganizationPost };
                    context.OrganizationPost.Add(organizationPost);
                }
                user.OrganizationPost.Add(organizationPost);
                context.User.Add(user);
                context.SaveChanges();
            }
        }


        public string GetUserFullName(int userID)
        {
            var context = new MyProjectEntities();
            var user = context.User.First(x => x.ID == userID);
            return user.FirstName + " " + user.LastName;
        }
        public UserDTO GetUser(int userID)
        {
            var context = new MyProjectEntities();
            var user = context.User.First(x => x.ID == userID);
            return ToUserDTO(user);
        }

        private List<UserDTO> ToUserDTOList(IQueryable<User> users)
        {
            List<UserDTO> result = new List<UserDTO>();
            foreach (var item in users)
            {
                UserDTO userDto = ToUserDTO(item);
                result.Add(userDto);
            }
            return result;
        }

        public UserDTO ToUserDTO(User item)
        {
            var result = new UserDTO();
            result.ID = item.ID;
            result.UserName = item.UserName;
            result.FirstName = item.FirstName;
            result.LastName = item.LastName;
            result.Password = item.Password;
            result.Email = item.Email;
            return result;
        }



        public int SaveUser(UserDTO userDto)
        { 
            using (var context = new MyProjectEntities())
            {
                User dbUser = ToUserDB(userDto, context);

                if (dbUser.ID == 0)
                    context.User.Add(dbUser);
                try
                {
                    context.SaveChanges();
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
                return dbUser.ID;
            }
        }

        private User ToUserDB(UserDTO userDto, MyProjectEntities context)
        {
            User dbUser = null;
            if (userDto.ID == 0)
                dbUser = new User();
            else
                dbUser = context.User.First(x => x.ID == userDto.ID);
            dbUser.FirstName = userDto.FirstName;
            dbUser.LastName = userDto.LastName;
            dbUser.UserName = userDto.UserName;
            dbUser.Password = userDto.Password;
            dbUser.Email = userDto.Email;

            return dbUser;
        }

        //public List<UserDTO> GetUserByOrganization(int OrganizationID)
        //{
        //    using (var context = new MyProjectEntities())
        //    {
        //        return ToUserDTOList(context.User.Where(x => x.Organization_User.Any(y => y.OrganizationID == OrganizationID)));
        //    }
        //}
    }


}
