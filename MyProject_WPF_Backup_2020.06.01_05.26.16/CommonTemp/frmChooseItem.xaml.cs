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
    /// Interaction logic for frmChooseItem.xaml
    /// </summary>
    public partial class frmChooseItem : UserControl
    {
        public event EventHandler<SelectedItemsArg> ItemSelected;
        public frmChooseItem()
        {
            InitializeComponent();

        }
        public void ShowItems<T>(List<T> items, bool multiSelect)
        {
            dtgItems.ItemsSource = items;

            dtgItems.SelectionMode = (multiSelect ? SelectionMode.Multiple : SelectionMode.Single);
            dtgItems.IsReadOnly = true;
        }


        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (dtgItems.SelectedItems.Count > 0)
            {
                List<object> selectedList = new List<object>();
                foreach (var item in dtgItems.SelectedItems)
                    selectedList.Add(item);
                if (ItemSelected != null)
                {
                    ItemSelected(this, new SelectedItemsArg() { Items = selectedList });
                }
            }
        }
    }
    public class SelectedItemsArg : EventArgs
    {
        public List<object> Items { set; get; }
    }
}
