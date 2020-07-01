using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConnectionManager
{
    public interface I_DBHelper
    {
        DbTransaction GetDBTransaction();
        DbConnection GetDBConnection();
        //DataTable ExecuteProcedure(string PROC_NAME, params object[] parameters);
        DataTable ExecuteQuery(string query, List<IDataParameter> parameters = null);

        //برای اپدیت و دیلیت 
        int ExecuteNonQuery(string query, List<IDataParameter> parameters = null);


        object ExecuteScalar(string query, List<IDataParameter> parameters = null);
        TestConnectionResult TestConnection();
        object ExecuteStoredProcedure(string spName, List<IDataParameter> parameters, string outputParameterName);
        //static int NonQuery(string query, IList<SqlParameter> parametros);
        //static object Scalar(string query, List<SqlParameter> parametros);
    }

    public interface I_TransactionalDBHelper
    {
        TransactionResult ExecuteTransactionalQueryItems(List<QueryItem> queryItems);
    }
    public class TestConnectionResult
    {
        public bool Successful { set; get; }
        public string Message { set; get; }
    }
    public class TransactionResult
    {
        public TransactionResult()
        {
            QueryItems = new List<MyConnectionManager.TransactionQueryResult>();
        }
        public bool Successful { set; get; }
        public string Message { set; get; }
        public List<TransactionQueryResult> QueryItems { set; get; }
    }
    public class TransactionQueryResult
    {
        public TransactionQueryResult(QueryItem queryItem)
        {
            QueryItem = queryItem;
        }
        public QueryItem QueryItem { set; get; }
        public Exception Exception { set; get; }
    }
}
