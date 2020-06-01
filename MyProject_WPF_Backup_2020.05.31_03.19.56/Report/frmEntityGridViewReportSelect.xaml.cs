
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
    public partial class frmEntityGridViewReportSelect: UserControl
    {
        public event EventHandler<EntityGridViewReportSelectedArg> EntityGridViewReportSelected;
        public int EntityID { set; get; }
        BizEntityGridViewReport bizEntityGridViewReport = new BizEntityGridViewReport();
        public frmEntityGridViewReportSelect(int entityID)
        {
            InitializeComponent();

            EntityID = entityID;
            GetEntityGridViewReports();
        }

      
     
        private void GetEntityGridViewReports()
        {
            var listEntityGridViewReports = bizEntityGridViewReport.GetEntityGridViewReports(EntityID);
            dtgItems.ItemsSource = listEntityGridViewReports;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var item = dtgItems.SelectedItem as EntityGridViewReportDTO;
            if (item != null)
            {
                if (EntityGridViewReportSelected != null)
                    EntityGridViewReportSelected(this, new EntityGridViewReportSelectedArg() { EntityGridViewReportID = item.ID });
            }
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }

    public class EntityGridViewReportSelectedArg : EventArgs
    {
        public int EntityGridViewReportID { set; get; }
    }

}
