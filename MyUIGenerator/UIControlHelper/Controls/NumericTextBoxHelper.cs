
using ModelEntites;
using MyUILibrary;
using MyUILibrary.EntityArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using MyUILibrary.Temp;
using System.Windows.Media;
using System.Windows.Data;
using ProxyLibrary;

namespace MyUIGenerator.UIControlHelper
{
    public class NumericTextBoxHelper : BaseControlHelper, I_ControlHelper
    {
        RadMaskedNumericInput textBox;
        ComboBox cmbOperators;
        public FrameworkElement MainControl { get { return textBox; } }
        public FrameworkElement WholeControl { get { return theGrid; } }
        public NumericTextBoxHelper(ColumnDTO correspondingTypeProperty, ColumnUISettingDTO columnSetting, List<SimpleSearchOperator> operators = null)
        {
            theGrid = new Grid();
            theGrid.ColumnDefinitions.Add(new ColumnDefinition());
            theGrid.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            textBox = new RadMaskedNumericInput();
            textBox.Name = "txtControl";
            textBox.ValueChanged += (sender, e) => textBox_ValueChanged(sender, e);
            textBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            textBox.IsLastPositionEditable = false;
            textBox.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
            textBox.IsClearButtonVisible = false;
            textBox.TextMode = Telerik.Windows.Controls.MaskedInput.TextMode.PlainText;
            textBox.SelectionOnFocus = SelectionOnFocus.SelectAll;


            //if (correspondingTypeProperty.DataType.Contains("float")
            //    || correspondingTypeProperty.DataType.Contains("decimal")
            //    || correspondingTypeProperty.DataType.Contains("double"))
            //{
            if (correspondingTypeProperty.NumericColumnType != null && correspondingTypeProperty.NumericColumnType.Precision != 0
                && correspondingTypeProperty.NumericColumnType.Scale != 0)
            {
                textBox.Mask = "#" + (correspondingTypeProperty.NumericColumnType.Precision - correspondingTypeProperty.NumericColumnType.Scale);
                textBox.Mask += "." + correspondingTypeProperty.NumericColumnType.Scale;
                textBox.FormatString = "";
                
            }
            else
            {


                textBox.Mask = "";
                if (correspondingTypeProperty.DataType.Contains("float")
                    || correspondingTypeProperty.DataType.Contains("decimal")
                    || correspondingTypeProperty.DataType.Contains("double"))
                {
                    //بطور پیش فرض دو رقم اعشار میگذارد و سه رقم سه رقم جدا میکند
                    //textBox.FormatString = "n";

                    //اگر اعشار خواست از پرسیجن و اسکیل استفاده شود
                    textBox.FormatString = "n0";
                }
                else
                {
                    
                    //رقم اعشار نمیگذارد و سه رقم سه رقم جدا میکند
                    textBox.FormatString = "n0";
                }
                بیشتر تست شود از نوع خود مقدار بایند شده به این کنترل
                //بعدا یک خصوصیت اضافه شود که جدا نکند مثل شماره شناسنامه
               
            // سه رقم سه رقم جدا نمیکند  
                // textBox.FormatString = "";

            }
            //}




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

        }

        //        < TextBlock Text = "Currency Pattern" />

        //< telerik:RadMaskedCurrencyInput Margin = "0,5,0,10"
        //                               Mask = "c3.3"
        //                               Value = "111.234" />
        //< TextBlock Text = "Decimal Pattern" />

        // < telerik:RadMaskedNumericInput Margin = "0,5,0,10"
        //                                Mask = "n3.3"
        //                                Value = "111.234" />
        //< TextBlock Text = "Percent Pattern" />

        // < telerik:RadMaskedNumericInput Margin = "0,5,0,10"
        //                                Mask = "p3.2"
        //                                Value = "111.234" />
        //< TextBlock Text = "Digit Pattern - d" />

        // < telerik:RadMaskedNumericInput Margin = "0,5,0,10"
        //                                Mask = "d3"
        //                                Value = "111.234" />
        //< TextBlock Text = "Digit Pattern - #" />

        // < telerik:RadMaskedNumericInput Margin = "0,5,0,10"
        //                                Mask = "#3.1"
        //                                Value = "111.234" />
        public void textBox_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            //ColumnValueChangeArg arg = new ColumnValueChangeArg();
            //arg.NewValue = GetValue(uiControlPackage);
            //uiControlPackage.OnValueChanged(sender, arg);
        }

        //internal static UIControlSetting GenerateUISetting(DataMaster.EntityDefinition.ND_Type_Property nD_Type_Property, UISetting.DataPackageUISetting.UI_PackagePropertySetting uI_PackagePropertySetting)
        //{
        //    throw new NotImplementedException();
        //}
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

        public bool SetValue(object value)
        {
            //FrameworkElement control;
            //control = (uiControl as Grid).Children[0] as RadMaskedNumericInput;
            //if (control is TextBlock)
            //{
            //    (control as TextBlock).Text = value;
            //}
            //else
            //{
            if (value == null)
                textBox.Value = null;
            else
            {
                if (value == "")
                    value = "0";
                textBox.Value = Convert.ToDouble(value);
            }

            //if (columnSetting != null)
            //    if (columnSetting.IsReadOnly)
            //        (control as RadMaskedNumericInput).IsReadOnly = true;
            //    else
            //        (control as RadMaskedNumericInput).IsReadOnly = false;

            //}
            return true;
        }

        public string GetValue()
        {
            //FrameworkElement control;
            //control = (uiControl as Grid).Children[0] as RadMaskedNumericInput;
            //if (control is TextBlock)
            //{
            //    return (control as TextBlock).Text;
            //}
            //else
            //{
            if (textBox.Value == null)
                return null;
            else
                return textBox.Value.ToString();
            //}
        }

        public void EnableDisable(bool enable)
        {
            textBox.IsEnabled = enable;
        }

        public void SetReadonly(bool isreadonly)
        {
            textBox.IsReadOnly = isreadonly;
        }


        public void Visiblity(bool visible)
        {
            textBox.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }
        public bool IsVisible()
        {
            return textBox.Visibility == Visibility.Visible;
        }
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

        public bool HasOperator()
        {
            return cmbOperators != null;
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
            textBox.SetBinding(RadMaskedNumericInput.ValueProperty, binding);
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


    }
}
