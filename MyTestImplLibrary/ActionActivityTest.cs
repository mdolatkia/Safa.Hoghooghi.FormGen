using MyInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTestImplLibrary
{
    public class ActionActivityTest
    {
        public CodeFunctionResult BeforeLoad(CodeFunctionParamManyDataItems param)
        {
            var data = param.DataItems.First();
            data.GetProperty(53).Value = "aaaaaaa";
            return new CodeFunctionResult();
        }
        public CodeFunctionResult BeforeSave(CodeFunctionParamOneDataItem param)
        {
            var data = param.DataItem;
            data.GetProperty(53).Value = "aaaaaaa";
            return new CodeFunctionResult();
        }
        public CodeFunctionResult AfterSave(CodeFunctionParamOneDataItem param)
        {
            //var data = param.DataItems.First();
            //data.GetProperty(53).Value = "aaaaaaa";
            return new CodeFunctionResult();
        }
        public CodeFunctionResult BeforeDelete(CodeFunctionParamOneDataItem param)
        {
            
            return new CodeFunctionResult() { };
        }
        public CodeFunctionResult AfterDelete(CodeFunctionParamOneDataItem param)
        {
            //var data = param.DataItems.First();
            //data.GetProperty(53).Value = "aaaaaaa";
            return new CodeFunctionResult();
        }
    }
}
