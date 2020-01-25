using ModelEntites;
using MyModelManager;
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
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace MySecurityUI
{
    /// <summary>
    /// Interaction logic for frmEntityRoleSecurity.xaml
    /// </summary>
    public partial class frmRoleSecurity : UserControl
    {
        List<EntityRoleSecurityDTO> Message;
        BizRoleSecurity bizRoleSecurity = new BizRoleSecurity();
        int EntityID { set; get; }
        public frmRoleSecurity(int entityID)
        {//فرم حذف شود؟
            InitializeComponent();
            EntityID = entityID;
            Message = bizRoleSecurity.GetEntityRoleSecurities(EntityID, false);
            SetRoles();
            ShowMessage();
        }

        private void SetRoles()
        {
            BizRole bizRole = new BizRole();
            var rel = dtgRoles.Columns[0] as GridViewComboBoxColumn;
            rel.ItemsSource = bizRole.GetAllRoles();
            rel.DisplayMemberPath = "Name";
            rel.SelectedValueMemberPath = "ID";

        }

        private void ShowMessage()
        {
            if (Message != null)
            {
                dtgRoles.ItemsSource = Message;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Message.Count == 0)
            {
                MessageBox.Show("نقشی انتخاب نشده است");
                return;
            }
            bizRoleSecurity.UpdateRoleSecurity(EntityID, Message);
            MessageBox.Show("اطلاعات ثبت شد");
        }

        //private void btnReturn_Click(object sender, RoutedEventArgs e)
        //{
        //    this.Close();
        //}
    }
}
