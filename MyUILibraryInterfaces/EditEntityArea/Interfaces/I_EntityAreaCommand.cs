using CommonDefinitions.UISettings;
using MyUILibrary.PackageArea;
using System;
using System.Collections.Generic;
namespace MyUILibrary.EntityArea.Commands
{
    public interface I_EntityAreaCommand : I_Command
    {

        //bool IsEnable(I_EditEntityArea editArea);

        void Execute(I_EditEntityArea editArea);
   

    }

}
