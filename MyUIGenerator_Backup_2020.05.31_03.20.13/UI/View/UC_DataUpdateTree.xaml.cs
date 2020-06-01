using MyUILibrary.EntityArea;
using MyUILibrary.Temp;
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

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for UC_DataUpdateTree.xaml
    /// </summary>
    public partial class UC_DataUpdateTree : UserControl, I_DataTree
    {
        public UC_DataUpdateTree()
        {
            InitializeComponent();
        }

        public event EventHandler CloseRequested;

        public object AddTreeNode(object parentNode, string title, bool expand, InfoColor color)
        {
            if (color == InfoColor.Null)
                color = InfoColor.Black;
            ItemCollection collection = null;
            if (parentNode != null)
            {
                collection = (parentNode as RadTreeViewItem).Items;
            }
            else
                collection = treeData.Items;
            var newNode = new RadTreeViewItem();
            newNode.Header = title;
            newNode.Foreground = UIManager.GetColorFromInfoColor(color);
            newNode.IsExpanded = expand;
            collection.Add(newNode);
            return newNode;
        }

        public void ClearTree()
        {
            treeData.Items.Clear();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (CloseRequested != null)
                CloseRequested(this, null);
        }
    }
}
