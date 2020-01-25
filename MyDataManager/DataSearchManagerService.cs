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
    public class DataSearchManagerService
    {
        BizEntitySearch bizEntitySearch = new BizEntitySearch();
        BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
        public EntitySearchDTO GetEntitySearch(DR_Requester requester, int ID)
        {
            return bizEntitySearch.GetEntitySearch(requester,ID);
        }

        public EntitySearchDTO GetDefaultEntitySearch(DR_Requester requester, int entityID)
        {
            return bizEntitySearch.GetDefaultEntitySearch(requester,entityID);
        }



    }
}
