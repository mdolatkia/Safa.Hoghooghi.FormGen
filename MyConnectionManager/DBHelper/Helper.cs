
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConnectionManager
{
    public class MyDataHelper
    {
        //internal static string GetConnectionString(DR_Request request)
        //{
        //    if (request.EditRequest != null)
        //    {
        //        if (request.EditRequest.EditPackages.Count > 0)
        //        {
        //            using (var context = new MyProjectEntities())
        //            {
        //                var entityID = request.EditRequest.EditPackages[0].TargetEntityID;
        //                var entity = context.TableDrivedEntity.First(x => x.ID == entityID);

        //                return context.DatabaseInformation.First(x => x.Name == entity.Table.Catalog).ConnectionString;
        //            }
        //        }
        //    }
        //    else if (request.SearchViewRequest != null)
        //    {
        //        if (request.SearchViewRequest.SearchPackages.Count > 0)
        //        {
        //            using (var context = new MyProjectEntities())
        //            {
        //                var entityID = request.SearchViewRequest.SearchPackages[0].TargetEntityID;
        //                var entity = context.TableDrivedEntity.First(x => x.ID == entityID);

        //                return context.DatabaseInformation.First(x => x.Name == entity.Table.Catalog).ConnectionString;
        //            }
        //        }
        //    }
        //    return "";
        //}

        public static bool DataItemPrimaryKeysHaveValue(DP_DataRepository dataItem)
        {
            var keyProperties = dataItem.KeyProperties;
            return keyProperties.Any() && keyProperties.All(x => x.Value != null && !string.IsNullOrEmpty(x.Value.ToString()));
        }

        public static bool DataItemNonPrimaryKeysHaveValues(DP_DataRepository dataItem)
        {
            return dataItem.GetProperties().Where(x => !x.IsKey).Any(x => x.Value != null && !string.IsNullOrEmpty(x.Value.ToString()));
        }

        //public static object GetPropertyValue(object value, Type dotNetType)
        //{//درست شود
        //    try
        //    {
        //        if (value == null)
        //            return null;
        //        else if (dotNetType == null)
        //            return value;
        //        else
        //            return Convert.ChangeType(value, dotNetType);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public static List<DP_DataRepository> GetDataItems(int entityID, List<EntityInstanceProperty> properties)
        //{
        //    SearchRequestProcessor searchProcessor = new MyConnectionManager.SearchRequestProcessor();
        //    return searchProcessor.GetDataItems(entityID, properties);
        //}
        //public static object GetPropertyValue(string value, Type dotNetType)
        //{
        //    if (value == null || value == "<Null>")
        //        return null;
        //    else
        //        return Convert.ChangeType(value, dotNetType);
        //}
    }
}
