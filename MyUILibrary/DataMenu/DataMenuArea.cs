using MyUILibrary.DataMenuArea;
using MyUILibraryInterfaces.DataMenuArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyUILibrary.EntityArea;
using ProxyLibrary;
using ModelEntites;
using MyRelationshipDataManager;

using MyModelManager;

namespace MyUILibrary.DataMenuArea
{
    public class DataMenuArea : I_DataMenuArea
    {
        DataMenuAreaInitializer AreaInitializer { set; get; }
        public DataMenuArea(DataMenuAreaInitializer initializer)
        {
            AreaInitializer = initializer;
        }
        List<DataMenuUI> Menus = null;
        private List<DataMenuUI> GetDataMenus()
        {
            if (Menus == null)
            {
                var Menus = new List<DataMenuUI>();
                var dataMenus = AgentUICoreMediator.GetAgentUICoreMediator.DataMenuManager.GetDataMenu(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), AreaInitializer.DataItem);
                AddUIMenu(Menus, dataMenus);
                return Menus;
            }
            else
                return Menus;
        }
        private void AddUIMenu(List<DataMenuUI> menus, List<DataMenu> dataMenus)
        {
            foreach (var item in dataMenus)
            {
                DataMenuUI menuUI = new DataMenuUI();
                menuUI.DataMenu = item;
                menuUI.Title = item.Title;
                menuUI.Tooltip = item.Tooltip;
                menus.Add(menuUI);
                menuUI.MenuClicked += MenuUI_MenuClicked;
                AddUIMenu(menuUI.SubMenus, item.SubMenus);
            }
        }
        internal void ShowMenu()
        {
            if (AreaInitializer.DataItem == null)
                return;
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowDataViewItemMenus(GetDataMenus(), AreaInitializer.SourceView);
        }
        private void MenuUI_MenuClicked(object sender, EventArgs e)
        {
            var dataMenuUI = sender as DataMenuUI;
            if (dataMenuUI != null)
            {
                var dataMenu = dataMenuUI.DataMenu;
                if (dataMenuUI.DataMenu.Type == DataMenuType.Archive)
                    AgentUICoreMediator.GetAgentUICoreMediator.ShowArchiveArea(AreaInitializer.DataItem.TargetEntityID, "آرشیو", true, AreaInitializer.DataItem);
                else if (dataMenuUI.DataMenu.Type == DataMenuType.RelationshipTailDataGrid)
                {
                    var searchDataTuple = AgentUICoreMediator.GetAgentUICoreMediator.RelationshipTailDataManager.GetTargetSearchItemFromRelationshipTail(AreaInitializer.DataItem, dataMenu.GridviewRelationshipTail);
                    AgentUICoreMediator.GetAgentUICoreMediator.ShowDataViewGridViewArea(dataMenu.GridviewRelationshipTail.TargetEntityID, dataMenu.GridviewRelationshipTail.TargetEntityAlias, true, false, false, searchDataTuple, true, 0, AreaInitializer.HostDataViewArea, AreaInitializer.HostDataViewItem);
                }
                else if (dataMenuUI.DataMenu.Type == DataMenuType.DataLink)
                {
                    AgentUICoreMediator.GetAgentUICoreMediator.ShowDataLinkArea(AreaInitializer.DataItem.TargetEntityID, dataMenu.Datalink.ID, true, dataMenu.Datalink.Name, AreaInitializer.DataItem);
                }
                else if (dataMenuUI.DataMenu.Type == DataMenuType.RelationshipTailDataView)
                {
                    var searchDataTuple = AgentUICoreMediator.GetAgentUICoreMediator.RelationshipTailDataManager.GetTargetSearchItemFromRelationshipTail(AreaInitializer.DataItem, dataMenu.DataviewRelationshipTail);
                    AgentUICoreMediator.GetAgentUICoreMediator.ShowDataViewGridViewArea(dataMenu.GridviewRelationshipTail.TargetEntityID, dataMenu.GridviewRelationshipTail.TargetEntityAlias, true, false, true, searchDataTuple, true, 0, AreaInitializer.HostDataViewArea, AreaInitializer.HostDataViewItem);
                }
                else if (dataMenuUI.DataMenu.Type == DataMenuType.DirectReport)
                {
                    AgentUICoreMediator.GetAgentUICoreMediator.ShowDirectReport(dataMenu.DirectReport.ID, true, AreaInitializer.DataItem);
                }
                else if (dataMenuUI.DataMenu.Type == DataMenuType.ViewRel)
                {
                    var menuInitializer = new DataMenuAreaInitializer();
                    menuInitializer.HostDataViewArea = AreaInitializer.HostDataViewArea;
                    menuInitializer.HostDataViewItem = AreaInitializer.HostDataViewItem;
                    menuInitializer.SourceView = AreaInitializer.SourceView;
                    menuInitializer.DataItem = dataMenu.ViewRelTargetDataItem;
                    AgentUICoreMediator.GetAgentUICoreMediator.ShowMenuArea(menuInitializer);
                }
                else if (dataMenuUI.DataMenu.Type == DataMenuType.Form)
                {
                    AgentUICoreMediator.GetAgentUICoreMediator.ShowEditEntityArea(AreaInitializer.DataItem.TargetEntityID, true, CommonDefinitions.UISettings.DataMode.None, new List<DP_DataView>() { AreaInitializer.DataItem });
                }
                else if (dataMenuUI.DataMenu.Type == DataMenuType.Letter)
                {
                    AgentUICoreMediator.GetAgentUICoreMediator.ShowLetterArea(AreaInitializer.DataItem.TargetEntityID, "نامه ها", true, AreaInitializer.DataItem);
                }
                else if (dataMenuUI.DataMenu.Type == DataMenuType.RelationshipTailSearchableReport)
                {
                    var searchItem = AgentUICoreMediator.GetAgentUICoreMediator.RelationshipTailDataManager.GetTargetSearchItemFromRelationshipTail(AreaInitializer.DataItem, dataMenu.ReportRelationshipTail.RelationshipTail);
                    AgentUICoreMediator.GetAgentUICoreMediator.ShowSearchableReportArea(dataMenu.ReportRelationshipTail.EntityReport, true, searchItem, false, true, AreaInitializer.HostDataViewArea, AreaInitializer.HostDataViewItem);
                }
                else if (dataMenuUI.DataMenu.Type == DataMenuType.Workflow)
                {
                    var initializer = new WorkflowArea.WorkflowReportAreaInitializer();
                    initializer.DataItem = AreaInitializer.DataItem;
                    AgentUICoreMediator.GetAgentUICoreMediator.ShowWorkflowReportArea(initializer, "گزارش جریان کار", true);
                }
            }
        }




        //private DataMenu GetOrCreateMenu1(List<DataMenu> collection, string key)
        //{
        //    var fITem = collection.FirstOrDefault(x => x.Title == key);
        //    if (fITem == null)
        //    {
        //        fITem = new DataMenu();
        //        fITem.Type = DataMenuType.Folder;
        //        fITem.Title = key;
        //        collection.Add(fITem);
        //        return fITem;
        //    }
        //    else
        //        return fITem;
        //}

    }
}
