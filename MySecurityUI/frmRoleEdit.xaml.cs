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
    /// Interaction logic for frmRoleEdit.xaml
    /// </summary>
    /// 

    public partial class frmRoleEdit : UserControl
    {
        public event EventHandler RoleSaved;
        public RoleDTO Role { set; get; }
        public frmRoleEdit()
        {
            InitializeComponent();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            ShowRole(new RoleDTO());
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            BizRole bizRole = new BizRole();
            if (Role == null) Role = new RoleDTO();
            Role.Name = txtRoleName.Text;
            Role.IsAdmin = chkIsAdmin.IsChecked == true;
            Role.IsSuperAdmin = chkIsSuperAdmin.IsChecked == true;

            bizRole.SaveRole(Role);
            if (RoleSaved != null)
                RoleSaved(this, null);
        }

        internal void ShowRole(RoleDTO RoleDTO)
        {
            Role = RoleDTO;
            txtRoleName.Text = RoleDTO.Name;
            chkIsAdmin.IsChecked = Role.IsAdmin;
            chkIsSuperAdmin.IsChecked = Role.IsSuperAdmin;
        }
    }
}
