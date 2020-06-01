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
    /// Interaction logic for MyStaticLookup.xaml
    /// </summary>
    public partial class MyStaticLookup : MyBaseLookup
    {
        public MyStaticLookup() : base()
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
                        if (SelectedValueMember != null)
                        {
                            var filter = new Dictionary<string, string>();
                            filter.Add(SelectedValueMember, value.ToString());
                            var items = GetFilteredItems(_BaseItemsSource as IList, filter);
                            if (filter.Count > 0 && items.Any())
                            {
                                SelectedItem = items.First();
                            }
                            else
                                SelectedItem = null;
                        }
                        else
                            SelectedItem = null;
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
            Dictionary<string, string> filters = new Dictionary<string, string>();
            var generalFilterValue = txtCombo.Text;
            if (generalFilterValue != "")
            {
                foreach (var item in Columns.Where(x => x.UseInSearch))
                    filters.Add(item.ColumnName, generalFilterValue);

                if (!filters.Any(x => x.Key == DisplayMember))
                    filters.Add(DisplayMember, generalFilterValue);
                if (!filters.Any(x => x.Key == SelectedValueMember))
                    filters.Add(SelectedValueMember, generalFilterValue);
            }
            if (filters.Count > 0)
            {
                var gridSource = GetFilteredItems(_BaseItemsSource as IList, filters);
                dtgItems.ItemsSource = gridSource;
            }
            else
                dtgItems.ItemsSource = _BaseItemsSource;
            Popup(true);
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
    }



}
