using ModelEntites;
using MyDatabaseToObject;
using MyModelManager;
using MySecurity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for frmObjectList.xaml
    /// </summary>
    public partial class frmSecurityObjectTree : UserControl
    {
        public event EventHandler<ObjectRightClickedArg> ObjectRightClick;
        public event EventHandler<ObjectSelectedArg> ObjectSelected;
        BizDatabaseToObject bizDatabaseToObject = new BizDatabaseToObject();
        BizDatabase bizDatabase = new BizDatabase();
        public frmSecurityObjectTree()
        {
            InitializeComponent();
            var databases = bizDatabase.GetDatabases();
            ShowDatabaseObjects(databases.Select(x => x.ID).ToList());
            //bizDatabaseToObject.IgnoreRelationshipColumns = true;
        }



        bool ExceptColumns { set; get; }
        //public void ShowDatabaseTree(List<int> databaseIds, bool exceptColumns = false)
        //{
        //    ExceptColumns = exceptColumns;
        //    ShowDatabaseObjects(databaseIds);
        //}

        internal void SetContextMenu(TreeViewItem node, RadContextMenu radContextMenu)
        {
            RadContextMenu.SetContextMenu(node, radContextMenu);
        }

        public void ShowDatabaseObjects(List<int> databaseIds)
        {
            //ItemCollection collection = null;
            //if (parentIdentity == 0)
            //    collection = treeDBObjects.Items;
            //else
            //    collection = FindTreeDBObject(treeDBObjects.Items, parentCategory, parentIdentity).Items;
            foreach (var id in databaseIds)
            {
                var database = bizDatabase.GetDatabase(id);
                ObjectDTO dbObject = new ObjectDTO();
                dbObject.ObjectCategory = DatabaseObjectCategory.Database;
                dbObject.ObjectIdentity = id.ToString();
                //dbObject.SecurityObjectID = database.SecurityObjectID;
                dbObject.Title = database.Name;
                var node = AddDBObjectsToTree(dbObject, treeDBObjects.Items);
            }
            //     var objects = bizDatabaseToObject.GetDatabaseChildObjects(parentCategory, parentIdentity);
            //  AddDBObjectsToTree(objects, collection);

        }
        private void AddDBObjectsToTree(List<ObjectDTO> objects, ItemCollection collection)
        {
            collection.Clear();
            foreach (var item in objects)
            {
                AddDBObjectsToTree(item, collection);
            }
        }
        private TreeViewItem AddDBObjectsToTree(ObjectDTO item, ItemCollection collection)
        {

            var treeItem = new TreeViewItem();
            if (!ExceptColumns || item.ObjectCategory != DatabaseObjectCategory.Entity)
            {
                treeItem.Items.Add("Loading...");
                treeItem.Expanded += treeItem_Expanded;
            }
            treeItem.Header = GetNodeHeader(item.Title, item.ObjectCategory);
            treeItem.DataContext = item;
            treeItem.MouseRightButtonDown += (sender, e) => TreeItem_MouseRightButtonDown(sender, e, treeItem, item);
            //if(item.ParentID==null)
            collection.Add(treeItem);
            return treeItem;
            //else
            //{
            //    treeItem.Items
            //}

        }

        private void TreeItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e, TreeViewItem treeItem, ObjectDTO item)
        {

            if (ObjectRightClick != null)
            {
                ObjectRightClickedArg arg = new MyProject_WPF.ObjectRightClickedArg();
                arg.Node = treeItem;
                arg.Object = item;
                ObjectRightClick(sender, arg);
            }

        }



        void treeItem_Expanded(object sender, RoutedEventArgs e)
        {

            var treeItem = e.Source as TreeViewItem;
            if (treeItem != null)
            {
                bool firstTime = false;
                if (treeItem.Items.Count > 0)
                {
                    var firstItem = treeItem.Items[0];
                    if (firstItem is string && firstItem.ToString() == "Loading...")
                        firstTime = true;

                }
                if (firstTime)
                {
                    treeItem.Items.Clear();


                    var objectDTO = treeItem.DataContext as ObjectDTO;
                    var objects = bizDatabaseToObject.GetDatabaseChildObjects(objectDTO.ObjectCategory, Convert.ToInt32(objectDTO.ObjectIdentity));
                    AddDBObjectsToTree(objects, treeItem.Items);
                }
            }
        }

        //private void ShowDatabaseTreeItem(ItemCollection itemCollection, ObjectDTO item)
        //{
        //    var node = AddNavigationNode(itemCollection, item);
        //    foreach (var citem in item.ChildObjects)
        //        ShowDatabaseTreeItem(node.Items, citem);
        //}
        //private TreeViewItem AddDBObjectsToTree(ItemCollection collection, ObjectDTO item)
        //{
        //    var node = new TreeViewItem();
        //    node.DataContext = item;
        //    node.Header = GetNodeHeader(item.Title, item.Category);
        //    collection.Add(node);
        //    return node;
        //}

        private TreeViewItem FindTreeDBObject(ItemCollection collection, DatabaseObjectCategory objectCategory, int objectIdentity)
        {

            foreach (var item in collection)
            {
                if ((item is TreeViewItem))
                {
                    var objectDTO = (item as TreeViewItem).DataContext as ObjectDTO;
                    if (objectDTO.ObjectIdentity == objectIdentity.ToString() && objectDTO.ObjectCategory == objectCategory)
                        return (item as TreeViewItem);
                    else
                    {
                        var result = FindTreeDBObject((item as TreeViewItem).Items, objectCategory, objectIdentity);
                        if (result != null)
                            return result;
                    }
                }

            }
            return null;
        }


        private FrameworkElement GetNodeHeader(string title, DatabaseObjectCategory type)
        {
            StackPanel pnlHeader = new StackPanel();
            TextBlock label = new TextBlock();
            label.Text = title;
            Image img = new Image();
            img.Width = 15;
            Uri uriSource = null;
            if (type == DatabaseObjectCategory.Database)
            {
                uriSource = new Uri("../Images/Database.png", UriKind.Relative);
            }
            else if (type == DatabaseObjectCategory.Schema)
            {
                uriSource = new Uri("../Images/folder.png", UriKind.Relative);
            }
            else if (type == DatabaseObjectCategory.Entity)
            {
                uriSource = new Uri("../Images/form.png", UriKind.Relative);
            }
            else if (type == DatabaseObjectCategory.Command)
            {
                uriSource = new Uri("../Images/command.png", UriKind.Relative);
            }
            if (uriSource != null)
                img.Source = new BitmapImage(uriSource);
            pnlHeader.Orientation = Orientation.Horizontal;
            pnlHeader.Children.Add(img);
            pnlHeader.Children.Add(label);
            return pnlHeader;
        }


        private void treeObjects_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeDBObjects.SelectedItem != null)
            {
                var ObjectDTO = (treeDBObjects.SelectedItem as TreeViewItem).DataContext as ObjectDTO;
                if (ObjectDTO != null)
                {
                    ObjectSelectedArg arg = new ObjectSelectedArg();
                    arg.Object = ObjectDTO;
                    if (ObjectSelected != null)
                        ObjectSelected(this, arg);
                }
            }
        }
    }
    public class ObjectSelectedArg : EventArgs
    {
        public ObjectDTO Object { set; get; }
    }

    public class ObjectRightClickedArg : EventArgs
    {
        public ObjectDTO Object { set; get; }
        public TreeViewItem Node { set; get; }
    }
}
