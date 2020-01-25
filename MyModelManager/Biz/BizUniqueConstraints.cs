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
    public class BizUniqueConstraints
    {
        public List<UniqueConstraintsDTO> GetUniqueConstraints(int databaseID)
        {
            List<UniqueConstraintsDTO> result = new List<UniqueConstraintsDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
           //     string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
                var listUniqueConstraint = projectContext.UniqueConstraint.Where(x => x.Table.DBSchema.DatabaseInformationID== databaseID);
                foreach (var item in listUniqueConstraint)
                    result.Add(ToUniqueConstraintDTO(item));

            }
            return result;
        }
        public UniqueConstraintsDTO GetUniqueConstraint(int UniqueConstraintsID)
        {
            List<UniqueConstraintsDTO> result = new List<UniqueConstraintsDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var UniqueConstraints = projectContext.UniqueConstraint.First(x => x.ID == UniqueConstraintsID);
                return ToUniqueConstraintDTO(UniqueConstraints);
            }
        }
        private UniqueConstraintsDTO ToUniqueConstraintDTO(UniqueConstraint item)
        {
            UniqueConstraintsDTO result = new UniqueConstraintsDTO();
            result.Name = item.Name;
            result.ID = item.ID;
            result.TableID = item.TableID;
            return result;
        }
        //public void UpdateUniqueConstraints(List<UniqueConstraintsDTO> UniqueConstraints)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        foreach (var item in UniqueConstraints)
        //        {
        //            var dbUniqueConstraints = projectContext.UniqueConstraint.First(x => x.ID == UniqueConstraints.ID);
        //            dbUniqueConstraints.Name = item.Name;
                   
        //            //  dbUniqueConstraints.BatchDataEntry = UniqueConstraints.BatchDataEntry;
        //            //  dbUniqueConstraints.IsAssociative = UniqueConstraints.IsAssociative;

        //            //   dbUniqueConstraints.IsDataReference = UniqueConstraints.IsDataReference;
        //            //   dbUniqueConstraints.IsStructurReferencee = UniqueConstraints.IsStructurReferencee;
        //        }
        //        projectContext.SaveChanges();
        //    }
        //}
    }
  
}
