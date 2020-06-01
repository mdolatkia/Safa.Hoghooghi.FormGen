
using ModelEntites;
using MyFormulaLibrary;
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
    public partial class frmActionActivitySelect : Window
    {
        public event EventHandler<ActionActivitySelectedArg> ActionActivitySelected;
        public string Catalog { set; get; }
        BizActionActivity bizActionActivity = new BizActionActivity();
        public frmActionActivitySelect(string catalog)
        {
            InitializeComponent();

            Catalog = catalog;
            this.Loaded += FrmFormula_Loaded;
        }

        private void FrmFormula_Loaded(object sender, RoutedEventArgs e)
        {
            GetActionActivitys();

        }
     
        private void GetActionActivitys()
        {
            var listActionActivitys = bizActionActivity.GetActionActivities(Catalog);
            dtgItems.ItemsSource = listActionActivitys;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var item = dtgItems.SelectedItem as ActionActivityDTO;
            if (item != null)
            {
                if (ActionActivitySelected != null)
                    ActionActivitySelected(this, new ActionActivitySelectedArg() { ActionActivityID = item.ID });
            }
            this.Close();
        }
    }

    public class ActionActivitySelectedArg : EventArgs
    {
        public int ActionActivityID { set; get; }
    }

}
