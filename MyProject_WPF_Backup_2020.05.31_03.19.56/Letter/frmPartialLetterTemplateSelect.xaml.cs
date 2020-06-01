
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
    public partial class frmPartialLetterTemplateSelect: UserControl
    {
        public event EventHandler<PartialLetterTemplateSelectedArg> LetterTemplateSelected;
        public int EntityID { set; get; }
        BizLetterTemplate bizLetterTemplate = new BizLetterTemplate();
        public frmPartialLetterTemplateSelect(int entityID)
        {
            InitializeComponent();

            EntityID = entityID;
            this.Loaded += FrmFormula_Loaded;
        }

        private void FrmFormula_Loaded(object sender, RoutedEventArgs e)
        {
            GetLetterRelationshipTemplates();
        }
     
        private void GetLetterRelationshipTemplates()
        {
            var listLetterTemplates = bizLetterTemplate.GetPartialLetterTemplates(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID);
            dtgItems.ItemsSource = listLetterTemplates;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var item = dtgItems.SelectedItem as PartialLetterTemplateDTO;
            if (item != null)
            {
                if (LetterTemplateSelected != null)
                    LetterTemplateSelected(this, new PartialLetterTemplateSelectedArg() { ID = item.ID });
            }
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }

    public class PartialLetterTemplateSelectedArg : EventArgs
    {
        public int ID { set; get; }
    }

}
