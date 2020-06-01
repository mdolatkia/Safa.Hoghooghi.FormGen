using MyCommonWPFControls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmOrganizationEdit.xaml
    /// </summary>
    /// 

    public partial class frmOrganizationType : UserControl
    {
        BizOrganizationType bizOrganizationType = new BizOrganizationType();
        public event EventHandler<OrganizationTypeArg> OrganizationTypeSaved;
        public OrganizationTypeDTO OrganizationType { set; get; }
        public frmOrganizationType(int organizationTypeID)
        {
            InitializeComponent();
            SetRoleTypes();
            if (organizationTypeID == 0)
            {
                OrganizationType = new OrganizationTypeDTO();
                ShowOrganizationType();
            }
            else
                GetOrganizationType(organizationTypeID);

            ControlHelper.GenerateContextMenu(dtgRoleTypes);
        }



        BizRoleType bizRoleType = new BizRoleType();
        private void SetRoleTypes()
        {
            if (colRoleTypes.DisplayMemberPath == "")
            {
                colRoleTypes.DisplayMemberPath = "Name";
                colRoleTypes.SelectedValueMemberPath = "ID";
                colRoleTypes.NewItemEnabled = true;
                colRoleTypes.EditItemEnabled = true;
                colRoleTypes.EditItemClicked += ColRoleTypes_EditItemClicked;
            }
            colRoleTypes.ItemsSource = bizRoleType.GetAllRoleTypes();
        }

        private void ColRoleTypes_EditItemClicked(object sender, MyCommonWPFControls.EditItemClickEventArg e)
        {
            frmRoleType frm = null;
            if ((sender as MyStaticLookup).SelectedItem == null)
                frm = new MyProject_WPF.frmRoleType(0);
            else
            {
                var roleTypeDTO = (sender as MyStaticLookup).SelectedItem as RoleTypeDTO;
                frm = new MyProject_WPF.frmRoleType(roleTypeDTO.ID);
            }
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "نوع نقش");
            frm.RoleTypeaved += (sender1, e1) => Frm_RoleTypeaved(sender1, e1, (sender as MyStaticLookup));
        }

        private void Frm_RoleTypeaved(object sender, RoleTypeEditArg e, MyStaticLookup lookup)
        {
            SetRoleTypes();
            //lookup.SelectedValue = e.RoleTypeID;
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            OrganizationType = new OrganizationTypeDTO();
            ShowOrganizationType();
        }

        internal void ShowOrganizationType()
        {
            txtName.Text = OrganizationType.Name;
            dtgRoleTypes.ItemsSource = OrganizationType.OrganizationTypeRoleTypes;
        }
        private void GetOrganizationType(int organizationTypeID)
        {
            OrganizationType = bizOrganizationType.GetOrganizationType(organizationTypeID, true);
            ShowOrganizationType();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var frmOrganizationTypeSelect = new MyProject_WPF.frmOrganizationTypeSelect();
            frmOrganizationTypeSelect.OrganizationTypeSelected += FrmOrganizationTypeSelect_OrganizationTypeSelected;
            MyProjectManager.GetMyProjectManager.ShowDialog(frmOrganizationTypeSelect, "انتخاب نوع سازمان");
        }

        private void FrmOrganizationTypeSelect_OrganizationTypeSelected(object sender, OrganizationTypeSelectedArg e)
        {
            GetOrganizationType(e.OrganizationTypeID);
            MyProjectManager.GetMyProjectManager.CloseDialog(sender);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            OrganizationType.Name = txtName.Text;
            var id = bizOrganizationType.SaveOrganizationType(OrganizationType);
            MessageBox.Show("اطلاعات ثبت شد");
            if (OrganizationTypeSaved != null)
                OrganizationTypeSaved(this, new OrganizationTypeArg() { OrganizationTypeID = id });
        }
    }
    public class OrganizationTypeArg : EventArgs
    {
        public int OrganizationTypeID { set; get; }
    }
}
