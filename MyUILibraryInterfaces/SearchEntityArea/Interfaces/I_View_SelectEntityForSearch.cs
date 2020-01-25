
using CommonDefinitions.UISettings;

using System;
using System.Collections.Generic;
namespace MyUILibrary.EntityArea
{
    public interface I_View_SelectEntityForSearch
    {
        void AddSearchAreaView(I_View_SearchEntityArea view);
        void AddViewAreaView(I_View_ViewEntityArea view);
    }

}
