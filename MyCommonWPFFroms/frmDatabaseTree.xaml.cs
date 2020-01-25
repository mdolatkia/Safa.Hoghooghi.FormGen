using ModelEntites;
using MyDatabaseToObject;
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

namespace MyCommonWPFUserControls
{
    /// <summary>
    /// در فرم Navigation استفاده میشود
    /// </summary>
    /// 
    public partial class frmDatabaseTree : UserControl
    {

        BizDatabaseToObject bizDatabase = new BizDatabaseToObject();
        public frmDatabaseTree()
        {//درست شود..کدام دیتابیس
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                ShowDatabaseObjects(DatabaseObjectCategory.Database, 0);
        }
        public object SelectedItem
        {
            get { return treeDBObjects.SelectedItem; }
        }

        public TreeView TreeDBObjects { get { return treeDBObjects; } }

        public void ShowDatabaseObjects(DatabaseObjectCategory parentCategory, int parentIdentity)
        {
            ItemCollection collection = null;
            if (parentIdentity == null)
                collection = treeDBObjects.Items;
            else
                collection = FindTreeDBObject(treeDBObjects.Items, parentCategory, parentIdentity).Items;

            var objects = bizDatabase.GetDatabaseChildObjects(parentCategory, parentIdentity);
            AddDBObjectsToTree(objects, collection);

        }
        private void AddDBObjectsToTree(List<ObjectDTO> objects, ItemCollection collection)
        {
            collection.Clear();
            foreach (var item in objects)
            {
                var treeItem = new TreeViewItem();
                treeItem.Items.Add("Loading...");
                treeItem.Header = GetNodeHeader(item.Title, item.ObjectCategory);
                treeItem.DataContext = item;
                treeItem.Expanded += treeItem_Expanded;
                //if(item.ParentID==null)
                collection.Add(treeItem);
                //else
                //{
                //    treeItem.Items
                //}
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
                    var objects = bizDatabase.GetDatabaseChildObjects(objectDTO.ObjectCategory, Convert.ToInt32(objectDTO.ObjectIdentity));
                    AddDBObjectsToTree(objects, treeItem.Items);
                }
            }
        }

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
                uriSource = new Uri("Images/Database.png", UriKind.Relative);
            }
            else if (type == DatabaseObjectCategory.Schema)
            {
                uriSource = new Uri("Images/folder.png", UriKind.Relative);
            }
            else if (type == DatabaseObjectCategory.Entity)
            {
                uriSource = new Uri("Images/form.png", UriKind.Relative);
            }
            if (uriSource != null)
                img.Source = new BitmapImage(uriSource);
            pnlHeader.Orientation = Orientation.Horizontal;
            pnlHeader.Children.Add(img);
            pnlHeader.Children.Add(label);
            return pnlHeader;
        }

    }
}
