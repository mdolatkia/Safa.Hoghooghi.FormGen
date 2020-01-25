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
    public class BizBackendBackendActionActivity
    {
        public List<BackendActionActivityDTO> GetActionActivities(int entityID, List<Enum_EntityActionActivityStep> BackendActionActivitySteps, bool withDetails)
        {
            List<BackendActionActivityDTO> result = new List<BackendActionActivityDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var listBackendActionActivity = projectContext.BackendActionActivity.Where(x => x.TableDrivedEntityID == entityID);

                //if (BackendActionActivityTypes != null && BackendActionActivityTypes.Any())
                //{
                //    List<short> BackendActionActivityTypeIds = new List<short>();
                //    foreach (var BackendActionActivityType in BackendActionActivityTypes)
                //        BackendActionActivityTypeIds.Add((short)BackendActionActivityType);
                //    listBackendActionActivity = listBackendActionActivity.Where(x => BackendActionActivityTypeIds.Contains(x.Type));
                //}
                if (BackendActionActivitySteps != null && BackendActionActivitySteps.Any())
                {
                    List<short> BackendActionActivityStepIds = new List<short>();
                    foreach (var BackendActionActivityStep in BackendActionActivitySteps)
                        BackendActionActivityStepIds.Add((short)BackendActionActivityStep);
                    listBackendActionActivity = listBackendActionActivity.Where(x => BackendActionActivityStepIds.Contains(x.StepType));
                }

                foreach (var item in listBackendActionActivity)
                    result.Add(ToBackendActionActivityDTO(item, withDetails));

            }
            return result;
        }
        public BackendActionActivityDTO GetBackendActionActivity(int BackendActionActivitysID)
        {
            List<BackendActionActivityDTO> result = new List<BackendActionActivityDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var BackendActionActivitys = projectContext.BackendActionActivity.First(x => x.ID == BackendActionActivitysID);
                return ToBackendActionActivityDTO(BackendActionActivitys, true);
            }
        }
        public BackendActionActivityDTO ToBackendActionActivityDTO(BackendActionActivity item, bool withDetails)
        {
            BackendActionActivityDTO result = new BackendActionActivityDTO();
            result.Type = (Enum_ActionActivityType)item.Type;


            if (withDetails && result.Type == Enum_ActionActivityType.DatabaseFunction)
            {
                result.DatabaseFunctionID = item.DatabaseFunctionID ?? 0;
                BizDatabaseFunction bizDatabaseFunction = new MyModelManager.BizDatabaseFunction();
                result.DatabaseFunction = bizDatabaseFunction.ToDatabaseFunctionDTO(item.DatabaseFunction, withDetails);
            }

            if (withDetails && result.Type == Enum_ActionActivityType.CodeFunction)
            {
                result.CodeFunctionID = item.CodeFunctionID ?? 0;
                BizCodeFunction bizCodeFunction = new MyModelManager.BizCodeFunction();
                if (item.CodeFunction != null)
                    result.CodeFunction = bizCodeFunction.ToCodeFunctionDTO(item.CodeFunction, withDetails);
            }


            result.ID = item.ID;
            result.EntityID = item.TableDrivedEntityID;
            result.Step = (Enum_EntityActionActivityStep)item.StepType;
            result.ResultSensetive = item.ResultSensetive == true;
            //result.BackendActionActivityType = (Enum_ActionActivityType)item.ActivityType;
            result.Title = item.Title;
            
            
            return result;
        }

        
        public int UpdateBackendActionActivitys(BackendActionActivityDTO BackendActionActivity)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbBackendActionActivity = projectContext.BackendActionActivity.FirstOrDefault(x => x.ID == BackendActionActivity.ID);
                if (dbBackendActionActivity == null)
                    dbBackendActionActivity = new DataAccess.BackendActionActivity();



                if (BackendActionActivity.CodeFunctionID != 0)
                    dbBackendActionActivity.CodeFunctionID = BackendActionActivity.CodeFunctionID;
                else
                    dbBackendActionActivity.CodeFunctionID = null;

                if (BackendActionActivity.DatabaseFunctionID != 0)
                    dbBackendActionActivity.DatabaseFunctionID = BackendActionActivity.DatabaseFunctionID;
                else
                    dbBackendActionActivity.DatabaseFunctionID = null;



                dbBackendActionActivity.ID = BackendActionActivity.ID;
                dbBackendActionActivity.Type = (short)BackendActionActivity.Type;
                //dbBackendActionActivity.ActivityType = (short)BackendActionActivity.BackendActionActivityType;
                dbBackendActionActivity.Title = BackendActionActivity.Title;
                dbBackendActionActivity.TableDrivedEntityID = BackendActionActivity.EntityID;
                dbBackendActionActivity.StepType = (short)BackendActionActivity.Step;
                dbBackendActionActivity.ResultSensetive = BackendActionActivity.ResultSensetive;
                if (dbBackendActionActivity.ID == 0)
                    projectContext.BackendActionActivity.Add(dbBackendActionActivity);
                projectContext.SaveChanges();
                return dbBackendActionActivity.ID;
            }
        }

        public void DeleteBackendActionActivity(int iD)
        {
            throw new NotImplementedException();
        }
    }
    public enum Enum_GetBackendActionActivityType
    {
        CodeFunctionOrDatabaseFunction,
        UIActions,
        All
    }

}
