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
    /// Interaction logic for frmSecuritySubjectList.xaml
    /// </summary>
    public partial class frmSecuritySubjectSelect : UserControl
    {
        public event EventHandler<SecuritySubjectSelectedArg> SecuritySubjectSelected;

        public frmSecuritySubjectSelect()
        {
            InitializeComponent();
            SearchSecuritySubjects();
        }
        public void SearchSecuritySubjects()
        {
            BizSecuritySubject bizSecuritySubject = new BizSecuritySubject();
            dtgItems.ItemsSource = bizSecuritySubject.GetSecuritySubjects(txtName.Text);
        }
        private void dtgSecuritySubjects_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                var SecuritySubjectDTO = e.AddedItems[0] as SecuritySubjectDTO;
                if (SecuritySubjectDTO != null)
                {
                    SecuritySubjectSelectedArg arg = new SecuritySubjectSelectedArg();
                    arg.SecuritySubjectID = SecuritySubjectDTO.ID;
                    if (SecuritySubjectSelected != null)
                        SecuritySubjectSelected(this, arg);
                }
            }
        }
       
        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtName.Text != "")
            {
                SearchSecuritySubjects();
            }
        }
    }
    public class SecuritySubjectSelectedArg : EventArgs
    {
        public int SecuritySubjectID { set; get; }
    }
}
