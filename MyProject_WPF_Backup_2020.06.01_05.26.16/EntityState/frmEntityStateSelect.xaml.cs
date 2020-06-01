
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
    public partial class frmEntityStateSelect : UserControl
    {
        public event EventHandler<EntityStateSelectedArg> EntityStateSelected;
        public int EntityID { set; get; }
        BizEntityState bizEntityState = new BizEntityState();
        public frmEntityStateSelect(int entityID)
        {
            InitializeComponent();

            EntityID = entityID;
            GetEntityStates();
        }

       
     
        private void GetEntityStates()
        {
            var listEntityStates = bizEntityState.GetEntityStates(EntityID,false);
            dtgItems.ItemsSource = listEntityStates;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var item = dtgItems.SelectedItem as EntityStateDTO;
            if (item != null)
            {
                if (EntityStateSelected != null)
                    EntityStateSelected(this, new EntityStateSelectedArg() { EntityStateID = item.ID });
            }
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }

    public class EntityStateSelectedArg : EventArgs
    {
        public int EntityStateID { set; get; }
    }

}
