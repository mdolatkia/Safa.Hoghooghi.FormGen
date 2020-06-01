
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
    public partial class frmEntityDirectReportSelect: UserControl
    {
        public event EventHandler<EntityDirectReportSelectedArg> EntityDirectReportSelected;
        public int EntityID { set; get; }
        BizEntityDirectReport bizEntityDirectReport = new BizEntityDirectReport();
        public frmEntityDirectReportSelect(int entityID)
        {
            InitializeComponent();

            EntityID = entityID;
            GetEntityDirectReports();
        }

      
     
        private void GetEntityDirectReports()
        {
            var listEntityDirectReports = bizEntityDirectReport.GetEntityDirectReports(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID);
            dtgItems.ItemsSource = listEntityDirectReports;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var item = dtgItems.SelectedItem as EntityDirectReportDTO;
            if (item != null)
            {
                if (EntityDirectReportSelected != null)
                    EntityDirectReportSelected(this, new EntityDirectReportSelectedArg() { EntityDirectReportID = item.ID });
            }
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }

    public class EntityDirectReportSelectedArg : EventArgs
    {
        public int EntityDirectReportID { set; get; }
    }

}
