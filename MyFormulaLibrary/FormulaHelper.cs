using MyModelManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelEntites;
using System.Collections;
using System.Linq.Expressions;
using Telerik.Windows.Controls;
using ProxyLibrary;
using MyDataManagerBusiness;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using MyGeneralLibrary;
using System.ComponentModel;
using MyDatabaseFunctionLibrary;

namespace MyFormulaFunctionStateFunctionLibrary
{
    //public static class FormulaHepler
    //{
    //    //public  FormulaHepler()
    //    //{

    //    //}
    //    //StringHelper _StringHelper;
    //    //public static StringHelper StringHelper
    //    //{
    //    //    get
    //    //    {
    //    //        if (_StringHelper == null)
    //    //            _StringHelper = new StringHelper();
    //    //        return _StringHelper;
    //    //    }
    //    //}
    //    //PersianDateHelper _PersianDateHelper;
    //    //public static PersianDateHelper PersianDateHelper
    //    //{
    //    //    get
    //    //    {
    //    //        if (_PersianDateHelper == null)
    //    //            _PersianDateHelper = new PersianDateHelper();
    //    //        return _PersianDateHelper;
    //    //    }
    //    //}


    //    public static  Type IsHelperType(string theWord)
    //    {
    //        if (!string.IsNullOrEmpty(theWord))
    //        {
    //            if (theWord.ToLower() == "PersianDateHelper".ToLower())
    //                return typeof(PersianDateHelper);
    //            else if (theWord.ToLower() == "MiladiDateHelper".ToLower())
    //                return typeof(MiladiDateHelper);
    //            else if (theWord.ToLower() == "StringHelper".ToLower())
    //                return typeof(StringHelper);
    //            else if (theWord.ToLower() == "NumericHelper".ToLower())
    //                return typeof(NumericHelper);
    //            else if (theWord.ToLower() == "DBFunctionHelper".ToLower())
    //                return typeof(DBFunctionHelper);
    //        }
    //        return null;
    //    }
    //}
    public static class NumericHelper
    {
        public static double IsNull(object value, double resValue)
        {
            if (value == null)
                return resValue;
            else
                return Convert.ToDouble(value);
        }
        public static int IsNull(object value, int resValue)
        {
            if (value == null)
                return resValue;
            else
                return Convert.ToInt32(value);
        }
        public static long IsNull(object value, long resValue)
        {
            if (value == null)
                return resValue;
            else
                return Convert.ToInt64(value);
        }

    }
    public static class StringHelper
    {
        //public static bool bbb(this string str)
        //{
        //    return String.IsNullOrEmpty(str);
        //}
        public static bool bbb(this String str)
        {
            return String.IsNullOrEmpty(str);
        }
        public static bool IsNullOrEmpty(string str)
        {
            return String.IsNullOrEmpty(str);
        }
        //public static string GetPropertiesString(ICustomTypeDescriptor obj, string delimiter, params string[] list)
        //{
        //    string result = "";
        //    foreach (var item in list)
        //    {
        //        var res = FollowPropertyPath(obj, item);
        //        return result += (result == "" ? "" : delimiter) + (res == null ? "" : res.ToString());
        //    }
        //    return result;
        //}
        public static string Trim(object value)
        {
            if (value == null)
                return null;
            else
                return value.ToString().Trim();
        }
        public static string GetPropertyString(ICustomTypeDescriptor obj, string propertyPath)
        {
            var res = FollowPropertyPath(obj, propertyPath);
            return res == null ? "" : res.ToString();
        }

        public static object FollowPropertyPath(ICustomTypeDescriptor value, string path)
        {
            //Type currentType = value.GetType();
            if (value == null)
                return null;
            object rValue = null;
            foreach (string propertyName in path.Split('.'))
            {
                var property = value.GetProperties().Find(propertyName, true);
                if (property == null)
                    return null;
                else
                {
                    rValue = property.GetValue(value);
                    if (value == null)
                        return null;
                    else if (rValue is ICustomTypeDescriptor)
                        value = (rValue as ICustomTypeDescriptor);
                }
                //currentType = property.PropertyType;
            }
            return rValue;
        }
    }

    public static class PersianDateHelper
    {
        public static string Today
        {
            get
            {
                return GeneralHelper.GetShamsiDate(DateTime.Today);
            }
        }
        public static string AddDays(string persianDate, double value)
        {
            try
            {
                var MiladiDate = GeneralHelper.GetMiladiDateFromShamsi(persianDate);
                return GeneralHelper.GetShamsiDate(MiladiDate.AddDays(value));
            }
            catch { return ""; }
        }

