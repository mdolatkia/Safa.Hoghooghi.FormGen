
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
    public partial class frmExternalReportSelect: UserControl
    {
        public event EventHandler<EntityExternalReportSelectedArg> EntityExternalReportSelected;
        public int EntityID { set; get; }
        BizEntityExternalReport bizEntityExternalReport = new BizEntityExternalReport();
        public frmExternalReportSelect(int entityID)
        {
            InitializeComponent();

            EntityID = entityID;
            this.Loaded += FrmFormula_Loaded;
        }

        private void FrmFormula_Loaded(object sender, RoutedEventArgs e)
        {
            GetEntityExternalReports();

        }
     
        private void GetEntityExternalReports()
        {
            var listEntityExternalReports = bizEntityExternalReport.GetEntityExternalReports(EntityID);
            dtgItems.ItemsSource = listEntityExternalReports;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var item = dtgItems.SelectedItem as EntityExternalReportDTO;
            if (item != null)
            {
                if (EntityExternalReportSelected != null)
                    EntityExternalReportSelected(this, new EntityExternalReportSelectedArg() { EntityExternalReportID = item.ID });
            }
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }

    public class EntityExternalReportSelectedArg : EventArgs
    {
        public int EntityExternalReportID { set; get; }
    }

}
