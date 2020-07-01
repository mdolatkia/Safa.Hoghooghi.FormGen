using MyModelManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelEntites;
using System.Collections;
using System.Linq.Expressions;
using Telerik.Windows.Controls;
using ProxyLibrary;
using MyDataManagerBusiness;
using System.Threading;
using System.Windows;

using MyConnectionManager;
using MyDataSearchManagerBusiness;
using System.ComponentModel;
using MyCodeFunctionLibrary;
using MyDatabaseFunctionLibrary;
using MyFormulaFunctionStateFunctionLibrary;

namespace MyFormulaFunctionStateFunctionLibrary
{
    public class FormulaInstance
    {
        public event EventHandler<PropertyGetArg> PropertyGetCalled;


        DR_Requester Requester { set; get; }
        public FormulaInstance(DP_DataRepository mainObject, DR_Requester requester, List<int> usedFormulaIDs = null)
        {
            Requester = requester;
            SetMainFormulaObject(mainObject);
            if (usedFormulaIDs != null)
                UsedFormulaIDs = usedFormulaIDs;
        }

        private void SetMainFormulaObject(DP_DataRepository mainDataItem)
        {
            MainFormulaObject = GetMainFormulaObject(mainDataItem);
            var entity = bizTableDrivedEntity.GetPermissionedEntity(Requester, MainFormulaObject.DataItem.TargetEntityID);
            var properties = FormulaInstanceInternalHelper.GetProperties(entity, null,  false, true);
            MainFormulaObject.SetProperties(properties);
            MainFormulaObject.PropertyGetCalled += BindableTypeDescriptor_PropertyGetCalled;
       
        }
        private CustomObject GetMainFormulaObject(DP_DataRepository mainDataItem)
        {
            CustomObject formulaObject = new CustomObject();
            if (!mainDataItem.IsNewItem && MyDataHelper.DataItemPrimaryKeysHaveValue(mainDataItem) && !MyDataHelper.DataItemNonPrimaryKeysHaveValues(mainDataItem))
            {
                SearchRequestManager searchProcessor = new SearchRequestManager();
                DP_SearchRepository searchDataItem = new DP_SearchRepository(mainDataItem.TargetEntityID);
                foreach (var property in mainDataItem.GetProperties())
                    searchDataItem.Phrases.Add(new SearchProperty() { ColumnID = property.ColumnID, Value = property.Value });

                //سکوریتی داده اعمال میشود
                //یعنی ممکن است به خود داده دسترسی نداشته باشد و یا حتی به بعضی از فیلدها و روابط
                DR_SearchFullDataRequest request = new DR_SearchFullDataRequest(Requester, searchDataItem);
                var result = searchProcessor.Process(request);
                if (result.Result == Enum_DR_ResultType.SeccessfullyDone)
                    formulaObject.DataItem = result.ResultDataItems.FirstOrDefault(); // searchProcessor.GetDataItemsByListOFSearchProperties(Requester, searchDataItem).FirstOrDefault();
                else if (result.Result == Enum_DR_ResultType.ExceptionThrown)
                    throw (new Exception(result.Message));
            }
            else
                formulaObject.DataItem = mainDataItem;
            return formulaObject;
        }


        BizFormula bizFormula = new BizFormula();
        BizDatabaseFunction bizDatabaseFunction = new BizDatabaseFunction();

        FormulaFunctionHandler FormulaFunctionHandler = new FormulaFunctionHandler();
        StateHandler StateHandler = new StateHandler();
        CodeFunctionHandler CodeFunctionHandler = new CodeFunctionHandler();
        BizCodeFunction bizCodeFunction = new BizCodeFunction();
        BizEntityState bizEntityState = new BizEntityState();
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        DP_DataRepository MainDataItem;
        List<int> UsedFormulaIDs = new List<int>();
        public CustomObject MainFormulaObject = null;

