using MyInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTestImplLibrary
{
    public class Class1
    {
        public CodeFunctionResult GetValidMany(CodeFunctionParamManyDataItems Param)
        {
            CodeFunctionResult result = new CodeFunctionResult();

            result.Result = "y";
            try
            {
                Param.DataItems.First().GetProperties().Last().Value = "رشت";
            }
            catch
            {

            }
            return result;
        }

        public CodeFunctionResult GetValidOne(CodeFunctionParamOneDataItem Param)
        {
            CodeFunctionResult result = new CodeFunctionResult();

            result.Result = "y";
            try
            {
                Param.DataItem.GetProperties().Last().Value = "رشت";
            }
            catch
            {

            }
            return result;
        }
        //کمبو شود تا تابع با اسم یکسان بشود
        public CodeFunctionResult GetValidKeyColumn(int id, string date)
        {
            CodeFunctionResult result = new CodeFunctionResult();
            result.Result = date;
            //  result.Result = "y";
            try
            {
                //Param.DataItem.Properties.Last().Value = "رشت";
            }
            catch
            {

            }
            return result;
        }
    }
}
