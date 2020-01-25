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
        public DataTable ExecuteProcedure(string PROC_NAME, params object[] parameters)
        {
            try
            {
                if (parameters.Length % 2 != 0)
                    throw new ArgumentException("Wrong number of parameters sent to procedure. Expected an even number.");
                DataTable a = new DataTable();
                List<SqlParameter> filters = new List<SqlParameter>();

                string query = "EXEC " + PROC_NAME;

                bool first = true;
                for (int i = 0; i < parameters.Length; i += 2)
                {
                    filters.Add(new SqlParameter(parameters[i] as string, parameters[i + 1]));
                    query += (first ? " " : ", ") + ((string)parameters[i]);
                    first = false;
                }

                a = Query(query, filters);
                return a;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ExecuteQuery(string query, params object[] parameters)
        {
            try
            {
                if (parameters.Length % 2 != 0)
                    throw new ArgumentException("Wrong number of parameters sent to procedure. Expected an even number.");
                DataTable a = new DataTable();
                List<SqlParameter> filters = new List<SqlParameter>();

                for (int i = 0; i < parameters.Length; i += 2)
                    filters.Add(new SqlParameter(parameters[i] as string, parameters[i + 1]));

                a = Query(query, filters);
                return a;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int ExecuteNonQuery(string query, params object[] parameters)
        {
            try
            {
                if (parameters.Length % 2 != 0)
                    throw new ArgumentException("Wrong number of parameters sent to procedure. Expected an even number.");
                List<SqlParameter> filters = new List<SqlParameter>();

                for (int i = 0; i < parameters.Length; i += 2)
                    filters.Add(new SqlParameter(parameters[i] as string, parameters[i + 1]));
                return NonQuery(query, filters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object ExecuteScalar(string query, params object[] parameters)
        {
            try
            {
                if (parameters.Length % 2 != 0)
                    throw new ArgumentException("Wrong number of parameters sent to query. Expected an even number.");
                List<SqlParameter> filters = new List<SqlParameter>();

                for (int i = 0; i < parameters.Length; i += 2)
                    filters.Add(new SqlParameter(parameters[i] as string, parameters[i + 1]));
                return Scalar(query, filters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public object ExecuteStoredProcedure(string spName, params object[] parameters)
        {
            try
            {
                if (parameters.Length % 2 != 0)
                    throw new ArgumentException("Wrong number of parameters sent to query. Expected an even number.");
                List<SqlParameter> filters = new List<SqlParameter>();

                for (int i = 0; i < parameters.Length; i += 2)
                    filters.Add(new SqlParameter(parameters[i] as string, parameters[i + 1]));
                return StoredProcedure(spName, filters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Private Methods

        private DataTable Query(String consulta, IList<SqlParameter> parametros)
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
                    command.CommandText = consulta;
                    if (parametros != null)
                    {
                        command.Parameters.AddRange(parametros.ToArray());
                    }
                    da = new SqlDataAdapter(command);
                    da.Fill(dt);
                }
                finally
                {
                    if (SQLTransaction== null)
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


        private int NonQuery(string query, IList<SqlParameter> parametros)
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
                    command.Parameters.AddRange(parametros.ToArray());
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

        private object Scalar(string query, List<SqlParameter> parametros)
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
                    command.Parameters.AddRange(parametros.ToArray());
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
        private object StoredProcedure(string spName, List<SqlParameter> parametros)
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
                    var returnParameter = new SqlParameter("@ReturnValue", SqlDbType.VarChar, 100);
                    returnParameter.Direction = ParameterDirection.Output;
                    parametros.Insert(0,returnParameter);


                    command.Connection = SQLConnection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = spName;
                    command.Parameters.AddRange(parametros.ToArray());
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

        public DbConnection GetDBConnection()
        {
            return SQLConnection;
        }






        #endregion
    }

}