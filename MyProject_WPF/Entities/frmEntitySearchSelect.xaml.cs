
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
    public partial class frmEntitySearchSelect : UserControl
    {
        public event EventHandler<EntitySearchSelectedArg> EntitySearchSelected;
        public int EntityID { set; get; }
        BizEntitySearch bizEntitySearch = new BizEntitySearch();
        public frmEntitySearchSelect(int entityID)
        {
            InitializeComponent();

            EntityID = entityID;
            GetEntitySearchs();
        }

        private void FrmFormula_Loaded(object sender, RoutedEventArgs e)
        {
            GetEntitySearchs();

        }
     
        private void GetEntitySearchs()
        {
            var listEntitySearchs = bizEntitySearch.GetEntitySearchs(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID);
            dtgItems.ItemsSource = listEntitySearchs;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var item = dtgItems.SelectedItem as EntitySearchDTO;
            if (item != null)
            {
                if (EntitySearchSelected != null)
                    EntitySearchSelected(this, new EntitySearchSelectedArg() { EntitySearchID = item.ID });
            }
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }

    public class EntitySearchSelectedArg : EventArgs
    {
        public int EntitySearchID { set; get; }
    }

}
