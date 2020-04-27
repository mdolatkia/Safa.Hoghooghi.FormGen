using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyLibrary
{
    public class DataLogDTO
    {
        public DataLogDTO()
        {
            EditDataItemColumnDetails = new List<ProxyLibrary.EditDataItemColumnDetailsDTO>();
        }
        public int ID { get; set; }
        public int DatItemID
        {
            get
            {
                return DatItem != null ? DatItem.DataItemID : 0;
            }
        }
        public DP_BaseData DatItem { set; get; }
        public string vwEntityAlias { set; get; }
        //public string DataInfo { set; get; }
        public string vwPersianDate { get; set; }
        public int UserID { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public DataLogType MainType { get; set; }
        public bool MajorException { get; set; }
        public bool MinorException { get; set; }
        public int Duration { get; set; }
        public string LocationInfo { get; set; }
        public Guid? PackageGuid { get; set; }
        public EditDataItemExceptionLogDTO EditDataItemExceptionLog { get; set; }
        public List<EditDataItemColumnDetailsDTO> EditDataItemColumnDetails { set; get; }
        public LetterItemLogDTO LetterItemLog { set; get; }
        //public ArchiveItemLogDTO ArchiveItemLog { set; get; }
        public string vwUserInfo { get; set; }
        public string MajorFunctionExceptionMessage { set; get; }
        public int RelatedItemID { set; get; }
    }
    public class EditDataItemExceptionLogDTO
    {
        public int ID { get; set; }
        //public string DataUpdateExceptionMessage { get; set; }
        //public string BeforeSaveActionExceptionMessage { get; set; }
        public string AfterSaveActionExceptionMessage { get; set; }
        public string DataUpdateQuery { get; set; }
    }
    public class EditDataItemColumnDetailsDTO
    {
        public EditDataItemColumnDetailsDTO()
        {
            FormulaUsageParemeters = new List<ProxyLibrary.FormulaUsageParemetersDTO>();
        }
        public int ID { get; set; }
        public int ColumnID { get; set; }
        public string vwColumnName { get; set; }
        public string vwColumnAlias { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
        public string Info { get; set; }
        public List<FormulaUsageParemetersDTO> FormulaUsageParemeters { set; get; }
        public int FormulaID { get; set; }
        public string FormulaException { get; set; }
    }
    public class FormulaUsageParemetersDTO
    {
        public FormulaUsageParemetersDTO()
        {
        }
        public int ID { set; get; }
        //      public int FormulaItemsID { set; get; }
        public string RelationshipPropertyTail { set; get; }
        //public string RelationshipKeyColumnTail { set; get; }
        public string ParameterName { set; get; }
        public string ParameterValue { set; get; }

        //اگر پارامتر خودش یک فرمولا پارامتر بود این پراپرتی که جزئیات محاسبه آنست پر میشود
        //public List<FormulaUsageParemetersDTO> ChildItems { set; get; }
    }
    public class LetterItemLogDTO
    {
        public int ID { get; set; }
        public string AfterUpdateException { get; set; }
    }
    public class ArchiveItemLogDTO
    {
        public int ID { get; set; }
        public string Exception { get; set; }
        //public int ArchiveID { get; set; }

    }
    public enum DataLogType
    {
        DataInsert,
        DataEdit,
        DataDelete,
        ArchiveInsert,
        ArchiveDelete,
        LetterInsert,
        LetterUpdate,
        LetterDelete,
        Unknown
    }

}
