using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGeneralLibrary
{
    public static class GeneralExceptionManager
    {
        public static string GetExceptionMessage(Exception exception)
        {

            if (exception.InnerException == null)
                return exception.Message ?? "";
            else
            {
                var message = exception.Message ?? "";
                var innerMessage = exception.InnerException.Message ?? "";
                return message + Environment.NewLine + innerMessage;
            }
        }
    }
}
