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
    /// Interaction logic for frmObjectEdit.xaml
    /// </summary>
    /// 

    public partial class frmObjectEdit : UserControl
    {
        public event EventHandler<ObjectSavedArg> ObjectSaved;
        public ObjectDTO Object { set; get; }
        public frmObjectEdit()
        {
            InitializeComponent();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            if (Object == null)
                ShowObject(new ObjectDTO());
            else
                ShowObject(new ObjectDTO() { ParentID = Object.ID });
        }
        private void btnNewSibling_Click(object sender, RoutedEventArgs e)
        {
            if (Object == null)
                ShowObject(new ObjectDTO());
            else
                ShowObject(new ObjectDTO() { ParentID = Object.ParentID });
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            BizObject bizObject = new BizObject();
            if (Object == null) Object = new ObjectDTO();
            Object.ObjectName = txtObjectName.Text;
            Object.Category = txtCategory.Text;
            Object.NeedsExplicitPermission = chkNeedsExplicitPermission.IsChecked == true;
            bizObject.SaveObject(Object);
            if (ObjectSaved != null)
            {
                ObjectSavedArg arg = new ObjectSavedArg();
                arg.Object = Object;
                ObjectSaved(this, arg);
            }
        }

        internal void ShowObject(ObjectDTO ObjectDTO)
        {
            Object = ObjectDTO;
            txtObjectName.Text = ObjectDTO.ObjectName;
            txtCategory.Text = ObjectDTO.Category;
            chkNeedsExplicitPermission.IsChecked = Object.NeedsExplicitPermission;
        }


    }

    public class ObjectSavedArg : EventArgs
    {
        public ObjectDTO Object { set; get; }
    }
}
