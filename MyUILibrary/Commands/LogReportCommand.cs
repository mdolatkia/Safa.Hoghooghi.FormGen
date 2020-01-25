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
    class LogReportCommand : BaseCommand
    {
        I_EditEntityArea EditArea { set; get; }
        public LogReportCommand(I_EditEntityArea editArea) : base()
        {
            EditArea = editArea;
            CommandManager.SetTitle("گزارش لاگ");
            CommandManager.ImagePath = "Images//zoom_extend.png";
            CommandManager.Clicked += CommandManager_Clicked;
        }

        private void CommandManager_Clicked(object sender, EventArgs e)
        {
            //DP_DataRepository dataInstance = null;
            //if (EditArea.AreaInitializer.DataMode != DataMode.Multiple)
            //{
            //    dataInstance = EditArea.GetDataList().FirstOrDefault();
            //}
            //else
            //    dataInstance = (EditArea as EditEntityAreaMultipleData).GetSelectedData().FirstOrDefault();
            //if (dataInstance != null)
            //    if (dataInstance.IsNewItem)
            //        dataInstance = null;
            //if (dataInstance != null)
            //{

            var initializer = new MyUILibraryInterfaces.LogReportArea.LogReportAreaInitializer();
            initializer.EntityID = EditArea.AreaInitializer.EntityID;
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
                    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عملیات لاگ تنها بروی داده های ثبت شده امکان پذیر است");
                    return;
                }

                initializer.DataItem = dataInstance.DataView;
                AgentUICoreMediator.GetAgentUICoreMediator.ShowLogReportArea(initializer, "گزارش لاگ", true);
            }

            //}
        }
    }
}
