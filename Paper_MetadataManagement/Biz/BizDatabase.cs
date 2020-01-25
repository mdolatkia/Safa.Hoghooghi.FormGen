
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paper_MetadataManagement
{
    public class BizDatabase
    {
        public DatabaseDTO GetDatabaseByEntityID(int entityID)
        {
            using (var context = new MyIdeaEntities())
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

        public DatabaseDTO GetDatabase(int databaseID)
        {
            using (var context = new MyIdeaEntities())
            {
                var db = context.DatabaseInformation.First(x => x.ID == databaseID);
                return ToDatabaseDTO(db);

            }
        }
        public DatabaseDTO GetDatabase(string dbName)
        {
            using (var context = new MyIdeaEntities())
            {
                var db = context.DatabaseInformation.First(x => x.Name == dbName);
                return ToDatabaseDTO(db);

            }
        }
        //public List<DbServerDTO> GetDBServers()
        //{
        //    List<DbServerDTO> result = new List<DbServerDTO>();
        //    using (var context = new MyIdeaEntities())
        //    {
        //        var servers = context.DBServer;
        //        foreach (var item in servers)
        //        {
        //            result.Add(ToDBServerDTO(item));
        //        }
        //        return result;
        //    }
        //}
        //public DbServerDTO GetDBServer(int dbServerID)
        //{
        //    using (var context = new MyIdeaEntities())
        //    {
        //        var db = context.DBServer.First(x => x.ID == dbServerID);
        //        return ToDBServerDTO(db);
        //    }
        //}

        //public int SaveServer(DbServerDTO message)
        //{
        //    using (var projectContext = new MyIdeaEntities())
        //    {
        //        var dbServer = projectContext.DBServer.FirstOrDefault(x => x.ID == message.ID);
        //        if (dbServer == null)
        //            dbServer = new DBServer();
        //        dbServer.Name = message.Name;
        //        dbServer.Title = message.Title;
        //        dbServer.IPAddress = message.IPAddress;
        //        if (dbServer.ID == 0)
        //            projectContext.DBServer.Add(dbServer);
        //        projectContext.SaveChanges();
        //        return dbServer.ID;
        //    }
        //}


        //private DbServerDTO ToDBServerDTO(DBServer dbServer)
        //{
        //    DbServerDTO result = new DbServerDTO();
        //    result.ID = dbServer.ID;
        //    result.Name = dbServer.Name;
        //    result.Title = dbServer.Title;
        //    result.IPAddress = dbServer.IPAddress;

        //    foreach (var linked in dbServer.LinkedServer)
        //    {
        //        result.LinkedServers.Add(ToLinkedServer(linked));
        //    }
        //    return result;
        //}





        //public DatabaseDTO GetDatabaseDTO(string catalog)
        //{
        //    using (var context = new MyIdeaEntities())
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
            using (var projectContext = new MyIdeaEntities())
            {
                var list = projectContext.DatabaseInformation;
                foreach (var item in list)
                {
                    result.Add(ToDatabaseDTO(item));
                }
            }
            return result;
        }

        private DatabaseDTO ToDatabaseDTO(DatabaseInformation item)
        {
            DatabaseDTO result = new DatabaseDTO();
            result.ID = item.ID;
            result.Name = item.Name;
            result.Title = item.Title;
            result.DBServerID = item.DBServerID;
            result.DBServerName = item.DBServer.Name;
            result.ConnectionString = item.ConnectionString;
            return result;
        }

     
        
    }

}
