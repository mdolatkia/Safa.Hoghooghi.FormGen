using ModelEntites;
using MyCommonWPFControls;

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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmLetterSetting.xaml
    /// </summary>
    /// 
    public partial class frmLetterSetting : UserControl
    {
        BizCodeFunction bizCodeFunction = new BizCodeFunction();
        LetterSettingDTO Message { set; get; }
        BizLetterTemplate bizLetterTemplate = new BizLetterTemplate();
        public frmLetterSetting()
        {
            InitializeComponent();
            Message = bizLetterTemplate.GetLetterSetting(false);
           
            SetCodeFunctions();
            SetConvertCodeFunctions();
            lokAfterSave.EditItemClicked += LokAfterSave_EditItemClicked;
            lokBeforeSave.EditItemClicked += LokAfterSave_EditItemClicked;
            lokBeforeLoad.EditItemClicked += LokAfterSave_EditItemClicked;
            lokExternalSource.EditItemClicked += LokAfterSave_EditItemClicked;
            lokConvert.EditItemClicked += LokConvert_EditItemClicked;
            if (Message == null)
                Message = new LetterSettingDTO();
            else
                ShowMassage();

        }

        private void LokConvert_EditItemClicked(object sender, EditItemClickEventArg e)
        {
            var lookup = (sender as MyStaticLookup);
            frmCodeFunction view;
            if (lookup.SelectedItem == null)
            {
                view = new frmCodeFunction(0, Enum_CodeFunctionParamType.LetterConvert,0);
            }
            else
            {
                view = new frmCodeFunction((int)lookup.SelectedValue, Enum_CodeFunctionParamType.LetterConvert,0);
            }
            view.CodeFunctionUpdated += (sender1, e1) => View_ConvertCodeFunctionUpdated(sender1, e1, lookup);
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "تنظیمات نامه");
        }
        private void View_ConvertCodeFunctionUpdated(object sender, CodeSelectedArg e, MyStaticLookup lookup)
        {
            SetConvertCodeFunctions();
            lookup.SelectedValue = e.CodeFunctionID;
        }
        private void ShowMassage()
        {
            lokAfterSave.SelectedValue = Message.AfterLetterSaveCodeID;
            lokBeforeLoad.SelectedValue = Message.BeforeLetterLoadCodeID;
            lokBeforeSave.SelectedValue = Message.BeforeLetterSaveCodeID;
            lokExternalSource.SelectedValue = Message.LetterExternalInfoCodeID;
            lokConvert.SelectedValue = Message.LetterSendToExternalCodeID;

        }

        private void LokAfterSave_EditItemClicked(object sender, MyCommonWPFControls.EditItemClickEventArg e)
        {
            var lookup = (sender as MyStaticLookup);
            frmCodeFunction view;
            if (lookup.SelectedItem == null)
            {
                view = new frmCodeFunction(0, Enum_CodeFunctionParamType.LetterFunction,0);
            }
            else
            {
                view = new frmCodeFunction((int)lookup.SelectedValue, Enum_CodeFunctionParamType.LetterFunction,0);
            }
            view.CodeFunctionUpdated += (sender1, e1) => View_CodeFunctionUpdated(sender1, e1, lookup);
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "تنظیمات نامه");
        }

        private void View_CodeFunctionUpdated(object sender, CodeSelectedArg e, MyStaticLookup lookup)
        {
            SetCodeFunctions();
            lookup.SelectedValue = e.CodeFunctionID;
        }

        private void SetCodeFunctions()
        {
            var codeFunctions = bizCodeFunction.GetCodeFunctions(new List<Enum_CodeFunctionParamType>() { Enum_CodeFunctionParamType.LetterFunction });
            lokAfterSave.DisplayMember = "Name";
            lokAfterSave.SelectedValueMember = "ID";
            lokAfterSave.ItemsSource = codeFunctions;
            lokBeforeLoad.DisplayMember = "Name";
            lokBeforeLoad.SelectedValueMember = "ID";
            lokBeforeLoad.ItemsSource = codeFunctions;
            lokBeforeSave.DisplayMember = "Name";
            lokBeforeSave.SelectedValueMember = "ID";
            lokBeforeSave.ItemsSource = codeFunctions;
            lokExternalSource.DisplayMember = "Name";
            lokExternalSource.SelectedValueMember = "ID";
            lokExternalSource.ItemsSource = codeFunctions;
          
        }
        private void SetConvertCodeFunctions()
        {
            var codeFunctions = bizCodeFunction.GetCodeFunctions(new List<Enum_CodeFunctionParamType>() { Enum_CodeFunctionParamType.LetterConvert });
            lokConvert.DisplayMember = "Name";
            lokConvert.SelectedValueMember = "ID";
            lokConvert.ItemsSource = codeFunctions;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (lokAfterSave.SelectedItem != null)
                Message.AfterLetterSaveCodeID = (int)lokAfterSave.SelectedValue;
            else
                Message.AfterLetterSaveCodeID = 0;
            if (lokBeforeLoad.SelectedItem != null)
                Message.BeforeLetterLoadCodeID = (int)lokBeforeLoad.SelectedValue;
            else
                Message.BeforeLetterLoadCodeID = 0;
            if (lokBeforeSave.SelectedItem != null)
                Message.BeforeLetterSaveCodeID = (int)lokBeforeSave.SelectedValue;
            else
                Message.BeforeLetterSaveCodeID = 0;
            if (lokExternalSource.SelectedItem != null)
                Message.LetterExternalInfoCodeID = (int)lokExternalSource.SelectedValue;
            else
                Message.LetterExternalInfoCodeID = 0;
            if (lokConvert.SelectedItem != null)
                Message.LetterSendToExternalCodeID = (int)lokConvert.SelectedValue;
            else
                Message.LetterSendToExternalCodeID = 0;
            bizLetterTemplate.UpdateLetterSetting(Message);
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
