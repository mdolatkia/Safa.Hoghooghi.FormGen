using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySecurity
{
    public class BizObject
    {
        //public List<ObjectDTO> GetAllRootObjects()
        //{
        //    var context = new MySecurityEntities();
        //    return ToObjectDTOList(context.Objects.Where(x=>x.ParentID==null).ToList());
        //}

        public List<ObjectDTO> GetObjectsByParentID(int? parentID)
        {
            var context = new MySecurityEntities();
            return ToObjectDTOList(context.Objects.Where(x => x.ParentID == parentID).ToList());
        }
        private List<ObjectDTO> ToObjectDTOList(List<Object> dbSet)
        {
            List<ObjectDTO> result = new List<ObjectDTO>();
            foreach (var item in dbSet)
            {
                ObjectDTO ObjectDto = ToObjectDTO(item);

                result.Add(ObjectDto);
            }
            return result;
        }

        private ObjectDTO ToObjectDTO(Object item)
        {
            var result = new ObjectDTO();
            result.ID = item.ID;
            result.ParentID = item.ParentID;
            result.ObjectName = item.ObjectName;
            result.Category = item.Category;
            result.ItemIdentity = item.ItemIdentity;
            result.NeedsExplicitPermission = item.NeedsExplicitPermission==true;
            return result;
        }

        public void SaveObject(ObjectDTO ObjectDto)
        {
            using (var context = new MySecurityEntities())
            {
                Object dbObject = ToObjectDB(ObjectDto, context);

                if (dbObject.ID == 0)
                    context.Objects.Add(dbObject);
                context.SaveChanges();
            }
        }

        private Object ToObjectDB(ObjectDTO ObjectDto, MySecurityEntities context)
        {
            Object dbObject = null;
            if (ObjectDto.ID == 0)
                dbObject = new Object();
            else
                dbObject = context.Objects.First(x => x.ID == ObjectDto.ID);

            dbObject.ObjectName = ObjectDto.ObjectName;
            dbObject.Category = ObjectDto.Category;
            dbObject.ParentID = ObjectDto.ParentID;
            dbObject.ItemIdentity = ObjectDto.ItemIdentity;
            dbObject.NeedsExplicitPermission = ObjectDto.NeedsExplicitPermission;

            return dbObject;
        }



        public void ExtractObjectsFromDB()
        {
            using (var myProjectContext = new MyProjectEntities())
            {
                using (var mySecurityContext = new MySecurityEntities())
                {
                    foreach (var database in myProjectContext.DatabaseInformation)
                    {
                        var dbObject = mySecurityContext.Objects.FirstOrDefault(x => x.ObjectName == database.Name && x.Category == "Database");
                        if (dbObject == null)
                            dbObject = new Object() { ObjectName = database.Name, Category = "Database" };
                        foreach (var schema in myProjectContext.TableDrivedEntity.Where(x => x.Table.Catalog == database.Name).GroupBy(x=>x.Table.RelatedSchema))
                        {
                            var schemaName="";
                            if (string.IsNullOrEmpty(schema.Key))
                                schemaName = "Default Schema";
                            else
                                schemaName = schema.Key;
                            var schemaObject = mySecurityContext.Objects.FirstOrDefault(x => x.Object2.ObjectName == dbObject.ObjectName && x.ObjectName == schemaName && x.Category == "Schema");
                            if (schemaObject == null)
                            {
                                schemaObject = new Object() { ObjectName = schemaName, Category = "Schema" };
                                dbObject.Object1.Add(schemaObject);
                            }
                            foreach (var entity in schema)
                            {
                                var tableObject = mySecurityContext.Objects.FirstOrDefault(x => x.Object2.Object2.ObjectName == dbObject.ObjectName && x.ObjectName == entity.Name && x.Category == "Entity");
                                if (tableObject == null)
                                {
                                    tableObject = new Object() {ItemIdentity=entity.ID,NeedsExplicitPermission=true, ObjectName = entity.Name, Category = "Entity" };
                                    schemaObject.Object1.Add(tableObject);
                                }
                                List<Column> columns = null;
                                if (entity.Column.Any())
                                    columns = entity.Column.ToList();
                                else
                                    columns = entity.Table.Column.ToList();
                                foreach (var column in columns)
                                {
                                    var columnObject = mySecurityContext.Objects.FirstOrDefault(x => x.Object2.Object2.Object2.ObjectName == dbObject.ObjectName && x.Object2.ObjectName == tableObject.ObjectName && x.ObjectName == column.Name && x.Category == "Column");
                                    if (columnObject == null)
                                    {
                                        columnObject = new Object() { ItemIdentity = column.ID, ObjectName = column.Name, Category = "Column" };
                                        tableObject.Object1.Add(columnObject);
                                    }
                                }
                            }
                        }
                        if (dbObject.ID == 0)
                            mySecurityContext.Objects.Add(dbObject);
                    }
                    mySecurityContext.SaveChanges();
                }
            }
        }
    }

  
}
