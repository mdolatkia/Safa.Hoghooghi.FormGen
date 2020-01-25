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

namespace MyUIGenerator.View
{
    /// <summary>
    /// Interaction logic for frmArchive.xaml
    /// </summary>
    public partial class frmArchive : UserControl, I_View_ArchiveArea
    {
        public List<ArchiveFolderDTO> SelectedFolders
        {
            get
            {
                List<ArchiveFolderDTO> list = new List<ArchiveFolderDTO>();
                foreach (ArchiveFolderDTO item in lstFolders.Items)
                    list.Add(item);
                return list;
            }

            set
            {
                lstFolders.SelectedItems.Clear();
                List<ArchiveFolderDTO> list = new List<ArchiveFolderDTO>();
                foreach (ArchiveFolderDTO item in lstFolders.Items)
                {
                    if (value.Any(x => x.ID == item.ID))
                        lstFolders.SelectedItems.Add(item);
                }
            }
        }

        public List<ArchiveItemDTO> SelectedArchiveItems
        {
            get
            {
                List<ArchiveItemDTO> list = new List<ArchiveItemDTO>();
                foreach (UC_ArchiveItem item in lstArchiveItem.SelectedItems)
                    if (item.Visibility == Visibility.Visible)
                        list.Add(item.ArchiveItemDataItem);
                return list;
            }

            set
            {
                lstArchiveItem.SelectedItems.Clear();
                List<ArchiveItemDTO> list = new List<ArchiveItemDTO>();
                foreach (ArchiveItemDTO item in lstArchiveItem.Items)
                {
                    if (value.Any(x => x.ID == item.ID))
                        lstArchiveItem.SelectedItems.Add(item);
                }
            }
        }

        public ArchiveItemSelectedMode ArchiveItemsSelectedMode
        {
            get
            {
                ArchiveItemSelectedMode mode = ArchiveItemSelectedMode.None;
                if (lstArchiveItem.SelectedItems.Count == 0)
                    mode = ArchiveItemSelectedMode.None;
                else if (lstArchiveItem.SelectedItems.Count == 1)
                    mode = ArchiveItemSelectedMode.One;
                else if (lstArchiveItem.SelectedItems.Count > 1)
                    mode = ArchiveItemSelectedMode.Multiple;
                return mode;
            }


        }

        public bool FolderTabIsSelected
        {
            get
            {
                return tabFolders.IsSelected;
            }


        }

        public bool ArchiveItemsTabIsSelected
        {
            get
            {
                return tabFiles.IsSelected;
            }
        }

        public frmArchive()
        {
            InitializeComponent();
            lstArchiveItem.SelectionChanged += LstArchiveItem_SelectionChanged;
        }



