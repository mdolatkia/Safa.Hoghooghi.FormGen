using Microsoft.Win32;
using ModelEntites;
using MyCommonWPFControls;
using MyFormulaFunctionStateFunctionLibrary;
using MyInterfaces;
using MyModelManager;
using ProxyLibrary;
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
    public partial class frmDataMenuSetting : UserControl
    {
        BizEntitySearchableReport bizEntitySearchableReport = new BizEntitySearchableReport();
        DataMenuSettingDTO Message { set; get; }
        BizDataMenuSetting bizEntityReportSetting = new BizDataMenuSetting();
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
        int EntityID { set; get; }

        public frmDataMenuSetting(int entityID)
        {
            InitializeComponent();
            EntityID = entityID;
            SetRelationshipTails();
            GetDataMenuSetting(entityID);
            ControlHelper.GenerateContextMenu(dtgDataGridRelationships);
            ControlHelper.GenerateContextMenu(dtgDataViewRelationships);
            ControlHelper.GenerateContextMenu(dtgReportRelationships);

            //ControlHelper.GenerateContextMenu(dtgExternalReports);
            dtgReportRelationships.RowLoaded += DtgColumns_RowLoaded;
            dtgReportRelationships.CellEditEnded += DtgConditions_CellEditEnded;
            colReportRelationshipTail.EditItemClicked += ColRelationshipTail_EditItemClicked;
            colDataViewRelationshipTail.EditItemClicked += ColRelationshipTail_EditItemClicked;
            colDataGridRelationshipTail.EditItemClicked += ColRelationshipTail_EditItemClicked;

        }
        private void SetRelationshipTails()
        {
            BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
            var relationshipTails = bizEntityRelationshipTail.GetEntityRelationshipTails(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID);

            colDataGridRelationshipTail.DisplayMemberPath = "EntityPath";
            colDataGridRelationshipTail.SelectedValueMemberPath = "ID";
            colDataGridRelationshipTail.ItemsSource = relationshipTails;

            colDataViewRelationshipTail.DisplayMemberPath = "EntityPath";
            colDataViewRelationshipTail.SelectedValueMemberPath = "ID";
            colDataViewRelationshipTail.ItemsSource = relationshipTails;

            colReportRelationshipTail.DisplayMemberPath = "EntityPath";
            colReportRelationshipTail.SelectedValueMemberPath = "ID";
            colReportRelationshipTail.ItemsSource = relationshipTails;

        }
        private void DtgColumns_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (e.DataElement is DataMenuReportRelationshipDTO)
            {
                var data = (e.DataElement as DataMenuReportRelationshipDTO);
                if (data.vwReports == null || data.vwReports.Count == 0)
                    SetRelationshipReports(MyProjectManager.GetMyProjectManager.GetRequester(), data);
            }
        }

        private void DtgConditions_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            if (e.Cell.Column == colReportRelationshipTail)
            {
                if (e.Cell.DataContext is DataMenuReportRelationshipDTO)
                {
                    var condition = (e.Cell.DataContext as DataMenuReportRelationshipDTO);
                    SetRelationshipReports(MyProjectManager.GetMyProjectManager.GetRequester(), condition);
                }
            }
        }

        private void SetRelationshipReports(DR_Requester requester, DataMenuReportRelationshipDTO condition)
        {
            if (condition.RelationshipTailID == 0)
                return;
            colReports.DisplayMemberPath = "ReportTitle";
            colReports.SelectedValueMemberPath = "ID";
            //    var relationshipTail = bizEntityRelationshipTail.GetEntityRelationshipTail(condition.EntityRelationshipTailID);
            var reports = bizEntitySearchableReport.GetEntityReportsOfRelationshipTail(requester, condition.RelationshipTailID);
            condition.vwReports = reports;
        }

        private void ColRelationshipTail_EditItemClicked(object sender, MyCommonWPFControls.EditItemClickEventArg e)
        {
            frmEntityRelationshipTail frm = null;
            frm = new frmEntityRelationshipTail(EntityID);
            MyProjectManager.GetMyProjectManager.ShowDialog(frm, "رابطه های مرتبط");
            frm.ItemSelected += (sender1, e1) => Frm_TailSelected(sender1, e1, (sender as MyStaticLookup));
        }
        private void Frm_TailSelected(object sender1, EntityRelationshipTailSelectedArg e1, MyStaticLookup myStaticLookup)
        {
            SetRelationshipTails();
            myStaticLookup.SelectedValue = e1.EntityRelationshipTailID;
        }

        private void GetDataMenuSetting(int entityID)
        {
            Message = bizEntityReportSetting.GetDataMenuSetting(MyProjectManager.GetMyProjectManager.GetRequester(), entityID, false);
            ShowMessage();
        }

        private void ShowMessage()
        {
            dtgReportRelationships.ItemsSource = Message.ReportRelationships;
            dtgDataViewRelationships.ItemsSource = Message.DataViewRelationships;
            dtgDataGridRelationships.ItemsSource = Message.GridViewRelationships;

            foreach (var item in Message.ReportRelationships)
            {
                SetRelationshipReports(MyProjectManager.GetMyProjectManager.GetRequester(), item);
            }
        }
        //private void AddFile_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.Filter = "Icons (*.png,*.ico)|*.png;*.ico|All files (*.*)|*.*";
        //    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //    //if (openFileDialog.ShowDialog() == true)
        //    //{
        //    //    txtFilePath.Text = openFileDialog.FileName;
        //    //}
        //}

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in Message.DataViewRelationships)
            {
                if (item.RelationshipTailID != 0)
                {
                    var relationshipTail = bizEntityRelationshipTail.GetEntityRelationshipTail(MyProjectManager.GetMyProjectManager.GetRequester(), item.RelationshipTailID);
                    BizTableDrivedEntity bizTableDrivedEntity = new MyModelManager.BizTableDrivedEntity();
                    var entity = bizTableDrivedEntity.GetSimpleEntity(MyProjectManager.GetMyProjectManager.GetRequester(), relationshipTail.TargetEntityID);
                    var linkedServerMessage = bizEntityRelationshipTail.CheckLinkedServers(entity, relationshipTail.ReverseRelationshipTail);
                    if (linkedServerMessage != "")
                    {
                        var message = "اشکال در تعریف نمایش داده و لینک سرور برای موجودیت" + " " + string.IsNullOrEmpty(relationshipTail.TargetEntityAlias);
                        message += Environment.NewLine + linkedServerMessage;
                        MessageBox.Show(message);
                        return;
                    }
                }
            }
            foreach (var item in Message.GridViewRelationships)
            {
                if (item.RelationshipTailID != 0)
                {
                    var relationshipTail = bizEntityRelationshipTail.GetEntityRelationshipTail(MyProjectManager.GetMyProjectManager.GetRequester(), item.RelationshipTailID);
                    BizTableDrivedEntity bizTableDrivedEntity = new MyModelManager.BizTableDrivedEntity();
                    var entity = bizTableDrivedEntity.GetSimpleEntity(MyProjectManager.GetMyProjectManager.GetRequester(), relationshipTail.TargetEntityID);
                    var linkedServerMessage = bizEntityRelationshipTail.CheckLinkedServers(entity, relationshipTail.ReverseRelationshipTail);
                    if (linkedServerMessage != "")
                    {
                        var message = "اشکال در تعریف گرید داده و لینک سرور برای موجودیت" + " " + string.IsNullOrEmpty(relationshipTail.TargetEntityAlias);
                        message += Environment.NewLine + linkedServerMessage;
                        MessageBox.Show(message);
                        return;
                    }
                }
            }
            foreach (var item in Message.ReportRelationships)
            {
                if (item.RelationshipTailID != 0)
                {
                    var relationshipTail = bizEntityRelationshipTail.GetEntityRelationshipTail(MyProjectManager.GetMyProjectManager.GetRequester(), item.RelationshipTailID);
                    BizTableDrivedEntity bizTableDrivedEntity = new MyModelManager.BizTableDrivedEntity();
                    var entity = bizTableDrivedEntity.GetSimpleEntity(MyProjectManager.GetMyProjectManager.GetRequester(), relationshipTail.TargetEntityID);
                    var linkedServerMessage = bizEntityRelationshipTail.CheckLinkedServers(entity, relationshipTail.ReverseRelationshipTail);
                    if (linkedServerMessage != "")
                    {
                        var message = "اشکال در تعریف گزارشات مرتبط و لینک سرور برای موجودیت" + " " + string.IsNullOrEmpty(relationshipTail.TargetEntityAlias);
                        message += Environment.NewLine + linkedServerMessage;
                        MessageBox.Show(message);
                        return;
                    }
                }
                if (item.EntitySearchableReportID == 0)
                {
                    var message = "اشکال در تعریف گزارشات مرتبط";
                    message += Environment.NewLine + "گزارش مرتبط اجباری می باشد";
                    MessageBox.Show(message);
                    return;
                }
            }
            if (Message.ReportRelationships.Any(x => x.RelationshipTailID == 0))
            {
                MessageBox.Show("انتخاب رابطه و گزارش برای لیست گزارشات اجباری می باشد");
                return;
            }
            Message.EntityID = EntityID;
            bizEntityReportSetting.UpdateEntityReportDataMenuSettings(EntityID, Message);
            MessageBox.Show("اطلاعات ثبت شد");
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {

        }

        //private void btnNew_Click(object sender, RoutedEventArgs e)
        //{
        //    Message = new  DataMenuSettingDTO();
        //    ShowMessage();
        //}

        //private void RemoveFile_Click(object sender, RoutedEventArgs e)
        //{
        //    txtFilePath.Text = "";
        //    grdExisting.Visibility = Visibility.Collapsed;
        //    grdAddFile.Visibility = Visibility.Visible;
        //    Message.IconContent = null;
        //}
    }

}
