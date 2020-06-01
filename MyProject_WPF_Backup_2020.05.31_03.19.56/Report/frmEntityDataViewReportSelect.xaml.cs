
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
    public partial class frmEntityDataViewReportSelect: UserControl
    {
        public event EventHandler<EntityDataViewReportSelectedArg> EntityDataViewReportSelected;
        public int EntityID { set; get; }
        BizEntityDataViewReport bizEntityDataViewReport = new BizEntityDataViewReport();
        public frmEntityDataViewReportSelect(int entityID)
        {
            InitializeComponent();

            EntityID = entityID;
            GetEntityDataViewReports();
        }

      
     
        private void GetEntityDataViewReports()
        {
            var listEntityDataViewReports = bizEntityDataViewReport.GetEntityDataViewReports(EntityID);
            dtgItems.ItemsSource = listEntityDataViewReports;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var item = dtgItems.SelectedItem as EntityDataViewReportDTO;
            if (item != null)
            {
                if (EntityDataViewReportSelected != null)
                    EntityDataViewReportSelected(this, new EntityDataViewReportSelectedArg() { EntityDataViewReportID = item.ID });
            }
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }

    public class EntityDataViewReportSelectedArg : EventArgs
    {
        public int EntityDataViewReportID { set; get; }
    }

}
