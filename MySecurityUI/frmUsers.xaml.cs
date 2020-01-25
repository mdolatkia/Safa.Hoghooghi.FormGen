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

namespace MySecurityUI
{
    /// <summary>
    /// Interaction logic for frmUsers.xaml
    /// </summary>
    public partial class frmUsers : UserControl
    {
        public event EventHandler<UserEditArg> UserSaved;
        BizUser bizUser = new BizUser();
        UserDTO User;
        public frmUsers(int userID = 0)
        {
            InitializeComponent();
            if (userID == 0)
            {
                User = new UserDTO();
            }
            else
            {
                User = bizUser.GetUser(userID);
                ShowUser();
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            ShowUser();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (User == null) User = new UserDTO();
            User.UserName = txtUserName.Text;
            User.Password = txtPassword.Text;
            var id = bizUser.SaveUser(User);
            if (UserSaved != null)
                UserSaved(this, new MySecurityUI.UserEditArg() { UserID = id });
        }

        internal void ShowUser()
        {
            txtUserName.Text = userDTO.UserName;
            txtPassword.Text = userDTO.Password;
        }
        frmUserSelect frmUserSelect;
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if(frmUserSelect==null)
            {
                frmUserSelect = new MySecurityUI.frmUserSelect();
                frmUserSelect.UserSelected += FrmUserSelect_UserSelected;
            }
           
        }

        private void FrmUserSelect_UserSelected(object sender, UserSelectedArg e)
        {
            throw new NotImplementedException();
        }
    }
    public class UserEditArg : EventArgs
    {
        public int UserID { set; get; }
    }
}
