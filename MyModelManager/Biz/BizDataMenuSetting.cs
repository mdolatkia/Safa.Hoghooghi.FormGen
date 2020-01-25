using DataAccess;
using ModelEntites;
using MyCacheManager;
using MyGeneralLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProxyLibrary;


namespace MyModelManager
{
    public class BizDataMenuSetting
    {
        BizProcess bizProcess = new BizProcess();
        BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
        BizEntityReport bizEntityReport = new BizEntityReport();
        SecurityHelper securityHelper = new SecurityHelper();
        BizEntitySearchableReport bizEntitySearchableReport = new BizEntitySearchableReport();
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        public DataMenuSettingDTO GetDataMenuSetting(DR_Requester requester, int entitiyID,bool withDetails)
        {
           
            DataMenuSettingDTO result = new DataMenuSettingDTO();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var entity = projectContext.TableDrivedEntity.FirstOrDefault(x => x.ID == entitiyID);
                if (entity.DataMenuSetting != null)
                {
                    foreach (var item in entity.DataMenuSetting.DataMenuReportRelationship)
                    {
                        //دسترسی به خود گزارش هم کنترل شود
                        if (bizEntityRelationshipTail.DataIsAccessable(requester, item.EntityRelationshipTail))
                        {
                            if (bizEntityReport.DataIsAccessable(requester, item.EntitySearchableReport.EntityReport))
                            {
                                var tail = bizEntityRelationshipTail.ToEntityRelationshipTailDTO( item.EntityRelationshipTail);
                                var entityReportDTO = new EntitySearchableReportDTO();
                                bizEntitySearchableReport.ToEntitySearchableReportDTO(item.EntitySearchableReport, entityReportDTO, withDetails);
                                result.ReportRelationships.Add(ToDataMenuReportRelationshipDTO(item, tail, entityReportDTO));
                            }
                        }
                    }

                    foreach (var item in entity.DataMenuSetting.DataMenuDataViewRelationship)
                    {
                        if (bizEntityRelationshipTail.DataIsAccessable(requester, item.EntityRelationshipTail))
                        {
                            var tail = bizEntityRelationshipTail.ToEntityRelationshipTailDTO( item.EntityRelationshipTail);
                            result.DataViewRelationships.Add(ToDataMenuDataViewRelationshipDTO(item, tail));
                        }
                    }

                    foreach (var item in entity.DataMenuSetting.DataMenuGridViewRelationship)
                    {
                        if (bizEntityRelationshipTail.DataIsAccessable(requester, item.EntityRelationshipTail))
                        {
                            var tail = bizEntityRelationshipTail.ToEntityRelationshipTailDTO( item.EntityRelationshipTail);
                            result.GridViewRelationships.Add(ToDataMenuGridViewRelationshipDTO(item, tail));
                        }
                    }
                }
            }
            return result;
        }
        private DataMenuGridViewRelationshipDTO ToDataMenuGridViewRelationshipDTO(DataMenuGridViewRelationship dbRel, EntityRelationshipTailDTO tail)
        {
            var rel = new DataMenuGridViewRelationshipDTO();
            rel.ID = dbRel.ID;
            rel.RelationshipTailID = dbRel.EntityRelationshipTailID;
            rel.RelationshipTail = tail;
            rel.Group1 = dbRel.Group1;
            return rel;
        }

        private DataMenuDataViewRelationshipDTO ToDataMenuDataViewRelationshipDTO(DataMenuDataViewRelationship dbRel, EntityRelationshipTailDTO tail)
        {
            var rel = new DataMenuDataViewRelationshipDTO();
            rel.ID = dbRel.ID;
            rel.RelationshipTailID = dbRel.EntityRelationshipTailID;
            rel.RelationshipTail = tail;
            rel.Group1 = dbRel.Group1;
            return rel;
        }

        private DataMenuReportRelationshipDTO ToDataMenuReportRelationshipDTO(DataMenuReportRelationship dbRel, EntityRelationshipTailDTO tail, EntitySearchableReportDTO entityReport)
        {
            var result = new DataMenuReportRelationshipDTO();
            result.ID = dbRel.ID;
            result.RelationshipTailID = dbRel.EntityRelationshipTailID;
            result.RelationshipTail = tail;
            BizEntityReport bizEntityReport = new BizEntityReport();
            result.EntitySearchableReportID = dbRel.EntitySearchableReportID;
            result.EntityReport = entityReport;
            result.Group1 = dbRel.Group1;
            return result;
        }

