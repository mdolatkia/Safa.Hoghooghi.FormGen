
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
using System.Windows.Data;
using System.Windows.Media;

namespace MyUIGenerator.UIControlHelper
{
    public class DatePickerHelper : BaseControlHelper, I_ControlHelper
    {
        DatePicker textBox;
        ComboBox cmbOperators;

        public FrameworkElement MainControl { get { return textBox; } }
        public FrameworkElement WholeControl { get { return theGrid; } }
        public DatePickerHelper(ColumnDTO correspondingTypeProperty, ColumnUISettingDTO columnSetting, List<SimpleSearchOperator> operators = null)
        {
            //UIControlPackage package = new UIControlPackage();

            //UIControlSetting controlUISetting = new UIControlSetting();
            //controlUISetting.DesieredColumns = ColumnWidth.Normal;
            //controlUISetting.DesieredRows = 1;
            //if (columnSetting.IsReadOnly && columnSetting.LabelForReadOnlyText == true)
            //{
            //    var textBox = new TextBlock();

            //    //textBox.Margin = new System.Windows.Thickness(5);
            //    textBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            //    textBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            //    package.FrameworkElement = new FrameworkElement() { Control = textBox, UIControlSetting = columnSetting.UISetting };
            //}
            //else
            //{+
            //if (operators == null || operators.Count == 0)
            //{
            //    var textBox = new DatePicker();
            //    textBox.SelectedDateChanged += (sender, e) => textBox_SelectedDateChanged(sender, e);
            //    //textBox.IsEnabled = !columnSetting.IsReadOnly;
            //    //if (!columnSetting.GridView)
            //    //{
            //    //    //textBox.Width = 200;
            //    //    textBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            //    //}
            //    //else
            //    textBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

            //    //   textBox.Margin = new System.Windows.Thickness(5);
            //    //textBox.MinHeight = 24;
            //    //textBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            //    textBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            //    return textBox;
            //}
            //else
            //{
            theGrid = new Grid();
            theGrid.ColumnDefinitions.Add(new ColumnDefinition());
            theGrid.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            textBox = new DatePicker();
            textBox.Name = "txtControl";
            textBox.SelectedDateChanged += (sender, e) => textBox_SelectedDateChanged(sender, e);
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

        void textBox_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //ColumnValueChangeArg arg = new ColumnValueChangeArg();
            //arg.NewValue = GetValue(uiControlPackage);
            //uiControlPackage.OnValueChanged(sender, arg);
        }
        public bool SetValue(object value)
        {
            textBox.Text = value == null ? "" : value.ToString();
            return true;
        }

        public string GetValue()
        {
            return textBox.Text;
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
            textBox.SetBinding(DatePicker.SelectedDateProperty, binding);

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
}
