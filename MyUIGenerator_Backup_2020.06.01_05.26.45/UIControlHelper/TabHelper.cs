
using MyUILibrary;
using MyUILibrary.EntityArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace MyUIGenerator.UIControlHelper
{
    public class TabHelper
    {
        //internal static RadTabControl GenerateTab(string name)
        //{
        //    var tabControl = new RadTabControl();
        //    tabControl.Name = name;
        //    return tabControl;
        //}

        //internal static UIControlSetting GenerateUISetting(ColumnDTO nD_Type_Property, UISetting.DataPackageUISetting.UI_PackagePropertySetting uI_PackagePropertySetting)
        //{
        //    return new UIControlSetting() { DesieredColumns = 1, DesieredRows = 1 };
        //}





        //internal static object AddTab(List<RadTabControl> TabControls, string groupKey, UIControl uiControl, UIControl labelUIControl)
        //{
        //    var keys = groupKey.Split('*');
        //    if (TabControls == null)
        //        TabControls = new List<RadTabControl>();
        //    bool newTabControl = false;
        //    var tabControlName = keys[0];
        //    var tabItemName = keys[1];
        //    var tabControl = TabControls.FirstOrDefault(x => x.Name == tabControlName);
        //    if (tabControl == null)
        //    {
        //        tabControl = new RadTabControl();
        //        tabControl.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE9F0F9"));
        //        tabControl.Name = tabControlName;
        //        TabControls.Add(tabControl);
        //        newTabControl = true;
        //        tabControl.Margin = new System.Windows.Thickness(0, 2, 0, 2);
        //    }
        //    var tabItem = new RadTabItem();
        //    tabItem.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE9F0F9"));
        //    tabItem.Name = tabItemName;
        //    tabItem.Header = labelUIControl.Control;
        //    tabItem.Content = uiControl.Control;
        //    tabControl.Items.Add(tabItem);

        //    if (newTabControl)
        //        return tabControl;
        //    else
        //        return null;
        //}

        //internal static string GetTabKey(string mainName, string itemName)
        //{
        //    return mainName + "*" + itemName;
        //}
    }
}
