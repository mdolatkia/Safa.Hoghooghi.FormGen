using DataAccess;
using ModelEntites;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyModelManager
{
    public class BizDataItem
    {
        public DataItemDTO GetDataItem(int ID)
        {
            var dbDataItem = GetDBDataItem(ID);
            if (dbDataItem != null)
                return ToDataItemDTO(dbDataItem);
            else return null;

        }
        public DataItemDTO GetDataItem(int tableDrivedEntityID, List<EntityInstanceProperty> keyProperties)
        {
            using (var model = new MyProjectEntities())
            {
                var dbDataItem = GetDBDataItem(model,tableDrivedEntityID, keyProperties);
                if (dbDataItem != null)
                    return ToDataItemDTO(dbDataItem);
                else return null;
            }

        }
        public DataItemDTO GetOrCreateDataItem(int tableDrivedEntityID, List<EntityInstanceProperty> keyProperties)
        {
            using (var model = new MyProjectEntities())
            {
                var dataItem = GetDBDataItem(model, tableDrivedEntityID, keyProperties);
                if (dataItem == null)
                {

                    dataItem = new DataItem();
                    dataItem.TableDrivedEntityID = tableDrivedEntityID;
                    foreach (var keyColumn in keyProperties)
                        dataItem.DataItemKeyColumns.Add(new DataItemKeyColumns() { ColumnID = keyColumn.ColumnID, Value = keyColumn.Value });
                    model.DataItem.Add(dataItem);
                    model.SaveChanges();
                }
                return ToDataItemDTO(dataItem);
            }
           
        }

        private DataItemDTO ToDataItemDTO(DataItem dbDataItem)
        {
            var result = new DataItemDTO();
            result.ID = dbDataItem.ID;
            result.TableDrivedEntityID = dbDataItem.TableDrivedEntityID;
            result.KeyProperties = new List<EntityInstanceProperty>();
            foreach (var item in dbDataItem.DataItemKeyColumns)
                result.KeyProperties.Add(new EntityInstanceProperty(null) { ColumnID = item.ColumnID, Value = item.Value });
            return result;
        }
        private DataItem GetDBDataItem(int ID)
        {
            using (var model = new MyProjectEntities())
            {
                DataItem dataItem;

                dataItem = model.DataItem.First(x => x.ID == ID);

                return dataItem;
            }
        }
        private DataItem GetDBDataItem(MyProjectEntities model, int tableDrivedEntityID, List<EntityInstanceProperty> keyProperties)
        {
           
                DataItem dataItem;

                var dataItems = model.DataItem.Where(x => x.TableDrivedEntityID == tableDrivedEntityID);
                foreach (var keyColumn in keyProperties)
                    dataItems = dataItems.Where(x => x.DataItemKeyColumns.Any(y => y.ColumnID == keyColumn.ColumnID && y.Value == keyColumn.Value));
                dataItem = dataItems.FirstOrDefault();
                return dataItem;
          
        }




    }
}
