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
    class SaveAndCloseDialogCommand : BaseCommand
    {
        I_EditEntityArea EditArea { set; get; }
        public SaveAndCloseDialogCommand(I_EditEntityArea editArea) : base()
        {
            EditArea = editArea;
            //if (AgentHelper.GetAppMode() == AppMode.Paper)
            //    CommandManager.SetTitle("Save and close");
            //else
            CommandManager.SetTitle("تایید و خروج");
            CommandManager.ImagePath = "Images//letter.png";
            CommandManager.Clicked += CommandManager_Clicked;
        }

        private void CommandManager_Clicked(object sender, EventArgs e)
        {
            var result = EditArea.UpdateData();
            if (!result.IsValid)
            {
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowMessage("پیام", result.Message);
                return;
            }
            if (EditArea.AreaInitializer.IntracionMode == IntracionMode.CreateInDirect
                || EditArea.AreaInitializer.IntracionMode == IntracionMode.CreateSelectInDirect)
            {
                if (EditArea is I_EditEntityAreaOneData)
                    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog((EditArea as I_EditEntityAreaOneData).DataView);
                else if (EditArea is I_EditEntityAreaMultipleData)
                    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog((EditArea as EditEntityAreaMultipleData).DataView);
            }

        }





    }
}
