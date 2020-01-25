using DataAccess;
using ModelEntites;

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProxyLibrary;

namespace MyConnectionManager
{
    public class ConnectionManager
    {

        public static TransactionResult ExecuteTransactionalQueryItems(List<QueryItem> queryItems)
        {

            TransactionResult result = new MyConnectionManager.TransactionResult();
            foreach (var queryItem in queryItems)
            {
                TransactionQueryResult queryResult = new MyConnectionManager.TransactionQueryResult(queryItem);
                result.QueryItems.Add(queryResult);
            }
            List<Tuple<int, I_DBHelper>> dbHelpers = new List<Tuple<int, MyConnectionManager.I_DBHelper>>();
            //بهتره ایدی دیتابیس در انتیت قرار گیرد
            //List<Tuple<int, DatabaseInformation>> entityDatabaseIDs = new List<Tuple<int, DatabaseInformation>>();

            //using (var context = new MyProjectEntities())
            //{
            //    foreach (var queryResult in result.QueryItems)
            //    {
            //        var queryItem = queryResult.QueryItem;
            //        if (!entityDatabaseIDs.Any(x => x.Item1 == queryItem.TargetEntityID))
            //        {
            //            var entityID = 0;
            //            if (queryItem.DataItem != null)
            //                entityID = queryItem.DataItem.TargetEntityID;
            //            else
            //                entityID = queryItem.TargetEntityID;
            //            var database = context.TableDrivedEntity.First(x => x.ID == entityID).Table.DBSchema.DatabaseInformation;
            //            entityDatabaseIDs.Add(new Tuple<int, DatabaseInformation>(entityID, database));
            //        }
            //    }
            //}
            try
            {
                foreach (var queryResult in result.QueryItems)
                {
                    try
                    {
                        var queryItem = queryResult.QueryItem;
                        //var entityID = 0;
                        //if (queryItem.DataItem != null)
                        //    entityID = queryItem.TargetEntity.ID;
                        //else
                        //    entityID = queryItem.TargetEntityID;
                        //var database = entityDatabaseIDs.First(x => x.Item1 == entityID).Item2;
                        I_DBHelper dbHelper = null;
                        if (!dbHelpers.Any(x => x.Item1 == queryItem.TargetEntity.DatabaseID))
                        {
                            dbHelper = GetDBHelper(queryItem.TargetEntity.DatabaseID, true);
                            dbHelpers.Add(new Tuple<int, I_DBHelper>(queryItem.TargetEntity.DatabaseID, dbHelper));
                        }
                        else
                            dbHelper = dbHelpers.First(x => x.Item1 == queryItem.TargetEntity.DatabaseID).Item2;

                        dbHelper.ExecuteNonQuery(queryItem.Query);

                        if (queryItem.DataItem != null && queryItem.DataItem.GetProperties().Any(x => x.IsIdentity))
                        {
                            if (queryItem.DataItem.IsNewItem)
                            {
                                var identity = dbHelper.ExecuteScalar("select scope_identity()");
                                foreach (var identityProperty in queryItem.DataItem.GetProperties().Where(x => x.IsIdentity))
                                {

                                    //ایونت باید اینجا ریز بشه
                                    queryItem.DataItem.GetProperty(identityProperty.ColumnID).Value = identity.ToString();
                                }
                                //if (queryItem.SetIdentities.Any())
                                //{
                                //    foreach (var identityItem in queryItem.SetIdentities)
                                //    {
                                //        identityItem.TargetQueryItem.DataItem.GetProperty(identityItem.TargetColumnID).Value = identity.ToString();
                                //        identityItem.TargetQueryItem.Query = string.Format(identityItem.TargetQueryItem.Query, identity.ToString());
                                //    }
                                //}
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        queryResult.Exception = ex;
                        throw ex;
                    }
                }
                foreach (var dbHelper in dbHelpers)
                {
                    dbHelper.Item2.GetDBTransaction().Commit();
                }
                result.Successful = true;
            }
            catch (Exception ex)
            {
                foreach (var dbHelper in dbHelpers)
                {
                    dbHelper.Item2.GetDBTransaction().Rollback();
                }
                result.Message = ex.Message;
                result.Successful = false;
            }
            finally
            {
                foreach (var dbHelper in dbHelpers)
                {
                    dbHelper.Item2.GetDBConnection().Close();
                }
            }
            return result;
        }
        //public static DbTransaction GetTransactionalDBHelper(int dbID)
        //{
        //    using (var context = new MyProjectEntities())
        //    {
        //        return new TransactionalDBHelper();
        //    }
        //}
       
        public static I_DBHelper GetDBHelperByEntityID(int entityID, bool withTransaction = false)
        {
            //کش شود
            using (var context = new MyProjectEntities())
            {
                var database = context.TableDrivedEntity.First(x => x.ID == entityID).Table.DBSchema.DatabaseInformation;
                return GetDBHelper(database, withTransaction);
            }

        }
        public static I_DBHelper GetDBHelper(int dbID, bool withTransaction = false)
        {
            using (var context = new MyProjectEntities())
            {
                var database = context.DatabaseInformation.First(x => x.ID == dbID);
                return GetDBHelper(database, withTransaction);
            }
        }

        private static I_DBHelper GetDBHelper(DatabaseInformation database, bool withTransaction)
        {
            I_DBHelper dbHelper = null;//= System.Runtime.Remoting.Messaging.CallContext.GetData(database.Name) as I_DBHelper;
            if (dbHelper == null)
            {
                var dbProvider = GetDatabaseProvider(database);
                if (dbProvider == DatabaseProvider.SQLServer)
                {
                    var connection = new SqlConnection(database.ConnectionString);
                    dbHelper = new SQLHelper(connection, withTransaction);
                    //System.Runtime.Remoting.Messaging.CallContext.SetData(database.Name, dbHelper);
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
        //public void SaveChanges()
        //{
        //    //if (this.isContextOwner)
        //    //this.GetCurrentDataContext().SaveChanges();
        //}

        //public void Rollback()
        //{
        //    //this.GetCurrentDataContext().Rollback();
        //}


        //public void Dispose()
        //{
        //    //throw new NotImplementedException();
        //}

        private static DatabaseProvider GetDatabaseProvider(DatabaseInformation databaseDTO)
        {
            return DatabaseProvider.SQLServer;
        }
        private static DatabaseProvider GetDatabaseProvider(DatabaseDTO databaseDTO)
        {
            return DatabaseProvider.SQLServer;
        }
    }
    public enum DatabaseProvider
    {
        SQLServer,
        Oracle
    }
    public class QueryItem
    {
        //public QueryItem()
        //{
        //    ChildQueries = new List<MyConnectionManager.QueryItem>();
        //    SetIdentities = new List<MyConnectionManager.SetIdentity>();
        //    FKSources = new List<MyConnectionManager.FKToPK>();
        //    //TargetEntityID = targetEntityID;
        //    //Query = query;
        //}
        public QueryItem(TableDrivedEntityDTO targetEntity, Enum_QueryItemType queryType, List<EntityInstanceProperty> editingProperties, DP_DataRepository dataItem)
        {
            DataItem = dataItem;
            QueryType = queryType;
            //ChildQueries = new List<MyConnectionManager.QueryItem>();
         //   SetIdentities = new List<MyConnectionManager.SetIdentity>();
         //   FKSources = new List<MyConnectionManager.FKToPK>();
            TargetEntity = targetEntity;
            EditingProperties = editingProperties;
        }
        public Enum_QueryItemType QueryType { get; set; }
        //public List<FKToPK> FKSources { get; set; }
        public string Query { set; get; }

        public List<EntityInstanceProperty> EditingProperties { set; get; }
        //public bool ShouldInserIdentityInChilds { set; get; }
        //public string IdentityParam { set; get; }
        public TableDrivedEntityDTO TargetEntity { set; get; }

        //public List<QueryItem> ChildQueries { set; get; }
        public DP_DataRepository DataItem { get; set; }
        //public bool ShouldWriteSimpleColumnsQuery { get; set; }
      //  public List<SetIdentity> SetIdentities { set; get; }
    }
    public enum Enum_QueryItemType
    {
        Insert,
        Update,
        Delete
    }
    public class SetIdentity
    {
        public QueryItem TargetQueryItem { set; get; }
        public int TargetColumnID { set; get; }
    }
    public class FKToPK
    {
        public FKToPK(RelationshipDTO relationship, QueryItem pkQueryItem, int pkIdentityColumnID, bool isSelfTable)
        {
            Relationship = relationship;
            PKQueryItem = pkQueryItem;
            PKIdentityColumnID = pkIdentityColumnID;
            IsSelfTable = isSelfTable;
        }
        public bool IsSelfTable { set; get; }
        public RelationshipDTO Relationship { set; get; }
        public QueryItem PKQueryItem { set; get; }
        public int PKIdentityColumnID { set; get; }
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