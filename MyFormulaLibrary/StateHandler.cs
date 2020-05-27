using ModelEntites;

using MyInterfaces;
using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFormulaFunctionStateFunctionLibrary
{
    public class StateHandler
    {
        BizEntityState bizStateFunction = new BizEntityState();

        public StateResult GetStateResult(int StateFunctionID, DP_DataRepository mainDataItem, DR_Requester requester)
        {
            var state = bizStateFunction.GetEntityState(StateFunctionID, true);
            return GetStateResult(state, mainDataItem, requester);
            //////    var parameters = new List<object>();
            //////    var StateFunction = bizStateFunction.GetStateFunction(StateFunctionID);
            //////    if (StateFunction.ParamType == ModelEntites.Enum_StateFunctionParamType.OneDataItem)
            //////    {
            //////        parameters.Add(new StateFunctionParamOneDataItem(dataItem));
            //////    }
            //////    else if (StateFunction.ParamType == ModelEntites.Enum_StateFunctionParamType.KeyColumns)
            //////    {
            //////        var StateFunctionEntity = bizStateFunction.GetStateFunctionEntity(StateFunctionID, dataItem.TargetEntityID);

            //////        foreach (var column in StateFunctionEntity.StateFunctionEntityColumns)
            //////        {
            //////            EntityInstanceProperty property = dataItem.Properties.FirstOrDefault(x => x.ColumnID == column.ColumnID);
            //////            if (property != null)
            //////            {
            //////                //stringParamList += (stringParamList == "" ? "" : ",") + column.FunctionColumnParamName;
            //////                //paramList.Add(column.FunctionColumnParamName);
            //////                parameters.Add(Convert.ChangeType(property.Value, column.FunctionColumnDotNetType));
            //////            }
            //////        }
            //////    }
            //////    return GetStateFunctionResult(StateFunction, parameters);
        }
        public StateResult GetStateResult(EntityStateDTO state, DP_DataRepository mainDataItem, DR_Requester requester)
        {
            StateResult result = new StateResult();

            if (state.ColumnID != 0)
            {
                DataitemRelatedColumnValueHandler dataitemRelatedColumnValueHandler = new MyFormulaFunctionStateFunctionLibrary.DataitemRelatedColumnValueHandler();
                var value = dataitemRelatedColumnValueHandler.GetValueSomeHow(requester, mainDataItem, state.RelationshipTail, state.ColumnID);
                if (state.EntityStateOperator == Enum_EntityStateOperator.Equals)
                    result.Result = state.Values.Any(x => x.Value.Equals(value));
                else
                    result.Result = !state.Values.Any(x => x.Value.Equals(value));
            }
            else if (state.FormulaID != 0)
            {
                FormulaFunctionHandler FormulaFunctionHandler = new FormulaFunctionHandler();
                var value = FormulaFunctionHandler.CalculateFormula(state.FormulaID, mainDataItem, requester);
                if (state.EntityStateOperator == Enum_EntityStateOperator.Equals)
                    result.Result = state.Values.Any(x => x.Value.Equals(value.Result));
                else
                    result.Result = !state.Values.Any(x => x.Value.Equals(value.Result));
            }
            return result;

        }

        //private StateFunctionResult GetStateFunctionResult(StateFunctionDTO StateFunction, List<object> parameters)
        //{
        //    var result = ReflectionHelper.CallMethod(StateFunction.Path, StateFunction.ClassName, StateFunction.FunctionName, parameters.ToArray());
        //    return result as StateFunctionResult;
        //}



    }
}
