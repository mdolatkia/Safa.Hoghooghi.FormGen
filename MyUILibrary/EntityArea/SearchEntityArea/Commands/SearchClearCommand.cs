using CommonDefinitions.UISettings;
using MyUILibrary;
using MyUILibrary.EntityArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.EntityArea.Commands
{
    public class SearchClearCommand : BaseCommand
    {
        I_BaseSearchEntityArea SearchArea { set; get; }
        public SearchClearCommand(I_BaseSearchEntityArea searchArea) : base()
        {
            SearchArea = searchArea;
            CommandManager.SetTitle("پاک کردن");
            CommandManager.ImagePath = "Images//Clear.png";
            CommandManager.Clicked += CommandManager_Clicked;
        }

        private void CommandManager_Clicked(object sender, EventArgs e)
        {
            SearchArea.ClearSearchData();
        }

    }
}
