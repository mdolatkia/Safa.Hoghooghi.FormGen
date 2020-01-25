using CommonDefinitions.BasicUISettings;
using CommonDefinitions.UISettings;
using MyUILibrary.PackageArea.Commands;
using System;
using System.Collections.Generic;
namespace MyUILibrary.PackageArea
{
    public interface I_View_EditPackageArea
    {




        //List<DataManager.DataPackage.DP_Package> CurrentShownDataPackages {
        //    get;
        //}


        I_EditPackageArea Controller
        {
            get;
            set;
        }


        //GridSetting UISetting
        //{
        //    get;
        //    set;
        //}

        //void ShowDataPckages(List<DataManager.DataPackage.DP_Package> dataPackages);
        //void UpdateDataPckages(List<DataManager.DataPackage.DP_Package> dataPackages);
        //void AddCommands(List<I_PackageAreaCommand> commands);
        
        //void ShowViewOfEditNDTypeArea(IAG_View_EditNDTypeArea view);

        void ShowEditAreaView(object view);
    }

}