        public List<DataMenu> GetDataMenu(DR_Requester requester, DP_DataView dataItem)
        {
            List<DataMenu> result = new List<DataMenu>();
            var entityID = dataItem.TargetEntityID;
            var simpleEntity = bizTableDrivedEntity.GetSimpleEntity(requester, entityID);
            if (simpleEntity == null)
                return null;
            if (simpleEntity.IsView)
            {
                var fullEntity = bizTableDrivedEntity.GetTableDrivedEntity(requester, entityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithRelationships);
                if (fullEntity.Relationships.Any())
                {
                    foreach (var relationsip in fullEntity.Relationships)
                    {
                        var dataView = CreateDataView(dataItem, relationsip);
                        var menu = AddMenu(result, relationsip.Alias, "", DataMenuType.ViewRel);
                        menu.ViewRelTargetDataItem = dataView;
                    }
                }
            }
            else
            {

                var dataEntryRootMenu = AddMenu(result, "نمایش/اصلاح داده", "", DataMenuType.Form);

                //آرشیو داده
               // BizArchive bizArchive = new BizArchive();
                if (bizTableDrivedEntity.DataIsAccessable(requester, entityID, new List<SecurityAction>() { SecurityAction.ArchiveView, SecurityAction.ArchiveEdit }))
                {
                    var archiveRootMenu = AddMenu(result, "آرشیو", "", DataMenuType.Archive);
                }


                //نامه های داده
              //  BizLetterTemplate bizLetterTemplate = new BizLetterTemplate();
                if (bizTableDrivedEntity.DataIsAccessable(requester, entityID,new List<SecurityAction>() { SecurityAction.LetterView, SecurityAction.LetterEdit }))
                {
                    var letterRootMenu = AddMenu(result, "نامه ها", "", DataMenuType.Letter);
                }
                //جریان کارهای مرتبط
                if (bizProcess.EntityHasAnyProcess(entityID))
                {
                    var workflowRootMenu = AddMenu(result, "جریان کار", "", DataMenuType.Workflow);
                }


                //گزارش مستقیم روی خود داده موجودیت
                BizEntityDirectReport bizEntityDirectReport = new BizEntityDirectReport();
                var directReports = bizEntityDirectReport.GetEntityDirectReports(requester, entityID);
                if (directReports.Any())
                {
                    var directReportRootMenu = AddMenu(result, "گزارش مستقیم", "", DataMenuType.Folder);
                    foreach (var directReport in directReports)
                    {
                        var directReportMenu = AddMenu(directReportRootMenu.SubMenus, directReport.ReportTitle, "", DataMenuType.DirectReport);
                        directReportMenu.DirectReport = directReport;
                    }
                }

                //لینک های داده
                BizDataLink bizDataLink = new MyModelManager.BizDataLink();
                var datalinks = bizDataLink.GetDataLinkByEntitiyID(requester, entityID);
                if (datalinks.Any())
                {
                    var dataViewRootMenu = AddMenu(result, "لینک داده", "", DataMenuType.Folder);
                    foreach (var datalink in datalinks)
                    {
                        var datalinkMenu = AddMenu(dataViewRootMenu.SubMenus, datalink.Name, "", DataMenuType.DataLink);
                        datalinkMenu.Datalink = datalink;
                    }
                }

                //گزارشهای داده های مرتبط
                var dataMenuSetting = GetDataMenuSetting(requester, entityID,true);
                if (dataMenuSetting.ReportRelationships.Any())
                {
                    var relationshipReportRootMenu = AddMenu(result, "گزارش داده های مرتبط", "", DataMenuType.Folder);
                    foreach (var group in dataMenuSetting.ReportRelationships.GroupBy(x => x.Group1 ?? ""))
                    {
                        DataMenu parentGroupMenu = GetGroupMenu(relationshipReportRootMenu, group.Key);
                        foreach (var rel in group)
                        {
                            var menu = AddMenu(parentGroupMenu.SubMenus, rel.EntityReport.ReportTitle, "", DataMenuType.RelationshipTailSearchableReport);
                            menu.ReportRelationshipTail = rel;
                        }
                    }
                }

                //نمای داده های مرتبط
                if (dataMenuSetting.DataViewRelationships.Any())
                {
                    var dataViewRootMenu = AddMenu(result, "نمایش داده های مرتبط", "", DataMenuType.Folder);
                    foreach (var group in dataMenuSetting.DataViewRelationships.GroupBy(x => x.Group1 ?? ""))
                    {
                        DataMenu parentGroupMenu = GetGroupMenu(dataViewRootMenu, group.Key);
                        foreach (var rel in group)
                        {
                            var dataViewRelMenu = AddMenu(parentGroupMenu.SubMenus, rel.RelationshipTail.TargetEntityAlias, rel.RelationshipTail.EntityPath, DataMenuType.RelationshipTailDataView);
                            dataViewRelMenu.DataviewRelationshipTail = rel.RelationshipTail;
                        }
                    }
                }

                if (dataMenuSetting.GridViewRelationships.Any())
                {
                    var gridViewRootMenu = AddMenu(result, "گرید داده های مرتبط", "", DataMenuType.Folder);
                    foreach (var group in dataMenuSetting.GridViewRelationships.GroupBy(x => x.Group1 ?? ""))
                    {
                        DataMenu parentGroupMenu = GetGroupMenu(gridViewRootMenu, group.Key);
                        foreach (var rel in group)
                        {
                            var gridViewRelMenu = AddMenu(parentGroupMenu.SubMenus, rel.RelationshipTail.TargetEntityAlias, rel.RelationshipTail.EntityPath, DataMenuType.RelationshipTailDataGrid);
                            gridViewRelMenu.GridviewRelationshipTail = rel.RelationshipTail;
                        }
                    }
                }
            }
            return result;
        }
        private DataMenu GetGroupMenu(DataMenu parentMenu, string key)
        {
            if (string.IsNullOrEmpty(key))
                return parentMenu;
            else
            {
                if (parentMenu.SubMenus.Any(x => x.Title == key))
                    return parentMenu.SubMenus.First(x => x.Title == key);
                else
                {
                    var subMenu = AddMenu(parentMenu.SubMenus, key, "", DataMenuType.Folder);
                    return subMenu;
                }
            }
        }
        DP_DataView CreateDataView(DP_DataView sourceDataItem, RelationshipDTO relationsip)
        {
            var dataView = new DP_DataView(relationsip.EntityID2, relationsip.Entity2);
            foreach (var relCol in relationsip.RelationshipColumns)
            {
                var property = sourceDataItem.GetProperty(relCol.FirstSideColumnID);
                if (property == null)
                    return null;
                dataView.Properties.Add(new EntityInstanceProperty(relCol.SecondSideColumn) { Value = sourceDataItem.GetProperty(relCol.FirstSideColumnID).Value });
            }
            return dataView;
        }
        private DataMenu AddMenu(List<DataMenu> result, string title, string tooltip, DataMenuType type)
        {
            var dataMenu = new DataMenu();
            dataMenu.Title = title;
            dataMenu.Tooltip = tooltip;
            dataMenu.Type = type;
            result.Add(dataMenu);
            return dataMenu;
        }

