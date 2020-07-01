using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Data.Common;
using MyGeneralLibrary;

namespace MyConnectionManager
{
    public class SQLHelper : I_DBHelper
    {


        public DbTransaction GetDBTransaction()
        {
            return SQLTransaction;
        }
        public TransactionResult ExecuteTransactionalQueryItems(List<QueryItem> queryItems)
        {
            throw new NotImplementedException();
        }
        SqlTransaction SQLTransaction { set; get; }
        SqlConnection SQLConnection { set; get; }
        public SQLHelper(SqlConnection sqlConnection, bool withTransaction)
        {
            SQLConnection = sqlConnection;
            if (withTransaction)
            {
                if (SQLConnection.State != ConnectionState.Open)
                    SQLConnection.Open();
                SQLTransaction = SQLConnection.BeginTransaction();
            }
        }
        public TestConnectionResult TestConnection()
        {
            TestConnectionResult result = new TestConnectionResult();

            try
            {
                SQLConnection.Open();
                result.Successful = true;
                SQLConnection.Close();
            }
            catch (SqlException ex)
            {
                result.Successful = false;
                result.Message = GeneralExceptionManager.GetExceptionMessage(ex);
            }
            return result;
        }
        //public DataTable ExecuteProcedure(string PROC_NAME, params object[] parameters)
        //{
        //    try
        //    {
        //        if (parameters.Length % 2 != 0)
        //            throw new ArgumentException("Wrong number of parameters sent to procedure. Expected an even number.");
        //        DataTable a = new DataTable();
        //        List<SqlParameter> filters = new List<SqlParameter>();

        //        string query = "EXEC " + PROC_NAME;

        //        bool first = true;
        //        for (int i = 0; i < parameters.Length; i += 2)
        //        {
        //            filters.Add(new SqlParameter(parameters[i] as string, parameters[i + 1]));
        //            query += (first ? " " : ", ") + ((string)parameters[i]);
        //            first = false;
        //        }

        //        a = Query(query, filters);
        //        return a;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataTable ExecuteQuery(string query, List<IDataParameter> parameters)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter da;
                try
                {
                    if (SQLTransaction != null)
                    {
                        command.Transaction = SQLTransaction;
                    }
                    if (SQLConnection.State != ConnectionState.Open)
                        SQLConnection.Open();
                    command.Connection = SQLConnection;
                    command.CommandText = query;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                    }
                    da = new SqlDataAdapter(command);
                    da.Fill(dt);
                }
                finally
                {
                    if (SQLTransaction == null)
                    {
                        if (SQLConnection != null)
                            SQLConnection.Close();
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public int ExecuteNonQuery(string query, List<IDataParameter> parameters)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlCommand command = new SqlCommand();

                try
                {
                    if (SQLTransaction != null)
                    {
                        command.Transaction = SQLTransaction;
                    }
                    if (SQLConnection.State != ConnectionState.Open)
                        SQLConnection.Open();
                    command.Connection = SQLConnection;
                    command.CommandText = query;
                    if (parameters != null)
                        command.Parameters.AddRange(parameters.ToArray());
                    return command.ExecuteNonQuery();

                }
                finally
                {
                    if (SQLTransaction == null)
                    {
                        if (SQLConnection != null)
                            SQLConnection.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object ExecuteScalar(string query, List<IDataParameter> parameters)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlCommand command = new SqlCommand();

                try
                {
                    if (SQLTransaction != null)
                    {
                        command.Transaction = SQLTransaction;
                    }
                    if (SQLConnection.State != ConnectionState.Open)
                        SQLConnection.Open();
                    command.Connection = SQLConnection;
                    command.CommandText = query;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                    }
                    return command.ExecuteScalar();

                }
                finally
                {
                    if (SQLTransaction == null)
                    {
                        if (SQLConnection != null)
                            SQLConnection.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public object ExecuteStoredProcedure(string spName, List<IDataParameter> parameters, string outputParameterName)
        {
            try
            {

                SqlCommand command = new SqlCommand();

                try
                {
                    if (SQLTransaction != null)
                    {
                        command.Transaction = SQLTransaction;
                    }
                    if (SQLConnection.State != ConnectionState.Open)
                        SQLConnection.Open();
                    if (parameters == null)
                        parameters = new List<IDataParameter>();
                    IDataParameter returnParameter = parameters.First(x=>x.ParameterName== outputParameterName);
                    //if (!parameters.Any(x => x.Direction == ParameterDirection.InputOutput ||
                    // x.Direction == ParameterDirection.Output || x.Direction == ParameterDirection.ReturnValue))
                    //{
                    //    returnParameter = new SqlParameter("@ReturnValue", SqlDbType.VarChar, 100);
                    //    returnParameter.Direction = ParameterDirection.ReturnValue;
                    //    parameters.Insert(0, returnParameter);
                    //}
                    //else
                    //{
                    //    returnParameter = parameters.FirstOrDefault(x => x.Direction == ParameterDirection.InputOutput ||
                    //   x.Direction == ParameterDirection.Output );
                    //}
                    command.Connection = SQLConnection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = spName;
                    command.Parameters.AddRange(parameters.ToArray());
                    command.ExecuteNonQuery();
                    return returnParameter.Value;
                }
                finally
                {
                    if (SQLTransaction == null)
                    {
                        if (SQLConnection != null)
                            SQLConnection.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Private Methods

        //private DataTable Query(String consulta, IList<IDataParameter> parametros)
        //{


        //}


        //private int NonQuery(string query, IList<SqlParameter> parametros)
        //{

        //}

        //private object Scalar(string query, List<DbParameter> parametros)
        //{

        //}
        //private object StoredProcedure(string spName, List<SqlParameter> parametros)
        //{

        //}

        public DbConnection GetDBConnection()
        {
            return SQLConnection;
        }






        #endregion
    }

}