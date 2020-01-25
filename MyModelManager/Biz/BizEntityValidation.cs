using DataAccess;
using ModelEntites;
using MyCacheManager;
using MyGeneralLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizEntityValidation
    {
        public List<EntityValidationDTO> GetEntityValidations(int entityID, bool withDetails)
        {
            var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.Validation, entityID.ToString());
            if (cachedItem != null)
                return (cachedItem as List<EntityValidationDTO>);

            List<EntityValidationDTO> result = new List<EntityValidationDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var listEntityValidation = projectContext.EntityValidation.Where(x => x.TableDrivedEntityID == entityID);
                foreach (var item in listEntityValidation)
                    result.Add(ToEntityValidationDTO(item, withDetails));

            }
            CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.Validation, entityID.ToString());
            return result;
        }
        public EntityValidationDTO GetEntityValidation(int entityValidationsID, bool withDetails)
        {
            List<EntityValidationDTO> result = new List<EntityValidationDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var EntityValidations = projectContext.EntityValidation.First(x => x.ID == entityValidationsID);
                return ToEntityValidationDTO(EntityValidations, withDetails);
            }
        }
        private EntityValidationDTO ToEntityValidationDTO(EntityValidation item, bool withDetails)
        {
            EntityValidationDTO result = new EntityValidationDTO();
            result.FormulaID = item.FormulaID ?? 0;
            if (result.FormulaID != 0 && withDetails)
            {
                //??با جزئیات؟؟........................................................................ 
                var bizFormula = new BizFormula();
                //result.Formula = bizFormula.GetFormula(item.FormulaID.Value,false);
            }
            //result.CodeFunctionID = item.CodeFunctionID ?? 0;
            //if (result.CodeFunctionID != 0 && withDetails)
            //{
            //    var bizCodeFunction = new BizCodeFunction();
            //    result.CodeFunction = bizCodeFunction.GetCodeFunction(item.CodeFunctionID.Value);
            //}
            result.TableDrivedEntityID = item.TableDrivedEntityID;
            result.ID = item.ID;
            result.Message = item.Message;
            result.Title = item.Title;
         //   result.ResultSensetive = item.ResultSensetive;
            //result.Value = item.Value;
            return result;
        }
        public void UpdateEntityValidations(EntityValidationDTO EntityValidation)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbEntityValidation = projectContext.EntityValidation.FirstOrDefault(x => x.ID == EntityValidation.ID);
                if (dbEntityValidation == null)
                    dbEntityValidation = new DataAccess.EntityValidation();
                if (EntityValidation.FormulaID != 0)
                    dbEntityValidation.FormulaID = EntityValidation.FormulaID;
                else
                    dbEntityValidation.FormulaID = null;
                //if (EntityValidation.CodeFunctionID != 0)
                //{
                //    dbEntityValidation.CodeFunctionID = EntityValidation.CodeFunctionID;

                //}
                //else
                //    dbEntityValidation.CodeFunctionID = null;
                dbEntityValidation.TableDrivedEntityID = EntityValidation.TableDrivedEntityID;
                dbEntityValidation.ID = EntityValidation.ID;
                dbEntityValidation.Message = EntityValidation.Message;
                dbEntityValidation.Title = EntityValidation.Title;
              //  dbEntityValidation.ResultSensetive = EntityValidation.ResultSensetive;
                //dbEntityValidation.Value = EntityValidation.Value;
                if (dbEntityValidation.ID == 0)
                    projectContext.EntityValidation.Add(dbEntityValidation);
                projectContext.SaveChanges();
            }
        }
    }

}
