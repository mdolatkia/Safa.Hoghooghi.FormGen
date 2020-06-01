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

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmRoles.xaml
    /// </summary>
    public partial class frmRoles : UserControl
    {
        public frmRoles()
        {
            InitializeComponent();
            ucRoleEdit.RoleSaved += ucRoleEdit_RoleSaved;
            ucRoleList.RoleSelected += ucRoleList_RoleSelected;
        }

        void ucRoleList_RoleSelected(object sender, RoleSelectedArg e)
        {
            ucRoleEdit.ShowRole(e.Role);
        }

        void ucRoleEdit_RoleSaved(object sender, EventArgs e)
        {
            ucRoleList.ShowRoles();
        }
    }
}
