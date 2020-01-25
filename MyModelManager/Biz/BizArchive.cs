using DataAccess;
using ModelEntites;
using MyGeneralLibrary;

using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizArchive
    {
        //public ArchiveTagDTO GetArchiveTag(int tagID)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var dbTag = projectContext.ArchiveTag.FirstOrDefault(x => x.ID == tagID);
        //        if (dbTag != null)
        //            return ToArchiveTagDTO(dbTag);
        //        else return null;
        //    }
        //}   
        SecurityHelper securityHelper = new SecurityHelper();
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        //public bool DataIsAccessable(DR_Requester requester, int entityID)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var permission = bizTableDrivedEntity.GetEntityAssignedPermissions(requester, entityID, false);
        //        if (permission.GrantedActions.Any(x => x == SecurityAction.ArchiveView || x == SecurityAction.ArchiveEdit))
        //            return true;
        //        else
        //            return false;
        //    }
        //}
        public List<ArchiveTagDTO> GetArchiveTags(List<int> entityIDs, bool withGeneralTags)
        {
            List<ArchiveTagDTO> result = new List<ArchiveTagDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                IQueryable<ArchiveTag> list;
                if (withGeneralTags)
                    list = projectContext.ArchiveTag.Where(x => x.TableDrivedEntityID == null || (x.TableDrivedEntityID != null && entityIDs.Contains(x.TableDrivedEntityID.Value)));
                else
                    list = projectContext.ArchiveTag.Where(x => (x.TableDrivedEntityID != null && entityIDs.Contains(x.TableDrivedEntityID.Value)));

                foreach (var item in list)
                    result.Add(ToArchiveTagDTO(item));

            }
            return result;
        }
        public List<ArchiveTagDTO> GetArchiveTags(int? entityID, bool withGeneralTags)
        {
            List<ArchiveTagDTO> result = new List<ArchiveTagDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                IQueryable<ArchiveTag> list;
                if (withGeneralTags)
                    list = projectContext.ArchiveTag.Where(x => x.TableDrivedEntityID == null || x.TableDrivedEntityID == entityID);
                else
                    list = projectContext.ArchiveTag.Where(x => x.TableDrivedEntityID == entityID);

                foreach (var item in list)
                    result.Add(ToArchiveTagDTO(item));

            }
            return result;
        }
        public List<ArchiveTagDTO> GetGeneralTags()
        {
            List<ArchiveTagDTO> result = new List<ArchiveTagDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {

                var list = projectContext.ArchiveTag.Where(x => x.TableDrivedEntityID == null);


                foreach (var item in list)
                    result.Add(ToArchiveTagDTO(item));

            }
            return result;
        }

        BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();

        public List<ArchiveRelationshipTailDTO> GetArchiveRelationshipTails(DR_Requester requester, int entityID, bool withDetails)
        {
            List<ArchiveRelationshipTailDTO> result = new List<ArchiveRelationshipTailDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var list = projectContext.EntityArchiveRelationshipTails.Where(x => x.TableDrivedEntityID == entityID);
                foreach (var item in list)
                {
                    if (bizEntityRelationshipTail.DataIsAccessable(requester, item.EntityRelationshipTail))
                        if (bizTableDrivedEntity.DataIsAccessable(requester, item.EntityRelationshipTail.TableDrivedEntity, new List<SecurityAction>() { SecurityAction.ArchiveView, SecurityAction.ArchiveEdit }))
                            result.Add(ToArchiveRelationshipTailDTO(item, withDetails));
                }
            }
            return result;
        }

        private ArchiveRelationshipTailDTO ToArchiveRelationshipTailDTO(EntityArchiveRelationshipTails item, bool withDetails)
        {
            ArchiveRelationshipTailDTO result = new ArchiveRelationshipTailDTO();
            result.EntityID = item.TableDrivedEntityID;
            result.ID = item.TableDrivedEntityID;
            result.RelationshipTailID = item.EntityRelationshipTailID;
            if (withDetails)
            {
                BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
                result.RelationshipTail = bizEntityRelationshipTail.ToEntityRelationshipTailDTO(item.EntityRelationshipTail);
            }
            return result;
        }

        public ArchiveTagDTO ToArchiveTagDTO(ArchiveTag item)
        {
            ArchiveTagDTO result = new ArchiveTagDTO();
            result.ID = item.ID;
            result.Name = item.Name;
            result.EntityID = item.TableDrivedEntityID ?? 0;
            return result;
        }

        public bool UpdateArchiveRelationshipTails(int entityID, List<ArchiveRelationshipTailDTO> list)
        {

            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var entity = projectContext.TableDrivedEntity.First(x => x.ID == entityID);
                while (entity.EntityArchiveRelationshipTails.Any(x => x.TableDrivedEntityID == entityID))
                    projectContext.EntityArchiveRelationshipTails.Remove(entity.EntityArchiveRelationshipTails.First(x => x.TableDrivedEntityID == entityID));
                foreach (var item in list)
                {
                    EntityArchiveRelationshipTails dbItem = new EntityArchiveRelationshipTails();
                    dbItem.TableDrivedEntityID = entityID;
                    dbItem.EntityRelationshipTailID = item.RelationshipTailID;
                    projectContext.EntityArchiveRelationshipTails.Add(dbItem);
                }
                projectContext.SaveChanges();
            }
            return true;

        }

        //public bool LoadArchiveRelatedItems(int entityID)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        return projectContext.TableDrivedEntity.First(x => x.ID == entityID).LoadArchiveRelatedItems==true;
        //    }
        //    return true;
        //}

        public bool UpdateArchiveTag(ArchiveTagDTO message)
        {
            List<ArchiveTagDTO> result = new List<ArchiveTagDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                ArchiveTag dbitem = null;
                if (message.ID != 0)
                    dbitem = projectContext.ArchiveTag.First(x => x.ID == message.ID);
                else
                    dbitem = new ArchiveTag();
                dbitem.Name = message.Name;
                dbitem.TableDrivedEntityID = (message.EntityID == 0 ? (int?)null : message.EntityID);
                if (dbitem.ID == 0)
                    projectContext.ArchiveTag.Add(dbitem);
                projectContext.SaveChanges();
            }
            return true;
        }

        public List<ArchiveFolderDTO> GetGeneralFolders()
        {
            List<ArchiveFolderDTO> result = new List<ArchiveFolderDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {

                IQueryable<ArchiveFolder> list = projectContext.ArchiveFolder.Where(x => x.TableDrivedEntityID == null);


                foreach (var item in list)
                    result.Add(ToArchiveFolderDTO(item));

            }
            return result;
        }
        public List<ArchiveFolderDTO> GetArchiveFolders(List<int> entityIDs, bool withGeneralFolders)
        {
            List<ArchiveFolderDTO> result = new List<ArchiveFolderDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {

                IQueryable<ArchiveFolder> list;
                if (withGeneralFolders)
                    list = projectContext.ArchiveFolder.Where(x => x.TableDrivedEntityID == null || (x.TableDrivedEntityID != null && entityIDs.Contains(x.TableDrivedEntityID.Value)));
                else
                    list = projectContext.ArchiveFolder.Where(x => x.TableDrivedEntityID != null && entityIDs.Contains(x.TableDrivedEntityID.Value));

                foreach (var item in list)
                    result.Add(ToArchiveFolderDTO(item));

            }
            return result;
        }
        public List<ArchiveFolderDTO> GetArchiveFolders(int? entityID, bool withGeneralFolders)
        {
            List<ArchiveFolderDTO> result = new List<ArchiveFolderDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {

                IQueryable<ArchiveFolder> list;
                if (withGeneralFolders)
                    list = projectContext.ArchiveFolder.Where(x => x.TableDrivedEntityID == null || x.TableDrivedEntityID == entityID);
                else
                    list = projectContext.ArchiveFolder.Where(x => x.TableDrivedEntityID == entityID);

                foreach (var item in list)
                    result.Add(ToArchiveFolderDTO(item));

            }
            return result;
        }

        public List<ArchiveTagDTO> GetArchiveItemsTags(DataItemDTO dataItem)
        {
            throw new NotImplementedException();
        }

        private ArchiveFolderDTO ToArchiveFolderDTO(ArchiveFolder item)
        {
            ArchiveFolderDTO result = new ArchiveFolderDTO();
            result.ID = item.ID;
            result.Name = item.Name;
            result.EntityID = item.TableDrivedEntityID ?? 0;

            return result;
        }

        public bool UpdateArchiveFolder(ArchiveFolderDTO message)
        {
            List<ArchiveFolderDTO> result = new List<ArchiveFolderDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                ArchiveFolder dbitem = null;
                if (message.ID != 0)
                    dbitem = projectContext.ArchiveFolder.First(x => x.ID == message.ID);
                else
                    dbitem = new ArchiveFolder();
                dbitem.Name = message.Name;
                dbitem.TableDrivedEntityID = (message.EntityID == 0 ? (int?)null : message.EntityID);
                if (dbitem.ID == 0)
                    projectContext.ArchiveFolder.Add(dbitem);
                projectContext.SaveChanges();
            }
            return true;
        }
    }

}
