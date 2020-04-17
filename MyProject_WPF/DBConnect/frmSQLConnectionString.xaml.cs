using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmSQLConnectionString.xaml
    /// </summary>
    public partial class frmSQLConnectionString : Window
    {
        SqlConnectionStringBuilder builder = null;
        public event EventHandler<MyConnectionString> ConnectionStringConfirmed;
        public frmSQLConnectionString(string serverName, string dbName, string connectionString)
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(serverName))
            {
                throw new Exception("نام سرور نامشخص می باشد");
            }
            txtServerName.TextChanged += TxtServerName_TextChanged;
            txtPassword.TextChanged += TxtPassword_TextChanged;
            cmbAuthenticationMode.SelectionChanged += CmbAuthenticationMode_SelectionChanged;
            txtUserName.TextChanged += TxtUserName_TextChanged;
            cmbDatabase.SelectionChanged += CmbDatabase_SelectionChanged;
            //txtConnectionString.TextChanged += TxtConnectionString_TextChanged;
            cmbAuthenticationMode.ItemsSource = Enum.GetValues(typeof(enum_AuthenticationMode)).Cast<object>();
            if (string.IsNullOrEmpty(connectionString))
            {
                builder = new SqlConnectionStringBuilder();
                cmbAuthenticationMode.SelectedItem = enum_AuthenticationMode.Windows;
                if (!string.IsNullOrEmpty(serverName))
                    txtServerName.Text = serverName;
                GetListDatabases(dbName);

             
                SetConnectionString();

            }
            else
            {
                builder = new SqlConnectionStringBuilder(connectionString);
                txtServerName.Text = builder.DataSource;
                if (builder.IntegratedSecurity)
                    cmbAuthenticationMode.SelectedItem = enum_AuthenticationMode.Windows;
                else
                {
                    cmbAuthenticationMode.SelectedItem = enum_AuthenticationMode.UserPass;
                    txtUserName.Text = builder.UserID;
                    txtPassword.Text = builder.Password;
                }
                GetListDatabases(builder.InitialCatalog);
                SetConnectionString();
            }
        }

        private void GetListDatabases(string defaultDB)
        {
            if (txtServerName.Text != "")
            {
                if (cmbAuthenticationMode.SelectedItem != null)
                {
                    string cnString = "";
                    var mode = (enum_AuthenticationMode)cmbAuthenticationMode.SelectedItem;
                    if (mode == enum_AuthenticationMode.Windows)
                    {
                        cnString = "Data Source=" + txtServerName.Text + "; Integrated Security=True;Connection Timeout=12;";
                    }
                    else if (txtUserName.Text != "" && txtPassword.Text != "")
                    {
                        cnString = "Data Source=" + txtServerName.Text + ";User Id =" + txtUserName.Text + "; Password = " + txtPassword.Text + ";Connection Timeout=12;";
                    }
                    if (cnString != "")
                    {
                        List<string> databaseNames = new List<string>();

                        try
                        {
                            using (SqlConnection cn = new SqlConnection(cnString))
                            {
                                // Open the connection
                                cn.Open();

                                using (SqlCommand cmd = new SqlCommand())
                                {
                                    cmd.CommandTimeout = 6;
                                    cmd.Connection = cn;
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.CommandText = "sp_databases";
                                    using (SqlDataReader myReader = cmd.ExecuteReader())
                                    {
                                        while ((myReader.Read()))
                                        {
                                            databaseNames.Add(myReader.GetString(0));
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {

                        }
                        cmbDatabase.ItemsSource = databaseNames;

                        if (!string.IsNullOrEmpty(defaultDB))
                        {
                            if (databaseNames.Any(x => x == defaultDB))
                                cmbDatabase.SelectedItem = defaultDB;
                        }
                    }
                }
            }
        }

        //private void TxtConnectionString_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    CheckTestButtonEnablity();
        //}

        private void CheckTestButtonEnablity()
        {
            btnTest.IsEnabled = txtConnectionString.Text != "";
        }

        private void SetConnectionString()
        {
            string cnString = "";
            if (txtServerName.Text != "" && cmbDatabase.SelectedItem != null)
            {
                var mode = (enum_AuthenticationMode)cmbAuthenticationMode.SelectedItem;
                if (mode == enum_AuthenticationMode.Windows
                    || (txtUserName.Text != "" && txtPassword.Text != ""))
                {
                    builder.DataSource = txtServerName.Text;
                    builder.InitialCatalog = (string)cmbDatabase.SelectedItem;
                    builder.IntegratedSecurity = mode == enum_AuthenticationMode.Windows;
                    if (mode == enum_AuthenticationMode.UserPass)
                    {
                        builder.UserID = txtUserName.Text;
                        builder.Password = txtPassword.Text;
                    }
                    else
                    {
                        builder.Remove("User Id");
                        builder.Remove("Password");
                    }
                    cnString = builder.ConnectionString;
                }
            }
            txtConnectionString.Text = cnString;
            CheckTestButtonEnablity();
        }
        private void CmbDatabase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetConnectionString();
        }

        private void TxtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetConnectionString();
        }

        private void CmbAuthenticationMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckUerPassEnablity();
            SetConnectionString();
        }

        private void TxtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetConnectionString();
        }

        private void TxtServerName_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetConnectionString();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtConnectionString.Text))
            {
                if (ConnectionStringConfirmed != null)
                    ConnectionStringConfirmed(this, new MyProject_WPF.MyConnectionString()
                    {
                        ConnectionString = txtConnectionString.Text,
                        DatabaseName = builder.InitialCatalog
                    });
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }





        private void CheckUerPassEnablity()
        {
            var mode = (enum_AuthenticationMode)cmbAuthenticationMode.SelectedItem;
            if (mode == enum_AuthenticationMode.Windows)
            {
                txtUserName.Text = "";
                txtPassword.Text = "";
                txtUserName.IsEnabled = false;
                txtPassword.IsEnabled = false;
            }
            else if (mode == enum_AuthenticationMode.UserPass)
            {
                txtUserName.IsEnabled = true;
                txtPassword.IsEnabled = true;
            }
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            if (txtConnectionString.Text != "")
            {
                string cnString = txtConnectionString.Text;
                if (!cnString.ToLower().Contains("timeout"))
                    cnString += ";Connection Timeout=4";
                using (SqlConnection connection = new SqlConnection(cnString))
                {
                    try
                    {
                        connection.Open();
                        MessageBox.Show("رشته ارتباطی صحیح می باشد", "تایید", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("خطا در ارتباط با پایگاه داده" + Environment.NewLine + ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void cmbDatabase_DropDownOpened(object sender, EventArgs e)
        {
            GetListDatabases("");
        }
    }
    public enum enum_AuthenticationMode
    {
        Windows,
        UserPass
    }
    public class MyConnectionString
    {
        public string ConnectionString { set; get; }
        public string DatabaseName { set; get; }
    }
}
