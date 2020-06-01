using MyModelManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using ProxyLibrary;
namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmOrganizationTypeList.xaml
    /// </summary>
    public partial class frmOrganizationTypeSelect : UserControl
    {
        public event EventHandler<OrganizationTypeSelectedArg> OrganizationTypeSelected;

        public frmOrganizationTypeSelect()
        {
            InitializeComponent();
            BizOrganizationType bizOrganizationType = new BizOrganizationType();
            dtgOrganizationTypes.ItemsSource = bizOrganizationType.GetAllOrganizationTypes();
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (dtgOrganizationTypes.SelectedItem != null)
            {
                var OrganizationTypeDTO = dtgOrganizationTypes.SelectedItem as OrganizationTypeDTO;
                if (OrganizationTypeDTO != null)
                {
                    OrganizationTypeSelectedArg arg = new OrganizationTypeSelectedArg();
                    arg.OrganizationTypeID = OrganizationTypeDTO.ID;
                    if (OrganizationTypeSelected != null)
                        OrganizationTypeSelected(this, arg);
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }
    public class OrganizationTypeSelectedArg : EventArgs
    {
        public int OrganizationTypeID { set; get; }
    }
}
