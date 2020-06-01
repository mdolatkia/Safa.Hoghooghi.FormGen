
using ModelEntites;
using MyUILibrary;
using MyUILibrary.EntityArea;
using MyUILibrary.Temp;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using System.Windows.Media;
using Telerik.Windows.Controls;
using WpfPersianDatePicker.Views;

namespace MyUIGenerator.UIControlHelper
{
    public class TimePickerHelper : BaseControlHelper, I_ControlHelper
    {
        Control textBox;
        ComboBox cmbOperators;

        public FrameworkElement MainControl { get { return textBox; } }
        public FrameworkElement WholeControl { get { return theGrid; } }
        bool? stringTimeIsMiladi;
        bool? stringTimeISAMPMFormat;
        bool valueIsString = false;
        public TimePickerHelper(ColumnDTO correspondingTypeProperty, ColumnUISettingDTO columnSetting, List<SimpleSearchOperator> operators = null)
        {
            theGrid = new Grid();
            theGrid.ColumnDefinitions.Add(new ColumnDefinition());
            theGrid.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            stringTimeIsMiladi = correspondingTypeProperty.TimeColumnType.StringTimeIsMiladi == true;
            stringTimeISAMPMFormat = correspondingTypeProperty.TimeColumnType.StringTimeISAMPMFormat == true;
            valueIsString = correspondingTypeProperty.OriginalColumnType == Enum_ColumnType.String;
            textBox = new RadTimePicker();
            if (correspondingTypeProperty.TimeColumnType.ShowAMPMFormat)
            {
                CultureInfo cultureInfo = null;
                if (correspondingTypeProperty.TimeColumnType.ShowMiladiTime)
                    cultureInfo = new CultureInfo("en-US");
                else
                    cultureInfo = new CultureInfo("fa-IR");

                //cultureInfo.DateTimeFormat.ShortTimePattern = "H:mm";
                //cultureInfo.DateTimeFormat.LongTimePattern = "H:mm";
                //cultureInfo.DateTimeFormat.PMDesignator = " za";
                //cultureInfo.DateTimeFormat.AMDesignator = " az";
                (textBox as RadTimePicker).Culture = cultureInfo;
            }
            else
            {
                //(textBox as RadTimePicker).DisplayFormat = DateTimePickerFormat.Long;
                CultureInfo cultureInfo = new CultureInfo("en-US");
                cultureInfo.DateTimeFormat.ShortTimePattern = "H:mm";
                cultureInfo.DateTimeFormat.LongTimePattern = "H:mm";
                //cultureInfo.DateTimeFormat.PMDesignator = " za";
                //cultureInfo.DateTimeFormat.AMDesignator = " az";
                (textBox as RadTimePicker).Culture = cultureInfo;
            }

            //////if (!correspondingTypeProperty.TimeColumnType.ShowMiladiDateInUI)
            //////{
            //////    textBox = new PDatePicker();
            //////    (textBox as PDatePicker).SelectedDateChanged += TimePickerHelper_SelectedDateChanged;
            //////}
            //////else
            //////{
            //////    textBox = new DatePicker();
            //////    (textBox as DatePicker).SelectedDateChanged += (sender, e) => textBox_SelectedDateChanged(sender, e);
            //////}

            textBox.Name = "txtControl";
            //
            textBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            //   textBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            theGrid.Children.Add(textBox);

            if (operators != null && operators.Count > 0)
            {

                theGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(70) });
                cmbOperators = new ComboBox();
                cmbOperators.Name = "cmbOperators";
                cmbOperators.ItemsSource = operators;
                cmbOperators.DisplayMemberPath = "Title";
                cmbOperators.SelectedValuePath = "Operator";
                Grid.SetColumn(cmbOperators, 1);
                theGrid.Children.Add(cmbOperators);
            }


