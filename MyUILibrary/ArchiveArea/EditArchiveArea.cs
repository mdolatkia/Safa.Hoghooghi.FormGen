using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelEntites;
using ProxyLibrary;
using MyCodeFunctionLibrary;

using MyUILibraryInterfaces.DataTreeArea;
using MyUILibrary.DataTreeArea;
using MyUILibraryInterfaces.EntityArea;
using MyUILibraryInterfaces.ContextMenu;

namespace MyUILibrary.EntityArea
{
    class EditArchiveArea : I_EntityArchiveArea
    {

        //public DataItemDTO DataItem { set; get; }
        public List<DataItemDTO> RelatedItems { set; get; }
        //public bool SecurityNoAccess { set; get; }
        //public bool SecurityReadonly { set; get; }
        //public bool SecurityEdit { set; get; }
        //public List<TableDrivedEntityDTO> Entities = new List<TableDrivedEntityDTO>();
        //public I_View_EntitySelectArea SelectAreaView
        //{
        //    set; get;
        //}
        public I_EntitySelectArea EntitySelectArea { set; get; }
        public object MainView
        {
            set; get;
        }
        public ArchiveAreaInitializer AreaInitializer { set; get; }
        public EditArchiveArea(ArchiveAreaInitializer areaInitializer)
        {
            AreaInitializer = areaInitializer;
            FilteredArchiveTags = new List<int>();

            ArchiveView = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateViewOfArchiveArea();
            ArchiveView.ArchiveItemAddNewRequested += View_AddNewRequested;
            ArchiveView.ArchiveItemDoubleCliked += View_ArchiveItemDoubleCliked;
            ArchiveView.ArchiveItemRightCliked += View_ArchiveItemRightCliked;
            ArchiveView.ArchiveItemsDeleteRequested += View_ArchiveItemDeleteRequested;
            ArchiveView.ArchiveItemInfoRequested += View_ArchiveItemInfoRequested;
            ArchiveView.FolderDoubleClicked += View_FolderDoubleCliked;
            ArchiveView.MultipleArchiveItemsInfoRequested += View_MultipleArchiveItemsInfoRequested;
            ArchiveView.FolderOrItemsTabChanged += View_FolderOrItemsTabChanged;
            ArchiveView.ArchiveItemViewRequested += View_ArchiveItemViewRequested;
            ArchiveView.DataTreeRequested += View_DataTreeRequested;
            ArchiveView.ArchiveItemDownloadRequested += View_ArchiveItemDownloadRequested;
            ArchiveView.ArchiveItemSelectedModeChanged += View_ArchiveItemSelectedModeChanged;
            //ArchiveView.ArchiveTagRequested += View_ArchiveTagRequested;
            ArchiveView.ArchiveTagFilterRequested += View_ArchiveTagFilterRequested;
            ArchiveView.ArchiveTagFilterClearRequested += View_ArchiveTagFilterClearRequested;
            ArchiveView.ArchiveItemAdd = false;
            ArchiveView.ArchiveItemDelete = false;
            ArchiveView.FilteresClear = false;
            ArchiveView.EnableDisable(false);

            MyUILibraryInterfaces.EntityArea.EntitySelectAreaInitializer selectAreaInitializer = new MyUILibraryInterfaces.EntityArea.EntitySelectAreaInitializer();
            selectAreaInitializer.ExternalView = ArchiveView;
            selectAreaInitializer.EntityID = areaInitializer.EntityID;
            selectAreaInitializer.SpecificActions = new List<SecurityAction>() { SecurityAction.ArchiveView, SecurityAction.ArchiveEdit };
            if (areaInitializer.EntityID != 0)
                selectAreaInitializer.LockEntitySelector = true;
            EntitySelectArea = new EntitySelectArea.EntitySelectArea(selectAreaInitializer);
            EntitySelectArea.DataItemSelected += EntitySelectArea_DataItemSelected;
            MainView = EntitySelectArea.View;

            //    ManageSecurity();
            //SecurityEdit = true;

            if (areaInitializer.DataInstance != null)
            {
                EntitySelectArea.EnableDisableSelectArea(false);
                EntitySelectArea.SelectData(areaInitializer.DataInstance);
            }


        }

        private void EntitySelectArea_DataItemSelected(object sender, EditAreaDataItemArg e)
        {
            if (e.DataItem != null)
            {
                // MainDataInstance = dataInstance;

                ArchiveView.EnableDisable(true);
                //WithRelatedItems = withRelatedItems;
                DataTreeArea = new MyUILibrary.DataTreeArea.DataTreeArea();
                DataTreeArea.ContextMenuLoaded += DataTreeArea_ContextMenuLoaded;
                DataTreeArea.DataAreaConfirmed += DataTreeArea_DataAreaConfirmed;
                var dataTreeInistializer = new DataTreeAreaInitializer();
                dataTreeInistializer.EntitiyID = EntitySelectArea.SelectedEntity.ID;
                dataTreeInistializer.RelationshipTailsLoaded = EntitySelectArea.SelectedEntity.LoadArchiveRelatedItems;
                dataTreeInistializer.FirstDataItem = e.DataItem.DataView;
                dataTreeInistializer.RelationshipTails = ArchiveRelationshipTails.Select(x => x.RelationshipTail).ToList();
                DataTreeArea.SetAreaInitializer(dataTreeInistializer);
                DataTreeArea.SelectAll();
                ArchiveView.DataTreeAreaEnabled = true;
                //  if (loadRelatedItems)
                ArchiveView.ShowDataTree(DataTreeArea.View);
                ShowFolders(true);
            }
            else
                ArchiveView.EnableDisable(false);

        }
        List<ArchiveRelationshipTailDTO> ArchiveRelationshipTails
        {
            get
            {
                return AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.GetArchiveRelationshipTails(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), EntitySelectArea.SelectedEntity.ID);
            }
        }
        I_DataTreeArea DataTreeArea { set; get; }
        //public DP_DataRepository MainDataInstance { set; get; }
        //  DP_DataView MainDataInstance { set; get; }
        //private void ShowDataItemArchives(DP_DataView dataInstance, bool loadRelatedItems)
        //{


