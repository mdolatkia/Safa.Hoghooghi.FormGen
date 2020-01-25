using MyModelManager;
using ProxyLibrary;
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

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmOrganizationList.xaml
    /// </summary>
    public partial class frmOrganizationSelect : UserControl
    {
        public event EventHandler<OrganizationSelectedArg> OrganizationSelected;
        bool LocalAdminMode { set; get; }
        public frmOrganizationSelect(bool localAdminMode)
        {
            InitializeComponent();
            LocalAdminMode = localAdminMode;
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                ShowOrganizations();
        }



        public void ShowOrganizations()
        {
            BizOrganization bizOrganization = new BizOrganization();
            var organizationList = bizOrganization.GetAllOrganizations();

            if (LocalAdminMode)
            {
                var organizationIds = MyProjectManager.GetMyProjectManager.UserInfo.OrganizationPosts.Where(x => x.IsAdmin).Select(x => x.OrganizationID).Distinct().ToList();
                dtgOrganizations.ItemsSource = organizationList.Where(y => organizationIds.Contains(y.ID)).ToList();
            }
            else
                dtgOrganizations.ItemsSource = organizationList;
        }


        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (dtgOrganizations.SelectedItem != null)
            {
                var OrganizationDTO = dtgOrganizations.SelectedItem as OrganizationDTO;
                if (OrganizationDTO != null)
                {
                    OrganizationSelectedArg arg = new OrganizationSelectedArg();
                    arg.OrganizationID = OrganizationDTO.ID;
                    if (OrganizationSelected != null)
                        OrganizationSelected(this, arg);
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }
    public class OrganizationSelectedArg : EventArgs
    {
        public int OrganizationID { set; get; }
    }
}
