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

namespace MySecurityUI
{
    /// <summary>
    /// Interaction logic for frmEntityRoleSecurity.xaml
    /// </summary>
    public partial class frmEntityRoleSecurityIndirect : UserControl
    {
        int EntityID { set; get; }
        EntityRoleSecurityInDirectDTO Message;
        BizRoleSecurity bizRoleSecurity = new BizRoleSecurity();
        public frmEntityRoleSecurityIndirect(int entityID)
        {
            InitializeComponent();
            EntityID = entityID;
            Message = bizRoleSecurity.GetEntityRoleSecurityInDirect(EntityID, false);
            ShowMessage();
        }
        private void ShowMessage()
        {
            if (Message != null)
            {
                cmbDirectRoleSecurity.SelectedValue = Message.DirectRoleSecurityID;
                cmbRelationshipTail.SelectedValue = Message.RelationshipTailID;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //if (cmbDirectRoleSecurity.SelectedItem == null)
            //{
            //    MessageBox.Show("امنیت سازمان معادل انتخاب نشده است");
            //    return;
            //}
            if (cmbRelationshipTail.SelectedItem == null)
            {
                MessageBox.Show("زنجیره رابطه معادل انتخاب نشده است");
                return;
            }
            if (Message == null)
                Message = new EntityRoleSecurityInDirectDTO();
            Message.TableDrivedEntityID = EntityID;
            //Message.DirectRoleSecurityID = (int)cmbDirectRoleSecurity.SelectedValue;
            Message.RelationshipTailID = (int)cmbRelationshipTail.SelectedValue;
        
            bizRoleSecurity.UpdateEntityRoleSecurityInDirect(Message);
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
        private void cmbDirectRoleSecurity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
