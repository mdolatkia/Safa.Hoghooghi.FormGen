using CommonDefinitions.UISettings;
using MyUILibrary;
using MyUILibrary.EntityArea;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.EntityArea.Commands
{
    class SelectCommand : BaseCommand
    {
        I_ViewEntityArea ViewArea { set; get; }
        public SelectCommand(I_ViewEntityArea viewArea) : base()
        {
            ViewArea = viewArea;
            CommandManager.SetTitle("انتخاب");
            CommandManager.ImagePath = "Images//Confirm.png";
            CommandManager.Clicked += CommandManager_Clicked;
        }
        private void CommandManager_Clicked(object sender, EventArgs e)
        {
            var selectedData = ViewArea.GetSelectedData();

            if (selectedData.Count > 0)
            {
                //packageArea.ViewInitializer.SourceEditArea.DataSelected(selectedData, packageArea);
                //AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog(packageArea.ViewView);
                ViewArea.OnDataSelected(selectedData);
            }
        }

    }
}
