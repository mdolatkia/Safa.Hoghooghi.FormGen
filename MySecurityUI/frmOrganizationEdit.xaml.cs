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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MySecurityUI
{
    /// <summary>
    /// Interaction logic for frmOrganizationEdit.xaml
    /// </summary>
    /// 

    public partial class frmOrganizationEdit : UserControl
    {
        public event EventHandler OrganizationSaved;
        public OrganizationDTO Organization { set; get; }
        public frmOrganizationEdit()
        {
            InitializeComponent();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
          ShowOrganization( new OrganizationDTO());
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            BizOrganization bizOrganization = new BizOrganization();
            if (Organization == null) Organization = new OrganizationDTO();
            Organization.Name = txtOrganizationName.Text;
         
            bizOrganization.SaveOrganization(Organization);
            if (OrganizationSaved != null)
                OrganizationSaved(this, null);
        }

        internal void ShowOrganization(OrganizationDTO OrganizationDTO)
        {
            Organization = OrganizationDTO;
            txtOrganizationName.Text = OrganizationDTO.Name;
         
        }
    }
}
