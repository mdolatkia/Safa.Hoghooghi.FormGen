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

namespace MyUIGenerator
{
    /// <summary>
    /// Interaction logic for UC_InfoDetails.xaml
    /// </summary>
    public partial class UC_Message : UserControl
    {
        List<ResultDetail> Details = null;
        UIManager UIManager;
        public UC_Message(UIManager uiMnager,string message, List<ResultDetail> details)
        {
            InitializeComponent();
            UIManager = uiMnager;
            lblMessage.Text = message;
            Details = details;
            if (details != null && details.Any())
                btnDetails.Visibility = Visibility.Visible;
            else
                btnDetails.Visibility = Visibility.Collapsed;
        }

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            if (Details != null && Details.Any())
                UIManager.ShowDetail("جزئیات", Details);
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            var window = this.ParentOfType<RadWindow>();
            if (window != null)
                window.Close();
        }
    }
}
