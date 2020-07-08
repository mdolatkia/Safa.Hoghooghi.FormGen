using ModelEntites;
using MyFormulaFunctionStateFunctionLibrary;
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

    public partial class frmFormulaTree : UserControl
    {
        //public event EventHandler<TreeFormulaParameterSelectedArg> ItemSelected;
        //FormulaIntention FormulaIntention { set; get; }
        BizFormula bizFormula = new BizFormula();
        BizDatabaseFunction bizDatabaseFunction = new BizDatabaseFunction();
        BizColumn bizColumn = new BizColumn();
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        BizCodeFunction bizCodeFunction = new BizCodeFunction();
        //FormulaHelper formulaHelper = new FormulaHelper();
        int EntityID { set; get; }
        TableDrivedEntityDTO Entity { get; set; }

        public frmFormulaTree(int entityID)
        {
            InitializeComponent();
            EntityID = entityID;
            Entity = bizTableDrivedEntity.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), entityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithRelationships);
            //treeItems.SelectedItemChanged += TreeItems_SelectedItemChanged;
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                PopulateTree();
        }

        //private void TreeItems_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        //{
        //    e.Handled = true;
        //    if (treeItems.SelectedItem != null)
        //    {
        //        CheckSelectedItem(treeItems.SelectedItem as RadTreeViewItem);
        //    }
        //}

        //private void CheckSelectedItem(RadTreeViewItem treeViewItem)
        //{
        //    if (ItemSelected != null)
        //    {
        //        TreeFormulaParameterSelectedArg arg = new TreeFormulaParameterSelectedArg(); ;
        //        var selectedItem = treeViewItem.DataContext;
        //        if (selectedItem is ColumnDTO)
        //        {
        //            var column = (selectedItem as ColumnDTO);

        //            var columnFormulaParameter = new ColumnFormulaParameter();
        //            columnFormulaParameter.ColumnID = column.ID;
        //            columnFormulaParameter.FormulaParameterFullPath = GetColumnParameterName(treeViewItem, column, "");
        //            columnFormulaParameter.ParameterTitle = column.Alias;
        //            columnFormulaParameter.ParameterType = bizColumn.GetColumnDotNetType(column);
        //            arg.Parameter = columnFormulaParameter;
        //        }
        //        else if (selectedItem is FormulaParameterDTO)
        //        {
        //            var paremeter = (selectedItem as FormulaParameterDTO);

        //            var existingFormulaParameter = new ExistingFormulaParameter();
        //            existingFormulaParameter.FormulaParameterID = paremeter.ID;
        //            existingFormulaParameter.FormulaParameterFullPath = GetParameterName(treeViewItem, paremeter, "");
        //            existingFormulaParameter.ParameterTitle = paremeter.Title;
        //            existingFormulaParameter.ParameterType = bizFormula.GetFormulaDotNetType(paremeter.FormulaID);
        //            arg.Parameter = existingFormulaParameter;
        //        }
        //        else if (selectedItem is RelationshipDTO)
        //        {
        //            var relationship = (selectedItem as RelationshipDTO);

        //            var relationshipFormulaParameter = new RelationshipFormulaParameter();
        //            relationshipFormulaParameter.FormulaParameterFullPath = GetRelaionshipName(treeViewItem, relationship, "");
        //            relationshipFormulaParameter.ParameterTitle = relationship.Entity2;
        //            relationshipFormulaParameter.ParameterType = null;
        //            arg.Parameter = relationshipFormulaParameter;
        //        }
        //        //if (arg.Parameter.ParameterName.EndsWith("." + singleName))
        //        //    arg.Parameter.FormulaParameterPath = arg.Parameter.ParameterName.Replace("." + singleName, "");
        //        //else
        //        //    arg.Parameter.FormulaParameterPath = "";
        //        ItemSelected(this, arg);
        //    }
        //}

        //private string GetColumnParameterName(RadTreeViewItem treeViewItem, ColumnDTO column, string result = "")
        //{
        //    if (treeViewItem.Parent is RadTreeViewItem)
        //    {
        //        if ((treeViewItem.Parent as RadTreeViewItem).Parent == treeItems.Items[0])
        //            return result + (result == "" ? "" : ".") + column.Name;
        //        else if ((treeViewItem.Parent as RadTreeViewItem).DataContext is RelationshipDTO)
        //        {
        //            var relationship = (treeViewItem.Parent as RadTreeViewItem).DataContext as RelationshipDTO;
        //            result = formulaHelper.GetRelationshipIdentifier(relationship) + (result == "" ? "" : ".") + result;
        //            return GetColumnParameterName(treeViewItem.Parent as RadTreeViewItem, column, result);
        //        }
        //        else
        //            return GetColumnParameterName(treeViewItem.Parent as RadTreeViewItem, column, result);
        //    }
        //    return "";

        //}
        //private string GetParameterName(RadTreeViewItem treeViewItem, FormulaParameterDTO parameter, string result = "")
        //{
        //    if (treeViewItem.Parent is RadTreeViewItem)
        //    {
        //        if ((treeViewItem.Parent as RadTreeViewItem).Parent == treeItems.Items[0])
        //            return result + (result == "" ? "" : ".") + parameter.Name;
        //        else if ((treeViewItem.Parent as RadTreeViewItem).DataContext is RelationshipDTO)
        //        {
        //            var relationship = (treeViewItem.Parent as RadTreeViewItem).DataContext as RelationshipDTO;
        //            result = formulaHelper.GetRelationshipIdentifier(relationship) + (result == "" ? "" : ".") + result;
        //            return GetParameterName(treeViewItem.Parent as RadTreeViewItem, parameter, result);
        //        }
        //        else
        //            return GetParameterName(treeViewItem.Parent as RadTreeViewItem, parameter, result);
        //    }
        //    return "";

        //}


        //private string GetRelaionshipName(RadTreeViewItem treeViewItem,RelationshipDTO relationship, string result = "")
        //{
        //    if (treeViewItem.Parent is RadTreeViewItem)
        //    {
        //        if ((treeViewItem.Parent as RadTreeViewItem).Parent == null)
        //            return result;
        //        else if ((treeViewItem as RadTreeViewItem).DataContext is RelationshipDTO)
        //        {
        //            var currentRelationship = (treeViewItem as RadTreeViewItem).DataContext as RelationshipDTO;
        //            result = formulaHelper.GetRelationshipIdentifier(currentRelationship) + (result == "" ? "" : ".") + result;
        //            return GetRelaionshipName(treeViewItem.Parent as RadTreeViewItem, relationship, result);
        //        }
        //        else
        //            return GetRelaionshipName(treeViewItem.Parent as RadTreeViewItem, relationship, result);
        //    }
        //    return result;

        //}




        private void PopulateTree()
        {
            if (EntityID != 0)
            {
                BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
                var entity = bizTableDrivedEntity.GetTableDrivedEntity(MyProjectManager.GetMyProjectManager.GetRequester(), EntityID, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships);
                var rootNode = AddFormulaObjectNode(treeItems.Items, DatabaseObjectCategory.Entity, 0, (string.IsNullOrEmpty(entity.Alias) ? entity.Name : entity.Alias));
                PopulateTreeItems(rootNode.Items, EntityID);
                rootNode.IsExpanded = true;
            }
        }


        private void PopulateTreeItems(ItemCollection items, int entityID)
        {
            var columnsNode = AddFormulaObjectNode(items, DatabaseObjectCategory.Folder, 0, "ستونها");
            var relatoinshipNode = AddFormulaObjectNode(items, DatabaseObjectCategory.Folder, 0, "روابط");
            var parametersNode = AddFormulaObjectNode(items, DatabaseObjectCategory.Folder, 0, "پارامترها");
            var databaseFunctionNode = AddFormulaObjectNode(items, DatabaseObjectCategory.Folder, 0, "Stored Procedures");
            var codeFunctionNode = AddFormulaObjectNode(items, DatabaseObjectCategory.Folder, 0, "کد/تابع");

            AddColumnNodes(columnsNode.Items, entityID);
            AddRalationshipNodes(relatoinshipNode.Items, entityID);
            AddParameterNodes(parametersNode.Items, entityID);
            //AddDatabaseFunctionNodes(databaseFunctionNode.Items, entityID);
            AddCodeFunctionNodes(codeFunctionNode.Items, entityID);

            RadContextMenu.SetContextMenu(parametersNode, GetParametersContextMenu(entityID, parametersNode));
            //RadContextMenu.SetContextMenu(databaseFunctionNode, GetDatabaseFunctionsContextMenu(entityID, databaseFunctionNode));
            RadContextMenu.SetContextMenu(codeFunctionNode, GetCodeFunctionsContextMenu(entityID, codeFunctionNode));
        }


        private RadContextMenu GetParametersContextMenu(int entityID, RadTreeViewItem parametersNode)
        {
            RadContextMenu menu = new RadContextMenu();
            RadMenuItem item = new RadMenuItem();
            item.Header = "تعریف پارامتر";
            item.Click += (sender, e) => item_Click(sender, e, entityID, parametersNode);
            menu.Items.Add(item);

            return menu;
        }

        void item_Click(object sender, RoutedEventArgs e, int entityID, RadTreeViewItem parametersNode)
        {


            //var formulaParameterIntention = new FormulaParameterIntention();
            //formulaParameterIntention.Type = Enum_FormulaParameterIntention.FormulaParameterForTable;
            //formulaParameterIntention.EntityID = entityID;
            frmFormula view = new frmFormula(0, entityID);
            view.FormulaUpdated += (sender1, e1) => view_FormulaParameterUpdated(sender1, e1, entityID, parametersNode.Items);
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "Form", Enum_WindowSize.Maximized);
        }



        void view_FormulaParameterUpdated(object sender, FormulaSelectedArg e, int entityID, ItemCollection collection)
        {
            AddParameterNodes(collection, entityID);
        }

        private void AddColumnNodes(ItemCollection items, int entityID)
        {

            //var columns = bizColumn.GetColumns(entityID, true);
            foreach (var column in Entity.Columns)
            {
                AddColumnNode(items, column);
            }
        }
        private void AddParameterNodes(ItemCollection items, int entityID)
        {
            items.Clear();
            var listFormulas = bizFormula.GetFormulas(entityID);
            foreach (var formula in listFormulas)
            {
                AddParameterNode(items, formula);
            }

        }

        private RadTreeViewItem AddParameterNode(ItemCollection collection, FormulaDTO formula)
        {
            var node = GetNode();
            node.DataContext = formula;
            node.Header = GetNodeHeader(formula.Name, DatabaseObjectCategory.Formula);
            node.ToolTip = formula.Title;
            collection.Add(node);
            _AllItems.Add(node);
            RadContextMenu.SetContextMenu(node, GetParameterContextMenu(collection, formula.EntityID, formula.ID));
            return node;
        }

        private RadTreeViewItem GetNode()
        {
            var node = new RadTreeViewItem();
            node.MouseRightButtonDown += Node_MouseRightButtonDown;
            return node;
        }

        private void Node_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var treeViewitem = sender as RadTreeViewItem;
            treeViewitem.IsSelected = true;
            e.Handled = true;
        }

        private RadContextMenu GetParameterContextMenu(ItemCollection collection, int entityID, int parameterID)
        {
            RadContextMenu menu = new RadContextMenu();
            RadMenuItem item = new RadMenuItem();
            item.Header = "اصلاح پارامتر";
            item.Click += (sender, e) => item_Click(sender, e, collection, entityID, parameterID);
            menu.Items.Add(item);

            return menu;
        }

        private void item_Click(object sender, RadRoutedEventArgs e, ItemCollection collection, int entityID, int parameterID)
        {

            frmFormula view = new frmFormula(parameterID, entityID);
            view.FormulaUpdated += (sender1, e1) => view_FormulaParameterUpdated(sender1, e1, entityID, collection);
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "فرمول", Enum_WindowSize.Maximized);
        }

        //private RadContextMenu GetDatabaseFunctionsContextMenu(int entityID, RadTreeViewItem databaseFunctionsNode)
        //{
        //    RadContextMenu menu = new RadContextMenu();
        //    RadMenuItem item = new RadMenuItem();
        //    item.Header = "تعریف فانکشن";
        //    item.Click += (sender, e) => item_ClickDatabaseFunction(sender, e, entityID, databaseFunctionsNode);
        //    menu.Items.Add(item);

        //    return menu;
        //}


        //void item_ClickDatabaseFunction(object sender, RoutedEventArgs e, int entityID, RadTreeViewItem databaseFunctionsNode)
        //{

        //    var DatabaseFunctionIntention = new DatabaseFunctionEntityIntention();
        //    DatabaseFunctionIntention.Type = Enum_DatabaseFunctionEntityIntention.DatabaseFunctionEntityDefinition;
        //    DatabaseFunctionIntention.EntityID = entityID;
        //    frmDatabaseFunction_Entity view = new frmDatabaseFunction_Entity(DatabaseFunctionIntention);
        //    view.DatabaseFunctionEntitySelected += (sender1, e1) => view_DatabaseFunctionSelected(sender1, e1, databaseFunctionsNode.Items);
        //    MyProjectManager.GetMyProjectManager.ShowDialog(view, "DatabaseFunction");
        //}
        //private void AddDatabaseFunctionNodes(ItemCollection items, int entityID)
        //{
        //    items.Clear();
        //    var listFunctoins = bizDatabaseFunction.GetDatabaseFunctionEntities(entityID);
        //    foreach (var databaseFunction in listFunctoins)
        //    {
        //        AddDatabaseFunctionNode(items, databaseFunction);
        //    }

        //}
        //private RadTreeViewItem AddDatabaseFunctionNode(ItemCollection collection, DatabaseFunction_EntityDTO databaseFunctionEntity)
        //{
        //    var node = GetNode();
        //    node.DataContext = databaseFunctionEntity;
        //    node.Header = GetNodeHeader(databaseFunctionEntity.DatabaseFunction.Title, DatabaseObjectCategory.DatabaseFunction);
        //    node.ToolTip = databaseFunctionEntity.DatabaseFunction.Title;
        //    collection.Add(node);
        //    _AllItems.Add(node);
        //    RadContextMenu.SetContextMenu(node, GetDatabaseFunctionContextMenu(collection, databaseFunctionEntity.ID));
        //    return node;
        //}

        //private RadContextMenu GetDatabaseFunctionContextMenu(ItemCollection collection, int databaseFunctionEntityID)
        //{
        //    RadContextMenu menu = new RadContextMenu();
        //    RadMenuItem item = new RadMenuItem();
        //    item.Header = "اصلاح فانکشن";
        //    item.Click += (sender, e) => item_ClickDatabaseFunction(sender, e, collection, databaseFunctionEntityID);
        //    menu.Items.Add(item);

        //    return menu;
        //}

        //private void item_ClickDatabaseFunction(object sender, RadRoutedEventArgs e, ItemCollection collection, int databaseFunctionEntityID)
        //{

        //    var DatabaseFunctionIntention = new DatabaseFunctionEntityIntention();
        //    DatabaseFunctionIntention.Type = Enum_DatabaseFunctionEntityIntention.DatabaseFunctionEntityEdit;
        //    DatabaseFunctionIntention.DatabaseFunctionEntityID = databaseFunctionEntityID;
        //    frmDatabaseFunction_Entity view = new frmDatabaseFunction_Entity(DatabaseFunctionIntention);
        //    view.DatabaseFunctionEntitySelected += (sender1, e1) => view_DatabaseFunctionSelected(sender1, e1, collection);
        //    MyProjectManager.GetMyProjectManager.ShowDialog(view, "DatabaseFunction_Entity");
        //}

        //void view_DatabaseFunctionSelected(object sender, DatabaseFunctionEntitySelectedArg e, ItemCollection collection)
        //{
        //    AddDatabaseFunctionNodes(collection, EntityID);
        //}


        private RadContextMenu GetCodeFunctionsContextMenu(int entityID, RadTreeViewItem codeFunctionsNode)
        {
            RadContextMenu menu = new RadContextMenu();
            RadMenuItem item = new RadMenuItem();
            item.Header = "تعریف فانکشن جدید";
            item.Click += (sender, e) => item_ClickCodeFunctionNew(sender, e, entityID, codeFunctionsNode);
            menu.Items.Add(item);

            RadMenuItem itemSelect = new RadMenuItem();
            itemSelect.Header = "انتخاب فانکشن موجود";
            itemSelect.Click += (sender, e) => item_ClickCodeFunctionSelect(sender, e, entityID, codeFunctionsNode);
            menu.Items.Add(itemSelect);

            return menu;
        }

        private void item_ClickCodeFunctionSelect(object sender, RadRoutedEventArgs e, int entityID, RadTreeViewItem codeFunctionsNode)
        {
            frmCodeFunctionSelect view = new frmCodeFunctionSelect(new List<Enum_CodeFunctionParamType>() { Enum_CodeFunctionParamType.OneDataItem });
            view.CodeFunctionSelected += (sender1, e1) => view_CodeFunctionSelected1(sender1, e1, codeFunctionsNode.Items);
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "ارتباط تابع و موجودیت");
        }
        void view_CodeFunctionSelected1(object sender, CodeSelectedArg e, ItemCollection collection)
        {
            bizCodeFunction.AddCodeFunctionToEntity(e.CodeFunctionID, EntityID,true);
            AddCodeFunctionNodes(collection, EntityID);
        }
        void item_ClickCodeFunctionNew(object sender, RoutedEventArgs e, int entityID, RadTreeViewItem codeFunctionsNode)
        {
            frmCodeFunction view = new frmCodeFunction(0, Enum_CodeFunctionParamType.OneDataItem, entityID, true);
            view.CodeFunctionUpdated += (sender1, e1) => view_CodeFunctionSelected(sender1, e1, codeFunctionsNode.Items);
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "ارتباط تابع و موجودیت");
        }
        void view_CodeFunctionSelected(object sender, CodeSelectedArg e, ItemCollection collection)
        {
            AddCodeFunctionNodes(collection, EntityID);
        }
        private void item_ClickCodeFunctionEdit(object sender, RadRoutedEventArgs e, ItemCollection collection, int entityID, int codeFunctionEntityID)
        {
            frmCodeFunction view = new frmCodeFunction(codeFunctionEntityID, Enum_CodeFunctionParamType.OneDataItem, entityID,true);
            view.CodeFunctionUpdated += (sender1, e1) => view_CodeFunctionSelected(sender1, e1, collection);
            MyProjectManager.GetMyProjectManager.ShowDialog(view, "ارتباط تابع و موجودیت");
        }
        private void AddCodeFunctionNodes(ItemCollection items, int entityID)
        {
            items.Clear();
            var listFunctoins = bizCodeFunction.GetCodeFunctionEntities(entityID);
            foreach (var codeFunction in listFunctoins.Where(x=>x.ShowInFormula==true))
            {
                AddCodeFunctionNode(items, codeFunction);
            }

        }
        private RadTreeViewItem AddCodeFunctionNode(ItemCollection collection, CodeFunction_EntityDTO codeFunctionEntity)
        {
            var node = GetNode();
            node.DataContext = codeFunctionEntity;
            node.Header = GetNodeHeader(codeFunctionEntity.CodeFunction.ClassName, DatabaseObjectCategory.CodeFunction);
            node.ToolTip = codeFunctionEntity.CodeFunction.ClassName;
            collection.Add(node);
            _AllItems.Add(node);
            RadContextMenu.SetContextMenu(node, GetCodeFunctionContextMenu(collection, codeFunctionEntity.EntityID, codeFunctionEntity.ID));
            return node;
        }

        private RadContextMenu GetCodeFunctionContextMenu(ItemCollection collection, int entityID, int codeFunctionEntityID)
        {
            RadContextMenu menu = new RadContextMenu();
            RadMenuItem item = new RadMenuItem();
            item.Header = "اصلاح فانکشن";
            item.Click += (sender, e) => item_ClickCodeFunctionEdit(sender, e, collection, entityID, codeFunctionEntityID);
            menu.Items.Add(item);

            return menu;
        }


        private void AddRalationshipNodes(ItemCollection items, int entityID)
        {
            foreach (var relationship in Entity.Relationships)
            {
                var node = AddRelationshipNode(items, relationship);
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
                    var objectDTO = treeItem.DataContext as RelationshipDTO;
                    var entityID = objectDTO.EntityID2;// bizRelationship.GetOtherSideEntityID(Convert.ToInt32(objectDTO.ID));
                    PopulateTreeItems(treeItem.Items, entityID);

                }
            }
        }

        //private RadTreeViewItem GetNavigationRootNode()
        //{
        //    if (treeItems.Items.Count > 0)
        //    {
        //        return treeItems.Items[0] as RadTreeViewItem;
        //    }
        //    else
        //    {
        //        var rootNode = AddFormulaObjectNode(treeItems.Items, "Entity", "Root", (string.IsNullOrEmpty(entity.Alias) ? entity.Name : entity.Alias));
        //        return rootNode;
        //    }
        //}
        List<RadTreeViewItem> _AllItems = new List<RadTreeViewItem>();
        private RadTreeViewItem AddColumnNode(ItemCollection collection, ColumnDTO column)
        {
            var node = GetNode();
            node.DataContext = column;
            node.Header = GetNodeHeader((string.IsNullOrEmpty(column.Alias) ? column.Name : column.Alias), DatabaseObjectCategory.Column);
            collection.Add(node);
            _AllItems.Add(node);
            return node;
        }


        private RadTreeViewItem AddRelationshipNode(ItemCollection collection, RelationshipDTO relationship)
        {
            var node = GetNode();
            node.DataContext = relationship;
            node.Header = GetNodeHeader(relationship.Entity2, DatabaseObjectCategory.Relationship);
            node.ToolTip = relationship.ID + "_" + (string.IsNullOrEmpty(relationship.Alias) ? relationship.Name : relationship.Alias);
            collection.Add(node);
            _AllItems.Add(node);
            return node;
        }

        private RadTreeViewItem AddFormulaObjectNode(ItemCollection collection, DatabaseObjectCategory objectCategory, int objectIdentity, string title, string tooltip = null)
        {
            var node = GetNode();
            var context = new ObjectDTO();
            context.ObjectCategory = objectCategory;
          //  context.Object = objectObject;
            context.Title = title;
            context.ObjectIdentity = objectIdentity;
            node.DataContext = context;
            node.Header = GetNodeHeader(context.Title, context.ObjectCategory);
            node.ToolTip = tooltip;
            collection.Add(node);
            _AllItems.Add(node);
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
                uriSource = new Uri("../Images/folder.png", UriKind.Relative);
            }
            else if (type == DatabaseObjectCategory.Entity)
            {
                uriSource = new Uri("../Images/form.png", UriKind.Relative);
            }
            else if (type == DatabaseObjectCategory.Column)
            {
                uriSource = new Uri("../Images/column.png", UriKind.Relative);
            }
            else if (type == DatabaseObjectCategory.Relationship)
            {
                uriSource = new Uri("../Images/relationship.png", UriKind.Relative);
            }
            else if (type == DatabaseObjectCategory.Formula)
            {
                uriSource = new Uri("../Images/parameter.png", UriKind.Relative);
            }
            else if (type == DatabaseObjectCategory.DatabaseFunction)
            {
                uriSource = new Uri("../Images/dbformula.png", UriKind.Relative);
            }
            if (uriSource != null)
                img.Source = new BitmapImage(uriSource);
            pnlHeader.Orientation = Orientation.Horizontal;
            pnlHeader.Children.Add(img);
            pnlHeader.Children.Add(label);
            return pnlHeader;
        }
    }


    public class TreeFormulaParameterSelectedArg : EventArgs
    {
        //public BaseFormulaParameter Parameter { set; get; }
        //public string ParameterName { set; get; }
        //public string ParameterTitle { set; get; }
        //public Type ParameterType { set; get; }
    }
}
