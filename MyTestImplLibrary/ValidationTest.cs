using MyInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTestImplLibrary
{
    public class ValidationTest
    {
        public CodeFunctionResult ValidateLocation(CodeFunctionParamOneDataItem data)
        {
            CodeFunctionResult result = new CodeFunctionResult();
               //  var data = param.DataItems.First();
               var address=data.DataItem.GetProperties().FirstOrDefault(x => x.Name == "Address");
            if (address.Value == "999")
                result.Result = false;
            else
                result.Result = true;
            return result;
        }
    }
}
