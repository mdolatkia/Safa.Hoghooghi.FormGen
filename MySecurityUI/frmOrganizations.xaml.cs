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
    /// Interaction logic for frmOrganizations.xaml
    /// </summary>
    public partial class frmOrganizations : UserControl
    {
        public frmOrganizations()
        {
            InitializeComponent();
            ucOrganizationEdit.OrganizationSaved += ucOrganizationEdit_OrganizationSaved;
            ucOrganizationList.OrganizationSelected += ucOrganizationList_OrganizationSelected;
        }

        void ucOrganizationList_OrganizationSelected(object sender, OrganizationSelectedArg e)
        {
            ucOrganizationEdit.ShowOrganization(e.Organization);
        }

        void ucOrganizationEdit_OrganizationSaved(object sender, EventArgs e)
        {
            ucOrganizationList.ShowOrganizations();
        }
    }
}