        //}

        private void DataTreeArea_DataAreaConfirmed(object sender, EventArgs e)
        {
            RefreshFoldersAndItems();
        }

        private void DataTreeArea_ContextMenuLoaded(object sender, ContextMenuArg e)
        {

            if (e.ContextObject is DP_DataView)
            {
                var data = (e.ContextObject as DP_DataView);
                List<ContextMenuItem> menus = new List<ContextMenuItem>();
                var permissions = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetEntityAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), data.TargetEntityID, false);
                if (permissions.GrantedActions.Any(x => x == SecurityAction.ArchiveEdit))
                {
                    ContextMenuItem newMenu = new ContextMenuItem() { Title = "افزودن فایل" };
                    newMenu.Clicked += (sender1, e1) => AddMenu_Clicked(sender1, e1, data);
                    menus.Add(newMenu);

                }
                e.ContextMenuManager.SetMenuItems(menus);
            }


            //////if (e.DataTreeItem.ItemType == DataTreeItemType.DataItem)
            //////{
            //////    bool allowView = false;
            //////    bool allowEdit = false;
            //////    if (Permission.SecurityObjectID == e.DataTreeItem.DataItem.TargetEntityID)
            //////    {
            //////        allowView = Permission.GrantedActions.Any(x => x == SecurityAction.ArchiveView);
            //////        allowEdit = Permission.GrantedActions.Any(x => x == SecurityAction.ArchiveEdit);
            //////    }
            //////    else if (ArchiveRelationshipTails.Any(x => x.RelationshipTail.TargetEntityID == e.DataTreeItem.DataItem.TargetEntityID))
            //////    {
            //////        //دسترسی درست شود
            //////        var arcTail = ArchiveRelationshipTails.First(x => x.RelationshipTail.TargetEntityID == e.DataTreeItem.DataItem.TargetEntityID);
            //////        //allowView = arcTail.Permission.GrantedActions.Any(x => x == SecurityAction.ArchiveView);
            //////        //allowEdit = arcTail.Permission.GrantedActions.Any(x => x == SecurityAction.ArchiveEdit);
            //////    }
            //////    //if (allowView)
            //////    //{
            //////    //    var viewMenu = new DynamicContextMenu() { Title = "نمایش آرشیو" };
            //////    //    viewMenu.Clicked += (sender1, e1) => ViewMenu_Clicked(sender1, e1, e.DataTreeItem.DataItem);
            //////    //    e.ContextMenus.Add(viewMenu);
            //////    //}


            //////    //////if (allowEdit)
            //////    //////{
            //////    //////    var editMenu = new DynamicContextMenu() { Title = "افزودن فایل" };
            //////    //////    editMenu.Clicked += (sender1, e1) => EditMenu_Clicked(sender1, e1, e.DataTreeItem.DataItem);
            //////    //////    e.ContextMenus.Add(editMenu);
            //////    //////}
            //////}
        }


        private void AddMenu_Clicked(object sender, EventArgs e, DP_DataView dataItem)
        {
            AddArchiveItems(dataItem);
        }
        private void View_DataTreeRequested(object sender, EventArgs e)
        {
            ArchiveView.DataTreeVisibility = !ArchiveView.DataTreeVisibility;
        }


        //List<Tuple<ArchiveRelationshipTailDTO, List<DataItemDTO>>> ArchiveRelationshipTailDatas = new List<Tuple<ArchiveRelationshipTailDTO, List<DataItemDTO>>>();
        private void View_ArchiveItemDownloadRequested(object sender, EventArgs e)
        {
            var selectedItems = ArchiveView.SelectedArchiveItems;
            if (selectedItems.Count <= 10)
            {
                foreach (var archiveItemDataItem in selectedItems)
                {
                    if (archiveItemDataItem.AttechedFile == null || archiveItemDataItem.AttechedFile.Content == null)
                        archiveItemDataItem.AttechedFile = AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.GetAttachedFile(archiveItemDataItem.ID);
                    var fileName = archiveItemDataItem.AttechedFile.FileName + "_" + DateTime.Now.ToString().Replace(" ", "").Replace("/", "").Replace(":", "") + "." + archiveItemDataItem.AttechedFile.FileExtension;

                    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.DownloadFile(archiveItemDataItem.AttechedFile, false);
                }
            }
            else
            {
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("امکان دانلود بیش از 10 فایل وجود ندارد");

            }
        }

        private void View_ArchiveItemViewRequested(object sender, EventArgs e)
        {
            var selectedItems = ArchiveView.SelectedArchiveItems;
            if (selectedItems.Count > 1)
            {
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("تنها یک مورد به منظور نمایش انتخاب شود");
                return;
            }
            else if (selectedItems.Count == 1)
            {
                ViewArchiveItem(selectedItems.First());
            }
        }

        //private void ManageSecurity()
        //{
        //    if (!Permission.GrantedActions.Any(x => x == SecurityAction.ArchiveView || x == SecurityAction.ArchiveEdit))
        //    {
        //        SecurityNoAccess = true;
        //    }
        //    else
        //    {
        //        if (Permission.GrantedActions.Any(x => x == SecurityAction.ArchiveEdit))
        //        {
        //            SecurityEdit = true;
        //        }
        //        else if (Permission.GrantedActions.Any(x => x == SecurityAction.ArchiveView))
        //        {
        //            SecurityReadonly = true;
        //        }
        //        else
        //            SecurityNoAccess = true;
        //    }
        //    ImposeSecurity();
        //}

        //private void ImposeSecurity()
        //{
        //    if (SecurityNoAccess)
        //    {
        //        MainView = null;
        //        AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عدم دسترسی به آرشیو");
        //    }
        //    else
        //    {
        //        if (SecurityReadonly)
        //        {
        //            ArchiveView.ArchiveItemDelete = false;
        //            ArchiveView.ArchiveItemAdd = false;

        //        }
        //    }
        //}

        //AssignedPermissionDTO _Permission;
        //public AssignedPermissionDTO Permission
        //{
        //    get
        //    {
        //        if (_Permission == null)
        //            _Permission = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetEntityAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), EntitySelectArea.SelectedEntity.ID, false);
        //        return _Permission;
        //    }
        //}
        //bool WithRelatedItems { set; get; }

        private List<int> GetDataItemIds()
        {
            List<int> result = new List<int>();
            List<DP_DataView> list = DataTreeArea.GetSelectedDataItems();
            foreach (var item in list)
            {
                if (item.DataItemID != 0)
                    result.Add(item.DataItemID);
                else
                {
                    if (!item.DataItemSearched)
                    {
                        if (AgentUICoreMediator.GetAgentUICoreMediator.DataItemManager.SetDataItemDTO(item))
                            result.Add(item.DataItemID);
                    }
                }
            }
            return result;
        }
        private void View_ArchiveItemSelectedModeChanged(object sender, EventArgs e)
        {
            CheckArchiveItemToolbar();
        }

        private void CheckArchiveItemToolbar()
        {
            bool archiveItemInfo = false;
            bool multipleArchiveItemInfo = false;
            bool archiveItemDelete = false;
            bool archiveItemView = false;
            bool archiveItemDownload = false;
            if (ArchiveView.FolderTabIsSelected == true)
            {
                archiveItemInfo = false;
                multipleArchiveItemInfo = false;
                archiveItemDelete = false;
            }
            else if (ArchiveView.ArchiveItemsTabIsSelected == true)
            {
                if (ArchiveView.ArchiveItemsSelectedMode == ArchiveItemSelectedMode.None)
                {
                    archiveItemInfo = false;
                    multipleArchiveItemInfo = false;
                    archiveItemDelete = false;
                }
                else if (ArchiveView.ArchiveItemsSelectedMode == ArchiveItemSelectedMode.One)
                {
                    archiveItemInfo = true;
                    multipleArchiveItemInfo = false;
                    archiveItemDelete = CheckArchiveItemsDeleteEnabled(ArchiveView.SelectedArchiveItems);
                    var selectedItem = ArchiveView.SelectedArchiveItems.FirstOrDefault();
                    if (selectedItem != null && GetArchiveItemViewer(selectedItem.MainType, selectedItem.FileType) != null)
                        archiveItemView = true;
                    archiveItemDownload = true;
                }
                else if (ArchiveView.ArchiveItemsSelectedMode == ArchiveItemSelectedMode.Multiple)
                {
                    archiveItemInfo = false;
                    multipleArchiveItemInfo = true;
                    archiveItemDelete = CheckArchiveItemsDeleteEnabled(ArchiveView.SelectedArchiveItems);
                    archiveItemDownload = true;
                }
            }
            else
            {
                archiveItemInfo = false;
                multipleArchiveItemInfo = false;
                archiveItemDelete = false;
            }
            ArchiveView.ArchiveItemDownload = archiveItemDownload;
            ArchiveView.ArchiveItemView = archiveItemView;
            ArchiveView.ArchiveItemInfo = archiveItemInfo;
            ArchiveView.MultipleArchiveItemInfo = multipleArchiveItemInfo;
            ArchiveView.ArchiveItemDelete = archiveItemDelete;

        }

        private bool CheckArchiveItemsDeleteEnabled(List<ArchiveItemDTO> selectedArchiveItems)
        {
            foreach (var item in selectedArchiveItems.GroupBy(x => x.DatItem.TargetEntityID))
            {
                var permissions = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetEntityAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), item.Key, false);
                if (permissions.GrantedActions.Any(x => x == SecurityAction.ArchiveEdit))
                    return true;
            }
            return false;
        }

        private void View_FolderOrItemsTabChanged(object sender, EventArgs e)
        {
            CheckArchiveItemToolbar();
        }

        private void View_MultipleArchiveItemsInfoRequested(object sender, EventArgs e)
        {
            var selectedItems = ArchiveView.SelectedArchiveItems;
            if (selectedItems.Count > 0)
            {
                if (ViewMultipleArchiveInfo == null)
                {
                    ViewMultipleArchiveInfo = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateViewOfMultipleArchiveItemInfo();
                    ViewMultipleArchiveInfo.MultipleArchiveInfosConfirmed += ViewMultipleArchiveInfo_MultipleArchiveInfosConfirmed;
                }

                bool allowEdit = true;
                foreach (var item in selectedItems.GroupBy(x => x.DatItem.TargetEntityID))
                {
                    var permissions = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetEntityAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), item.Key, false);
                    if (!permissions.GrantedActions.Any(x => x == SecurityAction.ArchiveEdit))
                        allowEdit = false;
                }
                ViewMultipleArchiveInfo.AllowSave = allowEdit;

                var group = selectedItems.GroupBy(x => x.DatItem.TargetEntityID);
                bool oneEntity = group.Count() == 1;

                var folders = new List<ArchiveFolderWithNullDTO>();
                folders.Add(new ArchiveFolderWithNullDTO() { Name = "بدون تغییر", ID = -66 });
                folders.Add(new ArchiveFolderWithNullDTO() { Name = "بدون فولدر", ID = null });

                List<ArchiveFolderDTO> possibleFolders = null;
                if (oneEntity)
                    possibleFolders = AllFolders(selectedItems.First().DatItem.TargetEntityID);
                else
                    possibleFolders = GeneralFolders();

                possibleFolders.ForEach(x => folders.Add(new ArchiveFolderWithNullDTO() { ID = x.ID, Name = x.Name }));
                ViewMultipleArchiveInfo.SetFolders(folders);

                ViewMultipleArchiveInfo.SelectedFolder = -66;
                ViewMultipleArchiveInfo.CanChangeTags = false;
                ViewMultipleArchiveInfo.IDs = selectedItems.Select(x => x.ID).ToList();

                var tags = new List<ArchiveTagDTO>();
                if (oneEntity)
                    tags = AllTags(selectedItems.First().DatItem.TargetEntityID);
                else
                    tags = GeneralTags();
                //اینجا دوتا چیز ظاهرا ایرادد داره و بررسی بشه
                //اول اینکه بصورت پیش فرض تگهایی که تو همه آیتمها مشترک هستنتد انتخاب بشن
                //دوما در صورت ثبت تگهای اختصاصی و غیر مشترکی که فایلها دارند حذف میشوند و این عمومی های انتخاب شده جایزگین می شوند
                //هوشمند تر شود مثلا تگهای عمومی که دارند بیایند و در صورت انتخاب افزوده شوند و نه جایگزین کل تگها
                tags.ForEach(x => x.tmpSelect = false);
                ViewMultipleArchiveInfo.ArchiveTags = tags;
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(ViewMultipleArchiveInfo, "اطلاعات", Enum_WindowSize.Big);
            }
        }

        private void ViewMultipleArchiveInfo_MultipleArchiveInfosConfirmed(object sender, EventArgs e)
        {
            var view = sender as I_View_MultipleArchiveItemsInfo;
            if (view != null)
            {
                var selectedTagIds = view.ArchiveTags.Where(x => x.tmpSelect).Select(x => x.ID).ToList();
                bool changeFolder = view.SelectedFolder != -66;
                bool result = AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.UpdateMultipleArchiveItemInfo(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), view.IDs, changeFolder, view.SelectedFolder, view.CanChangeTags, selectedTagIds);
                RefreshFoldersAndItems();
            }
        }

        private void RefreshFoldersAndItems()
        {
            ShowFolders(false);
            ShowArchivedItems(false);
        }

        private void View_ArchiveItemRightCliked(object sender, ArchiveItemSelectedArg e)
        {
            ShowArchivedItemInfo(e.ArchiveItem);
        }

        private void View_ArchiveItemInfoRequested(object sender, EventArgs e)
        {
            var selectedItems = ArchiveView.SelectedArchiveItems;
            if (selectedItems.Count > 0)
            {
                var first = selectedItems.First();
                ShowArchivedItemInfo(first);
            }
        }
        private void ShowArchivedItemInfo(ArchiveItemDTO archiveItem)
        {
            if (ViewArchiveInfo == null)
            {
                ViewArchiveInfo = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateViewOfArchiveItemInfo();
                ViewArchiveInfo.ArchiveInfoConfirmed += ViewArchiveInfo_ArchiveInfoConfirmed;
                ViewArchiveInfo.CloseRequested += ViewArchiveInfo_CloseRequested;
            }
            var permissions = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetEntityAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), archiveItem.DatItem.TargetEntityID, false);
            ViewArchiveInfo.AllowSave = permissions.GrantedActions.Any(x => x == SecurityAction.ArchiveEdit);

            var folders = new List<ArchiveFolderWithNullDTO>();
            folders.Add(new ArchiveFolderWithNullDTO() { Name = "بدون فولدر", ID = null });
            AllFolders(archiveItem.DatItem.TargetEntityID).ForEach(x => folders.Add(new ArchiveFolderWithNullDTO() { ID = x.ID, Name = x.Name }));
            ViewArchiveInfo.SetFolders(folders);
            ViewArchiveInfo.ID = archiveItem.ID;
            var allTags = AllTags(archiveItem.DatItem.TargetEntityID);
            allTags.ForEach(x => x.tmpSelect = archiveItem.TagIDs.Contains(x.ID));
            ViewArchiveInfo.ArchiveTags = allTags;
            ViewArchiveInfo.ItemID = archiveItem.ID;
            ViewArchiveInfo.ItemName = archiveItem.Name;
            ViewArchiveInfo.CreateDate = archiveItem.CreationDate;
            var user = AgentUICoreMediator.GetAgentUICoreMediator.userManagerService.GetUser(archiveItem.UserID);
            ViewArchiveInfo.UserRealName = user.FullName;
            //var relatedLogs = AgentUICoreMediator.GetAgentUICoreMediator.logManagerService.SearchDataLogs(archiveItem.ID, DataLogType.ArchiveInsert).FirstOrDefault();
            //if (relatedLogs != null)
            //    ViewArchiveInfo.UserRealName = relatedLogs.vwUserInfo;
            ViewArchiveInfo.SelectedFolder = archiveItem.FolderID;
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(ViewArchiveInfo, "اطلاعات", Enum_WindowSize.Big);
        }

        private void ViewArchiveInfo_CloseRequested(object sender, EventArgs e)
        {
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.CloseDialog(sender);
        }

        private void ViewArchiveInfo_ArchiveInfoConfirmed(object sender, EventArgs e)
        {
            var view = sender as I_View_ArchiveItemInfo;
            if (view != null)
            {
                var selectedTagIds = view.ArchiveTags.Where(x => x.tmpSelect).Select(x => x.ID).ToList();
                bool result = AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.UpdateArchiveItemInfo(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), view.ID, view.ItemName, view.SelectedFolder, selectedTagIds);
                RefreshFoldersAndItems();
            }
        }

        private void View_ArchiveItemDeleteRequested(object sender, EventArgs e)
        {
            var selectedItems = ArchiveView.SelectedArchiveItems;
            if (selectedItems.Count > 0)
            {
                var selectedCount = selectedItems.Count;
                List<int> allowedEntityIds = new List<int>();
                foreach (var item in selectedItems.GroupBy(x => x.DatItem.TargetEntityID))
                {
                    var permissions = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetEntityAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), item.Key, false);
                    if (permissions.GrantedActions.Any(x => x == SecurityAction.ArchiveEdit))
                        allowedEntityIds.Add(item.Key);
                }
                selectedItems = selectedItems.Where(x => allowedEntityIds.Contains(x.DatItem.TargetEntityID)).ToList();
                var allowedCount = selectedItems.Count;
                if (selectedItems.Count > 0)
                {
                    var text = "";
                    if (selectedCount == allowedCount)
                        text = "آیا " + selectedCount + " " + "مورد انتخاب شده حذف شوند؟";
                    else
                    {
                        text = "از" + " " + selectedCount + " " + "فایل انتخاب شده تنها اجازه حذف" + " " + allowedCount + " " + "موجود است. آیا حذف شوند؟";
                    }
                    var ask = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowConfirm("تایید", text, UserPromptMode.YesNo);
                    if (ask == Temp.ConfirmResul.Yes)
                    {
                        var result = AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.DeleteArchiveItems(selectedItems.Select(x => x.ID).ToList(), AgentUICoreMediator.GetAgentUICoreMediator.GetRequester());
                        AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowMessage("پیام", result.Message, result.Details);

                        RefreshFoldersAndItems();
                    }
                }
                else
                    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.ShowInfo("عدم دسترسی به حذف فایلهای آرشیو");
            }
        }

        private void View_ArchiveTagFilterRequested(object sender, EventArgs e)
        {

            if (ViewArchiveTagFilter == null)
            {
                ViewArchiveTagFilter = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateViewOfArchiveTagFilter();
                ViewArchiveTagFilter.ArchiveTagsFiltered += ViewArchiveTagFilter_ArchiveTagsFiltered;
            }
            //if (AllArchiveTags == null)

            var allArchiveItemsTagIds = AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.GetArchiveItemsTags(GetDataItemIds());
            if (FilteredArchiveTags == null)
                FilteredArchiveTags = new List<int>();

            //if (AllTags == null)
            //    AllTags = AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.GetArchiveTags( AreaInitializer.EntityID, true);

            List<ArchiveTagDTO> allArchivedItemsTags = new List<ArchiveTagDTO>();
            var allTags = AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.GetArchiveTags(GetEntityIDs(), true);

            foreach (var id in allArchiveItemsTagIds)
            {
                ArchiveTagDTO nItem = new ArchiveTagDTO();
                nItem.ID = id;
                if (allTags.Any(x => x.ID == id))
                {
                    var dbTag = allTags.First(x => x.ID == id);
                    nItem.EntityID = dbTag.EntityID;
                    nItem.Name = dbTag.Name;
                }
                else
                    nItem.Name = id.ToString();
                nItem.tmpSelect = FilteredArchiveTags.Contains(nItem.ID);
                allArchivedItemsTags.Add(nItem);
            }
            ViewArchiveTagFilter.ArchiveTags = allArchivedItemsTags;
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(ViewArchiveTagFilter, "فیلتر تگ", Enum_WindowSize.Big);

        }

        private void ViewArchiveTagFilter_ArchiveTagsFiltered(object sender, EventArgs e)
        {
            var view = sender as I_View_ArchiveTagFiltered;
            var selectedTag = view.ArchiveTags.Where(x => x.tmpSelect);
            FiltersDecided(selectedTag.ToList());
        }

        private void FiltersDecided(List<ArchiveTagDTO> list)
        {

            FilteredArchiveTags = list.Select(x => x.ID).ToList();
            if (FilteredArchiveTags.Count() > 0)
            {
                var title = "";
                foreach (var archiveTag in list)
                {
                    title += (title == "" ? "" : ",") + archiveTag.Name;
                }
                ArchiveView.ShowFilteredTags(title);
            }
            else
                ArchiveView.ClearFilteredTags();

            ArchiveView.FilteresClear = FilteredArchiveTags.Any();

            RefreshFoldersAndItems();
        }

        private void View_ArchiveTagFilterClearRequested(object sender, EventArgs e)
        {
            FiltersDecided(new List<ArchiveTagDTO>());
        }


        //private void View_ArchiveTagRequested(object sender, EventArgs e)
        //{
        //    if (ArchiveView.SelectedArchiveItems.Count > 0)
        //    {
        //        var archiveItem = ArchiveView.SelectedArchiveItems.First();
        //        if (ViewArchiveTag == null)
        //        {
        //            ViewArchiveTag = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateViewOfArchiveTag();
        //            ViewArchiveTag.ArchiveTagsConfirmed += ViewArchiveTag_ArchiveTagsConfirmed;
        //        }
        //        ViewArchiveTag.ArchiveItemDataItem = archiveItem;
        //        if (AllArchiveTags == null)
        //            AllArchiveTags = AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.GetArchiveTags( AreaInitializer.EntityID);

        //        //var archiveItemTagIds = AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.GetArchiveITemTagIds(ViewArchiveTag.ArchiveItem.ID);
        //        AllArchiveTags.ForEach(x => x.tmpSelect = archiveItem.ArchiveItem.TagIDs.Contains(x.ID));
        //        ViewArchiveTag.ArchiveTags = AllArchiveTags;

        //        AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(ViewArchiveTag, "تگ ها");
        //    }
        //}



        //private void ViewArchiveTag_ArchiveTagsConfirmed(object sender, EventArgs e)
        //{
        //    var view = sender as I_View_ArchiveTag;
        //    var selectedTagIds = view.ArchiveTags.Where(x => x.tmpSelect).Select(x => x.ID).ToList();
        //    AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.UpdateArhiveItemTags(view.ArchiveItemDataItem.ID, selectedTagIds);
        //    //   view.ArchiveItem.TagIDs = AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.GetArchiveITemTagIds(ViewArchiveTag.ArchiveItem.ID);
        //    view.ArchiveItemDataItem.ArchiveItem.TagIDs = selectedTagIds;
        //}
        DP_DataView singleDataItem = null;
        private void ShowFolders(bool activateFolderTab)
        {
            singleDataItem = null;
            var selectedDatas = DataTreeArea.GetSelectedDataItems();
            if (selectedDatas.Count == 0 || selectedDatas.Count > 1)
            {
                ArchiveView.ArchiveItemAdd = false;
                foreach (var item in selectedDatas.GroupBy(x => x.TargetEntityID))
                {
                    var permissions = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetEntityAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), item.Key, false);
                    if (permissions.GrantedActions.Any(x => x == SecurityAction.ArchiveEdit))
                        ArchiveView.ArchiveItemDelete = true;
                }
            }
            else
            {
                var permissions = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetEntityAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), selectedDatas.First().TargetEntityID, false);
                if (permissions.GrantedActions.Any(x => x == SecurityAction.ArchiveEdit))
                {
                    singleDataItem = selectedDatas.First();
                    ArchiveView.ArchiveItemAdd = true;
                    ArchiveView.ArchiveItemDelete = true;
                }
                else
                {
                    ArchiveView.ArchiveItemAdd = false;
                    ArchiveView.ArchiveItemDelete = false;
                }
            }

            var AllFolders = AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.GetArchiveFolders(GetEntityIDs(), true);
            var archivedFolders = AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.GetArchivedItemsFolderID(GetDataItemIds(), FilteredArchiveTags);
            if (ValidFolders == null)
                ValidFolders = new List<ArchiveFolderWithNullDTO>();
            ValidFolders.Clear();

            var allDocumentsFolder = new ArchiveFolderWithNullDTO();
            allDocumentsFolder.Name = "همه اسناد";
            allDocumentsFolder.ID = -99;
            allDocumentsFolder.tmpCount = archivedFolders.Sum(x => x.Item2);
            ValidFolders.Add(allDocumentsFolder);
            //bool hasFolder = false;
            foreach (var item in archivedFolders)
            {
                //hasFolder = true;
                var folder = new ArchiveFolderWithNullDTO();
                folder.ID = item.Item1;
                if (folder.ID != null)
                {
                    var inAllFolder = AllFolders.FirstOrDefault(x => x.ID == item.Item1.Value);
                    if (inAllFolder != null)
                    {
                        folder.Name = inAllFolder.Name;
                        folder.EntityID = inAllFolder.EntityID;
                    }
                    else
                        folder.Name = folder.ID.ToString();
                }
                else
                    folder.Name = "بدون فولدر";
                folder.tmpCount = item.Item2;
                ValidFolders.Add(folder);
            }

            ArchiveView.ClearFolders();
            ArchiveView.ShowFolders(ValidFolders, activateFolderTab);
        }
        List<TableDrivedEntityDTO> entities = new List<TableDrivedEntityDTO>();

        private TableDrivedEntityDTO GetSimpleEntity(int entityID)
        {
            if (entities.Any(x => x.ID == entityID))
                return entities.First(x => x.ID == entityID);
            else
            {
                var entity = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetSimpleEntity(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), entityID);
                entities.Add(entity);
                return entity;
            }
        }


        private List<int> GetEntityIDs()
        {
            return DataTreeArea.GetSelectedDataItems().GroupBy(x => x.TargetEntityID).Select(x => x.Key).ToList();
        }

        private void View_FolderDoubleCliked(object sender, FolderSelectedArg e)
        {
            SetCurrentFolder(e.FolderID);
        }

        private void SetCurrentFolder(int? folderID)
        {
            CurrentFolder = ValidFolders.FirstOrDefault(x => x.ID == folderID);
            ShowArchivedItems(true);
        }

        private void ShowArchivedItems(bool activateFileTab)
        {
            if (CurrentFolder != null)
            {
                ShowArchivedItems(CurrentFolder.ID, CurrentFolder.Name, activateFileTab);
            }
        }
        private void ShowArchivedItems(int? folderID, string title, bool activateFileTab)
        {
            if (folderID == -99)
                ArchivedItems = AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.GetArchiveItemsAllFolders(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), GetDataItemIds());
            else
                ArchivedItems = AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.GetArchiveItems(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), GetDataItemIds(), folderID);

            foreach (var item in ArchivedItems)
            {
                var tooltip = "شناسه فایل" + ":" + " " + item.ID;
                tooltip += Environment.NewLine + "نام فایل" + ":" + " " + item.Name;
                tooltip += Environment.NewLine + "داده مرتبط" + ":" + " " + GetSimpleEntity(item.DatItem.TargetEntityID).Alias + ", " + item.DatItem.ViewInfo;
                tooltip += Environment.NewLine + "تاریخ ایجاد" + ":" + " " + item.CreationDate;
                item.Tooltip = tooltip;

                item.Color = GetSimpleEntity(item.DatItem.TargetEntityID).Color;
            }
            ArchiveView.ShowArchiveItems(title, ArchivedItems, activateFileTab);

            foreach (var archiveItem in ArchivedItems)
            {
                if (FilteredArchiveTags != null && FilteredArchiveTags.Any())
                    ArchiveView.ChangeArchiveItemVisibility(archiveItem, archiveItem.TagIDs.Any(y => FilteredArchiveTags.Contains(y)));
                else
                    ArchiveView.ChangeArchiveItemVisibility(archiveItem, true);
            }
        }

        private void View_ArchiveItemDoubleCliked(object sender, ArchiveItemSelectedArg e)
        {
            ViewArchiveItem(e.ArchiveItem);
        }

        private void ViewArchiveItem(ArchiveItemDTO archiveItemDataItem)
        {
            if (archiveItemDataItem.AttechedFile == null || archiveItemDataItem.AttechedFile.Content == null)
                archiveItemDataItem.AttechedFile = AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.GetAttachedFile(archiveItemDataItem.ID);
            if (archiveItemDataItem.AttechedFile != null && archiveItemDataItem.AttechedFile.Content != null)
            {
                I_View_ViewArchiveItem viewer = GetArchiveItemViewer(archiveItemDataItem.MainType, archiveItemDataItem.FileType);

                if (viewer != null)
                {
                    var permissions = AgentUICoreMediator.GetAgentUICoreMediator.tableDrivedEntityManagerService.GetEntityAssignedPermissions(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), archiveItemDataItem.DatItem.TargetEntityID, false);
                    viewer.AllowSave = permissions.GrantedActions.Any(x => x == SecurityAction.ArchiveEdit);

                    viewer.ArchiveItemDataItem = archiveItemDataItem;
                    viewer.ShowArchiveItem();
                    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(viewer, "نمایش", Enum_WindowSize.Maximized);
                }
                else
                {
                    var fileName = archiveItemDataItem.AttechedFile.FileName + "_" + DateTime.Now.ToString().Replace(" ", "").Replace("/", "").Replace(":", "") + "." + archiveItemDataItem.AttechedFile.FileExtension;
                    AgentUICoreMediator.GetAgentUICoreMediator.UIManager.DownloadFile(archiveItemDataItem.AttechedFile, false);
                }
            }
        }

        private I_View_ViewArchiveItem GetArchiveItemViewer(Enum_ArchiveItemMainType mainType, Enum_ArchiveItemFileType fileType)
        {
            //if (mainType == Enum_ArchiveItemMainType.Image)
            //{
            if (ArchiveItemViewers == null)
                ArchiveItemViewers = new List<Tuple<List<Enum_ArchiveItemFileType>, I_View_ViewArchiveItem>>();
            if (ArchiveItemViewers.Any(x => x.Item1.Any(y => y == fileType)))
            {
                return ArchiveItemViewers.First(x => x.Item1.Any(y => y == fileType)).Item2;
            }
            else
            {
                Tuple<List<Enum_ArchiveItemFileType>, I_View_ViewArchiveItem> archiveItemViewer = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetArhiveItemViewer(mainType, fileType);
                if (archiveItemViewer != null)
                {
                    archiveItemViewer.Item2.DownloadItemClicked += Item2_DownloadItemClicked;
                    archiveItemViewer.Item2.NextItemClicked += Item2_NextItemClicked;
                    archiveItemViewer.Item2.PreviousItemClicked += Item2_PreviousItemClicked;
                    archiveItemViewer.Item2.SaveItemClicked += Item2_SaveItemClicked;
                    ArchiveItemViewers.Add(archiveItemViewer);
                    return archiveItemViewer.Item2;
                }
            }
            return null;
            //}
            //return null;
        }

        private void Item2_SaveItemClicked(object sender, EventArgs e)
        {
            if (sender is I_View_ViewArchiveItem)
            {
                var viewer = (sender as I_View_ViewArchiveItem);
                var binary = viewer.GetImage();
                var result = AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.UpdateArchiveItemFileBinary(AgentUICoreMediator.GetAgentUICoreMediator.GetRequester(), viewer.ArchiveItemDataItem.ID, binary);
                if (result)
                {
                    if (CurrentFolder != null)
                    {
                        ShowArchivedItems(false);
                    }
                }
            }
        }

        private void Item2_PreviousItemClicked(object sender, EventArgs e)
        {
            if (sender is I_View_ViewArchiveItem)
            {
                var viewer = (sender as I_View_ViewArchiveItem);
                var foundArchiveItem = GetViewerPreviousArchiveItem(viewer.ArchiveItemDataItem, viewer);
                if (foundArchiveItem != null)
                {
                    if (foundArchiveItem.AttechedFile == null || foundArchiveItem.AttechedFile.Content == null)
                        foundArchiveItem.AttechedFile = AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.GetAttachedFile(foundArchiveItem.ID);
                    viewer.ArchiveItemDataItem = foundArchiveItem;
                    viewer.ShowArchiveItem();
                }

            }
        }

        private ArchiveItemDTO GetViewerPreviousArchiveItem(ArchiveItemDTO archiveItem, I_View_ViewArchiveItem viewer)
        {
            if (ArchivedItems.Any(x => x == viewer.ArchiveItemDataItem))
            {
                var index = ArchivedItems.IndexOf(archiveItem);
                if (index != 0)
                {
                    var previndex = index - 1;
                    var previous = ArchivedItems[previndex];
                    var fViewer = ArchiveItemViewers.First(x => x.Item2 == viewer);
                    if (fViewer.Item1.Contains(previous.FileType))
                        return previous;
                    else
                    {
                        //while (index == 0)
                        //{
                        //index--;
                        return GetViewerPreviousArchiveItem(previous, viewer);
                        //}
                    }
                }
            }
            return null;

        }

        private void Item2_NextItemClicked(object sender, EventArgs e)
        {
            if (sender is I_View_ViewArchiveItem)
            {
                var viewer = (sender as I_View_ViewArchiveItem);
                var foundArchiveItem = GetViewerNextArchiveItem(viewer.ArchiveItemDataItem, viewer);
                if (foundArchiveItem != null)
                {
                    if (foundArchiveItem.AttechedFile == null || foundArchiveItem.AttechedFile.Content == null)
                        foundArchiveItem.AttechedFile = AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.GetAttachedFile(foundArchiveItem.ID);
                    viewer.ArchiveItemDataItem = foundArchiveItem;
                    viewer.ShowArchiveItem();
                }

            }

        }
        private ArchiveItemDTO GetViewerNextArchiveItem(ArchiveItemDTO archiveItem, I_View_ViewArchiveItem viewer)
        {
            if (ArchivedItems.Any(x => x == viewer.ArchiveItemDataItem))
            {
                var index = ArchivedItems.IndexOf(archiveItem);
                if (index != ArchivedItems.Count - 1)
                {
                    var nextindex = index + 1;
                    var next = ArchivedItems[nextindex];
                    var fViewer = ArchiveItemViewers.First(x => x.Item2 == viewer);
                    if (fViewer.Item1.Contains(next.FileType))
                        return next;
                    else
                    {
                        //while (index == 0)
                        //{
                        //index--;
                        return GetViewerNextArchiveItem(next, viewer);
                        //}
                    }
                }
            }
            return null;

        }
        private void Item2_DownloadItemClicked(object sender, EventArgs e)
        {
            if (sender is I_View_ViewArchiveItem)
            {
                var viewer = (sender as I_View_ViewArchiveItem);
                if (viewer.ArchiveItemDataItem.AttechedFile == null || viewer.ArchiveItemDataItem.AttechedFile.Content == null)
                    viewer.ArchiveItemDataItem.AttechedFile = AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.GetAttachedFile(viewer.ArchiveItemDataItem.ID);
                AgentUICoreMediator.GetAgentUICoreMediator.UIManager.DownloadFile(viewer.ArchiveItemDataItem.AttechedFile, false);
            }
        }

        private bool ViewerExists(Enum_ArchiveItemMainType mainType)
        {
            return mainType == Enum_ArchiveItemMainType.Image;
        }



        private void ViewAddArchiveItems_FileTagsRequested(object sender, FileTagsRequestedArg e)
        {

            var ViewFileTagFilter = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateViewOfArchiveTagFilter();
            ViewFileTagFilter.ArchiveTagsFiltered += (sender1, e1) => ViewFileTagFilter_ArchiveTagsFiltered(sender1, e1, e.File);

            List<ArchiveTagDTO> allTags = new List<ArchiveTagDTO>();

            var AllTags = AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.GetArchiveTags(addDataInstance.TargetEntityID, true);
            foreach (var item in AllTags)
            {
                ArchiveTagDTO nItem = new ArchiveTagDTO();
                nItem.ID = item.ID;
                nItem.EntityID = item.EntityID;
                nItem.Name = item.Name;
                nItem.tmpSelect = e.File.tmpTagIDs.Contains(nItem.ID);
                allTags.Add(nItem);
            }
            ViewFileTagFilter.ArchiveTags = allTags;
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(ViewFileTagFilter, "فیلتر تگ", Enum_WindowSize.Big);

        }

        private void ViewFileTagFilter_ArchiveTagsFiltered(object sender, EventArgs e, FileRepositoryDTO file)
        {
            var view = sender as I_View_ArchiveTagFiltered;
            file.tmpTagIDs = view.ArchiveTags.Where(x => x.tmpSelect).Select(x => x.ID).ToList(); ;
            file.tmpTags = "";
            foreach (var item in view.ArchiveTags.Where(x => x.tmpSelect))
                file.tmpTags += (file.tmpTags == "" ? "" : ",") + item.Name;
        }

        private void SetExtensions()
        {
            ViewAddArchiveItems.Extentions = AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.GetArchiveExtentions();
        }
        private void View_AddNewRequested(object sender, EventArgs e)
        {
            if (singleDataItem != null)
                AddArchiveItems(singleDataItem);
        }

        DP_DataView addDataInstance = null;
        private void AddArchiveItems(DP_DataView mainDataInstance)
        {
            addDataInstance = mainDataInstance;
            if (ViewAddArchiveItems == null)
            {
                ViewAddArchiveItems = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateViewOfAddArchiveItem();
                ViewAddArchiveItems.FilesConfirmed += ViewAddArchiveItems_FilesConfirmed;
                ViewAddArchiveItems.FileTagsRequested += ViewAddArchiveItems_FileTagsRequested;
                SetExtensions();
            }
            List<ArchiveFolderWithNullDTO> folders = new List<ArchiveFolderWithNullDTO>();

            folders.Add(new ArchiveFolderWithNullDTO() { ID = null, Name = " " });
            foreach (var item in AllFolders(mainDataInstance.TargetEntityID))
            {
                ArchiveFolderWithNullDTO nITem = new ArchiveFolderWithNullDTO();
                nITem.ID = item.ID;
                nITem.Name = item.Name;
                folders.Add(nITem);
            }
            ViewAddArchiveItems.Folders = folders;
            ViewAddArchiveItems.SelectedFolder = (CurrentFolder == null ? null : CurrentFolder.ID);
            ViewAddArchiveItems.ClearItems();
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(ViewAddArchiveItems, "افزودن فایل", Enum_WindowSize.Big);
        }
        private void ViewAddArchiveItems_FilesConfirmed(object sender, EventArgs e)
        {
            if (ViewAddArchiveItems.Files.Any())
            {
                foreach (var item in ViewAddArchiveItems.Files)
                {
                    if (item.tmpState == FileRepositoryState.Normal
                        || item.tmpState == FileRepositoryState.Failed)
                    {
                        ArchiveItemDTO archiveItem = new ArchiveItemDTO();
                        //شاتباهه
                        archiveItem.DatItem = addDataInstance;
                        archiveItem.FolderID = ViewAddArchiveItems.SelectedFolder;
                        archiveItem.TagIDs = item.tmpTagIDs;
                        archiveItem.Name = item.FileName;
                        archiveItem.AttechedFile = item;
                        var result = AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.CreateArchiveItems(archiveItem, AgentUICoreMediator.GetAgentUICoreMediator.GetRequester());
                        if (result.Result)
                        {
                            item.tmpState = FileRepositoryState.Succeed;

                        }
                        else
                        {
                            item.tmpState = FileRepositoryState.Failed;
                        }
                        item.tmpUploadMessage = result.Message;
                    }
                }

                ViewAddArchiveItems.RefreshFiles();
                RefreshFoldersAndItems();
            }
        }

        //public int  AreaInitializer.EntityID
        //{
        //    set; get;
        //}

        public List<EntityInstanceProperty> KeyProperties
        {
            set; get;
        }
        public I_View_AddArchiveItems ViewAddArchiveItems
        {
            set; get;
        }
        public I_View_ArchiveArea ArchiveView
        {
            set; get;
        }
        public I_View_MultipleArchiveItemsInfo ViewMultipleArchiveInfo { set; get; }
        public I_View_ArchiveItemInfo ViewArchiveInfo { set; get; }
        List<Tuple<List<Enum_ArchiveItemFileType>, I_View_ViewArchiveItem>> ArchiveItemViewers { set; get; }
        public ArchiveFolderWithNullDTO CurrentFolder { set; get; }
        public List<ArchiveFolderDTO> AllFolders(int entityID)
        {
            return AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.GetArchiveFolders(new List<int>() { entityID }, true);
        }
        public List<ArchiveFolderDTO> GeneralFolders()
        {
            return AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.GetGeneralFolders();
        }
        public List<ArchiveTagDTO> AllTags(int entityID)
        {
            return AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.GetArchiveTags(entityID, true);
        }
        public List<ArchiveTagDTO> GeneralTags()
        {
            return AgentUICoreMediator.GetAgentUICoreMediator.ArchiveManager.GeneralTags();

        }
        public List<ArchiveFolderWithNullDTO> ValidFolders { get; private set; }
        public List<ArchiveItemDTO> ArchivedItems { get; private set; }
        //public I_View_ArchiveTag ViewArchiveTag { get; private set; }
        //public I_View_ArchiveTag ViewFileTag { get; private set; }
        //public I_View_ArchiveTagFiltered ViewFileTagFilter { get; private set; }
        public I_View_ArchiveTagFiltered ViewArchiveTagFilter { get; private set; }
        //public List<ArchiveTagDTO> AllArchiveTags { get; private set; }
        public List<int> FilteredArchiveTags { get; private set; }
    }
}
