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
    /// InterActivity logic for frmAddSelectActivity.xaml
    /// </summary>
    public partial class frmSelectActivity: UserControl
    {
        public event EventHandler<SelectedItemsArg> ItemsSelected;
        int ProcessID { set; get; }
        List<int> SelectedActivitys { set; get; }
        BizActivity bizActivity = new BizActivity();
        public frmSelectActivity(int processID, List<int> selectedActivitys)
        {
            InitializeComponent();
            ProcessID = processID;
            SelectedActivitys = selectedActivitys;
            GetActivitys();
        }

        private void GetActivitys()
        {
            var Activitys = bizActivity.GetActivities(ProcessID, false);
            foreach (var item in Activitys)
            {
                if (SelectedActivitys.Any(x => x == item.ID))
                    item.vwSelected = true;
            }
            dtgList.ItemsSource = Activitys;
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MyProjectManager.GetMyProjectManager().CloseDialog(this);
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (ItemsSelected != null)
            {
                var Activitys = dtgList.ItemsSource as List<ActivityDTO>;
                if (Activitys.Count(x => x.vwSelected == true) > 0)
                {
                    var arg = new SelectedItemsArg();
                    arg.Items = new List<object>();
                    foreach (var item in Activitys)
                    {
                        if (item.vwSelected == true)
                            arg.Items.Add(item);
                    }
                    ItemsSelected(this, arg);
                    MyProjectManager.GetMyProjectManager().CloseDialog(this);
                }
            }

        }
        private void mnuAddNewActivity_Click(object sender, RoutedEventArgs e)
        {
            frmAddActivity view = new frmAddActivity(ProcessID, 0);
            view.ItemSaved += View_ItemSaved1;
               MyProjectManager.GetMyProjectManager().ShowDialog(view, "Form",Enum_WindowSize.Big);
        }



        private void mnuEditActivity_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as RadMenuItem;
            var contextMenu = menuItem.Parent as RadContextMenu;
            var source = contextMenu.GetClickedElement<GridViewRow>();
            if (contextMenu != null && source != null)
            {
                frmAddActivity view = new frmAddActivity(ProcessID, (source.DataContext as  ActivityDTO).ID);
                view.ItemSaved += View_ItemSaved1;
                   MyProjectManager.GetMyProjectManager().ShowDialog(view, "Form",Enum_WindowSize.Big);
            }
        }
        private void View_ItemSaved1(object sender, SavedItemArg e)
        {
            GetActivitys();
        }

    }

}
