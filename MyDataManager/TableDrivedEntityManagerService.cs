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
    public class TableDrivedEntityManagerService
    {
        //SecurityHelper securityHelper = new SecurityHelper();
      //  BizPackageManager bizPackageManager { set; get; }
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        public TableDrivedEntityManagerService()
        {
        }




        //public DP_EntityActionActivitiesResult GetEntityActionActivities(DP_EntityActionActivitiesRequest request)
        //{
        //    return bizPackageManager.GetEntityActionActivities(request);
        //}
    
        public TableDrivedEntityDTO GetSimpleEntity(DR_Requester requester, int entityID, List<SecurityAction> specificActions = null)
        {
            return bizTableDrivedEntity.GetSimpleEntity(requester,entityID, specificActions);
        }

        public TableDrivedEntityDTO GetEntityWithSimpleColumns(DR_Requester requester, int entityID, List<SecurityAction> specificActions = null)
        {
            return bizTableDrivedEntity.GetTableDrivedEntity(requester,entityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships, specificActions);
        }

        public TableDrivedEntityDTO GetFullEntity(DR_Requester requester, int entityID, List<SecurityAction> specificActions = null)
        {
         return bizTableDrivedEntity.GetTableDrivedEntity(requester,entityID, EntityColumnInfoType.WithFullColumns, EntityRelationshipInfoType.WithRelationships, specificActions);
        }

        public object GetSimpleEntity(DR_Requester dR_Requester, int v, object specificActions)
        {
            throw new NotImplementedException();
        }

        //public TableDrivedEntityDTO GetEntity(int entityID, EntityColumnInfoType entityColumnInfoType, EntityRelationshipInfoType entityRelationshipInfoType)
        //{
        //    return bizTableDrivedEntity.GetTableDrivedEntity(entityID, entityColumnInfoType, entityRelationshipInfoType);
        //}

        public List<TableDrivedEntityDTO> SearchEntities(DR_Requester requester, string singleFilterValue, bool? isView, List<SecurityAction> specificActions = null)
        {
            return bizTableDrivedEntity.GetAllEntities(requester,singleFilterValue, isView, specificActions);
        }

        //public DP_EntitySearchResult GetEntitySearch(DP_EntitySearchRequest request)
        //{
        //    return bizPackageManager.GetEntitySearch(request);
        //}
        //public RR_ReportResult GetReport(RR_ReportRequest request, bool withDetails)
        //{
        //    return bizPackageManager.GetReport(request, withDetails);
        //}
        //public List<RelationshipFilterDTO> GetRelationshipFilters(int relationshipID)
        //{
        //    return bizPackageManager.GetRelationshipFilters(relationshipID);
        //}

        //public DP_NavigatoinTreeResult GetNavigationTree(DP_NavigationTreeRequest request)
        //{
        //    return bizPackageManager.GetNavigationTree(request);
        //}
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

        public TableDrivedEntityDTO GetDataEntryEntity(DR_Requester requester, int entityID)
        {
            return bizTableDrivedEntity.GetDataEntryEntity(requester,entityID);
        }
        public TableDrivedEntityDTO GetPermissionedEntity(DR_Requester requester, int entityID)
        {
            return bizTableDrivedEntity.GetPermissionedEntity(requester, entityID);
        }

        public AssignedPermissionDTO GetEntityAssignedPermissions(DR_Requester requester, int entityID, bool withChildObjects)
        {
            return bizTableDrivedEntity.GetEntityAssignedPermissions(requester, entityID, withChildObjects);
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
