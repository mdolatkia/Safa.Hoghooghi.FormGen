using CommonDefinitions.UISettings;

using MyUILibrary;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.EntityArea.Commands
{
    class GridViewCommand : BaseCommand
    {
        I_EditEntityArea EditArea { set; get; }
        public GridViewCommand(I_EditEntityArea editArea) : base()
        {
            EditArea = editArea;
            CommandManager.SetTitle("گرید داده");
            CommandManager.ImagePath = "Images//grid.png";
            CommandManager.Clicked += CommandManager_Clicked;
        }

        private void CommandManager_Clicked(object sender, EventArgs e)
        {
            if (EditArea.SearchViewEntityArea != null)
            {
                var searchDP = EditArea.SearchViewEntityArea?.SearchEntityArea?.LastSearch;
                //if (searchDP != null)
                //{
                //if (packageArea.GridViewArea == null)
                //{

                //}
                //else
                //{
                AgentUICoreMediator.GetAgentUICoreMediator.ShowDataViewGridViewArea(EditArea.AreaInitializer.EntityID, EditArea.SimpleEntity.Alias, false, true, false, searchDP, true, 0, null, null);
                //}

                //}
            }
        }
    }
}
