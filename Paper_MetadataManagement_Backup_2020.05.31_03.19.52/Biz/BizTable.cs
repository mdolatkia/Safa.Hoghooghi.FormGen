
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paper_MetadataManagement
{
    public class BizTable
    {
        public List<TableDTO> GetTables(int databaseID)
        {
            List<TableDTO> result = new List<TableDTO>();
            using (var projectContext = new MyIdeaEntities())
            {
               // string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
                var list = projectContext.Table.Where(x => x.DBSchema.DatabaseInformationID== databaseID).ToList();
                foreach (var item in list)
                {
                    result.Add(ToTableDTO(item));
                }
            }
            return result;
        }
        public List<TableDTO> GetTablesWithManyToOneRelationships(int databaseID)
        {
            List<TableDTO> result = new List<TableDTO>();
            using (var projectContext = new MyIdeaEntities())
            {
                //string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
                var list = projectContext.Table.Where(x => x.DBSchema.DatabaseInformationID == databaseID);
                list = list.Where(x => x.TableDrivedEntity.Any(y => y.Relationship.Count(z => z.RelationshipType != null && z.RelationshipType.ManyToOneRelationshipType != null) > 0));
                foreach (var item in list)
                {
                    result.Add(ToTableDTO(item));
                }
            }
            return result;
        }

        public TableDTO GetTable(int tableID)
        {
            List<TableDTO> result = new List<TableDTO>();
            using (var projectContext = new MyIdeaEntities())
            {
                var table = projectContext.Table.First(x => x.ID == tableID);
                return ToTableDTO(table);
            }
        }
        private TableDTO ToTableDTO(Table item)
        {
            TableDTO result = new TableDTO();
            result.Name = item.Name;
            result.ID = item.ID;
            result.Alias = item.Alias;
            //result.IsInheritanceImplementation = item.IsInheritanceImplementation==true;
            return result;
        }
        public void UpdateTables(List<TableDTO> tables)
        {
            using (var projectContext = new MyIdeaEntities())
            {
                foreach (var table in tables)
                {
                    var dbTable = projectContext.Table.First(x => x.ID == table.ID);
                    dbTable.Alias = table.Alias;
                    //  dbTable.BatchDataEntry = table.BatchDataEntry;
                    //  dbTable.IsAssociative = table.IsAssociative;
                    //dbTable.IsInheritanceImplementation = table.IsInheritanceImplementation;
                    //   dbTable.IsDataReference = table.IsDataReference;
                    //   dbTable.IsStructurReferencee = table.IsStructurReferencee;
                }
                projectContext.SaveChanges();
            }
        }
    }
  
}
