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
    /// Interaction logic for frmUserList.xaml
    /// </summary>
    public partial class frmUserSelect : UserControl
    {
        public event EventHandler<UserSelectedArg> UserSelected;

        public frmUserSelect()
        {
            InitializeComponent();
        }
        public void SearchUsers()
        {
            BizUser bizUser = new BizUser();
            dtgUsers.ItemsSource = bizUser.GetAllUsers(txtUserName.Text);
        }
        private void dtgUsers_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                var userDTO = e.AddedItems[0] as UserDTO;
                if (userDTO != null)
                {
                    UserSelectedArg arg = new UserSelectedArg();
                    arg.UserID = userDTO.ID;
                    if (UserSelected != null)
                        UserSelected(this, arg);
                }
            }
        }
        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtUserName.Text != "")
            {
                SearchUsers();
            }
        }
    }
    public class UserSelectedArg : EventArgs
    {
        public int UserID { set; get; }
    }
}
