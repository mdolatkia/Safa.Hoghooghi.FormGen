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
    /// Interaction logic for frmActionActivitys.xaml
    /// </summary>
    public partial class frmBackendActionActivity : UserControl
    {
        BackendActionActivityDTO Message { set; get; }
        BizDatabaseFunction bizDatabaseFunction = new BizDatabaseFunction();
        BizBackendBackendActionActivity bizActionActivity = new BizBackendBackendActionActivity();
        public event EventHandler<SavedItemArg> ItemSaved;
        int ActionActivityID { set; get; }
        //string Catalog { set; get; }
        int EntityID { set; get; }
        //Enum_CodeFunctionParamType CodeFunctionParamType { set; get; }
        List<ColumnValueValidValuesDTO> ValidValues = new List<ColumnValueValidValuesDTO>();
        //bool VisualActions { set; get; }
        //  bool IncludeSteps { set; get; }
        //فرم جدا هم صدا زده شود 
        public frmBackendActionActivity(int actionActivityID, int entityID)
        {
            InitializeComponent();
            //EntityID = entityID;
            //Catalog = catalog;


            //     IncludeSteps = includeSteps;
            //   SetAllowedActivityTypes();
            ActionActivityID = actionActivityID;
            EntityID = entityID;
            //CodeFunctionParamType = codeFunctionParamType;
            //if(EntityID==0)
            //{
            //    optRelationshipEnablity.Visibility = Visibility.Collapsed;
            //    tabRelationshipEnablity.Visibility = Visibility.Collapsed;
            //    optColumnValue.Visibility = Visibility.Collapsed;
            //    tabColumnValue.Visibility = Visibility.Collapsed;
            //}
            //else
            //{
            SetStepTypes();

            //if (ActionActivityTypes.Contains(Enum_ActionActivityType.CodeFunction))
            //    SetCodeFunctions();
            if (ActionActivityID == 0)
                Message = new BackendActionActivityDTO();
            else
                GetActionActivity(ActionActivityID);


        }

        //private void SetAllowedActivityTypes()
        //{

        //}

        private void SetStepTypes()
        {
            SetDatabaseFunctions();

            cmbStep.SelectionChanged += CmbStep_SelectionChanged;
            cmbStep.ItemsSource = Enum.GetValues(typeof(Enum_EntityActionActivityStep));
        }



        private void CmbStep_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetCodeFunctions();
            if ((Enum_EntityActionActivityStep)cmbStep.SelectedItem == Enum_EntityActionActivityStep.BeforeLoad
                || (Enum_EntityActionActivityStep)cmbStep.SelectedItem == Enum_EntityActionActivityStep.BeforeSave
                  || (Enum_EntityActionActivityStep)cmbStep.SelectedItem == Enum_EntityActionActivityStep.BeforeDelete)
            {

                //optCode.Visibility = Visibility.Collapsed;
                //tabCode.Visibility = Visibility.Collapsed;

                optDatabaseFunction.Visibility = Visibility.Collapsed;
                tabDatabaseFunction.Visibility = Visibility.Collapsed;

            }
            else
            {
                optDatabaseFunction.Visibility = Visibility.Visible;
                tabDatabaseFunction.Visibility = Visibility.Visible;
            }
        }


        private void SetCodeFunctions()
        {

            Enum_CodeFunctionParamType? codeFunctionParamType = GetCodeFunctionParamType();


            if (codeFunctionParamType != null)
            {
                cmbCodeFunction.DisplayMemberPath = "Name";
                cmbCodeFunction.SelectedValuePath = "ID";
                BizCodeFunction bizCodeFunction = new BizCodeFunction();
                cmbCodeFunction.ItemsSource = bizCodeFunction.GetCodeFunctions(new List<Enum_CodeFunctionParamType>() { codeFunctionParamType.Value });
            }
            else
                cmbCodeFunction.ItemsSource = null;
        }

        private Enum_CodeFunctionParamType? GetCodeFunctionParamType()
        {

            if (cmbStep.SelectedItem != null)
            {

                var selectedItem = (Enum_EntityActionActivityStep)cmbStep.SelectedItem;
                if (selectedItem == Enum_EntityActionActivityStep.BeforeLoad)
                    return Enum_CodeFunctionParamType.ManyDataItems;
                else if (selectedItem == Enum_EntityActionActivityStep.BeforeSave ||
                        selectedItem == Enum_EntityActionActivityStep.AfterSave ||
                        selectedItem == Enum_EntityActionActivityStep.BeforeDelete ||
                        selectedItem == Enum_EntityActionActivityStep.AfterDelete)
                    return Enum_CodeFunctionParamType.OneDataItem;

            }
            return null;

        }


        //private void SetActionActivityType()
        //{
        //    cmbActionActivityType.ItemsSource = Enum.GetValues(typeof(Enum_ActionActivityType));
        //}

        private void SetDatabaseFunctions()
        {
            cmbDatabaseFunction.DisplayMemberPath = "FunctionName";
            cmbDatabaseFunction.SelectedValuePath = "ID";
            cmbDatabaseFunction.ItemsSource = bizDatabaseFunction.GetDatabaseFunctions(Enum_DatabaseFunctionType.StoredProcedure);
        }

        private void GetActionActivity(int actionActivityID)
        {
            Message = bizActionActivity.GetBackendActionActivity(actionActivityID);
            ShowMessage();
        }

        private void ShowMessage()
        {
            txtTitle.Text = Message.Title;
            //chkReslutSensetive.IsChecked = Message.ResultSensetive;
            cmbStep.SelectedItem = Message.Step;
            if (Message.Type == Enum_ActionActivityType.DatabaseFunction)
            {
                cmbDatabaseFunction.SelectedValue = Message.DatabaseFunctionID;
                optDatabaseFunction.IsChecked = true;
            }
            if (Message.Type == Enum_ActionActivityType.CodeFunction)
            {
                cmbCodeFunction.SelectedValue = Message.CodeFunctionID;
                optCode.IsChecked = true;
            }

        }
        private void optFormula_Checked(object sender, RoutedEventArgs e)
        {
            HideTabs();
            tabDatabaseFunction.Visibility = Visibility.Visible;
            tabDatabaseFunction.IsSelected = true;
        }

        private void HideTabs()
        {
            tabCode.Visibility = Visibility.Collapsed;
            tabDatabaseFunction.Visibility = Visibility.Collapsed;
        }


        //private void GetCodePath(int codeFunctionID)
        //{
        //    BizCodeFunction bizCodeFunction = new BizCodeFunction();
        //    var codeFunction = bizCodeFunction.GetCodeFunction(codeFunctionID);
        //    txtCodePath.Text = codeFunction.Path;
        //}

        private void optCode_Checked(object sender, RoutedEventArgs e)
        {
            HideTabs();
            tabCode.Visibility = Visibility.Visible;
            tabCode.IsSelected = true;
        }


        private void btnAddCodeFunction_Click(object sender, RoutedEventArgs e)
        {
            var codeFunctionParamType = GetCodeFunctionParamType();
            if (codeFunctionParamType != null)
            {
                var selectedItem = cmbCodeFunction.SelectedItem as CodeFunctionDTO;
                frmCodeFunction view = new frmCodeFunction((selectedItem == null ? 0 : selectedItem.ID), codeFunctionParamType.Value,EntityID);
                view.CodeFunctionUpdated += View_CodeSelected;
                MyProjectManager.GetMyProjectManager.ShowDialog(view, "تنظیمات نامه");
            }
            else
                MessageBox.Show("نوع کد مشخص نمیباشد");
        }

        private void View_CodeSelected(object sender, CodeSelectedArg e)
        {
            SetCodeFunctions();
            cmbCodeFunction.SelectedValue = e.CodeFunctionID;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtTitle.Text == "")
            {
                MessageBox.Show("عنوان مناسب تعریف نشده است");
                return;
            }

            if (cmbStep.SelectedItem == null)
            {
                MessageBox.Show("مرحله مناسب تعریف نشده است");
                return;
            }

            if (optDatabaseFunction.IsChecked == false && optCode.IsChecked == false)
            {
                MessageBox.Show("یکی از حالات شروط را انتخاب نمایید");
                return;
            }
            if (optDatabaseFunction.IsChecked == true)
            {
                if (cmbDatabaseFunction.SelectedItem == null)
                {
                    MessageBox.Show("تابع مشخص نشده است");
                    return;
                }
            }
            else if (optCode.IsChecked == true)
            {
                if (cmbCodeFunction.SelectedItem == null)
                {
                    MessageBox.Show("کد مشخص نشده است");
                    return;
                }
            }

            Message.EntityID = EntityID;
            Message.Title = txtTitle.Text;
            Message.Step = (Enum_EntityActionActivityStep)cmbStep.SelectedItem;
            //   Message.ResultSensetive = chkReslutSensetive.IsChecked == true;
            Message.DatabaseFunctionID = 0;
            Message.CodeFunctionID = 0;

            if (optDatabaseFunction.IsChecked == true)
            {
                Message.DatabaseFunctionID = (int)cmbDatabaseFunction.SelectedValue;
                Message.Type = Enum_ActionActivityType.DatabaseFunction;
            }
            else if (optCode.IsChecked == true)
            {
                Message.CodeFunctionID = (int)cmbCodeFunction.SelectedValue;
                Message.Type = Enum_ActionActivityType.CodeFunction;
            }


            Message.ID = bizActionActivity.UpdateBackendActionActivitys(Message);
            if (ItemSaved != null)
                ItemSaved(this, new SavedItemArg() { ID = Message.ID });
            MessageBox.Show("اطلاعات ثبت شد");


        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Message = new BackendActionActivityDTO();
            ShowMessage();
        }

        //private void btnSearch_Click(object sender, RoutedEventArgs e)
        //{
        //    frmActionActivitySelect view = new MyProject_WPF.frmActionActivitySelect(Catalog);
        //    view.ActionActivitySelected += View_ActionActivitySelected;
        //       MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form");
        //}

        //private void View_ActionActivitySelected(object sender, ActionActivitySelectedArg e)
        //{
        //    if (e.ActionActivityID != 0)
        //    {
        //        GetActionActivity(e.ActionActivityID);
        //    }
        //}

        private void btnDatabaseFunctionEntity_Click(object sender, RoutedEventArgs e)
        {
            if (cmbDatabaseFunction.SelectedItem != null)
            {
                var selected = cmbDatabaseFunction.SelectedItem as DatabaseFunctionDTO;
                var DatabaseFunctionEntityID = bizDatabaseFunction.GetDatabaseFunctionEntityID(EntityID, selected.ID);
                var DatabaseFunctionIntention = new DatabaseFunctionEntityIntention();
                DatabaseFunctionIntention.EntityID = EntityID;
                if (DatabaseFunctionEntityID == 0)
                {
                    DatabaseFunctionIntention.DatabaseFunctionID = selected.ID;
                    DatabaseFunctionIntention.Type = Enum_DatabaseFunctionEntityIntention.DatabaseFunctionEntityDefinition;
                }
                else
                {
                    DatabaseFunctionIntention.DatabaseFunctionEntityID = DatabaseFunctionEntityID;
                    DatabaseFunctionIntention.Type = Enum_DatabaseFunctionEntityIntention.DatabaseFunctionEntityEdit;
                }
                frmDatabaseFunction_Entity view = new frmDatabaseFunction_Entity(DatabaseFunctionIntention);
                MyProjectManager.GetMyProjectManager.ShowDialog(view, "DatabaseFunction_Entity");

            }
        }




    }
    //public class SavedItemArg : EventArgs
    //{
    //    public int ID { set; get; }
    //}

}
