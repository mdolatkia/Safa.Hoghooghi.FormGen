using MyUILibraryInterfaces.ContextMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;
using System.Windows.Controls;

namespace MyUIGenerator
{
    public class ContextMenuManager : I_ContextMenuManager
    {
        RadContextMenu radContextMenu;
        public ContextMenuManager(RadContextMenu contextMenu)
        {
            radContextMenu = contextMenu;
        }
        public void SetMenuItems(List<ContextMenuItem> items)
        {
            radContextMenu.Items.Clear();
            AddMenu(radContextMenu.Items, items);
        }

        private void AddMenu(ItemCollection items1, List<ContextMenuItem> menuItems)
        {
            if (menuItems == null)
                return;
            foreach (var item in menuItems)
            {
                RadMenuItem menu = new RadMenuItem();
                menu.Header = item.Title;
                menu.Click += (sender, e) => Menu_Click(sender, e, item);
                items1.Add(menu);
                if (item.ItemType == MenuItemType.Folder)
                    AddMenu(menu.Items, item.SubMenus);
            }
        }

        private void Menu_Click(object sender, Telerik.Windows.RadRoutedEventArgs e, ContextMenuItem contextMenuItem)
        {
            contextMenuItem.OnClicked();
        }
    }
}
