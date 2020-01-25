using ModelEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telerik.Windows.Controls;

namespace System
{
    public static class RadExpressionEditorExtensions
    {
        //public static string ToZZZ(this object str)
        //{
        //    return str.ToString();
        //}
    }
}
namespace MyProject_WPF
{

    public class MyProjectHelper
    {
        public static void SetContectMenuVisibility(RadContextMenu contextMenu)
        {
            bool menuExists = false;
            foreach (var item in contextMenu.Items)
            {
                if (!(item is RadMenuSeparatorItem))
                {
                    menuExists = true;
                    break;
                }
            }
            contextMenu.Visibility = (menuExists ? Visibility.Visible : Visibility.Collapsed);

        }
        //public static GetAppMenuObjects()
        //{
        //    ObjectDTO rootObject = new ObjectDTO();
        //    rootObject.ObjectCategory = DatabaseObjectCategory.RootMenu;
        //    rootObject.ObjectIdentity = "RootMenu";
        //    rootObject.Title = "منوها";

        //    ObjectDTO rootObject = new ObjectDTO();
        //    rootObject.ObjectCategory = DatabaseObjectCategory.RootMenu;
        //    rootObject.ObjectIdentity = "RootMenu";
        //    rootObject.Title = "منوها";

        //    rootObject.ChildObjects.Add()
        //}
    }

}
