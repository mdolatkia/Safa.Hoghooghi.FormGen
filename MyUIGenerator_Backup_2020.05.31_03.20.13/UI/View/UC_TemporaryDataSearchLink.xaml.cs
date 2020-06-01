
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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProxyLibrary;
using System.Windows.Threading;
using MyUILibrary.Temp;

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for UC_TemporaryViewLink.xaml
    /// </summary>
    public partial class UC_TemporaryDataSearchLink : UserControl, I_View_TemporaryView
    {
        DispatcherTimer timer = new DispatcherTimer();

        public UC_TemporaryDataSearchLink(TemporaryLinkState temporaryLinkState)
        {
            InitializeComponent();
            //this.Margin = new Thickness(2);

            //LinkType = linkType;
            //btnLinkClear.Visibility = Visibility.Collapsed;
            //btnLinkInfo.Visibility = Visibility.Collapsed;
            btnPopup.Visibility = temporaryLinkState.popup ? Visibility.Visible : Visibility.Collapsed;
            btnQuickSearch.Visibility = temporaryLinkState.quickSearch ? Visibility.Visible : Visibility.Collapsed;
            txtSearch.Visibility = temporaryLinkState.quickSearch ? Visibility.Visible : Visibility.Collapsed;
            btnLink.Visibility = temporaryLinkState.edit ? Visibility.Visible : Visibility.Collapsed;
            btnLinkSearch.Visibility = temporaryLinkState.searchView ? Visibility.Visible : Visibility.Collapsed;
            btnLinkClear.Visibility = temporaryLinkState.clear ? Visibility.Visible : Visibility.Collapsed;
            btnLinkInfo.Visibility = temporaryLinkState.info ? Visibility.Visible : Visibility.Collapsed;

            this.LostFocus += UC_TemporaryDataSearchLink_LostFocus;
            //if (LinkType == TemporaryLinkType.SerachView)

            //{
            //    btnLink.Visibility = System.Windows.Visibility.Collapsed;
            //}
            //else if (LinkType == TemporaryLinkType.DataView)
            //{
            //    btnQuickSearch.Visibility = System.Windows.Visibility.Collapsed;
            //    btnLinkSearch.Visibility = System.Windows.Visibility.Collapsed;
            //}
            //else if (LinkType == TemporaryLinkType.DataSearchView)
            //{
            //    //btnLink.Visibility = System.Windows.Visibility.Collapsed;
            //    //btnLinkSearch.Visibility = System.Windows.Visibility.Collapsed;
            //}
            txtSearch.TextChanged += TxtSearch_TextChanged;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Tick += Timer_Tick;
        }

        private void UC_TemporaryDataSearchLink_LostFocus(object sender, RoutedEventArgs e)
        {
            var aa = Keyboard.FocusedElement;
            bool focusIsInside = true;
            if (aa != null)
            {
                if (aa is FrameworkElement)
                {
                    if (!ElementOrParentIsInside(aa as FrameworkElement))
                        focusIsInside = false;
                }
            }
            //   FrameworkElement focusedElement = FocusManager.GetFocusedElement(popup1) as FrameworkElement;
            if (!focusIsInside)
            {
                if (FocusLost != null)
                    FocusLost(this, null);
            }
        }
        private bool ElementOrParentIsInside(FrameworkElement uIElement)
        {
            if (uIElement == this)
                return true;
            else if (uIElement.Parent != null && uIElement.Parent is FrameworkElement)
                return ElementOrParentIsInside(uIElement.Parent as FrameworkElement);
            else
            {
                var parent = VisualTreeHelper.GetParent(uIElement);
                if (parent != null && parent is FrameworkElement)
                    return ElementOrParentIsInside(parent as FrameworkElement);
                else
                    return false;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();
            if (SearchTextChanged != null)
                SearchTextChanged(this, new Arg_TemporaryDisplaySerachText() { Text = txtSearch.Text });


        }
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            timer.Start();
        }
        public event EventHandler FocusLost;
        public event EventHandler<Arg_TemporaryDisplayViewRequested> TemporaryDisplayViewRequested;
        public event EventHandler<Arg_TemporaryDisplaySerachText> SearchTextChanged;

        //public event EventHandler<Arg_TemporaryDisplayViewRequested> TemporarySearchViewRequested;

        public void SetLinkText(string text)
        {
            txtInfo.Text = text;
        }

        private void btnLink_Click(object sender, RoutedEventArgs e)
        {
            if (TemporaryDisplayViewRequested != null)
            {
                TemporaryDisplayViewRequested(this, new Arg_TemporaryDisplayViewRequested() { LinkType = TemporaryLinkType.DataView });

            }
        }

        //public DP_DataRepository ParentDataItem
        //{
        //    set;
        //    get;
        //}

        private void btnLinkSearch_Click(object sender, RoutedEventArgs e)
        {
            if (TemporaryDisplayViewRequested != null)
            {
                TemporaryDisplayViewRequested(this, new Arg_TemporaryDisplayViewRequested() { LinkType = TemporaryLinkType.SerachView });

            }
        }
        public void SetTooltip(object dataItem, string tooltip)
        {
            if (!string.IsNullOrEmpty(tooltip))
                ToolTipService.SetToolTip(this, tooltip);
            else
                ToolTipService.SetToolTip(this, null);
        }

        public void SetBorderColor(object dataItem, InfoColor color)
        {
            this.BorderBrush = UIManager.GetColorFromInfoColor(color);
            this.BorderThickness = new Thickness(1);
        }

        public void SetBackgroundColor(object dataItem, InfoColor color)
        {
            this.Background = UIManager.GetColorFromInfoColor(color);
        }

        public void SetForegroundColor(object dataItem, InfoColor color)
        {
            this.Foreground = UIManager.GetColorFromInfoColor(color);
        }
        //public object SearchDataItem
        //{
        //    set;
        //    get;
        //}

        //public void SetSearchLinkText(string text)
        //{
        //    btnLinkSearch.Content = text;
        //}

        //public ColumnDTO Column
        //{
        //    set;
        //    get;
        //}

        //   public TemporaryLinkType LinkType { get; }



        public List<DP_DataRepository> CurrentDataItems
        {
            set; get;
        }



        private void btnLinkInfo_Click(object sender, RoutedEventArgs e)
        {
            if (TemporaryDisplayViewRequested != null)
            {
                TemporaryDisplayViewRequested(this, new Arg_TemporaryDisplayViewRequested() { LinkType = TemporaryLinkType.Info });
            }
        }

        private void btnLinkClear_Click(object sender, RoutedEventArgs e)
        {
            if (TemporaryDisplayViewRequested != null)
            {
                TemporaryDisplayViewRequested(this, new Arg_TemporaryDisplayViewRequested() { LinkType = TemporaryLinkType.Clear });
            }
        }

        public void DisableEnable(bool enable)
        {
            this.IsEnabled = enable;
        }

        public void DisableEnable(TemporaryLinkType link, bool enable)
        {
            if (link == TemporaryLinkType.SerachView)
            {
                btnLinkSearch.IsEnabled = enable;
            }
            else if (link == TemporaryLinkType.DataView)
            {
                btnLink.IsEnabled = enable;
            }
            else if (link == TemporaryLinkType.Clear)
            {
                btnLinkClear.IsEnabled = enable;
            }
            else if (link == TemporaryLinkType.QuickSearch)
            {
                btnQuickSearch.IsEnabled = enable;
            }
            else if (link == TemporaryLinkType.Popup)
            {
                btnPopup.IsEnabled = enable;
            }
        }

        //public void SetComboBoxVisibile(bool combo)
        //{
        //    if (combo)
        //    {
        //        cmbItems.Visibility = Visibility.Visible;
        //        txtInfo.Visibility = Visibility.Collapsed;
        //    }
        //    else
        //    {
        //        cmbItems.Visibility = Visibility.Collapsed;
        //        txtInfo.Visibility = Visibility.Visible;
        //    }
        //}

        //public void SetTextReadonly(bool isreadonly)
        //{
        //    txtInfo.IsReadOnly = isreadonly;
        //}

        public string GetSearchText()
        {
            return txtSearch.Text;
        }

        public void RemovePopupView(I_View_ViewEntityArea viewView)
        {
            popup1.Child = null;
        }

        public bool PopupVisibility
        {
            get
            {
                return popup1.IsOpen;
            }
            set
            {
                popup1.IsOpen = value;
            }
        }

        public bool HasPopupView
        {
            get
            {
                return popup1.Child != null;
            }
        }

        public bool QuickSearchVisibility
        {
            get
            {
                return txtSearch.Visibility == Visibility.Visible;
            }

            set
            {
                txtSearch.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public void AddPopupView(I_View_ViewEntityArea viewView)
        {
            popup1.Child = viewView as UIElement;
        }

        private void btnQuickSearch_Click(object sender, RoutedEventArgs e)
        {
            if (TemporaryDisplayViewRequested != null)
            {
                TemporaryDisplayViewRequested(this, new Arg_TemporaryDisplayViewRequested() { LinkType = TemporaryLinkType.QuickSearch });

            }
        }

        private void btnPopup_Click(object sender, RoutedEventArgs e)
        {
            if (TemporaryDisplayViewRequested != null)
            {
                TemporaryDisplayViewRequested(this, new Arg_TemporaryDisplayViewRequested() { LinkType = TemporaryLinkType.Popup });
            }
        }

        public void QuickSearchSelectAll()
        {
            txtSearch.SelectAll();
            txtSearch.Focus();
        }

        public void ClearSearchText()
        {
            txtSearch.Text = "";
        }
    }
}
