using MyInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTestImplLibrary
{
    public class EntityCommandTest
    {
        public CodeFunctionResult TestCommand(CodeFunctionParamOneDataItem data)
        {
            CodeFunctionResult result = new CodeFunctionResult();
            System.Windows.MessageBox.Show("asdasd");
            return result;
        }
    }
}
