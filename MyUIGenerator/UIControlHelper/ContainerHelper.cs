

using ModelEntites;
using MyUILibrary;
using MyUILibrary.EntityArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace MyUIGenerator.UIControlHelper
{

    public static class ContainerHelper 
    {
        //////public static UIContainerPackage GenerateUIContainerPackage(string title, I_View_GridContainer container)
        //////{
        //////    //////Expander panel = new Expander();
        //////    //////panel.Header = title;
        //////    //////panel.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray);
        //////    //////panel.Content = container;
        //////    //////UIContainerPackage package = new UIContainerPackage();
        //////    //////package.UIControl = panel;
        //////    //////package.Container = container;
        //////    //////return package;
        //////    return null; 
        //////}
    }
    //    public UIElement UI { get { return Grid; } }
       

    //}

    //    internal static GridSetting GetGridSetting(UIControlPackage parentUIControlPackage)
    //    {
    //        var uiControl = parentUIControlPackage.UIControl;
    //        if (uiControl.Control is GroupBox)
    //        {
    //        }
    //    }
    //    internal static UIControlPackage GenerateControlPackage(object view, ColumnSetting columnSetting)
    //    {
    //        UIControlPackage package = new UIControlPackage();
    //        //package.UIControls = new List<UIControl>();

    //        UIControl control = new UIControl();
    //        control.UIControlSetting = columnSetting.UISetting;
    //        ////control.UIControlSetting = new UIControlSetting();
    //        ////if (columnSetting.aG_EnumViewControlInsertionMode == AG_EnumViewControlInsertionMode.NewLine)
    //        ////    control.UIControlSetting.DesieredColumns = ColumnWidth.Full;
    //        ////else
    //        ////    control.UIControlSetting.DesieredColumns = ColumnWidth.Normal;
    //        //control.UIControlSetting.DesieredRows = 1;
    //        control.Control = view;
    //        package.UIControl=control;
    //        return package;
    //    }
    //    //internal static UIControlSetting GenerateUISetting(ColumnDTO nD_Type_Property, UISetting.DataPackageUISetting.UI_PackagePropertySetting uI_PackagePropertySetting)
    //    //{
    //    //    throw new NotImplementedException();
    //    //}


    //    //public static UIControl GetUIControl(UIControlPackage aG_UIControlPackage)
    //    //{
    //    //    return aG_UIControlPackage.UIControls.First();
    //    //}


    //    //internal static object AddControl(UIControlPackage parentUIControlPackage, UIControlPackage controlPackage)
    //    //{
    //    //    var uiControl = parentUIControlPackage.UIControl;
    //    //    if(uiControl.Control is GroupBox)
    //    //    {
    //    //        GroupPanelHelper.Add
    //    //    }
    //    //}


    //}
}
