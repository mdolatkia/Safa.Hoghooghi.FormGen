using ModelEntites;
using MyGeneralLibrary;
using MyModelManager;
using ProxyLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MyFormulaFunctionStateFunctionLibrary
{
    //یچیزی مثل Type رو بصورت داینامیک شبیه سازی میکند
    public class FormulaObject : INotifyPropertyChanged, ICustomTypeDescriptor
    {
        public event PropertyChangedEventHandler PropertySetChanged;
        public event EventHandler<PropertyGetArg> PropertyGetCalled;
        public event PropertyChangedEventHandler PropertyChanged;
        //public BindableTypeDescriptorIntention Intention;
        //public string RelationshipTail = "";
        //public FormulaObject ParentFormulaObject { set; get; }
        //public int ParentRelationshipID { set; get; }
        //public int RelationshipLevel { set; get; }
        public bool PropertiesLoaded;
        public DP_DataRepository DataItem { set; get; }
        //public List<FormulaParameterDTO> Parameters { set; get; }

        BizFormula bizFormula = new BizFormula();
        //FormulaHelper formulaHelper = new FormulaHelper();


        //public bool HasState(int stateID)
        //{
        //    BizEntityState bizEntityState = new BizEntityState();
        //    return bizEntityState.EntityHasState(DataItem, stateID);
        //}
        //public bool HasState(string state)
        //{
        //    BizEntityState bizEntityState = new BizEntityState();
        //    return bizEntityState.EntityHasState(DataItem, state);
        //}
        public FormulaObject()
        {
        }

        void newObject_PropertySetChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PropertySetChanged != null)
                PropertySetChanged(this, null);
        }
        protected virtual void NotifyPropertyChanged(string _propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(_propertyName));
            }
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return m_properties;
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return GetProperties(null);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            ArrayList properties = new ArrayList();
            foreach (var e in m_properties)
            {
                properties.Add(new FormulaObjectPropertyDescriptor(m_properties, e.Key));
            }

            PropertyDescriptor[] props =
                (PropertyDescriptor[])properties.ToArray(typeof(PropertyDescriptor));

            return new PropertyDescriptorCollection(props);
        }

        public object this[string _name]
        {
            get { return m_properties[_name]; }
            set
            {
                (m_properties[_name] as MyPropertyInfo).Value = value;
                NotifyPropertyChanged(_name);
            }
        }
        public IReadOnlyDictionary<string, MyPropertyInfo> Properties { get { return m_properties; } }
        Dictionary<string, MyPropertyInfo> m_properties = new Dictionary<string, MyPropertyInfo>();

        internal void SetProperties(Dictionary<string, MyPropertyInfo> properties)
        {
            m_properties = properties;
            if (PropertySetChanged != null)
                PropertySetChanged(this, null);
            PropertiesLoaded = true;
        }


        public void OnPropertyGetCalled(FormulaObject bindablePropertyDescriptor, PropertyGetArg propertyGetArg)
        {
            if (PropertyGetCalled != null)
                PropertyGetCalled(bindablePropertyDescriptor, propertyGetArg);
        }

        internal void OnPropertySetChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PropertySetChanged != null)
                PropertySetChanged(sender, e);
        }
    }
    //آبجکت اصلی ICustomTypeDescriptor است
    //باید یه کالکشنی از PropertyDescriptor ها برگردونه
    class FormulaObjectPropertyDescriptor : PropertyDescriptor
    {
        Dictionary<string, MyPropertyInfo> _dictionary;
        string _key;
        internal FormulaObjectPropertyDescriptor(Dictionary<string, MyPropertyInfo> d, string key)
            : base(key.ToString(), null)
        {
            _dictionary = d;
            _key = key;

        }
        public override Type PropertyType
        {
            get
            {
                if (_dictionary[_key].Type != null)
                    return _dictionary[_key].Type;
                else
                {
                    if (_dictionary[_key].Value == null)
                        return null;
                    else
                        return _dictionary[_key].Value.GetType();
                }
            }
        }
        public override void SetValue(object component, object value)
        {
            _dictionary[_key].Value = value;
        }

        public override object GetValue(object component)
        {
            //     var prop = _dictionary[_key];
            //if (prop.FormulaObject == component)
            //{
            //try
            //{
            (component as FormulaObject).OnPropertyGetCalled((component as FormulaObject), new PropertyGetArg() { PropertyInfo = _dictionary[_key] });
            return _dictionary[_key].Value;
            //}
            //catch(Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
            //}
            //else
            //    return null;
        }
        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type ComponentType
        {
            get { return null; }
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override void ResetValue(object component)
        {
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }

    public class MyPropertyInfo
    {
        public MyPropertyInfo()
        {
            ChildProperties = new List<MyFormulaFunctionStateFunctionLibrary.MyPropertyInfo>();
        }
        public bool ValueSearched { set; get; }
        public string Name { set; get; }
        public RelationshipPropertyInfo RelationshipPropertyInfo { set; get; }
        public int ID { set; get; }

        public FormulaObject FormulaObject { set; get; }
        public int ParameterFormulaID { set; get; }
        public object Value { set; get; }
        public Type Type { set; get; }
        public PropertyType PropertyType { set; get; }
        //public ValueCustomType CustomType { set; get; }
        public int RelationshipLevel { set; get; }
        public string RelationshipTail { set; get; }
        public int RelationshipID { set; get; }
        public string Tooltip { set; get; }

        public List<MyPropertyInfo> ChildProperties { set; get; }
        public object Context { set; get; }
        public MyPropertyInfo ParentProperty { get; internal set; }
        public string RelationshipPropertyTail { get; internal set; }
    }

    public class PropertyGetArg : EventArgs
    {
        //public object Value { set; get; }
        public MyPropertyInfo PropertyInfo { set; get; }

        //اگر پارامتر خودش یک فرمولا پارامتر بود این پراپرتی که جزئیات محاسبه آنست پر میشود
        public List<FormulaUsageParemetersDTO> FormulaUsageParemeters { set; get; }
        //public FormulaObject FormulaObject { set; get; }
    }

    public class RelationshipPropertyInfo

    {
        public RelationshipPropertyInfo()
        {
            Properties = new List<EntityInstanceProperty>();
        }
        //public int TargetEntityID { set; get; }
        public RelationshipDTO Relationship { get; internal set; }
        public List<EntityInstanceProperty> Properties { set; get; }
    }


    //public class PersianDate
    //{
    //    //public override string ToString()
    //    //{
    //    //    return Value??"";
    //    //}

    //    DateTime MiladiDate;
    //    string _Value;
    //    public string Value
    //    {
    //        set
    //        {
    //            _Value = value;
    //            if (!string.IsNullOrEmpty(_Value))
    //            {
    //                MiladiDate = GeneralHelper.GetMiladiDateFromShamsi(_Value);
    //            }
    //            else
    //            {

    //                MiladiDate = DateTime.FromOADate(0);
    //            }
    //        }
    //        get
    //        {
    //            return _Value;
    //        }
    //    }
    //    public string AddDays(double value)
    //    {
    //        return GeneralHelper.GetShamsiDate(MiladiDate.AddDays(value));
    //    }

    //    public string AddHours(double value)
    //    {
    //        return GeneralHelper.GetShamsiDate(MiladiDate.AddHours(value));
    //    }
    //    public string AddMinutes(double value)
    //    { return GeneralHelper.GetShamsiDate(MiladiDate.AddMinutes(value)); }

    //    public string AddMonths(int value)
    //    { return GeneralHelper.GetShamsiDate(MiladiDate.AddMonths(value)); }

    //    public string AddSeconds(double value)
    //    { return GeneralHelper.GetShamsiDate(MiladiDate.AddSeconds(value)); }

    //    public string AddTicks(long value)
    //    { return GeneralHelper.GetShamsiDate(MiladiDate.AddTicks(value)); }

    //    public string AddYears(int value)
    //    { return GeneralHelper.GetShamsiDate(MiladiDate.AddYears(value)); }
    //}

    //public static class MyExtensions
    //{
    //    public static bool WordCount(this FormulaObject str)
    //    {
    //        return true;
    //        //return str.Split(new char[] { ' ', '.', '?' },
    //        //                 StringSplitOptions.RemoveEmptyEntries).Length;
    //    }
    //    public static bool WordCount(this BindableTypeDescriptor<tempClass1> str)
    //    {
    //        return true;
    //        //return str.Split(new char[] { ' ', '.', '?' },
    //        //                 StringSplitOptions.RemoveEmptyEntries).Length;
    //    }
    //    public static bool WordCount(this BindableTypeDescriptor<tempClass2> str)
    //    {
    //        return true;
    //        //return str.Split(new char[] { ' ', '.', '?' },
    //        //                 StringSplitOptions.RemoveEmptyEntries).Length;
    //    }

    //}

    public class PropertyFunction
    {
        public string Title { set; get; }
        public string ParamsStr { set; get; }
        public string Tooltip { set; get; }
        public string Name { set; get; }
        public ImageSource Image { set; get; }
        public Enum_PropertyFunctionType Type { set; get; }
    }
    public enum Enum_PropertyFunctionType
    {
        Property,
        Method
    }

}
