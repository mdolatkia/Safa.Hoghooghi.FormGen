using ProxyLibrary;
using System;
using System.Collections.Generic;
namespace ProxyLibrary
{
	public interface IRequestProcessor {


		InternalTaskResult CheckRequestValidation(BaseRequest request);
        BaseResult ProcessRequest(BaseRequest request);

	}

}
