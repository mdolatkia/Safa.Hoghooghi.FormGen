using MyUILibraryInterfaces.DataReportArea;

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
using Telerik.Windows.Controls.GridView;

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for frmDataView.xaml
    /// </summary>
    public partial class frmDataListReportContainer : UserControl, I_View_DataListReportAreaContainer
    {
        public frmDataListReportContainer()
        {
            InitializeComponent();

        }

        public void AddDataListReportArea(object view)
        {
            grdViews.Children.Clear();
            grdViews.Children.Add(view as UIElement);
        }

        public void ShowLinks(List<DataReportLink> links)
        {
            pnlLinks.Orientation = Orientation.Horizontal;
            pnlLinks.Children.Clear();
            foreach (var item in links)
            {
                TextBlock lbl = new TextBlock();
                lbl.Cursor = Cursors.Hand;
                lbl.Text = item.Title;
                
                lbl.TextDecorations = TextDecorations.Underline;
                lbl.MouseLeftButtonUp += (sender, e) => Button_MouseLeftButtonUp(sender, e, item);
                pnlLinks.Children.Add(lbl);
                if (item != links.LastOrDefault())
                    pnlLinks.Children.Add(new TextBlock() { Text = " > " });
            }
        }

        private void Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e, DataReportLink link)
        {
            link.OnClicked();
        }

    }
}
