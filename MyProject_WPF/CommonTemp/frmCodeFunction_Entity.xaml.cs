using ModelEntites;
using MyFormulaFunctionStateFunctionLibrary;
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
using Telerik.Windows.Controls;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmCodeFunction.xaml
    /// </summary>
    public partial class frmCodeFunction_Entity : UserControl
    {
        public event EventHandler<CodeFunctionEntitySelectedArg> CodeFunctionEntitySelected;

        BizCodeFunction bizCodeFunction = new BizCodeFunction();
        BizTableDrivedEntity bizEntity = new BizTableDrivedEntity();
        //  CodeFunctionEntityIntention CodeFunctionEntityIntention;
        CodeFunction_EntityDTO CodeFunctionEntity { set; get; }
        TableDrivedEntityDTO Entity { set; get; }
        List<Enum_CodeFunctionParamType> CodeFunctionParamTypes { set; get; }
        public frmCodeFunction_Entity(int EntityID, int CodeFunctionEntityID, List<Enum_CodeFunctionParamType> codeFunctionParamTypes)
        {
            InitializeComponent();
            CodeFunctionParamTypes = codeFunctionParamTypes;
            Entity = bizEntity.GetTableDrivedEntity(EntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
            if (CodeFunctionEntityID == 0)
            {
                CodeFunctionEntity = new CodeFunction_EntityDTO();
                GetCodeFunctions();
                cmbParamTypes.ItemsSource = CodeFunctionParamTypes;
                lblCodeFunction.Visibility = Visibility.Collapsed;
            }
            else
            {
                CodeFunctionEntity = bizCodeFunction.GetCodeFunctionEntity(CodeFunctionEntityID);
                cmbParamTypes.ItemsSource = Enum.GetValues(typeof(Enum_CodeFunctionParamType));
                cmbParamTypes.SelectedItem = CodeFunctionEntity.CodeFunction.ParamType;
                cmbParamTypes.IsEnabled = false;
                lblCodeFunction.Text = Entity.Alias + " - " + CodeFunctionEntity.CodeFunction.Name;
                lokCodeFunction.Visibility = Visibility.Collapsed;
                SetCodeFunctionParametersToGrid(CodeFunctionEntity.CodeFunctionID, CodeFunctionEntity.CodeFunctionEntityColumns);
            }
            SetDataGridColumns();
        }
        private void SetCodeFunctionParametersToGrid(int iD, List<CodeFunction_Entity_ColumnDTO> existing = null)
        {
            var functionColumns = bizCodeFunction.GetCodeFunctionParameters(iD);
            CodeFunctionEntity.CodeFunctionEntityColumns = new List<CodeFunction_Entity_ColumnDTO>();
            foreach (var item in functionColumns)
            {
                var row = new CodeFunction_Entity_ColumnDTO();
                row.CodeFunctionParameterID = item.ID;
                row.FunctionColumnParamName = item.ParameterName;
                row.FunctionDataType = item.DataType;
                row.FunctionColumnDotNetType = item.DotNetType;
                if (existing != null)
                {
                    var existingRow = existing.FirstOrDefault(x => x.FunctionColumnParamName == item.ParameterName);
                    if (existingRow != null)
                    {
                        row.ColumnID = existingRow.ColumnID;
                    }
                }
                CodeFunctionEntity.CodeFunctionEntityColumns.Add(row);
            }

            dtgCodeFunctionParams.ItemsSource = null;
            dtgCodeFunctionParams.ItemsSource = CodeFunctionEntity.CodeFunctionEntityColumns;
        }

        private void SetDataGridColumns()
        {
            colEntityColumn.ItemsSource = Entity.Columns;

            colEntityColumn.DisplayMemberPath = "Name";
            colEntityColumn.SelectedValueMemberPath = "ID";
        }

        private void GetCodeFunctions()
        {
            if (lokCodeFunction.ItemsSource == null)
            {
                lokCodeFunction.NewItemEnabled = true;
                lokCodeFunction.EditItemEnabled = true;
                lokCodeFunction.EditItemClicked += LokCodeFunction_EditItemClicked;
            }
            var list = bizCodeFunction.GetCodeFunctions(CodeFunctionParamTypes);
            lokCodeFunction.DisplayMember = "Name";
            lokCodeFunction.SelectedValueMember = "ID";
            lokCodeFunction.ItemsSource = list;
        }
        private void lokCodeFunction_SelectionChanged(object sender, MyCommonWPFControls.SelectionChangedArg e)
        {
            var selectedItem = lokCodeFunction.SelectedItem as CodeFunctionDTO;
            if (selectedItem != null)
            {
                CodeFunctionEntity.CodeFunctionID = selectedItem.ID;
                SetCodeFunctionParametersToGrid(CodeFunctionEntity.CodeFunctionID);
            }
        }


        private void LokCodeFunction_EditItemClicked(object sender, MyCommonWPFControls.EditItemClickEventArg e)
        {
            if (cmbParamTypes.SelectedItem != null)
            {
                var selectedItem = lokCodeFunction.SelectedItem as CodeFunctionDTO;
                frmCodeFunction view = null;
                if (selectedItem == null)
                    view = new frmCodeFunction(0, (Enum_CodeFunctionParamType)cmbParamTypes.SelectedItem);
                else
                    view = new frmCodeFunction(selectedItem.ID, selectedItem.ParamType);

                view.CodeFunctionUpdated += View_CodeSelected;
                MyProjectManager.GetMyProjectManager().ShowDialog(view, "CodeFunction");
            }
        }
        private void View_CodeSelected(object sender, CodeSelectedArg e)
        {
            GetCodeFunctions();
            lokCodeFunction.SelectedValue = e.CodeFunctionID;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (CodeFunctionEntity == null)
            {
                MessageBox.Show("فانکشن نامشخص است");
                return;
            }
            if(cmbParamTypes.SelectedItem==null)
            {
                MessageBox.Show("نوع نامشخص است");
                return;
            }
            //if (txtTitle.Text == "")
            //{
            //    MessageBox.Show("عنوان فانکشن نامشخص است");
            //    return;
            //}

            if ((Enum_CodeFunctionParamType)cmbParamTypes.SelectedItem==Enum_CodeFunctionParamType.KeyColumns)
            {
                if (CodeFunctionEntity.CodeFunctionEntityColumns.Any(x => x.ColumnID == 0))
                {
                    MessageBox.Show("ستون معادل برای یکی از پارامترهای فانکشن مشخص نشده است");
                    return;
                }
            }



            CodeFunctionEntity.EntityID = Entity.ID;
            CodeFunctionEntity.ID = bizCodeFunction.UpdateCodeFunctionEntity(CodeFunctionEntity);
            if (CodeFunctionEntitySelected != null)
                CodeFunctionEntitySelected(this, new CodeFunctionEntitySelectedArg() { CodeFunctionEntityID = CodeFunctionEntity.ID });
            MyProjectManager.GetMyProjectManager().CloseDialog(this);
        }

        private void cmbParamTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((Enum_CodeFunctionParamType)cmbParamTypes.SelectedItem != Enum_CodeFunctionParamType.KeyColumns)
            {
                dtgCodeFunctionParams.Visibility = Visibility.Collapsed;
            }
            else
                dtgCodeFunctionParams.Visibility = Visibility.Visible;
            //GetCodeFunctions();

        }
    }

    public class CodeFunctionEntitySelectedArg : EventArgs
    {
        public int CodeFunctionEntityID { set; get; }
    }


    //public class CodeFunctionEntityIntention
    //{
    //    public int CodeFunctionEntityID { set; get; }
    //    public int EntityID { set; get; }
    //    public Enum_CodeFunctionEntityIntention Type { set; get; }
    //}

    public enum Enum_CodeFunctionEntityIntention
    {
        CodeFunctionEntityDefinition,
        CodeFunctionEntityEdit
    }
}
