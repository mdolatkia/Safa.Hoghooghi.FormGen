
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
    public partial class frmEntityListViewSelect : UserControl
    {
        public event EventHandler<EntityListViewSelectedArg> EntityListViewSelected;
        public int EntityID { set; get; }
        BizEntityListView bizEntityListView = new BizEntityListView();
        public frmEntityListViewSelect(int entityID)
        {
            InitializeComponent();

            EntityID = entityID;
            GetEntityListViews();
        }

        private void FrmFormula_Loaded(object sender, RoutedEventArgs e)
        {
            GetEntityListViews();

        }
     
        private void GetEntityListViews()
        {
            var listEntityListViews = bizEntityListView.GetEntityListViews(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID);
            dtgItems.ItemsSource = listEntityListViews;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var item = dtgItems.SelectedItem as EntityListViewDTO;
            if (item != null)
            {
                if (EntityListViewSelected != null)
                    EntityListViewSelected(this, new EntityListViewSelectedArg() { EntityListViewID = item.ID });
            }
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }

    public class EntityListViewSelectedArg : EventArgs
    {
        public int EntityListViewID { set; get; }
    }

}
