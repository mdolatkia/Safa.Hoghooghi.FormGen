﻿using ModelEntites;
using MyDatabaseToObject;
using MyModelManager;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyProject_WPF
{
    /// <summary>
    /// در فرم Navigation استفاده میشود
    /// </summary>
    /// 
    public partial class frmDatabaseTreeForNavigation : UserControl
    {
        BizDatabase bizDatabase = new BizDatabase();
        public event EventHandler<ObjectSelectedArg> ObjectSelected;

        BizDatabaseToObject bizDatabaseToObject = new BizDatabaseToObject();
        public frmDatabaseTreeForNavigation(List<int> databaseIDs)
        {
            InitializeComponent();
            treeDBObjects.SelectedItemChanged += TreeDBObjects_SelectedItemChanged;
         
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                ShowDatabaseObjects(databaseIDs);
        }

        private void TreeDBObjects_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
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

        public object SelectedItem
        {
            get { return treeDBObjects.SelectedItem; }
        }
        public TreeView TreeDBObjects { get { return treeDBObjects; } }
        public void ShowDatabaseObjects(List<int> databaseIds)
        {
            ItemCollection collection = treeDBObjects.Items; ;
            //if (parentCategory == DatabaseObjectCategory.Database)
            //    collection = treeDBObjects.Items;
            //else
            //    collection = FindTreeDBObject(treeDBObjects.Items, parentCategory, parentIdentity).Items;

            foreach (var id in databaseIds)
            {
                var database = bizDatabase.GetDatabase(id);
                ObjectDTO dbObject = new ObjectDTO();
                dbObject.ObjectCategory = DatabaseObjectCategory.Database;
                dbObject.ObjectIdentity = id;
                //dbObject.SecurityObjectID = database.SecurityObjectID;
                dbObject.Title = database.Name;
                AddDBObjectsToTree(dbObject, treeDBObjects.Items);
            }

            //var objects = bizDatabaseToObject.GetDatabaseChildObjects(DatabaseObjectCategory.Database, databaseID);
            //AddDBObjectsToTree(objects, collection);
            if (treeDBObjects.Items.Count > 0 && treeDBObjects.Items[0] is TreeViewItem)
                (treeDBObjects.Items[0] as TreeViewItem).IsExpanded = true;

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
            //if (!ExceptColumns || item.ObjectCategory != DatabaseObjectCategory.Entity)
            //{
            treeItem.Items.Add("Loading...");
            treeItem.Expanded += treeItem_Expanded;
            //}
            treeItem.Header = GetNodeHeader(item.Title, item.ObjectCategory);
            treeItem.DataContext = item;
            //treeItem.MouseRightButtonDown += (sender, e) => TreeItem_MouseRightButtonDown(sender, e, treeItem, item);
            //if(item.ParentID==null)
            collection.Add(treeItem);
            return treeItem;
            //else
            //{
            //    treeItem.Items
            //}

        }
        //private void AddDBObjectsToTree(List<ObjectDTO> objects, ItemCollection collection)
        //{
        //    collection.Clear();
        //    foreach (var item in objects)
        //    {
        //        AddDBObjectsToTree(item, collection);
        //    }
        //}
        //private void AddDBObjectsToTree(List<ObjectDTO> objects, ItemCollection collection)
        //{
        //    collection.Clear();
        //    foreach (var item in objects)
        //    {
        //        var treeItem = new TreeViewItem();
        //        treeItem.Items.Add("Loading...");
        //        treeItem.Header = GetNodeHeader(item.Title, item.ObjectCategory);
        //        treeItem.DataContext = item;
        //        treeItem.Expanded += treeItem_Expanded;
        //        //if(item.ParentID==null)
        //        collection.Add(treeItem);
        //        //else
        //        //{
        //        //    treeItem.Items
        //        //}
        //    }
        //}
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

        //private TreeViewItem FindTreeDBObject(ItemCollection collection, DatabaseObjectCategory objectCategory, int objectIdentity)
        //{

        //    foreach (var item in collection)
        //    {
        //        if ((item is TreeViewItem))
        //        {
        //            var objectDTO = (item as TreeViewItem).DataContext as ObjectDTO;
        //            if (objectDTO.ObjectIdentity == objectIdentity.ToString() && objectDTO.ObjectCategory == objectCategory)
        //                return (item as TreeViewItem);
        //            else
        //            {
        //                var result = FindTreeDBObject((item as TreeViewItem).Items, objectCategory, objectIdentity);
        //                if (result != null)
        //                    return result;
        //            }
        //        }

        //    }
        //    return null;
        //}


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
            else if (type == DatabaseObjectCategory.Report)
            {
                uriSource = new Uri("../Images/report.png", UriKind.Relative);
            }
            if (uriSource != null)
                img.Source = new BitmapImage(uriSource);
            pnlHeader.Orientation = Orientation.Horizontal;
            pnlHeader.Children.Add(img);
            pnlHeader.Children.Add(label);
            return pnlHeader;
        }

    }
    //public class ObjectSelectedArg : EventArgs
    //{
    //    public ObjectDTO Object { set; get; }
    //}
}
