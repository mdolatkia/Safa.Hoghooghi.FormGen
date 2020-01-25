using MyUILibraryInterfaces.EntityArea;
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
    /// Interaction logic for frmEntitySelector.xaml
    /// </summary>
    public partial class frmEntitySelector : UserControl, I_View_EntitySelectArea
    {
        public frmEntitySelector()
        {
            InitializeComponent();
        }

        public void AddDataSelector(object view, string title)
        {
            txtDataSelector.Text = title;
            grdDataSelector.Children.Clear();
            grdDataSelector.Children.Add(view as UIElement);
        }

        public void AddEntitySelector(object view,string title)
        {
            txtEntitySelector.Text = title;
            grdEntitySelector.Children.Clear();
            grdEntitySelector.Children.Add(view as UIElement);
        }

        public void AddExternalArea(object view)
        {
            grdView.Children.Clear();
            grdView.Children.Add(view as UIElement);
        }

        public void RemoveDataSelector()
        {
            txtDataSelector.Text = "";
            grdDataSelector.Children.Clear();
        }
    }
}
