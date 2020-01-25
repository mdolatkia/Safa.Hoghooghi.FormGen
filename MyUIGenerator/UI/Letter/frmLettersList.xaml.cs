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
using ModelEntites;
using MyUILibraryInterfaces.DataTreeArea;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using MyUILibraryInterfaces.ContextMenu;

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for frmLetter.xaml
    /// </summary>
    public partial class frmLettersList : UserControl, I_View_EntityLettersArea
    {
        public bool EnableAdd
        {
            get
            {
                return btnAddItem.IsEnabled;
            }

            set
            {
                btnAddItem.IsEnabled = value;
            }
        }
        public bool AddButtonVisibility
        {
            get
            {
                return btnAddItem.Visibility == Visibility.Visible;
            }

            set
            {
                btnAddItem.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        //public bool EnableDelete
        //{
        //    get
        //    {
        //        return btnDeleteItem.IsEnabled;
        //    }

        //    set
        //    {
        //        btnDeleteItem.IsEnabled = value;
        //    }
        //}

        //public bool EnableEdit
        //{
        //    get
        //    {
        //        return btnEditItem.IsEnabled;
        //    }

        //    set
        //    {
        //        btnEditItem.IsEnabled = value;
        //    }
        //}

        public bool DataTreeAreaEnabled
        {
            get
            {
                return btnDataTree.IsEnabled;
            }

            set
            {
                btnDataTree.IsEnabled = value;
            }
        }

        public frmLettersList()
        {
            InitializeComponent();

        }
        //public event EventHandler<EditLetterArg> DeleteLetterClicked;
        //public event EventHandler<EditLetterArg> EditLetterClicked;
        public event EventHandler NewLetterClicked;
        public event EventHandler DataTreeRequested;
        public event EventHandler<ContextMenuArg> ContextMenuLoaded;

        public void ShowList(List<LetterDTO> letter)
        {
            dtgLetters.ItemsSource = letter;
        }
        public void EnableDisable(bool enable)
        {
            this.IsEnabled = enable;
        }

        public void ShowDataTree(I_DataTreeView view)
        {
            grdDataTree.Children.Clear();
            grdDataTree.Children.Add(view as UIElement);
        }

        private void btnDataTree_Click(object sender, RoutedEventArgs e)
        {
            if (DataTreeRequested != null)
                DataTreeRequested(this, null);
        }
        public bool DataTreeVisibility
        {
            get
            {
                return grdMain.ColumnDefinitions[0].Width.Value != 0;
            }

            set
            {
                //grdDataTree.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                if (!value)
                {
                    grdMain.ColumnDefinitions[0].Width = new GridLength(0);

                }
                else
                {
                    grdMain.ColumnDefinitions[0].Width = new GridLength(200);
                }
            }
        }



        //private void btnDeleteItem_Click(object sender, RoutedEventArgs e)
        //{
        //    if (dtgLetters.SelectedItem != null)
        //    {
        //        if (dtgLetters.SelectedItem is LetterDTO)
        //            if (DeleteLetterClicked != null)
        //                DeleteLetterClicked(this, new EditLetterArg() { LetterID = (dtgLetters.SelectedItem as LetterDTO).ID });
        //    }
        //}

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            if (NewLetterClicked != null)
                NewLetterClicked(this, null);
        }

        //private void btnEditItem_Click(object sender, RoutedEventArgs e)
        //{
        //    if (dtgLetters.SelectedItem != null)
        //    {
        //        if (dtgLetters.SelectedItem is LetterDTO)
        //            if (EditLetterClicked != null)
        //                EditLetterClicked(this, new EditLetterArg() { LetterID = (dtgLetters.SelectedItem as LetterDTO).ID });
        //    }
        //}

      

        private void menuGrid_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var contextMenu = sender as RadContextMenu;
            if (contextMenu != null && contextMenu.GetClickedElement<GridViewRow>() != null)
            {
                var letter = contextMenu.GetClickedElement<GridViewRow>().DataContext as LetterDTO;
                if (contextMenu != null && letter != null)
                {
                    if (ContextMenuLoaded != null)
                    {
                        ContextMenuArg arg = new ContextMenuArg();
                        arg.ContextObject = letter;
                        arg.ContextMenuManager = new ContextMenuManager(contextMenu);
                        ContextMenuLoaded(this, arg);

                    }

                }
            }
        }
    }
}
