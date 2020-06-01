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

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for UC_FormulaOptions.xaml
    /// </summary>
    public partial class UC_FormulaOptions : UserControl, I_FormulaOptions
    {
        public UC_FormulaOptions()
        {
            InitializeComponent();
        }

        public bool CalculationDetailsEnablity
        {
            get
            {
                return btnDetails.IsEnabled;
            }

            set
            {
                btnDetails.IsEnabled = value;
            }
        }

        public bool CalculationEnablity
        {
            get
            {
                return btnCalculate.IsEnabled;
            }

            set
            {
                btnCalculate.IsEnabled = value;
            }
        }

        public bool ErrorDetailsEnablity
        {
            get
            {
                return btnException.IsEnabled;
            }

            set
            {
                btnException.IsEnabled = value;
            }
        }

        public event EventHandler ClaculateRequested;
        public event EventHandler ClaculationDetailsRequested;
        public event EventHandler ErrorDetailRequested;

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            if (ClaculateRequested != null)
                ClaculateRequested(this, null);
        }

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            if (ClaculationDetailsRequested != null)
                ClaculationDetailsRequested(this, null);
        }

        private void btnException_Click(object sender, RoutedEventArgs e)
        {
            if (ErrorDetailRequested != null)
                ErrorDetailRequested(this, null);
        }
    }
}
