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
        public event EventHandler<NodeSelectedArg> NodeSelected;
        public FormulaAutoComplete()
        {
            InitializeComponent();
            this.Loaded += FormulaAutoComplete_Loaded;
            treeMainEntity.IsTextSearchEnabled = true;
            //قضیه تکست سرچ یا تایپ کردن تست بشه
        }

        private void FormulaAutoComplete_Loaded(object sender, RoutedEventArgs e)
        {
            treeMainEntity.Focus();
        }

        internal void SetTree(List<NodeContext> nodes)
        {
            treeMainEntity.Items.Clear();
            var list = nodes.GroupBy(x => new AutoCompleteItem(x.NodeType, x.Title));

            foreach (var node in list.OrderBy(x => x.Key.NodeType).ThenBy(x => x.Key.Title))
            {
                AddNode(treeMainEntity.Items, node.Key);
            }
            //treeMainEntity.TextSearchMode = TextSearchMode.StartsWith;
            treeMainEntity.IsTextSearchEnabled = true;
            treeMainEntity.IsTextSearchCaseSensitive = false;
        }



        private ListBoxItem AddNode(ItemCollection items, AutoCompleteItem context)
        {
            ListBoxItem node = new ListBoxItem();
            
            node.DataContext = context;
            node.Content = GetHeader(context.NodeType, context.Title);
            node.MouseDoubleClick += Node_MouseDoubleClick;
            node.KeyUp += Node_KeyUp;
            items.Add(node);
            return node;
        }

        private void Node_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var node = sender as ListBoxItem;
            var nodeContext = node.DataContext as AutoCompleteItem;
            //    var path = GetNodePath(node);
            if (NodeSelected != null)
                NodeSelected(sender, new NodeSelectedArg() { NodeType = nodeContext.NodeType, Title = nodeContext.Title });
        }

        private void Node_DoubleClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var node = sender as RadTreeViewItem;
            var nodeContext = node.DataContext as AutoCompleteItem;
            //    var path = GetNodePath(node);
            if (NodeSelected != null)
                NodeSelected(sender, new NodeSelectedArg() { NodeType = nodeContext.NodeType, Title = nodeContext.Title });
        }
        private void Node_KeyUp(object sender, KeyEventArgs e)
        {
         
            var node = sender as ListBoxItem;
            var nodeContext = node.DataContext as AutoCompleteItem;

            bool selected = false;
            var arg = new NodeSelectedArg();
            arg.NodeType = nodeContext.NodeType;
            arg.Title = nodeContext.Title;

            if (e.Key == Key.Enter)
            {
                selected = true;
            }
            //else if (e.Key == Key.OemPeriod)
            //{
            //    selected = true;
            //    arg.Dot = true;
            //}
            //else if (e.Key == Key.OemOpenBrackets)
            //{
            //    selected = true;
            //    arg.Parantese = true;
            //}
            if (selected)
            {
                if (NodeSelected != null)
                    NodeSelected(sender, arg);
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

}
