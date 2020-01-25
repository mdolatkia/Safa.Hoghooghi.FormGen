
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
    public partial class frmEntityListReportGroupedSelect :UserControl
    {
        public event EventHandler<EntityListReportGroupedSelectedArg> EntityListReportGroupedSelected;
        public int EntityID { set; get; }
        //BizEntityListReportGrouped bizEntityListReportGrouped = new BizEntityListReportGrouped();
        public frmEntityListReportGroupedSelect(int entityID)
        {
            InitializeComponent();

            EntityID = entityID;
            GetEntityListReportGroupeds();
        }

       
     
        //private void GetEntityListReportGroupeds()
        //{
        //    var listEntityListReportGroupeds = bizEntityListReportGrouped.GetEntityListReportGroupeds(EntityID);
        //    dtgItems.ItemsSource = listEntityListReportGroupeds;
        //}
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager().CloseDialog(this);
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var item = dtgItems.SelectedItem as EntityListReportGroupedDTO;
            if (item != null)
            {
                if (EntityListReportGroupedSelected != null)
                    EntityListReportGroupedSelected(this, new EntityListReportGroupedSelectedArg() { EntityListReportGroupedID = item.ID });
            }
            MyProjectManager.GetMyProjectManager().CloseDialog(this);
        }
    }

    public class EntityListReportGroupedSelectedArg : EventArgs
    {
        public int EntityListReportGroupedID { set; get; }
    }

}
