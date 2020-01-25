using MySecurity;
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

namespace MySecurityUI
{
    /// <summary>
    /// Interaction logic for frmOrganizationList.xaml
    /// </summary>
    public partial class frmOrganizationList : UserControl
    {
        public event EventHandler<OrganizationSelectedArg> OrganizationSelected;
        public frmOrganizationList()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            ShowOrganizations();
        }

        void frmOrganizationList_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        public void ShowOrganizations()
        {
            BizOrganization bizOrganization = new BizOrganization();
            dtgOrganizations.ItemsSource = bizOrganization.GetAllOrganizations();
        }

        private void dtgOrganizations_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                var OrganizationDTO = e.AddedItems[0] as OrganizationDTO;
                if (OrganizationDTO != null)
                {
                    OrganizationSelectedArg arg = new OrganizationSelectedArg();
                    arg.Organization = OrganizationDTO;
                    if (OrganizationSelected != null)
                        OrganizationSelected(this, arg);
                }
            }
        }
    }
    public class OrganizationSelectedArg : EventArgs
    {
        public OrganizationDTO Organization { set; get; }
    }
}
