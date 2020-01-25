using ModelEntites;
using MyCodeFunctionLibrary;
using MyDataManagerBusiness;

using MyFormulaFunctionStateFunctionLibrary;
using MyInterfaces;
using MyDataItemManager;
using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyDataManagerService
{
    public class DataLinkManagerService
    {
        BizDataLink bizDataLink = new BizDataLink();
        public DataLinkDTO GetDataLink(DR_Requester dR_Requester, int ID)
        {
            return bizDataLink.GetDataLink(dR_Requester,ID);
        }
     
        public List<DataLinkDTO> GetDataLinks(DR_Requester dR_Requester, int entityID)
        {
            return bizDataLink.GetDataLinkByEntitiyID(dR_Requester,entityID);
        }

        public List<DataLinkDTO> SearchDatalinks(DR_Requester dR_Requester, string singleFilterValue)
        {
            return bizDataLink.SearchDatalinks( dR_Requester, singleFilterValue);
        }
    }
}
