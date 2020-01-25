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
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace MySecurityUI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window
            {
                Title = "My User Control Dialog",
                Content = new Grid(),
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize
            };
            window.ShowDialog();

            //var window = new RadWindow();
            //window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
          
            ////frmUserList frm = new frmUserList();
            ////frm.UserSelected += (sender1, e1) => frm_UserSelected(sender1, e1, window);
            ////window.Content = new Grid(); ;
            //window.Header = "انتخاب کاربر";
            //window.Show();
        }
    }
}
