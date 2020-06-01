
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
    public partial class frmRelationshipFilterSelect: UserControl
    {
        public event EventHandler<RelationshipFilterSelectedArg> RelationshipFilterSelected;
        public int RelationshipID { set; get; }
        BizRelationshipFilter bizRelationshipFilter = new BizRelationshipFilter();
        public frmRelationshipFilterSelect(int relationshipID)
        {
            InitializeComponent();
            RelationshipID = relationshipID;
            this.Loaded += FrmFormula_Loaded;
        }

        private void FrmFormula_Loaded(object sender, RoutedEventArgs e)
        {
            GetRelationshipFilters();

        }
     
        private void GetRelationshipFilters()
        {
            var list = bizRelationshipFilter.GetRelationshipFilters(RelationshipID);
            dtgItems.ItemsSource = list;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager().CloseDialog(this);
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var item = dtgItems.SelectedItem as RelationshipFilterDTO;
            if (item != null)
            {
                if (RelationshipFilterSelected != null)
                    RelationshipFilterSelected(this, new  RelationshipFilterSelectedArg() { ID = item.ID });
            }
            MyProjectManager.GetMyProjectManager().CloseDialog(this);
        }
    }

    public class RelationshipFilterSelectedArg : EventArgs
    {
        public int ID { set; get; }
    }

}
