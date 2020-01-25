
using ModelEntites;
using MyFormulaFunctionStateFunctionLibrary;
using MyModelManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
using Telerik.Windows.Controls;


namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmFormula.xaml
    /// </summary>
    public partial class frmEntityChartReportSelect :UserControl
    {
        public event EventHandler<EntityChartReportSelectedArg> EntityChartReportSelected;
        public int EntityID { set; get; }
        BizEntityChartReport bizEntityChartReport = new BizEntityChartReport();
        public frmEntityChartReportSelect(int entityID)
        {
            InitializeComponent();

            EntityID = entityID;
            GetEntityChartReports();
        }

       
     
        private void GetEntityChartReports()
        {
            var listEntityChartReports = bizEntityChartReport.GetEntityChartReports(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID);
            dtgItems.ItemsSource = listEntityChartReports;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var item = dtgItems.SelectedItem as EntityChartReportDTO;
            if (item != null)
            {
                if (EntityChartReportSelected != null)
                    EntityChartReportSelected(this, new EntityChartReportSelectedArg() { EntityChartReportID = item.ID });
            }
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }

    public class EntityChartReportSelectedArg : EventArgs
    {
        public int EntityChartReportID { set; get; }
    }

}
