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
    class ArchiveCommand : BaseCommand
    {
        I_EditEntityArea EditArea { set; get; }
        public ArchiveCommand(I_EditEntityArea editArea) : base()
        {
            EditArea = editArea;
            CommandManager.SetTitle("آرشیو");
            CommandManager.ImagePath = "Images//archive.png";
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
            DP_DataView dataView = null;
            if (dataInstance != null)
            {
                if (dataInstance.DataView != null && !dataInstance.IsNewItem)
                {
                    dataView = dataInstance.DataView;
                    //AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عملیات آرشیو تنها بروی داده های ثبت شده امکان پذیر است");
                    //return;
                }
            }
            AgentUICoreMediator.GetAgentUICoreMediator.ShowArchiveArea(EditArea.AreaInitializer.EntityID, EditArea.SimpleEntity.Alias, true, dataView);

        }
    }
}
