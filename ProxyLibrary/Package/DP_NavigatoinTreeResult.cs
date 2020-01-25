using ModelEntites;
using System;
using System.Collections.Generic;
namespace ProxyLibrary
{
    public class DP_NavigatoinTreeResult : BaseResult
    {
        public DP_NavigatoinTreeResult()
        {
            Items = new List<NavigationItemDTO>();
        }
        public List<NavigationItemDTO> Items;
    }

}
