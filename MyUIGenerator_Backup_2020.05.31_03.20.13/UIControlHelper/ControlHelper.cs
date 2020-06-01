
using ModelEntites;
using MyUILibrary;
using MyUILibrary.EntityArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace MyUIGenerator.UIControlHelper
{
    public class ControlHelper
    {
        //static I_ControlHelper _TextboxHelper;
        //static I_ControlHelper TextboxHelper
        //{
        //    get
        //    {
        //        if (_TextboxHelper == null)
        //            _TextboxHelper = new TextBoxHelper();
        //        return _TextboxHelper;
        //    }
        //}
        //static I_ControlHelper _NumericTextBoxHelper;
        //static I_ControlHelper NumericTextBoxHelper
        //{
        //    get
        //    {
        //        if (_NumericTextBoxHelper == null)
        //            _NumericTextBoxHelper = new NumericTextBoxHelper();
        //        return _NumericTextBoxHelper;
        //    }
        //}

        //static I_ControlHelper _DatePickerHelper;
        //static I_ControlHelper DatePickerHelper
        //{
        //    get
        //    {
        //        if (_DatePickerHelper == null)
        //            _DatePickerHelper = new DatePickerHelper();
        //        return _DatePickerHelper;
        //    }
        //}

        //static I_ControlHelper _CheckBoxHelper;
        //static I_ControlHelper CheckBoxHelper
        //{
        //    get
        //    {
        //        if (_CheckBoxHelper == null)
        //            _CheckBoxHelper = new CheckBoxHelper();
        //        return _CheckBoxHelper;
        //    }
        //}

        //static I_ControlHelper _KeyValueControlHelper;
        //static I_ControlHelper KeyValueControlHelper(bool valueIsTitleOrValue)
        //{
        //    if (_KeyValueControlHelper == null)
        //        _KeyValueControlHelper = new KeyValueControlHelper(valueIsTitleOrValue);
        //    return _KeyValueControlHelper;
        //}

        //I_BaseControlHelper GetControlHelper(ColumnDTO column)
        //{

        //    if (column.ColumnType == Enum_ColumnType.String)
        //        return TextboxHelper;
        //    else if (column.ColumnType == Enum_ColumnType.Numeric)
        //        return NumericTextBoxHelper;
        //    else if (column.ColumnType == Enum_ColumnType.Date)
        //        return DatePickerHelper;
        //    else if (column.ColumnType == Enum_ColumnType.Boolean)
        //        return CheckBoxHelper;
        //    else
        //        return TextboxHelper;
        //}

        public static I_ControlHelper KeyValueControlHelper(ColumnDTO column)
        {
            return new KeyValueControlHelper();
        }
        public static I_ControlHelper GetControlHelper(ColumnDTO column, ColumnUISettingDTO columnSetting, List<SimpleSearchOperator> operators = null)
        {
            if (column.ColumnType == Enum_ColumnType.String)
                return new TextBoxHelper(column, columnSetting, operators);
            else if (column.ColumnType == Enum_ColumnType.Numeric)
                return new NumericTextBoxHelper(column, columnSetting, operators);
            else if (column.ColumnType == Enum_ColumnType.Date
                || column.ColumnType == Enum_ColumnType.Time
                || column.ColumnType == Enum_ColumnType.DateTime)
                return new DateTimePickerHelper(column, columnSetting, operators);
            else if (column.ColumnType == Enum_ColumnType.Boolean)
                return new CheckBoxHelper(column, columnSetting, operators);
            else
                return new TextBoxHelper(column, columnSetting, operators);
        }

        //internal I_SimpleControlManager GenerateControl(ColumnDTO column, ColumnUISettingDTO columnSetting, List<SimpleSearchOperator> operators = null)
        //{

        //}
        //internal I_SimpleControlManager GenerateKeyValueControl(List<ColumnKeyValueRangeDTO> keyValues, bool valueIsKeyOrTitle, ColumnDTO column, ColumnUISettingDTO columnSetting, List<SimpleSearchOperator> operators = null)
        //{
        //    var conreolHelper = new KeyValueControlHelper(keyValues, valueIsKeyOrTitle);
        //    var uiControlGroup = new LocalControlManager(conreolHelper, column, columnSetting, operators);

        //    return uiControlGroup;
        //}

        //internal UIControlPackage GenerateKeyValueControl(List<ColumnKeyValueRangeDTO> keyValues, bool valueIsKeyOrTitle, ColumnUISettingDTO columnSetting, List<SimpleSearchOperator> operators = null)
        //{
        //    var uiPackage = KeyValueControlHelper.GenerateControl(keyValues, valueIsKeyOrTitle, columnSetting, operators); ;
        //    uiPackage.BaseControlHelper = KeyValueControlHelper;
        //    return uiPackage;
        //}

        //internal UIControlPackage GenerateControlPackage(object view, ColumnUISettingDTO columnSetting)
        //{
        //    UIControlPackage package = new UIControlPackage();


        //    UIControl control = new UIControl();
        //    //control.UIControlSetting = new UIControlSetting();
        //    //if (columnSetting.aG_EnumViewControlInsertionMode == AG_EnumViewControlInsertionMode.NewLine)
        //    //    control.UIControlSetting.DesieredColumns = ColumnWidth.Full;
        //    //else
        //    //    control.UIControlSetting.DesieredColumns =  ColumnWidth.Normal;
        //    //control.UIControlSetting.DesieredRows = 1;
        //    //control.UIControlSetting = columnSetting.UISetting;


        //    //////control.Control = view;
        //    //////package.UIControl = control;
        //    return package;
        //}
        //internal static UIControlSetting GenerateUISetting(ColumnDTO nD_Type_Property, UISetting.DataPackageUISetting.UI_PackagePropertySetting uI_PackagePropertySetting)
        //{
        //    throw new NotImplementedException();
        //}

        //internal bool SetControlValue(ColumnDTO column, UIControlPackage controlPackage, string value, ColumnSetting columnSetting)
        //{
        //    return GetControlHelper(column).SetValue(controlPackage, value, columnSetting);
        //}
        //internal bool SetKeyValueControlValue(UIControlPackage controlPackage, string value, ColumnSetting columnSetting)
        //{
        //    return KeyValueControlHelper.SetValue(controlPackage, value, columnSetting);
        //}
        ////internal static bool SetValue(ColumnControl typePropertyControl, string value)
        ////{
        ////    if (typePropertyControl.UI_PropertySetting.PropertyType == CommonDefinitions.CommonUISettings.Enum_UI_PropertyType.Text)
        ////    {
        ////        return ControlHelpers.TextBoxHelper.SetValue(typePropertyControl, value);
        ////    }
        ////    else
        ////        return ControlHelpers.TextBoxHelper.SetValue(typePropertyControl, value);
        ////}

        //internal string GetControlValue(ColumnDTO column, UIControlPackage controlPackage)
        //{
        //    return GetControlHelper(column).GetValue(controlPackage, column);
        //}
        //internal Tuple<string, string> GetKeyValueControlValue(UIControlPackage controlPackage)
        //{
        //    return KeyValueControlHelper.GetValue(controlPackage);
        //}
        //internal SimpleSearchOperator GetOperator(ColumnDTO column, UIControlPackage controlPackage)
        //{
        //    return GetControlHelper(column).GetOperator(controlPackage);
        //}


        //internal void EnableDisable(ColumnDTO column, UIControlPackage controlPackage, bool enable)
        //{
        //    GetControlHelper(column).EnableDisable(controlPackage, enable);
        //}

        //internal void SetReadonly(ColumnDTO column, UIControlPackage controlPackage, bool isreadonly)
        //{
        //    GetControlHelper(column).SetReadonly(controlPackage, isreadonly);
        //}




        //public static UIControl GetUIControl(UIControlPackage aG_UIControlPackage)
        //{
        //    return aG_UIControlPackage.UIControls.First();
        //}

    }
}
