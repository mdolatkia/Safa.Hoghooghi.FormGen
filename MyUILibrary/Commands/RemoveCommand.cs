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
    class RemoveCommand : BaseCommand
    {
        I_EditEntityAreaMultipleData EditArea { set; get; }
        public RemoveCommand(I_EditEntityAreaMultipleData editArea) : base()
        {
            EditArea = editArea;
            //if (AgentHelper.GetAppMode() == AppMode.Paper)
            //    CommandManager.SetTitle("Remove");
            //else
                CommandManager.SetTitle("حذف");
            CommandManager.ImagePath = "Images//remove.png";
            CommandManager.Clicked += CommandManager_Clicked;
        }
        List<DP_DataRepository> deleteDataList = new List<DP_DataRepository>();
        private void CommandManager_Clicked(object sender, EventArgs e)
        {
            //if (EditArea.AreaInitializer.SourceRelation == null)
            //    return;

            //if (EditArea.AreaInitializer.SourceRelation != null)
            //{
            //    //EditArea.AreaInitializer.SourceRelation.RelatedData.ValueChanged = true;
            //}

            //(EditArea as I_EditEntityAreaMultipleData).RemoveSelectedData();
            var datas = EditArea.GetSelectedData();
            EditArea.RemoveData(datas, true);


        }


      


    }
}
