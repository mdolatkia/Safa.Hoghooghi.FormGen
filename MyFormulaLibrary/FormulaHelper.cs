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

namespace MyFormulaFunctionStateFunctionLibrary
{
    public class FormulaHepler
    {
        public FormulaHepler()
        {
            StringHelper = new StringHelper();
            PersianDateHelper = new PersianDate();
        }
        public StringHelper StringHelper { set; get; }
        public PersianDate PersianDateHelper { set; get; }

        public static Type IsHelperType(string theWord)
        {
            if (!string.IsNullOrEmpty(theWord))
            {
                if (theWord.ToLower() == "PersianDateHelper".ToLower())
                    return typeof(PersianDate);
                else if (theWord.ToLower() == "StringHelper".ToLower())
                    return typeof(StringHelper);
            }
            return null;
        }
    }
    public class StringHelper
    {
        public bool IsNullOrEmpty(string str)
        {
            return String.IsNullOrEmpty(str);
        }
    }

    public class PersianDate
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
