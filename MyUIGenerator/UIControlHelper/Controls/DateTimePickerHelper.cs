
using ModelEntites;
using MyUIGenerator.UIControlHelper.Controls;
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
using WpfPersianDatePicker.Views;

namespace MyUIGenerator.UIControlHelper
{
    public class DateTimePickerHelper : BaseControlHelper, I_ControlHelper
    {
        Control textBox;
        ComboBox cmbOperators;

        public FrameworkElement MainControl { get { return textBox; } }
        public FrameworkElement WholeControl { get { return theGrid; } }

        bool? stringDateIsMiladi;
        bool? stringTimeIsMiladi;
        bool valueIsString = false;
        bool? stringTimeISAMPMFormat;
        bool hasnotTimePicker;
        bool hideTimePicker;
        bool hideMiladiDatePicker;
        bool hideShamsiDatePicker;
        bool hasnotDatePicker;
        bool showAMPMFormat;
        bool showMiladiTime;
        bool showMiladiDate;
        public DateTimePickerHelper(ColumnDTO correspondingTypeProperty, ColumnUISettingDTO columnSetting, List<SimpleSearchOperator> operators = null)
        {
            theGrid = new Grid();
            theGrid.ColumnDefinitions.Add(new ColumnDefinition());
            theGrid.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            if (correspondingTypeProperty.ColumnType == Enum_ColumnType.DateTime)
            {
                showMiladiTime = correspondingTypeProperty.DateTimeColumnType.ShowMiladiDateInUI;
                showMiladiDate = correspondingTypeProperty.DateTimeColumnType.ShowMiladiDateInUI;
                showAMPMFormat = correspondingTypeProperty.DateTimeColumnType.ShowAMPMFormat;
                stringDateIsMiladi = correspondingTypeProperty.DateTimeColumnType.StringDateIsMiladi;
                stringTimeISAMPMFormat = correspondingTypeProperty.DateTimeColumnType.StringTimeISAMPMFormat;
                stringTimeIsMiladi = correspondingTypeProperty.DateTimeColumnType.StringTimeIsMiladi;
                hideTimePicker = correspondingTypeProperty.DateTimeColumnType.HideTimePicker;
            }
            else if (correspondingTypeProperty.ColumnType == Enum_ColumnType.Date)
            {
                showMiladiDate = correspondingTypeProperty.DateColumnType.ShowMiladiDateInUI;
                stringDateIsMiladi = correspondingTypeProperty.DateColumnType.StringDateIsMiladi;
                hasnotTimePicker = true;
            }
            else if (correspondingTypeProperty.ColumnType == Enum_ColumnType.Time)
            {
                showMiladiTime = correspondingTypeProperty.TimeColumnType.ShowMiladiTime;
                showAMPMFormat = correspondingTypeProperty.TimeColumnType.ShowAMPMFormat;
                stringTimeIsMiladi = correspondingTypeProperty.TimeColumnType.StringTimeIsMiladi;
                stringTimeISAMPMFormat = correspondingTypeProperty.TimeColumnType.StringTimeISAMPMFormat;
                hasnotDatePicker = true;
                hasnotTimePicker = false;
            }
            if (hasnotDatePicker)
            {
                hideShamsiDatePicker = true;
                hideMiladiDatePicker = true;
            }
            else
            {
                if (showMiladiDate)
                    hideShamsiDatePicker = true;
                else
                    hideMiladiDatePicker = true;
            }
            valueIsString = correspondingTypeProperty.OriginalColumnType == Enum_ColumnType.String;
            textBox = new MyDateTimePicker();


            (textBox as MyDateTimePicker).TimePickerVisiblity = !hasnotTimePicker && !hideTimePicker;
            (textBox as MyDateTimePicker).ShamsiDatePickerVisiblity = !hideShamsiDatePicker;
            (textBox as MyDateTimePicker).MiladiDatePickerVisiblity = !hideMiladiDatePicker;

            if ((textBox as MyDateTimePicker).TimePickerVisiblity)
            {
                if (showAMPMFormat)
                {
                    CultureInfo cultureInfo = null;
                    if (showMiladiTime)
                        cultureInfo = new CultureInfo("en-US");
                    else
                        cultureInfo = new CultureInfo("fa-IR");
                    (textBox as MyDateTimePicker).TimePickeCulture = cultureInfo;
                }
                else
                {
                    //(textBox as RadTimePicker).DisplayFormat = DateTimePickerFormat.Long;
                    CultureInfo cultureInfo = new CultureInfo("en-US");
                    cultureInfo.DateTimeFormat.ShortTimePattern = "H:mm";
                    cultureInfo.DateTimeFormat.LongTimePattern = "H:mm";
                    //cultureInfo.DateTimeFormat.PMDesignator = " za";
                    //cultureInfo.DateTimeFormat.AMDesignator = " az";
                    (textBox as MyDateTimePicker).TimePickeCulture = cultureInfo;
                }
            }
            textBox.Name = "txtControl";
            //
            textBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
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

        //private void DatePickerHelper_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    var binding = textBox.GetBindingExpression(PDatePicker.SelectedDateProperty);
        //    var bindinga = textBox.GetBindingExpression(PDatePicker.SelectedPersianDateProperty);


        //}

        private void DatePickerHelper_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
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
            DateTime? dateTime = null;
            if (value != null)
            {
                if (valueIsString)
                {
                    if (!hasnotDatePicker)
                    {
                        string stringdate = null;
                        if (hasnotTimePicker)
                            stringdate = value.ToString();
                        else
                            stringdate = value.ToString().Split(' ')[0];
                        if (stringDateIsMiladi == false)
                            dateTime = AgentHelper.GetMiladiDateFromShamsi(value.ToString());
                        else
                            dateTime = (DateTime?)value;
                    }
                    if (!hasnotTimePicker)
                    {
                        string stringtime = null;
                        if (hasnotDatePicker)
                            stringtime = value.ToString();
                        else
                            stringtime = value.ToString().Split(' ')[1];
                        if (stringTimeIsMiladi == false && stringTimeISAMPMFormat == true)
                            stringtime = stringtime.Replace("ق.ظ", "AM").Replace("ب.ظ", "PM");

                        DateTime time;
                        if (DateTime.TryParse(stringtime, out time))
                        {
                            if (dateTime == null)
                                dateTime = time;
                            else
                                dateTime.Value.Add(time.TimeOfDay);
                        }
                    }
                }
                else
                {
                    dateTime = (DateTime)value;
                }
            }
               (textBox as MyDateTimePicker).SelectedDateTime = dateTime;
            return true;
        }

