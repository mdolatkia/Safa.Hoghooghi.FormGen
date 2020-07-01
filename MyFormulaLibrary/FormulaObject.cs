using ModelEntites;
using MyCodeFunctionLibrary;
using MyConnectionManager;
using MyDataSearchManagerBusiness;
using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyFormulaFunctionStateFunctionLibrary
{
    public class FormulaObject : CustomObject
    {
        BizFormula bizFormula = new BizFormula();
        CodeFunctionHandler CodeFunctionHandler = new CodeFunctionHandler();
        BizCodeFunction bizCodeFunction = new BizCodeFunction();
        BizEntityState bizEntityState = new BizEntityState();
        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
        List<int> UsedFormulaIDs = new List<int>();
        StateHandler StateHandler = new StateHandler();
        FormulaFunctionHandler FormulaFunctionHandler = new FormulaFunctionHandler();
        bool IsDefinitionFormulaObject { set; get; }
        DR_Requester Requester { set; get; }

        public FormulaObject(DP_DataRepository dataItem, DR_Requester requester, bool isDefinitionFormulaObject = false)
        {
            Requester = requester;
            bool getFromDB = false;
            if (isDefinitionFormulaObject == false)
            {
                if (!dataItem.IsNewItem && MyDataHelper.DataItemPrimaryKeysHaveValue(dataItem) && !MyDataHelper.DataItemNonPrimaryKeysHaveValues(dataItem))
                {
                    getFromDB = true;
                }
            }
            if (getFromDB)
            {
                SearchRequestManager searchProcessor = new SearchRequestManager();
                DP_SearchRepository searchDataItem = new DP_SearchRepository(dataItem.TargetEntityID);
                foreach (var property in dataItem.GetProperties())
                    searchDataItem.Phrases.Add(new SearchProperty() { ColumnID = property.ColumnID, Value = property.Value });

                //سکوریتی داده اعمال میشود
                //یعنی ممکن است به خود داده دسترسی نداشته باشد و یا حتی به بعضی از فیلدها و روابط
                DR_SearchFullDataRequest request = new DR_SearchFullDataRequest(Requester, searchDataItem);
                var result = searchProcessor.Process(request);
                if (result.Result == Enum_DR_ResultType.SeccessfullyDone)
                    dataItem = result.ResultDataItems.FirstOrDefault(); // searchProcessor.GetDataItemsByListOFSearchProperties(Requester, searchDataItem).FirstOrDefault();
                else if (result.Result == Enum_DR_ResultType.ExceptionThrown)
                    throw (new Exception(result.Message));
            }
            DataItem = dataItem;
            this.PropertyGetCalled += FormulaObject_PropertyGetCalled;
            IsDefinitionFormulaObject = isDefinitionFormulaObject;

            var entity = bizTableDrivedEntity.GetPermissionedEntity(Requester, DataItem.TargetEntityID);
            var properties = FormulaInstanceInternalHelper.GetProperties(entity, null, false, true);
            SetProperties(properties);

        }

        private void FormulaObject_PropertyGetCalled(object sender, PropertyGetArg e)
        {
            if (IsDefinitionFormulaObject)
            {
                if (e.PropertyInfo.ValueSearched == false)
                {
                    if (e.PropertyInfo.PropertyType == PropertyType.Relationship)
                    {
                        //روابط به طور پیش فرض موقع ساخته شدن چه تعریف فرمول و چه محاسبه مقدار نمیگیرند و اینجا مقدار میگیرند 
                        e.PropertyInfo.ValueSearched = true;

                        var newObject = FormulaInstanceInternalHelper.GetNewFormulaObject(e.PropertyInfo);
                        newObject.DataItem = new DP_DataRepository(e.PropertyInfo.PropertyRelationship.EntityID2, e.PropertyInfo.PropertyRelationship.Entity2);
                        newObject.PropertyGetCalled += FormulaObject_PropertyGetCalled;
                        //newObject.PropertySetChanged += FormulaObject_PropertySetChanged;
                        //newObject.PropertyChanged += FormulaObject_PropertyChanged;
                        var entity = bizTableDrivedEntity.GetPermissionedEntity(Requester, newObject.DataItem.TargetEntityID);

                        if (e.PropertyInfo.PropertyRelationship.TypeEnum == Enum_RelationshipType.OneToMany)
                        {
                            var list = FormulaInstanceInternalHelper.GetNewFormulaObjectList(e.PropertyInfo);
                            list.Add(newObject);
                            e.PropertyInfo.Value = list;


                            foreach (CustomObject item in (e.PropertyInfo.Value as IList))
                            {  //بهتر نوشته شود.برای لیست لازم نیست هر دفعه خصوصیات خوانده شوند
                                if (item.PropertiesLoaded == false)
                                {

                                    var properties = FormulaInstanceInternalHelper.GetProperties(entity, e.PropertyInfo, true, false);
                                    item.SetProperties(properties);
                                }
                            }
                        }
                        else
                        {
                            e.PropertyInfo.Value = newObject;

                            if ((e.PropertyInfo.Value as CustomObject).PropertiesLoaded == false)
                            {
                                var properties = FormulaInstanceInternalHelper.GetProperties(entity, e.PropertyInfo, true, false);
                                (e.PropertyInfo.Value as CustomObject).SetProperties(properties);
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Sfsdf");
                    }
                }
            }
            else
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
                                newObject.PropertyGetCalled += FormulaObject_PropertyGetCalled;
                                //newObject.PropertySetChanged += FormulaObject_PropertySetChanged;
                                //newObject.PropertyChanged += FormulaObject_PropertyChanged;
                                //////newObject.PropertySetChanged += (sender1, e1) => NewObject_PropertySetChanged(sender1, e1, sender as FormulaObject);
                                //////newObject.PropertyGetCalled += (sender1, e1) => NewObject_PropertyGetCalled(sender1, e1, sender as FormulaObject);
                            }
                            e.PropertyInfo.Value = list;
                            var properties = FormulaInstanceInternalHelper.GetProperties(entity, e.PropertyInfo, false, false);


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
                            var properties = FormulaInstanceInternalHelper.GetProperties(entity, e.PropertyInfo, false, false);
                            var newObject = FormulaInstanceInternalHelper.GetNewFormulaObject(e.PropertyInfo);
                            newObject.DataItem = relatedDataItems.First();

                            newObject.PropertyGetCalled += FormulaObject_PropertyGetCalled;
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
                            var value = FormulaFunctionHandler.CalculateFormula(e.PropertyInfo.ParameterFormulaID, (sender as CustomObject).DataItem, Requester, IsDefinitionFormulaObject, UsedFormulaIDs);
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
            }
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

        public Dictionary<string, MyPropertyInfo> GetProperties(TableDrivedEntityDTO entity, MyPropertyInfo parentPropetyInfo, bool definition, bool withHelpers)
        {

            Dictionary<string, MyPropertyInfo> m_properties = new Dictionary<string, MyPropertyInfo>();
            //روابط
            foreach (var relationship in entity.Relationships.OrderBy(x => x.Name))
            {
                var name = "";
                if (relationship.TypeEnum == Enum_RelationshipType.OneToMany)
                    name = "OTMREL" + relationship.Entity2;// + +relationship.ID;// + "_" + relationship.ID;
                else if (relationship.TypeEnum == Enum_RelationshipType.ManyToOne)
                    name = "MTOREL" + relationship.Entity2;// +  + relationship.ID;// + "_" + relationship.ID;
                else
                    name = "OTOREL" + relationship.Entity2;// +  + relationship.ID;// + "_" + relationship.ID;
                MyPropertyInfo propertyInfo = GeneratePropertyInfo(entity, PropertyType.Relationship, parentPropetyInfo, relationship.ID, name, relationship);

                //else
                //    propertyInfo.RelationshipTail = relationship.ID.ToString();
                if (m_properties.Any(x => x.Key == propertyInfo.Name))
                    SetUniqueName(propertyInfo, m_properties);
                propertyInfo.PropertyRelationship = relationship;
                propertyInfo.Tooltip = relationship.TypeStr + Environment.NewLine + relationship.Name;
                if (relationship.TypeEnum == Enum_RelationshipType.OneToMany)
                    propertyInfo.Type = typeof(List<FormulaObject>);
                else
                    propertyInfo.Type = typeof(FormulaObject);
                m_properties.Add(propertyInfo.Name, propertyInfo);

            }

            foreach (var column in entity.Columns)
            {
                var name = "cl_" + column.Name;
                MyPropertyInfo propertyInfo = GeneratePropertyInfo(entity, PropertyType.Column, parentPropetyInfo, column.ID, name, column);

                //if (column.DateColumnType != null && column.DateColumnType.IsPersianDate)
                //{
                //    propertyInfo.Type = GetCustomTypePropertyType(propertyInfo, ValueCustomType.IsPersianDate);
                //}
                //else
                propertyInfo.Type = column.DotNetType;

                if (definition)
                {
                    //if (column.DateColumnType != null && column.DateColumnType.IsPersianDate)
                    //{
                    //    propertyInfo.Value = GetCustomTypePropertyDefaultValue(propertyInfo, ValueCustomType.IsPersianDate);
                    //}
                    //else
                    //{
                    propertyInfo.Value = GetPropertyDefaultValue(propertyInfo);
                    //}
                    propertyInfo.ValueSearched = true;
                }
                else
                {
                    propertyInfo.Value = null;
                }


                m_properties.Add(propertyInfo.Name, propertyInfo);

            }

            //مدل رو تا فولدر فرمول رفتم یا استیت رابطه داره و استیت هم با فرمول چیکار کنم
            var formulaStates = bizEntityState.GetEntityStates(entity.ID, false);
            foreach (var state in formulaStates)
            {
                var name = "st_" + state.Title;
                MyPropertyInfo propertyInfo = GeneratePropertyInfo(entity, PropertyType.State, parentPropetyInfo, state.ID, name, state);
                propertyInfo.Type = typeof(bool);
                propertyInfo.Name = "st_" + state.Title;
                if (definition)
                {
                    propertyInfo.Value = GetPropertyDefaultValue(propertyInfo);
                    propertyInfo.ValueSearched = true;
                }
                else
                {
                    propertyInfo.Value = null;
                }
                m_properties.Add(propertyInfo.Name, propertyInfo);
            }


            var formulaParameters = bizFormula.GetFormulas(entity.ID);
            foreach (var formulaParameter in formulaParameters)
            {
                var name = "p_" + formulaParameter.Name;
                MyPropertyInfo propertyInfo = GeneratePropertyInfo(entity, PropertyType.FormulaParameter, parentPropetyInfo, formulaParameter.ID, name, formulaParameter);
                propertyInfo.ParameterFormulaID = formulaParameter.ID;
                //if (formulaParameter.ValueCustomType != ValueCustomType.None)
                //{
                //    propertyInfo.Type = GetCustomTypePropertyType(propertyInfo, formulaParameter.ValueCustomType);
                //}
                //else
                propertyInfo.Type = formulaParameter.ResultDotNetType;
                if (definition)
                {
                    //if (formulaParameter.ValueCustomType != ValueCustomType.None)
                    //{
                    //    propertyInfo.Value = GetCustomTypePropertyDefaultValue(propertyInfo, formulaParameter.ValueCustomType);
                    //}
                    //else
                    //{
                    propertyInfo.Value = GetPropertyDefaultValue(propertyInfo);
                    //}
                    propertyInfo.ValueSearched = true;
                }
                else
                {
                    propertyInfo.Value = null;
                }

                m_properties.Add(propertyInfo.Name, propertyInfo);
            }

            //////var databaseFunctions = bizDatabaseFunction.GetDatabaseFunctionsByEntityID(entity.ID);
            //////foreach (var dbfunction in databaseFunctions)
            //////{
            //////    var name = "";
            //////    if (dbfunction.Type == Enum_DatabaseFunctionType.Function)
            //////        name = "fn_" + dbfunction.FunctionName;
            //////    else if (dbfunction.Type == Enum_DatabaseFunctionType.StoredProcedure)
            //////        name = "sp_" + dbfunction.FunctionName;
            //////    MyPropertyInfo propertyInfo = GeneratePropertyInfo(entity, PropertyType.DBFormula, parentPropetyInfo, dbfunction.ID, name, dbfunction, formulaObject);
            //////    //if (dbfunction.ValueCustomType != ValueCustomType.None)
            //////    //{
            //////    //    propertyInfo.Type = GetCustomTypePropertyType(propertyInfo, dbfunction.ValueCustomType);
            //////    //}
            //////    //else
            //////    propertyInfo.Type = dbfunction.DotNetType;
            //////    if (definition)
            //////    {
            //////        //if (dbfunction.ValueCustomType != ValueCustomType.None)
            //////        //{
            //////        //    propertyInfo.Value = GetCustomTypePropertyDefaultValue(propertyInfo, dbfunction.ValueCustomType);
            //////        //}
            //////        //else
            //////        //{
            //////        propertyInfo.Value = GetPropertyDefaultValue(propertyInfo);
            //////        //}

            //////        propertyInfo.ValueSearched = true;

            //////    }
            //////    else
            //////    {
            //////        propertyInfo.Value = null;
            //////    }
            //////    m_properties.Add(propertyInfo.Name, propertyInfo);
            //////}


            var codeFunctions = bizCodeFunction.GetCodeFunctionsByEntityID(entity.ID);
            foreach (var codeFunction in codeFunctions)
            {
                var name = "cd_" + codeFunction.FunctionName;
                MyPropertyInfo propertyInfo = GeneratePropertyInfo(entity, PropertyType.Code, parentPropetyInfo, codeFunction.ID, name, codeFunction);
                //if (codeFunction.ValueCustomType != ValueCustomType.None)
                //{
                //    propertyInfo.Type = GetCustomTypePropertyType(propertyInfo, codeFunction.ValueCustomType);
                //}
                //else
                propertyInfo.Type = codeFunction.RetrunDotNetType;
                if (definition)
                {
                    //if (codeFunction.ValueCustomType != ValueCustomType.None)
                    //{
                    //    propertyInfo.Value = GetCustomTypePropertyDefaultValue(propertyInfo, codeFunction.ValueCustomType);
                    //}
                    //else
                    //{
                    propertyInfo.Value = GetPropertyDefaultValue(propertyInfo);
                    //}
                    propertyInfo.ValueSearched = true;
                }
                else
                {
                    propertyInfo.Value = null;
                }

                //فانکشنها در تعریف  فرمول همینجا مقدار میگیرند 
                m_properties.Add(propertyInfo.Name, propertyInfo);
            }
            if (parentPropetyInfo == null)
            {
                //MyPropertyInfo numericHelperPropertyInfo = GeneratePropertyInfo(entity, PropertyType.Helper, parentPropetyInfo, 0, "NumericHelper", null);
                //numericHelperPropertyInfo.Type = typeof(NumericHelper);
                //numericHelperPropertyInfo.Value = new NumericHelper();
                //numericHelperPropertyInfo.ValueSearched = true;
                //m_properties.Add(numericHelperPropertyInfo.Name, numericHelperPropertyInfo);

                ////StringHelper
                //MyPropertyInfo stringHelperPropertyInfo = GeneratePropertyInfo(entity, PropertyType.Helper, parentPropetyInfo, 0, "StringHelper", null);
                //stringHelperPropertyInfo.Type = typeof(StringHelper);
                //stringHelperPropertyInfo.Value = new StringHelper();
                //stringHelperPropertyInfo.ValueSearched = true;
                //m_properties.Add(stringHelperPropertyInfo.Name, stringHelperPropertyInfo);

                //MyPropertyInfo persinaDateHelperPropertyInfo = GeneratePropertyInfo(entity, PropertyType.Helper, parentPropetyInfo, 0, "PersianDateHelper", null);
                //persinaDateHelperPropertyInfo.Type = typeof(PersianDateHelper);
                //persinaDateHelperPropertyInfo.Value = new PersianDateHelper();
                //persinaDateHelperPropertyInfo.ValueSearched = true;
                //m_properties.Add(persinaDateHelperPropertyInfo.Name, persinaDateHelperPropertyInfo);

                //MyPropertyInfo miladiDateHelperPropertyInfo = GeneratePropertyInfo(entity, PropertyType.Helper, parentPropetyInfo, 0, "MiladiDateHelper", null);
                //miladiDateHelperPropertyInfo.Type = typeof(MiladiDateHelper);
                //miladiDateHelperPropertyInfo.Value = new MiladiDateHelper();
                //miladiDateHelperPropertyInfo.ValueSearched = true;
                //m_properties.Add(miladiDateHelperPropertyInfo.Name, miladiDateHelperPropertyInfo);


                //MyPropertyInfo dbFunctionHelperHelperPropertyInfo = GeneratePropertyInfo(entity, PropertyType.Helper, parentPropetyInfo, 0, "DBFunctionHelper", null);
                //dbFunctionHelperHelperPropertyInfo.Type = typeof(DBFunctionHelper);
                //dbFunctionHelperHelperPropertyInfo.Value = new DBFunctionHelper(entity.DatabaseID);
                //dbFunctionHelperHelperPropertyInfo.ValueSearched = true;
                //m_properties.Add(dbFunctionHelperHelperPropertyInfo.Name, dbFunctionHelperHelperPropertyInfo);
            }
            return m_properties;
        }

        private static MyPropertyInfo GeneratePropertyInfo(TableDrivedEntityDTO entity, PropertyType propertyType, MyPropertyInfo parentProperty, int id, string name, object context)
        {

            MyPropertyInfo myPropertyInfo = new MyFormulaFunctionStateFunctionLibrary.MyPropertyInfo();
            myPropertyInfo.Name = name;
            //   myPropertyInfo.FormulaObject = formulaObject;
            myPropertyInfo.ID = id;
            myPropertyInfo.ParentProperty = parentProperty;
            myPropertyInfo.Context = context;
            myPropertyInfo.PropertyType = propertyType;
            if (parentProperty != null)
            {
                // myPropertyInfo.RelationshipTail = parentProperty.RelationshipTail + (string.IsNullOrEmpty(parentProperty.RelationshipTail) ? "" : ",") + relationship.ID;
                myPropertyInfo.RelationshipLevel = parentProperty.RelationshipLevel + 1;

                if (parentProperty.PropertyType == PropertyType.Relationship)
                {
                    myPropertyInfo.RelationshipTail = parentProperty.RelationshipTail + (string.IsNullOrEmpty(parentProperty.RelationshipTail) ? "" : ",") + parentProperty.ID;
                    myPropertyInfo.RelationshipPropertyTail = parentProperty.RelationshipPropertyTail + (string.IsNullOrEmpty(parentProperty.RelationshipPropertyTail) ? "" : ".") + parentProperty.Name;
                }
            }
            else
            {
                myPropertyInfo.RelationshipTail = "";
                myPropertyInfo.RelationshipPropertyTail = "";
            }
            return myPropertyInfo;
        }

        private static void SetUniqueName(MyPropertyInfo propertyInfo, Dictionary<string, MyPropertyInfo> m_properties, int index = 0)
        {
            if (m_properties.Any(x => x.Key == propertyInfo.Name))
            {
                propertyInfo.Name += (index + 1).ToString();
                SetUniqueName(propertyInfo, m_properties, index + 1);
            }
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
