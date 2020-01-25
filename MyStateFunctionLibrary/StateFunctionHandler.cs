using ModelEntites;

using MyInterfaces;
using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStateFunctionLibrary
{
    public class StateFunctionHandler
    {
        BizEntityState bizStateFunction = new BizEntityState();

        public StateFunctionResult GetStateFunctionResult(int StateFunctionID, List<DP_DataRepository> allDataItems, DP_DataRepository mainDataItem)
        {
            StateFunctionResult result = new StateFunctionResult();
            var state = bizStateFunction.GetEntityState(StateFunctionID, true);
            if (state.ColumnID != 0)
            {
                var property = mainDataItem.DataInstance.Properties.FirstOrDefault(x => x.ColumnID == state.ColumnID);
                if (property != null)
                {
                    if (property.Value == state.Value)
                    {
                        result.Result = true;
                    }
                    else
                        result.Result = false;
                }
            }
            else if (state.FormulaID != 0)
            {
                FormulaFunctionHandler 
                e.PropertyInfo.Value = 
                var value = formulaInstance.CalculateFormula(state.FormulaID);
                if (value == state.Value)
                {
                    stateIsValid = true;
                }

            }

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
            //////            EntityInstanceProperty property = dataItem.DataInstance.Properties.FirstOrDefault(x => x.ColumnID == column.ColumnID);
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


        //private StateFunctionResult GetStateFunctionResult(StateFunctionDTO StateFunction, List<object> parameters)
        //{
        //    var result = ReflectionHelper.CallMethod(StateFunction.Path, StateFunction.ClassName, StateFunction.FunctionName, parameters.ToArray());
        //    return result as StateFunctionResult;
        //}



    }
}
