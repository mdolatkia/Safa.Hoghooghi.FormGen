using ModelEntites;
using MyCodeFunctionLibrary;
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
using MyFormulaFunctionStateFunctionLibrary;

namespace MyFormulaManagerService
{
    public class StateManagerService
    {

        public StateResult CalculateState(int stateID, DP_DataRepository mainDataItem, DR_Requester requester)
        {
            StateHandler handler = new StateHandler();
            return handler.GetStateResult(stateID, mainDataItem,requester);
        }
        public StateResult CalculateState(EntityStateDTO state, DP_DataRepository mainDataItem, DR_Requester requester)
        {
            StateHandler handler = new StateHandler();
            return handler.GetStateResult(state, mainDataItem, requester);
        }

        //public FormulaDTO GetFormula(int formulaID)
        //{
        //    BizFormula bizFormula = new BizFormula();
        //    return bizFormula.GetFormula(formulaID);
        //}

    }
}
