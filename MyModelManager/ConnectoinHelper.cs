using DataAccess;
using ModelEntites;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDataManagerBusiness
{
    public class ConnectoinHelper
    {
        public static string GetConnectionString(int entityID)
        {
            using (var context = new MyProjectEntities())
            {
                var entity = context.TableDrivedEntity.First(x => x.ID == entityID);

                return entity.Table.DBSchema.DatabaseInformation.ConnectionString;
            }
        }
      
        //public static string GetConnectionString(DR_BaseRequest request)
        //{
        //    if (request.EditRequest != null)
        //    {
        //        if (request.EditRequest.EditPackages.Count > 0)
        //        {
        //            return GetConnectionString(request.EditRequest.EditPackages[0].TargetEntityID);
        //        }
        //    }
        //    else if (request.SearchViewRequest != null)
        //    {
        //        return GetConnectionString(request.SearchViewRequest.EntityID);
        //    }
        //    return "";
        //}

        //public static string GetConnectionString(string catalogName)
        //{
        //    using (var context = new MyProjectEntities())
        //    {
        //        return context.DatabaseInformation.First(x => x.Name == catalogName).ConnectionString;
        //    }

        //}
    }
}
