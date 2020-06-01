using ModelEntites;

using MyModelManager;

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
    /// Interaction logic for frmEntityRoleSecurity.xaml
    /// </summary>
    public partial class frmEntitySecurityIndirect : UserControl
    {
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        EntitySecurityInDirectDTO Message;
        BizRoleSecurity bizRoleSecurity = new BizRoleSecurity();
        public frmEntitySecurityIndirect()
        {
            InitializeComponent();
            SetEntites();

        }

        private void SetEntites()
        {
            lokEntities.DisplayMember = "Name";
            lokEntities.SelectedValueMember = "ID";
            lokEntities.SearchFilterChanged += LokEntities_SearchFilterChanged;
            lokEntities.SelectionChanged += LokEntities_SelectionChanged;
        }

        private void LokEntities_SelectionChanged(object sender, MyCommonWPFControls.SelectionChangedArg e)
        {
            if (lokEntities.SelectedItem == null)
            {
                cmbRelationshipTail.ItemsSource = null;
            }
            else
            {
                SetRelationshipTails();
                Message = bizRoleSecurity.GetEntitySecurityInDirect(MyProjectManager.GetMyProjectManager.GetRequester(), (int)lokEntities.SelectedValue, false);
                ShowMessage();

            }



        }

        private void LokEntities_SearchFilterChanged(object sender, MyCommonWPFControls.SearchFilterArg e)
        {
            if (e.SingleFilterValue != null)
            {
                if (!e.FilterBySelectedValue)
                {
                    var list = bizTableDrivedEntity.GetAllEntities(MyProjectManager.GetMyProjectManager.GetRequester(), e.SingleFilterValue, null);
                    e.ResultItemsSource = list;
                }
                else
                {
                    var id = Convert.ToInt32(e.SingleFilterValue);
                    if (id > 0)
                    {
                        //  lokEntities.ItemsSource = bizTableDrivedEntity.GetAllEntities();
                        var entity = bizTableDrivedEntity.GetSimpleEntity(MyProjectManager.GetMyProjectManager.GetRequester(), id); ;
                        e.ResultItemsSource = new List<TableDrivedEntityDTO> { entity };
                    }
                    else
                        e.ResultItemsSource = null;
                }
            }
        }

        private void ShowMessage()
        {
            if (Message != null)
            {
                //cmbDirectRoleSecurity.SelectedValue = Message.DirectRoleSecurityID;
                cmbRelationshipTail.SelectedValue = Message.RelationshipTailID;
            }
            else
                cmbRelationshipTail.SelectedItem = null;
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
                Message = new EntitySecurityInDirectDTO();
            Message.TableDrivedEntityID = (int)lokEntities.SelectedValue;
            //Message.DirectRoleSecurityID = (int)cmbDirectRoleSecurity.SelectedValue;
            Message.RelationshipTailID = (int)cmbRelationshipTail.SelectedValue;

            bizRoleSecurity.UpdateEntitySecurityInDirect(Message);
            MessageBox.Show("اطلاعات ثبت شد");
        }

        //private void btnReturn_Click(object sender, RoutedEventArgs e)
        //{
        //    MyProjectManager.GetMyProjectManager.CloseDialog(this);
        //}



        private void btnRelationshipTail_Click(object sender, RoutedEventArgs e)
        {
            if (lokEntities.SelectedItem != null)
            {
                frmEntityRelationshipTail view = new frmEntityRelationshipTail((int)lokEntities.SelectedValue);
                view.ItemSelected += View_ItemSelected;
                MyProjectManager.GetMyProjectManager.ShowDialog(view, "رابطه های مرتبط");
            }
        }

        private void View_ItemSelected(object sender, EntityRelationshipTailSelectedArg e)
        {
            SetRelationshipTails();
            cmbRelationshipTail.SelectedValue = e.EntityRelationshipTailID;

            //کنترل و پیغام با موجودیت مستقیم انتخاب شده
        }
        private void SetRelationshipTails()
        {
            if (lokEntities.SelectedItem != null)
            {
                BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
                cmbRelationshipTail.DisplayMemberPath = "EntityPath";
                cmbRelationshipTail.SelectedValuePath = "ID";
                cmbRelationshipTail.ItemsSource = bizEntityRelationshipTail.GetEntityRelationshipTails(MyProjectManager.GetMyProjectManager.GetRequester(), (int)lokEntities.SelectedValue);
            }
            else
                cmbRelationshipTail.ItemsSource = null;

        }

    }
}
