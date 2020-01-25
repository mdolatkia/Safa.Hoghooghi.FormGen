using DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyProject
{
    public partial class frmNavigationTree : Form
    {
        public frmNavigationTree()
        {
            InitializeComponent();

            PopulateDBObjectTree();
            PopulateNavigationTree();


            this.treeDBObjects.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView_ItemDrag);
            this.treeNavigation.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView_DragEnter);
            this.treeNavigation.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView_DragDrop);



        }
        private void treeView_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Copy);
        }
        private void treeView_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
        private void treeView_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            TreeNode sourceNode;

            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
                TreeNode DestinationNode = ((TreeView)sender).GetNodeAt(pt);
                sourceNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");

                DestinationNode.Nodes.Add((TreeNode)sourceNode.Clone());
                DestinationNode.Expand();
                SetNavigationTreeIcons(treeNavigation.Nodes);
                //Remove Original Node
                //NewNode.Remove();

            }
        }

        private void SetNavigationTreeIcons(TreeNodeCollection treeNodeCollection)
        {
            foreach (TreeNode node in treeNodeCollection)
            {
                SetNodeImage(node);
                SetNavigationTreeIcons(node.Nodes);
            }
        }

        private void SetNodeImage(TreeNode node)
        {
            if (node.Tag != null)
            {
                if (node.Tag.ToString() == "Database")
                {
                    node.ImageIndex = 0;
                }
                else if (node.Tag.ToString() == "Entity")
                {
                    node.ImageIndex = 2;
                }
                else
                {
                    node.ImageIndex = 1;
                }
            }
        }


        private void PopulateNavigationTree()
        {
            TreeNode rootNode = new TreeNode { Text = "ریشه", Tag = "Root" };
            treeNavigation.Nodes.Add(rootNode);
            using (var myProjectContext = new MyProjectEntities())
            {
                foreach (var item in myProjectContext.NavigationTree.Where(x => x.ParentID == null))
                {
                    var node = new TreeNode() { Text = item.ItemName, Tag = item.Category };
                    rootNode.Nodes.Add(rootNode);
                    PopulateNavigationTree(item.ID, node.Nodes, myProjectContext);
                }
            }
        }
        private void PopulateNavigationTree(int parentID, TreeNodeCollection collection, MyProjectEntities myProjectContext)
        {
            foreach (var item in myProjectContext.NavigationTree.Where(x => x.ParentID == parentID))
            {
                var node = new TreeNode() { Text = item.ItemName, Tag = item.Category };
                PopulateNavigationTree(item.ID, node.Nodes, myProjectContext);
            }
        }
        private void PopulateDBObjectTree()
        {
            using (var myProjectContext = new MyProjectEntities())
            {
                foreach (var database in myProjectContext.DatabaseInformation)
                {
                    TreeNode dbNode = new TreeNode { Text = database.Name, Tag = "Database" };
                    SetNodeImage(dbNode);
                    treeDBObjects.Nodes.Add(dbNode);
                    foreach (var schema in myProjectContext.TableDrivedEntity.Where(x => x.Table.Catalog == database.Name).GroupBy(x => x.Table.RelatedSchema))
                    {
                        var schemaName = "";
                        if (string.IsNullOrEmpty(schema.Key))
                            schemaName = "Default Schema";
                        else
                            schemaName = schema.Key;
                        TreeNode schemaNode = new TreeNode { Text = schemaName, Tag = "Schema" };
                        dbNode.Nodes.Add(schemaNode);
                        SetNodeImage(schemaNode);
                        foreach (var entity in schema)
                        {
                            TreeNode tableNode = new TreeNode { Text = entity.Name, Tag = "Entity" };
                            SetNodeImage(tableNode);
                            schemaNode.Nodes.Add(tableNode);
                        }
                    }
                }
            }

        }




    }
}
