using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paper_MetadataManagement
{
    public class BizISARelationship
    {
        public List<ISARelationshipDTO> GetISARelationshipsByEntityID(int superEntityID)
        {
            List<ISARelationshipDTO> result = new List<ISARelationshipDTO>();
            using (var projectContext = new MyIdeaEntities())
            {
                var isaRelationships = projectContext.ISARelationship.Where(x => x.SuperToSubRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.ID == superEntityID));
                foreach (var isaRelationship in isaRelationships)
                {
                    result.Add(ToISARelationshipDTO(isaRelationship));
                }

            }
            return result;
        }
        public ISARelationshipDTO GetISARelationships(int baseEntityID, int drivedEntitID)
        {
            ISARelationshipDTO result = new ISARelationshipDTO();
            using (var projectContext = new MyIdeaEntities())
            {
                var isaRelationship = projectContext.Relationship.First(x => x.TableDrivedEntityID2 == baseEntityID && x.RelationshipType.SubToSuperRelationshipType != null).RelationshipType.SubToSuperRelationshipType.ISARelationship;
                return ToISARelationshipDTO(isaRelationship);
            }
        }

        //public List<ISARelationshipDTO> GetISARelationships(int tableDrivedEntityID)
        //{
        //    List<ISARelationshipDTO> result = new List<ISARelationshipDTO>();
        //    using (var projectContext = new MyIdeaEntities())
        //    {
        //        var list = projectContext.ISARelationship.Where(x => x.TableDrivedEntityID1 == tableDrivedEntityID || x.TableDrivedEntityID2 == tableDrivedEntityID);
        //        foreach (var item in list)
        //        {
        //            result.Add(ToISARelationshipDTO(item));
        //        }
        //    }
        //    return result;
        //}
        private ISARelationshipDTO ToISARelationshipDTO(ISARelationship item)
        {
            ISARelationshipDTO result = new ISARelationshipDTO();
            result.Name = item.Name;
            result.ID = item.ID;
            result.IsGeneralization = item.IsGeneralization == true;
            result.IsTolatParticipation = item.IsTolatParticipation;
            result.IsDisjoint = item.IsDisjoint;
            result.SuperTypeEntities = "";
            foreach (var superType in item.SuperToSubRelationshipType)
            {
                if (!result.SuperTypeEntities.Contains(superType.RelationshipType.Relationship.TableDrivedEntity.Name))
                    result.SuperTypeEntities += (result.SuperTypeEntities == "" ? "" : ",") + superType.RelationshipType.Relationship.TableDrivedEntity.Name;
            }
            result.SubTypeEntities = "";
            foreach (var subType in item.SubToSuperRelationshipType)
            {
                result.SubTypeEntities += (result.SubTypeEntities == "" ? "" : ",") + subType.RelationshipType.Relationship.TableDrivedEntity.Name;
            }
            return result;
        }

        public int Save(ISARelationshipDTO item)
        {
            using (var projectContext = new MyIdeaEntities())
            {
                ISARelationship dbItem = null;
                if (item.ID == 0)
                {
                    dbItem = new ISARelationship();
                    projectContext.ISARelationship.Add(dbItem);
                }
                else
                    dbItem = projectContext.ISARelationship.First(x => x.ID == item.ID);

                dbItem.Name = item.Name;
                dbItem.IsDisjoint = item.IsDisjoint;
                dbItem.IsGeneralization = item.IsGeneralization;
                dbItem.IsSpecialization = !item.IsSpecialization;
                dbItem.IsTolatParticipation = item.IsTolatParticipation;
                projectContext.SaveChanges();
                return dbItem.ID;
            }
        }



        public List<ISARelationshipDTO> GetISARelationships(int databaseID)
        {
            List<ISARelationshipDTO> result = new List<ISARelationshipDTO>();
            using (var projectContext = new MyIdeaEntities())
            {
                // string catalogName = GeneralHelper.GetCatalogName(serverName, dbName);
                foreach (var item in projectContext.ISARelationship.Where(x => x.SuperToSubRelationshipType.Any(y => y.RelationshipType.Relationship.TableDrivedEntity.Table.DBSchema.DatabaseInformationID == databaseID)))
                {
                    result.Add(ToISARelationshipDTO(item));
                }

            }
            return result;
        }
        public void UpdateISARelationships(List<ISARelationshipDTO> relationships)
        {
            using (var projectContext = new MyIdeaEntities())
            {
                foreach (var relationship in relationships)
                {
                    Save(relationship);
                }
            }
        }

        public List<SuperToSubRelationshipDTO> GetSuperToSubRelationship(int iSARelationshipID)
        {
            List<SuperToSubRelationshipDTO> result = new List<SuperToSubRelationshipDTO>();
            using (var projectContext = new MyIdeaEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                var iSaRelationship = projectContext.ISARelationship.FirstOrDefault(x => x.ID == iSARelationshipID);
                if (iSaRelationship != null)
                {
                    BizRelationship biz = new BizRelationship();
                    foreach (var item in iSaRelationship.SuperToSubRelationshipType)
                    {
                        result.Add(ToSuperToSubRelationshipDTO(item));
                    }
                }
            }
            return result;
        }


        public SuperToSubRelationshipDTO ToSuperToSubRelationshipDTO(SuperToSubRelationshipType item, RelationshipDTO baseRelationship = null)
        {
            BizRelationship biz = new BizRelationship();
            if (baseRelationship == null)
                baseRelationship = biz.ToRelationshipDTO(item.RelationshipType.Relationship);
            Mapper.Initialize(cfg => cfg.CreateMap<RelationshipDTO, SuperToSubRelationshipDTO>());
            var result = AutoMapper.Mapper.Map<RelationshipDTO, SuperToSubRelationshipDTO>(baseRelationship);
            result.ISARelationship = ToISARelationshipDTO(item.ISARelationship);
            return result;
        }

        public List<SubToSuperRelationshipDTO> GetSubToSuperRelationship(int iSARelationshipID)
        {
            List<SubToSuperRelationshipDTO> result = new List<SubToSuperRelationshipDTO>();
            using (var projectContext = new MyIdeaEntities())
            {
                //projectContext.Configuration.LazyLoadingEnabled = false;
                var iSaRelationship = projectContext.ISARelationship.FirstOrDefault(x => x.ID == iSARelationshipID);
                if (iSaRelationship != null)
                {
                    BizRelationship biz = new BizRelationship();
                    foreach (var item in iSaRelationship.SubToSuperRelationshipType)
                    {
                        result.Add(ToSubToSuperRelationshipDTO(item));
                    }
                }
            }
            return result;
        }
        public SubToSuperRelationshipDTO ToSubToSuperRelationshipDTO(SubToSuperRelationshipType item, RelationshipDTO baseRelationship = null)
        {

            BizRelationship biz = new Paper_MetadataManagement.BizRelationship();
            if (baseRelationship == null)
                baseRelationship = biz.ToRelationshipDTO(item.RelationshipType.Relationship);
            Mapper.Initialize(cfg => cfg.CreateMap<RelationshipDTO, SubToSuperRelationshipDTO>());
            var result = AutoMapper.Mapper.Map<RelationshipDTO, SubToSuperRelationshipDTO>(baseRelationship);
            result.ISARelationship = ToISARelationshipDTO(item.ISARelationship);

            return result;
        }


        public void UpdateSuperToSubRelationships(List<SuperToSubRelationshipDTO> relationships)
        {
            using (var projectContext = new MyIdeaEntities())
            {
                foreach (var relationship in relationships)
                {
                    var dbRelationship = projectContext.SuperToSubRelationshipType.First(x => x.ID == relationship.ID);
                    dbRelationship.RelationshipType.IsOtherSideCreatable = relationship.IsOtherSideCreatable;
                    dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = relationship.IsOtherSideDirectlyCreatable;
                    dbRelationship.RelationshipType.IsOtherSideTransferable = relationship.IsOtherSideTransferable;
                    dbRelationship.RelationshipType.Relationship.Name = relationship.Name;
                    dbRelationship.RelationshipType.Relationship.Alias = relationship.Alias;
                    dbRelationship.RelationshipType.Relationship.Enabled = relationship.Enabled;
                }
                projectContext.SaveChanges();
            }
        }

        public void UpdateSubToSuperRelationships(List<SubToSuperRelationshipDTO> relationships)
        {
            using (var projectContext = new MyIdeaEntities())
            {
                foreach (var relationship in relationships)
                {
                    var dbRelationship = projectContext.SubToSuperRelationshipType.First(x => x.ID == relationship.ID);
                    dbRelationship.RelationshipType.IsOtherSideCreatable = relationship.IsOtherSideCreatable;
                    dbRelationship.RelationshipType.IsOtherSideDirectlyCreatable = relationship.IsOtherSideDirectlyCreatable;
                    dbRelationship.RelationshipType.IsOtherSideTransferable = relationship.IsOtherSideTransferable;
                    dbRelationship.RelationshipType.Relationship.Name = relationship.Name;
                    dbRelationship.RelationshipType.Relationship.Alias = relationship.Alias;
                    dbRelationship.RelationshipType.Relationship.Enabled = relationship.Enabled;
                }
                projectContext.SaveChanges();
            }
        }


        public void MergeISARelationships(string name, List<ISARelationshipDTO> relationships, ISARelationshipDTO selectedOne)
        {
            using (var projectContext = new MyIdeaEntities())
            {
                int isaRelationID = 0;
                foreach (var relationship in relationships)
                {

                    //if (relationship == selectedOne)
                    //    isaRelationID = relationship.ID;
                    //else
                    //{
                    var dbRelationship = projectContext.ISARelationship.First(x => x.ID == relationship.ID);
                    foreach (var detail in dbRelationship.SuperToSubRelationshipType)
                    {
                        detail.ISARelationshipID = selectedOne.ID;
                    }
                    foreach (var detail in dbRelationship.SubToSuperRelationshipType)
                    {
                        detail.ISARelationshipID = selectedOne.ID;
                    }
                    //}

                }
                projectContext.SaveChanges();
            }
        }
    }

}
