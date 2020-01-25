using CommonDefinitions.UISettings;

using MyUILibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.EntityArea.Commands
{
    class SearchCommand : BaseCommand
    {
        I_EditEntityArea EditArea { set; get; }
        public SearchCommand(I_EditEntityArea editArea) : base()
        {
            EditArea = editArea;
            //if (AgentHelper.GetAppMode() == AppMode.Paper)
            //    CommandManager.SetTitle("Search");
            //else
                CommandManager.SetTitle("جستجو");
            CommandManager.ImagePath = "Images//search.png";
            CommandManager.Clicked += CommandManager_Clicked;
        }

        private void CommandManager_Clicked(object sender, EventArgs e)
        {
            if (EditArea.SearchViewEntityArea != null)
            {
                //AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(packageArea.SearchViewEntityArea.View, packageArea.AreaInitializer.Title);
                EditArea.ShowTemporarySearchView(true);
            }
        }

    }
}
