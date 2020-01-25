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
    public class BaseControlManager 
    {
        //   public List<FrameworkElement> RelatedControl { set; get; }
        public BaseControlManager()
        {
            //    RelatedControl = new List<FrameworkElement>();
        }

        public I_LabelControlManager LabelControlManager
        {
            set; get;
        }

        //public void EnableDisable(object dataItem, bool enable)
        //{
        //    if (this is RelationshipControlManagerForMultipleDataForm)
        //    {
        //        (this as RelationshipControlManagerForMultipleDataForm).DataGridColumn.EnableDisable(dataItem, enable);
        //    }
        //    else if (this is SimpleControlManagerForMultipleDataForm)
        //    {
        //        (this as SimpleControlManagerForMultipleDataForm).DataGridColumn.EnableDisable(dataItem, enable);
        //    }
        //    else if (this is SimpleControlManagerForOneDataForm)
        //    {
        //        var form = (this as SimpleControlManagerForOneDataForm);
        //        form.MyControlHelper.EnableDisable(form.MainControl, enable);
        //    }
        //    else if (this is RelationshipControlManagerForOneDataForm)
        //    {
        //        var form = (this as RelationshipControlManagerForOneDataForm);
        //        form.MainControl.IsEnabled = enable;
        //    }

        //}

        //public void SetBackgroundColor(object dataItem, InfoColor color)
        //{
        //    throw new NotImplementedException();
        //}

        //public void SetBorderColor(object dataItem, InfoColor color)
        //{
        //    throw new NotImplementedException();
        //}

        //public void SetForegroundColor(object dataItem, InfoColor color)
        //{
        //    throw new NotImplementedException();
        //}

        //public void SetTooltip(object dataItem, string tooltip)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Visiblity(object dataItem, bool visible)
        //{
        //    throw new NotImplementedException();
        //}
        //public void SetMandatoryState(bool isMandatory)
        //{
        //    if (RelatedControl != null)
        //        foreach (var item in RelatedControl)
        //        {
        //            if (item is TextBlock)
        //            {
        //                var textblock = (item as TextBlock);

        //                if (isMandatory)
        //                {
        //                    if (!textblock.Text.StartsWith("* "))
        //                    {
        //                        textblock.Text = "* " + textblock.Text;
        //                    }
        //                    textblock.Foreground = new SolidColorBrush(Colors.DarkRed);
        //                }
        //                else
        //                {
        //                    if (textblock.Text.StartsWith("* "))
        //                    {
        //                        textblock.Text = textblock.Text.Substring(2, textblock.Text.Length - 2);
        //                    }
        //                    textblock.Foreground = new SolidColorBrush(UIManager.GetColorFromInfoColor(InfoColor.Default));
        //                }
        //            }
        //        }
        //}

    }
}
