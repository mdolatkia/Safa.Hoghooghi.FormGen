using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary
{
    public class BaseCommand : I_Command
    {
        public BaseCommand()
        {
            CommandManager = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetCommandManager();
        }
        public I_CommandManager CommandManager
        {
            set; get;
        }
    }
}
