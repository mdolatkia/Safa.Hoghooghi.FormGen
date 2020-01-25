using ModelEntites;
using MyUILibraryInterfaces.ContextMenu;
using MyUILibraryInterfaces.DataTreeArea;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary.DataTreeArea
{
    public class DataTreeArea : I_DataTreeArea
    {
        public I_DataTreeView View
        {
            set; get;
        }
        DataTreeAreaInitializer AreaInitializer { set; get; }
        public List<DataTreeItem> DataTreeItems = new List<DataTreeItem>();

        public event EventHandler DataAreaConfirmed;
        public event EventHandler<ContextMenuArg> ContextMenuLoaded;

        public void SetAreaInitializer(DataTreeAreaInitializer initParam)
        {
            View = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetViewOfDataTree();
            View.DataAreaConfirmed += View_DataAreaConfirmed;
            View.ContextMenuLoaded += View_ContextMenuLoaded;
            AreaInitializer = initParam;
            if (AreaInitializer.FirstDataItem != null)
            {
                DataTreeItem item = ToDataItemNode(null, AreaInitializer.FirstDataItem, true, false);
                foreach (var rItem in AreaInitializer.RelationshipTails)
                {
                    DataTreeItem citem = ToRelationItemNode(item, item.DataItem, rItem, false, !initParam.RelationshipTailsLoaded);
                    item.ChildNodes.Add(citem);

                    if (initParam.RelationshipTailsLoaded)
                    {
                        SetRelationsDataItem(citem);
                    }
                    else
                    {
                        citem.ViewItem.Expanded += (sender, e) => ViewItem_Expanded(sender, e, citem);
                    }
                }
                item.ChildSearched = true;
            }

        }

        private void View_ContextMenuLoaded(object sender, ContextMenuArg e)
        {
            if (e.ContextObject is I_ViewDataTreeItem)
            {
                var dataTreeItem = DataTreeItems.First(x => x.ViewItem == (e.ContextObject as I_ViewDataTreeItem));
                if (dataTreeItem.ItemType == DataTreeItemType.DataItem)
                {
                    if (ContextMenuLoaded != null)
                    {
                        e.ContextObject = dataTreeItem.DataItem;
                        ContextMenuLoaded(this, e);
                    }
                }
                else
                    e.ContextMenuManager.SetMenuItems(new List<ContextMenuItem>());
            }
            if (ContextMenuLoaded != null)
            {
                //var arg = new ContextMenuEventArg() { DataTreeItem = item };
                //ContextMenuLoaded(this, arg);
                //(sender as I_ViewDataTreeItem).ClearContextMenu();
                //if (arg.ContextMenus.Any())
                //{
                //    (sender as I_ViewDataTreeItem).ContextMenuVisiblie = true;
                //    foreach (var menu in arg.ContextMenus)
                //    {
                //        var viewMenu = (sender as I_ViewDataTreeItem).AddContextMenu(menu.Title);
                //        viewMenu.Cliecked += (sender1, e1) => ViewMenu_Cliecked(sender1, e1, menu);
                //    }
                //}
                //else
                //    (sender as I_ViewDataTreeItem).ContextMenuVisiblie = false;


            }
        }

        private void View_DataAreaConfirmed(object sender, EventArgs e)
        {
            if (DataAreaConfirmed != null)
                DataAreaConfirmed(this, e);
        }

        private void SetRelationsDataItem(DataTreeItem citem)
        {
            var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();
            var searchDataTuple = AgentUICoreMediator.GetAgentUICoreMediator.RelationshipTailDataManager.GetTargetSearchItemFromRelationshipTail(citem.DataItem, citem.Relation);
            DR_SearchViewRequest request = new DR_SearchViewRequest(requester, searchDataTuple);
            var searchResult = AgentUICoreMediator.GetAgentUICoreMediator.requestRegistration.SendSearchViewRequest(request);
            citem.ViewItem.ClearChildNodes();
            foreach (var relatedItem in searchResult.ResultDataItems)
            {
                var relatedkeyColumns = relatedItem.KeyProperties;
                citem.ChildNodes.Add(ToDataItemNode(citem, relatedItem, true, AreaInitializer.ExpandableMore));
            }
            citem.ChildSearched = true;
        }
        private void ViewItem_Expanded(object sender, EventArgs e, DataTreeItem citem)
        {
            if (!citem.ChildSearched)
                SetRelationsDataItem(citem);
        }
        private DataTreeItem ToDataItemNode(DataTreeItem parentItem, DP_DataView relatedItem, bool selectable, bool tempChild)
        {
            DataTreeItem item = new DataTreeItem();
            item.DataItem = relatedItem;
            item.ItemType = DataTreeItemType.DataItem;
            if (parentItem == null)
                item.ViewItem = View.AddRootNode(item.DataItem.ViewInfo, selectable, tempChild);
            else
                item.ViewItem = parentItem.ViewItem.AddNode(item.DataItem.ViewInfo, selectable, tempChild);

            DataTreeItems.Add(item);
            return item;
        }


        //private void ViewMenu_Cliecked(object sender, EventArgs e, DynamicContextMenu menu)
        //{
        //    menu.OnClicked();
        //}
        private DataTreeItem ToRelationItemNode(DataTreeItem parentItem, DP_DataView relatedItem, EntityRelationshipTailDTO relation, bool selectable, bool tempChild)
        {
            DataTreeItem item = new DataTreeItem();
            item.DataItem = relatedItem;
            item.Relation = relation;
            item.ItemType = DataTreeItemType.Relation;
            item.ViewItem = parentItem.ViewItem.AddNode(relation.EntityPath, selectable, tempChild);
            //item.ViewItem.ContextMenuLoaded += (sender, e) => ViewItem_ContextMenuLoaded(sender, e, item);
            DataTreeItems.Add(item);
            return item;
        }
        public List<DP_DataView> GetSelectedDataItems(List<DataTreeItem> dataTreeItems = null, List<DP_DataView> result = null)
        {
            if (result == null)
                result = new List<DP_DataView>();
            if (dataTreeItems == null)
                dataTreeItems = DataTreeItems;
            foreach (var item in dataTreeItems)
            {
                if (item.ItemType == DataTreeItemType.DataItem && item.ViewItem.IsSelected)
                    result.Add(item.DataItem);
                //   GetSelectedDataItems(item.ChildNodes, result);
            }
            return result;
        }

        public void Select(DP_DataView dataInstance)
        {
            DataTreeItem item = GetDataTreeItem(dataInstance);
            if (item != null)
                item.ViewItem.IsSelected = true;
        }

        private DataTreeItem GetDataTreeItem(DP_DataView dataInstance)
        {
            foreach (var item in DataTreeItems)
                if (item.ItemType == DataTreeItemType.DataItem && item.DataItem == dataInstance)
                    return item;
            //درست شود بصورت درختی جستجو کند
            return null;
        }

        public void SelectAll()
        {
            SelectItems(DataTreeItems);
        }

        private void SelectItems(List<DataTreeItem> dataTreeItems)
        {
            foreach (var item in dataTreeItems)
            {
                if (item.ItemType == DataTreeItemType.DataItem)
                {
                    item.ViewItem.IsSelected = true;
                    //   SelectItems(item.ChildNodes);
                }
            }
        }
    }
}
