using CommonDefinitions.UISettings;
using MyUILibrary;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.EntityArea.Commands
{
    class CloseDialogCommand : BaseCommand
    {
        I_EditEntityAreaMultipleData EditArea { set; get; }
        public CloseDialogCommand(I_EditEntityAreaMultipleData editArea) : base()
        {
            EditArea = editArea;
            //if (AgentHelper.GetAppMode() == AppMode.Paper)
            //    CommandManager.SetTitle("Close");
            //else
                CommandManager.SetTitle("بازگشت");
            CommandManager.ImagePath = "Images//Close.png";
            CommandManager.Clicked += CommandManager_Clicked;
        }
        private void CommandManager_Clicked(object sender, EventArgs e)
        {
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog(EditArea.DataView);
        }
    }
}
