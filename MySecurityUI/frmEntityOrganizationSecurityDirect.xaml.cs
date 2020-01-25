using ModelEntites;
using MyModelManager;
using MySecurity;
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

namespace MySecurityUI
{
    /// <summary>
    /// Interaction logic for frmEntityOrganizationSecurity.xaml
    /// </summary>
    public partial class frmEntityOrganizationSecurityDirect : UserControl
    {
        BizDatabaseFunction bizDatabaseFunction = new BizDatabaseFunction();
        EntityOrganizationSecurityDirectDTO Message;
        BizOrganizationSecurity bizOrganizationSecurity = new BizOrganizationSecurity();
        int EntityID { set; get; }
        public frmEntityOrganizationSecurityDirect(int entityID)
        {
            InitializeComponent();
            EntityID = entityID;
            Message = bizOrganizationSecurity.GetEntityOrganizationSecurityDirect(EntityID, false);
            SetOperators();
            SetDatabaseFunctions();
            SetColumns();
            ShowMessage();
        }
        private void SetOperators()
        {
            cmbOperator.ItemsSource = Enum.GetValues(typeof(EntitySecurityOperator));
        }
        private void SetDatabaseFunctions()
        {
            cmbDatabaseFunction.DisplayMemberPath = "FunctionName";
            cmbDatabaseFunction.SelectedValuePath = "ID";
            cmbDatabaseFunction.ItemsSource = bizDatabaseFunction.GetDatabaseFunctions(Enum_DatabaseFunctionType.None);
        }
        private void SetColumns()
        {
            BizColumn bizColumn = new BizColumn();
            cmbColumns.ItemsSource = bizColumn.GetColumns(EntityID, true);
            cmbColumns.DisplayMemberPath = "Name";
            cmbColumns.SelectedValuePath = "ID";
        }
        private void ShowMessage()
        {
            if (Message != null)
            {
                cmbColumns.SelectedValue = Message.ColumnID;
                cmbOperator.SelectedItem = Message.Operator;
                cmbDatabaseFunction.SelectedValue = Message.DBFunctionID;
                if (Message.DBFunctionID != 0)
                    optDBFunction.IsChecked = true;
                else
                    optOrganizationValue.IsChecked = true;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbColumns.SelectedItem == null)
            {
                MessageBox.Show("ستون معادل انتخاب نشده است");
                return;
            }
            if (Message == null)
                Message = new EntityOrganizationSecurityDirectDTO();
            Message.TableDrivedEntityID = EntityID;
            Message.ColumnID = (int)cmbColumns.SelectedValue;
            if (optDBFunction.IsChecked == true)
            {
                Message.Operator = (EntitySecurityOperator)cmbOperator.SelectedItem;
                Message.DBFunctionID = (int)cmbDatabaseFunction.SelectedValue;
            }
            else
            {
                Message.DBFunctionID = 0;

            }

            bizOrganizationSecurity.UpdateEntityOrganizationSecurityDirect(Message);
            MessageBox.Show("اطلاعات ثبت شد");
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
        }

        private void optOrganizationValue_Checked(object sender, RoutedEventArgs e)
        {
            cmbDatabaseFunction.IsEnabled = false;
            cmbOperator.IsEnabled = false;
        }

        private void optDBFunction_Checked(object sender, RoutedEventArgs e)
        {
            cmbDatabaseFunction.IsEnabled = true;
            cmbOperator.IsEnabled = true;
        }
    }
}
