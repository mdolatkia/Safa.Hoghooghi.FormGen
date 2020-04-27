using MyConnectionManager;
using MyDataManagerBusiness;
using MyInterfaces;
using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelEntites;

namespace MyDatabaseFunctionLibrary
{
    public class DatabaseFunctionHandler
    {
        BizDatabaseFunction bizDatabaseFunction = new BizDatabaseFunction();
        public DatabaseFunctionResult GetDatabaseFunctionValue(DR_Requester requester, int functionID, DP_DataRepository dataItem)
        {
            DatabaseFunctionResult result = new DatabaseFunctionResult();
            var databaseFunctionEntity = bizDatabaseFunction.GetDatabaseFunctionEntity(functionID, dataItem.TargetEntityID);
            var stringParamList = "";
            List<object> paramList = new List<object>();
            if (databaseFunctionEntity != null)
            {
                // List<Tuple<string, string>> parameters = new List<Tuple<string, string>>();


                foreach (var column in databaseFunctionEntity.DatabaseFunctionEntityColumns)
                {
                    if (column.ColumnID != 0)
                    {
                        EntityInstanceProperty property = dataItem.GetProperty(column.ColumnID);
                        if (property != null)
                        {
                            stringParamList += (stringParamList == "" ? "" : ",") + column.FunctionColumnParamName;
                            paramList.Add(column.FunctionColumnParamName);
                            paramList.Add(property.Value);
                            //   parameters.Add(new Tuple<string, string>(column.ParameterName, property.Value));
                        }
                    }
                    else
                    {
                        if (column.FixedParam == Enum_FixedParam.RequesterIdentity)
                        {
                            stringParamList += (stringParamList == "" ? "" : ",") + column.FunctionColumnParamName;
                            paramList.Add(column.FunctionColumnParamName);
                            paramList.Add(requester.Identity.ToString());
                        }

                    }



                }
            }

            return GetDatabaseFunctionValue(databaseFunctionEntity.DatabaseFunction, paramList, stringParamList);
        }



        public DatabaseFunctionResult GetDatabaseFunctionValue(DR_Requester resuester, int functionID, params string[] parameters)
        {
            DatabaseFunctionResult result = new DatabaseFunctionResult();
            var databaseFunction = bizDatabaseFunction.GetDatabaseFunction(functionID);
            //var dbHelper = ConnectionManager.GetDBHelper(dataItem.TargetEntityID);
            if (parameters.Count() != databaseFunction.DatabaseFunctionParameter.Count)
            {
                result.ExecutionException = new Exception("تعداد پارامترهای ارسالی و تابع یکسان نمیباشد");
                result.Result = null;
                return result;
            }
            else
            {
                var stringParamList = "";
                List<object> paramList = new List<object>();
                int index = 0;
                foreach (var column in databaseFunction.DatabaseFunctionParameter)
                {

                    stringParamList += (stringParamList == "" ? "" : ",") + column.ParameterName;
                    paramList.Add(column.ParameterName);
                    paramList.Add(parameters[index]);
                    index++;
                }
                return GetDatabaseFunctionValue(databaseFunction, paramList, stringParamList);

            }

        }
        private DatabaseFunctionResult GetDatabaseFunctionValue(DatabaseFunctionDTO databaseFunction, List<object> paramList, string stringParamList)
        {
            try
            {
                DatabaseFunctionResult result = new DatabaseFunctionResult();
                var dbHelper = ConnectionManager.GetDBHelper(databaseFunction.DatabaseID);
                if (databaseFunction.Type == ModelEntites.Enum_DatabaseFunctionType.Function)
                {
                    if (stringParamList != "")
                        stringParamList = "(" + stringParamList + ")";

                    result.Result = dbHelper.ExecuteScalar("select " + (string.IsNullOrEmpty(databaseFunction.RelatedSchema) ? "" : databaseFunction.RelatedSchema + ".") + databaseFunction.FunctionName + stringParamList, paramList.ToArray());
                }
                else if (databaseFunction.Type == ModelEntites.Enum_DatabaseFunctionType.StoredProcedure)
                {
                    result.Result = dbHelper.ExecuteStoredProcedure((string.IsNullOrEmpty(databaseFunction.RelatedSchema) ? "" : databaseFunction.RelatedSchema + ".") + databaseFunction.FunctionName, paramList.ToArray());
                }
                if (result.Result is string)
                {
                    if (Convert.ToString(result.Result) != null)
                    {
                        if (Convert.ToString(result.Result).Contains("@"))
                        {
                            var splt = Convert.ToString(result.Result).Split("@".ToCharArray());
                            var res = splt[0];
                            if (res != null && res.ToLower() == "ExceptionWithMessage".ToLower())
                                result.ExecutionException = new Exception(splt[1]);
                            else
                                result.Result = res;
                            
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                DatabaseFunctionResult result = new DatabaseFunctionResult();
                result.ExecutionException = ex;
                return result;
            }
        }
    }
}
