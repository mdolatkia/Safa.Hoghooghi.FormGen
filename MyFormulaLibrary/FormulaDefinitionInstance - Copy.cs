using MyModelManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelEntites;
using System.Collections;
using System.Linq.Expressions;

using ProxyLibrary;
using MyDataManagerBusiness;
using System.Threading;
using System.Windows;

using MyConnectionManager;
using MyDataSearchManagerBusiness;
using System.ComponentModel;
using MyCodeFunctionLibrary;
using MyDatabaseFunctionLibrary;
using Telerik.Windows.Controls;

namespace MyFormulaFunctionStateFunctionLibrary
{
    public class FormulaDefinitionInstance
    {
        public List<MyPropertyInfo> Properties = new List<MyPropertyInfo>();
        //  public event PropertyChangedEventHandler PropertySetChanged;
        public event EventHandler<PropertyGetArg> PropertyGetCalled;
        //   public event PropertyChangedEventHandler PropertyChanged;
        public FormulaDefinitionInstance(DR_Requester requester, DP_DataRepository mainObject,TableDrivedEntityDTO mainEntity)
        {
            Requester = requester;
            SetMainFormulaObject(mainObject, mainEntity);
        }
        public FormulaObject MainFormulaObject = null;
        DR_Requester Requester;
        private void SetMainFormulaObject(DP_DataRepository mainDataItem , TableDrivedEntityDTO mainEntity)
        {

            MainFormulaObject = GetMainFormulaObject(mainDataItem);
         
            var properties = FormulaInstanceInternalHelper.GetProperties(mainEntity, null, true,true);
            MainFormulaObject.SetProperties(properties);
            MainFormulaObject.PropertyGetCalled += BindableTypeDescriptor_PropertyGetCalled;
            //MainFormulaObject.PropertySetChanged += FormulaObject_PropertySetChanged;
            //MainFormulaObject.PropertyChanged += FormulaObject_PropertyChanged;
        }
        private FormulaObject GetMainFormulaObject(DP_DataRepository mainDataItem)
        {
            FormulaObject formulaObject = new FormulaObject();
            formulaObject.DataItem = mainDataItem;
            return formulaObject;
        }
        BizCodeFunction bizCodeFunction = new BizCodeFunction();
        BizEntityState bizEntityState = new BizEntityState();
        BizFormula bizFormula = new BizFormula();
        BizDatabaseFunction bizDatabaseFunction = new BizDatabaseFunction();
        DatabaseFunctionHandler DatabaseFunctionHandler = new DatabaseFunctionHandler();
        FormulaFunctionHandler FormulaFunctionHandler = new FormulaFunctionHandler();
        StateHandler StateFunctionHandler = new StateHandler();
        CodeFunctionHandler CodeFunctionHandler = new CodeFunctionHandler();



        BizTableDrivedEntity bizTableDrivedEntity = new BizTableDrivedEntity();
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
        public List<int> databaseIDs = new List<int>();
        //public List<EntityAndProperties> AllRegisterdProperties = new List<EntityAndProperties>();
        //public Dictionary<string, MyPropertyInfo> GetProperties(DR_Requester requester, FormulaObject formulaObject, MyPropertyInfo parentPropetyInfo)
        //{
        //    var entity = bizTableDrivedEntity.GetPermissionedEntity(requester, formulaObject.DataItem.TargetEntityID);
        //        if (!databaseIDs.Any(x => x == entity.DatabaseID))
        //            databaseIDs.Add(entity.DatabaseID);
        //    var properties = FormulaInstanceInternalHelper.GetProperties(entity, parentPropetyInfo, formulaObject, true);
        //    //var entityRecord = AllRegisterdProperties.FirstOrDefault(x => x.Entity.ID == entity.ID);
        //    //if (entityRecord == null)
        //    //{
        //    //    entityRecord = new MyFormulaFunctionStateFunctionLibrary.EntityAndProperties();
        //    //    entityRecord.Entity = entity;
        //    //    AllRegisterdProperties.Add(entityRecord);
        //    //}
        //    //foreach (var property in properties)
        //    //{
        //    //    if (!entityRecord.Properties.Any(x => x.ID == property.Value.ID && x.Name == property.Value.Name))
        //    //        entityRecord.Properties.Add(property.Value);

