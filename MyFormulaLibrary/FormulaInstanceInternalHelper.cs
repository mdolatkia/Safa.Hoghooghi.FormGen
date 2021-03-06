﻿using ModelEntites;
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

namespace MyFormulaFunctionStateFunctionLibrary
{
    public class FormulaInstanceInternalHelper
    {

        static BizCodeFunction bizCodeFunction = new BizCodeFunction();
        static BizEntityState bizEntityState = new BizEntityState();
        static BizFormula bizFormula = new BizFormula();
        static BizDatabaseFunction bizDatabaseFunction = new BizDatabaseFunction();
        static BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();

        public static Dictionary<string, MyPropertyInfo> GetProperties(TableDrivedEntityDTO entity, MyPropertyInfo parentPropetyInfo, bool definition)
        {

            Dictionary<string, MyPropertyInfo> m_properties = new Dictionary<string, MyPropertyInfo>();
            //روابط
            foreach (var relationship in entity.Relationships.OrderBy(x => x.Name))
            {
                var name = "";
                if (relationship.TypeEnum == Enum_RelationshipType.OneToMany)
                    name = "OTM_" + relationship.Entity2;// + +relationship.ID;// + "_" + relationship.ID;
                else if (relationship.TypeEnum == Enum_RelationshipType.ManyToOne)
                    name = "MTO_" + relationship.Entity2;// +  + relationship.ID;// + "_" + relationship.ID;
                else
                    name = "OTO_" + relationship.Entity2;// +  + relationship.ID;// + "_" + relationship.ID;
                MyPropertyInfo propertyInfo = GeneratePropertyInfo(entity, PropertyType.Relationship, parentPropetyInfo, relationship.ID, name, relationship);
                //List<MyCustomData> aa;
                //aa.First()
                //else
                //    propertyInfo.RelationshipTail = relationship.ID.ToString();
                if (m_properties.Any(x => x.Key == propertyInfo.Name))
                    SetUniqueName(propertyInfo, m_properties);
                propertyInfo.PropertyRelationship = relationship;
                propertyInfo.Tooltip = relationship.TypeStr + Environment.NewLine + relationship.Name;
                if (relationship.TypeEnum == Enum_RelationshipType.OneToMany)
                    propertyInfo.Type = typeof(List<MyCustomSingleData>);
                else
                    propertyInfo.Type = typeof(MyCustomSingleData);
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
        static I_ExpressionHandler _GetExpressionHandler;
        private static I_ExpressionHandler GetExpressionHandler
        {
            get
            {
                if (_GetExpressionHandler == null)
                    _GetExpressionHandler = new DynamicExpressoExpressionHandler();
                return _GetExpressionHandler;
            }
        }
        private static List<Type> GetRefTypes()
        {
            List<Type> result = new List<Type>();
            result.Add(typeof(NumericHelper));
            result.Add(typeof(StringHelper));
            result.Add(typeof(PersianDateHelper));
            result.Add(typeof(MiladiDateHelper));
            //result.Add(typeof(MyExtensions));

            return result;
        }
        //public static Dictionary<string, Type> GetExpressionBuiltinVariables()
        //{
        //    return GetExpressionHandler.GetExpressionBuiltinVariables();
        //}
        public static I_ExpressionEvaluator GetExpressionEvaluator(DP_DataRepository mainDataItem, DR_Requester requester, bool definition, List<int> usedFormulaIDs)
        {


            var entity = bizTableDrivedEntity.GetPermissionedEntity(requester, mainDataItem.TargetEntityID);
            var properties = GetProperties(entity, null, definition);
            MyCustomSingleData formulaObject = new MyCustomSingleData(GetMainDateItem(requester,mainDataItem), requester, definition, properties, usedFormulaIDs);
            var refTypes = GetRefTypes();
            var variables = GetRefObjects(entity, requester);
            return GetExpressionHandler.GetExpressionEvaluator(formulaObject, refTypes, variables);
        }

        private static DP_DataRepository GetMainDateItem(DR_Requester requester, DP_DataRepository mainDataItem)
        {
            if (!mainDataItem.IsNewItem && MyDataHelper.DataItemPrimaryKeysHaveValue(mainDataItem) && !MyDataHelper.DataItemNonPrimaryKeysHaveValues(mainDataItem))
            {
                SearchRequestManager searchProcessor = new SearchRequestManager();
                DP_SearchRepository searchDataItem = new DP_SearchRepository(mainDataItem.TargetEntityID);
                foreach (var property in mainDataItem.GetProperties())
                    searchDataItem.Phrases.Add(new SearchProperty() { ColumnID = property.ColumnID, Value = property.Value });

                //سکوریتی داده اعمال میشود
                //یعنی ممکن است به خود داده دسترسی نداشته باشد و یا حتی به بعضی از فیلدها و روابط
                DR_SearchFullDataRequest request = new DR_SearchFullDataRequest(requester, searchDataItem);
                var result = searchProcessor.Process(request);
                if (result.Result != Enum_DR_ResultType.ExceptionThrown)
                    return result.ResultDataItems.FirstOrDefault(); // searchProcessor.GetDataItemsByListOFSearchProperties(Requester, searchDataItem).FirstOrDefault();
                else
                    throw (new Exception(result.Message));
            }
            else
                return mainDataItem;
        }

        private static List<object> GetRefObjects(TableDrivedEntityDTO entity, DR_Requester requester)
        {
            List<object> result = new List<object>();
            result.Add(new DBFunctionHelper(entity.DatabaseID, requester));
            return result;
        }


        public static I_ExpressionDelegate GetExpressionDelegate()
        {
            var refTypes = GetRefTypes();
            return GetExpressionHandler.GetExpressionDelegate(refTypes, null);
        }

        public static string GetObjectPrefrix()
        {
            return GetExpressionHandler.GetObjectPrefrix();
        }

    }
    public interface I_ExpressionHandler
    {
        I_ExpressionEvaluator GetExpressionEvaluator(MyCustomSingleData customData, List<Type> refTypes, List<object> variables);
        I_ExpressionDelegate GetExpressionDelegate(List<Type> refTypes, List<object> variables);
        string GetObjectPrefrix();
    }
    public interface I_ExpressionEvaluator
    {
        List<BuiltinRefClass> GetExpressionBuiltinVariables();
        object Calculate(string expression);
    }
    public interface I_ExpressionDelegate
    {
        T GetDelegate<T>(string expression, string key);
    }
    public class BuiltinRefClass
    {
        public Type Type { set; get; }
        public bool IsType { set; get; }
        public bool IsObject { set; get; }
        public string Name { set; get; }
    }
}
