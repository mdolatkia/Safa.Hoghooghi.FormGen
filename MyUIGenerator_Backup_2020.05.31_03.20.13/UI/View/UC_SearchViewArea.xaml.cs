
using CommonDefinitions.BasicUISettings;
using MyUIGenerator;
using MyUIGenerator;
using MyUILibrary;
using MyUILibrary.EntityArea;
using MyUILibrary.PackageArea;
using MyUILibrary.PackageArea.Commands;
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

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for frmSearchViewPackageArea.xaml
    /// </summary>
    public partial class UC_SearchViewArea : UserControl, I_View_SearchViewArea
    {
        public UC_SearchViewArea()
          
        {
            InitializeComponent();
        }




        public void AddSearchView(I_View_SearchEntityArea searchView)
        {
            Border border = new Border();
            border.Child = searchView as UIElement;
            border.BorderThickness = new Thickness(1);
            border.BorderBrush = Brushes.Black;
            border.Margin = new Thickness(5);
            grdMain.Children.Add(border);

        }

        public void AddViewView(I_View_ViewEntityArea viewView)
        {
            Border border = new Border();
            border.Child = viewView as UIElement;
            border.BorderThickness = new Thickness(1);
            border.BorderBrush = Brushes.Black;
            border.Margin = new Thickness(5);
            grdMain.Children.Add(border);
            Grid.SetRow(border, 1);
        }
    }
}