        //    //}
        //    return properties;
        //}





        //private void AddPropertyTree(MyPropertyInfo parentPropetyInfo, MyPropertyInfo propertyInfo)
        //{
        //    if (parentPropetyInfo != null)
        //    {
        //        if (!parentPropetyInfo.ChildProperties.Any(x => x.PropertyType == propertyInfo.PropertyType && x.ID == propertyInfo.ID))
        //            parentPropetyInfo.ChildProperties.Add(propertyInfo);
        //    }
        //    else
        //    {
        //        if (!Properties.Any(x => x.PropertyType == propertyInfo.PropertyType && x.ID == propertyInfo.ID))
        //            Properties.Add(propertyInfo);
        //    }
        //}

        //public MyPropertyInfo GetObjectPropertyInfo(string objectName)
        //{
        //    return RelationshipProperties.FirstOrDefault(x => x.Name == objectName);
        //}


        //public object GetRelationshipPropertyDefaultValue(FormulaObject parentBindableTypeDescriptor, MyPropertyInfo propertyInfo, int entityID)
        //{

        //    //else
        //    //    return null;
        //}
        private void BindableTypeDescriptor_PropertyGetCalled(object sender, PropertyGetArg e)
        {

            if (e.PropertyInfo.ValueSearched == false)
            {
                if (e.PropertyInfo.PropertyType == PropertyType.Relationship)
                {
                    //روابط به طور پیش فرض موقع ساخته شدن چه تعریف فرمول و چه محاسبه مقدار نمیگیرند و اینجا مقدار میگیرند 
                    e.PropertyInfo.ValueSearched = true;

                    var newObject = FormulaInstanceInternalHelper.GetNewFormulaObject(e.PropertyInfo);
                    newObject.DataItem = new DP_DataRepository(e.PropertyInfo.PropertyRelationship.EntityID2, e.PropertyInfo.PropertyRelationship.Entity2);
                    newObject.PropertyGetCalled += BindableTypeDescriptor_PropertyGetCalled;
                    //newObject.PropertySetChanged += FormulaObject_PropertySetChanged;
                    //newObject.PropertyChanged += FormulaObject_PropertyChanged;
                    var entity = bizTableDrivedEntity.GetPermissionedEntity(Requester, newObject.DataItem.TargetEntityID);

                    if (e.PropertyInfo.PropertyRelationship.TypeEnum == Enum_RelationshipType.OneToMany)
                    {
                        var list = FormulaInstanceInternalHelper.GetNewFormulaObjectList(e.PropertyInfo);
                        list.Add(newObject);
                        e.PropertyInfo.Value = list;


                        foreach (FormulaObject item in (e.PropertyInfo.Value as IList))
                        {  //بهتر نوشته شود.برای لیست لازم نیست هر دفعه خصوصیات خوانده شوند
                            if (item.PropertiesLoaded == false)
                            {

                                var properties = FormulaInstanceInternalHelper.GetProperties(entity, e.PropertyInfo,  true,false);
                                item.SetProperties(properties);
                            }
                        }
                    }
                    else
                    {
                        e.PropertyInfo.Value = newObject;

                        if ((e.PropertyInfo.Value as FormulaObject).PropertiesLoaded == false)
                        {
                            var properties = FormulaInstanceInternalHelper.GetProperties(entity, e.PropertyInfo,  true,false);
                            (e.PropertyInfo.Value as FormulaObject).SetProperties(properties);
                        }
                    }
                }
                else
                {
                    throw new Exception("Sfsdf");
                }
            }
            //برای همه پراپرتی ها صدا زده میشود که در قسمت ساختن درخت خصوصیات استفاده میشود
            if (PropertyGetCalled != null)
                PropertyGetCalled(sender, e);
        }

      
        //public MyPropertyInfo GetEntityProperty(string entityName, string propertyName)
        //{
        //    entityName = entityName.ToLower();
        //    if (entityName.StartsWith("otorel"))
        //    {
        //        entityName = entityName.Substring(6, entityName.Length - 6);
        //    }
        //    else if (entityName.StartsWith("otmrel"))
        //    {
        //        entityName = entityName.Substring(6, entityName.Length - 6);
        //    }
        //    else if (entityName.StartsWith("mtorel"))
        //    {
        //        entityName = entityName.Substring(6, entityName.Length - 6);
        //    }
        //    if (entityName.EndsWith("1") || entityName.EndsWith("2") || entityName.EndsWith("3") || entityName.EndsWith("4"))
        //    {
        //        entityName = entityName.Substring(0, entityName.Length - 1);
        //    }
        //    var entityRecord = AllRegisterdProperties.FirstOrDefault(x => x.Entity.Name.ToLower() == entityName);
        //    if (entityRecord != null)
        //        return entityRecord.Properties.FirstOrDefault(x => x.Name == propertyName);
        //    else
        //    {
        //        //اینجا ایراد داره ممکنه مشابه پیدا بشه
        //        var entityID = bizTableDrivedEntity.GetTableDrivedEntityIDByName(Requester, entityName);
        //        if (entityID != 0)
        //        {

