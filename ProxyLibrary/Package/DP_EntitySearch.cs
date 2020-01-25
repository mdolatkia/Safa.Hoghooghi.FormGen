using ModelEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyLibrary
{
    public class DP_EntitySearchResult : BaseResult
    {
        public DP_EntitySearchResult()
        {

        }
        public EntitySearchDTO EntitySearch;
        //public AssignedPermissionDTO Permissoins { set; get; }

        //public List<EntityValidationDTO> Validations { set; get; }
        //public List<EntityUICompositionDTO> UICompositions { set; get; }

        //public List<EntityStateDTO> States { set; get; }
        //public List<EntityCommandDTO    > Commands { set; get; }
        //public List<ActionActivityDTO> ActionActivities { set; get; }
        //public List<ConditionalPermissionDTO> ConditionalPermissions { get; set; }
    }
    public class DP_EntitySearchRequest : BaseRequest
    {
        public DP_EntitySearchRequest(DR_Requester Requester) : base(Requester)
        {

        }
        public int EntitySearchID;
    }
}
