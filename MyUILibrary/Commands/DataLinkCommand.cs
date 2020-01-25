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
    class DataLinkCommand : BaseCommand
    {
        I_EditEntityArea EditArea { set; get; }
        public DataLinkCommand(I_EditEntityArea editArea) : base()
        {
            EditArea = editArea;
            CommandManager.SetTitle("لینک داده");
            CommandManager.ImagePath = "Images//Close.png";
            CommandManager.Clicked += CommandManager_Clicked;
        }

        private void CommandManager_Clicked(object sender, EventArgs e)
        {
            if (EditArea.SearchViewEntityArea != null)
            {
                var searchDP = EditArea.SearchViewEntityArea.SearchEntityArea.LastSearch;
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
                    }
                }
                //if (searchDP != null)
                //{
                //if (packageArea.DataLinkArea == null)
                //{



                //}
                //else
                //{

                //}
                AgentUICoreMediator.GetAgentUICoreMediator.ShowDataLinkArea(EditArea.AreaInitializer.EntityID, 0, false, "لینک داده ", dataView);
                //}
            }
        }






    }
}