        //            //فقط برای اینکه لیست ساخته شود
        //            var fObject = new FormulaObject() { DataItem = new DP_DataRepository(entityID, "") };
        //            GetProperties(Requester, fObject, null);
        //            entityRecord = AllRegisterdProperties.FirstOrDefault(x => x.Entity.ID == entityID);
        //            if (entityRecord != null)
        //                return entityRecord.Properties.FirstOrDefault(x => x.Name == propertyName);
        //        }
        //    }
        //    return null;
        //}
        public MyPropertyInfo GetProperty(MyPropertyInfo previousObject, string propertyName)
        {
            if (previousObject != null)
                return previousObject.ChildProperties.FirstOrDefault(x => x.Name == propertyName);
            else
                return Properties.FirstOrDefault(x => x.Name == propertyName);

        }

        public bool IsObject(string previousWord)
        {
            if (previousWord.Contains("OTOREL") || previousWord.Contains("OTMREL") || previousWord.Contains("MTOREL"))
                return true;
            else
                return false;
        }







        //private void NewObject_PropertyGetCalled(object sender, PropertyGetArg e, FormulaObject parentBindableTypeDescriptor)
        //{
        //    parentBindableTypeDescriptor.OnPropertyGetCalled((sender as FormulaObject), e);
        //}

        //private void NewObject_PropertySetChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e, FormulaObject parentBindableTypeDescriptor)
        //{
        //    parentBindableTypeDescriptor.OnPropertySetChanged(sender, e);
        //}

        //public FormulaObject GetNewFormulaObject(FormulaObject parentFormulaObject, MyPropertyInfo propertyInfo)
        //{
        //    return new FormulaObject();
        //    //////FormulaObject result = null;
        //    ////////short propertyInfo.RelationshipLevel = 0;
        //    ////////if (propertyInfo != null)
        //    ////////    propertyInfo.RelationshipLevel = propertyInfo.RelationshipLevel;
        //    //////if (propertyInfo.RelationshipLevel == 0)
        //    //////    result = new BindableTypeDescriptor<tempClass1>();
        //    //////else if (propertyInfo.RelationshipLevel == 1)
        //    //////    result = new BindableTypeDescriptor<tempClass2>();
        //    //////else if (propertyInfo.RelationshipLevel == 2)
        //    //////    result = new BindableTypeDescriptor<tempClass3>();
        //    //////else if (propertyInfo.RelationshipLevel == 3)
        //    //////    result = new BindableTypeDescriptor<tempClass4>();
        //    //////else if (propertyInfo.RelationshipLevel == 4)
        //    //////    result = new BindableTypeDescriptor<tempClass5>();
        //    //////else if (propertyInfo.RelationshipLevel == 5)
        //    //////    result = new BindableTypeDescriptor<tempClass6>();
        //    //////else if (propertyInfo.RelationshipLevel == 6)
        //    //////    result = new BindableTypeDescriptor<tempClass7>();
        //    //////else if (propertyInfo.RelationshipLevel == 7)
        //    //////    result = new BindableTypeDescriptor<tempClass8>();
        //    //////else if (propertyInfo.RelationshipLevel == 8)
        //    //////    result = new BindableTypeDescriptor<tempClass9>();

