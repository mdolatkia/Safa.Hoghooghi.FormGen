using ModelEntites;
using MyUILibrary;
using MyUILibrary.EntityArea;
using MyUILibrary.Temp;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyUIGenerator.UIControlHelper
{
    public class SimpleControlManagerForOneDataForm : BaseControlManager, I_SimpleControlManager
    {
        //public string ValidationMessage { set; get; }
        //public InfoColor ValidationColor { set; get; }
        //  private List<BaseMessageItem> MessageItems = new List<BaseMessageItem>();
        public ColumnDTO Column { set; get; }
        public ColumnUISettingDTO ColumnUISettingDTO { set; get; }
        MenuHelper MenuHelper = new MenuHelper();
        public SimpleControlManagerForOneDataForm(ColumnDTO column, ColumnUISettingDTO columnSetting, bool hasRangeOfValues, List<SimpleSearchOperator> operators = null) : base()
        {
            Column = column;
            ColumnUISettingDTO = columnSetting;
            //  RelatedControl = new List<FrameworkElement>();
            if (hasRangeOfValues)
                MyControlHelper = ControlHelper.KeyValueControlHelper(column);
            else
                MyControlHelper = ControlHelper.GetControlHelper(column, columnSetting, operators);
        }
        public I_ControlHelper MyControlHelper { set; get; }

        public bool IsVisible
        {
            get
            {
                return MyControlHelper.IsVisible();
            }
        }


        public CommonOperator GetOperator()
        {
            return MyControlHelper.GetOperator();
        }

        public object GetValue(object dataItem)
        {
            return MyControlHelper.GetValue();
        }
        public void SetReadonly(bool isreadonly)
        {
            MyControlHelper.SetReadonly(isreadonly);
        }
        public void SetReadonly(object dataItem, bool isreadonly)
        {
            MyControlHelper.SetReadonly(isreadonly);
        }

        public bool SetValue(object dataItem, object value)
        {
            return MyControlHelper.SetValue(value);
        }
        public bool HasOperator()
        {
            return MyControlHelper.HasOperator();
        }
        public bool SetOperator(CommonOperator searchOperator)
        {
            return MyControlHelper.SetOperator(searchOperator);
        }

        //public void ClearAllValidations()
        //{
        //    ValidationItems.Clear();
        //    SetTooltip();
        //    SetColor();
        //}


        //public void AddMessage(BaseMessageItem item)
        //{
        //    MessageItems.Add(item);
        //    SetTooltip();
        //    SetColor();
        //}
        //public void RemoveMessage(BaseMessageItem baseMessageItemem)
        //{
        //    foreach (var item in MessageItems.Where(x => x.CausingDataItem == baseMessageItemem.CausingDataItem && x.Key == baseMessageItemem.Key).ToList())
        //        MessageItems.Remove(item);
        //    SetTooltip();
        //    SetColor();
        //}
        //public void RemoveMessageByKey(string key)
        //{
        //    foreach (var item in MessageItems.Where(x => x.Key == key).ToList())
        //        MessageItems.Remove(item);
        //    SetTooltip();
        //    SetColor();
        //}

        //private void SetColor()
        //{
        //    var color = InfoColor.Default;
        //    if (MessageItems.Any(x => x.Color != InfoColor.Default))
        //        color = MessageItems.First(x => x.Color != InfoColor.Default).Color;
        //    if (color != InfoColor.Default)
        //        MyControlHelper.SetColor( color);
        //    else
        //        MyControlHelper.ClearColor();
        //}

        //private void SetTooltip()
        //{
        //    var tooltip = "";
        //    foreach (var item in MessageItems)
        //        tooltip += (tooltip == "" ? "" : Environment.NewLine) + item.Message;
        //    if (tooltip != "")
        //        MyControlHelper.SetTooltip( tooltip);
        //    else
        //        MyControlHelper.ClearTooltip();
        //}


        public void AddMenu(ConrolPackageMenu menu)
        {
            MenuHelper.GenerateMenu(MyControlHelper.MainControl, menu);
        }

        public void SetBinding(object dataItem, EntityInstanceProperty property)
        {
            MyControlHelper.SetBinding(property);
        }

        public void AddButtonMenu(ConrolPackageMenu menu)
        {
            MyControlHelper.AddButtonMenu(menu);
        }
        public void AddButtonMenu(object dataItem, ConrolPackageMenu menu)
        {
            MyControlHelper.AddButtonMenu(menu);
        }

        public void RemoveButtonMenu(string name)
        {
            MyControlHelper.RemoveButtonMenu(name);
        }

        public void RemoveButtonMenu(object dataItem, string name)
        {
            MyControlHelper.RemoveButtonMenu(name);
        }
        public void Visiblity(bool visible)
        {
            if (LabelControlManager != null)
                LabelControlManager.Visiblity(visible);
            MyControlHelper.Visiblity(visible);
        }
        public void Visiblity(object dataItem, bool visible)
        {
            //foreach (var item in RelatedControl)
            //{
            //    //بهتر شود
            //    if (item is TextBlock)
            //        LabelHelper.Visiblity(item, visible);
            //}
            if (LabelControlManager != null)
                LabelControlManager.Visiblity(visible);
            MyControlHelper.Visiblity(visible);
        }
        public void EnableDisable(bool enable)
        {
            MyControlHelper.EnableDisable(enable);
        }
        public void EnableDisable(object dataItem, bool enable)
        {
            MyControlHelper.EnableDisable(enable);
        }
        public void SetColumnValueRange(List<ColumnValueRangeDetailsDTO> candidates)
        {
            (MyControlHelper as I_ControlHelperValueRange).SetColumnValueRange(candidates);
        }
        public void SetColumnValueRange(List<ColumnValueRangeDetailsDTO> candidates, object dataItem)
        {
            (MyControlHelper as I_ControlHelperValueRange).SetColumnValueRange(candidates);
        }

        public void SetTooltip(object dataItem, string tooltip)
        {
            if (!string.IsNullOrEmpty(tooltip))
                ToolTipService.SetToolTip(MyControlHelper.WholeControl, tooltip);
            else
                ToolTipService.SetToolTip(MyControlHelper.WholeControl, null);
        }

        public void SetBorderColor(object dataItem, InfoColor color)
        {
            MyControlHelper.SetBorderColor(color);
        }

        public void SetBackgroundColor(object dataItem, InfoColor color)
        {
            MyControlHelper.SetBackgroundColor(color);
        }

        public void SetForegroundColor(object dataItem, InfoColor color)
        {
            MyControlHelper.SetForegroundColor(color);
        }

        public object GetUIControl(object dataItem)
        {
            return MyControlHelper.MainControl;
        }
    }


}
