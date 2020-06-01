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
    /// Interaction logic for frmRoleTypeList.xaml
    /// </summary>
    public partial class frmRoleTypeSelect : UserControl
    {
        public event EventHandler<RoleTypeSelectedArg> RoleTypeSelected;

        public frmRoleTypeSelect()
        {
            InitializeComponent();
            BizRoleType bizRoleType = new BizRoleType();
            dtgItems.ItemsSource = bizRoleType.GetAllRoleTypes();
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (dtgItems.SelectedItem != null)
            {
                var RoleTypeDTO = dtgItems.SelectedItem as RoleTypeDTO;
                if (RoleTypeDTO != null)
                {
                    RoleTypeSelectedArg arg = new RoleTypeSelectedArg();
                    arg.RoleTypeID = RoleTypeDTO.ID;
                    if (RoleTypeSelected != null)
                        RoleTypeSelected(this, arg);
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }
    public class RoleTypeSelectedArg : EventArgs
    {
        public int RoleTypeID { set; get; }
    }
}
