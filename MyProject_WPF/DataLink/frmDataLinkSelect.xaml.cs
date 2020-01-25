
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
    public partial class frmDataLinkSelect : UserControl
    {
        public event EventHandler<DataLinkSelectedArg> LetterTemplateSelected;
        public int EntityID { set; get; }
        BizDataLink bizDataLink = new BizDataLink();
        public frmDataLinkSelect(int entityID)
        {
            InitializeComponent();
            EntityID = entityID;
            GetLetterRelationshipTemplates();
        }
     
        private void GetLetterRelationshipTemplates()
        {
            var listLetterTemplates = bizDataLink.GetDataLinkByEntitiyID(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID);
            dtgItems.ItemsSource = listLetterTemplates;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var item = dtgItems.SelectedItem as DataLinkDTO;
            if (item != null)
            {
                if (LetterTemplateSelected != null)
                    LetterTemplateSelected(this, new DataLinkSelectedArg() { ID = item.ID });
            }
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }

    public class DataLinkSelectedArg : EventArgs
    {
        public int ID { set; get; }
    }

}
