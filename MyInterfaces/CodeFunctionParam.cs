using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyInterfaces
{
    public class BaseCodeFunctionParam
    {
        public BaseCodeFunctionParam(DR_Requester requester)
        {
            Requester = requester;
        }
        public DR_Requester Requester { set; get; }
    }
    public class CodeFunctionParamManyDataItems : BaseCodeFunctionParam
    {
        public CodeFunctionParamManyDataItems(List<DP_DataRepository> dataItems, DR_Requester requester):base(requester)
        {
            DataItems = dataItems;
        }
        public List<DP_DataRepository> DataItems { set; get; }
        public object[] OtherParams { set; get; }
    }
    public class CodeFunctionParamOneDataItem : BaseCodeFunctionParam
    {
        public CodeFunctionParamOneDataItem(DP_DataRepository dataItem, DR_Requester requester) : base(requester)
        {
            DataItem = dataItem;
        }

        public DP_DataRepository DataItem { set; get; }
        public object[] OtherParams { set; get; }
    }
    public class CodeFunctionParamKeyColumns : BaseCodeFunctionParam
    {
        public CodeFunctionParamKeyColumns(int entityID, List<EntityInstanceProperty> properties, DR_Requester requester) : base(requester)
        {
            EntityID = entityID;
            Properties = properties;
        }
        public int EntityID { set; get; }
        public List<EntityInstanceProperty> Properties { set; get; }
        public object[] OtherParams { set; get; }
    }
}
