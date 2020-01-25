using CommonDefinitions.UISettings;

using MyUILibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.EntityArea.Commands
{
    class InfoCommand : BaseCommand
    {
        I_EditEntityArea EditArea { set; get; }
        public InfoCommand(I_EditEntityArea editArea) : base()
        {
            EditArea = editArea;
            CommandManager.SetTitle("اطلاعات");
            CommandManager.ImagePath = "Images//info.png";
            CommandManager.Clicked += CommandManager_Clicked;
        }
        private void CommandManager_Clicked(object sender, EventArgs e)
        {
            //var tail = AgentUICoreMediator.GetAgentUICoreMediator.RelationshipTailDataManager.GetRelationshipTail(6041);
            //var val = AgentHelper.GetValueSomeHow(EditArea, tail, 33);
            AgentHelper.ShowEditEntityAreaInfo(EditArea);
        }

    }
}
