using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
namespace MyGeneralLibrary
{
    public class ReflectionHelper
    {

        public static object GetClassInstance(string assemblyName, string path, string className)
        {
            if (string.IsNullOrEmpty(path))
                assemblyName = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + assemblyName;
            Assembly assembly = Assembly.LoadFile(assemblyName);
            Type type = assembly.GetType(className);
            if (type != null)
            {
                return Activator.CreateInstance(type, null);
            }
            return null;
            //MethodInfo methodInfo = type.GetMethod("MyMethod");
            //if (methodInfo != null)
            //{
            //    object result = null;
            //    ParameterInfo[] parameters = methodInfo.GetParameters();
            //    object classInstance = Activator.CreateInstance(type, null);
            //    if (parameters.Length == 0)
            //    {
            //        //This works fine
            //        result = methodInfo.Invoke(classInstance, null);
            //    }
            //    else
            //    {
            //        object[] parametersArray = new object[] { "Hello" };

            //        //The invoke does NOT work it throws "Object does not match target type"             
            //        result = methodInfo.Invoke(classInstance, parametersArray);
            //    }
            //}
        }
        public static object FollowPropertyPath(object value, string path)
        {
            Type currentType = value.GetType();

            foreach (string propertyName in path.Split('.'))
            {
                PropertyInfo property = currentType.GetProperty(propertyName);
                value = property.GetValue(value, null);
                currentType = property.PropertyType;
            }
            return value;
        }
        public static bool MethodExistsOfType(string assemblyPath, string className, string function, Type expectedReturnType, Type expectedParamType)
        {
            Assembly assembly = Assembly.LoadFile(assemblyPath);
            Type type = assembly.GetType(className);
            if (type != null)
            {
                var method = type.GetMethods().FirstOrDefault(x => x.Name == function);
                if (method != null)
                {
                    if (method.GetParameters().Count() == 1)
                    {
                        if (method.GetParameters().First().ParameterType == expectedParamType)
                            return method.ReturnType == expectedReturnType;
                    }
                }
                // return Activator.CreateInstance(type, null);
            }

            return false;
        }

        public static MethodInfo GetMethod(string assemblyPath, string className, string function)
        {

            Assembly assembly = Assembly.LoadFile(assemblyPath);
            Type type = assembly.GetType(className);
            if (type != null)
            {
                var method = type.GetMethods().FirstOrDefault(x => x.Name == function);
                if (method != null)
                {
                    return method;
                    //////if (method.GetParameters().Count() == 1)
                    //////{
                    //////    if (method.GetParameters().First().ParameterType == expectedParamType)
                    //////        return method.ReturnType == expectedReturnType;
                    //////}
                }
                // return Activator.CreateInstance(type, null);
            }
            return null;
        }

        public static object CallMethod(string assemblyPath, string className, string function, object[] parameters)
        {
            Assembly assembly = Assembly.LoadFile(assemblyPath);
            Type type = assembly.GetType(className);
            if (type != null)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod(function);
                return methodInfo.Invoke(instance, parameters);
            }

            return null;
        }
        internal static bool ImplementsInterface(object instance, Type type)
        {
            var interfaces = instance.GetType().GetInterfaces();

            foreach (var i in interfaces)
            {
                if (i.ToString().Equals(type.ToString()))
                    return true;
            }
            return false;
        }

     
    }
}
