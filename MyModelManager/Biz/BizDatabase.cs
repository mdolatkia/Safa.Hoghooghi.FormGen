using DataAccess;
using ModelEntites;
using MyGeneralLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizDatabase
    {
        public DatabaseDTO GetDatabaseByEntityID(int entityID)
        {
            using (var context = new MyProjectEntities())
            {
                var entity = context.TableDrivedEntity.FirstOrDefault(x => x.ID == entityID);
                if (entity != null)
                {
                    var dbInfo = entity.Table.DBSchema.DatabaseInformation;
                    if (dbInfo != null)
                        return ToDatabaseDTO(dbInfo);
                }
                return null;
            }
        }
        public DatabaseDTO GetDatabaseByTableID(int tableID)
        {
            using (var context = new MyProjectEntities())
            {
                var table = context.Table.FirstOrDefault(x => x.ID == tableID);
                if (table != null)
                {
                    var dbInfo = table.DBSchema.DatabaseInformation;
                    if (dbInfo != null)
                        return ToDatabaseDTO(dbInfo);
                }
                return null;
            }
        }




        public int GetDatabaseIDByEntityID(int entityID)
        {
            using (var context = new MyProjectEntities())
            {
                var entity = context.TableDrivedEntity.FirstOrDefault(x => x.ID == entityID);
                if (entity != null)
                {
                    return entity.Table.DBSchema.DatabaseInformationID;
                }
            }
            return 0;
        }

        public bool SaveDatabaseSetting(int databaseID, DatabaseSettingDTO databaseSetting)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbDatabase = projectContext.DatabaseInformation.FirstOrDefault(x => x.ID == databaseID);
                if (dbDatabase.DatabaseUISetting == null)
                    dbDatabase.DatabaseUISetting = new DatabaseUISetting();

                dbDatabase.DatabaseUISetting.FlowDirectionLTR = databaseSetting.FlowDirectionLTR;
              dbDatabase.DatabaseUISetting.ShowMiladiDateInUI = databaseSetting.ShowMiladiDateInUI;
                dbDatabase.DatabaseUISetting.StringDateColumnIsMiladi = databaseSetting.StringDateColumnIsMiladi;

                projectContext.SaveChanges();
                return true;
            }
        }

        public DatabaseDTO GetDatabase(int databaseID, bool withSetting = false)
        {
            using (var context = new MyProjectEntities())
            {
                var db = context.DatabaseInformation.First(x => x.ID == databaseID);
                return ToDatabaseDTO(db, withSetting);

            }
        }

        public List<DbServerDTO> GetDBServers()
        {
            List<DbServerDTO> result = new List<DbServerDTO>();
            using (var context = new MyProjectEntities())
            {
                var servers = context.DBServer;
                foreach (var item in servers)
                {
                    result.Add(ToDBServerDTO(item));
                }
                return result;
            }
        }



        public DbServerDTO GetDBServer(int dbServerID)
        {
            using (var context = new MyProjectEntities())
            {
                var db = context.DBServer.First(x => x.ID == dbServerID);
                return ToDBServerDTO(db);
            }
        }
        public int SaveLinkedServer(LinkedServerDTO message)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbServer = projectContext.LinkedServer.FirstOrDefault(x => x.ID == message.ID);
                if (dbServer == null)
                    dbServer = new LinkedServer();
                dbServer.SourceDBServerID = message.SourceDBServerID;
                dbServer.TargetDBServerID = message.TargetDBServerID;
                dbServer.Name = message.Name;

                if (dbServer.ID == 0)
                    projectContext.LinkedServer.Add(dbServer);
                projectContext.SaveChanges();
                return dbServer.ID;
            }
        }

        public bool LinkedServerExists(int firstSideServerID, int secondSideServerId)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                return projectContext.LinkedServer.Any(x => x.SourceDBServerID == firstSideServerID && x.TargetDBServerID == secondSideServerId);
            }
        }
        public int SaveServer(DbServerDTO message)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbServer = projectContext.DBServer.FirstOrDefault(x => x.ID == message.ID);
                if (dbServer == null)
                    dbServer = new DBServer();
                dbServer.Name = message.Name;
                dbServer.Title = message.Title;
                dbServer.IPAddress = message.IPAddress;
                if (dbServer.ID == 0)
                    projectContext.DBServer.Add(dbServer);
                projectContext.SaveChanges();
                return dbServer.ID;
            }
        }
        public LinkedServerDTO GetLinkedServer(int ID)
        {
            using (var context = new MyProjectEntities())
            {
                var db = context.LinkedServer.First(x => x.ID == ID);
                return ToLinkedServer(db);
            }
        }
        public List<LinkedServerDTO> GetLinkedServerBySourceServerID(int iD)
        {
            List<LinkedServerDTO> result = new List<LinkedServerDTO>();
            using (var context = new MyProjectEntities())
            {
                foreach (var db in context.LinkedServer.Where(x => x.SourceDBServerID == iD))
                {
                    result.Add(ToLinkedServer(db));
                }
            }
            return result;
        }
        public LinkedServerDTO GetLinkedServer(int firstSideServerID, int secondSideServerID)
        {
            using (var context = new MyProjectEntities())
            {
                var db = context.LinkedServer.FirstOrDefault(x => x.SourceDBServerID == firstSideServerID && x.TargetDBServerID == secondSideServerID);
                if (db != null)
                    return ToLinkedServer(db);
                else
                    return null;
            }
        }
        private DbServerDTO ToDBServerDTO(DBServer dbServer)
        {
            DbServerDTO result = new DbServerDTO();
            result.ID = dbServer.ID;
            result.Name = dbServer.Name;
            result.Title = dbServer.Title;
            result.IPAddress = dbServer.IPAddress;

            foreach (var linked in dbServer.LinkedServer)
            {
                result.LinkedServers.Add(ToLinkedServer(linked));
            }
            return result;
        }



        private LinkedServerDTO ToLinkedServer(LinkedServer linked)
        {

            LinkedServerDTO result = new LinkedServerDTO();
            result.ID = linked.ID;
            result.SourceDBServerID = linked.SourceDBServerID;
            result.SourceDBServerName = linked.DBServer.Name;
            result.TargetDBServerID = linked.TargetDBServerID;
            result.TargetDBServerName = linked.DBServer1.Name;
            result.Name = linked.Name;
            return result;

        }

        //public DatabaseDTO GetDatabaseDTO(string catalog)
        //{
        //    using (var context = new MyProjectEntities())
        //    {

        //            var dbInfo = context.DatabaseInformation.FirstOrDefault(x => x.Name == catalog);
        //            if (dbInfo != null)
        //                return ToDatabaseDTO(dbInfo);

        //        return null;
        //    }
        //}
        public List<DatabaseDTO> GetDatabases()
        {
            List<DatabaseDTO> result = new List<DatabaseDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var list = projectContext.DatabaseInformation;
                foreach (var item in list)
                {
                    result.Add(ToDatabaseDTO(item));
                }
            }
            return result;
        }

        private DatabaseDTO ToDatabaseDTO(DataAccess.DatabaseInformation item, bool withSetting = false)
        {
            DatabaseDTO result = new DatabaseDTO();
            result.ID = item.ID;
            result.Name = item.Name;
            result.Title = item.Title;
            result.DBServerID = item.DBServerID;
            result.DBServerName = item.DBServer.Name;
            result.DBServerTitle = item.DBServer.Title;
            result.DBType = (enum_DBType)Enum.Parse(typeof(enum_DBType), item.DBType, true);
            result.DBHasData = item.DBHasDate;
            result.ConnectionString = item.ConnectionString;
            if (withSetting)
            {
                result.DatabaseSetting = GetDatabaseSetting(item);
            }

            return result;
        }

        private DatabaseSettingDTO GetDatabaseSetting(DatabaseInformation item)
        {
            if (item.DatabaseUISetting != null)
            {
                DatabaseSettingDTO result = new DatabaseSettingDTO();
                result.FlowDirectionLTR = item.DatabaseUISetting.FlowDirectionLTR;
                result.ShowMiladiDateInUI = item.DatabaseUISetting.ShowMiladiDateInUI;
                result.StringDateColumnIsMiladi = item.DatabaseUISetting.StringDateColumnIsMiladi;
                return result;
            }
            else
                return null;
        }

        public int SaveDatabase(DatabaseDTO message)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbDatabase = projectContext.DatabaseInformation.FirstOrDefault(x => x.ID == message.ID);
                if (dbDatabase == null)
                {
                    dbDatabase = new DatabaseInformation();
                    dbDatabase.SecurityObject = new SecurityObject();
                    dbDatabase.SecurityObject.Type = (int)DatabaseObjectCategory.Database;
                }
                dbDatabase.DBServerID = message.DBServerID;
                dbDatabase.Name = message.Name;
                dbDatabase.Title = message.Title;
                dbDatabase.DBHasDate = message.DBHasData;
                dbDatabase.ConnectionString = message.ConnectionString;
                dbDatabase.DBType = message.DBType.ToString();
                if (dbDatabase.ID == 0)
                    projectContext.DatabaseInformation.Add(dbDatabase);
                projectContext.SaveChanges();
                return dbDatabase.ID;
            }
        }
        //public bool TestConnection(int databaseID)
        //{
        //    var 
        //    //using (var context = new MyProjectEntities())
        //    //{
        //    // var database= context.DatabaseInformation.First(x => x.ID == databaseID);

        //    //    using (SqlConnection testConn = new SqlConnection(Database.ConnectionString))
        //    //    {
        //    //        try
        //    //        {
        //    //            testConn.Open();
        //    //            return true;
        //    //        }
        //    //        catch (SqlException)
        //    //        {
        //    //            return false;
        //    //        }
        //    //    }
        //    //}
        //}
        //public List<SchemaDTO> GetSchemaDTO(string databaseName)
        //{
        //    List<SchemaDTO> result = new List<SchemaDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        foreach (var schema in projectContext.TableDrivedEntity.Where(x => x.Table.Catalog == databaseName).GroupBy(x => x.Table.RelatedSchema))
        //        {
        //            result.Add(ToSchemaDTO(schema));
        //        }
        //    }
        //    return result;
        //}

        //private SchemaDTO ToSchemaDTO(IGrouping<string, DataAccess.TableDrivedEntity> schema)
        //{
        //    SchemaDTO result = new SchemaDTO();
        //    result.Name = schema.Key;
        //    return result;
        //}



        //public List<ObjectDTO> GetDatabaseObjectTree(string parentCategory, string parentIdentity)
        //{
        //    List<ObjectDTO> result = new List<ObjectDTO>();
        //    using (var myProjectContext = new MyProjectEntities())
        //    {
        //        if (string.IsNullOrEmpty(parentIdentity))
        //        {
        //            foreach (var database in myProjectContext.DatabaseInformation)
        //            {
        //                ObjectDTO dbObject = ToObjectDTO("Database", database.Name, database.Name);
        //                result.Add(dbObject);
        //            }
        //        }
        //        else if (parentCategory == "Database")
        //        {
        //            foreach (var schema in myProjectContext.TableDrivedEntity.Where(x => x.Table.Catalog == parentIdentity).GroupBy(x => x.Table.RelatedSchema))
        //            {
        //                var schemaName = "";
        //                if (string.IsNullOrEmpty(schema.Key))
        //                    schemaName = "Default Schema";
        //                else
        //                    schemaName = schema.Key;

        //                ObjectDTO schemaObject = ToObjectDTO("Schema", GetSchemaObjectName(parentIdentity, schema.Key), schemaName);
        //                result.Add(schemaObject);
        //            }
        //        }
        //        else if (parentCategory == "Schema")
        //        {
        //            var res = GetDBNameSchemaName(parentIdentity);
        //            foreach (var entity in myProjectContext.TableDrivedEntity.Where(x => x.Table.Catalog == res.Item1 && x.Table.RelatedSchema == res.Item2))
        //            {
        //                ObjectDTO entityObject = ToObjectDTO("Entity", entity.ID.ToString(), string.IsNullOrEmpty(entity.Alias) ? entity.Name : entity.Alias);
        //                result.Add(entityObject);
        //            }
        //        }
        //        else if (parentCategory == "Entity")
        //        {
        //            int id = Convert.ToInt32(parentIdentity);
        //            var dbEntity = myProjectContext.TableDrivedEntity.First(x => x.ID == id);
        //            List<Column> columns = null;
        //            if (dbEntity.Column.Any())
        //                columns = dbEntity.Column.ToList();
        //            else
        //                columns = dbEntity.Table.Column.ToList();
        //            foreach (var column in columns)
        //            {
        //                ObjectDTO columnObject = ToObjectDTO("Column", column.ID.ToString(), string.IsNullOrEmpty(column.Alias) ? column.Name : column.Alias);
        //                result.Add(columnObject);
        //            }
        //        }
        //    }
        //    return result;
        //}

        //private Tuple<string, string> GetDBNameSchemaName(string objectIdentity)
        //{
        //    return new Tuple<string, string>(objectIdentity.Split('>')[0], objectIdentity.Split('>')[1]);
        //}
        //private ObjectDTO ToObjectDTO(string objectCategory, string objectIdentity, string title)
        //{
        //    ObjectDTO result = new ObjectDTO();
        //    result.ObjectCategory = objectCategory;
        //    result.ObjectIdentity = objectIdentity;
        //    result.Title = title;
        //    result.NeedsExplicitPermission = (objectCategory == "Entity");
        //    return result;
        //}
        //private string GetSchemaObjectName(string dbName, string schemaName)
        //{
        //    return dbName + ">" + schemaName;
        //}

        //public ObjectDTO GetParentObject(string objectCategory, string objectIdentity)
        //{
        //    if (objectCategory == "Database")
        //        return null;
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        ObjectDTO result = null;
        //        if (objectCategory == "Schema")
        //        {
        //            var dbName = objectIdentity.Split('>')[0];
        //            result = ToObjectDTO("Database", dbName, dbName);
        //        }
        //        else if (objectCategory == "Entity")
        //        {
        //            int id = Convert.ToInt32(objectIdentity);
        //            var entity = projectContext.TableDrivedEntity.First(x => x.ID == id);
        //            result = ToObjectDTO("Schema", GetSchemaObjectName(entity.Table.Catalog, entity.Table.RelatedSchema), entity.Table.RelatedSchema);
        //        }
        //        else if (objectCategory == "Column")
        //        {
        //            //int id = Convert.ToInt32(objectIdentity);
        //            //var column = projectContext.Column.First(x => x.ID == id);
        //            //result = ToObjectDTO("Schema", GetSchemaObjectName(entity.Table.Catalog, entity.Table.RelatedSchema), entity.Table.RelatedSchema);
        //        }
        //        return result;
        //    }
        //}

        ////public List<ObjectDTO> GetChildObjects(string objectIdentity, string objectCategory)
        ////{
        ////    if (objectCategory == "Column")
        ////        return null;
        ////    var result = new List<ObjectDTO>();
        ////    using (var projectContext = new DataAccess.MyProjectEntities())
        ////    {
        ////        if (objectCategory == "Database")
        ////        {
        ////            var 
        ////        }
        ////        if (objectCategory == "Schema")
        ////        {
        ////            var dbName = objectIdentity.Split('>')[0];
        ////            result = new ObjectDTO();
        ////            result.ObjectIdentity = dbName;
        ////            result.Category = "Database";
        ////            result.Title = dbName;
        ////        }
        ////        if (objectCategory == "Schema")
        ////        {
        ////            var dbName = objectIdentity.Split('>')[0];
        ////            result = new ObjectDTO();
        ////            result.ObjectIdentity = dbName;
        ////            result.Category = "Database";
        ////            result.Title = dbName;
        ////        }
        ////        else if (objectCategory == "Entity")
        ////        {
        ////            int id = Convert.ToInt32(objectIdentity);
        ////            var entity = projectContext.TableDrivedEntity.First(x => x.ID == id);
        ////            result = new ObjectDTO();
        ////            result.ObjectIdentity = GetSchemaObjectName(entity.Table.Catalog, entity.Table.RelatedSchema);
        ////            result.Category = "Schema";
        ////            result.Title = entity.Table.RelatedSchema;
        ////        }
        ////        return result;
        ////    }
        ////}


        //public bool GetUpwardCondition(string objectCategory, string objectIdentity)
        //{
        //    return objectCategory == "Entity";
        //}
    }

}
