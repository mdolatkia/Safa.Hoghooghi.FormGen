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
    public class RelationshipControlManagerForOneDataForm : BaseControlManager, I_RelationshipControlManager
    {
        private List<DataMessageItem> MessageItems = new List<DataMessageItem>();
        //List<BaseMessageItem> ValidationItems = new List<BaseMessageItem>();

        public event EventHandler<Arg_MultipleTemporaryDisplayLoaded> TemporaryViewLoaded;
        public event EventHandler<Arg_MultipleTemporaryDisplayViewRequested> TemporaryViewRequested;
        public event EventHandler<Arg_TemporaryDisplaySerachText> TemporaryViewSerchTextChanged;
        public event EventHandler FocusLost;

        //public InfoColor ValidationColor { set; get; }
        public RelationshipUISettingDTO RelationshipUISettingDTO { set; get; }
        public Expander Expander { set; get; }
        public FrameworkElement View { set; get; }
        public RelationshipControlManagerForOneDataForm(FrameworkElement view, RelationshipUISettingDTO relationshipSetting) : base()
        {
            RelationshipUISettingDTO = relationshipSetting;
            //    RelatedControl = new List<FrameworkElement>();
            View = view;
            if (relationshipSetting.Expander == false)
                MainControl = view;
            else
            {
                Expander = new Expander();
                Expander.Margin = new Thickness(5, 5, 5, 5);
                Expander.BorderThickness = new Thickness(1);
                Expander.BorderBrush = new SolidColorBrush(Colors.LightGray);
                Expander.Content = view;
                Expander.IsExpanded = relationshipSetting.IsExpanded;
                MainControl = Expander;
                //   Expander.LostFocus += Expander_LostFocus;
                Expander.MouseLeave += Expander_MouseLeave;
                //  Expander.GotFocus += Expander_GotFocus;
                Expander.MouseEnter += Expander_MouseEnter;
            }
        }

        public bool HasExpander
        {
            get
            {
                return Expander != null;
            }
        }
        bool _mouseEnter = false;
        //   bool _focused = false;
        private void Expander_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //  _mouseEnter = true;
            HighlightExpander();
        }
        //private void Expander_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    _focused = true;
        //    HighlightExpander();
        //}
        private void HighlightExpander()
        {
            Expander.BorderBrush = new SolidColorBrush(Colors.Gray);
        }


        private void Expander_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //   _mouseEnter = false;
            DeHighlightExpander();
        }
        //private void Expander_LostFocus(object sender, RoutedEventArgs e)
        //{
        // //   _focused = false;
        //    DeHighlightExpander();
        //}
        private void DeHighlightExpander()
        {
            //if (_mouseEnter == false && _focused == false)
            Expander.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD3D3D3"));
        }

        public FrameworkElement MainControl { set; get; }

        public I_TabPageContainer TabPageContainer
        {
            set; get;
        }



        //        public List<FrameworkElement> RelatedControl { set; get; }
        //



        public void Visiblity(object dataItem, bool visible)
        {

            if (LabelControlManager != null)
                LabelControlManager.Visiblity(visible);
            View.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public void Visiblity(bool visible)
        {
            //foreach (var item in RelatedControl)
            //{
            //    //بهتر شود
            //    if (item is TextBlock)
            //        LabelHelper.Visiblity(item, visible);
            //}

            if (LabelControlManager != null)
                LabelControlManager.Visiblity(visible);
            View.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }
        public void EnableDisable(bool enable)
        {
            MainControl.IsEnabled = enable;
        }

        public void EnableDisable(object dataItem, bool enable)
        {
            MainControl.IsEnabled = enable;
        }

        public void EnableDisable(object dataItem, TemporaryLinkType link, bool enable)
        {
            if (MainControl is I_View_TemporaryView)
            {
                (MainControl as I_View_TemporaryView).DisableEnable(link, enable);
            }
        }
        public object GetUIControl(object dataItem)
        {
            return MainControl;
        }
        //public void AddValidation(BaseMessageItem item)
        //{
        //    ValidationItems.Add(item);
        //    SetTooltip();
        //    SetColor();
        //}

        //public void RemoveValidation(object dataItem, string key)
        //{
        //    foreach (var item in ValidationItems.Where(x => x.CausingDataItem == dataItem && x.Key == key).ToList())
        //        ValidationItems.Remove(item);
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

        //private void SetColor()
        //{
        //    Control control = null;
        //    if (RelationshipUISettingDTO.Expander == false)
        //        control = View as Control;
        //    else
        //        control = Expander;

        //    var color = InfoColor.Default;
        //    if (MessageItems.Any(x => x.Color != InfoColor.Default))
        //        color = MessageItems.First(x => x.Color != InfoColor.Default).Color;

        //    if (color != InfoColor.Default)
        //    {
        //        control.BorderBrush = UIManager.GetColorFromInfoColor(color);
        //        control.BorderThickness = new Thickness(1);
        //    }
        //    else
        //    {
        //        control.BorderBrush = new SolidColorBrush(UIManager.GetColorFromInfoColor(InfoColor.Default));
        //        control.BorderThickness = new Thickness(1);
        //    }

        //}

        //private void SetTooltip()
        //{
        //    Control control = null;
        //    if (RelationshipUISettingDTO.Expander == false)
        //        control = View as Control;
        //    else
        //        control = Expander;

        //    var tooltip = "";
        //    foreach (var item in MessageItems)
        //        tooltip += (tooltip == "" ? "" : Environment.NewLine) + item.Message;

        //    if (tooltip != "")
        //        ToolTipService.SetToolTip(control, tooltip);
        //    else
        //        ToolTipService.SetToolTip(control, null);
        //}


        public void SetTooltip(object dataItem, string tooltip)
        {
            Control control = null;
            if (RelationshipUISettingDTO.Expander == false)
                control = View as Control;
            else
                control = Expander;
            if (!string.IsNullOrEmpty(tooltip))
                ToolTipService.SetToolTip(control, tooltip);
            else
                ToolTipService.SetToolTip(control, null);
        }

        public void SetBorderColor(object dataItem, InfoColor color)
        {
            Control control = null;
            if (RelationshipUISettingDTO.Expander == false)
                control = View as Control;
            else
                control = Expander;

            control.BorderBrush = UIManager.GetColorFromInfoColor(color);
            control.BorderThickness = new Thickness(1);
        }

        public void SetBackgroundColor(object dataItem, InfoColor color)
        {
            Control control = null;
            if (RelationshipUISettingDTO.Expander == false)
                control = View as Control;
            else
                control = Expander;
            control.Background = UIManager.GetColorFromInfoColor(color);
        }

        public void SetForegroundColor(object dataItem, InfoColor color)
        {
            Control control = null;
            if (RelationshipUISettingDTO.Expander == false)
                control = View as Control;
            else
                control = Expander;
            control.Foreground = UIManager.GetColorFromInfoColor(color);
        }

        public void SetTemporaryViewText(object relatedData, string text)
        {
            if (MainControl is I_View_TemporaryView)
                (MainControl as I_View_TemporaryView).SetLinkText(text);
        }
        //public void ClearAllValidations()
        //{
        //    ValidationItems.Clear();
        //    SetTooltip();
        //    SetColor();
        //}




        public void SetQuickSearchVisibility(object parentData, bool v)
        {
            if (MainControl is I_View_TemporaryView)
            {
                (MainControl as I_View_TemporaryView).QuickSearchVisibility = v;
            }
        }

        public I_View_TemporaryView GetTemporaryView(object dataItem)
        {
            throw new NotImplementedException();
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
