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
    public class GridViewManagerService
    {
        BizEntityGridView bizEntityGridView = new BizEntityGridView();
        BizEntityListView bizEntityListView = new BizEntityListView();
        public GridViewSettingDTO GetGridViewSetting(int entitiyID)
        {
            return bizEntityGridView.GetGridViewSetting(entitiyID, true);
        }

        //public List<EntityListViewDTO> GetEntityListViews(DR_Requester requester, int entitiyID)
        //{
        //    return bizEntityListView.GetEntityListViews( requester, entitiyID);
        //}
        //public EntityListViewDTO GetEntityListView(DR_Requester requester, int ID)
        //{
        //    return bizEntityListView.GetEntityListView(requester, ID);
        //}

        //public EntityListViewDTO GetDefaultEntityListView(DR_Requester requester, int entityID)
        //{
        //    return bizEntityListView.GetEntityDefaultListView(requester, entityID);
        //}
    }
}
