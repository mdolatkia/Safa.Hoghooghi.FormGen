
using ModelEntites;
using MyCodeFunctionLibrary;
using MyDatabaseFunctionLibrary;
using MyDataEditManagerBusiness;
using MyDataItemManager;
using MyDataSearchManagerBusiness;

using MyFormulaFunctionStateFunctionLibrary;
using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDataRequestManagerBusiness
{
    public class RequestManager
    {
        //public BaseResult ProcessRequest(BaseRequest request)
        //{
        //    //DR_ResultRequest result = new DR_ResultRequest();
        //    //var resigerResult = RegisterRequest(request);

        //    var permissionResult = CheckRequestPermission(request);
        //    if (permissionResult.Result)
        //    {
        //        //var bizRequestProcessor = new MyDataManagerBusiness.BizRequestProcessor();
        //        var validationResult = CheckRequestValidation(request);
        //        if (validationResult.Result)
        //        {
        //            //if(request.RequestExecutionTime.Any(x=>x.EnumType==Enum_DR_ExecutionTime.Now))
        //            //{
        //            var processResult = Process(request);
        //            //var registerResultResult = RegisterResult(resigerResult, processResult);
        //            return processResult;
        //            //}
        //        }
        //    }

        //    return null;
        //}



        private BaseResult Process(BaseRequest request)
        {

            if (request is DR_EditRequest)
            {

               

                return result;
            }
            else if (request is DR_DeleteInquiryRequest)
            {
            
            }
            else if (request is DR_DeleteRequest)
            {
            
            }
            else if (request is DR_SearchViewRequest)
            {
              
            }
            else if (request is DR_SearchExistsRequest)
            {
              
            }
            else if (request is DR_SearchCountRequest)
            {
             
            }
            else if (request is DR_SearchEditRequest)
            {
            
            }
            else if (request is DR_SearchFullDataRequest)
            {
               
            }
            else if (request is DR_SearchKeysOnlyRequest)
            {
              
            }
            else if (request is DR_SearchByRelationshipTailRequest)
            {
              
            }
            //else if (request is RR_ReportSourceRequest)
            //{
            //    BizReportManager bizReportManager = new BizReportManager();
            //    return bizReportManager.GetReportSource(request as RR_ReportSourceRequest);
            //}
            //else if (request is DR_SearchByRelationViewRequest)
            //{
            //    SearchRequestProcessor editProcessor = new SearchRequestProcessor();
            //    return editProcessor.Process(request as DR_SearchByRelationViewRequest);
            //}
            throw (new Exception("چنین درخواستی قابل رسیدگی نمیباشد"));
        }


        private InternalTaskResult CheckRequestValidation(BaseRequest request)
        {
            InternalTaskResult result = new InternalTaskResult();
            result.Result = true;
            return result;
        }

        //LogHelper logHelper = new LogHelper();
        //public RequestRegisterResult RegisterRequest(BaseRequest request)
        //{
        //    RequestRegisterResult result = new RequestRegisterResult();

        //    if (request is DR_EditRequest)
        //    {
        //        List<MainLogDTO> logList = ToListMainLogDTO(request as DR_EditRequest);
        //        logHelper.InsertRequestLog(logList);
        //        result.LogGuidPairs = logList.Select(x => new LogGuidPair(x.ID, x.GUID)).ToList();
        //    }
        //    else if (request is DR_DeleteRequest)
        //    {
        //        List<MainLogDTO> logList = ToListMainLogDTO(request as DR_DeleteRequest);
        //        logHelper.InsertRequestLog(logList);
        //        result.LogGuidPairs = logList.Select(x => new LogGuidPair(x.ID, x.GUID)).ToList();
        //    }
        //    result.Result = true;


        //    return result;
        //}
        //private List<MainLogDTO> ToListMainLogDTO(DR_DeleteRequest dR_EditRequest)
        //{
        //    List<MainLogDTO> result = new List<MainLogDTO>();
        //    if (dR_EditRequest.Identity == Guid.Empty)
        //        dR_EditRequest.Identity = Guid.NewGuid();
        //    foreach (var item in dR_EditRequest.DataItems)
        //    {
        //        MainLogDTO logItem = ToDeleteMainLogDTO(item);
        //        logItem.Type = LogType.DeleteData;
        //        logItem.PackageGuid = dR_EditRequest.Identity;
        //        if (item.GUID == Guid.Empty)
        //            item.GUID = new Guid();
        //        logItem.GUID = item.GUID;
        //        SetBasicRequestLogInfo(dR_EditRequest as BaseRequest, logItem);
        //        result.Add(logItem);
        //    }
        //    return result;
        //}
        //private MainLogDTO ToDeleteMainLogDTO(DP_DataRepository item)
        //{
        //    MainLogDTO result = new MainLogDTO();
        //    result.EnityID = item.TargetEntityID;

        //    foreach (var column in item.KeyProperties)
        //    {
        //        var keyColumn = new EntityKeyColumnDTO();
        //        keyColumn.ColumnID = column.ColumnID;
        //        keyColumn.Value = column.Value;
        //        result.KeyColumns.Add(keyColumn);
        //    }
        //    return result;
        //}
        //private List<MainLogDTO> ToListMainLogDTO(DR_EditRequest dR_EditRequest)
        //{
        //    List<MainLogDTO> result = new List<MainLogDTO>();
        //    if (dR_EditRequest.Identity == Guid.Empty)
        //        dR_EditRequest.Identity = Guid.NewGuid();
        //    foreach (var item in dR_EditRequest.EditPackages)
        //    {
        //        MainLogDTO logItem = ToDataEntryMainLogDTO(item);
        //        if (item.IsNewItem)
        //            logItem.Type = LogType.InsertData;
        //        else
        //            logItem.Type = LogType.UpdateData;
        //        logItem.PackageGuid = dR_EditRequest.Identity;
        //        if (item.GUID == Guid.Empty)
        //            item.GUID = new Guid();
        //        logItem.GUID = item.GUID;
        //        SetBasicRequestLogInfo(dR_EditRequest as BaseRequest, logItem);
        //        result.Add(logItem);
        //    }
        //    return result;
        //}

        //private MainLogDTO ToDataEntryMainLogDTO(DP_DataRepository item)
        //{
        //    MainLogDTO result = new MainLogDTO();
        //    result.EnityID = item.TargetEntityID;

        //    foreach (var column in item.GetProperties())
        //    {
        //        var logColumn = new DataEntryColumnDTO();
        //        logColumn.ColumnID = column.ColumnID;
        //        logColumn.Value = column.Value;
        //        result.DataEntryColumns.Add(logColumn);
        //    }
        //    foreach (var column in item.KeyProperties)
        //    {
        //        var keyColumn = new EntityKeyColumnDTO();
        //        keyColumn.ColumnID = column.ColumnID;
        //        keyColumn.Value = column.Value;
        //        result.KeyColumns.Add(keyColumn);
        //    }
        //    return result;
        //}
        //private void SetBasicRequestLogInfo(BaseRequest BaseRequest, MainLogDTO logItem)
        //{
        //    logItem.UserID = BaseRequest.Requester.Identity;
        //    logItem.LocationInfo = BaseRequest.Requester.LocationInfo;
        //}



        public InternalTaskResult CheckRequestPermission(BaseRequest request)
        {
            InternalTaskResult result = new InternalTaskResult();
            result.Result = true;
            return result;
        }


        //InternalTaskResult RegisterRequestForProcess(BaseRequest request)
        //{
        //    throw new NotImplementedException();
        //}




        //private InternalTaskResult RegisterResult(RequestRegisterResult resigerResult, BaseResult processResult)
        //{
        //    InternalTaskResult result = new InternalTaskResult();

        //    if (processResult is DR_ResultEdit)
        //    {
        //        List<MainLogDTO> logList = ToUpdateListMainLogDTO(resigerResult, processResult as DR_ResultEdit);
        //        logHelper.UpdateResponseLog(logList);
        //    }
        //    else if (processResult is DR_ResultDelete)
        //    {
        //        List<MainLogDTO> logList = ToUpdateListMainLogDTO(resigerResult, processResult as DR_ResultDelete);
        //        logHelper.UpdateResponseLog(logList);
        //    }
        //    result.Result = true;


        //    return result;
        //}
        //private List<MainLogDTO> ToUpdateListMainLogDTO(RequestRegisterResult resigerResult, DR_ResultDelete dR_ResultEdit)
        //{
        //    List<MainLogDTO> result = new List<MainLogDTO>();

        //    foreach (var item in resigerResult.LogGuidPairs)
        //    {
        //        //var requestRegister = resigerResult.LogGuidPairs.FirstOrDefault(x => x.GUID == item.GUID);
        //        //if (requestRegister != null)
        //        //{
        //        MainLogDTO logItem = new MyLogManager.MainLogDTO();
        //        logItem.ID = item.LogID;
        //        logItem.Succeed = true;
        //        //خطا؟

        //        result.Add(logItem);
        //        //}
        //    }
        //    return result;
        //}
        //private List<MainLogDTO> ToUpdateListMainLogDTO(RequestRegisterResult resigerResult, DR_ResultEdit dR_ResultEdit)
        //{
        //    List<MainLogDTO> result = new List<MainLogDTO>();

        //    foreach (var item in dR_ResultEdit.DR_ResultEditTuples)
        //    {
        //        var requestRegister = resigerResult.LogGuidPairs.FirstOrDefault(x => x.GUID == item.GUID);
        //        if (requestRegister != null)
        //        {
        //            MainLogDTO logItem = new MyLogManager.MainLogDTO();
        //            logItem.ID = requestRegister.LogID;
        //            logItem.Succeed = true;
        //            //خطا؟

        //            result.Add(logItem);
        //        }
        //    }
        //    return result;
        //}

        //public IRequestProcessor GetProperProcessor(DR_Request request)
        //{
        //    return new MyDataManagerBusiness.BizRequestProcessor();
        //}


        //public bool DoAfterSaveActionActivities(List<DP_DataRepository> specificDate)
        //{

        //    //بازدارنده بودن کنترل شود
        //    //////var entityID = specificDate.FirstOrDefault(x => !x.ParentRelationInfos.Any()).TargetEntityID;
        //    //////if ( entityID != 0)
        //    //////{
        //    //////    BizActionActivity bizActionActivity = new BizActionActivity();
        //    //////    var actionActivities = bizActionActivity.GetActionActivities(entityID, new List<Enum_ActionActivityType>()
        //    //////    , new List<Enum_EntityActionActivityStep> { Enum_EntityActionActivityStep.AfterSave }, true);
        //    //////    foreach (var actionActivity in actionActivities)
        //    //////    {
        //    //////        if (actionActivity.CodeFunctionID != 0)
        //    //////        {
        //    //////            CodeFunctionHandler handler = new CodeFunctionHandler();
        //    //////            var res = handler.GetCodeFunctionResult(actionActivity.CodeFunctionID, specificDate);
        //    //////        }
        //    //////        else if (actionActivity.DatabaseFunctionID != 0)
        //    //////        {
        //    //////            DatabaseFunctionHandler handler = new DatabaseFunctionHandler();
        //    //////            foreach (var data in specificDate)
        //    //////            {
        //    //////                var res = handler.GetDatabaseFunctionValue(actionActivity.DatabaseFunctionID, data);
        //    //////            }
        //    //////        }
        //    //////    }
        //    //////}
        //    return true;
        //}

        //private bool DoStateActionActivities(BaseRequest request, UIActionActivityDTO actionActivity, DP_DataRepository specificDate)
        //{
        //    if (actionActivity.CodeFunctionID != 0)
        //    {
        //        CodeFunctionHandler handler = new CodeFunctionHandler();
        //        var res = handler.GetCodeFunctionResult(request.Requester, actionActivity.CodeFunctionID, specificDate);
        //        //if (result.ExecutionException == null)
        //        //{
        //        //    if (result.ExceptionWithMessage == false)
        //        //    {
        //        //اعمال شود
        //    }
        //    else if (actionActivity.DatabaseFunctionID != 0)
        //    {
        //        DatabaseFunctionHandler handler = new DatabaseFunctionHandler();
        //        var res = handler.GetDatabaseFunctionValue(request.Requester, actionActivity.DatabaseFunctionID, specificDate);
        //    }
        //    return true;
        //}
        //private void AddDataItemState(List<DataItemState> dataItemsStates, DP_DataRepository item, EntityStateDTO state)
        //{
        //    var dataItem = dataItemsStates.FirstOrDefault(x => x.DataItem == item);
        //    if (dataItem == null)
        //    {
        //        dataItem = new DataItemState();
        //        dataItem.DataItem = item;
        //        dataItemsStates.Add(dataItem);
        //    }
        //    if (!dataItem.States.Any(x => x.ID == state.ID))
        //        dataItem.States.Add(state);
        //}
    }
}
