
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
using WpfPersianDatePicker.Views;

namespace MyUIGenerator.UIControlHelper
{
    public class DatePickerHelper : BaseControlHelper, I_ControlHelper
    {
        Control textBox;
        ComboBox cmbOperators;

        public FrameworkElement MainControl { get { return textBox; } }
        public FrameworkElement WholeControl { get { return theGrid; } }

        bool? stringDateIsMiladi;
        bool valueIsString = false;
        public DatePickerHelper(ColumnDTO correspondingTypeProperty, ColumnUISettingDTO columnSetting, List<SimpleSearchOperator> operators = null)
        {
            theGrid = new Grid();
            theGrid.ColumnDefinitions.Add(new ColumnDefinition());
            theGrid.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            stringDateIsMiladi = correspondingTypeProperty.DateColumnType.StringDateIsMiladi;
            valueIsString = correspondingTypeProperty.OriginalColumnType == Enum_ColumnType.String;

            if (!correspondingTypeProperty.DateColumnType.ShowMiladiDateInUI)
            {
                textBox = new PDatePicker();
                (textBox as PDatePicker).SelectedDateChanged += DatePickerHelper_SelectedDateChanged;
            }
            else
            {
                textBox = new DatePicker();
                (textBox as DatePicker).SelectedDateChanged += (sender, e) => textBox_SelectedDateChanged(sender, e);
            }
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
            if (textBox is PDatePicker)
            {
                if (valueIsString && stringDateIsMiladi == false)
                    (textBox as PDatePicker).SelectedPersianDate = value?.ToString();
                else
                    (textBox as PDatePicker).SelectedDate = value?.ToString();
            }
            else
            {
                if (value != null && valueIsString && stringDateIsMiladi == false)
                {
                    var date = AgentHelper.GetMiladiDateFromShamsi(value.ToString());
                }
                else
                    (textBox as DatePicker).Text = value?.ToString();

            }
            return true;
        }

        public object GetValue()
        {
            if (textBox is PDatePicker)
            {
                if (valueIsString && stringDateIsMiladi == false)
                    return (textBox as PDatePicker).SelectedPersianDate;
                else
                    return (textBox as PDatePicker).SelectedDate?.ToString();
            }
            else
            {
                if ((textBox as DatePicker).SelectedDate != null && valueIsString && stringDateIsMiladi == false)
                {
                    return AgentHelper.GetShamsiDateFromMiladi((textBox as DatePicker).SelectedDate.Value);
                }
                else
                    return (textBox as DatePicker).SelectedDate?.ToString();
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
            if (textBox is PDatePicker)
            {
                if (valueIsString && stringDateIsMiladi == false)
                    textBox.SetBinding(PDatePicker.SelectedPersianDateProperty, binding);
                else
                {
                    textBox.SetBinding(PDatePicker.SelectedDateProperty, binding);
                }
            }
            else
            {
                if (valueIsString && stringDateIsMiladi == false)
                {
                    binding.Converter = new ConverterDate();
                    textBox.SetBinding(DatePicker.SelectedDateProperty, binding);
                }
                else
                    textBox.SetBinding(DatePicker.SelectedDateProperty, binding);
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
    public class ConverterDate : IValueConverter
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
}
