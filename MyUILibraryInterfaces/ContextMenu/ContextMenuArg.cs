using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibraryInterfaces.ContextMenu
{
    public interface I_ContextMenuManager
    {
        void SetMenuItems( List<ContextMenuItem> items);
    }

    public class ContextMenuArg
    {
        public object ContextObject { set; get; }
        public I_ContextMenuManager ContextMenuManager { set; get; }
    }
    public class ContextMenuItem
    {
        public event EventHandler Clicked;
        public string Title { set; get; }
        public string ImagePath { set; get; }
        public MenuItemType ItemType { set; get; }
        public List<ContextMenuItem> SubMenus { set; get; }

        public void OnClicked()
        {
            if (Clicked != null)
                Clicked(this, null);
        }
    }
    public enum MenuItemType
    {
        Menu,
        Folder
    }
}
