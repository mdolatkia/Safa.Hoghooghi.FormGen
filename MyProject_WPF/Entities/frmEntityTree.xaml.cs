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
    public partial class frmEntityTree : UserControl
    {
        BizTableDrivedEntity bizEntity = new BizTableDrivedEntity();
        TableDrivedEntityDTO entity { set; get; }
        public frmEntityTree(int entityId)
        {
            InitializeComponent();
            entity = bizEntity.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), entityId, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithRelationships);
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                PopulateEntityTree();
        }
        public ObjectDTO SelectedItem
        {
            get { return treeEntity.SelectedItem as ObjectDTO; }
        }
        List<RadTreeViewItem> _AllItems = new List<RadTreeViewItem>();
        public List<RadTreeViewItem> GetAllTreeItems
        {
            get { return _AllItems; }
        }
        private void PopulateEntityTree()
        {
            var parentNode = GetNavigationRootNode();
            var f1 = AddEntityObjectNode(parentNode.Items, DatabaseObjectCategory.Folder, 0, "ستونها و روابط صریح");
            foreach (var column in entity.Columns.OrderBy(x => x.Position).ThenBy(x => x.Alias).ThenBy(x => x.Name))
            {
                var columnName = string.IsNullOrEmpty(column.Alias) ? column.Name : column.Alias;
                if (entity.Relationships.Any(x => x.RelationshipColumns.Any(y => column.ID == y.FirstSideColumnID) &&
                    (x.TypeEnum == Enum_RelationshipType.ManyToOne
                    || x.TypeEnum == Enum_RelationshipType.ExplicitOneToOne
                    || x.TypeEnum == Enum_RelationshipType.SubToSuper
                    || x.TypeEnum == Enum_RelationshipType.UnionToSubUnion)))
                {
                    //ارتباط دو ستونی؟؟؟
                    var relationship = entity.Relationships.First(x => x.RelationshipColumns.Any(y => column.ID == y.FirstSideColumnID) &&
                       (x.TypeEnum == Enum_RelationshipType.ManyToOne
                       || x.TypeEnum == Enum_RelationshipType.ExplicitOneToOne
                       || x.TypeEnum == Enum_RelationshipType.SubToSuper
                     || x.TypeEnum == Enum_RelationshipType.UnionToSubUnion));
                    AddEntityObjectNode(f1.Items, DatabaseObjectCategory.Relationship, relationship.ID, columnName + " > " + " ارتباط با" + (string.IsNullOrEmpty(relationship.Alias) ? relationship.Entity2 : relationship.Alias), relationship.Name);
                }
                else
                    AddEntityObjectNode(f1.Items, DatabaseObjectCategory.Column, column.ID, columnName);
            }
            var f2 = AddEntityObjectNode(parentNode.Items, DatabaseObjectCategory.Folder, 0, "روابط ضمنی");
            foreach (var relationship in entity.Relationships.Where(x =>
                        (x.TypeEnum == Enum_RelationshipType.OneToMany
                        || x.TypeEnum == Enum_RelationshipType.ImplicitOneToOne
                        || x.TypeEnum == Enum_RelationshipType.SuperToSub
                        || x.TypeEnum == Enum_RelationshipType.SubUnionToUnion)))
            {
                AddEntityObjectNode(f2.Items, DatabaseObjectCategory.Relationship, relationship.ID, " ارتباط با" + (string.IsNullOrEmpty(relationship.Alias) ? relationship.Entity2 : relationship.Alias), relationship.Name);
            }

        }
        private RadTreeViewItem GetNavigationRootNode()
        {
            if (treeEntity.Items.Count > 0)
            {
                return treeEntity.Items[0] as RadTreeViewItem;
            }
            else
            {
                var rootNode = AddEntityObjectNode(treeEntity.Items, DatabaseObjectCategory.Entity, 0, (string.IsNullOrEmpty(entity.Alias) ? entity.Name : entity.Alias));
                return rootNode;
            }
        }
        private RadTreeViewItem AddEntityObjectNode(ItemCollection collection, DatabaseObjectCategory objectCategory, int objectIdentity, string title, string tooltip = null)
        {
            var node = new RadTreeViewItem();
            var context = new ObjectDTO();
            context.ObjectCategory = objectCategory;
            context.Title = title;
            context.ObjectIdentity = objectIdentity;
            node.DataContext = context;
            _AllItems.Add(node);
            node.Header = GetNodeHeader(context.Title, context.ObjectCategory);
            node.ToolTip = tooltip;
            //   node.Selected += node_Selected;
            collection.Add(node);
            return node;
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
            else if (type == DatabaseObjectCategory.Entity)
            {
                uriSource = new Uri("Images/form.png", UriKind.Relative);
            }
            else if (type == DatabaseObjectCategory.Column)
            {
                uriSource = new Uri("Images/column.png", UriKind.Relative);
            }
            else if (type == DatabaseObjectCategory.Relationship)
            {
                uriSource = new Uri("Images/relationship.png", UriKind.Relative);
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
