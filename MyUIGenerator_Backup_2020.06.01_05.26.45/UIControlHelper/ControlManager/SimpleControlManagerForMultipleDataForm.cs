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

    public class SimpleControlManagerForMultipleDataForm : BaseControlManager, I_SimpleControlManager
    {
        //private List<BaseMessageItem> ValidationItems = new List<BaseMessageItem>();

        private List<DataMessageItem> MessageItems = new List<DataMessageItem>();
        public DataGridTextColumn DataGridColumn;
        public SimpleControlManagerForMultipleDataForm(ColumnDTO column, ColumnUISettingDTO columnUISettingDTO, bool hasRangeOfValues) : base()
        {
            //  RelatedControl = new List<FrameworkElement>();
            DataGridColumn = new DataGridTextColumn(column, columnUISettingDTO, hasRangeOfValues);
        }

        //   public List<FrameworkElement> RelatedControl { set; get; }


        public void EnableDisable(bool enable)
        {
            DataGridColumn.EnableDisable(enable);
        }
        public void EnableDisable(object dataItem, bool enable)
        {
            DataGridColumn.EnableDisable(dataItem, enable);
        }
        public void Visiblity(bool visible)
        {

            DataGridColumn.Visiblity(visible);
        }
        public void Visiblity(object dataItem, bool visible)
        {

            DataGridColumn.Visiblity(dataItem, visible);
        }
        public bool IsVisible
        {
            get
            {
                return DataGridColumn.IsVisible;
            }


        }
        public object GetValue(object dataItem)
        {
            return DataGridColumn.GetValue(dataItem);
        }

        public void SetReadonly(bool isreadonly)
        {
            DataGridColumn.SetReadonly(isreadonly);
        }
        public object GetUIControl(object dataItem)
        {
            return DataGridColumn.GetUIControl(dataItem);
        }
        public void SetReadonly(object dataItem, bool isreadonly)
        {
            DataGridColumn.SetReadonly(dataItem, isreadonly);
        }


        //public void AddMessage(BaseMessageItem item)
        //{
        //    if (item.CausingDataItem != null || item.IsPermanentMessage == true)
        //    {
        //        MessageItems.Add(item);
        //        if (item.IsPermanentMessage == true)
        //        {
        //            SetPermanentTooltip();
        //            SetPermanentColor();
        //        }
        //        else
        //        {
        //            SetDataTooltip(item.CausingDataItem);
        //            SetDataColor(item.CausingDataItem);
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("asdasd");
        //    }
        //}

        //public void RemoveMessage(BaseMessageItem baseMessageItemem)
        //{
        //    if (baseMessageItemem.CausingDataItem != null || baseMessageItemem.IsPermanentMessage == true)
        //    {
        //        if (baseMessageItemem.IsPermanentMessage)
        //        {
        //            foreach (var item in MessageItems.Where(x => x.IsPermanentMessage && x.Key == baseMessageItemem.Key).ToList())
        //                MessageItems.Remove(item);
        //            SetPermanentTooltip();
        //            SetPermanentColor();
        //        }
        //        else
        //        {
        //            foreach (var item in MessageItems.Where(x => x.CausingDataItem == baseMessageItemem.CausingDataItem && x.Key == baseMessageItemem.Key).ToList())
        //                MessageItems.Remove(item);
        //            SetDataTooltip(baseMessageItemem.CausingDataItem);
        //            SetDataColor(baseMessageItemem.CausingDataItem);
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("asdasd");
        //    }
        //}



        //private void SetPermanentTooltip()
        //{
        //    var tooltip = "";
        //    foreach (var item in MessageItems.Where(x => x.IsPermanentMessage))
        //        tooltip += (tooltip == "" ? "" : Environment.NewLine) + item.Message;
        //    if (tooltip != "")
        //        DataGridColumn.SetTooltip(tooltip);
        //    else
        //        DataGridColumn.ClearTooltip();
        //}
        //private void SetDataTooltip(object dataItem)
        //{
        //    var tooltip = "";
        //    foreach (var item in MessageItems.Where(x => !x.IsPermanentMessage))
        //        tooltip += (tooltip == "" ? "" : Environment.NewLine) + item.Message;
        //    if (tooltip != "")
        //        DataGridColumn.SetTooltip(dataItem, tooltip);
        //    else
        //        DataGridColumn.ClearTooltip(dataItem);
        //}
        //private void SetPermanentColor()
        //{
        //    var color = InfoColor.Default;
        //    if (MessageItems.Any(x => x.IsPermanentMessage && x.Color != InfoColor.Default))
        //        color = MessageItems.First(x => x.Color != InfoColor.Default).Color;

        //    if (color != InfoColor.Default)
        //        DataGridColumn.SetColor(color);
        //    else
        //        DataGridColumn.ClearColor();
        //}
        //private void SetDataColor(object dataItem)
        //{
        //    var color = InfoColor.Default;
        //    if (MessageItems.Any(x => !x.IsPermanentMessage && x.Color != InfoColor.Default))
        //        color = MessageItems.First(x => x.Color != InfoColor.Default).Color;

        //    if (color != InfoColor.Default)
        //        DataGridColumn.SetColor(dataItem, color);
        //    else
        //        DataGridColumn.ClearColor(dataItem);
        //}
        public bool SetValue(object dataItem, object value)
        {
            var dataRow = DataGridColumn.DataControl.GetRowForItem(dataItem);
            return DataGridColumn.SetValue(dataItem, value);
        }



        public void AddButtonMenu(ConrolPackageMenu menu)
        {
            DataGridColumn.AddButtonMenu(menu);
        }

        public void AddButtonMenu(object dataItem, ConrolPackageMenu menu)
        {
            DataGridColumn.AddButtonMenu(menu, dataItem);
        }

        public void RemoveButtonMenu(string name)
        {
            DataGridColumn.RemoveButtonMenu(name);
        }

        public void RemoveButtonMenu(object dataItem, string name)
        {
            DataGridColumn.RemoveButtonMenu(name, dataItem);
        }

        public void SetBinding(object dataItem, EntityInstanceProperty property)
        {
            DataGridColumn.SetBinding(dataItem, property);
        }


        //public void ClearAllValidations()
        //{
        //    List<object> dataItems = new List<object>();
        //    //var validationMessage = cellValidationMessages.Where(x => x.CausingDataItem == dataItem).ToList();
        //    foreach (var item in cellValidationMessages)
        //    {
        //        dataItems.Add(item.CausingDataItem);
        //    }
        //    cellValidationMessages.Clear();
        //    foreach (var item in dataItems)
        //    {
        //        SetTooltip(item);
        //        SetColor(item);
        //    }
        //}

        public void SetColumnValueRange(List<ColumnValueRangeDetailsDTO> details)
        {
            DataGridColumn.SetColumnValueRange(details);
        }

        public void SetColumnValueRange(List<ColumnValueRangeDetailsDTO> details, object data)
        {
            DataGridColumn.SetColumnValueRange(details, data);
        }

        public CommonOperator GetOperator()
        {
            throw new NotImplementedException();
        }

        public bool HasOperator()
        {
            throw new NotImplementedException();
        }

        public bool SetOperator(CommonOperator searchOperator)
        {
            throw new NotImplementedException();
        }

        public void SetTooltip(object dataItem, string tooltip)
        {
            DataGridColumn.SetTooltip(dataItem, tooltip);
        }

        public void SetBorderColor(object dataItem, InfoColor color)
        {
            DataGridColumn.SetBorderColor(dataItem, color);
        }

        public void SetBackgroundColor(object dataItem, InfoColor color)
        {
            DataGridColumn.SetBackgroundColor(dataItem, color);
        }

        public void SetForegroundColor(object dataItem, InfoColor color)
        {
            DataGridColumn.SetForegroundColor(dataItem, color);
        }


        //public void SetMandatoryState(bool isMandatory)
        //{
        //    foreach (var item in RelatedControl)
        //    {

        //        if (item is TextBlock)
        //        {
        //            var textblock = (item as TextBlock);

        //            if (isMandatory)
        //            {
        //                if (!textblock.Text.StartsWith("*"))
        //                {
        //                    textblock.Text = "*" + textblock.Text;
        //                }
        //                textblock.Foreground = new SolidColorBrush(Colors.DarkRed);
        //            }
        //            else
        //            {
        //                if (textblock.Text.StartsWith("*"))
        //                {
        //                    textblock.Text = textblock.Text.Substring(1, textblock.Text.Length - 1);
        //                }
        //                textblock.Foreground = new SolidColorBrush(UIManager.GetColorFromInfoColor(InfoColor.Default));
        //            }
        //        }
        //    }
        //}
    }



}
