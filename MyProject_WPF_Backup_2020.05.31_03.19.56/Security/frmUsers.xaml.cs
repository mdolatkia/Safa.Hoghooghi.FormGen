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
    /// Interaction logic for frmUsers.xaml
    /// </summary>
    public partial class frmUsers : UserControl
    {
        public event EventHandler<UserEditArg> UserSaved;
        BizUser bizUser = new BizUser();
        UserDTO User;
        bool LocalAdminMode { set; get; }
        public frmUsers(bool localAdminMode = false)
        {
            InitializeComponent();
            LocalAdminMode = localAdminMode;
            //if (userID == 0)
            User = new UserDTO();
            //else
            //    GetUser(userID);

        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            User = new UserDTO();
            ShowUser();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (User == null) User = new UserDTO();
            User.UserName = txtUserName.Text;
            User.Password = txtPassword.Password;
            User.FirstName = txtFirstName.Text;
            User.LastName = txtLastName.Text;
            User.Email = txtEmail.Text;
            var id = bizUser.SaveUser(User);
            User.ID = id;
            MessageBox.Show("کاربر ثبت شد");
            if (UserSaved != null)
                UserSaved(this, new MyProject_WPF.UserEditArg() { UserID = id });
        }

        internal void ShowUser()
        {
            txtUserName.Text = User.UserName;
            txtPassword.Password = User.Password;
            txtFirstName.Text = User.FirstName;
            txtLastName.Text = User.LastName;
            txtEmail.Text = User.Email;
        }
        frmUserSelect frmUserSelect;
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (frmUserSelect == null)
            {
                frmUserSelect = new MyProject_WPF.frmUserSelect(LocalAdminMode);
                frmUserSelect.UserSelected += FrmUserSelect_UserSelected;
            }
            MyProjectManager.GetMyProjectManager.ShowDialog(frmUserSelect, "انتخاب کاربر");
        }

        private void FrmUserSelect_UserSelected(object sender, UserSelectedArg e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(sender);
            GetUser(e.UserID);
        }

        private void GetUser(int userID)
        {
            User = bizUser.GetUser(userID);
            ShowUser();
        }
    }
    public class UserEditArg : EventArgs
    {
        public int UserID { set; get; }
    }
}
