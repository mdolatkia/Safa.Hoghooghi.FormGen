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
        }

        internal void SetTree(List<NodeContext> nodes)
        {
            treeMainEntity.Items.Clear();
            foreach (var node in nodes)
            {
                AddNode(treeMainEntity.Items, node);
            }
        }
        private void Node_DoubleClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            e.Handled = true;
            var node = sender as RadTreeViewItem;
            var nodeContext = node.DataContext as NodeContext;
            //    var path = GetNodePath(node);
            if (NodeSelected != null)
                NodeSelected(sender, new NodeSelectedArg() { NodeTitle = nodeContext.Title, PropertyType = nodeContext.NodeType });
        }


        private RadTreeViewItem AddNode(ItemCollection items, NodeContext nodeContext)
        {
            RadTreeViewItem node = new RadTreeViewItem();
            node.DataContext = nodeContext;
            node.Header = GetHeader(node);

            node.DoubleClick += Node_DoubleClick;


            items.Add(node);
            return node;
        }
        private object GetHeader(RadTreeViewItem node)
        {
            var context = node.DataContext as NodeContext;
            StackPanel pnl = new StackPanel();
            pnl.Orientation = Orientation.Horizontal;
            System.Windows.Controls.TextBlock lbl = new System.Windows.Controls.TextBlock();
            lbl.Text = context.Title;
            Image img = new Image();
            img.Source = GetPropertyImage(context.NodeType);
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
        public NodeContext ParentNode { set; get; }
        public List<NodeContext> ChildNodes { set; get; }
        public string ParentPath { set; get; }
        public object Context { set; get; }
        public string Name { set; get; }
        public string Title { set; get; }
        public NodeType NodeType { set; get; }
        public object UIItem  { set; get; }

    }
    public enum NodeType
    {
        MainVariable,
        CustomProperty,
        RelationshipProperty,
        DotNetProperty,
        DotNetMethod,
        HelperProperty
    }
    public class NodeSelectedArg
    {
        public NodeType PropertyType { set; get; }
        public string NodeTitle { set; get; }
        //     public string NodePath { set; get; }
    }
}