        private void LstArchiveItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ArchiveItemSelectedModeChanged != null)
            {

                ArchiveItemSelectedModeChanged(this, null);
            }
            e.Handled = true;
        }

        public event EventHandler ArchiveItemAddNewRequested;
        public event EventHandler<ArchiveItemSelectedArg> ArchiveItemDoubleCliked;
        public event EventHandler<FolderSelectedArg> FolderDoubleClicked;
        public event EventHandler ArchiveItemInfoRequested;
        public event EventHandler ArchiveItemsDeleteRequested;
        //public event EventHandler ArchiveTagRequested;
        public event EventHandler ArchiveTagFilterRequested;
        public event EventHandler ArchiveTagFilterClearRequested;
        public event EventHandler DataTreeRequested;
        public event EventHandler<ArchiveItemSelectedArg> ArchiveItemRightCliked;
        public event EventHandler MultipleArchiveItemsInfoRequested;
        public event EventHandler ArchiveItemViewRequested;
        public event EventHandler ArchiveItemDownloadRequested;
        public event EventHandler FolderOrItemsTabChanged;
        public event EventHandler ArchiveItemSelectedModeChanged;

        public void ShowArchiveItems(string title, List<ArchiveItemDTO> items, bool activateFileTab)
        {
            tabFiles.Header = "پوشه" + " " + title;
            lstArchiveItem.Items.Clear();
            foreach (var item in items)
            {
                var uc_ArchiveItem = new UC_ArchiveItem(item);
                uc_ArchiveItem.MouseDoubleClick += (sender, e) => Uc_ArchiveItem_DoubleClicked(sender, e, item);
                uc_ArchiveItem.MouseRightButtonUp += (sender, e) => Uc_ArchiveItem_MouseRightButtonUp(sender, e, item);
                //uc_ArchiveItem.Cursor = Cursors.Hand;
                lstArchiveItem.Items.Add(uc_ArchiveItem);
            }
            if (activateFileTab)
                tabFiles.IsSelected = true;
        }

        private void Uc_ArchiveItem_MouseRightButtonUp(object sender, MouseButtonEventArgs e, ArchiveItemDTO item)
        {
            if (ArchiveItemRightCliked != null)
                ArchiveItemRightCliked(this, new ArchiveItemSelectedArg() { ArchiveItem = item });
        }

        private void Uc_ArchiveItem_DoubleClicked(object sender, EventArgs e, ArchiveItemDTO item)
        {
            if (ArchiveItemDoubleCliked != null)
                ArchiveItemDoubleCliked(this, new ArchiveItemSelectedArg() { ArchiveItem = item });
        }
        public void ShowFolders(List<ArchiveFolderWithNullDTO> folders, bool activateFolderTab)
        {
            lstFolders.Items.Clear();
            foreach (var item in folders)
            {
                var uc_ArchiveFolder = new UC_ArchiveFolder(item);
                uc_ArchiveFolder.MouseDoubleClick += (sender, e) => Uc_ArchiveFolder_Clicked(sender, e, item);
                //uc_ArchiveFolder.Cursor = Cursors.Hand;
                //pnlItems.Children.Add(uc_ArchiveFolder);
                lstFolders.Items.Add(uc_ArchiveFolder);
            }
            if (activateFolderTab)
                tabFolders.IsSelected = true;
        }
        public void ClearFolders()
        {
            lstFolders.Items.Clear();
        }
        public void ClearFiles()
        {
            lstArchiveItem.Items.Clear();
        }
        private void Uc_ArchiveFolder_Clicked(object sender, EventArgs e, ArchiveFolderWithNullDTO item)
        {
            if (FolderDoubleClicked != null)
                FolderDoubleClicked(this, new FolderSelectedArg() { FolderID = item.ID });
        }

        private void btnAddArchiveItem_Click(object sender, RoutedEventArgs e)
        {
            if (ArchiveItemAddNewRequested != null)
                ArchiveItemAddNewRequested(this, null);
        }

        private void btnArchiveItemInfo_Click(object sender, RoutedEventArgs e)
        {
            if (ArchiveItemInfoRequested != null)
                ArchiveItemInfoRequested(this, null);
        }

        private void btnArchiveItemDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ArchiveItemsDeleteRequested != null)
                ArchiveItemsDeleteRequested(this, null);
        }

        //private void btnArchiveTag_Click(object sender, RoutedEventArgs e)
        //{
        //    if (ArchiveTagRequested != null)
        //        ArchiveTagRequested(this, null);
        //}

        private void btnArchiveTagFilter_Click(object sender, RoutedEventArgs e)
        {
            if (ArchiveTagFilterRequested != null)
                ArchiveTagFilterRequested(this, null);
        }

        public void ChangeArchiveItemVisibility(ArchiveItemDTO archiveDataItemItem, bool v)
        {

            foreach (UC_ArchiveItem item in lstArchiveItem.Items)
            {
                if (item.ArchiveItemDataItem == archiveDataItemItem)
                {
                    item.Visibility = v ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        private void btnMultipleArchiveItemInfo_Click(object sender, RoutedEventArgs e)
        {
            if (MultipleArchiveItemsInfoRequested != null)
                MultipleArchiveItemsInfoRequested(this, null);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FolderOrItemsTabChanged != null)
            {
                FolderOrItemsTabChanged(this, null);
            }


        }

        public void ClearFilteredTags()
        {
            btnArchiveTagFilter.Foreground = new SolidColorBrush(Colors.Black);
            ToolTipService.SetToolTip(btnArchiveTagFilter, null);
        }

        public void ShowFilteredTags(string title)
        {
            btnArchiveTagFilter.Foreground = new SolidColorBrush(Colors.Red);
            ToolTipService.SetToolTip(btnArchiveTagFilter, title);
        }

        public void EnableDisable(bool enable)
        {
            this.IsEnabled = enable;
        }

        public bool ArchiveItemDelete
        {
            set
            {
                btnArchiveItemDelete.IsEnabled = value;
            }
            get
            {
                return btnArchiveItemDelete.IsEnabled;
            }
        }
        public bool ArchiveItemAdd
        {
            set
            {
                btnAddArchiveItem.IsEnabled = value;
            }
            get
            {
                return btnAddArchiveItem.IsEnabled;
            }
        }


        public bool ArchiveItemInfo
        {
            set
            {
                btnArchiveItemInfo.IsEnabled = value;
            }
            get
            {
                return btnArchiveItemInfo.IsEnabled;
            }

        }


        public bool MultipleArchiveItemInfo
        {
            set
            {
                btnMultipleArchiveItemInfo.IsEnabled = value;
            }
            get
            {
                return btnMultipleArchiveItemInfo.IsEnabled;
            }
        }

        public bool ArchiveItemView
        {
            set
            {
                btnArchiveItemView.IsEnabled = value;
            }
            get
            {
                return btnArchiveItemView.IsEnabled;
            }
        }

        public bool ArchiveItemDownload
        {
            set
            {
                btnArchiveItemDownload.IsEnabled = value;
            }
            get
            {
                return btnArchiveItemDownload.IsEnabled;
            }
        }

        public bool FilteresClear
        {
            set
            {
                btnArchiveTagFilterClear.IsEnabled = value;
            }
            get
            {
                return btnArchiveTagFilterClear.IsEnabled;
            }
        }

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



        private void btnArchiveItemDownload_Click(object sender, RoutedEventArgs e)
        {
            if (ArchiveItemDownloadRequested != null)
                ArchiveItemDownloadRequested(this, null);

        }

        private void btnArchiveItemView_Click(object sender, RoutedEventArgs e)
        {
            if (ArchiveItemViewRequested != null)
                ArchiveItemViewRequested(this, null);
        }

        private void btnArchiveTagFilterClear_Click(object sender, RoutedEventArgs e)
        {
            if (ArchiveTagFilterClearRequested != null)
                ArchiveTagFilterClearRequested(this, null);
        }

        private void btnDataTree_Click(object sender, RoutedEventArgs e)
        {
            if (DataTreeRequested != null)
                DataTreeRequested(this, null);
        }

        public void ShowDataTree(I_DataTreeView view)
        {

            //foreach (var item in grdMain.Children)
            //{
            //    if (item == view)
            //        exists = true;
            //}
            grdDataTree.Children.Clear();
            grdDataTree.Children.Add(view as UIElement);
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
    }
}
