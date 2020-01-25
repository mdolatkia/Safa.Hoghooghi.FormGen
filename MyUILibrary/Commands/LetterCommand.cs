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
    class LetterCommand : BaseCommand
    {
        I_EditEntityArea EditArea { set; get; }
        public LetterCommand(I_EditEntityArea editArea) : base()
        {
            EditArea = editArea;
            CommandManager.SetTitle("نامه");
            CommandManager.ImagePath = "Images//letter.png";
            CommandManager.Clicked += CommandManager_Clicked;
        }

        private void CommandManager_Clicked(object sender, EventArgs e)
        {
            DP_DataRepository dataInstance = null;
            if (EditArea.AreaInitializer.DataMode != DataMode.Multiple)
            {
                dataInstance = EditArea.GetDataList().FirstOrDefault();
            }
            else
                dataInstance = (EditArea as EditEntityAreaMultipleData).GetSelectedData().FirstOrDefault();
            if (dataInstance != null)
            {
                if (dataInstance.DataView == null || dataInstance.IsNewItem)
                {
                    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عملیات نامه ها تنها بروی داده های ثبت شده امکان پذیر است");
                    return;
                }

                AgentUICoreMediator.GetAgentUICoreMediator.ShowLetterArea(EditArea.AreaInitializer.EntityID, EditArea.SimpleEntity.Alias, false, dataInstance.DataView);

            }
        }


    }
}
