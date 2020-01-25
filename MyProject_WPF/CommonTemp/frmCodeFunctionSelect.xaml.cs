
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
    public partial class frmCodeFunctionSelect : UserControl
    {
        public event EventHandler<CodeSelectedArg> CodeFunctionSelected;
        //public string Catalog { set; get; }
        public int EntityID { set; get; }
        BizCodeFunction bizCode = new BizCodeFunction();
        List<Enum_CodeFunctionParamType> CodeFunctionParamType { set; get; }
        public frmCodeFunctionSelect(List<Enum_CodeFunctionParamType> codeFunctionParamType)
        {
            InitializeComponent();
            CodeFunctionParamType = codeFunctionParamType;
            //Catalog = catalog;
            this.Loaded += FrmFormula_Loaded;
        }

        private void FrmFormula_Loaded(object sender, RoutedEventArgs e)
        {
            GetCodes();

        }

        private void GetCodes()
        {
            var listCodes = bizCode.GetCodeFunctions(CodeFunctionParamType);
            dtgItems.ItemsSource = listCodes;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var item = dtgItems.SelectedItem as CodeFunctionDTO;
            if (item != null)
            {
                if (CodeFunctionSelected != null)
                    CodeFunctionSelected(this, new CodeSelectedArg() { CodeFunctionID = item.ID });
            }
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }

    public class CodeSelectedArg : EventArgs
    {
        public int CodeFunctionID { set; get; }
    }

}
