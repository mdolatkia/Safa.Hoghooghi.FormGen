using DataAccess;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizOrganization
    {
        public List<OrganizationDTO> GetAllOrganizations()
        {
            List<OrganizationDTO> result = new List<OrganizationDTO>();
            var context = new MyProjectEntities();
            var organizations = context.Organization;
            foreach (var item in organizations)
            {
                OrganizationDTO OrganizationDto = ToOrganizationDTO(item, false);
                result.Add(OrganizationDto);
            }
            return result;
        }
        public OrganizationDTO GetOrganization(int ID, bool withDetails)
        {
            var context = new MyProjectEntities();
            var organization = context.Organization.First(x => x.ID == ID);
            return ToOrganizationDTO(organization, withDetails);
        }


        public OrganizationDTO ToOrganizationDTO(Organization item, bool withDetails)
        {
            OrganizationDTO result = new OrganizationDTO();
            result.ID = item.ID;
            result.Name = item.Name;
            result.OrganizationTypeID = item.OrganizationTypeID;
            if (withDetails)
            {
                foreach (var dbpost in item.OrganizationPost)
                {
                    OrganizationPostDTO post = new OrganizationPostDTO();
                    post.CurrentUserID = dbpost.UserID??0;
                    post.OrganizationTypeRoleTypeID = dbpost.OrganizationType_RoleTypeID;
                    post.Name = dbpost.Name;
                    result.OrganizationPosts.Add(post);
                }
            }
            return result;
        }

        public void SaveOrganization(OrganizationDTO organizationDto)
        {
            using (var context = new MyProjectEntities())
            {
                Organization dbOrganization = null;
                if (organizationDto.ID == 0)
                {
                    dbOrganization = new Organization();
                    dbOrganization.SecuritySubject = new SecuritySubject();
                    dbOrganization.SecuritySubject.Type = (int)SecuritySubjectType.Organization;
                }
                else
                    dbOrganization = context.Organization.First(x => x.ID == organizationDto.ID);

                dbOrganization.Name = organizationDto.Name;
                dbOrganization.OrganizationTypeID = organizationDto.OrganizationTypeID;

                //چیزی حذف نمیشود
                foreach (var post in organizationDto.OrganizationPosts)
                {
                    var dbPost = dbOrganization.OrganizationPost.FirstOrDefault(x => x.ID == post.ID);
                    if (dbPost == null)
                    {
                        dbPost = new OrganizationPost();
                        dbOrganization.OrganizationPost.Add(dbPost);
                        dbPost.SecuritySubject = new SecuritySubject();
                        dbPost.SecuritySubject.Type = (int)SecuritySubjectType.OrganizationPost;
                    }
                    dbPost.Name = post.Name;
                    dbPost.UserID = post.CurrentUserID;
                    dbPost.OrganizationType_RoleTypeID = post.OrganizationTypeRoleTypeID;
                }

                if (dbOrganization.ID == 0)
                    context.Organization.Add(dbOrganization);
                context.SaveChanges();
            }
        }



        //public List<OrganizationDTO> GetOrganizationsByUser(int userID)
        //{
        //    using (var context = new MyProjectEntities())
        //    {
        //        return ToOrganizationDTOList(context.Organization.Where(x => x.Organization_User.Any(y => y.UserID == userID)).ToList());
        //    }
        //}


        //public int AddUserToOrganization(int OrganizationID, int userID)
        //{
        //    using (var context = new MyProjectEntities())
        //    {
        //        var organizationUser = context.Organization_User.FirstOrDefault(x => x.OrganizationID == OrganizationID
        //        && x.UserID == userID);

        //        if (organizationUser == null)
        //            context.Organization_User.Add(new Organization_User() { OrganizationID = OrganizationID, UserID = userID });

        //        context.SaveChanges();
        //        return organizationUser.ID;
        //    }
        //}

        //public OrganizationUserDTO GetOrganizationsUser(int organizationID, int userID)
        //{
        //    using (var context = new MyProjectEntities())
        //    {
        //        return ToOrganizationUserDTO(context.Organization_User.First(x => x.OrganizationID == organizationID && x.UserID == userID));
        //    }
        //}

        //private OrganizationUserDTO ToOrganizationUserDTO(Organization_User item)
        //{
        //    OrganizationUserDTO result = new OrganizationUserDTO();
        //    result.ID = item.ID;
        //    result.OrganizationID = item.OrganizationID;
        //    result.UserID = item.UserID;
        //    result.IsDefault = item.IsDefault == true;
        //    return result;
        //}
        public OrganizationPostDTO GetOrganizationPostsByID(int organizationPostID)
        {

            using (var context = new MyProjectEntities())
            {
                var post = context.OrganizationPost.First(x => x.ID == organizationPostID);
                return ToOrganizationPostDTO(post);
            }
        }
        public List<OrganizationPostDTO> GetOrganizationPostsByIDs(List<int> organizationPostIDs)
        {
            List<OrganizationPostDTO> result = new List<OrganizationPostDTO>();
            using (var context = new MyProjectEntities())
            {
                foreach (var organizationPostID in organizationPostIDs)
                {
                    var post = context.OrganizationPost.First(x => x.ID == organizationPostID);
                    result.Add(ToOrganizationPostDTO(post));
                }
                return result;
            }
        }
        public List<OrganizationPostDTO> GetOrganizationPostsByUserID(int userID)
        {
            List<OrganizationPostDTO> result = new List<OrganizationPostDTO>();
            using (var context = new MyProjectEntities())
            {

                var posts = context.OrganizationPost.Where(x => x.UserID == userID);
                foreach (var item in posts)
                {
                    result.Add(ToOrganizationPostDTO(item));
                }

                return result;
            }
        }
        //public List<OrganizationPostDTO> ToOrganizationPostDTOList(IQueryable<OrganizationPost> posts)
        //{
        //    List<OrganizationPostDTO> result = new List<OrganizationPostDTO>();
        //    foreach (var item in posts)
        //    {
        //        var RoleDto = ToOrganizationPostDTO(item);

        //        result.Add(RoleDto);
        //    }
        //    return result;
        //}
        public OrganizationPostDTO ToOrganizationPostDTO(OrganizationPost item)
        {
            OrganizationPostDTO result = new OrganizationPostDTO();
            result.ID = item.ID;
            result.Name = item.Name;
            result.CurrentUserID = item.UserID??0;
            if (result.CurrentUserID!=0)
            {
                BizUser bizUser = new MyModelManager.BizUser();
                result.CurrentUser = bizUser.GetUser(result.CurrentUserID);
                result.Name += " / " + result.CurrentUser.FullName;
            }
            result.OrganizationID = item.OrganizationID;
            result.OrganizationName = item.Organization.Name;
            result.OrganizationTypeID = item.Organization.OrganizationTypeID;
            result.OrganizationTypeRoleTypeID = item.OrganizationType_RoleTypeID;
            result.RoleTypeID = item.OrganizationType_RoleType.RoleTypeID;
            result.IsAdmin = item.OrganizationType_RoleType.IsAdmin == true;
            result.IsSuperAdmin = item.OrganizationType_RoleType.RoleType.IsSuperAdmin == true;
            return result;
        }

        public List<OrganizationPostDTO> GetOrganizationPosts(string searchText)
        {
            List<OrganizationPostDTO> result = new List<OrganizationPostDTO>();
            using (var context = new MyProjectEntities())
            {

                var posts = context.OrganizationPost.Where(x => x.Name.Contains(searchText));
                foreach (var item in posts)
                {
                    result.Add(ToOrganizationPostDTO(item));
                }

                return result;
            }
        }
        internal OrganizationPostDTO GetOrganizationPost(int id)
        {
            List<OrganizationPostDTO> result = new List<OrganizationPostDTO>();
            using (var context = new MyProjectEntities())
            {

                var post = context.OrganizationPost.First(x => x.ID == id);
                return ToOrganizationPostDTO(post);
            }
        }

        public List<OrganizationPostDTO> GetOrganizationPostsByOrganizationIds(string searchText, List<int> organizationIds)
        {
            List<OrganizationPostDTO> result = new List<OrganizationPostDTO>();
            using (var context = new MyProjectEntities())
            {

                var posts = context.OrganizationPost.Where(x => x.Name.Contains(searchText) && organizationIds.Contains(x.OrganizationID));
                foreach (var item in posts)
                {
                    result.Add(ToOrganizationPostDTO(item));
                }

                return result;
            }
        }
        public List<OrganizationPostDTO> GetOrganizationPostsByRoleTypeIds(string searchText, List<int> roleTypeIds)
        {
            List<OrganizationPostDTO> result = new List<OrganizationPostDTO>();
            using (var context = new MyProjectEntities())
            {

                var posts = context.OrganizationPost.Where(x => x.Name.Contains(searchText) && roleTypeIds.Contains(x.OrganizationType_RoleType.RoleTypeID));
                foreach (var item in posts)
                {
                    result.Add(ToOrganizationPostDTO(item));
                }

                return result;
            }
        }
        //public void AddOrganizationPost(int organizationUserID, int roleTypeID, string name)
        //{
        //    using (var context = new MyProjectEntities())
        //    {
        //        var organizationPost = context.OrganizationPost.FirstOrDefault(x => x.OrganizationUserID == organizationUserID
        //        && x.OrganizationRoleTypeID == roleTypeID);

        //        if (organizationPost == null)
        //            context.OrganizationPost.Add(new OrganizationPost() { OrganizationUserID = organizationUserID, OrganizationRoleTypeID = roleTypeID, Name = name });

        //        context.SaveChanges();
        //    }
        //}
        //public List<OrganizationActionObjectDTO> GetOrganizationActionsByObject(int objectID, int OrganizationID)
        //{
        //    List<OrganizationActionObjectDTO> result = new List<OrganizationActionObjectDTO>();
        //    using (var context = new MyProjectEntities())
        //    {
        //        var OrganizationActionObjectList = context.Organization_Action_Object.Where(x => x.ObjectID == objectID && x.OrganizationID == OrganizationID);
        //        foreach (var dbItem in OrganizationActionObjectList)
        //        {
        //            OrganizationActionObjectDTO item = new OrganizationActionObjectDTO();
        //            item.ActionID = dbItem.ActionID;
        //            item.ObjectID = dbItem.ObjectID;
        //            item.OrganizationID = dbItem.OrganizationID;
        //            result.Add(item);
        //        }
        //    }
        //    return result;
        //}

        //public List<ActionDTO> GetOrganizationActionsByObject(string objectCategory,string    objectIdentity, int OrganizationID)
        //{
        //    List<ActionDTO> result = new List<ActionDTO>();
        //    using (var context = new MyProjectEntities())
        //    {
        //        foreach (var dbItem in context.Actions)
        //        {
        //            ActionDTO item = new ActionDTO();
        //            item.Selected = context.Organization_Action_Object.Any(x => x.ActionID == dbItem.ID && x.ObjectIdentity == objectIdentity && x.ObjectCategory == objectCategory && x.OrganizationID == OrganizationID);
        //            item.ActionName = dbItem.ActionName;
        //            item.ID = dbItem.ID;
        //            result.Add(item);
        //        }
        //    }
        //    return result;
        //}

        //public void SaveOrganizationActoins(string objectCategory,string objectIdentity, int OrganizationID, List<ActionDTO> listActions)
        //{
        //    using (var context = new MyProjectEntities())
        //    {
        //        var selectedActionIDs = listActions.Where(x => x.Selected == true).Select(x => x.ID).ToList();
        //        var removeList = context.Organization_Action_Object.Where(x => x.ObjectIdentity == objectIdentity && x.ObjectCategory == objectCategory && x.OrganizationID == OrganizationID && !selectedActionIDs.Any(y => y == x.ActionID));
        //        foreach (var removeItem in removeList)
        //        {
        //            context.Organization_Action_Object.Remove(context.Organization_Action_Object.First(x => x.ID == removeItem.ID));
        //        }
        //        foreach (var item in listActions.Where(x=>x.Selected==true))
        //        {
        //            if (!context.Organization_Action_Object.Any(x => x.ObjectIdentity == objectIdentity && x.ObjectCategory == objectCategory && x.OrganizationID == OrganizationID && x.ActionID == item.ID))
        //            {
        //                context.Organization_Action_Object.Add(new Organization_Action_Object() { ObjectIdentity = objectIdentity,ObjectCategory = objectCategory , OrganizationID = OrganizationID, ActionID = item.ID });
        //            }
        //        }
        //        context.SaveChanges();
        //    }
        //}
    }


    //public class OrganizationActionObjectDTO
    //{
    //    public int OrganizationID { set; get; }
    //    public int ActionID { set; get; }
    //    public int ObjectID { set; get; }

    //}

}
