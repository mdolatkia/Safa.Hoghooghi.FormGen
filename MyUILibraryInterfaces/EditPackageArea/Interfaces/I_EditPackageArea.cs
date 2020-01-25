using MyUILibrary.PackageArea.Commands;
using System;
using System.Collections.Generic;
namespace MyUILibrary.PackageArea
{
    public interface I_EditPackageArea
    {




        I_View_EditPackageArea View
        {
			get;
			set;
		}


		List<I_PackageAreaCommand> Commands {
			get;
			set;
		}
        MyUILibrary.EntityArea.I_EditEntityArea MainEntityArea
        {
            get;
            set;
        }
     
        //////DataManager.DataPackage.DP_Package AreaInitializer.{
        //////    get;
        //////    set;
        //////}


        //List<DP_DataRepository> DataPackages
        //{
        //    get;
        //    set;
        //}
        //List<DP_DataRepository>  CurrentShownData
        //{
        //    get;
        //}
        //void AddEmptyDataPackage();
        void LoadTemplate(EditPackageAreaInitializer initParam);

        //////void RemoveData(List<DP_DataRepository> packageData);
        //////void AddData(List<DP_DataRepository> packageData);
		void CommandExecuted(I_PackageAreaCommand command);

        //void ShowData();
        MyUILibrary.EntityArea.UpdateValidationResult UpdateData();

        //MyUILibrary.EntityArea.DeleteResult DeleteData();
	}

}
