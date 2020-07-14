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
        }

        private void FormulaAutoComplete_Loaded(object sender, RoutedEventArgs e)
        {
            treeMainEntity.Focus();
        }

        internal void SetTree(List<NodeContext> nodes)
        {
            treeMainEntity.Items.Clear();
            foreach (var node in nodes.OrderBy(x => x.NodeType).ThenBy(x => x.Order1))
            {
                AddNode(treeMainEntity.Items, node);
            }
            //treeMainEntity.TextSearchMode = TextSearchMode.StartsWith;
            treeMainEntity.IsTextSearchEnabled = true;
           
        }



        private ListBoxItem AddNode(ItemCollection items, NodeContext nodeContext)
        {
            //RadTreeViewItem node = new RadTreeViewItem();
            //node.DataContext = nodeContext;
            //node.Header = GetHeader(node);
            //node.IsTextSearchEnabled = true;
            //node.DoubleClick += Node_DoubleClick;
            //node.KeyUp += Node_KeyUp;
            //items.Add(node);
            //return node;

            ListBoxItem node = new ListBoxItem();
            node.DataContext = nodeContext;
            node.Content = GetHeader(nodeContext);
            node.MouseDoubleClick += Node_MouseDoubleClick;
            node.KeyUp += Node_KeyUp;
            items.Add(node);
            return node;
        }

        private void Node_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            var node = sender as ListBoxItem;
            var nodeContext = node.DataContext as NodeContext;
            //    var path = GetNodePath(node);
            if (NodeSelected != null)
                NodeSelected(sender, new NodeSelectedArg() { Node = nodeContext });
        }

        private void Node_DoubleClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            e.Handled = true;
            var node = sender as RadTreeViewItem;
            var nodeContext = node.DataContext as NodeContext;
            //    var path = GetNodePath(node);
            if (NodeSelected != null)
                NodeSelected(sender, new NodeSelectedArg() { Node = nodeContext });
        }
        private void Node_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            var node = sender as ListBoxItem;
            var nodeContext = node.DataContext as NodeContext;

            bool selected = false;
            var arg = new NodeSelectedArg();
            arg.Node = nodeContext;

            if (e.Key == Key.Enter)
            {
                selected = true;
            }
            else if (e.Key == Key.OemPeriod)
            {
                selected = true;
                arg.Dot = true;
            }
            else if (e.Key == Key.OemOpenBrackets)
            {
                selected = true;
                arg.Parantese = true;
            }
            if (selected)
            {
                if (NodeSelected != null)
                    NodeSelected(sender, arg);
            }
        }

        private object GetHeader(NodeContext node)
        {
            StackPanel pnl = new StackPanel();
            pnl.Orientation = Orientation.Horizontal;
            System.Windows.Controls.TextBlock lbl = new System.Windows.Controls.TextBlock();
            lbl.Text = node.Title;
            Image img = new Image();
            img.Source = GetPropertyImage(node.NodeType);
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

    public class EntityAndProperties
    {
        public EntityAndProperties()
        {
            Properties = new List<MyFormulaFunctionStateFunctionLibrary.MyPropertyInfo>();
        }
        public TableDrivedEntityDTO Entity { set; get; }
        public List<MyPropertyInfo> Properties { set; get; }
    }
    public class NodeContext
    {
        public NodeContext()
        {

        }
        public NodeContext ParentNode { set; get; }
        public List<NodeContext> ChildNodes { set; get; }
        public string ParentPath { set; get; }
        public object Context { set; get; }
        public string Name { set; get; }
        public string Title { set; get; }
        public NodeType NodeType { set; get; }
        public object UIItem { set; get; }
        public int Order1 { set; get; }
    }
    public enum NodeType
    {
        MainVariable,
        HelperProperty,
        RelationshipProperty,
        CustomProperty,
        DotNetProperty,
        DotNetMethod
    }
    public class NodeSelectedArg
    {
        public NodeContext Node { set; get; }
        public bool Dot { set; get; }
        public bool Parantese { set; get; }
        //public NodeType PropertyType { set; get; }
        //public string NodeTitle { set; get; }
        //     public string NodePath { set; get; }
    }
}
