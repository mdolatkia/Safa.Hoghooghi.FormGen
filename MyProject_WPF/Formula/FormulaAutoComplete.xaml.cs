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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ModelEntites;
using Telerik.Windows.Controls;
using MyFormulaFunctionStateFunctionLibrary;
using System.Reflection;
using ProxyLibrary;
using MyModelManager;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for FormulaAutoComplete.xaml
    /// </summary>
    public partial class FormulaAutoComplete : UserControl
    {
        public event EventHandler<NodeSelectedArg> NodeSelected;
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        DR_Requester Requester { set; get; }
        TableDrivedEntityDTO MainEntity { set; get; }
        List<EntityAndProperties> EntityAndProperties = new List<EntityAndProperties>();
        public FormulaAutoComplete(DR_Requester requester, TableDrivedEntityDTO mainEntity)
        {
            InitializeComponent();
            treeCurrentEntity.IsLineEnabled = true;
            treeMainEntity.IsLineEnabled = true;
            Requester = requester;
            MainEntity = mainEntity;
            var entityAndProperties = GetEntityAndProperties(MainEntity);
            SetTree(treeMainEntity.Items, entityAndProperties);
        }

        public bool SetCurrentList(string entityName)
        {
            var entityAndProperties = GetEntityAndProperties(entityName);
            if (entityAndProperties != null)
            {
                SetTree(treeCurrentEntity.Items, entityAndProperties);
                tabCurrentEntity.Header = entityName;
                return true;
            }
            else
                return false;
        }
        public void SetCurrentList(Type type)
        {
            SetTree(treeCurrentEntity.Items, type);
            tabCurrentEntity.Header = type.Name;
        }
        public void SetTreeVisiblities(bool mainEntity, bool currentList)
        {
            tabMainEntity.Visibility = mainEntity ? Visibility.Visible : Visibility.Collapsed;
            if (currentList)
            {
                tabCurrentEntity.Visibility = Visibility.Visible;
                tabCurrentEntity.IsSelected = true;
            }
            else
            {
                tabCurrentEntity.Visibility = Visibility.Collapsed;
                tabMainEntity.IsSelected = true;
            }
        }
            EntityAndProperties GetEntityAndProperties(string entityName)
            {
                if (!EntityAndProperties.Any(x => x.Entity.Name.ToLower() == entityName.ToLower()))
                {
                    var entity = bizTableDrivedEntity.GetPermissionedEntityByName(MyProjectManager.GetMyProjectManager.GetRequester(), MainEntity.DatabaseID, entityName);
                    if (entity != null)
                    {
                        return GetEntityAndProperties(entity);
                    }
                    else
                        return null;
                }
                else
                {
                    return EntityAndProperties.First(x => x.Entity.Name == entityName);
                }
            }
            EntityAndProperties GetEntityAndProperties(int entityID)
            {
                if (!EntityAndProperties.Any(x => x.Entity.ID == entityID))
                {
                    var entity = bizTableDrivedEntity.GetPermissionedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), entityID);
                    if (entity != null)
                    {
                        return GetEntityAndProperties(entity);
                    }
                    else
                        return null;
                }
                else
                {
                    return EntityAndProperties.First(x => x.Entity.ID == entityID);
                }
            }
            EntityAndProperties GetEntityAndProperties(TableDrivedEntityDTO entity)
            {
                EntityAndProperties entityAndProperties = null;
                if (!EntityAndProperties.Any(x => x.Entity.ID == entity.ID))
                {
                    var properties = FormulaInstanceInternalHelper.GetProperties(entity, null, true).Select(x => x.Value).ToList();
                    entityAndProperties = new EntityAndProperties() { Entity = entity, Properties = properties };
                    EntityAndProperties.Add(entityAndProperties);
                }
                else
                {
                    entityAndProperties = EntityAndProperties.First(x => x.Entity.ID == entity.ID);
                }
                return entityAndProperties;
            }
        private void SetTree(ItemCollection items, EntityAndProperties entityAndProperties)
        {
            items.Clear();
            foreach (var property in entityAndProperties.Properties)
            {
                NodeContext nodeContext = new MyProject_WPF.NodeContext();
                nodeContext.Name = property.Name;
                nodeContext.Context = property;
                nodeContext.NodeType = NodeType.Property;
                AddNode(items, nodeContext);
            }
        }
        private void AddNode(ItemCollection items, NodeContext nodeContext)
        {
            RadTreeViewItem node = new RadTreeViewItem();
            node.DataContext = nodeContext;
            node.Header = GetHeader(nodeContext.Name, nodeContext.NodeType);
            node.Expanded += Node_Expanded;
            node.DoubleClick += Node_DoubleClick;

            if (nodeContext.Context is MyPropertyInfo)
            {
                var myPropertyInfo = (nodeContext.Context as MyPropertyInfo);
                if (myPropertyInfo.PropertyType == ProxyLibrary.PropertyType.Helper)
                {
                    node.Items.Add("aaa");
                }
                else if (myPropertyInfo.PropertyType == ProxyLibrary.PropertyType.Relationship)
                {
                    node.Items.Add("aaa");
                }
            }
            items.Add(node);
        }

        private void Node_DoubleClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            e.Handled = true;
            var node = sender as RadTreeViewItem;
            var nodeContext = node.DataContext as NodeContext;
            var path = GetNodePath(node);
            if (NodeSelected != null)
                NodeSelected(sender, new NodeSelectedArg() { NodeName = nodeContext.Name, NodePath = path, PropertyType = nodeContext.NodeType });
        }


        private string GetNodePath(RadTreeViewItem node, string currentPath = "")
        {
            if (currentPath == "")
                currentPath = (node.DataContext as NodeContext).Name;
            if (node.ParentItem != null)
            {
                currentPath = (node.ParentItem.DataContext as NodeContext).Name + "." + currentPath;
                return GetNodePath(node.ParentItem, currentPath);
            }
            else
            {
                return currentPath;
            }
        }

        private void Node_Expanded(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (sender is RadTreeViewItem)
            {
                var node = (sender as RadTreeViewItem);
                var nodeContext = (node.DataContext as NodeContext);
                if (node.Items.Count == 1 && node.Items[0].ToString() == "aaa")
                {
                    node.Items.Clear();
                    if (nodeContext.Context is MyPropertyInfo)
                    {
                        if ((nodeContext.Context as MyPropertyInfo).PropertyType == ProxyLibrary.PropertyType.Helper)
                        {
                            SetTree(node.Items, (nodeContext.Context as MyPropertyInfo).Type);
                        }
                        else if ((nodeContext.Context as MyPropertyInfo).PropertyType == ProxyLibrary.PropertyType.Relationship)
                        {
                            var entityAndProperties = GetEntityAndProperties((nodeContext.Context as MyPropertyInfo).PropertyRelationship.EntityID2);
                            if (entityAndProperties != null)
                            {
                                SetTree(node.Items, entityAndProperties);
                            }
                        }
                    }
                }
            }
        }

        private void SetTree(ItemCollection items, Type type)
        {
            foreach (var prop in type.GetProperties())
            {
                NodeContext nodeContext = new MyProject_WPF.NodeContext();
                nodeContext.Name = prop.Name;
                nodeContext.NodeType = NodeType.Property;
                nodeContext.Context = prop;
                AddNode(items, nodeContext);
            }

            foreach (var method in type.GetMethods())
            {
                var methodParamStr = "";
                var paramList = method.GetParameters();
                var methodName = method.Name;
                var paramsStr = "";
                if (paramList.Count() > 0)
                {
                    foreach (var param in paramList)
                    {
                        if (type == typeof(Enumerable) && param.Name.ToLower() == "source")
                            continue;
                        paramsStr += (paramsStr == "" ? "" : ",") + param.ParameterType.Name + " " + param.Name;
                    }
                    methodParamStr += "(" + paramsStr + ")";
                }
                else
                    methodParamStr += "()";


                NodeContext nodeContext = new MyProject_WPF.NodeContext();
                nodeContext.Name = methodName + methodParamStr;
                nodeContext.NodeType = NodeType.Method;
                nodeContext.Context = method;
                AddNode(items, nodeContext);
            }
        }


        private object GetHeader(string title, NodeType propertyType)
        {
            StackPanel pnl = new StackPanel();
            pnl.Orientation = Orientation.Horizontal;
            TextBlock lbl = new TextBlock();
            lbl.Text = title;
            Image img = new Image();
            img.Source = GetPropertyImage(propertyType);
            img.Width = 15;
            pnl.Children.Add(img);
            pnl.Children.Add(lbl);
            return pnl;
        }

        private ImageSource GetPropertyImage(NodeType propertyType)
        {
            if (propertyType == NodeType.Property)
            {
                return new BitmapImage(new Uri(@"/MyProject_WPF;component/Images/property.png", UriKind.Relative));
            }
            else if (propertyType == NodeType.Method)
            {
                return new BitmapImage(new Uri(@"/MyProject_WPF;component/Images/method.png", UriKind.Relative));
            }
            else if (propertyType == NodeType.RelationshipProperty)
            {
                return new BitmapImage(new Uri(@"/MyProject_WPF;component/Images/relationship1.png", UriKind.Relative));
            }

            return null;
        }
    }

    public class EntityAndProperties
    {
        public EntityAndProperties()
        {
            Properties = new List<MyFormulaFunctionStateFunctionLibrary.MyPropertyInfo>();
        }
        public TableDrivedEntityDTO Entity { set; get; }
        public List<MyPropertyInfo> Properties { set; get; }
    }
    public class NodeContext
    {
        public object Context { set; get; }
        public string Name { set; get; }
        public NodeType NodeType { set; get; }
    }
    public enum NodeType
    {
        Property,
        RelationshipProperty,
        Method,
        HelperProperty
    }
    public class NodeSelectedArg
    {
        public NodeType PropertyType { set; get; }
        public string NodeName { set; get; }
        public string NodePath { set; get; }
    }
}
