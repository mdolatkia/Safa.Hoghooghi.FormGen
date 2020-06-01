

using MyModelManager;
using ProxyLibrary.Workflow;
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

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmProcessSelect.xaml
    /// </summary>
    public partial class frmProcessSelect : UserControl
    {
        public event EventHandler<ProcessSelectedArg> ItemSelected;
        BizProcess bizProcess = new BizProcess();
        public frmProcessSelect()
        {
            InitializeComponent();
            dtgList.ItemsSource = bizProcess.GetProcesses(MyProjectManager.GetMyProjectManager.GetRequester());
        }

        private void btnRetuen_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (dtgList.SelectedItem != null)
            {
                if (dtgList.SelectedItem is ProcessDTO)
                {
                    ItemSelected(this, new ProcessSelectedArg() { ID = (dtgList.SelectedItem as ProcessDTO).ID });
                    MyProjectManager.GetMyProjectManager.CloseDialog(this);
                }
            }
        }

    }
    public class ProcessSelectedArg : EventArgs
    {
        public int ID { set; get; }
    }
    //public class SelectedItemsArg : EventArgs
    //{
    //    public List<object> Items { set; get; }
    //}
}
