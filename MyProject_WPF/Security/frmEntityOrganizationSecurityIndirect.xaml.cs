using ModelEntites;
using MyCommonWPFFroms;
using MyModelManager;
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

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmEntityOrganizationSecurity.xaml
    /// </summary>
    public partial class frmEntityOrganizationSecurityIndirect : UserControl
    {
        int EntityID { set; get; }
        EntityOrganizationSecurityInDirectDTO Message;
        BizOrganizationSecurity bizOrganizationSecurity = new BizOrganizationSecurity();
        public frmEntityOrganizationSecurityIndirect(int entityID)
        {
            InitializeComponent();
            EntityID = entityID;
            Message = bizOrganizationSecurity.GetEntityOrganizationSecurityInDirect(EntityID, false);
            SetRelationshipTails();
            SetEntityOrganizationSecurityDirects();
            ShowMessage();
        }

        private void SetEntityOrganizationSecurityDirects()
        {
           

            cmbDirectOrganizationSecurity.DisplayMemberPath = "EntityName";
            cmbDirectOrganizationSecurity.SelectedValuePath = "ID";
            cmbDirectOrganizationSecurity.ItemsSource = bizOrganizationSecurity.GetEntityOrganizationSecurityDirects();

        }

        private void ShowMessage()
        {
            if (Message != null)
            {
                cmbDirectOrganizationSecurity.SelectedValue = Message.DirectOrganizationSecurityID;
                cmbRelationshipTail.SelectedValue = Message.RelationshipTailID;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbDirectOrganizationSecurity.SelectedItem == null)
            {
                MessageBox.Show("امنیت سازمان معادل انتخاب نشده است");
                return;
            }
            if (cmbRelationshipTail.SelectedItem == null)
            {
                MessageBox.Show("زنجیره رابطه معادل انتخاب نشده است");
                return;
            }
            if (Message == null)
                Message = new EntityOrganizationSecurityInDirectDTO();
            Message.TableDrivedEntityID = EntityID;
            Message.DirectOrganizationSecurityID = (int)cmbDirectOrganizationSecurity.SelectedValue;
            Message.RelationshipTailID = (int)cmbRelationshipTail.SelectedValue;
            bizOrganizationSecurity.UpdateEntityOrganizationSecurityInDirect(Message);
            MessageBox.Show("اطلاعات ثبت شد");
        }

        //private void btnReturn_Click(object sender, RoutedEventArgs e)
        //{
        //    this.Close();
        //}



        private void btnRelationshipTail_Click(object sender, RoutedEventArgs e)
        {
            frmEntityRelationshipTail view = new frmEntityRelationshipTail(EntityID);
            view.ItemSelected += View_ItemSelected;
            view.ShowDialog();
        }

        private void View_ItemSelected(object sender, EntityRelationshipTailSelectedArg e)
        {
            SetRelationshipTails();
            cmbRelationshipTail.SelectedValue = e.EntityRelationshipTailID;

            //کنترل و پیغام با موجودیت مستقیم انتخاب شده
        }
        private void SetRelationshipTails()
        {

            BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();

            cmbRelationshipTail.DisplayMemberPath = "RaltionshipPath";
            cmbRelationshipTail.SelectedValuePath = "ID";
            cmbRelationshipTail.ItemsSource = bizEntityRelationshipTail.GetEntityRelationshipTails(EntityID);



        }
        private void cmbDirectOrganizationSecurity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
