
using MyModelManager;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmSecurityAction.xaml
    /// </summary>
    public partial class frmSecurityAction : UserControl
    {
        public frmSecurityAction()
        {
            InitializeComponent();
        }

        public void SetActionTree(List<SecurityActionTreeItem> actions)
        {
            treeActions.Items.Clear();
            foreach (var item in actions)
            {
                AddTreeNode(item, treeActions.Items);
            }
        }

        private void AddTreeNode(SecurityActionTreeItem item, ItemCollection items)
        {
            RadTreeViewItem node = new RadTreeViewItem();
            node.DataContext = item;
            CheckBox checkbox = new CheckBox();
            checkbox.Checked += (sender, e) => NodeChecked(sender, e, node);
            checkbox.Content = item.Action.ToString();
            node.Header = checkbox;
            node.IsExpanded = true;
            items.Add(node);

            foreach (var childitem in item.ChildItems)
            {
                AddTreeNode(childitem, node.Items);
            }

        }

        private void NodeChecked(object sender, RoutedEventArgs e, RadTreeViewItem node)
        {
            if ((sender as CheckBox).IsChecked == true)
            {
                UncheckNodeParent(node);
                UncheckNodeChilds(node.Items);
            }
        }

        private void UncheckNodeParent(RadTreeViewItem node)
        {
            if (node.Parent != null && node.Parent is RadTreeViewItem)
            {
                var checkbox = (node.Parent as RadTreeViewItem).Header as CheckBox;
                checkbox.IsChecked = false;
                UncheckNodeParent(node.Parent as RadTreeViewItem);
            }
        }

        private void UncheckNodeChilds(ItemCollection items)
        {
            foreach (RadTreeViewItem node in items)
            {
                var checkbox = node.Header as CheckBox;
                checkbox.IsChecked = false;
                UncheckNodeChilds(node.Items);
            }
        }


        public void ClearData(ItemCollection items = null)
        {
            if (items == null)
                items = treeActions.Items;
            foreach (RadTreeViewItem item in items)
            {
                (item.Header as CheckBox).IsChecked = false;
                ClearData(item.Items);
            }
        }

        public void ShowData(List<SecurityAction> actions)
        {
            ClearData();
            foreach (var action in actions)
            {
                CheckBox checkbox = GetActionCheckBox(action, treeActions.Items);
                if (checkbox != null)
                    checkbox.IsChecked = true;
            }

        }
        private CheckBox GetActionCheckBox(SecurityAction action, ItemCollection items)
        {
            foreach (RadTreeViewItem item in items)
            {
                if (item.DataContext != null)
                {
                    if ((item.DataContext as SecurityActionTreeItem).Action == action)
                    {
                        return item.Header as CheckBox;
                    }
                }
                var childResult = GetActionCheckBox(action, item.Items);
                if (childResult != null)
                    return childResult;
            }
            return null;
        }

        public List<SecurityAction> GetCheckedActions(ItemCollection items = null, List<SecurityAction> result = null)
        {
            if (result == null)
                result = new List<SecurityAction>();
            if (items == null)
                items = treeActions.Items;
            foreach (RadTreeViewItem item in items)
            {
                if (item.DataContext != null)
                {

                    var checkbox = item.Header as CheckBox;
                    if (checkbox.IsChecked == true)
                    {
                        var securityActoinTreeItem = item.DataContext as SecurityActionTreeItem;
                        result.Add(securityActoinTreeItem.Action);
                    }

                }
                GetCheckedActions(item.Items, result);
            }
            return result;
        }

    }
}
