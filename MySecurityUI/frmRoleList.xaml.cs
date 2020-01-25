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
    /// Interaction logic for frmRoleList.xaml
    /// </summary>
    public partial class frmRoleList : UserControl
    {
        public event EventHandler<RoleSelectedArg> RoleSelected;
        public frmRoleList()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            ShowRoles();
        }

        void frmRoleList_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        public void ShowRoles()
        {
            BizRole bizRole = new BizRole();
            dtgRoles.ItemsSource = bizRole.GetAllRoles();
        }

        private void dtgRoles_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                var RoleDTO = e.AddedItems[0] as RoleDTO;
                if (RoleDTO != null)
                {
                    RoleSelectedArg arg = new RoleSelectedArg();
                    arg.Role = RoleDTO;
                    if (RoleSelected != null)
                        RoleSelected(this, arg);
                }
            }
        }
    }
    public class RoleSelectedArg : EventArgs
    {
        public RoleDTO Role { set; get; }
    }
}
