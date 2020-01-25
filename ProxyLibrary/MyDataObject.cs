using ModelEntites;
using ProxyLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProxyLibrary
{
    public class MyDataObject : INotifyPropertyChanged, ICustomTypeDescriptor
    {
        public event PropertyChangedEventHandler PropertySetChanged;
        public event EventHandler<PropertyGetArg> PropertyGetCalled;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool PropertiesLoaded;
        public DP_DataRepository DataItem { set; get; }
        //public List<FormulaParameterDTO> Parameters { set; get; }



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
        public MyDataObject(DP_DataRepository dataItem)
        {
            DataItem = dataItem;
            foreach (var column in DataItem.GetProperties())
            {
                MyPropertyInfo1 propertyInfo = new MyPropertyInfo1();
                propertyInfo.ID = column.ColumnID;
                propertyInfo.PropertyType = PropertyType.Column;
                propertyInfo.Type = typeof(string);
                propertyInfo.Name = column.Name;
                propertyInfo.ValueSearched = true;
                propertyInfo.Value = column.Value;
                m_properties.Add(propertyInfo.Name, propertyInfo);
            }
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
                (m_properties[_name] as MyPropertyInfo1).Value = value;
                NotifyPropertyChanged(_name);
            }
        }
        public IReadOnlyDictionary<string, MyPropertyInfo1> Properties { get { return m_properties; } }
        Dictionary<string, MyPropertyInfo1> m_properties = new Dictionary<string, MyPropertyInfo1>();

        internal void SetProperties(Dictionary<string, MyPropertyInfo1> properties)
        {
            m_properties = properties;
            if (PropertySetChanged != null)
                PropertySetChanged(this, null);
            PropertiesLoaded = true;
        }


        public void OnPropertyGetCalled(MyDataObject bindablePropertyDescriptor, PropertyGetArg propertyGetArg)
        {
            if (PropertyGetCalled != null)
                PropertyGetCalled(bindablePropertyDescriptor, propertyGetArg);
        }

        class FormulaObjectPropertyDescriptor : PropertyDescriptor
        {
            Dictionary<string, MyPropertyInfo1> _dictionary;
            string _key;
            internal FormulaObjectPropertyDescriptor(Dictionary<string, MyPropertyInfo1> d, string key)
                : base(key.ToString(), null)
            {
                _dictionary = d;
                _key = key;
                //_dataItem = dataItem;
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
                (component as MyDataObject).OnPropertyGetCalled((component as MyDataObject), new PropertyGetArg() { PropertyInfo = _dictionary[_key] });
                return _dictionary[_key].Value;
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



        internal void OnPropertySetChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PropertySetChanged != null)
                PropertySetChanged(sender, e);
        }
    }


    public class MyPropertyInfo1
    {
        public MyPropertyInfo1()
        {
            ChildProperties = new List<MyPropertyInfo1>();
        }
        public bool ValueSearched { set; get; }
        public string Name { set; get; }
        public RelationshipPropertyInfo RelationshipPropertyInfo { set; get; }
        public int ID { set; get; }

        public MyDataObject ParentFormulaObjectForZoolbia { set; get; }
        public int ParameterFormulaID { set; get; }
        public object Value { set; get; }
        public Type Type { set; get; }
        public PropertyType PropertyType { set; get; }
        public int RelationshipLevel { set; get; }
        public string RelationshipTail { set; get; }
        public int ParentRelationshipID { set; get; }
        public string Tooltip { set; get; }

        public List<MyPropertyInfo1> ChildProperties { set; get; }
        //public bool ValueIsGenerated { get; internal set; }
    }

    public class PropertyGetArg : EventArgs
    {
        //public object Value { set; get; }
        public MyPropertyInfo1 PropertyInfo { set; get; }

        //public DataObject DataObject { set; get; }
    }
    public enum PropertyType
    {
        Column,
        FormulaParameter,
        Relationship,
        DBFormula,
        Code,
        State,
        Helper,
        This,
        Mock
    }
    public class RelationshipPropertyInfo

    {
        public RelationshipPropertyInfo()
        {
            Properties = new List<EntityInstanceProperty>();
        }
        public int TargetEntityID { set; get; }
        public bool OneToManyItems { get; internal set; }
        public List<EntityInstanceProperty> Properties { set; get; }
    }








}
