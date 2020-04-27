
using ModelEntites;
using MyCodeFunctionLibrary;
using MyConnectionManager;
using MyDatabaseFunctionLibrary;
using MyGeneralLibrary;
using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDataEditManagerBusiness
{
    public class EditQueryItemManager
    {
        //دسترسی ها برای ادیت به انیتی و رابطه اعمال نشده.بشود؟
        BizDatabase bizDatabase = new BizDatabase();
        DeleteQueryItemManager deleteQueryItemManager = new DeleteQueryItemManager();
        List<DP_DataRepository> ListData;
        BizRelationship bizRelationship = new BizRelationship();

        private List<TableDrivedEntityDTO> Entities = new List<TableDrivedEntityDTO>();
        private TableDrivedEntityDTO GetTableDrivedDTO(DR_Requester requester, int entityID)
        {
            if (Entities.Any(x => x.ID == entityID))
                return Entities.First(x => x.ID == entityID);
            else
            {
                var entity = bizTableDrivedEntity.GetSimpleEntity(requester, entityID);
                Entities.Add(entity);
                return entity;
            }

        }
        public List<QueryItem> GetQueryItems(DR_Requester requester, List<DP_DataRepository> listData)
        {
            ListData = listData;

            var queryDeleteItems = GetDeleteQueryQueue(requester);
            var queryRemoveItems = GetRemoveQueryQueue(requester);
            var queryEditItems = GetEditQueryQueue(requester);

            var allQueryItems = queryDeleteItems.Union(queryRemoveItems).Union(queryEditItems);
            return allQueryItems.ToList();
        }

        List<QueryItem> GetEditQueryQueue(DR_Requester requester)
        {
            var sortedList = SetSortedTree(requester);
            GetUpdateQueryItems(requester, sortedList);

            //foreach (var item in sortedList.Where(x => string.IsNullOrEmpty(x.Query)).ToList())
            //    sortedList.Remove(item);
            return sortedList;
        }


        private List<QueryItem> SetSortedTree(DR_Requester requester)
        {
            var result = new List<QueryItem>();
            foreach (var citem in ListData)
            {
                SetSortedTree(requester, result, citem);
            }
            //List<List<QueryItem>> selfTables = new List<List<QueryItem>>();
            //foreach (var item in result.Where(x => result.Any(y => y.FKSources.Any(z => z.IsSelfTable && z.PKQueryItem == x))))
            //{
            //    List<QueryItem> list = new List<QueryItem>();
            //    list.Add(item);
            //    foreach (var rItem in result.Where(x => x.FKSources.Any(y => y.IsSelfTable && y.PKQueryItem == item)))
            //    {
            //        list.Add(rItem);
            //    }
            //    selfTables.Add(list);
            //}
            //foreach (var list in selfTables)
            //{
            //    var fITem = list.First();
            //    foreach (var item in list.Where(x => x != fITem))
            //    {
            //        foreach (var prop in item.DataItem.GetProperties())
            //        {
            //            if (!fITem.DataItem.GetProperties().Any(x => x.ColumnID == prop.ColumnID))
            //                fITem.DataItem.AddProperty(prop.Column, prop.Value);
            //        }
            //        foreach (var lItem in result.Where(x => x.FKSources.Any(y => y.PKQueryItem == item)))
            //        {
            //            foreach (var rITem in lItem.FKSources.Where(x => x.PKQueryItem == item))
            //                rITem.PKQueryItem = fITem;
            //        }
            //        foreach (var lItem in item.FKSources)
            //        {
            //            if (lItem.PKQueryItem != fITem)
            //                fITem.FKSources.Add(lItem);
            //        }
            //        result.Remove(item);
            //    }
            //}
            return result;
        }
        private QueryItem SetSortedTree(DR_Requester requester, List<QueryItem> result, DP_DataRepository item, QueryItem parentQueryItem = null, ChildRelationshipInfo parentChildRelationshipInfo = null)
        {
            List<EntityInstanceProperty> editingProperties = new List<EntityInstanceProperty>();

            foreach (var child in item.ChildRelationshipInfos.Where(x => x.Relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary))
            {
                List<EntityInstanceProperty> foreignKeyProperties = new List<EntityInstanceProperty>();
                //foreach (var relCol in child.Relationship.RelationshipColumns)
                //{
                //    var prop = item.GetProperty(relCol.FirstSideColumnID);
                //    foreignKeyProperties.Add(prop);
                //}

                bool isSelfTable = false;
                if (child.Relationship.RelationshipColumns.All(x => x.FirstSideColumnID == x.SecondSideColumnID))
                {
                    isSelfTable = true;
                    //foreach (var prop in item.GetProperties())
                    //    childItem.AddProperty(prop.Column, prop.Value);
                }
                if (child.RelatedData.Any())
                {
                    foreach (var pkData in child.RelatedData)
                    {
                        var pkQueryItem = SetSortedTree(requester, result, pkData);

                        //int pkIdentityColumnID = 0;
                        //if (pkData.IsNewItem)
                        //    if (pkData.KeyProperties.Any(x => x.IsIdentity))
                        //    {
                        //        pkIdentityColumnID = pkData.KeyProperties.First(x => x.IsIdentity).ColumnID;
                        //    }
                        // queryItem.FKSources.Add(new FKToPK(child.Relationship, pkQueryItem, pkIdentityColumnID, isSelfTable));
                        if (child.RelationshipIsChangedForUpdate)
                        {
                            foreach (var relCol in child.Relationship.RelationshipColumns)
                            {
                                var fkprop = item.GetProperty(relCol.FirstSideColumnID);
                                if (fkprop == null)
                                {
                                    fkprop = new EntityInstanceProperty(relCol.FirstSideColumn);
                                }
                                var pkprop = pkData.GetProperty(relCol.SecondSideColumnID);
                                if (pkData.IsNewItem && (pkprop.Column.IsIdentity || pkprop.PKIdentityColumn != null))
                                {
                                    fkprop.Value = "{" + fkprop.ColumnID + "}";
                                    fkprop.PKIdentityColumn = pkprop;
                                }
                                else
                                    fkprop.Value = pkData.GetProperty(relCol.SecondSideColumnID).Value;
                                //fkprop.IsHidden = child.IsHidden;
                                foreignKeyProperties.Add(fkprop);
                            }
                        }
                    }
                }
                else
                {
                    if (item.IsNewItem || (child.RelationshipIsChangedForUpdate && child.RemovedDataForUpdate.Any()))
                    {
                        foreach (var relCol in child.Relationship.RelationshipColumns)
                        {
                            var fkprop = item.GetProperty(relCol.FirstSideColumnID);
                            if (fkprop == null)
                            {
                                fkprop = new EntityInstanceProperty(relCol.FirstSideColumn);
                            }
                            fkprop.Value = null;
                            //prop.IsHidden = child.IsHidden;
                            foreignKeyProperties.Add(fkprop);
                        }
                    }
                }
                foreach (var fkProp in foreignKeyProperties)
                {
                    fkProp.HasForeignKeyData = true;
                    editingProperties.Add(fkProp);
                }
            }


            if (parentChildRelationshipInfo != null && parentChildRelationshipInfo.Relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromPrimartyToForeign)
            {
                bool isSelfTable = false;
                if (parentChildRelationshipInfo.Relationship.RelationshipColumns.All(x => x.FirstSideColumnID == x.SecondSideColumnID))
                {
                    isSelfTable = true;
                    //foreach (var prop in item.GetProperties())
                    //    childItem.AddProperty(prop.Column, prop.Value);
                }

                //int pkIdentityColumnID = 0;
                //if (parentItem.IsNewItem)
                //    if (parentItem.KeyProperties.Any(x => x.IsIdentity))
                //    {
                //        pkIdentityColumnID = parentItem.KeyProperties.First(x => x.IsIdentity).ColumnID;
                //    }
                //parentQueryItem همون طرف pk هست
                //var relationship = bizRelationship.GetRelationship(parentChildRelationshipInfo.Relationship.PairRelationshipID);
                //queryItem.FKSources.Add(new FKToPK(relationship, parentQueryItem, pkIdentityColumnID, isSelfTable));
                if (parentChildRelationshipInfo.RelationshipIsChangedForUpdate)
                {
                    foreach (var relCol in parentChildRelationshipInfo.Relationship.RelationshipColumns)
                    {
                        var fkprop = item.GetProperty(relCol.SecondSideColumnID);
                        if (fkprop == null)
                        {
                            fkprop = new EntityInstanceProperty(relCol.SecondSideColumn);
                        }
                        var pkprop = parentQueryItem.DataItem.GetProperty(relCol.FirstSideColumnID);
                        if (parentQueryItem.DataItem.IsNewItem && (pkprop.Column.IsIdentity || pkprop.PKIdentityColumn != null))
                        {
                            fkprop.Value = "{" + fkprop.ColumnID + "}";
                            fkprop.PKIdentityColumn = pkprop;
                        }
                        else
                            fkprop.Value = pkprop.Value;
                        fkprop.HasForeignKeyData = true;
                        //fkprop.IsHidden = parentChildRelationshipInfo.IsHidden;
                        editingProperties.Add(fkprop);
                    }
                }
            }
            QueryItem queryItem = new QueryItem(GetTableDrivedDTO(requester, item.TargetEntityID), item.IsNewItem ? Enum_QueryItemType.Insert : Enum_QueryItemType.Update, editingProperties, item);
            result.Add(queryItem);

            foreach (var child in item.ChildRelationshipInfos.Where(x => x.Relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromPrimartyToForeign))
                foreach (var childItem in child.RelatedData)
                    SetSortedTree(requester, result, childItem, queryItem, child);

            return queryItem;
        }

        private void GetUpdateQueryItems(DR_Requester requester, List<QueryItem> sortedList)
        {
            List<QueryItem> removeList = new List<QueryItem>();
            foreach (var queryItem in sortedList)
            {
                if (queryItem.DataItem.IsEdited)
                {
                    var entity = bizTableDrivedEntity.GetTableDrivedEntity(requester, queryItem.DataItem.TargetEntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithRelationships);
                    //  List<Tuple<int, string, string, bool>> propValues = new List<Tuple<int, string, string, bool>>();
                    List<EntityInstanceProperty> changedPropeties = null;
                    if (queryItem.DataItem.IsNewItem)
                        changedPropeties = queryItem.DataItem.GetProperties();
                    else
                        changedPropeties = queryItem.DataItem.GetProperties().Where(x => x.ValueIsChanged).ToList();
                    foreach (var property in changedPropeties)
                    {
                        bool skip = false;
                        //چرا؟
                        //چون پایین اینها بررسی میشوند؟
                        //     if (entity.Relationships.Any(x => x.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary && x.RelationshipColumns.Any(y => y.FirstSideColumnID == property.ColumnID)))
                        if (queryItem.EditingProperties.Any(x => x.ColumnID == property.ColumnID && x.HasForeignKeyData))
                            skip = true;
                        if (property.Column.IsDBCalculatedColumn)
                            skip = true;
                        if (property.IsIdentity)
                            skip = true;
                        if (!queryItem.DataItem.IsNewItem && property.IsKey)
                            skip = true;
                        if (!skip)
                        {
                            //  propValues.Add(new Tuple<int, string, string, bool>(property.ColumnID, property.Name, property.Value, false));
                            queryItem.EditingProperties.Add(property);
                        }

                    }
                }
                if (queryItem.EditingProperties.Any())
                {
                    foreach (var identityProp in queryItem.EditingProperties.Where(x => x.PKIdentityColumn != null))
                    {
                        //    identityProp.PropertyValueChanged += IdentityProp_PropertyValueChanged;
                        identityProp.PKIdentityColumn.PropertyValueChanged += (sender, e) => IdentityProp_PropertyValueChanged(sender, e, identityProp, queryItem);
                    }
                    if (queryItem.DataItem.IsNewItem && queryItem.TargetEntity.DeterminerColumnID == 0)
                    {
                        queryItem.Query = GetInsertQuery(queryItem);
                    }
                    else
                        queryItem.Query = GetUpdateQuery(queryItem);
                }
                else
                    removeList.Add(queryItem);
            }
            foreach (var item in removeList)
                sortedList.Remove(item);
            //کلا این قسمتها زیاد قشنگ نیست و با بحث ارث بری داخل یک جدول کد زیاد جالب نشده .باز نویسی شود
            //foreach (var fkSource in queryItem.FKSources)
            //{

            //    foreach (var column in fkSource.Relationship.RelationshipColumns)
            //    {
            //        //برای ارث بری بروی یک جدول
            //        bool isSelfRelationColumn = false;
            //        if (column.SecondSideColumnID == column.FirstSideColumnID)
            //            isSelfRelationColumn = true;
            //        //
            //        //این بخش برای روابط یک به یک بین دو موجودیت مختلف که موجودیت دوم اصلاحی می باشد و رابطه بروی کلیدهای اصلی هست هم تست شود
            //        //زیرا فرک کنم برای موجودیت دوم یا غیر اصلی هم بروی کلید اصلی آپدیت می نویسد که درست نیست
            //        string value = "";
            //        if (fkSource.PKIdentityColumnID != 0 && column.SecondSideColumnID == fkSource.PKIdentityColumnID)
            //        {
            //            value = "{0}";
            //            var setIdentity = new SetIdentity();
            //            setIdentity.TargetQueryItem = queryItem;
            //            setIdentity.TargetColumnID = column.FirstSideColumnID;
            //            fkSource.PKQueryItem.SetIdentities.Add(setIdentity);
            //        }
            //        else
            //        {
            //            if (fkSource.PKQueryItem != null)
            //            {
            //                var primaryProperty = fkSource.PKQueryItem.DataItem.GetProperty(column.SecondSideColumnID);
            //                if (primaryProperty != null)
            //                    value = primaryProperty.Value;
            //            }
            //            else
            //            {
            //                value = "<Null>";
            //            }
            //        }
            //        // var fkColumn = entity.Columns.First(x => x.ID == column.FirstSideColumnID);
            //        propValues.Add(new Tuple<int, string, string, bool>(column.FirstSideColumnID, column.FirstSideColumn.Name, value, isSelfRelationColumn));

            //    }

            //}
            //if (propValues.Any())
            //{
            //    string propertyWithValues = "";
            //    string properties = "";
            //    string values = "";
            //    foreach (var prop in propValues)
            //    {
            //        properties += (properties == "" ? "" : ",") + prop.Item2;
            //        values += (values == "" ? "" : ",") + GetPropertyValue(prop.Item3);
            //        if (!prop.Item4)
            //            propertyWithValues += (propertyWithValues == "" ? "" : ",") + prop.Item2 + "=" + GetPropertyValue(prop.Item3);
            //    }

            //    if (queryItem.DataItem.IsNewItem && !propValues.Any(x => x.Item4))
            //    {
            //        queryItem.Query = GetInsertQuery(entity, properties, values);
            //    }
            //    else
            //    {
            //        //برای ارث بری های داخل یک جدول وقتی که کلید اصلی سوپر ایدنتیتی است.مقدار آنرا برابر {0} میکنه تا توقسمت 
            //        //where
            //        //هم مقدار ست شود
            //        //
            //        foreach (var keycol in queryItem.DataItem.KeyProperties)
            //        {
            //            if (propValues.Any(x => x.Item1 == keycol.ColumnID))
            //                keycol.Value = propValues.First(x => x.Item1 == keycol.ColumnID).Item3;
            //        }
            //        //
            //        if (!string.IsNullOrEmpty(propertyWithValues))
            //            queryItem.Query = GetUpdateQuery(entity, propertyWithValues, queryItem.DataItem.KeyProperties);
            //    }
            //}


        }

        private void IdentityProp_PropertyValueChanged(object sender, PropertyValueChangedArg e, EntityInstanceProperty targetFk, QueryItem queryItem)
        {
            var oldValue = targetFk.Value.ToString();
            targetFk.Value = e.NewValue;
            if (!string.IsNullOrEmpty(queryItem.Query))
                queryItem.Query = queryItem.Query.Replace(oldValue, e.NewValue.ToString());
        }

        private string GetInsertQuery(QueryItem queryItem)
        {
            //string propertyWithValues = "";
            string properties = "";
            string values = "";
            foreach (var prop in queryItem.EditingProperties)
            {
                properties += (properties == "" ? "" : ",") + "[" + prop.Column.Name + "]";
                values += (values == "" ? "" : ",") + GetPropertyValue(prop.Value);
                //if (!prop.Item4)
                //    propertyWithValues += (propertyWithValues == "" ? "" : ",") + prop.Item2 + "=" + GetPropertyValue(prop.Item3);
            }
            return GetInsertQuery(queryItem.TargetEntity, properties, values);
        }
        private string GetInsertQuery(TableDrivedEntityDTO entity, string propertyNames, string propertyValues)
        {
            var result = "";
            result = "insert into " + GetTableName(entity) + " (";
            result += propertyNames;
            result += ") values (";
            result += propertyValues += ")";
            //}
            return result;
        }
        public string GetUpdateQuery(QueryItem queryItem)
        {
            string propertyWithValues = "";
            foreach (var prop in queryItem.EditingProperties.Where(x => !x.Column.PrimaryKey))
            {
                propertyWithValues += (propertyWithValues == "" ? "" : ",") + "[" + prop.Column.Name + "]" + "=" + GetPropertyValue(prop.Value);
            }
            return queryItem.Query = GetUpdateQuery(queryItem.TargetEntity, propertyWithValues, queryItem.DataItem.KeyProperties);
        }
        private string GetUpdateQuery(TableDrivedEntityDTO entity, string propertyNameWithValues, List<EntityInstanceProperty> keyInstanceProperties)
        {
            string keyWhere = "";
            foreach (var column in keyInstanceProperties)
            {
                keyWhere += (keyWhere == "" ? "" : " and ") + column.Name + "=" + GetPropertyValue(column.Value);
            }
            var result = "";
            //if (entity != null && !string.IsNullOrEmpty(entity.Criteria))
            //{
            //    var criteria = entity.Criteria;
            //    if (entity.Criteria.Contains("="))
            //    {
            //        result = "update " + entity.TableName + " set ";
            //        result += propertyNameWithValues + "," + criteria;
            //        result += " where " + keyWhere;
            //    }
            //}
            //else
            //{
            result = "update " + GetTableName(entity) + " set ";
            result += propertyNameWithValues;
            result += " where " + keyWhere;
            //}
            return result;
        }
        List<QueryItem> GetRemoveQueryQueue(DR_Requester requester)
        {
            List<QueryItem> result = new List<QueryItem>();
            var removeItems = GetRemoveItems(ListData);
            foreach (var item in removeItems)
            {
                List<EntityInstanceProperty> listEditProperties = new List<EntityInstanceProperty>();

                foreach (var col in item.Item1.RelationshipColumns)
                {
                    var prop = item.Item2.GetProperty(col.SecondSideColumnID);
                    if (prop == null)
                    {
                        prop = new EntityInstanceProperty(col.SecondSideColumn);
                    }
                    prop.Value = null;
                    listEditProperties.Add(prop);
                }
                result.Add(new QueryItem(GetTableDrivedDTO(requester, item.Item2.TargetEntityID), Enum_QueryItemType.Update, listEditProperties, item.Item2));

            }
            foreach (var queryItem in result)
            {
                queryItem.Query = GetUpdateQuery(queryItem);
            }
            return result;
        }
        private List<Tuple<RelationshipDTO, DP_DataRepository>> GetRemoveItems(List<DP_DataRepository> listdata, List<Tuple<RelationshipDTO, DP_DataRepository>> result = null)
        {
            if (result == null)
                result = new List<Tuple<RelationshipDTO, DP_DataRepository>>();
            foreach (var item in listdata)
            {
                foreach (var removeChild in item.ChildRelationshipInfos.Where(x => x.RemovedDataForUpdate.Any() && x.Relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromPrimartyToForeign))
                {
                    if (removeChild.RelationshipDeleteOption == RelationshipDeleteOption.SetNull)
                        foreach (var removeItem in removeChild.RemovedDataForUpdate)
                        {
                            var relationship = bizRelationship.GetRelationship(removeChild.Relationship.ID);
                            result.Add(new Tuple<RelationshipDTO, DP_DataRepository>(relationship, removeItem));
                        }
                }
                foreach (var child in item.ChildRelationshipInfos)
                {
                    GetRemoveItems(child.RelatedData.ToList(), result);
                }
            }
            return result;
        }
        List<QueryItem> GetDeleteQueryQueue(DR_Requester requester)
        {
            List<QueryItem> result = new List<QueryItem>();
            List<DP_DataRepository> deleteITems = GetDeleteItems(ListData);
            var itemAndQueries = deleteQueryItemManager.GetDeleteQueryItems(requester, deleteITems);
            foreach (var item in itemAndQueries)
            {
                foreach (var query in item.Item2)
                    result.Add(query);
            }
            //foreach (var item in listData)
            //{
            //    var inner = GetDeleteQueryQueue(item);
            //    foreach (var it in inner)
            //        result.Add(it);
            //}
            return result;
        }

        private List<DP_DataRepository> GetDeleteItems(List<DP_DataRepository> listdata, List<DP_DataRepository> result = null)
        {
            if (result == null)
                result = new List<DP_DataRepository>();
            foreach (var item in listdata)
            {
                foreach (var removeChild in item.ChildRelationshipInfos.Where(x => x.RemovedDataForUpdate.Any() && x.Relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromPrimartyToForeign))
                {
                    if (removeChild.RelationshipDeleteOption == RelationshipDeleteOption.DeleteCascade || removeChild.Relationship.RelationshipColumns.Any(x => !x.SecondSideColumn.IsNull))
                        foreach (var removeItem in removeChild.RemovedDataForUpdate)
                            result.Add(removeItem);
                }
                foreach (var child in item.ChildRelationshipInfos)
                {
                    GetDeleteItems(child.RelatedData.ToList(), result);
                }
            }
            return result;
        }

        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();

        private string GetTableName(TableDrivedEntityDTO entity)
        {
            return (string.IsNullOrEmpty(entity.RelatedSchema) ? "" : "[" + entity.RelatedSchema + "]" + ".") + "[" + entity.TableName + "]";
        }

        private string GetPropertyValue(object value)
        {
            if (value == null)
                return "NULL";
            else
                return "'" + value + "'";
        }
    }

}
