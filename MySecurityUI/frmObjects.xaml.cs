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
    /// Interaction logic for frmObjects.xaml
    /// </summary>
    public partial class frmObjects : UserControl
    {
       
        public frmObjects()
        {
            InitializeComponent();
            ucObjectEdit.ObjectSaved += ucObjectEdit_ObjectSaved;
            ucObjectList.ObjectSelected += ucObjectList_ObjectSelected;
        }

        void ucObjectList_ObjectSelected(object sender, ObjectSelectedArg e)
        {
            ucObjectEdit.ShowObject(e.Object);
        }

        void ucObjectEdit_ObjectSaved(object sender, ObjectSavedArg e)
        {
            ucObjectList.ShowObjects(e.Object.ParentID);
        }

        private void btnExtractObjectFromDB_Click(object sender, RoutedEventArgs e)
        {
            BizObject bizObject = new BizObject();
            bizObject.ExtractObjectsFromDB();
            ucObjectList.ShowObjects(null);
        }
    }
}
