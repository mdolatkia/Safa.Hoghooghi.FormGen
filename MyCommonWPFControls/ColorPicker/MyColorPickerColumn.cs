using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace MyCommonWPFControls
{
    public class MyColorPickerColumn : GridViewDataColumn
    {

        public override FrameworkElement CreateCellElement(GridViewCell cell, object dataItem)
        {
            //Border cellElement = new Border();
            //  var valueBinding = new System.Windows.Data.Binding(this.DataMemberBinding.Path.Path)
            //{
            //    Mode = BindingMode.OneTime,
            //    Converter = new ColorToBrushConverter()
            //};
            //cellElement.SetBinding(Border.BackgroundProperty, valueBinding);
            //cellElement.Width = 45;
            //cellElement.Height = 20;
            //cellElement.CornerRadius = new CornerRadius(5);

            return GetCellContent(cell, dataItem);
        }

        private FrameworkElement GetCellContent(GridViewCell cell, object dataItem)
        {
            Type myType = dataItem.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
            var colorProp = props.FirstOrDefault(x => x.Name == this.DataMemberBinding.Path.Path);
            if (colorProp != null)
            {

                var valueColor = colorProp.GetValue(dataItem);
                RadColorPicker colorPicker = new RadColorPicker();
                if (valueColor != null && valueColor.ToString() != "")
                {
                    colorPicker.SelectedColor = (Color)System.Windows.Media.ColorConverter.ConvertFromString(valueColor.ToString());
                }
                colorPicker.SelectedColorChanged += (sender1, e1) => ColorPicker_SelectedColorChanged(sender1, e1, colorProp, dataItem);
                return colorPicker;
                // Grid grid = new Grid();
                // grid.HorizontalAlignment = HorizontalAlignment.Stretch;
                // grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });
                // grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength() });
                // var button = new Button();
                // button.Content = "...";
                // var textBlock = new TextBlock();
                //textBlock.Width = 50;
                // textBlock.HorizontalAlignment = HorizontalAlignment.Stretch;

                // if (valueColor != null && valueColor.ToString() != "")
                // {
                //     System.Windows.Media.Color color = (Color)System.Windows.Media.ColorConverter.ConvertFromString(valueColor.ToString());
                //     var colorBrush = new SolidColorBrush(color);
                //     textBlock.Background = colorBrush;
                // }
                // button.Click += (sender, e) => Button_Click(sender, e, textBlock, colorProp, dataItem, textBlock.Background as SolidColorBrush);
                // grid.Children.Add(button);
                // Grid.SetColumn(textBlock, 1);
                // grid.Children.Add(textBlock);
                // return grid;

            }




            return null;
        }

        private void ColorPicker_SelectedColorChanged1(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public override FrameworkElement CreateCellEditElement(GridViewCell cell, object dataItem)
        {
            return GetCellContent(cell, dataItem);
        }
        //dataItem.    this.DataMemberBinding.Path.Path

        //var value = GetDisplayPropertyValueFromItemsource(cell.DataContext);
        //  if (value != null)
        //      cellElement.Text = value.ToString();
        //  else
        //      cellElement.Text = "";
        //  return cellElement;

        RadColorPicker colorPicker = null;
        //private void Button_Click(object sender, RoutedEventArgs e, TextBlock textBlock, PropertyInfo property, object dataItem, SolidColorBrush colorBrush)
        //{
        //    RadColorPicker colorPicker = new RadColorPicker();
        //    if (colorBrush != null)
        //        colorPicker.SelectedColor = colorBrush.Color;
        //    colorPicker.SelectedColorChanged += (sender1, e1) => ColorPicker_SelectedColorChanged(sender1, e1, textBlock, property, dataItem);
        //    RadWindow window = new RadWindow();
        //    window.Content = colorPicker;
        //    window.ShowDialog();
        //}

        private void ColorPicker_SelectedColorChanged(object sender, EventArgs e, PropertyInfo property, object dataItem)
        {
            property.SetValue(dataItem, (sender as RadColorPicker).SelectedColor.ToString());
        //    var colorBrush = new SolidColorBrush((sender as RadColorPicker).SelectedColor);
          //  textBlock.Background = colorBrush;
        }

        //private Dictionary<object,>


        //private System.Windows.Data.Binding CreateValueBinding()
        //{
        //    System.Windows.Data.Binding valueBinding = new System.Windows.Data.Binding();
        //    valueBinding.Mode = BindingMode.TwoWay;
        //    valueBinding.NotifyOnValidationError = true;
        //    valueBinding.ValidatesOnExceptions = true;
        //    valueBinding.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;
        //    valueBinding.Path = new PropertyPath(this.DataMemberBinding.Path.Path);
        //    return valueBinding;
        //}

        //public ColorPreset MainPalette
        //{
        //    get
        //    {
        //        return (ColorPreset)GetValue(MainPaletteProperty);
        //    }
        //    set
        //    {
        //        SetValue(MainPaletteProperty, value);
        //    }
        //}
        //public static readonly DependencyProperty MainPaletteProperty = DependencyProperty.Register("MainPalette",
        //    typeof(ColorPreset),
        //    typeof(RadColorPickerColumn),
        //    new PropertyMetadata(null));



    }
}
