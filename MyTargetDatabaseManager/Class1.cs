using ModelEntites;
using MyConnectionManager;
using MyModelManager;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTargetDatabaseManager
{
    public class TargetDatabaseManager
    {
        public static bool InsertFromQueryToTargetDB(int entityID, string targetTableName, string selectQuery)
        {
            BizDatabase bizDatabase = new BizDatabase();
            var database = bizDatabase.GetDatabaseByEntityID(entityID);
            if (database.DBType == enum_DBType.SQLServer)
            {
                return InsertFromQueryToTargetSQLDB(database, targetTableName, selectQuery);
            }
            return false;
        }

        private static bool InsertFromQueryToTargetSQLDB(DatabaseDTO database, string targetTableName, string selectQuery)
        {
            string result = "";
            var dbHelper = ConnectionManager.GetDBHelper(database.ID, false);
            var existQuery = "SELECT count(*) FROM information_schema.tables where table_type='BASE TABLE' and TABLE_Name='" + targetTableName + "'";
            var count = dbHelper.ExecuteScalar(existQuery);
            //using (SqlConnection testConn = new SqlConnection(database.ConnectionString))
            //{
            //    using (SqlCommand command = new SqlCommand(, testConn))
            //    {
            //using (SqlDataReader reader = command.ExecuteReader())
            //{
            bool exists = (int)count > 0;
            if (exists)
            {
                result = "Insert into " + targetTableName + " " + selectQuery;
            }
            else
            {
                result = "Select * into " + targetTableName + " from (" + selectQuery + ") as aaa";
            }
            dbHelper.ExecuteNonQuery(result);
            if (!exists)
                dbHelper.ExecuteNonQuery("SET IDENTITY_INSERT " + targetTableName + " OFF");
            return true;
            //    }

            //}
            //}
        }
    }
}
