using DataAccess;
using ModelEntites;
using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizSecurityObjects
    {
        //public List<SecurityObjectDTO> GetSecurityObjectsOfEntity(int entityID)
        //{
        //    List<SecurityObjectDTO> result = new List<SecurityObjectDTO>();
        //    var context = new MyProjectEntities();
        //    var entity = context.TableDrivedEntity.First(x => x.ID == entityID);
        //    result.Add(ToSecurityObjectDTO(entity.SecurityObject));
        //    BizColumn bizColumn = new BizColumn();

        //    ICollection<Column> columns = bizColumn.GetColumns(entity);
        //    foreach (var column in columns)
        //    {
        //        result.Add(ToSecurityObjectDTO(column.SecurityObject));
        //    }
        //    foreach (var rel in entity.Relationship)
        //    {
        //        result.Add(ToSecurityObjectDTO(rel.SecurityObject));
        //    }
        //    foreach (var command in entity.EntityCommand)
        //    {
        //        result.Add(ToSecurityObjectDTO(command.SecurityObject));
        //    }
        //    foreach (var report in entity.EntityReport)
        //    {
        //        result.Add(ToSecurityObjectDTO(report.SecurityObject));
        //    }
        //    return result;
        //}
        public SecurityObjectDTO GetSecurityObject(int ID)
        {

            var context = new MyProjectEntities();
            var item = context.SecurityObject.First(x => x.ID == ID);

            return ToSecurityObjectDTO(item);

        }
        private SecurityObjectDTO ToSecurityObjectDTO(SecurityObject item)
        {
            SecurityObjectDTO result = new SecurityObjectDTO();
            result.ID = item.ID;
            result.Type = (DatabaseObjectCategory)item.Type;
            if (result.Type == DatabaseObjectCategory.Database)
            {
                result.Name = item.DatabaseInformation.Name;
            }
            else if (result.Type == DatabaseObjectCategory.Schema)
            {
                result.Name = item.DBSchema.Name;
            }
            else if (result.Type == DatabaseObjectCategory.Relationship)
            {
                result.Name = item.Relationship.Name;
            }
            else if (result.Type == DatabaseObjectCategory.Entity)
            {
                result.Name = item.TableDrivedEntity.Name;
            }
            else if (result.Type == DatabaseObjectCategory.Column)
            {
                result.Name = item.Column.Name;
            }
            else if (result.Type == DatabaseObjectCategory.Report)
            {
                result.Name = item.EntityReport.Title;
            }
            else if (result.Type == DatabaseObjectCategory.Command)
            {
                result.Name = item.EntityCommand.Title;
            }
            return result;
        }



        public RoleTypeDTO ToRoleTypeDTO(RoleType item)
        {
            RoleTypeDTO result = new RoleTypeDTO();
            result.ID = item.ID;
            result.Name = item.Name;
            result.IsSuperAdmin = item.IsSuperAdmin == true;
            return result;
        }





    }



}
