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

namespace MyFormulaFunctionStateFunctionLibrary
{
    public class FormulaHepler
    {
        public FormulaHepler()
        {

        }
        //StringHelper _StringHelper;
        //public StringHelper StringHelper
        //{
        //    get
        //    {
        //        if (_StringHelper == null)
        //            _StringHelper = new StringHelper();
        //        return _StringHelper;
        //    }
        //}
        //PersianDateHelper _PersianDateHelper;
        //public PersianDateHelper PersianDateHelper
        //{
        //    get
        //    {
        //        if (_PersianDateHelper == null)
        //            _PersianDateHelper = new PersianDateHelper();
        //        return _PersianDateHelper;
        //    }
        //}


        public static Type IsHelperType(string theWord)
        {
            if (!string.IsNullOrEmpty(theWord))
            {
                if (theWord.ToLower() == "PersianDateHelper".ToLower())
                    return typeof(PersianDateHelper);
                else if (theWord.ToLower() == "StringHelper".ToLower())
                    return typeof(StringHelper);
                else if (theWord.ToLower() == "NumericHelper".ToLower())
                    return typeof(NumericHelper);
            }
            return null;
        }
    }
    public class NumericHelper
    {
        public double IsNull(object value,double resValue)
        {
            if (value == null)
                return resValue;
            else
                return Convert.ToDouble(value);
        }
        public int IsNull(object value, int resValue)
        {
            if (value == null)
                return resValue;
            else
                return Convert.ToInt32(value);
        }
        public long IsNull(object value, long resValue)
        {
            if (value == null)
                return resValue;
            else
                return Convert.ToInt64(value);
        }
    }
    public class StringHelper
    {
        public bool IsNullOrEmpty(string str)
        {
            return String.IsNullOrEmpty(str);
        }
        //public string GetPropertiesString(ICustomTypeDescriptor obj, string delimiter, params string[] list)
        //{
        //    string result = "";
        //    foreach (var item in list)
        //    {
        //        var res = FollowPropertyPath(obj, item);
        //        return result += (result == "" ? "" : delimiter) + (res == null ? "" : res.ToString());
        //    }
        //    return result;
        //}
        public string Trim(string value)
        {
            if (value == null)
                return null;
            else
                return value.Trim();
        }
        public string GetPropertyString(ICustomTypeDescriptor obj, string propertyPath)
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

    public class PersianDateHelper
    {
        public string Today
        {
            get
            {
                return GeneralHelper.GetShamsiDate(DateTime.Today);
            }
        }
        public string AddDays(string persianDate, double value)
        {
            try
            {
                var MiladiDate = GeneralHelper.GetMiladiDateFromShamsi(persianDate);
                return GeneralHelper.GetShamsiDate(MiladiDate.AddDays(value));
            }
            catch { return ""; }
        }

        public string AddHours(string persianDate, double value)
        {
            try
            {
                var MiladiDate = GeneralHelper.GetMiladiDateFromShamsi(persianDate);
                return GeneralHelper.GetShamsiDate(MiladiDate.AddHours(value));
            }
            catch { return ""; }
        }
        public string AddMinutes(string persianDate, double value)
        {
            try
            {
                var MiladiDate = GeneralHelper.GetMiladiDateFromShamsi(persianDate);
                return GeneralHelper.GetShamsiDate(MiladiDate.AddMinutes(value));
            }
            catch { return ""; }
        }

        public string AddMonths(string persianDate, int value)
        {
            try
            {
                var MiladiDate = GeneralHelper.GetMiladiDateFromShamsi(persianDate);
                return GeneralHelper.GetShamsiDate(MiladiDate.AddMonths(value));
            }
            catch { return ""; }
        }

        public string AddSeconds(string persianDate, double value)
        {
            try
            {
                var MiladiDate = GeneralHelper.GetMiladiDateFromShamsi(persianDate);
                return GeneralHelper.GetShamsiDate(MiladiDate.AddSeconds(value));
            }
            catch { return ""; }
        }

        public string AddTicks(string persianDate, long value)
        {
            try
            {
                var MiladiDate = GeneralHelper.GetMiladiDateFromShamsi(persianDate);
                return GeneralHelper.GetShamsiDate(MiladiDate.AddTicks(value));
            }
            catch { return ""; }
        }

        public string AddYears(string persianDate, int value)
        {
            try
            {
                var MiladiDate = GeneralHelper.GetMiladiDateFromShamsi(persianDate);
                return GeneralHelper.GetShamsiDate(MiladiDate.AddYears(value));
            }
            catch { return ""; }
        }
    }
    //public interface Helper
    //{
    //    event EventHandler<string> ValueDefined

    //}
}
