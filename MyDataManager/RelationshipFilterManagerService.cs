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
    public class RelationshipFilterManagerService
    {
        BizRelationshipFilter bizRelationshipFilter = new BizRelationshipFilter();
        public List<RelationshipFilterDTO> GetRelationshipFilters(DR_Requester requester, int relationshipID)
        {
            return bizRelationshipFilter.GetRelationshipFilters( requester, relationshipID);
        }
    }
}
