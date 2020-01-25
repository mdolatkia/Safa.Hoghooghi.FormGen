using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyRuleEngine
{
    class ReflectionHelper
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
