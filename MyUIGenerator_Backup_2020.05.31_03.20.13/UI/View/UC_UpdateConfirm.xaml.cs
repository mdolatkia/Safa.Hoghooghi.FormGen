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
    /// Interaction logic for UC_UpdateConfirm.xaml
    /// </summary>
    public partial class UC_UpdateConfirm : UserControl, I_ConfirmUpdate
    {
        public UC_UpdateConfirm()
        {
            InitializeComponent();
        }

        public event EventHandler DateTreeRequested;
        public event EventHandler<ConfirmUpdateDecision> Decided;

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            if (Decided != null)
                Decided(this, new ConfirmUpdateDecision() { Confirm = true });
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            if (Decided != null)
                Decided(this, new ConfirmUpdateDecision() { Confirm = false });
        }

     

        private void btnDataTree_Click(object sender, RoutedEventArgs e)
        {
            if (DateTreeRequested != null)
                DateTreeRequested(this, null);
        }
    }
}
