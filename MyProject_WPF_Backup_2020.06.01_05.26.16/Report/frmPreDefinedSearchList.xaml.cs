
using ModelEntites;
using MyFormulaFunctionStateFunctionLibrary;
using MyModelManager;
using ProxyLibrary;
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
    public partial class frmSearchRepositoryList : UserControl
    {
        public event EventHandler<SearchRepositorySelectedArg> PreDefinedSearchSelected;
        public int EntityID { set; get; }
        BizSearchRepository bizSearchRepository = new BizSearchRepository();
        public frmSearchRepositoryList(int entityID)
        {
            InitializeComponent();

            EntityID = entityID;
            GetPreDefinedSearchs();
        }

      
     
        private void GetPreDefinedSearchs()
        {
            var listPreDefinedSearchs = bizSearchRepository.GetSearchRepositories(EntityID);
            dtgItems.ItemsSource = listPreDefinedSearchs;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var item = dtgItems.SelectedItem as DP_SearchRepository;
            if (item != null)
            {
                if (PreDefinedSearchSelected != null)
                    PreDefinedSearchSelected(this, new SearchRepositorySelectedArg() { SearchRepositoryID = item.ID });
            }
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }

    public class SearchRepositorySelectedArg : EventArgs
    {
        public int SearchRepositoryID { set; get; }
    }

}
