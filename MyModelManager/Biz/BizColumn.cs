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
            result.MaxValue = (item.MaxValue == null ? 0 : item.MaxValue.Value);
            result.MinValue = (item.MinValue == null ? 0 : item.MinValue.Value);
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
            result.IsPersianDate = item.IsPersianDate;
            return result;
        }

        public void ConvertToStringColumnType(int columnID)
        {
            using (var projectContext = new DataAccess.MyProjectEntities())
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
            using (var projectContext = new DataAccess.MyProjectEntities())
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

            if (item.TypeEnum != null)
                result.ColumnType = (Enum_ColumnType)item.TypeEnum;
            else
                result.ColumnType = Enum_ColumnType.None;

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

                BizColumnValueRange bizColumnValueRange = new MyModelManager.BizColumnValueRange();
                if (item.ColumnValueRange != null)
                    result.ColumnValueRange = bizColumnValueRange.ToColumnValueRangeDTO(item.ColumnValueRange);

                if (item.CustomCalculateFormulaID != null)
                {
                    result.CustomFormula = new BizFormula().ToFormulaDTO(item.Formula, false);
                }
            }
            CacheManager.GetCacheManager().AddCacheItem(result, CacheItemType.Column, item.ID.ToString(), simple.ToString());
            return result;
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
                if ( permission.GrantedActions.Any(y => y == SecurityAction.ReadOnly))
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
                    dbColumn.IsPersianDate = column.IsPersianDate;
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
