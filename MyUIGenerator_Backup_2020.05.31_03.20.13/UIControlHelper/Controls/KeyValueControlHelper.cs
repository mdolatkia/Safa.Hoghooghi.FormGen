
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
    public class KeyValueControlHelper : BaseControlHelper, I_ControlHelper, I_ControlHelperValueRange
    {
        ComboBox combo;
        ComboBox cmbOperators;

        public FrameworkElement MainControl { get { return combo; } }
        public FrameworkElement WholeControl { get { return theGrid; } }
        public KeyValueControlHelper()
        {
        //    ValueIsTitleOrValue = valueIsTitleOrValue;
            theGrid = new Grid();
            theGrid.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            theGrid.ColumnDefinitions.Add(new ColumnDefinition());
            combo = new ComboBox();
            combo.Name = "txtControl";
            //combo.SelectionChanged += Combo_SelectionChanged;
            //combo.SelectionChanged += (sender, e) => textBox_SelectionChanged(sender, e);
            //if (!columnSetting.GridView)
            //{
            //    //combo.Width = 200;
            //    combo.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            //}
            //else
            combo.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            combo.DisplayMemberPath = "KeyTitle";
            combo.SelectedValuePath = "Value";
            //combo.ItemsSource = KeyValues;
            //combo.IsReadOnly = columnSetting.IsReadOnly;
            //   combo.Margin = new System.Windows.Thickness(5);
            combo.MinHeight = 24;

            //     combo.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            theGrid.Children.Add(combo);
            //if (operators != null && operators.Count > 0)
            //{

            //    theGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(70) });
            //    cmbOperators = new ComboBox();
            //    cmbOperators.Name = "cmbOperators";
            //    cmbOperators.ItemsSource = operators;
            //    cmbOperators.DisplayMemberPath = "Title";
            //    cmbOperators.SelectedValuePath = "Operator";
            //    Grid.SetColumn(cmbOperators, 1);
            //    theGrid.Children.Add(cmbOperators);
            //}
            //}

        }
        //bool ValueIsTitleOrValue { set; get; }
        //public List<ColumnKeyValueRangeDetailsDTO> KeyValues { get; private set; }

        //public FrameworkElement GenerateControl(ColumnDTO correspondingTypeProperty, ColumnUISettingDTO columnSetting, List<SimpleSearchOperator> operators = null)
        //{

        //UIControlPackage package = new UIControlPackage();
        //package.UIControls = new List<FrameworkElement>();
        //UIControlSetting controlUISetting = new UIControlSetting();
        //controlUISetting.DesieredColumns = ColumnWidth.Normal;
        //controlUISetting.DesieredRows = 1;
        //if (columnSetting.IsReadOnly && columnSetting.LabelForReadOnlyText == true)
        //{
        //    var combo = new TextBlock();

        //    //combo.Margin = new System.Windows.Thickness(5);
        //    combo.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
        //    combo.VerticalAlignment = System.Windows.VerticalAlignment.Center;

        //    package.FrameworkElement = new FrameworkElement() { Control = combo, UIControlSetting = columnSetting.UISetting};
        //}
        //else
        //{

        //}

        //private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}

        //void textBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //////ColumnValueChangeArg arg = new ColumnValueChangeArg();
        //    //////arg.NewValue = GetValue(uiControlPackage);
        //    //////uiControlPackage.OnValueChanged(sender, arg);
        //}


        //public UIControlSetting GenerateUISetting(DataMaster.EntityDefinition.ND_Type_Property nD_Type_Property, UISetting.DataPackageUISetting.UI_PackagePropertySetting uI_PackagePropertySetting)
        //{
        //    throw new NotImplementedException();
        //}


        public bool SetValue(object value)
        {
            //var control = uiControl;

            //if (ValueIsTitleOrValue)
            //    combo.Text = value==null?"":value.ToString();
            //else
            //{
                if (value == null)
                    combo.SelectedValue = null;
                else
                    combo.SelectedValue = Convert.ToInt32(value);
            //}
            //if (columnSetting != null)
            //{
            //    if (columnSetting.IsReadOnly)
            //        combo.IsReadOnly = true;
            //    else
            //        combo.IsReadOnly = false;
            //}


            return true;
        }

        public object GetValue()
        {
            //var control = uiControl;
            //if (control is TextBlock)
            //{
            //    var text = (control as TextBlock).Text;
            //   ColumnKeyValueRangeDTO fItem = column.ColumnKeyValue.ColumnKeyValueRange.FirstOrDefault(x => x.KeyTitle == text);
            //    if (fItem != null)
            //    {
            //        if (column.ColumnKeyValue.ValueFromTitleOrValue)
            //            return fItem.KeyTitle;
            //        else
            //            return fItem.Value.ToString();
            //    }
            //    else
            //        return "";
            //}
            //else
            //{
            //if (column.ColumnKeyValue.ValueFromTitleOrValue)
            string value = "";
            string text = "";
            if (combo.Text != null)
                text = combo.Text;
            if (combo.SelectedValue != null)
                value = combo.SelectedValue.ToString();
            //if (ValueIsTitleOrValue)
            //    return text;
            //else
                return value;
            //else
            //    if (combo.SelectedValue != null)
            //        return combo.SelectedValue.ToString();
            //    else
            //        return "";
            //}
        }

        public void EnableDisable(bool enable)
        {
            combo.IsEnabled = enable;
        }
        public void Visiblity(bool visible)
        {
            combo.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }
        public bool IsVisible()
        {
            return combo.Visibility == Visibility.Visible;
        }
        public void SetReadonly(bool isreadonly)
        {
            // combo.IsReadOnly = isreadonly;
            combo.IsEnabled = !isreadonly;
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
        //public void SetTooltip( string tooltip)
        //{
        //    if (!string.IsNullOrEmpty(tooltip))
        //        ToolTipService.SetToolTip(combo, tooltip);
        //    else
        //        ToolTipService.SetToolTip(combo, null);
        //}

        //public void ClearTooltip()
        //{
        //    ToolTipService.SetToolTip(combo, null);
        //}

        public void SetBorderColor(InfoColor color)
        {
            combo.BorderBrush = UIManager.GetColorFromInfoColor(color);
            combo.BorderThickness = new Thickness(1);
        }
        public void SetBackgroundColor(InfoColor color)
        {
            combo.Background = UIManager.GetColorFromInfoColor(color);
        }
        public void SetForegroundColor(InfoColor color)
        {
            combo.Foreground = UIManager.GetColorFromInfoColor(color);
        }
        //public void ClearBorderColor()
        //{
        //    combo.BorderBrush = new SolidColorBrush(UIManager.GetColorFromInfoColor(InfoColor.Black));
        //    combo.BorderThickness = new Thickness(1);
        //}
        public void SetBinding(EntityInstanceProperty property)
        {
            Binding binding = new Binding("Value");
            binding.Source = property;
            //if (ValueIsTitleOrValue)
            //    combo.SetBinding(ComboBox.TextProperty, binding);
            //else
                combo.SetBinding(ComboBox.SelectedValueProperty, binding);



        }
        //public void AddButtonMenu( ConrolPackageMenu menu)
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

        public void SetColumnValueRange(List<ColumnValueRangeDetailsDTO> candidates)
        {
            combo.ItemsSource = candidates;
        }
    }
}
