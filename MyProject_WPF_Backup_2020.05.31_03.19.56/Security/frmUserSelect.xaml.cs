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
    /// Interaction logic for frmUserList.xaml
    /// </summary>
    public partial class frmUserSelect : UserControl
    {
        public event EventHandler<UserSelectedArg> UserSelected;
        bool LocalAdminMode { set; get; }
        public frmUserSelect(bool localAdminMode = false)
        {
            InitializeComponent();
            LocalAdminMode = localAdminMode;
        }
        public void SearchUsers()
        {
            BizUser bizUser = new BizUser();

            if (LocalAdminMode)
            {
                var orgIds = MyProjectManager.GetMyProjectManager.UserInfo.OrganizationPosts.Where(x => x.IsAdmin).Select(x => x.OrganizationID).Distinct().ToList();
                var userList = bizUser.GetAllLocalAdminUsers(txtUserName.Text,orgIds);
                dtgUsers.ItemsSource = userList;
            }
            else
            {
                var userList = bizUser.GetAllUsers(txtUserName.Text);
                dtgUsers.ItemsSource = userList;
            }

        
        }

        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtUserName.Text != "")
            {
                SearchUsers();
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (dtgUsers.SelectedItem != null)
            {
                var userDTO = dtgUsers.SelectedItem as UserDTO;
                if (userDTO != null)
                {
                    UserSelectedArg arg = new UserSelectedArg();
                    arg.UserID = userDTO.ID;
                    if (UserSelected != null)
                        UserSelected(this, arg);
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }
    public class UserSelectedArg : EventArgs
    {
        public int UserID { set; get; }
    }
}
