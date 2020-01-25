using MyUILibrary.Temp;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary
{
    //public class ColumnSetting
    //{
    //    public ColumnSetting()
    //    {

    //    }
    //    public bool LabelForReadOnlyText { set; get; }

    //    //public AG_EnumViewControlInsertionMode aG_EnumViewControlInsertionMode { set; get; }
    //    public bool GridView { set; get; }

    //    //public UIControlSetting UISetting { set; get; }
    //    public bool IsDisabled { get; set;}
    //}

    //public enum AG_EnumViewControlInsertionMode
    //{
    //    Normal,
    //    NewLine,
    //}

    //public enum AG_ControlRelationType
    //{
    //    Label

    //}

    //public class AG_RelatedConttol
    //{
    //    public AG_ControlRelationType RelationType { set; get; }
    //    public UIControl RelatedUIControl { set; get; }


    //}

    public class ConrolPackageMenu
    {
        public string Name { set; get; }
        public string Title { set; get; }
        public string Tooltip { set; get; }
        public void OnMenuClicked(object sender, ConrolPackageMenuArg e)
        {
            if (MenuClicked != null) MenuClicked(sender, e);
        }

        public event EventHandler<ConrolPackageMenuArg> MenuClicked;

        ////public object Menu { set; get; }
    }
    public class ConrolPackageMenuArg : EventArgs
    {
        public object data { set; get; }
    }

    //public enum ColumnWidth
    //{
    //    Normal = 0,
    //    Half,
    //    Full
    //}
}
