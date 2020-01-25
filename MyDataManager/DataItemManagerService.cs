using ModelEntites;
using MyCodeFunctionLibrary;
using MyDataManagerBusiness;

using MyFormulaFunctionStateFunctionLibrary;
using MyInterfaces;
using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDataItemManager;
using MyLetterGenerator;

namespace MyDataManagerService
{
    public class DataItemManagerService
    {
        //BizDataItem bizLetterDataItem = new BizDataItem();

        BizDataItem bizDataItem = new BizDataItem();

        public bool SetDataItemDTO(DP_BaseData dataItem)
        {
            return bizDataItem.SetDataItemDTO(dataItem);
        }

        //public DataItemDTO GetExistingDataItem(int tableDrivedEntityID, List<EntityInstanceProperty> keyProperties)
        //{
        //    return bizLetterDataItem.GetDataItem(tableDrivedEntityID, keyProperties);
        //}

        //public FormulaDTO GetFormula(int formulaID)
        //{
        //    BizFormula bizFormula = new BizFormula();
        //    return bizFormula.GetFormula(formulaID);
        //}

    }
}
