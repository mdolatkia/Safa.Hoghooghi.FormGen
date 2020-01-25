using ModelEntites;
using MyCodeFunctionLibrary;
using MyConnectionManager;
using MyDatabaseFunctionLibrary;
using MyDataItemManager;
using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDataEditManagerBusiness
{
    public class EditDataFormulaUsageManager
    {
        internal void UpdateFormulaUsage(List<QueryItem> allQueryItems, InternaleEditResult result)
        {
            BizDataItem bizDataItem = new BizDataItem();
            BizFormulaUsage bizFormulaUsage = new BizFormulaUsage();
            foreach (var editQuertyResult in result.EditQueryResults)
            {
                try
                {
                    var queryItem = editQuertyResult.QueryItem;
                    if (queryItem.DataItem.GetProperties().Any(x => x.FormulaID != 0))
                    {
                        var dataItemID = bizDataItem.GetOrCreateDataItem(queryItem.DataItem);
                        foreach (var property in queryItem.DataItem.GetProperties().Where(x => x.FormulaID != 0))
                        {
                            FormulaUsageDTO usage = new FormulaUsageDTO();
                            usage.DataItemID = dataItemID;
                            usage.ColumnID = property.ColumnID;
                            usage.FormulaID = property.FormulaID;
                            usage.FormulaUsageParemeters = property.FormulaUsageParemeters;
                            bizFormulaUsage.UpdateFormulaUsage(usage);
                        }
                    }
                }
                catch(Exception ex)
                {
                    editQuertyResult.ForumulaUsageResult = Enum_DR_ResultType.ExceptionThrown;
                    editQuertyResult.ForumulaUsageMessage = ex.Message;
                }


            }
        }
    }
}
