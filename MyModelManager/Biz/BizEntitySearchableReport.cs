using DataAccess;
using ModelEntites;
using MyCacheManager;
using MyGeneralLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProxyLibrary;


namespace MyModelManager
{
    public class BizEntitySearchableReport
    {

        SecurityHelper securityHelper = new SecurityHelper();
        BizEntityReport bizEntityReport = new BizEntityReport();

        //public EntitySearchableReportDTO GetEntitySearchableReport(DR_Requester requester, int EntityReportID, bool withDetails)
        //{

        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var dbReport = projectContext.EntitySearchableReport.First(x => x.ID == EntityReportID);
        //        if (bizEntityReport.DataIsAccessable(requester, dbReport.EntityReport))
        //        {
        //            return ToEntitySearchableReportDTO(dbReport, withDetails);
        //        }
        //        else
        //            return null;
        //    }

        //}
        public List<EntitySearchableReportDTO> GetEntityReportsOfRelationshipTail(DR_Requester requester, int entityRelationshipTailID)
        {
            List<EntitySearchableReportDTO> result = new List<EntitySearchableReportDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
                var relationshipTail = bizEntityRelationshipTail.GetEntityRelationshipTail(requester, entityRelationshipTailID);
                var listEntityReport = projectContext.EntitySearchableReport.Where(x => x.EntityReport.TableDrivedEntityID == relationshipTail.TargetEntityID);
                foreach (var item in listEntityReport)
                {
                    var nItem = new EntitySearchableReportDTO();
                    ToEntitySearchableReportDTO(item, nItem, false);
                    result.Add(nItem);
                }
            }
            return result;
        }

        BizSearchRepository bizSearchRepository = new BizSearchRepository();
        //private EntitySearchableReportDTO ToEntitySearchableReportDTO(EntitySearchableReport item, bool withDetails)
        //{
        //    EntitySearchableReportDTO result = new EntitySearchableReportDTO();
        //    bizEntityReport.ToEntityReportDTO(item.EntityReport, result);
        //    item.SearchRepositoryID = item.SearchRepositoryID ?? 0;
        //    if (item.SearchRepository != null)
        //        result.SearchRepository = bizSearchRepository.ToSearchRepositoryDTO(item.SearchRepository);


        //    return result;
        //}
        internal void ToEntitySearchableReportDTO(EntitySearchableReport entitySearchableReport, EntitySearchableReportDTO entitySearchableReportDTO, bool withDetails)
        {
            bizEntityReport.ToEntityReportDTO(entitySearchableReport.EntityReport, entitySearchableReportDTO);
            entitySearchableReport.SearchRepositoryID = entitySearchableReport.SearchRepositoryID ?? 0;
            if (entitySearchableReport.SearchRepository != null)
                entitySearchableReportDTO.SearchRepository = bizSearchRepository.ToSearchRepositoryDTO(entitySearchableReport.SearchRepository);
            entitySearchableReportDTO.SearchableReportType = (SearchableReportType)entitySearchableReport.SearchableReportType;
        }

        internal EntitySearchableReport ToNewEntitySearchableReport(EntitySearchableReportDTO message)
        {
            var result = new EntitySearchableReport();
            result.EntityReport = bizEntityReport.ToNewEntityReport(message);
            result.SearchRepositoryID = message.SearchRepositoryID == 0 ? (Int32?)null : message.SearchRepositoryID;
            result.SearchableReportType = (short)message.SearchableReportType;
            return result;
        }
        internal void ToUpdateEntitySearchableReport(EntitySearchableReport entitySearchableReport, EntitySearchableReportDTO message)
        {
            bizEntityReport.ToUpdateEntityReport(entitySearchableReport.EntityReport, message);
            entitySearchableReport.SearchRepositoryID = message.SearchRepositoryID == 0 ? (Int32?)null : message.SearchRepositoryID;
        }
    }

}
