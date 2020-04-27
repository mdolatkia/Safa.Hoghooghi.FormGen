

using ModelEntites;
using MyCodeFunctionLibrary;
using MyConnectionManager;
using MyDatabaseFunctionLibrary;
using MyDataManagerBusiness;
using MyModelManager;
using MyRelationshipDataManager;

using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyDataSearchManagerBusiness
{
    public class SearchRequestManager
    {
        BizFormula bizFormula = new BizFormula();
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        BizRelationship bizRElationship = new BizRelationship();
        BizColumn bizColumn = new BizColumn();
        //BizOrganizationSecurity bizOrganizationSecurity = new BizOrganizationSecurity();
        ModelDataHelper dataHelper = new ModelDataHelper();
        BizRoleSecurity bizRoleSecurity = new BizRoleSecurity();
        //TableDrivedEntityDTO mainEntity { set; get; }
        BizEntityRelationshipTail bizEntityRelationshipTail = new BizEntityRelationshipTail();
        BizEntityListView bizEntityListView = new BizEntityListView();
        BizEntitySearch bizEntitySearch = new BizEntitySearch();
        //TableDrivedEntityDTO connectionEntity { set; get; }

        public DR_ResultSearchExists Process(DR_SearchExistsRequest request)
        {
            DR_ResultSearchExists result = new DR_ResultSearchExists();
            try
            {
                // connectionEntity = mainEntity;
                var fromClause = GetFromQuery(request.Requester, request.SearchDataItems.TargetEntityID, request.SearchDataItems, request.SecurityMode);

                //بخش سلکت را طبق خصویات و روابط نمایشی میسازد
                var select = "Top 1 1";

                var commandStr = "select " + select + fromClause.Item2;
                var executeResult = ConnectionManager.GetDBHelperByEntityID(request.SearchDataItems.TargetEntityID).ExecuteScalar(commandStr);
                if (executeResult != null && executeResult.ToString() == "1")
                    result.ExistsResult = true;
                else
                    result.ExistsResult = false;
            }
            catch (Exception ex)
            {
                result.Result = Enum_DR_ResultType.ExceptionThrown;
                result.Message = "خطا در جستجو" + Environment.NewLine + ex.Message;
            }
            return result;
        }

        public DR_ResultSearchCount Process(DR_SearchCountRequest request)
        {
            DR_ResultSearchCount result = new DR_ResultSearchCount();

            try
            {

                // connectionEntity = mainEntity;
                var fromClause = GetFromQuery(request.Requester, request.SearchDataItems.TargetEntityID, request.SearchDataItems, request.SecurityMode);
                //بخش سلکت را طبق خصویات و روابط نمایشی میسازد
                var select = "Count(*)";
                var commandStr = "select " + select + fromClause.Item2;

                var executeResult = ConnectionManager.GetDBHelperByEntityID(request.SearchDataItems.TargetEntityID).ExecuteScalar(commandStr);
                if (executeResult != null)
                    result.ResultCount = (int)executeResult;
                else
                    result.ResultCount = 0;
            }
            catch (Exception ex)
            {
                result.Result = Enum_DR_ResultType.ExceptionThrown;
                result.Message = "خطا در جستجو" + Environment.NewLine + ex.Message;
            }
            return result;
        }


        //private int GetSearchCountResult(DR_Requester requester, TableDrivedEntityDTO entity, DP_SearchRepository searchDataItem)
        //{
        //    //TableDrivedEntityDTO entity = bizTableDrivedEntity.GetTableDrivedEntity(entityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithRelationships);
        //}
        public DR_ResultSearchView Process(DR_SearchViewRequest request)
        {
            DR_ResultSearchView result = new DR_ResultSearchView();
            //var mainEntity = GetTableDrivedEntity1(request.Requester, request.SearchDataItems.TargetEntityID, request.SecurityMode);
            //if (mainEntity == null)
            //{
            //    result.Result = Enum_DR_ResultType.ExceptionThrown;
            //    result.Message = "عدم دسترسی به موجودیت به شناسه" + " " + request.SearchDataItems.TargetEntityID + " ";
            //    return result;
            //}


            try
            {

                var dataTable = GetDataTableBySearchDataItems(request.Requester, request.SearchDataItems.TargetEntityID, request.SearchDataItems, request.SecurityMode, null, request.EntityViewID, request.MaxDataItems, request.OrderByEntityViewColumnID, request.SortType);
                result.ResultDataItems = DataTableToDP_ViewRepository(dataTable.Item1, dataTable.Item2, dataTable.Item3);
                result.Result = Enum_DR_ResultType.SeccessfullyDone;


            }
            catch (Exception ex)
            {
                result.Result = Enum_DR_ResultType.ExceptionThrown;
                result.Message = "خطا در جستجو" + Environment.NewLine + ex.Message;
            }
            return result;
        }



        SecurityHelper securityHelper = new SecurityHelper();
        //private TableDrivedEntityDTO GetTableDrivedEntity1(DR_Requester requester, int targetEntityID, SecurityMode securityMode)
        //{
        //    return ;

        //}
        //private TableDrivedEntityDTO GetTableDrivedEntity2(int targetEntityID, EntityColumnInfoType withSimpleColumns, EntityRelationshipInfoType withRelationships)
        //{
        //    //DP_EntityPermissionResult result = new DP_EntityPermissionResult();
        //    //var permissions = securityHelper.GetAssignedPermissions(requester, targetEntityID, DatabaseObjectCategory.Entity, false);
        //    //if (permissions.GrantedActions.Any(x => x == SecurityAction.NoAccess))
        //    //    return null;
        //    //else
        //    return bizTableDrivedEntity.GetTableDrivedEntity(targetEntityID, withSimpleColumns, withRelationships);
        //}
        public Tuple<TableDrivedEntityDTO, EntityListViewDTO, DataTable> GetDataTableBySearchDataItems(DR_Requester requester, int entityID, DP_SearchRepository searchDataItem, SecurityMode securityMode, EntityListViewDTO listView, int listViewID, int maxItems = 0, int orderColumnID = 0, Enum_OrderBy sortType = Enum_OrderBy.Ascending)
        {
            var queryParts = GetQueryParts(requester, entityID, searchDataItem, securityMode, listView, listViewID, maxItems, orderColumnID, sortType);
            var commandStr = "select " + queryParts.Item3 + queryParts.Item4 + queryParts.Item5;
            return new Tuple<TableDrivedEntityDTO, EntityListViewDTO, DataTable>(queryParts.Item1, queryParts.Item2, ConnectionManager.GetDBHelperByEntityID(entityID).ExecuteQuery(commandStr));

        }

        private Tuple<TableDrivedEntityDTO, EntityListViewDTO, string, string, string> GetQueryParts(DR_Requester requester, int entityID, DP_SearchRepository searchDataItem
            , SecurityMode securityMode, EntityListViewDTO listView, int listViewID
            , int maxItems = 0, int orderColumnID = 0, Enum_OrderBy sortType = Enum_OrderBy.Ascending)
        {
            var fromClause = GetFromQuery(requester, entityID, searchDataItem, securityMode);
            string orderBy = "";// GetOrderByQuery(fromClause.Item1, listView, orderColumnID, sortType);
            var select = GetSelectQuery(requester, fromClause.Item1, listView, listViewID);
            return new Tuple<TableDrivedEntityDTO, EntityListViewDTO, string, string, string>(fromClause.Item1, select.Item1, select.Item2, fromClause.Item2, orderBy);
        }







        public Tuple<string, string> GetSelectFromExternal(DR_Requester requester, int entityID, DP_SearchRepository searchDataItem, bool primaryKeys, SecurityMode securityMode)
        {


            //TableDrivedEntityDTO entity = bizTableDrivedEntity.GetTableDrivedEntity(entityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithRelationships);
            //var entity = GetTableDrivedEntity1(requester, entityID, securityMode);
            //if (entity == null)
            //{
            //    throw new Exception("عدم دسترسی به موجودیت به شناسه" + " " + entityID + " ");
            //}
            EntityListViewDTO listView = null;
            if (primaryKeys)
            {
                listView = bizEntityListView.GetEntityKeysListView(requester, entityID);
            }
            else
            {
                listView = bizEntityListView.GetEntityEditListView(requester, entityID);
            }

            var queryParts = GetQueryParts(requester, entityID, searchDataItem, securityMode, listView, 0);
            return new Tuple<string, string>(queryParts.Item3, queryParts.Item4);
            //return new Tuple<string, string>("", "");
        }
        //private string GetQuery(DR_Requester requester, TableDrivedEntityDTO mainEntity, DP_SearchRepository searchDataItem, int maxDataItems, EntityListViewDTO entityListView, int orderColumnID, Enum_OrderBy sortType, SecurityMode securityMode)
        //{

        //}


        private Tuple<EntityListViewDTO, string> GetSelectQuery(DR_Requester requester, TableDrivedEntityDTO entity, EntityListViewDTO listView, int listViewID)
        {
            BizEntityListView bizEntityListView = new BizEntityListView();
            if (listView == null)
            {
                if (listViewID == 0)
                    listView = bizEntityListView.GetEntityDefaultListView(requester, entity.ID);
                else
                    listView = bizEntityListView.GetEntityListView(requester, listViewID);
            }
            if (listView == null)
            {
                throw new Exception("عدم دسترسی به لیست نمایش موجودیت به شناسه" + " " + entity.ID);
            }
            var select = "";
            var searchTableName = GetSearchTableAlias(entity, 0);
            foreach (var column in listView.EntityListViewAllColumns)
            {
                if (column.RelationshipTailID == 0)
                {
                    var relativeColumnName = "'" + column.RelativeColumnName + "'";// "'" + column.Column.Name + "0'";// "'0>" + column.Column.Name + "'";
                    select += (select == "" ? "" : ",") + "[" + searchTableName + "]" + "." + "[" + column.Column.Name + "]" + " as " + relativeColumnName;
                }
                else
                {
                    //سلکت باید درست شود و هر فیلد خودش یک سلکت باشد
                    var aliasColumn = "'" + column.Column.Name + column.RelationshipTailID + "'";
                    var relatedSelectParam = GetSelectColumn(requester, searchTableName, 0, column.RelationshipTail, entity);
                    var from = relatedSelectParam[0].Item3 + " as " + relatedSelectParam[0].Item2;
                    for (int i = 1; i <= relatedSelectParam.Count - 1; i++)
                    {
                        from += (" inner join " + relatedSelectParam[i].Item3 + " as " + relatedSelectParam[i].Item2 + " on " + relatedSelectParam[i].Item4);
                    }
                    from += " where " + relatedSelectParam[0].Item4;
                    var lastTableName = relatedSelectParam.Last().Item2;
                    select += (select == "" ? "" : ",") + "(" + "select" + " " + "[" + lastTableName + "]" + "." + "[" + column.Column.Name + "]" + " " + "from" + " " + from + ")"
                        + " as " + aliasColumn;
                }
            }
            return new Tuple<EntityListViewDTO, string>(listView, select);
        }

        private string GetOrderByQuery(EntityListViewDTO entityListView, int orderColumnID, Enum_OrderBy sortType)
        {
            var orderBy = "";
            //if (orderColumnID != 0)
            //{
            //    string columnName = "";

            //        var column = entityListView.EntityListViewAllColumns.FirstOrDefault(x => x.ColumnID == orderColumnID);
            //        if (column != null)
            //        {
            //            columnName = column.Column.Name;
            //        }


            //    var searchTableName = GetSearchTableAlias(entity, 0);

            //    if (columnName != "")
            //        orderBy = " order by " + (string.IsNullOrEmpty(mainEntity.RelatedSchema) ? "" : mainEntity.RelatedSchema + ".") + searchTableName + "." + columnName + " " + (sortType == Enum_OrderBy.Ascending ? "Asc" : "Desc");
            //}
            return orderBy;
        }

        private Tuple<TableDrivedEntityDTO, string> GetFromQuery(DR_Requester requester, int entityID, DP_SearchRepository searchDataItem, SecurityMode securityMode)
        {
            var entity = bizTableDrivedEntity.GetTableDrivedEntity(requester, entityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithRelationships);
            if (entity == null)
            {
                throw new Exception("عدم دسترسی به موجودیت به شناسه" + " " + entityID + " ");
            }
            if (requester.SkipSecurity == false)
            {
                var pathPermission = CheckSearchRepositoryPermission(requester, searchDataItem);
                if (!string.IsNullOrEmpty(pathPermission))
                {
                    throw new Exception(pathPermission);
                }
            }
            if (entity.DeterminerColumnID != 0)
            {
                BizColumn bizColumn = new BizColumn();
                var column = bizColumn.GetColumn(entity.DeterminerColumnID, true);

                searchDataItem.Phrases.Add(new SearchProperty() { Operator = CommonOperator.InValues, ColumnID = entity.DeterminerColumnID, Name = column.Name, Value = GetDeterminerValues(entity) });
            }

            string securityQuery = "";
            if (requester != null && !requester.SkipSecurity)
            {
                securityQuery = GetSecurityQuery(requester, entity, securityMode);
            }

            var searchQueryResult = GetSearchQuery(requester, entity, searchDataItem);
            var searchQuery = searchQueryResult.Item1;
            //جوین خارجی خودش با خودش
            var innerjoin = "";
            var searchTableName = GetSearchTableAlias(entity, 0);
            var searchTableNameWithSchema = (string.IsNullOrEmpty(entity.RelatedSchema) ? "" : entity.RelatedSchema + ".") + searchTableName;
            if (!entity.IsView)
            {
                var onClause = "";
                foreach (var rCol in entity.Columns.Where(x => x.PrimaryKey))
                {
                    var ffield = "";
                    var sfield = "";
                    ffield = searchTableName + "." + rCol.Name;
                    sfield = "innerSearch" + "." + rCol.Name;
                    onClause += (onClause == "" ? "" : " and ") + ffield + "=" + sfield;
                }
                innerjoin = " inner join " + "(" + searchQuery + ") as innerSearch" + " on " + onClause;

                if (securityQuery != "")
                {
                    var securityOnClause = "";
                    foreach (var rCol in entity.Columns.Where(x => x.PrimaryKey))
                    {
                        var ffield = "";
                        var sfield = "";
                        ffield = searchTableName + "." + rCol.Name;
                        sfield = "securitySearch" + "." + rCol.Name;
                        securityOnClause += (securityOnClause == "" ? "" : " and ") + ffield + "=" + sfield;
                    }
                    innerjoin += " inner join " + "(" + securityQuery + ") as securitySearch" + " on " + securityOnClause;
                }
            }
            else
            {
                var firstSearchTableName = searchQueryResult.Item2;
                var index = searchQuery.IndexOf(firstSearchTableName);
                innerjoin = searchQuery.Substring(index + firstSearchTableName.Length, searchQuery.Length - (index + firstSearchTableName.Length));
                innerjoin = innerjoin.Replace(firstSearchTableName, searchTableNameWithSchema);

            }
            return new Tuple<TableDrivedEntityDTO, string>(entity, " from " + searchTableNameWithSchema + innerjoin);

        }
        private string CheckSearchRepositoryPermission(DR_Requester requester, DP_SearchRepository searchDataItem)
        {

            if (!bizTableDrivedEntity.DataIsAccessable(requester, searchDataItem.TargetEntityID))
                return "عدم دسترسی به موجودیت به شناسه" + " " + searchDataItem.TargetEntityID;

            List<DP_SearchRepository> relationshipPhrases = GetRelationshipPhrases(searchDataItem);
            foreach (var item in relationshipPhrases)
            {
                if (!bizRElationship.DataIsAccessable(requester, item.SourceRelationship.ID, false, true))
                    return "عدم دسترسی به رابطه به شناسه" + " " + item.SourceRelationship.ID;

            }

            List<SearchProperty> propertyPhrases = GetPropertyPhrase(searchDataItem);
            foreach (var item in propertyPhrases)
            {
                if (!bizColumn.DataIsAccessable(requester, item.ColumnID))
                    return "عدم دسترسی به خصوصیت به شناسه" + " " + item.ColumnID;
            }
            foreach (var item in relationshipPhrases)
            {
                return CheckSearchRepositoryPermission(requester, item as DP_SearchRepository);
            }
            return "";
        }
        private List<SearchProperty> GetPropertyPhrase(Phrase phrase, List<SearchProperty> result = null)
        {
            if (result == null) result = new List<SearchProperty>();
            if (phrase is SearchProperty)
                result.Add(phrase as SearchProperty);
            else if (phrase is LogicPhrase && !(phrase is DP_SearchRepository))
            {
                var logicPhrase = phrase as LogicPhrase;
                foreach (var item in logicPhrase.Phrases)
                {
                    GetPropertyPhrase(item, result);
                }
            }
            return result;
        }
        private List<DP_SearchRepository> GetRelationshipPhrases(LogicPhrase logicPhrase, List<DP_SearchRepository> result = null)
        {
            if (result == null) result = new List<DP_SearchRepository>();
            foreach (var phrase in logicPhrase.Phrases)
            {
                if (phrase is DP_SearchRepository)
                {
                    result.Add(phrase as DP_SearchRepository);
                }
                else if (phrase is LogicPhrase && !(phrase is DP_SearchRepository))
                {
                    GetRelationshipPhrases(phrase as LogicPhrase, result);
                }
            }

            return result;
        }

        private string GetDeterminerValues(TableDrivedEntityDTO mainEntity)
        {
            string result = "";
            foreach (var val in mainEntity.EntityDeterminers)
            {
                result += (result == "" ? "" : ",") + val.Value;
            }
            return result;
        }

        private string GetSecurityQuery(DR_Requester requester, TableDrivedEntityDTO mainEntity, SecurityMode securityMode)
        {

            SecurityHelper securityHelper = new SecurityHelper();
            Tuple<EntitySecurityInDirectDTO, List<Tuple<OrganizationPostDTO, List<EntitySecurityDirectDTO>>>> securityItems = null;
            if (securityMode == SecurityMode.View)
            {
                bool entityHasViewSecurity = securityHelper.EntityHasSecurity(mainEntity.ID, securityMode);
                if (entityHasViewSecurity)
                {
                    var viewSecurityItems = bizRoleSecurity.GetPostEntitySecurityItems(requester, mainEntity.ID, securityMode);
                    var editSecurityItems = bizRoleSecurity.GetPostEntitySecurityItems(requester, mainEntity.ID, SecurityMode.Edit);
                    if (editSecurityItems != null)
                        viewSecurityItems.Item2.AddRange(editSecurityItems.Item2);
                    securityItems = viewSecurityItems;
                }
                else
                    return "";
            }
            else
            {
                bool entityHasEditSecurity = securityHelper.EntityHasSecurity(mainEntity.ID, securityMode);
                if (entityHasEditSecurity)
                    securityItems = bizRoleSecurity.GetPostEntitySecurityItems(requester, mainEntity.ID, securityMode);
                else
                {
                    bool entityHasViewSecurity = securityHelper.EntityHasSecurity(mainEntity.ID, SecurityMode.View);
                    if (entityHasViewSecurity)
                    {
                        return GetNowRowSearchQuery(requester, mainEntity);
                    }
                    else
                        return "";

                }
            }
            if (securityItems.Item2.Any(x => x.Item2.Any(y => y.IgnoreSecurity)))
                return "";

            DP_SearchRepository mainDataItem = new DP_SearchRepository(mainEntity.ID);
            //mainDataItem.TargetEntityID = ;

            DP_SearchRepository mainSearchDataItem = null;

            if (securityItems != null && securityItems.Item1 != null)
            {
                //چرا همین که فرستاده میشود در فانکشن پر نمیشود؟   mainDataItem.Phrase

                //mainDataItem.Phrase = new LogicPhrase();
                var currentSearchRepository = CreateChildSearchRepository(mainDataItem, securityItems.Item1.RelationshipTail, mainDataItem);
                if (!mainDataItem.Phrases.Contains(currentSearchRepository.Item1))
                    mainDataItem.Phrases.Add(currentSearchRepository.Item1);
                mainSearchDataItem = currentSearchRepository.Item2;
            }
            else
            {
                mainSearchDataItem = mainDataItem;
            }
            if (securityItems != null && securityItems.Item2.Any())
            {

                //mainSearchDataItem.SourceToTargetRelationshipType = relationshipTail.SourceToTargetRelationshipType;
                //mainSearchDataItem.SourceToTargetMasterRelationshipType = relationshipTail.SourceToTargetMasterRelationshipType;
                //searchDataItems.Add(relatedSearchDataItem);
                //LogicPhrase mainLogicPhrase = new LogicPhrase();
                //mainLogicPhrase.AndOrType = AndORType.Or;
                mainSearchDataItem.AndOrType = AndORType.Or;
                //mainSearchDataItem.Phrase = mainLogicPhrase;
                foreach (var post in securityItems.Item2)
                {

                    foreach (var item in post.Item2)
                    {
                        LogicPhrase logicPhrase = new LogicPhrase();

                        //اینجا مهمه کا ان باشه یا اور
                        //در واقع مشخص کننده اینه که اون شرطهایی که برای یک پست در فرم دسترسی داده برای یک سابجکت تعریف شده اند اند بشوند یا اور

                        //logicPhrase.AndOrType = item.ConditionAndORType;
                        foreach (var condition in item.Conditions)
                        {
                            LogicPhrase conditionLogicPhrase = null;

                            if (condition.RelationshipTailID == 0)
                            {
                                conditionLogicPhrase = logicPhrase;
                                //currentSearchRepository = mainSearchDataItem;
                            }
                            else
                            {
                                var currentSearchRepository = CreateChildSearchRepository(mainSearchDataItem, condition.RelationshipTail, logicPhrase);
                                conditionLogicPhrase = currentSearchRepository.Item2 as LogicPhrase;
                                if (!logicPhrase.Phrases.Contains(currentSearchRepository.Item1))
                                    logicPhrase.Phrases.Add(currentSearchRepository.Item1);
                            }

                            var searchProperty = new SearchProperty();
                            searchProperty.ColumnID = condition.ColumnID;
                            searchProperty.IsKey = condition.Column.PrimaryKey;
                            searchProperty.Name = condition.Column.Name;
                            //درست شود
                            searchProperty.Operator = CommonOperator.Equals;
                            //حالت فانکشن
                            string value = "";
                            if (!string.IsNullOrEmpty(condition.Value))
                            {
                                value = condition.Value;
                            }
                            else if (condition.ReservedValue != SecurityReservedValue.None)
                            {
                                value = GerReserveValueFromPost(post, condition.ReservedValue);
                            }
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (condition.DBFunctionID != 0)
                                {
                                    DatabaseFunctionHandler dbFunctionHandler = new DatabaseFunctionHandler();
                                    var dbFunctionResult = dbFunctionHandler.GetDatabaseFunctionValue(requester, condition.DBFunctionID, value);
                                    value = dbFunctionResult.Result.ToString();
                                }
                                searchProperty.Value = value;
                                searchProperty.Name = condition.Column.Name;
                                conditionLogicPhrase.Phrases.Add(searchProperty);
                            }

                        }
                        mainSearchDataItem.Phrases.Add(logicPhrase);
                    }

                }
                if (!mainSearchDataItem.Phrases.Any())
                {
                    return GetNowRowSearchQuery(requester, mainEntity);
                }
                else
                    return GetSearchQuery(requester, mainEntity, mainSearchDataItem).Item1;


            }
            else
                return "";

        }

        private string GetNowRowSearchQuery(DR_Requester requester, TableDrivedEntityDTO entity)
        {
            List<DP_DataRepository> result = new List<DP_DataRepository>();

            var firstTableIndex = 1;
            var firstTableAlias = GetSearchTableAlias(entity, firstTableIndex);
            var keyColumns = dataHelper.GetKeyColumns(entity);
            var entityBaseSelectFromQuery = dataHelper.GetSingleEntityBaseSelectFromQuery(entity, firstTableAlias, keyColumns);
            var split = dataHelper.splitQuery(entityBaseSelectFromQuery);
            var selectfromClause = split.Item1;
            string criteriaClause = split.Item2;
            string searchWhereClause = "1=2";

            string whereClause = "";
            if (criteriaClause != "")
                whereClause += "(" + criteriaClause + ")";
            if (searchWhereClause != "")
                whereClause += (whereClause == "" ? "" : " and ") + "(" + searchWhereClause + ")";
            //فانکشن شود
            var commandStr = selectfromClause + (whereClause == "" ? "" : " where ") + whereClause;

            return commandStr;
        }

        private Tuple<DP_SearchRepository, DP_SearchRepository> CreateChildSearchRepository(DP_SearchRepository parentRepository, EntityRelationshipTailDTO relationshipTail, LogicPhrase parentLogicPhrase)
        {//اولی و آخری را برمیگگرداند
         //parentRepository اولی فکر کنم بیخود باشد زیرا از بیرون معلومه که چی فرستاده شده یا همان 
            DP_SearchRepository firstRepository = null;
            DP_SearchRepository lastRepository = null;
            CreateChildSearchRepository(parentRepository, relationshipTail, parentLogicPhrase, ref firstRepository, ref lastRepository);
            return new Tuple<DP_SearchRepository, DP_SearchRepository>(firstRepository, lastRepository);
        }
        private DP_SearchRepository CreateChildSearchRepository(DP_SearchRepository parentRepository, EntityRelationshipTailDTO relationshipTail, LogicPhrase parentLogicPhrase, ref DP_SearchRepository firstRepository, ref DP_SearchRepository lastRepository)
        {



            if (relationshipTail != null)
            {
                //LogicPhrase phrase = parentRepository.Phrase as LogicPhrase;
                //if (phrase == null)
                //{
                //    phrase = new LogicPhrase();
                //    parentRepository.Phrase = phrase;
                //}
                var newRepository = parentLogicPhrase.Phrases.FirstOrDefault(x => x is DP_SearchRepository && (x as DP_SearchRepository).SourceRelationship.ID == relationshipTail.Relationship.ID) as DP_SearchRepository;
                if (newRepository == null)
                {
                    newRepository = new DP_SearchRepository(relationshipTail.Relationship.EntityID2);
                    //newRepository.TargetEntityID = ;
                    newRepository.SourceRelationship = relationshipTail.Relationship;
                    //newRepository.SourceToTargetRelationshipType = relationshipTail.Relationship.TypeEnum;
                    //newRepository.SourceToTargetMasterRelationshipType = relationshipTail.Relationship.MastertTypeEnum;
                    // newRepository.Phrase = new LogicPhrase();
                }
                if (firstRepository == null)
                    firstRepository = newRepository;

                var childRepository = CreateChildSearchRepository(newRepository, relationshipTail.ChildTail, newRepository as LogicPhrase, ref firstRepository, ref lastRepository);
                if (childRepository != null)
                {
                    //LogicPhrase cphrase = newRepository.Phrase as LogicPhrase;
                    ////if (cphrase == null)
                    ////{
                    ////    cphrase = new LogicPhrase();
                    ////    newRepository.Phrase = cphrase;
                    ////}
                    (newRepository as LogicPhrase).Phrases.Add(childRepository);


                }
                else
                {
                    if (lastRepository == null)
                        lastRepository = newRepository;
                }
                return newRepository;
            }
            else
                return null;
        }

        private string GerReserveValueFromPost(Tuple<OrganizationPostDTO, List<EntitySecurityDirectDTO>> post, SecurityReservedValue reservedValue)
        {
            if (reservedValue == SecurityReservedValue.OrganizationID)
                return post.Item1.OrganizationID.ToString();
            else if (reservedValue == SecurityReservedValue.OrganizationPostID)
                return post.Item1.ID.ToString();
            else if (reservedValue == SecurityReservedValue.OrganizationTypeID)
                return post.Item1.OrganizationTypeID.ToString();
            else if (reservedValue == SecurityReservedValue.OrganizationTypeRoleTypeID)
                return post.Item1.OrganizationTypeRoleTypeID.ToString();
            else if (reservedValue == SecurityReservedValue.RoleTypeID)
                return post.Item1.RoleTypeID.ToString();
            return "";
        }



        ////private Tuple<bool, bool> HasGeneralAccess(List<OrganizationPostDTO> posts, int entityID, List<DP_SearchRepository> searchDataItems, DR_Requester requester, EntityRelationshipTailDTO sentRelationshipTail = null)
        ////{
        ////    //دسترسی مثل حقوقی چی؟ اگر اپراتور بود اداره پرونده درست باشد اگر کارشناس بود نامش در لیست جدول کارشناسان؟؟؟؟؟
        ////    //bool shouldImposeRoleSecurity = false;
        ////    //bool roleSecurityImposed = false;

        ////    var roleSecurityDirects = bizRoleSecurity.GetEntitySecurityDirects(entityID, true);
        ////    //if (directSecurity.Any())
        ////    //{
        ////    //    shouldImposeRoleSecurity = true;
        ////    //}

        ////    //if (roleSecurities.Any(x => roleIds.Contains(x.RoleID)))
        ////    //{//دسترسی همیشگی
        ////    //    roleSecurityImposed = true;
        ////    //}
        ////    //else
        ////    //{
        ////    DP_SearchRepository finalSearchDataItem = null;
        ////    //var roleSecurityDirects = bizRoleSecurity.GetEntityRoleSecurityDirects(entityID, true);
        ////    if (roleSecurityDirects.Any())
        ////    {
        ////        //shouldImposeRoleSecurity = true;
        ////        if (sentRelationshipTail == null)
        ////        {
        ////            finalSearchDataItem = searchDataItems.FirstOrDefault(x => x.SourceRelatedData == null);
        ////            if (finalSearchDataItem == null)
        ////            {
        ////                finalSearchDataItem = new DP_SearchRepository() { TargetEntityID = entityID };
        ////                searchDataItems.Add(finalSearchDataItem);
        ////            }
        ////        }
        ////        else
        ////        {
        ////            var firstSearchDataItem = searchDataItems.FirstOrDefault(x => x.SourceRelatedData == null);
        ////            if (firstSearchDataItem == null)
        ////            {
        ////                firstSearchDataItem = new DP_SearchRepository() { TargetEntityID = entityID };
        ////                searchDataItems.Add(firstSearchDataItem);
        ////            }


        ////            finalSearchDataItem = SetOrCreateSearchDataItems(searchDataItems, null, sentRelationshipTail, firstSearchDataItem);
        ////        }
        ////    }
        ////    else
        ////    {
        ////        if (sentRelationshipTail != null)
        ////        {
        ////            return new Tuple<bool, bool>(true, false);
        ////        }
        ////        else
        ////        {
        ////            var roleSecurityInDirect = bizRoleSecurity.GetEntitySecurityInDirect(entityID, true);
        ////            if (roleSecurityInDirect != null)
        ////            {
        ////                shouldImposeRoleSecurity = true;
        ////                //var searchDataItem = SetOrCreateSearchDataItems(searchDataItems, searchDataItems.First(x => x.SourceRelatedData == null), roleSecurityInDirect.RelationshipTail);
        ////                var redirectResult = HasGeneralAccess(posts, roleSecurityInDirect.RelationshipTail.TargetEntityID, searchDataItems, requester, roleSecurityInDirect.RelationshipTail);
        ////                roleSecurityImposed = redirectResult.Item2;
        ////            }
        ////        }
        ////    }
        ////    if (roleSecurityDirects.Any() && finalSearchDataItem != null)
        ////    {

        ////        List<SearchProperty> searchProperties = new List<SearchProperty>();
        ////        foreach (var roleSecurityDirect in roleSecurityDirects)
        ////        {
        ////            roleSecurityDirect.SecuritySubject.
        ////            foreach (var roldId in roleIds)
        ////            {
        ////                var securityDirects = roleSecurityDirects.Where(x => x.RoleID == roldId || (x.RoleGroup != null && x.RoleGroup.Roles.Any(y => y.ID == roldId)));

        ////                //if (securityDirects.Any())
        ////                //{
        ////                //    roleSecurityImposed = true;
        ////                //}

        ////                string value = "";
        ////                SearchOperator searchOperator;

        ////                if (roleSecurityDirect.DBFunctionID != 0)
        ////                {
        ////                    DatabaseFunctionHandler dbFunctionHandler = new DatabaseFunctionHandler();
        ////                    var dbFunctionResult = dbFunctionHandler.GetDatabaseFunctionValue(roleSecurityDirect.DBFunctionID, roldId.ToString());
        ////                    value = dbFunctionResult.Result.ToString();
        ////                    if (roleSecurityDirect.Operator == EntitySecurityOperator.Equals)
        ////                        searchOperator = new SearchOperator() { Name = StringOperator.Equals.ToString() };
        ////                    else
        ////                        searchOperator = new SearchOperator() { Name = StringOperator.In.ToString() };
        ////                }
        ////                else
        ////                {
        ////                    searchOperator = new SearchOperator() { Name = StringOperator.Equals.ToString() };
        ////                    value = roleSecurityDirect.Value;
        ////                }

        ////                //var property = finalSearchDataItem.Properties.FirstOrDefault(x => x.ColumnID == roleSecurityDirect.ColumnID);
        ////                //if (property == null)
        ////                //{
        ////                var property = new SearchProperty() { ColumnID = roleSecurityDirect.ColumnID };
        ////                property.Value = value;
        ////                property.Operator = searchOperator;
        ////                property.AndORType = AndORType.Or;
        ////                searchProperties.Add(property);


        ////                //}


        ////            }

        ////        }
        ////        if (searchProperties.Any())
        ////        {
        ////            var searchProperty = new SearchProperty();
        ////            searchProperty.ChildProperties = new Tuple<AndORType, List<SearchProperty>>(AndORType.And, searchProperties);
        ////            finalSearchDataItem.Properties.Add(searchProperty);
        ////            roleSecurityImposed = true;
        ////        }
        ////    }

        ////    //}
        ////    return new Tuple<bool, bool>(shouldImposeRoleSecurity, roleSecurityImposed);



        ////}


        //private bool HasGeneralAccess(List<int> roleIds, int entityID)
        //{

        //}

        //private DP_SearchRepository SetOrCreateSearchDataItems(List<DP_SearchRepository> searchDataItems, DP_SearchRepository currentSearchDataItem, EntityRelationshipTailDTO relationshipTail, DP_SearchRepository parentSearchDataItem = null)
        //{
        //    if (relationshipTail == null)
        //    {
        //        return currentSearchDataItem;
        //    }
        //    else
        //    {
        //        var relatedSearchDataItem = searchDataItems.FirstOrDefault(x => x.SourceRelatedData == currentSearchDataItem && x.SourceRelationID == relationshipTail.RelationshipID);
        //        if (relatedSearchDataItem == null)
        //        {
        //            relatedSearchDataItem = new DP_SearchRepository();
        //            relatedSearchDataItem.SourceRelationID = relationshipTail.RelationshipID;
        //            //relatedSearchDataItem.RelationshipColumns = relationshipTail.RelationshipColumns;
        //            relatedSearchDataItem.SourceRelatedData = parentSearchDataItem;
        //            relatedSearchDataItem.TargetEntityID = relationshipTail.RelationshipTargetEntityID;
        //            relatedSearchDataItem.SourceToTargetRelationshipType = relationshipTail.SourceToTargetRelationshipType;
        //            relatedSearchDataItem.SourceToTargetMasterRelationshipType = relationshipTail.SourceToTargetMasterRelationshipType;
        //            searchDataItems.Add(relatedSearchDataItem);
        //        }
        //        return SetOrCreateSearchDataItems(searchDataItems, relatedSearchDataItem, relationshipTail.ChildTail, currentSearchDataItem);
        //    }

        //}

        private Tuple<string, string> GetSearchQuery(DR_Requester requester, TableDrivedEntityDTO entity, DP_SearchRepository searchDataItem)
        {
            List<DP_DataRepository> result = new List<DP_DataRepository>();

            //////سلکت جدول اصلی فقط ساخته میشود
            var firstTanleIndex = 1;
            var firstTableAlias = GetSearchTableAlias(entity, firstTanleIndex);
            var keyColumns = dataHelper.GetKeyColumns(entity);
            var entityBaseSelectFromQuery = dataHelper.GetSingleEntityBaseSelectFromQuery(entity, firstTableAlias, keyColumns);
            var split = dataHelper.splitQuery(entityBaseSelectFromQuery);
            var selectfromClause = split.Item1;
            string criteriaClause = split.Item2;
            string searchWhereClause = GetWhereClause(requester, entity, entity, searchDataItem, firstTableAlias, firstTanleIndex);

            string whereClause = "";
            if (criteriaClause != "")
                whereClause += "(" + criteriaClause + ")";
            if (searchWhereClause != "")
                whereClause += (whereClause == "" ? "" : " and ") + "(" + searchWhereClause + ")";
            //فانکشن شود
            var commandStr = selectfromClause + (whereClause == "" ? "" : " where ") + whereClause;

            return new Tuple<string, string>(commandStr, firstTableAlias);

        }

        private string GetWhereClause(DR_Requester requester, TableDrivedEntityDTO entity, TableDrivedEntityDTO connectionEntity, Phrase phrase, string tableAlias, int tableIndex)
        {
            //string where = "";

            if (phrase is SearchProperty)
            {
                return GetPartialWhere((phrase as SearchProperty), entity, tableAlias);
            }
            else if (phrase is DP_SearchRepository && (phrase as DP_SearchRepository).SourceRelationship != null)
            {
                var searchRepository = (phrase as DP_SearchRepository);

                //return GetWhereClause(entity, searchRepository, tableAlias, tableIndex);
                //else
                return GetRelatedSearchParams(requester, searchRepository, tableAlias, tableIndex, connectionEntity);

            }
            else if (phrase is LogicPhrase)
            {
                var logicPhrase = (phrase as LogicPhrase);
                var result = "";
                foreach (var internalPhrase in logicPhrase.Phrases)
                {
                    var where = GetWhereClause(requester, entity, connectionEntity, internalPhrase, tableAlias, tableIndex);
                    if (where != "")
                        result += (result == "" ? "" : (logicPhrase.AndOrType == AndORType.And ? " and " : " or ")) + where;
                }
                if (result != "")
                {
                    if (logicPhrase.Phrases.Count > 1)
                        return "(" + result + ")";
                    else
                        return result;
                }
            }
            return "";
        }


        private string GetPartialWhere(SearchProperty property, TableDrivedEntityDTO entity, string searchTableName)
        {
            string where = "";
            if (PropertyHasValue(property))
            {

                if (property.ColumnID != 0)
                {
                    string columnName = property.Name;
                    if (string.IsNullOrEmpty(property.Name))
                        //میشه نام ستون رو در ایتم قرار داد.نه اینجا همه جا
                        columnName = entity.Columns.First(x => x.ID == property.ColumnID).Name;
                    where = GetEquation(searchTableName + "." + columnName, property, property.Value);
                }
                else
                {
                    throw new Exception("asdad");
                }
            }
            return where;
        }

        private string GetEquation(string columnName, SearchProperty property, object value)
        {
            if (property.Operator == CommonOperator.Equals ||
                property.Operator == CommonOperator.BiggerThan ||
                property.Operator == CommonOperator.SmallerThan)
            {
                return columnName + GetStringOperaor(property) + "'" + property.Value + "'";
            }
            else if (property.Operator == CommonOperator.Contains ||
                    property.Operator == CommonOperator.StartsWith ||
                      property.Operator == CommonOperator.EndsWith)
            {
                if (property.Operator == CommonOperator.Contains)
                    return columnName + " like " + "N'%" + property.Value + "%'";
                else if (property.Operator == CommonOperator.StartsWith)
                    return columnName + " like " + "N'" + property.Value + "%'";
                else if (property.Operator == CommonOperator.EndsWith)
                    return columnName + " like " + "N'%" + property.Value + "'";
            }
            else if (property.Operator == CommonOperator.InValues)
            {
                return columnName + " in " + GetInValuseRange(property);
            }
            return columnName + "=" + "'" + property.Value + "'";

        }

        private string GetInValuseRange(SearchProperty property)
        {
            string result = "";
            if (property.Value != null && property.Value.ToString().Contains(","))
            {
                var split = property.Value.ToString().Split(',');
                foreach (var splt in split)
                {
                    result += (result == "" ? "" : ",") + "'" + splt + "'";
                }
            }
            else
            {
                result = "'" + property.Value + "'";
            }
            if (result != "")
                result = "(" + result + ")";
            return result;
        }

        private string GetStringOperaor(SearchProperty property)
        {
            if (property.Operator == CommonOperator.Equals)
                return "=";
            else if (property.Operator == CommonOperator.BiggerThan)
                return ">";
            else if (property.Operator == CommonOperator.SmallerThan)
                return "<";
            return "=";

        }

        private string GetRelatedSearchParams(DR_Requester requester, DP_SearchRepository dataItem, string parentSearchTableAlias, int parentSearchTableIndex, TableDrivedEntityDTO connectionEntity)
        {
            if (dataItem.SourceRelationship == null)
                return "";
            var entity = bizTableDrivedEntity.GetSimpleEntityWithColumns(requester, dataItem.TargetEntityID);


            var currentTableIndex = parentSearchTableIndex + 1;
            var whereClause = "";
            var currentTableAlias = GetSearchTableAlias(entity, currentTableIndex);

            var relationship = dataItem.SourceRelationship;

            var select = "";
            var selectColumn = "";
            if ((dataItem.RelationshipFromCount != 0 && dataItem.RelationshipFromCount != null) || (dataItem.RelationshipToCount != 0 && dataItem.RelationshipToCount != null))
                selectColumn = "count(1)";
            else
                selectColumn = "1";
            select = dataHelper.GetRelationEntityBaseSelectFromQuery(connectionEntity, entity, currentTableAlias, selectColumn);

            var split = dataHelper.splitQuery(select);
            var selectfromClause = split.Item1;
            string criteriawhereClause = split.Item2;

            var joinWhereClause = dataHelper.GetOnClause(parentSearchTableAlias, currentTableAlias, relationship);

            //joinWhereClause += (partialWhere == "" ? "" : (" and " + partialWhere));

            var finalInnerWhereCluase = "";
            if (criteriawhereClause != "")
                finalInnerWhereCluase = "(" + criteriawhereClause + ")";
            if (joinWhereClause != "")
                finalInnerWhereCluase += (finalInnerWhereCluase == "" ? "" : " and ") + "(" + joinWhereClause + ")";

            var childPhrasesResult = "";
            foreach (var childPhrase in dataItem.Phrases)
            {
                var phraseWhere = GetWhereClause(requester, entity, connectionEntity, childPhrase, currentTableAlias, currentTableIndex);
                if (phraseWhere != "")
                    childPhrasesResult += (childPhrasesResult == "" ? "" : (dataItem.AndOrType == AndORType.And ? " and " : " or ")) + phraseWhere;
            }
            if (childPhrasesResult != "")
                finalInnerWhereCluase += (finalInnerWhereCluase == "" ? "" : " and ") + "(" + childPhrasesResult + ")";


            var selectfull = "(" + selectfromClause + (finalInnerWhereCluase == "" ? "" : " where ") + finalInnerWhereCluase + ")";

            if ((dataItem.RelationshipFromCount != 0 && dataItem.RelationshipFromCount != null) || (dataItem.RelationshipToCount != 0 && dataItem.RelationshipToCount != null))
            {


                //var finalCountQuery = 
                if (dataItem.RelationshipFromCount != 0 && dataItem.RelationshipToCount != 0)
                    whereClause = selectfull + " between " + dataItem.RelationshipFromCount + " and " + dataItem.RelationshipToCount;
                else if (dataItem.RelationshipFromCount != 0)
                    whereClause = selectfull + ">=" + dataItem.RelationshipFromCount;
                else if (dataItem.RelationshipToCount != 0)
                    whereClause = selectfull + "<=" + dataItem.RelationshipToCount;
            }
            else
            {

                //whereClause = dataItem.HasNotRelationshipCheck ? "not " : "" + "exists (" + selectfromClause + childInnerjoin + (joinWhereClause == "" ? "" : " where ") + joinWhereClause + ")";
                whereClause = dataItem.HasNotRelationshipCheck == true ? "not" : "" + " exists " + selectfull;
            }




            return whereClause;



        }





        private List<Tuple<string, string, string, string>> GetSelectColumn(DR_Requester requester, string parentTableName, int parentSearchTableIndex, EntityRelationshipTailDTO relationshipTail, TableDrivedEntityDTO connectionEntity, List<Tuple<string, string, string, string>> result = null)
        {
            if (result == null)
                result = new List<Tuple<string, string, string, string>>();
            //string resultInnerjoin = "";

            var entity = bizTableDrivedEntity.GetSimpleEntity(requester, relationshipTail.Relationship.EntityID2);


            int currentTableIndex = parentSearchTableIndex + 1;
            var currentTableName = GetSearchTableAlias(entity, currentTableIndex);
            //string innerjoin = "";
            //var select = "";

            //////if (parentRelationship.TypeEnum != Enum_RelationshipType.OneToMany)
            //////{

            var innerjoin = dataHelper.GetInnerjoinOnClause(parentTableName, currentTableName, relationshipTail.Relationship, connectionEntity, entity);
            result.Add(new Tuple<string, string, string, string>(parentTableName, currentTableName, innerjoin.Item1, innerjoin.Item2));
            if (relationshipTail.ChildTail == null)
                return result;
            else
            {
                return GetSelectColumn(requester, currentTableName, currentTableIndex, relationshipTail.ChildTail, connectionEntity, result);
                //var childResult = GetSelectColumn(requester, currentTableName, currentTableIndex, relationshipTail.ChildTail, entity);
                ////  if(childResult.Item2=="")
                //return new Tuple<string, string>(innerjoin + childResult.Item1, childResult.Item2);
            }

            //foreach (var column in entity.Columns)//.Where(x => x.ViewEnabled))
            //{
            //    //if (hasOneToManyParents)
            //    //    select += (select == "" ? "" : "+") + "'" + column.Alias + "'" + "+" + "' '" + "+" + "GROUP_CONCAT(" + currentTableName + "." + column.Name + ")";
            //    ////select += (select == "" ? "" : ",") + searchTableName + "." + column.Name + " as " + column.Alias;
            //    //else
            //    select += (select == "" ? "" : ",") + currentTableName + "." + column.Name + " as " + parentRelatoinshipIds + "_" + column.ID;
            //}

            //////foreach (var relationship in entity.Relationships.Where(x => x.ViewEnabled == true))
            //////{

            //////    var childResult = GetViewRelatedSelectParams(currentTableName, currentTableIndex, relationship, entity, parentRelatoinshipIds + "_" + relationship.ID, hasOneToManyParents);
            //////    select += (select == "" ? "" : ",") + childResult.Item2;
            //////    innerjoin += " " + childResult.Item1;

            //////}

            //////    return new Tuple<string, string>(innerjoin, select);
            //////}
            //////else
            //////{
            //////    var childInnerjoin = "";
            //////    var childSelect = "";

            //////    //////foreach (var relationship in entity.Relationships.Where(x => x.ViewEnabled == true))
            //////    //////{
            //////    //////    var childResult = GetViewRelatedSelectParams(currentTableName, currentTableIndex, relationship, entity, parentRelatoinshipIds + "_" + relationship.ID, true);
            //////    //////    childSelect += (childSelect == "" ? "" : ",") + childResult.Item2;
            //////    //////    childInnerjoin += " " + childResult.Item1;
            //////    //////}
            //////    var joinWhereClause = dataHelper.GetOnClause(parentTableName, currentTableName, parentRelationship);

            //////    var selectCount = "'تعداد' + ' : '+ " + "ltrim(str(count(*)))";
            //////    var selectMany = "";
            //////    foreach (var column in entity.Columns)//.Where(x => x.ViewEnabled))
            //////    {
            //////        selectMany += (selectMany == "" ? "'" : "+' / ") + column.Alias + "'" + "+ ' : ' +" + "dbo.GROUP_CONCAT(" + currentTableName + "." + column.Name + ")";// + " as " + parentRelatoinshipIds + "#" + entity.ID + "_" + column.ID;
            //////    }
            //////    selectMany = selectCount + (selectMany == "" ? "" : "+ ' / '+") + selectMany;
            //////    selectMany += (childSelect == "" ? "" : "+" + childSelect);

            //////    var entityBaseSelectFromQuery = dataHelper.GetRelationEntityBaseSelectFromQuery(connectionEntity, entity, currentTableName, selectMany);
            //////    var split = dataHelper.splitQuery(entityBaseSelectFromQuery);
            //////    var selectfromClause = split.Item1;
            //////    string criteriawhereClause = split.Item2;
            //////    joinWhereClause += (criteriawhereClause == "" ? "" : " and " + criteriawhereClause);

            //////    select += "(" + selectfromClause + " where " + joinWhereClause + ")" + " as " + parentRelatoinshipIds;
            //////    //var relatedSeachParam = GetRelatedSearchParams(searchDataItems, innerjoin, whereClause, searchTableName, 0, entity, mainSearchDataItem, dataItem);

            //////    //innerjoin خالیه تو این حالت
            //////}



            //return innerjoin;
        }
        private string GetSearchTableAlias(TableDrivedEntityDTO entity, int v)
        {
            if (v != 0)
                return entity.TableName + "_" + v;
            else
                return entity.TableName;
        }
        private bool PropertyHasValue(SearchProperty item)
        {
            return (item.Value != null && !string.IsNullOrEmpty(item.Value.ToString()) && item.Value.ToString() != "0");
        }
        private List<DP_DataRepository> DataTableToDP_DataRepository(TableDrivedEntityDTO entity, EntityListViewDTO editListView, DataTable dataTable)
        {
            //var mainEntity = GetTableDrivedEntity2( entityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
            List<Tuple<short, ColumnDTO>> listColumns = new List<Tuple<short, ColumnDTO>>();

            for (short i = 0; i < dataTable.Columns.Count; i++)
            {
                var tailWithColumnName = dataTable.Columns[i].ColumnName;
                //var splt = tailWithColumnName.Split('>');
                //int tailID = Convert.ToInt32(splt[0]);
                //var columnName = splt[1];

                var column = editListView.EntityListViewAllColumns.FirstOrDefault(x => x.RelativeColumnName == tailWithColumnName);
                if (column != null)
                {
                    listColumns.Add(new Tuple<short, ColumnDTO>(i, column.Column));
                }
            }

            List<DP_DataRepository> result = new List<DP_DataRepository>();
            //  BizFormulaUsage bizFormulaUsage = new BizFormulaUsage();
            foreach (DataRow row in dataTable.Rows)
            {
                var data = ToDataRepository(entity, row, listColumns);
                //    bizFormulaUsage.CheckFormulaUsages(data);
                result.Add(data);
            }
            return result;
        }
        private DP_DataRepository ToDataRepository(TableDrivedEntityDTO entity, DataRow reader, List<Tuple<short, ColumnDTO>> listColumns)
        {

            var result = new DP_DataRepository(entity.ID, entity.Alias);
            result.IsFullData = true;
            for (int i = 0; i < reader.Table.Columns.Count; i++)
            {
                if (listColumns.Any(x => x.Item1 == i))
                {
                    var column = listColumns.First(x => x.Item1 == i).Item2;
                    var property = new EntityInstanceProperty(column);
                    //property.Name = column.Name;
                    if (reader[i] != DBNull.Value)
                        property.Value = reader[i].ToString();
                    else
                        property.Value = null;
                    result.AddProperty(column, property.Value);
                }
            }
            return result;
        }

        private List<DP_DataView> DataTableToDP_ViewRepository(TableDrivedEntityDTO entity, EntityListViewDTO listView, DataTable dataTable)
        {
            //TableDrivedEntityDTO entity = bizTableDrivedEntity.GetTableDrivedEntity(entityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
            List<DP_DataView> result = new List<DP_DataView>();
            //////List<Tuple<int, int, short>> keyColumns = new List<Tuple<int, int, short>>();
            //////for (short i = 0; i < dataTable.Columns.Count; i++)
            //////{
            //////    var columnName = dataTable.Columns[i].ColumnName;
            //////    if (columnName.EndsWith("KeyColumns"))
            //////    {
            //////        var splt = columnName.Split('>');
            //////        keyColumns.Add(new Tuple<int, int, short>(Convert.ToInt32(splt[0]), Convert.ToInt32(splt[1]), i));
            //////    }
            //////}
            List<Tuple<short, EntityListViewColumnsDTO>> listColumns = new List<Tuple<short, EntityListViewColumnsDTO>>();
            //List<int> keyColumns = new List<int>();
            for (short i = 0; i < dataTable.Columns.Count; i++)
            {

                var tailWithColumnName = dataTable.Columns[i].ColumnName;
                //var splt = tailWithColumnName.Split('>');
                //int tailID = Convert.ToInt32(splt[0]);
                //var columnName = splt[1];

                //if (listView == null)
                //{
                //    var column = entity.Columns.FirstOrDefault(x => "0>" + x.Name == tailWithColumnName);
                //    if (column != null)
                //        listColumns.Add(new Tuple<short, ColumnDTO, int>(i, column, tailID));
                //}
                //else
                //{
                var column = listView.EntityListViewAllColumns.FirstOrDefault(x => x.RelativeColumnName == tailWithColumnName);
                if (column != null)
                    listColumns.Add(new Tuple<short, EntityListViewColumnsDTO>(i, column));
                //}
                //if (tailID == 0)
                //{
                //    if (entity.Columns.Where(x => x.PrimaryKey).Any(x => x.Name == columnName))
                //        keyColumns.Add(i);
                //}
            }
            foreach (DataRow row in dataTable.Rows)
            {
                result.Add(ToViewRepository(entity, row, listColumns));
            }
            return result;
        }
        private DP_DataView ToViewRepository(TableDrivedEntityDTO entity, DataRow reader, List<Tuple<short, EntityListViewColumnsDTO>> listColumns)
        {

            var result = new DP_DataView(entity.ID, entity.Alias);

            //////List<Tuple<int, int, List<EntityInstanceProperty>>> tailKeyColumns = new List<Tuple<int, int, List<EntityInstanceProperty>>>();
            //////foreach (var keyColumn in keyColumns)
            //////{
            //////    if (reader[keyColumn.Item3] != DBNull.Value)
            //////    {
            //////        var keyColumnValue = reader[keyColumn.Item3].ToString();
            //////        var splt1 = keyColumnValue.Split('#');
            //////        List<EntityInstanceProperty> list = new List<EntityInstanceProperty>();
            //////        foreach (var item in splt1)
            //////        {
            //////            var splt2 = item.Split('@');
            //////            list.Add(new EntityInstanceProperty() { ColumnID = Convert.ToInt32(splt2[0]), Value = splt2[1] });
            //////        }
            //////        tailKeyColumns.Add(new Tuple<int, int, List<EntityInstanceProperty>>(keyColumn.Item1, keyColumn.Item2, list));
            //////    }
            //////}

            for (int i = 0; i < reader.Table.Columns.Count; i++)
            {


                //foreach (var item in result.TypeOrTypeConditions.Select(x => x.Type))
                //{
                //foreach (var itemProperty in entity.Columns)
                //{
                //    if (itemProperty.Name.ToLower() == columnName.ToLower())
                //    {
                if (listColumns.Any(x => x.Item1 == i))
                {
                    var listColumnsItem = listColumns.First(x => x.Item1 == i);
                    var column = listColumnsItem.Item2;
                    // string columnName = reader.Table.Columns[i].ColumnName;

                    string value = "";
                    if (reader[i] != DBNull.Value)
                        value = reader[i].ToString();
                    else
                        value = null;

                    var property = new EntityInstanceProperty(column.Column);
                    //               property.EntityListViewColumnsID = listColumnsItem.Item2.ID;

                    property.ListViewColumn = column;
                    if (column.RelationshipTail != null)
                        property.RelationshipIDTailPath = column.RelationshipTail.RelationshipIDPath;
                    //property.Name = column.Column.Name;
                    property.RelativeName = column.RelativeColumnName;
                    property.Value = value;
                    //if (column.RelationshipTailID == 0)
                    //{

                    //}
                    result.Properties.Add(property);
                }



                //DP_DataViewItem viewItem;
                //if (columnName.Contains(">"))
                //{
                //    var splt = columnName.Split('>');
                //    int tailID = Convert.ToInt32(splt[0]);
                //    viewItem = GetViewItem(result, tailID);
                //    var tailKeyColumn = tailKeyColumns.First(x => x.Item1 == tailID);
                //    viewItem.EntityID = tailKeyColumn.Item2;
                //    viewItem.KeyProperties = tailKeyColumn.Item3;
                //}
                //else
                //    viewItem = GetViewItem(result, 0);



            }

            //if (!columnName.Contains(","))
            //{
            //    var column = entity.Columns.First(x => x.Name == columnName);

            //    property.IsKey = column.PrimaryKey;
            //    property.ColumnID = column.ID;

            //}
            //else
            //{


            //}
            //result.Properties.Add(property);
            //}
            //    }
            //    //}
            //}


            return result;
        }

        //private DP_DataViewItem GetViewItem(DP_DataView result, int tailID)
        //{
        //    var viewItem = result.DataViewItems.FirstOrDefault(x => x.RelationshipTailID == tailID);
        //    if (viewItem == null)
        //    {
        //        viewItem = new DP_DataViewItem() { RelationshipTailID = tailID };
        //        result.DataViewItems.Add(viewItem);
        //    }
        //    return viewItem;
        //}
        public DR_ResultSearchKeysOnly Process(DR_SearchKeysOnlyRequest request)
        {
            DR_ResultSearchKeysOnly result = new DR_ResultSearchKeysOnly();
            try
            {
                BizEntityListView bizEntityListView = new BizEntityListView();
                var listView = bizEntityListView.GetEntityKeysListView(request.Requester, request.SearchDataItem.TargetEntityID);

                var dataTable = GetDataTableBySearchDataItems(request.Requester, request.SearchDataItem.TargetEntityID, request.SearchDataItem, request.SecurityMode, listView, 0);
                result.ResultDataItems = DataTableToDP_ViewRepository(dataTable.Item1, dataTable.Item2, dataTable.Item3);
                result.Result = Enum_DR_ResultType.SeccessfullyDone;
            }
            catch (Exception ex)
            {
                result.Result = Enum_DR_ResultType.ExceptionThrown;
                result.Message = "خطا در جستجو" + Environment.NewLine + ex.Message;
            }
            return result;
        }
        public DR_ResultSearchFullData Process(DR_SearchFullDataRequest request)
        {
            DR_ResultSearchFullData result = new DR_ResultSearchFullData();
            try
            {
                result.ResultDataItems = GetFullDataResult(request.Requester, request.SearchDataItem, request.SecurityMode);
                result.Result = Enum_DR_ResultType.SeccessfullyDone;
            }
            catch (Exception ex)
            {
                result.Result = Enum_DR_ResultType.ExceptionThrown;
                result.Message = "خطا در جستجو" + Environment.NewLine + ex.Message;
            }
            return result;
        }
        public DR_ResultSearchFullData Process(DR_SearchEditRequest request)
        {
            DR_ResultSearchFullData result = new DR_ResultSearchFullData();
            try
            {
                result.ResultDataItems = GetFullDataResult(request.Requester, request.SearchDataItem, request.SecurityMode);
                DoBeforeLoadActionActivities(request, result);
            }
            catch (Exception ex)
            {
                result.Result = Enum_DR_ResultType.ExceptionThrown;
                result.Message = "خطا در جستجو" + Environment.NewLine + ex.Message;
            }
            return result;
        }

        private List<DP_DataRepository> GetFullDataResult(DR_Requester requester, DP_SearchRepository searchDataItem, SecurityMode securityMode)
        {
            BizEntityListView bizEntityListView = new BizEntityListView();
            var editListView = bizEntityListView.GetEntityEditListView(requester, searchDataItem.TargetEntityID);
            var dataTable = GetDataTableBySearchDataItems(requester, searchDataItem.TargetEntityID, searchDataItem, securityMode, editListView, 0);
            return DataTableToDP_DataRepository(dataTable.Item1, dataTable.Item2, dataTable.Item3);
        }

        public void DoBeforeLoadActionActivities(DR_SearchEditRequest request, DR_ResultSearchFullData result)
        {
            //بازدارنده بودن اقدام کنترل شود
            BizBackendBackendActionActivity bizActionActivity = new BizBackendBackendActionActivity();
            var actionActivities = bizActionActivity.GetActionActivities(request.SearchDataItem.TargetEntityID, new List<Enum_EntityActionActivityStep>() { Enum_EntityActionActivityStep.BeforeLoad }, true);
            CodeFunctionHandler codeFunctionHelper = new CodeFunctionHandler();
            foreach (var entityActionActivity in actionActivities)
            {
                if (entityActionActivity.CodeFunctionID != 0)
                {
                    var resultFunction = codeFunctionHelper.GetCodeFunctionResult(request.Requester, entityActionActivity.CodeFunctionID, result.ResultDataItems);
                    if (resultFunction.ExecutionException == null)
                    {

                    }
                    else
                    {
                        result.Result = Enum_DR_ResultType.ExceptionThrown;
                        result.Message += (string.IsNullOrEmpty(result.Message) ? "" : Environment.NewLine) + resultFunction.ExecutionException.Message;
                    }
                }

            }

        }

        //private List<DP_DataRepository> GetDataItemsByListOFSearchProperties(DR_Requester requester, DP_SearchRepository searchDataItem)
        //{


        //    return 
        //}

        //private DP_SearchRepository GetSearchDataItem(int entityID, List<EntityInstanceProperty> properties)
        //{
        //    DP_SearchRepository result = new DP_SearchRepository();
        //    result.TargetEntityID = entityID;
        //    result.Properties = properties;
        //    return result;
        //}


        //private DataTable GetDataTableByListOFSearchProperties(TableDrivedEntityDTO entity, List<List<EntityInstanceProperty>> listProperties)
        //{



        //    //string tableName = bizTableDrivedEntity.GetTableName(entityID);
        //    //var ConnectionString = ConnectoinHelper.GetConnectionString(entityID);
        //    //using (SqlConnection testConn = new SqlConnection(ConnectionString))
        //    //{
        //    //    testConn.Open();
        //    string commandStr = "select top 500 * from " + entity.TableName;
        //    string criteria = entity.Criteria;
        //    string whereClause = "";
        //    foreach (var listItem in listProperties)
        //    {
        //        string where = "";
        //        foreach (var item in listItem)
        //        {
        //            var column = entity.Columns.Where(x => x.ID == item.ColumnID).FirstOrDefault();
        //            if (column != null)
        //                if (!string.IsNullOrEmpty(item.Value) && item.Value.ToLower() != "<null>" && item.Value.ToLower() != "0")
        //                {
        //                    where += (where == "" ? "" : " and ") + column.Name + "='" + item.Value + "'";
        //                }
        //        }
        //        if (where != "")
        //            where = "(" + where + ")";
        //        whereClause += (whereClause == "" ? "" : " or ") + where;
        //    }
        //    if (whereClause != "")
        //        whereClause = "(" + whereClause + ")";
        //    string finalWhereClause = criteria + (string.IsNullOrEmpty(criteria) ? "" : " and ") + whereClause;
        //    commandStr += (finalWhereClause == "" ? "" : " where ") + finalWhereClause;

        //    //List<ColumnDTO> columns = null;
        //    //if (targetColumnIds != null && targetColumnIds.Count > 0)
        //    //    columns = entity.Columns.Where(x => targetColumnIds.Contains(x.ID)).ToList();
        //    //else
        //    //    columns = entity.Columns;


        //    //using (SqlCommand command = new SqlCommand(commandStr, testConn))
        //    //{
        //    //    using (SqlDataReader reader = command.ExecuteReader())
        //    //    {
        //    //        while (reader.Read())
        //    //        {
        //    return ConnectionManager.GetDBHelperByEntityID(entity.ID).ExecuteQuery(commandStr);

        //    //        }
        //    //    }
        //    //}







        //    //}
        //    //return result;
        //}









        //public DR_ResultSearchViewByRelatinoshipTail Process(DR_SearchViewByRelationshipTailRequest request)
        //{
        //    DR_ResultSearchViewByRelatinoshipTail result = new DR_ResultSearchViewByRelatinoshipTail();
        //    RelationshipTailDataManager relationshipTailDataManager = new RelationshipTailDataManager();
        //    var searchDataItem = relationshipTailDataManager.GetTargetSearchItemFromRelationshipTail(request.FirstDataItem, request.RelationshipTail);
        //    //یه نکته بعدا چک شود..باید در طول مسیر مقادیر کلید نیز در سرچ دیتا آیتمها قرار بگیرند
        //    //بدون آنها ممکن است اطلاعات مشابه نیز وجود داشته باشند

        //    var dataTable = GetDataTableBySearchDataItems(request.Requester, searchDataItem.TargetEntityID, searchDataItem, request.SecurityMode, null, request.EntityViewID);
        //    result.ResultDataItems = DataTableToDP_ViewRepository(dataTable.Item1, dataTable.Item2, dataTable.Item3);
        //    result.Result = Enum_DR_ResultType.SeccessfullyDone;
        //    return result;
        //}



        //public DR_ResultSearchEdit Process(DR_SearchByRelationViewRequest request)
        //{
        //    DR_ResultSearchEdit result = new DR_ResultSearchEdit();
        //    var entity = bizTableDrivedEntity.GetTableDrivedEntity(request.EntityID, EntityColumnInfoType.WithSimpleColumns, EntityRelationshipInfoType.WithoutRelationships);
        //    var dataTable = GetDataItems(entity, request.Properties);
        //    result.ResultDataItems = DataTableToDP_DataRepository(entity, dataTable);
        //    result.Result = Enum_DR_ResultType.SeccessfullyDone;
        //    return result;
        //}







        //private List<DP_DataRepository> DataTableToDP_DataRepository(TableDrivedEntityDTO entity, DataTable dataTable)
        //{
        //    List<DP_DataRepository> result = new List<DP_DataRepository>();
        //    foreach (DataRow row in dataTable.Rows)
        //    {
        //        result.Add(ToBaseDataRepository(entity, row));
        //    }
        //    return result;
        //}




        //private string GetTableName(TableDrivedEntity entity)
        //{
        //    if (entity != null)
        //        return (string.IsNullOrEmpty(entity.Table.RelatedSchema) ? "" : entity.Table.RelatedSchema + ".") + entity.Table.Name;
        //    else
        //        return entity.Name;
        //}




        //private DP_DataRepository ToBaseDataRepository(TableDrivedEntityDTO entity, DataRow reader)
        //{

        //    var result = new DP_DataRepository();
        //    result.TargetEntityID = entity.ID;
        //    for (int i = 0; i < reader.Table.Columns.Count; i++)
        //    {
        //        string columnName = reader.Table.Columns[i].ColumnName;
        //        //foreach (var item in result.TypeOrTypeConditions.Select(x => x.Type))
        //        //{
        //        foreach (var itemProperty in entity.Columns)
        //        {
        //            if (itemProperty.Name.ToLower() == columnName.ToLower())
        //            {

        //                var property = new EntityInstanceProperty();
        //                property.IsKey = itemProperty.PrimaryKey;
        //                property.ColumnID = itemProperty.ID;
        //                if (reader[i] != DBNull.Value)
        //                    property.Value = reader[i].ToString();
        //                else
        //                    property.Value = "<Null>";
        //                result.Properties.Add(property);
        //            }
        //        }
        //        //}
        //    }
        //    return result;
        //}

        //public static List<DataAccess.Column> GetColumnList(TableDrivedEntity template)
        //{
        //    if (template.Column == null || template.Column.Count == 0)
        //    {
        //        return template.Table.Column.ToList();
        //    }
        //    else
        //        return template.Column.ToList();
        //}
        //private DP_DataRepository ToDataRepository(SqlDataReader reader)
        //{
        //    //    DataManager.DataPackage.DP_Package result = new DataManager.DataPackage.DP_Package();
        //    //var result = CommonBusiness.BizHelper.Clone<DataManager.DataPackage.DP_Package>(dP_Package);var result

        //}
    }

}

