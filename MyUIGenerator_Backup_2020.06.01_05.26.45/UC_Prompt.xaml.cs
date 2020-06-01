using MyUILibrary.EntityArea;
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
    public partial class UC_Prompt : UserControl
    {
        UIManager UIManager;
        public bool? Result = null;
        public UC_Prompt(UIManager uiMnager,string message, UserPromptMode dialogMode)
        {
            InitializeComponent();
            UIManager = uiMnager;
            lblMessage.Text = message;
        }

        void CloseDialog()
        {
            var window = this.ParentOfType<RadWindow>();
            if (window != null)
                window.Close();
        }
        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            CloseDialog();
        }

        private void btnNos_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            CloseDialog();
        }
    }
}
