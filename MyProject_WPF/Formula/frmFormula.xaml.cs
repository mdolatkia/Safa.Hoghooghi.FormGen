
using ModelEntites;
using MyDataSearchManagerBusiness;
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
    public partial class frmFormula : UserControl
    {
        public event EventHandler<FormulaSelectedArg> FormulaUpdated;
        //FormulaIntention FormulaIntention { set; get; }
        int EntityID { set; get; }
        FormulaDTO Formula { set; get; }
        //public frmFormula()
        //{
        //    InitializeComponent();
        //    //FormulaIntention = new FormulaIntention() { DefaultFormulaID = 3006, Type = Enum_FormulaIntention.FormulaForParameter, EntityID = 11 };
        //}
        public frmFormula(int formulaID, int entityID)
        {
            InitializeComponent();
            //FormulaIntention = formulaIntention;
            EntityID = entityID;
            if (formulaID != 0)
                GetFormula(formulaID);
            else
            {
                Formula = new FormulaDTO();
                ShowFormula();
            }
            //cmbCustomType.ItemsSource = Enum.GetValues(typeof(ValueCustomType));
        }


        //FormulaHelper formulaHelper = new FormulaHelper();
        BizFormula bizFormula = new BizFormula();
        BizColumn bizColumn = new BizColumn();


        private void GetFormula(int formulaID)
        {
            Formula = bizFormula.GetFormula(formulaID, true);
            EntityID = Formula.EntityID;
            ShowFormula();
        }

        private void ShowFormula()
        {
            txtTitle.Text = Formula.Title;
            txtName.Text = Formula.Name;
            txtVersion.Text = Formula.Version.ToString();
            //cmbCustomType.SelectedItem = Formula.ValueCustomType;
            ShowTreeParameters(Formula.FormulaItems, treeParameters.Items);
            txtFormula.Text = Formula.Formula;
            btnFormula.IsEnabled = !Formula.FormulaUsed;
        }
        private void ShowTreeParameters(List<FormulaItemDTO> formulaItems, ItemCollection items)
        {
            if (items == treeParameters.Items)
                treeParameters.Items.Clear();
            foreach (var item in formulaItems)
            {
                RadTreeViewItem node = new RadTreeViewItem();
                node.Header =  item.RelationshipNameTail + (string.IsNullOrEmpty(item.RelationshipNameTail) ?"":".") + item.ItemTitle;
                items.Add(node);
                //ShowTreeParameters(item.ChildFormulaItems, node.Items);
                node.ExpandAll();
            }
        }
        //ParametersForFormula ParametersForFormula { set; get; }
        private void btnFormula_Click(object sender, RoutedEventArgs e)
        {
            //BindableTypeDescriptor CustomType = null;
            //ParametersForFormula = formulaHelper.GetFormulaParameters(FormulaIntention.Type, FormulaIntention.TableID);
            frmFormulaDefinition view = new frmFormulaDefinition(txtFormula.Text, EntityID);
            view.FormulaDefined += View_FormulaDefined;
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form",Enum_WindowSize.Maximized);
        }
        private void View_FormulaDefined(object sender, FormulaDefinedArg e)
        {
            txtFormula.Text = e.Expression;
            Formula.ResultType = e.ExpressionResultType.ToString();
            Formula.FormulaItems.Clear();
            Formula.FormulaItems = e.FormulaItems;
            ShowTreeParameters(Formula.FormulaItems, treeParameters.Items);


            MyProjectManager.GetMyProjectManager.CloseDialog(sender);
            //if (e.FormulaItems != null)
            //{
            //    Formula.FormulaItems.Clear();
            //    foreach (var item in e.FormulaItems)
            //    {
            //        Formula_FormulaParameterDTO param = new Formula_FormulaParameterDTO();
            //        if (item is ExistingFormulaParameter)
            //        {
            //            param.FormulaParameterID = (item as ExistingFormulaParameter).FormulaParameterID;
            //        }
            //        else if (item is ColumnFormulaParameter)
            //        {
            //            param.ColumnID = (item as ColumnFormulaParameter).ColumnID;
            //        }
            //        param.FormulaParameterPath = item.FormulaParameterFullPath;
            //        Formula.Parameters.Add(param);
            //    }

            //}
        }
        private void btnSaveAndSelect_Click(object sender, RoutedEventArgs e)
        {
            if (txtFormula.Text == "")
            {
                MessageBox.Show("فرمول نامشخص است");
                return;
            }
            if (txtName.Text == "")
            {
                MessageBox.Show("نام نامشخص است");
                return;
            }
            if (txtTitle.Text == "")
            {
                MessageBox.Show("عنوان نامشخص است");
                return;
            }
            Formula.Name = txtName.Text;
            Formula.EntityID = EntityID;
            Formula.Title = txtTitle.Text;
            //Formula.ValueCustomType = (ValueCustomType)cmbCustomType.SelectedItem;
            Formula.Version = (txtVersion.Text == "" ? 0 : Convert.ToInt32(txtVersion.Text));
            Formula.Formula = txtFormula.Text;
            Formula.ID = bizFormula.UpdateFormulas(Formula);

            //اگر برای ستون بود به ستون ارتباط میدهد
            //if (FormulaIntention.Type == Enum_FormulaIntention.FormulaForColumn)
            //{
            //bizColumn.UpdateCustomCalculation(FormulaIntention.ColumnID, Formula.ID);
            //}
            if (FormulaUpdated != null)
                FormulaUpdated(this, new FormulaSelectedArg() { FormulaID = Formula.ID });
            //MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Formula = new FormulaDTO();
            ShowFormula();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            frmFormulaSelect view = new frmFormulaSelect(EntityID);
            view.FormulaSelected += View_FormulaSelected;
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form");
        }

        private void View_FormulaSelected(object sender, FormulaSelectedArg e)
        {
            if (e.FormulaID != 0)
            {
                GetFormula(e.FormulaID);
            }
        }


      
     
    }







}
