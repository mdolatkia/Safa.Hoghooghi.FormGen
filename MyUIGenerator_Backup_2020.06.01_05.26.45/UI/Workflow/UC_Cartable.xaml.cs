using MyUILibrary.EntityArea;
using MyUILibrary.WorkflowArea;

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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for UC_Cartable.xaml
    /// </summary>
    public partial class UC_Cartable : UserControl, I_View_Cartable
    {
        //UIManager uiManager = new UIManager();

        //public event EventHandler<CartableEntityClickArg> EntitiyClicked;
        public event EventHandler CartableRefreshClicked;
        public event EventHandler<CartableMenuClickArg> MenuClicked;
        public event EventHandler<CartableItemSelectedArg> InfoClicked;

        List<WorkflowRequestDTO> Requests { set; get; }

        public UC_Cartable()
        {
            InitializeComponent();
            dtgWorkflowRequests.MouseDoubleClick += DataGrid_MouseDoubleClick;
        }


        private void RadContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

            var contextMenu = sender as RadContextMenu;
            if (contextMenu.GetClickedElement<GridViewRow>() != null)
            {
                var workflowRequest = contextMenu.GetClickedElement<GridViewRow>().DataContext as WorkflowRequestDTO;
                if (contextMenu != null && workflowRequest != null)
                {
                    if (MenuClicked != null)
                        MenuClicked(this, new CartableMenuClickArg() { Request = workflowRequest, ContextMenu = contextMenu });
                    //var requestActions = UIManagerGenerator.GetUIManager().GetRequestActions(workflowRequest.ID);
                    //همه نباید پاک شوند

                }
            }
        }
        //public void ShowMenuItems(List<RequestActionDTO> requestActions)
        //{
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }


        //}

        //private void MnuEntity_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        //{
        //    var arg = (sender as RadMenuItem).DataContext as CartableEntityClickArg;
        //    if (EntitiyClicked != null)
        //    {

        //        EntitiyClicked(this, arg);
        //    }
        //}

        //private void MnuAction_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        //{

        //}

        public void ShowWorkflowRequests(List<WorkflowRequestDTO> requests)
        {
            Requests = requests;
            dtgWorkflowRequests.ItemsSource = Requests;

        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            CartableRefreshClicked(this, null);
        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as RadGridView).SelectedItem != null)
            {
                if (InfoClicked != null)
                {
                    InfoClicked(this, new  CartableItemSelectedArg() { DataItem = (sender as RadGridView).SelectedItem, UIElement = (sender as RadGridView).GetRowForItem((sender as RadGridView).SelectedItem) });
                }
            }
        }

    }
}
