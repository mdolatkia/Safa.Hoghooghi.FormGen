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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MySecurityUI
{
    /// <summary>
    /// Interaction logic for frmUserEdit.xaml
    /// </summary>
    /// 

    public partial class frmUserEdit : UserControl
    {
        public event EventHandler UserSaved;
        public UserDTO User { set; get; }
        public frmUserEdit()
        {
            InitializeComponent();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
          ShowUser( new UserDTO());
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            BizUser bizUser = new BizUser();
            if (User == null) User = new UserDTO();
            User.UserName = txtUserName.Text;
            User.Password = txtPassword.Text;
            bizUser.SaveUser(User);
            if (UserSaved != null)
                UserSaved(this, null);
        }

        internal void ShowUser(UserDTO userDTO)
        {
            User = userDTO;
            txtUserName.Text = userDTO.UserName;
            txtPassword.Text = userDTO.Password;
        }
    }
}
