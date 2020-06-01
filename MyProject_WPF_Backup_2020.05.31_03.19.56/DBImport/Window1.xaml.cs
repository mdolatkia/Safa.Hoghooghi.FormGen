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

namespace MyProject_WPF.DBImport
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());

            aaa();
            textBlock.Text = "ccc";
        }
        private async void aaa()
        {
            MessageBox.Show(System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());

            textBlock.Text = "aaa";
            await bbb();


            textBlock.Text = "bbb";
            await bbb();
            textBlock.Text = "ddd";
        }

        private Task bbb()
        {
            return Task.Run(() =>
            {
                System.Threading.Thread.Sleep(5000);
                MessageBox.Show(System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());

                //  textBlock.Text = "ccc";
            });
        }
    }
}
