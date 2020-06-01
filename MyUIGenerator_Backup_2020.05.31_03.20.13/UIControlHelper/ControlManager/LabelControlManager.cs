
using ModelEntites;
using MyUILibrary;
using MyUILibrary.EntityArea;
using MyUILibrary.Temp;
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
    public class LabelControlManager : I_LabelControlManager
    {
        TextBlock TextBlock { set; get; }
        public FrameworkElement WholeControl { get { return TextBlock; } }

        public string Text
        {
            get
            {
                return TextBlock.Text;
            }

            set
            {
                TextBlock.Text = value;
            }
        }

        public LabelControlManager(string text, bool rightAlignment)
        {
            //FrameworkElement uiControl = new FrameworkElement();
            //UIControlSetting controlUISetting = new UIControlSetting();
            //if (title.Length>50)
            //controlUISetting.DesieredColumns = 2;
            //else

            //بقیه خصوصیات کالومن ستینگ ازینجا پر شود


            TextBlock = new TextBlock();
            if (!string.IsNullOrEmpty(text))
                TextBlock.Text = text;
            TextBlock.MaxWidth = 150;
            TextBlock.TextWrapping = System.Windows.TextWrapping.Wrap;
            TextBlock.Margin = new System.Windows.Thickness(2, 0, 2, 0);
            if (rightAlignment)
                TextBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            TextBlock.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            //if (title!=null && title.Length > 20)
            //    title = title.Substring(0, 17) + "...";
            // TextBlock.Text = title;
            //label.TextWrapping = System.Windows.TextWrapping.WrapWithOverflow;
            //label.Foreground = UIManager.GetColorFromInfoColor(color);
            //uiControl.DefaultColor = color;
            //uiControl.Color = color;
            //uiControl.DefaultTooltip = tooltip;
            //uiControl.Tooltip = tooltip;
            //uiControl.Control = label;
            //uiControl.UIControlSetting = controlUISetting;

            //if (!string.IsNullOrEmpty(tooltip))
            //{
            //    ToolTipService.SetToolTip(label, tooltip);
            //}

        }

        //internal static UIControlSetting GenerateUISetting(ColumnDTO nD_Type_Property, UISetting.DataPackageUISetting.UI_PackagePropertySetting uI_PackagePropertySetting)
        //{
        //    return new UIControlSetting() { DesieredColumns = 1, DesieredRows = 1 };
        //}



        internal static void Highlight(FrameworkElement uIControl, string message)
        {
            if (uIControl is TextBlock)
            {
                //////if (!string.IsNullOrEmpty(uIControl.Tooltip))
                //////{
                //////    message = message + Environment.NewLine + uIControl.Tooltip;
                //////}
                ToolTipService.SetToolTip((uIControl as TextBlock), message);
                (uIControl as TextBlock).Foreground = Brushes.Red;
                (uIControl as TextBlock).FontWeight = System.Windows.FontWeights.Bold;
            }
        }
        internal static void DeHighlight(FrameworkElement uIControl)
        {
            if (uIControl is TextBlock)
            {
                //////if (!string.IsNullOrEmpty(uIControl.Tooltip))
                //////{
                //////    ToolTipService.SetToolTip((uIControl.Control as TextBlock), uIControl.Tooltip);
                //////}
                //////else
                //////    ToolTipService.SetToolTip((uIControl.Control as TextBlock), null);
                //////(uIControl.Control as TextBlock).Foreground = new SolidColorBrush(UIManager.GetColorFromInfoColor(uIControl.DefaultColor));
                (uIControl as TextBlock).FontWeight = System.Windows.FontWeights.Normal;
            }
        }


        //internal static void Visiblity(FrameworkElement uIControl, bool visible)
        //{
        //    (uIControl as TextBlock).Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        //}

        public void Visiblity(bool visible)
        {
            TextBlock.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public void SetTooltip(object dataItem, string tooltip)
        {
            if (!string.IsNullOrEmpty(tooltip))
                ToolTipService.SetToolTip(TextBlock, tooltip);
            else
                ToolTipService.SetToolTip(TextBlock, null);
        }

        public void SetBorderColor(object dataItem, InfoColor color)
        {
            //   TextBlock.bo = UIManager.GetColorFromInfoColor(color);
        }

        public void SetBackgroundColor(object dataItem, InfoColor color)
        {
            TextBlock.Background = UIManager.GetColorFromInfoColor(color);
        }

        public void SetForegroundColor(object dataItem, InfoColor color)
        {
            TextBlock.Foreground = UIManager.GetColorFromInfoColor(color);

        }


    }
}
