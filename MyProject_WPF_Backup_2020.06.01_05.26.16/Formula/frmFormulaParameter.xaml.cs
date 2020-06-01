
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
    public partial class frmFormulaParameter : Window
    {
        public event EventHandler<FormulaParameterUpdatedArg> FormulaParameterUpdated;
        //public int TargetColumnID { set; get; }
        //public int DefaultFormulaID { set; get; }
        //FormulaParameterIntention FormulaParameterIntention { set; get; }
        //FormulaDTO Formula { set; get; }
        FormulaParameterDTO FormulaParameter { set; get; }
        //RadExpressionEditor ExpressionEditor;
        int EntityID { set; get; }
        public frmFormulaParameter(int parameterID, int entityID)
        {
            InitializeComponent();
            EntityID = entityID;
            //ExpressionEditor = new RadExpressionEditor();
            //grdMain.Children.Add(ExpressionEditor);
            //int entityID = 11;

            //FormulaParameterIntention = formulaParameterIntention;
            SetFormulas();
            if (parameterID != 0)
                GetFormulaParameter(parameterID);
            else
            {
                FormulaParameter = new FormulaParameterDTO();
                ShowFormulaParameter();
                //ShowFormulaOfFormulaParameter();
            }
        }

        private void SetFormulas()
        {
            var formulas = bizFormula.GetFormulas(EntityID);
            lokFormula.DisplayMember = "Name";
            lokFormula.SelectedValueMember = "ID";
            lokFormula.ItemsSource = formulas;
            lokFormula.NewItemEnabled = true;
            lokFormula.EditItemEnabled = true;
           
        }

      

        private void GetFormulaParameter(int formulaID)
        {
            FormulaParameter = bizFormula.GetFormulaParameter(formulaID);
            ShowFormulaParameter();
        }


        FormulaHelper formulaHelper = new FormulaHelper();
        BizFormula bizFormula = new BizFormula();
        BizColumn bizColumn = new BizColumn();


        private void ShowFormulaParameter()
        {
            txtParameterName.Text = FormulaParameter.Name;
            txtParameterTitle.Text = FormulaParameter.Title;
            lokFormula.SelectedValue = FormulaParameter.FormulaID;
            //GetFormulaOfFormulaParameter(FormulaParameter.FormulaID);
        }

        //private void GetFormulaOfFormulaParameter(int formulaID)
        //{
        //    if (formulaID != 0)
        //    {
        //        Formula = bizFormula.GetFormula(formulaID, true);
        //        ShowFormulaOfFormulaParameter();
        //    }
        //    else
        //    {
        //        dtgParameters.ItemsSource = null;
        //        //txtParameterFormula.Text = "";
        //    }
        //}
        //private void ShowFormulaOfFormulaParameter()
        //{
        //    dtgParameters.ItemsSource = Formula.FormulaItems;
        //    //txtParameterFormula.Text = Formula.Formula;
        //}
        private void btnSaveParameter_Click(object sender, RoutedEventArgs e)
        {
            if (lokFormula.SelectedItem == null)
            {
                MessageBox.Show("فرمول نامشخص است");
                return;
            }
            if (txtParameterName.Text == "")
            {
                MessageBox.Show("نام نامشخص است");
                return;
            }

            FormulaParameter.Name = txtParameterName.Text;
            FormulaParameter.Title = txtParameterTitle.Text;
            FormulaParameter.FormulaID = (int)lokFormula.SelectedValue ;
            FormulaParameter.EntityID = EntityID;
            FormulaParameter.ID = bizFormula.UpdateFormulaParameterss(FormulaParameter);
            if (FormulaParameterUpdated != null)
                FormulaParameterUpdated(this, new FormulaParameterUpdatedArg() { ParameterID = FormulaParameter.ID });
            //this.Close();
        }

        private void btnNewParameter_Click(object sender, RoutedEventArgs e)
        {
            FormulaParameter = new FormulaParameterDTO();
            ShowFormulaParameter();
        }



        //private void View_FormulaSelected(object sender, FormulaSelectedArg e)
        //{
        //    GetFormulaOfFormulaParameter(e.FormulaID);
        //}

        private void lokFormula_EditItemClicked(object sender, MyCommonWPFControls.EditItemClickEventArg e)
        {
            frmFormula view;
            if (lokFormula.SelectedItem != null)
                view = new frmFormula((int)lokFormula.SelectedValue, EntityID);
            else
                view = new frmFormula(0, EntityID);
            view.FormulaSelected += View_FormulaSelected;
            view.Show();
        }

        private void View_FormulaSelected(object sender, FormulaSelectedArg e)
        {
            SetFormulas();
            lokFormula.SelectedValue = e.FormulaID;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            frmFormulaParameterSelect view = new frmFormulaParameterSelect(EntityID);
            view.FormulaParameterSelected += View_FormulaSelected;
            view.ShowDialog();
        }

        private void View_FormulaSelected(object sender, FormulaParameterSelectedArg e)
        {
            if (e.FormulaParameterID != 0)
            {
                GetFormulaParameter(e.FormulaParameterID);
            }
        }
    }



    public class FormulaParameterUpdatedArg : EventArgs
    {
        public int ParameterID { set; get; }
    }




}
