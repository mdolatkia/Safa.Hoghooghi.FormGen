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

    public class RelationshipControlManagerForMultipleDataForm : BaseControlManager, I_RelationshipControlManager
    {
        //private List<BaseMessageItem> ValidationItems = new List<BaseMessageItem>();
        private List<DataMessageItem> MessageItems = new List<DataMessageItem>();


        public DataGridViewColumn DataGridColumn;

        public I_TabPageContainer TabPageContainer
        {
            set; get;
        }

        public bool HasExpander
        {
            get
            {
                return false;
            }
        }

        public RelationshipControlManagerForMultipleDataForm(TemporaryLinkState temporaryLinkState, RelationshipUISettingDTO relationshipSetting) : base()
        {
            // RelatedControl = new List<FrameworkElement>();
            DataGridColumn = new UIControlHelper.DataGridViewColumn(temporaryLinkState, relationshipSetting);
            DataGridColumn.TemporaryViewRequested += DataGridColumn_TemporaryViewRequested;
            DataGridColumn.TemporaryViewLoaded += DataGridColumn_TemporaryViewLoaded;
            DataGridColumn.TemporaryViewSerchTextChanged += DataGridColumn_TemporaryViewSerchTextChanged1;
            DataGridColumn.FocusLost += DataGridColumn_FocusLost;
        }

        private void DataGridColumn_FocusLost(object sender, EventArgs e)
        {
            if (FocusLost != null)
                FocusLost(sender, e);
        }
        private void DataGridColumn_TemporaryViewSerchTextChanged1(object sender, Arg_TemporaryDisplaySerachText e)
        {
            if (TemporaryViewSerchTextChanged != null)
                TemporaryViewSerchTextChanged(sender, e);
        }


        private void DataGridColumn_TemporaryViewLoaded(object sender, Arg_MultipleTemporaryDisplayLoaded e)
        {
            if (TemporaryViewLoaded != null)
                TemporaryViewLoaded(sender, e);
        }

        private void DataGridColumn_TemporaryViewRequested(object sender, Arg_MultipleTemporaryDisplayViewRequested e)
        {
            if (TemporaryViewRequested != null)
                TemporaryViewRequested(sender, e);
        }
        public void Visiblity(bool visible)
        {
            DataGridColumn.Visiblity(visible);
        }
        public void Visiblity(object dataItem, bool visible)
        {
            DataGridColumn.Visiblity(dataItem, visible);
        }
        //  public List<FrameworkElement> RelatedControl { set; get; }

        public event EventHandler<Arg_MultipleTemporaryDisplayViewRequested> TemporaryViewRequested;
        public event EventHandler<Arg_MultipleTemporaryDisplayLoaded> TemporaryViewLoaded;
        public event EventHandler<Arg_TemporaryDisplaySerachText> TemporaryViewSerchTextChanged;
        public event EventHandler FocusLost;

        public void SetTemporaryViewText(object relatedData, string text)
        {
            DataGridColumn.SetTemporaryViewText(relatedData, text);
        }
        public void SetQuickSearchVisibility(object relatedData, bool visible)
        {
            DataGridColumn.SetQuickSearchVisibility(relatedData, visible);
        }
        public void EnableDisable( bool enable)
        {
            DataGridColumn.EnableDisable( enable);
        }
        public void EnableDisable(object dataItem, bool enable)
        {
            DataGridColumn.EnableDisable(dataItem, enable);
        }
        public void EnableDisable(object dataItem, TemporaryLinkType link, bool enable)
        {
            DataGridColumn.DisableEnable(dataItem, link, enable);
        }
        public I_View_TemporaryView GetTemporaryView(object dataItem)
        {
            return DataGridColumn.GetTemporaryView(dataItem);
        }
        public object GetUIControl(object dataItem)
        {
            return DataGridColumn.GetTemporaryView(dataItem);
        }
        //public void AddValidation(BaseMessageItem item)
        //{
        //    ValidationItems.Add(item);
        //    SetTooltip(item.CausingDataItem);
        //    SetColor(item.CausingDataItem);
        //}

        //public void RemoveValidation(object dataItem, string key)
        //{
        //    foreach (var item in ValidationItems.Where(x => x.CausingDataItem == dataItem && x.Key == key).ToList())
        //        ValidationItems.Remove(item);
        //    SetTooltip(dataItem);
        //    SetColor(dataItem);
        //}
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

        //public bool SetValue(object dataItem, string value)
        //{
        //    var dataRow = DataGridColumn.DataControl.GetRowForItem(dataItem);
        //    return DataGridColumn.SetValue(dataItem, value);
        //}


        //public void ClearAllValidations()
        //{
        //    List<object> dataItems = new List<object>();
        //    foreach (var item in cellValidationMessages)
        //    {
        //        dataItems.Add(item.CausingDataItem);
        //    }
        //    cellValidationMessages.Clear();
        //    foreach (var item in cellValidationMessages)
        //    {
        //        SetTooltip(item.CausingDataItem);
        //        SetColor(item.CausingDataItem);
        //    }
        //}
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
