

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paper_MetadataManagement
{
    public class GeneralHelper
    {
        public static string GetCatalogName(string serverName, string databaseName)
        {
            return serverName + "\\" + databaseName;
        }
        public static Tuple<string, string> GetServerNameDatabaseName(string catalog)
        {
            return new Tuple<string, string>(catalog.Split('\\')[0], catalog.Split('\\')[1]);
        }
        public static List<T> CreateListFromSingleObject<T>(T item)
        {
            List<T> list = new List<T>();
            list.Add(item);
            return list;
        }


        public static string GetSQLConnectionString(string serverName, string dbName, string userName, string password)
        {
            //Server=.;Database=ccc;User Id=sa;Password=1;MultipleActiveResultSets=True;
            var str = "Server={0};Database={1};User Id={2};Password={3};MultipleActiveResultSets=True;";
            return string.Format(str, serverName, dbName, userName, password);
        }
        //public static string GetSQLConnectionString(int databaseID)
        //{
        //    //Server=.;Database=ccc;User Id=sa;Password=1;MultipleActiveResultSets=True;
        //    //var str = "Server={0};Database={1};User Id={2};Password={3};MultipleActiveResultSets=True;";
        //    //return string.Format(str, serverName, dbName, userName, password);
        //}

        //public static string GetPropertyValue(EntityInstanceProperty column)
        //{
        //    if (column.Value != null && column.Value.ToLower() == "<null>")
        //        return "NULL";
        //    else if (column != null && column.Value.ToLower() == "SCOPE_IDENTITY()".ToLower())
        //        return column.Value;
        //    else if (column != null && column.Value.Contains("()"))
        //        return column.Value;
        //    else
        //        return "'" + column.Value + "'";


        //}
        //public static ManyToOne ToManyToOne(RelationshipDTO rItem, ManyToOneRelationshipType manyToOneRelationshipType)
        //{
        //    Mapper.Initialize(cfg => cfg.CreateMap<RelationshipDTO, ManyToOne>());
        //    var result = AutoMapper.Mapper.Map<RelationshipDTO, ManyToOne>(rItem);
        //    result.IsOneSideTransferable = manyToOneRelationshipType.RelationshipType.IsOtherSideTransferable;
        //    result.IsOneSideCreatable = manyToOneRelationshipType.RelationshipType.IsOtherSideCreatable;
        //    result.IsOneSideMadatory = manyToOneRelationshipType.RelationshipType.IsOtherSideMandatory;
        //    result.IsOneSideDirectlyCreatable = manyToOneRelationshipType.RelationshipType.IsOtherSideDirectlyCreatable;

        //    return result;
        //}


    }


}
