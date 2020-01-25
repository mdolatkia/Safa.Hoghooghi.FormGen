using ModelEntites;
using MyGeneralLibrary;
using MyInterfaces;
using MyModelManager;
using MyUILibrary.EntityArea;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCodeFunctionLibrary
{
    public class CodeFunctionHandler
    {
        BizCodeFunction bizCodeFunction = new BizCodeFunction();

        //فانکشنها یکی شوند
        public LetterConvertToExternalResult GetLetterSendingCodeFunctionResult(DR_Requester resuester, int codeFunctionID, LetterDTO letter)
        {
            var codeFunction = bizCodeFunction.GetCodeFunction(codeFunctionID);
            if (codeFunction.ParamType == ModelEntites.Enum_CodeFunctionParamType.LetterConvert)
            {
                var parameters = new List<object>();
                parameters.Add(new LetterFunctionParam(letter, resuester));
                return GetLetterSendingCodeFunctionResult(codeFunction, parameters);

            }
            else
                return null;
        }
        public CodeFunctionResult GetCodeFunctionResult(DR_Requester resuester, int codeFunctionID, LetterDTO letter)
        {
            var codeFunction = bizCodeFunction.GetCodeFunction(codeFunctionID);
            if (codeFunction.ParamType == ModelEntites.Enum_CodeFunctionParamType.LetterFunction)
            {
                var parameters = new List<object>();
                parameters.Add(new LetterFunctionParam(letter, resuester));
                return GetCodeFunctionResult(codeFunction, parameters);
            }
            else
                return null;
        }

        public CodeFunctionResult GetCodeFunctionResult(DR_Requester resuester, int codeFunctionID, DP_DataRepository dataItem)
        {
            var parameters = new List<object>();
            var codeFunction = bizCodeFunction.GetCodeFunction(codeFunctionID);
            if (codeFunction.ParamType == ModelEntites.Enum_CodeFunctionParamType.OneDataItem)
            {
                parameters.Add(new CodeFunctionParamOneDataItem(dataItem, resuester));
            }
            else if (codeFunction.ParamType == ModelEntites.Enum_CodeFunctionParamType.KeyColumns)
            {
                var codeFunctionEntity = bizCodeFunction.GetCodeFunctionEntity(codeFunctionID, dataItem.TargetEntityID);

                foreach (var column in codeFunctionEntity.CodeFunctionEntityColumns)
                {
                    EntityInstanceProperty property = dataItem.GetProperty(column.ColumnID);
                    if (property != null)
                    {
                        //stringParamList += (stringParamList == "" ? "" : ",") + column.FunctionColumnParamName;
                        //paramList.Add(column.FunctionColumnParamName);
                        parameters.Add(Convert.ChangeType(property.Value, column.FunctionColumnDotNetType));
                    }
                }
            }
            return GetCodeFunctionResult(codeFunction, parameters);
        }

        public CodeFunctionResult GetCodeFunctionResult(DR_Requester resuester, int codeFunctionID, List<DP_DataRepository> allDataItems)
        {
            var codeFunction = bizCodeFunction.GetCodeFunction(codeFunctionID);
            if (codeFunction.ParamType == ModelEntites.Enum_CodeFunctionParamType.ManyDataItems)
            {
                var parameters = new List<object>() { new CodeFunctionParamManyDataItems(allDataItems, resuester) };
                return GetCodeFunctionResult(codeFunction, parameters);
            }
            else return null;
        }

        public CodeFunctionResult GetCodeFunctionResult(int codeFunctionID, List<object> parameters)
        {
            var codeFunction = bizCodeFunction.GetCodeFunction(codeFunctionID);
            return GetCodeFunctionResult(codeFunction, parameters);
        }

        private CodeFunctionResult GetCodeFunctionResult(CodeFunctionDTO codeFunction, List<object> parameters)
        {
            try
            {
                var result = ReflectionHelper.CallMethod(codeFunction.Path, codeFunction.ClassName, codeFunction.FunctionName, parameters.ToArray());
                return (CodeFunctionResult)result;
            }
            catch (Exception ex)
            {
                CodeFunctionResult result = new CodeFunctionResult();
                result.ExecutionException = ex;
                return result;
            }
        }
        private LetterConvertToExternalResult GetLetterSendingCodeFunctionResult(CodeFunctionDTO codeFunction, List<object> parameters)
        {
            try
            {
                var result = ReflectionHelper.CallMethod(codeFunction.Path, codeFunction.ClassName, codeFunction.FunctionName, parameters.ToArray());
                return (LetterConvertToExternalResult)result;
            }
            catch (Exception ex)
            {
                LetterConvertToExternalResult result = new LetterConvertToExternalResult();
                result.ExecutionException = ex;
                return result;
            }
        }

        //private CommandFunctionResult GetCommandFunctionResult(CodeFunctionDTO codeFunction, List<object> parameters)
        //{
        //    var result = ReflectionHelper.CallMethod(codeFunction.Path, codeFunction.ClassName, codeFunction.FunctionName, parameters.ToArray());
        //    return result as CommandFunctionResult;
        //}
        //private LetterFunctionResult GetLetterFunctionResult(CodeFunctionDTO codeFunction, List<object> parameters)
        //{
        //    var result = ReflectionHelper.CallMethod(codeFunction.Path, codeFunction.ClassName, codeFunction.FunctionName, parameters.ToArray());
        //    return result as LetterFunctionResult;
        //}


    }
}
