using DynamicExpresso;
using ModelEntites;
using MyFormulaFunctionStateFunctionLibrary;
using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFormulaFunctionStateFunctionLibrary
{

    public class FormulaFunctionHandler
    {
        List<FormulaUsageParemetersDTO> FormulaUsageParemeters = new List<FormulaUsageParemetersDTO>();
        BizFormula bizFormula = new BizFormula();
        FormulaDTO Formula { set; get; }
        public I_ExpressionEvaluator GetExpressionEvaluator(DP_DataRepository mainDataItem, DR_Requester requester, bool definition, List<int> usedFormulaIDs = null)
        {
            return FormulaInstanceInternalHelper.GetExpressionEvaluator(mainDataItem, requester, definition, usedFormulaIDs);
        }
        public FormulaResult CalculateFormula(int formulaID, DP_DataView mainDataItem, DR_Requester requester, bool deficition = false, List<int> usedFormulaIDs = null)
        {
            //DP_DataRepository data = new DP_DataRepository();
            return CalculateFormula(formulaID, mainDataItem, requester, deficition, usedFormulaIDs);
        }
        public FormulaResult CalculateFormula(int formulaID, DP_DataRepository mainDataItem, DR_Requester requester, bool deficition = false, List<int> usedFormulaIDs = null)
        {
            Formula = bizFormula.GetFormula(formulaID, true);
            return CalculateFormula(Formula.Formula, mainDataItem, requester, deficition, usedFormulaIDs);
        }

        public FormulaResult CalculateFormula(string expression, DP_DataRepository mainDataItem, DR_Requester requester, bool definition = false, List<int> usedFormulaIDs = null)
        {
            FormulaResult result = new FormulaResult();
            result.FormulaUsageParemeters = FormulaUsageParemeters;
            //FormulaInstance formulaInstance = null;
            try
            {

                var target = FormulaInstanceInternalHelper.GetExpressionEvaluator(mainDataItem, requester, definition, usedFormulaIDs);

                result.Result = target.Calculate(expression);

                //formulaInstance = new FormulaInstance(mainDataItem, requester, usedFormulaIDs);
                //formulaInstance.PropertyGetCalled += FormulaInstance_PropertyGetCalled;
                //var instanceResult = formulaInstance.CalculateExpression(expression);
                //if (formulaInstance.Exceptions.Any())
                //    throw new Exception("instance Error");
                //else
                //    result.Result = instanceResult;
                //formulaInstance.PropertyGetCalled -= FormulaInstance_PropertyGetCalled;


            }
            catch (Exception ex)
            {
                result.Exception = "خطا در محاسبه فرمول" + Environment.NewLine + ex.Message;
            }
            return result;
        }



        List<MyPropertyInfo> PropertyInfos = new List<MyPropertyInfo>();
        private void FormulaInstance_PropertyGetCalled(object sender, PropertyGetArg e)
        {
            if (e.PropertyInfo.PropertyType == PropertyType.Relationship)
                return;
            if (PropertyInfos.Any(x => x == e.PropertyInfo))
                return;
            if (!IsValidForUsage(e.PropertyInfo))
                return;
            PropertyInfos.Add(e.PropertyInfo);
            FormulaUsageParemetersDTO item = new FormulaUsageParemetersDTO();
            item.ParameterName = e.PropertyInfo.Name;
            item.ParameterValue = (e.PropertyInfo.Value != null ? e.PropertyInfo.Value.ToString() : "<Null>");
            item.RelationshipPropertyTail = e.PropertyInfo.RelationshipPropertyTail;// GetParentTail(e.PropertyInfo);

            if (e.FormulaUsageParemeters != null)
                foreach (var child in e.FormulaUsageParemeters)
                {
                    FormulaUsageParemeters.Add(item);
                }
            FormulaUsageParemeters.Add(item);
        }

        private bool IsValidForUsage(MyPropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType == PropertyType.Helper)
                return false;
            return true;
        }

        //private string GetParentTail(MyPropertyInfo propertyInfo, bool original = true)
        //{
        //    var RelationshipTail = "";

        //    if (propertyInfo.ParentProperty != null && propertyInfo.ParentProperty.PropertyType == PropertyType.Relationship)
        //    {
        //        var pKey = GetParentTail(propertyInfo.ParentProperty, false);
        //        //var key = propertyInfo.FormulaObject.DataItem.GUID.ToString();
        //        //foreach (var keyProperty in propertyInfo.FormulaObject.DataItem.KeyProperties)
        //        //{
        //        //    key += (key == "" ? "" : "&") + keyProperty.ColumnID + ":" + keyProperty.Value;
        //        //}
        //        if (!original)
        //            return (pKey != "" ? pKey + ">" : "") + key;
        //        else
        //            return propertyInfo.RelationshipTail + "@" + (pKey != "" ? pKey + ">" : "") + key;


        //    }
        //    return RelationshipTail;
        //}



        //////private string HandleException(Exception ex, FormulaInstance formulaInstance)
        //////{
        //////    var error = ex.Message;
        //////    if (error == null)
        //////        error = "";
        //////    if (formulaInstance != null)
        //////    {
        //////        foreach (var item in formulaInstance.Exceptions)
        //////        {
        //////            error += (error == "" ? "" : "<<>>") + item.Message;
        //////        }
        //////        //برای اکسپشن هندلینگ داخل قرمول اینستنس چون ممکن است تو در تو صدا زده شود باید لیست تهیه و مرتبا لیستها به هم اضافه شوند
        //////    }
        //////    var message = "خطا در محاسبه فرمول" + (error == "" ? "" : Environment.NewLine) + error;
        //////    return message;
        //////}
    }
   
  
   
    //public interface I_ExpressionEvaluator
    //{
    //    FormulaResult Calculate(string expression);
    //}
    //public class ExpressionEvaluator : I_ExpressionEvaluator
    //{
    //    I_Interpreter Interpreter { set; get; }
    //    public ExpressionEvaluator(DP_DataRepository myData, DR_Requester requester, bool definition)
    //    {
    //        Interpreter = FormulaInstanceInternalHelper.GetInterpreter(myData, requester, definition);
    //    }
    //    public FormulaResult Calculate(string expression)
    //    {
    //        return Interpreter.Calculate()
    //    }
    //}
}