        public static string AddHours(string persianDate, double value)
        {
            try
            {
                var MiladiDate = GeneralHelper.GetMiladiDateFromShamsi(persianDate);
                return GeneralHelper.GetShamsiDate(MiladiDate.AddHours(value));
            }
            catch { return ""; }
        }
        public static string AddMinutes(string persianDate, double value)
        {
            try
            {
                var MiladiDate = GeneralHelper.GetMiladiDateFromShamsi(persianDate);
                return GeneralHelper.GetShamsiDate(MiladiDate.AddMinutes(value));
            }
            catch { return ""; }
        }

        public static string AddMonths(string persianDate, int value)
        {
            try
            {
                var MiladiDate = GeneralHelper.GetMiladiDateFromShamsi(persianDate);
                return GeneralHelper.GetShamsiDate(MiladiDate.AddMonths(value));
            }
            catch { return ""; }
        }

        public static string AddSeconds(string persianDate, double value)
        {
            try
            {
                var MiladiDate = GeneralHelper.GetMiladiDateFromShamsi(persianDate);
                return GeneralHelper.GetShamsiDate(MiladiDate.AddSeconds(value));
            }
            catch { return ""; }
        }

        public static string AddTicks(string persianDate, long value)
        {
            try
            {
                var MiladiDate = GeneralHelper.GetMiladiDateFromShamsi(persianDate);
                return GeneralHelper.GetShamsiDate(MiladiDate.AddTicks(value));
            }
            catch { return ""; }
        }

        public static string AddYears(string persianDate, int value)
        {
            try
            {
                var MiladiDate = GeneralHelper.GetMiladiDateFromShamsi(persianDate);
                return GeneralHelper.GetShamsiDate(MiladiDate.AddYears(value));
            }
            catch { return ""; }
        }
    }
    public static class MiladiDateHelper
    {
        public static double DateDiffAsHours(object firstDateTime, object secondDateTime)
        {
            return ((DateTime)firstDateTime).Subtract((DateTime)secondDateTime).TotalHours;
        }
        public static double DateDiffAsDays(object firstDateTime, object secondDateTime)
        {
            return ((DateTime)firstDateTime).Subtract((DateTime)secondDateTime).TotalDays;
        }
    }
    public class DBFunctionHelper
    {
        public static object CalculateFunction()
        {
            return null;
        }
        public DBFunctionHelper(int databaseID, DR_Requester requester)
        {
            DatabaseID = databaseID;
            Requester = requester;
        }
        DatabaseFunctionHandler DatabaseFunctionHandler = new DatabaseFunctionHandler();
        public int DatabaseID { set; get; }
        DR_Requester Requester { set; get; }
        public object CalculateFunction(string name)
        {
            var result = DatabaseFunctionHandler.GetDatabaseFunctionValue(Requester, DatabaseID, name, null);
            if (result.ExecutionException != null)
                throw result.ExecutionException;
            return result.Result;
        }
        public object CalculateFunction(string name, string param1Name, object param1Value)
        {
            var result = DatabaseFunctionHandler.GetDatabaseFunctionValue(Requester, DatabaseID, name, new List<Tuple<string, object>>() { new Tuple<string, object>(param1Name, param1Value) });
            if (result.ExecutionException != null)
                throw result.ExecutionException;
            return result.Result;
        }
        public object CalculateFunction(string name, string param1Name, object param1Value, string param2Name, object param2Value)
        {
            var result = DatabaseFunctionHandler.GetDatabaseFunctionValue(Requester, DatabaseID, name, new List<Tuple<string, object>>() { new Tuple<string, object>(param1Name, param1Value), new Tuple<string, object>(param2Name, param2Value) });
            if (result.ExecutionException != null)
                throw result.ExecutionException;
            return result.Result;
        }
        public object CalculateFunction(string name, string param1Name, object param1Value, string param2Name, object param2Value, string param3Name, object param3Value)
        {
            var result = DatabaseFunctionHandler.GetDatabaseFunctionValue(Requester, DatabaseID, name, new List<Tuple<string, object>>() { new Tuple<string, object>(param1Name, param1Value), new Tuple<string, object>(param2Name, param2Value), new Tuple<string, object>(param3Name, param3Value) });
            if (result.ExecutionException != null)
                throw result.ExecutionException;
            return result.Result;
        }
    }

}
