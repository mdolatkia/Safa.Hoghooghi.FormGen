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
    public class DataViewManagerService
    {
        BizEntityDataView bizEntityDataView = new BizEntityDataView();
        public DataViewSettingDTO GetDataViewSetting(int entitiyID)
        {
            return bizEntityDataView.GetDataViewSetting(entitiyID, true);
        }

     
    }
}
