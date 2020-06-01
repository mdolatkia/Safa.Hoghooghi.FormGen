
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paper_MetadataManagement
{
    public class BizRelationship
    {

        public bool ReverseRelationshipIsMandatory(int relationshipID)
        {
            using (var context = new MyIdeaEntities())
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
            using (var projectContext = new MyIdeaEntities())
            {
                var relationship = projectContext.Relationship.First(x => x.ID == sourceRelationshipID);
                foreach (var relcolumn in relationship.RelationshipColumns)
                {
                    result.Add(new Tuple<ColumnDTO, ColumnDTO>(bizColumn.ToColumnDTO(relcolumn.Column, true), bizColumn.ToColumnDTO(relcolumn.Column1, true)));
                }
            }
            return result;
        }

        //public int GetOtherSideTableID(int relationshipID)
        //{
        //    RelationshipDTO result = new RelationshipDTO();
        //    using (var projectContext = new MyIdeaEntities())
        //    {
        //        var relationship = projectContext.Relationship.First(x => x.ID == relationshipID);
        //        return relationship.TableDrivedEntity1.TableID;
        //    }
        //}
        public int GetOtherSideEntityID(int relationshipID)
        {
            RelationshipDTO result = new RelationshipDTO();
            using (var projectContext = new MyIdeaEntities())
            {
                var relationship = projectContext.Relationship.First(x => x.ID == relationshipID);
                return relationship.TableDrivedEntityID2;
            }
        }
        public RelationshipDTO GetRelationship(int relationshipID)
        {
            RelationshipDTO result = new RelationshipDTO();
            using (var projectContext = new MyIdeaEntities())
            {
                var relationship = projectContext.Relationship.First(x => x.ID == relationshipID);
                return ToRelationshipDTO(relationship);
            }
        }
        public RelationshipDTO GetReverseRelationship(int relationshipID)
        {
            RelationshipDTO result = new RelationshipDTO();
            using (var projectContext = new MyIdeaEntities())
            {
                var dbRelationship = projectContext.Relationship.First(x => x.ID == relationshipID);
                if (dbRelationship.Relationship2 != null)
                    return ToRelationshipDTO(dbRelationship.Relationship2);
                else
                    return ToRelationshipDTO(projectContext.Relationship.First(x => x.RelationshipID == relationshipID));
            }
        }

        public void CreateUpdateRelationship(RelationshipDTO message, List<RelationshipColumnDTO> relationshipColumns)
        {
            //int createdID = 0;
            bool newItem = message.ID == 0;
            using (var projectContext = new MyIdeaEntities())
            {
                Relationship dbRelationship = null;
                if (message.ID == 0)
                {
                    dbRelationship = new Relationship();
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
                foreach (var col in relationshipColumns)
                {
                    RelationshipColumns rColumn = new RelationshipColumns();
                    if (col.FirstSideColumnID != null && col.FirstSideColumnID != 0)
                        rColumn.FirstSideColumnID = col.FirstSideColumnID;
                    else
                    {
                        rColumn.FirstSideColumnID = null;
                        if (!string.IsNullOrEmpty(col.PrimarySideFixedValue))
                        {
                            rColumn.PrimarySideFixedValue = col.PrimarySideFixedValue;
                        }
                    }
                    rColumn.SecondSideColumnID = col.SecondSideColumnID;
                    dbRelationship.RelationshipColumns.Add(rColumn);
                }


                Relationship dbReverseRelationship = null;
                if (message.ID == 0)
                {
                    dbReverseRelationship = new Relationship();
                    projectContext.Relationship.Add(dbReverseRelationship);
                    dbReverseRelationship.Relationship2 = dbRelationship;
                }
                else
                    dbReverseRelationship = dbRelationship.Relationship1.First();


                dbReverseRelationship.Name = "inverse_" + message.Name;
                dbReverseRelationship.Created = true;
                dbReverseRelationship.TableDrivedEntityID1 = message.EntityID2;
                dbReverseRelationship.TableDrivedEntityID2 = message.EntityID1;
                while (dbReverseRelationship.RelationshipColumns.Any())
                    projectContext.RelationshipColumns.Remove(dbReverseRelationship.RelationshipColumns.First());
                foreach (var col in relationshipColumns)
                {
                    RelationshipColumns rColumn = new RelationshipColumns();
                    rColumn.FirstSideColumnID = col.SecondSideColumnID;
                    if (col.FirstSideColumnID != null && col.FirstSideColumnID != 0)
                        rColumn.SecondSideColumnID = col.FirstSideColumnID;
                    else
                    {
                        rColumn.SecondSideColumnID = null;
                        if (!string.IsNullOrEmpty(col.PrimarySideFixedValue))
                        {
                            rColumn.PrimarySideFixedValue = col.PrimarySideFixedValue;
                        }
                    }
                    dbReverseRelationship.RelationshipColumns.Add(rColumn);
                }
                projectContext.SaveChanges();
                message.ID = dbRelationship.ID;

            }
            if (newItem)
            {

                //var dbRelationship = projectContext.Relationship.First(x => x.ID == createdID);
                //var dbReverseRelationship = dbRelationship.Relationship1.First();
                ImposeSingleRelationshipType(message.ID);
                //projectContext.SaveChanges();

            }
        }


        public void ImposeSingleRelationshipType(int relationshipID)
        {
            //BizRelationship bizRelationship = new BizRelationship();
            //var relationshipdto = bizRelationship.GetRelationship(relationshipID);
            //if (relationshipdto.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary)
            //    relationshipdto = bizRelationship.GetRelationship(relationshipdto.PairRelationshipID);
            //ModelDataHelper dataHelper = new ModelDataHelper();
            //var relationInfo = dataHelper.GetRelationshipsInfo(relationshipdto);

            //ImposeSingleRelationshipType(relationshipdto, relationInfo);
        }
        //////private void ImposeSingleRelationshipType(RelationshipDTO relationshipdto, RelationInfo relationInfo)
        //////{
        //////    using (var projectContext = new MyIdeaEntities())
        //////    {
        //////        var relationship = projectContext.Relationship.First(x => x.ID == relationshipdto.ID);
        //////        if (relationship.RelationshipType == null)
        //////        {
        //////            var reverseRelationship = projectContext.Relationship.First(x => x.RelationshipID == relationship.ID);
        //////            if (relationInfo.FKRelatesOnPrimaryKey)
        //////            {
        //////                relationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.SuperToSub);
        //////                relationship.RelationshipType = new RelationshipType();
        //////                relationship.RelationshipType.SuperToSubRelationshipType = new SuperToSubRelationshipType();
        //////                relationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData;

        //////                reverseRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.SubToSuper);
        //////                reverseRelationship.RelationshipType = new RelationshipType();
        //////                reverseRelationship.RelationshipType.SubToSuperRelationshipType = new SubToSuperRelationshipType();
        //////                reverseRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllFKSidesHavePKSide;

        //////                var isaRelationship = new ISARelationship();
        //////                isaRelationship.Name = relationshipdto.Entity1 + ">" + relationshipdto.Entity2;
        //////                isaRelationship.IsDisjoint = false;
        //////                isaRelationship.IsTolatParticipation = relationInfo.AllPrimarySideHasFkSideData;

        //////                relationship.RelationshipType.SuperToSubRelationshipType.ISARelationship = isaRelationship;
        //////                reverseRelationship.RelationshipType.SubToSuperRelationshipType.ISARelationship = isaRelationship;

        //////            }
        //////            else if (relationInfo.RelationType == RelationType.OnePKtoManyFK)
        //////            {
        //////                relationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.OneToMany);
        //////                relationship.RelationshipType = new RelationshipType();
        //////                relationship.RelationshipType.OneToManyRelationshipType = new OneToManyRelationshipType();
        //////                relationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData;

        //////                reverseRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.ManyToOne);
        //////                reverseRelationship.RelationshipType = new RelationshipType();
        //////                reverseRelationship.RelationshipType.ManyToOneRelationshipType = new ManyToOneRelationshipType();
        //////                reverseRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllFKSidesHavePKSide;
        //////            }
        //////            else if (relationInfo.RelationType == RelationType.OnePKtoOneFK)
        //////            {
        //////                relationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.ImplicitOneToOne);
        //////                relationship.RelationshipType = new RelationshipType();
        //////                relationship.RelationshipType.ImplicitOneToOneRelationshipType = new ImplicitOneToOneRelationshipType();
        //////                relationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllPrimarySideHasFkSideData;

        //////                reverseRelationship.TypeEnum = Convert.ToByte(Enum_RelationshipType.ExplicitOneToOne);
        //////                reverseRelationship.RelationshipType = new RelationshipType();
        //////                reverseRelationship.RelationshipType.ExplicitOneToOneRelationshipType = new ExplicitOneToOneRelationshipType();
        //////                reverseRelationship.RelationshipType.IsOtherSideMandatory = relationInfo.AllFKSidesHavePKSide;
        //////            }

        //////            projectContext.SaveChanges();
        //////        }
        //////    }

        //////}

        //public void SetISA_RelationshipProperties(Tuple<ISARelationshipDTO, TableDrivedEntityDTO, List<TableDrivedEntityDTO>> iSARelationship)
        //{//هربار کانکشتن استرینگ خونده نشود


        //        //}
        //        //catch (Exception ex)
        //        //{
        //        //    throw ex;
        //        //}

        //}
      
        public List<RelationshipDTO> GetRelationshipsByTableID(int tableID)
        {
            List<RelationshipDTO> result = new List<RelationshipDTO>();
            using (var projectContext = new MyIdeaEntities())
            {
                var list = projectContext.Relationship.Where(x => x.TableDrivedEntity.TableID == tableID || x.TableDrivedEntity1.TableID == tableID);
                foreach (var item in list)
                {
                    result.Add(ToRelationshipDTO(item));
                }
            }
            return result;
        }
        public List<RelationshipDTO> GetRelationshipsByTableIDasSource(int tableID)
        {
            List<RelationshipDTO> result = new List<RelationshipDTO>();
            using (var projectContext = new MyIdeaEntities())
            {
                var list = projectContext.Relationship.Where(x => x.TableDrivedEntity.TableID == tableID);
                foreach (var item in list)
                {
                    result.Add(ToRelationshipDTO(item));
                }
            }
            return result;
        }
        public List<RelationshipDTO> GetRelationships(int databaseID)
        {
            List<RelationshipDTO> result = new List<RelationshipDTO>();
            using (var projectContext = new MyIdeaEntities())
            {
                //string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
                var list = projectContext.Relationship.Where(x => x.TableDrivedEntity.Table.DBSchema.DatabaseInformationID == databaseID);
                foreach (var item in list)
                {
                    result.Add(ToRelationshipDTO(item));
                }
            }
            return result;
        }
        //هر دو طرف تکرار میشوند
        public List<RelationshipDTO> GetRelationshipsByEntityIDBothSides(int tableDrivedEntityID)
        {
            List<RelationshipDTO> result = new List<RelationshipDTO>();
            using (var projectContext = new MyIdeaEntities())
            {
                var list = projectContext.Relationship.Where(x => x.TableDrivedEntityID1 == tableDrivedEntityID || x.TableDrivedEntityID2 == tableDrivedEntityID);
                foreach (var item in list)
                {
                    result.Add(ToRelationshipDTO(item));
                }
            }
            return result;
        }
        public List<RelationshipDTO> GetRelationshipsByEntityID(int tableDrivedEntityID)
        {
            List<RelationshipDTO> result = new List<RelationshipDTO>();
            using (var projectContext = new MyIdeaEntities())
            {
                var list = projectContext.Relationship.Where(x => x.TableDrivedEntityID1 == tableDrivedEntityID);
                foreach (var item in list)
                {
                    result.Add(ToRelationshipDTO(item));
                }
            }
            return result;
        }

        public List<RelationshipDTO> GetRelationshipsBetweenEntities(int firstEntityID, int secondEnitityID)
        {
            List<RelationshipDTO> result = new List<RelationshipDTO>();
            using (var projectContext = new MyIdeaEntities())
            {
                var list = projectContext.Relationship.Where(x => x.TableDrivedEntityID1 == firstEntityID && x.TableDrivedEntityID2 == secondEnitityID);
                foreach (var item in list)
                {
                    result.Add(ToRelationshipDTO(item));
                }
            }
            return result;
        }

        //public List<RelationshipDTO> GetRelationshipsByEntityIDasSource(int tableDrivedEntityID)
        //{
        //    List<RelationshipDTO> result = new List<RelationshipDTO>();
        //    using (var projectContext = new MyIdeaEntities())
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
            using (var projectContext = new MyIdeaEntities())
            {
                foreach (var relationship in relationships)
                {
                    var dbRelationship = ToRelationship(relationship, projectContext);
                }
                projectContext.SaveChanges();
            }
        }
        private Relationship ToRelationship(RelationshipDTO item, MyIdeaEntities projectContext)
        {
            var dbRelationship = projectContext.Relationship.First(x => x.ID == item.ID);
            dbRelationship.Alias = item.Alias;
            dbRelationship.Name = item.Name;
            dbRelationship.DataEntryEnabled = item.DataEntryEnabled;
            dbRelationship.Enabled = item.Enabled;
            dbRelationship.SearchEnabled = item.SearchEnabled;
            //dbRelationship.ViewEnabled = item.ViewEnabled;
            if (dbRelationship.RelationshipType != null)
            {
                dbRelationship.RelationshipType.IsOtherSideCreatable = item.IsOtherSideCreatable;
                dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = item.IsOtherSideDirectlyCreatable;
                dbRelationship.RelationshipType.IsOtherSideMandatory = item.IsOtherSideMandatory;
                dbRelationship.RelationshipType.IsOtherSideTransferable = item.IsOtherSideTransferable;
            }

            return dbRelationship;
        }
        public RelationshipDTO ToRelationshipDTO(Relationship item)
        {
            //var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.Relationship, item.ID.ToString());
            //if (cachedItem != null)
            //    return (cachedItem as RelationshipDTO);

            RelationshipDTO result = new RelationshipDTO();
            result.Name = item.Name;
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
            result.ServerName1 = item.TableDrivedEntity.Table.DBSchema.DatabaseInformation.DBServer.Name;
            result.ServerName2 = item.TableDrivedEntity1.Table.DBSchema.DatabaseInformation.DBServer.Name;
            if (result.ServerName1 != result.ServerName2)
            {
             //   var linkedServer1 = item.TableDrivedEntity.Table.DBSchema.DatabaseInformation.DBServer.LinkedServer.FirstOrDefault(x => x.DBServer.Name == result.ServerName1
             //   && x.DBServer1.Name == result.ServerName2);
             //   if (linkedServer1 != null)
             //       result.LinkedServer1 = linkedServer1.Name;
             //   var linkedServer2 = item.TableDrivedEntity1.Table.DBSchema.DatabaseInformation.DBServer.LinkedServer.FirstOrDefault(x => x.DBServer.Name == result.ServerName2
             //&& x.DBServer1.Name == result.ServerName1);
             //   if (linkedServer2 != null)
             //       result.LinkedServer2 = linkedServer2.Name;

            }
            if (item.TypeEnum != null)
                result.TypeEnum = (Enum_RelationshipType)item.TypeEnum.Value;
            else
                result.TypeEnum = Enum_RelationshipType.None;
            result.MastertTypeEnum = GetMastertRelationshipType(item);

            foreach (var relcolumn in item.RelationshipColumns)
            {
                var rColumn = ToRelationshipColumn(result.MastertTypeEnum, relcolumn);
                result.RelationshipColumns.Add(rColumn);
            }


            //result.RelationshipColumns = "";
            //foreach (var relColumn in item.RelationshipColumns)
            //    result.RelationshipColumns += (result.RelationshipColumns == "" ? "" : ",") + "(PK)" + item.TableDrivedEntity.Name + "." + relColumn.Column.Name + ">(FK)" + item.TableDrivedEntity1.Name + "." + relColumn.Column1.Name;


            result.TypeStr = result.TypeEnum.ToString();
            result.DataEntryEnabled = item.DataEntryEnabled;
            result.SearchEnabled = item.SearchEnabled;
            result.Created = item.Created == true;
            //result.ViewEnabled = item.ViewEnabled;

            if (item != null && item.RelationshipType != null)
            {
                result.IsOtherSideMandatory = item.RelationshipType.IsOtherSideMandatory;
                result.IsOtherSideCreatable = item.RelationshipType.IsOtherSideCreatable;
                result.IsOtherSideDirectlyCreatable = item.RelationshipType.IsOtherSideDirectlyCreatable;
                result.IsOtherSideTransferable = item.RelationshipType.IsOtherSideTransferable;
            }
            //CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.Relationship, item.ID.ToString());
            return result;
        }

        public RelationshipColumnDTO ToRelationshipColumn(Enum_MasterRelationshipType masterType, RelationshipColumns relcolumn)
        {
            BizColumn bizColumn = new BizColumn();
            var rColumn = new RelationshipColumnDTO();
            if (relcolumn.FirstSideColumnID != null)
            {
                rColumn.FirstSideColumnID = relcolumn.FirstSideColumnID.Value;
                rColumn.FirstSideColumn = bizColumn.ToColumnDTO(relcolumn.Column, true);
            }
            if (relcolumn.SecondSideColumnID != null)
            {
                rColumn.SecondSideColumnID = relcolumn.SecondSideColumnID.Value;
                rColumn.SecondSideColumn = bizColumn.ToColumnDTO(relcolumn.Column1, true);
            }
            rColumn.PrimarySideFixedValue = relcolumn.PrimarySideFixedValue;
            return rColumn;
        }

        public Enum_MasterRelationshipType GetMastertRelationshipType(Relationship item)
        {
            if (item.RelationshipID == null)
                return Enum_MasterRelationshipType.FromPrimartyToForeign;
            else
                return Enum_MasterRelationshipType.FromForeignToPrimary;

        }


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

        public bool RelationshipIsMandatory(int relationshipID)
        {
            using (var projectContext = new MyIdeaEntities())
            {
                var dbRelationship = projectContext.Relationship.First(x => x.ID == relationshipID);
                return dbRelationship.RelationshipColumns.Any(y => (y.Column.PrimaryKey == null || y.Column.PrimaryKey == false) && y.Column.IsNull == false);
            }
        }


        //public event EventHandler<SimpleGenerationInfoArg> RuleImposedEvent;

        //public void ImposeRelationshipRule(int databaseID)
        //{
        //    //string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
        //    //MyIdeaEntities context = new MyIdeaEntities();
        //    //context.Configuration.LazyLoadingEnabled = true;
        //    //var list = context.TableDrivedEntity.Where(x => x.Table.DBSchema.DatabaseInformationID == databaseID);

        //    int index = 0;
        //    BizTableDrivedEntity bizEntity = new Paper_MetadataManagement.BizTableDrivedEntity();
        //    using (var projectContext = new MyIdeaEntities())
        //    {
        //        var listEntity = projectContext.TableDrivedEntity.Where(x => x.Table.DBSchema.DatabaseInformationID == databaseID);
        //        var count = listEntity.Count();
        //        foreach (var item in listEntity)
        //        {
        //            index++;
        //            var entity = bizEntity.GetTableDrivedEntity(item.ID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithRelationships);
        //            //ImposeRelationshipTypes(entity);
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

        public List<OneToManyRelationshipDTO> GetOneToManyRelationships(int databaseID)
        {
            List<OneToManyRelationshipDTO> result = new List<OneToManyRelationshipDTO>();
            using (var projectContext = new MyIdeaEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                //string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
                var list = projectContext.Relationship.Where(x => x.TableDrivedEntity.Table.DBSchema.DatabaseInformationID == databaseID
                    && x.RelationshipType != null && x.RelationshipType.OneToManyRelationshipType != null);
                foreach (var item in list)
                {

                    result.Add(ToOneToManyRelationship(item.RelationshipType.OneToManyRelationshipType));

                }
                return result;
            }
        }
        public OneToManyRelationshipDTO ToOneToManyRelationship(OneToManyRelationshipType oneToManyRelationshipType, RelationshipDTO baseRelationship = null)
        {
            BizRelationship biz = new Paper_MetadataManagement.BizRelationship();
            if (baseRelationship == null)
                baseRelationship = biz.ToRelationshipDTO(oneToManyRelationshipType.RelationshipType.Relationship);
            Mapper.Initialize(cfg => cfg.CreateMap<RelationshipDTO, OneToManyRelationshipDTO>());
            var result = AutoMapper.Mapper.Map<RelationshipDTO, OneToManyRelationshipDTO>(baseRelationship);


            result.MasterDetailState = oneToManyRelationshipType.MasterDetailState;
            result.DetailsCount = oneToManyRelationshipType.ManySideCount;


            return result;
        }
        public void UpdateOneToManyRelationships(List<OneToManyRelationshipDTO> relationships)
        {
            using (var projectContext = new MyIdeaEntities())
            {
                foreach (var relationship in relationships)
                {
                    var dbRelationship = projectContext.OneToManyRelationshipType.First(x => x.ID == relationship.ID);
                    var dbrel = ToRelationship(relationship, projectContext);
                    dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = relationship.IsOtherSideDirectlyCreatable;
                    dbRelationship.RelationshipType.IsOtherSideCreatable = relationship.IsOtherSideCreatable;
                    dbRelationship.RelationshipType.IsOtherSideMandatory = relationship.IsOtherSideMandatory;
                    dbRelationship.MasterDetailState = relationship.MasterDetailState;
                    dbRelationship.ManySideCount = relationship.DetailsCount;
                    dbRelationship.RelationshipType.Relationship.Name = relationship.Name;
                    dbRelationship.RelationshipType.Relationship.Alias = relationship.Alias;
                    dbRelationship.RelationshipType.Relationship.Enabled = relationship.Enabled;
                }
                projectContext.SaveChanges();
            }
        }



        public List<ManyToOneRelationshipDTO> GetManyToOneRelationships(int databaseID)
        {
            List<ManyToOneRelationshipDTO> result = new List<ManyToOneRelationshipDTO>();
            using (var projectContext = new MyIdeaEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                //string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
                var list = projectContext.Relationship.Where(x => x.TableDrivedEntity.Table.DBSchema.DatabaseInformationID == databaseID
                    && x.RelationshipType != null && x.RelationshipType.ManyToOneRelationshipType != null);
                foreach (var item in list)
                {

                    result.Add(ToManyToOneRelationshipDTO(item.RelationshipType.ManyToOneRelationshipType));

                }
                return result;
            }
        }
        public ManyToOneRelationshipDTO ToManyToOneRelationshipDTO(ManyToOneRelationshipType manyToOneRelationshipType, RelationshipDTO baseRelationship = null)
        {
            BizRelationship biz = new Paper_MetadataManagement.BizRelationship();
            if (baseRelationship == null)
                baseRelationship = biz.ToRelationshipDTO(manyToOneRelationshipType.RelationshipType.Relationship);
            Mapper.Initialize(cfg => cfg.CreateMap<RelationshipDTO, ManyToOneRelationshipDTO>());
            var result = AutoMapper.Mapper.Map<RelationshipDTO, ManyToOneRelationshipDTO>(baseRelationship);


            return result;
        }
        public void UpdateManyToOneRelationships(List<ManyToOneRelationshipDTO> relationships)
        {
            using (var projectContext = new MyIdeaEntities())
            {
                foreach (var relationship in relationships)
                {
                    var dbRelationship = projectContext.ManyToOneRelationshipType.First(x => x.ID == relationship.ID);
                    var dbrel = ToRelationship(relationship, projectContext);
                    dbRelationship.RelationshipType.IsOtherSideCreatable = relationship.IsOtherSideCreatable;
                    dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = relationship.IsOtherSideDirectlyCreatable;
                    dbRelationship.RelationshipType.IsOtherSideMandatory = relationship.IsOtherSideMandatory;
                    dbRelationship.RelationshipType.IsOtherSideTransferable = relationship.IsOtherSideTransferable;
                    dbRelationship.RelationshipType.Relationship.Name = relationship.Name;
                    dbRelationship.RelationshipType.Relationship.Alias = relationship.Alias;
                    dbRelationship.RelationshipType.Relationship.Enabled = relationship.Enabled;
                }
                projectContext.SaveChanges();
            }
        }



        public List<ExplicitOneToOneRelationshipDTO> GetExplicitOneToOneRelationships(int databaseID)
        {
            List<ExplicitOneToOneRelationshipDTO> result = new List<ExplicitOneToOneRelationshipDTO>();
            using (var projectContext = new MyIdeaEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                //string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
                var list = projectContext.Relationship.Where(x => x.TableDrivedEntity.Table.DBSchema.DatabaseInformationID == databaseID
                    && x.RelationshipType != null && x.RelationshipType.ExplicitOneToOneRelationshipType != null);
                foreach (var item in list)
                {

                    result.Add(ToExplicitOneToOneRelationshipDTO(item.RelationshipType.ExplicitOneToOneRelationshipType));

                }
                return result;
            }
        }
        public ExplicitOneToOneRelationshipDTO ToExplicitOneToOneRelationshipDTO(ExplicitOneToOneRelationshipType explicitOneToOneRelationshipType, RelationshipDTO baseRelationship = null)
        {
            BizRelationship biz = new Paper_MetadataManagement.BizRelationship();
            if (baseRelationship == null)
                baseRelationship = biz.ToRelationshipDTO(explicitOneToOneRelationshipType.RelationshipType.Relationship);
            Mapper.Initialize(cfg => cfg.CreateMap<RelationshipDTO, ExplicitOneToOneRelationshipDTO>());
            var result = AutoMapper.Mapper.Map<RelationshipDTO, ExplicitOneToOneRelationshipDTO>(baseRelationship);

            return result;
        }
        public void UpdateExplicitOneToOneRelationships(List<ExplicitOneToOneRelationshipDTO> relationships)
        {
            using (var projectContext = new MyIdeaEntities())
            {
                foreach (var relationship in relationships)
                {
                    var dbRelationship = projectContext.ExplicitOneToOneRelationshipType.First(x => x.ID == relationship.ID);
                    var dbrel = ToRelationship(relationship, projectContext);
                    dbRelationship.RelationshipType.IsOtherSideCreatable = relationship.IsOtherSideCreatable;
                    dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = relationship.IsOtherSideDirectlyCreatable;
                    dbRelationship.RelationshipType.IsOtherSideMandatory = relationship.IsOtherSideMandatory;
                    dbRelationship.RelationshipType.IsOtherSideTransferable = relationship.IsOtherSideTransferable;
                    dbRelationship.RelationshipType.Relationship.Name = relationship.Name;
                    dbRelationship.RelationshipType.Relationship.Alias = relationship.Alias;
                    dbRelationship.RelationshipType.Relationship.Enabled = relationship.Enabled;
                }
                projectContext.SaveChanges();
            }
        }


        public List<ImplicitOneToOneRelationshipDTO> GetImplicitOneToOneRelationships(int databaseID)
        {
            List<ImplicitOneToOneRelationshipDTO> result = new List<ImplicitOneToOneRelationshipDTO>();
            using (var projectContext = new MyIdeaEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                //string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
                var list = projectContext.Relationship.Where(x => x.TableDrivedEntity.Table.DBSchema.DatabaseInformationID == databaseID
                    && x.RelationshipType != null && x.RelationshipType.ImplicitOneToOneRelationshipType != null);
                foreach (var item in list)
                {

                    result.Add(ToImplicitOneToOneRelationshipDTO(item.RelationshipType.ImplicitOneToOneRelationshipType));

                }
                return result;
            }
        }
        public ImplicitOneToOneRelationshipDTO ToImplicitOneToOneRelationshipDTO(ImplicitOneToOneRelationshipType ImplicitOneToOneRelationshipType, RelationshipDTO baseRelationship = null)
        {
            BizRelationship biz = new Paper_MetadataManagement.BizRelationship();
            if (baseRelationship == null)
                baseRelationship = biz.ToRelationshipDTO(ImplicitOneToOneRelationshipType.RelationshipType.Relationship);
            Mapper.Initialize(cfg => cfg.CreateMap<RelationshipDTO, ImplicitOneToOneRelationshipDTO>());
            var result = AutoMapper.Mapper.Map<RelationshipDTO, ImplicitOneToOneRelationshipDTO>(baseRelationship);


            return result;
        }
        public void UpdateImplicitOneToOneRelationships(List<ImplicitOneToOneRelationshipDTO> relationships)
        {
            using (var projectContext = new MyIdeaEntities())
            {
                foreach (var relationship in relationships)
                {
                    var dbRelationship = projectContext.ImplicitOneToOneRelationshipType.First(x => x.ID == relationship.ID);
                    var dbrel = ToRelationship(relationship, projectContext);
                    dbRelationship.RelationshipType.IsOtherSideCreatable = relationship.IsOtherSideCreatable;
                    dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = relationship.IsOtherSideDirectlyCreatable;
                    dbRelationship.RelationshipType.IsOtherSideMandatory = relationship.IsOtherSideMandatory;
                    dbRelationship.RelationshipType.IsOtherSideTransferable = relationship.IsOtherSideTransferable;
                    dbRelationship.RelationshipType.Relationship.Name = relationship.Name;
                    dbRelationship.RelationshipType.Relationship.Alias = relationship.Alias;
                    dbRelationship.RelationshipType.Relationship.Enabled = relationship.Enabled;
                }
                projectContext.SaveChanges();
            }
        }





        public void RemoveManyToManyRelationships(int manyToManyID)
        {
            using (var projectContext = new MyIdeaEntities())
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
            //using (var projectContext = new MyIdeaEntities())
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

            using (var projectContext = new MyIdeaEntities())
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
            using (var projectContext = new MyIdeaEntities())
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
            using (var projectContext = new MyIdeaEntities())
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
        //    using (var projectContext = new MyIdeaEntities())
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
       
     

        public void ClearRelationshipType(RelationshipType relationshipType)
        {
            if (relationshipType.ManyToOneRelationshipType != null)
                relationshipType.ManyToOneRelationshipType = null;

            if (relationshipType.OneToManyRelationshipType != null)
                relationshipType.OneToManyRelationshipType = null;

            if (relationshipType.ExplicitOneToOneRelationshipType != null)
                relationshipType.ExplicitOneToOneRelationshipType = null;

            if (relationshipType.ImplicitOneToOneRelationshipType != null)
                relationshipType.ImplicitOneToOneRelationshipType = null;

            if (relationshipType.SuperToSubRelationshipType != null)
                relationshipType.SuperToSubRelationshipType = null;

            if (relationshipType.SubToSuperRelationshipType != null)
                relationshipType.SuperToSubRelationshipType = null;

            if (relationshipType.UnionToSubUnionRelationshipType != null)
                relationshipType.UnionToSubUnionRelationshipType = null;

            if (relationshipType.SubUnionToUnionRelationshipType != null)
                relationshipType.SubUnionToUnionRelationshipType = null;
        }

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
            else if (item == Enum_RelationshipType.SubUnionToUnion_SubUnionHoldsKeys)
                return "زیراجتماع به ابراجتماع_کلید در زیراجتماع";
            else if (item == Enum_RelationshipType.SubUnionToUnion_UnionHoldsKeys)
                return "زیراجتماع به ابراجتماع_کلید در ابراجتماع";
            else if (item == Enum_RelationshipType.UnionToSubUnion_SubUnionHoldsKeys)
                return "ابراجتماع به زیرجتماع_کلید در زیراجتماع";
            else if (item == Enum_RelationshipType.UnionToSubUnion_UnionHoldsKeys)
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
