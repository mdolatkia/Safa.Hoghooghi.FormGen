using MyUILibrary.PackageArea.Commands;
using System;
using System.Collections.Generic;
namespace MyUILibrary.PackageArea
{
    public interface I_SearchViewPackageArea
    {


		event EventHandler<Arg_PackageSelected> DataPackageSelected;


        I_View_SearchViewPackageArea View
        {
            get;
            set;
        }


		List<I_PackageAreaCommand> Commands {
			get;
			set;
		}


        //DataAccess.Entity SearchTemplate
        //{
        //    get;
        //    set;
        //}


        //////DataManager.DataPackage.DP_Package ViewTemplate {
        //////    get;
        //////    set;
        //////}


        //////DataManager.DataPackage.DP_Package SearchDataPackage {
        //////    get;
        //////    set;
        //////}


        //////List<DataManager.DataPackage.DP_Package> ViewDataPackages {
        //////    get;
        //////    set;
        //////}


        MyUILibrary.EntityArea.I_EditEntityArea SourceEditPackageArea
        {
			get;
			set;
		}

		void LoadTemplate(SearchViewPackageAreaInitializer initParam);
		void CommandExecuted(I_PackageAreaCommand command);


       void OnDataPackagesSelected(List<DataAccess.Entity> packages);
    }

}
