using ModelEntites;
using MyModelManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmFunctionTree.xaml
    /// </summary>
    public partial class frmDatabaseFunctionTree : UserControl
    {
        public event EventHandler<TreeDatabaseFunctionSelectedArg> ItemSelected;
        int DatabaseID;
        public frmDatabaseFunctionTree(int databaseID)
        {
            InitializeComponent();
            DatabaseID = databaseID;
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                PopulateTree();
            treeItems.SelectedItemChanged += TreeItems_SelectedItemChanged;
        }
        private void TreeItems_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            e.Handled = true;
            if (treeItems.SelectedItem != treeItems.Items[0])
            {
                if (ItemSelected != null)
                    ItemSelected(this, new TreeDatabaseFunctionSelectedArg() { Function = (treeItems.SelectedItem as TreeViewItem).DataContext as DatabaseFunctionDTO });
            }
        }

        BizDatabaseFunction bizDatabaseFunction = new BizDatabaseFunction();
        private void PopulateTree()
        {
            var rootNode = new TreeViewItem();
            rootNode.Header = GetNodeHeader("لیست فانکشن ها", "Folder");
            treeItems.Items.Add(rootNode);
            PopulateTreeItems(rootNode.Items);
            rootNode.IsExpanded = true;
        }


        private void PopulateTreeItems(ItemCollection items)
        {
            var listFunctions = bizDatabaseFunction.GetDatabaseFunctions(Enum_DatabaseFunctionType.None, DatabaseID);
            foreach (var function in listFunctions)
            {
                AddNode(items, function);
            }
        }

        private TreeViewItem AddNode(ItemCollection collection, DatabaseFunctionDTO DatabaseFunction)
        {
            var node = new TreeViewItem();
            node.DataContext = DatabaseFunction;
            node.Header = GetNodeHeader(string.IsNullOrEmpty(DatabaseFunction.Title) ? DatabaseFunction.FunctionName : DatabaseFunction.Title, "function");
            node.ToolTip = DatabaseFunction.Title;
            collection.Add(node);
            return node;
        }

        private FrameworkElement GetNodeHeader(string title, string type)
        {
            StackPanel pnlHeader = new StackPanel();
            TextBlock label = new TextBlock();
            label.Text = title;
            Image img = new Image();
            img.Width = 15;
            Uri uriSource = null;
            if (type.ToString() == "Folder")
            {
                uriSource = new Uri("Images/folder.png", UriKind.Relative);
            }
            else if (type.ToString() == "function")
            {
                uriSource = new Uri("Images/form.png", UriKind.Relative);
            }

            if (uriSource != null)
                img.Source = new BitmapImage(uriSource);
            pnlHeader.Orientation = Orientation.Horizontal;
            pnlHeader.Children.Add(img);
            pnlHeader.Children.Add(label);
            return pnlHeader;
        }
    }

    public class TreeDatabaseFunctionSelectedArg : EventArgs
    {
        public DatabaseFunctionDTO Function { set; get; }
    }
}
