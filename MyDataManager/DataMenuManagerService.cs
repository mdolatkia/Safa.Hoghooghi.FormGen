using ModelEntites;
using MyDataManagerBusiness;
using MyExternalReportLibrary;
using MyModelManager;
using MyPackageManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDataManagerService
{
    public class DataMenuManagerService
    {
        BizDataMenuSetting bizDataMenuSetting = new BizDataMenuSetting();
        //public DataMenuSettingDTO GetDataMenuSetting(int entityID)
        //{
        //    return bizDataMenuSetting.GetDataMenuSetting(entityID, true);
        //}
        public List<DataMenu> GetDataMenu(DR_Requester requester, DP_DataView dataItem)
        {
            return bizDataMenuSetting.GetDataMenu(requester, dataItem);
        }
    }
}
