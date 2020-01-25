using DataAccess;
using ModelEntites;
using MyCacheManager;
using MyGeneralLibrary;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizEntityDirectReport
    {
        BizEntityReport bizEntityReport = new MyModelManager.BizEntityReport();
        public List<EntityDirectReportDTO> GetEntityDirectReports(DR_Requester requester, int entityID)
        {
            List<EntityDirectReportDTO> result = new List<EntityDirectReportDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var list = projectContext.EntityDirectlReport.Where(x => x.EntityReport.TableDrivedEntityID == entityID);
                foreach (var item in list)
                {
                    if (bizEntityReport.DataIsAccessable(requester, item.EntityReport))
                        result.Add(ToEntityDirectReportDTO(item));
                }
            }
            return result;
        }

        public EntityDirectReportDTO GetEntityDirectReport(DR_Requester requester, int iD)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var item = projectContext.EntityDirectlReport.First(x => x.ID == iD);
                if (bizEntityReport.DataIsAccessable(requester, item.EntityReport))
                {
                    var rItem = ToEntityDirectReportDTO(item);
                    return rItem;
                }
                else
                    return null;
            }
        }




        private EntityDirectReportDTO ToEntityDirectReportDTO(EntityDirectlReport dbRel)
        {

            var result = new EntityDirectReportDTO();
            bizEntityReport.ToEntityReportDTO(dbRel.EntityReport, result);
            result.URL = dbRel.URL;
            return result;
        }

        public int UpdateEntityDirectReport(EntityDirectReportDTO message)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                BizEntityReport bizEntityReport = new MyModelManager.BizEntityReport();

                var dbEntitySpecifiedReport = projectContext.EntityDirectlReport.FirstOrDefault(x => x.ID == message.ID);
                if (dbEntitySpecifiedReport == null)
                {
                    message.ReportType = ReportType.DirectReport;
                    dbEntitySpecifiedReport = new EntityDirectlReport();
                    dbEntitySpecifiedReport.EntityReport = bizEntityReport.ToNewEntityReport(message);
                }
                else
                    bizEntityReport.ToUpdateEntityReport(dbEntitySpecifiedReport.EntityReport, message);

                dbEntitySpecifiedReport.URL = message.URL;
                if (dbEntitySpecifiedReport.ID == 0)
                    projectContext.EntityDirectlReport.Add(dbEntitySpecifiedReport);
                projectContext.SaveChanges();
                return dbEntitySpecifiedReport.ID;
            }
        }
    }

}
