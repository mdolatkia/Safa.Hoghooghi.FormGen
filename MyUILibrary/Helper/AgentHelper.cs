using CommonDefinitions.UISettings;
using ModelEntites;
using MyDataManagerService;

using MyUILibrary;
using MyUILibrary.EntityArea;
using MyUILibrary.EntityArea.Commands;

using ProxyLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyUILibrary
{
    public class AgentHelper
    {

        internal static byte[] GetBytesFromFilePath(string path)
        {
            return System.IO.File.ReadAllBytes(path);
        }

        public static T Clone<T>(T obj)
        {
            DataContractSerializer dcSer = new DataContractSerializer(obj.GetType());
            MemoryStream memoryStream = new MemoryStream();

            dcSer.WriteObject(memoryStream, obj);
            memoryStream.Position = 0;

            T newObject = (T)dcSer.ReadObject(memoryStream);
            return newObject;
        }
        public static void SetPropertyTitle(RelationshipColumnControl propertyControl)
        {
            //title += (title == "" ? "" : " > ") + (propertyControl as RelationSourceControl).EditNdTypeArea.AreaInitializer.TemplateEntity.Name + "." + (propertyControl as RelationSourceControl).EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType.ToString();
            var relationshipAlias = propertyControl.Relationship.Alias;
            propertyControl.Alias = (string.IsNullOrEmpty(relationshipAlias) ? "" : relationshipAlias + " : ");// + propertyControl.Column.Alias;
        }

        internal static string GetUniqueDataPostfix(DP_DataRepository dataItem, string result = "", RelationshipDTO relationship = null)
        {
            if (result == "")
                result = "_";

            if (dataItem.ParantChildRelationshipInfo != null)
                return result += "&" + dataItem.ParantChildRelationshipInfo.Relationship.ID + GetUniqueDataPostfix(dataItem.ParantChildRelationshipInfo.SourceData, result, dataItem.ParantChildRelationshipInfo.Relationship);
            else
                return "";
        }



        //public static void SetPropertyTitleOneData(RelationshipColumnControlOneData propertyControl)
        //{
        //    //title += (title == "" ? "" : " > ") + (propertyControl as RelationSourceControl).EditNdTypeArea.AreaInitializer.TemplateEntity.Name + "." + (propertyControl as RelationSourceControl).EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType.ToString();
        //    var relationshipAlias = propertyControl.Relationship.Alias;
        //    propertyControl.Alias = (string.IsNullOrEmpty(relationshipAlias) ? "" : relationshipAlias + " : ");// + propertyControl.Column.Alias;
        //}
        //public static void SetPropertyTitleMultipleData(RelationshipColumnControlMultipleData propertyControl)
        //{
        //    //title += (title == "" ? "" : " > ") + (propertyControl as RelationSourceControl).EditNdTypeArea.AreaInitializer.TemplateEntity.Name + "." + (propertyControl as RelationSourceControl).EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType.ToString();
        //    var relationshipAlias = propertyControl.Relationship.Alias;
        //    propertyControl.Alias = (string.IsNullOrEmpty(relationshipAlias) ? "" : relationshipAlias + " : ");// + propertyControl.Column.Alias;
        //}
        //public static void SetPropertyTitleOneData(SimpleColumnControlOneData propertyControl)
        //{

        //    if (propertyControl.Column != null)
        //        if (!string.IsNullOrEmpty(propertyControl.Column.Alias))
        //            propertyControl.Alias = propertyControl.Column.Alias;
        //        else if (!string.IsNullOrEmpty(propertyControl.Column.Name))
        //            propertyControl.Alias = propertyControl.Column.Name;
        //}
        public static void SetPropertyTitle(SimpleColumnControl propertyControl)
        {

            if (propertyControl.Column != null)
                if (!string.IsNullOrEmpty(propertyControl.Column.Alias))
                    propertyControl.Alias = propertyControl.Column.Alias;
                else if (!string.IsNullOrEmpty(propertyControl.Column.Name))
                    propertyControl.Alias = propertyControl.Column.Name;
        }
        public static void SetPropertyTitle(SimpleSearchColumnControl propertyControl)
        {

            if (propertyControl.Column != null)
                if (!string.IsNullOrEmpty(propertyControl.Column.Alias))
                    propertyControl.Alias = propertyControl.Column.Alias;
                else if (!string.IsNullOrEmpty(propertyControl.Column.Name))
                    propertyControl.Alias = propertyControl.Column.Name;
        }

        //public static string SetPropertyTitle(BaseColumnControl propertyControl)
        //{
        //    var title = "";
        //    if (propertyControl.Relationship != null)
        //    {
        //        //title += (title == "" ? "" : " > ") + (propertyControl as RelationSourceControl).EditNdTypeArea.AreaInitializer.TemplateEntity.Name + "." + (propertyControl as RelationSourceControl).EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType.ToString();
        //        var relationshipAlias = propertyControl.Relationship.Alias;
        //        propertyControl.Alias = (string.IsNullOrEmpty(relationshipAlias) ? "" : relationshipAlias + " : ") + propertyControl.Column.Alias;
        //    }
        //    else
        //    {
        //        if (propertyControl.Column != null)
        //            if (!string.IsNullOrEmpty(propertyControl.Column.Alias))
        //                propertyControl.Alias = propertyControl.Column.Alias;
        //            else if (!string.IsNullOrEmpty(propertyControl.Column.Name))
        //                propertyControl.Alias = propertyControl.Column.Name;
        //    }
        //    return title;
        //}
        //internal static EditEntityArea GetEditEntityAreaByRelationID(EditEntityArea sourceEditEntityArea, long relID)
        //{
        //  //foreach(var columnControl in sourceEditEntityArea.ColumnControls)
        //  //  {
        //  //      if(columnControl.Relationship!=null)

        //  //  }
        //}



        //internal static Temp.InfoColor GetPropertyColor(BaseColumnControl propertyControl)
        //{
        //    if (propertyControl.Alias == "BusinessEntityID")
        //    {
        //        Temp.InfoColor color = Temp.InfoColor.Red;
        //        return color;
        //    }
        //    else if (propertyControl.Alias == "PersonType")
        //    {
        //        Temp.InfoColor color = Temp.InfoColor.Blue;
        //        return color;
        //    }
        //    {
        //        Temp.InfoColor color = Temp.InfoColor.Black;
        //        return color;
        //    }
        //}
        internal static Temp.InfoColor GetPropertyColor(RelationshipColumnControl propertyControl)
        {
            Temp.InfoColor color = Temp.InfoColor.Black;
            //title += (title == "" ? "" : " > ") + (propertyControl as RelationSourceControl).EditNdTypeArea.AreaInitializer.TemplateEntity.Name + "." + (propertyControl as RelationSourceControl).EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType.ToString();
            if (propertyControl.Relationship.IsOtherSideMandatory)
                color = Temp.InfoColor.DarkRed;
            return color;
        }

        //internal static List<MyDataObject> GetDataObjects(List<DP_DataRepository> resultDataItems)
        //{
        //    List<MyDataObject> result = new List<MyDataObject>();
        //    resultDataItems.ForEach(x => result.Add(new MyDataObject(x)));
        //    return result;
        //}

        //internal static DP_DataRepository GetEquivalentDataItem(I_EditEntityArea editArea, DP_DataRepository item)
        //{
        //    return editArea.AreaInitializer.Datas.FirstOrDefault(x => x.KeyProperties.All(y => item.KeyProperties.Any(z => y.ColumnID == z.ColumnID && y.Value == z.Value))
        //     && item.KeyProperties.All(y => x.KeyProperties.Any(z => y.ColumnID == z.ColumnID && y.Value == z.Value)));
        //}

        //public static string GetPropertyTooltip(SimpleSearchColumnControl propertyControl)
        //{
        //    var title = "";
        //    if (propertyControl.Column != null)
        //        if (!string.IsNullOrEmpty(propertyControl.Column.DBFormula))
        //        {
        //            title += (title == "" ? "" : Environment.NewLine) + "DB Formula : " + propertyControl.Column.DBFormula;
        //        }
        //    return title;
        //}
        //public static string GetPropertyTooltip(SimpleColumnControl propertyControl)
        //{
        //    var title = "";
        //    if (propertyControl.Column != null)
        //        if (!string.IsNullOrEmpty(propertyControl.Column.DBFormula))
        //        {
        //            title += (title == "" ? "" : Environment.NewLine) + "DB Formula : " + propertyControl.Column.DBFormula;
        //        }
        //    return title;
        //}
        //public static string GetPropertyTooltip(RelationshipColumnControl propertyControl)
        //{
        //    var title = "";

        //    if (propertyControl.Relationship != null)
        //    {
        //        //title += (title == "" ? "" : " > ") + (propertyControl as RelationSourceControl).EditNdTypeArea.AreaInitializer.TemplateEntity.Name + "." + (propertyControl as RelationSourceControl).EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType.ToString();
        //        title = propertyControl.Relationship.Name + "___" + propertyControl.Relationship.TypeStr;
        //    }
        //    return title;
        //}
        //public static string GetPropertyTooltip(RelationshipSearchColumnControl propertyControl)
        //{
        //    var title = "";

        //    if (propertyControl.Relationship != null)
        //    {
        //        //title += (title == "" ? "" : " > ") + (propertyControl as RelationSourceControl).EditNdTypeArea.AreaInitializer.TemplateEntity.Name + "." + (propertyControl as RelationSourceControl).EditNdTypeArea.AreaInitializer.SourceRelation.RelationshipType.ToString();
        //        title = propertyControl.Relationship.Name + "___" + propertyControl.Relationship.TypeStr;
        //    }
        //    return title;
        //}
        //internal static List<DP_DataRepository> ExtractAreaInitializerData(EditEntityAreaInitializer AreaInitializer, DP_DataRepository sourceRelatedData = null)
        //{
        //    if (AreaInitializer.SourceRelation == null && sourceRelatedData != null)
        //    {
        //        throw new Exception("asd");
        //    }
        //    if (AreaInitializer.SourceRelation != null && sourceRelatedData == null)
        //    {
        //        throw new Exception("asd");
        //    }

        //    List<DP_DataRepository> dataResult = new List<DP_DataRepository>();
        //    foreach (var data in AreaInitializer.Data)
        //    {
        //        if (AreaInitializer.SourceRelation == null)
        //            dataResult.Add(data);
        //        else
        //        {
        //            if (data.ParentRelationInfos.Any(x => x.SourceRelatedData == sourceRelatedData))
        //                dataResult.Add(data);
        //        }

        //    }
        //    return dataResult;
        //}



        //internal static DP_SearchRepository ExtractSearchAreaInitializerData(SearchEntityAreaInitializer areaInitializer, DP_SearchRepository sourceRelatedData = null)
        //{
        //    //int relationID = 0;

        //    if (sourceRelatedData == null)
        //        if (areaInitializer.SourceRelation != null)
        //        {
        //            //    relationID = areaInitializer.SourceRelation.Relationship.ID;
        //            sourceRelatedData = areaInitializer.SourceRelation.RelatedData;
        //        }

        //    DP_SearchRepository dataResult = null;
        //    //foreach (var data in areaInitializer.SearchData)
        //    //{
        //    if (sourceRelatedData == null)
        //    {
        //        if (areaInitializer.SearchData.SourceRelatedData == null)
        //            dataResult = areaInitializer.SearchData;
        //    }
        //    else
        //    {
        //        if (areaInitializer.SearchData != null && areaInitializer.SearchData.SourceRelatedData != null)
        //            if (sourceRelatedData == areaInitializer.SearchData.SourceRelatedData)
        //                dataResult = areaInitializer.SearchData;
        //    }
        //    //}
        //    //}
        //    return dataResult;
        //    //////    return AgentHelper.ExtractDataFromDataRepository1(areaInitializer.Data, sourceRelatedData);
        //}
        //public static bool IsColumnValueChanged(SimpleColumnControl typePropertyControl, string currentValue, string controlValue)
        //{
        //    if (controlValue == "<Null>")
        //        controlValue = "";
        //    if (currentValue == "<Null>")
        //        currentValue = "";
        //    if (typePropertyControl.Column.DataType == "bit")
        //    {
        //        if (controlValue == "False")
        //            controlValue = "0";
        //        if (currentValue == "False")
        //            currentValue = "0";

        //        if (controlValue == "True")
        //            controlValue = "1";
        //        if (currentValue == "True")
        //            currentValue = "1";
        //    }



        //    var changed = !(currentValue == controlValue);

        //    if (changed)
        //    {

        //    }
        //    else
        //    {

        //    }
        //    return changed;
        //}
        //internal static List<ManyToOneRelationshipDTO> GetManyToOneRelationships(EditEntityAreaInitializer areaInitializer)
        //{
        //    List<ManyToOneRelationshipDTO> result = new List<ManyToOneRelationshipDTO>();
        //    foreach (var relationship in areaInitializer.TemplateEntity.ManyToOneRelationships.Where(x => x.Enabled == true || x.Enabled == null))
        //    {
        //        if (RelationshipIsValid(areaInitializer, relationship))
        //            result.Add(relationship);
        //    }
        //    return result;


        //}
        //internal static object SelectDataFromDB(long relID, DP_DataRepository relationData)
        //{
        //    return AgentUICoreMediator.GetData(relID, selectList);
        //}

        //موقع نمایش داده ها در ادیت انتیتی های مرتبط با هم صدا زده میشود

        //دوجا صدا زده میشود یکی موقع انتخاب شدن داده ها از ویوی نتیجه جستجو




        //internal static List<OneToManyRelationshipDTO> GetOneToManyRelationships(EditEntityAreaInitializer areaInitializer)
        //{
        //    List<OneToManyRelationshipDTO> result = new List<OneToManyRelationshipDTO>();
        //    foreach (var relationship in areaInitializer.TemplateEntity.OneToManyRelationships.Where(x => x.Enabled == true || x.Enabled == null))
        //    {
        //        if (RelationshipIsValid(areaInitializer, relationship))
        //            result.Add(relationship);
        //    }
        //    return result;


        //}




        //internal static List<ExplicitOneToOneRelationshipDTO> GetExplicitOntToOneRelationships(EditEntityAreaInitializer areaInitializer)
        //{
        //    List<ExplicitOneToOneRelationshipDTO> result = new List<ExplicitOneToOneRelationshipDTO>();
        //    foreach (var relationship in areaInitializer.TemplateEntity.ExplicitOneToOneRelationships.Where(x => x.Enabled == true || x.Enabled == null))
        //    {
        //        if (RelationshipIsValid(areaInitializer, relationship))
        //            result.Add(relationship);

        //    }
        //    return result;


        //}
        //internal static List<ImplicitOneToOneRelationshipDTO> GetImplicitOntToOneRelationships(EditEntityAreaInitializer areaInitializer)
        //{
        //    List<ImplicitOneToOneRelationshipDTO> result = new List<ImplicitOneToOneRelationshipDTO>();
        //    foreach (var relationship in areaInitializer.TemplateEntity.ImplicitOneToOneRelationships.Where(x => x.Enabled == true || x.Enabled == null))
        //    {
        //        if (RelationshipIsValid(areaInitializer, relationship))
        //            result.Add(relationship);
        //    }
        //    return result;


        //}


        //internal static List<SuperToSubRelationshipDTO> GetSuperToSubRelationshipRelationships(EditEntityAreaInitializer areaInitializer)
        //{
        //    List<SuperToSubRelationshipDTO> result = new List<SuperToSubRelationshipDTO>();
        //    foreach (var relationship in areaInitializer.TemplateEntity.SuperToSubRelationships.Where(x => x.Enabled == true || x.Enabled == null))
        //    {
        //        if (RelationshipIsValid(areaInitializer, relationship))
        //            result.Add(relationship);
        //    }
        //    return result;


        //}
        //internal static List<SubToSuperRelationshipDTO> GetSubToSuperRelationshipRelationships(EditEntityAreaInitializer areaInitializer)
        //{
        //    List<SubToSuperRelationshipDTO> result = new List<SubToSuperRelationshipDTO>();
        //    foreach (var relationship in areaInitializer.TemplateEntity.SubToSuperRelationships.Where(x => x.Enabled == true || x.Enabled == null))
        //    {
        //        if (RelationshipIsValid(areaInitializer, relationship))
        //            result.Add(relationship);
        //    }
        //    return result;


        //}
        //internal static List<SuperUnionToSubUnionRelationshipDTO> GetUnionToSubUnion_UnionHoldsKeysRelationships(EditEntityAreaInitializer areaInitializer)
        //{
        //    List<SuperUnionToSubUnionRelationshipDTO> result = new List<SuperUnionToSubUnionRelationshipDTO>();
        //    foreach (var relationship in areaInitializer.TemplateEntity.SuperUnionToSubUnionRelationships.Where(x => x.UnionHoldsKeys == true && (x.Enabled == true || x.Enabled == null)))
        //    {
        //        if (RelationshipIsValid(areaInitializer, relationship))
        //            result.Add(relationship);
        //    }
        //    return result;


        //}
        //internal static List<SuperUnionToSubUnionRelationshipDTO> GetUnionToSubUnion_SubUnionHoldsKeysRelationships(EditEntityAreaInitializer areaInitializer)
        //{
        //    List<SuperUnionToSubUnionRelationshipDTO> result = new List<SuperUnionToSubUnionRelationshipDTO>();
        //    foreach (var relationship in areaInitializer.TemplateEntity.SuperUnionToSubUnionRelationships.Where(x => x.UnionHoldsKeys != true &&( x.Enabled == true || x.Enabled == null)))
        //    {
        //        if (RelationshipIsValid(areaInitializer, relationship))
        //            result.Add(relationship);
        //    }
        //    return result;


        //}
        //internal static List<SubUnionToSuperUnionRelationshipDTO> GetSubUnionToUnion_UnionHoldsKeysRelationships(EditEntityAreaInitializer areaInitializer)
        //{
        //    List<SubUnionToSuperUnionRelationshipDTO> result = new List<SubUnionToSuperUnionRelationshipDTO>();
        //    foreach (var relationship in areaInitializer.TemplateEntity.SubUnionToSuperUnionRelationships.Where(x => x.UnionHoldsKeys == true && (x.Enabled == true || x.Enabled == null)))
        //    {
        //        if (RelationshipIsValid(areaInitializer, relationship))
        //            result.Add(relationship);
        //    }
        //    return result;


        //}
        //internal static List<SubUnionToSuperUnionRelationshipDTO> GetSubUnionToUnion_SubUnionHoldsKeysRelationships(EditEntityAreaInitializer areaInitializer)
        //{
        //    List<SubUnionToSuperUnionRelationshipDTO> result = new List<SubUnionToSuperUnionRelationshipDTO>();
        //    foreach (var relationship in areaInitializer.TemplateEntity.SubUnionToSuperUnionRelationships.Where(x => x.UnionHoldsKeys != true && (x.Enabled == true || x.Enabled == null)))
        //    {
        //        if (RelationshipIsValid(areaInitializer, relationship))
        //            result.Add(relationship);
        //    }
        //    return result;


        //}


        //یک قلم داده جدید ایجاد میکند
        internal static DP_DataRepository CreateAreaInitializerNewData(I_EditEntityArea editEntityArea)
        {
            if (editEntityArea is I_EditEntityAreaMultipleData)
            {
                if (editEntityArea.DataEntryEntity.IsReadonly)
                {
                    throw new Exception("Entity is readonly!");
                }
                if (editEntityArea.AreaInitializer.SourceRelation != null)
                {
                    if (editEntityArea.AreaInitializer.SourceRelation.Relationship.IsReadonly)
                        throw new Exception("Relationship is readonly!");
                }
            }
            var result = new DP_DataRepository(editEntityArea.AreaInitializer.EntityID, editEntityArea.SimpleEntity.Alias);

            result.EntityListView = editEntityArea.DefaultEntityListViewDTO;
            //result.TargetEntityID = editEntityArea.AreaInitializer.EntityID;
            // result.DataInstance = new EntityInstance();// Clone<TableDrivedEntityDTO>(AreaInitializer.Template);
            result.IsFullData = true;

            //result.Properties = new List<EntityInstanceProperty>();
            result.IsNewItem = true;
            List<ColumnDTO> columns = null;
            //فرض بر اینه که تنها ستونهایی که کنترل دارند اضافه شود.تازه برای روابط فارن به اصلی بهتر است ستونها کلید خارجی نیز اضافه شوند
            if (editEntityArea is I_EditEntityAreaOneData)
            {
                columns = (editEntityArea as I_EditEntityAreaOneData).SimpleColumnControls.Cast<SimpleColumnControl>().Select(x => x.Column).ToList();
                foreach (var relationshipControl in (editEntityArea as I_EditEntityAreaOneData).RelationshipColumnControls.Where(x => x.Relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary))
                {
                    foreach (var col in relationshipControl.Relationship.RelationshipColumns)
                    {
                        if (!columns.Any(x => x.ID == col.FirstSideColumnID))
                            columns.Add(editEntityArea.FullEntity.Columns.First(x => x.ID == col.FirstSideColumnID));
                    }
                }
                if ((editEntityArea as I_EditEntityAreaOneData).AreaInitializer.SourceRelation != null)
                {
                    foreach (var col in (editEntityArea as I_EditEntityAreaOneData).AreaInitializer.SourceRelation.Relationship.RelationshipColumns)
                    {
                        if (!columns.Any(x => x.ID == col.SecondSideColumnID))
                            columns.Add(editEntityArea.FullEntity.Columns.First(x => x.ID == col.SecondSideColumnID));
                    }
                }
            }
            else if (editEntityArea is I_EditEntityAreaMultipleData)
            {
                columns = (editEntityArea as I_EditEntityAreaMultipleData).SimpleColumnControls.Cast<SimpleColumnControl>().Select(x => x.Column).ToList();
                foreach (var relationshipControl in (editEntityArea as I_EditEntityAreaMultipleData).RelationshipColumnControls.Where(x => x.Relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary))
                {
                    foreach (var col in relationshipControl.Relationship.RelationshipColumns)
                    {
                        if (!columns.Any(x => x.ID == col.FirstSideColumnID))
                            columns.Add(editEntityArea.FullEntity.Columns.First(x => x.ID == col.FirstSideColumnID));
                    }
                }
                if ((editEntityArea as I_EditEntityAreaMultipleData).AreaInitializer.SourceRelation != null)
                {

                    foreach (var col in (editEntityArea as I_EditEntityAreaMultipleData).AreaInitializer.SourceRelation.Relationship.RelationshipColumns)
                    {
                        if (!columns.Any(x => x.ID == col.SecondSideColumnID))
                            columns.Add(editEntityArea.FullEntity.Columns.First(x => x.ID == col.SecondSideColumnID));
                    }

                }
            }

            //به حالت اول برمیگردانم یعنی استفاده از فول انتیتی
            //columns= editEntityArea.FullEntity.Columns;
            //همه ستونها؟؟؟
            foreach (var column in columns)
            {
                string value = "";
                //////if (editEntityArea.AreaInitializer.SourceRelation != null && result.TargetColumnIDs.Contains(column.ID)
                //////    &&
                //////    (result.SourceToTargetRelationshipType == Enum_RelationshipType.OneToMany
                //////        || result.SourceToTargetRelationshipType == Enum_RelationshipType.ImplicitOneToOne
                //////        || result.SourceToTargetRelationshipType == Enum_RelationshipType.SuperToSub
                //////        || result.SourceToTargetRelationshipType == Enum_RelationshipType.UnionToSubUnion_SubUnionHoldsKeys
                //////        || result.SourceToTargetRelationshipType == Enum_RelationshipType.SubUnionToUnion_UnionHoldsKeys))
                //////{
                //////    int index = 0;
                //////    foreach (var col in editEntityArea.AreaInitializer.SourceRelation.SourceRelationColumns)
                //////    {

                //////        if (result.TargetColumnIDs[index] == column.ID)
                //////        {
                //////            value = editEntityArea.AreaInitializer.SourceRelation.SourceEditArea.FetchTypePropertyControlValue(editEntityArea.AreaInitializer.SourceRelation.RelatedData, editEntityArea.AreaInitializer.SourceRelation.SourceEditArea.ColumnControls.First(x => x.Column.ID == col.ID));// AreaInitializer.SourceRelation.RelatedData.Properties.First(x => x.ColumnID == AreaInitializer.SourceRelation.SourceRelationColumns[index].ID).Value;
                //////        }
                //////        index++;
                //////    }
                //////}
                //////else
                //////{


                ////if (column.IsIdentity)
                ////    value = "<identity>";
                ////else
                ////{
                if (!string.IsNullOrEmpty(column.DefaultValue))
                {
                    var defVal = column.DefaultValue;
                    if (column.DefaultValue.StartsWith("(") && column.DefaultValue.EndsWith(")"))
                        defVal = column.DefaultValue.Substring(1, column.DefaultValue.Length - 2);
                    if (defVal.StartsWith("(") && defVal.EndsWith(")"))
                        defVal = defVal.Substring(1, defVal.Length - 2);
                    value = defVal;
                }
                else
                    value = (column.IsNull == true ? null : "");
                //    value = (column.IsNull == true ? "<Null>" : "");
                ////}
                //////}
                result.AddProperty(column, value);
            }
            return result;
        }
        //public static Tuple<List<ColumnDTO>, List<RelationshipDTO>> GetValidColumnsAndRelationships(TableDrivedEntityDTO entity, AssignedPermissionDTO permission)
        //{


        //    return new Tuple<List<ColumnDTO>, List<RelationshipDTO>>(ValidColumns, ValidRelationships);
        //}

        internal static bool DataItemsAreEqual(DP_BaseData item, DP_BaseData item1)
        {
            if (item.KeyProperties.Any() && item1.KeyProperties.Any())
            {
                if (item.TargetEntityID == item1.TargetEntityID)
                {
                    if (item.KeyProperties.All(x => item1.KeyProperties.Any(y => x.ColumnID == y.ColumnID && x.Value == y.Value))
                        && item1.KeyProperties.All(x => item.KeyProperties.Any(y => x.ColumnID == y.ColumnID && x.Value == y.Value)))
                        return true;
                }
            }
            return false;
        }

        //internal static DP_SearchRepository CreateNewSearchRepository(SearchEntityAreaInitializer areaInitializer)
        //{


        //}
        //internal static DP_SearchRepository CreateAreaInitializerNewData(SearchEntityAreaInitializer areaInitializer)
        //{

        //    ////var result = new DP_SearchRepository();
        //    ////if (areaInitializer.SourceRelationID != 0)
        //    ////{
        //    ////    //result.SourceRelatedData = areaInitializer.SourceRelation.RelatedData;
        //    ////    //result.SourceEntityID = areaInitializer.SourceRelation.SourceEntityID;
        //    ////    //result.SourceTableID = areaInitializer.SourceRelation.SourceTableID;
        //    ////    //result.RelationshipColumns = areaInitializer.SourceRelation.RelationshipColumns;
        //    ////    result.SourceRelationID = areaInitializer.SourceRelationID;
        //    ////    result.TargetEntityID = areaInitializer.TargetEntityID;
        //    ////    // result.TargetTableID = areaInitializer.SourceRelation.TargetTableID;
        //    ////    //result.TargetColumnIDs = areaInitializer.SourceRelation.TargetRelationColumns.Select(x => x.ID).ToList();
        //    ////    result.SourceToTargetRelationshipType = areaInitializer.SourceToTargetRelationshipType;
        //    ////    result.SourceToTargetMasterRelationshipType = areaInitializer.SourceToTargetMasterRelationshipType;
        //    ////}

        //    ////result.TargetEntityID = areaInitializer.EntityID;
        //    //////result.SourceRelatedData = relationData;
        //    ///////*   result.DataInstance = new EntityInstance();// Clone<TableDrivedEntity*/DTO>(areaInitializer.Template);
        //    //////result.DataInstance.ID = areaInitializer.SearchEntity.ID;
        //    //////result.Properties = new List<EntityInstanceProperty>();
        //    //////foreach (var column in areaInitializer.SearchEntity.Columns)
        //    //////{
        //    //////    string value = "";
        //    //////    if (!string.IsNullOrEmpty(column.DefaultValue))
        //    //////        value = column.DefaultValue;
        //    //////    else
        //    //////        value = (column.IsNull == true ? "<Null>" : "");
        //    //////    result.Properties.Add(new EntityInstanceProperty() { ColumnID = column.ID, Value = value });
        //    //////}
        //    ////return result;
        //    return null;
        //}
        //internal static DP_DataRepository CreateAreaInitializerNewData(ViewEntityAreaInitializer AreaInitializer)
        //{
        //    {
        //        var result = new DP_DataRepository();


        //        //result.RelationID = relationID;
        //        //result.SourceRelatedData = relationData;
        //        result.DataInstance = new EntityInstance();// Clone<TableDrivedEntityDTO>(AreaInitializer.Template);
        //        result.DataInstance.ID = AreaInitializer.ViewEntity.ID;
        //        result.Properties = new List<EntityInstanceProperty>();
        //        foreach (var column in AreaInitializer.ViewEntity.Columns)
        //        {
        //            string value = "";
        //            if (!string.IsNullOrEmpty(column.DefaultValue))
        //                value = column.DefaultValue;
        //            else
        //                value = (column.IsNull == true ? "<Null>" : "");
        //            result.Properties.Add(new EntityInstanceProperty() { ColumnID = column.ID, Value = value });
        //        }
        //        return result;
        //    }
        //}
        internal static List<T> CreateListFromSingleObject<T>(T item)
        {
            List<T> list = new List<T>();
            list.Add(item);
            return list;
        }

        //internal static bool TypePropertyIsKeyOf(ColumnDTO correspondingTypeProperty, TableDrivedEntityDTO nD_TypeOrTypeCondition)
        //{
        //    //////foreach (var typeProperty in nD_TypeOrTypeCondition.Type.Properties)
        //    //////{
        //    //////    if (typeProperty.IsKey)
        //    //////        if (typeProperty.ID == correspondingTypeProperty.ID)
        //    //////            return true;
        //    //////}
        //    //////return false;
        //    return correspondingTypeProperty.PrimaryKey == true;
        //}


        //internal static List<T> GenerateCommands<T>(IntracionMode intracionMode, DataMode dataMode)
        //{
        //    List<T> result = new List<T>();
        //    var currentAssembly = typeof(AgentHelper).Assembly;

        //    // we filter the defined classes according to the interfaces they implement
        //    var commands = currentAssembly.DefinedTypes.Where(type => type.ImplementedInterfaces.Any(inter => inter == typeof(T))).ToList();
        //    foreach (var scommand in commands)
        //    {

        //        //  Type t = Type.GetType(type);
        //        object item = currentAssembly.CreateInstance(scommand.FullName);
        //        //result = obj as IDataProviderUISettings;

        //        if (item is I_Command)
        //        {
        //            var command = item as I_Command;
        //            if (command.IsGeneralCommand)
        //            {
        //                if (item is I_EntityAreaCommand)
        //                {
        //                    List<IntracionMode> intracionModes = null;
        //                    try
        //                    {
        //                        intracionModes = command.CompatibaleIntractionMode;
        //                    }
        //                    catch
        //                    {

        //                    }
        //                    if (intracionMode != null && intracionModes != null && intracionModes.Count > 0)
        //                    {
        //                        if (!intracionModes.Contains(intracionMode))
        //                        {
        //                            continue;
        //                        }
        //                    }

        //                    List<DataMode> dataModes = null;
        //                    try
        //                    {
        //                        dataModes = command.CompatibaleDataMode;
        //                    }
        //                    catch
        //                    {

        //                    }
        //                    if (dataMode != null && dataModes != null && dataModes.Count > 0)
        //                    {
        //                        if (!dataModes.Contains(dataMode))
        //                        {
        //                            continue;
        //                        }
        //                    }
        //                }
        //                result.Add((T)command);
        //            }
        //            var title = UISettingHelper.GetCommandTitle(scommand.FullName);
        //            if (!string.IsNullOrEmpty(title))
        //                (item as I_Command).Title = title;
        //        }

        //    }
        //    return result;
        //}
        //internal static List<I_Command> ConvertToICommand(IList list)
        //{
        //    List<I_Command> result = new List<I_Command>();
        //    foreach (var item in list)
        //        result.Add(item as I_Command);
        //    return result;
        //}



        //public static string GetEntityTitle(TableDrivedEntityDTO entity)
        //{
        //    if (string.IsNullOrEmpty(entity.Alias))
        //        return entity.Name;
        //    else
        //        return entity.Alias;
        //}


        //public static DataMode GetDataEntryMode(TableDrivedEntityDTO template)
        //{
        //    if (template.BatchDataEntry == null ||
        //       template.BatchDataEntry == false)
        //        return DataMode.One;
        //    else
        //        return DataMode.Multiple;
        //}

        //internal static List<EntityInstanceProperty> GetRelationProperties(RelationshipDTO relationship, DP_DataRepository data)
        //{
        //    List<EntityInstanceProperty> result = new List<EntityInstanceProperty>();

        //    foreach (var dataColumn in data.Properties)
        //    {

        //        foreach (var relationColumn in relationship.RelationshipColumns)
        //        {

        //            if (dataColumn.ColumnID == relationColumn.FirstSideColumnID)
        //            {
        //                int otherColumnID = relationColumn.SecondSideColumnID;
        //                EntityInstanceProperty item = new EntityInstanceProperty();
        //                item.ColumnID = otherColumnID;
        //                item.Value = dataColumn.Value;
        //                result.Add(item);
        //            }

        //        }
        //    }

        //    return result;
        //}

        //internal static List<EntityInstanceProperty> GetKeyProperties(I_EditEntityArea editEntityArea, DP_DataRepository data)
        //{
        //    return data.KeyProperties;
        //    //List<EntityInstanceProperty> result = new List<EntityInstanceProperty>();
        //    ////foreach (var data in relationData)
        //    ////{
        //    //foreach (var dataColumn in data.Properties)
        //    //{
        //    //    foreach (var column in editEntityArea.EntityWithSimpleColumns.Columns.Where(x => x.PrimaryKey == true))
        //    //    {
        //    //        if (dataColumn.ColumnID == column.ID)
        //    //        {
        //    //            EntityInstanceProperty item = new EntityInstanceProperty();
        //    //            item.ColumnID = column.ID;
        //    //            item.Value = dataColumn.Value;
        //    //            result.Add(item);
        //    //        }
        //    //    }
        //    //}
        //    //}
        //    //return result;
        //}



        internal static bool DataOrRelatedChildDataHasValue(DP_DataRepository dataItem, RelationshipDTO relationship)
        {
            List<int> excludeColumns = new List<int>();
            if (relationship != null)
                if (relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromPrimartyToForeign)
                    excludeColumns = relationship.RelationshipColumns.Select(x => x.SecondSideColumnID).ToList();
            foreach (var item in dataItem.GetProperties())
                if (!excludeColumns.Contains(item.ColumnID) && !ValueIsEmpty(item))
                    return true;


            foreach (var childRel in dataItem.ChildRelationshipInfos)
                foreach (var data in childRel.RelatedData)
                {
                    if (DataOrRelatedChildDataHasValue(data, childRel.Relationship))
                        return true;
                }

            return false;
        }

        internal static bool DataHasValue(DP_DataRepository dataItem)
        {
            foreach (var item in dataItem.GetProperties())
                if (!ValueIsEmpty(item))
                    return true;

            return false;
        }
        //internal static bool DataShouldBeCounted(DP_DataRepository data)
        //{

        //    if (AgentHelper.DataHasValue(data))
        //        return true;

        //    if (data.ChildRelationshipInfos.Any(x => x.Relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromForeignToPrimary
        //      && x.RelatedData.Any(y => AgentHelper.DataShouldBeCounted(y))))
        //        return true;

        //    //زیرا پرنت قبلا بررسی شده
        //    if (data.ParantChildRelationshipInfo != null && data.ParantChildRelationshipInfo.Relationship.MastertTypeEnum == Enum_MasterRelationshipType.FromPrimartyToForeign
        //        && data.ParantChildRelationshipInfo.ParentData.HasDirectData)
        //    {
        //        //برای ارث بریهای داخل یک جدول
        //        if (data.ParantChildRelationshipInfo.Relationship.RelationshipColumns.Any(x => x.FirstSideColumnID != x.SecondSideColumnID))
        //            return true;
        //    }
        //    if (data.RemovedItems.Any())
        //        return true;
        //    return false;
        //}
        internal static bool ValueIsEmpty(EntityInstanceProperty property)
        {
            if (string.IsNullOrEmpty(property.Value) || property.Value == "0")
                return true;
            else
                return false;
        }
        internal static bool ValueIsEmptyOrDefaultValue(EntityInstanceProperty property)
        {
            return ValueIsEmpty(property) || property.Value == property.Column.DefaultValue;
        }

        //internal static string GetTypePropertyValue(DP_DataRepository dataRepository, ColumnDTO typeProperty)
        //{
        //    foreach (var prop in dataRepository.Properties)
        //    {
        //        if (prop.ColumnID == typeProperty.ID)
        //            return prop.Value;
        //    }
        //    throw new Exception("asdsadaa");
        //}

        internal static string GetTypePropertyValue(DP_SearchRepository dataRepository, ColumnDTO typeProperty)
        {
            foreach (SearchProperty prop in dataRepository.Phrases.Where(x => x is SearchProperty))
            {
                if (prop.ColumnID == typeProperty.ID)
                    return prop.Value;
            }
            throw new Exception("asdsadaa");
        }
        internal static void ShowEditEntityAreaInfo(I_EditEntityArea editArea)
        {
            EditEntityAreaInfo info = new EditEntityAreaInfo();
            //info.DataCount = editArea.AreaInitializer.Data.Count;
            var columns = editArea.EntityWithSimpleColumns.Columns;
            List<ColumnDTO> columnsSource = null;
            if (editArea.AreaInitializer.SourceRelation != null)
                columnsSource = editArea.AreaInitializer.SourceRelation.SourceEditArea.EntityWithSimpleColumns.Columns;
            foreach (var item in editArea.GetDataList().Take(100))
            {
                EditEntityAreaDataInfo rItem = new EditEntityAreaDataInfo();
                rItem.ColumnWithValues = "";

                rItem.IsNew = item.IsNewItem;
                rItem.ExcludeFromDataEntry = item.ExcludeFromDataEntry;
                rItem.HasData = AgentHelper.DataHasValue(item);
                foreach (var property in item.GetProperties())
                {
                    var column = columns.FirstOrDefault(x => x.ID == property.ColumnID);
                    rItem.ColumnWithValues += (rItem.ColumnWithValues == "" ? "" : " , ") + (column != null ? column.Name : property.ColumnID.ToString()) + "=" + property.Value;
                }

                info.DataInfo.Add(rItem);
                if (editArea.AreaInitializer.SourceRelation != null)
                {
                    //var parentDataItem = editArea.AreaInitializer.SourceRelation.SourceEditArea.AreaInitializer.Datas.First(x => item.ParentRelationInfos.Any(y => y.SourceRelatedData == x));

                    //EditEntityAreaDataInfo ssItem = new EditEntityAreaDataInfo();
                    //ssItem.ColumnWithValues = "";
                    //ssItem.IsNew = parentDataItem.IsNewItem;
                    //ssItem.ExcludeFromDataEntry = parentDataItem.ExcludeFromDataEntry;
                    //ssItem.HasData = AgentHelper.DataHasValue(parentDataItem);
                    //foreach (var col in parentDataItem.Properties)
                    //{
                    //    var column = columnsSource.FirstOrDefault(x => x.ID == col.ColumnID);
                    //    ssItem.ColumnWithValues += (ssItem.ColumnWithValues == "" ? "" : " , ") + (column != null ? column.Name : col.ColumnID.ToString()) + "=" + col.Value;

                    //}
                    //rItem.RelatedDataInfo.Add(ssItem);


                }
            }
            //foreach (var item in editArea.AreaInitializer.RemovedData.Take(100))
            //{
            //    EditEntityAreaDataInfo rItem = new EditEntityAreaDataInfo();
            //    rItem.ColumnWithValues = "";

            //    rItem.IsNew = item.IsNewItem;
            //    rItem.ExcludeFromDataEntry = item.ExcludeFromDataEntry;
            //    rItem.HasData = AgentHelper.DataHasValue(item);
            //    foreach (var col in item.Properties)
            //    {
            //        var column = columns.FirstOrDefault(x => x.ID == col.ColumnID);
            //        rItem.ColumnWithValues += (rItem.ColumnWithValues == "" ? "" : " , ") + (column != null ? column.Name : col.ColumnID.ToString()) + "=" + col.Value;
            //    }

            //    info.RemovedDataInfo.Add(rItem);
            //    if (item.SourceRelatedData != null)
            //    {
            //        //foreach (var sitem in item.SourceRelatedData.Take(100))
            //        //{
            //        EditEntityAreaDataInfo ssItem = new EditEntityAreaDataInfo();
            //        ssItem.ColumnWithValues = "";
            //        ssItem.IsNew = item.SourceRelatedData.IsNewItem;
            //        ssItem.ExcludeFromDataEntry = item.SourceRelatedData.ExcludeFromDataEntry;
            //        ssItem.HasData = AgentHelper.DataHasValue(item.SourceRelatedData);
            //        foreach (var col in item.SourceRelatedData.Properties)
            //        {
            //            var column = columnsSource.FirstOrDefault(x => x.ID == col.ColumnID);
            //            ssItem.ColumnWithValues += (ssItem.ColumnWithValues == "" ? "" : " , ") + (column != null ? column.Name : col.ColumnID.ToString()) + "=" + col.Value;

            //        }
            //        rItem.RelatedDataInfo.Add(ssItem);
            //        //}
            //    }
            //}
            if (editArea.SkippedRelationshipColumnControl != null)
                info.SkippedRelationships = editArea.SkippedRelationshipColumnControl.Select(x => x.Relationship).ToList();
            info.FormComposed = editArea.AreaInitializer.FormComposed;
            info.TemplateEntityName = editArea.SimpleEntity.Name;
            info.DataMode = editArea.AreaInitializer.DataMode.ToString();
            //info.DirectionMode = editArea.AreaInitializer.DirectionMode.ToString();
            info.IntracionMode = editArea.AreaInitializer.IntracionMode.ToString();
            if (editArea.AreaInitializer.SourceRelation != null)
            {
                info.SourceRalationType = editArea.AreaInitializer.SourceRelation.RelationshipType.ToString();
                info.SourceEntityName = editArea.AreaInitializer.SourceRelation.SourceEditArea.SimpleEntity.Name;
                info.SourceRalationName = editArea.AreaInitializer.SourceRelation.Relationship.Name;
                info.relationIsMandatory = editArea.AreaInitializer.SourceRelation.TargetSideIsMandatory;
            }
            var view = AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GenerateViewOfEditEntityAreaInfo();
            view.ShowInfo(info);
            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.GetDialogWindow().ShowDialog(view, "Info : ");

        }

        //internal static List<Tuple<I_EditEntityArea, SimpleColumnControl>> GetFormulaColumnProperties(DP_DataRepository data, List<FormulaItemDTO> formulaItems, List<EntityInstanceProperty> properties = null)
        //{
        //if (properties == null)
        //    properties = new List<EntityInstanceProperty>();
        //foreach (var item in formulaItems)
        //{
        //    if (item.ItemType == FormuaItemType.Column)
        //    {
        //        var columnControl = editEntityArea.SimpleColumnControls.FirstOrDefault(x => x.Column.ID == item.ItemID);
        //        if (columnControl != null)
        //            listColumns.Add(new Tuple<I_EditEntityArea, SimpleColumnControl>(editEntityArea, columnControl));
        //    }
        //    foreach (var child in item.ChildFormulaItems)
        //    {
        //        if (child.ItemType == FormuaItemType.Relationship)
        //        {
        //            var relatedEntityArea = editEntityArea.RelationshipColumnControls.FirstOrDefault(x => x.Relationship != null && x.Relationship.ID == child.ItemID);
        //            if (relatedEntityArea != null)
        //                GetFormulaColumnsList(relatedEntityArea.EditNdTypeArea, child.ChildFormulaItems, listColumns);
        //        }
        //    }
        //}
        //return listColumns;
        //}

        //internal static void ApplyRelationshipEnablity(I_EditEntityArea editEntityArea, RelationshipEnablityDTO relationshipEnablity)
        //{
        //    var result = GetEditEntityAreaByRelationshipTail(editEntityArea, relationshipEnablity.EntityRelationshipTail);
        //    if (result.EditEntityAreaFound)
        //    {
        //        ////result.FoundEditEntityArea.AreaInitializer.FormAttributes.BusinessFormDisabled =;
        //        ////اگر دیتا آیتم مهم بود چی؟ مالتی پل
        //        /////////////// موقتا result.FoundEditEntityArea.CheckFormDisablity(SecurityBusinessType.Business, !relationshipEnablity.Enable, null);
        //        ////result.FoundEditEntityArea.DisableEnable(relationshipEnablity.Enable);
        //    }
        //}

        //private static EditEntityAreaByRelationshipTail GetEditEntityAreaByRelationshipTail(I_EditEntityArea editEntityArea, EntityRelationshipTailDTO entityRelationshipTail)
        //{
        //    RelationshipColumnControl relatedEntityArea = null;
        //    if (editEntityArea is I_EditEntityAreaOneData)
        //        relatedEntityArea = (editEntityArea as I_EditEntityAreaOneData).RelationshipColumnControls.FirstOrDefault(x => x.Relationship.ID == entityRelationshipTail.Relationship.ID);
        //    else if (editEntityArea is I_EditEntityAreaMultipleData)
        //        relatedEntityArea = (editEntityArea as I_EditEntityAreaMultipleData).RelationshipColumnControls.FirstOrDefault(x => x.Relationship.ID == entityRelationshipTail.Relationship.ID);

        //    if (relatedEntityArea != null)
        //    {
        //        if (entityRelationshipTail.ChildTail == null)
        //        {
        //            EditEntityAreaByRelationshipTail result = new MyUILibrary.EditEntityAreaByRelationshipTail();
        //            result.EditEntityAreaFound = true;
        //            result.FoundEditEntityArea = relatedEntityArea.EditNdTypeArea;
        //            return result;
        //        }
        //        else
        //        {
        //            return GetEditEntityAreaByRelationshipTail(relatedEntityArea.EditNdTypeArea, entityRelationshipTail.ChildTail);
        //        }
        //    }
        //    else
        //    {
        //        EditEntityAreaByRelationshipTail result = new MyUILibrary.EditEntityAreaByRelationshipTail();
        //        result.EditEntityAreaFound = false;
        //        result.LastFoundEntityArea = new Tuple<I_EditEntityArea, EntityRelationshipTailDTO>(editEntityArea, entityRelationshipTail);
        //        return result;
        //    }

        //}

        //internal static void ApplyUIEnablityMultipleData(I_EditEntityAreaMultipleData editEntityArea, UIEnablityDTO uIEnablity)
        //{
        //    //////I_EditEntityArea targetEntityArea = null;
        //    //////if (uIEnablity.EntityRelationshipTail != null)
        //    //////{
        //    //////    var result = GetEditEntityAreaByRelationshipTail(editEntityArea, uIEnablity.EntityRelationshipTail);
        //    //////    if (result.EditEntityAreaFound)
        //    //////        targetEntityArea = result.FoundEditEntityArea;
        //    //////}
        //    //////else
        //    //////    targetEntityArea = editEntityArea;
        //    //////if (targetEntityArea != null)
        //    //////{//ریلیشن جداگانه ایجاد شود
        //    //////    if (uIEnablity.ColumnID != 0)
        //    //////    {
        //    //////        var columnControl = targetEntityArea.SimpleColumnControls.FirstOrDefault(x => x.Column.ID == uIEnablity.ColumnID);
        //    //////        if (columnControl != null)
        //    //////            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.EnableDisableColumnControl(columnControl.ControlPackage, columnControl.Column, uIEnablity.Enable);
        //    //////    }
        //    //////    else if (uIEnablity.UICompositionID != 0)
        //    //////    {
        //    //////        var uiComposition = targetEntityArea.UICompositionContainers.FirstOrDefault(x => x.Item1.ID == uIEnablity.UICompositionID);
        //    //////        if (uiComposition != null)
        //    //////            uiComposition.Item2.DisableEnable(uIEnablity.Enable);
        //    //////    }
        //    //////}
        //}
        //internal static void ApplyUIEnablityOneData(I_EditEntityAreaOneData editEntityArea, UIEnablityDTO uIEnablity)
        //{
        //    //////I_EditEntityArea targetEntityArea = null;
        //    //////if (uIEnablity.EntityRelationshipTail != null)
        //    //////{
        //    //////    var result = GetEditEntityAreaByRelationshipTail(editEntityArea, uIEnablity.EntityRelationshipTail);
        //    //////    if (result.EditEntityAreaFound)
        //    //////        targetEntityArea = result.FoundEditEntityArea;
        //    //////}
        //    //////else
        //    //////    targetEntityArea = editEntityArea;
        //    //////if (targetEntityArea != null)
        //    //////{//ریلیشن جداگانه ایجاد شود
        //    //////    if (uIEnablity.ColumnID != 0)
        //    //////    {
        //    //////        var columnControl = targetEntityArea.SimpleColumnControls.FirstOrDefault(x => x.Column.ID == uIEnablity.ColumnID);
        //    //////        if (columnControl != null)
        //    //////            AgentUICoreMediator.GetAgentUICoreMediator.UIManager.EnableDisableColumnControl(columnControl.ControlPackage, columnControl.Column, uIEnablity.Enable);
        //    //////    }
        //    //////    else if (uIEnablity.UICompositionID != 0)
        //    //////    {
        //    //////        var uiComposition = targetEntityArea.UICompositionContainers.FirstOrDefault(x => x.Item1.ID == uIEnablity.UICompositionID);
        //    //////        if (uiComposition != null)
        //    //////            uiComposition.Item2.DisableEnable(uIEnablity.Enable);
        //    //////    }
        //    //////}
        //}


        //internal static void ApplyColumnValueMultipleData(I_EditEntityArea editEntityArea, UIColumnValueDTO columnValue)
        //{
        //    //I_EditEntityArea targetEntityArea = null;
        //    //if (columnValue.EntityRelationshipTail != null)
        //    //{
        //    //    var result = GetEditEntityAreaByRelationshipTail(editEntityArea, columnValue.EntityRelationshipTail);
        //    //    if (result.EditEntityAreaFound)
        //    //        targetEntityArea = result.FoundEditEntityArea;
        //    //}
        //    //else
        //    //    targetEntityArea = editEntityArea;
        //    //if (targetEntityArea != null)
        //    //{
        //    //    var columnControl = targetEntityArea.SimpleColumnControls.FirstOrDefault(x => x.Column.ID == columnValue.ColumnID);
        //    //    if (columnControl != null)
        //    //    {
        //    //        if (columnValue.ValidValues != null)
        //    //        {//برای این فکری شود
        //    //         //ظاهرا فقط وقتی column.ColumnKeyValue
        //    //         //داشته باشد کاربرد دارد

        //    //        }
        //    //        if (columnValue.ExactValue != null)
        //    //        {
        //    //            //ColumnSetting columnSetting = new ColumnSetting();
        //    //            columnControl.ControlPackage.SetValue( columnValue.ExactValue);
        //    //        }
        //    //    }
        //    //}
        //}

        //internal static void ApplyColumnValueOneData(I_EditEntityArea editEntityArea, UIColumnValueDTO columnValue)
        //{
        //    //I_EditEntityArea targetEntityArea = null;
        //    //if (columnValue.EntityRelationshipTail != null)
        //    //{
        //    //    var result = GetEditEntityAreaByRelationshipTail(editEntityArea, columnValue.EntityRelationshipTail);
        //    //    if (result.EditEntityAreaFound)
        //    //        targetEntityArea = result.FoundEditEntityArea;
        //    //}
        //    //else
        //    //    targetEntityArea = editEntityArea;
        //    //if (targetEntityArea != null)
        //    //{
        //    //    var columnControl = targetEntityArea.SimpleColumnControls.FirstOrDefault(x => x.Column.ID == columnValue.ColumnID);
        //    //    if (columnControl != null)
        //    //    {
        //    //        if (columnValue.ValidValues != null)
        //    //        {//برای این فکری شود
        //    //         //ظاهرا فقط وقتی column.ColumnKeyValue
        //    //         //داشته باشد کاربرد دارد

        //    //        }
        //    //        if (columnValue.ExactValue != null)
        //    //        {
        //    //            //ColumnSetting columnSetting = new ColumnSetting();
        //    //            columnControl.ControlPackage.SetValue(columnValue.ExactValue);
        //    //        }
        //    //    }
        //    //}
        //}


        //internal static string GetValueSomeHow(I_EditEntityArea editEntityArea, EntityRelationshipTailDTO valueRelationshipTail, int valueColumnID)
        //{


        //    if (editEntityArea is I_EditEntityAreaMultipleData)
        //    {
        //        throw new Exception("asav");
        //    }
        //    else if (editEntityArea is I_EditEntityAreaOneData)
        //    {
        //        DP_DataRepository sentdata = (editEntityArea as I_EditEntityAreaOneData).GetDataList().FirstOrDefault();
        //        if (sentdata == null)
        //            throw new Exception("asav");
        //        else
        //            return GetValueSomeHow(sentdata, valueRelationshipTail, valueColumnID);
        //    }
        //    else
        //    {
        //        throw new Exception("asav");
        //    }





        //}


        public static List<DP_DataView> GetRelatedDataItemsSomeHow(DP_DataRepository sentdata, EntityRelationshipTailDTO valueRelationshipTail)
        {
            return GetRelatedDataItemsSomeHow(valueRelationshipTail, new List<DP_DataRepository>() { sentdata });
        }
        private static List<DP_DataView> GetRelatedDataItemsSomeHow(EntityRelationshipTailDTO valueRelationshipTail, List<DP_DataRepository> lastFoundItems, List<DP_DataView> result = null)
        {
            if (result == null)
                result = new List<DP_DataView>();
            //بررسی شود که آیا واقعا این فانکشن نیاز است؟

            if (valueRelationshipTail != null)
            {
                foreach (var sentdata in lastFoundItems)
                {
                    if ((sentdata.ParantChildRelationshipInfo != null && sentdata.ParantChildRelationshipInfo.Relationship.PairRelationshipID == valueRelationshipTail.Relationship.ID)
                        || (sentdata.ChildRelationshipInfos.Any(x => x.Relationship.ID == valueRelationshipTail.Relationship.ID)))
                    {
                        List<DP_DataRepository> relatedData = new List<DP_DataRepository>();

                        if (sentdata.ParantChildRelationshipInfo != null && sentdata.ParantChildRelationshipInfo.Relationship.PairRelationshipID == valueRelationshipTail.Relationship.ID)
                        {
                            relatedData.Add(sentdata.ParantChildRelationshipInfo.SourceData);
                        }
                        else if (sentdata.ChildRelationshipInfos.Any(x => x.Relationship.ID == valueRelationshipTail.Relationship.ID))
                        {
                            var childInfo = sentdata.ChildRelationshipInfos.First(x => x.Relationship.ID == valueRelationshipTail.Relationship.ID);
                            relatedData.AddRange(childInfo.RelatedData);
                        }
                        if (valueRelationshipTail.ChildTail == null)
                            result.AddRange(relatedData.Select(x => x.DataView));
                        else
                            GetRelatedDataItemsSomeHow(valueRelationshipTail.ChildTail, relatedData, result);
                    }
                    else
                    {
                        var requester = AgentUICoreMediator.GetAgentUICoreMediator.GetRequester();
                        var searchDataTuple = AgentUICoreMediator.GetAgentUICoreMediator.RelationshipTailDataManager.GetTargetSearchItemFromRelationshipTail(sentdata, valueRelationshipTail);
                        DR_SearchKeysOnlyRequest request = new DR_SearchKeysOnlyRequest(requester, searchDataTuple);
                        var searchResult = AgentUICoreMediator.GetAgentUICoreMediator.requestRegistration.SendSearchKeysOnlyRequest(request);
                        result.AddRange(searchResult.ResultDataItems);
                    }
                }
            }
            return result;

        }


        //private static EditEntityAreaWithDataByRelationshipTail GetFirstEntityAreaWithExistingDataByRelationshipTail(I_EditEntityArea editEntityArea, EntityRelationshipTailDTO entityRelationshipTail, DP_DataRepository relatedData)
        //{

        //    //if (editEntityArea.AreaInitializer.Data.Any(x => x.IsNewItem == false))
        //    if (relatedData != null && relatedData.IsNewItem == false)
        //    {
        //        EditEntityAreaWithDataByRelationshipTail result = new MyUILibrary.EditEntityAreaWithDataByRelationshipTail();
        //        result.EditEntityAreaFound = true;
        //        result.RelatedData = relatedData;
        //        result.FoundEditEntityArea = editEntityArea;
        //        result.RessOfRelationshipTail = entityRelationshipTail;
        //        return result;
        //    }
        //    else
        //    {
        //        if (entityRelationshipTail.ChildTail == null)
        //        {
        //            EditEntityAreaWithDataByRelationshipTail result = new MyUILibrary.EditEntityAreaWithDataByRelationshipTail();
        //            result.EditEntityAreaFound = false;
        //            return result;
        //        }
        //        else
        //        {
        //            if (editEntityArea.AreaInitializer.SourceRelation != null
        //                && editEntityArea.AreaInitializer.SourceRelation.Relationship.ID == entityRelationshipTail.RelationshipID)
        //            {
        //                return GetFirstEntityAreaWithExistingDataByRelationshipTail(editEntityArea.AreaInitializer.SourceRelation.SourceEditArea as I_EditEntityArea, entityRelationshipTail.ChildTail, editEntityArea.ChildRelationshipInfo.ParentData);
        //            }
        //            else
        //            {
        //                //////var relatedColumn = editEntityArea.RelationshipColumnControls.FirstOrDefault(x => x.Relationship.ID == entityRelationshipTail.RelationshipID);
        //                //////if (relatedColumn != null)
        //                //////{
        //                //////    //نباید بررسی بشه که آیا چند دیتا دارد و آن دیتایی که پرنتش این دیتاست انتخاب شود
        //                //////    //زیرا اساسا رابطه یک به چند نباشد در مسیر باشد.در تعریف هم این فیلتر ایجاد شود که رابطه یک به چند نباشد
        //                //////    DP_DataRepository columnControlData = relatedColumn.EditNdTypeArea.AreaInitializer.Data.FirstOrDefault();

        //                //////    return GetFirstEntityAreaWithExistingDataByRelationshipTail(relatedColumn.EditNdTypeArea, entityRelationshipTail.ChildTail, columnControlData);
        //                //////}

        //                EditEntityAreaWithDataByRelationshipTail result = new MyUILibrary.EditEntityAreaWithDataByRelationshipTail();
        //                result.EditEntityAreaFound = false;
        //                return result;
        //            }
        //        }
        //    }

        //}

        internal static DP_SearchRepository GetOrCreateSearchRepositoryFromRelationshipTail(LogicPhrase result, EntityRelationshipTailDTO relationshipTail, DP_BaseData lastDataItem)
        {
            if (relationshipTail == null)
            {

                var searchRep = result as DP_SearchRepository;
                if (lastDataItem != null)
                {
                    foreach (var property in lastDataItem.KeyProperties)
                    {

                        SearchProperty searchProperty = new SearchProperty();
                        //searchProperty.SearchColumnID = relControl.EntitySearchColumn != null ? relControl.EntitySearchColumn.ID : 0;
                        searchProperty.ColumnID = property.Column.ID;
                        searchProperty.IsKey = property.Column.PrimaryKey;
                        searchProperty.Value = property.Value;
                        searchProperty.Operator = CommonOperator.Equals;
                        searchRep.Phrases.Add(searchProperty);
                    }
                }
                return searchRep;
            }
            else
            {
                if (result.Phrases.Any(x => x is DP_SearchRepository && (x as DP_SearchRepository).SourceRelationship.ID == relationshipTail.Relationship.ID))
                {
                    var childSearchPhrase = result.Phrases.First(x => x is DP_SearchRepository && (x as DP_SearchRepository).SourceRelationship.ID == relationshipTail.Relationship.ID);
                    return GetOrCreateSearchRepositoryFromRelationshipTail(childSearchPhrase as LogicPhrase, relationshipTail.ChildTail, lastDataItem);
                }
                else
                {
                    DP_SearchRepository childSearchPhrase = new DP_SearchRepository(relationshipTail.Relationship.EntityID2);
                    childSearchPhrase.SourceRelationship = relationshipTail.Relationship;
                    childSearchPhrase.Title = relationshipTail.EntityPath;
                    //childSearchPhrase.TargetEntityID = ;
                    //   childSearchPhrase.SourceToTargetRelationshipType = relationshipTail.Relationship.TypeEnum;
                    //  childSearchPhrase.SourceToTargetMasterRelationshipType = relationshipTail.Relationship.MastertTypeEnum;
                    result.Phrases.Add(childSearchPhrase);
                    return GetOrCreateSearchRepositoryFromRelationshipTail(childSearchPhrase, relationshipTail.ChildTail, lastDataItem);
                }
            }
        }

        //private static List<EntityInstanceProperty> GetRelationshipColumnValues(DP_DataRepository foundData, RelationshipDTO relationship)
        //{
        //    List<EntityInstanceProperty> result = new List<EntityInstanceProperty>();
        //    if (relationship != null)
        //    {
        //        foreach (var column in relationship.RelationshipColumns)
        //        {
        //            var property = foundData.GetProperty(column.FirstSideColumnID.Value);
        //            if (property == null)
        //                return null;
        //            result.Add(property);
        //        }
        //    }
        //    return result;
        //}
        //private static List<EntityInstanceProperty> GetRelationshipColumnValues(DP_DataRepository foundData)
        //{
        //    List<EntityInstanceProperty> result = new List<EntityInstanceProperty>();
        //    if (relationship != null)
        //    {
        //        foreach (var column in relationship.RelationshipColumns)
        //        {
        //            var property = foundData.GetProperty(column.FirstSideColumnID.Value);
        //            if (property == null)
        //                return null;
        //            result.Add(property);
        //        }
        //    }
        //    return result;
        //}
        //private static EntityInstanceProperty GetColumnValue(DP_DataRepository foundData, I_EditEntityArea lastFoundEntityArea, int columnID)
        //{



        //    //if (lastFoundEntityArea.AreaInitializer.TemplateEntity.Columns.Count > 0)
        //    //    relationship = lastFoundEntityArea.AreaInitializer.TemplateEntity.Relationships.FirstOrDefault(x => x.ID == relationshipID);
        //    //else
        //    //    relationship = AgentUICoreMediator.GetAgentUICoreMediator.GetRelationship(relationshipID);

        //    //////if (foundData != null)
        //    //////{

        //    //////    var prop = foundData.Properties.FirstOrDefault(x => x.ColumnID == columnID);
        //    //////    if (prop != null)
        //    //////    {
        //    //////        var value = prop.Value;
        //    //////        return (new EntityInstanceProperty() { ColumnID = columnID, Name = prop.Name, IsKey = prop.IsKey, Value = value });
        //    //////    }

        //    //////}
        //    //////else
        //    //////{
        //    //////    //اگر ریلیشن بود چی؟ این فانکشن از بیخ بررسی شود
        //    //////    var columnControl = lastFoundEntityArea.SimpleColumnControls.FirstOrDefault(x => x.Column.ID == columnID);
        //    //////    if (columnControl != null)
        //    //////    {
        //    //////        var value = "";
        //    //////        //////if (lastFoundEntityArea.AreaInitializer.DataMode == DataMode.Multiple)
        //    //////        //////    value = (lastFoundEntityArea.DataView as I_View_EditEntityAreaMultiple).FetchMultipleDateItemControlValue(foundData, columnControl.ControlPackage as DataDependentControlPackage);
        //    //////        //////else
        //    //////        //////    value = columnControl.ControlPackage.BaseControlHelper.GetValue(columnControl.ControlPackage);

        //    //////        //AgentUICoreMediator.GetAgentUICoreMediator.UIManager.FetchControlValue(columnControl.ControlPackage, columnControl.Column);
        //    //////        return (new EntityInstanceProperty() { ColumnID = columnID, Value = value });
        //    //////    }

        //    //////}

        //    return null;
        //}
        public static AppMode GetAppMode()
        {
            string param = System.Configuration.ConfigurationManager.AppSettings["AppMode"];
            if (param == null)
                return AppMode.None;
            //else if (param.ToLower() == "paper")
            //    return AppMode.Paper;
            else
                return AppMode.None;

        }
    }
    public enum AppMode
    {
        None
    }

    class EditEntityAreaByRelationshipTail
    {
        public bool EditEntityAreaFound { set; get; }
        public I_EditEntityArea FoundEditEntityArea { set; get; }

        public Tuple<I_EditEntityArea, EntityRelationshipTailDTO> LastFoundEntityArea { set; get; }
    }

    class EditEntityAreaWithDataByRelationshipTail
    {
        public DP_DataRepository RelatedData { set; get; }
        public bool EditEntityAreaFound { set; get; }
        public I_EditEntityArea FoundEditEntityArea { set; get; }
        public EntityRelationshipTailDTO RessOfRelationshipTail { set; get; }
    }
}
