
using ModelEntites;

using MyModelManager;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
using Telerik.Windows.Controls;


namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmFormula.xaml
    /// </summary>
    public partial class frmEntitySecurityDirectSelect : UserControl
    {
        public event EventHandler<EntitySecurityDirectSelectArg> EntitySecurityDirectSelected;
    
        BizRoleSecurity bizRoleSecurity = new BizRoleSecurity();
        public frmEntitySecurityDirectSelect()
        {
            InitializeComponent();
            this.Loaded += FrmFormula_Loaded;
        }

        private void FrmFormula_Loaded(object sender, RoutedEventArgs e)
        {
            GetItems();

        }
     
        private void GetItems()
        {
            var listEntityValidations = bizRoleSecurity.GetEntitySecurityDirects(MyProjectManager.GetMyProjectManager.GetRequester(), "");
            dtgItems.ItemsSource = listEntityValidations;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var item = dtgItems.SelectedItem as EntitySecurityDirectDTO;
            if (item != null)
            {
                if (EntitySecurityDirectSelected != null)
                    EntitySecurityDirectSelected(this, new EntitySecurityDirectSelectArg() { ID = item.ID });
            }
        }
    }

    public class EntitySecurityDirectSelectArg : EventArgs
    {
        public int ID { set; get; }
    }

}
