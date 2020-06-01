using MyModelManager;
using ProxyLibrary;
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
    /// Interaction logic for frmRoleType.xaml
    /// </summary>
    public partial class frmRoleType : UserControl
    {
        public event EventHandler<RoleTypeEditArg> RoleTypeaved;
        BizRoleType bizRoleType = new BizRoleType();
        //RoleTypeDTO RoleType;
        public frmRoleType(int roleTypeID)
        {
            InitializeComponent();
            GetRoleTypes();
            ControlHelper.GenerateContextMenu(dtgItems);

            //if (roleTypeID == 0)
            //{
            //    RoleType = new RoleTypeDTO();
            //}
            //else
            //{
            //    GetRoleType(roleTypeID);
            //}
        }

        private void GetRoleTypes()
        {
            var roleTypes = bizRoleType.GetAllRoleTypes();
            dtgItems.ItemsSource = roleTypes;
        }

        //private void btnNew_Click(object sender, RoutedEventArgs e)
        //{
        //    RoleType = new RoleTypeDTO();
        //    ShowRoleType();
        //}

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //if (RoleType == null) RoleType = new RoleTypeDTO();
            //RoleType.Name = txtName.Text;
            List<RoleTypeDTO> list = dtgItems.ItemsSource as List<RoleTypeDTO>;
            if (list != null)
            {
                bizRoleType.SaveRoleTypes(list);
                MessageBox.Show("اطلاعات ثبت شد");
                if (RoleTypeaved != null)
                    RoleTypeaved(this, new MyProject_WPF.RoleTypeEditArg());
            }
        }

        //internal void ShowRoleType()
        //{
        //    txtName.Text = RoleType.Name;

        //}

        //private void btnSearch_Click(object sender, RoutedEventArgs e)
        //{

        //    frmRoleTypeSelect frmRoleTypSelect = new MyProject_WPF.frmRoleTypeSelect();
        //    frmRoleTypSelect.RoleTypeSelected += FrmRoleTypeelect_RoleTypeelected;
        //    MyProjectManager.GetMyProjectManager.ShowDialog(frmRoleTypSelect, "انتخاب نوع نقش");
        //}

        //private void FrmRoleTypeelect_RoleTypeelected(object sender, RoleTypeSelectedArg e)
        //{
        //    MyProjectManager.GetMyProjectManager.CloseDialog(sender);
        //    GetRoleType(e.RoleTypeID);
        //}

        //private void GetRoleType(int RoleTypeID)
        //{
        //    RoleType = bizRoleType.GetRoleType(RoleTypeID);
        //    ShowRoleType();
        //}
    }
    public class RoleTypeEditArg : EventArgs
    {
        //public int RoleTypeID { set; get; }
    }
}
