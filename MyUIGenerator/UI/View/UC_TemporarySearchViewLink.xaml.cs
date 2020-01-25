
using MyUILibrary;
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
    /// Interaction logic for UC_TemporaryViewLink.xaml
    /// </summary>
    public partial class UC_TemporarySearchViewLink : UserControl, IAG_View_TemporaryView
    {
        public UC_TemporarySearchViewLink()
        {
            InitializeComponent();
        }

        public DataAccess.Column Column
        {
            set;
            get;
        }
        public event EventHandler<Arg_TemporaryDisplayViewRequested> TemporaryDisplayViewRequested;



        public void SetLinkText(string text)
        {
            btnLink.Content = text;
        }

        private void btnLink_Click(object sender, RoutedEventArgs e)
        {
            if (TemporaryDisplayViewRequested != null)
            {
                TemporaryDisplayViewRequested(this, new Arg_TemporaryDisplayViewRequested() { DataItem = DataItem, LinkType = TemporaryLinkType.SerachView,Column=Column });
            }
        }

        public object DataItem
        {
            set;
            get;
        }



    }
}
