using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace MyCommonWPFControls
{
    public class MyStaticLookupColumn : GridViewComboBoxColumn
    {
        public bool NewItemEnabled { set; get; }
        public bool EditItemEnabled { set; get; }
        public event EventHandler<EditItemClickEventArg> EditItemClicked;
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
            var cellElement = new TextBlock();
            IEnumerable displayItemsSource = null;
            if (ItemsSourceBinding != null)
            {
                displayItemsSource = BindingExpressionHelper.GetValue(dataItem, ItemsSourceBinding) as IEnumerable;
            }
            else
                displayItemsSource = ItemsSource;
            var value = GetPropertyValue(displayItemsSource, cell.DataContext);
            if (value != null)
                cellElement.Text = value.ToString();
            else
                cellElement.Text = "";
            return cellElement;
        }

        private object GetPropertyValue(IEnumerable displayItemsSource, object dataContext)
        {
            if (dataContext != null)
            {
                var property = dataContext.GetType().GetProperty(this.DataMemberBinding.Path.Path);
                if (property != null)
                {
                    var bindvalue = property.GetValue(dataContext);
                    if (bindvalue != null)
                    {
                        if (displayItemsSource != null)
                            foreach (var item in displayItemsSource)
                            {
                                var sourcevalueproperty = item.GetType().GetProperty(this.SelectedValueMemberPath);
                                if (sourcevalueproperty != null)
                                {
                                    var value = sourcevalueproperty.GetValue(item);
                                    if (value != null)
                                        if (bindvalue.ToString() == value.ToString())
                                        {
                                            var sourcedisplayroperty = item.GetType().GetProperty(this.DisplayMemberPath);
                                            if (sourcedisplayroperty != null)
                                                return sourcedisplayroperty.GetValue(item);
                                        }
                                }
                            }
                    }
                }
            }
            return null;
        }
        //MyStaticLookup cellEditElement = null;
        bool internalChange = false;
        public override FrameworkElement CreateCellEditElement(GridViewCell cell, object dataItem)
        {

            var cellEditElement = new MyStaticLookup();
            cellEditElement.NewItemEnabled = NewItemEnabled;
            cellEditElement.EditItemEnabled = EditItemEnabled;

            cellEditElement.DisplayMember = this.DisplayMemberPath;
            cellEditElement.SelectedValueMember = this.SelectedValueMemberPath;

            IList itemsSource = null;
            if (ItemsSourceBinding != null)
            {
                itemsSource = BindingExpressionHelper.GetValue(dataItem, ItemsSourceBinding) as IList;
            }
            else
                itemsSource = ItemsSource as IList;

            cellEditElement.ItemsSource = itemsSource as IList;
            cellEditElement.SelectionChanged += (sender, e) => CellEditElement_SelectionChanged(sender, e, cell.DataContext);
            cellEditElement.EditItemClicked += (sender, e) => CellEditElement_EditItemClicked(sender, e, dataItem);
            //cellEditElement.Width = this.ActualWidth;
            cellEditElement.HorizontalAlignment = HorizontalAlignment.Stretch;

            if (cell.DataContext != null)
            {
                var property = cell.DataContext.GetType().GetProperty(this.DataMemberBinding.Path.Path);
                if (property != null)
                {
                    var bindvalue = property.GetValue(cell.DataContext);
                    internalChange = true;
                    cellEditElement.SelectedValue = bindvalue;
                    internalChange = false;
                }
            }

            //this.BindingTarget = RadColorPicker.SelectedColorProperty;

            //System.Windows.Data.Binding valueBinding = this.CreateValueBinding();
            //cellEditElement.SetBinding(RadColorPicker.SelectedColorProperty, valueBinding);
            return cellEditElement as FrameworkElement;
        }

        private void CellEditElement_EditItemClicked(object sender, EditItemClickEventArg e, object dataItem)
        {
            if (EditItemClicked != null)
            {
                e.DataConext = dataItem;
                EditItemClicked(sender, e);
            }
        }

        private void CellEditElement_SelectionChanged(object sender, SelectionChangedArg e, object dataContext)
        {
            if (internalChange)
                return;
            var property = dataContext.GetType().GetProperty(this.DataMemberBinding.Path.Path);
            if (property != null)
            {
                if (e.SelectedItem != null)
                {
                    var sourcevalueproperty = e.SelectedItem.GetType().GetProperty(this.SelectedValueMemberPath);
                    if (sourcevalueproperty != null)
                    {
                        var value = sourcevalueproperty.GetValue(e.SelectedItem);
                        property.SetValue(dataContext, value);
                    }

                }
                else
                {
                    if (property.PropertyType == typeof(string))
                        property.SetValue(dataContext, null);
                    else if (MyBaseLookup.IsNumericType(property.PropertyType))
                        property.SetValue(dataContext, 0);
                }

            }
        }

        private System.Windows.Data.Binding CreateValueBinding()
        {
            System.Windows.Data.Binding valueBinding = new System.Windows.Data.Binding();
            valueBinding.Mode = BindingMode.TwoWay;
            valueBinding.NotifyOnValidationError = true;
            valueBinding.ValidatesOnExceptions = true;
            valueBinding.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;
            valueBinding.Path = new PropertyPath(this.DataMemberBinding.Path.Path);
            return valueBinding;
        }
        //public override void CopyPropertiesFrom(Telerik.Windows.Controls.GridViewColumn source)
        //{
        //    base.CopyPropertiesFrom(source);
        //    //var radColorPickerColumn = source as RadColorPickerColumn;
        //    //if (radColorPickerColumn != null)
        //    //{
        //    //    this.MainPalette = radColorPickerColumn.MainPalette;
        //    //}
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
