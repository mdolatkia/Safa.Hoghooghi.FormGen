using CommonDefinitions.UISettings;
using MyUILibrary.EntityArea.Commands;
using System;
using System.Collections.Generic;
namespace MyUILibrary.PackageArea.Commands
{
    public interface I_PackageAreaCommand : I_Command
    {


		void Execute(I_EditPackageArea packageArea);
        //void Execute(I_SearchViewPackageArea packageArea);

        //bool IsEnable(I_EditPackageArea editArea);
	}

}
