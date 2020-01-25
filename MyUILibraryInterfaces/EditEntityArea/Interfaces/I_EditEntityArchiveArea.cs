using CommonDefinitions.UISettings;
using ModelEntites;
using MyUILibrary.EntityArea.Commands;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using MyUILibraryInterfaces.DataTreeArea;

using MyUILibraryInterfaces.EntityArea;

namespace MyUILibrary.EntityArea
{

    public class ArchiveAreaInitializer
    {
        public int EntityID { set; get; }
        //public bool UserCanChangeDataItem { set; get; }
        //public bool LoadRelatedItems { set; get; }
        public DP_DataView DataInstance { set; get; }
    }
    public interface I_EntityArchiveArea
    {
        //I_EntitySelectArea EntitySelectArea { set; get; }
        ArchiveAreaInitializer AreaInitializer { set; get; }
        //DP_DataRepository MainDataInstance { set; get; }
        //I_View_ArchiveArea ArchiveView { set; get; }

        //I_View_EntitySelectArea SelectAreaView { set; get; }

        object MainView { set; get; }
        //void ShowDataItemArchives(DP_DataRepository dataInstance, bool loadRelatedItems);
    }

    public interface I_View_ArchiveArea
    {
        void ShowFolders(List<ArchiveFolderWithNullDTO> folders, bool activateFolderTab);
        void ShowArchiveItems(string title, List<ArchiveItemDTO> items, bool activateFileTab);
        event EventHandler<FolderSelectedArg> FolderDoubleClicked;
        event EventHandler ArchiveItemAddNewRequested;
        event EventHandler ArchiveItemInfoRequested;
        event EventHandler ArchiveItemsDeleteRequested;
        event EventHandler ArchiveItemSelectedModeChanged;
        event EventHandler FolderOrItemsTabChanged;
        event EventHandler ArchiveTagFilterRequested;
        event EventHandler ArchiveTagFilterClearRequested;
        event EventHandler DataTreeRequested;
        event EventHandler ArchiveItemViewRequested;
        event EventHandler ArchiveItemDownloadRequested;
        event EventHandler MultipleArchiveItemsInfoRequested;
        event EventHandler<ArchiveItemSelectedArg> ArchiveItemDoubleCliked;
        event EventHandler<ArchiveItemSelectedArg> ArchiveItemRightCliked;
        ArchiveItemSelectedMode ArchiveItemsSelectedMode { get; }
        bool FolderTabIsSelected { get; }
        bool ArchiveItemsTabIsSelected { get; }
        List<ArchiveFolderDTO> SelectedFolders { set; get; }
        List<ArchiveItemDTO> SelectedArchiveItems { set; get; }
        //void HideDataTree();
        void EnableDisable(bool enable);
        void ClearFolders();
        void ClearFiles();
        void ChangeArchiveItemVisibility(ArchiveItemDTO archiveItem, bool v);
        bool ArchiveItemInfo { set; get; }
        bool ArchiveItemDelete { set; get; }
        bool ArchiveItemAdd { set; get; }
        bool MultipleArchiveItemInfo { set; get; }
        bool ArchiveItemView { get; set; }

        void ShowDataTree(I_DataTreeView view);

        bool ArchiveItemDownload { get; set; }
        bool FilteresClear { get; set; }
        bool DataTreeAreaEnabled { get; set; }
        bool DataTreeVisibility { get; set; }

        void ClearFilteredTags();
        void ShowFilteredTags(string title);
    }
    public interface I_View_AddArchiveItems
    {
        void RefreshFiles();
        List<Tuple<string, List<string>>> Extentions { set; get; }
        List<FileRepositoryDTO> Files { set; get; }
        List<ArchiveFolderWithNullDTO> Folders { set; }
        int? SelectedFolder { get; set; }
        event EventHandler FilesConfirmed;
        event EventHandler<FileTagsRequestedArg> FileTagsRequested;
        void ClearItems();

    }
    public class FileTagsRequestedArg : EventArgs
    {
        public FileRepositoryDTO File { set; get; }
    }
    public interface I_View_ArchiveItemInfo
    {
        //ArchiveItemDTO Message { set; get; }
        //void ShowInfo(string Name, int folderID);
        bool AllowSave { set; get; }
        int ID { set; get; }
        int? SelectedFolder { set; get; }
        string ItemName { set; get; }
        event EventHandler ArchiveInfoConfirmed;
        event EventHandler CloseRequested;
        void SetFolders(List<ArchiveFolderWithNullDTO> items);
        List<ArchiveTagDTO> ArchiveTags { set; get; }
        int ItemID { set; }
        DateTime CreateDate { set; }
        string UserRealName { set; }
    }
    public interface I_View_MultipleArchiveItemsInfo
    {
        int? SelectedFolder { set; get; }
        bool CanChangeTags { set; get; }
        List<int> IDs { set; get; }
        event EventHandler MultipleArchiveInfosConfirmed;
        void SetFolders(List<ArchiveFolderWithNullDTO> items);
        List<ArchiveTagDTO> ArchiveTags { set; get; }
        bool AllowSave { get; set; }
    }
    public class ArchiveItemInfoConfirmedArg : EventArgs
    {
        public string Name { set; get; }
        public int FolderID { set; get; }
        //public List<int> TagIDs { set; get; }
    }
    public class MultipleArchiveItemsInfoConfirmedArg : EventArgs
    {
        //public int FolderID { set; get; }
    }
    public interface I_View_ArchiveTag
    {
        List<ArchiveTagDTO> ArchiveTags { get; set; }
        ArchiveItemDTO ArchiveItemDataItem { get; set; }

        event EventHandler ArchiveTagsConfirmed;
    }
    public interface I_View_ArchiveTagFiltered
    {
        List<ArchiveTagDTO> ArchiveTags { set; get; }
        event EventHandler ArchiveTagsFiltered;
    }
    public interface I_View_ViewArchiveItem
    {
        event EventHandler NextItemClicked;
        event EventHandler PreviousItemClicked;
        event EventHandler SaveItemClicked;
        event EventHandler DownloadItemClicked;
          bool AllowSave { set; get; }
        byte[] GetImage();
        ArchiveItemDTO ArchiveItemDataItem { set; get; }
        void ShowArchiveItem();
    }
    public class FolderSelectedArg : EventArgs
    {
        public int? FolderID { set; get; }
    }
    public class ArchiveItemSelectedArg : EventArgs
    {
        public ArchiveItemDTO ArchiveItem { set; get; }
    }
    //public class ArchiveTabChangedArg : EventArgs
    //{
    //    public bool? FolderOrItemsTab;
    //}
    //public class ArchiveItemSelectedModeArg : EventArgs
    //{
    //    public ArchiveItemSelectedMode Mode { set; get; }
    //}
    public enum ArchiveItemSelectedMode
    {
        None,
        One,
        Multiple

    }

    //public class EntityArchiveRelationshipTail
    //{
    //    public EntityRelationshipTailDTO RelationshipTail { set; get; }
    //    public AssignedPermissionDTO Permission { set; get; }

    //}
}
