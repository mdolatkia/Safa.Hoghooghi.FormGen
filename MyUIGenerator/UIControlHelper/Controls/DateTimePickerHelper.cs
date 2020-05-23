
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
            DateConverterParameter param = GetConverterParameter();
            DateTime? dateTime = ReadValueFromProperty(value, param);
            (textBox as MyDateTimePicker).SelectedDateTime = dateTime;
            return true;
        }

        public static DateTime? ReadValueFromProperty(object value, DateConverterParameter param)
        {
            DateTime? dateTime = null;
            if (value != null && value.ToString() != "")
            {
                if (param.valueIsString)
                {
                    if (!param.hasnotDatePicker)
                    {
                        string stringdate = null;
                        if (param.hasnotTimePicker)
                            stringdate = value.ToString();
                        else
                            stringdate = value.ToString().Split(' ')[0];
                        if (param.stringDateIsMiladi == false)
                            dateTime = AgentHelper.GetMiladiDateFromShamsi(stringdate);
                        else
                            dateTime = Convert.ToDateTime(stringdate);
                    }
                    if (!param.hasnotTimePicker)
                    {
                        string stringtime = null;
                        if (param.hasnotDatePicker)
                            stringtime = value.ToString();
                        else
                        {
                            stringtime = value.ToString().Split(" ".ToCharArray(), 2)[1];
                        }
                        if (param.stringTimeIsMiladi == false && param.stringTimeISAMPMFormat == true)
                            stringtime = stringtime.Replace("ق.ظ", "AM").Replace("ب.ظ", "PM");

                        DateTime time;
                        if (DateTime.TryParse(stringtime, out time))
                        {
                            if (dateTime == null)
                                dateTime = time;
                            else
                                dateTime= dateTime.Value.Add(time.TimeOfDay);
                        }
                    }
                }
                else
                {
                    dateTime = Convert.ToDateTime(value);
                }
            }
            return dateTime;


        }

        public object GetValue()
        {
            return ReadValueFromControl((textBox as MyDateTimePicker).SelectedDateTime, GetConverterParameter());

        }

        public static object ReadValueFromControl(DateTime? selectedDateTime, DateConverterParameter param)
        {
            if (selectedDateTime != null && param.valueIsString)
            {
                var date = "";
                var time = "";
                if (!param.hasnotDatePicker)
                {
                    if (param.stringDateIsMiladi == false)
                        date = AgentHelper.GetShamsiDateFromMiladi(selectedDateTime.Value);
                    else
                        date = selectedDateTime.Value.ToShortDateString();
                }
                if (!param.hasnotTimePicker)
                {
                    if (param.stringTimeISAMPMFormat == true)
                    {
                        if (param.stringTimeIsMiladi == true)
                        {
                            time = selectedDateTime.Value.ToString("hh:mm tt").ToUpper().Replace("AM", "ق.ظ").Replace("PM", "ب.ظ");
                        }
                        else
                        {
                            time = selectedDateTime.Value.ToString("hh:mm tt");
                        }
                    }
                    else
                        time = selectedDateTime.Value.ToString("hh:mm:ss");
                }
                return date + (!string.IsNullOrEmpty(time) ? " " : "") + time;
            }
            else
            {
                return selectedDateTime;
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
            binding.Converter = new ConverterDate();
            DateConverterParameter param = GetConverterParameter();
            binding.ConverterParameter = param;
            (textBox as MyDateTimePicker).SetBinding(MyDateTimePicker.SelectedDateTimeProperty, binding);
            //پارامتر بشن خصوصیات بالا و در کانورتر از همون گت و ست استفاده یشه


            if (valueIsString)
            {
                //if (!hasnotDatePicker && !hasnotTimePicker)
                //{

                //}
                //else if (!hasnotDatePicker)
                //{
                //    if (stringDateIsMiladi == false)
                //        binding.Converter = new ConverterDateOnlyShamsi();
                //    binding.con
                //}
                //else if (!hasnotTimePicker)
                //{
                //if (stringTimeISAMPMFormat == true)
                //{
                //    if (stringTimeIsMiladi == false)
                //        binding.Converter = new ConverterTimeOnlyAMPMShamsi();
                //    else
                //        binding.Converter = new ConverterTimeOnlyAMPM();
                //}
                //else
                //    binding.Converter = new ConverterTimeOnlyLong();

                //}
            }

        }

        private DateConverterParameter GetConverterParameter()
        {
            DateConverterParameter param = new UIControlHelper.DateConverterParameter();
            param.hasnotDatePicker = hasnotDatePicker;
            param.hasnotTimePicker = hasnotTimePicker;
            param.hideMiladiDatePicker = hideMiladiDatePicker;
            param.hideShamsiDatePicker = hideShamsiDatePicker;
            param.hideTimePicker = hideTimePicker;
            param.showAMPMFormat = showAMPMFormat;
            param.showMiladiDate = showMiladiDate;
            param.showMiladiTime = showMiladiTime;
            param.stringDateIsMiladi = stringDateIsMiladi;
            param.stringTimeISAMPMFormat = stringTimeISAMPMFormat;
            param.stringTimeIsMiladi = stringTimeIsMiladi;
            param.valueIsString = valueIsString;
            return param;
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
    public class ConverterDate : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var cParam = parameter as DateConverterParameter;
            DateTime? dateTime = DateTimePickerHelper.ReadValueFromProperty(value, cParam);
            return dateTime;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var cParam = parameter as DateConverterParameter;
            return DateTimePickerHelper.ReadValueFromControl((DateTime?)value, cParam);
        }
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

    public class DateConverterParameter
    {
        public bool? stringDateIsMiladi;
        public bool? stringTimeIsMiladi;
        public bool valueIsString = false;
        public bool? stringTimeISAMPMFormat;
        public bool hasnotTimePicker;
        public bool hideTimePicker;
        public bool hideMiladiDatePicker;
        public bool hideShamsiDatePicker;
        public bool hasnotDatePicker;
        public bool showAMPMFormat;
        public bool showMiladiTime;
        public bool showMiladiDate;
    }
}