        //public List<EntityReportDataMenuRelationshipsDTO> GetEntityRelationshipReports(int entityID, bool withDetails)
        //{
        //    List<EntityReportDataMenuRelationshipsDTO> result = new List<EntityReportDataMenuRelationshipsDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var entityReportSetting = projectContext.EntityReportDataMenuSetting.FirstOrDefault(x => x.ID == entityID);
        //        if (entityReportSetting != null)
        //        {
        //            foreach (var item in entityReportSetting.EntityReportDataMenuRelationships)
        //            {
        //                result.Add(ToEntityReportDataMenuRelationshipsDTO(item, withDetails));
        //            }
        //        }
        //    }
        //    return result;
        //}

        //private EntityReportDataMenuRelationshipsDTO ToEntityReportDataMenuRelationshipsDTO(EntityReportDataMenuRelationships dbRel, bool withDetails)
        //{

        //}



        public void UpdateEntityReportDataMenuSettings(int entityID, DataMenuSettingDTO message)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbEntity = projectContext.TableDrivedEntity.First(x => x.ID == entityID);

                if (dbEntity.DataMenuSetting == null)
                {
                    dbEntity.DataMenuSetting = new DataMenuSetting();
                }
                while (dbEntity.DataMenuSetting.DataMenuReportRelationship.Any())
                    projectContext.DataMenuReportRelationship.Remove(dbEntity.DataMenuSetting.DataMenuReportRelationship.First());
                foreach (var item in message.ReportRelationships)
                {
                    DataMenuReportRelationship dbRel = new DataMenuReportRelationship();
                    //  dbRel.RelationshipID = (item.RelationshipID == 0 ? (int?)null : item.RelationshipID);
                    dbRel.EntityRelationshipTailID = item.RelationshipTailID;
                    dbRel.EntitySearchableReportID = item.EntitySearchableReportID;
                    dbRel.Group1 = item.Group1;
                    dbEntity.DataMenuSetting.DataMenuReportRelationship.Add(dbRel);
                }

                while (dbEntity.DataMenuSetting.DataMenuDataViewRelationship.Any())
                    projectContext.DataMenuDataViewRelationship.Remove(dbEntity.DataMenuSetting.DataMenuDataViewRelationship.First());
                foreach (var item in message.DataViewRelationships)
                {
                    DataMenuDataViewRelationship dbRel = new DataMenuDataViewRelationship();
                    dbRel.EntityRelationshipTailID = item.RelationshipTailID;
                    dbRel.Group1 = item.Group1;
                    dbEntity.DataMenuSetting.DataMenuDataViewRelationship.Add(dbRel);

                }

                while (dbEntity.DataMenuSetting.DataMenuGridViewRelationship.Any())
                    projectContext.DataMenuGridViewRelationship.Remove(dbEntity.DataMenuSetting.DataMenuGridViewRelationship.First());
                foreach (var item in message.GridViewRelationships)
                {
                    DataMenuGridViewRelationship dbRel = new DataMenuGridViewRelationship();
                    dbRel.EntityRelationshipTailID = item.RelationshipTailID;
                    dbRel.Group1 = item.Group1;
                    dbEntity.DataMenuSetting.DataMenuGridViewRelationship.Add(dbRel);

                }

                //dbEntity.EntityReportDataMenuSetting.IconContent = message.IconContent;
                projectContext.SaveChanges();
            }
        }
    }

}
