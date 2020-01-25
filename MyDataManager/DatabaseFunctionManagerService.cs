using ModelEntites;
using MyCodeFunctionLibrary;
using MyDatabaseFunctionLibrary;
using MyDataManagerBusiness;

using MyFormulaFunctionStateFunctionLibrary;
using MyInterfaces;
using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDataManagerService
{
    public class DatabaseFunctionManagerService
    {

        public DatabaseFunctionResult GetFunctionValue(DR_Requester requester, int dbFunctionID, DP_DataRepository dataItem)
        {
            DatabaseFunctionHandler handler = new DatabaseFunctionHandler();
            return handler.GetDatabaseFunctionValue(requester, dbFunctionID, dataItem);
        }

    }
}
