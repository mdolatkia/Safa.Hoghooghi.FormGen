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
    public class MySearchLookupColumn : GridViewComboBoxColumn
    {
        public bool NewItemEnabled { set; get; }
        public bool EditItemEnabled { set; get; }
        public event EventHandler<SearchFilterArg> SearchFilterChanged;
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
            var value = GetDisplayPropertyValueFromItemsource(cell.DataContext);
            if (value != null)
                cellElement.Text = value.ToString();
            else
                cellElement.Text = "";
            return cellElement;
        }
        //private Dictionary<object,>
        private object GetDisplayPropertyValueFromItemsource(object dataContext)
        {
            if (dataContext != null)
            {
                var property = dataContext.GetType().GetProperty(this.DataMemberBinding.Path.Path);
                if (property != null)
                {
                    var bindvalue = property.GetValue(dataContext);
                    if (bindvalue != null)
                    {
                        object fItem = null;
                        var fItems = FindItemInItemsSource(ItemsSource as IList, bindvalue.ToString());
                        if (fItems.Any())
                        {
                            fItem = fItems.First();
                        }
                        else if (SearchFilterChanged != null)
                        {
                            var filterArg = new MyCommonWPFControls.SearchFilterArg();
                            filterArg.FilterBySelectedValue = true;
                            filterArg.SingleFilterValue = bindvalue.ToString();
                            SearchFilterChanged(this, filterArg);
                            var displayItemsSource = filterArg.ResultItemsSource;
                            if (displayItemsSource != null && displayItemsSource.Count > 0)
                            {
                                fItem = displayItemsSource[0];
                                AppendToItemsSource(fItem);
                            }
                        }
                        if (fItem != null)
                        {
                            var sourcevalueproperty = fItem.GetType().GetProperty(this.SelectedValueMemberPath);
                            if (sourcevalueproperty != null)
                            {
                                var value = sourcevalueproperty.GetValue(fItem);
                                if (value != null)
                                    if (bindvalue.ToString() == value.ToString())
                                    {
                                        var sourcedisplayroperty = fItem.GetType().GetProperty(this.DisplayMemberPath);
                                        if (sourcedisplayroperty != null)
                                            return sourcedisplayroperty.GetValue(fItem);
                                    }
                            }
                        }
                    }
                }

            }

            return null;
        }
        //ایتم سورس فقط اونایین که تو جدول استفاده شده اند
        private void AppendToItemsSource(object selectedItem)
        {
            if (ItemsSource == null)
                ItemsSource = new List<object>();
            if (!(ItemsSource as IList).Contains(selectedItem))
                (ItemsSource as IList).Add(selectedItem);
        }
        private List<object> FindItemInItemsSource(IList itemsSource, string bindvalue)
        {
            if (itemsSource != null)
            {
                var filter = new Dictionary<string, string>();
                filter.Add(SelectedValueMemberPath, bindvalue.ToString());
                return MyBaseLookup.GetFilteredItems(itemsSource, filter);
            }
            else
                return new List<object>();
        }

        //MyStaticLookup cellEditElement = null;
        bool internalChange = false;
        private IList lastSearchItemsSource;
        private IList lookupItemsSource;
        public override FrameworkElement CreateCellEditElement(GridViewCell cell, object dataItem)
        {


            if (cell.DataContext != null)
            {
                var property = cell.DataContext.GetType().GetProperty(this.DataMemberBinding.Path.Path);
                if (property != null)
                {
                    var searchLookup = new MySearchLookup();
                    searchLookup.NewItemEnabled = NewItemEnabled;
                    searchLookup.EditItemEnabled = EditItemEnabled;

                    searchLookup.SearchFilterChanged += CellEditElement_SearchFilterChanged;
                    searchLookup.DisplayMember = this.DisplayMemberPath;
                    searchLookup.SelectedValueMember = this.SelectedValueMemberPath;

                    if (lastSearchItemsSource == null)
                        lookupItemsSource = ItemsSource as IList;
                    else
                        lookupItemsSource = MergeSources(ItemsSource as IList, lastSearchItemsSource);
                    searchLookup.ItemsSource = lookupItemsSource;

                    searchLookup.SelectionChanged += (sender, e) => CellEditElement_SelectionChanged(sender, e, cell.DataContext);
                    //searchLookup.Width = this.ActualWidth;
                    searchLookup.HorizontalAlignment = HorizontalAlignment.Stretch;
                    var bindvalue = property.GetValue(cell.DataContext);


                    internalChange = true;
                    searchLookup.SelectedValue = bindvalue;
                    //ایونت CellEditElement_SearchFilterChanged صدا زده میشود
                    internalChange = false;

                    return searchLookup as FrameworkElement;
                }
            }
            return null;
            //this.BindingTarget = RadColorPicker.SelectedColorProperty;

            //System.Windows.Data.Binding valueBinding = this.CreateValueBinding();
            //cellEditElement.SetBinding(RadColorPicker.SelectedColorProperty, valueBinding);

        }

        private IList MergeSources(IList list, IList lookupItemsSource)
        {
            var result = new List<object>();
            foreach (var item in lookupItemsSource)
                result.Add(item);
            foreach (var item in list)
            {
                bool repeated = false;
                foreach (var litem in lookupItemsSource)
                {
                    var lproperty = litem.GetType().GetProperty(this.SelectedValueMemberPath);
                    if (lproperty != null)
                    {
                        var iproperty = item.GetType().GetProperty(this.SelectedValueMemberPath);
                        if (iproperty != null)
                        {
                            var lvalue = lproperty.GetValue(litem);
                            var ivalue = iproperty.GetValue(litem);
                            if (lvalue != null && ivalue != null)
                                if (lvalue.ToString() == ivalue.ToString())
                                    repeated = true;
                        }
                    }
                }
                if (!repeated)
                    result.Add(item);
            }
            return result;
        }

        private void CellEditElement_SearchFilterChanged(object sender, SearchFilterArg e)
        {
            if (e.FilterBySelectedValue)
            {
                //یعنی از داخل صدا زده شده..بالایی
                var foundItems = FindItemInItemsSource(lookupItemsSource, e.SingleFilterValue);
                if (foundItems.Count == 0)
                {

                    //قاعدتا اینجا نباید هیچوقت بیاد
                    //چون در موقع نمایش ایتم سورس با تمام مقادیر ساخته شده
                    var filterArg = new MyCommonWPFControls.SearchFilterArg();
                    filterArg.FilterBySelectedValue = true;
                    filterArg.SingleFilterValue = e.SingleFilterValue.ToString();
                    SearchFilterChanged(this, filterArg);
                    if (lastSearchItemsSource == null)
                        lookupItemsSource = filterArg.ResultItemsSource;
                    else
                        lookupItemsSource = MergeSources(filterArg.ResultItemsSource, lookupItemsSource);

                    e.ResultItemsSource = lookupItemsSource;
                }
                else
                    e.ResultItemsSource = lookupItemsSource;

            }
            else if (SearchFilterChanged != null)
            {
                //با خود سرچ لوکاپ میاد اینجا
                SearchFilterChanged(sender, e);
              lastSearchItemsSource = e.ResultItemsSource;
            }
        }

        private void CellEditElement_SelectionChanged(object sender, SelectionChangedArg e, object dataContext)
        {
            if (internalChange)
                return;
            //فقط وقتی کاربر انتخاب میکند باید اینجا بیاد
            var property = dataContext.GetType().GetProperty(this.DataMemberBinding.Path.Path);
            if (property != null)
            {
                if (e.SelectedItem != null)
                {
                    var sourcevalueproperty = e.SelectedItem.GetType().GetProperty(this.SelectedValueMemberPath);
                    if (sourcevalueproperty != null)
                    {
                        //AppendToItemsSource(e.SelectedItem);
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
