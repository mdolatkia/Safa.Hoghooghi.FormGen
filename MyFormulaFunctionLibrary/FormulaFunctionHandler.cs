using ModelEntites;
using MyFormulaLibrary;

using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStateFunctionLibrary
{
    public class FormulaFunctionHandler
    {
        public object CalculateFormula(int formulaID, List<DP_DataRepository> allDataItems, DP_DataRepository mainDataItem)
        {
            FormulaInstance formulaInstance = new FormulaInstance(allDataItems, mainDataItem, BindableTypeDescriptorIntention.FormulaCalculation);
            return formulaInstance.CalculateFormula(formulaID);

        }
    }


        //private StateFunctionResult GetStateFunctionResult(StateFunctionDTO StateFunction, List<object> parameters)
        //{
        //    var result = ReflectionHelper.CallMethod(StateFunction.Path, StateFunction.ClassName, StateFunction.FunctionName, parameters.ToArray());
        //    return result as StateFunctionResult;
        //}



    
}
