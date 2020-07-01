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
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;

namespace MyDatabaseFunctionLibrary
{
    public class DatabaseFunctionHandler
    {
        BizDatabaseFunction bizDatabaseFunction = new BizDatabaseFunction();
        public DatabaseFunctionResult GetDatabaseFunctionValue(DR_Requester requester, int functionID, object value)
        {
            var databaseFunction = bizDatabaseFunction.GetDatabaseFunction(functionID);
            List<Tuple<string, object>> parameters = new List<Tuple<string, object>>();
            if (databaseFunction.DatabaseFunctionParameter.Any(x => x.InputOutput != Enum_DatabaseFunctionParameterType.ReturnValue && x.InputOutput != Enum_DatabaseFunctionParameterType.Output))
            {
                var firstParam = databaseFunction.DatabaseFunctionParameter.OrderBy(c => c.Order).First(x => x.InputOutput != Enum_DatabaseFunctionParameterType.ReturnValue && x.InputOutput != Enum_DatabaseFunctionParameterType.Output);
                parameters.Add(new Tuple<string, object>(firstParam.ParameterName, value));
            }
            return GetDatabaseFunctionValue(requester, databaseFunction, parameters);
        }

        public DatabaseFunctionResult GetDatabaseFunctionValue(DR_Requester requester, int databaseID, string name, object value)
        {
            var databaseFunction = bizDatabaseFunction.GetDatabaseFunctionByName(databaseID, name);
            List<Tuple<string, object>> parameters = new List<Tuple<string, object>>();
            if (databaseFunction.DatabaseFunctionParameter.Any(x => x.InputOutput != Enum_DatabaseFunctionParameterType.ReturnValue && x.InputOutput != Enum_DatabaseFunctionParameterType.Output))
            {
                var firstParam = databaseFunction.DatabaseFunctionParameter.OrderBy(c => c.Order).First(x => x.InputOutput != Enum_DatabaseFunctionParameterType.ReturnValue && x.InputOutput != Enum_DatabaseFunctionParameterType.Output);
                parameters.Add(new Tuple<string, object>(firstParam.ParameterName, value));
            }
            return GetDatabaseFunctionValue(requester, databaseFunction, parameters);
        }

        public DatabaseFunctionResult GetDatabaseFunctionValue(DR_Requester requester, int databaseID, string name, List<Tuple<string, object>> parameters)
        {
            var databaseFunction = bizDatabaseFunction.GetDatabaseFunctionByName(databaseID, name);
            return GetDatabaseFunctionValue(requester, databaseFunction, parameters);
        }

        public DatabaseFunctionResult GetDatabaseFunctionValue(DR_Requester requester, int functionID, DP_DataRepository dataItem)
        {
            DatabaseFunctionResult result = new DatabaseFunctionResult();
            var databaseFunctionEntity = bizDatabaseFunction.GetDatabaseFunctionEntity(functionID, dataItem.TargetEntityID);
            //   List<object> paramList = new List<object>();
            List<Tuple<string, object>> parameters = new List<Tuple<string, object>>();
            if (databaseFunctionEntity != null)
            {
                foreach (var column in databaseFunctionEntity.DatabaseFunctionEntityColumns)
                {
                    if (column.ColumnID != 0)
                    {
                        EntityInstanceProperty property = dataItem.GetProperty(column.ColumnID);
                        if (property != null)
                        {
                            parameters.Add(new Tuple<string, object>(column.FunctionColumnParamName, property.Value));
                        }
                    }
                    else
                    {
                        if (column.FixedParam == Enum_FixedParam.RequesterIdentity)
                        {
                            parameters.Add(new Tuple<string, object>(column.FunctionColumnParamName, requester.Identity.ToString()));
                        }
                    }
                }
            }

            return GetDatabaseFunctionValue(requester, databaseFunctionEntity.DatabaseFunction, parameters);
        }

        //public DatabaseFunctionResult GetDatabaseFunctionValue(DR_Requester resuester, int functionID, params object[] parameters)
        //{
        //    DatabaseFunctionResult result = new DatabaseFunctionResult();
        //    var databaseFunction = bizDatabaseFunction.GetDatabaseFunction(functionID);
        //    return GetDatabaseFunctionValue(resuester, databaseFunction, parameters);

