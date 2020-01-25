using MyInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTestImplLibrary
{
  public  class CommandTest
    {
        public CommandFunctionResult ArchiveCommand(CommandFunctionParam Param)
        {
            CommandFunctionResult result = new CommandFunctionResult();
            //////Param.EditEntityArea.ClearData(true);
            //var column = Param.DataItem.Properties.FirstOrDefault(x => x.ColumnID == 32);
            //var control= new UserControl1();
            //if (column != null)
            //    control.button.Content = column.Value;
            //result.Result = control;
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
