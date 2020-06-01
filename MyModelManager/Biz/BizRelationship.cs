using AutoMapper;
using DataAccess;
using ModelEntites;
using MyCacheManager;
using MyDataManagerBusiness;
using MyGeneralLibrary;
using MyRuleEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProxyLibrary;


namespace MyModelManager
{
    public class BizRelationship
    {
        public event EventHandler<ItemImportingStartedArg> ItemImportingStarted;

        public bool ReverseRelationshipIsMandatory(int relationshipID)
        {
            using (var context = new MyProjectEntities())
            {

                var reverseRelatoinship = context.Relationship.First(x => x.RelationshipID == relationshipID || x.Relationship1.Any(y => y.ID == relationshipID));
                //حالات دیگر افزوده شود
                if (reverseRelatoinship.RelationshipType.ManyToOneRelationshipType != null)
                {
                    return reverseRelatoinship.RelationshipType.IsOtherSideMandatory;
                }
                else if (reverseRelatoinship.RelationshipType.ExplicitOneToOneRelationshipType != null)
                {
                    return reverseRelatoinship.RelationshipType.IsOtherSideMandatory;

                }
                else if (reverseRelatoinship.RelationshipType.SubToSuperRelationshipType != null)
                {
                    return true;
                }
                else if (reverseRelatoinship.RelationshipType.UnionToSubUnionRelationshipType != null && reverseRelatoinship.RelationshipType.UnionToSubUnionRelationshipType.UnionRelationshipType.UnionHoldsKeys)
                {
                    return true;
                }
                else if (reverseRelatoinship.RelationshipType.SubUnionToUnionRelationshipType != null && !reverseRelatoinship.RelationshipType.SubUnionToUnionRelationshipType.UnionRelationshipType.UnionHoldsKeys)
                {
                    return true;
                }

            }
            throw (new Exception("جالات دیگر"));

        }



        public List<Tuple<ColumnDTO, ColumnDTO>> GetRelationshipColumns(int sourceRelationshipID)
        {
            BizColumn bizColumn = new BizColumn();
            List<Tuple<ColumnDTO, ColumnDTO>> result = new List<Tuple<ColumnDTO, ColumnDTO>>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var relationship = projectContext.Relationship.First(x => x.ID == sourceRelationshipID);
                foreach (var relcolumn in relationship.RelationshipColumns)
                {
                    result.Add(new Tuple<ColumnDTO, ColumnDTO>(bizColumn.ToColumnDTO(relcolumn.Column, true), bizColumn.ToColumnDTO(relcolumn.Column1, true)));
                }
            }
            return result;
        }

