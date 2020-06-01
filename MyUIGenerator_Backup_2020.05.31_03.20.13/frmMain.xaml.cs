

using ModelEntites;
using ProxyLibrary;
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
using System.Windows.Shapes;
using Telerik.Windows.Controls;

using MyUIGenerator.View;
using MyUIGenerator;
using MyUILibrary;
using System.Windows.Threading;

namespace WPF_MyIdea
{
    /// <summary>
    /// Interaction logic for frmMain.xaml
    /// </summary>
    public partial class frmMain : Window
    {

        UIManager UIManager;
        public frmMain(UIManager uiMnager)
        {
            InitializeComponent();
            UIManager = uiMnager;
            //     treeNavigatoin.MouseDoubleClick += treePackageList_MouseDoubleClick;
            this.Loaded += frmMain_Loaded;
            //if (AgentHelper.GetAppMode() != AppMode.Paper)
            CheckInfoPanel();
            //if (AgentHelper.GetAppMode() == AppMode.Paper)
            //    grdUserInfo.Visibility = Visibility.Collapsed;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Tick += Timer_Tick;
        }



        bool loaded = false;
        void frmMain_Loaded(object sender, RoutedEventArgs e)
        {
            if (loaded == false)
            {
                //UIManager.OnNavigationTreeRequested();
                loaded = true;
            }
        }



        //private void cmbDatabases_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (cmbDatabases.SelectedItem != null)
        //    {
        //        ////////UIManager.OnPackageTreeRequested((cmbDatabases.SelectedItem as DP_DatabaseListItem).Name);
        //    }
        //}


        internal void ClearNavigationTree()
        {
            treeNavigatoin.Items.Clear();
        }

        internal I_NavigationMenu AddNavigationTree(I_NavigationMenu parentItem, NavigationItemDTO item, bool expanded)
        {
            NavigationMenu menu = new WPF_MyIdea.NavigationMenu();
            var node = new RadTreeViewItem();
            node.DataContext = item;
            node.Header = GetNodeHeader(item.Title, item.ObjectCategory);
            ItemCollection parent = null;
            if (parentItem == null)
                parent = treeNavigatoin.Items;
            else
                parent = (parentItem.Node as RadTreeViewItem).Items;
            parent.Add(node);
            if (!string.IsNullOrEmpty(item.Tooltip))
                ToolTipService.SetToolTip(node, item.Tooltip);
            node.IsExpanded = expanded;
            menu.Node = node;
            node.MouseDoubleClick += (sender, e) => Node_MouseDoubleClick(sender, e, menu);
            return menu;
        }

        internal I_NavigationMenu AddSearchNavigationTree(I_NavigationMenu parentItem, NavigationItemDTO item, bool expanded)
        {
            NavigationMenu menu = new WPF_MyIdea.NavigationMenu();
            var node = new RadTreeViewItem();
            node.DataContext = item;
            node.Header = GetNodeHeader(item.Title, item.ObjectCategory);
            ItemCollection parent = null;
            if (parentItem == null)
                parent = treeSearchNavigation.Items;
            else
                parent = (parentItem.Node as RadTreeViewItem).Items;
            parent.Add(node);
            if (!string.IsNullOrEmpty(item.Tooltip))
                ToolTipService.SetToolTip(node, item.Tooltip);
            node.IsExpanded = expanded;
            menu.Node = node;
            node.MouseDoubleClick += (sender, e) => Node_MouseDoubleClick(sender, e, menu);
            return menu;
        }


        private void Node_MouseDoubleClick(object sender, MouseButtonEventArgs e, NavigationMenu menu)
        {
            if (sender is RadTreeViewItem)
                menu.OnClicked(sender as RadTreeViewItem);
        }


        //private void ShowTreeItem(List<NavigationItemDTO> treeItems, ItemCollection itemCollection, NavigationItemDTO item)
        //{

        //}
        private FrameworkElement GetNodeHeader(string title, DatabaseObjectCategory type)
        {
            StackPanel pnlHeader = new StackPanel();
            TextBlock label = new TextBlock();
            label.Text = title;
            Image img = new Image();
            img.Width = 15;
            Uri uriSource = null;
            if (type == DatabaseObjectCategory.Folder)
            {
                uriSource = new Uri("Images/folder.png", UriKind.Relative);
            }
            else if (type == DatabaseObjectCategory.Entity)
            {
                uriSource = new Uri("Images/form.png", UriKind.Relative);
            }
            else if (type == DatabaseObjectCategory.Report)
            {
                uriSource = new Uri("Images/report.png", UriKind.Relative);
            }
            else if (type == DatabaseObjectCategory.Archive)
            {
                uriSource = new Uri("Images/archive.png", UriKind.Relative);
            }
            else
            {
                uriSource = new Uri("Images/form.png", UriKind.Relative);
            }
            img.Source = new BitmapImage(uriSource);
            pnlHeader.Orientation = Orientation.Horizontal;
            pnlHeader.Children.Add(img);
            pnlHeader.Children.Add(label);
            return pnlHeader;
        }

        internal void ClearSearchNavigationTree()
        {
            treeSearchNavigation.Items.Clear();
        }


        //void treePackageList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    if (treeNavigatoin.SelectedItem != null)
        //    {
        //        var navigationItem = (treeNavigatoin.SelectedItem as RadTreeViewItem).DataContext as NavigationItemDTO;
        //        if (navigationItem != null)
        //            UIManagerGenerator.GetUIManager().OnNavigationItemSelected(navigationItem);
        //    }
        //    //}
        //    //catch { }
        //}





