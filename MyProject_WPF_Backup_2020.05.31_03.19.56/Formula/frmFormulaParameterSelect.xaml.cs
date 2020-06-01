
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
    public partial class frmFormulaParameterSelect : UserControl
    {
        public event EventHandler<FormulaParameterSelectedArg> FormulaParameterSelected;
        public int EntityID { set; get; }
        BizFormula bizFormula = new BizFormula();
        public frmFormulaParameterSelect(int entityID)
        {
            InitializeComponent();

            EntityID = entityID;
            this.Loaded += FrmFormula_Loaded;
        }

        private void FrmFormula_Loaded(object sender, RoutedEventArgs e)
        {
            GetFormulas();

        }
     
        private void GetFormulas()
        {
            var listFomula = bizFormula.GetFormulas(EntityID);
            dtgFormulas.ItemsSource = listFomula;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var formula = dtgFormulas.SelectedItem as FormulaDTO;
            if (formula != null)
            {
                if (FormulaParameterSelected != null)
                    FormulaParameterSelected(this, new FormulaParameterSelectedArg() { FormulaParameterID = formula.ID });
            }
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }

    public class FormulaParameterSelectedArg : EventArgs
    {
        public int FormulaParameterID { set; get; }
    }

}