        private IQueryable<Relationship> GetRelationships(MyProjectEntities projectContext)
        {
            return projectContext.Relationship
                                 .Include("TableDrivedEntity.Table.DBSchema.DatabaseInformation.DBServer.LinkedServer")
                                 .Include("TableDrivedEntity1.Table.DBSchema.DatabaseInformation.DBServer.LinkedServer")
                                 .Include("RelationshipColumns.Column")
                                 .Include("RelationshipType").Where(x => x.Removed != true);
        }
        public List<RelationshipDTO> GetOrginalRelationships(int databaseID)
        {
            List<RelationshipDTO> result = new List<RelationshipDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var relationships = GetRelationships(projectContext)
                    .Where(x => x.MasterTypeEnum == (byte)Enum_MasterRelationshipType.FromPrimartyToForeign && x.IsOrginal && x.TableDrivedEntity1.Table.DBSchema.DatabaseInformationID == databaseID).ToList();
                foreach (var relationship in relationships)
                {
                    if (ItemImportingStarted != null)
                        ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = "Fetching relationship" + " " + relationship.Name, TotalProgressCount = relationships.Count(), CurrentProgress = relationships.IndexOf(relationship) + 1 });
                    result.Add(ToRelationshipDTO(relationship));

                }
            }
            return result;
        }

        //public bool OrginalRelationshipExists(RelationshipDTO item, int databaseID)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {

        //    }
        //}

        //public string OrginalRelationshipEntitiesExists(RelationshipDTO relationship, int databaseID)
        //{

        //}

        //public List<RelationshipDTO> GetEnabledOrginalRelationships(int databaseID)
        //{
        //    List<RelationshipDTO> result = new List<RelationshipDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var dbRel = GetRelationships(projectContext).Where(x => x.Enabled == true && x.MasterTypeEnum == (byte)Enum_MasterRelationshipType.FromPrimartyToForeign && x.TableDrivedEntity.Table.DBSchema.DatabaseInformationID == databaseID && x.TableDrivedEntity1.Table.DBSchema.DatabaseInformationID == databaseID && x.Created != true);
        //        foreach (var item in dbRel)
        //            result.Add(ToRelationshipDTO(item));
        //    }
        //    return result;
        //}
        public List<RelationshipDTO> GetEnabledRelationships(int databaseID)
        {
            List<RelationshipDTO> result = new List<RelationshipDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbRel = GetRelationships(projectContext).Where(x => x.TableDrivedEntity.Table.DBSchema.DatabaseInformationID == databaseID && x.TableDrivedEntity1.Table.DBSchema.DatabaseInformationID == databaseID && x.Created != true);
                foreach (var item in dbRel)
                    result.Add(ToRelationshipDTO(item));
            }
            return result;
        }
        //public RelationshipDTO GetOrginalRelationship(RelationshipDTO item, int databaseID)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var dbRel = GetRelationships(projectContext).First(x => x.TableDrivedEntity.Table.Name == item.Entity1 && x.TableDrivedEntity1.Table.Name == item.Entity2
        //          && x.TableDrivedEntity.Table.DBSchema.DatabaseInformationID == databaseID && x.TableDrivedEntity1.Table.DBSchema.DatabaseInformationID == databaseID
        //          && x.RelationshipColumns.All(z => item.RelationshipColumns.Any(y => z.Column.Name == y.FirstSideColumn.Name))
        //          && item.RelationshipColumns.All(z => x.RelationshipColumns.Any(y => z.FirstSideColumn.Name == y.Column.Name)));
        //        return ToRelationshipDTO(dbRel);
        //    }
        //}



        //public int GetOtherSideTableID(int relationshipID)
        //{
        //    RelationshipDTO result = new RelationshipDTO();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var relationship = projectContext.Relationship.First(x => x.ID == relationshipID);
        //        return relationship.TableDrivedEntity1.TableID;
        //    }
        //}
        public int GetOtherSideEntityID(int relationshipID)
        {
            RelationshipDTO result = new RelationshipDTO();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var relationship = projectContext.Relationship.First(x => x.ID == relationshipID);
                return relationship.TableDrivedEntityID2;
            }
        }
        public RelationshipDTO GetRelationship(int relationshipID)
        {
            //var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.Relationship, relationshipID.ToString());
            //if (cachedItem != null)
            //    return (cachedItem as RelationshipDTO);

            RelationshipDTO result = new RelationshipDTO();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var relationship = GetRelationships(projectContext).First(x => x.ID == relationshipID);
                return ToRelationshipDTO(relationship);
            }
        }
        public RelationshipDTO GetReverseRelationship(int relationshipID)
        {
            RelationshipDTO result = new RelationshipDTO();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbRelationship = GetRelationships(projectContext).First(x => x.ID == relationshipID);
                if (dbRelationship.Relationship2 != null)
                    return ToRelationshipDTO(dbRelationship.Relationship2);
                else
                    return ToRelationshipDTO(projectContext.Relationship.First(x => x.RelationshipID == relationshipID));
            }
        }

        public int CreateUpdateRelationship(DR_Requester requester, RelationshipDTO message, bool secondSideDataEntry)
        {
            //int createdID = 0;
            //bool newItem = message.ID == 0;
            BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
            var entity1 = bizTableDrivedEntity.GetTableDrivedEntity(requester, message.EntityID1, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
            var entity2 = bizTableDrivedEntity.GetTableDrivedEntity(requester, message.EntityID2, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
            bool viewRelation = false;
            if (entity1.IsView)
            {
                throw new Exception("Asasasd");
            }
            if (entity2.IsView)
            {
                viewRelation = true;
            }
            ModelDataHelper dataHelper = new ModelDataHelper();
            foreach (var relCol in message.RelationshipColumns)
            {
                relCol.FirstSideColumn = entity1.Columns.First(x => x.ID == relCol.FirstSideColumnID);
                relCol.SecondSideColumn = entity2.Columns.First(x => x.ID == relCol.SecondSideColumnID);
            }
            if (!viewRelation)
            {

                if (message.CreateType == CreateRelationshipType.OneToOne)
                {
                    var relationInfo = dataHelper.GetRelationshipsInfo(message, entity1, entity2);
                    if (relationInfo.MoreThanOneFkForEachPK == true)
                    {
                        throw new Exception("بعلت وجود چندین داده امکان انتخاب نوع یک به یک وجود ندارد");
                    }
                }

            }
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                Relationship dbRelationship = null;
                if (message.ID == 0)
                {
                    dbRelationship = new Relationship();
                    dbRelationship.MasterTypeEnum = (byte)Enum_MasterRelationshipType.FromPrimartyToForeign;
                    dbRelationship.SecurityObject = new SecurityObject();
                    dbRelationship.SecurityObject.Type = (int)DatabaseObjectCategory.Relationship;
                    dbRelationship.Alias = entity2.Alias;
                    projectContext.Relationship.Add(dbRelationship);

                }
                else
                    dbRelationship = projectContext.Relationship.First(x => x.ID == message.ID);
                dbRelationship.Created = true;
                dbRelationship.Name = message.Name;
                dbRelationship.TableDrivedEntityID1 = message.EntityID1;
                dbRelationship.TableDrivedEntityID2 = message.EntityID2;
                while (dbRelationship.RelationshipColumns.Any())
                    projectContext.RelationshipColumns.Remove(dbRelationship.RelationshipColumns.First());
                foreach (var col in message.RelationshipColumns)
                {
                    RelationshipColumns rColumn = new RelationshipColumns();
                    rColumn.FirstSideColumnID = col.FirstSideColumnID;
                    rColumn.SecondSideColumnID = col.SecondSideColumnID;
                    dbRelationship.RelationshipColumns.Add(rColumn);
                }

                Relationship dbReverseRelationship = null;
                if (message.ID == 0)
                {
                    dbReverseRelationship = new Relationship();
                    dbReverseRelationship.MasterTypeEnum = (byte)Enum_MasterRelationshipType.FromForeignToPrimary;
                    dbReverseRelationship.Alias = entity1.Alias;
                    dbReverseRelationship.SecurityObject = new SecurityObject();
                    dbReverseRelationship.SecurityObject.Type = (int)DatabaseObjectCategory.Relationship;
                    projectContext.Relationship.Add(dbReverseRelationship);
                }
                else
                    dbReverseRelationship = dbRelationship.Relationship1.First();

                dbReverseRelationship.Name = "inverse_" + message.Name;
                dbReverseRelationship.Created = true;
                dbReverseRelationship.TableDrivedEntityID1 = message.EntityID2;
                dbReverseRelationship.TableDrivedEntityID2 = message.EntityID1;
                while (dbReverseRelationship.RelationshipColumns.Any())
                    projectContext.RelationshipColumns.Remove(dbReverseRelationship.RelationshipColumns.First());
                foreach (var col in message.RelationshipColumns)
                {
                    RelationshipColumns rColumn = new RelationshipColumns();
                    rColumn.FirstSideColumnID = col.SecondSideColumnID;
                    rColumn.SecondSideColumnID = col.FirstSideColumnID;
                    dbReverseRelationship.RelationshipColumns.Add(rColumn);
                }

                if (viewRelation)
                {
                    if (dbRelationship.ID == 0)
                    {
                        //Enum_RelationshipType relType;
                        //Enum_RelationshipType reverseRelType;
                        //if (!entity1.IsView)
                        //{
                        //relType = Enum_RelationshipType.TableToView;
                        //reverseRelType = Enum_RelationshipType.ViewToTable;
                        //}
                        //else
                        //{
                        //    if (!entity2.IsView)
                        //    {
                        //        relType = Enum_RelationshipType.ViewToTable;
                        //        reverseRelType = Enum_RelationshipType.TableToView;
                        //    }
                        //    else
                        //    {
                        //        relType = Enum_RelationshipType.ViewToView;
                        //        reverseRelType = Enum_RelationshipType.ViewToView;
                        //    }
                        //}
                        dbRelationship.MasterTypeEnum = (byte)Enum_MasterRelationshipType.NotImportant;
                        dbRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.TableToView);
                        dbRelationship.RelationshipType = new RelationshipType();

                        dbReverseRelationship.MasterTypeEnum = (byte)Enum_MasterRelationshipType.NotImportant;
                        dbReverseRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.ViewToTable);
                        dbReverseRelationship.RelationshipType = new RelationshipType();
                    }
                }
                else
                {
                    bool shoudChangeRelationshipTypes = false;
                    if (dbRelationship.ID == 0)
                        shoudChangeRelationshipTypes = true;
                    else
                    {
                        if (message.CreateType == CreateRelationshipType.OneToOne && ((Enum_RelationshipType)dbRelationship.TypeEnum) != Enum_RelationshipType.ImplicitOneToOne)
                            shoudChangeRelationshipTypes = true;
                        else if (message.CreateType == CreateRelationshipType.OneToMany && ((Enum_RelationshipType)dbRelationship.TypeEnum) != Enum_RelationshipType.OneToMany)
                            shoudChangeRelationshipTypes = true;
                    }
                    if (shoudChangeRelationshipTypes)
                    {
                        if (dbRelationship.ID != 0)
                        {
                            DeleteRelationshipType(projectContext, dbRelationship.RelationshipType);
                            DeleteRelationshipType(projectContext, dbReverseRelationship.RelationshipType);
                            if (message.CreateType == CreateRelationshipType.OneToMany)
                            {
                                dbRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.OneToMany);
                                dbRelationship.RelationshipType.OneToManyRelationshipType = new OneToManyRelationshipType();

                                dbReverseRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.ManyToOne);
                                dbReverseRelationship.RelationshipType.ManyToOneRelationshipType = new ManyToOneRelationshipType();
                            }
                            else if (message.CreateType == CreateRelationshipType.OneToOne)
                            {
                                dbRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.ImplicitOneToOne);
                                dbRelationship.RelationshipType.ImplicitOneToOneRelationshipType = new ImplicitOneToOneRelationshipType();

                                dbReverseRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.ExplicitOneToOne);
                                dbReverseRelationship.RelationshipType.ExplicitOneToOneRelationshipType = new ExplicitOneToOneRelationshipType();

                            }
                        }
                        else
                        {
                            if (message.CreateType == CreateRelationshipType.OneToMany)
                            {
                                dbRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.OneToMany);
                                dbRelationship.RelationshipType = new RelationshipType();
                                dbRelationship.RelationshipType.OneToManyRelationshipType = new OneToManyRelationshipType();
                                dbRelationship.RelationshipType.PKToFKDataEntryEnabled = secondSideDataEntry;
                                dbRelationship.RelationshipType.IsOtherSideMandatory = false;
                                dbRelationship.RelationshipType.IsOtherSideCreatable = true;
                                dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = true;
                                // dbRelationship.RelationshipType.IsOtherSideTransferable = true;

                                dbReverseRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.ManyToOne);
                                dbReverseRelationship.RelationshipType = new RelationshipType();
                                dbReverseRelationship.RelationshipType.ManyToOneRelationshipType = new ManyToOneRelationshipType();
                                //   dbReverseRelationship.DataEntryEnabled = firstSideDataEntry;
                                dbReverseRelationship.RelationshipType.IsOtherSideMandatory = false;
                                //   dbReverseRelationship.RelationshipType.IsOtherSideTransferable = true;
                                dbReverseRelationship.RelationshipType.IsOtherSideCreatable = true;
                                dbReverseRelationship.RelationshipType.IsOtherSideDirectlyCreatable = false;

                            }
                            else if (message.CreateType == CreateRelationshipType.OneToOne)
                            {
                                dbRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.ImplicitOneToOne);
                                dbRelationship.RelationshipType = new RelationshipType();
                                dbRelationship.RelationshipType.ImplicitOneToOneRelationshipType = new ImplicitOneToOneRelationshipType();
                                dbRelationship.RelationshipType.PKToFKDataEntryEnabled = secondSideDataEntry;
                                dbRelationship.RelationshipType.IsOtherSideMandatory = false;
                                dbRelationship.RelationshipType.IsOtherSideCreatable = true;
                                dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = false;
                                //        dbRelationship.RelationshipType.IsOtherSideTransferable = true;

                                dbReverseRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.ExplicitOneToOne);
                                dbReverseRelationship.RelationshipType = new RelationshipType();
                                dbReverseRelationship.RelationshipType.ExplicitOneToOneRelationshipType = new ExplicitOneToOneRelationshipType();
                                //   dbReverseRelationship.DataEntryEnabled = firstSideDataEntry;
                                dbRelationship.RelationshipType.IsOtherSideMandatory = false;
                                dbRelationship.RelationshipType.IsOtherSideCreatable = true;
                                dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = false;
                                //    dbRelationship.RelationshipType.IsOtherSideTransferable = true;

                            }
                        }
                    }

                }

                projectContext.SaveChanges();
                dbReverseRelationship.Relationship2 = dbRelationship;
                dbRelationship.Relationship2 = dbReverseRelationship;
                projectContext.SaveChanges();
                return dbRelationship.ID;
                //message.ID = dbRelationship.ID;
            }
        }
        public bool DataIsAccessable(DR_Requester requester, int relationshipID, bool checkFirstSideEntity, bool checkSecondSideEntity)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var relationship = projectContext.Relationship.First(x => x.ID == relationshipID);
                return DataIsAccessable(requester, relationship, checkFirstSideEntity, checkSecondSideEntity);
            }
        }
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();

        public bool DataIsAccessable(DR_Requester requester, Relationship relationship, bool checkFirstSideEntity, bool checkSecondSideEntity)
        {
            SecurityHelper securityHelper = new SecurityHelper();


            if (relationship.Removed == true)
                return false;
            else
            {
                Column firstFKCol = null;
                var relType = (Enum_MasterRelationshipType)relationship.MasterTypeEnum;
                if (relType == Enum_MasterRelationshipType.FromForeignToPrimary)
                    firstFKCol = relationship.RelationshipColumns.First().Column;
                else
                    firstFKCol = relationship.RelationshipColumns.First().Column1;
                if (firstFKCol.IsDisabled)
                    return false;
                else
                {
                    if (requester.SkipSecurity)
                        return true;

                    var permission = securityHelper.GetAssignedPermissions(requester, firstFKCol.ID, false);
                    if (permission.GrantedActions.Any(y => y == SecurityAction.NoAccess))
                        return false;
                    else
                    {
                        bool entitiesAccess = true;
                        if (checkFirstSideEntity)
                        {
                            if (!bizTableDrivedEntity.DataIsAccessable(requester, relationship.TableDrivedEntity))
                                entitiesAccess = false;
                        }
                        if (entitiesAccess && checkSecondSideEntity)
                        {
                            if (!bizTableDrivedEntity.DataIsAccessable(requester, relationship.TableDrivedEntity1))
                                entitiesAccess = false;
                        }
                        return entitiesAccess;
                    }
                }
                //else if (securityMode == SecurityMode.View)
                //{
                //    if (permission.GrantedActions.Any(y => y == SecurityAction.ReadOnly || y == SecurityAction.Edit || y == SecurityAction.EditAndDelete))
                //        return true;
                //    else
                //        return false;
                //}
                //else
                //{
                //    if (permission.GrantedActions.Any(y => y == SecurityAction.Edit || y == SecurityAction.EditAndDelete))
                //        return true;
                //    else
                //        return false;
                //}
            }
        }
        public bool DataIsReadonly(DR_Requester requester, int relationshipID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var relationship = projectContext.Relationship.First(x => x.ID == relationshipID);
                return DataIsReadonly(requester, relationship);
            }
        }
        public bool DataIsReadonly(DR_Requester requester, Relationship relationship)
        {
            SecurityHelper securityHelper = new SecurityHelper();

            Column firstFKCol = null;
            var relType = (Enum_MasterRelationshipType)relationship.MasterTypeEnum;
            TableDrivedEntity fkEntity = null;
            if (relType == Enum_MasterRelationshipType.FromForeignToPrimary)
            {
                firstFKCol = relationship.RelationshipColumns.First().Column;
                fkEntity = relationship.TableDrivedEntity;
            }
            else
            {
                firstFKCol = relationship.RelationshipColumns.First().Column1;
                fkEntity = relationship.TableDrivedEntity1;
            }
            if (firstFKCol.IsReadonly)
                return true;
            else if (bizTableDrivedEntity.DataIsReadonly(requester, fkEntity))
                return true;
            else
            {
                if (requester.SkipSecurity)
                    return false;

                var permission = securityHelper.GetAssignedPermissions(requester, firstFKCol.ID, false);
                if (permission.GrantedActions.Any(y => y == SecurityAction.ReadOnly))
                    return true;
                else
                {
                    return false;
                }
            }
        }
        public List<RelationshipDTO> GetRelationshipsByEntityID(int entityID)
        {
            List<RelationshipDTO> result = new List<RelationshipDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var relationships = GetRelationships(projectContext)
                    .Where(x => x.Removed != true && x.TableDrivedEntityID1 == entityID).ToList();
                foreach (var relationship in relationships)
                {
                    result.Add(ToRelationshipDTO(relationship));
                }
            }
            return result;
        }

        public void UpdateModel(DR_Requester requester, int databaseID, List<RelationshipDTO> listNew, List<RelationshipDTO> listDeleted)
        {

            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //foreach (var newRel in listNew)
                //{
                var addRels = UpdateRelationshipInModel(requester, projectContext, databaseID, listNew);
                //}
                foreach (var deleteRel in listDeleted)
                {
                    if (ItemImportingStarted != null)
                        ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = "Disabling" + " " + deleteRel.Name, TotalProgressCount = listDeleted.Count, CurrentProgress = listNew.IndexOf(deleteRel) + 1 });
                    var dbRel = projectContext.Relationship.First(x => x.ID == deleteRel.ID);
                    dbRel.Removed = true;
                    dbRel.Relationship2.Removed = true;
                }
                if (ItemImportingStarted != null)
                    ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = "Saving changes..." });
                projectContext.SaveChanges();
                foreach (var item in addRels)
                {
                    item.Item1.RelationshipID = item.Item2.ID;
                    item.Item2.RelationshipID = item.Item1.ID;
                }
                if (ItemImportingStarted != null)
                    ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = "Saving reverse relationships..." });
                projectContext.SaveChanges();
            }
        }
        private List<Tuple<Relationship, Relationship>> UpdateRelationshipInModel(DR_Requester requester, MyProjectEntities projectContext, int databaseID, List<RelationshipDTO> listNew)
        {
            BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();

            var allEntities = bizTableDrivedEntity.GetAllEntities(databaseID, EntityColumnInfoType.WithoutColumn, EntityRelationshipInfoType.WithoutRelationships, null);
            List<Tuple<Relationship, Relationship>> result = new List<Tuple<Relationship, Relationship>>();
            List<RelationshipDTO> reviewedRelationships = new List<RelationshipDTO>();
            var listOtOOtM = listNew.Where(x => x.OrginalTypeEnum == Enum_OrginalRelationshipType.OneToMany ||
             x.OrginalTypeEnum == Enum_OrginalRelationshipType.OneToOne).ToList();
            var dbDatabase = projectContext.DatabaseInformation.First(x => x.ID == databaseID);
            foreach (var relationship in listOtOOtM)
            {
                if (ItemImportingStarted != null)
                    ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = "Creating" + " " + relationship.Name, TotalProgressCount = listOtOOtM.Count, CurrentProgress = listOtOOtM.IndexOf(relationship) + 1 });

                var rels = GetModelRelationship(projectContext, relationship, allEntities);
                var dbRelationship = rels.Item1;
                var dbReverseRelationship = rels.Item2;
                if (relationship.OrginalTypeEnum == Enum_OrginalRelationshipType.OneToMany)
                {
                    dbRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.OneToMany);
                    //var relationshipType = new RelationshipType();
                    //relationshipType.Relationship = dbRelationship;
                    dbRelationship.RelationshipType = new RelationshipType();
                    dbRelationship.RelationshipType.OneToManyRelationshipType = new OneToManyRelationshipType();
                    //dbRelationship.PKToFKDataEntryEnabled = false;
                    //if (dbDatabase.DBHasDate)
                    //{
                    //    dbRelationship.RelationshipType.IsOtherSideMandatory = relationship.RelationInfo.AllPrimarySideHasFkSideData == true;
                    //}
                    //else
                    dbRelationship.RelationshipType.IsOtherSideMandatory = false;
                    dbRelationship.RelationshipType.IsOtherSideCreatable = true;
                    dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = true;
                    //dbRelationship.RelationshipType.IsSkippable = true;
                    //نمیخواهیم رابطه قابل ایجاد یا مستقیم باشد. در واقع رابطه نمایش داده شود. اما بعدا بررسی شود که آیا رابطه باید غیرفعال ورود اطلاعات شود یا خیر
                    //بهتره اینجا تصمیم گیری نشه چون اجباری هم نیست تو دیتابیس برای ایجاد طرف دیگر
                    //if (dbRelationship.RelationshipType.IsOtherSideMandatory)
                    //{
                    //    dbRelationship.RelationshipType.IsOtherSideCreatable = true;
                    //    dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = true;
                    //}
                    //dbRelationship.RelationshipType.IsOtherSideTransferable = true;
                    //projectContext.RelationshipType.Add(relationshipType);
                    //projectContext.SaveChanges();
                    //   var oneToManyRelationshipType = new OneToManyRelationshipType();
                    //  oneToManyRelationshipType.RelationshipType = relationshipType;
                    //  projectContext.OneToManyRelationshipType.Add(oneToManyRelationshipType);
                    //پشت هم که باشه نمیتونه ایدی ریلیشن تایپ رو تو وان تو منی هم بزاره!!

                    dbReverseRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.ManyToOne);
                    //var reverseRelationshipType = new RelationshipType();
                    dbReverseRelationship.RelationshipType = new RelationshipType();
                    dbReverseRelationship.RelationshipType.ManyToOneRelationshipType = new ManyToOneRelationshipType();
                    if (dbDatabase.DBHasDate)
                    {
                        dbReverseRelationship.RelationshipType.IsOtherSideMandatory = relationship.RelationInfo.AllFKSidesHavePKSide == true;
                    }
                    else
                        dbReverseRelationship.RelationshipType.IsOtherSideMandatory = !relationship.FKCoumnIsNullable;
                    //dbReverseRelationship.RelationshipType.IsOtherSideTransferable = true;
                    dbReverseRelationship.RelationshipType.IsOtherSideCreatable = true;
                    //dbReverseRelationship.RelationshipType.IsOtherSideDirectlyCreatable = false;

                    //projectContext.RelationshipType.Add(reverseRelationshipType);
                    ////projectContext.SaveChanges();
                    ////    reverseRelationship.RelationshipType.;
                    //var manyToOneRelationshipType = new ManyToOneRelationshipType();
                    //manyToOneRelationshipType.RelationshipType = reverseRelationshipType;
                    //projectContext.ManyToOneRelationshipType.Add(manyToOneRelationshipType);

                    result.Add(new Tuple<Relationship, Relationship>(dbRelationship, dbReverseRelationship));
                }
                else if (relationship.OrginalTypeEnum == Enum_OrginalRelationshipType.OneToOne)
                {
                    dbRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.ImplicitOneToOne);
                    dbRelationship.RelationshipType = new RelationshipType();
                    dbRelationship.RelationshipType.ImplicitOneToOneRelationshipType = new ImplicitOneToOneRelationshipType();
                    // dbRelationship.PKToFKDataEntryEnabled = false;
                    if (dbDatabase.DBHasDate)
                    {
                        dbRelationship.RelationshipType.IsOtherSideMandatory = relationship.RelationInfo.AllPrimarySideHasFkSideData == true;
                    }
                    else
                        dbRelationship.RelationshipType.IsOtherSideMandatory = false;
                    //dbRelationship.RelationshipType.IsOtherSideTransferable = true;
                    dbRelationship.RelationshipType.IsOtherSideCreatable = true;

                    //if (dbRelationship.RelationshipType.IsOtherSideMandatory)
                    //{
                    //    dbRelationship.RelationshipType.IsOtherSideCreatable = true;
                    //    dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = false;
                    //}

                    dbReverseRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.ExplicitOneToOne);
                    dbReverseRelationship.RelationshipType = new RelationshipType();
                    dbReverseRelationship.RelationshipType.ExplicitOneToOneRelationshipType = new ExplicitOneToOneRelationshipType();
                    if (dbDatabase.DBHasDate)
                    {
                        dbReverseRelationship.RelationshipType.IsOtherSideMandatory = relationship.RelationInfo.AllFKSidesHavePKSide == true;
                    }
                    else
                        dbReverseRelationship.RelationshipType.IsOtherSideMandatory = !relationship.FKCoumnIsNullable;
                    //dbReverseRelationship.RelationshipType.IsOtherSideTransferable = true;
                    dbReverseRelationship.RelationshipType.IsOtherSideCreatable = true;

                    //if (relationship.RelationInfo.AllFKSidesHavePKSide)
                    //{
                    //    dbReverseRelationship.RelationshipType.IsOtherSideCreatable = true;
                    //    dbReverseRelationship.RelationshipType.IsOtherSideDirectlyCreatable = true;
                    //}
                    result.Add(new Tuple<Relationship, Relationship>(dbRelationship, dbReverseRelationship));
                }
            }
            var dataHelper = new ModelDataHelper();

            var isaGroups = listNew.Where(x => x.OrginalTypeEnum == Enum_OrginalRelationshipType.SuperToSub).GroupBy(x => x.OrginalRelationshipGroup).ToList();
            foreach (var relationshipGroup in isaGroups)
            {
                if (ItemImportingStarted != null)
                    ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = "Creating ISA" + " " + relationshipGroup.Key, TotalProgressCount = isaGroups.Count, CurrentProgress = isaGroups.IndexOf(relationshipGroup) + 1 });

                var isaInfo = dataHelper.GetISARelationshipDetail(requester, relationshipGroup.ToList());

                var isaRelationship = new ISARelationship();
                isaRelationship.Name = relationshipGroup.Key;
                if (dbDatabase.DBHasDate)
                {
                    isaRelationship.IsTolatParticipation = isaInfo.IsTotalParticipation == true;
                    isaRelationship.IsDisjoint = isaInfo.IsDisjoint == true;
                }
                else
                {
                    if (relationshipGroup.Count() > 1)
                        isaRelationship.IsTolatParticipation = true;
                    else
                        isaRelationship.IsTolatParticipation = false;

                    isaRelationship.IsDisjoint = true;
                }


                foreach (var relationship in relationshipGroup)
                {
                    var rels = GetModelRelationship(projectContext, relationship, allEntities);
                    var dbRelationship = rels.Item1;
                    var dbReverseRelationship = rels.Item2;

                    dbRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.SuperToSub);
                    dbRelationship.RelationshipType = new RelationshipType();
                    dbRelationship.RelationshipType.SuperToSubRelationshipType = new SuperToSubRelationshipType();
                    //dbRelationship.RelationshipType.IsOtherSideMandatory = false;
                    dbRelationship.RelationshipType.IsOtherSideCreatable = true;
                    dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = true;
                    dbRelationship.RelationshipType.PKToFKDataEntryEnabled = true;
                    //dbRelationship.RelationshipType.IsOtherSideTransferable = true;

                    dbReverseRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.SubToSuper);
                    dbReverseRelationship.RelationshipType = new RelationshipType();
                    dbReverseRelationship.RelationshipType.SubToSuperRelationshipType = new SubToSuperRelationshipType();
                    dbReverseRelationship.RelationshipType.IsOtherSideMandatory = true;
                    dbReverseRelationship.RelationshipType.IsOtherSideCreatable = true;
                    dbReverseRelationship.RelationshipType.IsOtherSideDirectlyCreatable = true;
                    //dbReverseRelationship.RelationshipType.IsOtherSideTransferable = true;

                    dbRelationship.RelationshipType.SuperToSubRelationshipType.ISARelationship = isaRelationship;
                    dbReverseRelationship.RelationshipType.SubToSuperRelationshipType.ISARelationship = isaRelationship;
                    result.Add(new Tuple<Relationship, Relationship>(dbRelationship, dbReverseRelationship));
                }
            }
            //List<Tuple<string, List<RelationshipDTO>>> reverseFromSuperToSub = new List<Tuple<string, List<RelationshipDTO>>>();
            var unionGroups = listNew.Where(x => x.OrginalTypeEnum == Enum_OrginalRelationshipType.SubUnionToUnion).GroupBy(x => x.OrginalRelationshipGroup).ToList();
            foreach (var relationshipGroup in unionGroups)
            {
                if (ItemImportingStarted != null)
                    ItemImportingStarted(this, new ItemImportingStartedArg() { ItemName = "Creating Union" + " " + relationshipGroup.Key, TotalProgressCount = unionGroups.Count, CurrentProgress = unionGroups.IndexOf(relationshipGroup) + 1 });

                //var firstEntity = bizTableDrivedEntity.GetTableDrivedEntity(relationshipGroup.Item2.First().EntityID1);
                var isaInfo = dataHelper.GetUnionRelationshipDetail(requester, relationshipGroup.ToList());
                var unionRelationship = new UnionRelationshipType();
                unionRelationship.Name = relationshipGroup.Key;
                if (dbDatabase.DBHasDate)
                    unionRelationship.IsTolatParticipation = isaInfo.IsTotalParticipation == true;
                else
                    unionRelationship.IsTolatParticipation = false;
                foreach (var relationship in relationshipGroup)
                {
                    var rels = GetModelRelationship(projectContext, relationship, allEntities);
                    var dbRelationship = rels.Item1;
                    var dbReverseRelationship = rels.Item2;

                    dbRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.SubUnionToUnion);
                    dbRelationship.RelationshipType = new RelationshipType();
                    dbRelationship.RelationshipType.SubUnionToUnionRelationshipType = new SubUnionToUnionRelationshipType();
                    //dbReverseRelationship.RelationshipType.IsOtherSideMandatory = true;
                    dbRelationship.RelationshipType.IsOtherSideCreatable = true;
                    dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = false;
                    dbRelationship.RelationshipType.PKToFKDataEntryEnabled = true;
                    //dbRelationship.RelationshipType.IsOtherSideTransferable = true;

                    dbReverseRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.UnionToSubUnion);
                    dbReverseRelationship.RelationshipType = new RelationshipType();
                    dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType = new UnionToSubUnionRelationshipType();
                    //dbRelationship.RelationshipType.IsOtherSideMandatory = false;
                    dbReverseRelationship.RelationshipType.IsOtherSideCreatable = true;
                    dbReverseRelationship.RelationshipType.IsOtherSideDirectlyCreatable = false;
                    //dbReverseRelationship.RelationshipType.IsOtherSideTransferable = true;

                    dbRelationship.RelationshipType.SubUnionToUnionRelationshipType.UnionRelationshipType = unionRelationship;
                    dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType.UnionRelationshipType = unionRelationship;
                    result.Add(new Tuple<Relationship, Relationship>(dbRelationship, dbReverseRelationship));
                }

            }
            return result;
        }

        public void DeleteRelationship(int relationshipID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbrelationship = projectContext.Relationship.First(x => x.ID == relationshipID);
                var dbReverse = dbrelationship.Relationship2;
                DeleteRelationship(projectContext, dbrelationship);
                DeleteRelationship(projectContext, dbReverse);
                dbrelationship.Relationship2 = null;
                dbReverse.Relationship2 = null;
                projectContext.SaveChanges();

                projectContext.SecurityObject.Remove(dbrelationship.SecurityObject);
                projectContext.Relationship.Remove(dbrelationship);
                projectContext.SecurityObject.Remove(dbReverse.SecurityObject);
                projectContext.Relationship.Remove(dbReverse);

                projectContext.SaveChanges();
            }
        }
        void DeleteRelationship(MyProjectEntities projectContext, Relationship dbrelationship)
        {
            // var dbrelationship = projectContext.Relationship.First(x => x.ID == relationshipID);
            if (dbrelationship.Created)
            {
                while (dbrelationship.RelationshipColumns.Any())
                    projectContext.RelationshipColumns.Remove(dbrelationship.RelationshipColumns.First());
                if (dbrelationship.RelationshipType != null)
                {
                    DeleteRelationshipType(projectContext, dbrelationship.RelationshipType);
                    projectContext.RelationshipType.Remove(dbrelationship.RelationshipType);
                }
                //if (deleteReverse)
                //{
                //    if (dbrelationship.RelationshipID != null)
                //    {
                //        //dbrelationship.Relationship2 = null;
                //        DeleteRelationship(projectContext, dbrelationship.RelationshipID.Value, false);
                //    }
                //}
                //else
                //    dbrelationship.Relationship2 = null;

                //projectContext.Relationship.Remove(dbrelationship);

            }
        }
        void DeleteRelationshipType(MyProjectEntities projectContext, RelationshipType dbRelationshipType)
        {
            if (dbRelationshipType.ExplicitOneToOneRelationshipType != null)
                projectContext.ExplicitOneToOneRelationshipType.Remove(dbRelationshipType.ExplicitOneToOneRelationshipType);
            if (dbRelationshipType.ImplicitOneToOneRelationshipType != null)
                projectContext.ImplicitOneToOneRelationshipType.Remove(dbRelationshipType.ImplicitOneToOneRelationshipType);

            if (dbRelationshipType.ManyToOneRelationshipType != null)
                projectContext.ManyToOneRelationshipType.Remove(dbRelationshipType.ManyToOneRelationshipType);
            if (dbRelationshipType.OneToManyRelationshipType != null)
                projectContext.OneToManyRelationshipType.Remove(dbRelationshipType.OneToManyRelationshipType);

            if (dbRelationshipType.SubToSuperRelationshipType != null)
                projectContext.SubToSuperRelationshipType.Remove(dbRelationshipType.SubToSuperRelationshipType);
            if (dbRelationshipType.SuperToSubRelationshipType != null)
                projectContext.SuperToSubRelationshipType.Remove(dbRelationshipType.SuperToSubRelationshipType);

            if (dbRelationshipType.UnionToSubUnionRelationshipType != null)
                projectContext.UnionToSubUnionRelationshipType.Remove(dbRelationshipType.UnionToSubUnionRelationshipType);
            if (dbRelationshipType.SubUnionToUnionRelationshipType != null)
                projectContext.SubUnionToUnionRelationshipType.Remove(dbRelationshipType.SubUnionToUnionRelationshipType);


        }
        private Tuple<Relationship, Relationship> GetModelRelationship(MyProjectEntities projectContext, RelationshipDTO relationship, List<TableDrivedEntityDTO> entities)
        {
            var dbRelationship = new Relationship();
            dbRelationship.SecurityObject = new SecurityObject();
            dbRelationship.SecurityObject.Type = (int)DatabaseObjectCategory.Relationship;
            projectContext.Relationship.Add(dbRelationship);
            dbRelationship.IsOrginal = true;
            //   dbRelationship.DataEntryEnabled = true;
            dbRelationship.MasterTypeEnum = (byte)Enum_MasterRelationshipType.FromPrimartyToForeign;
            //          dbRelationship.RelatesOnPrimaryKeys = relationship.RelatesOnPrimaryKeys;

            var dbReverseRelationship = new Relationship();
            dbReverseRelationship.SecurityObject = new SecurityObject();
            dbReverseRelationship.SecurityObject.Type = (int)DatabaseObjectCategory.Relationship;
            projectContext.Relationship.Add(dbReverseRelationship);
            dbReverseRelationship.IsOrginal = true;
            //   dbReverseRelationship.DataEntryEnabled = true;
            dbReverseRelationship.MasterTypeEnum = (byte)Enum_MasterRelationshipType.FromForeignToPrimary;
            //   dbReverseRelationship.RelatesOnPrimaryKeys = relationship.RelatesOnPrimaryKeys;

            dbRelationship.TableDrivedEntityID1 = relationship.EntityID1;
            dbRelationship.TableDrivedEntityID2 = relationship.EntityID2;

            dbReverseRelationship.TableDrivedEntityID1 = relationship.EntityID2;
            dbReverseRelationship.TableDrivedEntityID2 = relationship.EntityID1;

            var info = "";
            var reverseInfo = "";
            foreach (var col in relationship.RelationshipColumns)
            {
                dbRelationship.RelationshipColumns.Add(new RelationshipColumns() { FirstSideColumnID = col.FirstSideColumnID, SecondSideColumnID = col.SecondSideColumnID });
                dbReverseRelationship.RelationshipColumns.Add(new RelationshipColumns() { FirstSideColumnID = col.SecondSideColumnID, SecondSideColumnID = col.FirstSideColumnID });
                info += (info == "" ? "" : ",") + relationship.Entity1 + "." + col.FirstSideColumn.Name + " = " + relationship.Entity2 + "." + col.SecondSideColumn.Name;
                reverseInfo += (reverseInfo == "" ? "" : ",") + relationship.Entity2 + "." + col.SecondSideColumn.Name + " = " + relationship.Entity1 + "." + col.FirstSideColumn.Name;
            }

            dbRelationship.Name = relationship.Name;
            if (string.IsNullOrEmpty(dbRelationship.Alias))
                dbRelationship.Alias = relationship.Alias;
            dbRelationship.Info = info;


            dbReverseRelationship.Name = relationship.Name;
            if (string.IsNullOrEmpty(dbReverseRelationship.Alias))
            {
                dbReverseRelationship.Alias = entities.First(x => x.ID == relationship.EntityID1).Alias;
            }
            dbReverseRelationship.Info = reverseInfo;

            return new Tuple<Relationship, Relationship>(dbRelationship, dbReverseRelationship);
        }
        //private void SetRelationshipAlias(RelationshipDTO relationship, List<TableDrivedEntityDTO> entities)
        //{
        //    foreach (var item in relationshipItems)
        //    {
        //        var firstSideEntity = entities.First(x => x.Name == item.Relationship.Entity1);
        //        var secondSideEntity = entities.First(x => x.Name == item.Relationship.Entity2);
        //        string otherSideName = secondSideEntity.Alias;
        //        bool shouldIncludForeignKeyNames = false;
        //        if (relationshipItems.Any(x => x != item && x.Relationship.Entity1 == item.Relationship.Entity1 && x.Relationship.Entity2 == item.Relationship.Entity2)
        //            || entities.Any(x => x.Relationships.Any(z => z.Entity1 == item.Relationship.Entity1 && z.Entity2 == item.Relationship.Entity2)))
        //            shouldIncludForeignKeyNames = true;
        //        var aa = relationshipItems.FirstOrDefault(x => x != item && x.Relationship.Entity1 == item.Relationship.Entity1 && x.Relationship.Entity2 == item.Relationship.Entity2);
        //        if (shouldIncludForeignKeyNames)
        //        {
        //            string colNames = "";
        //            foreach (var relCol in item.Relationship.RelationshipColumns)
        //            {
        //                colNames += (colNames == "" ? "" : ",") + secondSideEntity.Columns.First(x => x.ID == relCol.SecondSideColumnID).Alias;
        //            }
        //            item.Relationship.Alias = otherSideName + ":" + colNames;
        //        }
        //        else
        //            item.Relationship.Alias = otherSideName;
        //    }
        //}


        //public void UpdateRelationshipProperties(int databaseID, List<RelationshipDTO> listEntities)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        foreach (var item in listEntities)
        //        {
        //            var dbRel = projectContext.Relationship.First(x => x.ID == item.ID);
        //            //  dbRel.RelationshipType.IsOtherSideMandatory = item.IsOtherSideMandatory;
        //            dbRel.DataEntryEnabled = item.DataEntryEnabled;
        //            // dbRel.Relationship2.RelationshipType.IsOtherSideMandatory = item.IsPrimarySideMandatory;
        //            dbRel.Reviewed = true;
        //            dbRel.Relationship2.Reviewed = true;
        //        }
        //        projectContext.SaveChanges();
        //    }
        //}

        //public List<RelationshipDTO> GetOneToManyDataEntryNotEnabledRelationships(int databaseID)
        //{
        //    List<RelationshipDTO> result = new List<RelationshipDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {

        //        var dblist = GetRelationships(projectContext).Where(x => (x.TypeEnum == 2)
        //        && x.IsOrginal == true && x.Enabled == true && x.DataEntryEnabled == false && x.TableDrivedEntity.Table.DBSchema.DatabaseInformationID == databaseID
        //        && x.TableDrivedEntity1.Table.DBSchema.DatabaseInformationID == databaseID);
        //        foreach (var item in dblist)
        //        {
        //            result.Add(ToRelationshipDTO(item));
        //        }
        //        //foreach (var item in list.Where(x => x.MastertTypeEnum == Enum_MasterRelationshipType.FromPrimartyToForeign))
        //        //{
        //        //    item.IsOtherSideMandatory = item.IsOtherSideMandatory;
        //        //    //item.IsForeignSideCreatable = item.IsOtherSideCreatable;
        //        //    //item.IsForeignSideDirectlyCreatable = item.IsOtherSideDirectlyCreatable;
        //        //    //item.IsForeignSideTransferable = item.IsOtherSideTransferable;

        //        //    var reverse = list.First(x => x.ID == item.PairRelationshipID);
        //        //    item.IsPrimarySideMandatory = reverse.IsOtherSideMandatory;
        //        //    //item.IsPrimarySideCreatable = reverse.IsOtherSideCreatable;
        //        //    //item.IsPrimarySideDirectlyCreatable = reverse.IsOtherSideDirectlyCreatable;
        //        //    //item.IsPrimarySideTransferable = reverse.IsOtherSideTransferable;

        //        //    result.Add(new RelationshipImportItem(item, false, ""));
        //        //}
        //    }
        //    return result;
        //}

        //public List<RelationshipImportItem> GetEnabledNotReviewedRelationships(int databaseID)
        //{
        //    List<RelationshipImportItem> result = new List<RelationshipImportItem>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        List<RelationshipDTO> list = new List<RelationshipDTO>();
        //        var dblist = GetRelationships(projectContext).Where(x => (x.TypeEnum == 2 || x.TypeEnum == 4 || x.TypeEnum == 1 || x.TypeEnum == 3)
        //        && x.IsOrginal == true && x.Enabled == true && x.TableDrivedEntity.Table.DBSchema.DatabaseInformationID == databaseID
        //        && x.TableDrivedEntity1.Table.DBSchema.DatabaseInformationID == databaseID && x.Reviewed == false);
        //        foreach (var item in dblist)
        //        {
        //            list.Add(ToRelationshipDTO(item));

        //        }
        //        foreach (var item in list.Where(x => x.MastertTypeEnum == Enum_MasterRelationshipType.FromPrimartyToForeign))
        //        {
        //            item.IsOtherSideMandatory = item.IsOtherSideMandatory;
        //            //item.IsForeignSideCreatable = item.IsOtherSideCreatable;
        //            //item.IsForeignSideDirectlyCreatable = item.IsOtherSideDirectlyCreatable;
        //            //item.IsForeignSideTransferable = item.IsOtherSideTransferable;

        //            var reverse = list.First(x => x.ID == item.PairRelationshipID);
        //            item.IsPrimarySideMandatory = reverse.IsOtherSideMandatory;
        //            //item.IsPrimarySideCreatable = reverse.IsOtherSideCreatable;
        //            //item.IsPrimarySideDirectlyCreatable = reverse.IsOtherSideDirectlyCreatable;
        //            //item.IsPrimarySideTransferable = reverse.IsOtherSideTransferable;

        //            result.Add(new RelationshipImportItem(item, false, ""));
        //        }
        //    }
        //    return result;
        //}

        internal Tuple<Relationship, Relationship> CopyRelationshipTuple(MyProjectEntities projectContext, Relationship dbRelationship)
        {
            return new Tuple<Relationship, Relationship>(CopyRelationship(projectContext, dbRelationship), CopyRelationship(projectContext, dbRelationship.Relationship2));
        }
        private Relationship CopyRelationship(MyProjectEntities projectContext, Relationship dbRelationship)
        {
            Relationship newRelationship = new Relationship();
            newRelationship.SecurityObject = new SecurityObject();
            newRelationship.SecurityObject.Type = (int)DatabaseObjectCategory.Relationship;
            newRelationship.Alias = dbRelationship.Alias;
            newRelationship.Created = dbRelationship.Created;

            newRelationship.Removed = dbRelationship.Removed;
            newRelationship.FirstSideTableID = dbRelationship.FirstSideTableID;
            newRelationship.Info = dbRelationship.Info;
            newRelationship.IsOrginal = dbRelationship.IsOrginal;
            newRelationship.MasterTypeEnum = dbRelationship.MasterTypeEnum;
            newRelationship.Name = dbRelationship.Name;
            //      newRelationship.RelatesOnPrimaryKeys = dbRelationship.RelatesOnPrimaryKeys;
            newRelationship.RelationshipType = dbRelationship.RelationshipType;
            newRelationship.Reviewed = dbRelationship.Reviewed;
            newRelationship.SearchInitially = dbRelationship.SearchInitially;
            newRelationship.SecondSideTableID = dbRelationship.SecondSideTableID;
            if (dbRelationship.TableDrivedEntityID1 != 0)
                newRelationship.TableDrivedEntityID1 = dbRelationship.TableDrivedEntityID1;
            if (dbRelationship.TableDrivedEntityID2 != 0)
                newRelationship.TableDrivedEntityID2 = dbRelationship.TableDrivedEntityID2;
            newRelationship.TypeEnum = dbRelationship.TypeEnum;
            newRelationship.Created = dbRelationship.Created;
            foreach (var item in dbRelationship.RelationshipColumns)
            {
                newRelationship.RelationshipColumns.Add(new RelationshipColumns() { FirstSideColumnID = item.FirstSideColumnID, SecondSideColumnID = item.SecondSideColumnID });
            }

            newRelationship.RelationshipType = new RelationshipType();
            newRelationship.RelationshipType.DeleteOption = dbRelationship.RelationshipType.DeleteOption;
            newRelationship.RelationshipType.IsOtherSideCreatable = dbRelationship.RelationshipType.IsOtherSideCreatable;
            newRelationship.RelationshipType.IsOtherSideDirectlyCreatable = dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable;
            newRelationship.RelationshipType.IsOtherSideMandatory = dbRelationship.RelationshipType.IsOtherSideMandatory;
            newRelationship.RelationshipType.PKToFKDataEntryEnabled = dbRelationship.RelationshipType.PKToFKDataEntryEnabled;
            //newRelationship.RelationshipType.IsOtherSideTransferable = dbRelationship.RelationshipType.IsOtherSideTransferable;
            newRelationship.RelationshipType.IsNotSkippable = dbRelationship.RelationshipType.IsNotSkippable;
            if (dbRelationship.RelationshipType.ExplicitOneToOneRelationshipType != null)
            {
                newRelationship.RelationshipType.ExplicitOneToOneRelationshipType = new ExplicitOneToOneRelationshipType();
            }
            if (dbRelationship.RelationshipType.ImplicitOneToOneRelationshipType != null)
            {
                newRelationship.RelationshipType.ImplicitOneToOneRelationshipType = new ImplicitOneToOneRelationshipType();
            }

            if (dbRelationship.RelationshipType.ManyToOneRelationshipType != null)
            {
                newRelationship.RelationshipType.ManyToOneRelationshipType = new ManyToOneRelationshipType();
            }
            if (dbRelationship.RelationshipType.OneToManyRelationshipType != null)
            {
                newRelationship.RelationshipType.OneToManyRelationshipType = new OneToManyRelationshipType();
            }


            if (dbRelationship.RelationshipType.SubToSuperRelationshipType != null)
            {
                newRelationship.RelationshipType.SubToSuperRelationshipType = new SubToSuperRelationshipType();
                newRelationship.RelationshipType.SubToSuperRelationshipType.ISARelationshipID = dbRelationship.RelationshipType.SubToSuperRelationshipType.ISARelationshipID;
            }
            if (dbRelationship.RelationshipType.SuperToSubRelationshipType != null)
            {
                newRelationship.RelationshipType.SuperToSubRelationshipType = new SuperToSubRelationshipType();
                newRelationship.RelationshipType.SuperToSubRelationshipType.ISARelationshipID = dbRelationship.RelationshipType.SuperToSubRelationshipType.ISARelationshipID;
            }


            if (dbRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
            {
                newRelationship.RelationshipType.UnionToSubUnionRelationshipType = new UnionToSubUnionRelationshipType();
                newRelationship.RelationshipType.UnionToSubUnionRelationshipType.UnionRelationshipTypeID = dbRelationship.RelationshipType.UnionToSubUnionRelationshipType.UnionRelationshipTypeID;

            }
            if (dbRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
            {
                newRelationship.RelationshipType.SubUnionToUnionRelationshipType = new SubUnionToUnionRelationshipType();
                newRelationship.RelationshipType.SubUnionToUnionRelationshipType.UnionRelationshipTypeID = dbRelationship.RelationshipType.SubUnionToUnionRelationshipType.UnionRelationshipTypeID;
            }


            projectContext.Relationship.Add(newRelationship);
            return newRelationship;

        }
        //public void ImposeSingleRelationshipType(int relationshipID)
        //{
        //    BizRelationship bizRelationship = new BizRelationship();
        //    var relationshipdto = bizRelationship.GetRelationship(relationshipID);
        //    if (relationshipdto.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary)
        //        relationshipdto = bizRelationship.GetRelationship(relationshipdto.PairRelationshipID);
        //    ModelDataHelper dataHelper = new ModelDataHelper();
        //    var relationInfo = dataHelper.GetRelationshipsInfoWithEntityIds(relationshipdto);
        //    ImposeSingleRelationshipType(relationshipdto, relationInfo);
        //}
        //private void ImposeSingleRelationshipType(RelationshipDTO relationshipdto, RelationInfo relationInfo)
        //{
        //    //حتما بررسی و یکی شود با ویزارد
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var relationship = projectContext.Relationship.First(x => x.ID == relationshipdto.ID);
        //        if (relationship.RelationshipType == null)
        //        {
        //            var reverseRelationship = projectContext.Relationship.First(x => x.RelationshipID == relationship.ID);
        //            if (relationInfo.FKRelatesOnPrimaryKey)
        //            {
        //                relationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.SuperToSub);
        //                relationship.RelationshipType = new RelationshipType();
        //                relationship.RelationshipType.SuperToSubRelationshipType = new SuperToSubRelationshipType();
        //                relationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData == true;

        //                reverseRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.SubToSuper);
        //                reverseRelationship.RelationshipType = new RelationshipType();
        //                reverseRelationship.RelationshipType.SubToSuperRelationshipType = new SubToSuperRelationshipType();
        //                reverseRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllFKSidesHavePKSide == true;

        //                var isaRelationship = new ISARelationship();
        //                isaRelationship.Name = relationshipdto.Entity1 + ">" + relationshipdto.Entity2;
        //                isaRelationship.IsDisjoint = false;
        //                isaRelationship.IsTolatParticipation = relationInfo.AllPrimarySideHasFkSideData == true;

        //                relationship.RelationshipType.SuperToSubRelationshipType.ISARelationship = isaRelationship;
        //                reverseRelationship.RelationshipType.SubToSuperRelationshipType.ISARelationship = isaRelationship;

        //            }
        //            else if (relationInfo.RelationType == RelationType.OnePKtoManyFK)
        //            {
        //                relationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.OneToMany);
        //                var relationshipType = new RelationshipType();
        //                relationshipType.Relationship = relationship;
        //                relationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData == true;
        //                projectContext.RelationshipType.Add(relationshipType);
        //                projectContext.SaveChanges();
        //                var oneToManyRelationshipType = new OneToManyRelationshipType();
        //                oneToManyRelationshipType.RelationshipType = relationshipType;
        //                projectContext.OneToManyRelationshipType.Add(oneToManyRelationshipType);
        //                //پشت هم که باشه نمیتونه ایدی ریلیشن تایپ رو تو وان تو منی هم بزاره!!

        //                reverseRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.ManyToOne);
        //                var reverseRelationshipType = new RelationshipType();
        //                reverseRelationshipType.Relationship = reverseRelationship;
        //                reverseRelationshipType.IsOtherSideMandatory = relationInfo.AllFKSidesHavePKSide == true;
        //                projectContext.RelationshipType.Add(reverseRelationshipType);
        //                projectContext.SaveChanges();
        //                //    reverseRelationship.RelationshipType.;
        //                var manyToOneRelationshipType = new ManyToOneRelationshipType();
        //                manyToOneRelationshipType.RelationshipType = reverseRelationshipType;
        //                projectContext.ManyToOneRelationshipType.Add(manyToOneRelationshipType);
        //                //  projectContext.SaveChanges();

        //            }
        //            else if (relationInfo.RelationType == RelationType.OnePKtoOneFK)
        //            {
        //                relationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.ImplicitOneToOne);
        //                relationship.RelationshipType = new RelationshipType();
        //                relationship.RelationshipType.ImplicitOneToOneRelationshipType = new ImplicitOneToOneRelationshipType();
        //                relationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData == true;

        //                reverseRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.ExplicitOneToOne);
        //                reverseRelationship.RelationshipType = new RelationshipType();
        //                reverseRelationship.RelationshipType.ExplicitOneToOneRelationshipType = new ExplicitOneToOneRelationshipType();
        //                reverseRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllFKSidesHavePKSide == true;
        //            }

        //            projectContext.SaveChanges();
        //        }
        //    }

        //}

        //internal bool IsRelationshipEnabled(int relationshipID)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var entity = projectContext.Relationship.First(x => x.ID == relationshipID);
        //        return entity.Enabled == true;
        //    }
        //}

        //public void SetISA_RelationshipProperties(Tuple<ISARelationshipDTO, TableDrivedEntityDTO, List<TableDrivedEntityDTO>> iSARelationship)
        //{//هربار کانکشتن استرینگ خونده نشود


        //        //}
        //        //catch (Exception ex)
        //        //{
        //        //    throw ex;
        //        //}

        //}
        //public void ImposeRelationshipTypes(TableDrivedEntityDTO entity)
        //{
        //    //Tuple<TableDrivedEntityDTO, List<Tuple<RelationshipDTO, TableDrivedEntityDTO>>> isaRelationship = null;
        //    BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        //    foreach (var relationship in entity.Relationships.Where(x => x.MastertTypeEnum == Enum_MasterRelationshipType.FromPrimartyToForeign))
        //    {
        //        ModelDataHelper dataHelper = new ModelDataHelper();
        //        var relationInfo = dataHelper.GetRelationshipsInfoWithEntityIds(relationship);
        //        if (relationInfo.FKRelatesOnPrimaryKey)
        //        {
        //            var subEntity = bizTableDrivedEntity.GetTableDrivedEntity(relationship.EntityID2, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
        //            if (isaRelationship == null)
        //            {
        //                isaRelationship = new Tuple<TableDrivedEntityDTO, List<Tuple<RelationshipDTO, TableDrivedEntityDTO>>>(entity, new List<Tuple<RelationshipDTO, TableDrivedEntityDTO>>() { new Tuple<RelationshipDTO, TableDrivedEntityDTO>(relationship, subEntity) });
        //            }
        //            else
        //                isaRelationship.Item2.Add(new Tuple<RelationshipDTO, TableDrivedEntityDTO>(relationship, subEntity));
        //        }
        //        else
        //            ImposeSingleRelationshipType(relationship, relationInfo);
        //    }
        //    if (isaRelationship != null)
        //    {
        //        ModelDataHelper dataHelper = new ModelDataHelper();
        //        var isaRelationshipDetails = dataHelper.GetISARelationshipDetail(isaRelationship);
        //        ImposeISARelationshipType(isaRelationship, isaRelationshipDetails);
        //    }
        //}

        //private void ImposeISARelationshipType(Tuple<TableDrivedEntityDTO, List<Tuple<RelationshipDTO, TableDrivedEntityDTO>>> isaRelationship, ISARelationshipDetail isaRelationshipDetails)
        //{
        //    //شناسایی دسته جمعی توسط موجودیت پدر
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        foreach (var item in isaRelationship.Item2)
        //        {
        //            var relationship = projectContext.Relationship.First(x => x.ID == item.Item1.ID);
        //            if (relationship.RelationshipType == null)
        //            {
        //                var reverseRelationship = projectContext.Relationship.First(x => x.RelationshipID == relationship.ID);

        //                relationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.SuperToSub);
        //                relationship.RelationshipType = new RelationshipType();
        //                relationship.RelationshipType.SuperToSubRelationshipType = new SuperToSubRelationshipType();

        //                reverseRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.SubToSuper);
        //                reverseRelationship.RelationshipType = new RelationshipType();
        //                reverseRelationship.RelationshipType.SubToSuperRelationshipType = new SubToSuperRelationshipType();

        //                var dbisaRelationship = new ISARelationship();
        //                dbisaRelationship.Name = isaRelationshipDetails.Name;
        //                dbisaRelationship.IsDisjoint = isaRelationshipDetails.IsDisjoint;
        //                dbisaRelationship.IsTolatParticipation = isaRelationshipDetails.IsTotalParticipation;

        //                relationship.RelationshipType.SuperToSubRelationshipType.ISARelationship = dbisaRelationship;
        //                reverseRelationship.RelationshipType.SubToSuperRelationshipType.ISARelationship = dbisaRelationship;
        //            }
        //        }
        //    }
        //}

        //رابطه باید از پرایمری به فارن باشد
        //public DeterminedRelationshiop DetremineRelationshipType(RelationshipDTO relationship)
        //{
        //    DeterminedRelationshiop result = new MyModelManager.DeterminedRelationshiop();

        //    ModelDataHelper dataHelper = new ModelDataHelper();
        //    var relationInfo = dataHelper.GetRelationshipsInfo(relationship);

        //    if (relationInfo.FKRelatesOnPrimaryKey)
        //    {
        //        result.Type = Enum_RelationshipType.SuperToSub;
        //        result.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData;
        //        result.IsReverseSideMandatory = true;
        //    }

        //    else if (relationInfo.RelationType == RelationType.OnePKtoOneFK)
        //    {
        //        result.Type = Enum_RelationshipType.ImplicitOneToOne;
        //        result.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData;
        //        result.IsReverseSideMandatory = relationInfo.AllFKSidesHavePKSide;
        //    }
        //    else if (relationInfo.RelationType == RelationType.OnePKtoManyFK)
        //    {
        //        result.Type = Enum_RelationshipType.OneToMany;
        //        result.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData;
        //        result.IsReverseSideMandatory = relationInfo.AllFKSidesHavePKSide;

        //    }
        //    return result;
        //}
        //بهتره حذف بشه 
        //public int GetReverseRelationshipID(int relationshipID)
        //{

        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var dbRelationship = projectContext.Relationship.First(x => x.ID == relationshipID);
        //        if (dbRelationship.Relationship2 != null)
        //            return dbRelationship.RelationshipID.Value;
        //        else
        //            return projectContext.Relationship.First(x => x.RelationshipID == relationshipID).ID;
        //    }
        //}

        //public Enum_RelationshipType GetReverseRelationshipType(Enum_RelationshipType sourceToTargetRelationshipType)
        //{
        //    if (sourceToTargetRelationshipType == Enum_RelationshipType.ExplicitOneToOne)
        //        return Enum_RelationshipType.ImplicitOneToOne;
        //    else if (sourceToTargetRelationshipType == Enum_RelationshipType.ImplicitOneToOne)
        //        return Enum_RelationshipType.ExplicitOneToOne;
        //    else if (sourceToTargetRelationshipType == Enum_RelationshipType.ManyToOne)
        //        return Enum_RelationshipType.OneToMany;
        //    else if (sourceToTargetRelationshipType == Enum_RelationshipType.OneToMany)
        //        return Enum_RelationshipType.ManyToOne;
        //    else if (sourceToTargetRelationshipType == Enum_RelationshipType.SubToSuper)
        //        return Enum_RelationshipType.SuperToSub;
        //    else if (sourceToTargetRelationshipType == Enum_RelationshipType.SuperToSub)
        //        return Enum_RelationshipType.SubToSuper;
        //    else if (sourceToTargetRelationshipType == Enum_RelationshipType.SubUnionToUnion_SubUnionHoldsKeys)
        //        return Enum_RelationshipType.UnionToSubUnion_SubUnionHoldsKeys;
        //    else if (sourceToTargetRelationshipType == Enum_RelationshipType.UnionToSubUnion_SubUnionHoldsKeys)
        //        return Enum_RelationshipType.SubUnionToUnion_SubUnionHoldsKeys;
        //    else if (sourceToTargetRelationshipType == Enum_RelationshipType.SubUnionToUnion_UnionHoldsKeys)
        //        return Enum_RelationshipType.UnionToSubUnion_UnionHoldsKeys;
        //    else if (sourceToTargetRelationshipType == Enum_RelationshipType.UnionToSubUnion_UnionHoldsKeys)
        //        return Enum_RelationshipType.SubUnionToUnion_UnionHoldsKeys;
        //    return Enum_RelationshipType.None;
        //}

        public List<RelationshipDTO> GetRelationshipsByTableID(int tableID)
        {
            List<RelationshipDTO> result = new List<RelationshipDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var list = GetRelationships(projectContext).Where(x => x.TableDrivedEntity.TableID == tableID || x.TableDrivedEntity1.TableID == tableID);
                foreach (var item in list)
                {
                    result.Add(ToRelationshipDTO(item));
                }
            }
            return result;
        }
        //public List<RelationshipDTO> GetRelationshipsByTableIDasSource(int tableID)
        //{
        //    List<RelationshipDTO> result = new List<RelationshipDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var list = GetRelationships(projectContext).Where(x => x.TableDrivedEntity.TableID == tableID);
        //        foreach (var item in list)
        //        {
        //            result.Add(ToRelationshipDTO(item));
        //        }
        //    }
        //    return result;
        //}
        public List<RelationshipDTO> GetRelationships(int databaseID)
        {
            List<RelationshipDTO> result = new List<RelationshipDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
                var list = GetRelationships(projectContext).Where(x => x.TableDrivedEntity.Table.DBSchema.DatabaseInformationID == databaseID);
                foreach (var item in list)
                {
                    result.Add(ToRelationshipDTO(item));
                }
            }
            return result;
        }
        //هر دو طرف تکرار میشوند
        //public List<RelationshipDTO> GetRelationshipsByEntityIDBothSides(int tableDrivedEntityID)
        //{
        //    List<RelationshipDTO> result = new List<RelationshipDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var list = projectContext.Relationship.Where(x => x.TableDrivedEntityID1 == tableDrivedEntityID || x.TableDrivedEntityID2 == tableDrivedEntityID);
        //        foreach (var item in list)
        //        {
        //            result.Add(ToRelationshipDTO(item));
        //        }
        //    }
        //    return result;
        //}
        //public List<RelationshipDTO> GetRelationshipsByEntityID(int tableDrivedEntityID)
        //{
        //    List<RelationshipDTO> result = new List<RelationshipDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var list = projectContext.Relationship.Where(x => x.TableDrivedEntityID1 == tableDrivedEntityID);
        //        foreach (var item in list)
        //        {
        //            result.Add(ToRelationshipDTO(item));
        //        }
        //    }
        //    return result;
        //}

        //public List<RelationshipDTO> GetRelationshipsBetweenEntities(int firstEntityID, int secondEnitityID)
        //{
        //    List<RelationshipDTO> result = new List<RelationshipDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var list = projectContext.Relationship.Where(x => x.TableDrivedEntityID1 == firstEntityID && x.TableDrivedEntityID2 == secondEnitityID);
        //        foreach (var item in list)
        //        {
        //            result.Add(ToRelationshipDTO(item));
        //        }
        //    }
        //    return result;
        //}

        //public List<RelationshipDTO> GetRelationshipsByEntityIDasSource(int tableDrivedEntityID)
        //{
        //    List<RelationshipDTO> result = new List<RelationshipDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var list = projectContext.Relationship.Where(x => x.TableDrivedEntityID1 == tableDrivedEntityID);
        //        foreach (var item in list)
        //        {
        //            result.Add(ToRelationshipDTO(item));
        //        }
        //    }
        //    return result;
        //}
        public void UpdateRelationships(List<RelationshipDTO> relationships)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var relationship in relationships)
                {
                    var dbRelationship = ToRelationshipDB(relationship, projectContext);
                }
                projectContext.SaveChanges();
            }
        }
        private Relationship ToRelationshipDB(RelationshipDTO item, MyProjectEntities projectContext)
        {

            var dbRelationship = projectContext.Relationship.First(x => x.ID == item.ID);
            dbRelationship.Alias = item.Alias;
            dbRelationship.Name = item.Name;

            //bool fColIsPrimaryKey = false;
            //if (item.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary)
            //    fColIsPrimaryKey = dbRelationship.RelationshipColumns.Any(x => x.Column.PrimaryKey);
            //else
            //    fColIsPrimaryKey = dbRelationship.RelationshipColumns.Any(x => x.Column1.PrimaryKey);

            List<Column> fkColumns = new List<Column>();
            foreach (var relCol in dbRelationship.RelationshipColumns)
            {
                if (item.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary)
                    fkColumns = dbRelationship.RelationshipColumns.Select(x => x.Column).ToList();
                else
                    fkColumns = dbRelationship.RelationshipColumns.Select(x => x.Column1).ToList();
            }

            bool fkColumnReadonly = false;
            bool fkColumnDisabled = false;
            bool? fkColumnDataEntryEnabled = null;
            bool fkcolumnIsNotTransferable = false;
            if (item.AnyForeignKeyIsPrimaryKey)
            {
                fkcolumnIsNotTransferable = true;
                fkColumnReadonly = item.IsReadonly;
                fkColumnDisabled = false;
                if (item.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary)
                    fkColumnDataEntryEnabled = true;
            }
            else
            {
                if (item.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary)
                    fkColumnDataEntryEnabled = item.DataEntryEnabled;
                fkColumnReadonly = item.IsReadonly;
                fkColumnDisabled = item.IsDisabled;
                fkcolumnIsNotTransferable = item.IsNotTransferable; ;
            }
            foreach (var fkColumn in fkColumns)
            {
                if (fkColumnDataEntryEnabled != null)
                    fkColumn.DataEntryEnabled = fkColumnDataEntryEnabled.Value;
                fkColumn.IsReadonly = fkColumnReadonly;
                fkColumn.IsDisabled = fkColumnDisabled;
                fkColumn.IsNotTransferable = fkcolumnIsNotTransferable;
            }


            //برای فعال غیر فعال بودن رابطه برعکسه دسترسی و عدم دسترسی ستونها دیتکاری نمیشوند زیرا غیرفعال بودن رابطه دلیل غیر فعال شدن ستونها نیست
            //dbRelationship.Removed = item.Removed;
            dbRelationship.SearchInitially = item.SearchInitially;
            //dbRelationship.IsReadonly = item.IsReadonly;

            dbRelationship.RelationshipType.IsOtherSideCreatable = item.IsOtherSideCreatable;
            dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = item.IsOtherSideDirectlyCreatable;
            dbRelationship.RelationshipType.IsOtherSideMandatory = item.IsOtherSideMandatory;
            dbRelationship.RelationshipType.IsNotSkippable = item.IsNotSkippable;

            //dbRelationship.RelationshipType.IsOtherSideTransferable = item.IsOtherSideTransferable;
            dbRelationship.RelationshipType.DeleteOption = (short)item.DeleteOption;
            if (item.MastertTypeEnum == Enum_MasterRelationshipType.FromPrimartyToForeign)
                dbRelationship.RelationshipType.PKToFKDataEntryEnabled = item.DataEntryEnabled;
            else
                dbRelationship.RelationshipType.PKToFKDataEntryEnabled = null;


            return dbRelationship;
        }
        public RelationshipDTO ToRelationshipDTO(DataAccess.Relationship item)
        {
            //var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.Relationship, item.ID.ToString());
            //if (cachedItem != null)
            //    return (cachedItem as RelationshipDTO);

            RelationshipDTO result = new RelationshipDTO();
            result.Name = item.Name;
            result.Info = item.Info;
            if (!string.IsNullOrEmpty(item.Alias))
                result.Alias = item.Alias;
            else
                result.Alias = item.TableDrivedEntity.Name + ">" + item.TableDrivedEntity1.Name;
            result.ID = item.ID;
            //بهتره PairRelationshipID همواره ایدی برعکس را داشته باشد
            if (item.RelationshipID != null)
                result.PairRelationshipID = item.RelationshipID.Value;
            else
                result.PairRelationshipID = item.Relationship1.First().ID;
            result.EntityID1 = item.TableDrivedEntityID1;
            result.EntityID2 = item.TableDrivedEntityID2;
            result.Entity1 = item.TableDrivedEntity.Name;
            result.Entity2 = item.TableDrivedEntity1.Name;
            result.OtherSideIsView = item.TableDrivedEntity1.IsView;
            result.Entity1Alias = item.TableDrivedEntity.Alias;
            result.Entity2Alias = item.TableDrivedEntity1.Alias;
            result.TableID1 = item.TableDrivedEntity.TableID;
            result.TableID2 = item.TableDrivedEntity1.TableID;
            result.TableName1 = item.TableDrivedEntity.Table.Name;
            result.TableName2 = item.TableDrivedEntity1.Table.Name;
            result.TableSchema1 = item.TableDrivedEntity.Table.DBSchema.Name;
            result.TableSchema2 = item.TableDrivedEntity1.Table.DBSchema.Name;
            result.DatabaseID1 = item.TableDrivedEntity.Table.DBSchema.DatabaseInformationID;
            result.DatabaseID2 = item.TableDrivedEntity1.Table.DBSchema.DatabaseInformationID;
            result.DatabaseName1 = item.TableDrivedEntity.Table.DBSchema.DatabaseInformation.Name;
            result.DatabaseName2 = item.TableDrivedEntity1.Table.DBSchema.DatabaseInformation.Name;
            result.ServerID1 = item.TableDrivedEntity.Table.DBSchema.DatabaseInformation.DBServerID;
            result.ServerID2 = item.TableDrivedEntity1.Table.DBSchema.DatabaseInformation.DBServerID;
            result.FromDifferentServer = result.ServerID1 != result.ServerID2;
            result.ServerName1 = item.TableDrivedEntity.Table.DBSchema.DatabaseInformation.DBServer.Name;
            result.ServerName2 = item.TableDrivedEntity1.Table.DBSchema.DatabaseInformation.DBServer.Name;
            result.Entity2IsIndependent = item.TableDrivedEntity1.IndependentDataEntry == true;



            if (result.FromDifferentServer)
            {
                var linkedServer = item.TableDrivedEntity.Table.DBSchema.DatabaseInformation.DBServer.LinkedServer.FirstOrDefault(x => x.SourceDBServerID == result.ServerID1);
                if (linkedServer != null)
                    result.LinkedServer = linkedServer.Name;
            }


            //if (result.ServerName1 != result.ServerName2)
            //{
            //   var linkedServer1 = item.TableDrivedEntity.Table.DBSchema.DatabaseInformation.DBServer.LinkedServer.FirstOrDefault(x => x.DBServer.Name == result.ServerName1
            //   && x.DBServer1.Name == result.ServerName2);
            //   if (linkedServer1 != null)
            //       result.LinkedServer1 = linkedServer1.Name;
            //   var linkedServer2 = item.TableDrivedEntity1.Table.DBSchema.DatabaseInformation.DBServer.LinkedServer.FirstOrDefault(x => x.DBServer.Name == result.ServerName2
            //&& x.DBServer1.Name == result.ServerName1);
            //   if (linkedServer2 != null)
            //       result.LinkedServer2 = linkedServer2.Name;
            //}
            if (item.TypeEnum != null)
                result.TypeEnum = (Enum_RelationshipType)item.TypeEnum.Value;
            else
                result.TypeEnum = Enum_RelationshipType.None;
            //موقتی

            result.MastertTypeEnum = (Enum_MasterRelationshipType)item.MasterTypeEnum;// GetMastertRelationshipType(item);

            foreach (var relcolumn in item.RelationshipColumns)
            {
                var rColumn = ToRelationshipColumn(result.MastertTypeEnum, relcolumn);
                result.RelationshipColumns.Add(rColumn);
            }
            result.FKSidePKColumnsAreFkColumns = FKSidePKColumnsAreFkColumns(item);
            // result.RelatesOnPrimaryKeys = RelatesOnPrimaryKeys(item);
            if (result.MastertTypeEnum == Enum_MasterRelationshipType.FromPrimartyToForeign)
            {
                result.FKCoumnIsNullable = result.RelationshipColumns.Any(x => x.SecondSideColumn.IsNull);
                result.AnyForeignKeyIsPrimaryKey = result.RelationshipColumns.Any(x => x.SecondSideColumn.PrimaryKey);
            }
            else
            {
                result.FKCoumnIsNullable = result.RelationshipColumns.Any(x => x.FirstSideColumn.IsNull);
                result.AnyForeignKeyIsPrimaryKey = result.RelationshipColumns.Any(x => x.FirstSideColumn.PrimaryKey);
            }
            result.SearchInitially = item.SearchInitially;
            result.TypeStr = result.TypeEnum.ToString();

            result.Removed = item.Removed == true;
            //result.IsReadonly = item.IsReadonly == true;
            result.Created = item.Created == true;



            ColumnDTO firstfkColumn = null;
            if (result.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary)
                firstfkColumn = result.RelationshipColumns.First().FirstSideColumn;
            else
                firstfkColumn = result.RelationshipColumns.First().SecondSideColumn;
            result.IsReadonly = firstfkColumn.IsReadonly;
            result.IsDisabled = firstfkColumn.IsDisabled;
            result.IsNotTransferable = firstfkColumn.IsNotTransferable;




            if (result.MastertTypeEnum == Enum_MasterRelationshipType.FromPrimartyToForeign)
                result.DataEntryEnabled = item.RelationshipType.PKToFKDataEntryEnabled == true;
            else
                result.DataEntryEnabled = firstfkColumn.DataEntryEnabled;

            result.IsOtherSideMandatory = item.RelationshipType.IsOtherSideMandatory;
            result.IsOtherSideCreatable = item.RelationshipType.IsOtherSideCreatable;
            result.IsNotSkippable = item.RelationshipType.IsNotSkippable;
            result.IsOtherSideDirectlyCreatable = item.RelationshipType.IsOtherSideDirectlyCreatable;
            //result.IsOtherSideTransferable = item.RelationshipType.IsOtherSideTransferable;
            if (item.RelationshipType.DeleteOption != null)
                result.DeleteOption = (RelationshipDeleteOption)item.RelationshipType.DeleteOption.Value;

            CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.Relationship, item.ID.ToString());
            return result;
        }

        public bool FKSidePKColumnsAreFkColumns(Relationship relationship)
        {
            TableDrivedEntity fkEntity = null;
            if ((Enum_MasterRelationshipType)relationship.MasterTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary)
            {
                fkEntity = relationship.TableDrivedEntity;
            }
            else
            {
                fkEntity = relationship.TableDrivedEntity1;
            }
            var fkrelColumns = relationship.RelationshipColumns.Select(x => x.SecondSideColumnID).ToList();
            List<Column> primaryKeys = null;
            if (fkEntity.TableDrivedEntity_Columns.Any())
                primaryKeys = fkEntity.TableDrivedEntity_Columns.Where(x => x.Column.PrimaryKey).Select(x => x.Column).ToList();
            else
                primaryKeys = fkEntity.Table.Column.Where(x => x.PrimaryKey).ToList();
            return primaryKeys.All(y => fkrelColumns.Any(z => y.ID == z)) &&
             fkrelColumns.All(x => primaryKeys.Any(z => z.ID == x));
        }

        public RelationshipColumnDTO ToRelationshipColumn(Enum_MasterRelationshipType masterType, RelationshipColumns relcolumn)
        {
            BizColumn bizColumn = new BizColumn();
            var rColumn = new RelationshipColumnDTO();

            rColumn.FirstSideColumnID = relcolumn.FirstSideColumnID;
            rColumn.FirstSideColumn = bizColumn.ToColumnDTO(relcolumn.Column, true);

            rColumn.SecondSideColumnID = relcolumn.SecondSideColumnID;
            rColumn.SecondSideColumn = bizColumn.ToColumnDTO(relcolumn.Column1, true);

            return rColumn;
        }

        //public Enum_MasterRelationshipType GetMastertRelationshipType(Relationship item)
        //{
        //    //بهتره یه فیلد تو ریلیشنشیپ قرار بگیره و نوع مستر از اول ست شود
        //    if ((item.ID % 2) == 0)
        //        return Enum_MasterRelationshipType.FromForeignToPrimary;
        //    else
        //        return Enum_MasterRelationshipType.FromPrimartyToForeign;
        //    //var keyColumns = item.TableDrivedEntity.Table.Column.Where(x => x.PrimaryKey).Select(x => x.ID).ToList();
        //    //if (keyColumns.All(x => item.RelationshipColumns.Any(y => x == y.FirstSideColumnID)))

        //    //else
        //    //    return Enum_MasterRelationshipType.FromForeignToPrimary;

        //}


        //public Enum_RelationshipType GetRelationshipType(Relationship relationship)
        //{
        //    if (relationship.TypeEnum != null)
        //    {

        //        if (relationship.RelationshipType.OneToManyRelationshipType != null)
        //            return Enum_RelationshipType.OneToMany;
        //        else if (relationship.RelationshipType.ManyToOneRelationshipType != null)
        //            return Enum_RelationshipType.ManyToOne;
        //        else if (relationship.RelationshipType.ExplicitOneToOneRelationshipType != null)
        //            return Enum_RelationshipType.ExplicitOneToOne;
        //        else if (relationship.RelationshipType.ImplicitOneToOneRelationshipType != null)
        //            return Enum_RelationshipType.ImplicitOneToOne;
        //        else if (relationship.RelationshipType.SuperToSubRelationshipType != null)
        //            return Enum_RelationshipType.SuperToSub;
        //        else if (relationship.RelationshipType.SubToSuperRelationshipType != null)
        //            return Enum_RelationshipType.SubToSuper;
        //        else if (relationship.RelationshipType.UnionToSubUnionRelationshipType != null && relationship.RelationshipType.UnionToSubUnionRelationshipType.UnionRelationshipType.UnionHoldsKeys)
        //            return Enum_RelationshipType.UnionToSubUnion_UnionHoldsKeys;
        //        else if (relationship.RelationshipType.SubUnionToUnionRelationshipType != null && relationship.RelationshipType.SubUnionToUnionRelationshipType.UnionRelationshipType.UnionHoldsKeys)
        //            return Enum_RelationshipType.SubUnionToUnion_UnionHoldsKeys;
        //        else if (relationship.RelationshipType.UnionToSubUnionRelationshipType != null && !relationship.RelationshipType.UnionToSubUnionRelationshipType.UnionRelationshipType.UnionHoldsKeys)
        //            return Enum_RelationshipType.UnionToSubUnion_SubUnionHoldsKeys;
        //        else if (relationship.RelationshipType.SubUnionToUnionRelationshipType != null && !relationship.RelationshipType.SubUnionToUnionRelationshipType.UnionRelationshipType.UnionHoldsKeys)
        //            return Enum_RelationshipType.SubUnionToUnion_SubUnionHoldsKeys;
        //        else
        //            return Enum_RelationshipType.None;
        //        // else if (relationship.RelationshipType.OneToManyRelationshipType != null)
        //        //    return Enum_RelationshipType.;
        //        //else if (relationship.RelationshipType.OneToManyRelationshipType != null)
        //        //    return Enum_RelationshipType.;
        //        //else if (relationship.RelationshipType.OneToManyRelationshipType != null)
        //        //    return Enum_RelationshipType.;
        //    }
        //    else
        //        return Enum_RelationshipType.None;

        //}

        public bool RelationshipIsNotNulable(int relationshipID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbRelationship = projectContext.Relationship.First(x => x.ID == relationshipID);
                return dbRelationship.RelationshipColumns.Any(y => y.Column.PrimaryKey == false && y.Column.IsNull == false);
            }
        }


        //public event EventHandler<SimpleGenerationInfoArg> RuleImposedEvent;

        //public void ImposeRelationshipRule(int databaseID)
        //{
        //    //string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
        //    //MyProjectEntities context = new MyProjectEntities();
        //    //context.Configuration.LazyLoadingEnabled = true;
        //    //var list = context.TableDrivedEntity.Where(x => x.Table.DBSchema.DatabaseInformationID == databaseID);

        //    int index = 0;
        //    BizTableDrivedEntity bizEntity = new MyModelManager.BizTableDrivedEntity();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var listEntity = projectContext.TableDrivedEntity.Where(x => x.Table.DBSchema.DatabaseInformationID == databaseID);
        //        var count = listEntity.Count();
        //        foreach (var item in listEntity)
        //        {
        //            index++;
        //            var entity = bizEntity.GetTableDrivedEntity(item.ID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithRelationships);
        //            ImposeRelationshipTypes(entity);
        //            //if (RuleImposedEvent != null)
        //            //    RuleImposedEvent(this, new SimpleGenerationInfoArg() { Title = entity.Name, CurrentProgress = index, TotalProgressCount = count });

        //        }
        //    }
        //    //try
        //    //{
        //    //    context.SaveChanges();
        //    //}
        //    //catch (System.Data.Entity.Validation.DbEntityValidationException e)
        //    //{
        //    //    foreach (var eve in e.EntityValidationErrors)
        //    //    {
        //    //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //    //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //    //        foreach (var ve in eve.ValidationErrors)
        //    //        {
        //    //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //    //                ve.PropertyName, ve.ErrorMessage);
        //    //        }
        //    //    }
        //    //    throw;
        //    //}
        //    //if (RuleImposedEvent != null)
        //    //    RuleImposedEvent(this, new SimpleGenerationInfoArg() { Title = "Operation is completed.", CurrentProgress = index, TotalProgressCount = count });
        //}

        //public List<OneToManyRelationshipDTO> GetOneToManyRelationships(int databaseID)
        //{
        //    List<OneToManyRelationshipDTO> result = new List<OneToManyRelationshipDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        //projectContext.Configuration.LazyLoadingEnabled = false;
        //        //string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
        //        var list = GetRelationships(projectContext).Where(x => x.TableDrivedEntity.Table.DBSchema.DatabaseInformationID == databaseID
        //           && x.RelationshipType != null && x.RelationshipType.OneToManyRelationshipType != null);
        //        foreach (var item in list)
        //        {

        //            result.Add(ToOneToManyRelationship(item.RelationshipType.OneToManyRelationshipType));

        //        }
        //        return result;
        //    }
        //}
        public OneToManyRelationshipDTO ToOneToManyRelationship(OneToManyRelationshipType oneToManyRelationshipType, RelationshipDTO baseRelationship = null)
        {
            BizRelationship biz = new MyModelManager.BizRelationship();
            if (baseRelationship == null)
                baseRelationship = biz.ToRelationshipDTO(oneToManyRelationshipType.RelationshipType.Relationship);
            Mapper.Initialize(cfg => cfg.CreateMap<RelationshipDTO, OneToManyRelationshipDTO>());
            var result = AutoMapper.Mapper.Map<RelationshipDTO, OneToManyRelationshipDTO>(baseRelationship);


            result.MasterDetailState = oneToManyRelationshipType.MasterDetailState;
            result.DetailsCount = oneToManyRelationshipType.ManySideCount;


            return result;
        }
        //public void UpdateOneToManyRelationships(List<OneToManyRelationshipDTO> relationships)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        foreach (var relationship in relationships)
        //        {
        //            var dbRelationship = projectContext.OneToManyRelationshipType.First(x => x.ID == relationship.ID);
        //            var dbrel = ToRelationship(relationship, projectContext);
        //            dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = relationship.IsOtherSideDirectlyCreatable;
        //            dbRelationship.RelationshipType.IsOtherSideCreatable = relationship.IsOtherSideCreatable;
        //            dbRelationship.RelationshipType.IsOtherSideMandatory = relationship.IsOtherSideMandatory;
        //            dbRelationship.MasterDetailState = relationship.MasterDetailState;
        //            dbRelationship.ManySideCount = relationship.DetailsCount;
        //            dbRelationship.RelationshipType.Relationship.Name = relationship.Name;
        //            dbRelationship.RelationshipType.Relationship.Alias = relationship.Alias;
        //            dbRelationship.RelationshipType.Relationship.Enabled = relationship.Enabled;
        //        }
        //        projectContext.SaveChanges();
        //    }
        //}



        //public List<ManyToOneRelationshipDTO> GetManyToOneRelationships(int databaseID)
        //{
        //    List<ManyToOneRelationshipDTO> result = new List<ManyToOneRelationshipDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        //projectContext.Configuration.LazyLoadingEnabled = false;
        //        //string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
        //        var list = GetRelationships(projectContext).Where(x => x.TableDrivedEntity.Table.DBSchema.DatabaseInformationID == databaseID
        //           && x.RelationshipType != null && x.RelationshipType.ManyToOneRelationshipType != null);
        //        foreach (var item in list)
        //        {

        //            result.Add(ToManyToOneRelationshipDTO(item.RelationshipType.ManyToOneRelationshipType));

        //        }
        //        return result;
        //    }
        //}
        public ManyToOneRelationshipDTO ToManyToOneRelationshipDTO(ManyToOneRelationshipType manyToOneRelationshipType, RelationshipDTO baseRelationship = null)
        {
            BizRelationship biz = new MyModelManager.BizRelationship();
            if (baseRelationship == null)
                baseRelationship = biz.ToRelationshipDTO(manyToOneRelationshipType.RelationshipType.Relationship);
            Mapper.Initialize(cfg => cfg.CreateMap<RelationshipDTO, ManyToOneRelationshipDTO>());
            var result = AutoMapper.Mapper.Map<RelationshipDTO, ManyToOneRelationshipDTO>(baseRelationship);


            return result;
        }
        //public void UpdateManyToOneRelationships(List<ManyToOneRelationshipDTO> relationships)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        foreach (var relationship in relationships)
        //        {
        //            var dbRelationship = projectContext.ManyToOneRelationshipType.First(x => x.ID == relationship.ID);
        //            var dbrel = ToRelationship(relationship, projectContext);
        //            dbRelationship.RelationshipType.IsOtherSideCreatable = relationship.IsOtherSideCreatable;
        //            dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = relationship.IsOtherSideDirectlyCreatable;
        //            dbRelationship.RelationshipType.IsOtherSideMandatory = relationship.IsOtherSideMandatory;
        //            dbRelationship.RelationshipType.IsOtherSideTransferable = relationship.IsOtherSideTransferable;
        //            dbRelationship.RelationshipType.Relationship.Name = relationship.Name;
        //            dbRelationship.RelationshipType.Relationship.Alias = relationship.Alias;
        //            dbRelationship.RelationshipType.Relationship.Enabled = relationship.Enabled;
        //        }
        //        projectContext.SaveChanges();
        //    }
        //}



        //public List<ExplicitOneToOneRelationshipDTO> GetExplicitOneToOneRelationships(int databaseID)
        //{
        //    List<ExplicitOneToOneRelationshipDTO> result = new List<ExplicitOneToOneRelationshipDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        //projectContext.Configuration.LazyLoadingEnabled = false;
        //        //string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
        //        var list = GetRelationships(projectContext).Where(x => x.TableDrivedEntity.Table.DBSchema.DatabaseInformationID == databaseID
        //           && x.RelationshipType != null && x.RelationshipType.ExplicitOneToOneRelationshipType != null);
        //        foreach (var item in list)
        //        {

        //            result.Add(ToExplicitOneToOneRelationshipDTO(item.RelationshipType.ExplicitOneToOneRelationshipType));

        //        }
        //        return result;
        //    }
        //}
        public ExplicitOneToOneRelationshipDTO ToExplicitOneToOneRelationshipDTO(ExplicitOneToOneRelationshipType explicitOneToOneRelationshipType, RelationshipDTO baseRelationship = null)
        {
            BizRelationship biz = new MyModelManager.BizRelationship();
            if (baseRelationship == null)
                baseRelationship = biz.ToRelationshipDTO(explicitOneToOneRelationshipType.RelationshipType.Relationship);
            Mapper.Initialize(cfg => cfg.CreateMap<RelationshipDTO, ExplicitOneToOneRelationshipDTO>());
            var result = AutoMapper.Mapper.Map<RelationshipDTO, ExplicitOneToOneRelationshipDTO>(baseRelationship);

            return result;
        }
        //public void UpdateExplicitOneToOneRelationships(List<ExplicitOneToOneRelationshipDTO> relationships)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        foreach (var relationship in relationships)
        //        {
        //            var dbRelationship = projectContext.ExplicitOneToOneRelationshipType.First(x => x.ID == relationship.ID);
        //            var dbrel = ToRelationship(relationship, projectContext);
        //            dbRelationship.RelationshipType.IsOtherSideCreatable = relationship.IsOtherSideCreatable;
        //            dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = relationship.IsOtherSideDirectlyCreatable;
        //            dbRelationship.RelationshipType.IsOtherSideMandatory = relationship.IsOtherSideMandatory;
        //            dbRelationship.RelationshipType.IsOtherSideTransferable = relationship.IsOtherSideTransferable;
        //            dbRelationship.RelationshipType.Relationship.Name = relationship.Name;
        //            dbRelationship.RelationshipType.Relationship.Alias = relationship.Alias;
        //            dbRelationship.RelationshipType.Relationship.Enabled = relationship.Enabled;
        //        }
        //        projectContext.SaveChanges();
        //    }
        //}


        //public List<ImplicitOneToOneRelationshipDTO> GetImplicitOneToOneRelationships(int databaseID)
        //{
        //    List<ImplicitOneToOneRelationshipDTO> result = new List<ImplicitOneToOneRelationshipDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        //projectContext.Configuration.LazyLoadingEnabled = false;
        //        //string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
        //        var list = GetRelationships(projectContext).Where(x => x.TableDrivedEntity.Table.DBSchema.DatabaseInformationID == databaseID
        //           && x.RelationshipType != null && x.RelationshipType.ImplicitOneToOneRelationshipType != null);
        //        foreach (var item in list)
        //        {

        //            result.Add(ToImplicitOneToOneRelationshipDTO(item.RelationshipType.ImplicitOneToOneRelationshipType));

        //        }
        //        return result;
        //    }
        //}
        public ImplicitOneToOneRelationshipDTO ToImplicitOneToOneRelationshipDTO(ImplicitOneToOneRelationshipType ImplicitOneToOneRelationshipType, RelationshipDTO baseRelationship = null)
        {
            BizRelationship biz = new MyModelManager.BizRelationship();
            if (baseRelationship == null)
                baseRelationship = biz.ToRelationshipDTO(ImplicitOneToOneRelationshipType.RelationshipType.Relationship);
            Mapper.Initialize(cfg => cfg.CreateMap<RelationshipDTO, ImplicitOneToOneRelationshipDTO>());
            var result = AutoMapper.Mapper.Map<RelationshipDTO, ImplicitOneToOneRelationshipDTO>(baseRelationship);


            return result;
        }
        //public void UpdateImplicitOneToOneRelationships(List<ImplicitOneToOneRelationshipDTO> relationships)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        foreach (var relationship in relationships)
        //        {
        //            var dbRelationship = projectContext.ImplicitOneToOneRelationshipType.First(x => x.ID == relationship.ID);
        //            var dbrel = ToRelationship(relationship, projectContext);
        //            dbRelationship.RelationshipType.IsOtherSideCreatable = relationship.IsOtherSideCreatable;
        //            dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = relationship.IsOtherSideDirectlyCreatable;
        //            dbRelationship.RelationshipType.IsOtherSideMandatory = relationship.IsOtherSideMandatory;
        //            dbRelationship.RelationshipType.IsOtherSideTransferable = relationship.IsOtherSideTransferable;
        //            dbRelationship.RelationshipType.Relationship.Name = relationship.Name;
        //            dbRelationship.RelationshipType.Relationship.Alias = relationship.Alias;
        //            dbRelationship.RelationshipType.Relationship.Enabled = relationship.Enabled;
        //        }
        //        projectContext.SaveChanges();
        //    }
        //}





        public void RemoveManyToManyRelationships(int manyToManyID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var manyToOneDB = projectContext.ManyToManyRelationshipType.First(x => x.ID == manyToManyID);
                while (manyToOneDB.ManyToOneRelationshipType.Any())
                    manyToOneDB.ManyToOneRelationshipType.Remove(manyToOneDB.ManyToOneRelationshipType.First());
                projectContext.ManyToManyRelationshipType.Remove(manyToOneDB);
                projectContext.SaveChanges();

            }
        }
        public void CreateManyToManyRelationships()
        {
            //using (var projectContext = new DataAccess.MyProjectEntities())
            //{
            //    ManyToManyRelationshipType manyToManyRelationshipType = new ManyToManyRelationshipType();
            //    manyToManyRelationshipType.Name = e.Name;
            //    foreach (var id in e.ManyToOneIDs)
            //        manyToManyRelationshipType.ManyToOneRelationshipType.Add(projectContext.ManyToOneRelationshipType.First(x => x.ID == id));

            //    var dbTable = projectContext.Table.First(x => x.ID == e.TableID);
            //    dbTable.IsAssociative = true;

            //    projectContext.ManyToManyRelationshipType.Add(manyToManyRelationshipType);
            //    projectContext.SaveChanges();

            //    var srvdb = GeneralHelper.GetServerNameDatabaseName(dbTable.Catalog);
            //    GetManyToManyRelationships(srvdb.Item1, srvdb.Item2);
            //}
        }

        public void UpdateManyToManyRelationships(List<ManyToManyRelationshipDTO> relationships)
        {

            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var relationship in relationships)
                {
                    ManyToManyRelationshipType dbRelationship = projectContext.ManyToManyRelationshipType.First(x => x.ID == relationship.ID);
                    dbRelationship.Name = relationship.Name;
                }
                projectContext.SaveChanges();
            }

        }

        public List<ManyToManyRelationshipDTO> GetManyToManyRelationships(int databaseID)
        {
            List<ManyToManyRelationshipDTO> result = new List<ManyToManyRelationshipDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                // string catalogName = GeneralHelper.GetCatalogName(databaseID);
                var list = projectContext.ManyToManyRelationshipType.Where(x => x.ManyToOneRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.Table.DBSchema.DatabaseInformationID == databaseID));
                foreach (var item in list)
                {
                    result.Add(ToManyToManyRelationshipDTO(item));
                }

            }
            return result;
        }
        public ManyToManyRelationshipDTO ToManyToManyRelationshipDTO(ManyToManyRelationshipType ManyToManyRelationshipType)
        {
            var result = new ManyToManyRelationshipDTO();
            result.Name = ManyToManyRelationshipType.Name;
            result.ID = ManyToManyRelationshipType.ID;
            return result;
        }

        public List<ManyToOneRelationshipDTO> GetManyToMany_ManyToOneRelationships(int manyToManyId)
        {
            List<ManyToOneRelationshipDTO> result = new List<ManyToOneRelationshipDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                var manyToOneList = projectContext.ManyToOneRelationshipType.Where(x => x.ManyToManyRelationshipTypeID == manyToManyId);
                if (manyToOneList != null)
                {

                    foreach (var item in manyToOneList)
                    {
                        RelationshipDTO rItem = ToRelationshipDTO(item.RelationshipType.Relationship);
                        result.Add(ToManyToOneRelationshipDTO(item));
                    }

                }

            }
            return result;
        }

        //فقط اصلی به فرعی
        //public bool RelationshipHasManyData(RelationshipDTO relationship)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        ModelDataHelper dataHelper = new ModelDataHelper();
        //        var entityInfo = dataHelper.GetRelationshipsInfo(relationship.ID);
        //        if (entityInfo.RelationType == RelationType.OnePKtoManyFK && entityInfo.FKHasData)
        //        {
        //            return true;
        //        }
        //        else
        //            return false;
        //    }
        //}
        public void ConvertRelationship(DR_Requester requester, RelationshipDTO relationship, Enum_RelationshipType targetRaltionshipType, int isaRelationshipID = 0, int unionRelationshipID = 0)
        {
            //بعدا بررسی شود
            //خصوصیات مثل اجباری بودن به هنگام تبدیل تغییر نکند
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                Relationship dbPKtoFKRelationship = null;
                Relationship dbFKtoPKRelationship = null;
                if (relationship.TypeEnum == Enum_RelationshipType.OneToMany
                    || relationship.TypeEnum == Enum_RelationshipType.ImplicitOneToOne
                    || relationship.TypeEnum == Enum_RelationshipType.SuperToSub
                    //|| relationship.TypeEnum == Enum_RelationshipType.UnionToSubUnion_SubUnionHoldsKeys
                    || relationship.TypeEnum == Enum_RelationshipType.SubUnionToUnion)
                {
                    dbPKtoFKRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID);
                    dbFKtoPKRelationship = projectContext.Relationship.First(x => x.RelationshipID == dbPKtoFKRelationship.ID);
                }
                else
                {
                    dbFKtoPKRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID);
                    dbPKtoFKRelationship = dbFKtoPKRelationship.Relationship2;
                }

                DeleteRelationshipType(projectContext, dbPKtoFKRelationship.RelationshipType);
                DeleteRelationshipType(projectContext, dbFKtoPKRelationship.RelationshipType);

                ModelDataHelper dataHelper = new ModelDataHelper();
                var relationInfo = dataHelper.GetRelationshipsInfoWithEntityIds(requester, relationship);


                if (targetRaltionshipType == Enum_RelationshipType.OneToMany
                    || targetRaltionshipType == Enum_RelationshipType.ManyToOne)
                {
                    dbPKtoFKRelationship.RelationshipType.OneToManyRelationshipType = new OneToManyRelationshipType();
                    dbPKtoFKRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData == true;
                    dbPKtoFKRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.OneToMany);

                    dbFKtoPKRelationship.RelationshipType.ManyToOneRelationshipType = new ManyToOneRelationshipType();
                    dbFKtoPKRelationship.RelationshipType.IsOtherSideCreatable = true;
                    dbFKtoPKRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllFKSidesHavePKSide == true;
                    dbFKtoPKRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.ManyToOne);
                }
                else if (targetRaltionshipType == Enum_RelationshipType.ImplicitOneToOne
                     || targetRaltionshipType == Enum_RelationshipType.ExplicitOneToOne)
                {
                    dbPKtoFKRelationship.RelationshipType.ImplicitOneToOneRelationshipType = new ImplicitOneToOneRelationshipType();
                    dbPKtoFKRelationship.RelationshipType.IsOtherSideCreatable = true;
                    dbPKtoFKRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData == true;
                    dbPKtoFKRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.ImplicitOneToOne);

                    dbFKtoPKRelationship.RelationshipType.ExplicitOneToOneRelationshipType = new ExplicitOneToOneRelationshipType();
                    dbFKtoPKRelationship.RelationshipType.IsOtherSideCreatable = true;
                    dbFKtoPKRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllFKSidesHavePKSide == true;
                    dbFKtoPKRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.ExplicitOneToOne);

                }
                else if (targetRaltionshipType == Enum_RelationshipType.SuperToSub
                    || targetRaltionshipType == Enum_RelationshipType.SubToSuper)
                {

                    dbPKtoFKRelationship.RelationshipType.SuperToSubRelationshipType = new SuperToSubRelationshipType();
                    dbPKtoFKRelationship.RelationshipType.IsOtherSideCreatable = true;
                    dbPKtoFKRelationship.RelationshipType.SuperToSubRelationshipType.ISARelationshipID = isaRelationshipID;
                    dbPKtoFKRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.SuperToSub);

                    dbFKtoPKRelationship.RelationshipType.SubToSuperRelationshipType = new SubToSuperRelationshipType();
                    dbFKtoPKRelationship.RelationshipType.IsOtherSideCreatable = true;
                    dbFKtoPKRelationship.RelationshipType.IsOtherSideMandatory = true;
                    dbFKtoPKRelationship.RelationshipType.SubToSuperRelationshipType.ISARelationshipID = isaRelationshipID;
                    dbFKtoPKRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.SubToSuper);
                }
                else if (targetRaltionshipType == Enum_RelationshipType.UnionToSubUnion
                    || targetRaltionshipType == Enum_RelationshipType.SubUnionToUnion)
                {
                    dbPKtoFKRelationship.RelationshipType.SubUnionToUnionRelationshipType = new SubUnionToUnionRelationshipType();
                    dbPKtoFKRelationship.RelationshipType.IsOtherSideCreatable = true;
                    dbPKtoFKRelationship.RelationshipType.SubUnionToUnionRelationshipType.UnionRelationshipTypeID = unionRelationshipID;
                    dbPKtoFKRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.SubUnionToUnion);

                    dbFKtoPKRelationship.RelationshipType.UnionToSubUnionRelationshipType = new UnionToSubUnionRelationshipType();
                    dbFKtoPKRelationship.RelationshipType.IsOtherSideCreatable = true;
                    dbFKtoPKRelationship.RelationshipType.UnionToSubUnionRelationshipType.UnionRelationshipTypeID = unionRelationshipID;
                    dbFKtoPKRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.UnionToSubUnion);
                }
                //else if (targetRaltionshipType == Enum_RelationshipType.SubUnionToUnion_SubUnionHoldsKeys
                //    || targetRaltionshipType == Enum_RelationshipType.UnionToSubUnion_SubUnionHoldsKeys)
                //{
                //    dbPKtoFKRelationship.RelationshipType.UnionToSubUnionRelationshipType = new UnionToSubUnionRelationshipType();
                //    dbPKtoFKRelationship.RelationshipType.IsOtherSideCreatable = true;
                //    dbPKtoFKRelationship.RelationshipType.UnionToSubUnionRelationshipType.UnionRelationshipTypeID = unionRelationshipID;
                //    dbPKtoFKRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.UnionToSubUnion_SubUnionHoldsKeys);

                //    dbFKtoPKRelationship.RelationshipType.SubUnionToUnionRelationshipType = new SubUnionToUnionRelationshipType();
                //    dbFKtoPKRelationship.RelationshipType.IsOtherSideCreatable = true;
                //    dbFKtoPKRelationship.RelationshipType.SubUnionToUnionRelationshipType.UnionRelationshipTypeID = unionRelationshipID;
                //    dbFKtoPKRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.SubUnionToUnion_SubUnionHoldsKeys);

                //}

                projectContext.SaveChanges();

            }
        }


        //public void ConvertManyToOneToExplicit(RelationshipDTO relationship, Enum_RelationshipType targetRaltionshipType, int isaRelationshipID = 0, int unionRelationshipID = 0)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var dbFKtoPKRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID);
        //        var dbPKtoFKRelationship = dbFKtoPKRelationship.Relationship2;

        //        DataHelper dataHelper = new DataHelper();
        //        var relationInfo = dataHelper.GetRelationshipsInfo(relationship.ID);

        //        projectContext.OneToManyRelationshipType.Remove(dbPKtoFKRelationship.RelationshipType.OneToManyRelationshipType);
        //        projectContext.ManyToOneRelationshipType.Remove(dbFKtoPKRelationship.RelationshipType.ManyToOneRelationshipType);

        //        if (targetRaltionshipType == Enum_RelationshipType.ExplicitOneToOne)
        //        {
        //            dbPKtoFKRelationship.RelationshipType.ImplicitOneToOneRelationshipType = new ImplicitOneToOneRelationshipType();
        //            dbPKtoFKRelationship.RelationshipType.IsOtherSideCreatable = true;
        //            dbPKtoFKRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData;

        //            dbFKtoPKRelationship.RelationshipType.ExplicitOneToOneRelationshipType = new ExplicitOneToOneRelationshipType();
        //            dbFKtoPKRelationship.RelationshipType.IsOtherSideCreatable = true;
        //            dbFKtoPKRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.FKColumnIsMandatory;
        //            projectContext.SaveChanges();
        //        }
        //        else if (targetRaltionshipType == Enum_RelationshipType.SubToSuper)
        //        {
        //            dbPKtoFKRelationship.RelationshipType.SuperToSubRelationshipType = new SuperToSubRelationshipType();
        //            dbPKtoFKRelationship.RelationshipType.IsOtherSideCreatable = true;
        //            dbPKtoFKRelationship.RelationshipType.SuperToSubRelationshipType.ISARelationshipID = isaRelationshipID;

        //            dbFKtoPKRelationship.RelationshipType.SubToSuperRelationshipType = new SubToSuperRelationshipType();
        //            dbFKtoPKRelationship.RelationshipType.IsOtherSideCreatable = true;
        //            dbFKtoPKRelationship.RelationshipType.SubToSuperRelationshipType.ISARelationshipID = isaRelationshipID;
        //            projectContext.SaveChanges();
        //        }


        //    }
        //}
        //public void ConvertManyToOneToSubToSuper(RelationshipDTO relationship, int ISARelationshipID)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var dbFKtoPKRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID);
        //        var dbPKtoFKRelationship = dbFKtoPKRelationship.Relationship2;

        //        DataHelper dataHelper = new DataHelper();
        //        var relationInfo = dataHelper.GetRelationshipsInfo(relationship.ID);

        //        projectContext.ManyToOneRelationshipType.Remove(dbPKtoFKRelationship.RelationshipType.ManyToOneRelationshipType);
        //        projectContext.ManyToOneRelationshipType.Remove(dbFKtoPKRelationship.RelationshipType.ManyToOneRelationshipType);

        //        dbPKtoFKRelationship.RelationshipType.SuperToSubRelationshipType = new SuperToSubRelationshipType();
        //        dbPKtoFKRelationship.RelationshipType.IsOtherSideCreatable = true;
        //        dbPKtoFKRelationship.RelationshipType.SuperToSubRelationshipType.ISARelationshipID = ISARelationshipID;

        //        dbFKtoPKRelationship.RelationshipType.SubToSuperRelationshipType = new SubToSuperRelationshipType();
        //        dbFKtoPKRelationship.RelationshipType.IsOtherSideCreatable = true;
        //        dbFKtoPKRelationship.RelationshipType.SubToSuperRelationshipType.ISARelationshipID = ISARelationshipID;

        //        projectContext.SaveChanges();
        //    }
        //}
        //public void ConvertManyToOneToSubUnionToUnion(RelationshipDTO relationship, int UnionRelationshipID)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var dbPKtoFKRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID);
        //        var dbFKtoPKRelationship = projectContext.Relationship.First(x => x.RelationshipID == dbPKtoFKRelationship.ID);

        //        DataHelper dataHelper = new DataHelper();
        //        var relationInfo = dataHelper.GetRelationshipsInfo(relationship.ID);

        //        projectContext.ManyToOneRelationshipType.Remove(dbPKtoFKRelationship.RelationshipType.ManyToOneRelationshipType);
        //        projectContext.ManyToOneRelationshipType.Remove(dbFKtoPKRelationship.RelationshipType.ManyToOneRelationshipType);

        //        dbPKtoFKRelationship.RelationshipType.SubUnionToUnionRelationshipType = new SubUnionToUnionRelationshipType();
        //        dbPKtoFKRelationship.RelationshipType.IsOtherSideCreatable = true;
        //        dbPKtoFKRelationship.RelationshipType.SubUnionToUnionRelationshipType.UnionRelationshipTypeID = UnionRelationshipID;

        //        dbFKtoPKRelationship.RelationshipType.UnionToSubUnionRelationshipType = new UnionToSubUnionRelationshipType();
        //        dbFKtoPKRelationship.RelationshipType.IsOtherSideCreatable = true;
        //        dbFKtoPKRelationship.RelationshipType.UnionToSubUnionRelationshipType.UnionRelationshipTypeID = UnionRelationshipID;

        //        projectContext.SaveChanges();
        //    }
        //}

        //public void ConvertManyToOneToUnionToSubUnion(RelationshipDTO relationship, int UnionRelationshipID)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var dbPKtoFKRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID);
        //        var dbFKtoPKRelationship = projectContext.Relationship.First(x => x.RelationshipID == dbPKtoFKRelationship.ID);

        //        DataHelper dataHelper = new DataHelper();
        //        var relationInfo = dataHelper.GetRelationshipsInfo(relationship.ID);

        //        projectContext.ManyToOneRelationshipType.Remove(dbPKtoFKRelationship.RelationshipType.ManyToOneRelationshipType);
        //        projectContext.ManyToOneRelationshipType.Remove(dbFKtoPKRelationship.RelationshipType.ManyToOneRelationshipType);

        //        dbPKtoFKRelationship.RelationshipType.UnionToSubUnionRelationshipType = new UnionToSubUnionRelationshipType();
        //        dbPKtoFKRelationship.RelationshipType.IsOtherSideCreatable = true;
        //        dbPKtoFKRelationship.RelationshipType.UnionToSubUnionRelationshipType.UnionRelationshipTypeID = UnionRelationshipID;

        //        dbFKtoPKRelationship.RelationshipType.SubUnionToUnionRelationshipType = new SubUnionToUnionRelationshipType();
        //        dbFKtoPKRelationship.RelationshipType.IsOtherSideCreatable = true;
        //        dbFKtoPKRelationship.RelationshipType.SubUnionToUnionRelationshipType.UnionRelationshipTypeID = UnionRelationshipID;

        //        projectContext.SaveChanges();
        //    }
        //}
        //public void ConvertRelationship(RelationshipDTO relationship)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {

        //        if (convertType == "OneToMany_ImplicitOneToOne"
        //            || convertType == "OneToMany_SuperToSub"
        //            || convertType == "OneToMany_SubUnionToUnion"
        //            || convertType == "OneToMany_UnionToSubUnion"
        //            || convertType == "ManyToOne_ExplicitOneToOne"
        //            || convertType == "ManyToOne_SuperToSub"
        //            || convertType == "ManyToOne_SubUnionToUnion"
        //            || convertType == "ManyToOne_UnionToSubUnion")
        //        {
        //            Relationship dbRelationship = null;
        //            if (convertType == "ManyToOne_ExplicitOneToOne"
        //             || convertType == "ManyToOne_SuperToSub"
        //             || convertType == "ManyToOne_SubUnionToUnion"
        //             || convertType == "ManyToOne_UnionToSubUnion")
        //            {
        //                if (convertType == "ManyToOne_ExplicitOneToOne")
        //                    convertType = "OneToMany_ImplicitOneToOne";
        //                else if (convertType == "ManyToOne_SuperToSub")
        //                    convertType = "OneToMany_SuperToSub";
        //                else if (convertType == "ManyToOne_SubUnionToUnion")
        //                    convertType = "OneToMany_UnionToSubUnion";
        //                else if (convertType == "ManyToOne_UnionToSubUnion")
        //                    convertType = "OneToMany_SubUnionToUnion";
        //                dbRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID).Relationship2;
        //            }
        //            else
        //                dbRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID);
        //            //OneToMany oneToMany = relationship as OneToMany;
        //            //dbRelationship = projectContext.Relationship.First(x => x.ID == dbRelationship.ID);





        //            else if (convertType == "OneToMany_SuperToSub")
        //            {
        //                var existingISA = projectContext.ISARelationship.Where(x => x.SuperToSubRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity.ID));
        //                ISARelationshipCreateOrSelect(existingISA.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
        //            }//union standard
        //            else if (convertType == "OneToMany_SubUnionToUnion")
        //            {
        //                var existingUnionRelarionship = projectContext.UnionRelationshipType.Where(x => x.UnionToSubUnionRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity1.ID));
        //                UnionRelationshipCreateOrSelect(existingUnionRelarionship.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
        //            }
        //            else if (convertType == "OneToMany_UnionToSubUnion")
        //            {
        //                var existingUnionRelarionship = projectContext.UnionRelationshipType.Where(x => x.UnionToSubUnionRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity.ID));
        //                UnionRelationshipCreateOrSelect(existingUnionRelarionship.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
        //            }
        //        }
        //        else if (convertType == "ImplicitOneToOne_OneToMany"
        //             || convertType == "ImplicitOneToOne_SuperToSub"
        //             || convertType == "ImplicitOneToOne_SubUnionToUnion"
        //             || convertType == "ImplicitOneToOne_UnionToSubUnion"
        //             || convertType == "ExplicitOneToOne_ManyToOne"
        //             || convertType == "ExplicitOneToOne_SuperToSub"
        //             || convertType == "ExplicitOneToOne_SubUnionToUnion"
        //             || convertType == "ExplicitOneToOne_UnionToSubUnion"
        //            )
        //        {
        //            Relationship dbRelationship = null;
        //            if (convertType == "ExplicitOneToOne_ManyToOne"
        //             || convertType == "ExplicitOneToOne_SuperToSub"
        //             || convertType == "ExplicitOneToOne_SubUnionToUnion"
        //             || convertType == "ExplicitOneToOne_UnionToSubUnion")
        //            {
        //                if (convertType == "ExplicitOneToOne_ManyToOne")
        //                    convertType = "ImplicitOneToOne_OneToMany";
        //                else if (convertType == "ExplicitOneToOne_SuperToSub")
        //                    convertType = "ImplicitOneToOne_SuperToSub";
        //                else if (convertType == "ExplicitOneToOne_SubUnionToUnion")
        //                    convertType = "ImplicitOneToOne_UnionToSubUnion";
        //                else if (convertType == "ExplicitOneToOne_UnionToSubUnion")
        //                    convertType = "ImplicitOneToOne_SubUnionToUnion";
        //                dbRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID).Relationship2;
        //            }
        //            else
        //                dbRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID);


        //            //////var entityInfo = DataHelper.GetEntityRelationshipsInfo(dbRelationship.TableDrivedEntity, dbRelationship);
        //            //////if (entityInfo.RelationInfos.Any(x => x.RelationType == RelationType.ManyDataItems && x.FKHasData))
        //            //////{
        //            //////    MessageBox.Show("بعلت وجود ارتباط یک به چند بین داده های دو جدول امکان تبدیل وجود ندارد");
        //            //////    return;
        //            //////}
        //            //////var relationInfo = entityInfo.RelationInfos.First(x => x.RelationType == RelationType.ManyDataItems);
        //            var dbReverseRelationship = projectContext.Relationship.First(x => x.RelationshipID == dbRelationship.ID);
        //            if (convertType == "ImplicitOneToOne_OneToMany")
        //            {
        //                var entityInfo = DataHelper.GetEntityRelationshipsInfo(dbRelationship.TableDrivedEntity, dbRelationship);
        //                var relationInfo = entityInfo.RelationInfos.FirstOrDefault(x => x.RelationType == RelationType.OneDataItems);
        //                if (relationInfo == null)
        //                    relationInfo = entityInfo.RelationInfos.First(x => x.RelationType == RelationType.ManyDataItems && !x.FKHasData);

        //                projectContext.ImplicitOneToOneRelationshipType.Remove(dbRelationship.RelationshipType.ImplicitOneToOneRelationshipType);
        //                projectContext.ExplicitOneToOneRelationshipType.Remove(dbReverseRelationship.RelationshipType.ExplicitOneToOneRelationshipType);


        //                dbRelationship.RelationshipType.OneToManyRelationshipType = new OneToManyRelationshipType();
        //                dbRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData;

        //                dbReverseRelationship.RelationshipType.ManyToOneRelationshipType = new ManyToOneRelationshipType();
        //                dbReverseRelationship.RelationshipType.IsOtherSideCreatable = true;
        //                dbReverseRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.FKColumnIsMandatory;
        //                projectContext.SaveChanges();

        //            }
        //            else if (convertType == "ImplicitOneToOne_SuperToSub")
        //            {
        //                var existingISA = projectContext.ISARelationship.Where(x => x.SuperToSubRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity.ID));
        //                ISARelationshipCreateOrSelect(existingISA.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
        //            }//union standard
        //            else if (convertType == "ImplicitOneToOne_SubUnionToUnion")
        //            {
        //                var existingUnionRelarionship = projectContext.UnionRelationshipType.Where(x => x.UnionToSubUnionRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity1.ID));
        //                UnionRelationshipCreateOrSelect(existingUnionRelarionship.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
        //            }
        //            else if (convertType == "ImplicitOneToOne_UnionToSubUnion")
        //            {
        //                var existingUnionRelarionship = projectContext.UnionRelationshipType.Where(x => x.UnionToSubUnionRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity.ID));
        //                UnionRelationshipCreateOrSelect(existingUnionRelarionship.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
        //            }
        //        }
        //        else if (convertType == "SuperToSub_OneToMany"
        //              || convertType == "SuperToSub_ImplicitOneToOne"
        //              || convertType == "SuperToSub_SubUnionToUnion"
        //              || convertType == "SuperToSub_UnionToSubUnion"
        //              || convertType == "SubToSuper_ManyToOne"
        //              || convertType == "SubToSuper_ExplicitOneToOne"
        //              || convertType == "SubToSuper_SubUnionToUnion"
        //              || convertType == "SubToSuper_UnionToSubUnion"
        //              || convertType == "SuperToSub_SuperToSub"
        //              || convertType == "SubToSuper_SubToSuper"
        //           )
        //        {
        //            Relationship dbRelationship = null;
        //            if (convertType == "SubToSuper_ManyToOne"
        //             || convertType == "SubToSuper_ExplicitOneToOne"
        //             || convertType == "SubToSuper_SubUnionToUnion"
        //             || convertType == "SubToSuper_UnionToSubUnion"
        //             || convertType == "SubToSuper_SubToSuper")
        //            {
        //                if (convertType == "SubToSuper_ManyToOne")
        //                    convertType = "SuperToSub_OneToMany";
        //                else if (convertType == "SubToSuper_ExplicitOneToOne")
        //                    convertType = "SuperToSub_ImplicitOneToOne";
        //                else if (convertType == "SubToSuper_SubUnionToUnion")
        //                    convertType = "SuperToSub_UnionToSubUnion";
        //                else if (convertType == "SubToSuper_UnionToSubUnion")
        //                    convertType = "SuperToSub_SubUnionToUnion";
        //                else if (convertType == "SubToSuper_SubToSuper")
        //                    convertType = "SuperToSub_SuperToSub";
        //                dbRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID).Relationship2;
        //            }
        //            else
        //                dbRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID);

        //            var dbReverseRelationship = projectContext.Relationship.First(x => x.RelationshipID == dbRelationship.ID);

        //            if (convertType != "SuperToSub_SuperToSub")
        //            {
        //                var entityInfo = DataHelper.GetEntityRelationshipsInfo(dbRelationship.TableDrivedEntity, dbRelationship);
        //                if (entityInfo.RelationInfos.Any(x => x.RelationType == RelationType.OneDataItems && x.FKRelatesOnPrimaryKey))
        //                {
        //                    MessageBox.Show("بعلت وجود ارتباط بروی کلیدهای اصلی تنها رابطه ارث بری معنی دارد و تبدیل میسر نمیباشد");
        //                    return;
        //                }
        //                var relationInfo = entityInfo.RelationInfos.FirstOrDefault(x => x.RelationType == RelationType.OneDataItems);
        //                if (relationInfo == null)
        //                    relationInfo = entityInfo.RelationInfos.First(x => x.RelationType == RelationType.ManyDataItems && !x.FKHasData);



        //                if (convertType == "SuperToSub_OneToMany")
        //                {


        //                    projectContext.SuperToSubRelationshipType.Remove(dbRelationship.RelationshipType.SuperToSubRelationshipType);
        //                    projectContext.SubToSuperRelationshipType.Remove(dbReverseRelationship.RelationshipType.SubToSuperRelationshipType);

        //                    dbRelationship.RelationshipType.OneToManyRelationshipType = new OneToManyRelationshipType();
        //                    dbRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData;

        //                    dbReverseRelationship.RelationshipType.ManyToOneRelationshipType = new ManyToOneRelationshipType();
        //                    dbReverseRelationship.RelationshipType.IsOtherSideCreatable = true;
        //                    dbReverseRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.FKColumnIsMandatory;

        //                    projectContext.SaveChanges();

        //                }
        //                else if (convertType == "SuperToSub_ImplicitOneToOne")
        //                {
        //                    projectContext.SuperToSubRelationshipType.Remove(dbRelationship.RelationshipType.SuperToSubRelationshipType);
        //                    projectContext.SubToSuperRelationshipType.Remove(dbReverseRelationship.RelationshipType.SubToSuperRelationshipType);

        //                    dbRelationship.RelationshipType.ImplicitOneToOneRelationshipType = new ImplicitOneToOneRelationshipType();
        //                    dbRelationship.RelationshipType.IsOtherSideCreatable = true;
        //                    dbRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData;

        //                    dbReverseRelationship.RelationshipType.ExplicitOneToOneRelationshipType = new ExplicitOneToOneRelationshipType();
        //                    dbReverseRelationship.RelationshipType.IsOtherSideCreatable = true;
        //                    dbReverseRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.FKColumnIsMandatory;

        //                    projectContext.SaveChanges();
        //                }//union standard
        //                else if (convertType == "SuperToSub_SubUnionToUnion")
        //                {
        //                    var existingUnionRelarionship = projectContext.UnionRelationshipType.Where(x => x.UnionToSubUnionRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity1.ID));
        //                    UnionRelationshipCreateOrSelect(existingUnionRelarionship.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
        //                }
        //                else if (convertType == "SuperToSub_UnionToSubUnion")
        //                {
        //                    var existingUnionRelarionship = projectContext.UnionRelationshipType.Where(x => x.UnionToSubUnionRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity.ID));
        //                    UnionRelationshipCreateOrSelect(existingUnionRelarionship.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
        //                }
        //            }
        //            else
        //            {
        //                var existingISA = projectContext.ISARelationship.Where(x => x.ID != dbRelationship.RelationshipType.SuperToSubRelationshipType.ISARelationshipID && x.SuperToSubRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity.ID));
        //                ISARelationshipCreateOrSelect(existingISA.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
        //            }//un
        //        }


        //        else if (convertType == "UnionToSubUnion_!UnionHoldsKeys_OneToMany"
        //              || convertType == "UnionToSubUnion_!UnionHoldsKeys_ImplicitOneToOne"
        //              || convertType == "UnionToSubUnion_!UnionHoldsKeys_SuperToSub"
        //              || convertType == "UnionToSubUnion_!UnionHoldsKeys_SubUnionToUnion"
        //              || convertType == "UnionToSubUnion_!UnionHoldsKeys_UnionToSubUnion"
        //              || convertType == "SubUnionToUnion_!UnionHoldsKeys_ManyToOne"
        //              || convertType == "SubUnionToUnion_!UnionHoldsKeys_ExplicitOneToOne"
        //              || convertType == "SubUnionToUnion_!UnionHoldsKeys_SubToSuper"
        //              || convertType == "SubUnionToUnion_!UnionHoldsKeys_UnionToSubUnion"
        //              || convertType == "SubUnionToUnion_!UnionHoldsKeys_SubUnionToUnion"
        //           )
        //        {


        //            Relationship dbRelationship = null;
        //            if (convertType == "SubUnionToUnion_!UnionHoldsKeys_ManyToOne"
        //            || convertType == "SubUnionToUnion_!UnionHoldsKeys_ExplicitOneToOne"
        //            || convertType == "SubUnionToUnion_!UnionHoldsKeys_SubToSuper"
        //            || convertType == "SubUnionToUnion_!UnionHoldsKeys_UnionToSubUnion"
        //                || convertType == "SubUnionToUnion_!UnionHoldsKeys_SubUnionToUnion"
        //             )
        //            {
        //                if (convertType == "SubUnionToUnion_!UnionHoldsKeys_ManyToOne")
        //                    convertType = "UnionToSubUnion_!UnionHoldsKeys_OneToMany";
        //                else if (convertType == "SubUnionToUnion_!UnionHoldsKeys_ExplicitOneToOne")
        //                    convertType = "UnionToSubUnion_!UnionHoldsKeys_ImplicitOneToOne";
        //                else if (convertType == "SubUnionToUnion_!UnionHoldsKeys_SubToSuper")
        //                    convertType = "UnionToSubUnion_!UnionHoldsKeys_SuperToSub";
        //                else if (convertType == "SubUnionToUnion_!UnionHoldsKeys_UnionToSubUnion")
        //                    convertType = "UnionToSubUnion_!UnionHoldsKeys_SubUnionToUnion";
        //                else if (convertType == "SubUnionToUnion_!UnionHoldsKeys_SubUnionToUnion")
        //                    convertType = "UnionToSubUnion_!UnionHoldsKeys_UnionToSubUnion";
        //                dbRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID).Relationship2;
        //            }
        //            else
        //                dbRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID);



        //            var dbReverseRelationship = projectContext.Relationship.First(x => x.RelationshipID == dbRelationship.ID);


        //            if (convertType != "UnionToSubUnion_!UnionHoldsKeys_UnionToSubUnion")
        //            {
        //                var entityInfo = DataHelper.GetEntityRelationshipsInfo(dbRelationship.TableDrivedEntity, dbRelationship);
        //                //if (entityInfo.RelationInfos.Any(x => x.RelationType == RelationType.OneDataItems && x.FKRelatesOnPrimaryKey))
        //                //{
        //                //    MessageBox.Show("بعلت وجود ارتباط بروی کلیدهای اصلی تنها رابطه ارث بری معنی دارد و تبدیل میسر نمیباشد");
        //                //    return;
        //                //}
        //                var relationInfo = entityInfo.RelationInfos.FirstOrDefault(x => x.RelationType == RelationType.OneDataItems);
        //                if (relationInfo == null)
        //                    relationInfo = entityInfo.RelationInfos.First(x => x.RelationType == RelationType.ManyDataItems && !x.FKHasData);
        //                if (convertType == "UnionToSubUnion_!UnionHoldsKeys_OneToMany")
        //                {


        //                    if (dbRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
        //                        projectContext.UnionToSubUnionRelationshipType.Remove(dbRelationship.RelationshipType.UnionToSubUnionRelationshipType);
        //                    //if (dbRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
        //                    //    projectContext.SubUnionToUnionRelationshipType.Remove(dbRelationship.RelationshipType.SubUnionToUnionRelationshipType);
        //                    if (dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
        //                        projectContext.SubUnionToUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType);
        //                    //if (dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
        //                    //    projectContext.UnionToSubUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType);

        //                    dbRelationship.RelationshipType.OneToManyRelationshipType = new OneToManyRelationshipType();
        //                    dbRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData;

        //                    dbReverseRelationship.RelationshipType.ManyToOneRelationshipType = new ManyToOneRelationshipType();
        //                    dbReverseRelationship.RelationshipType.IsOtherSideCreatable = true;
        //                    dbReverseRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.FKColumnIsMandatory;

        //                    projectContext.SaveChanges();

        //                }
        //                else if (convertType == "UnionToSubUnion_!UnionHoldsKeys_ImplicitOneToOne")
        //                {
        //                    if (dbRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
        //                        projectContext.UnionToSubUnionRelationshipType.Remove(dbRelationship.RelationshipType.UnionToSubUnionRelationshipType);
        //                    //if (dbRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
        //                    //    projectContext.SubUnionToUnionRelationshipType.Remove(dbRelationship.RelationshipType.SubUnionToUnionRelationshipType);
        //                    if (dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
        //                        projectContext.SubUnionToUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType);
        //                    //if (dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
        //                    //    projectContext.UnionToSubUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType);

        //                    dbRelationship.RelationshipType.ImplicitOneToOneRelationshipType = new ImplicitOneToOneRelationshipType();
        //                    dbRelationship.RelationshipType.IsOtherSideCreatable = true;
        //                    dbRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData;

        //                    dbReverseRelationship.RelationshipType.ExplicitOneToOneRelationshipType = new ExplicitOneToOneRelationshipType();
        //                    dbReverseRelationship.RelationshipType.IsOtherSideCreatable = true;
        //                    dbReverseRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.FKColumnIsMandatory;

        //                    projectContext.SaveChanges();
        //                }//union standard
        //                else if (convertType == "UnionToSubUnion_!UnionHoldsKeys_SuperToSub")
        //                {
        //                    var existingISA = projectContext.ISARelationship.Where(x => x.SuperToSubRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity.ID));
        //                    ISARelationshipCreateOrSelect(existingISA.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
        //                }
        //                else if (convertType == "UnionToSubUnion_!UnionHoldsKeys_SubUnionToUnion")
        //                {
        //                    var existingUnionRelarionship = projectContext.UnionRelationshipType.Where(x => x.UnionToSubUnionRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity.ID));
        //                    UnionRelationshipCreateOrSelect(existingUnionRelarionship.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
        //                }
        //            }
        //            else
        //            {
        //                var existingUnionRelarionship = projectContext.UnionRelationshipType.Where(x => x.UnionHoldsKeys == false && x.ID != dbRelationship.RelationshipType.UnionToSubUnionRelationshipType.UnionRelationshipTypeID && x.UnionToSubUnionRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity.ID));
        //                UnionRelationshipCreateOrSelect(existingUnionRelarionship.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);

        //            }

        //        }
        //        else if (convertType == "UnionToSubUnion_UnionHoldsKeys_ManyToOne"
        //                 || convertType == "UnionToSubUnion_UnionHoldsKeys_ExplicitOneToOne"
        //                 || convertType == "UnionToSubUnion_UnionHoldsKeys_SubToSuper"
        //                 || convertType == "UnionToSubUnion_UnionHoldsKeys_SubUnionToUnion"
        //            || convertType == "UnionToSubUnion_UnionHoldsKeys_UnionToSubUnion"
        //                 || convertType == "SubUnionToUnion_UnionHoldsKeys_OneToMany"
        //                 || convertType == "SubUnionToUnion_UnionHoldsKeys_ImplicitOneToOne"
        //                 || convertType == "SubUnionToUnion_UnionHoldsKeys_SuperToSub"
        //                 || convertType == "SubUnionToUnion_UnionHoldsKeys_UnionToSubUnion"
        //             || convertType == "SubUnionToUnion_UnionHoldsKeys_SubUnionToUnion"
        //          )
        //        {
        //            Relationship dbRelationship = null;
        //            if (convertType == "UnionToSubUnion_UnionHoldsKeys_ManyToOne"
        //            || convertType == "UnionToSubUnion_UnionHoldsKeys_ExplicitOneToOne"
        //            || convertType == "UnionToSubUnion_UnionHoldsKeys_SubToSuper"
        //            || convertType == "UnionToSubUnion_UnionHoldsKeys_SubUnionToUnion"
        //               || convertType == "UnionToSubUnion_UnionHoldsKeys_UnionToSubUnion"
        //                )
        //            {
        //                if (convertType == "UnionToSubUnion_UnionHoldsKeys_ManyToOne")
        //                    convertType = "SubUnionToUnion_UnionHoldsKeys_OneToMany";
        //                else if (convertType == "UnionToSubUnion_UnionHoldsKeys_ExplicitOneToOne")
        //                    convertType = "SubUnionToUnion_UnionHoldsKeys_ImplicitOneToOne";
        //                else if (convertType == "UnionToSubUnion_UnionHoldsKeys_SubToSuper")
        //                    convertType = "SubUnionToUnion_UnionHoldsKeys_SuperToSub";
        //                else if (convertType == "UnionToSubUnion_UnionHoldsKeys_SubUnionToUnion")
        //                    convertType = "SubUnionToUnion_UnionHoldsKeys_UnionToSubUnion";
        //                else if (convertType == "UnionToSubUnion_UnionHoldsKeys_UnionToSubUnion")
        //                    convertType = "SubUnionToUnion_UnionHoldsKeys_SubUnionToUnion";
        //                dbRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID).Relationship2;
        //            }
        //            else
        //                dbRelationship = projectContext.Relationship.First(x => x.ID == relationship.ID);

        //            var dbReverseRelationship = projectContext.Relationship.First(x => x.RelationshipID == dbRelationship.ID);
        //            if (convertType != "SubUnionToUnion_UnionHoldsKeys_SubUnionToUnion")
        //            {
        //                var entityInfo = DataHelper.GetEntityRelationshipsInfo(dbRelationship.TableDrivedEntity, dbRelationship);
        //                //if (entityInfo.RelationInfos.Any(x => x.RelationType == RelationType.OneDataItems && x.FKRelatesOnPrimaryKey))
        //                //{
        //                //    MessageBox.Show("بعلت وجود ارتباط بروی کلیدهای اصلی تنها رابطه ارث بری معنی دارد و تبدیل میسر نمیباشد");
        //                //    return;
        //                //}
        //                var relationInfo = entityInfo.RelationInfos.FirstOrDefault(x => x.RelationType == RelationType.OneDataItems);
        //                if (relationInfo == null)
        //                    relationInfo = entityInfo.RelationInfos.First(x => x.RelationType == RelationType.ManyDataItems && !x.FKHasData);




        //                if (convertType == "SubUnionToUnion_UnionHoldsKeys_OneToMany")
        //                {


        //                    //if (dbRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
        //                    //    projectContext.UnionToSubUnionRelationshipType.Remove(dbRelationship.RelationshipType.UnionToSubUnionRelationshipType);
        //                    if (dbRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
        //                        projectContext.SubUnionToUnionRelationshipType.Remove(dbRelationship.RelationshipType.SubUnionToUnionRelationshipType);
        //                    //if (dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
        //                    //    projectContext.SubUnionToUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType);
        //                    if (dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
        //                        projectContext.UnionToSubUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType);

        //                    dbRelationship.RelationshipType.OneToManyRelationshipType = new OneToManyRelationshipType();
        //                    dbRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData;

        //                    dbReverseRelationship.RelationshipType.ManyToOneRelationshipType = new ManyToOneRelationshipType();
        //                    dbReverseRelationship.RelationshipType.IsOtherSideCreatable = true;
        //                    dbReverseRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.FKColumnIsMandatory;

        //                    projectContext.SaveChanges();

        //                }
        //                else if (convertType == "SubUnionToUnion_UnionHoldsKeys_ImplicitOneToOne")
        //                {
        //                    //if (dbRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
        //                    //    projectContext.UnionToSubUnionRelationshipType.Remove(dbRelationship.RelationshipType.UnionToSubUnionRelationshipType);
        //                    if (dbRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
        //                        projectContext.SubUnionToUnionRelationshipType.Remove(dbRelationship.RelationshipType.SubUnionToUnionRelationshipType);
        //                    //if (dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType != null)
        //                    //    projectContext.SubUnionToUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.SubUnionToUnionRelationshipType);
        //                    if (dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType != null)
        //                        projectContext.UnionToSubUnionRelationshipType.Remove(dbReverseRelationship.RelationshipType.UnionToSubUnionRelationshipType);

        //                    dbRelationship.RelationshipType.ImplicitOneToOneRelationshipType = new ImplicitOneToOneRelationshipType();
        //                    dbRelationship.RelationshipType.IsOtherSideCreatable = true;
        //                    dbRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData;

        //                    dbReverseRelationship.RelationshipType.ExplicitOneToOneRelationshipType = new ExplicitOneToOneRelationshipType();
        //                    dbReverseRelationship.RelationshipType.IsOtherSideCreatable = true;
        //                    dbReverseRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.FKColumnIsMandatory;

        //                    projectContext.SaveChanges();
        //                }//union standard
        //                else if (convertType == "SubUnionToUnion_UnionHoldsKeys_SuperToSub")
        //                {
        //                    var existingISA = projectContext.ISARelationship.Where(x => x.SuperToSubRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity.ID));
        //                    ISARelationshipCreateOrSelect(existingISA.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
        //                }
        //                else if (convertType == "SubUnionToUnion_UnionHoldsKeys_UnionToSubUnion")
        //                {
        //                    var existingUnionRelarionship = projectContext.UnionRelationshipType.Where(x => x.SubUnionToUnionRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity.ID));
        //                    UnionRelationshipCreateOrSelect(existingUnionRelarionship.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
        //                }
        //            }
        //            else
        //            {
        //                var existingUnionRelarionship = projectContext.UnionRelationshipType.Where(x => x.UnionHoldsKeys == true && x.ID != dbRelationship.RelationshipType.SubUnionToUnionRelationshipType.UnionRelationshipTypeID && x.SubUnionToUnionRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == dbRelationship.TableDrivedEntity.ID));
        //                UnionRelationshipCreateOrSelect(existingUnionRelarionship.ToList(), convertType, dbRelationship.ID, dbReverseRelationship.ID);
        //            }
        //        }

        //    }
        //}


        //public void ClearRelationshipType(RelationshipType relationshipType)
        //{
        //    if (relationshipType.ManyToOneRelationshipType != null)
        //        relationshipType.ManyToOneRelationshipType = null;

        //    if (relationshipType.OneToManyRelationshipType != null)
        //        relationshipType.OneToManyRelationshipType = null;

        //    if (relationshipType.ExplicitOneToOneRelationshipType != null)
        //        relationshipType.ExplicitOneToOneRelationshipType = null;

        //    if (relationshipType.ImplicitOneToOneRelationshipType != null)
        //        relationshipType.ImplicitOneToOneRelationshipType = null;

        //    if (relationshipType.SuperToSubRelationshipType != null)
        //        relationshipType.SuperToSubRelationshipType = null;

        //    if (relationshipType.SubToSuperRelationshipType != null)
        //        relationshipType.SuperToSubRelationshipType = null;

        //    if (relationshipType.UnionToSubUnionRelationshipType != null)
        //        relationshipType.UnionToSubUnionRelationshipType = null;

        //    if (relationshipType.SubUnionToUnionRelationshipType != null)
        //        relationshipType.SubUnionToUnionRelationshipType = null;
        //}

        public string GetRelationshipTypeTitle(Enum_RelationshipType item)
        {
            if (item == Enum_RelationshipType.OneToMany)
                return "یک به چند";
            else if (item == Enum_RelationshipType.ManyToOne)
                return "چند به یک";
            else if (item == Enum_RelationshipType.ImplicitOneToOne)
                return "یک به یک ضمنی";
            else if (item == Enum_RelationshipType.ExplicitOneToOne)
                return "یک به یک صریح";
            else if (item == Enum_RelationshipType.SubToSuper)
                return "زیرکلاس به ابرکلاس";
            else if (item == Enum_RelationshipType.SuperToSub)
                return "ابرکلاس به زیرکلاس";
            //else if (item == Enum_RelationshipType.SubUnionToUnion_SubUnionHoldsKeys)
            //    return "زیراجتماع به ابراجتماع_کلید در زیراجتماع";
            else if (item == Enum_RelationshipType.SubUnionToUnion)
                return "زیراجتماع به ابراجتماع_کلید در ابراجتماع";
            //else if (item == Enum_RelationshipType.UnionToSubUnion_SubUnionHoldsKeys)
            //    return "ابراجتماع به زیرجتماع_کلید در زیراجتماع";
            else if (item == Enum_RelationshipType.UnionToSubUnion)
                return "ابراجتماع به زیرجتماع_کلید در ابراجتماع";
            return "نامشخص";
        }
    }

    public class DeterminedRelationshiop
    {
        public Enum_RelationshipType Type { set; get; }
        public bool IsOtherSideMandatory { set; get; }
        public bool IsReverseSideMandatory { set; get; }
    }


}
