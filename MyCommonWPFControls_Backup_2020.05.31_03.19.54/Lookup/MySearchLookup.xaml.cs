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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace MyCommonWPFControls
{
    /// <summary>
    /// Interaction logic for MySearchLookup.xaml
    /// </summary>
    public partial class MySearchLookup : MyBaseLookup
    {
        public event EventHandler<SearchFilterArg> SearchFilterChanged;
        public MySearchLookup() : base()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                InitializeComponent();
        }

        object _SelectedValue;
        public override object SelectedValue
        {
            set
            {
                _SelectedValue = value;
                if (!internalSelectedValue)
                {
                    if (value == null || value.ToString() == "" || (!ZeroIsAcceptedValue && value.ToString() == "0"))
                    {
                        SelectedItem = null;
                    }
                    else
                    {

                        if (SearchFilterChanged != null)
                        {
                            var filterArg = new MyCommonWPFControls.SearchFilterArg();
                            filterArg.FilterBySelectedValue = true;
                            filterArg.SingleFilterValue = value.ToString();
                            SearchFilterChanged(this, filterArg);
                            ItemsSource = filterArg.ResultItemsSource;

                            var filter = new Dictionary<string, string>();
                            filter.Add(SelectedValueMember, value.ToString());
                            var items = GetFilteredItems(ItemsSource, filter);
                            if (items.Count > 0)
                            {
                                SelectedItem = items.First();
                            }
                            else
                                SelectedItem = null;
                        }
                    }
                }
            }
            get
            {
                if (SelectedItem != null)
                    return _SelectedValue;
                else
                    return null;
            }
        }
        public override void SearchFunction()
        {
            var generalFilterValue = "";

            if (SearchTextbox.Text != "")
                generalFilterValue = SearchTextbox.Text;

            Dictionary<string, string> filters = new Dictionary<string, string>();
            //?بقیه فیلترها

            if (SearchFilterChanged != null)
            {
                var filterArg = new MyCommonWPFControls.SearchFilterArg();
                filterArg.SingleFilterValue = generalFilterValue;
             //   filterArg.Filters = filters;
                SearchFilterChanged(this, filterArg);
                ItemsSource = filterArg.ResultItemsSource;
                Popup(true);
            }

        }

        public override void ClearSearchFunction()
        {
            dtgItems.ItemsSource = _BaseItemsSource;
        }
        object _BaseItemsSource { set; get; }
        IList _ItemsSource { set; get; }
        public override IList ItemsSource
        {
            set
            {
                _ItemsSource = value;
                dtgItems.ItemsSource = value;
                _BaseItemsSource = value;
            }
            get
            {
                return _ItemsSource;
            }
        }
        public override TextBox SearchTextbox
        {
            get
            {
                return txtCombo;
            }
        }
        public override Button FormButton
        {
            get
            {
                return btnForm;
            }
        }
        public override Button PopupButton
        {
            get
            {
                return btnPopup;
            }
        }
        public override Image FormImage
        {
            get
            {
                return imgForm;
            }
        }

        public override Popup PopupControl
        {
            get
            {
                return popup;
            }
        }

        public override Button ClearButton
        {
            get
            {
                return btnClear;
            }
        }

        public override RadGridView GridView
        {
            get
            {
                return dtgItems;
            }
        }

        public override UserControl MainUserControl
        {
            get
            {
                return this;
            }
        }

        public bool SearchIsEnabled
        {
            get { return txtCombo.IsEnabled; }
            set
            {
                txtCombo.IsEnabled = value;
            }
        }
    }



}
