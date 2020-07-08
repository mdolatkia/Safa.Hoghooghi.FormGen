using ModelEntites;
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
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmObjects.xaml
    /// </summary>
    public partial class frmPermission : UserControl
    {
        BizDatabase bizDatabase = new BizDatabase();
        BizSubSystem bizSubSystem = new BizSubSystem();
        public ObjectDTO Object { set; get; }
        BizSecuritySubject bizSecuritySubject = new BizSecuritySubject();
        BizPermission bizPermission = new BizPermission();
        int SecuritySubjectID { set; get; }
        PermissionDTO Message { set; get; }
        public frmPermission()
        {
            InitializeComponent();
            //AddObjectListPane();
            AddSubjectListPane();
            AddSubsystemsTree();
            AddSecurityActionPane();
            SetDatabaseLookup();
            lokDatabase.SelectionChanged += LokDatabase_SelectionChanged;

            //  ucRoleList.RoleOrRoleGroupSelected += UcRoleList_RoleOrRoleGroupSelected;

            //درست شود

        }

        private void AddSubsystemsTree()
        {
            var list = bizSubSystem.GetAllSubSystemsObjectDTO();
            var root = AddDBObjectsToTree(new ObjectDTO() { Title = "زیر سیستمها", ObjectCategory = DatabaseObjectCategory.RootMenu }, treeSubSystems.Items);
            foreach (var item in list)
            {
                AddDBObjectsToTree(item, root.Items);
            }
            treeSubSystems.SelectionChanged += TreeSubSystems_SelectionChanged;
        }

        private void TreeSubSystems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (treeSubSystems.SelectedItem != null)
            {
                var ObjectDTO = (treeSubSystems.SelectedItem as RadTreeViewItem).DataContext as ObjectDTO;
                if (ObjectDTO != null && ObjectDTO.ObjectIdentity != 0)
                {
                    Object = ObjectDTO;
                    Actions = SecurityHelper.GetActionsByCategory(ObjectDTO.ObjectCategory);
                    frmSecurityAction.SetActionTree(Actions);
                    GetMessage();
                }
            }
        }

      

        private RadTreeViewItem AddDBObjectsToTree(ObjectDTO item, ItemCollection collection)
        {
            var treeItem = new RadTreeViewItem();
            treeItem.Header = GetNodeHeader(item.Title, item.ObjectCategory);
            treeItem.DataContext = item;
            collection.Add(treeItem);
            return treeItem;
        }
        private FrameworkElement GetNodeHeader(string title, DatabaseObjectCategory type)
        {
            StackPanel pnlHeader = new StackPanel();
            System.Windows.Controls.TextBlock label = new System.Windows.Controls.TextBlock();
            label.Text = title;
            Image img = new Image();
            img.Width = 15;
            Uri uriSource = null;
            if (type == DatabaseObjectCategory.Archive)
            {
                uriSource = new Uri("../Images/archive.png", UriKind.Relative);
            }
            //else if (type == DatabaseObjectCategory.Schema)
            //{
            //    uriSource = new Uri("../Images/folder.png", UriKind.Relative);
            //}
            //else if (type == DatabaseObjectCategory.Entity)
            //{
            //    uriSource = new Uri("../Images/form.png", UriKind.Relative);
            //}
            //else if (type == DatabaseObjectCategory.Report)
            //{
            //    uriSource = new Uri("../Images/report.png", UriKind.Relative);
            //}
            if (uriSource != null)
                img.Source = new BitmapImage(uriSource);
            pnlHeader.Orientation = Orientation.Horizontal;
            pnlHeader.Children.Add(img);
            pnlHeader.Children.Add(label);
            return pnlHeader;
        }


        frmSecurityAction frmSecurityAction;
        private void AddSecurityActionPane()
        {
            frmSecurityAction = new frmSecurityAction();
            RadPane pane = new RadPane();
            pane.CanUserClose = false;
            pane.Header = "دسترسی";
            pane.Content = frmSecurityAction;
            pnlActionList.Items.Add(pane);
        }

        frmSecuritySubjectSelect frmSecuritySubjectSelect;
        private void AddSubjectListPane()
        {
            frmSecuritySubjectSelect = new frmSecuritySubjectSelect();
            RadPane pane = new RadPane();
            pane.Header = "لیست موضوع";
            pane.CanUserClose = false;
            pane.Content = frmSecuritySubjectSelect;
            pnlSubjectList.Items.Add(pane);

            frmSecuritySubjectSelect.SecuritySubjectSelected += FrmSecuritySubjectSelect_SecuritySubjectSelected;
        }

        private void FrmSecuritySubjectSelect_SecuritySubjectSelected(object sender, SecuritySubjectSelectedArg e)
        {
            SecuritySubjectID = e.SecuritySubjectID;
            GetMessage();
        }

        private void LokDatabase_SelectionChanged(object sender, MyCommonWPFControls.SelectionChangedArg e)
        {
            if (lokDatabase.SelectedItem != null)
                AddDatabaseTree((int)lokDatabase.SelectedValue);
        }
        frmDatabaseTree ucObjectTree1;
        private void AddDatabaseTree(int databaseID)
        {
            ucObjectTree1 = new MyProject_WPF.frmDatabaseTree(new List<int>() { databaseID }, false, false, false, false, true, true, true, false, true, true);
            ucObjectTree1.ObjectSelected += ucObjectList_ObjectSelected;
            grdDatabaseTree.Children.Clear();
            grdDatabaseTree.Children.Add(ucObjectTree1);
        }

        private void SetDatabaseLookup()
        {
            BizDatabase bizDatabase = new BizDatabase();
            lokDatabase.SelectedValueMember = "ID";
            lokDatabase.DisplayMember = "Title";
            lokDatabase.NewItemEnabled = false;
            lokDatabase.EditItemEnabled = false;
            //   قابل دسترسی کاربر     //
            var databases = bizDatabase.GetDatabases();
            lokDatabase.ItemsSource = databases;
        }

        List<SecurityActionTreeItem> Actions { set; get; }
        BizColumn bizColumn = new BizColumn();
        void ucObjectList_ObjectSelected(object sender, ObjectSelectedArg e)
        {
            Object = e.Object;
            Actions = SecurityHelper.GetActionsByCategory(Object.ObjectCategory);
            //if(Object.ObjectCategory==DatabaseObjectCategory.Column )
            // {
            //     if(bizColumn.IsColumnPrimaryKey(Object.ObjectIdentity))
            //     {
            //         if(Actions.Any(x=>x.Action==))
            //     }
            // }
            //List<SecActionDTO> list = new List<SecActionDTO>();
            //foreach (var action in actions)
            //{
            //    SecActionDTO item = new SecActionDTO();
            //    item.Action = action;
            //    list.Add(item);
            //}
            //dtgRoleActions.ItemsSource = list;
            frmSecurityAction.SetActionTree(Actions);
            GetMessage();
        }




        private void GetMessage()
        {
            if (Object != null && SecuritySubjectID != 0)
            {

                Message = bizPermission.GetPermission(SecuritySubjectID, Object.ObjectIdentity);
                if (Message != null)
                    frmSecurityAction.ShowData(Message.Actions);
                else
                    frmSecurityAction.ClearData();
            }
        }







        //private void SetActionCheckbox(SecurityActoinTreeItem action, bool check, ItemCollection items)
        //{
        //    foreach (RadTreeViewItem item in items)
        //    {
        //        if (item.DataContext != null)
        //        {
        //            if (item.DataContext == action)
        //            {
        //                var checkbox = item.Header as CheckBox;
        //                checkbox.IsChecked = check;
        //                return;
        //            }
        //        }
        //        SetActionCheckbox(action, check, item.Items);
        //    }


        //}



        //void ucObjectEdit_ObjectSaved(object sender, ObjectSavedArg e)
        //{
        //    ucObjectList.ShowObjects(e.Object.ParentID);
        //}

        //private void btnExtractObjectFromDB_Click(object sender, RoutedEventArgs e)
        //{
        //    BizObject bizObject = new BizObject();
        //    bizObject.ExtractObjectsFromDB();
        //    ucObjectList.ShowObjects(null);
        //}

        private void btnSaveRoleActions_Click(object sender, RoutedEventArgs e)
        {
            var listActions = frmSecurityAction.GetCheckedActions();
            if (listActions.Any(x => x == SecurityAction.NoAccess))
            {
                if (listActions.Any(x => x != SecurityAction.NoAccess))
                {
                    MessageBox.Show("امکان انتخاب گزینه های عدم دسترسی و سایر گزینه ها نمی باشد");
                    return;
                }
            }
            //if (listActions.Any(x => x == SecurityAction.ReadOnly))
            //{
            //    if (listActions.Any(x => x != SecurityAction.NoAccess && x != SecurityAction.ReadOnly))
            //    {
            //        MessageBox.Show("امکان انتخاب گزینه های فقط خواندنی و سایر گزینه ها نمی باشد");
            //        return;
            //    }
            //}
            if (SecuritySubjectID == 0)
            {
                MessageBox.Show("نقشی انتخاب نشده است");
                return;
            }
            if (Message == null)
                Message = new PermissionDTO();
            Message.SecuritySubjectID = SecuritySubjectID;
            Message.SecurityObjectID = Convert.ToInt32(Object.ObjectIdentity);
            Message.SecurityObjectCategory = Object.ObjectCategory;
            Message.Actions = listActions;
            var result = bizPermission.SavePermission(MyProjectManager.GetMyProjectManager.GetRequester(), Message);
            if (result.Result == ProxyLibrary.Enum_DR_ResultType.SeccessfullyDone)
                MessageBox.Show("اطلاعات ثبت شد");
            else
                MessageBox.Show(result.Message);
        }


    }
}
