
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGeneralLibrary
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
        //    if (column.Value == null )
        //        return "NULL";
        //    else if (column != null && column.Value.ToLower() == "SCOPE_IDENTITY()".ToLower())
        //        return column.Value;
        //    else if (column != null && column.Value.Contains("()"))
        //        return column.Value;
        //    else
        //        return "'" + column.Value + "'";


        //}


        public static DateTime GetMiladiDateFromShamsi(string date)
        {
            System.Globalization.PersianCalendar P = new System.Globalization.PersianCalendar();

            string shdate = date;
            var t = shdate.Split('/');
            int Y = Convert.ToInt32(t[0]);
            int M = Convert.ToInt32(t[1]);
            int D = (t[2].Length > 2) ? Convert.ToInt32(t[2].Substring(0, 2)) : Convert.ToInt32(t[2]);
            DateTime MDate = P.ToDateTime(Y, M, D, 0, 0, 0, 0);
            return MDate;
        }

        public static string GetShamsiDate(DateTime date)
        {
            System.Globalization.PersianCalendar a = new System.Globalization.PersianCalendar();
            // DateTime today = DateTime.Today;
            string year = a.GetYear(date).ToString();
            //if (year.Length == 4)
            //    year = year.Remove(0, 2);
            string month = a.GetMonth(date).ToString();
            if (month.Length == 1)
                month = "0" + month;
            string day = a.GetDayOfMonth(date).ToString();
            if (day.Length == 1)
                day = "0" + day;
            return year + "/" + month + "/" + day;
        }
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
