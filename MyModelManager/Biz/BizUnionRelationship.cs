using AutoMapper;
using DataAccess;
using ModelEntites;
using MyGeneralLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizUnionRelationship
    {
        //public List<UnionRelationshipDTO> GetUnionRelationships(int tableDrivedEntityID)
        //{
        //    List<UnionRelationshipDTO> result = new List<UnionRelationshipDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var list = projectContext.UnionRelationship.Where(x => x.TableDrivedEntityID1 == tableDrivedEntityID || x.TableDrivedEntityID2 == tableDrivedEntityID);
        //        foreach (var item in list)
        //        {
        //            result.Add(ToUnionRelationshipDTO(item));
        //        }
        //    }
        //    return result;
        //}

        public List<UnionRelationshipDTO> GetUnionRelationshipsBySuperUnionEntity(int superunionEntityID, bool unionHoldsKeys)
        {
            List<UnionRelationshipDTO> result = new List<UnionRelationshipDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var UnionRelationships = projectContext.UnionRelationshipType.Where(x => x.UnionHoldsKeys == unionHoldsKeys && x.UnionToSubUnionRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == superunionEntityID));
                foreach (var UnionRelationship in UnionRelationships)
                {
                    result.Add(ToUnionRelationshipDTO(UnionRelationship));
                }

            }
            return result;
        }
        public UnionRelationshipDTO ToUnionRelationshipDTO(UnionRelationshipType item)
        {
            UnionRelationshipDTO result = new UnionRelationshipDTO();
            result.Name = item.Name;
            result.ID = item.ID;
            result.IsTolatParticipation = item.IsTolatParticipation;
            result.UnionHoldsKeys = item.UnionHoldsKeys;
            if (item.UnionToSubUnionRelationshipType.Any())
                result.SuperEntityID = item.UnionToSubUnionRelationshipType.First().RelationshipType.Relationship.TableDrivedEntityID1;
            result.SuperTypeEntities = "";
            foreach (var superType in item.UnionToSubUnionRelationshipType)
            {
                if (!result.SuperTypeEntities.Contains(superType.RelationshipType.Relationship.TableDrivedEntity.Name))
                    result.SuperTypeEntities += (result.SuperTypeEntities == "" ? "" : ",") + superType.RelationshipType.Relationship.TableDrivedEntity.Name;
            }
            result.SubTypeEntities = "";
            foreach (var subType in item.SubUnionToUnionRelationshipType)
            {
                result.SubTypeEntities += (result.SubTypeEntities == "" ? "" : ",") + subType.RelationshipType.Relationship.TableDrivedEntity.Name;
            }
            return result;
        }

        public int Save(UnionRelationshipDTO item)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                UnionRelationshipType dbItem = null;
                if (item.ID == 0)
                {
                    dbItem = new UnionRelationshipType();
                    projectContext.UnionRelationshipType.Add(dbItem);
                }
                else
                    dbItem = projectContext.UnionRelationshipType.First(x => x.ID == item.ID);

                dbItem.Name = item.Name;
                dbItem.IsTolatParticipation = item.IsTolatParticipation;
                dbItem.UnionHoldsKeys = item.UnionHoldsKeys;
                projectContext.SaveChanges();
                return dbItem.ID;
            }
        }



        public List<UnionRelationshipDTO> GetUnionRelationships(int databaseID)
        {
            List<UnionRelationshipDTO> result = new List<UnionRelationshipDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
                foreach (var item in projectContext.UnionRelationshipType.Where(x => x.UnionToSubUnionRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.Table.DBSchema.DatabaseInformationID == databaseID)))
                {
                    result.Add(ToUnionRelationshipDTO(item));
                }

            }
            return result;
        }
        public void UpdateUnionRelationships(List<UnionRelationshipDTO> relationships)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var relationship in relationships)
                {
                    Save(relationship);
                }
            }
        }

        public List<UnionToSubUnionRelationshipDTO> GetSuperUnionToSubUnionRelationship(int UnionRelationshipID)
        {
            List<UnionToSubUnionRelationshipDTO> result = new List<UnionToSubUnionRelationshipDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                var UnionRelationship = projectContext.UnionRelationshipType.FirstOrDefault(x => x.ID == UnionRelationshipID);
                if (UnionRelationship != null)
                {
                    BizRelationship biz = new BizRelationship();
                    foreach (var item in UnionRelationship.UnionToSubUnionRelationshipType)
                    {

                        result.Add(ToSuperUnionToSubUnionRelationshipDTO(item));
                    }
                }
            }
            return result;
        }


        public UnionToSubUnionRelationshipDTO ToSuperUnionToSubUnionRelationshipDTO(UnionToSubUnionRelationshipType item, RelationshipDTO baseRelationship = null)
        {
            BizRelationship biz = new MyModelManager.BizRelationship();
            if (baseRelationship == null)
                baseRelationship = biz.ToRelationshipDTO(item.RelationshipType.Relationship);
            Mapper.Initialize(cfg => cfg.CreateMap<RelationshipDTO, UnionToSubUnionRelationshipDTO>());
            var result = AutoMapper.Mapper.Map<RelationshipDTO, UnionToSubUnionRelationshipDTO>(baseRelationship);
            result.UnionRelationship = ToUnionRelationshipDTO(item.UnionRelationshipType);
            result.DeterminerColumnValue = item.DeterminerColumnValue;
            result.DeterminerColumnID = item.DeterminerColumnID ?? 0;
            return result;
        }

        public List<SubUnionToSuperUnionRelationshipDTO> GetSubUnionToSuperUnionRelationship(int UnionRelationshipID)
        {
            List<SubUnionToSuperUnionRelationshipDTO> result = new List<SubUnionToSuperUnionRelationshipDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                var UnionRelationship = projectContext.UnionRelationshipType.FirstOrDefault(x => x.ID == UnionRelationshipID);
                if (UnionRelationship != null)
                {
                    BizRelationship biz = new BizRelationship();
                    foreach (var item in UnionRelationship.SubUnionToUnionRelationshipType)
                    {

                        result.Add(ToSubUnionToSuperUnionRelationshipDTO(item));
                    }
                }
            }
            return result;
        }
        public SubUnionToSuperUnionRelationshipDTO ToSubUnionToSuperUnionRelationshipDTO(SubUnionToUnionRelationshipType item, RelationshipDTO baseRelationship = null)
        {
            BizRelationship biz = new MyModelManager.BizRelationship();
            if (baseRelationship == null)
                baseRelationship = biz.ToRelationshipDTO(item.RelationshipType.Relationship);
            Mapper.Initialize(cfg => cfg.CreateMap<RelationshipDTO, SubUnionToSuperUnionRelationshipDTO>());
            var result = AutoMapper.Mapper.Map<RelationshipDTO, SubUnionToSuperUnionRelationshipDTO>(baseRelationship);
            result.UnionRelationship = ToUnionRelationshipDTO(item.UnionRelationshipType);
            result.DeterminerColumnValue = item.RelationshipType.Relationship.Relationship2.RelationshipType.UnionToSubUnionRelationshipType.DeterminerColumnValue;
            result.DeterminerColumnID = item.RelationshipType.Relationship.Relationship2.RelationshipType.UnionToSubUnionRelationshipType.DeterminerColumnID ?? 0;
            return result;
        }


        public void UpdateSuperUnionToSubUnionRelationships(List<UnionToSubUnionRelationshipDTO> relationships)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var relationship in relationships)
                {
                    var dbRelationship = projectContext.UnionToSubUnionRelationshipType.First(x => x.ID == relationship.ID);
                    dbRelationship.RelationshipType.IsOtherSideCreatable = relationship.IsOtherSideCreatable;
                    dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = relationship.IsOtherSideDirectlyCreatable;
                    //dbRelationship.RelationshipType.IsOtherSideTransferable = relationship.IsOtherSideTransferable;
                    dbRelationship.RelationshipType.Relationship.Name = relationship.Name;
                    dbRelationship.RelationshipType.Relationship.Alias = relationship.Alias;
                    dbRelationship.DeterminerColumnValue = relationship.DeterminerColumnValue;
                    if (relationship.DeterminerColumnID != 0)
                        dbRelationship.DeterminerColumnID = relationship.DeterminerColumnID;
                    else
                        dbRelationship.DeterminerColumnID = null;
                }
                projectContext.SaveChanges();
            }
        }

        public void UpdateSubUnionToSuperUnionRelationships(List<SubUnionToSuperUnionRelationshipDTO> relationships)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var relationship in relationships)
                {
                    var dbRelationship = projectContext.SubUnionToUnionRelationshipType.First(x => x.ID == relationship.ID);
                    dbRelationship.RelationshipType.IsOtherSideCreatable = relationship.IsOtherSideCreatable;
                    dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = relationship.IsOtherSideDirectlyCreatable;
                    //dbRelationship.RelationshipType.IsOtherSideTransferable = relationship.IsOtherSideTransferable;
                    dbRelationship.RelationshipType.Relationship.Name = relationship.Name;
                    dbRelationship.RelationshipType.Relationship.Alias = relationship.Alias;
                }
                projectContext.SaveChanges();
            }
        }


        public void MergeUnionRelationships(string name, List<UnionRelationshipDTO> relationships, UnionRelationshipDTO selectedOne)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                //   int UnionRelationID = 0;
                foreach (var relationship in relationships)
                {

                    //if (relationship == selectedOne)
                    //    UnionRelationID = relationship.ID;
                    //else
                    //{
                    var dbRelationship = projectContext.UnionRelationshipType.First(x => x.ID == relationship.ID);
                    foreach (var detail in dbRelationship.UnionToSubUnionRelationshipType)
                    {
                        detail.UnionRelationshipTypeID = selectedOne.ID;
                    }
                    foreach (var detail in dbRelationship.SubUnionToUnionRelationshipType)
                    {
                        detail.UnionRelationshipTypeID = selectedOne.ID;
                    }
                    //}

                }
                projectContext.SaveChanges();
            }
        }
    }

}
