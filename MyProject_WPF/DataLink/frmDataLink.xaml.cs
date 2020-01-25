using Microsoft.Win32;
using ModelEntites;
using MyCommonWPFControls;

using MyFormulaFunctionStateFunctionLibrary;
using MyInterfaces;
using MyModelManager;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for frmEntityCommands.xaml
    /// </summary>
    public partial class frmDataLink : UserControl
    {
        DataLinkDTO Message { set; get; }
        BizDataLink bizDataLink = new BizDataLink();
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
        int FirstEntityID { set; get; }
        public event EventHandler<UpdatedEventArg> Updated;
        public frmDataLink(int entityID, int dataLinkID)
        {
            InitializeComponent();
            FirstEntityID = entityID;
            //   var entity = bizTableDrivedEntity.GetTableDrivedEntity(EntityID, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships);
            SetEntities();

            if (dataLinkID == 0)
            {
                NewItem();
            }
            else
            {
                GetDataLink(dataLinkID);
            }
            //SetRelationshipTails();


            colRelationshipTail.NewItemEnabled = true;
            colRelationshipTail.EditItemEnabled = true;
            colRelationshipTail.EditItemClicked += ColRelationshipTail_EditItemClicked;

            ControlHelper.GenerateContextMenu(dtgRelationships);
        }
        private void ColRelationshipTail_EditItemClicked(object sender, EditItemClickEventArg e)
        {
            if (lokFirstSideEntity.SelectedItem != null)
            {
                frmEntityRelationshipTail view = new frmEntityRelationshipTail((int)lokFirstSideEntity.SelectedValue);
                MyProjectManager.GetMyProjectManager.ShowDialog(view, "رابطه");
                view.ItemSelected += (sender1, e1) => View_ItemSelected(sender1, e1, (sender as MyStaticLookup));
            }
        }
        private void View_ItemSelected(object sender, EntityRelationshipTailSelectedArg e, MyStaticLookup lookup)
        {
            SetRelationshipTails();
            lookup.SelectedValue = e.EntityRelationshipTailID;
        }
        private void SetEntities()
        {
            lokFirstSideEntity.DisplayMember = "Name";
            lokFirstSideEntity.SelectedValueMember = "ID";
            lokFirstSideEntity.SearchFilterChanged += LokEntitiesFirst_SearchFilterChanged;

            lokSecondSideEntity.DisplayMember = "Name";
            lokSecondSideEntity.SelectedValueMember = "ID";
            lokSecondSideEntity.SearchFilterChanged += LokEntitiesSecond_SearchFilterChanged;
        }
        private void LokEntitiesFirst_SearchFilterChanged(object sender, MyCommonWPFControls.SearchFilterArg e)
        {
            if (e.SingleFilterValue != null)
            {
                if (!e.FilterBySelectedValue)
                {
                    var list = bizTableDrivedEntity.GetAllEntities(MyProjectManager.GetMyProjectManager.GetRequester(), e.SingleFilterValue, false);
                    e.ResultItemsSource = list;
                }
                else
                {
                    var id = Convert.ToInt32(e.SingleFilterValue);
                    if (id > 0)
                    {
                        //lokSecondSideEntity.ItemsSource = bizTableDrivedEntity.GetAllEntities();
                        var entity = bizTableDrivedEntity.GetSimpleEntity(MyProjectManager.GetMyProjectManager.GetRequester(), id);
                        e.ResultItemsSource = new List<TableDrivedEntityDTO> { entity };
                    }
                    else
                        e.ResultItemsSource = null;
                }
            }
        }

        private void LokEntitiesSecond_SearchFilterChanged(object sender, MyCommonWPFControls.SearchFilterArg e)
        {
            if (e.SingleFilterValue != null)
            {
                if (!e.FilterBySelectedValue)
                {
                    var list = bizTableDrivedEntity.GetAllEntities(MyProjectManager.GetMyProjectManager.GetRequester(), e.SingleFilterValue, false);
                    e.ResultItemsSource = list;
                }
                else
                {
                    var id = Convert.ToInt32(e.SingleFilterValue);
                    if (id > 0)
                    {
                        //lokSecondSideEntity.ItemsSource = bizTableDrivedEntity.GetAllEntities();
                        var entity = bizTableDrivedEntity.GetSimpleEntity(MyProjectManager.GetMyProjectManager.GetRequester(),id); 
                        e.ResultItemsSource = new List<TableDrivedEntityDTO> { entity };
                    }
                    else
                        e.ResultItemsSource = null;
                }
            }
        }
        private void GetDataLink(int ID)
        {
            Message = bizDataLink.GetDataLink(MyProjectManager.GetMyProjectManager.GetRequester(), ID);
            ShowMessage();
        }
        private void SetRelationshipTails()
        {

            //چک شود فقط یکی ازین دو پر شوند
            if (lokFirstSideEntity.SelectedItem != null)
            {
                var relationshipTails = bizEntityRelationshipTail.GetEntityRelationshipTails(MyProjectManager.GetMyProjectManager.GetRequester(), (int)lokFirstSideEntity.SelectedValue);
                colRelationshipTail.DisplayMemberPath = "EntityPath";
                colRelationshipTail.SelectedValueMemberPath = "ID";
                colRelationshipTail.ItemsSource = relationshipTails;
            }
        }
        private void ShowMessage()
        {

            //  var entity = bizTableDrivedEntity.GetTableDrivedEntity(EntityID, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships);
            lokFirstSideEntity.SelectedValue = Message.FirstSideEntityID;
            lokSecondSideEntity.SelectedValue = Message.SecondSideEntityID;
            if (Message.ID == 0)
            {
                lokFirstSideEntity.IsEnabled = false;
            }
            else
            {
                lokFirstSideEntity.IsEnabled = false;
                lokSecondSideEntity.IsEnabled = false;
            }
            //}
            //else
            //{
            //    var entity = bizTableDrivedEntity.GetTableDrivedEntity(Message.FirstSideEntityID, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships);
            //    txtFirstSideEntity.Text = entity.Alias;
            //    lokSecondSideEntity.IsEnabled = false;
            //}
            SetRelationshipTails();
            dtgRelationships.ItemsSource = Message.RelationshipsTails;
            txtName.Text = Message.Name;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {


            if (txtName.Text == "")
            {
                MessageBox.Show("نام");
                return;
            }
            if (lokFirstSideEntity.SelectedItem == null)
            {
                MessageBox.Show("موجودیت سمت اول انتخاب نشده است");
                return;
            }
            if (lokSecondSideEntity.SelectedItem == null)
            {
                MessageBox.Show("موجودیت سمت دوم انتخاب نشده است");
                return;
            }



            foreach (var item in Message.RelationshipsTails)
            {
                if (colRelationshipTail.ItemsSource != null && colRelationshipTail.ItemsSource is List<EntityRelationshipTailDTO>)
                {
                    var tail = (colRelationshipTail.ItemsSource as List<EntityRelationshipTailDTO>).FirstOrDefault(x => x.ID == item.RelationshipTailID);
                    if (tail != null)
                    {
                        TableDrivedEntityDTO sEntity = lokSecondSideEntity.SelectedItem as TableDrivedEntityDTO;
                        if (tail.TargetEntityID != sEntity.ID)
                        {

                            MessageBox.Show("رشته رابطه" + " " + tail.EntityPath + " " + "با موجودیت" + " " + sEntity.Alias + " " + "در ارتباط نمی باشد");
                            return;
                        }
                    }
                }
            }
            foreach (var item in Message.RelationshipsTails)
            {
                if (item.RelationshipTailID != 0)
                {
                    var relationshipTail = bizEntityRelationshipTail.GetEntityRelationshipTail(MyProjectManager.GetMyProjectManager.GetRequester(), item.RelationshipTailID);
                    BizTableDrivedEntity bizTableDrivedEntity = new MyModelManager.BizTableDrivedEntity();
                    var entity = bizTableDrivedEntity.GetSimpleEntity(MyProjectManager.GetMyProjectManager.GetRequester(), relationshipTail.TargetEntityID); 
                    var viewMessage = bizEntityRelationshipTail.CheckTailHasRelationshipWithView(relationshipTail);
                    if (viewMessage != "")
                    {
                        var message = "اشکال در تعریف لینک داده با موجودیت" + " " + string.IsNullOrEmpty(relationshipTail.TargetEntityAlias);
                        message += Environment.NewLine + "در رشته رابطه ارتباط با نمای" + " " + viewMessage + " " + "امکان پذیر نمی باشد";
                        MessageBox.Show(message);
                        return;
                    }
                }
            }

            var firstEntity = lokFirstSideEntity.SelectedItem as TableDrivedEntityDTO;
            var secondEntity = lokSecondSideEntity.SelectedItem as TableDrivedEntityDTO;
            if (firstEntity.ServerID != secondEntity.ServerID)
            {
                BizDatabase bizDatabase = new BizDatabase();
                if (!bizDatabase.LinkedServerExists(firstEntity.ServerID, secondEntity.ServerID))
                {
                    MessageBox.Show("ارتباط لینک سرور بین موجودیت طرف اول و دوم وجود ندارد");
                    return;
                }
            }
            foreach (var item in Message.RelationshipsTails)
            {

                if (item.RelationshipTailID != 0)
                {
                    var relationshipTail = bizEntityRelationshipTail.GetEntityRelationshipTail(MyProjectManager.GetMyProjectManager.GetRequester(), item.RelationshipTailID);
                    var linkedServerMessage = bizEntityRelationshipTail.CheckRelationshipsLinkedServers(relationshipTail);
                    if (linkedServerMessage != "")
                    {
                        var message = "اشکال در رشته رابطه" + " " + string.IsNullOrEmpty(relationshipTail.EntityPath);
                        message += Environment.NewLine + linkedServerMessage;
                        MessageBox.Show(message);
                        return;
                    }
                }
            }

            Message.Name = txtName.Text;
            if (Message.ID == 0)
            {
                Message.FirstSideEntityID = (int)lokFirstSideEntity.SelectedValue;
                Message.SecondSideEntityID = (int)lokSecondSideEntity.SelectedValue;
            }

            Message.ID = bizDataLink.UpdateDataLink(Message);
            MessageBox.Show("اطلاعات ثبت شد");
            if (Updated != null)
                Updated(this, new MyProject_WPF.UpdatedEventArg() { ID = Message.ID });
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);

        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            NewItem();
        }

        private void NewItem()
        {
            Message = new DataLinkDTO();
            Message.FirstSideEntityID = FirstEntityID;
            ShowMessage();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            frmDataLinkSelect view = new MyProject_WPF.frmDataLinkSelect(FirstEntityID);
            view.LetterTemplateSelected += View_LetterTemplateSelected;
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form");
        }

        private void View_LetterTemplateSelected(object sender, DataLinkSelectedArg e)
        {
            if (e.ID != 0)
            {
                GetDataLink(e.ID);
            }
        }
    }
    public class UpdatedEventArg : EventArgs
    {
        public int ID { set; get; }
    }
}
