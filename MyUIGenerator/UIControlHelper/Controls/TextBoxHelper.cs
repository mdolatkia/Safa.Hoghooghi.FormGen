
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
using ProxyLibrary;
using System.Windows.Data;

namespace MyUIGenerator.UIControlHelper
{
    public class TextBoxHelper : BaseControlHelper, I_ControlHelper
    {

        TextBox textBox;
        ComboBox cmbOperators;
        public FrameworkElement MainControl { get { return textBox; } }
        public FrameworkElement WholeControl { get { return theGrid; } }
        public TextBoxHelper(ColumnDTO correspondingTypeProperty, ColumnUISettingDTO columnSetting, List<SimpleSearchOperator> operators = null)
        {
            theGrid = new Grid();
            theGrid.ColumnDefinitions.Add(new ColumnDefinition());
            theGrid.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            textBox = new TextBox();
            textBox.Name = "txtControl";
            textBox.GotFocus += TextBox_GotFocus;
            textBox.TextChanged += (sender, e) => textBox_TextChanged(sender, e);
            textBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            theGrid.Children.Add(textBox);

            if (columnSetting != null && columnSetting.UIRowsCount > 1)
            {
                textBox.AcceptsReturn = true;
                textBox.TextWrapping = TextWrapping.Wrap;
            }
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

        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            //var binding = (sender as TextBox).GetBindingExpression(TextBox.TextProperty);
            //binding.UpdateTarget();
        }

        void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //ColumnValueChangeArg arg = new ColumnValueChangeArg();
            //arg.NewValue = GetValue(uiControlPackage);
            //uiControlPackage.OnValueChanged(sender, arg);
        }

        //public  UIControlSetting GenerateUISetting(DataMaster.EntityDefinition.ND_Type_Property nD_Type_Property, UISetting.DataPackageUISetting.UI_PackagePropertySetting uI_PackagePropertySetting)
        //{
        //    throw new NotImplementedException();
        //}


        public bool SetValue(object value)
        {
            textBox.Text = value == null ? "" : value.ToString();
            return true;
        }

        public object GetValue()
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

        public void SetReadonly(bool isreadonly)
        {
            textBox.IsReadOnly = isreadonly;
        }

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
        public void SetBinding(EntityInstanceProperty property)
        {
            var bindinga = textBox.GetBindingExpression(TextBox.TextProperty);

            Binding binding = new Binding("Value");
            binding.Mode = BindingMode.TwoWay;
            binding.Source = property;
            textBox.SetBinding(TextBox.TextProperty, binding);
        }

        //private void MenuButton_Click(object sender, RoutedEventArgs e, ConrolPackageMenu menu)
        //{
        //    ConrolPackageMenuArg arg = new ConrolPackageMenuArg();
        //    menu.OnMenuClicked(sender, arg);
        //}
        public void Visiblity(bool visible)
        {
            textBox.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }
        public bool IsVisible()
        {
            return textBox.Visibility == Visibility.Visible;
        }
    }
}
