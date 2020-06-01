using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace MyCommonWPFControls
{
    public abstract class MyBaseLookup : UserControl
    {
        DispatcherTimer timer = new DispatcherTimer();
        //public event EventHandler NewItemClicked;
        public event EventHandler<EditItemClickEventArg> EditItemClicked;
        public event EventHandler<SelectionChangedArg> SelectionChanged;
        public List<Dictionary<string, SimpleCondition>> EditFormConditions { set; get; }
        //public bool SearchConcurrent { set; get; }
        public ObservableCollection<LookupColumn> Columns { set; get; }
        public bool IsEnabledLookup
        {
            set
            {
                IsEnabled = value;
            }

            get
            {
                return IsEnabled;
            }
        }


        bool _NewItemEnabled;
        public bool NewItemEnabled
        {
            set
            {
                _NewItemEnabled = value;
                if (SelectedItem == null)
                    FormButton.IsEnabled = value;
            }

            get
            {
                return _NewItemEnabled;
            }
        }

        bool _EditItemEnabled;
        public bool EditItemEnabled
        {
            set
            {
                _EditItemEnabled = value;
                if (SelectedItem != null)
                    FormButton.IsEnabled = value;

            }
            get
            {
                return _EditItemEnabled;
            }
        }

        public bool ZeroIsAcceptedValue { set; get; }
        public List<string> SearchableColumns { set; get; }
        //public List<string> Columns { set; get; }
        //public bool AllDataArePresent { set; get; }

        public abstract void ClearSearchFunction();
        public abstract TextBox SearchTextbox { get; }
        public abstract Button FormButton { get; }
        public abstract Button PopupButton { get; }
        public abstract Image FormImage { get; }
        public abstract Popup PopupControl { get; }
        public abstract Button ClearButton { get; }
        public abstract RadGridView GridView { get; }
        public abstract UserControl MainUserControl { get; }
        public abstract void SearchFunction();
        public abstract object SelectedValue { set; get; }
        public bool internalSelectedValue = false;

        bool internalSelectItem = false;
        public object _SelectedItem;
        public object SelectedItem
        {

            set
            {
                if (_SelectedItem != value)
                {
                    //اگر از بیرون صدا زده شد باید چک شود که جزو لیست باشد
                    _SelectedItem = value;
                    if (value == null)
                    {
                        FormImage.Source = GetImageSource("/MyCommonWPFControls;component/Images/newform.png");
                        FormButton.IsEnabled = NewItemEnabled;
                    }
                    else
                    {
                        FormImage.Source = GetImageSource("/MyCommonWPFControls;component/Images/editform.png");
                        FormButton.IsEnabled = EditItemEnabled;
                    }

                    if (!internalSelectItem)
                    {
                        if (value != null)
                        {
                            ExternalSelectItem(value);
                        }
                        else
                            ClearItem(true);
                    }


                    if (SelectionChanged != null)
                        SelectionChanged(this, new SelectionChangedArg() { SelectedItem = SelectedItem });

                }
            }
            get
            {
                return _SelectedItem;
            }
        }




        public virtual IList ItemsSource
        {
            set; get;
        }
        public MyBaseLookup()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                (this as IComponentConnector).InitializeComponent();
                Columns = new ObservableCollection<MyCommonWPFControls.LookupColumn>();
                Columns.CollectionChanged += Columns_CollectionChanged;
                NewItemEnabled = false;
                MainUserControl.LostFocus += MyLookup_LostFocus;

                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick;

                FormButton.Click += FormButton_Click;
                PopupButton.Click += PopupButton_Click;
                ClearButton.Click += ClearButton_Click;
                GridView.KeyboardCommandProvider = new CustomKeyboardCommandProvider(GridView);
                GridView.AutoGenerateColumns = false;
                GridView.IsReadOnly = true;
                GridView.SelectionMode = SelectionMode.Single;
                GridView.AddHandler(GridViewRow.MouseUpEvent, new RoutedEventHandler(row_Selected), true);

                Binding myBinding = new Binding();
                myBinding.Source = MainUserControl;
                myBinding.Path = new PropertyPath("ActualWidth");
                myBinding.Mode = BindingMode.OneWay;
                myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                BindingOperations.SetBinding(GridView, RadGridView.WidthProperty, myBinding);

                GridView.KeyUp += GridView_KeyUp;
                SearchTextbox.TextChanged += SearchTextbox_TextChanged;
                SearchTextbox.KeyUp += SearchTextbox_KeyUp;
            }
        }



        //public void InitializeBaseControl()
        //{

        //}





        //public bool SearchExistringDataFirst { set; get; }






        private string _SelectedValueMember { set; get; }
        public string SelectedValueMember
        {
            set
            {
                _SelectedValueMember = value;
                AddColumn(value, "شناسه");
            }

            get
            {
                return _SelectedValueMember;
            }
        }
        private string _DisplayMember { set; get; }
        public string DisplayMember
        {

            set
            {
                _DisplayMember = value;
                AddColumn(value, "عنوان");
            }
            get
            {
                return _DisplayMember;
            }

        }

        private void SearchTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                Popup(true);
                GridView.Focus();
            }
            else if (e.Key == Key.Return)
            {
                if (PopupControl.IsOpen)
                {
                    if (GridView.ItemsSource is IList)
                        if ((GridView.ItemsSource as IList).Count == 1)
                        {
                            SelectedItem = (GridView.ItemsSource as IList)[0];
                        }
                }
            }
        }

        private void MyLookup_LostFocus(object sender, RoutedEventArgs e)
        {
            var focused_element = FocusManager.GetFocusedElement(Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive));
            if (focused_element != null)
            {
                var parent = (focused_element as FrameworkElement).TryFindParent<MyBaseLookup>();
                if (parent == null)
                {
                    Popup(false);
                }
            }
        }

        bool internalTextChanged = false;
        private void SearchTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!internalTextChanged)
            {

                if (SearchTextbox.Text == "")
                    ClearItem();
                else
                    SilentClearItem();

                timer.Stop();
                timer.Start();
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();
            SearchFunction();
        }

        public static List<object> GetFilteredItems(IList source, Dictionary<string, string> filters)
        {
            //and , or درست شود
            List<object> gridSource = new List<object>();
            if (source != null)
                foreach (var item in source)
                {
                    bool isValid = false;
                    foreach (var filter in filters)
                    {
                        if (CheckFilter(item, filter.Key, filter.Value))
                        {
                            isValid = true;
                            break;
                        }
                    }
                    if (isValid)
                        gridSource.Add(item);

                    //}

                }
            return gridSource;
        }


        private static bool CheckFilter(object item, string propertyName, object filtervalue)
        {
            List<LookupPropertyInfo> PropertyInfos = new List<LookupPropertyInfo>();
            var lookupPropertyInfo = PropertyInfos.FirstOrDefault(x => x.PropertyName == propertyName);
            if (lookupPropertyInfo == null)
            {
                PropertyInfo propertyInfo = item.GetType().GetProperty(propertyName);
                lookupPropertyInfo = new LookupPropertyInfo();
                lookupPropertyInfo.PropertyName = propertyName;
                lookupPropertyInfo.Property = propertyInfo;
                lookupPropertyInfo.PropertyType = propertyInfo.PropertyType;
                if (IsNumericType(propertyInfo.PropertyType))
                    lookupPropertyInfo.ExactValueProperty = true;
                else if (propertyInfo.PropertyType == typeof(bool))
                    lookupPropertyInfo.ExactValueProperty = true;
                PropertyInfos.Add(lookupPropertyInfo);
            }
            var value = lookupPropertyInfo.Property.GetValue(item);
            if (lookupPropertyInfo.ExactValueProperty)
            {
                if (value != null && filtervalue != null)
                    return value.ToString() == filtervalue.ToString();
            }
            else
            {
                if (value != null && filtervalue != null)
                    return value.ToString().ToLower().Contains(filtervalue.ToString().ToLower());
            }
            return false;
        }

        public static bool IsNumericType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
        private void GridView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (GridView.SelectedItem != null)
                {
                    SelectedItem = GridView.SelectedItem;
                    Popup(false);
                }
            }
        }
        void row_Selected(object sender, RoutedEventArgs e)
        {
            if (GridView.SelectedItem != null)
            {
                SelectedItem = GridView.SelectedItem;
                Popup(false);
            }
        }

        private ImageSource GetImageSource(string path)
        {
            Image img = new Image();
            img.Width = 15;
            Uri uriSource = new Uri(path, UriKind.Relative);
            return new BitmapImage(uriSource);
        }
        private void ExternalSelectItem(object item)
        {
            object displayName = item.GetType().GetProperty(DisplayMember).GetValue(item, null);

            if (displayName != null)
            {
                internalTextChanged = true;
                SearchTextbox.Text = displayName.ToString();
                internalTextChanged = false;
            }
            object selectedValue = item.GetType().GetProperty(SelectedValueMember).GetValue(item, null);
            internalSelectedValue = true;
            SelectedValue = selectedValue;
            internalSelectedValue = false;

            ClearSearchFunction();
            Popup(false);
        }

        public void Popup(bool isOpen)
        {
            PopupControl.IsOpen = isOpen;
        }
        private void ClearItem(bool fromSelectedItem = false)
        {
            internalTextChanged = true;
            SearchTextbox.Text = "";
            internalTextChanged = false;
            internalSelectedValue = true;
            SelectedValue = null;
            internalSelectedValue = false;
            if (!fromSelectedItem)
            {
                internalSelectItem = true;
                SelectedItem = null;
                internalSelectItem = false;
            }

            ClearSearchFunction();
            Popup(false);
        }
        public void SilentClearItem()
        {
            internalSelectItem = true;
            SelectedItem = null;
            internalSelectItem = false;

            internalSelectedValue = true;
            SelectedValue = null;
            internalSelectedValue = true;
        }
        private void FormButton_Click(object sender, RoutedEventArgs e)
        {
            if (EditItemClicked != null)
            {
                var arg = new EditItemClickEventArg();
                EditItemClicked(this, arg);
            }
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearItem();
        }
        private void PopupButton_Click(object sender, RoutedEventArgs e)
        {
            Popup(!PopupControl.IsOpen);
            if (PopupControl.IsOpen)
                GridView.Focus();
        }
        public void AddColumn(string columnName, string header)
        {
            if (!Columns.Any(x => x.ColumnName == columnName))
                Columns.Add(new MyCommonWPFControls.LookupColumn() { ColumnName = columnName, Header = header });
        }
        private void Columns_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (LookupColumn item in e.NewItems)
                {
                    AddOrUpdateGridViewColumn(item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (LookupColumn item in e.OldItems)
                {
                    RemoveGridViewColumn(item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
            {
                GridView.Columns.Clear();
            }
        }

        private void RemoveGridViewColumn(LookupColumn item)
        {
            if (GridView.Columns.Cast<GridViewDataColumn>().Any(x => x.UniqueName == item.ColumnName))
            {
                var gridColumn = GridView.Columns.Cast<GridViewDataColumn>().First(x => x.UniqueName == item.ColumnName);
                GridView.Columns.Remove(gridColumn);
            }
        }

        private void AddOrUpdateGridViewColumn(LookupColumn item)
        {
            GridViewDataColumn gridColumn = null;
            if (GridView.Columns.Cast<GridViewDataColumn>().Any(x => x.UniqueName == item.ColumnName))
                gridColumn = GridView.Columns.Cast<GridViewDataColumn>().First(x => x.UniqueName == item.ColumnName);
            else
            {
                gridColumn = new GridViewDataColumn();
                gridColumn.DataMemberBinding = new Binding(item.ColumnName);
                gridColumn.UniqueName = item.ColumnName;
                GridView.Columns.Add(gridColumn);
            }
            gridColumn.Header = item.Header;
        }



        //public GridViewDataColumn GetColumn(string columnName)
        //{

        //    foreach (var column in GridView.Columns)
        //    {
        //        if (column.UniqueName == columnName)
        //            return column as GridViewDataColumn;
        //    }
        //    return null;
        //}
    }

    public class SearchFilterArg : EventArgs
    {
        //این دوتا با هم معنی دارند
        //اگر FilerBySelectedValue 
        //یعنی در تکست باکس کلی لوکاپ چیزی تایپ شده
        //اگر ترو باشد یعنی توسط سلکتد ولیو پر صدا زده شده است
        public string SingleFilterValue { set; get; }
        public bool FilterBySelectedValue { set; get; }
        //

        public IList ResultItemsSource { set; get; }


        //      public Dictionary<string, string> Filters { get; internal set; }
    }
    public class EditItemClickEventArg : EventArgs
    {//برای گرید
        public object DataConext { set; get; }
    }
    public class SimpleCondition
    {
        public string PropertyName { set; get; }
        public string Value { set; get; }
    }
    public class SelectionChangedArg : EventArgs
    {
        public object SelectedItem { set; get; }
    }
    public class LookupPropertyInfo
    {
        public PropertyInfo Property { set; get; }
        public string PropertyName { set; get; }
        public bool ExactValueProperty { set; get; }
        public Type PropertyType { set; get; }
    }

    public static class VisualTreeHelperExt
    {
        public static T TryFindParent<T>(this DependencyObject child)
    where T : DependencyObject
        {
            DependencyObject parentObject = GetParentObject(child);
            if (parentObject == null) return null;
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return TryFindParent<T>(parentObject);
            }
        }

        public static DependencyObject GetParentObject(this DependencyObject child)
        {
            if (child == null) return null;
            ContentElement contentElement = child as ContentElement;
            if (contentElement != null)
            {
                DependencyObject parent = ContentOperations.GetParent(contentElement);
                if (parent != null) return parent;
                FrameworkContentElement fce = contentElement as FrameworkContentElement;
                return fce != null ? fce.Parent : null;
            }
            FrameworkElement frameworkElement = child as FrameworkElement;
            if (frameworkElement != null)
            {
                DependencyObject parent = frameworkElement.Parent;
                if (parent != null) return parent;
            }
            return VisualTreeHelper.GetParent(child);
        }
    }
    public class LookupColumn
    {
        public string ColumnName { set; get; }
        public string Header { set; get; }
        public bool UseInSearch { get; set; }
    }
}
