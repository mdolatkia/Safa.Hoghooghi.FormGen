using ModelEntites;
using MyCodeFunctionLibrary;
using MyConnectionManager;
using MyDatabaseFunctionLibrary;
using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDataEditManagerBusiness
{
    public class EditDataActionActivityManager
    {
        public void DoBeforeEditActionActivities(DR_Requester requester, List<EditQueryResultItem> items)
        {
            BizBackendBackendActionActivity bizActionActivity = new BizBackendBackendActionActivity();
            foreach (var editQuertyResult in items.Where(x => x.QueryItem.QueryType == Enum_QueryItemType.Insert || x.QueryItem.QueryType == Enum_QueryItemType.Update))
            {
                var queryItem = editQuertyResult.QueryItem;
                var actionActivities = bizActionActivity.GetActionActivities(queryItem.DataItem.TargetEntityID, new List<Enum_EntityActionActivityStep>() { Enum_EntityActionActivityStep.BeforeSave }, true);
                CodeFunctionHandler codeFunctionHelper = new CodeFunctionHandler();
                foreach (var entityActionActivity in actionActivities)
                {
                    if (entityActionActivity.CodeFunctionID != 0)
                    {
                        var resultFunction = codeFunctionHelper.GetCodeFunctionResult(requester, entityActionActivity.CodeFunctionID, queryItem.DataItem);
                        if (resultFunction.ExecutionException != null)
                        {
                            editQuertyResult.BeforeSaveActionActivitiesResult = Enum_DR_SimpleResultType.ExceptionThrown;
                            editQuertyResult.BeforeSaveActionActivitiesMessage += resultFunction.ExecutionException.Message;
                            return;
                        }
                    }
                }
                //foreach (var child in dataItem.ChildRelationshipInfos)
                //{
                //    DoBeforeSaveActionActivities(requester, child.RelatedData.ToList(), result);
                //    if (result.Result == Enum_DR_ResultType.ExceptionThrown)
                //        return;
                //}
            }

            foreach (var editQuertyResult in items.Where(x => x.QueryItem.QueryType == Enum_QueryItemType.Delete))
            {
                var queryItem = editQuertyResult.QueryItem;
                var actionActivities = bizActionActivity.GetActionActivities(queryItem.TargetEntity.ID, new List<Enum_EntityActionActivityStep>() { Enum_EntityActionActivityStep.BeforeDelete }, true);
                CodeFunctionHandler codeFunctionHelper = new CodeFunctionHandler();
                foreach (var entityActionActivity in actionActivities)
                {
                    if (entityActionActivity.CodeFunctionID != 0)
                    {
                        var resultFunction = codeFunctionHelper.GetCodeFunctionResult(requester, entityActionActivity.CodeFunctionID, queryItem.DataItem);
                        if (resultFunction.ExecutionException != null)
                        {

                            editQuertyResult.BeforeSaveActionActivitiesResult = Enum_DR_SimpleResultType.ExceptionThrown;
                            editQuertyResult.BeforeSaveActionActivitiesMessage += resultFunction.ExecutionException.Message;
                            return;
                        }
                    }
                }

            }
        }
        public void DoAfterEditActionActivities(DR_Requester requester, List<EditQueryResultItem> items)
        {
            BizBackendBackendActionActivity bizActionActivity = new BizBackendBackendActionActivity();
            foreach (var editQuertyResult in items.Where(x => x.QueryItem.QueryType == Enum_QueryItemType.Insert || x.QueryItem.QueryType == Enum_QueryItemType.Update))
            {
                var queryItem = editQuertyResult.QueryItem;
                if (queryItem.QueryType == Enum_QueryItemType.Insert || queryItem.QueryType == Enum_QueryItemType.Update)
                {
                    var actionActivities = bizActionActivity.GetActionActivities(queryItem.DataItem.TargetEntityID, new List<Enum_EntityActionActivityStep>() { Enum_EntityActionActivityStep.AfterSave }, true);
                    CodeFunctionHandler codeFunctionHelper = new CodeFunctionHandler();
                    DatabaseFunctionHandler databaseFunctionHandler = new DatabaseFunctionHandler();
                    foreach (var entityActionActivity in actionActivities)
                    {
                        if (entityActionActivity.CodeFunctionID != 0)
                        {
                            var resultFunction = codeFunctionHelper.GetCodeFunctionResult(requester, entityActionActivity.CodeFunctionID, queryItem.DataItem);
                            if (resultFunction.ExecutionException != null)
                            {

                                editQuertyResult.AfterSaveActionActivitiesResult = Enum_DR_SimpleResultType.ExceptionThrown;
                                editQuertyResult.AfterSaveActionActivitiesMessage += resultFunction.ExecutionException.Message;
                            }
                        }
                        else if (entityActionActivity.DatabaseFunctionID != 0)
                        {
                            ////اصلاح شود و با خصوصیات صدا زده شود یا حداقل لیست خصوصیات ارسال شود چون بهتره ارتباط 
                            var resultFunction = databaseFunctionHandler.GetDatabaseFunctionValue(requester, entityActionActivity.DatabaseFunctionID, queryItem.DataItem);
                            if (resultFunction.ExecutionException != null)
                            {

                                editQuertyResult.AfterSaveActionActivitiesResult = Enum_DR_SimpleResultType.ExceptionThrown;
                                editQuertyResult.AfterSaveActionActivitiesMessage += resultFunction.ExecutionException.Message;
                            }
                        }
                    }
                }
            }
            foreach (var editQuertyResult in items.Where(x => x.QueryItem.QueryType == Enum_QueryItemType.Delete))
            {
                var queryItem = editQuertyResult.QueryItem;
                var actionActivities = bizActionActivity.GetActionActivities(queryItem.TargetEntity.ID, new List<Enum_EntityActionActivityStep>() { Enum_EntityActionActivityStep.AfterDelete }, true);
                CodeFunctionHandler codeFunctionHelper = new CodeFunctionHandler();
                DatabaseFunctionHandler databaseFunctionHandler = new DatabaseFunctionHandler();
                foreach (var entityActionActivity in actionActivities)
                {
                    if (entityActionActivity.CodeFunctionID != 0)
                    {
                        var resultFunction = codeFunctionHelper.GetCodeFunctionResult(requester, entityActionActivity.CodeFunctionID, queryItem.DataItem);
                        if (resultFunction.ExecutionException != null)
                        {

                            editQuertyResult.AfterSaveActionActivitiesResult = Enum_DR_SimpleResultType.ExceptionThrown;
                            editQuertyResult.AfterSaveActionActivitiesMessage += resultFunction.ExecutionException.Message;
                        }
                    }
                    else if (entityActionActivity.DatabaseFunctionID != 0)
                    {
                        var resultFunction = databaseFunctionHandler.GetDatabaseFunctionValue(requester, entityActionActivity.DatabaseFunctionID, queryItem.DataItem);
                        if (resultFunction.ExecutionException != null)
                        {

                            editQuertyResult.AfterSaveActionActivitiesResult = Enum_DR_SimpleResultType.ExceptionThrown;
                            editQuertyResult.AfterSaveActionActivitiesMessage += resultFunction.ExecutionException.Message;
                        }
                    }
                }
            }
            //    var deleteResult = new DR_ResultDelete();
            //new DeleteRequestManager().DoAfterDeleteActionActivities(requester, queryItems.Where(x => x.QueryType == Enum_QueryItemType.Delete), deleteResult);
            //result.Message += deleteResult.Message;
        }
    }
}
