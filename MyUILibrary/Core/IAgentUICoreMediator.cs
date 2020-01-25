using System;
using System.Collections.Generic;
namespace MyUILibrary
{
    public interface IAgentUICoreMediator
    {




        IAgentUIManager UIManager
        {
            get;
            set;
        }


        //////DataManager.DataRequest.DR_Requester Requester {
        //////    get;
        //////    set;
        //////}


        //////IAG_RequestManager RequestManager {
        //////    get;
        //////    set;
        //////}
        UserInfo UserInfo { set; get; }
        void SetUIManager(IAgentUIManager uiMnager);
        void StartApp();
        //////DataManager.PackageManager.DP_ResultPackage GetListPackage(DataManager.PackageManager.DP_EntityRequest request);
        //////DataManager.PackageManager.DP_ResultPackageStructure GetPackageTreeStructe(DataManager.PackageManager.DP_BasePackageRequest request);
        //////DataManager.PackageManager.DP_ResultRelatedPackage GetRelatedPackage(DataManager.PackageManager.DP_RequestRelatedPackage request);
        //////DataManager.DataRequest.DR_ResultRequest ExecuteCommand(AG_CommandExecutionRequest command);

    }
 
}
