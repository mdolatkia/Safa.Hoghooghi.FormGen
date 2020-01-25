


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManagerBusiness.PackageManager
{
    //public class BizUISetting
    //{
    //    public Guid UpdateUITypeSettings(DataManager.DataPackage.DataPackageUISetting.DPUI_TypeSettings ndItem, bool generateUISetting)
    //    {

    //        //try
    //        //{

    //            using (IdeaEntities context = new IdeaEntities())
    //            {
    //                if (generateUISetting)
    //                {
    //                    ndItem.ItemSetting = GeneratePropertyItemSettingsFromNDTypeID(ndItem.TypeID);
    //                }
    //                DB_UI_TypeSetting editItem = UITypeSettingNDToDB(ndItem, context);
    //                if (editItem.ID == Guid.Empty)
    //                {
    //                    editItem.ID = Guid.NewGuid();
    //                    context.DB_UI_TypeSetting.Add(editItem);
    //                }

    //                context.SaveChanges();
    //                return editItem.ID;
    //            }

    //        //}
    //        //catch (Exception ex)
    //        //{
    //        //    throw ex;
    //        //}


    //    }

    //    private List<DPUI_ItemSetting> GeneratePropertyItemSettingsFromNDTypeID(Guid typeID)
    //    {
    //        DataMasterBusiness.EntityDefinition.BizType bizType = new DataMasterBusiness.EntityDefinition.BizType();
    //        var ndType = bizType.GetNDTypeById(typeID, true);
    //        List<DPUI_ItemSetting> result = new List<DPUI_ItemSetting>();
    //        CommonBusiness.CommonUISettings.BizPropertySettingGenerator bizPropertySettingGenerator = new CommonBusiness.CommonUISettings.BizPropertySettingGenerator();
    //        //using (IdeaEntities context = new IdeaEntities())
    //        //{
    //        //    var dbType = context.DB_Type.Where(x => x.ID == typeID).FirstOrDefault();
    //        //    if (dbType != null)
    //        //    {
    //        foreach (var typeProprty in ndType.Properties)
    //        {
    //            //string dataType = "";
    //            //var dataTypeTag = dbTypeProprty.DB_Type_Property_PropertyTag.FirstOrDefault(x => x.DB_PropertyTag.Name == GlobalVariables.PropertyTag.DataType);
    //            //if (dataTypeTag != null)
    //            //    dataType = dataTypeTag.Value;

    //            //if (dataType != null)
    //            //{
    //            DPUI_ItemSetting setting = new DPUI_ItemSetting();
    //            setting.PropertySetting = bizPropertySettingGenerator.GetPropertySetting(ndType.Categories, typeProprty);
    //            setting.ItemType = Enum_DPUI_ItemType.Property;


    //            //switch (dataType)
    //            //{
    //            //    case "char":
    //            //    case "varchar":
    //            //    case "text":
    //            //    case "nchar":
    //            //    case "nvarchar":
    //            //    case "ntext":
    //            //        {
    //            //            setting.PropertySetting.TextPropertySetting = new DPUI_TextPropertySetting();
    //            //            setting.PropertySetting.PropertyType = CommonDefinitions.CommonUISettings.Enum_UI_PropertyType.Text;
    //            //            var maximunLengthTag = dbTypeProprty.DB_Type_Property_PropertyTag.FirstOrDefault(x => x.DB_PropertyTag.Name == GlobalVariables.PropertyTag.MaximumLength);
    //            //            if (maximunLengthTag != null)
    //            //            {
    //            //                setting.PropertySetting.TextPropertySetting.MaximumLength = Convert.ToInt16(maximunLengthTag.Value);
    //            //            }
    //            //            break;
    //            //        }
    //            //    case "bigint":
    //            //    case "numeric":
    //            //    case "bit":
    //            //    case "smallint":
    //            //    case "decimal":
    //            //    case "smallmoney":
    //            //    case "int":
    //            //    case "tinyint":
    //            //    case "money":
    //            //        {
    //            //            //    setting.TextPropertySetting = new ();
    //            //            setting.PropertySetting.PropertyType = CommonDefinitions.CommonUISettings.Enum_UI_PropertyType.Numeric;
    //            //            break;
    //            //        }
    //            //    default:
    //            //        {
    //            //            //    setting.TextPropertySetting = new ();
    //            //            setting.PropertySetting.PropertyType = CommonDefinitions.CommonUISettings.Enum_UI_PropertyType.Numeric;
    //            //            break;
    //            //        }
    //            //}

    //            result.Add(setting);
    //            //}
    //            //    }
    //            //}
    //        }

    //        return result;
    //    }
    //    public DB_UI_TypeSetting UITypeSettingNDToDB(DPUI_TypeSettings ndItem, IdeaEntities context)
    //    {
    //        DB_UI_TypeSetting result;

    //        result = context.DB_UI_TypeSetting.FirstOrDefault(x => x.ID == ndItem.ID);
    //        if (result == null)
    //        {
    //            result = new DB_UI_TypeSetting();
    //            result.TypeID = ndItem.TypeID;
    //            result.TypeConditionID = ndItem.TypeConditionID;
    //            foreach (var item in ndItem.ItemSetting)
    //            {
    //                DB_UI_ItemSetting editDB_Package_ItemSetting = PackageItemSettingNDToDB(item, context, result);
    //                if (editDB_Package_ItemSetting.ID == Guid.Empty)
    //                {
    //                    editDB_Package_ItemSetting.ID = Guid.NewGuid();
    //                    result.DB_UI_ItemSetting.Add(editDB_Package_ItemSetting);
    //                }
    //            }
    //        }

    //        return result;
    //    }

    //    private DB_UI_ItemSetting PackageItemSettingNDToDB(DPUI_ItemSetting ndItem, IdeaEntities context, DB_UI_TypeSetting dbDPTypeSetting)
    //    {
    //        DB_UI_ItemSetting result;

    //        result = dbDPTypeSetting.DB_UI_ItemSetting.FirstOrDefault(x => x.ID == ndItem.ID);
    //        if (result == null)
    //        {
    //            result = new DB_UI_ItemSetting();
    //            result.Priority = ndItem.Priority;
    //            result.ItemType = (Int16)ndItem.ItemType;

    //            if (ndItem.PropertySetting != null)
    //            {
    //                DB_UI_PropertySetting editDB_Package_PropertySetting = PackagePropertySettingNDToDB(ndItem.PropertySetting, context, result);
    //                if (editDB_Package_PropertySetting.ID == Guid.Empty)
    //                {
    //                    editDB_Package_PropertySetting.ID = Guid.NewGuid();
    //                    result.DB_UI_PropertySetting.Add(editDB_Package_PropertySetting);
    //                }
    //            }
    //        }

    //        return result;
    //    }

    //    private DB_UI_PropertySetting PackagePropertySettingNDToDB(UI_PropertySetting ndItem, IdeaEntities context, DB_UI_ItemSetting dbDPItemSetting)
    //    {

    //        DB_UI_PropertySetting result;

    //        result = dbDPItemSetting.DB_UI_PropertySetting.FirstOrDefault(x => x.ID == ndItem.ID);
    //        if (result == null)
    //        {
    //            result = new DB_UI_PropertySetting();
    //            result.PropertyID = ndItem.PropertyID;
    //            if (ndItem.TextPropertySetting != null)
    //            {
    //                DB_UI_TextPropertySetting editDB_Package_PropertySetting = PackageTextPropertySettingNDToDB(ndItem.TextPropertySetting, context, result);
    //                if (editDB_Package_PropertySetting.ID == Guid.Empty)
    //                {
    //                    editDB_Package_PropertySetting.ID = Guid.NewGuid();
    //                    result.DB_UI_TextPropertySetting.Add(editDB_Package_PropertySetting);
    //                }
    //            }
    //        }

    //        return result;
    //    }

    //    private DB_UI_TextPropertySetting PackageTextPropertySettingNDToDB(UI_TextPropertySetting ndItem, IdeaEntities context, DB_UI_PropertySetting dbDPPropertySetting)
    //    {
    //        DB_UI_TextPropertySetting result;

    //        result = dbDPPropertySetting.DB_UI_TextPropertySetting.FirstOrDefault(x => x.ID == ndItem.ID);
    //        if (result == null)
    //        {
    //            result = new DB_UI_TextPropertySetting();
    //            result.Type = (Int16)ndItem.Type;
    //            //maxlength

    //        }

    //        return result;
    //    }


    //    public DPUI_TypeSettings GetUITypeSettings(Guid typeID, Guid? typeConditionID)
    //    {

    //        try
    //        {
    //            DPUI_TypeSettings result = null;
    //            using (IdeaEntities context = new IdeaEntities())
    //            {
    //                DB_UI_TypeSetting dbItem;
    //                if (typeConditionID != null)
    //                    dbItem = context.DB_UI_TypeSetting.Where(x => x.TypeID == typeID && x.TypeConditionID == typeConditionID).FirstOrDefault();
    //                else
    //                    dbItem = context.DB_UI_TypeSetting.Where(x => x.TypeID == typeID).FirstOrDefault();
    //                if (dbItem != null)
    //                    result = PackageTypeSettingDBToND(dbItem, false);
    //            }
    //            return result;

    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }

    //    }


    //    public DPUI_TypeSettings PackageTypeSettingDBToND(DB_UI_TypeSetting dbItem, bool complex)
    //    {
    //        DPUI_TypeSettings result = new DPUI_TypeSettings(); ;

    //        result.ID = dbItem.ID;
    //        result.TypeID = dbItem.TypeID;
    //        result.TypeConditionID = dbItem.TypeConditionID;
    //        foreach (var item in dbItem.DB_UI_ItemSetting)
    //        {
    //            result.ItemSetting.Add(PackageItemSettingDBToND(item, complex));
    //        }

    //        return result;
    //    }

    //    public DPUI_ItemSetting PackageItemSettingDBToND(DB_UI_ItemSetting dbItem, bool complex)
    //    {
    //        DPUI_ItemSetting result = new DPUI_ItemSetting();


    //        result.ID = dbItem.ID;
    //        result.Priority = Convert.ToInt32(dbItem.Priority);
    //        result.ItemType = (Enum_DPUI_ItemType)dbItem.ItemType;

    //        //foreach (var item in dbItem.DB_UI_ItemGroup)
    //        //{
    //        //    result.ItemSetting.Add(PackagePropertySettingNDToDB(item, complex));
    //        //}


    //        foreach (var item in dbItem.DB_UI_PropertySetting)
    //        {
    //            result.PropertySetting = PackagePropertySettingDBToND(item, complex);
    //        }
    //        return result;
    //    }

    //    private UI_PropertySetting PackagePropertySettingDBToND(DB_UI_PropertySetting dbItem, bool complex)
    //    {
    //        UI_PropertySetting result = new UI_PropertySetting();
    //        result.ID = dbItem.ID;
    //        result.PropertyID = dbItem.PropertyID;
    //        foreach (var item in dbItem.DB_UI_TextPropertySetting)
    //        {
    //            result.TextPropertySetting = PackageTextPropertySettingDBToND(item, complex);
    //        }




    //        return result;
    //    }
    //    private UI_TextPropertySetting PackageTextPropertySettingDBToND(DB_UI_TextPropertySetting dbItem, bool complex)
    //    {
    //        UI_TextPropertySetting result = new UI_TextPropertySetting();

    //        result.ID = dbItem.ID;
    //        result.MaximumLength = Convert.ToInt16(dbItem.MaximumLength);
    //        result.Type = (Enum_UI_TextPropertyType)dbItem.Type;
    //        return result;
    //    }

    //}
}
