

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paper_MetadataManagement
{
    public class TestConnectionResult
    {
        public bool Successful { set; get; }
        public string Message { set; get; }
    }
    public interface I_DBHelper
    {
        DataTable ExecuteProcedure(string PROC_NAME, params object[] parameters);
        DataTable ExecuteQuery(string query, params object[] parameters);

        //برای اپدیت و دیلیت 
        int ExecuteNonQuery(string query, params object[] parameters);


        object ExecuteScalar(string query, params object[] parameters);
        TestConnectionResult TestConnection();
        object ExecuteStoredProcedure(string spName, params object[] parameters);
        //static int NonQuery(string query, IList<SqlParameter> parametros);
        //static object Scalar(string query, List<SqlParameter> parametros);
    }
    public class ConnectionManager
    {//ترنزکشن اضافه شود
     //public static I_DBHelper GetDBHelper(string catalog)
     //{
     //    //کش شود
     //    var databaseDTO = new BizDatabase().GetDatabaseDTO(catalog);
     //    return GetDBHelper(databaseDTO);
     //}

        public static I_DBHelper GetDBHelperByEntityID(int entityID)
        {
            //کش شود
            using (var context = new MyIdeaEntities())
            {
                var database = context.TableDrivedEntity.First(x => x.ID == entityID).Table.DBSchema.DatabaseInformation;
                return GetDBHelper(database);
            }

        }
        public static I_DBHelper GetDBHelper(int dbID)
        {
            using (var context = new MyIdeaEntities())
            {
                var database = context.DatabaseInformation.First(x => x.ID == dbID);
                return GetDBHelper(database);
            }
        }

        public static I_DBHelper GetDBHelper(string dbName)
        {
            using (var context = new MyIdeaEntities())
            {
                var database = context.DatabaseInformation.First(x => x.Name == dbName);
                return GetDBHelper(database);
            }
        }

        private static I_DBHelper GetDBHelper(DatabaseInformation database)
        {
            I_DBHelper dbHelper = System.Runtime.Remoting.Messaging.CallContext.GetData(database.Name) as I_DBHelper;
            if (dbHelper == null)
            {
                var dbProvider = GetDatabaseProvider(database);
                if (dbProvider == DatabaseProvider.SQLServer)
                {
                    var connection = new SqlConnection(database.ConnectionString);
                    dbHelper = new SQLHelper(connection);
                    System.Runtime.Remoting.Messaging.CallContext.SetData(database.Name, dbHelper);
                }
                else if (dbProvider == DatabaseProvider.Oracle)
                {

                }

            }

            return dbHelper;

        }
        //private DbConnection GetConnection(DatabaseProvider dbProvider, DatabaseDTO databaseDTO)
        //{
        //    DbConnection connection = System.Runtime.Remoting.Messaging.CallContext.GetData(databaseDTO.Name) as DbConnection;
        //    if (connection == null)
        //    {
        //        if (dbProvider == DatabaseProvider.SQLServer)
        //        {
        //            connection = new SqlConnection(databaseDTO.ConnectionString);
        //        }
        //        else if (dbProvider == DatabaseProvider.Oracle)
        //        {

        //        }
        //        System.Runtime.Remoting.Messaging.CallContext.SetData(key, connection);
        //    }

        //    return connection;

        //}
        public void SaveChanges()
        {
            //if (this.isContextOwner)
            //this.GetCurrentDataContext().SaveChanges();
        }

        public void Rollback()
        {
            //this.GetCurrentDataContext().Rollback();
        }


        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        private static DatabaseProvider GetDatabaseProvider(DatabaseInformation databaseDTO)
        {
            return DatabaseProvider.SQLServer;
        }
    }
    public enum DatabaseProvider
    {
        SQLServer,
        Oracle
    }


    //public class ConnectionStore
    //{
    //    //public MellatCodeGenDBContext()
    //    //{
    //    //    //New added
    //    //    //connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ModelTestEntities1ForOracle"].ConnectionString;

    //    //    if (this.GetCurrentDataContext() != null)
    //    //        isContextOwner = false;
    //    //    else
    //    //    {


    //    //        SetCurrentTransaction(this.CreateNewTransaction(connectionString));
    //    //        isContextOwner = true;
    //    //    }
    //    //}



    //}
}