using Microsoft.Win32;
using ModelEntites;

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
    public partial class frmEntityGridView : UserControl
    {
        GridViewSettingDTO Message { set; get; }
        BizEntityGridView bizEntityGridView = new BizEntityGridView();
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
        int EntityID { set; get; }

        public frmEntityGridView(int entityID)
        {
            InitializeComponent();
            EntityID = entityID;
            SetGridViewList();
            //SetRelationships();
            //SetRelationshipTails();
            GetEntityCommand(entityID);
        }

        //private void SetRelationshipTails()
        //{//چک شود فقط یکی ازین دو پر شوند
        //    var relationshipTails = bizEntityRelationshipTail.GetEntityRelationshipTails(EntityID);
        //    colRelationshipTail.DisplayMemberPath = "EntityPath";
        //    colRelationshipTail.SelectedValueMemberPath = "ID";
        //    colRelationshipTail.ItemsSource = relationshipTails;

        //}

        //private void SetRelationships()
        //{
        //    var entity = bizTableDrivedEntity.GetTableDrivedEntity(EntityID, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithRelationships);
        //    var relationships = entity.Relationships;
        //    colRelationship.DisplayMemberPath = "Alias";
        //    colRelationship.SelectedValueMemberPath = "ID";
        //    colRelationship.ItemsSource = relationships;
        //}

        private void SetGridViewList()
        {
            BizEntityListView bizEntityListView = new BizEntityListView();
            lokEntityGridView.DisplayMember = "Title";
            lokEntityGridView.SelectedValueMember = "ID";
            lokEntityGridView.ItemsSource = bizEntityListView.GetEntityListViews(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID);
        }
        private void GetEntityCommand(int entityID)
        {
            Message = bizEntityGridView.GetGridViewSetting(entityID, false);
            if (Message == null)
                Message = new GridViewSettingDTO();
            ShowMessage();
        }

        private void ShowMessage()
        {
            lokEntityGridView.SelectedValue = Message.EntityListViewID;
            //dtgRelationships.ItemsSource = Message.EntityGridViewRelationships;
            //if (Message.IconContent != null)
            //{
            //    grdExisting.Visibility = Visibility.Visible;
            //    grdAddFile.Visibility = Visibility.Collapsed;
            //    txtExistingFile.Text = "دارای فایل";
            //}
            //else
            //{
            //    grdExisting.Visibility = Visibility.Collapsed;
            //    grdAddFile.Visibility = Visibility.Visible;
            //}
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
            //if (txtFilePath.Text != "")
            //{
            //    Message.IconContent = File.ReadAllBytes(txtFilePath.Text);
            //}
            if (lokEntityGridView.SelectedItem != null)
                Message.EntityListViewID = (int)lokEntityGridView.SelectedValue;
            else
                Message.EntityListViewID = 0;
            bizEntityGridView.UpdateEntityGridViews(EntityID, Message);
            MessageBox.Show("اطلاعات ثبت شد");
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Message = new GridViewSettingDTO();
            ShowMessage();
        }

        //private void RemoveFile_Click(object sender, RoutedEventArgs e)
        //{
        //    txtFilePath.Text = "";
        //    grdExisting.Visibility = Visibility.Collapsed;
        //    grdAddFile.Visibility = Visibility.Visible;
        //    Message.IconContent = null;
        //}
    }

}
