using DataAccess;
using ModelEntites;
using MyDataSearchManagerBusiness;
using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDataItemManager
{
    public class BizDataItem
    {
        public bool SetDataItemDTO(DP_BaseData dataItem)
        {
            if (dataItem.DataItemID != 0)
            {
                return true;
            }
            else
            {
                using (var model = new MyIdeaDataDBEntities())
                {
                    var dbDataItem = GetDBDataItem(model, dataItem.TargetEntityID, dataItem.KeyProperties);
                    if (dbDataItem != null)
                    {
                        dataItem.DataItemID = dbDataItem.ID;
                        return true;
                    }
                    else
                        return false;
                }
            }
        }
        //public DataItemDTO GetDataItem(int tableDrivedEntityID, List<EntityInstanceProperty> keyProperties)
        //{
        //    using (var model = new MyIdeaDataDBEntities())
        //    {
        //        var dbDataItem = GetDBDataItem(model, tableDrivedEntityID, keyProperties);
        //        if (dbDataItem != null)
        //            return ToDataItemDTO(dbDataItem);
        //        else return null;
        //    }

        //}

        public int GetOrCreateDataItem(DP_BaseData dataItem)
        {

            using (var model = new MyIdeaDataDBEntities())
            {
                var MyDataItem = GetDBDataItem(model, dataItem.TargetEntityID, dataItem.KeyProperties);
                if (MyDataItem == null)
                {


                    //if(dataItem.DataView!=null)
                    //{
                    //    foreach(var item in dataItem.DataView.Properties)
                    //    {
                    //        info += (info == "" ? "" : ",") + item.Value;
                    //    }
                    //}
                    //else
                    //{
                    //    foreach (var item in dataItem.KeyProperties)
                    //    {
                    //        info += (info == "" ? "" : ",") + item.Value;
                    //    }
                    //}
                    MyDataItem = new MyDataItem();
                    MyDataItem.TableDrivedEntityID = dataItem.TargetEntityID;
                    if (dataItem is DP_DataView)
                        MyDataItem.Info = (dataItem as DP_DataView).ViewInfo;
                    else if (dataItem is DP_DataRepository)
                        MyDataItem.Info = (dataItem as DP_DataRepository).ViewInfo;

                    foreach (var keyColumn in dataItem.KeyProperties)
                        MyDataItem.MyDataItemKeyColumns.Add(new MyDataItemKeyColumns() { ColumnID = keyColumn.ColumnID, Value = keyColumn.Value==null?null:keyColumn.Value.ToString() });
                    model.MyDataItem.Add(MyDataItem);
                    model.SaveChanges();
                }
                return MyDataItem.ID;
            }


        }


        //public DataItemDTO ToDataItemDTO(MyDataItem dbDataItem)
        //{
        //    var result = new DataItemDTO();
        //    result.ID = dbDataItem.ID;
        //    result.TableDrivedEntityID = dbDataItem.TableDrivedEntityID;
        //    result.KeyProperties = new List<EntityInstanceProperty>();
        //    foreach (var item in dbDataItem.MyDataItemKeyColumns)
        //        result.KeyProperties.Add(new EntityInstanceProperty(null) { ColumnID = item.ColumnID, Value = item.Value });
        //    return result;
        //}
        SearchRequestManager searchRequestManager = new SearchRequestManager();

        public DP_DataView ToDataViewDTO(DR_Requester requester, MyDataItem dbDataItem, bool lastInfo)
        {
            DP_DataView result = null;
            bool error = false;
            if (lastInfo)
            {
                DP_SearchRepository searchDataItem = new DP_SearchRepository(dbDataItem.TableDrivedEntityID);
                foreach (var property in dbDataItem.MyDataItemKeyColumns)
                    searchDataItem.Phrases.Add(new SearchProperty() { ColumnID = property.ColumnID, Value = property.Value });
                DR_SearchViewRequest request = new DR_SearchViewRequest(requester, searchDataItem);
                var searchResult = searchRequestManager.Process(request);
                if (searchResult.Result == Enum_DR_ResultType.SeccessfullyDone)
                    result = searchResult.ResultDataItems.FirstOrDefault();
                else if (searchResult.Result == Enum_DR_ResultType.ExceptionThrown)
                {
                    error = true;
                }
            }

            if (!lastInfo || error)
            {
                result = new DP_DataView(dbDataItem.TableDrivedEntityID, "");
                result.DataItemID = dbDataItem.ID;
                //result.TargetEntityID = dbDataItem.TableDrivedEntityID;
                //   List<EntityInstanceProperty> listProperties = new List<EntityInstanceProperty>();
                BizColumn bizColumn = new BizColumn();

                foreach (var property in dbDataItem.MyDataItemKeyColumns)
                {
                    result.Properties.Add(new EntityInstanceProperty(bizColumn.GetColumn(property.ColumnID, true))
                    {
                        Value = property.Value
                    });
                }

            }
            return result;
            //result.SetProperties(listProperties);

        }

        private DR_Requester GetRequester()
        {
            return new DR_Requester();// { SkipSecurity = true };
        }

        //public DP_DataView ToDataViewDTO(MyDataItem dbDataItem)
        //{
        //    var result = new DP_DataView();
        //    result.DataItemID = dbDataItem.ID;
        //    result.TargetEntityID = dbDataItem.TableDrivedEntityID;
        //    //   List<EntityInstanceProperty> listProperties = new List<EntityInstanceProperty>();
        //    BizColumn bizColumn = new BizColumn();

        //    foreach (var item in dbDataItem.MyDataItemKeyColumns)
        //    {
        //        result.Properties.Add(new EntityInstanceProperty(bizColumn.GetColumn(item.ColumnID, true), item.Value)));
        //    }
        //    //result.SetProperties(listProperties);
        //    return result;
        //}
        //private MyDataItem GetDBDataItem(int ID)
        //{

        //}

        public int GetDataItemID(int tableDrivedEntityID, List<EntityInstanceProperty> keyProperties)
        {
            using (var context = new MyIdeaDataDBEntities())
            {
                var MyDataItem = GetDBDataItem(context, tableDrivedEntityID, keyProperties);
                if (MyDataItem == null)
                    return 0;
                else
                    return MyDataItem.ID;
            }

        }
        private MyDataItem GetDBDataItem(MyIdeaDataDBEntities context, int tableDrivedEntityID, List<EntityInstanceProperty> keyProperties)
        {

            MyDataItem MyDataItem;

            var dataItems = context.MyDataItem.Where(x => x.TableDrivedEntityID == tableDrivedEntityID);
            foreach (var keyColumn in keyProperties)
                dataItems = dataItems.Where(x => x.MyDataItemKeyColumns.Any(y => y.ColumnID == keyColumn.ColumnID && y.Value == keyColumn.Value));
            MyDataItem = dataItems.FirstOrDefault();
            return MyDataItem;

        }




    }
}
