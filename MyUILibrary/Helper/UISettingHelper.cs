
//
using CommonDefinitions.UISettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyUILibrary
{
    public class UISettingHelper
    {
        //public static GridSetting DefaultPackageAreaSetting { set; get; }
        //public static void GetDefaultSettings()
        //{
        //    //webrefBasicUISetting.BasicUISettingServiceClient webref = new webrefBasicUISetting.BasicUISettingServiceClient();
        //    //webref.GetPackageAreaSettingCompleted += webref_GetPackageAreaSettingCompleted;
        //    //webref.GetPackageAreaSettingAsync();

        //    MyDataManager.BasicUISettingService webref = new MyDataManager.BasicUISettingService();
        //    DefaultPackageAreaSetting = webref.GetPackageAreaSetting();
        //}

        //static void webref_GetPackageAreaSettingCompleted(object sender, webrefBasicUISetting.GetPackageAreaSettingCompletedEventArgs e)
        //{
        //    if (e.Error == null)
        //    {
        //        DefaultPackageAreaSetting = e.Result;
        //    }
        //}

        

        internal static string GetCommandTitle( string commandName)
        {
            if (commandName.ToLower().Contains("clear"))
                return "جدید";

            if (commandName.ToLower().Contains("save"))
                return "ذخیره";

            if (commandName.ToLower().Contains("delete"))
                return "حذف";

            if (commandName.ToLower().Contains("search"))
                return "جستجو";
            if (commandName.ToLower().Contains("select"))
                return "انتخاب";
            if (commandName.ToLower().Contains("info"))
                return "اطلاعات";
            if (commandName.ToLower().Contains("add"))
                return "افزودن";
            if (commandName.ToLower().Contains("remove"))
                return "حذف مورد";
            return "";
        }
    }
}
