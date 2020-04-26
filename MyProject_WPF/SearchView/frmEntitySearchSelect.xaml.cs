
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
        SelectorGrid SelectorGrid = null;

        public frmEntitySearchSelect(int entityID)
        {
            InitializeComponent();

            EntityID = entityID;

            var listColumns = new Dictionary<string, string>();
            listColumns.Add("ID", "شناسه");
            listColumns.Add("Title", "عنوان");
            SelectorGrid = ControlHelper.SetSelectorGrid(dtgItems, listColumns);
            SelectorGrid.DataItemSelected += SelectorGrid_DataItemSelected;


            GetEntitySearchs();
        }
        private void SelectorGrid_DataItemSelected(object sender, object e)
        {
            CheckSelectedItem(e);
        }

        private void CheckSelectedItem(object item)
        {
            if (item != null)
            {
                var selected = dtgItems.SelectedItem as EntitySearchDTO;
                if (selected != null)
                {
                    if (EntitySearchSelected != null)
                        EntitySearchSelected(this, new EntitySearchSelectedArg() { EntitySearchID = selected.ID });
                }
            }
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
            CheckSelectedItem(dtgItems.SelectedItem);
        }
    }

    public class EntitySearchSelectedArg : EventArgs
    {
        public int EntitySearchID { set; get; }
    }

}
