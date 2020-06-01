using MyUILibrary.EntityArea;
using MyUILibrary.Temp;
using MyUILibraryInterfaces.FormulaCalculationArea;
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
    /// Interaction logic for UC_FormulaCalculationArea.xaml
    /// </summary>
    public partial class UC_FormulaDataTree : UserControl, I_FormulaDataTree
    {
        public UC_FormulaDataTree()
        {
            InitializeComponent();
        }

        public event EventHandler NoClicked;
        public event EventHandler YesClicked;

        public void AddTitle(string title)
        {
            txtTitle.Text = title;
        }

        public object AddTreeNode(object parentNode, string title, string tooltip, InfoColor color, bool expand)
        {
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
            if (!string.IsNullOrEmpty(tooltip))
                ToolTipService.SetToolTip(newNode, tooltip);
            collection.Add(newNode);
            newNode.IsExpanded = expand;
            return newNode;
        }

        public void ClearTree()
        {
            treeData.Items.Clear();
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            if (YesClicked != null)
                YesClicked(this, null);
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            if (NoClicked != null)
                NoClicked(this, null);
        }
    }
}
