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

    public partial class frmOrganization : UserControl
    {
        BizOrganizationType bizOrganizationType = new BizOrganizationType();
        BizUser bizUser = new BizUser();
        BizOrganization bizOrganization = new BizOrganization();
        //public event EventHandler OrganizationSaved;
        public OrganizationDTO Organization { set; get; }
        bool LocalAdminMode { set; get; }
        public frmOrganization(bool localAdminMode=false)
        {
            InitializeComponent();
            LocalAdminMode = localAdminMode;
            SetUsers();
            SetOrganizationTypes();

            //if (organizationID == 0)
            //{
            Organization = new OrganizationDTO();
            ShowOrganization();
            //}
            //else
            //    GetOrganization(organizationID);

            ControlHelper.GenerateContextMenu(dtgPosts);
        }



        private void GetOrganization(int organizationID)
        {
            Organization = bizOrganization.GetOrganization(organizationID, true);
            ShowOrganization();
        }

        private void SetOrganizationTypes()
        {
            if (lokOrganizationType.DisplayMember == null)
            {
                lokOrganizationType.DisplayMember = "Name";
                lokOrganizationType.SelectedValueMember = "ID";
                lokOrganizationType.SelectionChanged += LokOrganizationType_SelectionChanged;
            }
            lokOrganizationType.ItemsSource = bizOrganizationType.GetAllOrganizationTypes();
        }
        private void MyStaticLookup_EditItemClicked(object sender, MyCommonWPFControls.EditItemClickEventArg e)
        {
            frmOrganizationType frm = null;
            if (lokOrganizationType.SelectedItem == null)
                frm = new MyProject_WPF.frmOrganizationType(0);
            else
            {
                var orgType = lokOrganizationType.SelectedItem as OrganizationTypeDTO;
                frm = new MyProject_WPF.frmOrganizationType(orgType.ID);
            }
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "نوع سازمان");
            frm.OrganizationTypeSaved += Frm_OrganizationTypeSaved;
        }

        private void Frm_OrganizationTypeSaved(object sender, OrganizationTypeArg e)
        {
            SetOrganizationTypes();
            lokOrganizationType.SelectedValue = e.OrganizationTypeID;
        }

        private void LokOrganizationType_SelectionChanged(object sender, MyCommonWPFControls.SelectionChangedArg e)
        {
            var OrganizationType = e.SelectedItem as OrganizationTypeDTO;
            if (OrganizationType != null)
            {
                var fullOrganizationType = bizOrganizationType.GetOrganizationType(OrganizationType.ID, true);
                if (string.IsNullOrEmpty(colOrganizationRoleTypes.DisplayMemberPath))
                {
                    colOrganizationRoleTypes.DisplayMemberPath = "Name";
                    colOrganizationRoleTypes.SelectedValueMemberPath = "ID";
                }
                colOrganizationRoleTypes.ItemsSource = fullOrganizationType.OrganizationTypeRoleTypes;
            }
        }

        private void SetUsers()
        {
            colUser.DisplayMemberPath = "UserName";
            colUser.SelectedValueMemberPath = "ID";
            colUser.SearchFilterChanged += ColUser_SearchFilterChanged;
        }

        private void ColUser_SearchFilterChanged(object sender, MyCommonWPFControls.SearchFilterArg e)
        {
            if (!string.IsNullOrEmpty(e.SingleFilterValue))
            {
                if (e.FilterBySelectedValue)
                {
                    var id = Convert.ToInt32(e.SingleFilterValue);
                    if (id > 0)
                    {
                        var user = bizUser.GetUser(id);
                        e.ResultItemsSource = new List<UserDTO> { user };
                    }
                    else
                        e.ResultItemsSource = null;
                }
                else
                {
                    e.ResultItemsSource = bizUser.GetAllUsers(e.SingleFilterValue);
                }
            }
            //else if (e.Filters.Count > 0)
            //{

            //}
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Organization = new OrganizationDTO();
            ShowOrganization();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            BizOrganization bizOrganization = new BizOrganization();
            if (Organization == null) Organization = new OrganizationDTO();
            Organization.Name = txtOrganizationName.Text;
            Organization.OrganizationTypeID = (int)lokOrganizationType.SelectedValue;
            bizOrganization.SaveOrganization(Organization);
            MessageBox.Show("اطلاعات ثبت شد");
            //if (OrganizationSaved != null)
            //    OrganizationSaved(this, null);
        }

        internal void ShowOrganization()
        {
            txtOrganizationName.Text = Organization.Name;
            lokOrganizationType.SelectedValue = Organization.OrganizationTypeID;
            dtgPosts.ItemsSource = Organization.OrganizationPosts;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

         
            frmOrganizationSelect frmOrganizationSelect = new frmOrganizationSelect(LocalAdminMode);
            frmOrganizationSelect.OrganizationSelected += FrmOrganizationSelect_OrganizationSelected;
            MyProjectManager.GetMyProjectManager.ShowDialog(frmOrganizationSelect, "انتخاب سازمان");
        }

        private void FrmOrganizationSelect_OrganizationSelected(object sender, OrganizationSelectedArg e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(sender);
            GetOrganization(e.OrganizationID);
        }
    }
}
