using DataAccess;
using ModelEntites;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizSearchRepository
    {
        public List<DP_SearchRepository> GetSearchRepositories(int entityID)
        {
            List<DP_SearchRepository> result = new List<DP_SearchRepository>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var listEntityReport = projectContext.SearchRepository.Where(x => x.Title != null && x.Title != "" && x.TableDrivedEntityID == entityID);
                foreach (var item in listEntityReport)
                    result.Add(ToSearchRepositoryDTO(item));
            }
            return result;
        }
        public DP_SearchRepository GetSearchRepository(int ID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var listEntityReport = projectContext.SearchRepository.First(x => x.ID == ID);
                return ToSearchRepositoryDTO(listEntityReport);
            }
        }
        public int Update(DP_SearchRepository preDefinedSearch)
        {
            try
            {
                using (var projectContext = new DataAccess.MyProjectEntities())
                {
                    var dbPreDefinedSearch = projectContext.SearchRepository.FirstOrDefault(x => x.ID == preDefinedSearch.ID);
                    //if (dbPreDefinedSearch == null)
                    //{
                    //    dbPreDefinedSearch = new SearchRepository();
                    //    projectContext.SearchRepository.Add(dbPreDefinedSearch);
                    //}
                    if (dbPreDefinedSearch != null)
                        RemoveSearchRepository(projectContext, dbPreDefinedSearch);
                    var phrase = CreateSearchRepository(projectContext, preDefinedSearch);
                    projectContext.SaveChanges();
                    return phrase.ID;
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
        private DataAccess.Phrase CreateSearchRepository(MyProjectEntities projectContext, DP_SearchRepository searchRepository)
        {
            DataAccess.Phrase phrase = new DataAccess.Phrase();
            phrase.Type = 1;
            projectContext.Phrase.Add(phrase);
            phrase.SearchRepository = new SearchRepository();
            var dbSearchRepository = phrase.SearchRepository;


            //projectContext.LogicPhrase.Add(dbLogicPhrase);
            //dbSearchRepository.Phrase.Add(new DataAccess.Phrase());
            dbSearchRepository.LogicPhrase = ToLogicPhrase(projectContext, searchRepository as ProxyLibrary.LogicPhrase).Item1;
            //   ToLogicPhraseAsPhrase(searchRepository as ProxyLibrary.LogicPhrase)
            dbSearchRepository.IsSimpleSearch = searchRepository.IsSimpleSearch;
            if (searchRepository.EntitySearchID != 0)
                dbSearchRepository.EntitySearchID = searchRepository.EntitySearchID;
            else
                dbSearchRepository.EntitySearchID = null;
            dbSearchRepository.TableDrivedEntityID = searchRepository.TargetEntityID;
            if (searchRepository.SourceRelationship == null)
                dbSearchRepository.Title = searchRepository.Title;
            else
                dbSearchRepository.Title = "";
            dbSearchRepository.HasNotRelationshipCheck = searchRepository.HasNotRelationshipCheck;
            //dbSearchRepository.HasRelationshipCheck = searchRepository.HasRelationshipCheck;
            dbSearchRepository.RelationshipFromCount = searchRepository.RelationshipFromCount;
            dbSearchRepository.RelationshipToCount = searchRepository.RelationshipToCount;
            if (searchRepository.SourceRelationship != null)
                dbSearchRepository.SourceRelationID = searchRepository.SourceRelationship.ID;
            else
                dbSearchRepository.SourceRelationID = null;
            //   SetLogicPhrase(dbSearchRepository.LogicPhrase, searchRepository as ProxyLibrary.LogicPhrase);

            return phrase;
        }




        private void SetLogicPhrase(MyProjectEntities projectContext, DataAccess.LogicPhrase dbLogicPhrase, ProxyLibrary.LogicPhrase logicPhrase)
        {
            dbLogicPhrase.AndOrType = logicPhrase.AndOrType == AndORType.And ? true : false;
            foreach (var phrase in logicPhrase.Phrases)
            {
                DataAccess.Phrase dbPhrase = null;
                if (phrase is SearchProperty)
                    dbPhrase = ToColumnPhrase(projectContext, phrase as SearchProperty).Item2;
                else if (phrase is DP_SearchRepository)
                    dbPhrase = CreateSearchRepository(projectContext, phrase as DP_SearchRepository);
                else if (phrase is ProxyLibrary.LogicPhrase)
                    dbPhrase = ToLogicPhrase(projectContext, phrase as ProxyLibrary.LogicPhrase).Item2;
                dbPhrase.LogicPhrase1 = dbLogicPhrase;
            }
        }

        //private DataAccess.Phrase ToSearchRepositoryAsPhrase(DP_SearchRepository dP_SearchRepository)
        //{
        //    DataAccess.Phrase dbphrase = new DataAccess.Phrase();
        //    dbphrase.SearchRepository = ToSearchRepository(dP_SearchRepository);
        //    return dbphrase;
        //}
        //private DataAccess.LogicPhrase ToLogicPhrase(ProxyLibrary.LogicPhrase logicPhrase)
        //{
        //    DataAccess.LogicPhrase dbphrase = new DataAccess.LogicPhrase();
        //    dbphrase.Phrase.Add(new DataAccess.Phrase());
        //    dbphrase.LogicPhrase = ToLogicPhrase(logicPhrase);
        //    return dbphrase;
        //}
        private Tuple<DataAccess.LogicPhrase, DataAccess.Phrase> ToLogicPhrase(MyProjectEntities projectContext, ProxyLibrary.LogicPhrase logicPhrase)
        {
            //if (logicPhrase.Phrases.Any())
            //{
            DataAccess.Phrase phrase = new DataAccess.Phrase();
            phrase.Type = 2;
            projectContext.Phrase.Add(phrase);
            phrase.LogicPhrase = new DataAccess.LogicPhrase();
            var dbLogicPhrase = phrase.LogicPhrase;
            SetLogicPhrase(projectContext, dbLogicPhrase, logicPhrase);
            return new Tuple<DataAccess.LogicPhrase, DataAccess.Phrase>(dbLogicPhrase, phrase);
            //}
            //else
            //    return null;
        }


        //private DataAccess.Phrase ToLogicPhraseAsPhrase(ProxyLibrary.LogicPhrase logicPhrase)
        //{
        //    DataAccess.Phrase dbphrase = new DataAccess.Phrase();
        //    dbphrase.LogicPhrase = ToLogicPhrase(logicPhrase);
        //    return dbphrase;
        //}
        private Tuple<DataAccess.ColumnPhrase, DataAccess.Phrase> ToColumnPhrase(MyProjectEntities projectContext, SearchProperty searchProperty)
        {
            DataAccess.Phrase phrase = new DataAccess.Phrase();
            phrase.Type = 3;
            projectContext.Phrase.Add(phrase);
            phrase.ColumnPhrase = new ColumnPhrase();
            var dbColumnPhrase = phrase.ColumnPhrase;

            dbColumnPhrase.ColumnID = searchProperty.ColumnID;
            dbColumnPhrase.EntitySearchColumnsID = searchProperty.SearchColumnID == 0 ? (int?)null : searchProperty.SearchColumnID;
            dbColumnPhrase.Operator = searchProperty.Operator.ToString();
            dbColumnPhrase.Value = searchProperty.Value == null ? null : searchProperty.Value.ToString();
            return new Tuple<ColumnPhrase, DataAccess.Phrase>(dbColumnPhrase, phrase);
        }

        private void RemoveLogicPhrase(MyProjectEntities projectContext, DataAccess.LogicPhrase logicPhrase)
        {
            while (logicPhrase.Phrase1.Any())
            {
                foreach (var item in logicPhrase.Phrase1.ToList())
                {
                    RemovePhrase(projectContext, item);
                }
            }
            projectContext.LogicPhrase.Remove(logicPhrase);
        }

        private void RemovePhrase(MyProjectEntities projectContext, DataAccess.Phrase item)
        {
            if (item.ColumnPhraseID != null)
            {
                projectContext.ColumnPhrase.Remove(item.ColumnPhrase);
                //  item.ColumnPhrase = null;
            }
            else if (item.SearchRepositoryID != null)
            {
                RemovePhrase(projectContext, item.SearchRepository.LogicPhrase.Phrase.First());
                //    projectContext.LogicPhrase.Remove(item.SearchRepository.LogicPhrase);
                //projectContext.SearchRepository.Remove(item.SearchRepository);

                // item.SearchRepository = null;
            }
            else if (item.LogicPhraseID != null)
            {
                //Phrase1 به ParentLogicPhraseID ربط دارد
                foreach (var litem in item.LogicPhrase.Phrase1.ToList())
                    RemovePhrase(projectContext, litem);
                projectContext.LogicPhrase.Remove(item.LogicPhrase);
                //    item.LogicPhrase = null;
            }
            projectContext.Phrase.Remove(item);
        }
        private void RemoveSearchRepository(MyProjectEntities projectContext, SearchRepository searchRepository)
        {

            if (searchRepository.Phrase.Any())
                RemovePhrase(projectContext, searchRepository.Phrase.First());
        }

        //public DP_SearchRepository ToSearchRepository(SearchRepository entityPreDefinedSearch, bool withDetails)
        //{
        //    //فانکشن به یه کلاس مستقل دیگر منتقل شود
        //    DP_SearchRepository result = new DP_SearchRepository();
        //    result.TargetEntityID = entityPreDefinedSearch.TableDrivedEntityID??0;
        //    result.Title = entityPreDefinedSearch.Title;
        //    result.EntitySearchID = entityPreDefinedSearch.EntitySearchID ?? 0;
        //    //result.IsSimpleSearch = entityPreDefinedSearch.IsSimpleSearch;
        //    result.ID = entityPreDefinedSearch.ID;
        //    if (withDetails)
        //    {
        //        if (entityPreDefinedSearch.SearchRepository != null)
        //            result.SearchRepository = ToSearchRepositoryDTO(entityPreDefinedSearch.SearchRepository, entityPreDefinedSearch.TableDrivedEntityID, entityPreDefinedSearch.TableDrivedEntity.Name);
        //        //foreach (var item in entityPreDefinedSearch.PreDefinedSearchSimpleSearchColumns)
        //        //{
        //        //    PreDefinedSearchColumns newCol = new PreDefinedSearchColumns();
        //        //    newCol.ColumnID = item.ColumnID ?? 0;
        //        //    newCol.EntitySearchColumnsID = item.EntitySearchColumnsID ?? 0;
        //        //    newCol.Value = item.Value;
        //        //    newCol.Operator = (CommonOperator)Enum.Parse(typeof(CommonOperator), item.Operator);
        //        //    result.SimpleColumns.Add(newCol);
        //        //}
        //    }
        //    return result;
        //}

        private ProxyLibrary.LogicPhrase ToLogicPhraseDTO(DataAccess.LogicPhrase logicPhrase)
        {
            ProxyLibrary.LogicPhrase result = new ProxyLibrary.LogicPhrase();
            SetLogicPhraseDTO(logicPhrase, result);
            return result;
        }

        private void SetLogicPhraseDTO(DataAccess.LogicPhrase logicPhrase, ProxyLibrary.LogicPhrase logicPhraseDTO)
        {
            logicPhraseDTO.AndOrType = logicPhrase.AndOrType ? AndORType.And : AndORType.Or;
            foreach (var dbPhrase in logicPhrase.Phrase1)
            {
                if (dbPhrase.ColumnPhrase != null)
                    logicPhraseDTO.Phrases.Add(ToColumnPhraseDTO(dbPhrase.ColumnPhrase));
                else if (dbPhrase.SearchRepository != null)
                    logicPhraseDTO.Phrases.Add(ToSearchRepositoryDTO(dbPhrase.SearchRepository));
                else if (dbPhrase.LogicPhrase != null)
                    logicPhraseDTO.Phrases.Add(ToLogicPhraseDTO(dbPhrase.LogicPhrase));
            }
        }
        public DP_SearchRepository ToSearchRepositoryDTO(SearchRepository SearchRepository)
        {
            DP_SearchRepository searchRepository = null;
            if (SearchRepository.SourceRelationID == null)
            {
                searchRepository = new DP_SearchRepository(SearchRepository.TableDrivedEntityID ?? 0);
                searchRepository.Title = SearchRepository.TableDrivedEntity.Alias;
            }
            else
            {
                var relDTO = new BizRelationship().ToRelationshipDTO(SearchRepository.Relationship);
                searchRepository = new DP_SearchRepository(relDTO.EntityID2);
                searchRepository.Title = relDTO.Name;
                searchRepository.HasNotRelationshipCheck = SearchRepository.HasNotRelationshipCheck;
                searchRepository.RelationshipFromCount = SearchRepository.RelationshipFromCount;
                searchRepository.RelationshipToCount = SearchRepository.RelationshipToCount;
                //searchRepository.SourceEntityID = relDTO.EntityID1;
                searchRepository.SourceRelationship = relDTO;


            }
            searchRepository.ID = SearchRepository.ID;
            searchRepository.Title = SearchRepository.Title;
            searchRepository.EntitySearchID = SearchRepository.EntitySearchID ?? 0;
            searchRepository.IsSimpleSearch = SearchRepository.IsSimpleSearch;

            SetLogicPhraseDTO(SearchRepository.LogicPhrase, searchRepository);

            return searchRepository;
        }
        //private DP_SearchRepository ToSearchRepositoryDTO(SearchRepository SearchRepository)
        //{
        //    var relDTO = new BizRelationship().GetRelationship(SearchRepository.SourceRelationID);
        //    DP_SearchRepository searchRepository = new DP_SearchRepository(relDTO.EntityID2);
        //    searchRepository.Name = relDTO.Name;
        //    searchRepository.HasNotRelationshipCheck = SearchRepository.HasNotRelationshipCheck;
        //    //searchRepository.HasRelationshipCheck = SearchRepository.HasRelationshipCheck;
        //    searchRepository.RelationshipFromCount = SearchRepository.RelationshipFromCount;
        //    searchRepository.RelationshipToCount = SearchRepository.RelationshipToCount;
        //    searchRepository.SourceEntityID = relDTO.EntityID1;
        //    searchRepository.SourceRelationID = relDTO.ID;
        //    searchRepository.SourceToTargetMasterRelationshipType = relDTO.MastertTypeEnum;
        //    searchRepository.SourceToTargetRelationshipType = relDTO.TypeEnum;
        //    if (SearchRepository.LogicPhrase != null)
        //        searchRepository.Phrases.Add(ToLogicPhraseDTO(SearchRepository.LogicPhrase));

        //    return searchRepository;
        //}

        private ProxyLibrary.Phrase ToColumnPhraseDTO(ColumnPhrase columnPhrase)
        {
            SearchProperty property = new SearchProperty();
            property.ColumnID = columnPhrase.ColumnID;
            property.SearchColumnID = columnPhrase.EntitySearchColumnsID ?? 0;
            property.Operator = (CommonOperator)Enum.Parse(typeof(CommonOperator), columnPhrase.Operator);
            property.Value = columnPhrase.Value;
            return property;
        }
    }
}
