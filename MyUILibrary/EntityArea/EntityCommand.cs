using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDefinitions.UISettings;
using ModelEntites;
using MyUILibrary.EntityArea.Commands;
using MyCodeFunctionLibrary;
using ProxyLibrary;
using MyUILibrary.EntityArea;
using MyUILibraryInterfaces.EditEntityArea;
using MyUILibraryInterfaces;

namespace MyUILibrary.EntityArea
{
    public class EntityCommand : BaseCommand
    {
        //CommandAttributes CommandAttributes
        EntityCommandDTO EntityCommandDTO { set; get; }
        I_EditEntityArea EditArea { set; get; }
        public EntityCommand(EntityCommandDTO entityCommand, I_EditEntityArea editArea)
        {
            EntityCommandDTO = entityCommand;
            EditArea = editArea;
            CommandManager.SetTitle(EntityCommandDTO.Title);
            //CommandManager.ImagePath = EntityCommandDTO. "Images//clear.png";
            CommandManager.Clicked += CommandManager_Clicked;
        }

        private void CommandManager_Clicked(object sender, EventArgs e)
        {
            CodeFunctionHandler codeFunctionHandler = new CodeFunctionHandler();
            //var parameters = new List<object>();
            //parameters.Add(new CommandFunctionParam(editArea, AgentUICoreMediator.GetAgentUICoreMediator.GetRequester()));
            DP_DataRepository data = null;
            if (EditArea is I_EditEntityAreaOneData)
            {
                data = (EditArea as I_EditEntityAreaOneData).GetDataList().FirstOrDefault();

            }
            else if (EditArea is I_EditEntityAreaMultipleData)
            {
                data = (EditArea as I_EditEntityAreaMultipleData).GetSelectedData().FirstOrDefault();
            }
            if (data != null)
            {
                var result = codeFunctionHandler.GetCodeFunctionResult(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), EntityCommandDTO.CodeFunctionID, data);
                if (result.ExecutionException != null)
                {
                    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("خطا", result.ExecutionException.Message, Temp.InfoColor.Red);
                }
            }
            else
            {
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("داده ای انتخاب نشده است");
            }
        }

        //public EntityCommand(CommandAttributes commandAttributes)
        //{
        //    CommandAttributes = commandAttributes;
        //    Title = CommandAttributes.EntityCommandDTO.Title;
        //}
        //public List<DataMode> CompatibaleDataMode
        //{
        //    get
        //    {
        //        return new List<DataMode>();
        //    }
        //}

        //public List<IntracionMode> CompatibaleIntractionMode
        //{
        //    get
        //    {
        //        return new List<IntracionMode>();
        //    }
        //}

        //public event EventHandler EnabledChanged;

        //public void OnEnabledChanged()
        //{
        //    if (EnabledChanged != null)
        //        EnabledChanged(this, null);
        //}

        public void Execute(I_EditEntityArea editArea)
        {


        }
    }
}
