using MyUILibraryInterfaces.DataViewArea;
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
    public partial class frmDataViewContainer : UserControl, I_View_DataViewAreaContainer
    {
        public frmDataViewContainer()
        {
            InitializeComponent();

        }

        public void AddDataViewArea(object view)
        {
            grdViews.Children.Clear();
            grdViews.Children.Add(view as UIElement);
        }

        public void ClearDataView()
        {
            grdViews.Children.Clear();
        }

        public void EnableDisable(bool enable)
        {
            this.IsEnabled = enable;
        }

        //public void AddGenerealSearchAreaView(object view)
        //{
        //    grdSearch.Children.Add(view as UIElement);
        //}

        public void ShowLinks(List<DataViewLink> links)
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

        private void Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e, DataViewLink link)
        {
            link.OnClicked();
        }

    }
}
