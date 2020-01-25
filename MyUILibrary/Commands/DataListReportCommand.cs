using CommonDefinitions.UISettings;

using MyUILibrary;
using MyUILibrary.DataReportArea;
using MyUILibraryInterfaces.DataReportArea;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.EntityArea.Commands
{
    class DataListReportCommand : BaseCommand
    {
        I_EditEntityArea EditArea { set; get; }
        public DataListReportCommand(I_EditEntityArea editArea) : base()
        {
            EditArea = editArea;
            CommandManager.SetTitle("گزارش لیست");
            CommandManager.ImagePath = "Images//grid.png";
            CommandManager.Clicked += CommandManager_Clicked;
        }

        private void CommandManager_Clicked(object sender, EventArgs e)
        {
            if (EditArea.SearchViewEntityArea != null)
            {
                var searchDP = EditArea.SearchViewEntityArea.SearchEntityArea.LastSearch;
                if (searchDP != null)
                {
                    //if (EditArea.DataViewArea == null)
                    //{
                    DP_SearchRepository searchRepository = new DP_SearchRepository(EditArea.AreaInitializer.EntityID);
                    searchRepository.Phrases = searchDP.Phrases;
                    searchRepository.AndOrType = searchDP.AndOrType;
                    EditArea.DataListReportAreaContainer = new DataListReportAreaContainer();
                    var initializer = new DataListReportAreaContainerInitializer();
                    initializer.EntitiyID = EditArea.AreaInitializer.EntityID;
                    initializer.Title = EditArea.SimpleEntity.Alias;
                    initializer.SearchRepository = searchRepository;
                    EditArea.DataListReportAreaContainer.SetAreaInitializer(initializer);
                    //}
                    //else
                    //{

                    //}

                    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(EditArea.DataListReportAreaContainer.View, EditArea.SimpleEntity.Alias, Enum_WindowSize.Maximized);
                }
            }
        }


    }
}
