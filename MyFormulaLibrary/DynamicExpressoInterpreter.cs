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
        public static Interpreter GetInterpreter(List<Type> refTypes, List<object> variables)
        {
            Interpreter target = new Interpreter();
            if (refTypes != null)
            {
                foreach (var type in refTypes)
                {
                    var refType = new ReferenceType(type);
                    //foreach (var method in extenstion.GetMethods())
                    //{
                    //    refType.ExtensionMethods.Add(method);
                    //}
                    target.Reference(refType);
                }
            }
            if (variables != null)
            {
                foreach (var variable in variables)
                {
                    target.SetVariable(variable.GetType().Name, variable);
                }
            }
            //var ttype = typeof(AAA);
            //var refType1 = new ReferenceType(ttype);
            //target.Reference(refType1);
            return target;
        }
    }
    public class DynamicExpressoExpressionHandler : I_ExpressionHandler
    {
        public I_ExpressionDelegate GetExpressionDelegate(List<Type> refTypes, List<object> variables)
        {
            return new DynamicExpressoDelegate(refTypes, variables);
        }

        public I_ExpressionEvaluator GetExpressionEvaluator(MyCustomSingleData customData, List<Type> refTypes, List<object> variables)
        {
            return new DynamicExpressoInterpreter(customData, refTypes, variables);
        }
        //public Dictionary<string, Type> GetExpressionBuiltinVariables()
        //{
        //    Dictionary<string, Type> result = new Dictionary<string, Type>();
        //    var target = InterpreterGenerator.GetInterpreter();
        //    foreach (var refType in target.ReferencedTypes)
        //    {
        //        result.Add(refType.Name, refType.Type);
        //    }
        //    return result;
        //}

        public string GetObjectPrefrix()
        {
            return "x";
        }
    }
    public class DynamicExpressoInterpreter : I_ExpressionEvaluator
    {
        private MyCustomSingleData CustomData;
        Interpreter target = null;
        List<object> Variables;
        public DynamicExpressoInterpreter(MyCustomSingleData customData, List<Type> refTypes, List<object> variables)
        {
            Variables = variables;
            target = InterpreterGenerator.GetInterpreter(refTypes, variables);
            CustomData = customData;
            target.SetVariable("x", CustomData);
        }
        public object Calculate(string expression)
        {
            return target.Eval(expression);
        }

        public List<BuiltinRefClass> GetExpressionBuiltinVariables()
        {
          
            List<BuiltinRefClass> result = new List<MyFormulaFunctionStateFunctionLibrary.BuiltinRefClass>();
            foreach (var refType in target.ReferencedTypes)
            {
                BuiltinRefClass rItem = new MyFormulaFunctionStateFunctionLibrary.BuiltinRefClass();
                rItem.IsType = true;
                rItem.Type = refType.Type;
                rItem.Name = refType.Name;
                result.Add(rItem);
            }
            if (Variables != null)
            {
                foreach (var variable in Variables)
                {
                    BuiltinRefClass rItem = new MyFormulaFunctionStateFunctionLibrary.BuiltinRefClass();
                    rItem.IsObject = true;
                    rItem.Type = variable.GetType();
                    rItem.Name = variable.GetType().Name;
                    result.Add(rItem);
                }
            }
            return result;
        }
    }
    public class DynamicExpressoDelegate : I_ExpressionDelegate
    {
        Interpreter target = null;
        public DynamicExpressoDelegate(List<Type> refTypes, List<object> variables)
        {
            target = InterpreterGenerator.GetInterpreter(refTypes, variables);
        }
        public T GetDelegate<T>(string expression, string key)
        {
            return target.ParseAsDelegate<T>(expression, key);
        }
    }
    public static class AAA
    {
        public static int WordCount(this String str)
        {
            return str.Split(new char[] { ' ', '.', '?' },
                             StringSplitOptions.RemoveEmptyEntries).Length;
        }
        //public string TestString()
        //{
        //    return "aaa";
        //}
        public static string TestStatic()
        {
            return "bbb";
        }
    }
    //public static class BBB
    //{
    //    public static int WordCount(this String str)
    //    {
    //        return str.Split(new char[] { ' ', '.', '?' },
    //                         StringSplitOptions.RemoveEmptyEntries).Length;
    //    }
    //}
}
