using ModelEntites;
using MyGeneralLibrary;
using MyInterfaces;
using MyModelManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for frmCodeSelector.xaml
    /// </summary>
    public partial class frmCodeFunction : UserControl
    {
        BizCodeFunction bizCodeFunction = new BizCodeFunction();
        public event EventHandler<CodeSelectedArg> CodeFunctionUpdated;
        //Type ExpectedReturnType { set; get; }
        //Type ExpectedParamType { set; get; }
        //string Catalog { set; get; }
        int EntityID { set; get; }

        CodeFunctionDTO Message { set; get; }
        Enum_CodeFunctionParamType CodeFunctionParamType { set; get; }
        bool? ShowInFormula { set; get; }
        public frmCodeFunction(int codeFunctionID, Enum_CodeFunctionParamType codeFunctionParamType, int entityID, bool? showInFormula = null)
        {
            InitializeComponent();
            ShowInFormula = showInFormula;
            EntityID = entityID;
            CodeFunctionParamType = codeFunctionParamType;
            SetCombos();
            //ExpectedReturnType = expectedReturnType;
            if (CodeFunctionParamType == Enum_CodeFunctionParamType.ManyDataItems)
            {
                lblExpectedParameter.Text = typeof(CodeFunctionParamManyDataItems).ToString();
                lblExpectedReturnValue.Text = typeof(CodeFunctionResult).ToString();
            }
            else if (CodeFunctionParamType == Enum_CodeFunctionParamType.OneDataItem)
            {
                lblExpectedParameter.Text = typeof(CodeFunctionParamOneDataItem).ToString();
                lblExpectedReturnValue.Text = typeof(CodeFunctionResult).ToString();
            }
            else if (CodeFunctionParamType == Enum_CodeFunctionParamType.KeyColumns)
            {
                lblExpectedParameter.Text = "Primitive params";
                lblExpectedReturnValue.Text = typeof(CodeFunctionResult).ToString();
            }
            ////else if (CodeFunctionParamType == Enum_CodeFunctionParamType.CommandFunction)
            ////{
            ////    lblExpectedParameter.Text = typeof(CommandFunctionParam).ToString();
            ////    lblExpectedReturnValue.Text = typeof(CommandFunctionResult).ToString();
            ////}
            else if (CodeFunctionParamType == Enum_CodeFunctionParamType.LetterFunction)
            {
                lblExpectedParameter.Text = typeof(LetterFunctionParam).ToString();
                lblExpectedReturnValue.Text = typeof(CodeFunctionResult).ToString();
            }
            else if (CodeFunctionParamType == Enum_CodeFunctionParamType.LetterConvert)
            {
                lblExpectedParameter.Text = typeof(LetterFunctionParam).ToString();
                lblExpectedReturnValue.Text = typeof(LetterConvertToExternalResult).ToString();
            }
            //Catalog = catalog;
            //if (CodeFunctionParamType != Enum_CodeFunctionParamType.KeyColumns)
            //    HideColumn();
            //else
            //    SetColumnSource();

            if (codeFunctionID == 0)
                Message = new CodeFunctionDTO();
            else
                GetCodeFuntion(codeFunctionID);
        }

        private void SetCombos()
        {
            //cmbValueCustomType.ItemsSource = Enum.GetValues(typeof(ValueCustomType)).Cast<object>();
        }

        //private void SetColumnSource()
        //{
        //    BizTableDrivedEntity bizEntity = new BizTableDrivedEntity();
        //    var entity = bizEntity.GetTableDrivedEntity(EntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
        //    var rel = dtgCodeFunctionParams.Columns[1] as GridViewComboBoxColumn;
        //    rel.ItemsSource = entity.Columns;
        //    rel.DisplayMemberPath = "Name";
        //    rel.SelectedValueMemberPath = "ID";

        //}

        //private void HideColumn()
        //{
        //    var rel = dtgCodeFunctionParams.Columns[1] as GridViewComboBoxColumn;
        //    rel.IsVisible = false;
        //}

        private void GetCodeFuntion(int codeFunctionID)
        {
            Message = bizCodeFunction.GetCodeFunction(codeFunctionID);
            ShowCodeFuntion();
        }
        private void ShowCodeFuntion()
        {
            dtgSavedFunctionCurrentParams.ItemsSource = Message.Parameters;
            txtCodePath.Text = Message.Path;
            lokClassName.SelectedValue = Message.ClassName;
            lokFunctionName.SelectedValue = Message.FunctionName;
            txtName.Text = Message.Name;
            //cmbValueCustomType.SelectedItem = Message.ValueCustomType;
        }
        private void btnSaveAndSelect_Click(object sender, RoutedEventArgs e)
        {
            //if(CodeFunctionParamType==Enum_CodeFunctionParamType.KeyColumns)
            //{
            //    if (Message.Parameters.Any(x => x.ColumnID == 0))
            //    {
            //        MessageBox.Show("ستون معادل برای یکی از پارامترهای فانکشن مشخص نشده است");
            //        return;
            //    }
            //}
            if (txtName.Text != "" && txtCodePath.Text != "" && lokClassName.SelectedItem != null && lokFunctionName.SelectedItem != null)
            {
                if (File.Exists(txtCodePath.Text))
                {
                    if (CheckMethodValidity())
                    {
                        Message.Name = txtName.Text;
                        //Message.Catalog = Catalog;
                        Message.ClassName = lokClassName.SelectedValue.ToString();
                        Message.EntityID = EntityID;
                        Message.ShowInFormula = ShowInFormula;
                        Message.FunctionName = lokFunctionName.SelectedValue.ToString();
                        Message.Path = txtCodePath.Text;
                        Message.ParamType = CodeFunctionParamType;
                        Message.RetrunType = lblReturnValue.Text;
                        //if (cmbValueCustomType.SelectedItem != null)
                        //    Message.ValueCustomType = (ValueCustomType)cmbValueCustomType.SelectedItem;
                        //else
                        //    Message.ValueCustomType = ValueCustomType.None;
                        var id = bizCodeFunction.UpdateCodeFunctions(Message);
                        MessageBox.Show("اطلاعات ثبت شد");
                        if (CodeFunctionUpdated != null)
                        {
                            CodeFunctionUpdated(this, new CodeSelectedArg() { CodeFunctionID = id });
                        }
                    }
                    else
                    {

                        MessageBox.Show("تابع انتخاب شده با استاندارد کد اعتبارسنجی مطابقت ندارد");
                    }
                }
            }
        }

        private bool CheckMethodValidity()
        {
            return true;
            if (lokFunctionName.SelectedItem != null)
            {
                var method = (lokFunctionName.SelectedItem as ClassOrFunctionItem).Context as MethodInfo;
                if (method != null)
                {
                    if (CodeFunctionParamType == Enum_CodeFunctionParamType.OneDataItem)
                    {
                        if (method.ReturnType.FullName != typeof(CodeFunctionResult).FullName)
                            return false;
                        if (method.GetParameters().Count() == 1)
                        {
                            if (method.GetParameters().First().ParameterType == typeof(CodeFunctionParamOneDataItem))
                                return true;
                        }
                    }
                    else if (CodeFunctionParamType == Enum_CodeFunctionParamType.ManyDataItems)
                    {
                        if (method.ReturnType.FullName != typeof(CodeFunctionResult).FullName)
                            return false;
                        if (method.GetParameters().Count() == 1)
                        {
                            if (method.GetParameters().First().ParameterType == typeof(CodeFunctionParamManyDataItems))
                                return true;
                        }
                    }
                    else if (CodeFunctionParamType == Enum_CodeFunctionParamType.KeyColumns)
                    {
                        if (method.ReturnType.FullName != typeof(CodeFunctionResult).FullName)
                            return false;
                        foreach (var param in method.GetParameters())
                        {
                            if (!param.ParameterType.IsPrimitive)
                                if (param.ParameterType != typeof(string))
                                    return false;
                        }

                        return true;
                    }
                    ////else if (CodeFunctionParamType == Enum_CodeFunctionParamType.CommandFunction)
                    ////{
                    ////    if (method.ReturnType != typeof(CommandFunctionResult))
                    ////        return false;
                    ////    if (method.GetParameters().Count() == 1)
                    ////    {
                    ////        if (method.GetParameters().First().ParameterType == typeof(CommandFunctionParam))
                    ////            return true;
                    ////    }
                    ////}
                    else if (CodeFunctionParamType == Enum_CodeFunctionParamType.LetterFunction)
                    {
                        if (method.ReturnType.FullName != typeof(CodeFunctionResult).FullName)
                            return false;
                        if (method.GetParameters().Count() == 1)
                        {
                            if (method.GetParameters().First().ParameterType == typeof(LetterFunctionParam))
                                return true;
                        }
                    }
                    else if (CodeFunctionParamType == Enum_CodeFunctionParamType.LetterConvert)
                    {
                        if (method.ReturnType.FullName != typeof(LetterConvertToExternalResult).FullName)
                            return false;
                        if (method.GetParameters().Count() == 1)
                        {
                            if (method.GetParameters().First().ParameterType == typeof(LetterFunctionParam))
                                return true;
                        }
                    }
                }
            }
            return false;

        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSelectCode_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Filter =
          "Assembly files (*.dll)|*.dll";
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var file = fileDialog.FileName;
                    txtCodePath.Text = file;
                    txtCodePath.ToolTip = file;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    break;
            }
        }

        private void btnCodeList_Click(object sender, RoutedEventArgs e)
        {
            frmCodeFunctionSelect view = new frmCodeFunctionSelect(new List<Enum_CodeFunctionParamType>() { CodeFunctionParamType });
            view.CodeFunctionSelected += View_CodeFunctionSelected;
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "انتخاب تابع");

        }

        private void View_CodeFunctionSelected(object sender, CodeSelectedArg e)
        {
            if (e.CodeFunctionID != 0)
            {
                GetCodeFuntion(e.CodeFunctionID);
            }
        }



        private void txtCodePath_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtCodePath.Text != "")
            {
                Assembly assembly = null;
                try
                {
                    assembly = Assembly.ReflectionOnlyLoadFrom(txtCodePath.Text);

                }
                catch
                {

                }
                SetClassNames(assembly);
            }
            else
            {
                lokClassName.ItemsSource = null;
            }

        }

        private void SetClassNames(Assembly assembly)
        {
            List<ClassOrFunctionItem> list = new List<MyProject_WPF.ClassOrFunctionItem>();
            if (assembly != null)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    list.Add(new MyProject_WPF.ClassOrFunctionItem() { ItemName = type.FullName, Context = type });
                }
            }
            lokClassName.DisplayMember = "ItemName";
            lokClassName.SelectedValueMember = "ItemName";
            lokClassName.ItemsSource = list;
            if (Message.ID != 0)
            {
                lokClassName.SelectedValue = Message.ClassName;
            }
        }
        private void lokClassName_SelectionChanged(object sender, MyCommonWPFControls.SelectionChangedArg e)
        {
            if (lokClassName.SelectedItem != null)
            {
                Type type = (lokClassName.SelectedItem as ClassOrFunctionItem).Context as Type;
                SetFunctions(type);
            }
            else
            {
                lokFunctionName.ItemsSource = null;
            }


        }
        private void SetFunctions(Type type)
        {
            List<ClassOrFunctionItem> list = new List<MyProject_WPF.ClassOrFunctionItem>();
            foreach (var item in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                list.Add(new MyProject_WPF.ClassOrFunctionItem() { ItemName = item.Name, Context = item });
            }
            lokFunctionName.DisplayMember = "ItemName";
            lokFunctionName.SelectedValueMember = "ItemName";
            lokFunctionName.ItemsSource = list;
            if (Message.ID != 0)
            {
                lokFunctionName.SelectedValue = Message.FunctionName;
            }
        }
        private void lokFunctionName_SelectionChanged(object sender, MyCommonWPFControls.SelectionChangedArg e)
        {
            if (lokFunctionName.SelectedItem != null)
            {
                var method = (lokFunctionName.SelectedItem as ClassOrFunctionItem).Context as MethodInfo;
                if (method != null)
                {
                    try
                    {
                        lblReturnValue.Text = method.ReturnType.ToString();
                    }
                    catch
                    {

                    }
                    try
                    {
                        var listMethodParams = new List<CodeFunctionColumnDTO>();

                        foreach (var param in method.GetParameters())
                        {
                            var codeFunctionParam = new CodeFunctionColumnDTO();
                            codeFunctionParam.ParameterName = param.Name;
                            codeFunctionParam.DataType = param.ParameterType.FullName;
                            codeFunctionParam.DotNetType = param.ParameterType;

                            listMethodParams.Add(codeFunctionParam);
                        }
                        Message.Parameters = listMethodParams;
                        dtgCodeFunctionCurrentParams.ItemsSource = null;
                        dtgCodeFunctionCurrentParams.ItemsSource = Message.Parameters;
                    }
                    catch
                    {

                    }
                    //if (Message.ID == 0)
                    //{
                    //    Message.Parameters = listMethodParams;

                    //}
                    //else
                    //{
                    //    //مقایسه شود و با رنگ مشخص شوند
                    //}


                }
            }
            else
            {
                dtgCodeFunctionCurrentParams.ItemsSource = null;
            }
        }

    }
    public class ClassOrFunctionItem
    {
        //public Assembly Assembly { set; get; }
        public String ItemName { set; get; }
        public object Context { set; get; }
    }
}
