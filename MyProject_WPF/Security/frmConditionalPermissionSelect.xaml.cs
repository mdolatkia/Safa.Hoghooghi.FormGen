using ModelEntites;
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

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmConditionalPermissionList.xaml
    /// </summary>
    public partial class frmConditionalPermissionSelect: UserControl
    {
        public event EventHandler<ConditionalPermissionSelectedArg> ConditionalPermissionSelected;

        public frmConditionalPermissionSelect()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                ShowConditionalPermissions();

        }


        public void ShowConditionalPermissions()
        {
            BizPermission bizConditionalPermission = new BizPermission();
            dtgItems.ItemsSource = bizConditionalPermission.GetConditionalPermissions();
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (dtgItems.SelectedItem != null)
            {
                var ConditionalPermissionDTO = dtgItems.SelectedItem as ConditionalPermissionDTO;
                if (ConditionalPermissionDTO != null)
                {
                    ConditionalPermissionSelectedArg arg = new ConditionalPermissionSelectedArg();
                    arg.ConditionalPermissionID = ConditionalPermissionDTO.ID;
                    if (ConditionalPermissionSelected != null)
                        ConditionalPermissionSelected(this, arg);
                }
            }
        }
    }
    public class ConditionalPermissionSelectedArg : EventArgs
    {
        public int ConditionalPermissionID { set; get; }
    }
}
