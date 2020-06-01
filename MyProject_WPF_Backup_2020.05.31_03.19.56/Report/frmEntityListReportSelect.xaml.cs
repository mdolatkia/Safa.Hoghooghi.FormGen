
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
    public partial class frmEntityListReportSelect: UserControl
    {
        public event EventHandler<EntityListReportSelectedArg> EntityListReportSelected;
        public int EntityID { set; get; }
        BizEntityListReport bizEntityListReport = new BizEntityListReport();
        public frmEntityListReportSelect(int entityID)
        {
            InitializeComponent();

            EntityID = entityID;
            GetEntityListReports();
        }

      
     
        private void GetEntityListReports()
        {
            var listEntityListReports = bizEntityListReport.GetEntityListReports(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID);
            dtgItems.ItemsSource = listEntityListReports;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var item = dtgItems.SelectedItem as EntityListReportDTO;
            if (item != null)
            {
                if (EntityListReportSelected != null)
                    EntityListReportSelected(this, new EntityListReportSelectedArg() { EntityListReportID = item.ID });
            }
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }

    public class EntityListReportSelectedArg : EventArgs
    {
        public int EntityListReportID { set; get; }
    }

}
