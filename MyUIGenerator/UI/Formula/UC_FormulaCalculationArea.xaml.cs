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
    public partial class UC_FormulaCalculationArea : UserControl, I_View_FormulaCalculationArea
    {
        public UC_FormulaCalculationArea()
        {
            InitializeComponent();
        }

        public string FromulaExpression
        {
            get
            {
                return txtFormula.Text;
            }

            set
            {
                txtFormula.Text = value;
            }
        }
        public object ResultString
        {
            get
            {
                return txtResult.Text;
            }

            set
            {
                txtResult.Text = value == null ? "<Null>" : value.ToString();
            }
        }
        public event EventHandler CloseRequested;

        public void AddException(string exception)
        {
            txtException.Text = exception;
        }

        public object AddTreeNode(object parentNode, string title)
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
            collection.Add(newNode);
            return newNode;
        }

        public void ClearTree()
        {
            treeData.Items.Clear();
        }

        public void ExceptionTabSelect()
        {
            tabException.IsSelected = true;
        }

        public void ParameterTabSelect()
        {
            tabParameters.IsSelected = true;

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (CloseRequested != null)
                CloseRequested(this, null);
        }
    }
}
