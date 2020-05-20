using DataAccess;
using ModelEntites;
using MyCacheManager;
using MyGeneralLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProxyLibrary;


namespace MyModelManager
{
    public class BizColumn
    {
        public Type GetColumnDotNetType(int columnID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var column = projectContext.Column.First(x => x.ID == columnID);
                return GetColumnDotNetType(column.DataType);
            }
        }
        public Type GetColumnDotNetType(ColumnDTO column)
        {
            return GetColumnDotNetType(column.DataType);
        }
        public Type GetColumnDotNetType(string type)
        {
            type = type.Trim();
            if (type == "char" || type == "nvarchar" || type == "varchar" || type == "text")
                return typeof(string);
            else if (type == "bigint" || type == "numeric" || type == "smallint"
                || type == "decimal" || type == "smallmoney" || type == "int"
                || type == "tinyint" || type == "money")
                return typeof(int);
            else if (type == "date" || type == "datetime")
                return typeof(string);
            else if (type == "bit")
                return typeof(bool);
            return null;
        }
        public List<ColumnDTO> GetTableColumns(int tableID, bool simple)
        {
            List<ColumnDTO> result = new List<ColumnDTO>();

            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var table = projectContext.Table.First(x => x.ID == tableID);
                foreach (var column in table.Column)
                    result.Add(ToColumnDTO(column, simple));
            }
            return result;
        }

        //public ICollection<Column> GetColumns(TableDrivedEntity entity)
        //{
        //    if (entity.Column.Any())
        //        return entity.Column;
        //    else
        //        return entity.Table.Column;
        //}

        public ColumnDTO GetColumn(int columnID, bool simple)
        {
            ColumnDTO result = new ColumnDTO();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var column = projectContext.Column.First(x => x.ID == columnID);
                return ToColumnDTO(column, simple);
            }
        }

        public List<StringColumnTypeDTO> GetStringColumType(int columnID)
        {
            ColumnDTO result = new ColumnDTO();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var column = projectContext.Column.First(x => x.ID == columnID);
                if (column.StringColumnType != null)
                    return GeneralHelper.CreateListFromSingleObject<StringColumnTypeDTO>(ToStringColumTypeDTO(column.StringColumnType));
            }
            return null;
        }

        //public void UpdateCustomCalculation(int columnID, int formulaID)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var column = projectContext.Column.First(x => x.ID == columnID);
        //        if (column.CustomCalculatedColumn == null)
        //            column.CustomCalculatedColumn = new CustomCalculatedColumn();
        //        column.CustomCalculatedColumn.FormulaID = formulaID;
        //        projectContext.SaveChanges();
        //    }
        //}

        private StringColumnTypeDTO ToStringColumTypeDTO(DataAccess.StringColumnType item)
        {
            StringColumnTypeDTO result = new StringColumnTypeDTO();
            result.ColumnID = item.ColumnID;
            result.Format = item.Format;
            result.MaxLength = item.MaxLength;
            return result;
        }
        public List<NumericColumnTypeDTO> GetNumericColumType(int columnID)
        {
            ColumnDTO result = new ColumnDTO();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var column = projectContext.Column.First(x => x.ID == columnID);
                if (column.NumericColumnType != null)
                    return GeneralHelper.CreateListFromSingleObject<NumericColumnTypeDTO>(ToNumericColumTypeDTO(column.NumericColumnType));
            }
            return null;
        }
        private NumericColumnTypeDTO ToNumericColumTypeDTO(DataAccess.NumericColumnType item)
        {
            NumericColumnTypeDTO result = new NumericColumnTypeDTO();
            result.ColumnID = item.ColumnID;
            result.MaxValue = item.MaxValue;
            result.MinValue = item.MinValue;
            result.Precision = (item.Precision == null ? 0 : item.Precision.Value);
            result.Scale = (item.Scale == null ? 0 : item.Scale.Value);
            return result;
        }
        public List<DateColumnTypeDTO> GetDateColumType(int columnID)
        {
            ColumnDTO result = new ColumnDTO();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var column = projectContext.Column.First(x => x.ID == columnID);
                if (column.DateColumnType != null)
                    return GeneralHelper.CreateListFromSingleObject<DateColumnTypeDTO>(ToDateColumTypeDTO(column.DateColumnType));
            }
            return null;
        }
        private DateColumnTypeDTO ToDateColumTypeDTO(DataAccess.DateColumnType item)
        {
            DateColumnTypeDTO result = new DateColumnTypeDTO();
            result.ColumnID = item.ColumnID;
            result.ShowMiladiDateInUI = item.ShowMiladiDateInUI;
            result.StringDateIsMiladi = item.StringDateIsMiladi;
            //     result.ValueIsPersianDate = item.ValueIsPersianDate;
            return result;
        }
        public List<TimeColumnTypeDTO> GetTimeColumType(int columnID)
        {
            ColumnDTO result = new ColumnDTO();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var column = projectContext.Column.First(x => x.ID == columnID);
                if (column.TimeColumnType != null)
                    return GeneralHelper.CreateListFromSingleObject<TimeColumnTypeDTO>(ToTimeColumTypeDTO(column.TimeColumnType));
            }
            return null;
        }
        private TimeColumnTypeDTO ToTimeColumTypeDTO(DataAccess.TimeColumnType item)
        {
            TimeColumnTypeDTO result = new TimeColumnTypeDTO();
            result.ColumnID = item.ColumnID;
            result.ShowMiladiTime = item.ShowMiladiTime;
            result.StringTimeIsMiladi = item.StringTimeIsMiladi;
            result.StringTimeISAMPMFormat = item.StringTimeISAMPMFormat;
            result.ShowAMPMFormat = item.ShowAMPMFormat;
            //     result.ValueIsPersianDate = item.ValueIsPersianDate;
            return result;
        }
        public List<DateTimeColumnTypeDTO> GetDateTimeColumType(int columnID)
        {
            ColumnDTO result = new ColumnDTO();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var column = projectContext.Column.First(x => x.ID == columnID);
                if (column.DateTimeColumnType != null)
                    return GeneralHelper.CreateListFromSingleObject<DateTimeColumnTypeDTO>(ToDateTimeColumTypeDTO(column.DateTimeColumnType));
            }
            return null;
        }
        private DateTimeColumnTypeDTO ToDateTimeColumTypeDTO(DataAccess.DateTimeColumnType item)
        {
            DateTimeColumnTypeDTO result = new DateTimeColumnTypeDTO();
            result.ColumnID = item.ColumnID;
            result.HideTimePicker = item.HideTimePicker;
            result.ShowAMPMFormat = item.ShowAMPMFormat;
            result.ShowMiladiDateInUI = item.ShowMiladiDateInUI;
            result.StringDateIsMiladi = item.StringDateIsMiladi;
            result.StringTimeIsMiladi = item.StringTimeIsMiladi;
            result.StringTimeISAMPMFormat = item.StringTimeISAMPMFormat;

            return result;
        }


        public void ConvertColumnToStringColumnType(int columnID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbColumn = projectContext.Column.First(x => x.ID == columnID);
                if ((Enum_ColumnType)dbColumn.OriginalTypeEnum != Enum_ColumnType.String)
                {
                    throw new Exception("ستون امکان تبدیل به نوع رشته ای ندارد");
                }

                dbColumn.TypeEnum = Convert.ToByte(Enum_ColumnType.String);
                RemoveColumnTypes(projectContext, dbColumn, new List<Enum_ColumnType>() { Enum_ColumnType.String });

                projectContext.SaveChanges();
            }
        }


        //public void ConvertStringTimeColumnToStringColumnType(int columnID)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var dbColumn = projectContext.TimeColumnType.First(x => x.ColumnID == columnID);

        //        if ((Enum_ColumnType)dbColumn.Column.OriginalTypeEnum != Enum_ColumnType.String)
        //        {
        //            throw new Exception("ستون امکان تبدیل به نوع رشته ای ندارد");
        //        }

        //        if (dbColumn.Column.TimeColumnType != null)
        //        {
        //            projectContext.TimeColumnType.Remove(dbColumn.Column.TimeColumnType);
        //            dbColumn.Column.TimeColumnType = null;
        //        }
        //        dbColumn.Column.TypeEnum = Convert.ToByte(Enum_ColumnType.String);
        //        projectContext.SaveChanges();
        //    }
        //}
        //public void ConvertStringDateTimeColumnToStringColumnType(int columnID)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var dbColumn = projectContext.TimeColumnType.First(x => x.ColumnID == columnID);

        //        if ((Enum_ColumnType)dbColumn.Column.OriginalTypeEnum != Enum_ColumnType.String)
        //        {
        //            throw new Exception("ستون امکان تبدیل به نوع رشته ای ندارد");
        //        }

        //        if (dbColumn.Column.TimeColumnType != null)
        //        {
        //            projectContext.TimeColumnType.Remove(dbColumn.Column.TimeColumnType);
        //            dbColumn.Column.TimeColumnType = null;
        //        }
        //        dbColumn.Column.TypeEnum = Convert.ToByte(Enum_ColumnType.String);
        //        projectContext.SaveChanges();
        //    }
        //}
        public void ConvertStringColumnToDateColumnType(int columnID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbColumn = projectContext.Column.First(x => x.ID == columnID);

                if (dbColumn.DateColumnType == null)
                    dbColumn.DateColumnType = new DateColumnType();

                var dbUISetting = dbColumn.Table.DBSchema.DatabaseInformation.DatabaseUISetting;
                dbColumn.DateColumnType.ShowMiladiDateInUI = dbUISetting != null ? dbUISetting.ShowMiladiDateInUI : false;
                dbColumn.DateColumnType.StringDateIsMiladi = dbUISetting != null ? dbUISetting.StringDateColumnIsMiladi : false;
                dbColumn.TypeEnum = Convert.ToByte(Enum_ColumnType.Date);
                RemoveColumnTypes(projectContext, dbColumn, new List<Enum_ColumnType>() { Enum_ColumnType.Date, Enum_ColumnType.String });

                projectContext.SaveChanges();
            }
        }
        public void ConvertStringColumnToTimeColumnType(int columnID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbColumn = projectContext.Column.First(x => x.ID == columnID);

                if (dbColumn.TimeColumnType == null)
                    dbColumn.TimeColumnType = new TimeColumnType();

                var dbUISetting = dbColumn.Table.DBSchema.DatabaseInformation.DatabaseUISetting;
                dbColumn.TimeColumnType.ShowMiladiTime = dbUISetting != null ? dbUISetting.ShowMiladiDateInUI : false;
                //    dbColumn.TimeColumnType.StringValueIsMiladi = dbUISetting != null ? dbUISetting.StringDateColumnIsMiladi : false;

                dbColumn.TypeEnum = Convert.ToByte(Enum_ColumnType.Time);
                RemoveColumnTypes(projectContext, dbColumn, new List<Enum_ColumnType>() { Enum_ColumnType.Time, Enum_ColumnType.String });

                projectContext.SaveChanges();
            }
        }
        public void ConvertStringColumnToDateTimeColumnType(int columnID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbColumn = projectContext.Column.First(x => x.ID == columnID);
                if (dbColumn.DateTimeColumnType == null)
                    dbColumn.DateTimeColumnType = new DateTimeColumnType();

                var dbUISetting = dbColumn.Table.DBSchema.DatabaseInformation.DatabaseUISetting;
                dbColumn.DateTimeColumnType.ShowMiladiDateInUI = dbUISetting != null ? dbUISetting.ShowMiladiDateInUI : false;
                dbColumn.DateTimeColumnType.StringDateIsMiladi = dbUISetting != null ? dbUISetting.StringDateColumnIsMiladi : false;
                dbColumn.DateTimeColumnType.StringTimeIsMiladi = dbUISetting != null ? dbUISetting.StringDateColumnIsMiladi : false;

                dbColumn.TypeEnum = Convert.ToByte(Enum_ColumnType.DateTime);
                RemoveColumnTypes(projectContext, dbColumn, new List<Enum_ColumnType>() { Enum_ColumnType.DateTime, Enum_ColumnType.String });
                projectContext.SaveChanges();
            }
        }
        private void RemoveColumnTypes(MyProjectEntities projectContext, Column dbColumn, List<Enum_ColumnType> exceptionTypes)
        {
            if (!exceptionTypes.Contains(Enum_ColumnType.Numeric))
            {
                if (dbColumn.NumericColumnType != null)
                    projectContext.NumericColumnType.Remove(dbColumn.NumericColumnType);
            }
            if (!exceptionTypes.Contains(Enum_ColumnType.DateTime))
            {
                if (dbColumn.DateTimeColumnType != null)
                    projectContext.DateTimeColumnType.Remove(dbColumn.DateTimeColumnType);
            }
            if (!exceptionTypes.Contains(Enum_ColumnType.Date))
            {
                if (dbColumn.DateColumnType != null)
                    projectContext.DateColumnType.Remove(dbColumn.DateColumnType);
            }
            if (!exceptionTypes.Contains(Enum_ColumnType.Time))
            {
                if (dbColumn.TimeColumnType != null)
                    projectContext.TimeColumnType.Remove(dbColumn.TimeColumnType);
            }
            if (!exceptionTypes.Contains(Enum_ColumnType.String))
            {
                if (dbColumn.StringColumnType != null)
                    projectContext.StringColumnType.Remove(dbColumn.StringColumnType);
            }
        }
        public List<ColumnDTO> GetAllColumns(int entityID, bool simple)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbEntity = projectContext.TableDrivedEntity.First(x => x.ID == entityID);
                List<ColumnDTO> result = new List<ColumnDTO>();
                List<Column> columns = null;
                if (dbEntity.TableDrivedEntity_Columns.Count > 0)
                {
                    //اینجا باید لیست مقادیر هم ست شود
                    columns = dbEntity.TableDrivedEntity_Columns.Select(x => x.Column).ToList();
                }
                else
                {
                    columns = dbEntity.Table.Column.ToList();
                }
                foreach (var column in columns)
                    result.Add(ToColumnDTO(column, simple));
                return result;
            }
        }

        //private List<ColumnDTO> GetColumns(TableDrivedEntity item, bool simple)
        //{


        //}

        //public List<ColumnDTO> GetColumnsForEdit(int entityID)
        //{
        //    List<ColumnDTO> result = new List<ColumnDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var entity = projectContext.TableDrivedEntity.First(x => x.ID == entityID);
        //        ICollection<Column> columns = null;
        //        if (entity.Column.Count > 0)
        //        {
        //            columns = entity.Column;
        //        }
        //        else
        //        {
        //            columns = entity.Table.Column;
        //        }
        //        columns = columns.Where(x => x.IsDBCalculatedColumn == false).ToList();
        //        foreach (var column in columns)
        //            result.Add(ToColumnDTO(column, true));
        //        return result;
        //    }
        //}

        //public List<ColumnDTO> GetColumnsSimple(int entityID)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var entity = projectContext.TableDrivedEntity.First(x => x.ID == entityID);
        //        if (entity.Column.Count > 0)
        //        {
        //            return GetColumnsSimple(entity.Column);
        //        }
        //        else
        //        {
        //            return GetColumnsSimple(entity.Table.Column);
        //        }
        //    }
        //}

        //private List<ColumnDTO> GetColumnsSimple(ICollection<Column> columns)
        //{
        //    BizColumn bizColumn = new BizColumn();
        //    List<ColumnDTO> result = new List<ColumnDTO>();
        //    foreach (var column in columns)
        //        result.Add(bizColumn.ToColumnDTO(column, true));
        //    return result;
        //}


        //public List<ColumnDTO> GetColums(int tableID, bool simple)
        //{
        //    List<ColumnDTO> result = new List<ColumnDTO>();
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var list = projectContext.Column.Where(x => x.TableID == tableID);
        //        foreach (var item in list)
        //        {
        //            result.Add(ToColumnDTO(item, simple));
        //        }
        //    }
        //    return result;
        //}
        public List<ColumnDTO> GetOtherColums(int columnID)
        {
            List<ColumnDTO> result = new List<ColumnDTO>();
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var column = projectContext.Column.First(x => x.ID == columnID);
                var list = column.Table.Column.ToList();
                foreach (var item in list)
                {
                    result.Add(ToColumnDTO(item, true));
                }
            }
            return result;
        }
        public ColumnDTO ToColumnDTO(DataAccess.Column item, bool simple)
        {
            //var cachedItem = CacheManager.GetCacheManager().GetCachedItem(CacheItemType.Column, item.ID.ToString(), simple.ToString());
            //if (cachedItem != null)
            //    return (cachedItem as ColumnDTO);

            ColumnDTO result = new ColumnDTO();
            result.Name = item.Name;
            result.DataType = item.DataType;

            result.ID = item.ID;
            result.TableID = item.TableID;
            result.IsNull = item.IsNull;

            result.PrimaryKey = item.PrimaryKey;


            result.ColumnType = (Enum_ColumnType)item.TypeEnum;
            result.OriginalColumnType = (Enum_ColumnType)item.OriginalTypeEnum;

            if (!string.IsNullOrEmpty(item.Alias))
                result.Alias = item.Alias;
            else
                result.Alias = item.Name;
            if (item.RelationshipColumns.Any())
                result.ForeignKey = item.RelationshipColumns.Any(x => x.Relationship.Removed != true && x.Relationship.MasterTypeEnum == (int)Enum_MasterRelationshipType.FromForeignToPrimary);
            result.DataEntryEnabled = item.DataEntryEnabled;
            result.DefaultValue = item.DefaultValue;
            result.IsMandatory = item.IsMandatory;
            result.IsIdentity = item.IsIdentity;
            result.Position = (item.Position == null ? 0 : item.Position.Value);
            result.IsDisabled = item.IsDisabled;
            result.IsNotTransferable = item.IsNotTransferable;

            result.DBFormula = item.DBCalculateFormula;
            // result.IsDBCalculatedColumn = !string.IsNullOrEmpty(result.DBFormula);
            result.IsReadonly = item.IsReadonly;
            result.DotNetType = GetColumnDotNetType(result.DataType);

            result.CustomFormulaID = item.CustomCalculateFormulaID ?? 0;
            if (!simple)
            {
                if (item.StringColumnType != null)
                    result.StringColumnType = ToStringColumTypeDTO(item.StringColumnType);
                if (item.NumericColumnType != null)
                    result.NumericColumnType = ToNumericColumTypeDTO(item.NumericColumnType);
                if (item.DateColumnType != null)
                    result.DateColumnType = ToDateColumTypeDTO(item.DateColumnType);
                if (item.TimeColumnType != null)
                    result.TimeColumnType = ToTimeColumTypeDTO(item.TimeColumnType);
                if (item.DateTimeColumnType != null)
                    result.DateTimeColumnType = ToDateTimeColumTypeDTO(item.DateTimeColumnType);
                BizColumnValueRange bizColumnValueRange = new MyModelManager.BizColumnValueRange();
                if (item.ColumnValueRange != null)
                    result.ColumnValueRange = bizColumnValueRange.ToColumnValueRangeDTO(item.ColumnValueRange, true);

                if (item.CustomCalculateFormulaID != null)
                {
                    result.CustomFormula = new BizFormula().ToFormulaDTO(item.Formula, false);
                }
            }
            CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.Column, item.ID.ToString(), simple.ToString());
            return result;
        }
        private bool IsStringType(string datatype)
        {
            return (datatype.Contains("char") || datatype.Contains("text"));
        }
        //public FormulaDTO GetCustomCalculationFormula(int columnID)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var column = projectContext.Column.FirstOrDefault(x => x.ID == columnID);
        //        if (column.CustomCalculatedColumn != null)
        //            return new BizFormula().GetFormula(column.CustomCalculatedColumn.FormulaID, false);
        //    }
        //    return null;
        //}



        public void UpdateColumns(int entityID, List<ColumnDTO> columns)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbEntity = projectContext.TableDrivedEntity.First(x => x.ID == entityID);
                dbEntity.ColumnsReviewed = true;
                foreach (var column in columns)
                {
                    var dbColumn = projectContext.Column.First(x => x.ID == column.ID);
                    dbColumn.Alias = column.Alias;
                    dbColumn.DefaultValue = column.DefaultValue;
                    dbColumn.IsMandatory = column.IsMandatory;
                    dbColumn.Position = column.Position;
                    dbColumn.Description = column.Description;
                    if (column.PrimaryKey || column.ForeignKey)
                    {

                    }
                    else
                    {
                        dbColumn.IsDisabled = column.IsDisabled;
                        dbColumn.IsReadonly = column.IsReadonly;
                        dbColumn.DataEntryEnabled = column.DataEntryEnabled;
                        dbColumn.IsNotTransferable = column.IsNotTransferable;
                    }
                    if (column.CustomFormulaID != 0)
                        dbColumn.CustomCalculateFormulaID = column.CustomFormulaID;
                    else
                        dbColumn.CustomCalculateFormulaID = null;

                    // if (column.CustomFormulaID != 0)
                    // {
                    //     //if (dbColumn.CustomCalculatedColumn == null)
                    //     //    dbColumn.CustomCalculatedColumn = new CustomCalculatedColumn();
                    //     //dbColumn.CustomCalculatedColumn.FormulaID = column.CustomFormulaID;
                    //     //dbColumn.IsCustomColumn = true;
                    // }
                    // else
                    // {
                    ////     dbColumn.IsCustomColumn = false;
                    //     //if (dbColumn.CustomCalculatedColumn != null)
                    //     //{
                    //     //    projectContext.CustomCalculatedColumn.Remove(dbColumn.CustomCalculatedColumn);
                    //     //}
                    // }
                }
                projectContext.SaveChanges();
            }
        }
        public void UpdateStringColumnType(List<StringColumnTypeDTO> columnTypes)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var column in columnTypes)
                {
                    var dbColumn = projectContext.StringColumnType.First(x => x.ColumnID == column.ColumnID);
                    dbColumn.MaxLength = column.MaxLength;
                    dbColumn.Format = column.Format;
                }
                projectContext.SaveChanges();
            }
        }

        public void UpdateNumericColumnType(List<NumericColumnTypeDTO> columnTypes)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var column in columnTypes)
                {
                    var dbColumn = projectContext.NumericColumnType.First(x => x.ColumnID == column.ColumnID);
                    dbColumn.MaxValue = column.MaxValue;
                    dbColumn.MinValue = column.MinValue;
                    dbColumn.Precision = column.Precision;
                    dbColumn.Scale = column.Scale;
                }
                projectContext.SaveChanges();
            }
        }

        public bool DataIsAccessable(DR_Requester requester, int columnID, bool isNotReadonlyCheck = false)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbColumn = projectContext.Column.First(x => x.ID == columnID);
                return DataIsAccessable(requester, dbColumn, isNotReadonlyCheck);
            }
        }
        public bool DataIsAccessable(DR_Requester requester, Column column, bool isNotReadonlyCheck = false)
        {
            SecurityHelper securityHelper = new SecurityHelper();
            if (column.IsDisabled == true)
                return false;
            else if (isNotReadonlyCheck && column.IsReadonly)
                return false;
            else
            {
                if (requester.SkipSecurity)
                    return true;
                var permission = securityHelper.GetAssignedPermissions(requester, column.ID, false);
                if (permission.GrantedActions.Any(y => y == SecurityAction.NoAccess))
                    return false;
                else if (isNotReadonlyCheck && permission.GrantedActions.Any(y => y == SecurityAction.ReadOnly))
                    return false;
                else
                {
                    return true;
                }
            }
        }

        public bool DataIsReadonly(DR_Requester requester, int columnID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                var dbColumn = projectContext.Column.First(x => x.ID == columnID);
                return DataIsReadonly(requester, dbColumn);
            }
        }
        public bool DataIsReadonly(DR_Requester requester, Column column)
        {
            SecurityHelper securityHelper = new SecurityHelper();

            if (column.IsReadonly == true)
                return true;
            else
            {
                if (requester.SkipSecurity)
                    return false;
                var permission = securityHelper.GetAssignedPermissions(requester, column.ID, false);
                if (permission.GrantedActions.Any(y => y == SecurityAction.ReadOnly))
                    return true;
                else
                {
                    return false;
                }
            }
        }


        public void UpdateDateColumnType(List<DateColumnTypeDTO> columnTypes)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var column in columnTypes)
                {
                    var dbColumn = projectContext.DateColumnType.First(x => x.ColumnID == column.ColumnID);
                    dbColumn.ShowMiladiDateInUI = column.ShowMiladiDateInUI;
                    if ((Enum_ColumnType)dbColumn.Column.OriginalTypeEnum == Enum_ColumnType.String)
                        dbColumn.StringDateIsMiladi = column.StringDateIsMiladi;
                    else
                        dbColumn.StringDateIsMiladi = null;

                }
                projectContext.SaveChanges();
            }
        }

        public void UpdateDateTimeColumnType(List<DateTimeColumnTypeDTO> columnTypes)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var column in columnTypes)
                {
                    var dbColumn = projectContext.DateTimeColumnType.First(x => x.ColumnID == column.ColumnID);
                    dbColumn.ShowMiladiDateInUI = column.ShowMiladiDateInUI;
                    dbColumn.ShowAMPMFormat = column.ShowAMPMFormat;
                    dbColumn.HideTimePicker = column.HideTimePicker;
                    if ((Enum_ColumnType)dbColumn.Column.OriginalTypeEnum == Enum_ColumnType.String)
                    {
                        dbColumn.StringDateIsMiladi = column.StringDateIsMiladi;
                        dbColumn.StringTimeISAMPMFormat = column.StringTimeISAMPMFormat;
                        dbColumn.StringTimeIsMiladi = column.StringTimeIsMiladi;
                    }
                    else
                    {
                        dbColumn.StringDateIsMiladi = null;
                        dbColumn.StringTimeISAMPMFormat = null;
                        dbColumn.StringTimeIsMiladi = null;
                    }

                }
                projectContext.SaveChanges();
            }
        }

        public void UpdateTimeColumnType(List<TimeColumnTypeDTO> columnTypes)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
            {
                foreach (var column in columnTypes)
                {
                    var dbColumn = projectContext.TimeColumnType.First(x => x.ColumnID == column.ColumnID);
                    //if (!column.StringValueIsMiladi)
                    //{
                    //    if ((Enum_ColumnType)dbColumn.Column.OriginalTypeEnum == Enum_ColumnType.Time)
                    //    {
                    //        throw new Exception("ستون از نوع زمان می باشد و گزینه مقدار شمسی به اشتباه انتخاب شده است");
                    //    }
                    //}
                    dbColumn.ShowMiladiTime = column.ShowMiladiTime;
                    dbColumn.ShowAMPMFormat = column.ShowAMPMFormat;
                    if ((Enum_ColumnType)dbColumn.Column.OriginalTypeEnum == Enum_ColumnType.String)
                    {
                        dbColumn.StringTimeIsMiladi = column.StringTimeIsMiladi;
                        dbColumn.StringTimeISAMPMFormat = column.StringTimeISAMPMFormat;
                    }
                    else
                    {
                        dbColumn.StringTimeIsMiladi = null;
                        dbColumn.StringTimeISAMPMFormat = null;
                    }
                }
                projectContext.SaveChanges();
            }
        }

        //public void UpdateColumnValueRangeID(int columnID, int columnValueRangeID)
        //{
        //    using (var projectContext = new DataAccess.MyProjectEntities())
        //    {
        //        var dbColumn = projectContext.Column.First(x => x.ID == columnID);
        //        dbColumn.ColumnValueRangeID = columnValueRangeID;
        //        projectContext.SaveChanges();
        //    }
        //}



        //private Enum_ColumnType GetColumnType(DataAccess.Column column)
        //{

        //    if (column.StringColumnType != null)
        //    {
        //        return Enum_ColumnType.String;
        //    }
        //    else if (column.NumericColumnType != null)
        //    {
        //        return Enum_ColumnType.Numeric;
        //    }
        //    else if (column.DateColumnType != null)
        //    {
        //        return Enum_ColumnType.Date;
        //    }

        //    return Enum_ColumnType.None;
        //}



    }

}
