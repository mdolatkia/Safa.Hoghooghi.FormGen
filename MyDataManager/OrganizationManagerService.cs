using ModelEntites;
using MyDataManagerBusiness;
using MyModelManager;
using MyPackageManager;

using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDataManagerService
{
    public class OrganizationManagerService
    {
        BizOrganization bizOrganization = new BizOrganization();
        BizUser bizUser = new BizUser();
        public List<OrganizationPostDTO> GetOrganizationPosts(int userID)
        {
         
            return bizOrganization.GetOrganizationPostsByUserID(userID);
        }
        public List<OrganizationPostDTO> GetOrganizationPosts(List<int> postIDs)
        {
         
            return bizOrganization.GetOrganizationPostsByIDs(postIDs);
        }
        public List<OrganizationPostDTO> GetOrganizationPosts(string searchText)
        {
         
            return bizOrganization.GetOrganizationPosts(searchText);
        }
        public List<OrganizationPostDTO> GetOrganizationPostsByOrganizationIds(string searchText, List<int> organizationIds)
        {
           
            return bizOrganization.GetOrganizationPostsByOrganizationIds(searchText, organizationIds);
        }
        public List<OrganizationPostDTO> GetOrganizationPostsByRoleTypeIds(string searchText, List<int> roleTypeIds)
        {
         
            return bizOrganization.GetOrganizationPostsByRoleTypeIds(searchText, roleTypeIds);
        }
        public UserDTO GetUser(int userID)
        {

            return bizUser.GetUser(userID);
        }
    }
}
