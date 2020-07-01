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
        //public event PropertyChangedEventHandler PropertySetChanged;
        public event EventHandler<PropertyGetArg> PropertyGetCalled;
        //public event PropertyChangedEventHandler PropertyChanged;


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
            //MainFormulaObject.PropertySetChanged += FormulaObject_PropertySetChanged;
            //MainFormulaObject.PropertyChanged += FormulaObject_PropertyChanged;
        }
        private FormulaObject GetMainFormulaObject(DP_DataRepository mainDataItem)
        {
            FormulaObject formulaObject = new FormulaObject();
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
        public FormulaObject MainFormulaObject = null;

        //private void FormulaObject_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (PropertyChanged != null)
        //        PropertyChanged(sender, e);
        //}

        //private void FormulaObject_PropertySetChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (PropertySetChanged != null)
        //        PropertySetChanged(sender, e);
        //}

        //public Dictionary<string, MyPropertyInfo> GetProperties(DR_Requester requester, FormulaObject formulaObject, MyPropertyInfo parentPropetyInfo)
        //{
        //    var entity = bizTableDrivedEntity.GetPermissionedEntity(requester, formulaObject.DataItem.TargetEntityID);
        //    return FormulaInstanceInternalHelper.GetProperties(entity, parentPropetyInfo, formulaObject, false);
        //}
        private void SetUniqueName(MyPropertyInfo propertyInfo, Dictionary<string, MyPropertyInfo> m_properties, int index = 0)
        {
            if (m_properties.Any(x => x.Key == propertyInfo.Name))
            {
                propertyInfo.Name += (index + 1).ToString();
                SetUniqueName(propertyInfo, m_properties, index + 1);
            }

        }
        private void BindableTypeDescriptor_PropertyGetCalled(object sender, PropertyGetArg e)
        {

            if (e.PropertyInfo.ValueSearched == false)
            {
                if (e.PropertyInfo.PropertyType == PropertyType.Relationship)
                {
                  
                    List<DP_DataRepository> relatedDataItems = GetRelatedDataItems((sender as FormulaObject).DataItem, e.PropertyInfo.PropertyRelationship, e.PropertyInfo.PropertyRelationshipProperties);
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
                        var properties = FormulaInstanceInternalHelper.GetProperties(entity, e.PropertyInfo, false, false);


                        foreach (FormulaObject item in (e.PropertyInfo.Value as IList))
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
                        var properties = FormulaInstanceInternalHelper.GetProperties(entity, e.PropertyInfo, false, false);
                        var newObject = FormulaInstanceInternalHelper.GetNewFormulaObject(e.PropertyInfo);
                        newObject.DataItem = relatedDataItems.First();

                        newObject.PropertyGetCalled += BindableTypeDescriptor_PropertyGetCalled;
                        //newObject.PropertySetChanged += FormulaObject_PropertySetChanged;
                        //newObject.PropertyChanged += FormulaObject_PropertyChanged;

                        e.PropertyInfo.Value = newObject;


                        if ((e.PropertyInfo.Value as FormulaObject).PropertiesLoaded == false)
                        {
                            (e.PropertyInfo.Value as FormulaObject).SetProperties(properties);
                        }
                    }
                    e.PropertyInfo.ValueSearched = true;
                }
                else if (e.PropertyInfo.PropertyType == PropertyType.Column)
                {
                    e.PropertyInfo.ValueSearched = true;
                    EntityInstanceProperty property = (sender as FormulaObject).DataItem.GetProperty(e.PropertyInfo.ID);
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
                        var value = FormulaFunctionHandler.CalculateFormula(e.PropertyInfo.ParameterFormulaID, (sender as FormulaObject).DataItem, Requester, UsedFormulaIDs);
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
                        DP_DataRepository dataItem = (sender as FormulaObject).DataItem;
                        e.PropertyInfo.ValueSearched = true;
                        var result = StateHandler.GetStateResult(e.PropertyInfo.ID, (sender as FormulaObject).DataItem, Requester);
                        if (string.IsNullOrEmpty(result.Message))
                            e.PropertyInfo.Value = result.Result;
                        else
                            AddException(result.Message);
                    });


                }
                //else if (e.PropertyInfo.PropertyType == PropertyType.DBFormula)
                //{
                //    Application.Current.Dispatcher.Invoke((Action)delegate
                //    {
                //        DP_DataRepository dataItem = (sender as FormulaObject).DataItem;
                //        e.PropertyInfo.ValueSearched = true;
                //        var result = DatabaseFunctionHandler.GetDatabaseFunctionValue(Requester, e.PropertyInfo.ID, dataItem);

                //        //if ((e.PropertyInfo.Context is DatabaseFunctionDTO) && (e.PropertyInfo.Context as DatabaseFunctionDTO).ValueCustomType != ValueCustomType.None)
                //        //    e.PropertyInfo.Value = GetCustomTypePropertyValue(e.PropertyInfo, (e.PropertyInfo.Context as DatabaseFunctionDTO).ValueCustomType, result.Result);
                //        //else
                //        e.PropertyInfo.Value = result.Result;

                //    });
                //}
                else if (e.PropertyInfo.PropertyType == PropertyType.Code)
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        DP_DataRepository dataItem = (sender as FormulaObject).DataItem;
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

        //private static object GetCustomTypePropertyValue(MyPropertyInfo propertyInfo, ValueCustomType valueCustomType, object value)
        //{//<NULL> چی مقدار
        //    if (valueCustomType == ValueCustomType.IsPersianDate)
        //    {
        //        return new PersianDate() { Value = value.ToString() };
        //    }
        //    return null;
        //}


        //public FormulaObject GetNewFormulaObject()
        //{
        //    return new FormulaObject();
        //}

        //public IList GetNewFormulaObjectList()
        //{
        //    return new List<FormulaObject>();
        //}

        //public object CalculateFormula(int formulaID)
        //{

        //    return CalculateExpression(formula.Formula);
        //}
        public object CalculateExpression(string expressionStr)
        {
            try
            {
                //  expressionStr = "OTORELLegalPerson.cl_Name";
                //  Param_0.GetProperties().Find("OTORELCustomer", False)
                // var aa = (MainFormulaObject.GetProperties().Find("OTORELLegalPerson", false).GetValue(MainFormulaObject) as ICustomTypeDescriptor).GetProperties().Find("cl_Name", false).GetValue((ICustomTypeDescriptor)(MainFormulaObject.GetProperties().Find("OTORELLegalPerson", false).GetValue(MainFormulaObject)));

                var aa = MainFormulaObject.GetProperties().Find("cl_TotalPrice", false);
                var bb = aa.GetValue(MainFormulaObject);

                var expression = GetExpression(expressionStr);
                dynamic dynamicExpression = expression;

                dynamic compiledExpression = dynamicExpression.Compile();

                bool hasParameters = dynamicExpression.Parameters.Count > 0;

                //aa aaa = new aa();
                //var Param_0 = aaa.GetType();

                //  var expressiona=a      Param_0.GetProperties().First(x=>x.Name== "Helper").GetValue(Param_0)).StringHelper.GetPropertyString(Convert(Param_0.GetProperties().Find("OTORELLegalPerson", False).GetValue(Param_0)))}

                return hasParameters ? compiledExpression(MainFormulaObject) : compiledExpression();
            }
            catch (Exception ex)
            {
                AddException(ex.Message);
                return null;
            }


        }
        private System.Linq.Expressions.Expression GetExpression(string expressionStr)
        {

            System.Linq.Expressions.Expression expressionResult = null;
            RadExpressionEditor editor = new RadExpressionEditor();
            //if (MainFormulaObject is IList)
            //    editor.Item = MainFormulaObject as List<FormulaObject>;
            //else
            editor.Item = MainFormulaObject as FormulaObject;

            //اینجا اکسپشن را درست برنمیگردونه چون try  داره
            editor.TryParse(expressionStr, out expressionResult);
            return expressionResult;

        }

        public List<FormulaItemDTO> GetFormulaItems(string expressionStr)
        {
            //////var itemsHelper = new FormulaInstanceItemsHelper();
            //////return itemsHelper.GetFormulaItems(this, expressionStr);
            return null;
        }


    }

    public enum BindableTypeDescriptorIntention
    {
        FormulaDefinition,
        FormulaDefinitionWithKeyValues,
        FormulaCalculation
    }
    //public class aa
    //{
    //    public aa()
    //    {
    //        Helper = new MyFormulaFunctionStateFunctionLibrary.FormulaHepler();
    //    }
    //    public FormulaHepler Helper { set; get; }
    //    public bb bbb { set; get; }
    //}
    //public class bb
    //{

    //}

}
