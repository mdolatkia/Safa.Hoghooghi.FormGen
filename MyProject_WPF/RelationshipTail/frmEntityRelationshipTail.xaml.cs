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
using Telerik.Windows.Controls.GridView;
using Telerik.Windows;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmFormulaTree.xaml
    /// </summary>
    /// 

    public partial class frmEntityRelationshipTail : UserControl
    {
        public event EventHandler<EntityRelationshipTailSelectedArg> ItemSelected;
        BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        int EntityID { set; get; }
        public frmEntityRelationshipTail(int entityID)
        {
            InitializeComponent();
            EntityID = entityID;
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                PopulateTree();
        }


        private void PopulateTree()
        {
            if (EntityID != 0)
            {

                var entity = bizTableDrivedEntity.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithRelationships);
                var rootNode = AddRootNode(treeItems.Items, (string.IsNullOrEmpty(entity.Alias) ? entity.Name : entity.Alias));
                PopulateTreeItems(rootNode, entity);
            }
        }
        private RadTreeViewItem AddRootNode(ItemCollection collection, string title)
        {
            var node = new RadTreeViewItem();
            node.Header = GetNodeHeader(title, "Folder");
            collection.Add(node);
            return node;
        }

        private void PopulateTreeItems(RadTreeViewItem parentNode, TableDrivedEntityDTO entity)
        {


            //var columnsNode = AddFormulaObjectNode(items, "Folder", null, "", "ستونها");
            //var relatoinshipNode = AddFormulaObjectNode(items, "Folder", null, "", "روابط");
            //var parametersNode = AddFormulaObjectNode(items, "Folder", null, "", "پارامترها");
            //var functionNode = AddFormulaObjectNode(items, "Folder", null, "", "Stored Procedures");


            foreach (var relationship in entity.Relationships)
            {
                if (parentNode.DataContext != null)
                {
                    if ((parentNode.DataContext as RelationshipDTO).PairRelationshipID == relationship.ID)
                        continue;
                }
                var node = AddRelationshipNode(parentNode.Items, relationship);
                node.Expanded += Node_Expanded;
                node.Items.Add("Loading...");
            }

            //AddColumnNodes(columnsNode.Items, entityID);
            //AddRalationshipNodes(relatoinshipNode.Items, entityID);
            //AddParameterNodes(parametersNode.Items, entityID);
            //AddFunctionNodes(functionNode.Items, entityID);

            //RadContextMenu.SetContextMenu(parametersNode, GetParametersContextMenu(entityID, parametersNode));
            //RadContextMenu.SetContextMenu(functionNode, GetFunctionsContextMenu(entityID, functionNode));

        }
        private RadTreeViewItem AddRelationshipNode(ItemCollection collection, RelationshipDTO relationship)
        {
            var node = new RadTreeViewItem();
            node.DataContext = relationship;
            node.Header = GetNodeHeader(relationship.Entity2, "Relationship");
            node.ToolTip = relationship.ID + (string.IsNullOrEmpty(relationship.Alias) ? "" : Environment.NewLine + relationship.Alias) + (string.IsNullOrEmpty(relationship.Name) ? "" : Environment.NewLine + relationship.Name);
            collection.Add(node);
            return node;
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
                    var relationshipDTO = treeItem.DataContext as RelationshipDTO;
                    var entity = bizTableDrivedEntity.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), relationshipDTO.EntityID2, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithRelationships);
                    PopulateTreeItems(treeItem, entity);
                }
            }
        }

        private FrameworkElement GetNodeHeader(string title, string type)
        {
            StackPanel pnlHeader = new StackPanel();
            System.Windows.Controls.TextBlock label = new System.Windows.Controls.TextBlock();
            label.Text = title;
            Image img = new Image();
            img.Width = 15;
            Uri uriSource = null;
            if (type.ToString() == "Folder")
            {
                uriSource = new Uri("Images/folder.png", UriKind.Relative);
            }
            else if (type.ToString() == "Entity")
            {
                uriSource = new Uri("Images/form.png", UriKind.Relative);
            }
            else if (type.ToString() == "Column")
            {
                uriSource = new Uri("Images/column.png", UriKind.Relative);
            }
            else if (type.ToString() == "Relationship")
            {
                uriSource = new Uri("Images/relationship.png", UriKind.Relative);
            }
            if (uriSource != null)
                img.Source = new BitmapImage(uriSource);
            pnlHeader.Orientation = Orientation.Horizontal;
            pnlHeader.Children.Add(new CheckBox());
            pnlHeader.Children.Add(img);
            pnlHeader.Children.Add(label);
            return pnlHeader;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            EntityRelationshipTailDTO dto = new EntityRelationshipTailDTO();
            List<RelationshipDTO> listRelationships = new List<RelationshipDTO>();
            var relationshipFound = GetCheckedNode(listRelationships);
            if (relationshipFound)
            {
                //var targetEntityID = listRelationships.First().EntityID2;
                //    dto = ConvertRelationshipPath(listRelationships);

                var result = bizEntityRelationshipTail.GetOrCreateEntityRelationshipTailID(EntityID, GetPath(listRelationships));
                if (ItemSelected != null)
                    ItemSelected(this, new EntityRelationshipTailSelectedArg() { EntityRelationshipTailID = result });
                MessageBox.Show("ثبت انجام شد");
            }
        }

        private string GetPath(List<RelationshipDTO> listRelationships)
        {
            string path = "";
            foreach (var item in listRelationships)
            {
                path = item.ID + (path == "" ? "" : ",") + path;
            }
            //if (listRelationships.Count > 0)
            //{
            //    var last = listRelationships.Last();
            //    EntityRelationshipTailDTO item = new EntityRelationshipTailDTO();
            //    item.Relationship = new RelationshipDTO();
            //    item.RelationshipID = last.ID;
            //    listRelationships.Remove(last);
            //    item.ChildTail = ConvertRelationshipPath(listRelationships);
            //    return item;
            //}
            return path;
        }

        private bool GetCheckedNode(List<RelationshipDTO> listRelationships, ItemCollection collection = null)
        {
            if (listRelationships == null)
                listRelationships = new List<RelationshipDTO>();
            if (collection == null)
                collection = treeItems.Items;
            foreach (var item in collection)
            {
                if (item is RadTreeViewItem)
                {
                    var node = (item as RadTreeViewItem);
                    if (node.Header is StackPanel)
                    {
                        foreach (var control in (node.Header as StackPanel).Children)
                        {
                            if (control is CheckBox)
                                if ((control as CheckBox).IsChecked == true)
                                {
                                    if (node.DataContext != null)
                                        listRelationships.Add(node.DataContext as RelationshipDTO);
                                    return true;
                                }
                        }
                    }

                    var result = GetCheckedNode(listRelationships, node.Items);
                    if (result)
                    {
                        if (node.DataContext != null)
                            listRelationships.Add(node.DataContext as RelationshipDTO);
                        return true;
                    }
                }
            }
            return false;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //MyProjectManager.GetMyProjectManager.clo(frm, "نوع نقش");
            //MyProjectManager.GetMyProjectManager.CloseDialog(this);
        }
    }


    public class EntityRelationshipTailSelectedArg : EventArgs
    {
        public int EntityRelationshipTailID { set; get; }
        //public BaseFormulaParameter Parameter { set; get; }
        //public string ParameterName { set; get; }
        //public string ParameterTitle { set; get; }
        //public Type ParameterType { set; get; }
    }
}
