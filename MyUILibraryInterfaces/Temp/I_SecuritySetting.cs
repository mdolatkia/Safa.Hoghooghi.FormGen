
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary
{
    public interface I_SecuritySetting
    {
        //event EventHandler<AdminOrganizationChangedArg> AdmindOrganizationChanged;
        event EventHandler<AdminOrganizationPostConfirmedArg> AdminOrganizationPostsConfirmed;
        event EventHandler AssignedOrganizationPostsConfirmed;


        event EventHandler<OrganizationPostSelectedArg> SearchedOrganizationPostSelected;
        event EventHandler<OrganizationPostSearchArg> OrganizationPostSearchChanged;
        void LoadAssignedOrganizationPosts(List<OrganizationPostDTO> organizationPosts);
        //void SelectAssignedOrganizations(List<OrganizationPostDTO> organizationPosts);
        //void ShowAssignedOrganizationPosts(List<OrganizationPostDTO> posts);
        List<OrganizationPostDTO> ConfirmedOrganizatoinPosts { set; get; }
        bool ShowAdminTab { set; }
        bool ShowByPassSecurityCheckBox { set; }
        bool ByPassSecurityCheckBoxValue { get; set; }
        //void LoadAdminOrganizations(List<OrganizationDTO> organizations);
        void LoadSearchedAdminOrganizationPosts(List<OrganizationPostDTO> posts);
        void LoadConfirmedAdminOrganizationPosts(List<OrganizationPostDTO> posts);
        //void LoadMultiSelectAdminRoles(List<RoleDTO> roles);
    }
    //public class AssignedOrganizationChangedArg : EventArgs
    //{
    //    public OrganizationWithRolesDTO Organization { set; get; }
    //}
    //public class AdminOrganizationChangedArg : EventArgs
    //{
    //    public OrganizationDTO Organization { set; get; }
    //}
    //public class AssignedOrganizationConfirmedArg : EventArgs
    //{
    //    public OrganizationWithRolesDTO Organization { set; get; }
    //}
    public class OrganizationPostSearchArg : EventArgs
    {
        public string SearchText { set; get; }
    }
    public class OrganizationPostSelectedArg : EventArgs
    {
        public List<OrganizationPostDTO> SelectedOrganizationPosts { set; get; }
    }

    public class AssignedOrganizationPostsConfirmedArg : EventArgs
    {
        public List<OrganizationPostDTO> SelectedOrganizationPosts { set; get; }
    }
    public class AdminOrganizationPostConfirmedArg : EventArgs
    {
        public bool ByPassSecurity { set; get; }
        public List<OrganizationPostDTO> SelectedOrganizationPosts { set; get; }
        //public List<int> SelectedRoleIds { set; get; }
    }
}
