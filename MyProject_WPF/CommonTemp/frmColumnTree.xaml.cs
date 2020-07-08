using ModelEntites;
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
using Telerik.Windows.Controls;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmDatabaseTree.xaml
    /// </summary>
    /// 

    public partial class frmColumnTree : UserControl
    {
        //public event EventHandler<TreeColumnSelectedArg> ItemSelected;
        BizTableDrivedEntity bizEntity = new BizTableDrivedEntity();
        TableDrivedEntityDTO MainEntity { set; get; }
        public frmColumnTree()
        {
            InitializeComponent();


            //   treeItems.SelectedItemChanged += TreeItems_SelectedItemChanged;
        }
        public void SetEntityID(int entityID)
        {
            if (MainEntity == null || MainEntity.ID != entityID)
            {
                MainEntity = bizEntity.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), entityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithRelationships);
                if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                    PopulateTree();
            }
        }
        //private void TreeItems_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        //{
        //    e.Handled = true;
        //    if (treeItems.SelectedItem != (treeItems.Items[0] as RadTreeViewItem).DataContext)
        //    {
        //        if (ItemSelected != null)
        //        {
        //            TreeColumnSelectedArg arg = new TreeColumnSelectedArg();
        //            arg.ColumnID = Convert.ToInt32(treeItems.SelectedItem);
        //            arg.ColumnName = entity.Columns.First(x => x.ID == arg.ColumnID).Name;
        //            ItemSelected(this, arg);
        //        }
        //    }
        //}

        public object SelectedItem
        {
            get { return treeItems.SelectedItem; }
        }

        public RadTreeView TreeItems { get { return treeItems; } }

        private void PopulateTree()
        {
            var rootNode = AddFolderNode(treeItems.Items, "لیست ستونها");
            PopulateTreeItems(rootNode.Items, MainEntity, "", "");
            rootNode.IsExpanded = true;
        }


        private void PopulateTreeItems(ItemCollection items, TableDrivedEntityDTO entity, string relationshipPath, string entityPath)
        {

            foreach (var column in entity.Columns)
            {
                var treeItem = new TreeColumnItem();
                treeItem.ColumnID = column.ID;
                treeItem.ColumnName = column.Name;
                treeItem.Alias = column.Alias;
                treeItem.RelationshipPath = relationshipPath;
                treeItem.EntityPath = entityPath;
                AddColumnNode(items, treeItem, column.Alias);
            }
            foreach (var relationship in entity.Relationships)
            {
                var newrelationshipPath = (relationshipPath == "" ? "" : relationshipPath + ",") + relationship.ID;
                var newEntityPath = (entityPath == "" ? "" : entityPath + ",") + relationship.Entity2;
                TreeRelationshipItem relItem = new TreeRelationshipItem();
                relItem.Relationship = relationship;
                relItem.RelationshipPath = newrelationshipPath;
                relItem.EntityPath = newEntityPath;
                var node = AddRelationshipNode(items, relItem);
                node.Expanded += Node_Expanded;
                node.Items.Add("Loading...");
            }
        }
        private void Node_Expanded(object sender, RoutedEventArgs e)
        {
            var treeItem = e.Source as RadTreeViewItem;
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

                    BizRelationship bizRelationship = new BizRelationship();
                    var objectDTO = treeItem.DataContext as TreeRelationshipItem;
                    var entityID = objectDTO.Relationship.EntityID2;
                    var cEntity = bizEntity.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), entityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithRelationships);
                    TreeRelationshipItem relItem = new TreeRelationshipItem();
                    PopulateTreeItems(treeItem.Items, cEntity, objectDTO.RelationshipPath, objectDTO.EntityPath);

                }
            }
        }
        private RadTreeViewItem AddRelationshipNode(ItemCollection collection, TreeRelationshipItem relationship)
        {
            var node = new RadTreeViewItem();
            node.DataContext = relationship;
            node.Header = GetNodeHeader(relationship.Relationship.Entity2, DatabaseObjectCategory.Relationship);
            node.ToolTip = relationship.Relationship.ID + "_" + (string.IsNullOrEmpty(relationship.Relationship.Alias) ? relationship.Relationship.Name : relationship.Relationship.Alias);
            collection.Add(node);
            return node;
        }

        private RadTreeViewItem AddColumnNode(ItemCollection collection, TreeColumnItem treeColumnItem, string title)
        {
            var node = new RadTreeViewItem();
            node.DataContext = treeColumnItem;
            node.Header = GetNodeHeader(title, DatabaseObjectCategory.Column);
            collection.Add(node);
            return node;
        }
        private RadTreeViewItem AddFolderNode(ItemCollection collection, string title)
        {
            var node = new RadTreeViewItem();
            //node.DataContext = context;
            node.Header = GetNodeHeader(title, DatabaseObjectCategory.Folder);
            collection.Add(node);
            return node;
        }
        private FrameworkElement GetNodeHeader(string title, DatabaseObjectCategory type)
        {
            StackPanel pnlHeader = new StackPanel();
            System.Windows.Controls.TextBlock label = new System.Windows.Controls.TextBlock();
            label.Text = title;
            Image img = new Image();
            img.Width = 15;
            Uri uriSource = null;
            if (type == DatabaseObjectCategory.Folder)
            {
                uriSource = new Uri("Images/folder.png", UriKind.Relative);
            }
            else if (type == DatabaseObjectCategory.Column)
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





        //private RadTreeViewItem FindTreeDBObject(ItemCollection collection, string objectIdentity, string objectCategory)
        //{

        //    foreach (var item in collection)
        //    {
        //        if ((item is RadTreeViewItem))
        //        {
        //            var objectDTO = (item as RadTreeViewItem).DataContext as ObjectDTO;
        //            if (objectDTO.ObjectIdentity == objectIdentity && objectDTO.ObjectCategory == objectCategory)
        //                return (item as RadTreeViewItem);
        //            else
        //            {
        //                var result = FindTreeDBObject((item as RadTreeViewItem).Items, objectIdentity, objectCategory);
        //                if (result != null)
        //                    return result;
        //            }
        //        }

        //    }
        //    return null;
        //}




    }
    public class TreeColumnItem
    {
        public int ColumnID { set; get; }
        public string ColumnName { set; get; }
        public string Alias { set; get; }
        public string RelationshipPath { set; get; }
        public string EntityPath { set; get; }
    }
    public class TreeRelationshipItem
    {
        public RelationshipDTO Relationship { set; get; }
        public string RelationshipPath { set; get; }
        public string EntityPath { set; get; }
    }
    //public class TreeColumnSelectedArg : EventArgs
    //{
    //    public int ColumnID { set; get; }
    //    public string ColumnName { set; get; }
    //}
}
