using ModelEntites;
using MyDataSearchManagerBusiness;
using MyRelationshipDataManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFormulaFunctionStateFunctionLibrary
{
    public class DataitemRelatedColumnValueHandler
    {
        public string GetValueSomeHow(DR_Requester requester, DP_DataRepository sentdata, EntityRelationshipTailDTO valueRelationshipTail, int valueColumnID)
        {
            if (valueRelationshipTail == null)
            {
                var proprty = sentdata.GetProperty(valueColumnID);
                return proprty?.Value;
            }
            else
            {
                DP_DataRepository relatedData = null;
                if (sentdata.ParantChildRelationshipInfo != null && sentdata.ParantChildRelationshipInfo.Relationship.PairRelationshipID == valueRelationshipTail.Relationship.ID)
                {
                    if (sentdata.ParantChildRelationshipInfo.Relationship.PairRelationshipID == valueRelationshipTail.Relationship.ID)
                        relatedData = sentdata.ParantChildRelationshipInfo.SourceData;
                }
                else if (sentdata.ChildRelationshipInfos.Any(x => x.Relationship.ID == valueRelationshipTail.Relationship.ID))
                {
                    var childInfo = sentdata.ChildRelationshipInfos.First(x => x.Relationship.ID == valueRelationshipTail.Relationship.ID);
                    if (childInfo.RelatedData.Count > 1)
                    {
                        throw new Exception("asav");
                    }
                    else if (childInfo.RelatedData.Count == 1)
                        relatedData = childInfo.RelatedData.First();
                    else if (childInfo.RelatedData.Count == 0)
                    {
                        //یعنی یا داده مرتبطی وجود نداشته یا حذف شده
                        return "";
                    }
                }
                if (relatedData != null)
                    return GetValueSomeHow(requester, relatedData, valueRelationshipTail.ChildTail, valueColumnID);
                else
                {
                    //var columnValues = sentdata.KeyProperties;
                    //if (columnValues == null || columnValues.Count == 0)
                    //    throw new Exception("asasd");

                    //سکوریتی داده اعمال میشود
                    //  var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();

                    //این شرط منطقیه؟؟
                    if (!sentdata.IsNewItem)
                    {
                        var relationshipTailDataManager = new RelationshipTailDataManager();
                        var searchDataTuple = relationshipTailDataManager.GetTargetSearchItemFromRelationshipTail(sentdata, valueRelationshipTail);
                        DR_SearchFullDataRequest request = new DR_SearchFullDataRequest(requester, searchDataTuple);
                        var searchResult = new SearchRequestManager().Process(request);
                        if (searchResult.ResultDataItems.Count > 1)
                        {
                            throw new Exception("asdasd");
                        }
                        else if (searchResult.ResultDataItems.Count == 1)
                        {
                            var foundDataItem = searchResult.ResultDataItems.First();
                            var prop = foundDataItem.GetProperty(valueColumnID);
                            if (prop != null)
                                return prop.Value;
                            else
                                return "";
                        }
                        else
                            return "";
                    }
                    else
                        return "";
                }

            }
            //return "";
        }
    }
}
