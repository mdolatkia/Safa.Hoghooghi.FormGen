using ModelEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary
{

    public class Arg_NavigationTreeRequest : EventArgs
    {

    }
    public class Arg_NavigationItemRequest : EventArgs
    {
        public NavigationItemDTO NavigationItem;
    }

   
}
