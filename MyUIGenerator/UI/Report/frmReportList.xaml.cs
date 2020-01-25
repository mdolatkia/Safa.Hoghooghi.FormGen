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
using ModelEntites;

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for frmReportList.xaml
    /// </summary>
    public partial class frmReportList : UserControl, I_View_ReportList
    {
        public frmReportList()
        {
            InitializeComponent();
        }

        public event EventHandler<ReportClickedArg> ReportClicked;

        public void SetReportList(List<EntityReportDTO> reports)
        {
            lstItems.Items.Clear();
            foreach (var item in reports)
            {
                StackPanel pnl = new StackPanel();
                pnl.Cursor = Cursors.Hand;
                pnl.Margin = new Thickness(5);
                Image img = new Image();
                img.Source = UIHelper.GetImageSource(@"..\..\images\report.png");
                img.Width = 30;
                pnl.Children.Add(img);
                TextBlock lbl = new TextBlock();
                lbl.Text = item.ReportTitle;
                pnl.Children.Add(lbl);
                pnl.MouseLeftButtonUp += (sender, e) => pnl_MouseLeftButtonUp(sender, e, item);
                lstItems.Items.Add(pnl);
            }
        }

        private void pnl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e, EntityReportDTO report)
        {
            if (ReportClicked != null)
                ReportClicked(this, new ReportClickedArg() { ReportID = report.ID });
        }
    }
}
