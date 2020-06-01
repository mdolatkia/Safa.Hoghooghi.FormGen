using ModelEntites;
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

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for SelectDrivedEntites.xaml
    /// </summary>
    /// 
    public partial class SelectDrivedEntites : UserControl
    {
        public event EventHandler<frmEditSubEntity> EntitySelected;
        List<frmEditSubEntity> DrivedEntityViews { set; get; }
        public SelectDrivedEntites(List<frmEditSubEntity> drivedEntityViews)
        {
            InitializeComponent();
            DrivedEntityViews = drivedEntityViews;
            dtgItems.ItemsSource = drivedEntityViews.Select(x=>x.Message).ToList();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (dtgItems.SelectedItem != null)
            {
                EntitySelected(this, DrivedEntityViews.First(x=>x.Message== dtgItems.SelectedItem));
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