        //    ////////var aa = (result as BindableTypeDescriptor<tempClass3>).WordCount();
        //    //////return result;

        //}

        //public IList GetNewFormulaObjectList(MyPropertyInfo propertyInfo)
        //{
        //    return new List<FormulaObject>() { new FormulaObject() };
        //    //var propertyInfo.RelationshipLevel = parentFormulaObject.RelationshipLevel;
        //    //if (propertyInfo.RelationshipLevel == 0)
        //    //    return new List<BindableTypeDescriptor<tempClass1>>();
        //    //else if (propertyInfo.RelationshipLevel == 1)
        //    //    return new List<BindableTypeDescriptor<tempClass2>>();
        //    //else if (propertyInfo.RelationshipLevel == 2)
        //    //    return new List<BindableTypeDescriptor<tempClass3>>();
        //    //else if (propertyInfo.RelationshipLevel == 3)
        //    //    return new List<BindableTypeDescriptor<tempClass4>>();
        //    //else if (propertyInfo.RelationshipLevel == 4)
        //    //    return new List<BindableTypeDescriptor<tempClass5>>();
        //    //else if (propertyInfo.RelationshipLevel == 5)
        //    //    return new List<BindableTypeDescriptor<tempClass6>>();
        //    //else if (propertyInfo.RelationshipLevel == 6)
        //    //    return new List<BindableTypeDescriptor<tempClass7>>();
        //    //else if (propertyInfo.RelationshipLevel == 7)
        //    //    return new List<BindableTypeDescriptor<tempClass8>>();
        //    //else if (propertyInfo.RelationshipLevel == 8)
        //    //    return new List<BindableTypeDescriptor<tempClass9>>();
        //    //return null;
        //}






        //public object CalculateFormula(int formulaID)
        //{
        //    var formula = bizFormula.GetFormula(formulaID,false);
        //    return CalculateExpression(formula.Formula);
        //}


        public object CalculateExpression(string expressionStr)
        {

            var expression = GetExpression(expressionStr);
            dynamic dynamicExpression = expression;
            dynamic compiledExpression = dynamicExpression.Compile();

            bool hasParameters = dynamicExpression.Parameters.Count > 0;
            //if (mainFormulaObjectOrList is IList)
            //    return hasParameters ? compiledExpression(mainFormulaObjectOrList as List<FormulaObject>) : compiledExpression();
            //else
            return hasParameters ? compiledExpression(MainFormulaObject as FormulaObject) : compiledExpression();

        }
        private System.Linq.Expressions.Expression GetExpression(string expressionStr)
        {
            System.Linq.Expressions.Expression expressionResult = null;
            RadExpressionEditor editor = new RadExpressionEditor();
            //if (MainFormulaObject is IList)
            //    editor.Item = MainFormulaObject as List<FormulaObject>;
            //else
            editor.Item = MainFormulaObject as FormulaObject;
            editor.TryParse(expressionStr, out expressionResult);
            return expressionResult;
        }

        public List<FormulaItemDTO> GetFormulaItems(string expressionStr)
        {
            var itemsHelper = new FormulaInstanceItemsHelper();

            return itemsHelper.GetFormulaItems(this, expressionStr);
        }



    }
    
    //public enum BindableTypeDescriptorIntention
    //{
    //    FormulaDefinition,
    //    FormulaDefinitionWithKeyValues,
    //    FormulaCalculation
    //}


}
