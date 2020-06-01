using MyModelManager;

using ProxyLibrary.Workflow;
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
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace MyProject_WPF
{
    /// <summary>
    /// Interaction logic for frmAddSelectAction.xaml
    /// </summary>
    public partial class frmSelectAction : UserControl
    {
        public event EventHandler<SelectedItemsArg> ItemsSelected;
        int ProcessID { set; get; }
        List<int> SelectedActions { set; get; }
        BizAction bizAction = new BizAction();
        public frmSelectAction(int processID, List<int> selectedActions)
        {
            InitializeComponent();
            ProcessID = processID;
            SelectedActions = selectedActions;
            GetActions();
        }

        private void GetActions()
        {
            var Actions = bizAction.GetActions(ProcessID, false);
            foreach (var item in Actions)
            {
                if (SelectedActions.Any(x => x == item.ID))
                    item.vwSelected = true;
            }
            dtgList.ItemsSource = Actions;
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager().CloseDialog(this);
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (ItemsSelected != null)
            {
                var actions = dtgList.ItemsSource as List<WFActionDTO>;
                if (actions.Count(x => x.vwSelected == true) > 0)
                {
                    var arg = new SelectedItemsArg();
                    arg.Items = new List<object>();
                    foreach (var item in actions)
                    {
                        if (item.vwSelected == true)
                            arg.Items.Add(item);
                    }
                    ItemsSelected(this, arg);
                    MyProjectManager.GetMyProjectManager().CloseDialog(this);
                }
            }

        }
        private void mnuAddNewAction_Click(object sender, RoutedEventArgs e)
        {
            frmAddAction view = new frmAddAction(ProcessID, 0);
            view.ItemSaved += View_ItemSaved1;
            MyProjectManager.GetMyProjectManager().ShowDialog(view, "Form", Enum_WindowSize.Big);
        }



        private void mnuEditAction_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as RadMenuItem;
            var contextMenu = menuItem.Parent as RadContextMenu;
            var source = contextMenu.GetClickedElement<GridViewRow>();
            if (contextMenu != null && source != null)
            {
                frmAddAction view = new frmAddAction(ProcessID, (source.DataContext as WFActionDTO).ID);
                view.ItemSaved += View_ItemSaved1;
                MyProjectManager.GetMyProjectManager().ShowDialog(view, "Form", Enum_WindowSize.Big);
            }
        }
        private void View_ItemSaved1(object sender, SavedItemArg e)
        {
            GetActions();
        }

    }

}
