using MyUILibrary.EntityArea;
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
using ProxyLibrary;
using Telerik.Windows.Controls;

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for UC_DeleteInquiry.xaml
    /// </summary>
    public partial class UC_DeleteInquiry : UserControl, I_ViewDeleteInquiry
    {
        public UC_DeleteInquiry()
        {
            InitializeComponent();
        }

        public event EventHandler<ConfirmModeClickedArg> ButtonClicked;

        public void SetMessage(string message)
        {
            lblMessage.Text = message;
        }

        public void SetTreeItems(List<DP_DataRepository> dataItems)
        {
            foreach (var item in dataItems)
            {
                AddTreeItem(treeItems.Items, item);
            }
            treeItems.ExpandAll();
        }

        private void AddTreeItem(ItemCollection items, DP_DataRepository item)
        {
            RadTreeViewItem node = new RadTreeViewItem();
            node.Header = item.ViewInfo;
            if (!string.IsNullOrEmpty(item.Error))
            {
                node.Foreground = new SolidColorBrush(Colors.Red);
                ToolTipService.SetToolTip(node, item.Error);
            }
            items.Add(node);
            foreach (var child in item.ChildRelationshipInfos)
            {
                AddTreeItem(node.Items, child);
            }
           
        }

        private void AddTreeItem(ItemCollection items, ChildRelationshipInfo child)
        {
            RadTreeViewItem node = new RadTreeViewItem();
            node.Header = child.Relationship.Alias;
            items.Add(node);
            foreach (var data in child.RelatedData)
            {
                AddTreeItem(node.Items, data);
            }
        }

        public void SetUserConfirmMode(UserDialogMode mode)
        {
            if (mode == UserDialogMode.Ok)
            {
                Button okButton = GetButton("تایید");
                okButton.Click += OkButton_Click;
            }
            else if (mode == UserDialogMode.YesNo)
            {
                Button yesButton = GetButton("بله");
                yesButton.Click += YesButton_Click;
                Button noButton = GetButton("خیر");
                noButton.Click += NoButton_Click;
            }
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            if (ButtonClicked != null)
                ButtonClicked(this, new ConfirmModeClickedArg() { Result = UserDialogResult.No });
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            if (ButtonClicked != null)
                ButtonClicked(this, new ConfirmModeClickedArg() { Result = UserDialogResult.Yes });
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (ButtonClicked != null)
                ButtonClicked(this, new ConfirmModeClickedArg() { Result = UserDialogResult.Ok });
        }

        private Button GetButton(string title)
        {
            var button = new Button();
            button.Width = 80;
            button.Content = title;
            stkButtons.Children.Add(button);
            return button;
        }
    }
}
