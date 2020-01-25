using ModelEntites;
using MyDataManagerBusiness;
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
    public class PackageManagerService 
    {

        BizPackageManager bizPackageManager { set; get; }
        public PackageManagerService()
        {
            bizPackageManager = new BizPackageManager();
        }

        //public DP_EntityUISettingResult GetEntityUISettings(DP_EntityUIsettingsRequest request)
        //{
        //    return bizPackageManager.GetEntityUISettings(request);
        //}
        //public DP_EntityPermissionResult GetEntityPermissions(DP_EntityPermissionRequest request)
        //{
        //    return bizPackageManager.GetEntityPermissions(request);
        //}
       
        //public DP_EntityUICompositionResult GetEntityUICompositionComposite(DP_EntityUICompositionRequest request)
        //{
          
        //}
     
     
        //public DP_EntityActionActivitiesResult GetEntityActionActivities(DP_EntityActionActivitiesRequest request)
        //{
        //    return bizPackageManager.GetEntityActionActivities(request);
        //}

        public DP_EntityResult GetEntity(DP_EntityRequest request, EntityColumnInfoType entityColumnInfoType, EntityRelationshipInfoType entityRelationshipInfoType)
        {
            return bizPackageManager.GetEntity(request, entityColumnInfoType, entityRelationshipInfoType);
        }
        public List<TableDrivedEntityDTO> SearchEntities(DP_SearchEntitiesRequest request)
        {
            return bizPackageManager.SearchEntities(request);
        }
        public DP_EntitySearchResult GetEntitySearch(DP_EntitySearchRequest request)
        {
            return bizPackageManager.GetEntitySearch(request);
        }
        public RR_ReportResult GetReport(RR_ReportRequest request, bool withDetails)
        {
            return bizPackageManager.GetReport(request, withDetails);
        }
        //public List<RelationshipFilterDTO> GetRelationshipFilters(int relationshipID)
        //{
        //    return bizPackageManager.GetRelationshipFilters(relationshipID);
        //}

        public DP_NavigatoinTreeResult GetNavigationTree(DP_NavigationTreeRequest request)
        {
            return bizPackageManager.GetNavigationTree(request);
        }
        public bool IndependentDataEntry(int entityID)
        {
            BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
            return bizTableDrivedEntity.IndependentDataEntry(entityID) ;
        }

        public RelationshipDTO GetRelationship(int relationshipID)
        {
            BizRelationship bizRelationship = new BizRelationship();
            return bizRelationship.GetRelationship(relationshipID);
        }

        public  bool IsEntityEnabled(int entityID)
        {
            BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
            return bizTableDrivedEntity.IsEntityEnabled(entityID);
        }

 
        //public DP_ResultPackage FindPackages(DP_EntityRequest request)
        //{
        //    throw new NotImplementedException();
        //}


        //public DP_ResultRelatedPackage GetRelatedPackage(DP_RequestRelatedPackage request)
        //{
        //    return bizPackageManager.GetRelatedPackage(request);
        //}

        //public DP_ResultDatabaseList GetDatabaseList(DP_DatabaseRequest request)
        //{
        //    return bizPackageManager.GetDatabaseList(request);
        //}
    }
}
