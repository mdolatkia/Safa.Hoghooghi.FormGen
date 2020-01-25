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
using MyLogManager;

namespace MyDataManagerService
{

    public class LogManagerService
    {
        BizLogManager bizLogManager = new BizLogManager();
        public List<DataLogDTO> SearchDataLogs(DR_Requester requester, int entityID, DateTime? fromDate, DateTime? toDate, DP_BaseData dataItem, DataLogType? logType,
            int columnID, int userID, bool? withMajorException, bool? withMinorException)
        {
            return bizLogManager.SearchDataLogs(requester, entityID, fromDate, toDate, dataItem, logType, columnID, userID, withMajorException, withMinorException);
        }
        public List<DataLogDTO> SearchDataLogs(DR_Requester requester, int relatedITemID, DataLogType? logType)
        {
            return bizLogManager.SearchDataLogs(requester, relatedITemID, logType);
        }
        public DataLogDTO GetDataLog(DR_Requester requester, int iD)
        {
            BizLogManager bizLogManager = new BizLogManager();
            return bizLogManager.GetDataLog( requester, iD);
        }

        public List<DataLogDTO> GetDataLogsByPackageID(DR_Requester requester, Guid guid)
        {
            BizLogManager bizLogManager = new BizLogManager();
            return bizLogManager.GetDataLogsByPackageID( requester, guid);
        }
    }
}
