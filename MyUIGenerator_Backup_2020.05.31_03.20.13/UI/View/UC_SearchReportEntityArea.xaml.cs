using CommonDefinitions.BasicUISettings;
using ModelEntites;
using MyUIGenerator.UIControlHelper;
using MyUILibrary;
using MyUILibrary.EntityArea;
using MyUILibrary.EntityArea.Commands;
using MyUILibrary.Temp;
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
    /// Interaction logic for UC_ViewPackageArea.xaml
    /// </summary>
    public partial class UC_SearchReportEntityArea : UserControl, I_View_SearchReportEntityArea
    {


        //View_Container View_Container { set; get; }
        public UC_SearchReportEntityArea()
        {
            InitializeComponent();
        }



        public void AddSearchAreaView(I_View_SearchEntityArea view)
        {
            grdSearchArea.Children.Add(view as UIElement);
        }

        public void AddViewAreaView(I_View_ViewReportArea view)
        {
            grdViewArea.Children.Add(view as UIElement);
        }

       

        private void imgDown_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            imgDown.Visibility = Visibility.Collapsed;
            imgUp.Visibility = Visibility.Visible;
            grdSearchArea.Visibility = Visibility.Visible;
        }

        private void imgUp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            imgDown.Visibility = Visibility.Visible;
            imgUp.Visibility = Visibility.Collapsed;
            grdSearchArea.Visibility = Visibility.Collapsed;
        }
    }
}
