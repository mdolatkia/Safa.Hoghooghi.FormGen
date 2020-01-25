﻿using CommonDefinitions.UISettings;
using MyUILibrary;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.EntityArea.Commands
{
    public class AdvancedSearchConfirmCommand : BaseCommand
    {
        I_AdvancedSearchEntityArea SearchArea { set; get; }
        public AdvancedSearchConfirmCommand(I_AdvancedSearchEntityArea searchArea) : base()
        {
            SearchArea = searchArea;
            //if (AgentHelper.GetAppMode() == AppMode.Paper)
            //    CommandManager.SetTitle("Search");
            //else
                CommandManager.SetTitle("جستجو");
            CommandManager.ImagePath = "Images//search.png";
            CommandManager.Clicked += CommandManager_Clicked;
        }
        private void CommandManager_Clicked(object sender, EventArgs e)
        {
            //Enabled = false;
            //try
            //{
            var searchRepository = SearchArea.GetSearchRepository();
            SearchArea.OnSearchDataDefined(searchRepository);
            //Enabled = true;
            //var command = new AG_CommandExecutionRe
        }

    }
}