using MyUILibraryInterfaces.ContextMenu;
using MyUILibraryInterfaces.DataTreeArea;
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
using Telerik.Windows.Controls;

namespace MyUIGenerator.UI.DataTree
{
    /// <summary>
    /// Interaction logic for frmDataTree.xaml
    /// </summary>
    public partial class frmDataTree : UserControl, I_DataTreeView
    {
        public frmDataTree()
        {
            InitializeComponent();

        }
        public event EventHandler<ContextMenuArg> ContextMenuLoaded;

        // List<ViewDataTreeItem> DataTreeItems = new List<ViewDataTreeItem>();

        public event EventHandler DataAreaConfirmed;

        public I_ViewDataTreeItem AddRootNode(string title, bool selectable, bool tempChild)
        {
            return AddViewDataTreeItem(treeItems.Items, title, selectable, tempChild);
        }

        public I_ViewDataTreeItem AddViewDataTreeItem(ItemCollection items, string title, bool selectable, bool tempChild)
        {
            RadTreeViewItem node = new RadTreeViewItem();
            if (selectable)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Content = title;
                node.Header = checkBox;
            }
            else
                node.Header = title;
            items.Add(node);
            if (tempChild)
            {
                node.Items.Add(new RadTreeViewItem() { Header = "Loading..." });
            }
            node.Foreground = new SolidColorBrush(Colors.Green);

            ViewDataTreeItem viewDataTreeItem = new DataTree.ViewDataTreeItem(this, node);
            node.DataContext = viewDataTreeItem;
            node.Expanded += (sender, e) => Node_Expanded(sender, e, viewDataTreeItem);
            // DataTreeItems.Add(viewDataTreeItem);
            return viewDataTreeItem;
        }

        private void Node_Expanded(object sender, Telerik.Windows.RadRoutedEventArgs e, ViewDataTreeItem viewDataTreeItem)
        {
            viewDataTreeItem.OnExpanded();
        }

        //private RadTreeViewItem AddTreeNode(ItemCollection items, string title, bool selectable, bool tempChild)
        //{

        //}

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (DataAreaConfirmed != null)
                DataAreaConfirmed(this, null);
        }

        private void menuTree_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void treeItems_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //var source = e.OriginalSource as DependencyObject;
            //while (source != null && !(source is RadTreeViewItem))
            //    source = VisualTreeHelper.GetParent(source);
            //if (source != null)
            //{
            //    (source as RadTreeViewItem).IsSelected = true;
            //    //  e.Handled = true;
            //}
        }

        private void menuGrid_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var contextMenu = sender as RadContextMenu;
            if (contextMenu != null && contextMenu.GetClickedElement<RadTreeViewItem>() != null)
            {
                var viewDataTreeItem = contextMenu.GetClickedElement<RadTreeViewItem>().DataContext as ViewDataTreeItem;
                if (contextMenu != null && viewDataTreeItem != null)
                {
                    if (ContextMenuLoaded != null)
                    {
                        ContextMenuArg arg = new ContextMenuArg();
                        arg.ContextObject = viewDataTreeItem;
                        arg.ContextMenuManager = new ContextMenuManager(contextMenu);
                        ContextMenuLoaded(this, arg);

                    }

                }

                //var fTreeItem = DataTreeItems.FirstOrDefault(x => x.ViewItem == node);
                //if (fTreeItem != null)
                //{
                //    fTreeItem.ContextMenu = sender as ContextMenu;
                //    fTreeItem.OnContextMenuLoaded();
                //}
            }
        }
    }
    public class ViewDataTreeItem : I_ViewDataTreeItem
    {
        private frmDataTree frmDataTree;
        private RadTreeViewItem node;

        public ViewDataTreeItem(frmDataTree frmDataTree, RadTreeViewItem node)
        {
            this.frmDataTree = frmDataTree;
            this.node = node;
        }

        public bool IsSelected
        {
            get
            {
                if (node.Header is CheckBox)
                {
                    return (node.Header as CheckBox).IsChecked == true;
                }
                return false;
            }
            set
            {
                if (node.Header is CheckBox)
                {
                    (node.Header as CheckBox).IsChecked = value;
                }
            }
        }
        public I_ViewDataTreeItem AddNode(string title, bool selectable, bool tempChild)
        {
            return frmDataTree.AddViewDataTreeItem(node.Items, title, selectable, tempChild);
        }
        //public ContextMenu ContextMenu { get; internal set; }

        //public bool ContextMenuVisiblie
        //{
        //    get
        //    {
        //        return ContextMenu.Visibility == Visibility.Visible;
        //    }

        //    set
        //    {
        //   //     ContextMenu.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
        //    }
        //}

        public event EventHandler Expanded;
        //   public event EventHandler ContextMenuLoaded;

        //public void ClearContextMenu()
        //{
        //  //  ContextMenu.Items.Clear();
        //}

        //public I_ViewContextMenu AddContextMenu(string title)
        //{
        //    ViewContextMenu viewMenu = new ViewContextMenu();
        //    viewMenu.MenuItem = new MenuItem();
        //    viewMenu.MenuItem.Header = title;
        //    viewMenu.MenuItem.Click += (sender, e) => Menu_Click(sender, e, viewMenu);
        //    ContextMenu.Items.Add(viewMenu.MenuItem);
        //    return viewMenu;
        //}

        //private void Menu_Click(object sender, RoutedEventArgs e, ViewContextMenu viewMenu)
        //{
        //    viewMenu.OnClick();
        //}

        //internal void OnContextMenuLoaded()
        //{
        //    if (ContextMenuLoaded != null)
        //        ContextMenuLoaded(this, null);
        //}

        internal void OnExpanded()
        {
            if (Expanded != null)
                Expanded(this, null);
        }

        public void ClearChildNodes()
        {
            node.Items.Clear();
        }


    }
    public class ViewContextMenu : I_ViewContextMenu
    {
        public MenuItem MenuItem { set; get; }
        public event EventHandler Cliecked;

        internal void OnClick()
        {
            if (Cliecked != null)
                Cliecked(this, null);
        }
    }
}
