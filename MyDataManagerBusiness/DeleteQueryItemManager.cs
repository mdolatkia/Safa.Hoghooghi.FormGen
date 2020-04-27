
using ModelEntites;
using MyCodeFunctionLibrary;
using MyConnectionManager;
using MyDatabaseFunctionLibrary;
using MyDataManagerBusiness;
using MyDataSearchManagerBusiness;
using MyModelManager;
using MyRelationshipDataManager;
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
    public class DeleteQueryItemManager
    {
        BizDatabase bizDatabase = new BizDatabase();
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        //List<DP_DataRepository> listData;
        SearchRequestManager SearchRequestManager = new SearchRequestManager();
        RelationshipDataManager RelationshipDataManager = new RelationshipDataManager();
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

        public bool GetTreeItems(DR_Requester requester, DP_DataRepository deleteDataItem, DP_DataRepository rootDeleteItem)
        {
            List<ChildRelationshipInfo> result = new List<ChildRelationshipInfo>();
            bool loop = false;
            //   DP_DataRepository resultItem = item;
            var entity = bizTableDrivedEntity.GetTableDrivedEntity(requester, deleteDataItem.TargetEntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithRelationships);
            foreach (var relationship in entity.Relationships.Where(x=>x.MastertTypeEnum==Enum_MasterRelationshipType.FromPrimartyToForeign))
            {
                if (!loop)

                {
                    var searchRepository = RelationshipDataManager.GetSecondSideSearchDataItemByRelationship(deleteDataItem, relationship.ID);
                    var newrequester = new DR_Requester();
                    newrequester.SkipSecurity = true;
                    DR_SearchViewRequest searchViewRequest = new DR_SearchViewRequest(newrequester, searchRepository);
                    var searchViewResult = SearchRequestManager.Process(searchViewRequest);
                    if (searchViewResult.ResultDataItems.Any())
                    {
                        var childRelationshipInfo = new ChildRelationshipInfo();
                        childRelationshipInfo.SourceData = deleteDataItem;
                        childRelationshipInfo.Relationship = relationship;
                        result.Add(childRelationshipInfo);
                        childRelationshipInfo.RelationshipDeleteOption = relationship.DeleteOption;
                        deleteDataItem.ChildRelationshipInfos.Add(childRelationshipInfo);
                        if (childRelationshipInfo.RelationshipDeleteOption == RelationshipDeleteOption.SetNull)
                        {
                            foreach (var childItem in searchViewResult.ResultDataItems)
                            {
                                DP_DataRepository dataItem = new DP_DataRepository(childItem.TargetEntityID, childItem.TargetEntityAlias);
                                dataItem.DataView = childItem;
                                dataItem.ParantChildRelationshipInfo = childRelationshipInfo;
                                childRelationshipInfo.RelatedData.Add(dataItem);


                            }
                        }
                        else
                        {
                            bool repeatedInParents = false;
                            foreach (var childItem in searchViewResult.ResultDataItems)
                            {//هردفعه پرنتها برای هر ایتم گرفته نشود
                                List<DP_DataRepository> parents = GetParentDataItems(childRelationshipInfo);
                                if (parents.Any(z => z.TargetEntityID == childItem.TargetEntityID && z.KeyProperties.All(x => childItem.Properties.Any(y => y.IsKey && x.ColumnID == y.ColumnID && x.Value == y.Value))))
                                {
                                    var parentRepeted = parents.First(z => z.TargetEntityID == childItem.TargetEntityID && z.KeyProperties.All(x => childItem.Properties.Any(y => y.IsKey && x.ColumnID == y.ColumnID && x.Value == y.Value)));
                                    loop = true;
                                    repeatedInParents = true;
                                    DP_DataRepository dataItem = new DP_DataRepository(childItem.TargetEntityID, childItem.TargetEntityAlias);
                                    dataItem.DataView = childItem;
                                    dataItem.ParantChildRelationshipInfo = childRelationshipInfo;
                                    dataItem.Error = "وابستگی تکراری با " + parentRepeted.ViewInfo;
                                    childRelationshipInfo.RelatedData.Add(dataItem);
                                }
                            }
                            if (!repeatedInParents)
                            {
                                foreach (var childItem in searchViewResult.ResultDataItems)
                                {
                                    if (ChildItemExistInTree(rootDeleteItem, childItem))
                                    {

                                    }
                                    else
                                    {
                                        DP_DataRepository dataItem = new DP_DataRepository(childItem.TargetEntityID, childItem.TargetEntityAlias);
                                        dataItem.DataView = childItem;
                                        dataItem.ParantChildRelationshipInfo = childRelationshipInfo;
                                        childRelationshipInfo.RelatedData.Add(dataItem);
                                        var innerloop = GetTreeItems(requester, dataItem, rootDeleteItem);
                                        if (innerloop)
                                        {
                                            loop = true;
                                            return loop;
                                        }
                                    }
                                }
                            }

                        }

                    }
                }
            }
            return loop;
        }
        public DP_DataView GetDataView(DP_BaseData data)
        {
            var newrequester = new DR_Requester();
            newrequester.SkipSecurity = true;
            DP_SearchRepository searchDataViewItem = new DP_SearchRepository(data.TargetEntityID);
            foreach (var col in data.KeyProperties)
            {
                searchDataViewItem.Phrases.Add(new SearchProperty() { ColumnID = col.ColumnID, Value = col.Value });
            }
            DR_SearchViewRequest searchViewRequest = new DR_SearchViewRequest(newrequester, searchDataViewItem);
            var searchViewResult = SearchRequestManager.Process(searchViewRequest);
            if (searchViewResult.ResultDataItems.Any())
                return searchViewResult.ResultDataItems[0];
            else
                return null;
        }

        private List<DP_DataRepository> GetParentDataItems(ChildRelationshipInfo childRelationshipInfo, List<DP_DataRepository> items = null)
        {
            if (items == null)
                items = new List<DP_DataRepository>();
            if (childRelationshipInfo != null)
            {
                items.Add(childRelationshipInfo.SourceData);
                return GetParentDataItems(childRelationshipInfo.SourceData.ParantChildRelationshipInfo, items);
            }
            else
            {
                return items;
            }
        }

        private bool ChildItemExistInTree(DP_DataRepository treeItem, DP_DataView childItem)
        {
            //برای برابری یه چیز کاملتو عمومی نوشته شود
            if (treeItem.TargetEntityID == childItem.TargetEntityID && treeItem.KeyProperties.All(x => childItem.Properties.Any(y => y.IsKey && x.ColumnID == y.ColumnID && x.Value == y.Value)))
                return true;
            foreach (var childInfo in treeItem.ChildRelationshipInfos.Where(x => x.RelatedData.Any(y => y.TargetEntityID == childItem.TargetEntityID)))
            {
                foreach (var item in childInfo.RelatedData)
                    return ChildItemExistInTree(item, childItem);
            }
            return false;
        }
        EditDataActionActivityManager actionActivityManager = new EditDataActionActivityManager();
        public List<Tuple<DP_DataRepository, List<QueryItem>>> GetDeleteQueryItems(DR_Requester requester, List<DP_DataRepository> items)
        {
            List<Tuple<DP_DataRepository, List<QueryItem>>> result = new List<Tuple<DP_DataRepository, List<QueryItem>>>();
            List<DP_DataRepository> rootDataItems = new List<DP_DataRepository>();
            foreach (var item in items)
            {
                DP_DataRepository rootDeleteITem = new DP_DataRepository(item.TargetEntityID, item.TargetEntityAlias);
                rootDeleteITem.SetProperties(item.GetProperties());
                rootDeleteITem.DataView = item.DataView;
                rootDeleteITem.IsFullData = item.IsFullData;

                var loop = GetTreeItems(requester, rootDeleteITem, rootDeleteITem);
                if (loop)
                {
                    throw new Exception("امکان حذف بعلت وابستگی داده ها وجود ندارد");
                }
                rootDataItems.Add(rootDeleteITem);
            }
            foreach (var item in rootDataItems)
            {
                var queryItems = GetDeleteQueryQueue(requester, item);
                foreach (var queryItem in queryItems)
                {
                    if (queryItem.QueryType == Enum_QueryItemType.Delete)
                    {
                        queryItem.Query = GetDeleteQueryQueue(requester, queryItem);
                    }
                    else if (queryItem.QueryType == Enum_QueryItemType.Update)
                    {
                        EditQueryItemManager editQueryItemManage=new EditQueryItemManager();
                        queryItem.Query = editQueryItemManage.GetUpdateQuery(queryItem);
                    }
                }
                result.Add(new Tuple<DP_DataRepository, List<QueryItem>>(item, queryItems));
            }
            return result;
        }

        private string GetDeleteQueryQueue(DR_Requester requester, QueryItem queryItem)
        {
            if (queryItem.TargetEntity.DeterminerColumnID == 0)
            {

                string keyWhere = "";
                foreach (var column in queryItem.DataItem.KeyProperties)
                {
                    keyWhere += (keyWhere == "" ? "" : " and ") + column.Name + "=" + GetPropertyValue(column.Value);
                }
                return "delete " + GetTableName(queryItem.TargetEntity) + " where " + keyWhere;
            }
            else
            {
                var entity = bizTableDrivedEntity.GetTableDrivedEntity(requester, queryItem.TargetEntity.ID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
                EditQueryItemManager editQueryItemManager = new MyDataEditManagerBusiness.EditQueryItemManager();
                var listProperties = new List<EntityInstanceProperty>();
                foreach (var column in entity.Columns)
                    listProperties.Add(new EntityInstanceProperty(column) { Value = null });
                listProperties.Add(new EntityInstanceProperty(entity.DeterminerColumn) { Value = null});

                var newQuertItem = new QueryItem(entity, Enum_QueryItemType.Update, listProperties, queryItem.DataItem);
                return editQueryItemManager.GetUpdateQuery(newQuertItem);
            }
        }
        private string GetPropertyValue(object value)
        {
            if (value == null )
                return "NULL";
            else
                return "'" + value + "'";
        }
        private List<QueryItem> GetDeleteQueryQueue(DR_Requester requester, DP_DataRepository item, ChildRelationshipInfo parentChildRelatoinshipInfo = null, List<QueryItem> result = null)
        {
            //اینجا کوئری آیتمها بصورت درختی ست نمیشوند چون لازم نیست همینکه بترتیب می آیند کافی است
            if (result == null)
                result = new List<QueryItem>();

            if (item.ChildRelationshipInfos.Any(x => x.RelatedData.Any()))
            {
                foreach (var child in item.ChildRelationshipInfos)
                {
                    //اول زیر مجموعه ها حذف شوند 
                    foreach (var cItem in child.RelatedData)
                        GetDeleteQueryQueue(requester, cItem, child, result);
                }
            }
            var query = GetQueryDeleteOrUpdateNull(item, parentChildRelatoinshipInfo);
            result.Add(new QueryItem(GetTableDrivedDTO(requester, item.TargetEntityID), query.Item1, query.Item2, item));
            return result;
        }

        private Tuple<Enum_QueryItemType, List<EntityInstanceProperty>> GetQueryDeleteOrUpdateNull(DP_DataRepository item, ChildRelationshipInfo parentChildRelatoinshipInfo)
        {

            Enum_QueryItemType queryItemType;
            List<EntityInstanceProperty> listEditProperties = new List<EntityInstanceProperty>();

            if (parentChildRelatoinshipInfo == null)
            {
                queryItemType = Enum_QueryItemType.Delete;
            }
            else
            {
                if (parentChildRelatoinshipInfo.RelationshipDeleteOption == RelationshipDeleteOption.DeleteCascade)
                {
                    queryItemType = Enum_QueryItemType.Delete;
                }
                else
                {
                    queryItemType = Enum_QueryItemType.Update;

                    foreach (var col in parentChildRelatoinshipInfo.Relationship.RelationshipColumns)
                    {
                        var prop = item.GetProperty(col.SecondSideColumnID);
                        if (prop == null)
                            prop = new EntityInstanceProperty(col.SecondSideColumn);
                        prop.Value = null;
                        listEditProperties.Add(prop);

                    }
                }
            }
            return new Tuple<Enum_QueryItemType, List<EntityInstanceProperty>>(queryItemType, listEditProperties);
        }
        private string GetTableName(TableDrivedEntityDTO entity)
        {
            return (string.IsNullOrEmpty(entity.RelatedSchema) ? "" : "[" + entity.RelatedSchema + "]" + ".") + "[" + entity.TableName + "]";
        }

    }


}
