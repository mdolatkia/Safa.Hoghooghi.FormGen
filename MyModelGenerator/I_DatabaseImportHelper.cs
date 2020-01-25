using ModelEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelGenerator
{
  public  interface I_DatabaseImportHelper
    {
        ColumnValueRangeDTO GetColumnValueRange(int columnID);
        event EventHandler<ItemImportingStartedArg> ItemImportingStarted;
        //event EventHandler<ImportCompletedArg> ImportCompleted;
        List<TableImportItem> GetTablesAndColumnInfo();
        List<RelationshipImportItem> GetRelationships();
        ImportResult GenerateUniqueConstraints();
        List<FunctionImportItem> GetFunctions();
        List<TableImportItem> GetViewsInfo();
    }

 

    //public class ImportCompletedArg : EventArgs
    //{
    //    public string TaskName;
      
    //}
}
