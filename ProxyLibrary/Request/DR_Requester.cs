
using System;
using System.Collections.Generic;
namespace ProxyLibrary
{
	public class DR_Requester {

        public DR_Requester()
        {

        }
        public DR_Requester(int a)
        {
            Successors = new List<DR_Requester>();
        }


        public int Identity;
        //public int OrganizationID;
        public List<int> PostIds;
        public List<OrganizationPostDTO> Posts;
        //public List<RoleDTO> Roles;
        //پر شود
        //public List<int> RoleGroupIds;
        public bool SkipSecurity;

        public string Name;


		public Enum_DR_RequesterType Type;


		public List<DR_Requester> Successors;

        public string LocationInfo { get; set; }
    }

}
