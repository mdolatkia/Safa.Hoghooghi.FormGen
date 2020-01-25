using ModelEntites;
using MyDataManagerBusiness;
using MyModelManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProxyLibrary;
using MyDataSearchManagerBusiness;
using MyRelationshipDataManager;

namespace MyDataManagerService
{
    public class RelationshipManagerService
    {
        BizRelationship biz = new BizRelationship();
        BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
        public RelationshipManagerService()
        {
            //bizRelationshipManager = new BizRelationshipManager();
        }

        public bool ReverseRelationshipIsMandatory(int relationshipID)
        {

            return biz.ReverseRelationshipIsMandatory(relationshipID);
        }
        //public List<RelationshipDTO> GetRelationshipsByEntityID(int entityID)
        //{

        //    return biz.GetRelationshipsByEntityID(entityID);
        //}
        //public List<RelationshipDTO> GetRelationshipsBetweenEntities(int firstEntityID, int secondEnitityID)
        //{

        //    return biz.GetRelationshipsBetweenEntities(firstEntityID, secondEnitityID);
        //}
        //چون از فانکشن های سیستمی هست ریکوئستر نیاز ندارد

     

        //public DP_SearchRepository GetSearchDataItemByRelationship(RelationshipSreachType searchType, DP_DataView dataView, int relationshipID)
        //{//firstSideDataItem باید داری فیلد های کلید اصلی باشد
           
        //    return relationshipDataManager.GetSearchDataItemByRelationship(searchType, dataRepository, relationshipID);
        //}

        //public DP_SearchRepository GetSearchDataItemByRelationship(RelationshipSreachType searchType, DP_DataRepository dataItem, int relationshipID)
        //{//firstSideDataItem باید داری فیلد های کلید اصلی باشد
        //    RelationshipDataManager relationshipDataManager = new RelationshipDataManager();
        //    return relationshipDataManager.GetSearchDataItemByRelationship(searchType, dataItem, relationshipID);
        //}



        //public RelationshipDTO GetRelationship(int relationshipID)
        //{
        //    return biz.GetRelationship(relationshipID);
        //}

        //public EntityRelationshipTailDTO GetRelationshipTail(DR_Requester requester, int ID, bool withDetails)
        //{
        //    return bizEntityRelationshipTail.GetEntityRelationshipTail( requester, ID);
        //}
        //public List<EntityRelationshipTailDTO> GetRelationshipTails(DR_Requester requester, int entityID)
        //{
        //    return bizEntityRelationshipTail.GetEntityRelationshipTails( requester, entityID);
        //}
    }
}
