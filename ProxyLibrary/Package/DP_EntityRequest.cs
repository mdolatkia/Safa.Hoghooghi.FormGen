using ModelEntites;

using System;
using System.Collections.Generic;
namespace ProxyLibrary
{
    public class DP_EntityRequest : BaseRequest
    {
        public DP_EntityRequest(DR_Requester Requester) : base(Requester)
        {

        }
        public EntityColumnInfoType EntityColumnInfoType { set; get; }
        public EntityRelationshipInfoType EntityRelationshipInfoType { set; get; }
        public int EntityID;

        //////public List<DataManager.DataPackage.DP_PackageCategory> Categories;

    }
    public class DP_SearchEntitiesRequest : BaseRequest
    {
        public DP_SearchEntitiesRequest(DR_Requester Requester) : base(Requester)
        {

        }
        public string SearchText;

        //////public List<DataManager.DataPackage.DP_PackageCategory> Categories;

    }
    public class DP_EntityUIsettingsRequest : BaseRequest
    {
        public DP_EntityUIsettingsRequest(DR_Requester Requester) : base(Requester)
        {

        }
        public int EntityID;
    }
    public class DP_EntityUISettingResult : BaseResult
    {
        public EntityUISettingDTO UISetting { set; get; }
    }
    public class DP_EntityPermissionRequest : BaseRequest
    {
        public DP_EntityPermissionRequest(DR_Requester Requester) : base(Requester)
        {

        }
        public int EntityID;
        public bool WithChilds;
    }
    public class DP_EntityPermissionResult : BaseResult
    {
        public AssignedPermissionDTO Permissions { set; get; }
    }

    public class DP_EntityConditionalPermissionRequest : BaseRequest
    {
        public DP_EntityConditionalPermissionRequest(DR_Requester Requester) : base(Requester)
        {

        }

        public int EntityID;
    }
    public class DP_EntityConditionalPermissionResult : BaseResult
    {
        public List<ConditionalPermissionDTO> ConditionalPermissions { get; set; }
    }

    public class DP_EntityUICompositionRequest : BaseRequest
    {
        public DP_EntityUICompositionRequest(DR_Requester Requester) : base(Requester)
        {

        }
        public int EntityID;
    }
    public class DP_EntityUICompositionResult : BaseResult
    {
        public EntityUICompositionCompositeDTO UIComposition { set; get; }
    }
    public class DP_EntityCommandsRequest : BaseRequest
    {
        public DP_EntityCommandsRequest(DR_Requester Requester) : base(Requester)
        {

        }
        public int EntityID;
    }
    public class DP_EntityCommandsResult : BaseResult
    {
        public List<EntityCommandDTO> Commands { set; get; }
    }

    public class DP_EntityValidationsRequest : BaseRequest
    {
        public DP_EntityValidationsRequest(DR_Requester Requester) : base(Requester)
        {

        }
        public int EntityID;
    }
    public class DP_EntityValidationsResult : BaseResult
    {
        public List<EntityValidationDTO> Validations { set; get; }
    }

    public class DP_EntityStatesRequest : BaseRequest
    {
        public DP_EntityStatesRequest(DR_Requester Requester) : base(Requester)
        {

        }
        public int EntityID;
    }
    public class DP_EntityStatesResult : BaseResult
    {
        public List<EntityStateDTO> States { set; get; }
    }

    public class DP_EntityActionActivitiesRequest : BaseRequest
    {
        public DP_EntityActionActivitiesRequest(DR_Requester Requester) : base(Requester)
        {

        }
        public int EntityID;
    }
    public class DP_EntityActionActivitiesResult : BaseResult
    {
        public List<UIActionActivityDTO> ActionActivities { set; get; }
    }
}
