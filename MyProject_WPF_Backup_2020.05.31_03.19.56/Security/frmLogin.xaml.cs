using MyUILibrary;
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
    /// Interaction logic for frmLogin.xaml
    /// </summary>
    public partial class frmLogin :UserControl
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        public event EventHandler<LoginRequestedArg> LoginRequested;

        public void ShowForm()
        {
            MyProjectManager.GetMyProjectManager.ShowDialog(this, "Login");
        }

        public void ShowMessage(string message)
        {
            lblMessage.Text = message;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (txtUserName.Text != "" && txtPassword.Text != "")
            {
                if (LoginRequested != null)
                {
                    var arg = new LoginRequestedArg();
                    arg.UserName = txtUserName.Text;
                    arg.Password = txtPassword.Text;
                    LoginRequested(this, arg);
                }
            }
        }
    }
    public class LoginRequestedArg : EventArgs
    {
        public string UserName { set; get; }
        public string Password { set; get; }
    }
}
