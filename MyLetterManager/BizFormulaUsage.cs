using DataAccess;
using ModelEntites;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDataItemManager
{
    public class BizFormulaUsage
    {
        //public List<FormulaUsageDTO> GetFormulaUsages(int dataItemID)
        //{
        //    List<FormulaUsageDTO> result = new List<FormulaUsageDTO>();
        //    using (var projectContext = new DataAccess.MyIdeaDataDBEntities())
        //    {
        //        var Formulas = projectContext.FormulaUsage.Where(x => x.MyDataItemID == dataItemID);
        //        foreach (var item in Formulas)
        //            result.Add(ToFormulaUsageDTO(item));
        //        return result;
        //    }
        //}

        //private FormulaUsageDTO ToFormulaUsageDTO(FormulaUsage item)
        //{
        //    FormulaUsageDTO result = new FormulaUsageDTO();
        //    result.DataItemID = item.MyDataItemID;
        //    result.FormulaID = item.FormulaID;
        //    result.ColumnID = item.ColumnID;
        //    result.ID = item.ID;
        //    foreach (var usageParamter in item.FormulaUsageParemeters)
        //    {
        //        result.FormulaUsageParemeters.Add(AddUsageParameterDTO(usageParamter));

        //    }
        //    return result;
        //}

        //private FormulaUsageParemetersDTO AddUsageParameterDTO(FormulaUsageParemeters usageParamter)
        //{
        //    FormulaUsageParemetersDTO pItem = new FormulaUsageParemetersDTO();
        //    pItem.ParameterName = usageParamter.ParameterName;
        //    pItem.ParameterValue = usageParamter.ParameterValue;
        //    pItem.RelationshipPropertyTail = usageParamter.RelationshipKeyColumnTail;
        //    foreach (var cItem in usageParamter.FormulaUsageParemeters1)
        //    {
        //        pItem.ChildItems.Add(AddUsageParameterDTO(cItem));
        //    }
        //    return pItem;
        //}


        //public void UpdateFormulaUsage(FormulaUsageDTO usage)
        //{
        //    using (var projectContext = new DataAccess.MyIdeaDataDBEntities())
        //    {
        //        var dbUsage = projectContext.FormulaUsage.FirstOrDefault(x => x.MyDataItemID == usage.DataItemID && x.ColumnID == usage.ColumnID);
        //        if (dbUsage == null)
        //            dbUsage = new DataAccess.FormulaUsage();
        //        dbUsage.MyDataItemID = usage.DataItemID;
        //        dbUsage.FormulaID = usage.FormulaID;
        //        dbUsage.ColumnID = usage.ColumnID;
        //        while (dbUsage.FormulaUsageParemeters.Any())
        //        {
        //            RemoveUsageParameterChilds(projectContext, dbUsage.FormulaUsageParemeters.First());
        //        }
        //        if (usage.FormulaUsageParemeters != null)
        //        {
        //            foreach (var parameter in usage.FormulaUsageParemeters)
        //            {
        //                AddUsageParameter(dbUsage.FormulaUsageParemeters, parameter);
        //            }
        //        }
        //        projectContext.FormulaUsage.Add(dbUsage);
        //        projectContext.SaveChanges();
        //    }
        //}

        //private void AddUsageParameter(ICollection<FormulaUsageParemeters> formulaUsageParemeters, FormulaUsageParemetersDTO parameter)
        //{
        //    FormulaUsageParemeters dbItem = new FormulaUsageParemeters();
        //    dbItem.ParameterName = parameter.ParameterName;
        //    dbItem.ParameterValue = parameter.ParameterValue;
        //    dbItem.RelationshipKeyColumnTail = parameter.RelationshipPropertyTail;
        //    formulaUsageParemeters.Add(dbItem);
        //    foreach (var cparameter in parameter.ChildItems)
        //    {
        //        AddUsageParameter(dbItem.FormulaUsageParemeters1, cparameter);
        //    }
        //}

        //private void RemoveUsageParameterChilds(DataAccess.MyIdeaDataDBEntities projectContext, FormulaUsageParemeters formulaUsageParemeters)
        //{
        //    while (formulaUsageParemeters.FormulaUsageParemeters1.Any())
        //    {
        //        RemoveUsageParameterChilds(projectContext, formulaUsageParemeters.FormulaUsageParemeters1.First());
        //    }
        //    projectContext.FormulaUsageParemeters.Remove(formulaUsageParemeters);
        //}
        //public void CheckFormulaUsages(DP_DataRepository data)
        //{
        //    BizDataItem bizDataItem = new MyDataItemManager.BizDataItem();
        //    var ex = bizDataItem.SetDataItemDTO(data);
        //    if (ex)
        //    {
        //        var formulaUsages = GetFormulaUsages(data.DataItemID);
        //        foreach (var item in formulaUsages)
        //        {
        //            var prop = data.GetProperties().FirstOrDefault(x => x.ColumnID == item.ColumnID);
        //            if (prop != null)
        //            {
        //                prop.FormulaID = item.FormulaID;
        //                prop.FormulaUsageParemeters = item.FormulaUsageParemeters;
        //            }
        //        }
        //    }

        //}
    }
}
