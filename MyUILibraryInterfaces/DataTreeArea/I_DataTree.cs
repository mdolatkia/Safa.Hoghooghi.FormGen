using ModelEntites;
using MyUILibraryInterfaces.ContextMenu;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibraryInterfaces.DataTreeArea
{
    public interface I_ViewDataTreeItem
    {
        event EventHandler Expanded;
        //   object ViewItem { set; get; }
        bool IsSelected { set; get; }
        //   event EventHandler ContextMenuLoaded;
        //   void ClearContextMenu();
        //   bool ContextMenuVisiblie { set; get; }
        // I_ViewContextMenu AddContextMenu(string title);
        I_ViewDataTreeItem AddNode(string title, bool selectable, bool tempChild);
        void ClearChildNodes();
    }
    public interface I_ViewContextMenu
    {
        event EventHandler Cliecked;
    }
    public class DataTreeItem
    {
        public DataTreeItem()
        {
            ChildNodes = new List<DataTreeArea.DataTreeItem>();
        }
        public bool ChildSearched { set; get; }
        public DataTreeItemType ItemType { set; get; }
        public I_ViewDataTreeItem ViewItem { set; get; }
        public DP_DataView DataItem { set; get; }
        public EntityRelationshipTailDTO Relation { set; get; }
        public List<DataTreeItem> ChildNodes { set; get; }
    }
    public interface I_DataTreeView
    {
        event EventHandler DataAreaConfirmed;
        I_ViewDataTreeItem AddRootNode(string title, bool selectable, bool tempChild);
        event EventHandler<ContextMenuArg> ContextMenuLoaded;

    }
    public interface I_DataTreeArea
    {
        I_DataTreeView View { set; get; }
        event EventHandler DataAreaConfirmed;
        event EventHandler<ContextMenuArg> ContextMenuLoaded;

        void SetAreaInitializer(DataTreeAreaInitializer initParam);
        List<DP_DataView> GetSelectedDataItems(List<DataTreeItem> dataTreeItems = null, List<DP_DataView> result = null);
        void Select(DP_DataView dataInstance);
        void SelectAll();
    }

    public class DataTreeAreaInitializer
    {
        public string Title { set; get; }
        public DP_DataView FirstDataItem { set; get; }
        public List<EntityRelationshipTailDTO> RelationshipTails { set; get; }
        public int EntitiyID { set; get; }

        public bool RelationshipTailsLoaded { set; get; }
        public bool ExpandableMore { set; get; }

    }
    public enum DataTreeItemType
    {
        DataItem,
        Relation
    }
    //public class ContextMenuEventArg : EventArgs
    //{
    //    public ContextMenuEventArg()
    //    {
    //        ContextMenus = new List<DataTreeArea.DynamicContextMenu>();
    //    }
    //    public DataTreeItem DataTreeItem { set; get; }
    //    public List<DynamicContextMenu> ContextMenus { set; get; }
    //}
    //public class DynamicContextMenu
    //{
    //    public string Title { set; get; }
    //    public event EventHandler Clicked;

    //    public void OnClicked()
    //    {
    //        if (Clicked != null)
    //            Clicked(this, null);
    //    }
    //}

}
