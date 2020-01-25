using ModelEntites;
using MyCodeFunctionLibrary;
using MyDataManagerBusiness;

using MyFormulaFunctionStateFunctionLibrary;
using MyInterfaces;
using MyDataItemManager;
using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProxyLibrary.Request;

namespace MyFormulaManagerService
{
    public class ArchiveManagerService
    {
        BizArchive bizArchive = new BizArchive();
        BizArchiveManager bizArchiveManager = new BizArchiveManager();
        public List<ArchiveItemDTO> GetArchiveItemsAllFolders(DR_Requester requester, List<int> dataitemIDS)
        {
            return bizArchiveManager.GetArchiveItemsAllFolders(requester, dataitemIDS);
        }
        public List<ArchiveItemDTO> GetArchiveItems(DR_Requester requester, List<int> dataitemIDS, int? folderID)
        {
            return bizArchiveManager.GetArchiveItems(requester, dataitemIDS, folderID);
        }

        public ArchiveResult CreateArchiveItems(ArchiveItemDTO archiveItem, DR_Requester requester)
        {
            return bizArchiveManager.CreateArchiveItems(archiveItem, requester);
        }

        public ArchiveItemDTO GetArchiveItem(DR_Requester requester, int ArchiveItemID)
        {
            return bizArchiveManager.GetArchiveItem(requester, ArchiveItemID);
        }

        public List<ArchiveFolderDTO> GetArchiveFolders(List<int> entityIDs, bool withGeneralFolders)
        {
            return bizArchive.GetArchiveFolders(entityIDs, withGeneralFolders);
        }
        public List<ArchiveFolderDTO> GetGeneralFolders()
        {
            return bizArchive.GetGeneralFolders();
        }
        public List<Tuple<int?, int>> GetArchivedItemsFolderID(List<int> dataitemIDS, List<int> tagIDs)
        {
            return bizArchiveManager.GetArchivedItemsFolderIDs(dataitemIDS, tagIDs);
        }

        public FileRepositoryDTO GetAttachedFile(int iD)
        {
            return bizArchiveManager.GetAttachedFile(iD);
        }

        public List<Tuple<string, List<string>>> GetArchiveExtentions()
        {
            return bizArchiveManager.GetArchiveExtentions();
        }
        public List<ArchiveTagDTO> GetArchiveTags(List<int> entityIDs, bool withGeneralTags)
        {
            return bizArchive.GetArchiveTags(entityIDs, withGeneralTags);
        }
        public List<ArchiveTagDTO> GetArchiveTags(int entityID, bool withGeneralTags)
        {
            return bizArchive.GetArchiveTags(entityID, withGeneralTags);
        }
        public List<ArchiveTagDTO> GeneralTags()
        {
            return bizArchive.GetGeneralTags();
        }

        public List<int> GetArchiveItemsTags(List<int> dataitemIds)
        {
            return bizArchiveManager.GetArchiveItemsTags(dataitemIds);
        }
        public bool UpdateArchiveItemFileBinary(DR_Requester requester,int iD, byte[] file)
        {
            return bizArchiveManager.UpdateArchiveItemFileBinary(requester, iD, file);
        }

        public ArchiveDeleteResult DeleteArchiveItems(List<int> items, DR_Requester requester)
        {
            return bizArchiveManager.DeleteArchiveItemDataItems(items, requester);
        }

        public List<ArchiveRelationshipTailDTO> GetArchiveRelationshipTails(DR_Requester requester, int entityID)
        {
            return bizArchive.GetArchiveRelationshipTails(requester, entityID, true);
        }

        public List<int> GetArchiveITemTagIds(int iD)
        {
            return bizArchiveManager.GetArchiveItemTagIds(iD);
        }

        //public bool UpdateArhiveItemTags(int iD, List<int> selectedTags)
        //{
        //    return bizArchiveManager.UpdateArhiveItemTags(iD, selectedTags);
        //}

        public bool UpdateArchiveItemInfo(DR_Requester requester, int iD, string name, int? folderID, List<int> tagIDs)
        {
            return bizArchiveManager.UpdateArchiveItemInfo(requester,iD, name, folderID, tagIDs);
        }

        public bool UpdateMultipleArchiveItemInfo(DR_Requester requester, List<int> IDs, bool changeFolder, int? folderID, bool changeTagIDs, List<int> selectedTagIds)
        {
            return bizArchiveManager.UpdateMultipleArchiveItemInfo(requester,IDs, changeFolder, folderID, changeTagIDs, selectedTagIds);
        }

        //public bool LoadArchiveRelatedItems(int entityID)
        //{
        //     return bizArchive.LoadArchiveRelatedItems(entityID);
        //}
    }
}
