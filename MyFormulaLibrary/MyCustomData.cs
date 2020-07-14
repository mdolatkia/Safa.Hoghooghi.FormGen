﻿using ModelEntites;
using MyCodeFunctionLibrary;
using MyDataSearchManagerBusiness;
using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyFormulaFunctionStateFunctionLibrary
{
    public class MyCustomData : DynamicObject
    {
        CodeFunctionHandler CodeFunctionHandler = new CodeFunctionHandler();

        FormulaFunctionHandler FormulaFunctionHandler = new FormulaFunctionHandler();
        StateHandler StateHandler = new StateHandler();
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        Dictionary<string, MyPropertyInfo> m_properties = new Dictionary<string, MyPropertyInfo>();
        public IReadOnlyDictionary<string, MyPropertyInfo> Properties { get { return m_properties; } }
        DP_DataRepository DataItem { set; get; }
        DR_Requester Requester { set; get; }
        bool Definition { set; get; }
        List<int> UsedFormulaIDs = new List<int>();
        public MyCustomData(DP_DataRepository dataItem, DR_Requester requester, bool definition, Dictionary<string, MyPropertyInfo> properties, List<int> usedFormulaIDs)
        {
            DataItem = dataItem;
            Requester = requester;
            Definition = definition;
            m_properties = properties;
            UsedFormulaIDs = usedFormulaIDs;
        }
        internal void SetProperties(Dictionary<string, MyPropertyInfo> properties)
        {
            m_properties = properties;
        }
        public override bool TryGetMember(GetMemberBinder binder,
                                out object result)
        {
            if (m_properties.ContainsKey(binder.Name))
            {
                var property = m_properties.FirstOrDefault(x => x.Key == binder.Name).Value;
                if (!property.ValueSearched)
                {
                    property.ValueSearched = true;
                    if (Definition)
                    {
                        if (property.PropertyType == PropertyType.Relationship)
                        {

                            var dataItem = new DP_DataRepository(property.PropertyRelationship.EntityID2, property.PropertyRelationship.Entity2);
                            var entity = bizTableDrivedEntity.GetPermissionedEntity(Requester, dataItem.TargetEntityID);
                            var properties = FormulaInstanceInternalHelper.GetProperties(entity, property, Definition);
                            var newObject = new MyCustomData(dataItem, Requester, Definition, properties, UsedFormulaIDs);

                            //newObject.PropertyGetCalled += BindableTypeDescriptor_PropertyGetCalled;
                            //newObject.PropertySetChanged += FormulaObject_PropertySetChanged;
                            //newObject.PropertyChanged += FormulaObject_PropertyChanged;
                            if (property.PropertyRelationship.TypeEnum == Enum_RelationshipType.OneToMany)
                            {
                                var list = new List<MyCustomData>();
                                list.Add(newObject);
                                property.Value = list;

                            }
                            else
                            {
                                property.Value = newObject;
                            }
                        }
                        else
                        {
                            property.Value = GetPropertyDefaultValue(property);
                        }
                    }
                    else
                    {
                        if (property.PropertyType == PropertyType.Relationship)
                        {
                            var entity = bizTableDrivedEntity.GetPermissionedEntity(Requester, property.PropertyRelationship.EntityID2);
                            var properties = FormulaInstanceInternalHelper.GetProperties(entity, property, false);
                            List<DP_DataRepository> relatedDataItems = GetRelatedDataItems(DataItem, property.PropertyRelationship, property.PropertyRelationshipProperties);
                            if (property.PropertyRelationship.TypeEnum == Enum_RelationshipType.OneToMany)
                            {
                                var list = new List<MyCustomData>();

                                foreach (var relatedDataItem in relatedDataItems)
                                {
                                    var newObject = new MyCustomData(relatedDataItem, Requester, Definition, properties, UsedFormulaIDs);
                                    list.Add(newObject);
                                    //newObject.PropertyGetCalled += BindableTypeDescriptor_PropertyGetCalled;
                                }
                                property.Value = list;
                            }
                            else if (relatedDataItems.Any())
                            {
                            
                                var newObject = new MyCustomData(relatedDataItems.First(), Requester, Definition, properties, UsedFormulaIDs);

                                //newObject.PropertyGetCalled += BindableTypeDescriptor_PropertyGetCalled;
                                //newObject.PropertySetChanged += FormulaObject_PropertySetChanged;
                                //newObject.PropertyChanged += FormulaObject_PropertyChanged;
                                property.Value = newObject;
                            }
                        }
                        else
                        {

                            if (property.PropertyType == PropertyType.Column)
                            {
                                EntityInstanceProperty dataproperty = DataItem.GetProperty(property.ID);
                                if (property != null)
                                {
                                    property.Value = property.Value;
                                }
                                else
                                    throw new Exception("Date property" + " " + property.Name + " not found!");
                            }
                            else if (property.PropertyType == PropertyType.FormulaParameter)
                            {
                                if (UsedFormulaIDs != null && UsedFormulaIDs.Contains(property.ParameterFormulaID))
                                    throw new Exception("Loop");
                                UsedFormulaIDs.Add(property.ParameterFormulaID);
                                Application.Current.Dispatcher.Invoke((Action)delegate
                                {

                                    var value = FormulaFunctionHandler.CalculateFormula(property.ParameterFormulaID, DataItem, Requester, false, UsedFormulaIDs);
                                    if (string.IsNullOrEmpty(value.Exception))
                                    {
                                        //if ((property.Context is FormulaDTO) && (property.Context as FormulaDTO).ValueCustomType != ValueCustomType.None)
                                        //    property.Value = GetCustomTypePropertyValue(property, (property.Context as FormulaDTO).ValueCustomType, value.Result);
                                        //else
                                        property.Value = value.Result;
                                        //   e.FormulaUsageParemeters = value.FormulaUsageParemeters;
                                    }
                                    else
                                        throw new Exception(value.Exception);
                                });

                            }
                            else if (property.PropertyType == PropertyType.State)
                            {
                                Application.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    DP_DataRepository dataItem = DataItem;

                                    var stateresult = StateHandler.GetStateResult(property.ID, DataItem, Requester);
                                    if (string.IsNullOrEmpty(stateresult.Message))
                                        property.Value = stateresult.Result;
                                    else
                                        throw new Exception(stateresult.Message);
                                });
                            }
                            else if (property.PropertyType == PropertyType.Code)
                            {
                                Application.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    DP_DataRepository dataItem = DataItem;

                                    var coderesult = CodeFunctionHandler.GetCodeFunctionResult(Requester, property.ID, dataItem);
                                    if (coderesult.ExecutionException == null)
                                    {
                                        property.Value = coderesult.Result;
                                    }
                                    else
                                    {
                                        throw coderesult.ExecutionException;
                                    }

                                    //if ((property.Context is CodeFunctionDTO) && (property.Context as CodeFunctionDTO).ValueCustomType != ValueCustomType.None)
                                    //    property.Value = GetCustomTypePropertyValue(property, (property.Context as CodeFunctionDTO).ValueCustomType, result.Result);
                                    //else


                                });
                            }
                        }
                    }
                }

                result = property.Value;
                return true;

            }
            else
            {
                throw new Exception("Property" + " " + binder.Name + " not found!");
            }
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
        public static object GetPropertyDefaultValue(MyPropertyInfo propertyInfo)
        {
            if (propertyInfo.Type == typeof(long) || propertyInfo.Type == typeof(long?))
                return (long)1;
            else if (propertyInfo.Type == typeof(int?) || propertyInfo.Type == typeof(int))
                return 1;
            else if (propertyInfo.Type == typeof(short?) || propertyInfo.Type == typeof(short))
                return (short)1;
            else if (propertyInfo.Type == typeof(byte?) || propertyInfo.Type == typeof(byte))
                return (byte)1;
            else if (propertyInfo.Type == typeof(double?) || propertyInfo.Type == typeof(double))
                return (double)1;
            else if (propertyInfo.Type == typeof(decimal?) || propertyInfo.Type == typeof(decimal))
                return (decimal)1;
            else if (propertyInfo.Type == typeof(float?) || propertyInfo.Type == typeof(float))
                return (float)1;
            else if (propertyInfo.Type == typeof(Guid) || propertyInfo.Type == typeof(Guid?))
                return propertyInfo.Name;
            else if (propertyInfo.Type == typeof(string))
                return propertyInfo.Name;
            else if (propertyInfo.Type == typeof(DateTime) || propertyInfo.Type == typeof(DateTime?))
                return DateTime.Now;
            else if (propertyInfo.Type == typeof(bool?) || propertyInfo.Type == typeof(bool))
                return true;

            return propertyInfo.Name;
        }
    }
}