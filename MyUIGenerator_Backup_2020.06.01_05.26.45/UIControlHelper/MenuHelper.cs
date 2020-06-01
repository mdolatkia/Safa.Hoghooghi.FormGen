
using MyUILibrary;
using MyUILibrary.EntityArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace MyUIGenerator.UIControlHelper
{
    public class MenuHelper
    {
        internal void GenerateMenu(FrameworkElement element, ConrolPackageMenu cpMenu)
        {
            RadContextMenu contextMenu = RadContextMenu.GetContextMenu(element);
            if (contextMenu == null)
            {
                contextMenu = new RadContextMenu();
                RadContextMenu.SetContextMenu(element, contextMenu);
            }

            var menuItem = new RadMenuItem();
            menuItem.Name = cpMenu.Name;
            menuItem.Header = cpMenu.Title;
            menuItem.Click += (sender, e) => MenuItem_Click(sender, e, cpMenu);
            contextMenu.Items.Add(menuItem);

        }

        private void MenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e, ConrolPackageMenu cpMenu)
        {
            if (cpMenu != null)
            {
                ConrolPackageMenuArg arg = new ConrolPackageMenuArg();
                cpMenu.OnMenuClicked(sender, arg);
            }
        }
    }
}
