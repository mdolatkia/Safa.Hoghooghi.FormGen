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
        public CustomObject MainFormulaObject = null;
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
        private CustomObject GetMainFormulaObject(DP_DataRepository mainDataItem)
        {
            CustomObject formulaObject = new CustomObject();
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
    
        public List<int> databaseIDs = new List<int>();
    

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


                        foreach (CustomObject item in (e.PropertyInfo.Value as IList))
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

                        if ((e.PropertyInfo.Value as CustomObject).PropertiesLoaded == false)
                        {
                            var properties = FormulaInstanceInternalHelper.GetProperties(entity, e.PropertyInfo,  true,false);
                            (e.PropertyInfo.Value as CustomObject).SetProperties(properties);
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




        //public List<FormulaItemDTO> GetFormulaItems(string expressionStr)
        //{
        //    var itemsHelper = new FormulaInstanceItemsHelper();

        //    return itemsHelper.GetFormulaItems(this, expressionStr);
        //}



    }
    
    //public enum BindableTypeDescriptorIntention
    //{
    //    FormulaDefinition,
    //    FormulaDefinitionWithKeyValues,
    //    FormulaCalculation
    //}


}