        //internal void ShowForm(Control userControl, string title)
        //{
        //    RadPane pane = new RadPane();
        //    pane.Header = title;
        //    pane.Content = userControl;
        //    pnlForms.Items.Add(pane);
        //}

        private void CheckInfoPanel()
        {
            ScrollViewer scroll = new ScrollViewer();
            InfoTextBlock = new TextBlock();
            InfoTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
            scroll.Content = InfoTextBlock;
            pnlInfo.Content = scroll;
        }
        TextBlock InfoTextBlock;
        internal void ShowInfo(string text, string detail, Brush color)
        {
            pnlInfo.IsActive = true;
            AddInfoTitleRow(text, color);
            if (!string.IsNullOrEmpty(detail))
            {
                var runDetail = new Run(Environment.NewLine + "   " + detail);
                InfoTextBlock.Inlines.InsertAfter(InfoTextBlock.Inlines.First(), runDetail);
            }
        }
        private Run AddInfoTitleRow(string text, Brush color)
        {
            text = DateTime.Now.ToString("HH:mm:ss") + " " + text;
            var runTitle = new Run(Environment.NewLine + text) { Foreground = color };

            if (InfoTextBlock.Inlines.Any())
                InfoTextBlock.Inlines.InsertBefore(InfoTextBlock.Inlines.First(), runTitle);
            else
                InfoTextBlock.Inlines.Add(runTitle);
            return runTitle;
        }

        internal void ShowInfo(string text, List<ResultDetail> details, Brush color)
        {
            pnlInfo.IsActive = true;
            var title = AddInfoTitleRow(text, color);
            if (details.Any())
            {
                var link = AddInfoHyperlink("جزئیات", title);
                //link.Click += (sender, e) => LinkeClicked(sender, e, details);
                link.MouseLeftButtonUp += (sender, e) => RunTitle_MouseLeftButtonUp(sender, e, details);
            }
        }


        private Run AddInfoHyperlink(string link, Run title)
        {
            //Hyperlink hyperLink = new Hyperlink()
            //{
            //    NavigateUri = new Uri(link),
            //    Foreground = new SolidColorBrush(Colors.Blue)

            //};

            var runTitle = new Run("   " + link) { Foreground = new SolidColorBrush(Colors.Blue), FontWeight = FontWeights.Heavy };
            InfoTextBlock.Inlines.InsertAfter(title, runTitle);
            //   InfoTextBlock.Inlines.Add(runTitle);
            return runTitle;
        }

        private void RunTitle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e, List<ResultDetail> details)
        {
            UIManager.ShowDetail("جزئیات", details);
        }

        private void LinkeClicked(object sender, RoutedEventArgs e, List<ResultDetail> details)
        {

        }
        internal List<string> GetPaneTitles()
        {
            List<string> result = new List<string>();
            foreach (RadPane item in pnlForms.Items)
            {
                result.Add((item as RadPane).Header.ToString());
            }
            return result;
        }
        internal void ActivatePane(string title)
        {
            foreach (RadPane item in pnlForms.Items)
            {
                if ((item as RadPane).Header.ToString() == title)
                    (item as RadPane).IsActive = true;
            }
        }
        internal void ShowPane(UIElement element, string title)
        {
            RadPane pane = new RadPane();
            pane.Header = title;
            var scrollViewer = new ScrollViewer();
            scrollViewer.Content = element;
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            pane.Content = scrollViewer;
            pnlForms.Items.Add(pane);
        }

        internal void SetUserInfo(string userName, string organizationName, string roles)
        {
            txtUserName.Text = userName;
            txtOrganization.Text = organizationName;
            txtRoles.Text = roles;
        }

        internal I_MainFormMenu AddToolsMenu(string title, string image)
        {
            var menu = new ToolsMenu(title, image);
            toolbar.Items.Add(menu.button);
            return menu;
        }
        DispatcherTimer timer = new DispatcherTimer();

        public bool SearchNavigationTreeVisiblity
        {
            get
            {
                return treeSearchNavigation.Visibility == Visibility.Visible;
            }
            set
            {
                treeSearchNavigation.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }

        }
        public bool NavigationTreeVisiblity
        {
            get
            {
                return treeNavigatoin.Visibility == Visibility.Visible;
            }
            set
            {
                treeNavigatoin.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();
            UIManagerGenerator.GetUIManager().OnSeatchTextChanged(txtSearch.Text);


        }

        private void imgRemopveSearch_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtSearch.Text = "";
        }





        //internal void ShowDatabaseList(DP_ResultDatabaseList result)
        //{
        //    cmbDatabases.SelectedValuePath = "Name";
        //    cmbDatabases.DisplayMemberPath = "Name";
        //    cmbDatabases.ItemsSource = result.Databases;
        //    if (result.Databases.Count > 0)
        //        cmbDatabases.SelectedItem = result.Databases.FirstOrDefault(x => x.Name.ToLower().Contains("sample"));

        //}
    }
    public class NavigationMenu : I_NavigationMenu
    {
        public object Node
        {
            set; get;
        }

        public event EventHandler Clicked;

        internal void OnClicked(RadTreeViewItem treeViewItem)
        {
            if (Clicked != null)
                Clicked(this, null);
        }
    }
}
