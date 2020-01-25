
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
    public partial class frmMainLetterTemplateSelect: UserControl
    {
        public event EventHandler<MainLetterTemplateSelectedArg> LetterTemplateSelected;
        public int EntityID { set; get; }
        BizLetterTemplate bizLetterTemplate = new BizLetterTemplate();
        public frmMainLetterTemplateSelect(int entityID)
        {
            InitializeComponent();

            EntityID = entityID;
            this.Loaded += FrmFormula_Loaded;
        }

        private void FrmFormula_Loaded(object sender, RoutedEventArgs e)
        {
            GetLetterTemplates();

        }
     
        private void GetLetterTemplates()
        {
            var listLetterTemplates = bizLetterTemplate.GetMainLetterTemplates(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID);
            dtgItems.ItemsSource = listLetterTemplates;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var item = dtgItems.SelectedItem as MainLetterTemplateDTO;
            if (item != null)
            {
                if (LetterTemplateSelected != null)
                    LetterTemplateSelected(this, new MainLetterTemplateSelectedArg() { LetterTemplateID = item.ID });
            }
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }

    public class MainLetterTemplateSelectedArg : EventArgs
    {
        public int LetterTemplateID { set; get; }
    }

}
