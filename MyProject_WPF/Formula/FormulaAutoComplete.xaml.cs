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
using ModelEntites;
using Telerik.Windows.Controls;
using MyFormulaFunctionStateFunctionLibrary;
using System.Reflection;
using ProxyLibrary;
using MyModelManager;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for FormulaAutoComplete.xaml
    /// </summary>
    public partial class FormulaAutoComplete : UserControl
    {
        public List<ListBoxItem> ListItems { get; private set; }

        public event EventHandler<NodeSelectedArg> NodeSelected;
        public FormulaAutoComplete()
        {
            InitializeComponent();
            this.Loaded += FormulaAutoComplete_Loaded;
            lstProperties.IsTextSearchEnabled = true;
            //     txtSearch.KeyUp += TxtSearch_KeyUp;
            //   txtSearch.KeyDown += TxtSearch_KeyUp;
            txtSearch.PreviewKeyDown += TxtSearch_PreviewKeyDown;
            this.KeyUp += FormulaAutoComplete_KeyUp;
        }

        private void FormulaAutoComplete_KeyUp(object sender, KeyEventArgs e)
        {
          if(e.Key==Key.Escape)
            {
                CloseDialog();
            }
        }

        private void CloseDialog()
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }

        private void TxtSearch_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var selectedItem = GetSelectedItem();
                if (selectedItem != null)
                {
                    NodeIsSelected(sender, selectedItem.DataContext as AutoCompleteItem);
                }
            }
            else if (e.Key == Key.Down)
            {
                e.Handled = true;
                if (SelectItem(SelectMode.Next))
                    txtSearch.Focus();
            }
            else if (e.Key == Key.Up)
            {
                e.Handled = true;
                if (SelectItem(SelectMode.Prev))
                    txtSearch.Focus();
            }
        }

        private bool SelectItem(SelectMode mode)
        {
            var selectedItem = GetSelectedItem();
            var index = ListItems.IndexOf(selectedItem);
            ListBoxItem target = null;
            if (selectedItem != null)
            {
                if (mode == SelectMode.Next)
                {
                    if (index != ListItems.Count - 1)
                    {
                        target = ListItems[index + 1];
                    }
                }
                else if (mode == SelectMode.Prev)
                {
                    if (index != 0)
                    {
                        target = ListItems[index - 1];
                    }
                }
            }
            if (target != null)
            {

                target.IsSelected = true;
                target.Focus();
                return true;
            }
            return false;
        }

        private ListBoxItem GetSelectedItem()
        {
            if (lstProperties.Items != null)
            {
                foreach (var item in lstProperties.Items)
                {
                    if ((item as ListBoxItem).IsSelected)
                        return item as ListBoxItem;
                }
            }
            return null;
        }

        private void FormulaAutoComplete_Loaded(object sender, RoutedEventArgs e)
        {
            txtSearch.Focus();
        }

        internal void SetTree(List<NodeContext> nodes)
        {
            //   lstProperties.ItemsSource = nodes;
            ListItems = null;
            txtSearch.Text = "";

            var list = nodes.GroupBy(x => new { x.NodeType, x.Title });
            List<ListBoxItem> items = new List<ListBoxItem>();
            foreach (var node in list.OrderBy(x => x.Key.NodeType).ThenBy(x => x.Key.Title))
            {
                items.Add(AddNode(new AutoCompleteItem(node.Key.NodeType,node.Key.Title)));
            }
            ListItems = items;
            SetListItems("");

        }
        private ListBoxItem AddNode(AutoCompleteItem context)
        {
            ListBoxItem node = new ListBoxItem();

            node.DataContext = context;
            node.Content = GetHeader(context.NodeType, context.Title);
            node.MouseDoubleClick += Node_MouseDoubleClick;
            node.KeyUp += Node_KeyUp;
            return node;
        }

        private void SetListItems(string text)
        {
            lstProperties.Items.Clear();
            if (ListItems != null)
            {
                foreach (var item in ListItems)
                {
                    if (text == "" || (item.DataContext as AutoCompleteItem).Title.ToLower().StartsWith(text.ToLower()))
                        lstProperties.Items.Add(item);
                }
                if (lstProperties.Items.Count > 0)
                {
                    (lstProperties.Items[0] as ListBoxItem).IsSelected = true;
                }
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetListItems(txtSearch.Text);
        }

      
        private void Node_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var node = sender as ListBoxItem;
            var nodeContext = node.DataContext as AutoCompleteItem;
            //    var path = GetNodePath(node);
            NodeIsSelected(sender, nodeContext);
        }

        private void NodeIsSelected(object sender, AutoCompleteItem nodeContext)
        {
            if (NodeSelected != null)
                NodeSelected(sender, new NodeSelectedArg() { NodeType = nodeContext.NodeType, Title = nodeContext.Title });
        }

        private void Node_DoubleClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var node = sender as RadTreeViewItem;
            var nodeContext = node.DataContext as AutoCompleteItem;
            NodeIsSelected(sender, nodeContext);

        }
        private void Node_KeyUp(object sender, KeyEventArgs e)
        {
            var node = sender as ListBoxItem;
            var nodeContext = node.DataContext as AutoCompleteItem;


            if (e.Key == Key.Enter)
            {
                NodeIsSelected(sender, nodeContext);
            }
            else if (e.Key == Key.Up)
            {
                if ((lstProperties.Items[0] as ListBoxItem).IsSelected)
                    txtSearch.Focus();
            }
        }

        private object GetHeader(NodeType nodeeType, string title)
        {
            StackPanel pnl = new StackPanel();
            pnl.Orientation = Orientation.Horizontal;
            System.Windows.Controls.TextBlock lbl = new System.Windows.Controls.TextBlock();
            lbl.Text = title;
            Image img = new Image();
            img.Source = GetPropertyImage(nodeeType);
            img.Width = 15;
            pnl.Children.Add(img);
            pnl.Children.Add(lbl);
            return pnl;
        }

        private ImageSource GetPropertyImage(NodeType propertyType)
        {
            if (propertyType == NodeType.DotNetProperty || propertyType == NodeType.CustomProperty)
            {
                return new BitmapImage(new Uri(@"/MyProject_WPF;component/Images/property.png", UriKind.Relative));
            }
            else if (propertyType == NodeType.DotNetMethod)
            {
                return new BitmapImage(new Uri(@"/MyProject_WPF;component/Images/method.png", UriKind.Relative));
            }
            else if (propertyType == NodeType.RelationshipProperty)
            {
                return new BitmapImage(new Uri(@"/MyProject_WPF;component/Images/type.png", UriKind.Relative));
            }
            else if (propertyType == NodeType.HelperProperty)
            {
                return new BitmapImage(new Uri(@"/MyProject_WPF;component/Images/validate.png", UriKind.Relative));
            }
            return null;
        }


    }
    public enum SelectMode
    {
        Next,
        Prev

    }
}
