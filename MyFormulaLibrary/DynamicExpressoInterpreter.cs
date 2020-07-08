using DynamicExpresso;
using ProxyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFormulaFunctionStateFunctionLibrary
{
    public class InterpreterGenerator
    {
        public static Interpreter GetInterpreter(Type extenstion = null)
        {
            Interpreter target = new Interpreter();
            if (extenstion != null)
            {
                var refType = new ReferenceType(extenstion);
                foreach (var method in extenstion.GetMethods())
                {
                    refType.ExtensionMethods.Add(method);
                }
                target.Reference(refType);
            }
            return target;
        }
    }
    public class DynamicExpressoExpressionHandler : I_ExpressionHandler
    {
        public I_ExpressionDelegate GetExpressionDelegate(Type extenstion)
        {
            return new DynamicExpressoDelegate(extenstion);
        }

        public I_ExpressionEvaluator GetExpressionEvaluator(MyCustomData customData, Dictionary<string, Type> helpers, Type extenstion)
        {
            return new DynamicExpressoInterpreter(customData, helpers, extenstion);
        }
        public Dictionary<string, Type> GetExpressionBuiltinVariables()
        {
            Dictionary<string, Type> result = new Dictionary<string, Type>();
            var target = InterpreterGenerator.GetInterpreter();
            foreach (var refType in target.ReferencedTypes)
            {
                result.Add(refType.Name, refType.Type);
            }
            return result;
        }

        public string GetObjectPrefrix()
        {
            return "x";
        }
    }
    public class DynamicExpressoInterpreter : I_ExpressionEvaluator
    {
        private MyCustomData CustomData;
        private DP_DataRepository MainDataItem;
        private DR_Requester Requester;
        private List<Type> Helpers;
        Interpreter target = null;
        public DynamicExpressoInterpreter(MyCustomData customData, Dictionary<string, Type> helpers, Type extenstion)
        {
            target = InterpreterGenerator.GetInterpreter(extenstion);
            CustomData = customData;
            foreach (var item in helpers)
            {
                target.SetVariable(item.Key, Activator.CreateInstance(item.Value));
            }
            target.SetVariable("x", CustomData);
        }
        public object Calculate(string expression)
        {
            return target.Eval(expression);
        }
    }
    public class DynamicExpressoDelegate : I_ExpressionDelegate
    {
        Interpreter target = null;
        public DynamicExpressoDelegate(Type extenstion)
        {
            target = InterpreterGenerator.GetInterpreter(extenstion);
        }
        public T GetDelegate<T>(string expression, string key)
        {
            return target.ParseAsDelegate<T>(expression, key);
        }
    }
}
