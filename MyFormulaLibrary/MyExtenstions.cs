using DynamicExpresso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFormulaFunctionStateFunctionLibrary
{
    public class MyExtensions
    {
        public static MyCustomData First(object obj, string criteria = null)
        {
            var list = GetList(obj);
            var delegateExpression = GetExpression(obj, criteria);
            return list.First(delegateExpression);
        }
        public static MyCustomData First(object obj)
        {
            var list = GetList(obj);
            return list.First();
        }
        public static MyCustomData FirstOrDefault(object obj, string criteria = null)
        {
            var list = GetList(obj);
            var delegateExpression = GetExpression(obj, criteria);
            return list.FirstOrDefault(delegateExpression);

        }
        public static MyCustomData FirstOrDefault(object obj)
        {
            var list = GetList(obj);
            return list.FirstOrDefault();
        }
        public static MyCustomData Last(object obj, string criteria = null)
        {
            var list = GetList(obj);
            var delegateExpression = GetExpression(obj, criteria);
            return list.Last(delegateExpression);
        }
        public static MyCustomData Last(object obj)
        {
            var list = GetList(obj);
            return list.Last();
        }
        public static MyCustomData LastOrDefault(object obj, string criteria = null)
        {
            var list = GetList(obj);
            var delegateExpression = GetExpression(obj, criteria);
            return list.LastOrDefault(delegateExpression);
        }
        public static MyCustomData LastOrDefault(object obj)
        {
            var list = GetList(obj);
            return list.LastOrDefault();
        }
        public static bool All(object obj, string criteria = null)
        {
            var list = GetList(obj);
            var delegateExpression = GetExpression(obj, criteria);
            return list.All(delegateExpression);

        }
        public static int Count(object obj, string criteria = null)
        {
            var list = GetList(obj);
            var delegateExpression = GetExpression(obj, criteria);
            return list.Count(delegateExpression);
        }
        public static int Count(object obj)
        {
            var list = GetList(obj);
            return list.Count();
        }
        public static bool Any(object obj, string criteria = null)
        {
            var list = GetList(obj);
            var delegateExpression = GetExpression(obj, criteria);
            return list.Any(delegateExpression);
        }
        public static bool Any(object obj)
        {
            var list = GetList(obj);
            return list.Any();
        }
        private static List<MyCustomData> GetList(object obj)
        {
            if (obj is List<MyCustomData>)
            {
                return obj as List<MyCustomData>;
            }
            else
            {
                throw new Exception("Object is not a list :" + obj.ToString());
            }
        }

        private static Func<MyCustomData, bool> GetExpression(object obj, string criteria)
        {
            var keyCriteria = GetKeyAndCriteria(criteria);
            var interpreter = FormulaInstanceInternalHelper.GetExpressionDelegate();
            return interpreter.GetDelegate<Func<MyCustomData, bool>>(keyCriteria.Item2, keyCriteria.Item1);

        }
        private static Tuple<string, string> GetKeyAndCriteria(string criteria)
        {
            string key = "";
            string where = "";
            if (criteria.Contains("=>"))
            {
                var splt = criteria.Split(new string[] { "=>" }, 2, StringSplitOptions.None);
                key = splt[0];
                where = splt[1];
            }
            else
            {
                throw new Exception("Criteria not in proper format : " + criteria);
            }
            return new Tuple<string, string>(key, where);
        }

        public static bool IsEqual(object obj, object value)
        {
            if (obj == null && value == null)
                return true;
            else if (obj == null || value == null)
                return false;
            else
                return obj.ToString() == value.ToString();
        }
        //public static int ToInt(object obj)
        //{
        //    return Convert.ToInt32(obj);
        //}
    }
}
