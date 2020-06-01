
using ModelEntites;
using MyUIGenerator.UIControlHelper;
using MyUILibrary;
using MyUILibrary.EntityArea;
using MyUILibrary.EntityArea.Commands;
using MyUILibrary.Temp;
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
using Telerik.Windows.Controls;

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for UC_ViewPackageArea.xaml
    /// </summary>
    public partial class UC_AdvancedSearchEntityArea : UserControl, I_View_AdvancedSearchEntityArea
    {


        //View_Container View_Container { set; get; }
        public UC_AdvancedSearchEntityArea()

        {
            InitializeComponent();

            //View_Container = new View_Container(basicGridSetting);
            //if (AgentHelper.GetAppMode() != AppMode.Paper)
                FlowDirection = System.Windows.FlowDirection.RightToLeft;
            //grdArea.Children.Add(View_Container.Grid);




        }

        public event EventHandler<Arg_CommandExecuted> CommandExecuted;

        public void EnableDisable(bool enable)
        {
            this.IsEnabled = enable;
        }
        //public override Expander Expander
        //{
        //    get
        //    {
        //        return expander;
        //    }
        //}

        //public override Grid expanderHeader
        //{
        //    get
        //    {
        //        return grdExpanderHeader;
        //    }
        //}
        //private void InitializePackageArea()
        //{
        //    //foreach (var type in TemplatePackage.TypeConditions)
        //    //{
        //    //    //foreach (var property in type.NDType.Properties)
        //    //    //{
        //    //    //    var column = new DataGridViewTextBoxColumn();
        //    //    //    column.HeaderText = (property.Title == null ? property.Property.Title : property.Title);
        //    //    //    column.Tag = property;
        //    //    //    column.Name = property.Property.Name;
        //    //    //    //column.CellType=GetType(string);
        //    //    //    dataGridView1.Columns.Add(column);
        //    //    //}
        //    //}

        //}


        //public I_SearchViewEntityArea Controller
        //{
        //    set;
        //    get;
        //}



        public void DeHighlightCommands()
        {
            toolbar.Background = null;
        }
        public void AddCommand(I_CommandManager item, bool indirect)
        {
            //Button btnCommand = UIHelper.GenerateCommand(item);
            //item.EnabledChanged += (sender, e) => item_EnabledChanged(sender, e, btnCommand);
            //btnCommand.Click += btnCommand_Click;
            toolbar.Items.Add((item as MyUIGenerator.UIControlHelper.CommandManager).Button);

        }
        //private void item_EnabledChanged(object sender, EventArgs e, Button btnCommand)
        //{
        //    btnCommand.IsEnabled = (sender as I_Command).Enabled;
        //}
        //void btnCommand_Click(object sender, EventArgs e)
        //{
        //    if (CommandExecuted != null)
        //        CommandExecuted(this, new Arg_CommandExecuted() { Command = (sender as Button).Tag as I_Command });
        //    ////  Controller.CommandExecuted(((sender as Button).Tag as I_EntityAreaCommand));
        //}

        public void SetBackgroundColor(string color)
        {
            this.Background = new SolidColorBrush(UIHelper.getColorFromHexString(color));
        }

        public I_AdvanceSearchNodeManager AddTreeItem()
        {
            RadTreeViewItem node = new RadTreeViewItem();
            treeItems.Items.Add(node); ;
            var newAdvenveSearchNode = new AdvenveSearchNode(node);
            return newAdvenveSearchNode;
        }
        public void ClearTreeItems()
        {
            treeItems.Items.Clear();
        }

        public void DisableEnableDataSection(bool enable)
        {
            treeItems.IsEnabled = enable;
        }

        public void Visiblity(bool visible)
        {
            this.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        //public bool AddControlPackageToHeader(object uiControlPackage, string title, InfoColor titleColor, string tooltip = "")
        //{
        //    throw new NotImplementedException();
        //}
    }
    public class AdvenveSearchNode : I_AdvanceSearchNodeManager
    {
        public RadTreeViewItem Node;
        public AdvenveSearchNode(RadTreeViewItem node)
        {
            Node = node;
        }
        ContextMenu contextMenu = new ContextMenu();
        public I_AdvanceSearchMenu AddMenu(string header)
        {
            if (Node.ContextMenu == null)
                Node.ContextMenu = new ContextMenu();
            MenuItem menu = new MenuItem();
            menu.Header = header;
            AdvanceSearchMenu advanceSearchMenu = new AdvanceSearchMenu(menu);
            Node.ContextMenu.Items.Add(menu);
            menu.Click += (sender, e) => Menu_Click(sender, e, advanceSearchMenu);
            return advanceSearchMenu;
        }

        private void Menu_Click(object sender, RoutedEventArgs e, AdvanceSearchMenu advanceSearchMenu)
        {
            advanceSearchMenu.OnClicked();
        }

        public void SetHeader(string header)
        {

            Node.Header = header;

        }
        public void RemoveItem(I_AdvanceSearchNodeManager nodeManager)
        {
            Node.Items.Remove((nodeManager as AdvenveSearchNode).Node);
        }
        public I_AdvanceSearchNodeManager AddChildItem()
        {
            RadTreeViewItem cnode = new RadTreeViewItem();
            Node.Items.Add(cnode);
            Node.IsExpanded = true;
            var newAdvenveSearchNode = new AdvenveSearchNode(cnode);
            return newAdvenveSearchNode;
        }

        public void AddExistingChildItem(I_AdvanceSearchNodeManager item)
        {
            Node.Items.Add((item as AdvenveSearchNode).Node);
        }

        public void ClearItems()
        {
            Node.Items.Clear();
        }
    }
    public class AdvanceSearchMenu : I_AdvanceSearchMenu
    {
        MenuItem Menu { set; get; }
        public AdvanceSearchMenu(MenuItem menu)
        {
            Menu = menu;
        }

        public void OnClicked()
        {
            if (Clicked != null)
                Clicked(this, null);
        }
        public event EventHandler Clicked;
    }
}
