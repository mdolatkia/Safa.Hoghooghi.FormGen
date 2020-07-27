using DynamicExpresso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFormulaFunctionStateFunctionLibrary
{
    public static class MyExtensions
    {
        public static MyCustomSingleData First(this MyCustomSingleData obj, string criteria = null)
        {
            var list = GetList(obj);
            var delegateExpression = GetExpression(obj, criteria);
            return list.First(delegateExpression);
        }
        public static MyCustomSingleData First(this MyCustomSingleData obj)
        {
            var list = GetList(obj);
            return list.First();
        }
        public static MyCustomSingleData FirstOrDefault(this MyCustomSingleData obj, string criteria = null)
        {
            var list = GetList(obj);
            var delegateExpression = GetExpression(obj, criteria);
            return list.FirstOrDefault(delegateExpression);

        }
        public static MyCustomSingleData FirstOrDefault(this MyCustomSingleData obj)
        {
            var list = GetList(obj);
            return list.FirstOrDefault();
        }
        public static MyCustomSingleData Last(this MyCustomSingleData obj, string criteria = null)
        {
            var list = GetList(obj);
            var delegateExpression = GetExpression(obj, criteria);
            return list.Last(delegateExpression);
        }
        public static MyCustomSingleData Last(this MyCustomSingleData obj)
        {
            var list = GetList(obj);
            return list.Last();
        }
        public static MyCustomSingleData LastOrDefault(this MyCustomSingleData obj, string criteria = null)
        {
            var list = GetList(obj);
            var delegateExpression = GetExpression(obj, criteria);
            return list.LastOrDefault(delegateExpression);
        }
        public static MyCustomSingleData LastOrDefault(this MyCustomSingleData obj)
        {
            var list = GetList(obj);
            return list.LastOrDefault();
        }
        public static bool All(this MyCustomSingleData obj, string criteria = null)
        {
            var list = GetList(obj);
            var delegateExpression = GetExpression(obj, criteria);
            return list.All(delegateExpression);

        }
        public static int Count(this MyCustomSingleData obj, string criteria = null)
        {
            var list = GetList(obj);
            var delegateExpression = GetExpression(obj, criteria);
            return list.Count(delegateExpression);
        }
        public static int Count(this MyCustomSingleData obj)
        {
            var list = GetList(obj);
            return list.Count();
        }
        public static bool Any(this MyCustomSingleData obj, string criteria = null)
        {
            var list = GetList(obj);
            var delegateExpression = GetExpression(obj, criteria);
            return list.Any(delegateExpression);
        }
        public static bool Any(this MyCustomSingleData obj)
        {
            var list = GetList(obj);
            return list.Any();
        }
        private static List<MyCustomSingleData> GetList(this object obj)
        {
            if (obj is List<MyCustomSingleData>)
            {
                return obj as List<MyCustomSingleData>;
            }
            else
            {
                throw new Exception("Object is not a list :" + obj.ToString());
            }
        }

        private static Func<MyCustomSingleData, bool> GetExpression(object obj, string criteria)
        {
            var keyCriteria = GetKeyAndCriteria(criteria);
            var interpreter = FormulaInstanceInternalHelper.GetExpressionDelegate();
            return interpreter.GetDelegate<Func<MyCustomSingleData, bool>>(keyCriteria.Item2, keyCriteria.Item1);

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

        public static bool IsEqual(this object obj, object value)
        {
            if (obj == null && value == null)
                return true;
            else if (obj == null || value == null)
                return false;
            else
                return obj.ToString() == value.ToString();
        }
        //public static int ToInt(this  object obj)
        //{
        //    return Convert.ToInt32(obj);
        //}
    }
}
