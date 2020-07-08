using ModelEntites;
using MyDatabaseToObject;
using MyModelManager;
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
using Telerik.Windows;
using Telerik.Windows.Controls;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmNavigationTree.xaml
    /// </summary>
    public partial class frmNavigationTree : UserControl, ImportWizardForm
    {
        BizDatabaseToObject bizDatabaseToObject = new BizDatabaseToObject();
        BizDatabase bizDatabase = new BizDatabase();
        BizNavigationTree bizNavigationTree = new BizNavigationTree();
        //   frmDatabaseTree frmDBTree;
        DatabaseDTO Database { set; get; }
        //List<int> DatabaseIDs { set; get; }
        //     bool IgnoreAlreadyInNavigationTree { set; get; }
        public frmNavigationTree()
        {
            InitializeComponent();
            // this.Loaded += frmNavigationTree_Loaded;
            SetDatabaseLookup();
            lokDatabase.SelectionChanged += LokDatabase_SelectionChanged;
            //if (databaseID != 0)
            //    lokDatabase.SelectedValue = databaseID;
            //this.frmDBTree.treeDBObjects.startdr
            //this.frmDBTree.treeDBObjects.MouseLeftButtonDown += frmDBTree_MouseLeftButtonDown;
            treeNavigation.DragOver += treeNavigation_DragOver;
            treeNavigation.Drop += treeNavigation_Drop;

            treeDBObjects.MouseMove += treeDBObjects_MouseMove;

            bizDatabaseToObject.IgnoreViews = false;
            bizDatabaseToObject.IgnoreColumns = true;
            bizDatabaseToObject.IgnoreCommands = true;
            bizDatabaseToObject.IgnoreRelationships = true;
            //bizDatabaseToObject. = true;
            //bizDatabaseToObject.IgnoreAlreadyInNavigationTree = true;

            this.Loaded += FrmNavigationTree_Loaded;

            btnExtract.Visibility = Visibility.Collapsed;
        }
        public frmNavigationTree(DatabaseDTO database, bool ignoreNotIndependentOrAlreadyInNavigationTree) : this()
        {
            Database = database;
            bizDatabaseToObject.IgnoreNotIndependentOrAlreadyInNavigationTree = ignoreNotIndependentOrAlreadyInNavigationTree;
            grdDatabase.Visibility = Visibility.Collapsed;

        }
        private void FrmNavigationTree_Loaded(object sender, RoutedEventArgs e)
        {
            if (Database != null)
                SetImportedInfo();
            SetNavigationTree();
        }


        private void LokDatabase_SelectionChanged(object sender, MyCommonWPFControls.SelectionChangedArg e)
        {
            btnExtract_Click(null, null);
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
        private async void SetNavigationTree()
        {
            var navigationTree = await GetFullNavigatoinTree();
            var parentNode = GetNavigationRootNode();
            parentNode.Items.Clear();
            foreach (var item in navigationTree.TreeItems.Where(x => x.ParentItem == null))
            {
                ShowTreeItem(navigationTree.TreeItems, parentNode.Items, item);
            }
            parentNode.ExpandAll();
        }
        private async void SetImportedInfo()
        {
            try
            {
                if (FormIsBusy != null)
                    FormIsBusy(this, null);
                DatabaseDTO database = null;
                if (Database == null)
                {
                    if (lokDatabase.SelectedItem != null)
                        database = (DatabaseDTO)lokDatabase.SelectedItem;
                }
                else
                    database = Database;
                if (database != null)
                {
                    var dbObject = await GetDatabaseObjects(database);
                    if (dbObject.ChildObjects.Any())
                    {
                        AddDBObjectsToTree(new List<ObjectDTO>() { dbObject }, treeDBObjects.Items);
                    }
                    else
                    {
                        treeDBObjects.Items.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در پردازش اطلاعات" + Environment.NewLine + ex.Message);
            }
            finally
            {
                if (treeDBObjects.Items.Count > 0 && (treeDBObjects.Items[0] as RadTreeViewItem).Items.Count > 0)
                {
                    if (FormIsFree != null)
                        FormIsFree(this, null);
                    lblMessage.Visibility = Visibility.Collapsed;
                }
                else
                {
                    treeDBObjects.Items.Clear();
                    lblMessage.Text = "موجودیتی به منظور افزودن به درخت منو موجود نمی باشد";
                    lblMessage.Visibility = Visibility.Visible;
                }


            }
        }
        public Task<ObjectDTO> GetDatabaseObjects(DatabaseDTO database)
        {
            return Task.Run(() =>
            {
                ObjectDTO dbObject = new ObjectDTO();
                dbObject.ObjectCategory = DatabaseObjectCategory.Database;
                dbObject.ObjectIdentity = database.ID;
                dbObject.Title = database.Name;
                AddChildObjects(dbObject);
                return dbObject;
            });
        }


        private void AddChildObjects(ObjectDTO objectDTO)
        {
            if (InfoUpdated != null)
                InfoUpdated(this, new ItemImportingStartedArg() { ItemName = "Fetching database object " + objectDTO.Title });
            var childObjects = bizDatabaseToObject.GetDatabaseChildObjects(objectDTO.ObjectCategory, objectDTO.Title, Convert.ToInt32(objectDTO.ObjectIdentity));
            foreach (var child in childObjects)
            {
                objectDTO.ChildObjects.Add(child);
                AddChildObjects(child);
            }
        }

        private void AddDBObjectsToTree(List<ObjectDTO> objects, ItemCollection collection)
        {
            collection.Clear();
            foreach (var item in objects)
            {
                var node = AddDBObjectsToTree(item, collection);
                var childObjects = item.ChildObjects;// bizDatabaseToObject.GetDatabaseChildObjects(item.ObjectCategory, item.Title, Convert.ToInt32(item.ObjectIdentity));
                if (childObjects.Any())
                {
                    AddDBObjectsToTree(childObjects, node.Items);
                    if (item.ObjectCategory == DatabaseObjectCategory.Database || item.ObjectCategory == DatabaseObjectCategory.Schema)
                        node.IsExpanded = true;
                }
                else
                {
                    //if (item.ObjectCategory != DatabaseObjectCategory.Archive && item.ObjectCategory != DatabaseObjectCategory.Letter)
                    //{

                    //}
                    if (item.ObjectCategory == DatabaseObjectCategory.Schema || item.ObjectCategory == DatabaseObjectCategory.Database)
                        collection.Remove(node);
                }
            }
        }
        private RadTreeViewItem AddDBObjectsToTree(ObjectDTO item, ItemCollection collection)
        {

            var treeItem = new RadTreeViewItem();
            //if (!ExceptColumns || item.ObjectCategory != DatabaseObjectCategory.Entity)
            //{
            //treeItem.Items.Add("Loading...");
            //treeItem.Expanded += treeItem_Expanded;
            //}
            treeItem.Header = GetNodeHeader(item.Title, item.ObjectCategory, item.EntityType);
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
        //private void AddDatabaseTree(int databaseID)
        //{
        //    //   DatabaseID = databaseID;
        //    //frmDBTree = new frmDatabaseTree(new List<int>() { databaseID }, true, true, true, true, false, false, IgnoreAlreadyInNavigationTree);
        //    //grdDatabaseTree.Children.Clear();
        //    //grdDatabaseTree.Children.Add(frmDBTree);

        //}

        //void frmNavigationTree_Loaded(object sender, RoutedEventArgs e)
        //{
        //    //if (treeNavigation.Items.Count == 0)

        //}

        public Task<NavigationTreeDTO> GetFullNavigatoinTree()
        {
            return Task.Run(() =>
                {
                    return bizNavigationTree.GetFullNavigatoinTree(MyProjectManager.GetMyProjectManager.GetRequester());
                });
        }

        private void ShowTreeItem(List<NavigationItemDTO> treeItems, ItemCollection itemCollection, NavigationItemDTO item)
        {
            var node = AddNavigationNode(itemCollection, item);
            foreach (var citem in treeItems.Where(x => x.ParentItem == item))
                ShowTreeItem(treeItems, node.Items, citem);
        }
        //private RadTreeViewItem FindTreeItemByObjectID(ItemCollection collection, int objectID)
        //{

        //    foreach (var item in collection)
        //    {
        //        if ((item is RadTreeViewItem))
        //            if (((item as RadTreeViewItem).DataContext as NavigationItemDTO).ID == objectID)
        //                return (item as RadTreeViewItem);
        //            else
        //            {
        //                var result = FindTreeItemByObjectID((item as RadTreeViewItem).Items, objectID);
        //                if (result != null)
        //                    return result;
        //            }

        //    }
        //    return null;
        //}

        private RadTreeViewItem GetNavigationRootNode()
        {
            if (treeNavigation.Items.Count > 0)
            {
                return treeNavigation.Items[0] as RadTreeViewItem;
            }
            else
            {
                var rootNode = CreateNavigationNode(treeNavigation.Items, DatabaseObjectCategory.Folder, 0, "ریشه", "Root", 0);
                return rootNode;
            }
        }
        private RadTreeViewItem AddNavigationNode(ItemCollection collection, NavigationItemDTO item)
        {
            if (InfoUpdated != null)
                InfoUpdated(this, new ItemImportingStartedArg() { ItemName = "Fetching menu item " + item.Title });
            var node = new RadTreeViewItem();
            node.DataContext = item;
            node.Header = GetNodeHeader(item.Title, item.ObjectCategory);
            //   node.Selected += node_Selected;
            collection.Add(node);
            return node;
        }

        private RadTreeViewItem CreateNavigationNode(ItemCollection collection, DatabaseObjectCategory objectCategory, int objectIdentity, string title, string name, int entitiyID)
        {
            if (InfoUpdated != null)
                InfoUpdated(this, new ItemImportingStartedArg() { ItemName = "Fetching menu item " + title });
            var node = new RadTreeViewItem();
            var context = new NavigationItemDTO();
            //context.ID = id;
            context.ObjectCategory = objectCategory;
            context.Title = title;
            context.Name = name;
            context.ObjectIdentity = objectIdentity;
            context.TableDrivedEntityID = entitiyID;
            context.Title = title;

            node.DataContext = context;
            node.Header = GetNodeHeader(context.Title, context.ObjectCategory);
            //   node.Selected += node_Selected;
            collection.Add(node);
            return node;
        }


        private FrameworkElement GetNodeHeader(string title, DatabaseObjectCategory type, EntityObjectType entityType = EntityObjectType.None)
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
            else if (type == DatabaseObjectCategory.Database)
            {
                uriSource = new Uri("../Images/database.png", UriKind.Relative);
            }
            else if (type == DatabaseObjectCategory.Schema)
            {
                uriSource = new Uri("../Images/folder.png", UriKind.Relative);
            }
            else if (type == DatabaseObjectCategory.Entity)
            {
                if (entityType == EntityObjectType.View)
                    uriSource = new Uri("../Images/view.png", UriKind.Relative);
                else
                    uriSource = new Uri("../Images/form.png", UriKind.Relative);

            }
            else if (type == DatabaseObjectCategory.Report)
            {
                uriSource = new Uri("../Images/report.png", UriKind.Relative);
            }
            else if (type == DatabaseObjectCategory.Archive)
            {
                uriSource = new Uri("../Images/archive.png", UriKind.Relative);
            }
            else if (type == DatabaseObjectCategory.Letter)
            {
                uriSource = new Uri("../Images/mail.png", UriKind.Relative);
            }
            else
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


        bool _isDragging = false;

        public event EventHandler<ItemImportingStartedArg> InfoUpdated;
        public event EventHandler FormIsBusy;
        public event EventHandler FormIsFree;

        void treeDBObjects_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                _isDragging = false;
            if (!_isDragging && e.LeftButton == MouseButtonState.Pressed)
            {
                if (treeDBObjects.SelectedItem != null)
                {
                    _isDragging = true;
                    DragDrop.DoDragDrop(treeDBObjects, treeDBObjects.SelectedItem,
                        DragDropEffects.Copy);
                }
            }
        }

        void treeNavigation_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(RadTreeViewItem)))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }
        void treeNavigation_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(RadTreeViewItem)))
            {
                var sourceNode = (RadTreeViewItem)e.Data.GetData(typeof(RadTreeViewItem));
                var target = e.Source as DependencyObject;
                while (target != null && !(target is RadTreeViewItem))
                    target = VisualTreeHelper.GetParent(target);

                var targetNode = target as RadTreeViewItem;
                if (sourceNode != null && targetNode != null)
                {
                    CloneTreeNode(sourceNode, targetNode);

                }

            }
        }

        private void CloneTreeNode(RadTreeViewItem sourceNode, RadTreeViewItem targetNode)
        {
            if ((targetNode.DataContext as NavigationItemDTO).ObjectCategory != DatabaseObjectCategory.Folder)
                return;
            if (sourceNode == null)
                return;
            if ((sourceNode.DataContext as ObjectDTO).EntityType == EntityObjectType.View)
                return;
            DatabaseObjectCategory objectCategory;
            if (sourceNode.DataContext is ObjectDTO)
            {
                var objectDTO = sourceNode.DataContext as ObjectDTO;
                if (objectDTO.ObjectCategory == DatabaseObjectCategory.Database)
                    objectCategory = DatabaseObjectCategory.Folder;
                else if (objectDTO.ObjectCategory == DatabaseObjectCategory.Schema)
                    objectCategory = DatabaseObjectCategory.Folder;
                else
                {
                    objectCategory = objectDTO.ObjectCategory;
                }
                var newNode = CreateNavigationNode(targetNode.Items, objectCategory, objectDTO.ObjectIdentity, objectDTO.Title, objectDTO.Name, objectDTO.TableDrivedEntityID);
                foreach (var tsNode in sourceNode.Items)
                    CloneTreeNode(tsNode as RadTreeViewItem, newNode);

            }





        }




        //private void SetNavigationTreeIcons(ItemCollection treeNodeCollection)
        //{
        //    foreach (RadTreeViewItem node in treeNodeCollection)
        //    {
        //        SetNodeImage(node);
        //        SetNavigationTreeIcons(node.Nodes);
        //    }
        //}

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            var node = treeNavigation.SelectedItem as RadTreeViewItem;
            if (node != null)
            {
                if (node != treeNavigation.Items[0])
                {
                    (node.Parent as RadTreeViewItem).Items.Remove(node);

                }
            }
        }

        private void MenuItemAdd_Click(object sender, RoutedEventArgs e)
        {
            var node = treeNavigation.SelectedItem as RadTreeViewItem;
            if (node != null)
            {
                var context = node.DataContext as NavigationItemDTO;

                if (context.ObjectCategory == DatabaseObjectCategory.Folder)
                {
                    var newNode = CreateNavigationNode(node.Items, DatabaseObjectCategory.Folder, 0, "منوی جدید", "", 0);
                    newNode.IsSelected = true;
                }
            }
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtName.IsEnabled == true)
            {
                var item = treeNavigation.SelectedItem as RadTreeViewItem;
                if (item != null && item != treeNavigation.Items[0] && item == treeNavigation.SelectedItem)
                {
                    var context = (item as RadTreeViewItem).DataContext as NavigationItemDTO;
                    //if (context.Category == "Folder")
                    //{
                    context.Title = txtName.Text;

                    (item as RadTreeViewItem).Header = GetNodeHeader(context.Title, context.ObjectCategory);
                    //}
                }
            }
        }
        private void txtTooltip_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtTooltip.IsEnabled == true)
            {
                var item = treeNavigation.SelectedItem as RadTreeViewItem;
                if (item != null && item != treeNavigation.Items[0] && item == treeNavigation.SelectedItem)
                {
                    var context = (item as RadTreeViewItem).DataContext as NavigationItemDTO;
                    //if (context.Category == "Folder")
                    //{
                    context.Tooltip = txtTooltip.Text;

                    //   (item as RadTreeViewItem).Header = GetNodeHeader(context.Title, context.ObjectCategory);
                    //}
                }
            }
        }
        //void node_Selected(object sender, RoutedEventArgs e)
        //{
        //    e.Handled = true;
        //    var item = e.Source as RadTreeViewItem;
        //    if ((item !=null) && item.DataContext is NavigationItemDTO)
        //    {
        //        var navigationItem = item.DataContext as NavigationItemDTO;
        //        if (navigationItem.Category == "Folder")
        //        {
        //            txtName.IsEnabled = true;
        //            txtName.Text = navigationItem.ItemName;
        //            txtName.Focus();
        //        }
        //    }
        //}
     
        private void treeNavigation_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var source = e.OriginalSource as DependencyObject;
            while (source != null && !(source is RadTreeViewItem))
                source = VisualTreeHelper.GetParent(source);
            if (source != null)
            {
                treeNavigation.SelectedItem = (source as RadTreeViewItem);
                (source as RadTreeViewItem).Focus();
                e.Handled = true;
            }
        }
        //private void btnSave_Click(object sender, RoutedEventArgs e)
        //{

        //}

        private void CollectNavigationTreeItems(List<NavigationItemDTO> items, RadTreeViewItem parentNode)
        {
            NavigationItemDTO parentContext = null;
            if (parentNode != treeNavigation.Items[0])
                parentContext = parentNode.DataContext as NavigationItemDTO;
            foreach (RadTreeViewItem node in parentNode.Items)
            {
                var context = node.DataContext as NavigationItemDTO;
                context.ParentItem = parentContext;
                items.Add(context);
                CollectNavigationTreeItems(items, node);
            }
        }

        private void btnExtract_Click(object sender, RoutedEventArgs e)
        {
            SetImportedInfo();
        }





        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            bool updated = false;
            try
            {
                if (FormIsBusy != null)
                    FormIsBusy(this, null);
                List<NavigationItemDTO> items = new List<NavigationItemDTO>();
                CollectNavigationTreeItems(items, (treeNavigation.Items[0] as RadTreeViewItem));
                bizNavigationTree.Save(items);
                updated = true;
                MessageBox.Show("انتقال اطلاعات انجام شد");

            }
            catch (Exception ex)
            {
                MessageBox.Show("انتقال اطلاعات انجام نشد" + Environment.NewLine + ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (FormIsFree != null)
                    FormIsFree(this, null);
                if (updated)
                {
                    if (bizDatabaseToObject.IgnoreNotIndependentOrAlreadyInNavigationTree)
                        SetImportedInfo();
                    //SetNavigationTree();
                }
            }
        }

        private void treeNavigation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtName.IsEnabled = false;
            txtName.Text = "";
            txtTooltip.IsEnabled = false;
            txtTooltip.Text = "";
            var item = treeNavigation.SelectedItem as RadTreeViewItem;
            if (item != null && item != treeNavigation.Items[0])
            {
                var context = (item as RadTreeViewItem).DataContext as NavigationItemDTO;
                //if (context.Category == "Folder")
                //{
                txtName.IsEnabled = true;
                txtName.Text = context.Title;

                txtTooltip.IsEnabled = true;
                txtTooltip.Text = context.Tooltip;
                //}
            }
        }



        //public bool HasData()
        //{

        //}
    }
}
