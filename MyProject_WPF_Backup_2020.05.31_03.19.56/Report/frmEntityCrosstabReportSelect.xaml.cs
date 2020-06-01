
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
    public partial class frmEntityCrosstabReportSelect :UserControl
    {
        public event EventHandler<EntityCrosstabReportSelectedArg> EntityCrosstabReportSelected;
        public int EntityID { set; get; }
        BizEntityCrosstabReport bizEntityCrosstabReport = new BizEntityCrosstabReport();
        public frmEntityCrosstabReportSelect(int entityID)
        {
            InitializeComponent();

            EntityID = entityID;
            GetEntityCrosstabReports();
        }

      
     
        private void GetEntityCrosstabReports()
        {
            var listEntityCrosstabReports = bizEntityCrosstabReport.GetEntityCrosstabReports(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID);
            dtgItems.ItemsSource = listEntityCrosstabReports;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var item = dtgItems.SelectedItem as EntityCrosstabReportDTO;
            if (item != null)
            {
                if (EntityCrosstabReportSelected != null)
                    EntityCrosstabReportSelected(this, new EntityCrosstabReportSelectedArg() { EntityCrosstabReportID = item.ID });
            }
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }

    public class EntityCrosstabReportSelectedArg : EventArgs
    {
        public int EntityCrosstabReportID { set; get; }
    }

}
