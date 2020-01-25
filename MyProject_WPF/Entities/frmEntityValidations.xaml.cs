using ModelEntites;

using MyFormulaFunctionStateFunctionLibrary;
using MyInterfaces;
using MyModelManager;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmEntityValidations.xaml
    /// </summary>
    public partial class frmEntityValidations : UserControl
    {
        EntityValidationDTO ValidationMessage { set; get; }
        BizEntityValidation bizEntityValidation = new BizEntityValidation();
        int EntityID { set; get; }
        //string Catalog { set; get; }
        int EntityValidationID { set; get; }
        public frmEntityValidations(int entityID, int entityValidationID)
        {
            InitializeComponent();
            EntityID = entityID;
            EntityValidationID = entityValidationID;
            //GetCatalog();
            //SetCodeFunctions();
            SetFromulas();
            if (EntityValidationID == 0)
                ValidationMessage = new EntityValidationDTO();
            else
                GetEntityValidation(EntityValidationID);
        }
        //private void GetCatalog()
        //{
        //    BizTableDrivedEntity bizTableDrivedActivity = new BizTableDrivedEntity();
        //    Catalog = bizTableDrivedActivity.GetCatalog(EntityID);
        //}
        private void SetFromulas()
        {
            cmbFormula.DisplayMemberPath = "Name";
            cmbFormula.SelectedValuePath = "ID";
            BizFormula bizFormula = new BizFormula();
            cmbFormula.ItemsSource = bizFormula.GetFormulas(EntityID);
        }

        //private void SetCodeFunctions()
        //{
        //    cmbCodeFunction.DisplayMemberPath = "Name";
        //    cmbCodeFunction.SelectedValuePath = "ID";
        //    BizCodeFunction bizCodeFunction = new BizCodeFunction();
        //    cmbCodeFunction.ItemsSource = bizCodeFunction.GetCodeFunctions(Enum_CodeFunctionParamType.ManyDataItems);
        //}
        private void GetEntityValidation(int entityValidationID)
        {
            ValidationMessage = bizEntityValidation.GetEntityValidation(entityValidationID, false);
            ShowValidationMessage();
        }

        private void ShowValidationMessage()
        {
            txtTitle.Text = ValidationMessage.Title;
            txtMessage.Text = ValidationMessage.Message;
            //chkReslutSensetive.IsChecked = ValidationMessage.ResultSensetive;
            if (ValidationMessage.FormulaID != 0)
            {
                cmbFormula.SelectedValue = ValidationMessage.FormulaID;
                optFormula.IsChecked = true;
            }
            //else if (ValidationMessage.CodeFunctionID != 0)
            //{

            //    cmbCodeFunction.SelectedValue = ValidationMessage.CodeFunctionID;
            //    optCode.IsChecked = true;
            //}
        }
        private void optFormula_Checked(object sender, RoutedEventArgs e)
        {
            //    tabCode.Visibility = Visibility.Collapsed;
            tabFormula.Visibility = Visibility.Visible;
            tabFormula.IsSelected = true;
        }


        //private void GetCodePath(int codeFunctionID)
        //{
        //    BizCodeFunction bizCodeFunction = new BizCodeFunction();
        //    var codeFunction = bizCodeFunction.GetCodeFunction(codeFunctionID);
        //    txtCodePath.Text = codeFunction.Path;
        //}

        //private void optCode_Checked(object sender, RoutedEventArgs e)
        //{
        //    tabFormula.Visibility = Visibility.Collapsed;
        //    tabCode.Visibility = Visibility.Visible;
        //    tabCode.IsSelected = true;
        //}

        private void btnAddFormula_Click(object sender, RoutedEventArgs e)
        {

            frmFormula view = new frmFormula(cmbFormula.SelectedItem==null?0: (int)cmbFormula.SelectedValue, EntityID);
            view.FormulaUpdated += View_FormulaSelected;
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form");
        }

        private void View_FormulaSelected(object sender, FormulaSelectedArg e)
        {
            SetFromulas();
            cmbFormula.SelectedValue = e.FormulaID;

        }

        //private void GetFormulaName(int formulaID)
        //{
        //    BizFormula bizFormula = new BizFormula();
        //    var formula = bizFormula.GetFormula(formulaID);
        //    txtFormulaName.Text = formula.Name;

        //}
        //private void btnAddCodeFunction_Click(object sender, RoutedEventArgs e)
        //{
        //    var selectedItem = cmbCodeFunction.SelectedItem as CodeFunctionDTO;
        //    frmCodeFunction view = new frmCodeFunction((selectedItem == null ? 0 : selectedItem.ID), Enum_CodeFunctionParamType.ManyDataItems);
        //    view.CodeFunctionUpdated += View_CodeSelected;
        //    MyProjectManager.GetMyProjectManager.ShowDialog(view, "تنظیمات نامه");
        //}


        //private void View_CodeSelected(object sender, CodeSelectedArg e)
        //{
        //    SetCodeFunctions();
        //    cmbCodeFunction.SelectedValue = e.CodeFunctionID;
        //}

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtTitle.Text == "")
            {
                MessageBox.Show("عنوان مناسب تعریف نشده است");
                return;
            }
            //if (txtFormulaValue.Text == "")
            //{
            //    MessageBox.Show("مقدار مورد تایید وارد نشده است");
            //    return;
            //}
            if (txtMessage.Text == "")
            {
                MessageBox.Show("پیغام مناسب تعریف نشده است");
                return;
            }
            if (optFormula.IsChecked == false)
            {
                MessageBox.Show("یکی از حالات فرمول و یا کد را انتخاب نمایید");
                return;
            }
            if (optFormula.IsChecked == true)
            {
                if (cmbFormula.SelectedItem == null)
                {
                    MessageBox.Show("فرمول مشخص نشده است");
                    return;
                }
            }
            if (cmbFormula.SelectedItem != null)
            {
                var formula = cmbFormula.SelectedItem as FormulaDTO;
                if (formula.ResultDotNetType!=typeof(bool)
                    && formula.ResultDotNetType != typeof(Boolean))
                {
                    MessageBox.Show("مقدار نتیجه فرمول باید Boolean باشد");
                    return;
                }
            }
            //else if (optCode.IsChecked == true)
            //{
            //    if (cmbCodeFunction.SelectedItem == null)
            //    {
            //        MessageBox.Show("کد مشخص نشده است");
            //        return;
            //    }
            //}

            ValidationMessage.TableDrivedEntityID = EntityID;
            ValidationMessage.Message = txtMessage.Text;
            ValidationMessage.Title = txtTitle.Text;
            //    ValidationMessage.ResultSensetive = chkReslutSensetive.IsChecked == true;
            if (optFormula.IsChecked == true)
            {
                //   ValidationMessage.CodeFunctionID = 0;
                ValidationMessage.FormulaID = (int)cmbFormula.SelectedValue;
                //ValidationMessage.Value = txtFormulaValue.Text;

            }
            //else if (optCode.IsChecked == true)
            //{
            //    ValidationMessage.FormulaID = 0;
            //    ValidationMessage.CodeFunctionID = (int)cmbCodeFunction.SelectedValue;
            //    ValidationMessage.Value = txtCodeValue.Text;
            //}
            bizEntityValidation.UpdateEntityValidations(ValidationMessage);
            MessageBox.Show("اطلاعات ثبت شد");
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            ValidationMessage = new EntityValidationDTO();
            ShowValidationMessage();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            frmEntityValidationSelect view = new MyProject_WPF.frmEntityValidationSelect(EntityID);
            view.EntityValidationSelected += View_EntityValidationSelected;
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "frmEntityValidationSelect");
        }

        private void View_EntityValidationSelected(object sender, EntityValidationSelectedArg e)
        {
            if (e.EntityValidationID != 0)
            {
                GetEntityValidation(e.EntityValidationID);
            }
        }


    }

}
