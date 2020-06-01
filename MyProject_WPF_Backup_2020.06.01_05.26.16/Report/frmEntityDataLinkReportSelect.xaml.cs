
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
    public partial class frmEntityDataLinkReportSelect: UserControl
    {
        public event EventHandler<EntityDataLinkReportSelectedArg> EntityDataLinkReportSelected;
        public int EntityID { set; get; }
        BizEntityDataLinkReport bizEntityDataLinkReport = new BizEntityDataLinkReport();
        public frmEntityDataLinkReportSelect(int entityID)
        {
            InitializeComponent();

            EntityID = entityID;
            GetEntityDataLinkReports();
        }

      
     
        private void GetEntityDataLinkReports()
        {
            var listEntityDataLinkReports = bizEntityDataLinkReport.GetEntityDataLinkReports(EntityID);
            dtgItems.ItemsSource = listEntityDataLinkReports;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var item = dtgItems.SelectedItem as EntityDataLinkReportDTO;
            if (item != null)
            {
                if (EntityDataLinkReportSelected != null)
                    EntityDataLinkReportSelected(this, new EntityDataLinkReportSelectedArg() { EntityDataLinkReportID = item.ID });
            }
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }

    public class EntityDataLinkReportSelectedArg : EventArgs
    {
        public int EntityDataLinkReportID { set; get; }
    }

}
