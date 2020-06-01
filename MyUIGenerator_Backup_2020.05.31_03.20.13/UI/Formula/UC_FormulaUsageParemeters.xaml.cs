using MyUILibrary.EntityArea;
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
    /// Interaction logic for UC_FormulaOptions.xaml
    /// </summary>
    public partial class UC_FormulaUsageParemeters : UserControl, I_FormulaUsageParameters
    {
        public UC_FormulaUsageParemeters()
        {
            InitializeComponent();
        }

        public event EventHandler CloseRequested;

        public object AddTreeNode(object parentNode, string title)
        {
         
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