        //}
        private DatabaseFunctionResult GetDatabaseFunctionValue(DR_Requester resuester, DatabaseFunctionDTO databaseFunction, List<Tuple<string, object>> parameters = null)// params object[] parameters)
        {
            DatabaseFunctionResult result = new DatabaseFunctionResult();
            //if (parameters.Count() != databaseFunction.DatabaseFunctionParameter.Count(x => x.InputOutput != Enum_DatabaseFunctionParameterType.ReturnValue))
            //{
            //    result.ExecutionException = new Exception("تعداد پارامترهای ارسالی و تابع یکسان نمیباشد");
            //    result.Result = null;
            //    return result;
            //}
            //else
            //{
            //if (databaseFunction.Type == Enum_DatabaseFunctionType.Function)
            //{

            //}
            //else
            //{

            //}
            int paramCount = 0;
            if (parameters != null)
                paramCount = parameters.Count() ;
            List<IDataParameter> paramList = new List<IDataParameter>();

            if (databaseFunction.Type == Enum_DatabaseFunctionType.Function)
            {

                if (paramCount < databaseFunction.DatabaseFunctionParameter.Count(x => x.InputOutput == Enum_DatabaseFunctionParameterType.Input))
                {
                    result.ExecutionException = new Exception("تعداد پارامترهای ارسالی و تابع یکسان نمیباشد");
                    result.Result = null;
                    return result;
                }

                //    var indexer = 0;
                foreach (var column in databaseFunction.DatabaseFunctionParameter.Where(x => x.InputOutput == Enum_DatabaseFunctionParameterType.Input).OrderBy(x => x.Order))
                {
                    if (parameters != null && parameters.Any(x => x.Item1 == column.ParameterName))
                        paramList.Add(ToDbParameter(column, parameters.First(x => x.Item1 == column.ParameterName).Item2));
                    else
                    {
                        result.ExecutionException = new Exception("پارامتر" + " " + column.ParameterName + " " + "مقداردهی نشده است");
                        result.Result = null;
                        return result;
                    }
                }
                return GetDBFunctionValue(databaseFunction, paramList);
            }
            else
            {
                if (paramCount < databaseFunction.DatabaseFunctionParameter.Count(x => x.InputOutput == Enum_DatabaseFunctionParameterType.Input))
                {
                    result.ExecutionException = new Exception("تعداد پارامترهای ارسالی و تابع یکسان نمیباشد");
                    result.Result = null;
                    return result;
                }
                DatabaseFunctionColumnDTO outputParameter = null;
                DatabaseFunctionColumnDTO returnValueParameter = returnValueParameter = databaseFunction.DatabaseFunctionParameter.First(x => x.InputOutput == Enum_DatabaseFunctionParameterType.ReturnValue);
                outputParameter = databaseFunction.DatabaseFunctionParameter.FirstOrDefault(x => x.InputOutput == Enum_DatabaseFunctionParameterType.InputOutput || x.InputOutput == Enum_DatabaseFunctionParameterType.Output);
                if (outputParameter == null)
                {
                    if (returnValueParameter != null)
                        outputParameter = returnValueParameter;
                    else
                    {
                        result.ExecutionException = new Exception("این تابع خروجی ندارد!");
                        result.Result = null;
                        return result;
                    }

                }
                //List<Tuple<DatabaseFunctionColumnDTO, object>> columnsAndValues = new List<Tuple<DatabaseFunctionColumnDTO, object>>();
                //var indexer = 0;
                foreach (var column in databaseFunction.DatabaseFunctionParameter.Where(x => x.InputOutput != Enum_DatabaseFunctionParameterType.ReturnValue).OrderBy(x => x.Order))
                {
                    if (column.InputOutput != Enum_DatabaseFunctionParameterType.Output)
                    {
                        if (parameters != null && parameters.Any(x => x.Item1 == column.ParameterName))
                        {
                            paramList.Add(ToDbParameter(column, parameters.First(x => x.Item1 == column.ParameterName).Item2));
                        }
                        else
                        {
                            if (column.InputOutput == Enum_DatabaseFunctionParameterType.Input)
                            {
                                result.ExecutionException = new Exception("پارامتر" + " " + column.ParameterName + " " + "مقداردهی نشده است");
                                result.Result = null;
                                return result;
                            }
                            else
                            {
                                paramList.Add(ToDbParameter(column, GetPropertyDefaultValue(column.DotNetType)));
                            }
                        }
                    }
                    else
                    {
                        //تاحالا پارامتری که output خالی باشه ندیدم پس این تیکه تست نشده
                        paramList.Add(ToDbParameter(column, DBNull.Value));
                    }
                }
                if (returnValueParameter != null)
                    paramList.Add(ToDbParameter(returnValueParameter, DBNull.Value));
                return GetDBSPValue(databaseFunction, paramList, outputParameter.ParameterName);
            }

            //}

        }
        public static object GetPropertyDefaultValue(Type type)
        {