            //}
            //}
            //return package;
        }

        //private void TimePickerHelper_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    var binding = textBox.GetBindingExpression(PDatePicker.SelectedDateProperty);
        //    var bindinga = textBox.GetBindingExpression(PDatePicker.SelectedPersianDateProperty);


        //}

        private void TimePickerHelper_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var binding = textBox.GetBindingExpression(PDatePicker.SelectedDateProperty);
            var bindinga = textBox.GetBindingExpression(PDatePicker.SelectedPersianDateProperty);
        }

        void textBox_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var binding = textBox.GetBindingExpression(PDatePicker.SelectedDateProperty);
            var bindinga = textBox.GetBindingExpression(PDatePicker.SelectedPersianDateProperty);
        }
        public bool SetValue(object value)
        {
            if (value != null && valueIsString && stringTimeIsMiladi == false && stringTimeISAMPMFormat == true)
            {
             
                   (textBox as RadTimePicker).DateTimeText = value.ToString().Replace("ق.ظ", "AM").Replace("ب.ظ", "PM");
            }
            else
                (textBox as RadTimePicker).DateTimeText = value == null ? null : value.ToString();

            return true;
        }

        public object GetValue()
        {
            if (valueIsString && (textBox as RadTimePicker).SelectedValue != null)
            {
                if (stringTimeISAMPMFormat == true)
                {
                    if (stringTimeIsMiladi == false)
                    {
                        return (textBox as RadTimePicker).SelectedValue.Value.ToString("hh:mm tt").ToUpper().Replace("am", "ق.ظ").Replace("pm", "ب.ظ");
                    }
                    else
                    {
                        return (textBox as RadTimePicker).SelectedValue.Value.ToString("hh:mm tt");
                    }
                }
                else
                    return (textBox as RadTimePicker).SelectedValue.Value.ToString("hh:mm:ss");
            }
            else
                return (textBox as RadTimePicker).SelectedValue;
        }

        public CommonOperator GetOperator()
        {
            if (cmbOperators != null)
            {
                return (cmbOperators.SelectedItem as SimpleSearchOperator).Operator;
            }
            else
                return CommonOperator.Equals;
        }
        public bool SetOperator(CommonOperator searchOperator)
        {
            cmbOperators.SelectedValue = searchOperator;
            return false;
        }
        public bool HasOperator()
        {
            return cmbOperators != null;
        }
        public void EnableDisable(bool enable)
        {
            textBox.IsEnabled = enable;
        }
        public void Visiblity(bool visible)
        {
            textBox.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }
        public bool IsVisible()
        {
            return textBox.Visibility == Visibility.Visible;
        }
        public void SetReadonly(bool isreadonly)
        {
            textBox.IsEnabled = !isreadonly;
        }

        //public void SetTooltip( string tooltip)
        //{
        //    if (!string.IsNullOrEmpty(tooltip))
        //        ToolTipService.SetToolTip(textBox, tooltip);
        //    else
        //        ToolTipService.SetToolTip(textBox, null);
        //}

        //public void ClearTooltip()
        //{
        //    ToolTipService.SetToolTip(textBox, null);
        //}

        public void SetBorderColor(InfoColor color)
        {
            textBox.BorderBrush = UIManager.GetColorFromInfoColor(color);
            textBox.BorderThickness = new Thickness(1);
        }
        public void SetBackgroundColor(InfoColor color)
        {
            textBox.Background = UIManager.GetColorFromInfoColor(color);
        }
        public void SetForegroundColor(InfoColor color)
        {
            textBox.Foreground = UIManager.GetColorFromInfoColor(color);
        }
        //public void ClearBorderColor()
        //{
        //    textBox.BorderBrush = new SolidColorBrush(UIManager.GetColorFromInfoColor(InfoColor.Black));
        //    textBox.BorderThickness = new Thickness(1);
        //}
        public void SetBinding(EntityInstanceProperty property)
        {
            Binding binding = new Binding("Value");
            binding.Source = property;
            binding.Mode = BindingMode.TwoWay;
            if (valueIsString)
            {
                if (stringTimeISAMPMFormat == true)
                {
                    if (stringTimeIsMiladi == false)
                    {
                        binding.Converter = new ConverterAMPMShamsi();
                        textBox.SetBinding(RadTimePicker.SelectedValueProperty, binding);
                    }
                    else
                    {
                        binding.Converter = new ConverterAMPM();
                        textBox.SetBinding(RadTimePicker.SelectedValueProperty, binding);
                    }
                }
                else
                {
                    binding.Converter = new ConverterLongTime();
                    textBox.SetBinding(RadTimePicker.SelectedValueProperty, binding);
                }
            }
            else
                textBox.SetBinding(RadTimePicker.SelectedValueProperty, binding);
        }

        //public void AddButtonMenu(ConrolPackageMenu menu)
        //{
        //    theGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(50) });
        //    var menuButton = new Button();
        //    menuButton.Name = menu.Name;
        //    menuButton.Content = menu.Title;
        //    menuButton.Click += (sender, e) => MenuButton_Click(sender, e, menu);
        //    Grid.SetColumn(menuButton, theGrid.ColumnDefinitions.Count);
        //    theGrid.Children.Add(menuButton);
        //}

        //private void MenuButton_Click(object sender, RoutedEventArgs e, ConrolPackageMenu menu)
        //{
        //    ConrolPackageMenuArg arg = new ConrolPackageMenuArg();
        //    menu.OnMenuClicked(sender, arg);
        //}
    }
    //public class ConverterDate : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        if (value != null && value.ToString() != "")
    //        {
    //            return AgentHelper.GetMiladiDateFromShamsi(value.ToString());
    //        }
    //        else
    //            return null;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        if (value != null)
    //        {
    //            return AgentHelper.GetShamsiDateFromMiladi((DateTime)value);
    //        }
    //        else
    //            return null;
    //    }
    //}

    public class ConverterLongTime : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var time = (DateTime)value;
                return time.ToString("hh:mm:ss");
            }
            else
                return null;
        }
    }
    public class ConverterAMPM : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var time = (DateTime)value;
                return time.ToString("hh:mm tt");
            }
            else
                return null;
        }
    }
    public class ConverterAMPMShamsi : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                return value.ToString().Replace("ق.ظ", "AM").Replace("ب.ظ", "PM");
            }
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var time = (DateTime)value;
                return time.ToString("hh:mm tt").ToUpper().Replace("AM", "ق.ظ").Replace("PM", "ب.ظ");
            }
            else
                return null;
        }
    }
}
