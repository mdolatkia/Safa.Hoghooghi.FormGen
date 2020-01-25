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

namespace MySecurityUI
{
    /// <summary>
    /// Interaction logic for frmConditionalPermissionList.xaml
    /// </summary>
    public partial class frmConditionalPermissionList : Window
    {
        public event EventHandler<ConditionalPermissionSelectedArg> ConditionalPermissionSelected;
      
        public frmConditionalPermissionList()
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

        private void dtgItems_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                var ConditionalPermissionDTO = e.AddedItems[0] as ConditionalPermissionDTO;
                if (ConditionalPermissionDTO != null)
                {
                    ConditionalPermissionSelectedArg arg = new ConditionalPermissionSelectedArg();
                    arg.ConditionalPermission = ConditionalPermissionDTO;
                    if (ConditionalPermissionSelected != null)
                        ConditionalPermissionSelected(this, arg);
                }
            }
        }
    }
    public class ConditionalPermissionSelectedArg : EventArgs
    {
        public ConditionalPermissionDTO ConditionalPermission { set; get; }
    }
}
