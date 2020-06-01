using ModelEntites;
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

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmRoleList.xaml
    /// </summary>
    public partial class frmRoleOrRoleGroupList : UserControl
    {
        public event EventHandler<RoleOrRoleGroupSelectedArg> RoleOrRoleGroupSelected;
        public frmRoleOrRoleGroupList()
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
            dtgRoles.ItemsSource = bizRole.GetRoleOrRoleGroups();
        }

        private void dtgRoles_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                var RoleDTO = e.AddedItems[0] as RoleOrRoleGroupDTO;
                if (RoleDTO != null)
                {
                    RoleOrRoleGroupSelectedArg arg = new RoleOrRoleGroupSelectedArg();
                    arg.RoleOrRoleGroup = RoleDTO;
                    if (RoleOrRoleGroupSelected != null)
                        RoleOrRoleGroupSelected(this, arg);
                }
            }
        }
    }
    public class RoleOrRoleGroupSelectedArg : EventArgs
    {
        public RoleOrRoleGroupDTO RoleOrRoleGroup { set; get; }
    }
}
