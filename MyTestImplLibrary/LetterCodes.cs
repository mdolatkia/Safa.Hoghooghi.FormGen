using MyInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTestImplLibrary
{
    class LetterCodes
    {
        public CodeFunctionResult BeforeLoad(LetterFunctionParam Param)
        {
            CodeFunctionResult result = new CodeFunctionResult();
            Param.Letter.Title += "_a";
            return result;
        }
        public CodeFunctionResult BeforeSave(LetterFunctionParam Param)
        {
            CodeFunctionResult result = new CodeFunctionResult();
            //Param.Letter.Title += "_a";
            return result;
        }
        public CodeFunctionResult AfterSave(LetterFunctionParam Param)
        {
            CodeFunctionResult result = new CodeFunctionResult();
            //Param.Letter.Title += "_a";
            return result;
        }
        public CodeFunctionResult ExternalCode(LetterFunctionParam Param)
        {
            CodeFunctionResult result = new CodeFunctionResult();
            Param.Letter.Title = " عنوان خارجی";
            Param.Letter.LetterDate = DateTime.Now - TimeSpan.FromDays(1);
            Param.Letter.AttechedFile = new ModelEntites.FileRepositoryDTO();
            var filePath = "D:\\testExternalSource.doc";
            Param.Letter.AttechedFile.FileExtension = System.IO.Path.GetExtension(filePath).Replace(".", "");
                   Param.Letter.AttechedFile.FileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
            Param.Letter.AttechedFile.Content = System.IO.File.ReadAllBytes(filePath);
            return result;
        }


        public LetterConvertToExternalResult ConvertToExternal(LetterFunctionParam Param)
        {
            LetterConvertToExternalResult result = new LetterConvertToExternalResult();
            result.ExternalCode = "1111";
            result.Result = true;
            return result;
        }
    }
}
