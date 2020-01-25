using ModelEntites;
using MyDataManagerBusiness;
using MyExternalReportLibrary;
using MyModelManager;
using MyPackageManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDataManagerService
{
    public class ReportManagerService
    {
        //BizEntityListReportGrouped bizEntityListReportGrouped = new BizEntityListReportGrouped();
        BizEntityListReport bizEntityListReport = new BizEntityListReport();
        BizEntityReport bizEntityReport = new BizEntityReport();
        BizEntitySearchableReport bizEntitySearchableReport = new BizEntitySearchableReport();

        BizEntityDirectReport bizEntityDirectReport = new BizEntityDirectReport();
        BizDataMenuSetting bizEntityReportSetting = new BizDataMenuSetting();
        MyExternalReportHandler myExternalReportHandler = new MyExternalReportHandler();
        BizEntityDataViewReport bizEntityDataViewReport = new BizEntityDataViewReport();
        BizEntityGridViewReport bizEntityGridViewReport = new BizEntityGridViewReport();
        //BizEntityDataLinkReport bizEntityDataLinkReport = new BizEntityDataLinkReport();
        BizEntityExternalReport bizEntityExternalReport = new BizEntityExternalReport();
        //public List<EntityReportDTO> GetReports(int entityID)
        //{
        //    return bizEntityReport.GetEntityReports(entityID);
        //}
        public EntityReportDTO GetReport(DR_Requester requester, int reportID)
        {
            return bizEntityReport.GetEntityReport(requester, reportID);
        }

        //اینجا دسترسی چک نشه؟
        //public EntitySearchableReportDTO GetSearchableReport(DR_Requester requester, int reportID)
        //{
        //    return bizEntitySearchableReport.GetEntitySearchableReport(requester, reportID, true);
        //}
        public EntityListReportDTO GetListReport(DR_Requester requester, int reportID)
        {
            return bizEntityListReport.GetEntityListReport(requester, reportID, true);
        }
        //public EntityListReportGroupedDTO GetListReportGrouped(int reportID)
        //{
        //    return bizEntityListReportGrouped.GetEntityListReportGrouped(reportID, true);
        //}
        public EntityDataViewReportDTO GetDataViewReport(DR_Requester requester, int reportID)
        {
            return bizEntityDataViewReport.GetEntityDataViewReport(requester, reportID, true);
        }
        public EntityGridViewReportDTO GetGridViewReport(DR_Requester requester, int reportID)
        {
            return bizEntityGridViewReport.GetEntityGridViewReport(requester,reportID, true);
        }
        //public EntityDataLinkReportDTO GetDataLinkReport(int reportID)
        //{
        //    return bizEntityDataLinkReport.GetEntityDataLinkReport(reportID, true);
        //}
        public long GetExternalReportKey(DR_Requester requester, int reportID, int entityID, DP_SearchRepository searchItem)
        {
            return myExternalReportHandler.GetExternalReportKey(requester, reportID, entityID, searchItem);
        }

        public EntityExternalReportDTO GetExternalReport(int iD)
        {
            return bizEntityExternalReport.GetEntityExternalReport(iD, true);
        }

        //public List<EntityDirectReportDTO> GetEntityDirectReports(DR_Requester requester, int entityID)
        //{
        //    return bizEntityDirectReport.GetEntityDirectReports(requester, entityID);
        //}

        public EntityDirectReportDTO GetEntityDirectReport(DR_Requester requester, int iD)
        {
            return bizEntityDirectReport.GetEntityDirectReport(requester, iD);
        }
    }
}