        private void BindableTypeDescriptor_PropertyGetCalled(object sender, PropertyGetArg e)
        {
            if (e.PropertyInfo.ValueSearched == false)
            {
                if (e.PropertyInfo.PropertyType == PropertyType.Relationship)
                {
                    List<DP_DataRepository> relatedDataItems = GetRelatedDataItems((sender as CustomObject).DataItem, e.PropertyInfo.PropertyRelationship, e.PropertyInfo.PropertyRelationshipProperties);
                    if (e.PropertyInfo.PropertyRelationship.TypeEnum == Enum_RelationshipType.OneToMany)
                    {
                        var list = FormulaInstanceInternalHelper.GetNewFormulaObjectList(e.PropertyInfo);
                        var entity = bizTableDrivedEntity.GetPermissionedEntity(Requester, e.PropertyInfo.PropertyRelationship.EntityID2);

                        foreach (var relatedDataItem in relatedDataItems)
                        {
                            var newObject = FormulaInstanceInternalHelper.GetNewFormulaObject(e.PropertyInfo);
                            newObject.DataItem = relatedDataItem;
                            list.Add(newObject);
                            newObject.PropertyGetCalled += BindableTypeDescriptor_PropertyGetCalled;
                            //newObject.PropertySetChanged += FormulaObject_PropertySetChanged;
                            //newObject.PropertyChanged += FormulaObject_PropertyChanged;
                            //////newObject.PropertySetChanged += (sender1, e1) => NewObject_PropertySetChanged(sender1, e1, sender as FormulaObject);
                            //////newObject.PropertyGetCalled += (sender1, e1) => NewObject_PropertyGetCalled(sender1, e1, sender as FormulaObject);
                        }
                        e.PropertyInfo.Value = list;
                        var properties = FormulaInstanceInternalHelper.GetProperties(entity, e.PropertyInfo, false);

                        foreach (CustomObject item in (e.PropertyInfo.Value as IList))
                        {  //بهتر نوشته شود.برای لیست لازم نیست هر دفعه خصوصیات خوانده شوند
                            if (item.PropertiesLoaded == false)
                            {
                                item.SetProperties(properties);
                            }
                        }
                    }
                    else if (relatedDataItems.Any())
                    {
                        var entity = bizTableDrivedEntity.GetPermissionedEntity(Requester, e.PropertyInfo.PropertyRelationship.EntityID2);
                        var properties = FormulaInstanceInternalHelper.GetProperties(entity, e.PropertyInfo, false);
                        var newObject = FormulaInstanceInternalHelper.GetNewFormulaObject(e.PropertyInfo);
                        newObject.DataItem = relatedDataItems.First();

                        newObject.PropertyGetCalled += BindableTypeDescriptor_PropertyGetCalled;
                        //newObject.PropertySetChanged += FormulaObject_PropertySetChanged;
                        //newObject.PropertyChanged += FormulaObject_PropertyChanged;
                        e.PropertyInfo.Value = newObject;

                        if ((e.PropertyInfo.Value as CustomObject).PropertiesLoaded == false)
                        {
                            (e.PropertyInfo.Value as CustomObject).SetProperties(properties);
                        }
                    }
                    e.PropertyInfo.ValueSearched = true;
                }
                else if (e.PropertyInfo.PropertyType == PropertyType.Column)
                {
                    e.PropertyInfo.ValueSearched = true;
                    EntityInstanceProperty property = (sender as CustomObject).DataItem.GetProperty(e.PropertyInfo.ID);
                    if (property != null)
                    {
                        e.PropertyInfo.Value = property.Value;
                    }
                }
                else if (e.PropertyInfo.PropertyType == PropertyType.FormulaParameter)
                {
                    if (UsedFormulaIDs != null && UsedFormulaIDs.Contains(e.PropertyInfo.ParameterFormulaID))
                        AddException("Loop");
                    UsedFormulaIDs.Add(e.PropertyInfo.ParameterFormulaID);
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        e.PropertyInfo.ValueSearched = true;
                        var value = FormulaFunctionHandler.CalculateFormula(e.PropertyInfo.ParameterFormulaID, (sender as CustomObject).DataItem, Requester,false, UsedFormulaIDs);
                        if (string.IsNullOrEmpty(value.Exception))
                        {
                            //if ((e.PropertyInfo.Context is FormulaDTO) && (e.PropertyInfo.Context as FormulaDTO).ValueCustomType != ValueCustomType.None)
                            //    e.PropertyInfo.Value = GetCustomTypePropertyValue(e.PropertyInfo, (e.PropertyInfo.Context as FormulaDTO).ValueCustomType, value.Result);
                            //else
                            e.PropertyInfo.Value = value.Result;
                            e.FormulaUsageParemeters = value.FormulaUsageParemeters;
                        }
                        else
                            AddException(value.Exception);
                    });

                }
                else if (e.PropertyInfo.PropertyType == PropertyType.State)
                {

                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        DP_DataRepository dataItem = (sender as CustomObject).DataItem;
                        e.PropertyInfo.ValueSearched = true;
                        var result = StateHandler.GetStateResult(e.PropertyInfo.ID, (sender as CustomObject).DataItem, Requester);
                        if (string.IsNullOrEmpty(result.Message))
                            e.PropertyInfo.Value = result.Result;
                        else
                            AddException(result.Message);
                    });


                }
                else if (e.PropertyInfo.PropertyType == PropertyType.Code)
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        DP_DataRepository dataItem = (sender as CustomObject).DataItem;
                        e.PropertyInfo.ValueSearched = true;
                        var result = CodeFunctionHandler.GetCodeFunctionResult(Requester, e.PropertyInfo.ID, dataItem);
                        if (result.ExecutionException == null)
                        {
                            e.PropertyInfo.Value = result.Result;
                        }
                        else
                        {
                            AddException(result.ExecutionException.Message);
                            e.PropertyInfo.Value = "";
                        }

                        //if ((e.PropertyInfo.Context is CodeFunctionDTO) && (e.PropertyInfo.Context as CodeFunctionDTO).ValueCustomType != ValueCustomType.None)
                        //    e.PropertyInfo.Value = GetCustomTypePropertyValue(e.PropertyInfo, (e.PropertyInfo.Context as CodeFunctionDTO).ValueCustomType, result.Result);
                        //else


                    });
                }
            }
            if (PropertyGetCalled != null)
                PropertyGetCalled(sender, e);
        }
        public List<Exception> Exceptions = new List<Exception>();
        private void AddException(string message)
        {
            var exception = new Exception(message);
            Exceptions.Add(exception);
            throw new Exception(message);
        }

        private List<DP_DataRepository> GetRelatedDataItems(DP_DataRepository dataItem, RelationshipDTO relationship, List<EntityInstanceProperty> relationshipProperties)
        {
            List<DP_DataRepository> result = new List<DP_DataRepository>();
            var childRelationshipInfo = dataItem.ChildRelationshipInfos.FirstOrDefault(x => x.Relationship.ID == relationship.ID);
            if (childRelationshipInfo != null)
                result = childRelationshipInfo.RelatedData.ToList();
            else
            {
                bool gotFromParant = false;
                if (dataItem.ParantChildRelationshipInfo != null)
                {
                    //اگر داده پرنت بصورت بازگشتی صدا زده شده باشد از پایگاه داده نمی خواند و سعی میکند از همان داده پرنت هر چه که هست استفاده کند
                    if (dataItem.ParantChildRelationshipInfo.Relationship.TypeEnum != Enum_RelationshipType.ManyToOne)
                    {
                        if (dataItem.ParantChildRelationshipInfo.Relationship.ID == relationship.PairRelationshipID)
                        {
                            result.Add(dataItem.ParantChildRelationshipInfo.SourceData);
                            gotFromParant = true;
                        }
                    }
                }
                if (!gotFromParant)
                {
                    result = new List<DP_DataRepository>();
                    if (!relationshipProperties.Any(x => x.Value == null || string.IsNullOrEmpty(x.Value.ToString())))
                    {
                        SearchRequestManager searchProcessor = new SearchRequestManager();
                        DP_SearchRepository searchItem = new DP_SearchRepository(relationship.EntityID2);
                        foreach (var col in relationshipProperties)
                            searchItem.Phrases.Add(new SearchProperty() { ColumnID = col.ColumnID, Value = col.Value });

                        //سکوریتی داده اعمال میشود
                        DR_SearchFullDataRequest request = new DR_SearchFullDataRequest(Requester, searchItem);

                        var searchResult = searchProcessor.Process(request);
                        if (searchResult.Result == Enum_DR_ResultType.SeccessfullyDone)
                            result = searchResult.ResultDataItems; // searchProcessor.GetDataItemsByListOFSearchProperties(Requester, searchDataItem).FirstOrDefault();
                        else if (searchResult.Result == Enum_DR_ResultType.ExceptionThrown)
                            throw (new Exception(searchResult.Message));

                        //result = searchProcessor.GetDataItemsByListOFSearchProperties(Requester, searchItem);

                        //nullدرست شود
                        childRelationshipInfo = new ChildRelationshipInfo();
                        childRelationshipInfo.SourceData = dataItem;
                        childRelationshipInfo.Relationship = relationship;
                        foreach (var item in result)
                            childRelationshipInfo.RelatedData.Add(item);
                    }
                }
            }
            return result;
        }

    }

    public enum BindableTypeDescriptorIntention
    {
        FormulaDefinition,
        FormulaDefinitionWithKeyValues,
        FormulaCalculation
    }
 

}
