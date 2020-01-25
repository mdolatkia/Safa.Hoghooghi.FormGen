
using ModelEntites;

using MyModelManager;
using MySecurity;
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


namespace MySecurityUI
{
    /// <summary>
    /// Interaction logic for frmFormula.xaml
    /// </summary>
    public partial class frmEntityRoleSecurityDirectSelect : Window
    {
        public event EventHandler<EntityRoleSecurityDirectSelectArg> EntityRoleSecurityDirectSelected;
        public int EntityID { set; get; }
        BizRoleSecurity bizRoleSecurity = new BizRoleSecurity();
        public frmEntityRoleSecurityDirectSelect(int entityID)
        {
            InitializeComponent();

            EntityID = entityID;
            this.Loaded += FrmFormula_Loaded;
        }

        private void FrmFormula_Loaded(object sender, RoutedEventArgs e)
        {
            GetItems();

        }
     
        private void GetItems()
        {
            var listEntityValidations = bizRoleSecurity.GetEntityRoleSecurityDirects(EntityID,false);
            dtgItems.ItemsSource = listEntityValidations;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var item = dtgItems.SelectedItem as EntityRoleSecurityDirectDTO;
            if (item != null)
            {
                if (EntityRoleSecurityDirectSelected != null)
                    EntityRoleSecurityDirectSelected(this, new EntityRoleSecurityDirectSelectArg() { ID = item.ID });
            }
            this.Close();
        }
    }

    public class EntityRoleSecurityDirectSelectArg : EventArgs
    {
        public int ID { set; get; }
    }

}