            if (type == typeof(long) || type == typeof(long?)
                    || type == typeof(int?) || type == typeof(int)
                           || type == typeof(short?) || type == typeof(short)
                             || type == typeof(byte?) || type == typeof(byte))
                return 1;
            else if (type == typeof(double?) || type == typeof(double))
                return (double)1;
            else if (type == typeof(decimal?) || type == typeof(decimal))
                return (decimal)1;
            else if (type == typeof(float?) || type == typeof(float))
                return (float)1;
            else if (type == typeof(Guid) || type == typeof(Guid?))
                return "";
            else if (type == typeof(string))
                return "";
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
                return DateTime.Now;
            else if (type == typeof(bool?) || type == typeof(bool))
                return true;

            return "";
        }
        private IDataParameter ToDbParameter(DatabaseFunctionColumnDTO column, object value)
        {
            IDataParameter result = new SqlParameter();
            if (column.InputOutput == Enum_DatabaseFunctionParameterType.Input)
                result.Direction = ParameterDirection.Input;
            else if (column.InputOutput == Enum_DatabaseFunctionParameterType.Output)
                result.Direction = ParameterDirection.Output;
            else if (column.InputOutput == Enum_DatabaseFunctionParameterType.InputOutput)
                result.Direction = ParameterDirection.InputOutput;
            else if (column.InputOutput == Enum_DatabaseFunctionParameterType.ReturnValue)
                result.Direction = ParameterDirection.ReturnValue;
            result.ParameterName = column.ParameterName;
            result.Value = value;
            return result;
        }

        private DatabaseFunctionResult GetDBFunctionValue(DatabaseFunctionDTO databaseFunction, List<IDataParameter> paramList)
        {
            try
            {
                string stringParamList = "";
                foreach (var column in paramList)
                {
                    stringParamList += (stringParamList == "" ? "" : ",") + column.ParameterName;
                }
                if (stringParamList != "")
                    stringParamList = "(" + stringParamList + ")";

                DatabaseFunctionResult result = new DatabaseFunctionResult();
                var dbHelper = ConnectionManager.GetDBHelper(databaseFunction.DatabaseID);

                result.Result = dbHelper.ExecuteScalar("select " + (string.IsNullOrEmpty(databaseFunction.RelatedSchema) ? "" : databaseFunction.RelatedSchema + ".") + databaseFunction.FunctionName + stringParamList, paramList);


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
        private DatabaseFunctionResult GetDBSPValue(DatabaseFunctionDTO databaseFunction, List<IDataParameter> paramList, string outputParameterName)
        {
            try
            {
                DatabaseFunctionResult result = new DatabaseFunctionResult();
                var dbHelper = ConnectionManager.GetDBHelper(databaseFunction.DatabaseID);

                result.Result = dbHelper.ExecuteStoredProcedure((string.IsNullOrEmpty(databaseFunction.RelatedSchema) ? "" : databaseFunction.RelatedSchema + ".") + databaseFunction.FunctionName, paramList, outputParameterName);

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
