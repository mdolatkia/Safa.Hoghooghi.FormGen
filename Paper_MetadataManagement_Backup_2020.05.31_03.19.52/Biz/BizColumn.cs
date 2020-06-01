
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paper_MetadataManagement
{
    public class BizColumn
    {
        public Type GetColumnDotNetType(int columnID)
        {
            using (var projectContext = new MyIdeaEntities())
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

        public ICollection<Column> GetColumns(TableDrivedEntity entity)
        {
            if (entity.Column.Any())
                return entity.Column;
            else
                return entity.Table.Column;
        }

        public ColumnDTO GetColumn(int columnID,bool simple)
        {
            ColumnDTO result = new ColumnDTO();
            using (var projectContext = new MyIdeaEntities())
            {
                var column = projectContext.Column.First(x => x.ID == columnID);
                return ToColumnDTO(column, simple);
            }
        }
        public ColumnKeyValueDTO GetColumnKeyValue(int columnID)
        {
            ColumnKeyValueDTO result = new ColumnKeyValueDTO();
            using (var projectContext = new MyIdeaEntities())
            {
                var column = projectContext.Column.FirstOrDefault(x => x.ID == columnID);
                if (column.ColumnKeyValue != null)
                    return ToColumnKeyValueDTO(column.ColumnKeyValue);
            }
            return null;
        }

        private ColumnKeyValueDTO ToColumnKeyValueDTO(ColumnKeyValue item)
        {
            ColumnKeyValueDTO result = new ColumnKeyValueDTO();
            result.ValueFromKeyOrValue = item.ValueFromKeyOrValue;
            result.ColumnKeyValueRange = new List<ColumnKeyValueRangeDTO>();
            foreach (var rItem in item.ColumnKeyValueRange)
            {
                var keyValueRange = new ColumnKeyValueRangeDTO();
                keyValueRange.ColumnID = rItem.ColumnID;
                keyValueRange.ColumnName = item.Column.Name;
                keyValueRange.ID = rItem.ID;
                keyValueRange.KeyTitle = rItem.KeyTitle;
                keyValueRange.Value = rItem.Value;
                result.ColumnKeyValueRange.Add(keyValueRange);
            }
            return result;
        }
        public void UpdateColumnKeyValue(int columnID, bool ValueFromKeyOrValue, List<ColumnKeyValueRangeDTO> columnKeyValueRange)
        {
            using (var projectContext = new MyIdeaEntities())
            {
                var column = projectContext.Column.First(x => x.ID == columnID);
                if (columnKeyValueRange.Count > 0)
                {
                    if (column.ColumnKeyValue == null)
                        column.ColumnKeyValue = new ColumnKeyValue();
                    column.ColumnKeyValue.ValueFromKeyOrValue = ValueFromKeyOrValue;
                    while (column.ColumnKeyValue.ColumnKeyValueRange.Any())
                        projectContext.ColumnKeyValueRange.Remove(column.ColumnKeyValue.ColumnKeyValueRange.First());
                    foreach (var keyValueRange in columnKeyValueRange)
                    {
                        column.ColumnKeyValue.ColumnKeyValueRange.Add(new ColumnKeyValueRange() { Value = keyValueRange.Value, KeyTitle = keyValueRange.KeyTitle });
                    }
                }
                else
                {
                    if (column.ColumnKeyValue != null)
                    {
                        while (column.ColumnKeyValue.ColumnKeyValueRange.Any())
                            projectContext.ColumnKeyValueRange.Remove(column.ColumnKeyValue.ColumnKeyValueRange.First());
                        projectContext.ColumnKeyValue.Remove(column.ColumnKeyValue);

                    }
                }
                projectContext.SaveChanges();
            }
        }
        public List<StringColumnTypeDTO> GetStringColumType(int columnID)
        {
            ColumnDTO result = new ColumnDTO();
            using (var projectContext = new MyIdeaEntities())
            {
                var column = projectContext.Column.First(x => x.ID == columnID);
                if (column.StringColumnType != null)
                    return GeneralHelper.CreateListFromSingleObject<StringColumnTypeDTO>(ToStringColumTypeDTO(column.StringColumnType));
            }
            return null;
        }

        //public void UpdateCustomCalculation(int columnID, int formulaID)
        //{
        //    using (var projectContext = new MyIdeaEntities())
        //    {
        //        var column = projectContext.Column.First(x => x.ID == columnID);
        //        if (column.CustomCalculatedColumn == null)
        //            column.CustomCalculatedColumn = new CustomCalculatedColumn();
        //        column.CustomCalculatedColumn.FormulaID = formulaID;
        //        projectContext.SaveChanges();
        //    }
        //}

        private StringColumnTypeDTO ToStringColumTypeDTO(StringColumnType item)
        {
            StringColumnTypeDTO result = new StringColumnTypeDTO();
            result.ColumnID = item.ColumnID;
            result.ColumnName= item.Column.Name;
            result.Format = item.Format;
            result.MaxLength = item.MaxLength;
            return result;
        }
        public List<NumericColumnTypeDTO> GetNumericColumType(int columnID)
        {
            ColumnDTO result = new ColumnDTO();
            using (var projectContext = new MyIdeaEntities())
            {
                var column = projectContext.Column.First(x => x.ID == columnID);
                if (column.NumericColumnType != null)
                    return GeneralHelper.CreateListFromSingleObject<NumericColumnTypeDTO>(ToNumericColumTypeDTO(column.NumericColumnType));
            }
            return null;
        }
        private NumericColumnTypeDTO ToNumericColumTypeDTO(NumericColumnType item)
        {
            NumericColumnTypeDTO result = new NumericColumnTypeDTO();
            result.ColumnID = item.ColumnID;
            result.MaxValue = (item.MaxValue == null ? 0 : item.MaxValue.Value);
            result.MinValue = (item.MinValue == null ? 0 : item.MinValue.Value);
            result.Precision = (item.Precision == null ? 0 : item.Precision.Value);
            result.Scale = (item.Scale == null ? 0 : item.Scale.Value);
            return result;
        }
        public List<DateColumnTypeDTO> GetDateColumType(int columnID)
        {
            ColumnDTO result = new ColumnDTO();
            using (var projectContext = new MyIdeaEntities())
            {
                var column = projectContext.Column.First(x => x.ID == columnID);
                if (column.DateColumnType != null)
                    return GeneralHelper.CreateListFromSingleObject<DateColumnTypeDTO>(ToDateColumTypeDTO(column.DateColumnType));
            }
            return null;
        }

        private DateColumnTypeDTO ToDateColumTypeDTO(DateColumnType item)
        {
            DateColumnTypeDTO result = new DateColumnTypeDTO();
            result.ColumnID = item.ColumnID;
            result.IsPersianDate = item.IsPersianDate;
            return result;
        }

        public void ConvertToStringColumnType(int columnID)
        {
            using (var projectContext = new MyIdeaEntities())
            {
                var dbColumn = projectContext.DateColumnType.First(x => x.ColumnID == columnID);
                if (dbColumn.Column.DateColumnType != null)
                    dbColumn.Column.DateColumnType = null;
                if (dbColumn.Column.NumericColumnType != null)
                    dbColumn.Column.NumericColumnType = null;
                if (dbColumn.Column.StringColumnType == null)
                    dbColumn.Column.StringColumnType = new StringColumnType();
                dbColumn.Column.TypeEnum = Convert.ToByte(Enum_ColumnType.String);
                projectContext.SaveChanges();
            }
        }
        public void ConvertToDateColumnType(int columnID)
        {
            using (var projectContext = new MyIdeaEntities())
            {
                var dbColumn = projectContext.StringColumnType.First(x => x.ColumnID == columnID);
                if (dbColumn.Column.StringColumnType != null)
                    dbColumn.Column.StringColumnType = null;
                if (dbColumn.Column.NumericColumnType != null)
                    dbColumn.Column.NumericColumnType = null;
                if (dbColumn.Column.DateColumnType == null)
                    dbColumn.Column.DateColumnType = new DateColumnType();
                dbColumn.Column.TypeEnum = Convert.ToByte(Enum_ColumnType.Date);
                projectContext.SaveChanges();
            }
        }

        public List<ColumnDTO> GetColumns(int entityID, bool simple)
        {
            using (var projectContext = new MyIdeaEntities())
            {
                var dbEntity = projectContext.TableDrivedEntity.First(x => x.ID == entityID);
                return GetColumns(dbEntity, simple);
            }
        }

        internal List<ColumnDTO> GetColumns(TableDrivedEntity item, bool simple)
        {
           
            List<ColumnDTO> result = new List<ColumnDTO>();
            if (item.Column.Count > 0)
            {
                foreach (var column in item.Column)
                    result.Add(ToColumnDTO(column, simple));
                return result;
            }
            else
            {
                foreach (var column in item.Table.Column)
                    result.Add(ToColumnDTO(column, simple));
            }
            return result;
        }

        //public List<ColumnDTO> GetColumnsForEdit(int entityID)
        //{
        //    List<ColumnDTO> result = new List<ColumnDTO>();
        //    using (var projectContext = new MyIdeaEntities())
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
        //    using (var projectContext = new MyIdeaEntities())
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
        //    using (var projectContext = new MyIdeaEntities())
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
            using (var projectContext = new MyIdeaEntities())
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
        public ColumnDTO ToColumnDTO(Column item, bool simple)
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
            if (item.TypeEnum != null)
                result.ColumnType = (Enum_ColumnType)item.TypeEnum;
            else
                result.ColumnType = Enum_ColumnType.None;

            if(!string.IsNullOrEmpty(item.Alias))
            result.Alias = item.Alias;
            else
                result.Alias = item.Name;
            result.DataEntryEnabled = item.DataEntryEnabled;
            result.DefaultValue = item.DefaultValue;
            result.IsMandatory = item.IsMandatory;
            result.IsIdentity = item.IsIdentity;
            result.IsDBCalculatedColumn = item.IsDBCalculatedColumn;
            result.Position = (item.Position == null ? 0 : item.Position.Value);
            result.SearchEnabled = item.SearchEnabled == true;
            //result.ViewEnabled = item.ViewEnabled == true;
            result.DataEntryView = item.DataEntryView;
            result.DotNetType = GetColumnDotNetType(result.DataType);
            if (!simple)
            {
                if (item.StringColumnType != null)
                    result.StringColumnType = ToStringColumTypeDTO(item.StringColumnType);
                if (item.NumericColumnType != null)
                    result.NumericColumnType = ToNumericColumTypeDTO(item.NumericColumnType);
                if (item.DateColumnType != null)
                    result.DateColumnType = ToDateColumTypeDTO(item.DateColumnType);

                if (item.ColumnKeyValue != null)
                    result.ColumnKeyValue = ToColumnKeyValueDTO(item.ColumnKeyValue);
                //    if (item.DBCalculatedColumn != null)
                //        result.DBFormula = item.DBCalculatedColumn.Formula;

                //    if (item.CustomCalculatedColumn != null)
                //        result.CustomFormula = new BizFormula().ToFormulaDTO(item.CustomCalculatedColumn.Formula,false);
                //}
            }
            //CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.Column, item.ID.ToString(), simple.ToString());
            return result;
        }

        //public FormulaDTO GetCustomCalculationFormula(int columnID)
        //{
        //    using (var projectContext = new MyIdeaEntities())
        //    {
        //        var column = projectContext.Column.FirstOrDefault(x => x.ID == columnID);
        //        if (column.CustomCalculatedColumn != null)
        //            return new BizFormula().GetFormula(column.CustomCalculatedColumn.FormulaID,false);
        //    }
        //    return null;
        //}



        public void UpdateColumns(List<ColumnDTO> columns)
        {
            using (var projectContext = new MyIdeaEntities())
            {
                foreach (var column in columns)
                {
                    var dbColumn = projectContext.Column.First(x => x.ID == column.ID);
                    dbColumn.Alias = column.Alias;
                    dbColumn.DataEntryEnabled = column.DataEntryEnabled;
                    dbColumn.DefaultValue = column.DefaultValue;
                    dbColumn.IsMandatory = column.IsMandatory;
                    dbColumn.Position = column.Position;
                    dbColumn.SearchEnabled = column.SearchEnabled;
                    //dbColumn.ViewEnabled = column.ViewEnabled;
                    dbColumn.DataEntryView = column.DataEntryView;
                }
                projectContext.SaveChanges();
            }
        }
        public void UpdateStringColumnType(List<StringColumnTypeDTO> columnTypes)
        {
            using (var projectContext = new MyIdeaEntities())
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
            using (var projectContext = new MyIdeaEntities())
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

        public void UpdateDateColumnType(List<DateColumnTypeDTO> columnTypes)
        {
            using (var projectContext = new MyIdeaEntities())
            {
                foreach (var column in columnTypes)
                {
                    var dbColumn = projectContext.DateColumnType.First(x => x.ColumnID == column.ColumnID);
                    dbColumn.IsPersianDate = column.IsPersianDate;
                }
                projectContext.SaveChanges();
            }
        }

        //private Enum_ColumnType GetColumnType(Column column)
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