        public object GetValue()
        {
            if ((textBox as MyDateTimePicker).SelectedDateTime != null && valueIsString)
            {
                var selectedDateTime = (textBox as MyDateTimePicker).SelectedDateTime.Value;
                var date = "";
                var time = "";
                if (!hasnotDatePicker)
                {
                    if (stringDateIsMiladi == false)
                        date = AgentHelper.GetShamsiDateFromMiladi(selectedDateTime);
                    else
                        date = selectedDateTime.ToString();
                }
                if (!hasnotTimePicker)
                {
                    if (stringTimeISAMPMFormat == true)
                    {
                        if (stringTimeIsMiladi == false)
                        {
                            time = (textBox as MyDateTimePicker).SelectedTime.Value.TimeOfDay.ToString("hh:mm tt").ToUpper().Replace("am", "ق.ظ").Replace("pm", "ب.ظ");
                        }
                        else
                        {
                            time = (textBox as MyDateTimePicker).SelectedTime.Value.TimeOfDay.ToString("hh:mm tt");
                        }
                    }
                    else
                        time = (textBox as MyDateTimePicker).SelectedDateTime.Value.TimeOfDay.ToString("hh:mm:ss");
                }
                return date + (!string.IsNullOrEmpty(time) ? "" : " ") + time;
            }
            else
            {
                return (textBox as MyDateTimePicker).SelectedDateTime;
                //DateTime? date = null;
                //DateTime? time = null;
                //if (!hasnotDatePicker)
                //    date = (textBox as MyDateTimePicker).MiladiDate;
                //if (!hasnotTimePicker)
                //    time = (textBox as MyDateTimePicker).SelectedTime;
                //if (date != null && time != null)
                //    return date.Value.Add(time.Value.TimeOfDay);
                //else if (date != null)
                //    return date.Value;
                //else if (time != null)
                //    return time.Value;
                //else return null;
            }

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
            (textBox as MyDateTimePicker).SetBinding(MyDateTimePicker.SelectedDateTimeProperty, binding);
            پارامتر بشن خصوصیات بالا و در کانورتر از همون گت و ست استفاده یشه
            if (valueIsString)
            {
                if (!hasnotDatePicker && !hasnotTimePicker)
                {

                }
                else if (!hasnotDatePicker)
                {
                    if (stringDateIsMiladi == false)
                        binding.Converter = new ConverterDateOnlyShamsi();
                    binding.con
                }
                else if (!hasnotTimePicker)
                {
                    if (stringTimeISAMPMFormat == true)
                    {
                        if (stringTimeIsMiladi == false)
                            binding.Converter = new ConverterTimeOnlyAMPMShamsi();
                        else
                            binding.Converter = new ConverterTimeOnlyAMPM();
                    }
                    else
                        binding.Converter = new ConverterTimeOnlyLong();

                }
            }

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
    public class ConverterDateOnlyShamsi : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value.ToString() != "")
            {
                return AgentHelper.GetMiladiDateFromShamsi(value.ToString());
            }
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                return AgentHelper.GetShamsiDateFromMiladi((DateTime)value);
            }
            else
                return null;
        }
    }
    public class ConverterTimeOnlyLong : IValueConverter
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
    public class ConverterTimeOnlyAMPM : IValueConverter
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
    public class ConverterTimeOnlyAMPMShamsi : IValueConverter
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
