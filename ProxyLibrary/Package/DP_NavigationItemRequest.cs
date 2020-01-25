using ModelEntites;
using System;
using System.Collections.Generic;
namespace ProxyLibrary
{
	public class DP_NavigationItemRequest : BaseRequest 
 {
        public DP_NavigationItemRequest(DR_Requester Requester) : base(Requester)
        {

        }
        public NavigationTreeDTO NavigationItem;
	}

}
