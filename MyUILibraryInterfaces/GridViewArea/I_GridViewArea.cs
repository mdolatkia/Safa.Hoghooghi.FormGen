using ModelEntites;
using MyUILibrary.EntityArea;
using MyUILibraryInterfaces.EntityArea;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibraryInterfaces.GridViewArea
{


 

    public class EntitiListViewChangedArg : EventArgs
    {
        public int ListViewID { set; get; }
    }
   
}
