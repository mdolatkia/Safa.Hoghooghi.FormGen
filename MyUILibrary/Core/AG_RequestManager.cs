using MyDataManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Reporting;

namespace MyUILibrary
{
    public class AG_RequestManager /*: IAG_RequestManager*/
    {
        RequestRegistrationService requestRegistration = new RequestRegistrationService();
        //public AG_InternalTaskResult RegisterRequest(BaseRequest request)
        //{
        //    //AG_InternalTaskResult result = new AG_InternalTaskResult();
        //    //var sendResult = SendRequest(request);
        //    //result.Result = true;
        //    //return result;
        //}

        public AG_InternalTaskResult CheckRequestPermission(BaseRequest request)
        {
            return null;
        }

        public AG_InternalTaskResult CheckRequestValidation(BaseRequest request)
        {
            return null;
        }

        public BaseResult SendRequest(BaseRequest request)
        {
            //var resigerResult = RegisterRequest(request);
            //if (resigerResult.Result)
            //{

            //    var permissionResult = CheckRequestPermission(request);
            //    if (permissionResult.Result)
            //    {
            //        //var bizRequestProcessor = new MyDataManagerBusiness.BizRequestProcessor();
            //        var validationResult = CheckRequestValidation(request);
            //        if (validationResult.Result)
            //        {
            //            //if(request.RequestExecutionTime.Any(x=>x.EnumType==Enum_DR_ExecutionTime.Now))
            //            //{

            var processResult = Process(request);
            //var registerResultResult = RegisterResult(processResult);
            return processResult;
            //            var registerResultResult = RegisterResult(processResult);
            //            return processResult;
            //            //}
            //        }
            //    }

            //}


            //return requestRegistration.RecieveRequest(request);
        }

        private BaseResult Process(BaseRequest request)
        {
            //DR_ResultRequest result = new DR_ResultRequest();

            if (request is DR_EditRequest)
            {
                return requestRegistration.SendEditRequest(request as DR_EditRequest);
            }
            else if (request is DR_DeleteInquiryRequest)
            {
                return requestRegistration.SendDeleteInquiryRequest(request as DR_DeleteInquiryRequest);
            }
            else if (request is DR_DeleteRequest)
            {
                return requestRegistration.SendDeleteRequest(request as DR_DeleteRequest);
            }
            //else if (request is DR_SearchViewRequest)
            //{
            //    return ;
            //}
            else if (request is DR_SearchExistsRequest)
            {
                return requestRegistration.SendSearchExistsRequest(request as DR_SearchExistsRequest);
            }
            else if (request is DR_SearchCountRequest)
            {
                return requestRegistration.SendSearchCountRequest(request as DR_SearchCountRequest);
            }
            else if (request is DR_SearchEditRequest)
            {
                return requestRegistration.SendSearchEditRequest(request as DR_SearchEditRequest);
            }
            else if (request is DR_SearchFullDataRequest)
            {
                return requestRegistration.SendSearchFullDataRequest(request as DR_SearchFullDataRequest);
            }
            else if (request is DR_SearchKeysOnlyRequest)
            {
                return requestRegistration.SendSearchKeysOnlyRequest(request as DR_SearchKeysOnlyRequest);
            }
            //else if (request is DR_SearchByRelationshipTailRequest)
            //{
            //    return requestRegistration.SendSearchByRelationshipTail(request as DR_SearchByRelationshipTailRequest);
            //}
            else if (request is RR_ReportSourceRequest)
            {
                UriReportSource rpSource = new UriReportSource();
                rpSource.Uri = "http://localhost:25667/api/myreport";
                return new RR_ReportSourceResult() { ReportSource = rpSource };
                //rpSource.
                //return requestRegistration.SendReportSourceRequest(request as RR_ReportSourceRequest);
            }
            //else if (request is DR_SearchByRelationViewRequest)
            //{
            //    SearchRequestProcessor editProcessor = new SearchRequestProcessor();
            //    return editProcessor.Process(request as DR_SearchByRelationViewRequest);
            //}
            throw (new Exception("چنین درخواستی قابل رسیدگی نمیباشد"));
        }

   

        //public void ProcessResult(DR_ResultRequest result)
        //{

        //}

        //public DR_ResultEdit SendEditRequest(DR_EditRequest request)
        //{
        //    throw new NotImplementedException();
        //}

        //public DR_ResultDelete SendDeleteRequest(DR_DeleteRequest request)
        //{
        //    throw new NotImplementedException();
        //}

        //public DR_ResultSearchView SendSearchViewRequest(DR_SearchViewRequest request)
        //{
        //    throw new NotImplementedException();
        //}

        //public DR_ResultSearchEdit SendSearchEditRequest(DR_SearchEditRequest request)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
