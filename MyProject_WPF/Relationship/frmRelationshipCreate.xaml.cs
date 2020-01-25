using ModelEntites;

using MyGeneralLibrary;
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
using Telerik.Windows.Controls;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmCodeSelector.xaml
    /// </summary>
    public partial class frmRelationshipCreate : UserControl
    {

        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        RelationshipDTO Message { set; get; }
        BizRelationship bizRelationship = new BizRelationship();
        // List<RelationshipColumnDTO> RelationshipColumns = new List<RelationshipColumnDTO>();
        public frmRelationshipCreate(int relatoinshipID)
        {
            InitializeComponent();


            SetEntities();
            ControlHelper.GenerateContextMenu(dtgRelationshipColumns);
            cmbRelationshipType.ItemsSource = Enum.GetValues(typeof(CreateRelationshipType));
            if (relatoinshipID == 0)
            {
                Message = new RelationshipDTO();
                ShowMessage();
            }
            else
            {
                GetRelationship(relatoinshipID);
            }

        }

        private void GetRelationship(int relatoinshipID)
        {
            var relationship = bizRelationship.GetRelationship(relatoinshipID);
            if (relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary)
                Message = bizRelationship.GetRelationship(relationship.PairRelationshipID);
            else
                Message = relationship;
            ShowMessage();
        }

        private void ShowMessage()
        {
            txtName.Text = Message.Name;
            cmbFirstEntity.SelectedValue = Message.EntityID1;
            cmbSecondEntity.SelectedValue = Message.EntityID2;
            cmbRelationshipType.SelectedItem = Message.CreateType;

            cmbFirstEntity.IsEnabled = Message.ID == 0;
            cmbSecondEntity.IsEnabled = Message.ID == 0;

            //chkFirstSideDataEntry.Visibility = Message.ID == 0 ? Visibility.Visible : Visibility.Collapsed;
            chkSecondSideDataEntry.Visibility = Message.ID == 0 ? Visibility.Visible : Visibility.Collapsed;

            //  RelationshipColumns = new List<RelationshipColumnDTO>();

            //foreach (var col in Message.RelationshipColumns)
            //{
            //    var rColumn = new RelationshipColumnDTO();

            //    rColumn.FirstSideColumnID = col.FirstSideColumnID;
            //    //   rColumn.PrimarySideFixedValue = col.PrimarySideFixedValue;
            //    rColumn.SecondSideColumnID = col.SecondSideColumnID;
            //    RelationshipColumns.Add(rColumn);
            //}
            dtgRelationshipColumns.ItemsSource = Message.RelationshipColumns;
        }


        private void SetEntities()
        {

            // var entities = bizTableDrivedEntity.GetAllEntities();

            cmbFirstEntity.AddColumn("Alias", "عنوان");
            cmbFirstEntity.AddColumn("Name", "نام");
            cmbFirstEntity.SelectionChanged += CmbFirstEntity_SelectionChanged;
            cmbFirstEntity.DisplayMember = "Name";
            cmbFirstEntity.SelectedValueMember = "ID";
            cmbFirstEntity.SearchFilterChanged += CmbFirstEntity_SearchFilterChanged;

            //cmbFirstEntity.ItemsSource = entities;

            cmbSecondEntity.AddColumn("Alias", "عنوان");
            cmbSecondEntity.AddColumn("Name", "نام");
            cmbSecondEntity.SelectionChanged += cmbSecondEntity_SelectionChanged;
            cmbSecondEntity.DisplayMember = "Name";
            cmbSecondEntity.SelectedValueMember = "ID";
            cmbSecondEntity.SearchFilterChanged += cmbSecondEntity_SearchFilterChanged;



        }

        private void CmbFirstEntity_SearchFilterChanged(object sender, MyCommonWPFControls.SearchFilterArg e)
        {
            if (!string.IsNullOrEmpty(e.SingleFilterValue))
            {
                if (e.FilterBySelectedValue)
                {
                    var entities = bizTableDrivedEntity.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), Convert.ToInt32(e.SingleFilterValue), EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships);
                    e.ResultItemsSource = new List<TableDrivedEntityDTO> { entities };
                }
                else
                {
                    var entities = bizTableDrivedEntity.GetAllEntities(MyProjectManager.GetMyProjectManager.GetRequester(), e.SingleFilterValue, false);
                    e.ResultItemsSource = entities;
                }
            }
            //else if (e.Filters.Count > 0)
            //{

            //}

        }

        private void CmbFirstEntity_SelectionChanged(object sender, MyCommonWPFControls.SelectionChangedArg e)
        {

            if (cmbFirstEntity.SelectedItem != null)
            {
                var entity = cmbFirstEntity.SelectedItem as TableDrivedEntityDTO;
                var targetEntity = bizTableDrivedEntity.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), entity.ID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
                colFirst.ItemsSource = targetEntity.Columns;
                colFirst.DisplayMemberPath = "Alias";
                colFirst.SelectedValueMemberPath = "ID";
                CheckRelatioshipProperties();
            }
        }

        private void CheckRelatioshipProperties()
        {
            var firstEntity = cmbFirstEntity.SelectedItem as TableDrivedEntityDTO;
            var secondEntity = cmbSecondEntity.SelectedItem as TableDrivedEntityDTO;
            if (firstEntity != null && secondEntity != null)
            {
                if (firstEntity.IsView || secondEntity.IsView)
                {
                    //chkFirstSideDataEntry.Visibility = Visibility.Collapsed;
                    chkSecondSideDataEntry.Visibility = Visibility.Collapsed;

                    lblRelationshipType.Visibility = Visibility.Collapsed;
                    cmbRelationshipType.Visibility = Visibility.Collapsed;

                }
                else
                {
                    //chkFirstSideDataEntry.Visibility = Visibility.Visible;
                    chkSecondSideDataEntry.Visibility = Visibility.Visible;

                    lblRelationshipType.Visibility = Visibility.Visible;
                    cmbRelationshipType.Visibility = Visibility.Visible;

                }
            }

        }

        private void cmbSecondEntity_SearchFilterChanged(object sender, MyCommonWPFControls.SearchFilterArg e)
        {
            if (!string.IsNullOrEmpty(e.SingleFilterValue))
            {
                if (e.FilterBySelectedValue)
                {
                    var entities = bizTableDrivedEntity.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), Convert.ToInt32(e.SingleFilterValue), EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships);
                    e.ResultItemsSource = new List<TableDrivedEntityDTO> { entities };
                }
                else
                {
                    var entities = bizTableDrivedEntity.GetAllEntities(MyProjectManager.GetMyProjectManager.GetRequester(), e.SingleFilterValue, null);
                    e.ResultItemsSource = entities;
                }
            }
            //else if (e.Filters.Count > 0)
            //{

            //}

        }

        private void cmbSecondEntity_SelectionChanged(object sender, MyCommonWPFControls.SelectionChangedArg e)
        {
            if (cmbSecondEntity.SelectedItem != null)
            {
                var entity = cmbSecondEntity.SelectedItem as TableDrivedEntityDTO;
                var targetEntity = bizTableDrivedEntity.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), entity.ID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);

                colSecond.ItemsSource = targetEntity.Columns;
                colSecond.DisplayMemberPath = "Alias";
                colSecond.SelectedValueMemberPath = "ID";
                CheckRelatioshipProperties();


            }
        }
        private void cmbSecondEntity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSecondEntity.SelectedItem != null)
            {
                var entity = cmbSecondEntity.SelectedItem as TableDrivedEntityDTO;
                var column = dtgRelationshipColumns.Columns[1] as GridViewComboBoxColumn;
                var targetEntity = bizTableDrivedEntity.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), entity.ID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);

                column.ItemsSource = targetEntity.Columns;
                column.DisplayMemberPath = "Alias";
                column.SelectedValueMemberPath = "ID";
            }
        }
        //private void GetRelationship(int ID)
        //{

        //}

        private void btnSaveAndSelect_Click(object sender, RoutedEventArgs e)
        {
            //در صورت ایجاد اولیه باید چک شود که طرف اول پرایمری کی باشد
            if (txtName.Text == "")
            {
                MessageBox.Show("نام رابطه");
                return;
            }
            if (cmbFirstEntity.SelectedItem == null)
            {
                MessageBox.Show("موجودیت سمت اول");
                return;
            }
            if (cmbSecondEntity.SelectedItem == null)
            {
                MessageBox.Show("موجودیت سمت دوم");
                return;
            }
            if (Message.RelationshipColumns.Count == 0)
            {
                MessageBox.Show("ستونها");
                return;
            }

            string linkedServerMessage = "";
            BizDatabase bizDatabase = new BizDatabase();
            var firstEntity = bizTableDrivedEntity.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), (int)cmbFirstEntity.SelectedValue, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
            var secondEntity = bizTableDrivedEntity.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), (int)cmbSecondEntity.SelectedValue, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
            if (firstEntity.ServerID != secondEntity.ServerID)
            {
                if (!bizDatabase.LinkedServerExists(firstEntity.ServerID, secondEntity.ServerID))
                    linkedServerMessage += (linkedServerMessage == "" ? "" : Environment.NewLine) + "لینک سرور از طرف موجودیت" + " '" + firstEntity.Alias + "' " + "به موجودیت" + " '" + secondEntity.Alias + "' " + "تعریف نشده است";
                if (!bizDatabase.LinkedServerExists(secondEntity.ServerID, firstEntity.ServerID))
                    linkedServerMessage += (linkedServerMessage == "" ? "" : Environment.NewLine) + "لینک سرور از طرف موجودیت" + " '" + secondEntity.Alias + "' " + "به موجودیت" + " '" + firstEntity.Alias + "' " + "تعریف نشده است";
            }
            if(!string.IsNullOrEmpty(linkedServerMessage))
            {
                var message = linkedServerMessage;
                message += Environment.NewLine + "به منظور استفاده از این رابطه در نمایش و جستجوی داده ها و عدم بروز خطا لینک سرورها تعریف شوند";
                MessageBox.Show(message);
       //         return;
            }
            if (!firstEntity.IsView && !secondEntity.IsView)
            {
                foreach (var relCol in Message.RelationshipColumns)
                {
                    var firstSideColumn = firstEntity.Columns.First(x => x.ID == relCol.FirstSideColumnID);
                    if (!firstSideColumn.PrimaryKey)
                    {
                        MessageBox.Show("ستون" + " " + firstSideColumn.Name + " " + "کلید اصلی نمی باشد");
                        return;
                    }
                }
            }
            Message.Name = txtName.Text;
            Message.EntityID1 = (int)cmbFirstEntity.SelectedValue;
            Message.EntityID2 = (int)cmbSecondEntity.SelectedValue;
            Message.CreateType = (CreateRelationshipType)cmbRelationshipType.SelectedItem;

            var id = bizRelationship.CreateUpdateRelationship(MyProjectManager.GetMyProjectManager.GetRequester(), Message, chkSecondSideDataEntry.IsChecked == true);

            MessageBox.Show("اطلاعات ثبت شد");
            GetRelationship(id);
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Message = new RelationshipDTO();
            ShowMessage();
        }
    }


}
